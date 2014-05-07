using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Scheduling;
using Vmgr.SharePoint.Enumerations;

namespace Vmgr.SharePoint
{
    public partial class ScheduleEditorControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<PluginMetaData> _plugins = null;
        private ScheduleMetaData _schedule = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected string selectedPluginUniqueId
        {
            get
            {
                if (this.schedule.PluginId.HasValue)
                    return string.Format("'{0}'", this.schedule.PluginUniqueId);

                return "null";
            }
        }

        protected IList<PluginMetaData> plugins
        {
            get
            {
                if (this._plugins == null)
                {
                    int serverId = 0;

                    if ((this.Page as BasePage).server != null)
                        serverId = (this.Page as BasePage).server.ServerId;

                    using (AppService app = new AppService())
                    {
                        this._plugins = app.GetPlugins()
                            .Where(p => p.Schedulable)
                            .Where(p => p.PackageServerId == serverId)
                            .ToList()
                            ;

                        //  If a schedule exists, and there is a plugin defined for it, then only bring back one result.
                        if (this.schedule.PluginId.HasValue)
                            this._plugins = this._plugins
                                .Where(p => p.PluginId == this.schedule.PluginId)
                                .ToList()
                                ;
                    }
                }

                return this._plugins;
            }
        }

        protected Guid scheduleUniqueId
        {
            get
            {
                Guid g = Guid.Empty;

                if (this.Request.QueryString["UniqueId"] != null)
                    g = new Guid(this.Request.QueryString["UniqueId"]);

                return g;
            }
        }

