using System;
using Vmgr.Plugins;
using Quartz;

namespace Vmgr.TestFailingPlugin
{
    [Serializable]
    public class PluginTestFail : BasePlugin, IPluginJob
    {
        public override string Name
        {
            get { return "Test Fail Plugin"; }
        }

        public override string Description
        {
            get { return "Throws an unhandled exception."; }
        }

        public override Guid UniqueId
        {
            get { return new Guid("6c9929dc-1acc-467f-8241-a1fa9ce1df1b"); }
        }

        public override bool Schedulable
        {
            get { return true; }
        }

        public JobBuilder GetJobToRun()
        {
            return JobBuilder.Create<PluginTestFailJob>();
        }
    }
}
