using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.SharePoint.Enumerations;

namespace Vmgr.SharePoint
{
    public partial class FilterJobControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<FilterMetaData> _filters = null;

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
                            .Where(p => p.FilterType == FilterType.Job.ToString())
                            .Where(m => m.Username == WindowsIdentity.GetCurrent().Name)
                            .ToList()
                            ;
                    }

                }

                return this._filters;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void Apply()
        {
            this.filter.FireApplyCommand();
        }

        private void DeleteFilter(int filterId)
        {
            using (AppService app = new AppService())
            {
                app.RemoveFilter(filterId.ToNullable<int>() ?? 0);
            }

            this.filter.LoadSettings(JOB_UNDO_NEW_FILTER);
            this.textBoxName.Text = string.Empty;
            this.labelSelectedFilter.Text = string.Empty;

            this.ajaxPanelFilter.ResponseScripts.Add(string.Format("OnDeleteComplete()"));
        }

        private void EditName()
        {
            this.textBoxName.Visible = true;
            this.labelSelectedFilter.Visible = false;
            this.buttonEditName.Visible = false;
        }

        private void NewFilter()
        {
            this.textBoxName.Visible = true;
            this.textBoxName.Text = string.Empty;
            this.labelSelectedFilter.Visible = false;
            this.buttonEditName.Visible = false;
            this.filter.LoadSettings(JOB_UNDO_NEW_FILTER);
            this.treeViewOptions.UnselectAllNodes();
        }

        private void Save()
        {
            FilterMetaData myFilter = new FilterMetaData { };

            if (this.treeViewOptions.SelectedNode != null)
            {
                myFilter = this.filters
                    .Where(f => f.FilterId == int.Parse(this.treeViewOptions.SelectedNode.Value))
                    .FirstOrDefault()
                    ;
            }

            myFilter.ServerId = (this.Page as BasePage).server.ServerId;
            myFilter.Name = this.textBoxName.Text.Trim();
            myFilter.FilterType = FilterType.Job.ToString();
            myFilter.Deactivated = false;
            myFilter.Expression = this.filter.SaveSettings();
            myFilter.Username = WindowsIdentity.GetCurrent().Name;

            using (AppService app = new AppService())
            {
                if (app.Save(myFilter))
                {
                    this.ajaxPanelFilter.ResponseScripts.Add(string.Format("OnSaveComplete({0})", myFilter.FilterId));
                }
                else
                {
                    Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);

                    (this.Page as BasePage).Alert("The was a problem saving the filter.  Please check logs to determine the cause."
                        , "Failed to Save Filter"
                        , null
                        , AlertType.Error
                        )
                        ;
                }
            }
        }

        private void Undo()
        {
            if (this.treeViewOptions.SelectedNode != null)
            {
                FilterMetaData myFilter = this.filters
                    .Where(f => f.FilterId == int.Parse(this.treeViewOptions.SelectedNode.Value))
                    .FirstOrDefault()
                    ;

                this.filter.LoadSettings(myFilter.Expression);
                this.textBoxName.Text = myFilter.Name;
            }
            else
            {
                this.filter.LoadSettings(JOB_UNDO_NEW_FILTER);
                this.textBoxName.Text = string.Empty;
            }
        }

        private void Refresh(int filterId)
        {
            this._filters = null;
            this.treeViewOptions.DataBind();

            RadTreeNode selectedNode = this.treeViewOptions.Nodes.FindNodeByValue(filterId.ToString());

            if (selectedNode != null)
            {
                selectedNode.Selected = true;
                this.NodeSelected(filterId);
            }
            else
            {
                this.filter.LoadSettings(JOB_UNDO_NEW_FILTER);
                this.labelSelectedFilter.Text = string.Empty;
                this.labelSelectedFilter.Visible = false;
                this.textBoxName.Text = string.Empty;
                this.textBoxName.Visible = true;
                this.buttonEditName.Visible = false;
            }
        }

        private void NodeSelected(int filterId)
        {
            FilterMetaData myFilter = this.filters
                .Where(f => f.FilterId == filterId)
                .FirstOrDefault()
                ;

            this.filter.LoadSettings(myFilter.Expression);
            this.labelSelectedFilter.Text = myFilter.Name;
            this.labelSelectedFilter.Visible = true;
            this.textBoxName.Text = myFilter.Name;
            this.textBoxName.Visible = false;
            this.buttonEditName.Visible = true;
        }

        private static void ExpressionEvaluated(RadFilterEvaluationData evaluationData)
        {
            RadFilterFunction filterFunction = evaluationData.Expression.FilterFunction;
            
            if (evaluationData.Expression.FieldName == "ScheduleUniqueId" &&
                filterFunction == RadFilterFunction.EqualTo ||
                filterFunction == RadFilterFunction.NotEqualTo)
            {
                Guid value = new Guid(((IRadFilterValueExpression)evaluationData.Expression).Values[0].ToString());
                RadFilterDynamicLinqExpressionEvaluator evaluator = null;
                RadFilterNonGroupExpression expression = null;

                if (filterFunction == RadFilterFunction.EqualTo)
                {
                    evaluator = RadFilterDynamicLinqExpressionEvaluator.GetEvaluator(RadFilterFunction.EqualTo);
                    expression = new RadFilterEqualToFilterExpression<Guid>("ScheduleUniqueId")
                    {
                        Value = value,
                    };
                }
                else
                {
                    evaluator = RadFilterDynamicLinqExpressionEvaluator.GetEvaluator(RadFilterFunction.NotEqualTo);
                    expression = new RadFilterNotEqualToFilterExpression<Guid>("ScheduleUniqueId")
                    {
                        Value = value,
                    };
                }
                evaluator.GetEvaluationData(expression).CopyTo(evaluationData);
            }
        }
        
        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public const string JOB_UNDO_NEW_FILTER = "AAEAAAD/////AQAAAAAAAAAQAQAAAAMAAAANAgkCAAAADAMAAABXVGVsZXJpay5XZWIuVUksIFZlcnNpb249MjAxMy4xLjQwMy4zNSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj0xMjFmYWU3ODE2NWJhM2Q0BQIAAAAmVGVsZXJpay5XZWIuVUkuUmFkRmlsdGVyR3JvdXBPcGVyYXRpb24BAAAAB3ZhbHVlX18ACAMAAAAAAAAACw==";

        public static string DecodeLinqFilterExpression(RadFilterGroupExpression exp)
        {
            RadFilterDynamicLinqQueryProvider provider = new RadFilterDynamicLinqQueryProvider();
            provider.OnExpressionEvaluated = ExpressionEvaluated;
            provider.ProcessGroup(exp);

            return provider.Result;
        }

        public static string DecodePreviewFilterExpression(RadFilterGroupExpression exp, RadFilter filter)
        {
            RadFilterExpressionPreviewProvider provider = new RadFilterExpressionPreviewProvider(filter);
            provider.ProcessGroup(exp);
            
            return string.Format("<div class=\"rfPreview\">{0}</div>", provider.Result); 
        }

        public static void PopulateFilterFields(RadFilter filter)
        {
            filter.FieldEditors.Add(new RadFilterDateFieldEditor { DataType = typeof(System.DateTime), FieldName = "CreateDate", DisplayName = "Create Date" });
            filter.FieldEditors.Add(new RadFilterTextFieldEditor { DataType = typeof(System.String), FieldName = "CreateUser", DisplayName = "Create User" });
            filter.FieldEditors.Add(new RadFilterNumericFieldEditor { DataType = typeof(System.Int32), FieldName = "JobId", DisplayName = "Job ID" });

            filter.FieldEditors.Add(new RadFilterTextFieldEditor { DataType = typeof(System.String), FieldName = "JobKey", DisplayName = "Job Key" });
            filter.FieldEditors.Add(new RadFilterTextFieldEditor { DataType = typeof(System.String), FieldName = "JobKeyGroup", DisplayName = "Job Key Group" });
            filter.FieldEditors.Add(new RadFilterTextFieldEditor { DataType = typeof(System.String), FieldName = "ScheduleName", DisplayName = "Schedule Name" });
            filter.FieldEditors.Add(new RadFilterTextFieldEditor { DataType = typeof(System.Guid), FieldName = "ScheduleUniqueId", DisplayName = "Schedule Unique ID" });

            filter.FieldEditors.Add(new RadFilterDateFieldEditor { DataType = typeof(System.DateTime), FieldName = "UpdateDate", DisplayName = "Update Date" });
            filter.FieldEditors.Add(new RadFilterTextFieldEditor { DataType = typeof(System.String), FieldName = "UpdateUser", DisplayName = "Update User" });
        }
        
        #endregion

        #region EVENTS

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            FilterJobControl.PopulateFilterFields(this.filter);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!this.IsPostBack)
            {
                this.labelSelectedFilter.Visible = false;
                this.textBoxName.Visible = true;
                this.buttonEditName.Visible = false;
            }
        }

        protected void filter_ApplyExpressions(object sender, RadFilterApplyExpressionsEventArgs e)
        {
            string dlinq = FilterJobControl.DecodeLinqFilterExpression(e.ExpressionRoot);
            string preview = FilterJobControl.DecodePreviewFilterExpression(this.filter.RootGroup, filter);
            string expression = this.filter.SaveSettings();

            HttpCookie cookieJobFilterDLinq = new HttpCookie("JOB_FILTER_DLINQ");
            HttpCookie cookieJobFilterExpression = new HttpCookie("JOB_FILTER_EXPRESSION");
            HttpCookie cookieJobFilterPreview = new HttpCookie("JOB_FILTER_PREVIEW");

            if (this.Request.Cookies["JOB_FILTER_DLINQ"] != null)
                cookieJobFilterDLinq = Request.Cookies["JOB_FILTER_DLINQ"];

            if (this.Request.Cookies["JOB_FILTER_EXPRESSION"] != null)
                cookieJobFilterExpression = Request.Cookies["JOB_FILTER_EXPRESSION"];

            if (this.Request.Cookies["JOB_FILTER_PREVIEW"] != null)
                cookieJobFilterPreview = Request.Cookies["JOB_FILTER_PREVIEW"];

            cookieJobFilterDLinq.Value = dlinq;
            cookieJobFilterDLinq.Expires = DateTime.Now.AddYears(1);

            cookieJobFilterExpression.Value = expression;
            cookieJobFilterExpression.Expires = DateTime.Now.AddYears(1);

            cookieJobFilterPreview.Value = preview;
            cookieJobFilterPreview.Expires = DateTime.Now.AddYears(1);

            // Add the cookie.
            this.Response.Cookies.Add(cookieJobFilterDLinq);
            this.Response.Cookies.Add(cookieJobFilterExpression);
            this.Response.Cookies.Add(cookieJobFilterPreview);

            this.ajaxPanelFilter.ResponseScripts.Add(string.Format("OnFilterApplied('APPLY_FILTER,')"));
        }

        protected void ajaxPanelFilter_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
                case "SAVE":
                    this.Save();
                    break;
                case "APPLY":
                    this.Apply();
                    break;
                case "UNDO":
                    this.Undo();
                    break;
                case "EDIT":
                    this.EditName();
                    break;
                case "NEW":
                    this.NewFilter();
                    break;
                case "REFRESH":
                    this.Refresh(int.Parse(param));
                    break;
                case "DELETE":
                    this.DeleteFilter(int.Parse(param));
                    break;
            }
        }

        protected void linqDataSourceFilter_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (this.filters.Count == 0)
                e.Cancel = true;

            e.Result = this.filters;
        }

        protected void treeViewOptions_NodeDataBound(object sender, RadTreeNodeEventArgs e)
        {
            e.Node.ImageUrl = "/_layouts/images/Vmgr/user-blue-icon-16.png";
        }

        protected void treeViewOptions_NodeClick(object sender, RadTreeNodeEventArgs e)
        {
            this.NodeSelected(e.Node.Value.ToNullable<int>() ?? 0);
        }

        #endregion
    }
}