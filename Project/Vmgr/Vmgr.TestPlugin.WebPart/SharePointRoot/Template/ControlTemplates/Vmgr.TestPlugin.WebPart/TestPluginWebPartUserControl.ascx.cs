using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.ServiceModel;
using System.Drawing;
using Vmgr.Messaging;
using Vmgr.Data.Biz.MetaData;
using System.Collections.Generic;
using Vmgr.Data.Biz;
using System.Security.Principal;

namespace Vmgr.TestPlugin.WebPart.UI
{
    public partial class TestPluginWebPartUserControl : UserControl
    {
        #region PRIVATE PROPERTIES

        private IList<ServerMetaData> _servers = null;

        #endregion

        #region PROTECTED PROPERTIES

        protected string serverName
        {
            get
            {
                return this.servers
                    .Where(s => s.UniqueId == this.ServerUniqueId)
                    .Select(s => s.Name)
                    .FirstOrDefault();
            }
        }

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

        public Guid ServerUniqueId { get; set; }

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        #endregion

        #region EVENTS
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ChannelFactory<IDoesSomethingElsePlugin> httpFactory = new ChannelFactory<IDoesSomethingElsePlugin>(new BasicHttpBinding()
                    , new EndpointAddress(string.Format("http://{0}:{1}/Vmgr.TestPlugin.DoesSomethingElsePlugin/GetMessage"
                        , this.serverName
                        , "8000"
                        )
                        )
                        )
                        ;
                IDoesSomethingElsePlugin doesSomethingElsePluginProxy = httpFactory.CreateChannel();
                string message = doesSomethingElsePluginProxy.GetMessage();

                if (!string.IsNullOrEmpty(message))
                {
                    this.labelMessage.Text = message;
                    this.labelMessage.ForeColor = Color.Green;
                }
                else
                {
                    this.labelMessage.Text = "Where's my message!";
                    this.labelMessage.ForeColor = Color.Red;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                this.labelMessage.Text = string.Format("Unable to get message on server.  The service does not appear to be online. Exception: {0}", ex);
                this.labelMessage.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                this.labelMessage.Text = string.Format("Unable to get message on server. Exception: {0}", ex);
                this.labelMessage.ForeColor = Color.Red;
            }

            try
            {
                // Web service end point accessible to monitor the presence of a V-Manager package.
                ChannelFactory<IMonitoring> httpFactory = new ChannelFactory<IMonitoring>(new BasicHttpBinding()
                    , new EndpointAddress(string.Format("http://{0}:{1}/Vmgr.Services/{2}/MonitoringManager"
                        , this.serverName
                        , "8000"
                        , "0D0B21FD-7146-4986-8EDC-C26D786AD570"
                        )
                        )
                        )
                        ;
                IMonitoring monitoringProxy = httpFactory.CreateChannel();
                Monitor monitor = monitoringProxy.GetMonitor();

                if (monitor != null)
                {
                    this.labelCpuUtilization.Text = monitor.CpuUtilization.ToString() + " %";
                    this.labelAvgMonitoringTotalProcessorTime.Text = monitor.AvgMonitoringTotalProcessorTime.ToString() + " %";
                    this.labelMonitoringSurvivedMemorySize.Text = monitor.MonitoringSurvivedMemorySize.ToString() + " MB";
                    this.labelMemoryUtilization.Text = monitor.MemoryUtilization.ToString() + " %";

                    this.labelCpuUtilizationRt.Text = monitor.CpuUtilization.ToString() + " %";
                    this.labelAvgMonitoringTotalProcessorTimeRt.Text = monitor.AvgMonitoringTotalProcessorTime.ToString() + " %";
                    this.labelMonitoringSurvivedMemorySizeRt.Text = monitor.MonitoringSurvivedMemorySize.ToString() + " MB";
                    this.labelMemoryUtilizationRt.Text = monitor.MemoryUtilization.ToString() + " %";
                }
                else
                {
                    this.labelCpuUtilization.Text = "Where's my monitor!";
                    this.labelCpuUtilization.ForeColor = Color.Red;
                }
            }
            catch (EndpointNotFoundException ex)
            {
                this.labelMessage.Text = string.Format("Unable to get monitor on server.  The service does not appear to be online. Exception: {0}", ex);
                this.labelMessage.ForeColor = Color.Red;
            }
            catch (Exception ex)
            {
                this.labelMessage.Text = string.Format("Unable to get monitor on server. Exception: {0}", ex);
                this.labelMessage.ForeColor = Color.Red;
            }
        }

        #endregion
    }
}
