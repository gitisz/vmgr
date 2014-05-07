using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

namespace Vmgr.SharePoint
{
    public partial class FrameMaster : MasterPage
    {
        #region PRIVATE PROPERTIES

        private HtmlGenericControl _jqueryPath = null;
        private HtmlGenericControl _frameCssPath = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected global::System.Web.UI.ScriptManager ScriptManager;

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

        protected HtmlGenericControl FrameCssPath
        {
            get
            {
                if (_frameCssPath == null)
                {
                    HtmlGenericControl control = new HtmlGenericControl("link");
                    control.Attributes.Add("rel", "stylesheet");
                    control.Attributes.Add("type", "text/css");
                    control.Attributes.Add("href", "/_layouts/Vmgr/stylesheet-frame.css");

                    _frameCssPath = control;
                }

                return _frameCssPath;
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            this.head.Controls.AddAt(0, this.FrameCssPath);
            this.head.Controls.AddAt(0, this.JqueryPath);
        }
        
        #endregion
    }
}