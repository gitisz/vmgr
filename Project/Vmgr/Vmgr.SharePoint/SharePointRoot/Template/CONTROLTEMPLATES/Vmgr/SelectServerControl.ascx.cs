using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.UI.WebControls;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.MetaData;
using Telerik.Web.UI;
using System;
using System.ServiceModel;
using Vmgr.Packaging;
using Vmgr.Configuration;
using Vmgr.Data.Biz.Logging;
using Vmgr.SharePoint.Enumerations;
using System.Web;
using Microsoft.SharePoint;

namespace Vmgr.SharePoint
{
    public class ClientServerMetaData : ServerMetaData
    {
        public string ImageServerStatusId { get; set; }
        public string HubConnectionUrl { get; set; }
    }

    public partial class ServerControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<ClientServerMetaData> _servers = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected IList<ClientServerMetaData> servers
        {
            get
            {
                if (this._servers == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._servers = app.GetServers()
                            .Select(s => new  ClientServerMetaData
                                {
                                    CreateDate = s.CreateDate,
                                    CreateUser = s.CreateUser,
                                    Description = s.Description,
                                    Name = s.Name,
                                    RTPort = s.RTPort,
                                    RTProtocol = s.RTProtocol,
                                    RTFqdn = s.RTFqdn,
                                    ServerId = s.ServerId,
                                    UniqueId = s.UniqueId,
                                    UpdateDate = s.UpdateDate,
                                    UpdateUser = s.UpdateUser,
                                    WSPort = s.WSPort,
                                    WSProtocol = s.WSProtocol,
                                    WSFqdn = s.WSFqdn,
                                }
                                )
                            .ToList()
                            ;
                    }
                }

                return this._servers;
            }
        }

        protected string redirectUrl
        {
            get
            {
                if (this.Request.QueryString["redirectUrl"] != null)
                    return this.Request.QueryString["redirectUrl"];

                return string.Empty;
            }
        }

        protected string javascriptRedirectUrl
        {
            get
            {
                if (!string.IsNullOrEmpty(this.redirectUrl))
                {
                    return string.Format("window.location = '{0}';", HttpUtility.UrlDecode(this.redirectUrl));
                }

                return string.Empty;
            }
        }

        protected string pollingServiceUrl
        {
            get
            {
                return "/_vti_bin/Vmgr/PollingService.asmx/IsStarted";
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void selectServer(string param)
        {
            //  TODO: re-write this to use cookies.
            using (AppService app = new AppService())
            {
                ServerMetaData server = app.GetServers()
                    .Where(s => s.UniqueId == new Guid(param))
                    .FirstOrDefault()
                    ;

                if (server != null)
                {
                    SPSecurity.RunWithElevatedPrivileges(() =>
                    {

                        HttpCookie cookieSelectedServer = new HttpCookie("SELECTED_SERVER");

                        if (this.Request.Cookies["SELECTED_SERVER"] != null)
                            cookieSelectedServer = Request.Cookies["SELECTED_SERVER"];

                        cookieSelectedServer.Value = param;
                        cookieSelectedServer.Expires = DateTime.Now.AddYears(1);

                        this.Response.Cookies.Add(cookieSelectedServer);

                        (this.Page as BasePage).Alert("The sever was successfullly selected."
                            , "Select Server Success"
                            , "OnSelectServerComplete"
                            , AlertType.Check
                            )
                            ;
                    }
                    )
                    ;
                }
            }

            this.gridServer.Rebind();
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        #endregion

        #region EVENTS

        protected void ajaxPanelServer_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
                case "SELECT_SERVER":
                    this.selectServer(param);
                    break;
            }

        }

        protected void gridServer_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item
                || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
            {
                ClientServerMetaData server = e.Item.DataItem as ClientServerMetaData;

                if ((this.Page as BasePage).server != null)
                    if (server.UniqueId == (this.Page as BasePage).server.UniqueId)
                        e.Item.Selected = true;

                Image imageServerStatus = e.Item.FindControl("imageServerStatus") as Image;

                Literal literalScript = e.Item.FindControl("literalScript") as Literal;
                literalScript.Text = string.Format("<script>function OnPollingServiceSuccess{0}(data, status) {{ OnPollingServiceSuccess('{1}', data, status); }}</script>", server.ServerId, imageServerStatus.ClientID);


            }
        }

        protected void linqDataSourceServer_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.servers;
        }

        #endregion
    }
}