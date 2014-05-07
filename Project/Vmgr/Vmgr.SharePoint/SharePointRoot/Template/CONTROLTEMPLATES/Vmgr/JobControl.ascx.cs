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
using Vmgr.Scheduling;

namespace Vmgr.SharePoint
{
    public partial class JobControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<JobMetaData> _jobs = null;
        private IList<FilterMetaData> _filters = null;
        private string _jobScript = string.Empty;

        #endregion

        #region PROTECTED PROPERTIES

        protected int? pageIndex
        {
            get
            {
                if (this.Session["JOB_PAGE_INDEX"] == null)
                    return null;
                else
                    return this.Session["JOB_PAGE_INDEX"] as int?;
            }
            set
            {
                if (this.Session["JOB_PAGE_INDEX"] == null)
                    this.Session.Add("JOB_PAGE_INDEX", value);
                else
                    this.Session["JOB_PAGE_INDEX"] = value;
            }
        }

        protected int? pageSize
        {
            get
            {
                if (this.Session["JOB_PAGE_SIZE"] == null)
                    return null;
                else
                    return this.Session["JOB_PAGE_SIZE"] as int?;
            }
            set
            {
                if (this.Session["JOB_PAGE_SIZE"] == null)
                    this.Session.Add("JOB_PAGE_SIZE", value);
                else
                    this.Session["JOB_PAGE_SIZE"] = value;
            }
        }

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
                            .Where(p => p.FilterType == FilterType.Job.ToString())
                            .Where(m => m.Username == WindowsIdentity.GetCurrent().Name)
                            .ToList()
                            ;
                    }

                }

                return this._filters;
            }
        }
		
        protected IList<JobMetaData> jobs
        {
            get
            {
                if (this._jobs == null)
                {
                    using (AppService app = new AppService())
                    {
                        var query = app.GetJobs();
							
                        if (this.Request.Cookies["JOB_FILTER_DLINQ"] != null)
                        {
                            string jobFilterExpression = this.Request.Cookies["JOB_FILTER_DLINQ"].Value;

                            if (!string.IsNullOrEmpty(jobFilterExpression))
                            {
                                query = query
                                    .Where(jobFilterExpression);
                            }
                        }

                        this._jobs = query
                            .OrderByDescending(j => j.JobId)
                            .ToList()
                            ;
                    }
					
					
                }
                return this._jobs;
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
            HttpCookie cookieJobFilterDLinq = new HttpCookie("JOB_FILTER_DLINQ");
            HttpCookie cookieJobFilterExpression = new HttpCookie("JOB_FILTER_EXPRESSION");
            HttpCookie cookieJobFilterPreview = new HttpCookie("JOB_FILTER_PREVIEW");

            if (this.Request.Cookies["JOB_FILTER_DLINQ"] != null)
                cookieJobFilterDLinq = Request.Cookies["JOB_FILTER_DLINQ"];

            if (this.Request.Cookies["JOB_FILTER_EXPRESSION"] != null)
                cookieJobFilterExpression = Request.Cookies["JOB_FILTER_EXPRESSION"];

            if (this.Request.Cookies["JOB_FILTER_PREVIEW"] != null)
                cookieJobFilterPreview = Request.Cookies["JOB_FILTER_PREVIEW"];

            FilterMetaData myFilter = this.filters
                .Where(f => f.FilterId == filterId)
                .FirstOrDefault()
                ;

            RadFilter filter = new RadFilter();
            FilterJobControl.PopulateFilterFields(filter);

            this.buttonClearFilter.Visible = true;

            if (myFilter != null)
            {
                filter.LoadSettings(myFilter.Expression);

                string dlinq = FilterPackageControl.DecodeLinqFilterExpression(filter.RootGroup);
                string preview = FilterPackageControl.DecodePreviewFilterExpression(filter.RootGroup, filter);
                string expression = filter.SaveSettings();

                cookieJobFilterExpression.Value = expression;
                cookieJobFilterExpression.Expires = DateTime.Now.AddYears(1);

                cookieJobFilterPreview.Value = preview;
                cookieJobFilterPreview.Expires = DateTime.Now.AddYears(1);
                
                cookieJobFilterDLinq.Value = dlinq;
                cookieJobFilterDLinq.Expires = DateTime.Now.AddYears(1);

                this.literalJobFilterExpression.Text = preview;

                this.UpdateNodeSelection(expression);
            }
            else
            {
                if (filterId.HasValue)
                {
                    filter.LoadSettings(FilterJobControl.JOB_UNDO_NEW_FILTER);
                    cookieJobFilterExpression.Value = FilterJobControl.JOB_UNDO_NEW_FILTER;
                    cookieJobFilterExpression.Expires = DateTime.Now.AddYears(-1);

                    cookieJobFilterPreview.Value = string.Empty;
                    cookieJobFilterPreview.Expires = DateTime.Now.AddYears(-1);

                    cookieJobFilterDLinq.Value = string.Empty;
                    cookieJobFilterDLinq.Expires = DateTime.Now.AddYears(-1);
                    
                    this.buttonClearFilter.Visible = false;
                    this.literalJobFilterExpression.Text = string.Empty;
                    this.treeViewOptions.UnselectAllNodes();
                }
                else
                {
                    if (!string.IsNullOrEmpty(cookieJobFilterExpression.Value))
                    {
                        filter.LoadSettings(cookieJobFilterExpression.Value);
                        this.literalJobFilterExpression.Text = cookieJobFilterPreview.Value;
                        this.UpdateNodeSelection(cookieJobFilterExpression.Value);
                    }
                    else
                    {
                        filter.LoadSettings(FilterJobControl.JOB_UNDO_NEW_FILTER);
                        this.buttonClearFilter.Visible = false;
                        this.literalJobFilterExpression.Text = string.Empty;
                        this.treeViewOptions.UnselectAllNodes();
                    }
                }
            }

            // Add the cookie.
            this.Response.Cookies.Add(cookieJobFilterDLinq);
            this.Response.Cookies.Add(cookieJobFilterExpression);
            this.Response.Cookies.Add(cookieJobFilterPreview);

            this.gridJob.Rebind();
        }
        private void SetOptionsCookie(bool expanded)
        {
            HttpCookie cookieOpen = new HttpCookie("JOB_OPTIONS");

            if (this.Request.Cookies["JOB_OPTIONS"] != null)
                cookieOpen = Request.Cookies["JOB_OPTIONS"];

            if (!string.IsNullOrEmpty(cookieOpen.Value))
                expanded = (!bool.Parse(cookieOpen.Value));

            cookieOpen.Value = expanded.ToString();
            cookieOpen.Expires = DateTime.Now.AddYears(1);

            // Add the cookie.
            this.Response.Cookies.Add(cookieOpen);

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

            if (this.Request.Cookies["JOB_OPTIONS"] != null)
            {
                bool expanded = bool.Parse(this.Request.Cookies["JOB_OPTIONS"].Value);

                this.optionsTd.Visible = expanded;
            }

            if (this.Request.Cookies["JOB_FILTER_PREVIEW"] != null)
            {
                string packageFilterPreview = this.Request.Cookies["JOB_FILTER_PREVIEW"].Value;
                this.literalJobFilterExpression.Text = packageFilterPreview;
            }

            string filterExp = string.Empty;

            if (this.Request.Cookies["JOB_FILTER_EXPRESSION"] != null)
            {
                filterExp = this.Request.Cookies["JOB_FILTER_EXPRESSION"].Value;

                if (filterExp != FilterJobControl.JOB_UNDO_NEW_FILTER)
                    this.buttonClearFilter.Visible = true;
            }

            if (!this.IsPostBack)
            {
                this.UpdateNodeSelection(filterExp);

                this.gridJob.PageSize = this.pageSize ?? 5;
                this.gridJob.CurrentPageIndex = this.pageIndex ?? 0;
            }
        }

        protected void ajaxPanelJob_AjaxRequest(object sender, AjaxRequestEventArgs e)
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

                    this.gridJob.Rebind();
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

        protected void gridJob_ItemCreated(object sender, GridItemEventArgs e)
        {
        }

        protected void gridJob_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item
                || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
            {
                JobMetaData job = e.Item.DataItem as JobMetaData;

                Image imageJobStatusType = e.Item.FindControl("imageJobStatusType") as Image;
                Image imageCalendar = e.Item.FindControl("imageCalendar") as Image;
                RadButton buttonJobKey = e.Item.FindControl("buttonJobKey") as RadButton;

                buttonJobKey.Text = job.JobKey;
                buttonJobKey.CommandArgument = job.JobId.ToString();

                switch ((JobStatusType)job.JobStatusTypeId)
                {
                    case JobStatusType.Scheduled:
                        imageJobStatusType.ToolTip = "Scheduled";
                        imageJobStatusType.ImageUrl = "/_layouts/images/Vmgr/actions-view-calendar-day-icon-16.png";
                        break;
                    case JobStatusType.Recurrence:
                        imageJobStatusType.ToolTip = "Recurrence";
                        imageJobStatusType.ImageUrl = "/_layouts/images/Vmgr/recurrence-icon-16.png";
                        break;
                    case JobStatusType.InProgress:
                        imageJobStatusType.ToolTip = "InProgress";
                        imageJobStatusType.ImageUrl = "/_layouts/images/Vmgr/hourglass-select-icon-16.png";
                        break;
                    case JobStatusType.Succeeded:
                        imageJobStatusType.ToolTip = "Succeeded";
                        imageJobStatusType.ImageUrl = "/_layouts/images/Vmgr/green-check-16.png";
                        break;
                    case JobStatusType.Failed:
                        imageJobStatusType.ToolTip = "Failed";
                        imageJobStatusType.ImageUrl = "/_layouts/images/Vmgr/actions-view-calendar-day-icon-misfire-16.png";
                        break;
                    case JobStatusType.Deleted:
                        imageJobStatusType.ToolTip = "Removed";
                        imageJobStatusType.ImageUrl = "/_layouts/images/Vmgr/stop-icon-16.png";
                        break;
                }

                _jobScript += string.Format("jobFunctionArray.add('{0}', function (data) {{", job.JobKey);
                _jobScript += string.Format("data = $telerik.$.parseJSON(JSON.stringify(data));");
                _jobScript += string.Format("var imageJobStatusType = $('#{0}');", imageJobStatusType.ClientID);
                _jobScript += string.Format("if (data.JobStatusType == 1) {{");
                _jobScript += string.Format("imageJobStatusType.attr('src', '/_layouts/images/Vmgr/actions-view-calendar-day-icon-16.png');");
                _jobScript += string.Format("imageJobStatusType.attr('alt', 'Jobd');");
                _jobScript += string.Format("imageJobStatusType.attr('title', 'Jobd');");
                _jobScript += string.Format("}}");
                _jobScript += string.Format("else if (data.JobStatusType == 2) {{");
                _jobScript += string.Format("imageJobStatusType.attr('src', '/_layouts/images/Vmgr/recurrence-icon-16.png');");
                _jobScript += string.Format("imageJobStatusType.attr('alt', 'Recurrence');");
                _jobScript += string.Format("imageJobStatusType.attr('title', 'Recurrence');");
                _jobScript += string.Format("}}");
                _jobScript += string.Format("else if (data.JobStatusType == 3) {{");
                _jobScript += string.Format("imageJobStatusType.attr('src', '/_layouts/images/Vmgr/hourglass-select-icon-16.png');");
                _jobScript += string.Format("imageJobStatusType.attr('alt', 'Total elapsed: ' + data.ElapsedTime);");
                _jobScript += string.Format("imageJobStatusType.attr('title', 'Total elapsed: ' + data.ElapsedTime);");
                _jobScript += string.Format("}}");
                _jobScript += string.Format("else if (data.JobStatusType == 4) {{");
                _jobScript += string.Format("imageJobStatusType.attr('src', '/_layouts/images/Vmgr/green-check-16.png');");
                _jobScript += string.Format("imageJobStatusType.attr('alt', 'Succeeded');");
                _jobScript += string.Format("imageJobStatusType.attr('title', 'Succeeded');");
                _jobScript += string.Format("}}");
                _jobScript += string.Format("else if (data.JobStatusType == 5) {{");
                _jobScript += string.Format("imageJobStatusType.attr('src', '/_layouts/images/Vmgr/actions-view-calendar-day-icon-misfire-16.png');");
                _jobScript += string.Format("imageJobStatusType.attr('alt', 'Failed');");
                _jobScript += string.Format("imageJobStatusType.attr('title', 'Failed');");
                _jobScript += string.Format("}}");
                _jobScript += string.Format("else if (data.JobStatusType == 6) {{");
                _jobScript += string.Format("imageJobStatusType.attr('src', '/_layouts/images/Vmgr/stop-icon-16.png');");
                _jobScript += string.Format("imageJobStatusType.attr('alt', 'Removed');");
                _jobScript += string.Format("imageJobStatusType.attr('title', 'Removed');");
                _jobScript += string.Format("}}");
                _jobScript += string.Format("}}");
                _jobScript += string.Format(")");
                _jobScript += string.Format(";");

                _jobScript += string.Format("scheduleFunctionArray.add('{0}', function (data) {{", job.ScheduleUniqueId);
                _jobScript += string.Format("var imageCalendar = $('#{0}');", imageCalendar.ClientID);
                _jobScript += string.Format("imageCalendar.attr('src', '/_layouts/images/Vmgr/nav-schedule-enabled-32.png');");
                _jobScript += string.Format("if (data.IsRunning) {{");
                _jobScript += string.Format("imageCalendar.attr('src', '/_layouts/images/Vmgr/nav-schedule-active-32.png');");
                _jobScript += string.Format("}}");
                _jobScript += string.Format("else {{");
                _jobScript += string.Format("}}");
                _jobScript += string.Format("}}");
                _jobScript += string.Format(")");
                _jobScript += string.Format(";");

            }

            if (e.Item is GridPagerItem)
            {
                RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

                PageSizeCombo.Items.Clear();
                PageSizeCombo.Items.Add(new RadComboBoxItem("5"));
                PageSizeCombo.FindItemByText("5").Attributes.Add("ownerTableViewId", this.gridJob.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("10"));
                PageSizeCombo.FindItemByText("10").Attributes.Add("ownerTableViewId", this.gridJob.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("20"));
                PageSizeCombo.FindItemByText("20").Attributes.Add("ownerTableViewId", this.gridJob.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("50"));
                PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", this.gridJob.MasterTableView.ClientID);
                PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
            }
        }

        protected void gridJob_PreRender(object sender, EventArgs e)
        {
            this.ajaxPanelJob.ResponseScripts.Add(this._jobScript);
        }

        protected void gridJob_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            this.pageIndex = e.NewPageIndex;
        }

        protected void gridJob_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {
            this.pageSize = e.NewPageSize;

            if (this.pageIndex.HasValue)
            {
                if (this.pageIndex >= this.gridJob.PageCount - 1)
                {
                    this.pageIndex = (this.gridJob.PageCount - 1);
                }
            }
        }

        protected void linqDataSourceJob_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.jobs
                .GroupBy(j => j.JobKeyGroup)
                .Select(j => new { K = j.Key, V = j.OrderByDescending(g => g.JobId).Take(1) })
                .SelectMany(j => j.V)
                .ToList()
                ;
        }

        #endregion
    }
}