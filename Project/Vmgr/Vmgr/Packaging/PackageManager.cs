using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Vmgr.Configuration;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Data.Biz.Logging;
using Vmgr.Helpers;
using Vmgr.Messaging;
using Vmgr.Plugins;

#if NET_40
using Microsoft.AspNet.SignalR.Client;
using System.Security.Principal;
using Microsoft.AspNet.SignalR;
#endif

namespace Vmgr.Packaging
{
    public class PackageManager
    {
        #region PRIVATE PROPERTIES

        private static int MAX_TRIES = 0;
        private static PackageManager _manage = null;
        private IDictionary<string, AppDomain> _appDomains = null;
        private ServerMetaData _server = null;

#if NET_40
        private HubConnection _hubConnection = null;
        private IHubProxy _proxy = null;
#endif


        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public const string DEFAULT_PACKAGE_RELATIONSHIP = "http://schemas.dominionnet.com/package/2013/vmgx";
        public const string DEFAULT_PACKAGE_SCHEMA = "Vmgr.Packaging.Package.xsd";

        public static PackageManager Manage
        {
            get
            {
                if (_manage == null)
                {
                    _manage = new PackageManager();
                }
                return _manage;
            }
        }

        public IDictionary<string, AppDomain> AppDomains
        {
            get
            {
                if (_appDomains == null)
                {
                    _appDomains = new Dictionary<string, AppDomain> { };
                }
                return _appDomains;
            }
        }


