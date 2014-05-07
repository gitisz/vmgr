using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using System.Net.NetworkInformation;
using Microsoft.SharePoint;
using Vmgr.Data.Biz.MetaData;

namespace Vmgr.SharePoint
{
    public partial class WindowMaster : MasterPage, IWindowManager
    {
        #region PRIVATE PROPERTIES
      
        private HtmlGenericControl _jqueryPath = null;
        private HtmlGenericControl _windowCssPath = null;
        private HtmlGenericControl _commonJsPath = null;
        private HtmlGenericControl _jquerySignalRPath = null;
        private HtmlGenericControl _json2Path = null;
        private HtmlGenericControl _jqueryUI = null;
        private HtmlGenericControl _jqueryUiCssPath = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected global::System.Web.UI.ScriptManager ScriptManager;

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
                if (!(this.Page as BasePage).server.RTFqdn.Equals(this.Request.Url.DnsSafeHost, StringComparison.InvariantCultureIgnoreCase))
                    return "true";

                return "false";
            }
        }

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

        protected HtmlGenericControl WindowCssPath
        {
            get
            {
                if (_windowCssPath == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("link");
                    control.Attributes.Add("rel", "stylesheet");
                    control.Attributes.Add("type", "text/css");
                    control.Attributes.Add("href", "/_layouts/Vmgr/stylesheet-window.css");

                    _windowCssPath = control;
                }

                return _windowCssPath;
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

        protected string redirectServerUrl
        {
            get
            {
                if (this.Page is BasePage)
                {
                    if ((this.Page as BasePage).server == null)
                        return string.Format("parentWindow.BrowserWindow.location = '{0}'", BasePage.ServerUrl);
                }

                return string.Empty;
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

            if(string.IsNullOrEmpty(hostName))
                return string.Format("{0}.{1}", ipProperties.HostName, ipProperties.DomainName);
            else
                return string.Format("{0}.{1}", hostName, ipProperties.DomainName);
        }

        #endregion

        #region EVENTS

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (this.Page is BasePage)
            {
                if ((this.Page as BasePage).server == null)
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "SERVER_NULL_REDIRECT", "OnClose(true);", true);
            }
			
            this.head.Controls.AddAt(0, this.WindowCssPath);
            this.head.Controls.AddAt(0, this.JquerySignalRPath);
            this.head.Controls.AddAt(0, this.Json2Path);
            this.head.Controls.AddAt(0, this.CommonJsPath);
            this.head.Controls.AddAt(0, this.JQueryUiCssPath);
            this.head.Controls.AddAt(0, this.JQueryUI);
            this.head.Controls.AddAt(0, this.JqueryPath);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.head.Controls.AddAt(0, this.WindowCssPath);
            this.head.Controls.AddAt(0, this.CommonJsPath);
            this.head.Controls.AddAt(0, this.JqueryPath);
        }

        #endregion
    }
}