using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;

#if NET_40

using Microsoft.AspNet.SignalR;
using Vmgr.Messaging;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using Vmgr.Configuration;
using Microsoft.AspNet.SignalR.Hubs;
using Vmgr.Packaging;

#endif

namespace Vmgr.Scheduling
{
    public class GlobalTriggerListener : ITriggerListener
    {
        #region PRIVATE PROPERTIES

        private Timer _timer = null;
        private string _name = null;
        private JobMetaData _job = null;
        private ScheduleMetaData _schedule = null;
        private TriggerMetaData _trigger = null;
        private DateTime _startTime = DateTime.MinValue;
        private TimeSpan _currentElapsedTime = TimeSpan.Zero;
        private TimeSpan _totalElapsedTime = TimeSpan.Zero;
        private bool _isRunning = false;

#if NET_40
        private HubConnection _hubConnection = null;
        private IHubProxy _proxy = null;
#endif

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public string Name
        {
            get { return this._name; }
        }

        #endregion

        #region CTOR

        public GlobalTriggerListener(string name)
        {
            this._name = name;
        }

        #endregion

        #region PRIVATE METHODS

#if NET_40

        protected HubConnection hubConnection
        {
            get
            {
                if (this._hubConnection == null)
                {
                    _hubConnection = new HubConnection(string.Format("{0}://{1}:{2}/"
                        , PackageManager.Manage.Server.RTProtocol
                        , PackageManager.Manage.Server.RTFqdn
                        , PackageManager.Manage.Server.RTPort
                        )
                        )
                        ;

                    IHubProxy proxy = _hubConnection.CreateHubProxy("VmgrHub");
                }

                return this._hubConnection;
            }
        }

        protected IHubProxy proxy
        {
            get
            {
                if (this._proxy == null)
                {
                    _proxy = hubConnection.CreateHubProxy("VmgrHub");
                    hubConnection.Start().Wait();
                }

                if (hubConnection.State == ConnectionState.Disconnected)
                {
                    _proxy = hubConnection.CreateHubProxy("VmgrHub");
                    hubConnection.Start().Wait();
                }

                return this._proxy;
            }
        }

        private void OnNotifyClient()
        {
            try
            {
                proxy
                    .Invoke("Schedule"
                        , this._schedule.UniqueId
                        , new Schedule
                        {
                            ScheduleUniqueId = this._schedule.UniqueId,
                            PrimaryScheduleText = this._schedule.GetPrimaryScheduleText(),
                            SecondaryScheduleText = this._schedule.GetSecondaryScheduleText(),
                            AnticipatedScheduleText = this._schedule.GetAnticipatedScheduleText(),
                            IsRunning = this._isRunning,
                            ElapsedTime = string.Format("{0:00}:{1:00}:{2:00}"
                                , this._currentElapsedTime.TotalHours
                                , this._currentElapsedTime.TotalMinutes
                                , this._currentElapsedTime.TotalSeconds
                                ),
                        }
                        )
                    .Wait()
                    ;

            }
            catch (Exception ex)
            {
                Logger.Logs.Log("OnNotifyClient failed.", ex, LogType.Error);
            }
        }
#endif

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public void TriggerFired(ITrigger t, IJobExecutionContext context)
        {
            Logger.Logs.Log(string.Format("Trigger with key {0} fired for job key {1} and job group {2} at {3}."
                , t.Key.Name
                , context.JobDetail.Key.Name
                , context.JobDetail.Key.Group
                , DateTime.Now
                )
                , LogType.Info);

            try
            {
                this._job = context.JobDetail.JobDataMap[context.JobDetail.Key.Name] as JobMetaData;
                this._schedule = context.JobDetail.JobDataMap[this._job.ScheduleUniqueId.ToString()] as ScheduleMetaData;
                this._timer = new Timer(1000);
                this._timer.Elapsed += timer_Elapsed;
                this._startTime = DateTime.Now;
                this._totalElapsedTime = this._currentElapsedTime;
                this._timer.Start();
                this._isRunning = true;

                using (AppService app = new AppService())
                {
                    //  Create a new trigger to record.
                    this._trigger = new TriggerMetaData
                    {
                        JobId = this._job.JobId,
                        TriggerKey = t.Key.Name,
                        TriggerKeyGroup = t.Key.Group,
                        Started = DateTime.Now,
                        TriggerStatusTypeId = (int)TriggerStatusType.Started,
                    }
                    ;

                    //  Previousfire
                    if (t.GetPreviousFireTimeUtc().HasValue)
                        this._trigger.Previousfire = t.GetPreviousFireTimeUtc().Value.LocalDateTime;

                    //  Nextfire
                    if (t.GetNextFireTimeUtc().HasValue)
                        this._trigger.Nextfire = t.GetNextFireTimeUtc().Value.LocalDateTime;

                    //  Mayfire
                    this._trigger.Mayfire = t.GetMayFireAgain();

                    if (this._trigger.Mayfire ?? false)
                        this._job.JobStatusTypeId = (int)JobStatusType.Recurrence;

                    app.Save(this._job);
                    app.Save(this._trigger);

                    if (t.JobDataMap.Contains(t.Key.Name))
                        t.JobDataMap.Remove(t.Key.Name);

                    t.JobDataMap.Add(t.Key.Name, this._trigger);
                }

#if NET_40
                this.OnNotifyClient();
#endif
            }
            catch (Exception ex)
            {
                //  Attempt to log an exception, if the database is offline, logging should try email.
                SmtpLogger.Logs.Log(string.Format("Trigger with key {0} fired for job key {1} but failed miserably at {2}."
                    , t.Key.Name
                    , context.JobDetail.Key.Name
                    , DateTime.Now)
                    , ex
                    , LogType.Error);
            }
        }

