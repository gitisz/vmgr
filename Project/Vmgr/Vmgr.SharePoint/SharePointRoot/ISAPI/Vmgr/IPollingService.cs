using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;

namespace Vmgr.SharePoint
{
    [XmlSerializerFormat]
    [ServiceContract(Namespace = "http://Vmgr.SharePoint/PollingService")]
    public interface IPollingService
    {
        [OperationContract]
        bool IsStarted(string serverId);
    }
}