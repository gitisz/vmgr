using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Vmgr.Configuration;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Helpers;
using Vmgr.Operations;
using Vmgr.Packaging;
using Vmgr.Scheduling;

namespace Vmgr.Packager
{
    public partial class PackageHandler
    {
        #region PRIVATE PROPERTIES

        private string _packagePath = string.Empty;
        private Dictionary<string, AssemblyInfo> _assembliesFound = null;

        #endregion

        #region PROTECTED PROPERTIES

        Dictionary<string, AssemblyInfo> assembliesFound
        {
            get
            {
                if (this._assembliesFound == null)
                {
                    _assembliesFound = new Dictionary<string, AssemblyInfo> { };
                }

                return this._assembliesFound;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        public string PackagePath
        {
            get
            {
                if (String.IsNullOrEmpty(_packagePath))
                {
                    DirectoryInfo di = new DirectoryInfo(Config.Current.ProjectPath);

                    bool exists = false;

                    while (!exists)
                    {
                        exists = File.Exists(di.FullName + @"\Package.xml");

                        if (!exists)
                        {
                            if (di.Parent != null)
                                di = di.Parent;
                            else
                                exists = true;
                        }
                    }
                    _packagePath = di.FullName + @"\Package.xml";
                }
                return _packagePath;
            }
            set { _packagePath = value; }
        }

        #endregion

        #region CTOR

        public PackageHandler()
        {
        }

        #endregion

        #region PRIVATE METHODS

        private void addCandidateAssembly(FileInfo candidateAssemblyFileHandle, Dictionary<string, AssemblyInfo> assembliesFound)
        {
            // Create the asseblyInfo object form the file
            AssemblyInfo candidateAssemblyInfo = new AssemblyInfo(candidateAssemblyFileHandle);

            if (assemblyLocationCheck(candidateAssemblyInfo))
            {
                if (multipleAssemblyCheck(candidateAssemblyInfo, assembliesFound))
                {
                    assembliesFound.Add(candidateAssemblyInfo.Key, candidateAssemblyInfo);
                }
            }
        }

        private bool assemblyLocationCheck(AssemblyInfo info)
        {
            return true;
        }

        private Dictionary<string, AssemblyInfo> findAssemblies(DirectoryInfo parentDir)
        {
            Dictionary<string, AssemblyInfo> assembliesFound =
                new Dictionary<string, AssemblyInfo>(StringComparer.InvariantCultureIgnoreCase);

            if (Directory.Exists(Config.Current.ProjectPath))
            {
                DirectoryInfo projectDllPathDir = new DirectoryInfo(Config.Current.ProjectPath);

                findAssembliesInDirectory(projectDllPathDir, assembliesFound);
            }

            return assembliesFound;
        }

        private void findAssembliesInDirectory(DirectoryInfo parentDir, Dictionary<string, AssemblyInfo> assembliesFound)
        {
            foreach (FileInfo dllFileInfo in parentDir.GetFiles("*.dll"))
            {
                if (FileProvider.IncludeFile(dllFileInfo))
                {
                    addCandidateAssembly(dllFileInfo, assembliesFound);
                }
            }

            foreach (DirectoryInfo childDir in FileProvider.GetDirectories(parentDir))
            {
                findAssembliesInDirectory(childDir, assembliesFound);
            }
        }

        private bool multipleAssemblyCheck(AssemblyInfo candidateAssemblyInfo, Dictionary<string, AssemblyInfo> assembliesFound)
        {
            bool result = true;

            // If the assembly already have been added then check on LastWrite date
            if (assembliesFound.ContainsKey(candidateAssemblyInfo.Key))
            {
                AssemblyInfo assemblyInDictionary = assembliesFound[candidateAssemblyInfo.Key];

                string fullName = string.Empty;

                // Check the write date
                if (candidateAssemblyInfo.FileHandle.LastWriteTime > assemblyInDictionary.FileHandle.LastWriteTime)
                {
                    assembliesFound.Remove(assemblyInDictionary.Key);
                    fullName = candidateAssemblyInfo.FileHandle.FullName;
                }
                else
                {
                    // Keep the assembly in the Dictionary
                    result = false;
                    fullName = assemblyInDictionary.FileHandle.FullName;
                }
            }
            return result;
        }

        private PackageMetaData updatePackage(string packagePath)
        {
            PackageMetaData package = null;

            XDocument document = updatePackageXml(XmlReader.Create(packagePath).GetXml());

            if (validatePackageSchema(document))
            {
                string packageName = document
                    .Elements("package")
                    .Select(s => s.Attribute("name").Value)
                    .First();

                string vmgxPath = Config.Current.OutputPath + @"\" + packageName + ".vmgx";

                Console.WriteLine(string.Format("Creating V-Manager package: {0}.vmgx."
                    , packageName
                    )
                    )
                    ;
                using (System.IO.Packaging.Package p = System.IO.Packaging.Package.Open(vmgxPath, FileMode.Create))
                {
                    /*
                     * Adds the assemblies to the package
                     */
                    foreach (KeyValuePair<string, AssemblyInfo> assembly in assembliesFound)
                    {
                        Uri assemblyUri = PackUriHelper.CreatePartUri(new Uri(assembly.Key, UriKind.Relative));
                        PackagePart assemblyPart = p.CreatePart(assemblyUri, "application/x-vmgx", CompressionOption.Normal);

                        using (FileStream fs = assembly.Value.FileHandle.OpenRead())
                        {
                            fs.CopyTo(assemblyPart.GetStream(), 0x1000);
                        }

                        string assemblyRelationship = Settings.GetSetting(Settings.Setting.DefaultPackageRelationship);
                        p.CreateRelationship(assemblyPart.Uri, TargetMode.Internal, assemblyRelationship);
                    }

                    /*
                     * Adds the Package.xml to the package
                     */
                    Uri packageUri = PackUriHelper.CreatePartUri(new Uri("package.xml", UriKind.Relative));
                    PackagePart packagePart = p.CreatePart(packageUri, "text/xml", CompressionOption.Normal);

                    using (StreamWriter sw = new StreamWriter(packagePart.GetStream(), System.Text.Encoding.UTF8))
                    {
                        sw.Write(document.ToString());
                    }

                    string packageRelationship = Settings.GetSetting(Settings.Setting.DefaultPackageRelationship);
                    p.CreateRelationship(packagePart.Uri, TargetMode.Internal, packageRelationship);
                }

                using (FileStream vmgxFile = new FileStream(vmgxPath, FileMode.Open, FileAccess.Read))
                {
                    var memoryStream = new MemoryStream();

                    vmgxFile.CopyTo(memoryStream, 0x1000);

                    package = new PackageMetaData
                    {
                        Deactivated = false,
                        Package = memoryStream.ToArray(),
                        Name = packageName,
                        Description = document.Elements("package").Select(s => s.Attribute("description").Value).FirstOrDefault(),
                        UniqueId = new Guid(document.Elements("package").Select(s => s.Attribute("uniqueId").Value).First()),
                    }
                    ;
                }
            }

            return package;
        }

        private XDocument updatePackageXml(string xml)
        {
            XDocument document = null;

            Console.WriteLine(string.Format("Attempting to update package with detected assemblies."));

            try
            {
                document = XDocument.Parse(xml);

                XElement assembliesElement = document
                    .Element("package")
                    .Element("assemblies")
                    ;

                foreach (KeyValuePair<string, AssemblyInfo> assembly in assembliesFound)
                {
                    XElement element = assembliesElement.Elements()
                        .Where(x =>
                        {
                            return x.Attribute("location").Value.Contains(assembly.Value.Key, StringComparison.InvariantCultureIgnoreCase)
                                || x.Attribute("location").Value.Equals(assembly.Value.Name.FullName);
                        }
                        )
                        .FirstOrDefault()
                        ;

                    if (element == null)
                        assembliesElement.Add(new XElement("assembly"
                            , new XAttribute("location", assembly.Value.Key)
                            , new XAttribute("name", assembly.Value.Name.FullName)
                            )
                            )
                            ;
                    else
                        element.ReplaceWith(new XElement("assembly"
                            , new XAttribute("location", assembly.Value.Key)
                            , new XAttribute("name", assembly.Value.Name.FullName)
                            )
                            )
                            ;

                    Console.WriteLine(string.Format("Updated package with assembly {0}, {1}."
                        , assembly.Value.Key
                        , assembly.Value.Name.FullName
                        )
                        )
                        ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed to update package with detected assemblies.  Exception: {0}."
                    , ex));
            }

            return document;
        }

        private bool validatePackageSchema(XDocument document)
        {
            bool result = false;

            XmlSchema schema = null;

            XmlUrlResolver resolver = new XmlUrlResolver();
            resolver.Credentials = System.Net.CredentialCache.DefaultCredentials;

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;
            settings.IgnoreWhitespace = true;
            settings.XmlResolver = resolver;

            using (var schemaReader = XmlReader.Create(ResourceHelper.UnpackEmbeddedResourceToStream(PackageManager.DEFAULT_PACKAGE_SCHEMA), settings))
            {
                schema = XmlSchema.Read(schemaReader, xmlSchemaValidationEventHandler);
            }

            XmlSchemaSet schemaSet = new XmlSchemaSet();
            schemaSet.Add(schema);

            Console.WriteLine(string.Format("Attempting to validate schema against package."));

            try
            {
                document.Validate(schemaSet
                    , (o, e) =>
                    {
                        throw new XmlException(string.Format("{0}", e.Exception));
                    }
                    , true
                    )
                    ;

                result = true;

                Console.WriteLine(string.Format("Successfully validated schema against package."));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Failed to validated schema against package.  Exception: {0}."
                    , ex));
            }

