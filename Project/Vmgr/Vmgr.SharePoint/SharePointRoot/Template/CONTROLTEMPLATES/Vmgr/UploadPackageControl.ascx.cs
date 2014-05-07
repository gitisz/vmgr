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
using Vmgr.Scheduling;
using Telerik.Web.UI.Upload;
using System.Web.UI;
using Vmgr.Operations;

namespace Vmgr.SharePoint
{
    public partial class UploadPackageControl : BaseControl
    {
        #region PRIVATE PROPERTIES

        private IList<UploadedFile> _uploadedFiles = new List<UploadedFile> { };
        private string _groupKey = string.Empty;

        #endregion

        #region PROTECTED PROPERTIES

        protected string groupKey
        {
            get
            {
                if (this.Session["UPLOAD_GROUP_KEY"] == null)
                {
                    this.Session.Add("UPLOAD_GROUP_KEY", Guid.NewGuid().ToUniqueName());
                }

                return this.Session["UPLOAD_GROUP_KEY"].ToString();
            }
        }

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        private void process()
        {
            foreach (UploadedFile file in this._uploadedFiles)
            {
                try
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        file.InputStream.CopyTo(memoryStream, 0x1000);

                        BasicHttpBinding binding = new BasicHttpBinding();

                        if ((this.Page as BasePage).server.WSProtocol.Equals("http", StringComparison.InvariantCultureIgnoreCase))
                        {
                            binding.Security.Mode = BasicHttpSecurityMode.None;
                        }

                        if ((this.Page as BasePage).server.WSProtocol.Equals("https", StringComparison.InvariantCultureIgnoreCase))
                        {
                            binding.Security.Mode = BasicHttpSecurityMode.Transport;
                        }

                        binding.SendTimeout = new TimeSpan(0, 2, 0); // Two minutes.

                        ChannelFactory<IUploadOperation> httpFactory = new ChannelFactory<IUploadOperation>(binding
                            , new EndpointAddress(string.Format("{0}://{1}:{2}/Vmgr.Operations/UploadOperation"
                                , (this.Page as BasePage).server.WSProtocol
                                , (this.Page as BasePage).server.WSFqdn
                                , (this.Page as BasePage).server.WSPort
                                )
                                )
                                )
                                ;

                        IUploadOperation uploadOperationProxy = httpFactory.CreateChannel();
                        uploadOperationProxy.Upload(memoryStream.ToArray(), this.groupKey, this.buttonCheckboxOverwrite.Checked);
                    }
                }
                catch (EndpointNotFoundException ex)
                {
                    Logger.Logs.Log("Failed to upload package.  The service does not appear to be online.", ex, LogType.Error);
                }
                catch (Exception ex)
                {
                    Logger.Logs.Log("Failed to upload package.", ex, LogType.Error);
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

            ScriptManager scriptManager = RadScriptManager.GetCurrent(this.Page);
            scriptManager.AsyncPostBackTimeout = 3600 * 30; // 15 minutes!!!
        }

        protected void ajaxPanelPackage_AjaxRequest(object sender, AjaxRequestEventArgs e)
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
                case "PLUGIN_PACKAGES_UPLOADED":
                    this.process();
                    break;
            }
        }

        protected void asycUploadPackage_FileUploaded(object sender, FileUploadedEventArgs e)
        {
            this._uploadedFiles.Add(e.File);
        }

        #endregion

    }
}