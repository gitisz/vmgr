using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Packaging;

namespace Vmgr.Operations
{
    [ServiceKnownType(typeof(ScheduleOperation))]
    [ServiceContract(Namespace = "http://Vmgr.Operations")]
    public interface IScheduleOperation
    {
        [OperationContract]
        void Schedule();

        [OperationContract]
        [ServiceKnownType(typeof(PackageMetaData))]
        void SchedulePackage(IPackage package);

        [OperationContract]
        void Unschedule();

        [OperationContract]
        [ServiceKnownType(typeof(PackageMetaData))]
        void UnschedulePackage(IPackage package);

    }
}
