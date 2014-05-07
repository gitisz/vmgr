using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Telerik.Web.UI;

namespace Vmgr.SharePoint
{
    public partial class WindowFilterJobControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private string _filterType = null;
     
        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public string FilterType
        {
            get
            {
                return this._filterType;
            }
            set
            {
                this._filterType = value;
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        #endregion
			
			
    }
}