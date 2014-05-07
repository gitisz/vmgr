using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Packaging;

namespace Vmgr.Operations
{
    [ServiceKnownType(typeof(PackageOperation))]
    [ServiceContract(Namespace = "http://Vmgr.Operations")]
    public interface IPackageOperation
    {
        [OperationContract]
        void Load();

        [OperationContract]
        [ServiceKnownType(typeof(PackageMetaData))]
        void LoadPackage(IPackage package);

        [OperationContract]
        void Unload();

        [OperationContract]
        [ServiceKnownType(typeof(PackageMetaData))]
        void UnloadPackage(IPackage package);
    }
}
