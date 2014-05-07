using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Quartz;
using Telerik.Web.UI;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Scheduling;
using System.ServiceModel;
using Vmgr.Data.Biz.Logging;

namespace Vmgr.SharePoint
{
    public partial class MonitoringControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<MonitorMetaData> _monitors = null;
        private IList<ScheduleMetaData> _schedules = null;
        private IList<PluginMetaData> _plugins = null;
        private IList<PackageMetaData> _packages = null;
       
        #endregion

        #region PROTECTED PROPERTIES

        protected IList<MonitorMetaData> monitors
        {
            get
            {
                if (this._monitors == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._monitors = app.GetMonitors()
                            .Where(s => s.Username == WindowsIdentity.GetCurrent().Name)
                            .Where(s => this.packages.Select(p => p.PackageId).Contains(s.PackageId))
                            .ToList()
                            ;
                    }
                }

                return this._monitors;
            }
        }

        protected IList<ScheduleMetaData> schedules
        {
            get
            {
                if (this._schedules == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._schedules = app.GetSchedules()
                            .Where(s => !s.Deactivated)
                            .ToList()
                            ;
                    }
                }

                return this._schedules;
            }
        }

        protected IList<PluginMetaData> plugins
        {
            get
            {
                if (this._plugins == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._plugins = app.GetPlugins()
                            .ToList()
                            ;
                    }
                }

                return this._plugins;
            }
        }

        protected IList<PackageMetaData> packages
        {
            get
            {
                if (this._packages == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._packages = app.GetPackages()
                            .Where(s => !s.Deactivated)
                            .Where(s => s.ServerUniqueId == (this.Page as BasePage).server.UniqueId)
                            .ToList()
                            ;
                    }
                }

                return this._packages;
            }
        }

        protected string javascriptRedirectUrl
        {
            get
            {
                return string.Format("window.location = '{0}';", this.Request.Url.PathAndQuery);
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void deleteMonitor(string monitorId)
        {
            using (AppService app = new AppService())
            {
                int id = monitorId.ToNullable<int>() ?? 0;

                MonitorMetaData monitor = app.GetMonitors()
                    .Where(m => m.MonitorId == id)
                    .FirstOrDefault()
                    ;

                app.RemoveMonitor(id);

                try
                {
                    BasicHttpBinding binding = new BasicHttpBinding();

                    if ((this.Page as BasePage).server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.None;
                    }

                    if ((this.Page as BasePage).server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.Transport;
                    }

                    ChannelFactory<Vmgr.Monitoring.IMonitoring> httpFactory = new ChannelFactory<Vmgr.Monitoring.IMonitoring>(binding
                        , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Monitoring/{3}/MonitoringManager"
                            , (this.Page as BasePage).server.WSProtocol
                            , (this.Page as BasePage).server.WSFqdn
                            , (this.Page as BasePage).server.WSPort
                            , monitor.PackageUniqueId
                            )
                            )
                            )
                            ;
                    Vmgr.Monitoring.IMonitoring monitorProxy = httpFactory.CreateChannel();
                    monitorProxy.TryStop();
                }
                catch (EndpointNotFoundException ex)
                {
                    Logger.Logs.Log("Unable to start monitoring on server.  The service does not appear to be online.", ex, LogType.Error);
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Unable to start monitoring on server.", ex, LogType.Error);
                }
            }
        }

        #endregion

        #region PROTECTED METHODS

        protected string getAnticipatedScheduleText(ISchedule schedule)
        {
            string result = string.Empty;

            CronExpression cronExpression = new CronExpression(schedule.RecurrenceRule);
            DateTimeOffset? next = cronExpression.GetNextValidTimeAfter(DateTime.Now);

            if (next.HasValue)
            {
                DateTime nextAnticipated = next.Value.LocalDateTime;

                if (next.Value.LocalDateTime < schedule.Start)
                    nextAnticipated = schedule.Start;

                switch ((RecurrenceType)schedule.RecurrenceTypeId)
                {
                    case RecurrenceType.Minutely:

                        CronMinute cronMinute = new CronMinute();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronMinute))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronMinute.IsSimple)
                        {
                            nextAnticipated = schedule.Start;

                            while (nextAnticipated < DateTime.Now)
                            {
                                nextAnticipated = nextAnticipated.AddMinutes(cronMinute.Minute);
                            }
                        }

                        break;

                    case RecurrenceType.Hourly:

                        CronHour cronHour = new CronHour();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronHour))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronHour.IsSimple)
                        {
                            nextAnticipated = schedule.Start;

                            while (nextAnticipated < DateTime.Now)
                            {
                                nextAnticipated = nextAnticipated.AddHours(cronHour.Hour);
                            }
                        }

                        break;

                    case RecurrenceType.Daily:
                        break;
                    case RecurrenceType.Weekly:
                        break;
                    case RecurrenceType.Monthly:
                        break;
                    case RecurrenceType.Yearly:
                        break;
                    default:
                        break;
                }

                result = string.Format("{0}"
                    , Cron.GenerateDateString(nextAnticipated)
                    )
                    ;

            }

            return result;
        }

        #endregion

        #region PUBLIC METHODS

        #endregion

        #region EVENTS

        protected void ajaxPanelMonitor_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            string command = e.Argument
               .Split(',')
               .FirstOrDefault()
               ;

            string param = e.Argument
                .Split(',')
                .LastOrDefault()
                ;

            switch (command)
            {
                case "REFRESH":
                    this._monitors = null;
                    this.repeaterMonitor.DataBind();
                    break;
                case "DELETE_MONITOR":
                    this.deleteMonitor(param);
                    this._monitors = null;
                    this.repeaterMonitor.DataBind();
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void linqDataSourceMonitor_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.monitors;
        }

        protected void linqDataSourceSchedule_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
           IList<int> plugins = this.plugins
                .Where(s => s.PackageId == e.WhereParameters["PackageId"].ToNullable<int>())
                .Select(s => s.PluginId)
                .ToList()
                ;

           e.Result = this.schedules
                .Where(s => plugins.Contains(s.PluginId ?? 0))
                .Select(s => new 
                { 
                    ScheduleId = s.ScheduleId, 
                    ScheduleUniqueId = s.UniqueId, 
                    Name = s.Name, 
                    NextAnticipated = this.getAnticipatedScheduleText(s),
                    PackageId = e.WhereParameters["PackageId"].ToNullable<int>() ?? 0,
                }
                )
                .ToList()
                ;
        }

        protected void repeaterMonitor_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                MonitorMetaData monitor = e.Item.DataItem as MonitorMetaData;

                RadButton buttonDelete = e.Item.FindControl("buttonDelete") as RadButton;
                buttonDelete.CommandArgument = monitor.MonitorId.ToString();
            }
        }

        #endregion

    }
}