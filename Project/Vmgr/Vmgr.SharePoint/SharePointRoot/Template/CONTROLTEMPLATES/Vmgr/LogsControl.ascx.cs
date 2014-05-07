using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vmgr.Data.Biz.MetaData;
using Telerik.Web.UI;
using Vmgr.Data.Biz;
using System.Security.Principal;
using Vmgr.Scheduling;
using Telerik.Web.UI.Calendar;
using Vmgr.Messaging;
using System.ServiceModel;
using Vmgr.Operations;

namespace Vmgr.SharePoint
{
    public partial class LogsControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        protected AppService _app = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected AppService app
        {
            get
            {
                if (this._app == null)
                {
                    this._app = new AppService();
                }
                return this._app;
            }

        }

        protected string getAppDomainsServiceUrl
        {
            get
            {
                return "/_vti_bin/Vmgr/AppDomainsService.asmx/GetAppDomains";
            }
        }

        protected string getAppDomainPluginsServiceUrl
        {
            get
            {
                return "/_vti_bin/Vmgr/AppDomainsService.asmx/GetPluginsByAppDomain";
            }
        }

        protected string serverId
        {
            get
            {
                if (this.Request.Cookies["SELECTED_SERVER"] != null)
                    return this.Request.Cookies["SELECTED_SERVER"].Value;
                return Guid.Empty.ToString();
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        protected DateTime? filterCreateDate
        {
            get
            {
                if (this.Session["FILTER_CREATEDATE"] == null)
                    return null;
                else
                    return this.Session["FILTER_CREATEDATE"] as DateTime?;
            }
            set
            {
                if (this.Session["FILTER_CREATEDATE"] == null)
                    this.Session.Add("FILTER_CREATEDATE", value);
                else
                    this.Session["FILTER_CREATEDATE"] = value;
            }
        }

        #endregion

        #region PUBLIC METHODS

        #endregion

        #region EVENTS

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ScriptManager scriptManager = RadScriptManager.GetCurrent(this.Page);
            scriptManager.AsyncPostBackTimeout = 3600 * 30; // 15 minutes!!!

            this.gridAppDomains.DataSource = new List<AppDomainMetaData> { };
            this.gridAppDomains.DataBind();

            this.gridAppDomainPlugins.DataSource = new List<PluginMetaData> { };
            this.gridAppDomainPlugins.DataBind();
        }

        protected void ajaxPanelLogs_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
                    this.gridLogs.Rebind();
                    break;
            }
        }

        protected void datePickerCreateDate_SelectedDateChanged(object sender, SelectedDateChangedEventArgs e)
        {
            RadDatePicker datePicker = sender as RadDatePicker;

            GridColumn column = this.gridLogs.MasterTableView.GetColumnSafe("CreateDate");
            column.CurrentFilterFunction = GridKnownFunction.NoFilter;
            column.CurrentFilterValue = string.Empty;
            this.filterCreateDate = null;

            if (e.NewDate != null)
            {
                this.filterCreateDate = e.NewDate;
                //column.CurrentFilterFunction = GridKnownFunction.EqualTo;
                //column.CurrentFilterValue = this.filterCreateDate.Value.ToShortDateString();
            }

            this.gridLogs.Rebind();
        }

        protected void gridLogs_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item
                || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
            {
                LogMetaData log = e.Item.DataItem as LogMetaData;

                Image imageLevel = e.Item.FindControl("imageLevel") as Image;

                if (log != null)
                {
                    switch (log.Level)
                    {
                        case "INFO":
                            imageLevel.ImageUrl = "/_layouts/images/Vmgr/info-icon-16.png";
                            break;
                        case "WARN":
                            imageLevel.ImageUrl = "/_layouts/images/Vmgr/warning-icon-16.png";
                            break;
                        case "ERROR":
                            imageLevel.ImageUrl = "/_layouts/images/Vmgr/error-icon-16.png";
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected void gridLogs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            this.gridLogs.DataSource = app.GetLogs();

            IQueryable<LogMetaData> query = app.GetLogs();

            if (this.filterCreateDate.HasValue)
            {
                query = query
                    .Where(l => l.CreateDate > this.filterCreateDate)
                    .Where(l => l.CreateDate < this.filterCreateDate.Value.AddDays(1));

            }

            int count = 0;

            if (!string.IsNullOrEmpty(this.gridLogs.MasterTableView.FilterExpression))
            {
                count = query
                    .Where(this.gridLogs.MasterTableView.FilterExpression)
                    .Count();
            }
            else
                count = query
                    .Count();


            this.gridLogs.DataSource = query;
            this.gridLogs.MasterTableView.VirtualItemCount = count;
            this.gridLogs.VirtualItemCount = count;
        }



        #endregion
    }
}