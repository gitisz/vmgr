using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Packaging;
using Vmgr.Scheduling;
using Vmgr.SharePoint.Enumerations;

namespace Vmgr.SharePoint
{
    public partial class MonitorPackageControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<PackageMetaData> _packages = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected IList<PackageMetaData> packages
        {
            get
            {
                if (this._packages == null)
                {
                    using (AppService app = new AppService())
                    {
                        this._packages = app.GetPackages()
                            .Where(p => !p.Deactivated)
                            .Where(p => p.ServerId == (this.Page as BasePage).server.ServerId)
                            .ToList()
                            ;
                    }
                }

                return this._packages;
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void confirmMonitorPackage(string param)
        {
            PackageMetaData selectedPackage = this.packages
                .Where(s => s.UniqueId == new Guid(param))
                .FirstOrDefault()
                ;

            (this.Page as BasePage).Confirm(string.Format("Are you sure you wish to monitor the package <span style=\"font-weight: bold;\">{0}</span>?", selectedPackage.Name)
                , "Confirm Monitor Package"
                , "OnConfirmMonitorPackageHandler"
                , Telerik.Charting.Styles.Unit.Pixel(500)
                , Telerik.Charting.Styles.Unit.Pixel(200)
                , Enumerations.ConfirmType.Confirm
                );
        }

        private void monitorPackage(string param)
        {
            IList<string> messages = new List<string> { };

            PackageMetaData selectedPackage = this.packages
                .Where(p => p.UniqueId == new Guid(param))
                .FirstOrDefault()
                ;

            using (AppService a = new AppService())
            {
                MonitorMetaData monitor = a.GetMonitors()
                    .Where(m => m.PackageUniqueId == selectedPackage.UniqueId)
                    .Where(m => m.Username == WindowsIdentity.GetCurrent().Name)
                    .FirstOrDefault()
                    ;

                if (monitor == null)
                {
                    monitor = new MonitorMetaData
                    {
                        PackageId = selectedPackage.PackageId,
                        Username = WindowsIdentity.GetCurrent().Name,
                    }
                    ;

                    if (!a.Save(monitor))
                    {
                        Logger.Logs.Log(string.Join(", ", a.BrokenRules.Select(r => r.Message).ToArray()), LogType.Warn);

                        (this.Page as BasePage).Alert("The was a problem selecting a package to monitor.  Please check logs to determine the cause."
                            , "Failed to Save Monitor"
                            , null
                            , AlertType.Error
                            )
                            ;

                    }
                    else
                    {

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

                            ChannelFactory<Vmgr.Monitoring.IMonitoring> httpFactory = new ChannelFactory<Vmgr.Monitoring.IMonitoring>(binding
                                , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Monitoring/{3}/MonitoringManager"
                                    , (this.Page as BasePage).server.WSProtocol
                                    , (this.Page as BasePage).server.WSFqdn
                                    , (this.Page as BasePage).server.WSPort
                                    , selectedPackage.UniqueId
                                    )
                                    )
                                    )
                                    ;
                            Vmgr.Monitoring.IMonitoring monitorProxy = httpFactory.CreateChannel();
                            monitorProxy.TryStart();
                        }
                        catch (EndpointNotFoundException ex)
                        {
                            Logger.Logs.Log("Unable to start monitoring on server.  The service does not appear to be online.", ex, LogType.Error);
                        }
                        catch (Exception ex)
                        {
                            Logger.Logs.Log("Unable to start monitoring on server.", ex, LogType.Error);
                        }

                        (this.Page as BasePage).Alert("This package can now be monitored on your dashboard."
                            , "Package Monitor Success"
                            , "OnCloseMonitorPackage"
                            , AlertType.Check
                            )
                            ;
                    }
                }
                else
                {
                    (this.Page as BasePage).Alert(string.Format("You are already monitoring the package <span style=\"font-weight: bold;\">{0}</span>. Please select another.", selectedPackage.Name)
                        , "Monitor Package Warning"
                        , null
                        , Telerik.Charting.Styles.Unit.Pixel(500)
                        , Telerik.Charting.Styles.Unit.Pixel(200)
                        , Enumerations.AlertType.Error
                        );
                }
            }
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
        }

        protected void ajaxPanelMonitorPackage_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
                case "CONFIRM_MONITOR_PACKAGE":
                    this.confirmMonitorPackage(param);
                    break;
                case "MONITOR_PACKAGE":
                    this.monitorPackage(param);
                    break;
            }
        }

        protected void gridPackage_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item
                || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem)
            {
                PackageMetaData server = e.Item.DataItem as PackageMetaData;
            }
        }

        protected void linqDataSourcePackage_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.packages;
        }

        #endregion
    }
}