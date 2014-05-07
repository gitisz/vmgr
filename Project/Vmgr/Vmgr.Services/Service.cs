using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.Win32;
using Owin;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;
using System.Threading;
using Vmgr.Configuration;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Messaging;
using Vmgr.Operations;
using Vmgr.Packaging;
using Vmgr.Scheduling;

namespace Vmgr.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class Service : ServiceBase
    {
        #region PRIVATE PROPERTIES

        private Thread onStartThread = null;

        private int retries = 0;

        #endregion

        #region PROTECTED PROPERTIES

        protected static ServerMetaData server { get; set; }
        protected static ServiceHost packageServiceHost = new ServiceHost(typeof(PackageOperation));
        protected static ServiceHost uploadServiceHost = new ServiceHost(typeof(UploadOperation));
        protected static ServiceHost moveServiceHost = new ServiceHost(typeof(MoveOperation));
        protected static ServiceHost scheduleServiceHost = new ServiceHost(typeof(ScheduleOperation));
        protected static ServiceHost statusServiceHost = new ServiceHost(typeof(StatusOperation));
        protected static IHubContext hubContext = null;

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        public Service()
        {

#if CONSOLE
#else	
            InitializeComponent();

            this.ServiceName = "Vmgr.Services";
#endif
        }

        #endregion

        #region PRIVATE METHODS

        private void configureServer()
        {
            using (AppService app = new AppService(WindowsIdentity.GetCurrent()))
            {
                ServerMetaData server = app.GetServers()
                    .Where(s => s.Name == System.Environment.MachineName)
                    .FirstOrDefault() ?? new ServerMetaData
                        {
                            Name = System.Environment.MachineName,
                            Description = null,
                            UniqueId = Guid.NewGuid()
                        }
                        ;

                server.WSProtocol = Settings.GetSetting(Settings.Setting.WSProtocol);
                server.WSPort = int.Parse(Settings.GetSetting(Settings.Setting.WSPort));

                if (server.ServerId == 0)
                    server.WSFqdn = VmgrHub.GetLocalhostFqdn(System.Environment.MachineName);
                
                server.RTProtocol = Settings.GetSetting(Settings.Setting.RTProtocol);
                server.RTPort = int.Parse(Settings.GetSetting(Settings.Setting.RTPort));
                
                if (server.ServerId == 0)
                    server.RTFqdn = VmgrHub.GetLocalhostFqdn(System.Environment.MachineName);

                if (app.Save(server))
                    Service.server = server;
                else
                    throw new ApplicationException("Failed to define server.");
            }
        }

        private void configureMessaging()
        {
            string registerUrl = string.Format("{0}://*:{1}/", Service.server.RTProtocol, Service.server.RTPort);

            try
            {
#if DEBUG
                //System.Diagnostics.Debugger.Launch();
#endif
                //  Starts an owin web application to host SignalR, using the protocol and port defined.
                WebApp.Start<StartUp>(registerUrl);
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to configure messaging.", ex, LogType.Error);

                if (ex is HttpListenerException || ex.InnerException is HttpListenerException)
                {
                    try
                    {
                        Process p = new Process();
                        p.StartInfo.UseShellExecute = false;
                        p.StartInfo.RedirectStandardOutput = true;
                        p.StartInfo.FileName = "netsh.exe";
                        p.StartInfo.Arguments = string.Format("netsh http delete urlacl url={0}"
                            , registerUrl
                            );
                        p.Start();
                        p.StandardOutput.ReadToEnd();
                        p.WaitForExit();
                    }
                    catch (Exception ex2)
                    {
                        Logger.Logs.Log(string.Format("Failed to delete urlacl {0}.", registerUrl)
                            , ex2
                            , LogType.Error
                            )
                            ;

                        retries = 5;
                    }
                }

                if (retries < 5)
                {
                    retries++;

                    Logger.Logs.Log(string.Format("Attempting to configure messaging again.  Attempt No. {0}", retries), LogType.Warn);

                    Thread.Sleep(1000);

                    configureMessaging();
                }
                else
                    Logger.Logs.Log("Exceeded total number of retries to configure messaging.", LogType.Error);

            }
        }

        private void initialize()
        {
            this.initializeCertificatesAndPorts();

            this.initializeServiceOperations();

            this.initializePluginsAndSchedules();
        }

        private void initializeCertificatesAndPorts()
        {
            Logger.Logs.Log("Attempting to initialize certificates and ports.", LogType.Info);
            
            try
            {
                RegistryKey certKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Dominion\Vmgr\Ssl");

                if (certKey == null)
                    Logger.Logs.Log(@"The registry follwing key was not found: 'SOFTWARE\Wow6432Node\Dominion\Vmgr\Ssl'", LogType.Warn);

                string thumbprint = string.Empty;

                if (certKey != null)
                {
                    thumbprint = (string)certKey.GetValue("Thumbprint");

                    bool needsCert = false;

                    if (server.WSProtocol.Equals("HTTPS", StringComparison.InvariantCultureIgnoreCase))
                    {
                        needsCert = true;

                        Process wsProcess = new Process();
                        wsProcess.StartInfo.UseShellExecute = false;
                        wsProcess.StartInfo.RedirectStandardOutput = true;
                        wsProcess.StartInfo.FileName = @"..\SSL\Vmgr.SSLTool.exe";
#if DEBUG && CONSOLE
                        wsProcess.StartInfo.FileName = @"..\..\Vmgr.SSLTool\bin\Vmgr.SSLTool.exe";
#endif
                        wsProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        wsProcess.StartInfo.Arguments = string.Format("-r {0} -s {1} -t {2} -u {3}"
                            , server.WSPort
                            , server.WSProtocol
                            , thumbprint
                            , WindowsIdentity.GetCurrent().Name
                            );
                        wsProcess.Start();

                        string wsOutput = wsProcess.StandardOutput.ReadToEnd();
                        wsProcess.WaitForExit();

                        Logger.Logs.Log(wsOutput, LogType.Info);
                    }

                    if (server.RTProtocol.Equals("HTTPS", StringComparison.InvariantCultureIgnoreCase))
                    {
                        needsCert = true;

                        Process rtProcess = new Process();
                        rtProcess.StartInfo.UseShellExecute = false;
                        rtProcess.StartInfo.RedirectStandardOutput = true;
                        rtProcess.StartInfo.FileName = @"..\SSL\Vmgr.SSLTool.exe";
#if DEBUG && CONSOLE
                        rtProcess.StartInfo.FileName = @"..\..\Vmgr.SSLTool\bin\Vmgr.SSLTool.exe";
#endif

                        rtProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        rtProcess.StartInfo.Arguments = string.Format("-r {0} -s {1} -t {2} -u {3}"
                            , server.RTPort
                            , server.RTProtocol
                            , thumbprint
                            , WindowsIdentity.GetCurrent().Name
                            );
                        rtProcess.Start();

                        string rtOutput = rtProcess.StandardOutput.ReadToEnd();
                        rtProcess.WaitForExit();

                        Logger.Logs.Log(rtOutput, LogType.Info);
                    }

                    if (needsCert)
                    {
                        //  Write the certificate to the current directory so SignalR can use it.
                        X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                        store.Open(OpenFlags.ReadOnly);

                        X509Certificate2 cert = store.Certificates.Cast<X509Certificate2>()
                            .Where(c => c.Thumbprint.Equals(thumbprint, StringComparison.InvariantCultureIgnoreCase))
                            .FirstOrDefault();

                        File.WriteAllBytes(Environment.CurrentDirectory + @"\Vmgr.cer", cert.GetRawCertData());
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to initialize certificates and ports", ex, LogType.Error);
            }

            Logger.Logs.Log("Successfully initialized certificates and ports.", LogType.Info);
        }

        private void initializeServiceOperations()
        {
            Logger.Logs.Log("Attempting to initialize service operations.", LogType.Info);
            
            try
            {
                /*
                 * PackageOperation
                 */
                string packageOperation = string.Format("{0}://localhost:{1}/Vmgr.Operations/PackageOperation"
                    , Service.server.WSProtocol
                    , Service.server.WSPort
                    )
                    ;

                Logger.Logs.Log(string.Format("Attempting to open service host at {0}.", packageOperation), LogType.Info);

                ServiceMetadataBehavior packageServiceBehavior = new ServiceMetadataBehavior();

                BasicHttpBinding packageBinding = new BasicHttpBinding();
                packageBinding.MaxReceivedMessageSize = Int64.Parse(Settings.GetSetting(Settings.Setting.PackageMaxReceivedMessageSize)); // Default: 20971520 => 20MB
                packageBinding.ReaderQuotas.MaxArrayLength = 2147483647;

                if (Service.server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    packageServiceBehavior.HttpGetEnabled = true;
                    packageServiceBehavior.HttpGetUrl = new Uri(packageOperation);
                    packageBinding.Security.Mode = BasicHttpSecurityMode.None;
                }

                if (Service.server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    packageServiceBehavior.HttpsGetEnabled = true;
                    packageServiceBehavior.HttpsGetUrl = new Uri(packageOperation);
                    packageBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                    packageBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                }


                Service.packageServiceHost.Description.Behaviors.Add(packageServiceBehavior);
                Service.packageServiceHost.AddServiceEndpoint(typeof(IPackageOperation)
                    , packageBinding
                    , new Uri(packageOperation)
                    );
                Service.packageServiceHost.Open();

                Logger.Logs.Log(string.Format("Successfully opened service host at {0}.", packageOperation), LogType.Info);


                /*
                 * UploadOperation
                 */
                string uploadOperation = string.Format("{0}://localhost:{1}/Vmgr.Operations/UploadOperation"
                    , Service.server.WSProtocol
                    , Service.server.WSPort
                    )
                    ;

                Logger.Logs.Log(string.Format("Attempting to open service host at {0}.", uploadOperation), LogType.Info);

                ServiceMetadataBehavior uploadServiceBehavior = new ServiceMetadataBehavior();

                BasicHttpBinding uploadBinding = new BasicHttpBinding();
                uploadBinding.MaxReceivedMessageSize = Int64.Parse(Settings.GetSetting(Settings.Setting.PackageMaxReceivedMessageSize)); // Default: 20971520 => 20MB
                uploadBinding.ReaderQuotas.MaxArrayLength = 2147483647;

                if (Service.server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    uploadServiceBehavior.HttpGetEnabled = true;
                    uploadServiceBehavior.HttpGetUrl = new Uri(uploadOperation);
                    uploadBinding.Security.Mode = BasicHttpSecurityMode.None;
                }

                if (Service.server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    uploadServiceBehavior.HttpsGetEnabled = true;
                    uploadServiceBehavior.HttpsGetUrl = new Uri(uploadOperation);
                    uploadBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                    uploadBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                }


                Service.uploadServiceHost.Description.Behaviors.Add(uploadServiceBehavior);
                Service.uploadServiceHost.AddServiceEndpoint(typeof(IUploadOperation)
                    , uploadBinding
                    , new Uri(uploadOperation)
                    );
                Service.uploadServiceHost.Open();

                Logger.Logs.Log(string.Format("Successfully opened service host at {0}.", uploadOperation), LogType.Info);

                /*
                 * MoveOperation
                 */
                string moveOperation = string.Format("{0}://localhost:{1}/Vmgr.Operations/MoveOperation"
                    , Service.server.WSProtocol
                    , Service.server.WSPort
                    )
                    ;

                Logger.Logs.Log(string.Format("Attempting to open service host at {0}.", moveOperation), LogType.Info);

                ServiceMetadataBehavior moveServiceBehavior = new ServiceMetadataBehavior();

                BasicHttpBinding moveBinding = new BasicHttpBinding();
                moveBinding.MaxReceivedMessageSize = Int64.Parse(Settings.GetSetting(Settings.Setting.PackageMaxReceivedMessageSize)); // Default: 20971520 => 20MB
                moveBinding.ReaderQuotas.MaxArrayLength = 2147483647;

                if (Service.server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    moveServiceBehavior.HttpGetEnabled = true;
                    moveServiceBehavior.HttpGetUrl = new Uri(moveOperation);
                    moveBinding.Security.Mode = BasicHttpSecurityMode.None;
                }

                if (Service.server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    moveServiceBehavior.HttpsGetEnabled = true;
                    moveServiceBehavior.HttpsGetUrl = new Uri(moveOperation);
                    moveBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                    moveBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                }


                Service.moveServiceHost.Description.Behaviors.Add(moveServiceBehavior);
                Service.moveServiceHost.AddServiceEndpoint(typeof(IMoveOperation)
                    , moveBinding
                    , new Uri(moveOperation)
                    );
                Service.moveServiceHost.Open();

                Logger.Logs.Log(string.Format("Successfully opened service host at {0}.", moveOperation), LogType.Info);

                /*
                 * ScheduleOperation
                 */

                string scheduleOperation = string.Format("{0}://localhost:{1}/Vmgr.Operations/ScheduleOperation"
                    , Service.server.WSProtocol
                    , Service.server.WSPort
                    )
                    ;

                Logger.Logs.Log(string.Format("Attempting to open service host at {0}.", scheduleOperation), LogType.Info);

                ServiceMetadataBehavior scheduleServiceBehavior = new ServiceMetadataBehavior();

                BasicHttpBinding scheduleBinding = new BasicHttpBinding();
                scheduleBinding.MaxReceivedMessageSize = Int64.Parse(Settings.GetSetting(Settings.Setting.PackageMaxReceivedMessageSize)); // Default: 20971520 => 20MB
                scheduleBinding.ReaderQuotas.MaxArrayLength = 2147483647;

                if (Service.server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    scheduleServiceBehavior.HttpGetEnabled = true;
                    scheduleServiceBehavior.HttpGetUrl = new Uri(scheduleOperation);
                    scheduleBinding.Security.Mode = BasicHttpSecurityMode.None;
                }

                if (Service.server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    scheduleServiceBehavior.HttpsGetEnabled = true;
                    scheduleServiceBehavior.HttpsGetUrl = new Uri(scheduleOperation);
                    scheduleBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                    scheduleBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                }

                Service.scheduleServiceHost.Description.Behaviors.Add(scheduleServiceBehavior);
                Service.scheduleServiceHost.AddServiceEndpoint(typeof(IScheduleOperation)
                    , scheduleBinding
                    , new Uri(scheduleOperation)
                    );
                Service.scheduleServiceHost.Open();

                Logger.Logs.Log(string.Format("Successfully opened service host at {0}.", scheduleOperation), LogType.Info);

                /*
                 * StatusOperation
                 */

                string statusOperation = string.Format("{0}://localhost:{1}/Vmgr.Operations/StatusOperation"
                    , Service.server.WSProtocol
                    , Service.server.WSPort
                    )
                    ;

                Logger.Logs.Log(string.Format("Attempting to open service host at {0}.", statusOperation), LogType.Info);

                ServiceMetadataBehavior statusServiceBehavior = new ServiceMetadataBehavior();

                BasicHttpBinding statusBinding = new BasicHttpBinding();
                statusBinding.MaxReceivedMessageSize = Int64.Parse(Settings.GetSetting(Settings.Setting.PackageMaxReceivedMessageSize)); // Default: 20971520 => 20MB
                statusBinding.ReaderQuotas.MaxArrayLength = 2147483647;

                if (Service.server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    statusServiceBehavior.HttpGetEnabled = true;
                    statusServiceBehavior.HttpGetUrl = new Uri(statusOperation);
                    statusBinding.Security.Mode = BasicHttpSecurityMode.None;
                }

                if (Service.server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    statusServiceBehavior.HttpsGetEnabled = true;
                    statusServiceBehavior.HttpsGetUrl = new Uri(statusOperation);
                    statusBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                    statusBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                }

                Service.statusServiceHost.Description.Behaviors.Add(statusServiceBehavior);
                Service.statusServiceHost.AddServiceEndpoint(typeof(IStatusOperation)
                    , statusBinding
                    , new Uri(statusOperation)
                    );
                Service.statusServiceHost.Open();

                Logger.Logs.Log(string.Format("Successfully opened service host at {0}.", statusOperation), LogType.Info);
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to initialize service hosts", ex, LogType.Error);
            }

            Logger.Logs.Log("Successfully initialized service operations.", LogType.Info);
        }

        private void initializePluginsAndSchedules()
        {
            Logger.Logs.Log("Attempting to initialize plugins and schedules.", LogType.Info);

            IPackageOperation packageOperation = new PackageOperation();
            packageOperation.Load();

            IScheduleOperation scheduleOperation = new ScheduleOperation();
            scheduleOperation.Schedule();

            Logger.Logs.Log("Successfully initialized plugins and schedules.", LogType.Info);
        }

        #endregion

        #region PROTECTED METHODS

        protected override void OnStart(string[] args)
        {
#if DEBUG
            //System.Diagnostics.Debugger.Launch();

            //SD.Tools.OrmProfiler.Interceptor.InterceptorCore.Initialize("Vmgr.Services");
#endif
            //  Set to run in the install directory.
            String path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            path = System.IO.Path.GetDirectoryName(path);
            Directory.SetCurrentDirectory(path);

            Logger.Logs.Log("Attempting to configure server.", LogType.Info);

            this.configureServer();

            Logger.Logs.Log("Successfully configured server.", LogType.Info);

            Logger.Logs.Log("Attempting to configure messaging.", LogType.Info);

            this.configureMessaging();

            Logger.Logs.Log("Successfully configured messaging.", LogType.Info);

            ThreadStart st = new ThreadStart(initialize);
            onStartThread = new Thread(st);
            onStartThread.Start();
        }

        protected override void OnStop()
        {
            try
            {
                Logger.Logs.Log("Attempting to unschedule all plugins.", LogType.Info);

                IScheduleOperation scheduleOperation = new ScheduleOperation();
                scheduleOperation.Unschedule();

                Logger.Logs.Log("Successfully unscheduled all packages.", LogType.Info);

                Logger.Logs.Log("Attempting to unload all packages.", LogType.Info);

                IPackageOperation packageOperation = new PackageOperation();
                packageOperation.Unload();

                Logger.Logs.Log("Successfully unloaded all packages.", LogType.Info);
            }
            catch(Exception ex)
            {
                Logger.Logs.Log("There was a problem unloading the application.", ex, LogType.Error); 
            }
            finally
            {
                Logger.Logs.Log("Closing service hosts.", LogType.Info);

                if (Service.packageServiceHost != null)
                    Service.packageServiceHost.Close();

                if (Service.uploadServiceHost != null)
                    Service.uploadServiceHost.Close();

                if (Service.moveServiceHost != null)
                    Service.moveServiceHost.Close();

                if (Service.scheduleServiceHost != null)
                    Service.scheduleServiceHost.Close();

                if (Service.statusServiceHost != null)
                    Service.statusServiceHost.Close();
            }
        }

#if CONSOLE
        public void Start()
        {
            this.OnStart(null);
        }
#endif
        #endregion

        #region PUBLIC METHODS


        #endregion

        #region EVENTS

        #endregion

    }
}