        protected ScheduleMetaData schedule
        {
            get
            {
                if (this._schedule == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._schedule = app.GetSchedules()
                            .Where(s => s.UniqueId == scheduleUniqueId)
                            .FirstOrDefault() ?? new ScheduleMetaData { Start = DateTime.Now, RecurrenceRule = "0 0/1 * 1/1 * ? *" };
                    }
                }
                return this._schedule;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void saveSchedule(string param)
        {
            PluginMetaData selectedPlugin = this.plugins
                .Where(p => p.UniqueId == new Guid(param))
                .FirstOrDefault()
                ;

            if (selectedPlugin == null)
                throw new ApplicationException("Selected plugin must not be undefined.");

            string selectedTab = this.tabStripRecurrence.SelectedTab.Text;

            Cron chronExpBuilder = new Cron();

            string recurrenceRule = string.Empty;

            switch (selectedTab)
            {
                case "Minutely":

                    CronMinute minute = new CronMinute();
                    minute.IsSimple = true;
                    minute.Minute = (int)this.numericTextBoxMinutely.Value;

                    recurrenceRule = chronExpBuilder.BuildExpression(minute);

                    break;

                case "Hourly":

                    CronHour hour = new CronHour();

                    if (this.buttonToggleHourly1.Checked)
                    {
                        hour.IsSimple = true;
                        hour.Hour = (int)this.numericTextBoxHourly.Value;
                    }
                    else
                    {
                        int hourlyMin = 0;
                        int hourlyHour = 0;

                        if (this.timePickerHourlyStartTime.SelectedDate.HasValue)
                        {
                            hourlyMin = this.timePickerHourlyStartTime.SelectedDate.Value.Minute;
                            hourlyHour = this.timePickerHourlyStartTime.SelectedDate.Value.Hour;
                        }

                        hour.IsSimple = false;
                        hour.Minute = hourlyMin;
                        hour.Hour = hourlyHour;
                    }

                    recurrenceRule = chronExpBuilder.BuildExpression(hour);

                    break;

                case "Daily":

                    CronDay day = new CronDay();

                    TimeSpan dailyStart = this.timePickerDailyStartTime.SelectedDate.Value.TimeOfDay;

                    if (this.buttonToggleDaily1.Checked)
                    {
                        day.IsSimple = true;
                        day.Day = this.numericTextBoxDaily.Value.ToString();
                    }
                    else
                    {
                        day.IsSimple = false;
                    }

                    day.Hour = dailyStart.Hours;
                    day.Minute = dailyStart.Minutes;

                    recurrenceRule = chronExpBuilder.BuildExpression(day);

                    break;

                case "Weekly":

                    IList<string> weeklyWeekDays = new List<string> { };

                    if (buttonWeeklyMonday.Checked)
                        weeklyWeekDays.Add("MON");

                    if (buttonWeeklyTuesday.Checked)
                        weeklyWeekDays.Add("TUE");

                    if (buttonWeeklyWednesday.Checked)
                        weeklyWeekDays.Add("WED");

                    if (buttonWeeklyThursday.Checked)
                        weeklyWeekDays.Add("THU");

                    if (buttonWeeklyFriday.Checked)
                        weeklyWeekDays.Add("FRI");

                    if (buttonWeeklySaturday.Checked)
                        weeklyWeekDays.Add("SAT");

                    if (buttonWeeklySunday.Checked)
                        weeklyWeekDays.Add("SUN");

                    CronWeek week = new CronWeek();
                    week.WkDays = weeklyWeekDays.ToArray();

                    TimeSpan weeklyStart = this.timePickerWeeklyStartTime.SelectedDate.Value.TimeOfDay;

                    week.Hour = weeklyStart.Hours;
                    week.Minute = weeklyStart.Minutes;

                    recurrenceRule = chronExpBuilder.BuildExpression(week);

                    break;

                case "Monthly":

                    CronMonth month = new CronMonth();

                    TimeSpan monthlyStart = this.timePickerMonthlyStartTime.SelectedDate.Value.TimeOfDay;

                    if (this.buttonToggleMonthly1.Checked)
                    {
                        month.IsMod = false;
                        month.Day = (int)this.numericTextBoxMonthlyDay.Value;
                        month.Month = (int)this.numericTextBoxMonthlyMonths.Value;
                    }
                    else
                    {
                        month.IsMod = true;
                        month.Index = this.comboBoxMonthlyOccurrenceIndex.SelectedValue.ToNullable<int>() ?? 0;
                        month.WkDay = this.comboBoxMonthlyOccurrenceWeekDay.SelectedValue;
                        month.Occur = (int)this.numericTextBoxOccurrenceMonth.Value;
                    }

                    month.Hour = monthlyStart.Hours;
                    month.Minute = monthlyStart.Minutes;

                    recurrenceRule = chronExpBuilder.BuildExpression(month);

                    break;

                case "Yearly":

                    CronYear year = new CronYear();

                    TimeSpan yearlyStart = this.timePickerYearlyStartTime.SelectedDate.Value.TimeOfDay;

                    if (this.buttonToggleYearly1.Checked)
                    {
                        year.IsMod = false;
                        year.Month = this.comboBoxYearlyOccurrenceMonth.SelectedValue;
                        year.Day = (int)this.numericTextBoxYearlyOccurrenceDay.Value;
                    }
                    else
                    {
                        year.IsMod = true;
                        year.Index = this.comboBoxYearlyOccurrenceIndex.SelectedValue.ToNullable<int>() ?? 0;
                        year.WkDay = this.comboBoxYearlyOccurrenceWeekDay.SelectedValue;
                        year.WkMonth = this.comboBoxYearlyOccurrenceWeekMonth.SelectedValue.ToNullable<int>() ?? 0;
                    }

                    year.Hour = yearlyStart.Hours;
                    year.Minute = yearlyStart.Minutes;

                    recurrenceRule = chronExpBuilder.BuildExpression(year);

                    break;
            }

            this.schedule.Deactivated = false;
            this.schedule.Name = this.textBoxName.Text;
            this.schedule.Description = this.textBoxDescription.Text;
            this.schedule.PluginId = selectedPlugin.PluginId;
            this.schedule.RecurrenceRule = recurrenceRule;
            this.schedule.RecurrenceTypeId = (int)(Enum.Parse(typeof(RecurrenceType), selectedTab));

            DateTime startDate = this.dateTimePickerStart.SelectedDate.Value;

            this.schedule.Start = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, 0);

            if (this.schedule.ScheduleId == 0)
                this.schedule.UniqueId = Guid.NewGuid();

            this.schedule.End = null;

            if (this.dateTimePickerEnd.SelectedDate.HasValue)
                this.schedule.End = this.dateTimePickerEnd.SelectedDate.Value;

            this.schedule.MisfireInstruction = 2;

            schedule.Exclusions = JsonConvert.SerializeObject(this.calendarExclusions.SelectedDates.Cast<RadDate>()
                .Select(d => d.Date)
                .ToList()
                )
                ;

            using (AppService app = new AppService())
            {
                if (app.Save(this.schedule))
                {
                    this.schedule.PluginUniqueId = selectedPlugin.UniqueId;

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

                        ChannelFactory<Vmgr.Scheduling.IScheduler> httpFactory = new ChannelFactory<Vmgr.Scheduling.IScheduler>(binding
                            , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Scheduling/{3}/SchedulerManager"
                                , (this.Page as BasePage).server.WSProtocol
                                , (this.Page as BasePage).server.WSFqdn
                                , (this.Page as BasePage).server.WSPort
                                , selectedPlugin.PackageUniqueId
                                )
                                )
                                )
                                ;
                        Vmgr.Scheduling.IScheduler schedulerProxy = httpFactory.CreateChannel();
                        schedulerProxy.Schedule(schedule);
                    }
                    catch (EndpointNotFoundException ex)
                    {
                        Logger.Logs.Log("Unable to create schedule on server.  The service does not appear to be online.", ex, LogType.Error);
                    }
                    catch (Exception ex)
                    {
                        Logger.Logs.Log("Unable to create schedule on server.", ex, LogType.Error);
                    }

