using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using Vmgr.Configuration;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Monitoring;
using Vmgr.Packaging;
using Vmgr.Scheduling;

#if NET_40

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Client.Hubs;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNet.SignalR.Client;
using System.Diagnostics;
using Vmgr.Messaging;
using System.Timers;
using Vmgr.Data.Biz;

#endif

namespace Vmgr.Plugins
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public sealed class PluginManager<P> : MarshalByRefObject
        where P : class, IPlugin
    {
        #region PRIVATE PROPERTIES

        private static PluginManager<P> _manage;
        private static ServiceHost packageInspectorServiceHost = new ServiceHost(typeof(PackageInspector));
        private static ServiceHost pluginInspectorServiceHost = new ServiceHost(typeof(PluginInspector));
        private static ServiceHost schedulerManagerServiceHost = new ServiceHost(typeof(SchedulerManager));
        private static ServiceHost monitoringManagerServiceHost = new ServiceHost(typeof(MonitoringManager));

        private IList<P> _plugins = null;
        private IList<IMonitor> _monitors = null;

#if NET_40

        private PerformanceCounter _cpuCounter = null;
        private PerformanceCounter _processCounter = null;
        private PerformanceCounter _memoryCounter = null;
        private IHubProxy _proxy = null;
        private HubConnection _hubConnection = null;

#endif
        #endregion

        #region PROTECTED PROPERTIES

        private bool ProtocolHttp
        {
            get
            {
                return Settings.GetSetting(Settings.Setting.WSProtocol).Equals("http", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        private bool ProtocolHttps
        {
            get
            {
                return Settings.GetSetting(Settings.Setting.WSProtocol).Equals("https", StringComparison.InvariantCultureIgnoreCase);
            }
        }

        public IList<IMonitor> Monitors
        {
            get
            {
                if (this._monitors == null)
                {
                    this._monitors = new List<IMonitor> { };
                }

                return this._monitors;
            }
            set
            {
                this._monitors = value;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

#if NET_40

        public PerformanceCounter CpuCounter
        {
            get
            {
                if (this._cpuCounter == null)
                {
                    this._cpuCounter = new PerformanceCounter("Process", "% Processor Time", "_Total");
                }

                return this._cpuCounter;
            }
        }

        public PerformanceCounter ProcessCounter
        {
            get
            {
                if (this._processCounter == null)
                {
                    this._processCounter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);
                }

                return this._processCounter;
            }
        }

        public PerformanceCounter MemoryCounter
        {
            get
            {
                if (this._memoryCounter == null)
                {
                    this._memoryCounter = new PerformanceCounter("Process", "Working Set", Process.GetCurrentProcess().ProcessName);
                }

                return this._memoryCounter;
            }
        }

        public Timer MonitorTimer { get; set; }

        public bool IsMonitoring { get; set; }

#endif

        public static PluginManager<P> Manage
        {
            get
            {
                if (_manage == null)
                {
                    _manage = new PluginManager<P> { };
                }
                return _manage;
            }
        }

        public IList<P> Plugins
        {
            get
            {
                if (this._plugins == null)
                {
                    this._plugins = new List<P> { };
                }

                return this._plugins;
            }
        }

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void initializePackageInspectorService()
        {
            ServiceMetadataBehavior packageInspectorServiceBehavior = new ServiceMetadataBehavior();

            BasicHttpBinding packageInspectorBinding = new BasicHttpBinding();

            Uri serviceUri = new Uri(string.Format("{0}://localhost:{1}/Vmgr.Packaging/{2}/PackageInspector"
                    , Settings.GetSetting(Settings.Setting.WSProtocol)
                    , Settings.GetSetting(Settings.Setting.WSPort)
                    , AppDomain.CurrentDomain.FriendlyName
                    )
                    );

            if (ProtocolHttp)
            {
                packageInspectorServiceBehavior.HttpGetEnabled = true;
                packageInspectorServiceBehavior.HttpGetUrl = serviceUri;
                packageInspectorBinding.Security.Mode = BasicHttpSecurityMode.None;
            }

            if (ProtocolHttps)
            {
                packageInspectorServiceBehavior.HttpsGetEnabled = true;
                packageInspectorServiceBehavior.HttpsGetUrl = serviceUri;
                packageInspectorBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                packageInspectorBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            }

            packageInspectorServiceHost = new ServiceHost(typeof(PackageInspector));
            packageInspectorServiceHost.Description.Behaviors.Add(packageInspectorServiceBehavior);
            packageInspectorServiceHost.AddServiceEndpoint(typeof(IPackageInspector)
                , packageInspectorBinding
                , serviceUri
                )
                ;
            packageInspectorServiceHost.Open();
        }

        private void initializePluginInspectorService()
        {
            ServiceMetadataBehavior pluginInspectorServiceBehavior = new ServiceMetadataBehavior();

            BasicHttpBinding pluginInspectorBinding = new BasicHttpBinding();

            Uri serviceUri = new Uri(string.Format("{0}://localhost:{1}/Vmgr.Plugins/{2}/PluginInspector"
                    , Settings.GetSetting(Settings.Setting.WSProtocol)
                    , Settings.GetSetting(Settings.Setting.WSPort)
                    , AppDomain.CurrentDomain.FriendlyName
                    )
                    );

            if (ProtocolHttp)
            {
                pluginInspectorServiceBehavior.HttpGetEnabled = true;
                pluginInspectorServiceBehavior.HttpGetUrl = serviceUri;
                pluginInspectorBinding.Security.Mode = BasicHttpSecurityMode.None;
            }

            if (ProtocolHttps)
            {
                pluginInspectorServiceBehavior.HttpsGetEnabled = true;
                pluginInspectorServiceBehavior.HttpsGetUrl = serviceUri;
                pluginInspectorBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                pluginInspectorBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            }

            pluginInspectorServiceHost = new ServiceHost(typeof(PluginInspector));
            pluginInspectorServiceHost.Description.Behaviors.Add(pluginInspectorServiceBehavior);
            pluginInspectorServiceHost.AddServiceEndpoint(typeof(IPluginInspector)
                , pluginInspectorBinding
                , serviceUri
                )
                ;
            pluginInspectorServiceHost.Open();
        }

        private void initializeMonitoringService()
        {
            ServiceMetadataBehavior monitoringServiceBehavior = new ServiceMetadataBehavior();

            BasicHttpBinding monitoringBinding = new BasicHttpBinding();

            Uri serviceUri = new Uri(string.Format("{0}://localhost:{1}/Vmgr.Monitoring/{2}/MonitoringManager"
                , Settings.GetSetting(Settings.Setting.WSProtocol)
                , Settings.GetSetting(Settings.Setting.WSPort)
                , AppDomain.CurrentDomain.FriendlyName
                )
                );

            if (ProtocolHttp)
            {
                monitoringServiceBehavior.HttpGetEnabled = true;
                monitoringServiceBehavior.HttpGetUrl = serviceUri;
                monitoringBinding.Security.Mode = BasicHttpSecurityMode.None;
            }

            if (ProtocolHttps)
            {
                monitoringServiceBehavior.HttpsGetEnabled = true;
                monitoringServiceBehavior.HttpsGetUrl = serviceUri;
                monitoringBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                monitoringBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            }

            monitoringManagerServiceHost = new ServiceHost(typeof(MonitoringManager));
            monitoringManagerServiceHost.Description.Behaviors.Add(monitoringServiceBehavior);
            monitoringManagerServiceHost.AddServiceEndpoint(typeof(IMonitoring)
                , monitoringBinding
                , serviceUri
                )
                ;
            monitoringManagerServiceHost.Open();
        }

        private void initializeSchedulableService()
        {
            ServiceMetadataBehavior pluginSchedulerServiceBehavior = new ServiceMetadataBehavior();

            BasicHttpBinding pluginSchedulerBinding = new BasicHttpBinding();

            Uri serviceUri = new Uri(string.Format("{0}://localhost:{1}/Vmgr.Scheduling/{2}/SchedulerManager"
                , Settings.GetSetting(Settings.Setting.WSProtocol)
                , Settings.GetSetting(Settings.Setting.WSPort)
                , AppDomain.CurrentDomain.FriendlyName
                )
                );

            if (ProtocolHttp)
            {
                pluginSchedulerServiceBehavior.HttpGetEnabled = true;
                pluginSchedulerServiceBehavior.HttpGetUrl = serviceUri;
                pluginSchedulerBinding.Security.Mode = BasicHttpSecurityMode.None;
            }

            if (ProtocolHttps)
            {
                pluginSchedulerServiceBehavior.HttpsGetEnabled = true;
                pluginSchedulerServiceBehavior.HttpsGetUrl = serviceUri;
                pluginSchedulerBinding.Security.Mode = BasicHttpSecurityMode.Transport;
                pluginSchedulerBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            }

            schedulerManagerServiceHost = new ServiceHost(typeof(SchedulerManager));
            schedulerManagerServiceHost.Description.Behaviors.Add(pluginSchedulerServiceBehavior);
            schedulerManagerServiceHost.AddServiceEndpoint(typeof(IScheduler)
                , pluginSchedulerBinding
                , serviceUri
                )
                ;
            schedulerManagerServiceHost.Open();
        }

        private void initializeMonitoring()
        {

#if NET_40
            AppDomain.MonitoringIsEnabled = true;

            PluginManager<P>.Manage.MonitorTimer = new Timer(6000);
            PluginManager<P>.Manage.MonitorTimer.Elapsed += monitorTimer_Elapsed;

            using (AppService app = new AppService())
            {
                int monitors = app.GetMonitors()
                    .Where(m => m.PackageUniqueId == new Guid(AppDomain.CurrentDomain.FriendlyName))
                    .Count()
                    ;

                if (monitors > 0)
                {
                    PluginManager<P>.Manage.MonitorTimer.Start();
                }
            }
#endif

        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        internal void Initialize()
        {
            //  Setup any web service hosts here, use the AppDomain.FriendlyName to create unique URIs.
            this.initializePackageInspectorService();
            this.initializePluginInspectorService();
            this.initializeSchedulableService();
            this.initializeMonitoringService();
            this.initializeMonitoring();

            AppDomain.CurrentDomain.ResourceResolve += PluginManager<P>.PluginManager_ResourceResolve;

        }

        public void Load(byte[] bytes)
        {
            Assembly assembly = null;

            try
            {
                assembly = AppDomain.CurrentDomain.Load(bytes);
            }
            catch (Exception ex)
            {
                Logger.Logs.Log(string.Format("Failed to load assembly into AppDomain: {0}.", AppDomain.CurrentDomain.FriendlyName)
                    , ex
                    , LogType.Warn
                    )
                    ;
            }

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly == a)
                {
                    string location = a.FullName;

                    //  Get any exclusions defined in the app.config section
                    IList<string> exclusions = BaseSettings.GetValues("pluginAssemblyExclusions")
                        .ToPairs()
                        .Select(n => n.Value)
                        .ToList();

                    Type type;

                    try
                    {
                        if (!exclusions.Contains(location))
                        {
                            var plugin = typeof(IPlugin);

                            foreach (Type t in a.GetTypes())
                            {
                                type = t;

                                if (!type.IsInterface)
                                {
                                    if (type.GetInterface(typeof(P).Name) != null
                                        && type != typeof(PluginMetaData)
                                        && type.IsAbstract == false
                                        && plugin.IsAssignableFrom(t)
                                        )
                                    {
                                        P p = (P)Activator.CreateInstance(type);

                                        if (p.Schedulable)
                                        {
                                        }

                                        PluginManager<P>.Manage.Plugins.Add(p);

                                        Logger.Logs.Log(string.Format("The following plugin '{0} - {1}' was loaded into application domain '{2}'."
                                            , p.Name
                                            , p.UniqueId
                                            , AppDomain.CurrentDomain.FriendlyName)
                                            , LogType.Info
                                            )
                                            ;

                                        //  Hook to allow any plugin to execute code when the plugin is loaded.
                                        p.Loaded();
                                    }
                                }
                            }
                        }
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        StringBuilder sb = new StringBuilder();

                        foreach (Exception exSub in ex.LoaderExceptions)
                        {
                            sb.AppendLine(exSub.Message);

                            if (exSub is FileNotFoundException)
                            {
                                FileNotFoundException exFileNotFound = exSub as FileNotFoundException;

                                if (!string.IsNullOrEmpty(exFileNotFound.FusionLog))
                                {
                                    sb.AppendLine("Fusion Log:");
                                    sb.AppendLine(exFileNotFound.FusionLog);
                                }
                            }

                            sb.AppendLine();
                        }

                        string errorMessage = sb.ToString();

                        Logger.Logs.Log(string.Format("Failed to create instance of plugin from location {0}. ReflectionTypeLoadException: {1}"
                            , location
                            , errorMessage
                            )
                            , ex
                            , LogType.Warn
                            )
                            ;
                    }
                    catch (Exception ex)
                    {
                        Logger.Logs.Log(string.Format("Failed to create instance of plugin from location {0}.", location)
                            , ex
                            , LogType.Warn
                            )
                            ;
                    }
                }
            }
        }

        public void Unload()
        {
            /*
             * Shutdown the wcf endpoints.
             */

            if (packageInspectorServiceHost != null)
                packageInspectorServiceHost.Close();

            if (pluginInspectorServiceHost != null)
                pluginInspectorServiceHost.Close();

            if (monitoringManagerServiceHost != null)
                monitoringManagerServiceHost.Close();

            if (schedulerManagerServiceHost != null)
                schedulerManagerServiceHost.Close();

#if NET_40
            if (PluginManager<P>.Manage.MonitorTimer != null)
                PluginManager<P>.Manage.MonitorTimer.Elapsed -= monitorTimer_Elapsed;

            PluginManager<P>.Manage.ProcessCounter.Close();
            PluginManager<P>.Manage.CpuCounter.Close();
            PluginManager<P>.Manage.MemoryCounter.Close();
            PluginManager<P>.Manage.MonitorTimer.Close();
#endif
        }

        public IList<P> GetPlugins()
        {
            return PluginManager<P>.Manage.Plugins;
        }

        #endregion

        #region EVENTS

        private static Assembly PluginManager_ResourceResolve(object sender, ResolveEventArgs args)
        {
            IList<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .ToList();

            foreach (Assembly a in assemblies)
                if (a.GetName().FullName == args.Name)
                    return a;

            return null;
        }

#if NET_40

        private HubConnection hubConnection
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

        private IHubProxy proxy
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

        private void monitorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                PluginManager<P>.Manage.IsMonitoring = true;

                float p = PluginManager<P>.Manage.ProcessCounter.NextValue();
                float c = PluginManager<P>.Manage.CpuCounter.NextValue();
                float m = PluginManager<P>.Manage.MemoryCounter.NextValue();

                double u = Math.Round(p / c * 100, 2);

                // Check for Not-A-Number (Division by Zero)
                if (Double.IsNaN(u))
                    u = 0;

                if (Double.IsInfinity(u))
                    u = 0;

                if (PluginManager<P>.Manage.Monitors.Count >= 10)
                    PluginManager<P>.Manage.Monitors = PluginManager<P>.Manage.Monitors
                        .OrderByDescending(mon => mon.Date)
                        .Take(10)
                        .ToList()
                        ;

                IMonitor monitor = new Monitor
                {
                    PackageUniqueId = AppDomain.CurrentDomain.FriendlyName,
                    CpuUtilization = Math.Round(u / Environment.ProcessorCount, 2, MidpointRounding.AwayFromZero),
                    MemoryUtilization = Math.Round(m / 1024 / 1024 / 1024, 2, MidpointRounding.AwayFromZero),
                    MonitoringTotalProcessorTime = Math.Round(AppDomain.CurrentDomain.MonitoringTotalProcessorTime.TotalSeconds, 2, MidpointRounding.AwayFromZero),
                    AvgMonitoringTotalProcessorTime = Math.Round(PluginManager<P>.Manage.Monitors.Select(mon => mon.MonitoringTotalProcessorTime).ToList().StandardDeviation(), 2, MidpointRounding.AwayFromZero),
                    MonitoringSurvivedMemorySize = Math.Round(((double)AppDomain.CurrentDomain.MonitoringSurvivedMemorySize / 1024), 2, MidpointRounding.AwayFromZero),
                    Date = DateTime.Now,
                    Seconds = 0,
                }
                ;

                PluginManager<P>.Manage.Monitors.Add(monitor);

                for (int i = 0; i < PluginManager<P>.Manage.Monitors.Count; i++)
                    PluginManager<P>.Manage.Monitors[i].Seconds += 6;

                PluginManager<P>.Manage.OnMonitor();
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("MonitorTimer_Elapsed failed.", ex, LogType.Error);
            }
        }

        private void OnMonitor()
        {
            try
            {
                proxy.Invoke("Monitor"
                    , AppDomain.CurrentDomain.FriendlyName
                    , PluginManager<P>.Manage.Monitors.OrderBy(mon => mon.Date)
                    )
                    .Wait()
                    ;
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("OnMonitor failed.", ex, LogType.Error);
            }
        }
#endif

        #endregion
    }
}
