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
using Vmgr.Scheduling;
using Telerik.Web.UI.Calendar;
using Vmgr.Data.Biz.Logging;
using Newtonsoft.Json;
using Vmgr.AD;
using Vmgr.SharePoint.Enumerations;

namespace Vmgr.SharePoint
{
    public partial class SecuritiesControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private AppService _app = null;
        private IList<SecurityMembershipMetaData> _securityMemberships = null;
        private IList<SecurityRoleMetaData> _securityRoles = null;
        private IList<SecurityRolePermissionMetaData> _securityRolePermissions = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected AppService app
        {
            get
            {
                if (this._app == null)
                {
                    this._app = new AppService();
                }
                return this._app;
            }

        }

        protected IList<SecurityMembershipMetaData> SecurityMemberships
        {
            get
            {
                if (this._securityMemberships == null)
                {
                    this._securityMemberships = this.app.GetSecurityMemberships()
                        .Where(s => s.Active)
                        .ToList()
                        ;
                }

                return this._securityMemberships;
            }
        }

        protected IList<SecurityRoleMetaData> SecurityRoles
        {
            get
            {
                if (this._securityRoles == null)
                {
                    this._securityRoles = this.app.GetSecurityRoles()
                        .Where(s => s.Active)
                        .ToList()
                        ;
                }

                return this._securityRoles;
            }
        }

