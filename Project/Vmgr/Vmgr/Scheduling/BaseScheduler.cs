using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vmgr.Plugins;

namespace Vmgr.Scheduling
{
    internal abstract class BaseScheduler
    {
        #region PRIVATE PROPERTIES

        private static Quartz.ISchedulerFactory _schedulerFactory = null;
        private static Quartz.IScheduler _scheduler = null;

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public static Quartz.ISchedulerFactory SchedulerFactory
        {
            get
            {
                if (_schedulerFactory == null)
                {
                    _schedulerFactory = new StdSchedulerFactory();
                }

                return _schedulerFactory;
            }
        }

        public static Quartz.IScheduler Scheduler
        {
            get
            {
                if (_scheduler == null)
                {
                    _scheduler = SchedulerFactory.GetScheduler();
                }

                return _scheduler;
            }
        }

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public static DateTimeOffset? GetNextFireTime(ITrigger trigger)
        {
            return trigger.GetNextFireTimeUtc();
        }

        public static DateTimeOffset? GetNextFireTimeAfterMessage(ITrigger trigger)
        {
            return trigger.GetFireTimeAfter(trigger.GetNextFireTimeUtc());
        }

        public static IJobDetail GetJobDetail(ISchedule schedule, IPluginJob pluginJob)
        {
            IJobDetail j = null;

            RecurrenceType recurrenceType = (RecurrenceType)schedule.RecurrenceTypeId;

            switch (recurrenceType)
            {
                case RecurrenceType.Secondly:

                    j = pluginJob.GetJobToRun()
                        .WithIdentity(Guid.NewGuid().ToString(), schedule.UniqueId.ToString())
                        .Build();

                    break;

                case RecurrenceType.Minutely:

                    j = pluginJob.GetJobToRun()
                        .WithIdentity(Guid.NewGuid().ToString(), schedule.UniqueId.ToString())
                        .Build();

                    break;

                case RecurrenceType.Hourly:

                    j = pluginJob.GetJobToRun()
                        .WithIdentity(Guid.NewGuid().ToString(), schedule.UniqueId.ToString())
                        .Build();

                    break;

                case RecurrenceType.Daily:

                    j = pluginJob.GetJobToRun()
                        .WithIdentity(Guid.NewGuid().ToString(), schedule.UniqueId.ToString())
                        .Build();

                    break;

                case RecurrenceType.Weekly:

                    j = pluginJob.GetJobToRun()
                        .WithIdentity(Guid.NewGuid().ToString(), schedule.UniqueId.ToString())
                        .Build();

                    break;

                case RecurrenceType.Monthly:

                    j = pluginJob.GetJobToRun()
                        .WithIdentity(Guid.NewGuid().ToString(), schedule.UniqueId.ToString())
                        .Build();

                    break;

                case RecurrenceType.Yearly:

                    j = pluginJob.GetJobToRun()
                        .WithIdentity(Guid.NewGuid().ToString(), schedule.UniqueId.ToString())
                        .Build();

                    break;
            }

            j.JobDataMap.Add("ScheduleMetaData", schedule);

            return j;
        }