        public void TriggerComplete(ITrigger t, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode)
        {
            Logger.Logs.Log(string.Format("Trigger with key {0} completed for job key {1} and job group {2} at {3}."
                , t.Key.Name
                , context.JobDetail.Key.Name
                , context.JobDetail.Key.Group
                , DateTime.Now
                )
                , LogType.Info);

            try
            {
                this._job = context.JobDetail.JobDataMap[context.JobDetail.Key.Name] as JobMetaData;
                this._schedule = context.JobDetail.JobDataMap[this._job.ScheduleUniqueId.ToString()] as ScheduleMetaData;

                if (this._timer != null)
                {
                    this._timer.Stop();
                    this._timer.Dispose();
                }

                this._isRunning = false;
                this._totalElapsedTime = TimeSpan.Zero;
                this._currentElapsedTime = TimeSpan.Zero;

                using (AppService appService = new AppService())
                {
                    if (this._job == null)
                        throw Logger.Logs.Log("Unable to find job.", LogType.Error);

                    TriggerMetaData trigger = t.JobDataMap[t.Key.Name] as TriggerMetaData;

                    if (trigger == null)
                        throw Logger.Logs.Log("Unable to find trigger.", LogType.Error);

                    trigger.Ended = DateTime.Now;

                    //  Previousfire
                    if (t.GetPreviousFireTimeUtc().HasValue)
                        trigger.Previousfire = t.GetPreviousFireTimeUtc().Value.LocalDateTime;

                    //  Nextfire
                    if (t.GetNextFireTimeUtc().HasValue)
                        trigger.Nextfire = t.GetNextFireTimeUtc().Value.LocalDateTime;

                    //  Mayfire
                    trigger.Mayfire = t.GetMayFireAgain();
                    trigger.TriggerStatusTypeId = (int)TriggerStatusType.Completed;

                    if (trigger.Mayfire ?? false)
                        this._job.JobStatusTypeId = (int)JobStatusType.Recurrence;

                    appService.Save(trigger);
                    appService.Save(this._job);
                }

#if NET_40
                this.OnNotifyClient();
#endif
            }
            catch (Exception ex)
            {
                //  Attempt to log an exception, if the database is offline, logging should try email.
                SmtpLogger.Logs.Log(string.Format("Trigger with key {0} completed for job key {1} but failed miserably at {2}."
                    , t.Key.Name
                    , context.JobDetail.Key.Name
                    , DateTime.Now)
                    , ex
                    , LogType.Error);
            }
        }

