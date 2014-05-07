using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Vmgr.Data.Biz.MetaData;

namespace Vmgr.Scheduling
{
    [ServiceKnownType(typeof(ScheduleMetaData))]
    public interface ISchedule
    {
        /// <summary>
        /// The schedule name. 
        /// </summary>
        String Name { get; set; }

        /// <summary>
        /// A description of the schedule.
        /// </summary>
        String Description { get; set; }

        /// <summary>
        /// A unique ID for the scheule.
        /// </summary>
        Guid UniqueId { get; set; }

        /// <summary>
        /// The datetime the schedule starts.
        /// </summary>
        DateTime Start { get; set; }

        /// <summary>
        /// The datetime the schedule ends.
        /// </summary>
        DateTime? End { get; set; }

        /// <summary>
        /// The recurrence pattern type.
        /// </summary>
        int RecurrenceTypeId { get; set; }

        /// <summary>
        /// The recurrence chron expression to use.
        /// </summary>
        String RecurrenceRule { get; set; }

        /// <summary>
        /// Determines the bhavior of the trigger when a misfire occurrs. Options: 1 - FireOnceNow, 2 - DoNothing.
        /// </summary>
        int MisfireInstruction { get; set; }

        /// <summary>
        /// The unique ID of the plugin assigned to this schedule.
        /// </summary>
        Guid PluginUniqueId { get; set; }
    }
}
