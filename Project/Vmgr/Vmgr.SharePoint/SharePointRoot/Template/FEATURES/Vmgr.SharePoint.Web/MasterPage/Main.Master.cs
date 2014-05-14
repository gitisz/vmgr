using Microsoft.SharePoint;
using Microsoft.SharePoint.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using Vmgr.AD;
using Vmgr.Data.Biz.MetaData;

namespace Vmgr.SharePoint
{
    public partial class Main : MasterPage, IWindowManager
    {
        #region PRIVATE PROPERTIES

        private HtmlGenericControl _jqueryPath = null;
        private HtmlGenericControl _commonJsPath = null;
        private HtmlGenericControl _vmgrCssPath = null;
        private HtmlGenericControl _jquerySignalRPath = null;
        private HtmlGenericControl _json2Path = null;
        private HtmlGenericControl _jquerydotdotdot = null;
        private HtmlGenericControl _jqueryTextillate = null;
        private HtmlGenericControl _jqueryFittext = null;
        private HtmlGenericControl _jqueryLettering = null;
        private HtmlGenericControl _vmgrAnimate = null;
        private HtmlGenericControl _jqueryUI = null;
        private HtmlGenericControl _jqueryUiCssPath = null;
        private IUser _vmgrUser = null;
        private IList<IGroup> _vmgrGroups = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected global::System.Web.UI.ScriptManager ScriptManager;
        