        public void TriggerMisfired(ITrigger t)
        {
            Logger.Logs.Log(string.Format("Trigger with key {0} misfired at {1}."
                , t.Key.Name
                , DateTime.Now
                )
                , LogType.Warn);

            try
            {
                this._trigger = t.JobDataMap[t.Key.Name] as TriggerMetaData;
                this._job = t.JobDataMap[t.JobKey.Name] as JobMetaData;
                this._schedule = t.JobDataMap[this._job.ScheduleUniqueId.ToString()] as ScheduleMetaData;

                //  Record a misfire.  Misfires occur because of a lack of resources, or provisioned threads.
                using (AppService appService = new AppService())
                {
                    if (this._trigger == null)
                    {
                        //  In case no TriggerMetaData was ever inserted into the JobDataMap.
                        this._trigger = new TriggerMetaData
                        {
                            JobId = this._job.JobId,
                            TriggerKey = t.Key.Name,
                            TriggerKeyGroup = t.Key.Group,
                            TriggerStatusTypeId = (int)TriggerStatusType.Misfired,
                        }
                        ;
                    }

                    if (this._trigger.Started == null)
                        this._trigger.Started = DateTime.Now;

                    this._trigger.Ended = DateTime.Now;
                    this._trigger.Misfire = true;
                    this._trigger.TriggerStatusTypeId = (int)TriggerStatusType.Misfired;

                    //  Previousfire
                    if (t.GetPreviousFireTimeUtc().HasValue)
                        this._trigger.Previousfire = t.GetPreviousFireTimeUtc().Value.LocalDateTime;

                    //  Nextfire
                    if (t.GetNextFireTimeUtc().HasValue)
                        if (t.GetFireTimeAfter(DateTimeOffset.Now).HasValue)
                            this._trigger.Nextfire = t.GetFireTimeAfter(DateTimeOffset.Now).Value.LocalDateTime;

                    //  Mayfire
                    this._trigger.Mayfire = t.GetMayFireAgain();

                    if (this._trigger.Mayfire ?? false)
                        this._job.JobStatusTypeId = (int)JobStatusType.Recurrence;

                    appService.Save(this._job);
                    appService.Save(this._trigger);
                }

                if (this._timer != null)
                {
                    this._timer.Stop();
                    this._timer.Dispose();
                }

                this._isRunning = false;
                this._totalElapsedTime = TimeSpan.Zero;
                this._currentElapsedTime = TimeSpan.Zero;

#if NET_40
                this.OnNotifyClient();
#endif

            }
            catch (Exception ex)
            {
                //  Attempt to log an exception, if the database is offline, logging should try email.
                SmtpLogger.Logs.Log(string.Format("Trigger with key {0} misfired at {1}, but failed miserably."
                    , t.Key.Name
                    , DateTime.Now)
                    , ex
                    , LogType.Error);
            }
        }

        public bool VetoJobExecution(ITrigger trigger, IJobExecutionContext context)
        {
            this._schedule = context.JobDetail.JobDataMap[this._job.ScheduleUniqueId.ToString()] as ScheduleMetaData;

#if NET_40
            if (!string.IsNullOrEmpty(this._schedule.Exclusions))
            {
                IList<DateTime> dates = JsonConvert.DeserializeObject<List<DateTime>>(this._schedule.Exclusions);

                if (dates.Select(d => d.Date).Contains(DateTime.Now.Date))
                {
                    try
                    {
                        this._trigger = trigger.JobDataMap[trigger.Key.Name] as TriggerMetaData;

                        //  Record a misfire.  Misfires occur because of a lack of resources, or provisioned threads.
                        using (AppService appService = new AppService())
                        {
                            if (this._trigger == null)
                            {
                                //  In case no TriggerMetaData was ever inserted into the JobDataMap.
                                this._trigger = new TriggerMetaData
                                {
                                    JobId = this._job.JobId,
                                    TriggerKey = trigger.Key.Name,
                                    TriggerKeyGroup = trigger.Key.Group,
                                    TriggerStatusTypeId = (int)TriggerStatusType.Vetoed,
                                }
                                ;
                            }

                            if (this._trigger.Started == null)
                                this._trigger.Started = DateTime.Now;

                            this._trigger.Ended = DateTime.Now;
                            this._trigger.Misfire = false;
                            this._trigger.TriggerStatusTypeId = (int)TriggerStatusType.Vetoed;

                            //  Previousfire
                            if (trigger.GetPreviousFireTimeUtc().HasValue)
                                this._trigger.Previousfire = trigger.GetPreviousFireTimeUtc().Value.LocalDateTime;

                            //  Nextfire
                            if (trigger.GetNextFireTimeUtc().HasValue)
                                if (trigger.GetFireTimeAfter(DateTimeOffset.Now).HasValue)
                                    this._trigger.Nextfire = trigger.GetFireTimeAfter(DateTimeOffset.Now).Value.LocalDateTime;

                            //  Mayfire
                            this._trigger.Mayfire = trigger.GetMayFireAgain();

                            appService.Save(this._trigger);
                        }

                        if (this._timer != null)
                        {
                            this._timer.Stop();
                            this._timer.Dispose();
                        }

                        this._isRunning = false;
                        this._totalElapsedTime = TimeSpan.Zero;
                        this._currentElapsedTime = TimeSpan.Zero;

#if NET_40
                        this.OnNotifyClient();
#endif
                    }
                    catch (Exception ex)
                    {
                        //  Attempt to log an exception, if the database is offline, logging should try email.
                        SmtpLogger.Logs.Log(string.Format("Trigger with key {0} failed to be vetoed at {1}, but failed miserably."
                            , trigger.Key.Name
                            , DateTime.Now)
                            , ex
                            , LogType.Error);
                    }

                    return true;
                }
            }
#endif
            return false;
        }

        #endregion

        #region EVENTS

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var s = DateTime.Now - this._startTime;
            s = new TimeSpan(s.Hours, s.Minutes, s.Seconds);
            this._currentElapsedTime = s + this._totalElapsedTime;

#if NET_40
            this.OnNotifyClient();
#endif
        }

        #endregion
    }
}