        public ServerMetaData Server
        {
            get
            {
                if (this._server == null)
                {
                    using (AppService app = new AppService())
                    {
                        Guid uniqueId = Guid.Empty;
#if NET_40
                        if (Guid.TryParse(AppDomain.CurrentDomain.FriendlyName, out uniqueId))
                        {
#else
                        bool valid = false;

                        try
                        {
                            uniqueId = new Guid(AppDomain.CurrentDomain.FriendlyName);

                            valid = true;
                        }
                        catch
                        {
                            valid = false;
                        }

                        if (valid)
                        {
#endif
                            var package = app.GetPackages()
                                         .Where(p => p.UniqueId == uniqueId)
                                         .FirstOrDefault()
                                         ;

                            if (package != null)
                                this._server = app.GetServers()
                                    .Where(s => s.ServerId == package.ServerId)
                                    .FirstOrDefault()
                                    ;
                        }
                        else
                        {
                            this._server = app.GetServers()
                                .Where(s => s.Name.Equals(System.Environment.MachineName))
                                .FirstOrDefault()
                                ;
                        }
                    }
                }

                return _server;
            }
        }

        #endregion

        #region CTOR

        public PackageManager()
        {
        }

        #endregion

        #region PRIVATE METHODS

#if NET_40

        protected HubConnection hubConnection
        {
            get
            {
                if (this._hubConnection == null)
                {
                    _hubConnection = new HubConnection(string.Format("{0}://{1}:{2}/"
                        , PackageManager.Manage.Server.RTProtocol
                        , PackageManager.Manage.Server.RTFqdn
                        , PackageManager.Manage.Server.RTPort
                        )
                        )
                        ;

                    IHubProxy proxy = _hubConnection.CreateHubProxy("VmgrHub");
                }

                return this._hubConnection;
            }
        }

        protected IHubProxy proxy
        {
            get
            {
                if (this._proxy == null)
                {
                    _proxy = hubConnection.CreateHubProxy("VmgrHub");
                    hubConnection.Start().Wait();
                }

                if (hubConnection.State == ConnectionState.Disconnected)
                {
                    _proxy = hubConnection.CreateHubProxy("VmgrHub");
                    hubConnection.Start().Wait();
                }

                return this._proxy;
            }
        }

        private void OnGlobalMessage(string message)
        {
            try
            {
                proxy.Invoke("GlobalMessage", message)
                    .Wait()
                    ;
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("OnGlobalMessage failed.", ex, LogType.Error);
            }
        }
#endif

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public void LoadPackage(IPackage package)
        {
            string name = package.Name;

            try
            {
                XDocument packageManifest = GetPackageManifest(package);

                Vmgx vmgx = PackageManager.DeserializePackage(packageManifest);

                if (PackageManager.Manage.AppDomains.ContainsKey(package.UniqueId.ToString()))
                    this.UnloadPackage(package);

                AppDomainSetup setupInformation = AppDomain.CurrentDomain.SetupInformation;
                setupInformation.PrivateBinPath = package.UniqueId.ToString();

                string path = setupInformation.ApplicationBase + package.UniqueId.ToString();

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                else
                {
                    Directory.Delete(path, true);

                    System.Threading.Thread.Sleep(2000);    // Seems it could be that the directory won't get recreated in come cases, so give it some rest.

                    Directory.CreateDirectory(path);
                }

                Logger.Logs.Log(string.Format("Creating a new application domain: '{0}' for package '{1}'."
                    , package.UniqueId
                    , package.Name)
                    , LogType.Info
                    )
                    ;

                AppDomain domain = AppDomain.CreateDomain(package.UniqueId.ToString()
                    , AppDomain.CurrentDomain.Evidence
                    , setupInformation
                    )
                    ;

                PackageManager.Manage.AppDomains.Add(package.UniqueId.ToString(), domain);

                PluginManager<IPlugin> manager = (PluginManager<IPlugin>)domain.CreateInstanceAndUnwrap(typeof(PluginManager<IPlugin>).Assembly.FullName
                    , typeof(PluginManager<IPlugin>).FullName);

                manager.Initialize();

                IList<byte[]> list = new List<byte[]>();

                foreach (AssemblyItem item in vmgx.Assemblies)
                {
                    byte[] assemblyFromPackage = GetAssemblyFromPackage(package, item.Location);

                    File.WriteAllBytes(Path.Combine(path, item.Location), assemblyFromPackage);

                    list.Add(assemblyFromPackage);
                }

                foreach (byte[] buff in list)
                {
                    manager.Load(buff);
                }

#if NET_40
                this.OnGlobalMessage(string.Format("The package '{0}' was loaded at {1}.", package.Name, DateTime.Now));
#endif

                vmgx = null;

                list.Clear();
            }
            catch (Exception ex)
            {
                throw Logger.Logs.Log("Failed to load plugins.", ex, LogType.Error);
            }
        }

        public void UnloadPackage(IPackage package)
        {
            string name = string.Empty;

            try
            {
                AppDomain domain = PackageManager.Manage.AppDomains
                    .Where(p => p.Key.Equals(package.UniqueId.ToString(), StringComparison.InvariantCultureIgnoreCase))
                    .Select(p => p.Value)
                    .FirstOrDefault()
                    ;

                if (domain != null)
                {
                    domain.DomainUnload += new EventHandler(PackageManager.PackagerManager_DomainUnload);

                    name = domain.FriendlyName;

                    if (MAX_TRIES > 0)
                        Logger.Logs.Log(string.Format("Attempting to remove application domain for the {0} time."
                            , MAX_TRIES)
                            , LogType.Info
                            )
                            ;

                    Logger.Logs.Log(string.Format("Removing the application domain: '{0}' for package '{1}'."
                        , package.UniqueId
                        , package.Name)
                        , LogType.Info
                        )
                        ;

                    AppDomain.Unload(domain);

#if NET_40
                    this.OnGlobalMessage(string.Format("The package '{0}' was unloaded at {1}.", package.Name, DateTime.Now));
#endif

                }
                else
                {
                    Logger.Logs.Log(string.Format("Nothing to unload for package '{0}'."
                        , package.UniqueId)
                        , LogType.Info
                        )
                        ;
                }

            }
            catch (CannotUnloadAppDomainException ex)
            {
                Logger.Logs.Log("Unable to unload the application domain.  Trying again in 1 second.", ex, LogType.Error);

                System.Threading.Thread.Sleep(1000);

                MAX_TRIES += 1;

                if (MAX_TRIES < 5)
                    this.UnloadPackage(package);
                else
                    throw Logger.Logs.Log("Unable to unload the application domain.  All retry attempts exhausted.", ex, LogType.Error);
            }
            catch (Exception ex)
            {
                throw Logger.Logs.Log("Failure to unload the application domain.", ex, LogType.Error);
            }
            finally
            {
                MAX_TRIES = 0;

                PackageManager.Manage.AppDomains.Remove(name);
            }
        }

        public static Vmgx DeserializePackage(XDocument document)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Vmgx));

