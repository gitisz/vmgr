using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.CommandBars;
using System.Windows.Forms;

namespace Vmgr.VisualStudioAddin.Library.Menu.Commands
{
    public class UploadPackage: CommandBase
    {
        public UploadPackage(DTEHandler dteHandler)
            : base(dteHandler)
        {
        }

        public static UploadPackage Create(DTEHandler dteHandler, bool beginGroup, CommandBar[] commandbars)
        {
            UploadPackage instance = new UploadPackage(dteHandler);
            instance.CommandInstance = dteHandler.Menu.CreateCommand(
                "UPLOADPROJECTVMGR", 
                "&Upload V-Manager Package", 
                "Upload a V-Manager package from the current project", 
                "Global::Ctrl+Shift+Alt+B,u", 
                new ExecuteDelegate(instance.Execute),
                new StatusDelegate(instance.Status));

            dteHandler.Menu.AddToCommandBars(instance.CommandInstance, beginGroup, commandbars);

            return instance;
        }

        protected override void Execute()
        {
            VmgrFileHandle wspHandle = new VmgrFileHandle(this.DTEInstance);

            VmgrBuilderHandle vmgrBuilderTool = new VmgrBuilderHandle(this.DTEInstance);
            vmgrBuilderTool.RunVmgrBuilder(wspHandle.SelectedProject.FullPath
                , "-BuildVmgr true -UploadPackage true"
                )
                ;
        }
    }
}