        protected IList<SecurityRolePermissionMetaData> SecurityRolePermissions
        {
            get
            {
                if (this._securityRolePermissions == null)
                {
                    this._securityRolePermissions = this.app.GetSecurityRolePermissions()
                        .ToList()
                        ;
                }

                return this._securityRolePermissions;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void SetOptionsCookie(bool expanded)
        {
            HttpCookie cookieOpen = new HttpCookie("SECURITY_OPTIONS");

            if (this.Request.Cookies["SECURITY_OPTIONS"] != null)
                cookieOpen = Request.Cookies["SECURITY_OPTIONS"];

            if (!string.IsNullOrEmpty(cookieOpen.Value))
                expanded = (!bool.Parse(cookieOpen.Value));

            cookieOpen.Value = expanded.ToString();
            cookieOpen.Expires = DateTime.Now.AddYears(1);

            // Add the cookie.
            this.Response.Cookies.Add(cookieOpen);

        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        #endregion

        #region EVENTS

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.optionsTd.Visible = false;

            if (this.Request.Cookies["SECURITY_OPTIONS"] != null)
            {
                bool expanded = bool.Parse(this.Request.Cookies["SECURITY_OPTIONS"].Value);

                this.optionsTd.Visible = expanded;
            }
        }

        protected void ajaxPanelSecurities_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            string command = e.Argument
                 .Split(',')
                 .FirstOrDefault()
                 ;

            string param = e.Argument
                .Split(',')
                .LastOrDefault()
                ;

            switch (command)
            {
                case "OPEN_OPTIONS":

                    this.SetOptionsCookie(true);
                    this.optionsTd.Visible = true;

                    break;

                case "CLOSE_OPTIONS":

                    this.SetOptionsCookie(false);
                    this.optionsTd.Visible = false;

                    break;


                case "REFRESH":

                    this._securityMemberships = null;
                    this.gridSecurityMembership.MasterTableView.Rebind();

                    this._securityRoles = null;
                    this.gridSecurityRole.MasterTableView.Rebind();

                    this._securityRolePermissions = null;
                    this.gridSecurityRolePermission.MasterTableView.Rebind();

                    break;

                case "DEACTIVATE_MEMBERSHIP":

                    int securityMembershipId = int.Parse(e.Argument.Split(',')[1]);

                    using (AppService app = new AppService())
                    {
                        SecurityMembershipMetaData securityMembership = app.GetSecurityMemberships()
                            .Where(s => s.SecurityMembershipId == securityMembershipId)
                            .FirstOrDefault()
                            ;

                        securityMembership.Active = false;

                        if (app.Save(securityMembership))
                        {
                            (this.Page as BasePage).Alert(string.Format("The security membership <span style=\"font-weight: bold;\">{0}</span> has been successfully deactivated."
                                , securityMembership.Account.EncodeJsString())
                                , "Security Membership Deactivate Confirmation"
                                , null
                                , AlertType.Check
                                )
                                ;

                            this._securityMemberships = null;
                            this.gridSecurityMembership.MasterTableView.Rebind();
                        }
                        else
                        {
                            Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(s => s.Message).ToArray()), LogType.Warn);

                            (this.Page as BasePage).Alert(string.Format("Failed to deactivate security membership.  Please check log table for additional details."), "Security Membership Deactivate Failure"
                                , null
                                , AlertType.Error
                                )
                                ;
                        }
                    }

                    break;

                case "DEACTIVATE_ROLE":

                    int securityRoleId = int.Parse(e.Argument.Split(',')[1]);

                    using (AppService app = new AppService())
                    {
                        SecurityRoleMetaData securityRole = app.GetSecurityRoles()
                            .Where(s => s.SecurityRoleId == securityRoleId)
                            .FirstOrDefault()
                            ;

                        securityRole.Active = false;

                        if (app.Save(securityRole))
                        {
                            (this.Page as BasePage).Alert(string.Format("The security role <span style=\"font-weight: bold;\">{0}</span> has been successfully deactivated."
                                , securityRole.Name.EncodeJsString())
                                , "Security Role Deactivate Confirmation"
                                , null
                                , AlertType.Check
                                );

                            this._securityRoles = null;
                            this.gridSecurityRole.MasterTableView.Rebind();
                        }
                        else
                        {
                            Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(s => s.Message).ToArray()), LogType.Warn);

                            (this.Page as BasePage).Alert(string.Format("Failed to deactivate security role.  Please check log table for additional details."), "Security Role Deactivate Failure"
                                , null
                                , AlertType.Error
                                )
                                ;
                        }
                    }

                    break;
            }
        }

        protected void linqDataSourceSecurityMembership_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (this.SecurityMemberships.Count == 0)
                e.Result = new List<SecurityMembershipMetaData> { };
            else
                e.Result = this.SecurityMemberships;
        }

        protected void linqDataSourceSecurityRole_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (this.SecurityRoles.Count == 0)
                e.Result = new List<SecurityRoleMetaData> { };
            else
                e.Result = this.SecurityRoles;
        }

        protected void linqDataSourceSecurityRolePermission_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            if (this.SecurityRolePermissions.Count == 0)
                e.Result = new List<SecurityRolePermissionMetaData> { };
            else
                e.Result = this.SecurityRolePermissions;
        }

        protected void gridSecurityMembership_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                SecurityMembershipMetaData securityMembership = e.Item.DataItem as SecurityMembershipMetaData;

                RadToolBar toolBar = e.Item.FindControl("toolBar") as RadToolBar;

                if (securityMembership != null)
                {
                    if (toolBar != null)
                    {
                        if (securityMembership.AccountType == 1)
                        {
                            IUser user = JsonConvert.DeserializeObject<User>(securityMembership.Account);

                            toolBar.OnClientButtonClicked =
                                string.Format("function(s, e){{ e.accountName = '{0}'; OnToolBarClientButtonClicked(s, e); }}"
                                    , user.EID.EncodeJsString()
                                    );
                            toolBar.Items[0].Text = user.DisplayName;
                        }
                        else if (securityMembership.AccountType == 2)
                        {
                            IGroup group = JsonConvert.DeserializeObject<Group>(securityMembership.Account);

                            toolBar.OnClientButtonClicked =
                                string.Format("function(s, e){{ e.accountName = '{0}'; OnToolBarClientButtonClicked(s, e); }}"
                                    , group.Name.EncodeJsString()
                                    );

                            toolBar.Items[0].Text = group.Name;
                        }

                        RadToolBarButton buttonEdit = (toolBar.Items[0] as RadToolBarDropDown).Buttons.FindItemByText("Edit membership") as RadToolBarButton;
                        buttonEdit.CommandArgument = securityMembership.SecurityMembershipId.ToString();

                        RadToolBarButton buttonDelete = (toolBar.Items[0] as RadToolBarDropDown).Buttons.FindItemByText("Deactivate membership") as RadToolBarButton;
                        buttonDelete.CommandArgument = securityMembership.SecurityMembershipId.ToString();
                    }

                    Label labelAccountType = e.Item.FindControl("labelAccountType") as Label;
                    labelAccountType.Text = securityMembership.AccountType == 1 ? "User" : "Group";
                }
            }
        }

        protected void gridSecurityRole_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                SecurityRoleMetaData securityRole = e.Item.DataItem as SecurityRoleMetaData;

                RadToolBar toolBar = e.Item.FindControl("toolBar") as RadToolBar;

                if (toolBar != null && securityRole != null)
                {
                    toolBar.OnClientButtonClicked =
                        string.Format("function(s, e){{ e.name = '{0}'; OnToolBarClientButtonClicked(s, e); }}"
                            , securityRole.Name.EncodeJsString()
                            );

                    toolBar.Items[0].Text = securityRole.Name;

                    RadToolBarButton buttonEdit = (toolBar.Items[0] as RadToolBarDropDown).Buttons.FindItemByText("Edit role") as RadToolBarButton;
                    buttonEdit.CommandArgument = securityRole.SecurityRoleId.ToString();

                    RadToolBarButton buttonDelete = (toolBar.Items[0] as RadToolBarDropDown).Buttons.FindItemByText("Deactivate role") as RadToolBarButton;
                    buttonDelete.CommandArgument = securityRole.SecurityRoleId.ToString();
                }
            }
        }

        protected void gridSecurityRolePermission_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                SecurityRolePermissionMetaData securityRolePermission = e.Item.DataItem as SecurityRolePermissionMetaData;

                Image imageActive = e.Item.FindControl("imageActive") as Image;
                Label labelSecurityPermissionName = e.Item.FindControl("labelSecurityPermissionName") as Label;

                if (imageActive != null && securityRolePermission != null)
                {
                    if (securityRolePermission.Active)
                        imageActive.ImageUrl = "/_layouts/images/Vmgr/green-checkmark-icon-16.png";
                    else
                        imageActive.ImageUrl = "/_layouts/images/Vmgr/stop-icon-16.png";

                    labelSecurityPermissionName.Text = securityRolePermission.SecurityPermissionName.HumanizeString(">");
                }
            }
        }


        #endregion
    }
}