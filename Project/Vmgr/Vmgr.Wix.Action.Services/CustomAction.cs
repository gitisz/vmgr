using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Deployment.WindowsInstaller;
using Vmgr.Wix.Action.Services.AD;
using Vmgr.Wix.Action.Services.ServiceHelper;
using Vmgr.Crypto;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net;
using Microsoft.Win32;

namespace Vmgr.Wix.Action.Services
{
    public class CustomActions
    {
        private static string INSTALLOCATION = string.Empty;

        #region CUSTOM ACTIONS

        [CustomAction]
        public static ActionResult OnInstallVmgrServicesAction(Session session)
        {
            session.Log("Beginning OnInstallVmgrServicesAction...");

#if DEBUG
            //System.Diagnostics.Debugger.Break();
#endif

            CustomActions.INSTALLOCATION = session.CustomActionData["INSTALLOCATION"] + "Services\\";

            if (CustomActions.Install(session))
                return ActionResult.Success;

            return ActionResult.Failure;
        }

        [CustomAction]
        public static ActionResult OnUnInstallVmgrServicesAction(Session session)
        {
            session.Log("Beginning OnUnInstallVmgrServicesAction...");

#if DEBUG
            //System.Diagnostics.Debugger.Break();
#endif
            CustomActions.INSTALLOCATION = session.CustomActionData["INSTALLOCATION"] + "Services\\";

            if (CustomActions.UnInstall(session))
                return ActionResult.Success;

            return ActionResult.Failure;
        }