                    (this.Page as BasePage).Alert("The schedule was successfully saved."
                        , "Save Schedule Success"
                        , "OnCloseSchedulerEditor"
                        , AlertType.Check
                        )
                        ;
                }
                else
                {
                    Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);

                    (this.Page as BasePage).Alert("The was a problem saving the schedule.  Please check logs to determine the cause."
                        , "Failed to Save Schedule"
                        , null
                        , AlertType.Error
                        )
                        ;
                }
            }
        }

        private void Set()
        {
            this.textBoxName.Text = this.schedule.Name;
            this.textBoxDescription.Text = this.schedule.Description;
            this.dateTimePickerStart.SelectedDate = this.schedule.Start;
            this.dateTimePickerEnd.SelectedDate = this.schedule.End;

            foreach (RadTab tab in this.tabStripRecurrence.Tabs)
                if (int.Parse(tab.Value) == this.schedule.RecurrenceTypeId)
                    tab.Selected = true;

            foreach (RadPageView pageView in this.multiPageRecurrence.PageViews)
                if (this.tabStripRecurrence.SelectedTab.PageViewID == pageView.ID)
                    pageView.Selected = true;

            if (CronExpression.IsValidExpression(this.schedule.RecurrenceRule))
            {
                switch ((RecurrenceType)this.schedule.RecurrenceTypeId)
                {
                    case RecurrenceType.Minutely:

                        CronMinute cronMinute = new CronMinute();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronMinute))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        this.numericTextBoxMinutely.Value = cronMinute.Minute;

                        break;

                    case RecurrenceType.Hourly:

                        CronHour cronHour = new CronHour();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronHour))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronHour.IsSimple)
                        {
                            this.buttonToggleHourly1.Checked = true;
                            this.numericTextBoxHourly.Value = cronHour.Hour;
                        }
                        else
                        {
                            this.buttonToggleHourly2.Checked = true;

                            this.timePickerHourlyStartTime.SelectedDate = new DateTime(
                                DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day,
                                cronHour.Hour,
                                cronHour.Minute,
                                0
                                )
                                ;
                        }

                        break;

                    case RecurrenceType.Daily:

                        CronDay cronDay = new CronDay();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronDay))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronDay.IsSimple)
                        {
                            this.buttonToggleDaily1.Checked = true;
                            this.numericTextBoxDaily.Value = double.Parse(cronDay.Day);
                        }
                        else
                        {
                            this.buttonToggleDaily2.Checked = true;
                        }

                        this.timePickerDailyStartTime.SelectedDate = new DateTime(
                                DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day,
                                cronDay.Hour,
                                cronDay.Minute,
                                0
                                )
                                ;

                        break;

                    case RecurrenceType.Weekly:

                        CronWeek cronWeek = new CronWeek();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronWeek))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronWeek.WkDays.Contains("MON"))
                            this.buttonWeeklyMonday.Checked = true;

                        if (cronWeek.WkDays.Contains("TUE"))
                            this.buttonWeeklyTuesday.Checked = true;

                        if (cronWeek.WkDays.Contains("WED"))
                            this.buttonWeeklyWednesday.Checked = true;

                        if (cronWeek.WkDays.Contains("THU"))
                            this.buttonWeeklyThursday.Checked = true;

                        if (cronWeek.WkDays.Contains("FRI"))
                            this.buttonWeeklyFriday.Checked = true;

                        if (cronWeek.WkDays.Contains("SAT"))
                            this.buttonWeeklySaturday.Checked = true;

                        if (cronWeek.WkDays.Contains("SUN"))
                            this.buttonWeeklySunday.Checked = true;

                        this.timePickerWeeklyStartTime.SelectedDate = new DateTime(
                                DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day,
                                cronWeek.Hour,
                                cronWeek.Minute,
                                0
                                )
                                ;

                        break;

                    case RecurrenceType.Monthly:

                        CronMonth cronMonth = new CronMonth();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronMonth))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronMonth.IsMod)
                        {
                            this.buttonToggleMonthly2.Checked = true;
                            this.comboBoxMonthlyOccurrenceIndex.SelectedValue = cronMonth.Index.ToString();
                            this.comboBoxMonthlyOccurrenceWeekDay.SelectedValue = cronMonth.WkDay;
                            this.numericTextBoxOccurrenceMonth.Value = cronMonth.Occur;
                        }
                        else
                        {
                            this.buttonToggleMonthly1.Checked = true;
                            this.numericTextBoxMonthlyDay.Value = cronMonth.Day;
                            this.numericTextBoxMonthlyMonths.Value = cronMonth.Month;
                        }

                        this.timePickerMonthlyStartTime.SelectedDate = new DateTime(
                                DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day,
                                cronMonth.Hour,
                                cronMonth.Minute,
                                0
                                )
                                ;

                        break;

                    case RecurrenceType.Yearly:

                        CronYear cronYear = new CronYear();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronYear))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronYear.IsMod)
                        {
                            this.buttonToggleYearly2.Checked = true;
                            this.comboBoxYearlyOccurrenceIndex.SelectedValue = cronYear.Index.ToString();
                            this.comboBoxYearlyOccurrenceWeekDay.SelectedValue = cronYear.WkDay;
                            this.comboBoxYearlyOccurrenceWeekMonth.SelectedValue = cronYear.WkMonth.ToString();
                        }
                        else
                        {
                            this.buttonToggleYearly1.Checked = true;
                            this.comboBoxYearlyOccurrenceMonth.SelectedValue = cronYear.Month;
                            this.numericTextBoxYearlyOccurrenceDay.Value = cronYear.Day;
                        }

                        this.timePickerYearlyStartTime.SelectedDate = new DateTime(
                                DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day,
                                cronYear.Hour,
                                cronYear.Minute,
                                0
                                )
                                ;

                        break;

                    default:
                        break;
                }
            }

            this.SetExclusions();
        }

        private void SetExclusions()
        {
            this.calendarExclusions.SelectedDates.Clear();

            if (!string.IsNullOrEmpty(this.schedule.Exclusions))
            {
                IList<DateTime> dates = JsonConvert.DeserializeObject<List<DateTime>>(this.schedule.Exclusions);

                IList<RadDate> exclusions = dates
                    .Select(r => new RadDate(r))
                    .ToList();

                this.calendarExclusions.SelectedDates.AddRange(exclusions.ToArray());

                this.labelExclusions.Text = string.Join(", ", this.calendarExclusions.SelectedDates.Cast<RadDate>()
                    .OrderBy(d => d.Date)
                    .Select(d => d.Date.ToShortDateString())
                    .ToArray()
                    )
                    ;

                this.panelExclusions.Visible = this.calendarExclusions.SelectedDates.Count > 0;

            }
        }

        private void SetOptionsCookie(bool expanded)
        {
            HttpCookie cookieOpen = new HttpCookie("EXCLUSION_OPTIONS");

            if (this.Request.Cookies["EXCLUSION_OPTIONS"] != null)
                cookieOpen = Request.Cookies["EXCLUSION_OPTIONS"];

            if (!string.IsNullOrEmpty(cookieOpen.Value))
                expanded = (!bool.Parse(cookieOpen.Value));

            cookieOpen.Value = expanded.ToString();
            cookieOpen.Expires = DateTime.Now.AddYears(1);

            // Add the cookie.
            this.Response.Cookies.Add(cookieOpen);
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        #endregion

        #region EVENTS

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.optionsTd.Visible = false;

            if (this.Request.Cookies["EXCLUSION_OPTIONS"] != null)
            {
                bool expanded = bool.Parse(this.Request.Cookies["EXCLUSION_OPTIONS"].Value);

                this.optionsTd.Visible = expanded;
            }

        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.Set();
            }
        }

        protected void ajaxPanelExclusions_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
                case "OPEN_OPTIONS":

                    this.SetOptionsCookie(true);
                    this.optionsTd.Visible = true;

                    break;

                case "CLOSE_OPTIONS":

                    this.SetOptionsCookie(false);
                    this.optionsTd.Visible = false;

                    break;

                case "UNDO_EXCLUSIONS":

                    this.SetExclusions();

                    this.treeViewOptions.UnselectAllNodes();

                    break;

                case "REMOVE_EXCLUSIONS":

                    this.calendarExclusions.SelectedDates.Clear();
                    this.labelExclusions.Text = "No dates selected";
                    this.treeViewOptions.UnselectAllNodes();
                   
                    break;

            }
        }

        protected void gridPlugin_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item
                || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
            {
                PluginMetaData plugin = e.Item.DataItem as PluginMetaData;

                if (this.schedule.PluginId > 0)
                    if (plugin.PluginId == this.schedule.PluginId)
                        e.Item.Selected = true;
            }

            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Pager)
            {
                if (this.schedule.PluginId.HasValue)
                    e.Item.Display = false;
            }
        }

        protected void linqDataSourcePlugin_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.plugins;
        }

        protected void ajaxPanelSchedule_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
                case "SAVE_SCHEDULE":
                    this.saveSchedule(param);
                    break;
            }
        }

        protected void calendarExclusions_SelectionChanged(object sender, Telerik.Web.UI.Calendar.SelectedDatesEventArgs e)
        {
            this.labelExclusions.Text = string.Join(", ", this.calendarExclusions.SelectedDates.Cast<RadDate>()
                .OrderBy(d => d.Date)
                .Select(d => d.Date.ToShortDateString())
                .ToArray()
                )
                ;

            if (this.calendarExclusions.SelectedDates.Count == 0)
                this.labelExclusions.Text = "No dates selected";

            this.panelExclusions.Visible = true;
        }


        #endregion

    }
}