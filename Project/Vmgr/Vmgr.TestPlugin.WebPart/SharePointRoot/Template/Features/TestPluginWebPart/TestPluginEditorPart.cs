using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using Vmgr.Data.Biz.MetaData;

namespace Vmgr.TestPlugin.WebPart.UI.WebControls.WebParts
{
    public class TestPluginEditorPart : EditorPart
    {
        #region PRIVATE PROPERTIES

        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/Vmgr.TestPlugin.WebPart/TestPluginEditorPartUserControl.ascx";

        private TestPluginEditorPartUserControl _testPluginEditorPartUserControl;

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public event WebPartEditorEventHandler ChangesApplied;
        public event WebPartEditorEventHandler ChangesSynced;

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        protected override void CreateChildControls()
        {
            this._testPluginEditorPartUserControl = Page.LoadControl(_ascxPath) as TestPluginEditorPartUserControl;
            Controls.Add(this._testPluginEditorPartUserControl);
        }

        #endregion

        #region PUBLIC METHODS

        public override bool ApplyChanges()
        {
            try
            {
                this.EnsureChildControls();

                ServerMetaData server = this._testPluginEditorPartUserControl.GetServer();

                if (this.ChangesApplied != null)
                {
                    this.ChangesApplied(this, new ServerListEditorEventArgs(server.UniqueId));
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public override void SyncChanges()
        {
            this.EnsureChildControls();

            ServerListEditorEventArgs webPartEditorEventArgs = new ServerListEditorEventArgs();

            if (this.ChangesSynced != null)
            {
                this.ChangesSynced(this, webPartEditorEventArgs);
            }

            try
            {
                if(webPartEditorEventArgs.ServerUniqueId != null)
                    this._testPluginEditorPartUserControl.SetServer(webPartEditorEventArgs.ServerUniqueId);
            }
            catch
            {
            }
        }

        #endregion

        #region EVENTS

        #endregion

    }

    [Serializable]
    public class ServerListEditorEventArgs : EventArgs
    {
        public Guid ServerUniqueId { get; set; }

        public ServerListEditorEventArgs()
        {
            this.ServerUniqueId = Guid.Empty;
        }

        public ServerListEditorEventArgs(Guid serverUniqueId)
        {
            this.ServerUniqueId = serverUniqueId;
        }
    }

    [Serializable]
    public delegate void WebPartEditorEventHandler(object sender, ServerListEditorEventArgs e);
}
