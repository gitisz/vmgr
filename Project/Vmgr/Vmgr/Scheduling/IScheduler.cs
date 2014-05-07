using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Plugins;

namespace Vmgr.Scheduling
{
    [ServiceContract(Namespace = "http://Vmgr.Scheduling")]
    public interface IScheduler
    {
        [OperationContract]
        [ServiceKnownType(typeof(ScheduleMetaData))]
        void Schedule(ISchedule schedule);

        [OperationContract]
        [ServiceKnownType(typeof(ScheduleMetaData))]
        void UnSchedule(ISchedule schedule);
    }
}
