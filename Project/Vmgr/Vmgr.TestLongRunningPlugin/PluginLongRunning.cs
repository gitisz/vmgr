using System;
using Vmgr.Plugins;
using Quartz;

namespace Vmgr.TestLongRunningPlugin
{
    [Serializable]
    public class PluginLongRunning : BasePlugin, IPluginJob
    {
        public override string Name
        {
            get { return "Test Long Plugin"; }
        }

        public override string Description
        {
            get { return "Test"; }
        }

        public override Guid UniqueId
        {
            get { return new Guid("2d72ad92-0d84-4e58-b89c-dc3771a0d10f"); }
        }

        public override bool Schedulable
        {
            get { return true; }
        }

        public JobBuilder GetJobToRun()
        {
            return JobBuilder.Create<PluginLongRunningJob>();
        }
    }
}
