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
    /// Summary description for Polling
    /// </summary>
    [WebService(Namespace = "http://Vmgr.SharePoint/PollingService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class PollingService : WebService, IPollingService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public bool IsStarted(string serverId)
        {
            bool result = false;

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

                result = statusOperationProxy.GetStatus();
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
