using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Vmgr.Data.Biz.MetaData;

namespace Vmgr.Packaging
{
    [ServiceKnownType(typeof(PackageInspector))]
    [ServiceContract(Namespace = "http://Vmgr.Packaging")]
    public interface IPackageInspector
    {
        [OperationContract]
        IList<AssemblyMetaData> GetAssemblies();
    }
}
