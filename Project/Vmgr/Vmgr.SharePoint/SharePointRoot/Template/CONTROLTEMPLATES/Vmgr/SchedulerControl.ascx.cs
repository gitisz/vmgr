using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Telerik.Web.UI;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Packaging;
using Vmgr.SharePoint.Enumerations;
using System.Web;
using Vmgr.Plugins;
using System.Web.UI;

namespace Vmgr.SharePoint
{
    public partial class SchedulerControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<PluginMetaData> _plugins = null;
        private IList<FilterMetaData> _filters = null;
        private IList<ScheduleMetaData> _schedules = null;
        private string _scheduleScript = string.Empty;

        #endregion

        #region PROTECTED PROPERTIES

        protected IList<FilterMetaData> filters
        {
            get
            {
                if (this._filters == null)
                {

                    using (AppService app = new AppService())
                    {
                        this._filters = app.GetFilters()
                            .Where(p => !p.Deactivated)
                            .Where(p => p.FilterType == FilterType.Schedule.ToString())
                            .Where(m => m.Username == WindowsIdentity.GetCurrent().Name)
                            .ToList()
                            ;
                    }

                }

                return this._filters;
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
                    }
                }

                return this._plugins;
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
                        var query = app.GetSchedules()
                            .Where(s => this.plugins.Select(p => p.PluginId).Contains(s.PluginId ?? 0))
                            ;

                        if (this.Request.Cookies["SCHEDULE_FILTER_DLINQ"] != null)
                        {
                            string scheduleFilterExpression = this.Request.Cookies["SCHEDULE_FILTER_DLINQ"].Value;

                            if (!string.IsNullOrEmpty(scheduleFilterExpression))
                            {
                                query = query
                                    .Where(scheduleFilterExpression);
                            }
                        }

