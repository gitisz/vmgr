using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace Vmgr.SharePoint
{
    [XmlSerializerFormat]
    [ServiceContract(Namespace = "http://Vmgr.SharePoint/PermissionsService")]
    public interface IPermissionsService
    {
        [OperationContract]
        void ClearPermissionCache();
    }
}