            using (StringReader s = new StringReader(document.ToString()))
            {
                return (serializer.Deserialize(s) as Vmgx);
            }
        }

        public static byte[] GetAssemblyFromPackage(IPackage package, string location)
        {
            byte[] buffer = package.Package;

            try
            {
                using (MemoryStream stream = new MemoryStream(buffer))
                {
                    using (System.IO.Packaging.Package p = System.IO.Packaging.Package.Open(stream, FileMode.Open, FileAccess.Read))
                    {
                        PackagePart part = null;

                        foreach (PackageRelationship relationship in p.GetRelationshipsByType(PackageManager.DEFAULT_PACKAGE_RELATIONSHIP))
                        {
                            Uri partUri = PackUriHelper.ResolvePartUri(relationship.SourceUri, new Uri(string.Format("/{0}", location), UriKind.Relative));

                            if (partUri != null)
                            {
                                part = p.GetPart(partUri);

                                using (Stream s = part.GetStream())
                                {
                                    buffer = new byte[s.Length];
                                    s.Read(buffer, 0, (int)s.Length);
                                }

                                break;
                            }
                        }

                        if (part.Package != null)
                        {
                            part.Package.Close();
                        }
                    }

                    return buffer;
                }
            }
            catch (Exception ex)
            {
                throw Logger.Logs.Log(string.Format("Failed to get assembly {0} from package: {1}.", location, package), ex, LogType.Error);
            }
        }

        public static XDocument GetPackageManifest(IPackage package)
        {
            return PackageManager.GetPackageManifest(package.Package);
        }

        public static XDocument GetPackageManifest(byte[] bytes)
        {
            XDocument document = null;

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (Package package2 = Package.Open(stream, FileMode.Open, FileAccess.Read))
                {
                    PackagePart part = null;

                    foreach (PackageRelationship relationship in package2.GetRelationshipsByType(PackageManager.DEFAULT_PACKAGE_RELATIONSHIP))
                    {
                        Uri partUri = PackUriHelper.ResolvePartUri(new Uri("/", UriKind.Relative), relationship.TargetUri);

                        if ((partUri != null) && partUri.OriginalString.Equals("/package.xml", StringComparison.InvariantCultureIgnoreCase))
                        {
                            part = package2.GetPart(partUri);

                            using (Stream stream2 = part.GetStream())
                            {
                                using (XmlReader reader = XmlReader.Create(stream2))
                                {
                                    string xml = reader.GetXml();

                                    if (PackageManager.ValidatePackageManifestAgainstSchema(xml))
                                    {
                                        document = XDocument.Parse(xml);
                                    }
                                }
                            }

                            break;
                        }
                    }

                    if (part.Package != null)
                    {
                        part.Package.Close();
                    }
                }
            }
            return document;
        }

        public static bool ValidatePackageManifestAgainstSchema(string xml)
        {
            bool result = false;

            try
            {
                XmlUrlResolver resolver = new XmlUrlResolver
                {
                    Credentials = CredentialCache.DefaultCredentials
                };

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    IgnoreComments = true,
                    IgnoreProcessingInstructions = true,
                    IgnoreWhitespace = true,
                    XmlResolver = resolver
                };

                using (Stream stream = ResourceHelper.UnpackEmbeddedResourceToStream(PackageManager.DEFAULT_PACKAGE_SCHEMA))
                {
                    using (XmlReader reader = XmlReader.Create(stream, settings))
                    {
                        XmlSchema schema = XmlSchema.Read(reader, new ValidationEventHandler(PackageManager.OnValidateSchema));
                        XmlSchemaSet schemas = new XmlSchemaSet();
                        schemas.Add(schema);

                        XDocument source = XDocument.Parse(xml);

                        try
                        {
                            source.Validate(schemas, delegate(object o, ValidationEventArgs e)
                            {
                                throw new XmlException(string.Format("{0}", e.Exception));
                            }
                            , true
                            )
                            ;

                            result = true;
                        }
                        catch (Exception ex)
                        {
                            throw Logger.Logs.Log("Failed to validate Package.xsd against provided XML.", ex, LogType.Error);
                        }
                        finally
                        {
                            source = null;
                            schema = null;
                            schemas = null;
                        }

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw Logger.Logs.Log("Failed to retrieve XSD from from embedded resource.", ex, LogType.Error);
            }
        }

        #endregion

        #region EVENTS

        private static void OnValidateSchema(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
                throw new XmlException(string.Format("{0}", e.Exception));
        }

        /// <summary>
        /// Unloads objects in the domain.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Static implementation, otherwise PackagerManager and member instances would have to be serializable, which is not possible.</remarks>
        private static void PackagerManager_DomainUnload(object sender, EventArgs e)
        {
            try
            {
                /*
                 * Withing the context of the domain being unloaded.
                 */

                PluginManager<IPlugin>.Manage.Unload();

                foreach (IPlugin plugin in PluginManager<IPlugin>.Manage.Plugins)
                {
                    string msg = string.Format("Unloading plugin '{0} - {1}' from application domain '{2}'.", plugin.Name, plugin.UniqueId, AppDomain.CurrentDomain.FriendlyName);

                    Logger.Logs.BypassLog4Net(LogType.Info, msg);

                    plugin.UnLoaded();
                }

                PluginManager<IPlugin>.Manage.Plugins.Clear();
            }
            catch (Exception ex)
            {
                throw Logger.Logs.Log(string.Format("Failed to unload application domain '{0}'.", AppDomain.CurrentDomain.FriendlyName), ex, LogType.Error);
            }
        }

        #endregion

    }
}
