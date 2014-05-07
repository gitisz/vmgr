using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint.WebPartPages;
using Microsoft.SharePoint;
using Telerik.Web.UI;
using System.Web.UI;
using Telerik.Charting.Styles;
using Vmgr.SharePoint.Enumerations;
using System.Text;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Data.Biz;
using System.Security.Principal;
using Vmgr.Data.Biz.Logging;
using Microsoft.SharePoint.Utilities;

namespace Vmgr.SharePoint
{
    public partial class BasePage : WebPartPage
    {
        #region PRIVATE PROPERTIES

        private ServerMetaData _server = null;

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public RadWindowManager windowManagerAlertCheck
        {
            get
            {
                return (this.Page.Master as IWindowManager).WindowManagerAlertCheck;
            }
        }

        public RadWindowManager windowManagerAlertError
        {
            get
            {
                return (this.Page.Master as IWindowManager).WindowManagerAlertError;
            }
        }

        public RadWindowManager windowManagerAlertInfo
        {
            get
            {
                return (this.Page.Master as IWindowManager).WindowManagerAlertInfo;
            }
        }

        public RadWindowManager windowManagerConfirm
        {
            get
            {
                return (this.Page.Master as IWindowManager).WindowManagerConfirm;
            }
        }

        public RadWindowManager windowManagerConfirmComment
        {
            get
            {
                return (this.Page.Master as IWindowManager).WindowManagerConfirmComment;
            }
        }

        public RadWindowManager windowManagerConfirmCommentEmail
        {
            get
            {
                return (this.Page.Master as IWindowManager).WindowManagerConfirmCommentEmail;
            }
        }

        public ServerMetaData server
        {
            get
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                    {
                        if (this.Request.Cookies["SELECTED_SERVER"] == null)
                        {
                            if (this.Page is Server)
                            {
                            }
                            else
                            {
                                if (!(this.Page is BaseWindowPage))
                                    this.Response.Redirect(BasePage.RedirectServerUrl);
                            }
                        }
                        else
                        {
                            if (this._server == null)
                            {
                                using (AppService app = new AppService())
                                {
                                    if (this.Request.Cookies["SELECTED_SERVER"] != null)
                                    {
                                        this._server = app.GetServers()
                                            .Where(s => s.UniqueId == new Guid(this.Request.Cookies["SELECTED_SERVER"].Value as string))
                                            .FirstOrDefault()
                                            ;

                                        if (this._server == null)
                                        {
                                            HttpCookie selectedServerookie = new HttpCookie("SELECTED_SERVER");
                                            selectedServerookie.Expires = DateTime.Now.AddDays(-1);
                                            this.Response.Cookies.Add(selectedServerookie); 

                                            this.Response.Redirect(BasePage.RedirectServerUrl);
                                        }
                                    }
                                }
                            }
                        }
                    }
                );

                return this._server;
            }
        }

        public static string RedirectServerUrl
        {
            get
            {
                return string.Format("{0}?redirectUrl={1}"
                    , BasePage.ServerUrl
                    , HttpUtility.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath));
            }
        }

        public static string ServerUrl
        {
            get
            {
                string path = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)
                    + SPContext.Current.Web.ServerRelativeUrl;

                path = path.TrimEnd('/');

                return string.Format("{0}/Server.aspx", path);
            }
        }

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS
        
        public void Alert(string message, string caption, string callback, AlertType type)
        {
            this.Alert(message, caption, callback, Unit.Pixel(330), Unit.Pixel(100), type);
        }

        /// <summary>
        /// Shows a Telerik alert after postback has occurred.
        /// </summary>
        /// <param name="message">Provide a message to appear in the main body of the popup alert.</param>
        /// <param name="caption">Provide a caption to appear in the header of the popup alert.</param>
        /// <param name="callback">Provide a javasctip function to be called after the user closes the alert.</param>
        /// <param name="type">Provide and WindowType to set a different predefined icon in the main body of the popup alert.</param>
        public void Alert(string message, string caption, string callback, Unit width, Unit height, AlertType type)
        {
            RadWindowManager manager = null;

            switch (type)
            {
                case AlertType.Check:
                    manager = this.windowManagerAlertCheck;
                    break;
                case AlertType.Error:
                    manager = this.windowManagerAlertError;
                    break;
                default:
                    break;
            }

            if (manager == null)
                throw new NullReferenceException("RadWindowManager must not be undefined.  Does this control depend on Window.Master?");

            manager.RadAlert(message, (int)width.Value, (int)height.Value, caption, callback);

        }

        /// <summary>
        /// Shows a Telerik alert after postback has occurred.
        /// </summary>
        /// <param name="message">Provide a message to appear in the main body of the popup alert.</param>
        /// <param name="caption">Provide a caption to appear in the header of the popup alert.</param>
        /// <param name="callback">Provide a javasctip function to be called after the user closes the alert.</param>
        /// <param name="type">Provide and WindowType to set a different predefined icon in the main body of the popup alert.</param>
        public void Confirm(string message, string caption, string callback, ConfirmType type)
        {
            this.Confirm(message, caption, callback, Unit.Pixel(330), Unit.Pixel(100), type);
        }

        public void Confirm(string message, string caption, string callback, Unit width, Unit height, ConfirmType type)
        {
            RadWindowManager manager = null;

            switch (type)
            {
                case ConfirmType.Confirm:
                    manager = this.windowManagerConfirm;
                    break;
                case ConfirmType.Comment:
                    manager = this.windowManagerConfirmComment;
                    break;
                case ConfirmType.CommentEmail:
                    manager = this.windowManagerConfirmCommentEmail;
                    break;
                default:
                    break;
            }

            if (manager == null)
                throw new NullReferenceException("RadWindowManager must not be undefined.  Does this control depend on Window.Master?");

            manager.RadConfirm(message, callback, (int)width.Value, (int)height.Value, null, caption);

        }

        /// <summary>
        /// Encodes a string to be represented as a string literal. The format
        /// is essentially a JSON string.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodeJsString(string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\'':
                        sb.Append("\\\'");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }

        public string GetHubConnectionUrl()
        {
            return GetHubConnectionUrl(this.server);
        }

        public string GetHubConnectionUrl(ServerMetaData s)
        {
            return string.Format("{0}://{1}:{2}"
                , s.RTProtocol
                , Main.GetLocalhostFqdn(s.Name)
                , s.RTPort
                )
                ;
        }

        public string UseJsonp()
        {
            return this.server.RTProtocol.Equals("HTTPS", StringComparison.InvariantCultureIgnoreCase).ToString().ToLower();
        }

        #endregion

        #region EVENTS

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.Page.MasterPageFile = "~site/_catalogs/masterpage/Base.master";
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            Page.ClientScript.RegisterStartupScript(this.Page.GetType()
                , this.ID
                , "_spOriginalFormAction = document.forms[0].action;_spSuppressFormOnSubmitWrapper=true;"
                , true);

            if (this.Page.Form != null)
            {
                string formOnSubmitAtt = this.Page.Form.Attributes["onsubmit"];

                if (!string.IsNullOrEmpty(formOnSubmitAtt) && formOnSubmitAtt == "return _spFormOnSubmitWrapper();")
                {
                    this.Page.Form.Attributes["onsubmit"] = "_spFormOnSubmitWrapper();";
                }
            }
        }

        #endregion
    }
}