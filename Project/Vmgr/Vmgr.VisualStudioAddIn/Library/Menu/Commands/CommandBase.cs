using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;
using Microsoft.VisualStudio.CommandBars;

namespace Vmgr.VisualStudioAddin.Library.Menu.Commands
{
    public class CommandBase
    {
        private DTEHandler _DTEInstance = null;
        private Command _commandInstance = null;
        private CommandBarControl _barControl = null;

        public DTEHandler DTEInstance
        {
            get { return _DTEInstance; }
            set { _DTEInstance = value; }
        }

        public Command CommandInstance
        {
            get { return _commandInstance; }
            set { _commandInstance = value; }
        }

        public CommandBarControl CommandControl
        {
            get { return _barControl; }
            set { _barControl = value; }
        }

        public CommandBase(DTEHandler dteHandler)
        {
            this.DTEInstance = dteHandler;
        }

        protected virtual void Execute()
        {
        }

        protected virtual vsCommandStatus Status()
        {
            vsCommandStatus result = vsCommandStatus.vsCommandStatusNinched | vsCommandStatus.vsCommandStatusSupported;
            if (this.DTEInstance.Application.Solution.IsOpen)
            {
                result = vsCommandStatus.vsCommandStatusEnabled | vsCommandStatus.vsCommandStatusSupported;
            }
            return result;
        }
    }
}
