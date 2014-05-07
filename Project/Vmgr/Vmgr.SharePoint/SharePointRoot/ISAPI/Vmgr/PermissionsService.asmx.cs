using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using Vmgr.Data.Biz.Logging;

namespace Vmgr.SharePoint
{
    /// <summary>
    /// A web service that allows the permission cache to be cleared.
    /// </summary>
    [WebService(Namespace = "http://Vmgr.SharePoint/PermissionsService")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class PermissionsService : WebService, IPermissionsService
    {
        [WebMethod]
        public void ClearPermissionCache()
        {
            Logger.Logs.Log(string.Format("Attempting to clear permission cache via web service call to IP: {0}."
                , PermissionModule.GetLocalIpAddress()
                ), LogType.Info);

            try
            {
                if (AppDomain.CurrentDomain.GetData(PermissionModule.VMGR_MEMBERSHIP_LIST_KEY) != null)
                    AppDomain.CurrentDomain.SetData(PermissionModule.VMGR_MEMBERSHIP_LIST_KEY, null);

                if (AppDomain.CurrentDomain.GetData(PermissionModule.VMGR_ROLE_LIST_KEY) != null)
                    AppDomain.CurrentDomain.SetData(PermissionModule.VMGR_ROLE_LIST_KEY, null);

                if (AppDomain.CurrentDomain.GetData(PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY) != null)
                    AppDomain.CurrentDomain.SetData(PermissionModule.VMGR_ROLE_PERMISSION_LIST_KEY, null);

                if (AppDomain.CurrentDomain.GetData(PermissionModule.PERMISSION_VMGR_MAP_KEY) != null)
                    AppDomain.CurrentDomain.SetData(PermissionModule.PERMISSION_VMGR_MAP_KEY, null);
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to call ClearPermissionCache WebMethod", ex, LogType.Error);

                throw ex;
            }

            Logger.Logs.Log(string.Format("Successfully able to clear permission cache via web service call to IP: {0}."
               , PermissionModule.GetLocalIpAddress()
               ), LogType.Info);

        }
    }
}
