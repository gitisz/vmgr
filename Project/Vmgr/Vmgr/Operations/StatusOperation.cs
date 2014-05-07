using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Runtime.InteropServices;
using mscoree;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Packaging;
using Vmgr.Plugins;
using Vmgr.Scheduling;
using Vmgr.Configuration;
using System.ServiceModel.Activation;
using Vmgr.Messaging;

namespace Vmgr.Operations
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class StatusOperation : IStatusOperation
    {
        #region PRIVATE PROPERTIES

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
        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        public StatusOperation()
        {
        }

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public bool GetStatus()
        {
            return true;
        }

        public IList<AppDomainMetaData> GetAppDomains()
        {
            List<AppDomainMetaData> appDomains = new List<AppDomainMetaData>();
            
            IntPtr enumHandle = IntPtr.Zero;
            
            ICorRuntimeHost host = new mscoree.CorRuntimeHost();
           
            try
            {
                host.EnumDomains(out enumHandle);
               
                object domain = null;
                
                while (true)
                {
                    host.NextDomain(enumHandle, out domain);
                   
                    if (domain == null) 
                        break;

                    AppDomain appDomain = (AppDomain)domain;
                    appDomains.Add(new AppDomainMetaData { Name = appDomain.FriendlyName });
                }
                
                return appDomains;
            }
            catch
            {
                return null;
            }
            finally
            {
                host.CloseEnum(enumHandle);
                Marshal.ReleaseComObject(host);
            }
        }

        public IList<IPlugin> GetPluginsByDomain(string domainName)
        {
            IList<IPlugin> plugins = new List<IPlugin>();

            if (AppDomain.CurrentDomain.FriendlyName == domainName)
                plugins = PluginManager<IPlugin>.Manage.Plugins;
            else
            {
                //   TODO: call appdomain to get its list of plugins.

                string url = string.Format("{0}://{1}:{2}/Vmgr.Plugins/{3}/PluginInspector"
                    , PackageManager.Manage.Server.WSProtocol
                    , PackageManager.Manage.Server.WSFqdn
                    , PackageManager.Manage.Server.WSPort
                    , domainName
                    )
                    ;

                try
                {
                    BasicHttpBinding binding = new BasicHttpBinding();

                    if (ProtocolHttps)
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.Transport;
                        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
                    }

                    ChannelFactory<IPluginInspector> httpFactory = new ChannelFactory<IPluginInspector>(binding
                        , new EndpointAddress(url)
                        )
                        ;

                    var pluginManagerProxy = httpFactory.CreateChannel();
                   
                    plugins = pluginManagerProxy.GetPlugins()
                        .ToList()
                        ;
                }
                catch (EndpointNotFoundException ex)
                {
                    Logger.Logs.Log("Unable to inspect package from source server.  The service does not appear to be online.", ex, LogType.Warn);
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Unable to inspect package from source server.", ex, LogType.Warn);
                }
            }

            return plugins;
        }

        #endregion

        #region EVENTS

        #endregion

    }

    public class AppDomainMetaData
    {
        public string Name { get; set; }
    }
}
