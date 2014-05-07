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
using Vmgr.Operations;
using System.Web.UI;

namespace Vmgr.SharePoint
{
    public partial class PackageManagerControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<PackageMetaData> _packages = null;
        private IList<PluginMetaData> _plugins = null;
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
                            .Where(p => p.FilterType == FilterType.Package.ToString())
                            .Where(m => m.Username == WindowsIdentity.GetCurrent().Name)
                            .ToList()
                            ;
                    }

                }

                return this._filters;
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

                        //  We don't require the binary data, this query should be less intensive.
                        var query = app.GetPackages()
                            .Select(p => new PackageMetaData
                            {
                                PackageId = p.PackageId,
                                CreateDate = p.CreateDate,
                                CreateUser = p.CreateUser,
                                Deactivated = p.Deactivated,
                                Description = p.Description,
                                Name = p.Name,
                                ServerId = p.ServerId,
                                ServerUniqueId = p.ServerUniqueId,
                                UniqueId = p.UniqueId,
                                UpdateDate = p.UpdateDate,
                                UpdateUser = p.UpdateUser,
                            }
                            )
                            .Where(p => p.ServerId == (this.Page as BasePage).server.ServerId)
                            ;

                        if (this.Request.Cookies["PACKAGE_FILTER_DLINQ"] != null)
                        {
                            string packageFilterExpression = this.Request.Cookies["PACKAGE_FILTER_DLINQ"].Value;

                            if (!string.IsNullOrEmpty(packageFilterExpression))
                            {
                                query = query
                                    .Where(packageFilterExpression);
                            }
                        }

                        this._packages = query
                            .ToList()
                            ;
                    }
                }

                return this._packages;
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
                if (this.Session["PACKAGEMGR_PAGE_INDEX"] == null)
                    return null;
                else
                    return this.Session["PACKAGEMGR_PAGE_INDEX"] as int?;
            }
            set
            {
                if (this.Session["PACKAGEMGR_PAGE_INDEX"] == null)
                    this.Session.Add("PACKAGEMGR_PAGE_INDEX", value);
                else
                    this.Session["PACKAGEMGR_PAGE_INDEX"] = value;
            }
        }

        protected int? pageSize
        {
            get
            {
                if (this.Session["PACKAGEMGR_PAGE_SIZE"] == null)
                    return null;
                else
                    return this.Session["PACKAGEMGR_PAGE_SIZE"] as int?;
            }
            set
            {
                if (this.Session["PACKAGEMGR_PAGE_SIZE"] == null)
                    this.Session.Add("PACKAGEMGR_PAGE_SIZE", value);
                else
                    this.Session["PACKAGEMGR_PAGE_SIZE"] = value;
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
            HttpCookie cookiePackageFilterDLinq = new HttpCookie("PACKAGE_FILTER_DLINQ");
            HttpCookie cookiePackageFilterExpression = new HttpCookie("PACKAGE_FILTER_EXPRESSION");
            HttpCookie cookiePackageFilterPreview = new HttpCookie("PACKAGE_FILTER_PREVIEW");

            if (this.Request.Cookies["PACKAGE_FILTER_DLINQ"] != null)
                cookiePackageFilterDLinq = Request.Cookies["PACKAGE_FILTER_DLINQ"];

            if (this.Request.Cookies["PACKAGE_FILTER_EXPRESSION"] != null)
                cookiePackageFilterExpression = Request.Cookies["PACKAGE_FILTER_EXPRESSION"];

            if (this.Request.Cookies["PACKAGE_FILTER_PREVIEW"] != null)
                cookiePackageFilterPreview = Request.Cookies["PACKAGE_FILTER_PREVIEW"];

            FilterMetaData myFilter = this.filters
                .Where(f => f.FilterId == filterId)
                .FirstOrDefault()
                ;

            RadFilter filter = new RadFilter();
            FilterPackageControl.PopulateFilterFields(filter);

            this.buttonClearFilter.Visible = true;

            if (myFilter != null)
            {
                filter.LoadSettings(myFilter.Expression);

                string dlinq = FilterPackageControl.DecodeLinqFilterExpression(filter.RootGroup);
                string preview = FilterPackageControl.DecodePreviewFilterExpression(filter.RootGroup, filter);
                string expression = filter.SaveSettings();

                cookiePackageFilterExpression.Value = expression;
                cookiePackageFilterExpression.Expires = DateTime.Now.AddYears(1);

                cookiePackageFilterPreview.Value = preview;
                cookiePackageFilterPreview.Expires = DateTime.Now.AddYears(1);
                
                cookiePackageFilterDLinq.Value = dlinq;
                cookiePackageFilterDLinq.Expires = DateTime.Now.AddYears(1);

                this.literalPackageFilterExpression.Text = preview;

                this.UpdateNodeSelection(expression);
            }
            else
            {
                if (filterId.HasValue)
                {
                    filter.LoadSettings(FilterPackageControl.PACKAGE_UNDO_NEW_FILTER);
                    cookiePackageFilterExpression.Value = FilterPackageControl.PACKAGE_UNDO_NEW_FILTER;
                    cookiePackageFilterExpression.Expires = DateTime.Now.AddYears(-1);

                    cookiePackageFilterPreview.Value = string.Empty;
                    cookiePackageFilterPreview.Expires = DateTime.Now.AddYears(-1);

                    cookiePackageFilterDLinq.Value = string.Empty;
                    cookiePackageFilterDLinq.Expires = DateTime.Now.AddYears(-1);
                    
                    this.buttonClearFilter.Visible = false;
                    this.literalPackageFilterExpression.Text = string.Empty;
                    this.treeViewOptions.UnselectAllNodes();
                }
                else
                {
                    if (!string.IsNullOrEmpty(cookiePackageFilterExpression.Value))
                    {
                        filter.LoadSettings(cookiePackageFilterExpression.Value);
                        this.literalPackageFilterExpression.Text = cookiePackageFilterPreview.Value;
                        this.UpdateNodeSelection(cookiePackageFilterExpression.Value);
                    }
                    else
                    {
                        filter.LoadSettings(FilterPackageControl.PACKAGE_UNDO_NEW_FILTER);
                        this.buttonClearFilter.Visible = false;
                        this.literalPackageFilterExpression.Text = string.Empty;
                        this.treeViewOptions.UnselectAllNodes();
                    }
                }
            }

            // Add the cookie.
            this.Response.Cookies.Add(cookiePackageFilterDLinq);
            this.Response.Cookies.Add(cookiePackageFilterExpression);
            this.Response.Cookies.Add(cookiePackageFilterPreview);

            this.gridPackage.Rebind();
        }

        private void ConfirmPauseResumePackage(string uniqueId)
        {
            using (AppService app = new AppService())
            {
                PackageMetaData package = app.GetPackages()
                    .Where(m => m.UniqueId == new Guid(uniqueId))
                    .FirstOrDefault()
                    ;

                if (package == null)
                    throw new ApplicationException("Package must not be undefined.");

                string message = string.Format("Are you sure you want to pause the package <span style=\"font-weight: bold;\">{0}</span>?<br /><br />", package.Name);

                if (package.Deactivated)
                    message = string.Format("Are you sure you want to resume the package <span style=\"font-weight: bold;\">{0}</span>?<br /><p style=\"font-style: italic; color: orange;\";>Any existing schedules for plugins within this package will resume!</p>", package.Name);

                (this.Page as BasePage).Confirm(message
                    , string.Format("{0} Package", package.Deactivated ? "Resume" : "Pause")
                    , "OnConfirmPauseResumePackageHandler"
                    , Telerik.Charting.Styles.Unit.Pixel(500)
                    , Telerik.Charting.Styles.Unit.Pixel(200)
                    , Enumerations.ConfirmType.Confirm
                    );

            }
        }

        private void ConfirmDeletePackage(string uniqueId)
        {
            using (AppService app = new AppService())
            {
                PackageMetaData package = app.GetPackages()
                    .Where(m => m.UniqueId == new Guid(uniqueId))
                    .FirstOrDefault()
                    ;

                if (package == null)
                    throw new ApplicationException("Package must not be undefined.");

                string message = string.Format("Are you sure you want to permanently delete the package <span style=\"font-weight: bold;\">{0}</span>?<br /><p style=\"font-style: italic; color: orange;\";>All schedules and job history will also be deleted for this package!</p>", package.Name);

                (this.Page as BasePage).Confirm(message
                    , "Delete Package"
                    , "OnConfirmDeletePackageHandler"
                    , Telerik.Charting.Styles.Unit.Pixel(500)
                    , Telerik.Charting.Styles.Unit.Pixel(200)
                    , Enumerations.ConfirmType.Confirm
                    );
            }
        }

        private void PauseResumePackage(string uniqueId)
        {
            IList<string> messages = new List<string> { };

            PackageMetaData package = null;

            using (AppService app = new AppService())
            {
                package = app.GetPackages()
                    .Where(m => m.UniqueId == new Guid(uniqueId))
                    .FirstOrDefault()
                    ;

                if (package == null)
                    throw new ApplicationException("Package must not be undefined.");
            }

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

                ChannelFactory<IPackageOperation> httpFactory = new ChannelFactory<IPackageOperation>(binding
                    , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/PackageOperation"
                    , (this.Page as BasePage).server.WSProtocol
                    , (this.Page as BasePage).server.WSFqdn
                    , (this.Page as BasePage).server.WSPort
                    )
                    )
                    )
                    ;
                IPackageOperation packageOperationProxy = httpFactory.CreateChannel();

                if (package.Deactivated)
                    packageOperationProxy.LoadPackage(package);
                else
                    packageOperationProxy.UnloadPackage(package);
            }
            catch (EndpointNotFoundException ex)
            {
                Logger.Logs.Log("Unable to load/unload package from source server.  The service does not appear to be online.", ex, LogType.Error);
                messages.Add("Unable to load/unload package from source server.  The service does not appear to be online.");
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Unable to load/unload package from source server.", ex, LogType.Error);
                messages.Add("Unable to load/unload package from source server.");
            }

            package.Deactivated = !package.Deactivated;

            using (AppService app = new AppService())
            {
                if (!app.Save(package))
                {
                    Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);

                    (this.Page as BasePage).Alert(string.Format("The package failed to be {0}, please check the log for detials.", package.Deactivated ? "paused" : "resumed")
                        , string.Format("{0} Package Failure", package.Deactivated ? "Pause" : "Resume")
                        , null
                        , Telerik.Charting.Styles.Unit.Pixel(500)
                        , Telerik.Charting.Styles.Unit.Pixel(200)
                        , AlertType.Error
                        )
                        ;
                }
                else
                {
                    string message = string.Join("</li><li>", messages.ToArray());

                    if (string.IsNullOrEmpty(message))
                    {
                        (this.Page as BasePage).Alert(string.Format("The package was successfullly {0}.", package.Deactivated ? "paused" : "resumed")
                            , string.Format("{0} Package Success", package.Deactivated ? "Pause" : "Resume")
                            , "OnPauseResumePackageComplete"
                            , AlertType.Check
                            )
                            ;
                    }
                    else
                    {
                        (this.Page as BasePage).Alert(string.Format("The package was {0}, however please review the following messages: <br /> <ul><li>{1}</li></ul>", package.Deactivated ? "paused" : "resumed", message)
                            , string.Format("{0} Package Warning", package.Deactivated ? "Pause" : "Resume")
                            , "OnPauseResumePackageComplete"
                            , Telerik.Charting.Styles.Unit.Pixel(550)
                            , Telerik.Charting.Styles.Unit.Pixel(200)
                            , AlertType.Error);
                    }
                }
            }

            this._packages = null;
            this.gridPackage.DataBind();
        }

        private void SetOptionsCookie(bool expanded)
        {
            HttpCookie cookieOpen = new HttpCookie("PACKAGE_OPTIONS");

            if (this.Request.Cookies["PACKAGE_OPTIONS"] != null)
                cookieOpen = Request.Cookies["PACKAGE_OPTIONS"];

            if (!string.IsNullOrEmpty(cookieOpen.Value))
                expanded = (!bool.Parse(cookieOpen.Value));

            cookieOpen.Value = expanded.ToString();
            cookieOpen.Expires = DateTime.Now.AddYears(1);

            // Add the cookie.
            this.Response.Cookies.Add(cookieOpen);

        }

        private void DeletePackage(string uniqueId)
        {
            IList<string> messages = new List<string> { };

            PackageMetaData package = null;

            try
            {
                using (AppService app = new AppService())
                {
                    package = app.GetPackages()
                        .Where(m => m.UniqueId == new Guid(uniqueId))
                        .FirstOrDefault()
                        ;

                    if (package == null)
                        throw new ApplicationException("Package must not be undefined.");
                }

                BasicHttpBinding binding = new BasicHttpBinding();

                if ((this.Page as BasePage).server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    binding.Security.Mode = BasicHttpSecurityMode.None;
                }

                if ((this.Page as BasePage).server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                }

                ChannelFactory<IPackageOperation> httpFactory = new ChannelFactory<IPackageOperation>(binding
                    , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/PackageOperation"
                    , (this.Page as BasePage).server.WSProtocol
                    , (this.Page as BasePage).server.WSFqdn
                    , (this.Page as BasePage).server.WSPort
                    )
                    )
                    )
                    ;
                IPackageOperation packageOperationProxy = httpFactory.CreateChannel();
                packageOperationProxy.UnloadPackage(package);
            }
            catch (EndpointNotFoundException ex)
            {
                Logger.Logs.Log("Unable to unload package from source server.  The service does not appear to be online.", ex, LogType.Error);
                messages.Add("Unable to unload package from source server.  The service does not appear to be online.");
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Unable to unload package from source server.", ex, LogType.Error);
                messages.Add("Unable to unload package from source server.");
            }

            string message = string.Join("</li><li>", messages.ToArray());

            using (AppService app = new AppService())
            {
                try
                {
                    app.RemovePackage(package.PackageId);

                    if (string.IsNullOrEmpty(message))
                    {
                        (this.Page as BasePage).Alert("The package was successfullly deleted."
                            , "Delete Package Success"
                            , "OnDeletePackageComplete"
                            , AlertType.Check
                            )
                            ;
                    }
                    else
                    {
                        (this.Page as BasePage).Alert(string.Format("The package was deleted, however please review the following messages: <br /> <ul><li>{0}</li></ul>", message)
                            , "Delete Package Warning"
                            , "OnDeletePackageComplete"
                            , Telerik.Charting.Styles.Unit.Pixel(550)
                            , Telerik.Charting.Styles.Unit.Pixel(200)
                            , AlertType.Error);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Failed to delete package database.", ex, LogType.Error);

                    (this.Page as BasePage).Alert(string.Format("Failed to delete package database.  Please reive the logs for additional details.")
                        , "Delete Package Failure"
                        , "OnDeletePackageComplete"
                        , Telerik.Charting.Styles.Unit.Pixel(550)
                        , Telerik.Charting.Styles.Unit.Pixel(200)
                        , AlertType.Error);

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
                    fNode.Selected = expression == f.Expression;
                    node.Nodes.Add(fNode);
                }
                else
                {
                    n.Selected = expression == f.Expression;
                }
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

            if (this.Request.Cookies["PACKAGE_OPTIONS"] != null)
            {
                bool expanded = bool.Parse(this.Request.Cookies["PACKAGE_OPTIONS"].Value);

                this.optionsTd.Visible = expanded;
            }

            if (this.Request.Cookies["PACKAGE_FILTER_PREVIEW"] != null)
            {
                string packageFilterPreview = this.Request.Cookies["PACKAGE_FILTER_PREVIEW"].Value;
                this.literalPackageFilterExpression.Text = packageFilterPreview;
            }

            string filterExp = string.Empty;

            if (this.Request.Cookies["PACKAGE_FILTER_EXPRESSION"] != null)
            {
                filterExp = this.Request.Cookies["PACKAGE_FILTER_EXPRESSION"].Value;

                if (filterExp != FilterPackageControl.PACKAGE_UNDO_NEW_FILTER)
                    this.buttonClearFilter.Visible = true;
            }

            if (!this.IsPostBack)
            {
                this.UpdateNodeSelection(filterExp);

                this.gridPackage.PageSize = this.pageSize ?? 5;
                this.gridPackage.CurrentPageIndex = this.pageIndex ?? 0;
            }
        }

        protected void ajaxPanelPackageManager_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
                case "CONFIRM_PAUSE_RESUME_PACKAGE":

                    this.ConfirmPauseResumePackage(param);
                    break;

                case "PAUSE_RESUME_PACKAGE":

                    this.PauseResumePackage(param);
                    break;

                case "CONFIRM_DELETE_PACKAGE":

                    this.ConfirmDeletePackage(param);
                    break;

                case "DELETE_PACKAGE":

                    this.DeletePackage(param);
                    break;

                case "REFRESH":

                    this.gridPackage.Rebind();
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

        protected void ajaxPanelAssembly_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            RadAjaxPanel panel = sender as RadAjaxPanel;
            RadGrid gridAssembly = panel.FindControl("gridAssembly") as RadGrid;
            LinqDataSource linqDataSourceAssembly = panel.FindControl("linqDataSourceAssembly") as LinqDataSource;
            gridAssembly.DataSourceID = linqDataSourceAssembly.ID;
            gridAssembly.DataBind();
        }

        protected void gridPackage_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item
                || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
            {
                PackageMetaData package = e.Item.DataItem as PackageMetaData;

                RadButton buttonDelete = e.Item.FindControl("buttonDelete") as RadButton;
                buttonDelete.CommandArgument = package.UniqueId.ToString();
                buttonDelete.Visible = PermissionModule.GrantPermission(base.User, base.Groups, Permission.DeletePackage);

                RadButton buttonPauseResume = e.Item.FindControl("buttonPauseResume") as RadButton;
                buttonPauseResume.CommandArgument = package.UniqueId.ToString();
                buttonPauseResume.Visible = PermissionModule.GrantPermission(base.User, base.Groups, Permission.PausePackage);

                RadButton buttonMovePackage = e.Item.FindControl("buttonMovePackage") as RadButton;
                buttonMovePackage.CommandArgument = package.UniqueId.ToString();
                buttonMovePackage.Visible = PermissionModule.GrantPermission(base.User, base.Groups, Permission.MovePackage);

                HyperLink hyperLinkPackageDownload = e.Item.FindControl("hyperLinkPackageDownload") as HyperLink;
                hyperLinkPackageDownload.NavigateUrl = SPContext.Current.Web.Url + string.Format("/Package.ashx?UniqueId={0}"
                    , package.UniqueId)
                    ;

                Label labelUniqueId = e.Item.FindControl("labelUniqueId") as Label;
                Label labelDescription = e.Item.FindControl("labelDescription") as Label;

                Image imagePackageIcon = e.Item.FindControl("imagePackageIcon") as Image;
                imagePackageIcon.ImageUrl = "/_layouts/Images/Vmgr/nav-packagemgr-enabled-32.png";

                buttonPauseResume.ToolTip = "Pause package";
                buttonPauseResume.Image.ImageUrl = "/_layouts/Images/Vmgr/pause-icon-16.png";

                if (package.Deactivated)
                {
                    imagePackageIcon.ImageUrl = "/_layouts/Images/Vmgr/nav-packagemgr-disabled-32.png";

                    hyperLinkPackageDownload.ForeColor = System.Drawing.Color.LightGray;
                    labelUniqueId.ForeColor = System.Drawing.Color.LightGray;
                    labelDescription.ForeColor = System.Drawing.Color.LightGray;

                    buttonPauseResume.ToolTip = "Resume package";
                    buttonPauseResume.Image.ImageUrl = "/_layouts/Images/Vmgr/play-icon-16.png";
                }
            }

            if (e.Item.ItemType == GridItemType.NestedView)
            {
                RadTabStrip tabStripPackageResource = e.Item.FindControl("tabStripPackageResource") as RadTabStrip;
                RadTab tabAssemblies = tabStripPackageResource.FindTabByText("Assemblies");
                RadAjaxPanel ajaxPanelAssembly = e.Item.FindControl("ajaxPanelAssembly") as RadAjaxPanel;
                RadGrid gridAssembly = e.Item.FindControl("gridAssembly") as RadGrid;

                PackageMetaData package = e.Item.DataItem as PackageMetaData;

                if (package.Deactivated)
                    tabAssemblies.Enabled = false;

                tabStripPackageResource.OnClientLoad = string.Format("function(s, e){{ function readyAssemblies () {{ assemblyTabFunctionArray.add('{0}', function () {{  var ajaxPanelAssembly = $find('{1}'); ajaxPanelAssembly.ajaxRequest(); }}); }} function OnReadyAssemblies() {{ readyAssemblies(); Sys.Application.remove_load(OnReadyAssemblies); }} Sys.Application.add_load(OnReadyAssemblies);  }}", package.UniqueId, ajaxPanelAssembly.ClientID);
                tabStripPackageResource.OnClientTabSelected = string.Format("function (s, e){{ var tab = e.get_tab(); if(tab.get_text() == 'Assemblies') {{ assemblyTabFunctionArray.execute('{0}', null); }} }}", package.UniqueId);
            }
        
            if (e.Item is GridPagerItem)
            {
                RadComboBox PageSizeCombo = (RadComboBox)e.Item.FindControl("PageSizeComboBox");

                PageSizeCombo.Items.Clear();
                PageSizeCombo.Items.Add(new RadComboBoxItem("5"));
                PageSizeCombo.FindItemByText("5").Attributes.Add("ownerTableViewId", this.gridPackage.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("10"));
                PageSizeCombo.FindItemByText("10").Attributes.Add("ownerTableViewId", this.gridPackage.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("20"));
                PageSizeCombo.FindItemByText("20").Attributes.Add("ownerTableViewId", this.gridPackage.MasterTableView.ClientID);
                PageSizeCombo.Items.Add(new RadComboBoxItem("50"));
                PageSizeCombo.FindItemByText("50").Attributes.Add("ownerTableViewId", this.gridPackage.MasterTableView.ClientID);
                PageSizeCombo.FindItemByText(e.Item.OwnerTableView.PageSize.ToString()).Selected = true;
            }
        }

        protected void gridPackage_PageIndexChanged(object sender, GridPageChangedEventArgs e)
        {
            this.pageIndex = e.NewPageIndex;
        }

        protected void gridPackage_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
        {
            this.pageSize = e.NewPageSize;

            if (this.pageIndex.HasValue)
            {
                if (this.pageIndex >= this.gridPackage.PageCount - 1)
                {
                    this.pageIndex = (this.gridPackage.PageCount - 1);
                }
            }
        }

        protected void gridPlugin_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item
                || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
            {
                PluginMetaData plugin = e.Item.DataItem as PluginMetaData;

                Image imagePluginIcon = e.Item.FindControl("imagePluginIcon") as Image;
                imagePluginIcon.ImageUrl = "/_layouts/Images/Vmgr/package-plugin-enabled-32.png";

                if(plugin.PackageDeactivated)
                    imagePluginIcon.ImageUrl = "/_layouts/Images/Vmgr/package-plugin-disabled-32.png";

            }
        }

        protected void linqDataSourcePackage_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.packages;
        }

        protected void linqDataSourcePlugin_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.plugins
                .Where(s => s.PackageId == e.WhereParameters["PackageId"].ToNullable<int>());
        }

        protected void linqDataSourceAssembly_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            PackageMetaData package = this.packages
                .Where(p => p.UniqueId == new Guid(e.WhereParameters["Id"].ToString()))
                .FirstOrDefault()
                ;

            IList<AssemblyMetaData> assemblies = new List<AssemblyMetaData> { };

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

                ChannelFactory<IPackageInspector> httpFactory = new ChannelFactory<IPackageInspector>(binding
                    , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Packaging/{3}/PackageInspector"
                    , (this.Page as BasePage).server.WSProtocol
                    , (this.Page as BasePage).server.WSFqdn
                    , (this.Page as BasePage).server.WSPort
                    , package.UniqueId
                    )
                    )
                    )
                    ;
                IPackageInspector packageInspectorProxy = httpFactory.CreateChannel();
               
                // TODO: Only get these if the server is online.
                assemblies = packageInspectorProxy.GetAssemblies()
                    .OrderBy(a => a.FullName)
                    .ToList()
                    ;
            }
            catch (EndpointNotFoundException ex)
            {
                Logger.Logs.Log("Unable to inspect package from source server.  The service does not appear to be online.", ex, LogType.Error);
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Unable to inspect package from source server.", ex, LogType.Error);
            }

            e.Result = assemblies;
        }

        protected void treeViewOptions_Load(object sender, EventArgs e)
        {
            RadTreeView treeView = sender as RadTreeView;

            RadTreeNode node = treeView.Nodes.Cast<RadTreeNode>()
                .Where(t => t.Value == "UPLOAD_PACKAGE")
                .FirstOrDefault()
                ;

            if (node != null)
            {
                node.Visible = PermissionModule.GrantPermission(base.User, base.Groups, Permission.EditPackage);
            }
        }

        #endregion
    }
}