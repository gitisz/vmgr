using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using Telerik.Web.UI;
using System;
using System.Collections.Generic;
using Vmgr.AD;

namespace Vmgr.SharePoint
{
    public class BaseControl : UserControl
    {
        #region PRIVATE PROPERTIES

        private IUser _user = null;
        private IList<IGroup> _groups = null;
        
        #endregion

        #region PROTECTED PROPERTIES

        protected string webName
        {
            get
            {
                return SPContext.Current.Web.Title;
            }
        }

        protected string source
        {
            get
            {
                if (this.Request["Source"] != null)
                    return this.Request["Source"];

                return HttpUtility.UrlEncode(Page.Request.Url.AbsoluteUri);
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
                if (!(this.Page as BasePage).server.RTFqdn.Equals(this.Request.Url.DnsSafeHost, StringComparison.InvariantCultureIgnoreCase))
                    return "true";

                return "false";
            }
        }


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

        public IUser User
        {
            get
            {
                if (this._user == null)
                {
                    this._user = PermissionModule.GetUser(HttpContext.Current.User.Identity);
                }

                return this._user;
            }
        }

        public IList<IGroup> Groups
        {
            get
            {
                if (this._groups == null)
                {
                    this._groups = PermissionModule.GetGroupsByMembership(this.User);
                }

                return this._groups;
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

        #endregion

        #region EVENTS

        #endregion
        

    }
}