        [CustomAction]
        public static ActionResult OnStartVmgrServicesAction(Session session)
        {
#if DEBUG
            //System.Diagnostics.Debugger.Break();
#endif
            try
            {
                ServiceState state = ServiceInstaller.GetServiceStatus("Vmgr.Services");

                switch (state)
                {
                    case ServiceState.Unknown:
                        break;
                    case ServiceState.NotFound:
                        break;
                    case ServiceState.Stopped:
                        ServiceInstaller.StartService("Vmgr.Services");
                        break;
                    case ServiceState.StartPending:
                        break;
                    case ServiceState.StopPending:
                        break;
                    case ServiceState.Running:
                        break;
                    case ServiceState.ContinuePending:
                        break;
                    case ServiceState.PausePending:
                        break;
                    case ServiceState.Paused:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                session.Log("Failed to start Vmgr.Services.  Exception: {0}", ex);
            }

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult OnValidateServiceCredentials(Session session)
        {

            session.Log("Beginning OnValidateServiceCredentials...");

#if DEBUG
            //System.Diagnostics.Debugger.Break();
#endif

            bool isValid = false;

            string ldap = session["VMGR_LDAP"];
            string credential = session["VMGR_USERNAME"];
            string domain = string.Empty;
            string username = string.Empty;

            try
            {
                if (credential.IndexOf("\\") > -1)
                    domain = credential.Substring(0, credential.IndexOf("\\"));

                if (credential.IndexOf("\\") > -1)
                    username = credential.Substring(credential.IndexOf("\\") + 1);

                string password = session["VMGR_PASSWORD"]; ;

                ActiveDirectoryContext adc = null;

                if (!string.IsNullOrEmpty(ldap))
                    adc = new ActiveDirectoryContext("");
                else
                    adc = new ActiveDirectoryContext();

                IList<IUser> users =
                    adc.Users.SelectByProperty(UserSearchableProperties.EID, username);

                if (users.Count > 0)
                {
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
                    {
                        // validate the credentials
                        isValid = pc.ValidateCredentials(username, password);
                    }
                }

                session["VMGR_SERVICE_CRED_IS_VALID"] = "0";

                if (isValid)
                    session["VMGR_SERVICE_CRED_IS_VALID"] = "1";
            }
            catch (Exception ex)
            {
                session.Log("Feild to validate service credentials. Exception: {0}", ex);
            }

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult OnValidateSqlServerAuth(Session session)
        {
            session.Log("Beginning OnValidateSqlServerAuth...");

#if DEBUG
            //System.Diagnostics.Debugger.Break();
#endif

            bool isValid = false;

            string connectionString = "data source={0};initial catalog=DOM_Config;integrated security=SSPI;persist security info=False;packet size=4096";

            if (!string.IsNullOrEmpty(session["VMGR_SQL_AUTHENTICATION_MODE"]))
            {
                string sqlServerName = string.Empty;

                if (session["VMGR_SQL_AUTHENTICATION_MODE"] == "Windows Authentication")
                {
                    if (!string.IsNullOrEmpty(session["VMGR_SQL_SERVERNAME"]))
                    {
                        sqlServerName = session["VMGR_SQL_SERVERNAME"];

                        connectionString = string.Format(connectionString, sqlServerName);
                    }
                }

                if (session["VMGR_SQL_AUTHENTICATION_MODE"] == "SQL Server Authentication")
                {
                    string userName = string.Empty;
                    string password = string.Empty;

                    connectionString = "data source={0};initial catalog=DOM_Config;integrated security=False;User ID={1};Password={2};packet size=4096";

                    if (!string.IsNullOrEmpty(session["VMGR_SQL_SERVERNAME"]))
                        sqlServerName = session["VMGR_SQL_SERVERNAME"];

                    if (!string.IsNullOrEmpty(session["VMGR_SQL_USERNAME"]))
                        userName = session["VMGR_SQL_USERNAME"];

                    if (!string.IsNullOrEmpty(session["VMGR_SQL_PASSWORD"]))
                        password = session["VMGR_SQL_PASSWORD"];

                    connectionString = string.Format("data source={0};initial catalog=DOM_Config;integrated security=False;User ID={1};Password={2};packet size=4096"
                        , sqlServerName
                        , userName
                        , password
                        );
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    isValid = true;
                }
                catch (SqlException ex)
                {
                    // output the error to see what's going on
                    session.Message(InstallMessage.Warning, new Record(new string[]
			        {
				        string.Format("Unable to connection to SQL instance: {0}", ex.ToString())
			        }
                    )
                    )
                    ;
                }
            }

            session["SQL_AUTH_IS_VALID"] = "0";

            if (isValid)
            {
                session["SQL_AUTH_IS_VALID"] = "1";
                session["SQL_CONNECTION_STRING"] = connectionString.Replace(";", ";;");
            }

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult OnValidatePorts(Session session)
        {
            session.Log("Beginning OnValidatePorts...");

#if DEBUG
            //System.Diagnostics.Debugger.Launch();
#endif

            bool isValid = false;

            string rtPort = session["VMGR_RT_PORT"].Trim();
            string wsPort = session["VMGR_WS_PORT"].Trim();

            if (rtPort != wsPort)
                isValid = true;

            if (isValid)
            {
                IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
                IPEndPoint[] endPoints = ipGlobalProperties.GetActiveTcpListeners();

                foreach (IPEndPoint ep in endPoints)
                {
                    if (ep.Port.ToString() == rtPort || ep.Port.ToString() == wsPort)
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            if (isValid)
                session["VMGR_PORTS_IS_VALID"] = "1";
            else
            {
                MessageResult r = session.Message(InstallMessage.User + (int)MessageBoxIcon.Warning + (int)MessageBoxButtons.OK,
                     new Record { FormatString = "Please specify a different value for each port." });

            }

            string thumbPrint = string.Empty;

            //  TODO: check registry for a previous installation thumbprint, if none then permit creating a new certificate
            RegistryKey certKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Dominion\Vmgr\Ssl");

            if (certKey != null)
            {
                thumbPrint = (string)certKey.GetValue("Thumbprint");
            }

            if (string.IsNullOrEmpty(thumbPrint))
                session["VMGR_CERTIFICATE_NEEDED"] = "TRUE";
            else
            {
                X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                store.Open(OpenFlags.ReadOnly);
                X509Certificate2 cert = store.Certificates.Cast<X509Certificate2>().Where(c => c.Thumbprint == thumbPrint)
                    .FirstOrDefault();

                if (cert == null)
                    session["VMGR_CERTIFICATE_NEEDED"] = "TRUE";
                else
                {
                    session["VMGR_CERT_THUMBPRINT"] = thumbPrint;

                    IList<string> parts = cert.Subject
                        .Replace(", ", ",")
                        .Split(',')
                        .ToList()
                        ;

                    session["VMGR_CERT_NAME"] = parts
                        .Where(p => p.Contains("CN="))
                        .Select(p => p.Remove(0, 3))
                        .FirstOrDefault();

                    session["VMGR_CERT_ORGANIZATION"] = parts
                        .Where(p => p.Contains("O="))
                        .Select(p => p.Remove(0, 2))
                        .FirstOrDefault();

                    session["VMGR_CERT_ORGUNIT"] = parts
                        .Where(p => p.Contains("OU="))
                        .Select(p => p.Remove(0, 3))
                        .FirstOrDefault();

                    session["VMGR_CERT_LOCATION"] = parts
                        .Where(p => p.Contains("L="))
                        .Select(p => p.Remove(0, 2))
                        .FirstOrDefault();

                    session["VMGR_CERT_STATE"] = parts
                        .Where(p => p.Contains("S="))
                        .Select(p => p.Remove(0, 2))
                        .FirstOrDefault();

                    session["VMGR_CERT_COUNTRY"] = parts
                        .Where(p => p.Contains("C="))
                        .Select(p => p.Remove(0, 2))
                        .FirstOrDefault();

                    session["VMGR_CERT_SIZE"] = cert.PublicKey.Key.KeySize.ToString();
                }
            }

            if (string.IsNullOrEmpty(session["VMGR_RT_PROTOCOL"]))
                session["VMGR_RT_PROTOCOL"] = "HTTP";

            if (string.IsNullOrEmpty(session["VMGR_WS_PROTOCOL"]))
                session["VMGR_WS_PROTOCOL"] = "HTTP";

            return ActionResult.Success;
        }

        [CustomAction]
        public static ActionResult OnPopulatePorts(Session session)
        {
            session.Log("Beginning OnGenerateCertificate...");

#if DEBUG
            //System.Diagnostics.Debugger.Launch();
#endif
            int rtPort = 8080;
            int wsPort = 8000;
            string rtProtocol = string.Empty;
            string wsProtocol = string.Empty;

            RegistryKey certKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Dominion\Vmgr\Ssl");

            if (certKey != null)
            {
                object objRtPort = certKey.GetValue("RTPort");
                object objWsPort = certKey.GetValue("WSPort");
                object objRtProto = certKey.GetValue("RTProtocol");
                object objWsProto = certKey.GetValue("WSProtocol");

                if (objRtPort != null)
                    int.TryParse(objRtPort.ToString(), out rtPort);

                if (objWsPort != null)
                    int.TryParse(objWsPort.ToString(), out wsPort);

                if (objRtProto != null)
                    rtProtocol = certKey.GetValue("RTProtocol").ToString();

                if (objWsProto != null)
                    wsProtocol = certKey.GetValue("WSProtocol").ToString();
            }

            session["VMGR_RT_PORT"] = rtPort.ToString();
            session["VMGR_WS_PORT"] = wsPort.ToString();
            session["VMGR_RT_PROTOCOL"] = rtProtocol;
            session["VMGR_WS_PROTOCOL"] = wsProtocol;

            if (rtProtocol.Equals("HTTP", StringComparison.InvariantCultureIgnoreCase))
                session["VMGR_RT_PROTOCOL"] = string.Empty;

            if (wsProtocol.Equals("HTTP", StringComparison.InvariantCultureIgnoreCase))
                session["VMGR_WS_PROTOCOL"] = string.Empty;

            return ActionResult.Success;

        }

        [CustomAction]
        public static ActionResult OnGenerateCertificate(Session session)
        {
            session.Log("Beginning OnGenerateCertificate...");

#if DEBUG
            System.Diagnostics.Debugger.Launch();
#endif

            bool createCert = false;
           
            string thumbPrint = string.Empty;

            bool.TryParse(session["VMGR_CERTIFICATE_NEEDED"], out createCert);

            if (createCert)
            {
                try
                {
                    using (CryptContext ctx = new CryptContext())
                    {
                        ctx.Open();

                        IList<string> name = new List<string> { };

                        if (string.IsNullOrEmpty(session["VMGR_CERT_NAME"]))
                            throw new InstallCanceledException("Certificate validation error: Certificate Name is a requried field.");

                        name.Add(string.Format("CN={0}", session["VMGR_CERT_NAME"]));

                        if (!string.IsNullOrEmpty(session["VMGR_CERT_ORGANIZATION"]))
                            name.Add(string.Format("O={0}", session["VMGR_CERT_ORGANIZATION"]));

                        if (!string.IsNullOrEmpty(session["VMGR_CERT_ORGUNIT"]))
                            name.Add(string.Format("OU={0}", session["VMGR_CERT_ORGUNIT"]));

                        if (!string.IsNullOrEmpty(session["VMGR_CERT_STATE"]))
                            name.Add(string.Format("S={0}", session["VMGR_CERT_STATE"]));

                        if (!string.IsNullOrEmpty(session["VMGR_CERT_LOCATION"]))
                            name.Add(string.Format("L={0}", session["VMGR_CERT_LOCATION"]));

                        if (!string.IsNullOrEmpty(session["VMGR_CERT_COUNTRY"]))
                            name.Add(string.Format("C={0}", session["VMGR_CERT_COUNTRY"]));

                        string distinguishedName = string.Join(",", name.ToArray());

                        X509Certificate2 cert = ctx.CreateSelfSignedCertificate(
                            new SelfSignedCertProperties
                            {
                                IsPrivateKeyExportable = true,
                                KeyBitLength = int.Parse(session["VMGR_CERT_SIZE"]),
                                Name = new X500DistinguishedName(distinguishedName),
                                ValidFrom = DateTime.Today.AddDays(-1),
                                ValidTo = DateTime.Today.AddYears(1),
                            }
                        )
                        ;

                        //  Write to installation folder
                        File.WriteAllBytes(CustomActions.INSTALLOCATION + "Vmgr.cer", cert.GetRawCertData());

                        //  Write to personal certificate store
                        X509Store storePersonal = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                        storePersonal.Open(OpenFlags.ReadWrite);
                        storePersonal.Add(cert);
                        storePersonal.Close();

                        //  Write to trusted root certificate store
                        X509Store storeRoot = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
                        storeRoot.Open(OpenFlags.ReadWrite);
                        storeRoot.Add(cert);
                        storeRoot.Close();

                        thumbPrint = cert.Thumbprint;
                    }

                    session["VMGR_CERT_THUMBPRINT"] = thumbPrint;
                    session["VMGR_CERTIFICATE_NEEDED"] = "FALSE";
                }
                catch (Exception ex)
                {
                    session.Log("Failed to create/store certificate.  Exception: {0}", ex);
                }

            }

            session.Log("Successfully completed OnGenerateCertificate...");
            
            return ActionResult.Success;

        }

        [CustomAction]
        public static ActionResult OnResetCertificateFields(Session session)
        {
            session.Log("Beginning OnGenerateCertificate...");

#if DEBUG
            //System.Diagnostics.Debugger.Launch();
#endif
            session["VMGR_CERTIFICATE_NEEDED"] = "TRUE";
            session["VMGR_CERT_THUMBPRINT"] = "";
            session["VMGR_CERT_NAME"] = "";
            session["VMGR_CERT_ORGANIZATION"] = "";
            session["VMGR_CERT_ORGUNIT"] = "";
            session["VMGR_CERT_STATE"] = "";
            session["VMGR_CERT_LOCATION"] = "";
            session["VMGR_CERT_COUNTRY"] = "";
            session["VMGR_CERT_SIZE"] = "2048";
            
            return ActionResult.Success;

        }

        #endregion

        #region PRIVATE METHODS

        private static bool Install(Session session)
        {
            session.Message(InstallMessage.Progress, new Record { FormatString = "Changing V-Manager Service configuration connection string..." });

            try
            {
                CustomActions.ChangeConnectionString(session);
            }
            catch (Exception ex)
            {
                session.Log("Failed to change VmgrConnectionString.  Exception: {0}", ex);

                return false;
            }

            session.Message(InstallMessage.Progress, new Record { FormatString = "Configuring V-Manager Service credentials..." });
           
            try
            {
                CustomActions.ConfigureService(session);
            }
            catch (Exception ex)
            {
                session.Log("Failed to configure V-Manager Service.  Exception: {0}", ex);

                return false;
            }

            session.Message(InstallMessage.Progress, new Record { FormatString = "Configuring encryption options, ports, and certificate..." });

            try
            {
                CustomActions.ChangePortsAndProtocol(session);
            }
            catch (Exception ex)
            {
                session.Log("Failed to configure encryption.  Exception: {0}", ex);

                return false;
            }

            return true;
        }

        private static void ChangeConnectionString(Session session)
        {
            string connectionString = string.Empty;
            string configurationFile = CustomActions.INSTALLOCATION + "Vmgr.Services.exe.config";

            if (!string.IsNullOrEmpty(session.CustomActionData["SQL_CONNECTION_STRING"]))
            {
                connectionString = session.CustomActionData["SQL_CONNECTION_STRING"];
            }

            //  Write connection string to config file.
            XDocument document = XDocument.Load(configurationFile);
            XElement appSetttings = document
                .Elements("configuration")
                .Elements("appSettings")
                .FirstOrDefault()
                ;

            XElement vmgrConnectionStringSetting = appSetttings.Descendants()
                .Where(a => a.Attribute("key").Value == "VmgrConnectionString")
                .First()
                ;

            vmgrConnectionStringSetting.Attribute("value").Value = connectionString;

            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(configurationFile, xws))
            {
                document.WriteTo(xw);
            }
        }

        private static void ChangePortsAndProtocol(Session session)
        {
            string configurationFile = CustomActions.INSTALLOCATION + "Vmgr.Services.exe.config";

            int rtPort = 8080;
            int wsPort = 8000;

            string rtProtocol = "HTTP";
            string wsProtocol = "HTTP";

            int.TryParse(session.CustomActionData["VMGR_RT_PORT"].Trim(), out rtPort);
            int.TryParse(session.CustomActionData["VMGR_WS_PORT"].Trim(), out wsPort);

            rtProtocol = session.CustomActionData["VMGR_RT_PROTOCOL"];
            wsProtocol = session.CustomActionData["VMGR_WS_PROTOCOL"];

            if (rtPort == 0)
                rtPort = 8080;

            if (wsPort == 0)
                wsPort = 8000;

            //  Write connection string to config file.
            XDocument document = XDocument.Load(configurationFile);
            XElement appSetttings = document
                .Elements("configuration")
                .Elements("appSettings")
                .FirstOrDefault()
                ;

            XElement rtProtocolSetting = appSetttings.Descendants()
                .Where(a => a.Attribute("key").Value == "RTProtocol")
                .First()
                ;

            XElement wsProtocolSetting = appSetttings.Descendants()
                .Where(a => a.Attribute("key").Value == "WSProtocol")
                .First()
                ;

            rtProtocolSetting.Attribute("value").Value = rtProtocol;
            wsProtocolSetting.Attribute("value").Value = wsProtocol;

            XElement rtPortSetting = appSetttings.Descendants()
                .Where(a => a.Attribute("key").Value == "RTPort")
                .First()
                ;

            XElement wsPortSetting = appSetttings.Descendants()
                .Where(a => a.Attribute("key").Value == "WSPort")
                .First()
                ;

            rtPortSetting.Attribute("value").Value = rtPort.ToString();
            wsPortSetting.Attribute("value").Value = wsPort.ToString();

            XmlWriterSettings xws = new XmlWriterSettings();
            xws.OmitXmlDeclaration = true;
            xws.Indent = true;

            using (XmlWriter xw = XmlWriter.Create(configurationFile, xws))
            {
                document.WriteTo(xw);
            }
        }

        private static void ConfigureService(Session session)
        {
            ServiceInstaller.Install("Vmgr.Services"
                , "V-Manager Service"
                , CustomActions.INSTALLOCATION + "Vmgr.Services.exe"
                , session.CustomActionData["VMGR_USERNAME"]
                , session.CustomActionData["VMGR_PASSWORD"]
                );

            string credential = session.CustomActionData["VMGR_USERNAME"];
            string domain = string.Empty;
            string username = string.Empty;
            {
                if (credential.IndexOf("\\") > -1)
                    domain = credential.Substring(0, credential.IndexOf("\\"));

                if (credential.IndexOf("\\") > -1)
                    username = credential.Substring(credential.IndexOf("\\") + 1);

                long result = ServiceInstaller.SetRight(username, "SeServiceLogonRight");
            }
        }

        private static void RemoveServiceFiles(Session session)
        {
            DirectoryInfo serviceDirectory = null;

            if (Directory.Exists(CustomActions.INSTALLOCATION))
            {
                serviceDirectory = new DirectoryInfo(CustomActions.INSTALLOCATION);

                foreach (FileInfo file in serviceDirectory.GetFiles())
                    file.Delete();

                foreach (DirectoryInfo dir in serviceDirectory.GetDirectories())
                    dir.Delete(true);
            }
        }

        private static void StopService(Session session)
        {
            ServiceState state = ServiceInstaller.GetServiceStatus("Vmgr.Services");

            switch (state)
            {
                case ServiceState.Unknown:
                    break;
                case ServiceState.NotFound:
                    break;
                case ServiceState.Stopped:
                    break;
                case ServiceState.StartPending:
                    ServiceInstaller.StopService("Vmgr.Services");
                    break;
                case ServiceState.StopPending:
                    break;
                case ServiceState.Running:
                    ServiceInstaller.StopService("Vmgr.Services");
                    break;
                case ServiceState.ContinuePending:
                    ServiceInstaller.StopService("Vmgr.Services");
                    break;
                case ServiceState.PausePending:
                    ServiceInstaller.StopService("Vmgr.Services");
                    break;
                case ServiceState.Paused:
                    ServiceInstaller.StopService("Vmgr.Services");
                    break;
                default:
                    break;
            }
        }

        private static bool UnInstall(Session session)
        {
            try
            {
                CustomActions.StopService(session);
            }
            catch (Exception ex)
            {
                session.Log("Failed to stop Vmgr.Services.  Exception: {0}", ex);

                return false;
            }

            try
            {
                CustomActions.UninstallService(session);
            }
            catch (Exception ex)
            {
                session.Log("Failed to uninstall Vmgr.Services.  Exception: {0}", ex);

                return false;
            }

            try
            {
                CustomActions.RemoveServiceFiles(session);
            }
            catch (Exception ex)
            {
                session.Log("Failed to remove Vmgr.Services files.  Exception: {0}", ex);

                return false;
            }

            return true;

        }

        private static void UninstallService(Session session)
        {
            ServiceState state = ServiceInstaller.GetServiceStatus("Vmgr.Services");

            switch (state)
            {
                case ServiceState.Unknown:
                    session.Log("The state of Vmgr.Services is Unknown.");
                    break;
                case ServiceState.NotFound:
                    session.Log("The state of Vmgr.Services is NotFound.");
                    break;
                case ServiceState.Stopped:
                    ServiceInstaller.Uninstall("Vmgr.Services");
                    break;
                case ServiceState.StartPending:
                    session.Log("The state of Vmgr.Services is StartPending.");
                    break;
                case ServiceState.StopPending:
                    session.Log("The state of Vmgr.Services is StopPending.");
                    break;
                case ServiceState.Running:
                    session.Log("The state of Vmgr.Services is Running.");
                    break;
                case ServiceState.ContinuePending:
                    session.Log("The state of Vmgr.Services is ContinuePending.");
                    break;
                case ServiceState.PausePending:
                    session.Log("The state of Vmgr.Services is PausePending.");
                    break;
                case ServiceState.Paused:
                    session.Log("The state of Vmgr.Services is Paused.");
                    break;
                default:
                    break;
            }
        }

        #endregion

    }
}
