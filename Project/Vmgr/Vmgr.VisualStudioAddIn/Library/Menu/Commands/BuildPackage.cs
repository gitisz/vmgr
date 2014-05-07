using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.CommandBars;
using System.Windows.Forms;

namespace Vmgr.VisualStudioAddin.Library.Menu.Commands
{
    public class BuildPackage : CommandBase
    {
        public BuildPackage(DTEHandler dteHandler)
            : base(dteHandler)
        {
        }

        public static BuildPackage Create(DTEHandler dteHandler, bool beginGroup, CommandBar[] commandbars)
        {
            BuildPackage instance = new BuildPackage(dteHandler);
            instance.CommandInstance = dteHandler.Menu.CreateCommand(
                "BUILDPROJECTVMGR", 
                "&Build V-Manager Package", 
                "Build a V-Manager package from the current project", 
                "Global::Ctrl+Shift+Alt+B,v", 
                new ExecuteDelegate(instance.Execute),
                new StatusDelegate(instance.Status));

            dteHandler.Menu.AddToCommandBars(instance.CommandInstance, beginGroup, commandbars);

            return instance;
        }

        protected override void Execute()
        {
            VmgrFileHandle vmgrHandle = new VmgrFileHandle(this.DTEInstance);

            ProjectPaths projectPaths = new ProjectPaths(this.DTEInstance.SelectedProject);

            VmgrBuilderHandle vmgrBuilderTool = new VmgrBuilderHandle(this.DTEInstance);
            vmgrBuilderTool.RunVmgrBuilder(vmgrHandle.SelectedProject.FullPath
                , "-BuildVmgr true"
                )
                ;
        }
    }
}
