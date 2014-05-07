using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.ServiceModel;
using System.Drawing;
using Vmgr.Messaging;
using System.Collections.Generic;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Data.Biz;
using System.Security.Principal;

namespace Vmgr.TestPlugin.WebPart.UI
{
    public partial class TestPluginEditorPartUserControl : UserControl
    {
        #region PRIVATE PROPERTIES

        private IList<ServerMetaData> _servers = null;
       
        #endregion

        #region PROTECTED PROPERTIES

        protected IList<ServerMetaData> servers
        {
            get
            {
                if (this._servers == null)
                {
                    using (AppService app = new AppService(WindowsIdentity.GetCurrent()))
                    {
                        this._servers = app.GetServers()
                            .ToList();
                    }
                }

                return this._servers;
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

        internal ServerMetaData GetServer()
        {
            return this.servers
                .Where(s => s.UniqueId == new Guid(this.listBoxServers.SelectedValue))
                .FirstOrDefault()
                ;
        }

        public void SetServer(Guid serverUniqueId)
        {
            this.listBoxServers.DataBind();

            foreach (ListItem item in this.listBoxServers.Items)
            {
                if (item.Value == serverUniqueId.ToString())
                {
                    item.Selected = true;
                    break;
                }
            }
        }

        #endregion

        #region EVENTS

        protected void linqDataSourceServers_Selecting(object sender, System.Web.UI.WebControls.LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.servers;
        }

        #endregion
    }
}
