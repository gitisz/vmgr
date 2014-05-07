using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Packaging;

namespace Vmgr.Operations
{
    [ServiceContract(Namespace = "http://Vmgr.Operations")]
    public interface IMoveOperation
    {
        [OperationContract]
        [ServiceKnownType(typeof(PackageMetaData))]
        void Move(IPackage package, Guid destination, string group);
        
        [OperationContract]
        [ServiceKnownType(typeof(PackageMetaData))]
        void Load(IPackage package, string group);
    }
}
