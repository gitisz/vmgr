using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Web.UI;

namespace Vmgr.SharePoint
{
    interface IWindowManager
    {
        RadWindowManager WindowManagerAlertCheck { get; }
        RadWindowManager WindowManagerAlertError { get; }
        RadWindowManager WindowManagerAlertInfo { get; }
        RadWindowManager WindowManagerConfirm { get; }
        RadWindowManager WindowManagerConfirmComment { get; }
        RadWindowManager WindowManagerConfirmCommentEmail { get; }
    }
}
