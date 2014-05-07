using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vmgr.Scheduling;

namespace Vmgr.Messaging
{
    public class JobHistory
    {
        public Guid JobHistoryUniqueId { get; set; }
        public JobStatusType JobStatusType { get; set; }
        public string ElapsedTime { get; set; }
    }
}
