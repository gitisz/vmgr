using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Messaging
{
    public class Schedule
    {
        public Guid ScheduleUniqueId { get; set; }
        public bool IsRunning { get; set; }
        public string PrimaryScheduleText { get; set; }
        public string SecondaryScheduleText { get; set; }
        public string AnticipatedScheduleText { get; set; }
        public string ElapsedTime { get; set; }
    }
}
