using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vmgr.Plugins;
using Vmgr.Packaging;
using Vmgr.Data.Biz.MetaData;
using Quartz.Impl.Matchers;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz;

namespace Vmgr.Scheduling
{
    internal class SchedulerManager : BaseScheduler, IScheduler
    {
        #region PRIVATE PROPERTIES

        private IList<ISchedule> _schedules = null;
        private static SchedulerManager _manage;

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public static SchedulerManager Manage
        {
            get
            {
                if (_manage == null)
                {
                    _manage = new SchedulerManager();
                }
                return _manage;
            }
        }

        public IList<ISchedule> Schedules
        {
            get
            {
                if (this._schedules == null)
                {
                    this._schedules = new List<ISchedule> { };
                }

                return this._schedules;
            }
        }

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void scheduleTheJob(ISchedule schedule, IJobDetail jobDetail, ITrigger trigger, JobMetaData job)
        {
            /*
             * JobDetail
             */

            //  Remove the existing JobMetaData from the Job's JobDataMap. 
            if (jobDetail.JobDataMap.Contains(jobDetail.Key.Name))
                jobDetail.JobDataMap.Remove(jobDetail.Key.Name);

            //  Add the new JobMetaData to the Job's JobDataMap. 
            jobDetail.JobDataMap.Add(jobDetail.Key.Name, job);

            //  Remove the existing ScheduleMetaData from the Job's JobDataMap. 
            if (jobDetail.JobDataMap.Contains(schedule.UniqueId.ToString()))
                jobDetail.JobDataMap.Remove(schedule.UniqueId.ToString());

            //  Add the new ScheduleMetaData to the Job's JobDataMap. 
            jobDetail.JobDataMap.Add(schedule.UniqueId.ToString(), schedule);

            /*
             * Trigger
             */

            //  Remove the existing JobMetaData from the Trigger's JobDataMap. 
            if (trigger.JobDataMap.Contains(jobDetail.Key.Name))
                trigger.JobDataMap.Remove(jobDetail.Key.Name);

            //  Add the new JobMetaData to the Trigger's JobDataMap. 
            trigger.JobDataMap.Add(jobDetail.Key.Name, job);

            //  Remove the existing ScheduleMetaData from the Trigger's JobDataMap. 
            if (trigger.JobDataMap.Contains(schedule.UniqueId.ToString()))
                trigger.JobDataMap.Remove(schedule.UniqueId.ToString());

            //  Add the new ScheduleMetaData to the Trigger's JobDataMap. 
            trigger.JobDataMap.Add(schedule.UniqueId.ToString(), schedule);

            //  Wire up a job listener and a trigger listener.
            IMatcher<JobKey> jobMatcher = KeyMatcher<JobKey>.KeyEquals(jobDetail.Key);
            IMatcher<TriggerKey> triggerMatcher = KeyMatcher<TriggerKey>.KeyEquals(trigger.Key);

            BaseScheduler.Scheduler.ListenerManager.AddJobListener(new GlobalJobListener(jobDetail.Key.Name), jobMatcher);
            BaseScheduler.Scheduler.ListenerManager.AddTriggerListener(new GlobalTriggerListener(trigger.Key.Name), triggerMatcher);

            //  Finally, schedule the job using the trigger created for it.
            BaseScheduler.Scheduler.ScheduleJob(jobDetail, trigger);
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public void Schedule(ISchedule schedule)
        {
            try
            {
                IPluginJob pluginJob = PluginManager<IPlugin>.Manage.Plugins
                    .Where(p => p.UniqueId == schedule.PluginUniqueId)
                    .Select(p => p as IPluginJob)
                    .FirstOrDefault()
                    ;

                //  The job is the plugin's responsibility.
                IJobDetail jobDetail = BaseScheduler.GetJobDetail(schedule, pluginJob);

                //  The trigger is the service's responsibility.
                ITrigger trigger = BaseScheduler.GetTrigger(jobDetail, schedule);

                if (trigger == null)
                {
                    Logger.Logs.Log(string.Format("The schedule '{0}' has elapsed and there is no recurrence pattern defined.", schedule.UniqueId), LogType.Info);
                }
                else
                {
                    if (!BaseScheduler.Scheduler.CheckExists(trigger.Key))
                    {
                        /*
                         * Attempt to unschedule the job if it is already scheduled.
                         */
                        this.UnSchedule(schedule);

                        using (AppService app = new AppService())
                        {
                            JobMetaData job = new JobMetaData
                            {
                                ScheduleId = (schedule as ScheduleMetaData).ScheduleId,
                                JobKey = jobDetail.Key.Name,
                                JobKeyGroup = jobDetail.Key.Group,
                                JobStatusTypeId = (int)JobStatusType.Scheduled,
                                ScheduleUniqueId = schedule.UniqueId,
                            }
                            ;

                            if (app.Save(job))
                            {
                                this.scheduleTheJob(schedule, jobDetail, trigger, job);

                                Logger.Logs.Log(string.Format("Scheduled job {0} at {1}. Next fire time is {2} followed by another at {3}."
                                    , jobDetail.Key.Name
                                    , DateTime.Now.ToString()
                                    , BaseScheduler.GetNextFireTime(trigger)
                                    , BaseScheduler.GetNextFireTimeAfterMessage(trigger)
                                    )
                                    , LogType.Info);
                            }
                        }

                    }
                    else
                    {
                        this.UnSchedule(schedule);

                        this.Schedule(schedule);
                    }
                }

                if (!SchedulerManager.Scheduler.IsStarted)
                    SchedulerManager.Scheduler.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to create schedule.  Error: {0}", ex.ToString());
            }
        }

        public void UnSchedule(ISchedule schedule)
        {
            IList<string> jobGroups = BaseScheduler.Scheduler.GetJobGroupNames();
            IList<string> triggerGroups = BaseScheduler.Scheduler.GetTriggerGroupNames();

            foreach (string group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = BaseScheduler.Scheduler.GetJobKeys(groupMatcher);

                foreach (var jobKey in jobKeys)
                {
                    Guid jk = new Guid(jobKey.Group);

                    if (schedule.UniqueId == jk)
                        BaseScheduler.Scheduler.DeleteJob(jobKey);
                }
            }
        }

        #endregion

        #region EVENTS

        #endregion

    }
}
