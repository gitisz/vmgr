using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnvDTE;

namespace Vmgr.VisualStudioAddin
{
    public class CommandHandle
    {
        private Command _commandObject = null;

        private ExecuteDelegate _executeMethod = null;
        private StatusDelegate _statusMethod = null;

        public Command CommandObject
        {
            get { return _commandObject; }
            set { _commandObject = value; }
        }

        public ExecuteDelegate ExecuteMethod
        {
            get { return _executeMethod; }
            set { _executeMethod = value; }
        }

        public StatusDelegate StatusMethod
        {
            get { return _statusMethod; }
            set { _statusMethod = value; }
        }

        public CommandHandle()
        {
        }
    }
}
