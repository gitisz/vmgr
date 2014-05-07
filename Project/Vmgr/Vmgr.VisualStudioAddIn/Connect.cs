using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Vmgr.VisualStudioAddin.Library;
using Vmgr.VisualStudioAddin.Library.Menu;
using Vmgr.VisualStudioAddin.Library.Menu.Commands;
using EnvDTE;
using EnvDTE80;
using Extensibility;
using Microsoft.VisualStudio.CommandBars;

namespace Vmgr.VisualStudioAddin
{
    [GuidAttribute("311e8dfd-2a56-4ebf-9fb3-a6321335b8ca"), ProgId("Vmgr.VisualStudioAddin")]
    public class Connect : IDTExtensibility2, IDTCommandTarget
    {
        private VmgrBuilderHandle _vmgrTool = null;
        private DTEHandler _DTEInstance = null;
        private CommandBar[] _allbars = null;
        private CommandBar[] ProjectBars = null;

        public VmgrBuilderHandle VmgrTool
        {
            get
            {
                if (_vmgrTool == null)
                {
                    _vmgrTool = new VmgrBuilderHandle(this.DTEInstance);
                }
                return _vmgrTool;
            }
            set { _vmgrTool = value; }
        }


        public DTEHandler DTEInstance
        {
            get { return _DTEInstance; }
            set { _DTEInstance = value; }
        }

