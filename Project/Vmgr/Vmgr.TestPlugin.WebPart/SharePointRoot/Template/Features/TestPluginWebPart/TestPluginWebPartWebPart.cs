using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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
    [ToolboxItemAttribute(false)]
    [Guid("2fda2c3e-c451-4db5-b7fb-50fd6a23d797")]
    public class TestPluginWebPartWebPart : Microsoft.SharePoint.WebPartPages.WebPart
    {
        #region PRIVATE PROPERTIES

        private const string ASCX_PATH = @"~/_CONTROLTEMPLATES/Vmgr.TestPlugin.WebPart/TestPluginWebPartUserControl.ascx";

        private bool _error = false;

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        public Guid ServerUniqueId { get; set; }

        #endregion

        #region CTOR

        public TestPluginWebPartWebPart()
        {
        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// Clear all child controls and add an error message for display.
        /// </summary>
        /// <param name="ex"></param>
        private void HandleException(Exception ex)
        {
            this._error = true;
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(ex.Message));
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Create all your controls here for rendering.
        /// Try to avoid using the RenderWebPart() method.
        /// </summary>
        protected override void CreateChildControls()
        {
            if (!_error)
            {
                try
                {
                    base.CreateChildControls();

                    if (this.ServerUniqueId == Guid.Empty)
                    {
                        Controls.Add(new Label() { Text = "Please configure this web part.", ForeColor = Color.Red });
                    }

                    TestPluginWebPartUserControl control = this.Page.LoadControl(ASCX_PATH) as TestPluginWebPartUserControl;
                    control.ServerUniqueId = this.ServerUniqueId;
                    Controls.Add(control);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        public override EditorPartCollection CreateEditorParts()
        {
            TestPluginEditorPart editor = new TestPluginEditorPart();
            editor.ID = "TestPluginEditorPart";
            editor.ChangesApplied += new WebPartEditorEventHandler(editor_ChangesApplied);
            editor.ChangesSynced += new WebPartEditorEventHandler(editor_ChangesSynced);

            IList editorParts = new List<EditorPart>();
            editorParts.Add(editor);

            return new EditorPartCollection(editorParts);
        }


        #endregion

        #region EVENTS

        /// <summary>
        /// Ensures that the CreateChildControls() is called before events.
        /// Use CreateChildControls() to create your controls.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            if (!_error)
            {
                try
                {
                    base.OnLoad(e);
                    this.EnsureChildControls();

                    // Your code here...
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }

        private void editor_ChangesApplied(object sender, ServerListEditorEventArgs e)
        {
            this.ServerUniqueId = e.ServerUniqueId;
        }

        private void editor_ChangesSynced(object sender, ServerListEditorEventArgs e)
        {
            e.ServerUniqueId = this.ServerUniqueId;
        }
        
        #endregion


    }
}