        protected string pollingServiceUrl
        {
            get
            {
                return "/_vti_bin/Vmgr/PollingService.asmx/IsStarted";
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

        protected string siteUrl
        {
            get
            {
                return SPContext.Current.Web.Url;
            }
        }

        protected string useJsonP
        {
            get
            {
                if (this.Request.Browser.Type.StartsWith("IE"))
                    return "true";

                return "false";
            }
        }

        protected string xDomain
        {
            get
            {
                if (!(this.Page as BasePage).server.WSFqdn.Equals(this.Request.Url.DnsSafeHost, StringComparison.InvariantCultureIgnoreCase))
                    return "true";

                return "false";
            }
        }

        protected HtmlGenericControl JquerySignalRPath
        {
            get
            {
                if (_jquerySignalRPath == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("script");
                    control.Attributes.Add("type", "text/javascript");
                    control.Attributes.Add("src", "/_layouts/Vmgr/jquery.signalR-2.0.3.min.js");

                    _jquerySignalRPath = control;
                }

                return _jquerySignalRPath;
            }
        }

        protected HtmlGenericControl Json2Path
        {
            get
            {
                if (_json2Path == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("script");
                    control.Attributes.Add("type", "text/javascript");
                    control.Attributes.Add("src", "/_layouts/Vmgr/json2.min.js");

                    _json2Path = control;
                }

                return _json2Path;
            }
        }

        protected HtmlGenericControl JqueryPath
        {
            get
            {
                if (_jqueryPath == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("script");
                    control.Attributes.Add("type", "text/javascript");
            
                    if (getBrowser())
                        control.Attributes.Add("src", "/_layouts/Vmgr/jquery-2.1.0.min.js");
                    else
                        control.Attributes.Add("src", "/_layouts/Vmgr/jquery.1.7.2.js");

                    _jqueryPath = control;
                }

                return _jqueryPath;
            }
        }

        protected HtmlGenericControl CommonJsPath
        {
            get
            {
                if (_commonJsPath == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("script");
                    control.Attributes.Add("type", "text/javascript");
                    control.Attributes.Add("src", "/_layouts/Vmgr/common.js");

                    _commonJsPath = control;
                }

                return _commonJsPath;
            }
        }

        protected HtmlGenericControl VmgrCssPath
        {
            get
            {
                if (_vmgrCssPath == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("link");
                    control.Attributes.Add("rel", "stylesheet");
                    control.Attributes.Add("type", "text/css");
                    control.Attributes.Add("href", "/_layouts/Vmgr/stylesheet-vmgr.css");

                    _vmgrCssPath = control;
                }

                return _vmgrCssPath;
            }
        }

        protected HtmlGenericControl JQueryDotDotDot
        {
            get
            {
                if (_jquerydotdotdot == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("script");
                    control.Attributes.Add("type", "text/javascript");
                    control.Attributes.Add("src", "/_layouts/Vmgr/jquery.dotdotdot.min.js");

                    _jquerydotdotdot = control;
                }

                return _jquerydotdotdot;
            }
        }

        protected HtmlGenericControl JQueryTextillate
        {
            get
            {
                if (_jqueryTextillate == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("script");
                    control.Attributes.Add("type", "text/javascript");
                    control.Attributes.Add("src", "/_layouts/Vmgr/jquery.textillate.js");

                    _jqueryTextillate = control;
                }

                return _jqueryTextillate;
            }
        }

        protected HtmlGenericControl JQueryFittext
        {
            get
            {
                if (_jqueryFittext == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("script");
                    control.Attributes.Add("type", "text/javascript");
                    control.Attributes.Add("src", "/_layouts/Vmgr/jquery.fittext.js");

                    _jqueryFittext = control;
                }

                return _jqueryFittext;
            }
        }

        protected HtmlGenericControl JQueryLettering
        {
            get
            {
                if (_jqueryLettering == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("script");
                    control.Attributes.Add("type", "text/javascript");
                    control.Attributes.Add("src", "/_layouts/Vmgr/jquery.lettering.js");

                    _jqueryLettering = control;
                }

                return _jqueryLettering;
            }
        }

        protected HtmlGenericControl JQueryUI
        {
            get
            {
                if (_jqueryUI == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("script");
                    control.Attributes.Add("type", "text/javascript");
                    control.Attributes.Add("src", "/_layouts/Vmgr/jquery-ui-1.10.4.js");

                    _jqueryUI = control;
                }

                return _jqueryUI;
            }
        }

        protected HtmlGenericControl JQueryUiCssPath
        {
            get
            {
                if (_jqueryUiCssPath == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("link");
                    control.Attributes.Add("rel", "stylesheet");
                    control.Attributes.Add("type", "text/css");
                    control.Attributes.Add("href", "/_layouts/Vmgr/jquery-ui.css");

                    _jqueryUiCssPath = control;
                }

                return _jqueryUiCssPath;
            }
        }

        protected HtmlGenericControl VmgrAnimate
        {
            get
            {
                if (_vmgrAnimate == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("link");
                    control.Attributes.Add("rel", "stylesheet");
                    control.Attributes.Add("type", "text/css");
                    control.Attributes.Add("href", "/_layouts/Vmgr/animate.css");

                    _vmgrAnimate = control;
                }

                return _vmgrAnimate;
            }
        }

        protected bool isExpanded
        {
            get
            {
                if (this.Session["ISEXPANDED"] == null)
                    return false;
                else
                    return bool.Parse(this.Session["ISEXPANDED"].ToString());
            }
            set
            {
                if (this.Session["ISEXPANDED"] == null)
                    this.Session.Add("ISEXPANDED", value.ToString());
                else
                    this.Session["ISEXPANDED"] = value.ToString();
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        public RadWindowManager WindowManagerAlertCheck
        {
            get
            {
                return this.windowManagersControl.FindControl("windowManagerAlertCheck") as RadWindowManager;
            }
        }

        public RadWindowManager WindowManagerAlertError
        {
            get
            {
                return this.windowManagersControl.FindControl("windowManagerAlertError") as RadWindowManager;
            }
        }

        public RadWindowManager WindowManagerAlertInfo
        {
            get
            {
                return this.windowManagersControl.FindControl("windowManagerAlertInfo") as RadWindowManager;
            }
        }

        public RadWindowManager WindowManagerConfirm
        {
            get
            {
                return this.windowManagersControl.FindControl("windowManagerConfirm") as RadWindowManager;
            }
        }

        public RadWindowManager WindowManagerConfirmComment
        {
            get
            {
                return this.windowManagersControl.FindControl("windowManagerConfirmComment") as RadWindowManager;
            }
        }

        public RadWindowManager WindowManagerConfirmCommentEmail
        {
            get
            {
                return this.windowManagersControl.FindControl("windowManagerConfirmCommentEmail") as RadWindowManager;
            }
        }

        public IUser VmgrUser
        {
            get
            {
                if (this._vmgrUser == null)
                {
                    this._vmgrUser = PermissionModule.GetUser(HttpContext.Current.User.Identity);
                }

                return this._vmgrUser;
            }
        }

        public IList<IGroup> VmgrGroups
        {
            get
            {
                if (this._vmgrGroups == null)
                {
                    this._vmgrGroups = PermissionModule.GetGroupsByMembership(this.VmgrUser);
                }

                return this._vmgrGroups;
            }
        }

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void ConfigureMenu(bool expanded)
        {
            this.menuMain.Items.Clear();

            using (SPSite site = new SPSite(SPContext.Current.Site.ID))
            {
                using (SPWeb web = site.OpenWeb(SPContext.Current.Web.ID))
                {
                    SPNavigationNodeCollection quickLaunchNodesCollection = web.Navigation.QuickLaunch;
                    IList<MenuPanelNode> nodes = quickLaunchNodesCollection.Cast<SPNavigationNode>()
                        .Select(t => new MenuPanelNode
                        {
                            Id = t.Id,
                            ParentId = t.ParentId,
                            Title = t.Title,
                        }
                        )
                        .ToList();

                    foreach (SPNavigationNode node in quickLaunchNodesCollection)
                    {
                        bool permitted = false;

                        if (node.Properties.ContainsKey("Permission"))
                            permitted = PermissionModule.GrantPermission(this.VmgrUser, this.VmgrGroups, (Permission)node.Properties["Permission"]);

                        if (permitted)
                        {
                            RadMenuItem item = new RadMenuItem
                            {
                                NavigateUrl = node.Url,
                                ImageUrl = node.Properties["IMAGE_DISABLED_URL"].ToString(),
                                SelectedImageUrl =  node.Properties["IMAGE_URL"].ToString(),
                                HoveredImageUrl = node.Properties["IMAGE_URL"].ToString(),
                                ToolTip = node.Title,
                                Text = expanded ? node.Title : " ",
                                CssClass = "Main"
                            }
                            ;

                            if (node.Url.Contains(this.Request.Url.AbsolutePath, StringComparison.InvariantCultureIgnoreCase))
                                item.Selected = true;

                            this.menuMain.Items.Add(item);
                        }
                    }
                }
            }
        }

        #endregion

        #region PROTECTED METHODS

        /// <summary>
        /// Return true if the browser is worthy of being called a browser.
        /// </summary>
        /// <returns></returns>
        protected bool getBrowser()
        {
            bool result = true;

            if (this.Request.Browser.Type.ToUpper().Contains("IE"))
            {
                if (this.Request.Browser.MajorVersion < 9)
                    result = false;
            }

            return result;
        }

        #endregion

        #region PUBLIC METHODS

        public string GetHubConnectionUrl()
        {
            return GetHubConnectionUrl((this.Page as BasePage).server);
        }

        public string GetHubConnectionUrl(ServerMetaData s)
        {
            return string.Format("{0}://{1}:{2}"
                , s.RTProtocol
                , s.RTFqdn
                , s.RTPort
                )
                ;
        }

        public string UseJsonp()
        {
            return (this.Page as BasePage).server.RTProtocol.Equals("HTTPS", StringComparison.InvariantCultureIgnoreCase).ToString().ToLower();
        }

        public static string GetLocalhostFqdn(string hostName)
        {
            var ipProperties = IPGlobalProperties.GetIPGlobalProperties();

            if (string.IsNullOrEmpty(hostName))
                return string.Format("{0}.{1}", ipProperties.HostName, ipProperties.DomainName);
            else
                return string.Format("{0}.{1}", hostName, ipProperties.DomainName);
        }

        #endregion

        #region EVENTS

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.head.Controls.AddAt(0, this.VmgrCssPath);
            this.head.Controls.AddAt(0, this.VmgrAnimate);
            this.head.Controls.AddAt(0, this.JquerySignalRPath);
            this.head.Controls.AddAt(0, this.Json2Path);
            this.head.Controls.AddAt(0, this.CommonJsPath);
            this.head.Controls.AddAt(0, this.JQueryDotDotDot);
            this.head.Controls.AddAt(0, this.JQueryLettering);
            this.head.Controls.AddAt(0, this.JQueryFittext);

            if (this.getBrowser())
                this.head.Controls.AddAt(0, this.JQueryTextillate);
            else
            {
            }
            
            this.head.Controls.AddAt(0, this.JQueryUiCssPath);
            this.head.Controls.AddAt(0, this.JQueryUI);
            this.head.Controls.AddAt(0, this.JqueryPath);

            this.hyperLinkBreadCrumbSite.Text = SPContext.Current.Site.RootWeb.Title;
            this.hyperLinkBreadCrumbSite.NavigateUrl = SPContext.Current.Site.RootWeb.Url;
            this.labelBreadCrumbSubSite.Text = this.Page.Title;
            this.hyperLinkServer.Visible = false;

            ServerMetaData serv = (this.Page as BasePage).server;

            if (serv != null)
            {
                this.hyperLinkServer.Visible = true;
                this.hyperLinkServer.ToolTip = string.Format("Selected server: {0}.", serv.Name);
                this.hyperLinkServer.NavigateUrl = BasePage.RedirectServerUrl;
                this.hyperLinkServer.Text = string.Format("{0} ", serv.Name);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.Request.Cookies["EXPANDCONTRACT"] != null)
            {
                bool expanded = bool.Parse(Request.Cookies["EXPANDCONTRACT"].Value);

                if (expanded)
                {
                    this.buttonExpandContract.Image.ImageUrl = "/_layouts/images/Vmgr/navigate-left-icon-32.png";
                    this.buttonExpandContract.ToolTip = "Contract menu";
                    this.menuMain.Width = 165;
                    this.tdMenu.Style["width"] = "170px";
                }
                else
                {
                    this.buttonExpandContract.Image.ImageUrl = "/_layouts/images/Vmgr/navigate-right-icon-32.png";
                    this.buttonExpandContract.ToolTip = "Expand menu";
                    this.menuMain.Width = 40;
                    this.tdMenu.Style["width"] = "42px";
                }

                this.ConfigureMenu(expanded);
            }
            else
                this.ConfigureMenu(true);

            this.labelUser.Text = WindowsIdentity.GetCurrent().Name;
        }

        #endregion

        protected void buttonLeft_Click(object sender, EventArgs e)
        {

        }

        protected void buttonExpandContract_Click(object sender, EventArgs e)
        {
            HttpCookie cookie = new HttpCookie("EXPANDCONTRACT");

            if (this.Request.Cookies["EXPANDCONTRACT"] != null)
                cookie = Request.Cookies["EXPANDCONTRACT"];

            bool expanded = false;

            if (!string.IsNullOrEmpty(cookie.Value))
                expanded = (!bool.Parse(cookie.Value));

            cookie.Value = expanded.ToString();
            cookie.Expires = DateTime.Now.AddYears(1);

            // Add the cookie.
            this.Response.Cookies.Add(cookie);
            this.Response.Redirect(this.Request.Url.PathAndQuery);
        }
    }

    public class MenuPanelNode
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Title { get; set; }
    }
}