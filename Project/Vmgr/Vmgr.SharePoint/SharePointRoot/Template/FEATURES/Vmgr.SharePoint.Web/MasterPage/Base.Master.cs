using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using Microsoft.SharePoint.Navigation;
using Microsoft.SharePoint;

namespace Vmgr.SharePoint
{
    public partial class BaseMaster : MasterPage, IWindowManager
    {
        #region PRIVATE PROPERTIES

        private HtmlGenericControl _jqueryPath = null;
        private HtmlGenericControl _commonJsPath = null;
        private HtmlGenericControl _vmgrCssPath = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected global::System.Web.UI.ScriptManager ScriptManager;

        protected string siteId
        {
            get
            {
                return SPContext.Current.Web.ID.ToString();
            }
        }

        protected string siteUrl
        {
            get
            {
                return SPContext.Current.Web.Url;
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

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

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

        #endregion

        #region EVENTS

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.head.Controls.AddAt(0, this.VmgrCssPath);
            this.head.Controls.AddAt(0, this.CommonJsPath);
            this.head.Controls.AddAt(0, this.JqueryPath);

            this.hyperLinkBreadCrumbSite.Text = SPContext.Current.Site.RootWeb.Title;
            this.hyperLinkBreadCrumbSite.NavigateUrl = SPContext.Current.Site.RootWeb.Url;

            this.labelBreadCrumbSubSite.Text = this.Page.Title;

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

                    foreach (SPNavigationNode spNode in quickLaunchNodesCollection)
                    {
                        foreach (SPNavigationNode childNode in spNode.Children)
                        {

                            RadMenuItem item = new RadMenuItem
                            {
                                NavigateUrl = childNode.Url,
                                ImageUrl = childNode.Properties["IMAGE_URL"].ToString(),
                                ToolTip = childNode.Title,
                                Text = " ",
                            }
                            ;

                            this.menuMain.Items.Add(item);
                        }
                    }
                }
            }
        }

        #endregion
    }

}