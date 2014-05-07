using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Vmgr.Configuration;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;

#if NET_40

using Microsoft.AspNet.SignalR;
using Vmgr.Messaging;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNet.SignalR.Client;
using Vmgr.Packaging;
using Vmgr.Plugins;
using Microsoft.AspNet.SignalR.Client.Hubs;

#endif

namespace Vmgr.Scheduling
{
    public class GlobalJobListener : IJobListener
    {
        #region PRIVATE PROPERTIES

        private string _name = null;
        private Timer _timer = null;
        private JobMetaData _job = null;
        private ScheduleMetaData _schedule = null;
        private DateTime _startTime = DateTime.MinValue;
        private TimeSpan _currentElapsedTime = TimeSpan.Zero;
        private TimeSpan _totalElapsedTime = TimeSpan.Zero;

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

        public GlobalJobListener(string name)
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
                    .Invoke("JobHistory"
                        , this._job.JobKey
                        , new JobHistory
                        {
                            JobHistoryUniqueId = new Guid(this._job.JobKey),
                            JobStatusType = (JobStatusType)this._job.JobStatusTypeId,
                            ElapsedTime = string.Format("{0:00}:{1:00}:{2:00}"
                                , this._currentElapsedTime.Hours
                                , this._currentElapsedTime.Minutes
                                , this._currentElapsedTime.Seconds
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

        private void OnGlobalMessage(string message)
        {
            try
            {
                proxy
                    .Invoke("GlobalMessage", message)
                    .Wait()
                    ;
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("OnGlobalMessage failed.", ex, LogType.Error);
            }
        }
#endif

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public void JobExecutionVetoed(IJobExecutionContext context)
        {
            Logger.Logs.Log(string.Format("Job with key {0} and job group {1} is was vetoed."
                , context.JobDetail.Key.Name
                , context.JobDetail.Key.Group
                , DateTime.Now
                )
                , LogType.Info);

            this._job = context.JobDetail.JobDataMap[context.JobDetail.Key.Name] as JobMetaData;
            this._schedule = context.JobDetail.JobDataMap[this._job.ScheduleUniqueId.ToString()] as ScheduleMetaData;

            try
            {
                if (context.NextFireTimeUtc.HasValue)
                    this._job.JobStatusTypeId = (int)JobStatusType.Recurrence;
                else
                    this._job.JobStatusTypeId = (int)JobStatusType.Succeeded;

                using (AppService app = new AppService())
                {
                    if (!app.Save(this._job))
                    {
                        foreach (IValidationMessage message in app.BrokenRules)
                            Logger.Logs.Log(message.Message, LogType.Info);
                    }
                }

                if (this._timer != null)
                {
                    this._timer.Stop();
                    this._timer.Dispose();
                }

                this._totalElapsedTime = TimeSpan.Zero;
                this._currentElapsedTime = TimeSpan.Zero;

#if NET_40
                this.OnNotifyClient();

                this.OnGlobalMessage(string.Format("A scheduled job '{0}' was vetoed at {1}.", this._schedule.Name, DateTime.Now));
#endif

            }
            catch (Exception ex)
            {
                //  Attempt to log an exception, if the database is offline, logging should try email.
                SmtpLogger.Logs.Log(string.Format("Job with key {0} failed miserably at {1}."
                    , context.JobDetail.Key.Name
                    , DateTime.Now)
                    , ex
                    , LogType.Error);
            }

        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {
            Logger.Logs.Log(string.Format("Job with key {0} and job group {1} is to be executed."
                , context.JobDetail.Key.Name
                , context.JobDetail.Key.Group
                , DateTime.Now
                )
                , LogType.Info);

            this._job = context.JobDetail.JobDataMap[context.JobDetail.Key.Name] as JobMetaData;
            this._schedule = context.JobDetail.JobDataMap[this._job.ScheduleUniqueId.ToString()] as ScheduleMetaData;

            this._job.JobStatusTypeId = (int)JobStatusType.InProgress;

            this._timer = new Timer(1000);
            this._timer.Elapsed += timer_Elapsed;
            this._startTime = DateTime.Now;
            this._totalElapsedTime = this._currentElapsedTime;
            this._timer.Start();

            try
            {
#if NET_40
                this.OnGlobalMessage(string.Format("A scheduled job '{0}' was started at {1}.", this._schedule.Name, DateTime.Now));
#endif

                using (AppService app = new AppService())
                {
                    if (!app.Save(this._job))
                    {
                        foreach (IValidationMessage message in app.BrokenRules)
                            Logger.Logs.Log(message.Message, LogType.Info);
                    }
                }
            }
            catch (Exception ex)
            {
                //  Attempt to log an exception, if the database is offline, logging should try email.
                SmtpLogger.Logs.Log(string.Format("Job with key {0} failed miserably at {1}."
                    , context.JobDetail.Key.Name
                    , DateTime.Now)
                    , ex
                    , LogType.Error);
            }
        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            Logger.Logs.Log(string.Format("Job with key {0} and job group {1} is was executed."
                , context.JobDetail.Key.Name
                , context.JobDetail.Key.Group
                , DateTime.Now
                )
                , LogType.Info);

            this._job = context.JobDetail.JobDataMap[context.JobDetail.Key.Name] as JobMetaData;
            this._schedule = context.JobDetail.JobDataMap[this._job.ScheduleUniqueId.ToString()] as ScheduleMetaData;

            try
            {
                if (jobException != null)
                {
                    Logger.Logs.Log(string.Format("GlobalJobListener captured an exception at {0}."
                        , DateTime.Now)
                        , jobException
                        , LogType.Warn);

                    this._job.JobStatusTypeId = (int)JobStatusType.Failed;
                }
                else
                {
                    if (context.NextFireTimeUtc.HasValue)
                        this._job.JobStatusTypeId = (int)JobStatusType.Recurrence;
                    else
                        this._job.JobStatusTypeId = (int)JobStatusType.Succeeded;
                }

                using (AppService app = new AppService())
                {
                    if (!app.Save(this._job))
                    {
                        foreach (IValidationMessage message in app.BrokenRules)
                            Logger.Logs.Log(message.Message, LogType.Info);
                    }
                }

                if (this._timer != null)
                {
                    this._timer.Stop();
                    this._timer.Dispose();
                }

                this._totalElapsedTime = TimeSpan.Zero;
                this._currentElapsedTime = TimeSpan.Zero;

#if NET_40
                this.OnNotifyClient();

                this.OnGlobalMessage(string.Format("A scheduled job '{0}' completed at {1}.", this._schedule.Name, DateTime.Now));
#endif

            }
            catch (Exception ex)
            {
                //  Attempt to log an exception, if the database is offline, logging should try email.
                SmtpLogger.Logs.Log(string.Format("Job with key {0} failed miserably at {1}."
                    , context.JobDetail.Key.Name
                    , DateTime.Now)
                    , ex
                    , LogType.Error
                    )
                    ;
            }
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
