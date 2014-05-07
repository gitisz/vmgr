using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using Vmgr.Data.Biz.Logging;

namespace Vmgr.SharePoint
{
    public class BaseWindowPage : BasePage
    {
        #region PRIVATE PROPERTIES

        #endregion

        #region PROTECTED PROPERTIES

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

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            if (this.server == null)
                Logger.Logs.Log("The server is not defined.", LogType.Info);

            this.Page.MasterPageFile = "~site/_catalogs/masterpage/Window.master";
        }

        #endregion

    }
}