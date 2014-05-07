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
    public partial class SecurityMembershipEditorControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<SecurityRoleMetaData> _securityRoles = null;
        private SecurityMembershipMetaData _securityMembership = null;

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        protected IList<SecurityRoleMetaData> securityRoles
        {
            get
            {
                if (this._securityRoles == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._securityRoles = app.GetSecurityRoles()
                            .Where(p => p.Active)
                            .ToList()
                            ;
                    }
                }

                return this._securityRoles;
            }
        }

        protected string selectedSecurityRolId
        {
            get
            {
                if (this.securityMembership.SecurityRoleId > 0)
                    return string.Format("'{0}'", this.securityMembership.SecurityRoleId);

                return "null";
            }
        }

        protected int securityMembershipId
        {
            get
            {
                int id = 0;

                if (this.Request.QueryString["SecurityMembershipId"] != null)
                    int.TryParse(this.Request.QueryString["SecurityMembershipId"], out id);

                return id;
            }
        }

        protected SecurityMembershipMetaData securityMembership
        {
            get
            {
                if (this._securityMembership == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._securityMembership = app.GetSecurityMemberships()
                            .Where(s => s.SecurityMembershipId == this.securityMembershipId)
                            .FirstOrDefault() ?? new SecurityMembershipMetaData { };
                    }
                }
                return this._securityMembership;
            }
        }


        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void CheckEntity()
        {
            this.textBoxEntityName.Focus();

            bool isValid = false;

            bool.TryParse(this.hiddenFieldIsValidEntity.Value, out isValid);

            if (!String.IsNullOrEmpty(this.textBoxEntityName.Text))
            {
                string searchBy = this.textBoxEntityName.Text.Trim();

                if (!string.IsNullOrEmpty(this.hiddenFieldEntity.Value))
                    searchBy = JsonConvert.DeserializeObject<User>(this.hiddenFieldEntity.Value).EID;

                this.hiddenFieldEntity.Value = string.Empty;

                IList<IUser> users = null;

                try
                {
                    Impersonation.Impersonate(delegate
                    {
                        ActiveDirectoryContext adc = new ActiveDirectoryContext();

                        users = adc.Users
                            .SelectByProperty(UserSearchableProperties.DisplayName, searchBy)
                            .OrderBy(u => u.EID)
                            .ToList();

                        if (users.Count != 1)
                        {
                            searchBy = Regex.Replace(searchBy, @"[^\w\s]", string.Empty);

                            string[] name = searchBy.Split(' ');

                            string first = name[0];

                            string last = string.Empty;

                            if (name.Length > 1)
                                last = name[1];

                            if (name.Length > 2)
                                last = name[2];

                            if (!string.IsNullOrEmpty(first))
                            {
                                users = users.Union(adc.Users
                                    .SelectByProperty(UserSearchableProperties.FirstName, first + "*")
                                    .ToList())
                                    .GroupBy(x => x.EID)
                                    .Select(x => x.First())
                                    .OrderBy(u => u.EID)
                                    .ToList();
                                ;

                                //  Need to use the first argument in array to also search LastName property.
                                users = users.Union(adc.Users
                                    .SelectByProperty(UserSearchableProperties.LastName, first + "*")
                                    .ToList())
                                    .GroupBy(x => x.EID)
                                    .Select(x => x.First())
                                    .OrderBy(u => u.EID)
                                    .ToList();
                                ;
                            }

                            if (!string.IsNullOrEmpty(last))
                            {
                                users = users.Union(adc.Users
                                    .SelectByProperty(UserSearchableProperties.LastName, last + "*")
                                    .ToList())
                                    .GroupBy(x => x.EID)
                                    .Select(x => x.First())
                                    .OrderBy(u => u.EID)
                                    .ToList();
                                ;
                            }

                            IUser perfectMatch = users
                                .Where(u => string.Equals(first, u.FirstName, StringComparison.InvariantCultureIgnoreCase))
                                .Where(u => string.Equals(last, u.LastName, StringComparison.InvariantCultureIgnoreCase))
                                .OrderBy(u => u.EID)
                                .FirstOrDefault()
                                ;

                            if (perfectMatch != null)
                                users = new List<IUser> { perfectMatch };
                            else
                                users = users.Union(adc.Users
                                    .SelectByProperty(UserSearchableProperties.EID, searchBy)
                                    .ToList())
                                    .GroupBy(x => x.EID)
                                    .Select(x => x.First())
                                    .OrderBy(u => u.EID)
                                    .ToList();
                            ;
                        }
                    }
                    );
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log(ex, LogType.Error);
                }

                if (users != null)
                {
                    if (users.Count > 0)
                    {
                        if (users.Count() > 1 && !isValid)
                        {
                            this.ajaxPanelEntity.ResponseScripts.Add("OnShowEntitySearchWindow();");
                        }
                        else
                        {
                            IUser user = users
                                .FirstOrDefault()
                                ;

                            this.textBoxEntityName.Text = user.DisplayName;
                            this.hiddenFieldEntity.Value = JsonConvert.SerializeObject(user);
                            this.hiddenFieldIsValidEntity.Value = "true";
                            this.lblEntityResult.Text = "Entity was found, click 'Save' to complete changes.";
                            this.lblEntityResult.ForeColor = Color.Green;
                        }
                    }
                    else
                    {

                        //  Try looking for a group

                        Impersonation.Impersonate(delegate
                        {
                            ActiveDirectoryContext adc = new ActiveDirectoryContext();

                            IList<IGroup> groups = adc.Groups
                                .SelectByName(searchBy + "*")
                                .OrderBy(u => u.Name)
                                .ToList();

                            if (groups.Count() == 0)
                            {
                                this.lblEntityResult.Text = string.Format("Entity was not found, try another search.");
                                this.lblEntityResult.ForeColor = Color.Red;
                            }
                            else if (groups.Count() > 1)
                            {
                                this.lblEntityResult.Text = string.Format("Active Directory Returned {0} # Records. Please redefine search criteria.", groups.Count());
                                this.lblEntityResult.ForeColor = Color.Red;
                            }
                            else
                            {
                                IGroup group = groups
                                    .FirstOrDefault()
                                    ;

                                this.textBoxEntityName.Text = group.Name;
                                this.hiddenFieldEntity.Value = JsonConvert.SerializeObject(group);
                                this.lblEntityResult.Text = "Entity was found, click 'Save' to complete changes.";
                                this.lblEntityResult.ForeColor = Color.Green;
                                this.hiddenFieldIsValidEntity.Value = "true";
                            }
                        }
                        )
                        ;
                    }
                }
                else
                {
                    this.lblEntityResult.Text = string.Format("Entity was not found, try another search.");
                    this.lblEntityResult.ForeColor = Color.Red;
                }
            }
            else
            {
                this.lblEntityResult.Text = string.Format("Please enter a value to search.");
                this.lblEntityResult.ForeColor = Color.Red;
            }
        }

        private void OnSave()
        {
            try
            {
                SecurityRoleMetaData securityRole = this.securityRoles
                    .Where(s => s.SecurityRoleId == this.gridSecurityRole.SelectedValue.ToNullable<int>())
                    .FirstOrDefault()
                    ;

                if (securityRole == null)
                    throw new ApplicationException("Selected security role must not be undefined.");

                int accountType = 1;

                try
                {
                    if (JsonConvert.DeserializeObject<Vmgr.AD.Group>(this.hiddenFieldEntity.Value) is IGroup)
                        accountType = 2;
                }
                catch
                {
                    //  Nuthin.
                }

                this.securityMembership.SecurityRoleId = securityRole.SecurityRoleId;
                this.securityMembership.Account = this.hiddenFieldEntity.Value;
                this.securityMembership.AccountType = accountType;
                this.securityMembership.Active = true;

                using (AppService app = new AppService())
                {
                    if (app.Save(this.securityMembership))
                    {
                        PermissionModule.ClearPermissionCache();
                        PermissionModule.CreatePermissionCache();
                        
                        this.ajaxPanelEntity.ResponseScripts.Add("OnSaveSuccess('The security membership was successfully saved.');");
                    }
                    else
                    {
                        Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);

                        this.ajaxPanelEntity.ResponseScripts.Add("OnSaveFail('The was a problem saving the security membership.  Please check logs to determine the cause.');");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Logs.Log(ex, LogType.Error);

                this.ajaxPanelEntity.ResponseScripts.Add("OnSaveFail('The was a problem saving the security membership.  Please check log table for additional details.');");
            }
        }

        private void OnSet()
        {
            this.hiddenFieldIsValidEntity.Value = "false";

            if (!string.IsNullOrEmpty(this.securityMembership.Account))
            {
                if (this.securityMembership.AccountType == 1)
                {
                    IUser user = JsonConvert.DeserializeObject<User>(this.securityMembership.Account);
                    this.textBoxEntityName.Text = user.DisplayName;
                }
                else if (this.securityMembership.AccountType == 2)
                {
                    IGroup group = JsonConvert.DeserializeObject<Vmgr.AD.Group>(this.securityMembership.Account);
                    this.textBoxEntityName.Text = group.Name;
                }

                this.hiddenFieldEntity.Value = this.securityMembership.Account;
                this.hiddenFieldIsValidEntity.Value = "true";
            }
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        #endregion

        #region EVENTS

        protected void ajaxPanelEntity_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            switch (e.Argument)
            {
                case "CHECK":
                    this.CheckEntity();
                    break;
                case "SAVE":
                    this.OnSave();
                    break;
                default:
                    break;
            }
        }

        protected void linqDataSourceSecurityRole_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.securityRoles;
        }

        protected void gridSecurityRole_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item
                || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
            {
                SecurityRoleMetaData securityRole = e.Item.DataItem as SecurityRoleMetaData;

                if (this.securityMembership.SecurityRoleId > 0)
                    if (securityRole.SecurityRoleId == this.securityMembership.SecurityRoleId)
                        e.Item.Selected = true;
            }

            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Pager)
            {
                if (this.securityMembership.SecurityRoleId > 0)
                    e.Item.Display = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Page.IsPostBack)
            {
                this.OnSet();
            }
        }

        #endregion
    }
}