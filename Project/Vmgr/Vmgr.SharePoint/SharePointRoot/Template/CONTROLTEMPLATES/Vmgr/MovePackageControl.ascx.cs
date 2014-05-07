using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Security.Principal;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Vmgr.Configuration;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.SharePoint.Enumerations;
using Telerik.Web.UI;
using System.ServiceModel;
using Vmgr.Packaging;
using Vmgr.Plugins;
using System.Web.UI.WebControls;
using System.Text;
using Vmgr.Scheduling;
using Vmgr.Operations;
using System.Web.UI;

namespace Vmgr.SharePoint
{
    public partial class MovePackageControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<ClientServerMetaData> _servers = null;
        private PackageMetaData _package = null;
        private string _groupKey = string.Empty;
        private string _movePackageScript = string.Empty;

        #endregion

        #region PROTECTED PROPERTIES

        protected PackageMetaData package
        {
            get
            {
                if (this._package == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._package = app.GetPackages()
                            .Where(p => p.UniqueId == new Guid(this.Request.QueryString["UniqueId"]))
                            .FirstOrDefault()
                            ;
                    }
                }

                return this._package;
            }
        }

        protected IList<ClientServerMetaData> servers
        {
            get
            {
                if (this._servers == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._servers = app.GetServers()
                            .Select(s => new ClientServerMetaData
                                {
                                    HubConnectionUrl = (this.Page as BasePage).GetHubConnectionUrl(s),
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

        protected string pollingServiceUrl
        {
            get
            {
                return "/_vti_bin/Vmgr/PollingService.asmx/IsStarted";
            }
        }

        protected string groupKey
        {
            get
            {
                if (this.Session["MOVE_GROUP_KEY"] == null)
                {
                    this.Session.Add("MOVE_GROUP_KEY", Guid.NewGuid().ToUniqueName());
                }

                return this.Session["MOVE_GROUP_KEY"].ToString();
            }
        }

        protected Guid selectedServerUniqueId
        {
            get
            {
                return this.package.ServerUniqueId;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void confirmMovePackage(string param)
        {
            ServerMetaData selectedServer = this.servers
                .Where(s => s.UniqueId == new Guid(param))
                .FirstOrDefault()
                ;

            (this.Page as BasePage).Confirm(string.Format("Are you sure you wish to move the package to the server <span style=\"font-weight: bold;\">{0}</span>?", selectedServer.Name)
                , "Confirm Move Package"
                , "OnConfirmMovePackageHandler"
                , Telerik.Charting.Styles.Unit.Pixel(500)
                , Telerik.Charting.Styles.Unit.Pixel(200)
                , Enumerations.ConfirmType.Confirm
                );
        }

        private void movePackage(string param)
        {
            ServerMetaData destination = this.servers
                .Where(s => s.UniqueId == new Guid(param))
                .FirstOrDefault()
                ;

            try
            {
                BasicHttpBinding binding = new BasicHttpBinding();

                if ((this.Page as BasePage).server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    binding.Security.Mode = BasicHttpSecurityMode.None;
                }

                if ((this.Page as BasePage).server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                }

                ChannelFactory<IMoveOperation> httpFactory = new ChannelFactory<IMoveOperation>(binding
                    , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/MoveOperation"
                        , (this.Page as BasePage).server.WSProtocol
                        , (this.Page as BasePage).server.WSFqdn
                        , (this.Page as BasePage).server.WSPort
                        )
                        )
                        )
                        ;

                IMoveOperation moveOperationProxy = httpFactory.CreateChannel();
                moveOperationProxy.Move(this.package, destination.UniqueId, this.groupKey);
            }
            catch (EndpointNotFoundException ex)
            {
                Logger.Logs.Log("Failed to move package.  The service does not appear to be online.", ex, LogType.Error);

                (this.Page as BasePage).Confirm(string.Format("Failed to move package.  The source server does not appear to be online.  Would you like to assign this package to the detination server {0} anyway?", destination.Name)
                    , "Confirm Assign Package"
                    , "OnConfirmAssignPackageHandler"
                    , Telerik.Charting.Styles.Unit.Pixel(500)
                    , Telerik.Charting.Styles.Unit.Pixel(200)
                    , Enumerations.ConfirmType.Confirm
                    );
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to move package.", ex, LogType.Error);

                (this.Page as BasePage).Alert(string.Format("Failed to move package.  Please review the logs for more information.", destination.Name)
                    , "Move Package Failure"
                    , "OnMovePackageFailHandler('Test')"
                    , Telerik.Charting.Styles.Unit.Pixel(500)
                    , Telerik.Charting.Styles.Unit.Pixel(200)
                    , AlertType.Error
                    );
            }
        }

        private void loadPackageDestination(string param)
        {
            ServerMetaData destination = this.servers
                .Where(s => s.UniqueId == new Guid(param))
                .FirstOrDefault()
                ;

            try
            {
                using(AppService app = new AppService())
                {
                    this.package.ServerId = destination.ServerId;
                    
                    if(!app.Save(this.package))
                        Logger.Logs.Log(string.Join(", ", app.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);
                }

                BasicHttpBinding binding = new BasicHttpBinding();

                if ((this.Page as BasePage).server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                {
                    binding.Security.Mode = BasicHttpSecurityMode.None;
                }

                if ((this.Page as BasePage).server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                {
                    binding.Security.Mode = BasicHttpSecurityMode.Transport;
                }

                ChannelFactory<IMoveOperation> httpFactory = new ChannelFactory<IMoveOperation>(binding
                    , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/MoveOperation"
                        , destination.WSProtocol
                        , destination.WSFqdn
                        , destination.WSPort
                        )
                        )
                        )
                        ;

                IMoveOperation moveOperationProxy = httpFactory.CreateChannel();
                moveOperationProxy.Load(this.package, this.groupKey);
            }
            catch (EndpointNotFoundException ex)
            {
                Logger.Logs.Log("Failed to load package on destination server.  The service does not appear to be online.", ex, LogType.Error);

                (this.Page as BasePage).Alert(string.Format("Failed to load package on destination server.  The source server does not appear to be online, however the package has been assigned to server {0}.", destination.Name)
                    , "Package Load Failure"
                    , string.Format("OnPackageAssignedHandler('{0}')", destination.UniqueId.ToString())
                    , Telerik.Charting.Styles.Unit.Pixel(500)
                    , Telerik.Charting.Styles.Unit.Pixel(200)
                    , Enumerations.AlertType.Error
                    );
            }
            catch (Exception ex)
            {
                Logger.Logs.Log("Failed to load package on destination server.", ex, LogType.Error);

                (this.Page as BasePage).Alert(string.Format("Failed to load package on destination server.  Please review the logs for more information.", destination.Name)
                    , "Package Assigned Failure"
                    , "OnPackageAssignedFailHandler"
                    , Telerik.Charting.Styles.Unit.Pixel(500)
                    , Telerik.Charting.Styles.Unit.Pixel(200)
                    , AlertType.Error
                    );
            }
        }

        private void set()
        {
            this.labelPackage.Text = this.package.Name;
            this.labelServer.Text = this.servers.Where(s => s.UniqueId == this.selectedServerUniqueId)
                .Select(s => s.Name)
                .FirstOrDefault();

            this.hiddenFieldSelectedServer.Value = this.selectedServerUniqueId.ToString();
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

            this.set();
        }

        protected void ajaxPanelMovePackage_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
                case "CONFIRM_MOVE_PACKAGE":
                    this.confirmMovePackage(param);
                    break;

                case "LOAD_PACKAGE_DESTINATION":
                    this.loadPackageDestination(param);
                    break;
                case "MOVE_PACKAGE":
                    this.movePackage(param);
                    break;
                case "REFRESH":
                    this._package = null;
                    this.set();
                    break;
            }
        }

        protected void gridServer_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item
                || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
            {
                ServerMetaData server = e.Item.DataItem as ServerMetaData;

                if (server.UniqueId == this.package.ServerUniqueId)
                    e.Item.Selected = true;

                Control imageServerStatus = e.Item.FindControl("imageServerStatus") as Control;
                Control progressUpdateMessage = e.Item.FindControl("progressUpdateMessage") as Control;
                Control progressUpdateBar = e.Item.FindControl("progressUpdateBar") as Control;
                
                this._movePackageScript += ResourceHelper.UnpackEmbeddedResourceToString("Vmgr.SharePoint.Scripts.movePackage.js")
                    .Replace("[UNIQUEID]", server.UniqueId.ToString())
                    .Replace("[SERVERID]", server.ServerId.ToString())
                    .Replace("[HUBCONNECTION]", (server as ClientServerMetaData).HubConnectionUrl)
                    .Replace("[GROUPKEY]", this.groupKey)
                    .Replace("[POLLINGSERVICEURL]", this.pollingServiceUrl)
                    .Replace("[IMAGESERVERSTATUSCLIENTID]", imageServerStatus.ClientID)
                    .Replace("[PROGRESSUPDATEMESSAGECLIENTID]", progressUpdateMessage.ClientID)
                    .Replace("[PROGRESSUPDATEBARCLIENTID]", progressUpdateBar.ClientID)
                    .Replace("[AJAXPANELMOVEPACKAGECLIENTID]", this.ajaxPanelMovePackage.ClientID)
                    .Replace("[HIDDENFIELDSELECTEDSERVERCLIENTID]", this.hiddenFieldSelectedServer.ClientID)
                    ;
            }
        }
        
        protected void gridServer_PreRender(object sender, EventArgs e)
        {
            this.ajaxPanelMovePackage.ResponseScripts.Add(this._movePackageScript);
        }

        protected void linqDataSourceServer_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.servers;
        }

        #endregion
    }
}