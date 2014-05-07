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
using Vmgr.Data.Biz.EntityServices;

namespace Vmgr.SharePoint
{
    public partial class SecurityRolePermissionEditorControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<SecurityRoleMetaData> _securityRoles = null;
        private IList<SecurityPermissionMetaData> _permissionSecuritys = null;
        private IList<SecurityRolePermissionMetaData> _securityRolePermissionSecuritys = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected string gridPermissionClientID
        {
            get
            {
                return this.gridPermission.ClientID;
            }
        }

        protected IList<SecurityPermissionMetaData> permissionSecuritys
        {
            get
            {
                if (this._permissionSecuritys == null)
                {
                    using (AppService app = new AppService())
                    {

                        this._permissionSecuritys = app.GetSecurityPermissions()
                            .ToList()
                            ;
                    }
                }

                return this._permissionSecuritys;
            }
        }

        protected IList<SecurityRoleMetaData> securityRoles
        {
            get
            {
                if (this._securityRoles == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._securityRoles = app.GetSecurityRoles()
                            .Where(s => s.Active)
                            .ToList()
                            ;

                        this._securityRoles.Insert(0, new SecurityRoleMetaData
                        {
                            SecurityRoleId = 0,
                            Name = "Please select...",
                        }
                        )
                        ;
                    }
                }

                return this._securityRoles;
            }
        }

        protected IList<SecurityRolePermissionMetaData> securityRolePermissionSecuritys
        {
            get
            {
                if (this._securityRolePermissionSecuritys == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._securityRolePermissionSecuritys = app.GetSecurityRolePermissions()
                            .Where(s => s.SecurityRoleId == this.comboBoxSecurityRole.SelectedValue.ToNullable<int>())
                            .ToList()
                            ;
                    }
                }

                return this._securityRolePermissionSecuritys;
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

        #endregion

        #region PUBLIC METHODS

        internal void Set()
        {
            this.comboBoxSecurityRole.DataBind();
        }

        #endregion

        #region EVENTS

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.Set();
            }
        }

        protected void ajaxPanelSecurityRolePermissionEditor_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            string[] events = e.Argument.Split(',');
            string commandName = events[0].Trim();
            string commandArgs = events[1].Trim();

            switch (commandName)
            {
                case "SAVE":

                    try
                    {
                        IList<int> permissionIds = events
                           .Skip(1)
                           .Select(s =>
                           {
                               int result = 0;

                               int.TryParse(s, out result);

                               return result;
                           }
                           )
                           .ToList()
                           ;

                        SecurityRolePermission securityRolePermission = new SecurityRolePermission
                        {
                            SecurityRole = this.securityRoles
                                .Where(sr => sr.SecurityRoleId == this.comboBoxSecurityRole.SelectedValue.ToNullable<int>())
                                .FirstOrDefault(),
                            SelectedPermissions = permissionIds,
                        }
                        ;

                        using (AppService app = new AppService())
                        {
                            if (app.Save(securityRolePermission))
                            {
                                PermissionModule.ClearPermissionCache();
                                PermissionModule.CreatePermissionCache();

                                this.ajaxPanelSecurityRolePermissionEditor.ResponseScripts.Add(string.Format("OnSaveSuccess('The permission set for the selected role <span style=\"font-weight: bold;\">{0}</span> has successfully been saved.');"
                                    , this.comboBoxSecurityRole.Text.EncodeJsString()))
                                    ;
                                
                            }
                            else
                            {
                                Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);

                                this.ajaxPanelSecurityRolePermissionEditor.ResponseScripts.Add("OnSaveFail('Failed to save permission set.  Please check log table for additional details.');");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Logger.Logs.Log(ex, LogType.Error);

                        this.ajaxPanelSecurityRolePermissionEditor.ResponseScripts.Add("OnSaveFail('Failed to save permission set.  Please check log table for additional details.');");
                    }

                    break;

                case "RESET":

                    int? securityRoleId = this.comboBoxSecurityRole.SelectedValue.ToNullable<int>();

                    this._securityRolePermissionSecuritys = null;
                    this._permissionSecuritys = null;
                    this.gridPermission.MasterTableView.Rebind();

                    break;
            }
        }

        protected void gridPermission_ItemCreated(object sender, GridItemEventArgs e)
        {
            SecurityPermissionMetaData permissionSecurity = e.Item.DataItem as SecurityPermissionMetaData;

            if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                if (permissionSecurity != null)
                {
                    bool selected = this.securityRolePermissionSecuritys
                        .Where(s => s.SecurityPermissionId == permissionSecurity.SecurityPermissionId)
                        .Where(s => s.Active)
                        .FirstOrDefault() != null;


                    RadButton buttonCheckboxSelect = e.Item.FindControl("buttonCheckboxSelect") as RadButton;
                    buttonCheckboxSelect.OnClientCheckedChanged = string.Format("function(s, e){{ var checked = s.get_checked(); if(checked){{ OnAddPermission({0}); }}else{{ OnRemovePermission({0});  }} }}"
                        , permissionSecurity.SecurityPermissionId
                        )
                        ;

                    if (this.comboBoxSecurityRole.SelectedValue == "0")
                    {
                        buttonCheckboxSelect.Enabled = false;
                    }

                    e.Item.Selected = selected;
                    buttonCheckboxSelect.Checked = selected;

                    Label labelSecurityPermissionName = e.Item.FindControl("labelSecurityPermissionName") as Label;
                    labelSecurityPermissionName.Text = permissionSecurity.Name.HumanizeString(">");

                }
            }
        }

        protected void linqDataSourceSecurityRole_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.securityRoles;
        }

        protected void linqDataSourcePermission_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.permissionSecuritys;
        }


        #endregion
    }
}