        public CommandBar[] Allbars
        {
            get { return _allbars; }
            set { _allbars = value; }
        }

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>
        /// Adds the Commandbar to the context menu.
        /// </summary>
        /// <param name="application"></param>
        /// <param name="connectMode"></param>
        /// <param name="addInInst"></param>
        /// <param name="custom"></param>
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            try
            {
                this.DTEInstance = new DTEHandler((DTE2)application, (AddIn)addInInst);
             
                switch (connectMode)
                {
                    case ext_ConnectMode.ext_cm_AfterStartup:
                        CreateCommands();
                        break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Vmgr AddIn Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Adds the Commands and Commandbars to the DTE.
        /// </summary>
        /// <param name="application">DTE object</param>
        /// <param name="addInInst">AddIn Instance</param>
        //private void OnStartupComplete(DTE2 application, AddIn addInInst)
        private void CreateCommands()
        {
            CommandBar toolsMenuBar = this.DTEInstance.Menu.GetCommandBar(VSMenuConstants.MainToolsMenu);
            CommandBar projectContextBar = this.DTEInstance.Menu.GetCommandBar(VSMenuConstants.ProjectContextMenu);
            CommandBar folderContextBar = this.DTEInstance.Menu.GetCommandBar(VSMenuConstants.FolderContextMenu);
            CommandBar itemContextBar = this.DTEInstance.Menu.GetCommandBar(VSMenuConstants.ItemContextMenu);

            // Create or get the commandbar popup
            CommandBarPopup toolsVmgrBuilderPopup = this.DTEInstance.Menu.CreateCommandBarPopup("toolsVmgrBuilderPopup", "VmgrBuilder", true, 1, toolsMenuBar);
            CommandBarPopup projectVmgrBuilderPopup = this.DTEInstance.Menu.CreateCommandBarPopup("projectVmgrBuilderPopup", "VmgrBuilder", true, 1, projectContextBar);

            Allbars = new CommandBar[] { 
                projectVmgrBuilderPopup.CommandBar, 
                toolsVmgrBuilderPopup.CommandBar, 
                itemContextBar, 
                folderContextBar };

            ProjectBars = new CommandBar[] { 
                projectVmgrBuilderPopup.CommandBar, 
                toolsVmgrBuilderPopup.CommandBar };

            BuildPackage.Create(this.DTEInstance, false, ProjectBars);
            UploadPackage.Create(this.DTEInstance, false, ProjectBars);
        }

        public void QueryStatus(string commandName, vsCommandStatusTextWanted neededText, ref vsCommandStatus status, ref object commandText)
        {
            try
            {
                if (neededText == vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
                {
                    status = vsCommandStatus.vsCommandStatusNinched | vsCommandStatus.vsCommandStatusSupported;
                    
                    if (this.DTEInstance.RunningCommand)
                    {
                        status = vsCommandStatus.vsCommandStatusNinched | vsCommandStatus.vsCommandStatusSupported;
                    }
                    else
                    {
                        status = this.DTEInstance.Menu.QueryStatus(commandName);
                    }
                }
            }
            catch (Exception ex)
            {
                // Sometimes the Query status throws a lot of exceptions. Therefore do not show this, just trace it.
                Trace.WriteLine(ex.ToString(), "VmgrBuilder Extensions");
            }
        }


        public void Exec(string commandName, vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
        {
            handled = false;

            try
            {
                if (executeOption == vsCommandExecOption.vsCommandExecOptionDoDefault)
                {
                    if (this.DTEInstance.Application != null)
                    {
                        this.DTEInstance.Menu.Execute(commandName);
                        handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "VmgrBuilder AddIn Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                handled = true;

                Debug.Write(ex);
            }
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {
            try
            {
                Debug.WriteLine("OnDisconnection called!");

                if (disconnectMode == ext_DisconnectMode.ext_dm_HostShutdown ||
                    disconnectMode == ext_DisconnectMode.ext_dm_UserClosed)
                {
                    Debug.WriteLine("ext_DisconnectMode: " + disconnectMode);

                    // Important: First clear out every command control
                    //this.DTEInstance.Menu.DeleteAllCommandControls();
                    // Then clear out every command bar
                    //this.DTEInstance.Menu.DeleteCustomBarPopups();
                    foreach (CommandBarPopup commandBarPopup in this.DTEInstance.Menu.CustomBarPopups)
                    {
                        CleanUpPopup(commandBarPopup.Parent, commandBarPopup.CommandBar.Name);
                    }

                    CleanUpCommands();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "VmgrBuilder AddIn Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.Write(ex);
            }
        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
            //Log.Write(this, "OnAddInsUpdate()");
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
            CreateCommands();
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
            //Log.Write(this, "OnBeginShutdown() - Connect variables is set " + (wspBuilderMenuControl != null));
        }

        private void CleanUpPopup(CommandBar commandBar, string name)
        {
            Debug.WriteLine("Commandbars to delete: " + commandBar.Controls.Count);
            List<CommandBarPopup> popups = new List<CommandBarPopup>();
            foreach (CommandBarControl control in commandBar.Controls)
            {
                if (control is CommandBarPopup)
                {
                    popups.Add(control as CommandBarPopup);
                }
            }

            foreach (CommandBarPopup commandbarPopup in popups)
            {
                if (commandbarPopup.CommandBar.Name == name)
                {
                    CleanUpControls(commandbarPopup);
                    Debug.WriteLine("Commandbar delete: " + commandbarPopup.CommandBar.Name + " from Bar " + commandBar.Name);
                    commandbarPopup.Delete(false);
                }
            }
        }

        private void CleanUpControls(CommandBarPopup commandBar)
        {
            Debug.WriteLine("Controls delete on command bar: " + commandBar.Caption);
            
            List<CommandBarControl> barControls = new List<CommandBarControl>();
            
            foreach (CommandBarControl barControl in commandBar.Controls)
            {
                barControls.Add(barControl);
            }

            foreach (CommandBarControl barControl in barControls)
            {
                Debug.WriteLine("Control Deleted: " + barControl.Caption);
                barControl.Delete(false);
            }
        }

        private void CleanUpCommands()
        {
            Debug.WriteLine("Commands delete!");
            
            foreach (CommandHandle commandHandle in this.DTEInstance.Menu.CommandList.Values)
            {
                Debug.WriteLine("Command delete: " + commandHandle.CommandObject.Name);
                commandHandle.CommandObject.Delete();
            }
        }
    }
}
