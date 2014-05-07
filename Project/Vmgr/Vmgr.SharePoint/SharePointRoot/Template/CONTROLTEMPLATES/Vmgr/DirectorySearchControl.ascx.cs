using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Vmgr.AD;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.SharePoint.Enumerations;

namespace Vmgr.SharePoint
{
    public interface IDirectoryEntry
    {
        string DisplayName { get; set; }
        string Eid { get; set; }
        string DomainAccount { get; set; }
        string Email { get; set; }
    }

    public class DirectoryEntry : IDirectoryEntry
    {
        public string DisplayName { get; set; }
        public string Eid { get; set; }
        public string DomainAccount { get; set; }
        public string Email { get; set; }
    }

    public enum SearchType
    {
        /// <summary>
        /// User = 1
        /// </summary>
        User = 1,

        /// <summary>
        /// Group = 2
        /// </summary>
        Group,
    }

    public partial class DirectorySearchControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<IUser> _users = null;
        private IList<IGroup> _groups = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected string search
        {
            get
            {
                string result = string.Empty;

                if (this.Request.QueryString["Search"] != null)
                    result = this.Request.QueryString["Search"];

                return result;
            }
        }

        protected SearchType searchType
        {
            get
            {
                SearchType result = SearchType.User;

                if (this.Request.QueryString["SearchType"] != null)
                    result = (SearchType)Enum.Parse(typeof(SearchType), this.Request.QueryString["SearchType"], true);

                return result;
            }
        }

        protected IList<IUser> users
        {
            get
            {
                try
                {
                    Impersonation.Impersonate(delegate
                    {
                        ActiveDirectoryContext adc = new ActiveDirectoryContext();

                        this._users = adc.Users
                            .SelectByProperty(UserSearchableProperties.DisplayName, this.textBoxDirectorySearch.Text)
                            .OrderBy(u => u.EID)
                            .ToList();

                        string[] name = this.textBoxDirectorySearch.Text.Split(' ');

                        string first = name[0];

                        string last = string.Empty;

                        if (name.Length > 1)
                            last = name[1];

                        if (name.Length > 2)
                            last = name[2];

                        if (!string.IsNullOrEmpty(first))
                        {
                            this._users = this._users.Union(adc.Users
                                .SelectByProperty(UserSearchableProperties.FirstName, first + "*")
                                .ToList())
                                .GroupBy(x => x.EID)
                                .Select(x => x.First())
                                .OrderBy(u => u.EID)
                                .ToList();
                            ;

                            //  Need to use the first argument in array to also search LastName property.
                            this._users = this._users.Union(adc.Users
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
                            this._users = this._users.Union(adc.Users
                                .SelectByProperty(UserSearchableProperties.LastName, last + "*")
                                .ToList())
                                .GroupBy(x => x.EID)
                                .Select(x => x.First())
                                .OrderBy(u => u.EID)
                                .ToList();
                            ;
                        }

                        this._users = this._users.Union(adc.Users
                            .SelectByProperty(UserSearchableProperties.LastName, this.textBoxDirectorySearch.Text + "*")
                            .ToList())
                            .GroupBy(x => x.EID)
                            .Select(x => x.First())
                            .OrderBy(u => u.EID)
                            .ToList();
                        ;

                        this._users = this._users.Union(adc.Users
                            .SelectByProperty(UserSearchableProperties.EID, this.textBoxDirectorySearch.Text)
                            .ToList())
                            .GroupBy(x => x.EID)
                            .Select(x => x.First())
                            .OrderBy(u => u.EID)
                            .ToList();
                        ;
                    }
                    );
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log(ex, LogType.Error);
                }

                return this._users;
            }
        }

        protected IList<IGroup> groups
        {
            get
            {
                try
                {
                    Impersonation.Impersonate(delegate
                    {
                        ActiveDirectoryContext adc = new ActiveDirectoryContext();

                        this._groups = adc.Groups
                            .SelectByName(this.textBoxDirectorySearch.Text + "*")
                            .ToList();
                    }
                    );
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log(ex, LogType.Error);
                }

                return this._groups;
            }
        }

        protected IList<IDirectoryEntry> results
        {
            get
            {
                IList<IDirectoryEntry> r = new List<IDirectoryEntry> { };

                switch (this.searchType)
                {
                    case SearchType.User:

                        r = this.users
                            .Select(u => (IDirectoryEntry)(new DirectoryEntry
                            {
                                DisplayName = u.DisplayName,
                                Eid = u.EID,
                                DomainAccount = "MBULOGIN\\" + u.EID,
                                Email = u.Email,
                            }
                                )
                            )
                            .ToList()
                            ;

                        break;

                    case SearchType.Group:

                        r = this.groups
                            .Select(u => (IDirectoryEntry)(new DirectoryEntry { DisplayName = u.Name, Eid = " - ", DomainAccount = "MBULOGIN\\" + u.Name }))
                            .ToList()
                            ;

                        break;

                    default:

                        break;
                }

                return r;
            }
        }

        protected string parentCallbackJs
        {
            get
            {
                string js = string.Empty;

                //  Go back to the root page, then push the result into the originating content window.
                js = string.Format("var result = new Object(); result.Eid = row.getDataKeyValue('Eid'); result.DisplayName = row.getDataKeyValue('DisplayName'); result.IsValid = true; var windowEditor = parentWindow.BrowserWindow.getWindowSecurityMembershipEditor(); if (windowEditor != null) {{ var contentFrame =  windowEditor.GetContentFrame(); if (contentFrame != null) {{ contentFrame.contentWindow.{0}(result); }}  }} parentWindow.close(null);"
                    , this.callback
                    );

                return js;
            }
        }

        protected string callback
        {
            get
            {
                if (this.Request.QueryString["Callback"] != null)
                    return this.Request.QueryString["Callback"];

                return string.Empty;
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

        internal void Search(string searchParameter)
        {
            this.textBoxDirectorySearch.Text = searchParameter;
            this.gridDirectorySearch.CurrentPageIndex = 0;
            this.gridDirectorySearch.DataBind();
        }

        #endregion

        #region EVENTS

        protected void buttonDirectorySearch_Click(object sender, EventArgs e)
        {
            this.Search(this.textBoxDirectorySearch.Text.Trim());
        }

        protected void linqDataSourceDirectorySearch_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            //  Allow the grid to be created if a search term has not yet been provided.
            if (string.IsNullOrEmpty(this.textBoxDirectorySearch.Text))
                e.Result = new List<IDirectoryEntry> { };
            else
            {
                e.Result = this.results;
            }

            if (this.searchType == SearchType.Group)
                this.gridDirectorySearch.Columns[1].Visible = false;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!Page.IsPostBack)
            {
                this.textBoxDirectorySearch.Text = this.search;
            }
        }

        #endregion
    }
}