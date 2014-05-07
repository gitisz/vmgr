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

namespace Vmgr.SharePoint
{
    public partial class TriggerHistoryControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private JobMetaData _job = null;
        private IList<TriggerMetaData> _triggers = null;
     
        #endregion

        #region PROTECTED PROPERTIES

        protected int jobId
        {
            get
            {
                int result = 0;

                if (this.Request.QueryString["JobId"] != null)
                    int.TryParse(this.Request.QueryString["JobId"], out result);

                return result;
            }
        }

        protected JobMetaData job
        {
            get
            {
                if (this._job == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._job = app.GetJob(this.jobId);
                    }
                }

                return this._job;
            }
        }

        protected IList<TriggerMetaData> triggers
        {
            get
            {
                if (this._triggers == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._triggers = app.GetTriggers()
                            .Where(t => t.JobId == this.jobId)
                            .OrderByDescending(j => j.TriggerId)
                            .ToList()
                            ;
                    }
                }

                return this._triggers;
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

        #endregion

        #region PUBLIC METHODS

        #endregion

        #region EVENTS

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.labelScheduleName.Text = job.ScheduleName;
            this.labelScheduleUniqueId.Text = "{" + job.ScheduleUniqueId.ToString() + "}";

            this.Page.Title = String.Format("Trigger History - Job: {0}", job.JobKey);
        }

        protected void ajaxPanelTriggerHistory_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
                    this.gridTriggerHistory.Rebind();
                    break;
            }
        }

        protected void gridTriggerHistory_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item
                || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
            {
                TriggerMetaData trigger = e.Item.DataItem as TriggerMetaData;

                Image imageTriggerStatusType = e.Item.FindControl("imageTriggerStatusType") as Image;

                switch ((TriggerStatusType)trigger.TriggerStatusTypeId)
                {
                    case TriggerStatusType.Unknown:
                        imageTriggerStatusType.ToolTip = "Unknown";
                        imageTriggerStatusType.ImageUrl = "/_layouts/images/Vmgr/actions-view-calendar-day-icon-16.png";
                        break;
                    case TriggerStatusType.Started:
                        imageTriggerStatusType.ToolTip = "Started";
                        imageTriggerStatusType.ImageUrl = "/_layouts/images/Vmgr/hourglass-select-icon-16.png";
                        break;
                    case TriggerStatusType.Completed:
                        imageTriggerStatusType.ToolTip = "Completed";
                        imageTriggerStatusType.ImageUrl = "/_layouts/images/Vmgr/green-check-16.png";
                        break;
                    case TriggerStatusType.Misfired:
                        imageTriggerStatusType.ToolTip = "Misfired";
                        imageTriggerStatusType.ImageUrl = "/_layouts/images/Vmgr/actions-view-calendar-day-icon-misfire-16.png";
                        break;
                    case TriggerStatusType.Vetoed:
                        imageTriggerStatusType.ToolTip = "Vetoed";
                        imageTriggerStatusType.ImageUrl = "/_layouts/images/Vmgr/thumb-down-icon-16.png";
                        break;
                    default:
                        break;
                }

            }
        }

        protected void linqDataSourceTriggerHistory_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.triggers;
        }

        #endregion
    }
}