                        this._schedules = query
                            .ToList()
                            ;

                    }
                }

                return this._schedules;
            }
        }

        protected string javascriptRedirectUrl
        {
            get
            {
                return string.Format("window.location = '{0}';", this.Request.Url.PathAndQuery);
            }
        }

        protected int? pageIndex
        {
            get
            {
                if (this.Session["SCHEDULER_PAGE_INDEX"] == null)
                    return null;
                else
                    return this.Session["SCHEDULER_PAGE_INDEX"] as int?;
            }
            set
            {
                if (this.Session["SCHEDULER_PAGE_INDEX"] == null)
                    this.Session.Add("SCHEDULER_PAGE_INDEX", value);
                else
                    this.Session["SCHEDULER_PAGE_INDEX"] = value;
            }
        }

        protected int? pageSize
        {
            get
            {
                if (this.Session["SCHEDULER_PAGE_SIZE"] == null)
                    return null;
                else
                    return this.Session["SCHEDULER_PAGE_SIZE"] as int?;
            }
            set
            {
                if (this.Session["SCHEDULER_PAGE_SIZE"] == null)
                    this.Session.Add("SCHEDULER_PAGE_SIZE", value);
                else
                    this.Session["SCHEDULER_PAGE_SIZE"] = value;
            }
        }
        
        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void ApplyFilter(int? filterId)
        {
            HttpCookie cookieScheduleFilterDLinq = new HttpCookie("SCHEDULE_FILTER_DLINQ");
            HttpCookie cookieScheduleFilterExpression = new HttpCookie("SCHEDULE_FILTER_EXPRESSION");
            HttpCookie cookieScheduleFilterPreview = new HttpCookie("SCHEDULE_FILTER_PREVIEW");

            if (this.Request.Cookies["SCHEDULE_FILTER_DLINQ"] != null)
                cookieScheduleFilterDLinq = Request.Cookies["SCHEDULE_FILTER_DLINQ"];

            if (this.Request.Cookies["SCHEDULE_FILTER_EXPRESSION"] != null)
                cookieScheduleFilterExpression = Request.Cookies["SCHEDULE_FILTER_EXPRESSION"];

            if (this.Request.Cookies["SCHEDULE_FILTER_PREVIEW"] != null)
                cookieScheduleFilterPreview = Request.Cookies["SCHEDULE_FILTER_PREVIEW"];

            FilterMetaData myFilter = this.filters
                .Where(f => f.FilterId == filterId)
                .FirstOrDefault()
                ;

            RadFilter filter = new RadFilter();
            FilterScheduleControl.PopulateFilterFields(filter);

            this.buttonClearFilter.Visible = true;

            if (myFilter != null)
            {
                filter.LoadSettings(myFilter.Expression);

                string dlinq = FilterScheduleControl.DecodeLinqFilterExpression(filter.RootGroup);
                string preview = FilterScheduleControl.DecodePreviewFilterExpression(filter.RootGroup, filter);
                string expression = filter.SaveSettings();

                cookieScheduleFilterExpression.Value = expression;
                cookieScheduleFilterExpression.Expires = DateTime.Now.AddYears(1);

                cookieScheduleFilterPreview.Value = preview;
                cookieScheduleFilterPreview.Expires = DateTime.Now.AddYears(1);

                cookieScheduleFilterDLinq.Value = dlinq;
                cookieScheduleFilterDLinq.Expires = DateTime.Now.AddYears(1);

                this.literalScheduleFilterExpression.Text = preview;

                this.UpdateNodeSelection(expression);
            }
            else
            {
                if (filterId.HasValue)
                {
                    filter.LoadSettings(FilterScheduleControl.SCHEDULE_UNDO_NEW_FILTER);
                    cookieScheduleFilterExpression.Value = FilterScheduleControl.SCHEDULE_UNDO_NEW_FILTER;
                    cookieScheduleFilterExpression.Expires = DateTime.Now.AddYears(-1);

                    cookieScheduleFilterPreview.Value = string.Empty;
                    cookieScheduleFilterPreview.Expires = DateTime.Now.AddYears(-1);

                    cookieScheduleFilterDLinq.Value = string.Empty;
                    cookieScheduleFilterDLinq.Expires = DateTime.Now.AddYears(-1);

                    this.buttonClearFilter.Visible = false;
                    this.literalScheduleFilterExpression.Text = string.Empty;
                    this.treeViewOptions.UnselectAllNodes();
                }
                else
                {
                    if (!string.IsNullOrEmpty(cookieScheduleFilterExpression.Value))
                    {
                        filter.LoadSettings(cookieScheduleFilterExpression.Value);
                        this.literalScheduleFilterExpression.Text = cookieScheduleFilterPreview.Value;
                        this.UpdateNodeSelection(cookieScheduleFilterExpression.Value);
                    }
                    else
                    {
                        filter.LoadSettings(FilterScheduleControl.SCHEDULE_UNDO_NEW_FILTER);
                        this.buttonClearFilter.Visible = false;
                        this.literalScheduleFilterExpression.Text = string.Empty;
                        this.treeViewOptions.UnselectAllNodes();
                    }
                }
            }

            // Add the cookie.
            this.Response.Cookies.Add(cookieScheduleFilterDLinq);
            this.Response.Cookies.Add(cookieScheduleFilterExpression);
            this.Response.Cookies.Add(cookieScheduleFilterPreview);

            this.gridSchedule.Rebind();
        }

        private void ConfirmPauseResumeSchedule(string uniqueId)
        {
            using (AppService app = new AppService())
            {
                ScheduleMetaData schedule = app.GetSchedules()
                    .Where(s => s.UniqueId == new Guid(uniqueId))
                    .FirstOrDefault()
                    ;

                if (schedule == null)
                    throw new ApplicationException("Schedule must not be undefined.");

                string message = string.Format("Are you sure you want to pause the schedule <span style=\"font-weight: bold;\">{0}</span>?<br /><br />", schedule.Name);

                if (schedule.Deactivated)
                    message = string.Format("Are you sure you want to resume the schedule <span style=\"font-weight: bold;\">{0}</span>?<br /><br />", schedule.Name);

                (this.Page as BasePage).Confirm(message
                    , string.Format("{0} Scheudle", schedule.Deactivated ? "Resume" : "Pause")
                    , "OnConfirmPauseResumeScheduleHandler"
                    , Telerik.Charting.Styles.Unit.Pixel(500)
                    , Telerik.Charting.Styles.Unit.Pixel(200)
                    , Enumerations.ConfirmType.Confirm
                    );
            }
        }

        private void ConfirmDeleteSchedule(string uniqueId)
        {
            using (AppService app = new AppService())
            {
                ScheduleMetaData schedule = app.GetSchedules()
                    .Where(s => s.UniqueId == new Guid(uniqueId))
                    .FirstOrDefault()
                    ;

                if (schedule == null)
                    throw new ApplicationException("Schedule must not be undefined.");

                string message = string.Format("Are you sure you want to permanently delete the schedule <span style=\"font-weight: bold;\">{0}</span>?<br /><p style=\"font-style: italic; color: orange;\";>All job history will also be deleted for this schedule!</p>", schedule.Name);

                (this.Page as BasePage).Confirm(message
                    , "Delete Schedule"
                    , "OnConfirmDeleteScheduleHandler"
                    , Telerik.Charting.Styles.Unit.Pixel(500)
                    , Telerik.Charting.Styles.Unit.Pixel(200)
                    , Enumerations.ConfirmType.Confirm
                    );
            }
        }

        private void PauseResmueSchedule(string uniqueId)
        {
            try
            {
                using (AppService app = new AppService())
                {
                    ScheduleMetaData schedule = app.GetSchedules()
                        .Where(s => s.UniqueId == new Guid(uniqueId))
                        .FirstOrDefault()
                        ;

                    if (schedule == null)
                        throw new ApplicationException("Schedule must not be undefined.");

                    schedule.Deactivated = !schedule.Deactivated;

                    if (!app.Save(schedule))
                        Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);
                    else
                    {
                        try
                        {
                            PluginMetaData plugin = this.plugins
                                .Where(p => schedule.PluginUniqueId == p.UniqueId)
                                .FirstOrDefault()
                                ;

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
                                    , plugin.PackageUniqueId
                                    )
                                    )
                                    )
                                    ;
                            Vmgr.Scheduling.IScheduler schedulerProxy = httpFactory.CreateChannel();

                            if (schedule.Deactivated)
                                schedulerProxy.UnSchedule(schedule);
                            else
                                schedulerProxy.Schedule(schedule);

                        }
                        catch (EndpointNotFoundException ex)
                        {
                            Logger.Logs.Log("Unable to load/unload schedule on server.  The service does not appear to be online.", ex, LogType.Error);
                        }
                        catch (Exception ex)
                        {
                            Logger.Logs.Log("Unable to load/unload schedule on server.", ex, LogType.Error);
                        }

                        (this.Page as BasePage).Alert(string.Format("The schedule was successfullly {0}.", schedule.Deactivated ? "paused" : "resumed")
                            , string.Format("{0} Schedule Success", schedule.Deactivated ? "Pause" : "Resume")
                            , "OnDeleteScheduleComplete"
                            , AlertType.Check
                            )
                            ;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logs.Log(ex, LogType.Error);

                (this.Page as BasePage).Alert(string.Format("Failed to load/unload schedule.  Please check the log for additional information.")
                    , "Load/Unload Schedule Failure"
                    , null
                    , Telerik.Charting.Styles.Unit.Pixel(550)
                    , Telerik.Charting.Styles.Unit.Pixel(200)
                    , AlertType.Error
                    )
                    ;
            }
        }

        private void SetOptionsCookie(bool expanded)
        {
            HttpCookie cookieOpen = new HttpCookie("SCHEDULE_OPTIONS");

            if (this.Request.Cookies["SCHEDULE_OPTIONS"] != null)
                cookieOpen = Request.Cookies["SCHEDULE_OPTIONS"];

            if (!string.IsNullOrEmpty(cookieOpen.Value))
                expanded = (!bool.Parse(cookieOpen.Value));

            cookieOpen.Value = expanded.ToString();
            cookieOpen.Expires = DateTime.Now.AddYears(1);

            // Add the cookie.
            this.Response.Cookies.Add(cookieOpen);

        }

        private void DeleteSchedule(string uniqueId)
        {
            IList<string> messages = new List<string> { };

            ScheduleMetaData schedule = null;

            try
            {
                using (AppService app = new AppService())
                {
                    schedule = app.GetSchedules()
                        .Where(s => s.UniqueId == new Guid(uniqueId))
                        .FirstOrDefault()
                        ;

                    if (schedule == null)
                        throw new ApplicationException("Schedule must not be undefined.");

                    PluginMetaData plugin = this.plugins
                        .Where(p => schedule.PluginUniqueId == p.UniqueId)
                        .FirstOrDefault()
                        ;

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
                            , plugin.PackageUniqueId
                            )
                            )
                            )
                            ;
                    Vmgr.Scheduling.IScheduler schedulerProxy = httpFactory.CreateChannel();
                    schedulerProxy.UnSchedule(schedule);
                }

            }
            catch (EndpointNotFoundException ex)
            {
                Logger.Logs.Log("Unable to delete schedule on server.  The service does not appear to be online.", ex, LogType.Error);
                messages.Add("Unable to delete schedule on server.  The service does not appear to be online.");
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Unable to delete schedule on server.", ex, LogType.Error);
                messages.Add("Unable to delete schedule on server.");
            }

            string message = string.Join("</li><li>", messages.ToArray());

            using (AppService app = new AppService())
            {
                try
                {
                    app.RemoveSchedule(schedule.ScheduleId);

                    if (string.IsNullOrEmpty(message))
                    {

                        (this.Page as BasePage).Alert("The schedule was successfullly deleted."
                            , "Delete Schedule Success"
                            , "OnDeleteScheduleComplete"
                            , AlertType.Check
                            )
                            ;
                    }
                    else
                    {
                        (this.Page as BasePage).Alert(string.Format("The schedule was deleted, however please review the following messages: <br /> <ul><li>{0}</li></ul>", message)
                            , "Delete Schedule Warning"
                            , "OnDeleteScheduleComplete"
                            , Telerik.Charting.Styles.Unit.Pixel(550)
                            , Telerik.Charting.Styles.Unit.Pixel(200)
                            , AlertType.Error
                            )
                            ;
                    }
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log(ex, LogType.Error);

                    (this.Page as BasePage).Alert(string.Format("Failed to delete schedule.  Please check the log for additional information.")
                        , "Delete Schedule Failure"
                        , null
                        , Telerik.Charting.Styles.Unit.Pixel(550)
                        , Telerik.Charting.Styles.Unit.Pixel(200)
                        , AlertType.Error
                        )
                        ;
                }
            }
        }

        private void RefreshFilters()
        {
            this._filters = null;

            RadTreeNode mangeFiltersNode = this.treeViewOptions.Nodes.FindNodeByValue("MANAGE_FILTERS");

            IList<RadTreeNode> nodesToRemove = mangeFiltersNode.Nodes
                .Cast<RadTreeNode>()
                .Where(n => n.Value != "APPLY_FILTER,0")
                .ToList();

            foreach (RadTreeNode node in nodesToRemove)
                mangeFiltersNode.Nodes.Remove(node);

            foreach (FilterMetaData filter in this.filters)
                mangeFiltersNode.Nodes.Add(new RadTreeNode
                {
                    Text = filter.Name,
                    ImageUrl = "/_layouts/images/Vmgr/user-blue-icon-16.png",
                    Value = "APPLY_FILTER," + filter.FilterId,
                }
                )
                ;
        }

        private void UpdateNodeSelection(string expression)
        {
            RadTreeNode node = this.treeViewOptions.FindNodeByValue("MANAGE_FILTERS");

            foreach (FilterMetaData f in this.filters)
            {
                var n = node.Nodes
                    .Cast<RadTreeNode>()
                    .Where(r => r.Text == f.Name)
                    .FirstOrDefault()
                    ;

                var fNode = new RadTreeNode
                {
                    Text = f.Name,
                    ImageUrl = "/_layouts/images/Vmgr/user-blue-icon-16.png",
                    Value = "APPLY_FILTER," + f.FilterId,
                }
                    ;

                if (n == null)
                {
                    node.Nodes.Add(fNode);
                }

                fNode.Selected = expression == f.Expression;
            }
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

            if (this.Request.Cookies["SCHEDULE_OPTIONS"] != null)
            {
                bool expanded = bool.Parse(this.Request.Cookies["SCHEDULE_OPTIONS"].Value);

                this.optionsTd.Visible = expanded;
            }

            if (this.Request.Cookies["SCHEDULE_FILTER_PREVIEW"] != null)
            {
                string packageFilterPreview = this.Request.Cookies["SCHEDULE_FILTER_PREVIEW"].Value;
                this.literalScheduleFilterExpression.Text = packageFilterPreview;
            }

            string filterExp = string.Empty;

            if (this.Request.Cookies["SCHEDULE_FILTER_EXPRESSION"] != null)
            {
                filterExp = this.Request.Cookies["SCHEDULE_FILTER_EXPRESSION"].Value;

                if (filterExp != FilterScheduleControl.SCHEDULE_UNDO_NEW_FILTER)
                    this.buttonClearFilter.Visible = true;
            }

            if (this.Request.Cookies["SCHEDULE_FILTER_EXPRESSION"] != null)
            {
                filterExp = this.Request.Cookies["SCHEDULE_FILTER_EXPRESSION"].Value;

                if (filterExp != FilterScheduleControl.SCHEDULE_UNDO_NEW_FILTER)
                    this.buttonClearFilter.Visible = true;
            }

            if (!this.IsPostBack)
            {
                this.UpdateNodeSelection(filterExp);

                this.gridSchedule.PageSize = this.pageSize ?? 5;
                this.gridSchedule.CurrentPageIndex = this.pageIndex ?? 0;
            }
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
                case "CONFIRM_PAUSE_RESUME_SCHEDULE":
                    this.ConfirmPauseResumeSchedule(param);
                    break;
                case "PAUSE_RESUME_SCHEDULE":
                    this.PauseResmueSchedule(param);
                    break;
                case "CONFIRM_DELETE_SCHEDULE":
                    this.ConfirmDeleteSchedule(param);
                    break;
                case "DELETE_SCHEDULE":
                    this.DeleteSchedule(param);
                    break;
                case "REFRESH":
                    this._schedules = null;
                    this.gridSchedule.Rebind();
                    break;
                case "REFRESH_FILTERS":

                    this.RefreshFilters();
                    break;

                case "OPEN_OPTIONS":

                    this.SetOptionsCookie(true);
                    this.optionsTd.Visible = true;

                    break;

                case "CLOSE_OPTIONS":

                    this.SetOptionsCookie(false);
                    this.optionsTd.Visible = false;

                    break;

                case "APPLY_FILTER":

                    this.ApplyFilter(param.ToNullable<int>());

                    break;
            }
        }

        protected void gridSchedule_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item
                || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
            {
                ScheduleMetaData schedule = e.Item.DataItem as ScheduleMetaData;

                Image imageCalendar = e.Item.FindControl("imageCalendar") as Image;

                RadButton buttonEdit = e.Item.FindControl("buttonEdit") as RadButton;
                buttonEdit.CommandArgument = schedule.UniqueId.ToString();
                buttonEdit.Visible = PermissionModule.GrantPermission(base.User, base.Groups, Permission.EditSchedule);

                RadButton buttonDelete = e.Item.FindControl("buttonDelete") as RadButton;
                buttonDelete.CommandArgument = schedule.UniqueId.ToString();
                buttonDelete.Visible = PermissionModule.GrantPermission(base.User, base.Groups, Permission.DeleteSchedule);

                RadButton buttonPauseResume = e.Item.FindControl("buttonPauseResume") as RadButton;
                buttonPauseResume.CommandArgument = schedule.UniqueId.ToString();
                buttonPauseResume.Visible = PermissionModule.GrantPermission(base.User, base.Groups, Permission.PauseSchedule);

                Literal literalSchedule = e.Item.FindControl("literalSchedule") as Literal;
                Label labelName = e.Item.FindControl("labelName") as Label;
                Label labelUniqueId = e.Item.FindControl("labelUniqueId") as Label;
                Label labelDescription = e.Item.FindControl("labelDescription") as Label;
                Panel panelSchedule = e.Item.FindControl("panelSchedule") as Panel;
                Panel panelPackage = e.Item.FindControl("panelPackage") as Panel;
                HyperLink hyperLinkChangeThat = e.Item.FindControl("hyperLinkChangeThat") as HyperLink;
               
                string result = schedule.GetPrimaryScheduleText();
                result += schedule.GetAnticipatedScheduleText();

                literalSchedule.Text = result;

                buttonPauseResume.ToolTip = "Pause schedule";
                buttonPauseResume.Image.ImageUrl = "/_layouts/Images/Vmgr/pause-icon-16.png";

                imageCalendar.ToolTip = "Schedule resumed";
                imageCalendar.ImageUrl = "/_layouts/Images/Vmgr/nav-schedule-enabled-32.png";

                if (schedule.Deactivated)
                {
                    buttonPauseResume.ToolTip = "Resume schedule";
                    buttonPauseResume.Image.ImageUrl = "/_layouts/Images/Vmgr/play-icon-16.png";

                    imageCalendar.ToolTip = "Schedule paused";
                    imageCalendar.ImageUrl = "/_layouts/Images/Vmgr/nav-schedule-disabled-32.png";

                    labelName.ForeColor = System.Drawing.Color.LightGray;
                    labelUniqueId.ForeColor = System.Drawing.Color.LightGray;
                    labelDescription.ForeColor = System.Drawing.Color.LightGray;

                    literalSchedule.Text = "<span style=\"color: gray;\">" + result + "</span>";
                }

                _scheduleScript += string.Format("scheduleFunctionArray.add('{0}', function (data) {{", schedule.UniqueId);
                _scheduleScript += string.Format("data = $telerik.$.parseJSON(JSON.stringify(data));");
                _scheduleScript += string.Format("var imageCalendar = $('#{0}');", imageCalendar.ClientID);
                _scheduleScript += string.Format("var panelSchedule = $('#{0}');", panelSchedule.ClientID);
                _scheduleScript += string.Format("imageCalendar.attr('src', '/_layouts/images/Vmgr/nav-schedule-enabled-32.png');");
                _scheduleScript += string.Format("if (data.IsRunning) {{");
                _scheduleScript += string.Format("imageCalendar.attr('src', '/_layouts/images/Vmgr/nav-schedule-active-32.png');");
                _scheduleScript += string.Format("}}");
                _scheduleScript += string.Format("else {{");
                _scheduleScript += string.Format("panelSchedule.html(data.PrimaryScheduleText + data.AnticipatedScheduleText);");
                _scheduleScript += string.Format("}}");
                _scheduleScript += string.Format("}}");
                _scheduleScript += string.Format(")");
                _scheduleScript += string.Format(";");

                panelPackage.Visible = this.plugins
                    .Where(p => p.PluginId == schedule.PluginId)
                    .Where(p => p.PackageDeactivated)
                    .FirstOrDefault() != null;

                hyperLinkChangeThat.NavigateUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)
                    + SPContext.Current.Web.ServerRelativeUrl + "/Plugins/PackageManager.aspx";

            }

            if (e.Item is GridPagerItem)
            {
                RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

                PageSizeCombo.Items.Clear();
                PageSizeCombo.Items.Add(new RadComboBoxItem("5"));
                PageSizeCombo.FindItemByText("5").Attributes.Add("ownerTableViewId", this.gridSchedule.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("10"));
                PageSizeCombo.FindItemByText("10").Attributes.Add("ownerTableViewId", this.gridSchedule.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("20"));
                PageSizeCombo.FindItemByText("20").Attributes.Add("ownerTableViewId", this.gridSchedule.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("50"));
                PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", this.gridSchedule.MasterTableView.ClientID);
                PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
            } 
        }

        protected void gridSchedule_PreRender(object sender, EventArgs e)
        {
            this.ajaxPanelSchedule.ResponseScripts.Add(this._scheduleScript);
        }

        protected void linqDataSourceSchedule_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.schedules;
        }

        protected void treeViewOptions_Load(object sender, EventArgs e)
        {
            RadTreeView treeView = sender as RadTreeView;

            RadTreeNode node = treeView.Nodes.Cast<RadTreeNode>()
                .Where(t => t.Value == "EDIT_SCHEDULE")
                .FirstOrDefault()
                ;

            if (node != null)
            {
                node.Visible = PermissionModule.GrantPermission(base.User, base.Groups, Permission.EditSchedule);
            }
        }

        protected void gridSchedule_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            this.pageIndex = e.NewPageIndex;
        }

        protected void gridSchedule_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {
            this.pageSize = e.NewPageSize;

            if (this.pageIndex.HasValue)
            {
                if (this.pageIndex >= this.gridSchedule.PageCount - 1)
                {
                    this.pageIndex = (this.gridSchedule.PageCount - 1);
                }
            }
        }

        #endregion
    }
}