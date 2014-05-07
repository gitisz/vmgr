using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Security.Principal;
using System.ServiceModel;
using Vmgr.Operations;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using System.Net;

namespace Vmgr.SharePoint
{
    /// <summary>
    /// Summary description for AppDomains
    /// </summary>
    [WebService(Namespace = "http://Vmgr.SharePoint/AppDomainsService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class AppDomainsService : WebService, IAppDomainsService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public IList<AppDomainMetaData> GetAppDomains(string serverId)
        {
            IList<AppDomainMetaData> result = new List<AppDomainMetaData> { };

            ServerMetaData server = null;

            using (AppService app = new AppService())
            {
                server = app.GetServers()
                    .Where(s => s.UniqueId == new Guid(serverId))
                    .FirstOrDefault()
                    ;
            }

            if (server == null)
                throw new NullReferenceException("Server must not be undefined.");

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) =>
                {
                    return true;
                };

                BasicHttpBinding binding = new BasicHttpBinding();

                if (server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    binding.Security.Mode = BasicHttpSecurityMode.None;
                }

                if (server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                }

                ChannelFactory<IStatusOperation> httpFactory = new ChannelFactory<IStatusOperation>(binding
                    , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/StatusOperation"
                        , server.WSProtocol
                        , server.WSFqdn
                        , server.WSPort
                    )
                    )
                    )
                    ;
                IStatusOperation statusOperationProxy = httpFactory.CreateChannel();

                result = statusOperationProxy.GetAppDomains();
            }
            catch (EndpointNotFoundException ex)
            {
                Logger.Logs.Log("Unable to contact server.  The service does not appear to be online.", ex, LogType.Error);
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Unable to contact server.", ex, LogType.Error);
            }

            return result;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public IList<PluginMetaData> GetPluginsByAppDomain(string serverId, string domainName)
        {
            IList<PluginMetaData> result = new List<PluginMetaData> { };

            ServerMetaData server = null;

            using (AppService app = new AppService())
            {
                server = app.GetServers()
                    .Where(s => s.UniqueId == new Guid(serverId))
                    .FirstOrDefault()
                    ;
            }

            if (server == null)
                throw new NullReferenceException("Server must not be undefined.");

            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (se, cert, chain, sslerror) =>
                {
                    return true;
                };

                BasicHttpBinding binding = new BasicHttpBinding();

                if (server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    binding.Security.Mode = BasicHttpSecurityMode.None;
                }

                if (server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                }

                ChannelFactory<IStatusOperation> httpFactory = new ChannelFactory<IStatusOperation>(binding
                    , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/StatusOperation"
                        , server.WSProtocol
                        , server.WSFqdn
                        , server.WSPort
                    )
                    )
                    )
                    ;
                IStatusOperation statusOperationProxy = httpFactory.CreateChannel();

                result = statusOperationProxy.GetPluginsByDomain(domainName)
                    .Select(p => p as PluginMetaData)
                    .ToList();
            }
            catch (EndpointNotFoundException ex)
            {
                Logger.Logs.Log("Unable to contact server.  The service does not appear to be online.", ex, LogType.Error);
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Unable to contact server.", ex, LogType.Error);
            }

            return result;
        }
    }
}
