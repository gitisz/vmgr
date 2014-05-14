using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Vmgr.SharePoint
{
    public partial class Dashboard : BaseVmgrPage
    {
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
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!getBrowser())
            {
                this.PanelWeHateIe8.Visible = true;
            }
        }
    }
}