using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Operations;

namespace Vmgr.SharePoint
{
    [XmlSerializerFormat]
    [ServiceContract(Namespace = "http://Vmgr.SharePoint/AppDomainsService")]
    public interface IAppDomainsService
    {
        [OperationContract]
        IList<AppDomainMetaData> GetAppDomains(string serverId);

        [OperationContract]
        IList<PluginMetaData> GetPluginsByAppDomain(string serverId, string domainName);
    }
}