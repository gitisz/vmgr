using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Scheduling
{
    public enum RecurrenceType
    {
        Minutely,
        Hourly,
        Daily,
        Weekly,
        Monthly,
        Yearly,
        Secondly,
    }

    public enum JobStatusType
    {
        Scheduled = 1,
        Recurrence = 2,
        InProgress = 3,
        Succeeded = 4,
        Failed = 5,
        Deleted = 6,
    }

    public enum TriggerStatusType
    {
        Unknown = 1,
        Started = 2,
        Completed = 3,
        Misfired = 4,
        Vetoed = 5,
    }

}