            return result;
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Builds an entire solution.
        /// </summary>
        public void Execute()
        {
            Console.WriteLine(string.Format("Using project path: '{0}'", Config.Current.ProjectPath));
          
            DirectoryInfo dirInfo = new DirectoryInfo(Config.Current.ProjectPath);

            // Add dlls to the manifest file
            foreach (KeyValuePair<string, AssemblyInfo> assemblies in findAssemblies(dirInfo))
                this.assembliesFound.Add(assemblies.Key, assemblies.Value);
           
            PackageMetaData package = this.updatePackage(this.PackagePath);

            if (package != null)
            {
                //  TODO: Develop a way to automatically deploy package to local server.
                if (Config.Current.UploadPackage)
                {
                    using (AppService app = new AppService())
                    {
                        string serverName = Settings.GetSetting(Settings.Setting.ServerNameOverride);

                        if(string.IsNullOrEmpty(serverName))
                            serverName = System.Environment.MachineName;

                        ServerMetaData server = app.GetServers()
                            .Where(s => s.Name == serverName)
                            .FirstOrDefault()
                            ;

                        if (server == null)
                        {
                            Console.WriteLine(string.Format("This machine '{0}' has not been registered with the V-Manager service.  Registration occurrs when the V-Manager service is first started.", System.Environment.MachineName));
                        }
                        else
                        {
                            Console.WriteLine(string.Format("Saving package: '{0}' to database...", Config.Current.ProjectPath));

                            PackageMetaData existingPackage = app.GetPackages()
                                .Where(p => p.UniqueId == package.UniqueId)
                                .FirstOrDefault()
                                ;

                            if (existingPackage != null)
                                package.PackageId = existingPackage.PackageId;

                            package.ServerId = server.ServerId;
                            package.Deactivated = false;

                            if (!app.Save(package))
                            {
                                Console.WriteLine(string.Format("Failed to save package: '{0}'.  Messages: {1}."
                                    , package.Name
                                    , string.Join(",", app.BrokenRules.Select(r => r.Message).ToArray())));
                            }
                            else
                            {
                                // The package is saved to the database, now we can try to contact the service.
                                Console.WriteLine(string.Format("Loading package: '{0}' to service...", Config.Current.ProjectPath));

                                try
                                {
                                    BasicHttpBinding binding = new BasicHttpBinding();

                                    if (Settings.GetSetting(Settings.Setting.WSProtocol).Equals("https", StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        binding.Security.Mode = BasicHttpSecurityMode.Transport;
                                        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                                    }

                                    ChannelFactory<IPackageOperation> httpFactory = new ChannelFactory<IPackageOperation>(binding
                                        , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/PackageOperation"
                                            , server.WSProtocol
                                            , server.WSFqdn
                                            , server.WSPort
                                        )
                                        )
                                        )
                                        ;
                                    IPackageOperation packageOperationProxy = httpFactory.CreateChannel();
                                    packageOperationProxy.LoadPackage(package);
                                }
                                catch (EndpointNotFoundException ex)
                                {
                                    Console.WriteLine(string.Format("Unable to load package on server.  The service does not appear to be online. Exception: {0}", ex));
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(string.Format("Unable to load package on server on server. Exception: {0}", ex));
                                }

                                if (bool.Parse(Settings.GetSetting(Settings.Setting.CreateSchedules)))
                                {
                                    // The package was loaded in the service, now we can try to recreate any schedules.
                                    Console.WriteLine(string.Format("Recreating schedules: '{0}'...", Config.Current.ProjectPath));

                                    try
                                    {
                                        BasicHttpBinding binding = new BasicHttpBinding();

                                        if (server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            binding.Security.Mode = BasicHttpSecurityMode.Transport;
                                            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                                        }

                                        ChannelFactory<IScheduleOperation> httpFactory = new ChannelFactory<IScheduleOperation>(binding
                                            , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/ScheduleOperation"
                                            , server.WSProtocol
                                            , server.WSFqdn
                                            , server.WSPort
                                            )
                                            )
                                            )
                                            ;

                                        IScheduleOperation scheduleOperationProxy = httpFactory.CreateChannel();
                                        scheduleOperationProxy.SchedulePackage(package);
                                    }
                                    catch (EndpointNotFoundException ex)
                                    {
                                        Console.WriteLine(string.Format("Unable to schedule package on destination server.  The service does not appear to be online. Exception: {0}", ex));
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(string.Format("Unable to schedule package on destination server. Exception: {0}", ex));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region EVENTS

        private static void xmlSchemaValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Error)
                throw new XmlException(string.Format("{0}", e.Exception));
        }

        #endregion

    }
}
