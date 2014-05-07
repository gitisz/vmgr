using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vmgr.Data.Biz.MetaData;
using Telerik.Web.UI;
using Vmgr.Data.Biz;
using System.Security.Principal;
using Vmgr.SharePoint.Enumerations;
using Vmgr.Data.Biz.Logging;
using System.ServiceModel;
using Vmgr.Scheduling;
using Quartz;
using Vmgr.AD;
using System.Drawing;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Vmgr.SharePoint
{
    public partial class SecurityRoleEditorControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private SecurityRoleMetaData _securityRole = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected int selectedSecurityRoleId
        {
            get
            {
                int id = 0;

                if (this.Request.QueryString["SecurityRoleId"] != null)
                    int.TryParse(this.Request.QueryString["SecurityRoleId"], out id);

                return id;
            }
        }

        protected SecurityRoleMetaData securityRole
        {
            get
            {
                if (this._securityRole == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._securityRole = app.GetSecurityRoles()
                            .Where(r => r.SecurityRoleId == this.selectedSecurityRoleId)
                            .FirstOrDefault() ?? new SecurityRoleMetaData
                            {
                            }
                            ;
                    }
                }

                return this._securityRole;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void OnSave()
        {
            try
            {
                this.securityRole.Name = this.textBoxName.Text.Trim();
                this.securityRole.Description = this.textBoxDescription.Text.Trim();
                this.securityRole.Active = true;

                using (AppService app = new AppService())
                {
                    if (app.Save(this.securityRole))
                    {
                        PermissionModule.ClearPermissionCache();
                        PermissionModule.CreatePermissionCache();

                        this.ajaxPanelRole.ResponseScripts.Add(string.Format("OnSaveSuccess('The security role with name <span style=\"font-weight: bold;\">{0}</span> has successfully been saved.');"
                            , this.textBoxName.Text.EncodeJsString()));
                    }
                    else
                    {
                        Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(s => s.Message).ToArray()), LogType.Warn);

                        this.ajaxPanelRole.ResponseScripts.Add(string.Format("OnSaveFail('Failed to save security role.  Please check log table for additional details.');"));
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logs.Log(ex, LogType.Error);

                this.ajaxPanelRole.ResponseScripts.Add(string.Format("OnSaveFail('Failed to save security role.  Please check log table for additional details.');"));
            }
        }

        internal void Set()
        {
            this.textBoxName.Text = this.securityRole.Name;
            this.textBoxDescription.Text = this.securityRole.Description;
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        #endregion

        #region EVENTS

        protected void ajaxPanelRole_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            switch (e.Argument)
            {
                case "SAVE":
                    this.OnSave();
                    break;
                default:
                    break;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.Set();
            }
        }
        #endregion


    }
}