        public static ITrigger GetTrigger(IJobDetail job, ISchedule schedule)
        {
            ITrigger t = null;
            TriggerBuilder triggerBuilder = null;
            IScheduleBuilder scheduleBuilder = null;

            RecurrenceType recurrenceType = (RecurrenceType)schedule.RecurrenceTypeId;

            DateTimeOffset startDate = schedule.Start;

            switch ((RecurrenceType)schedule.RecurrenceTypeId)
            {

                case RecurrenceType.Secondly:

                    CronSecond cronSecond = new CronSecond();

                    if (!Cron.TryParse(schedule.RecurrenceRule, out cronSecond))
                        throw new ApplicationException("Unable to parse expresion to Cron.");

                    if (cronSecond.IsSimple)
                    {
                        scheduleBuilder = SimpleScheduleBuilder.Create()
                            .WithIntervalInSeconds(cronSecond.Second)
                            .RepeatForever()
                            ;
                    }
                    else
                    {
                        scheduleBuilder = CronScheduleBuilder.CronSchedule(schedule.RecurrenceRule);
                    }

                    break;

               case RecurrenceType.Minutely:

                    CronMinute cronMinute = new CronMinute();

                    if (!Cron.TryParse(schedule.RecurrenceRule, out cronMinute))
                        throw new ApplicationException("Unable to parse expresion to Cron.");

                    if (cronMinute.IsSimple)
                    {
                        scheduleBuilder = SimpleScheduleBuilder.Create()
                            .WithIntervalInMinutes(cronMinute.Minute)
                            .RepeatForever()
                            ;
                    }
                    else
                    {
                        scheduleBuilder = CronScheduleBuilder.CronSchedule(schedule.RecurrenceRule);
                    }

                    break;

                case RecurrenceType.Hourly:

                    CronHour cronHour = new CronHour();

                    if (!Cron.TryParse(schedule.RecurrenceRule, out cronHour))
                        throw new ApplicationException("Unable to parse expresion to Cron.");

                    if (cronHour.IsSimple)
                    {
                        scheduleBuilder = SimpleScheduleBuilder.Create()
                            .WithIntervalInHours(cronHour.Hour)
                            .RepeatForever()
                            ;
                    }
                    else
                    {
                        scheduleBuilder = CronScheduleBuilder.CronSchedule(schedule.RecurrenceRule);
                    }

                    break;

                case RecurrenceType.Daily:

                    CronDay cronDay = new CronDay();

                    if (!Cron.TryParse(schedule.RecurrenceRule, out cronDay))
                        throw new ApplicationException("Unable to parse daily expresion to Cron.");

                    scheduleBuilder = CronScheduleBuilder.CronSchedule(schedule.RecurrenceRule);

                    break;

                case RecurrenceType.Weekly:

                    if (!Cron.TryParse(schedule.RecurrenceRule, out cronDay))
                        throw new ApplicationException("Unable to parse weekly expresion to Cron.");

                    scheduleBuilder = CronScheduleBuilder.CronSchedule(schedule.RecurrenceRule);

                    break;

                case RecurrenceType.Monthly:

                    if (!Cron.TryParse(schedule.RecurrenceRule, out cronDay))
                        throw new ApplicationException("Unable to parse monthly expresion to Cron.");

                    scheduleBuilder = CronScheduleBuilder.CronSchedule(schedule.RecurrenceRule);

                    break;

                case RecurrenceType.Yearly:

                    if (!Cron.TryParse(schedule.RecurrenceRule, out cronDay))
                        throw new ApplicationException("Unable to parse yearly expresion to Cron.");

                    scheduleBuilder = CronScheduleBuilder.CronSchedule(schedule.RecurrenceRule);

                    break;

                default:
                    break;
            }

            if (scheduleBuilder is CronScheduleBuilder)
            {
                scheduleBuilder = (scheduleBuilder as CronScheduleBuilder).WithMisfireHandlingInstructionFireAndProceed();

                if (schedule.MisfireInstruction == MisfireInstruction.CronTrigger.DoNothing)
                    scheduleBuilder = (scheduleBuilder as CronScheduleBuilder).WithMisfireHandlingInstructionDoNothing();
            }

            if (scheduleBuilder is SimpleScheduleBuilder)
            {
                scheduleBuilder = (scheduleBuilder as SimpleScheduleBuilder).WithMisfireHandlingInstructionFireNow();

                if (schedule.MisfireInstruction == MisfireInstruction.CronTrigger.DoNothing)
                    scheduleBuilder = (scheduleBuilder as SimpleScheduleBuilder).WithMisfireHandlingInstructionNextWithExistingCount();
            }

            triggerBuilder = TriggerBuilder.Create()
               .WithIdentity(Guid.NewGuid().ToString(), job.Key.Name)
               .WithSchedule(scheduleBuilder)
               .StartAt(startDate)
               ;

            if (schedule.End.HasValue)
                triggerBuilder = triggerBuilder
                    .EndAt(schedule.End)
                    ;

            t = triggerBuilder
                .Build();

            return t;
        }

        #endregion

        #region EVENTS

        #endregion
    }
}
