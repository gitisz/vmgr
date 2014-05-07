using System;
using Quartz;
using Vmgr.Plugins;

namespace Vmgr.TestPlugin
{
    [Serializable]
    public class DoesSomethingPlugin : BasePlugin, IPluginJob
    {
        public override string Name
        {
            get { return "A Plugin That Does Something"; }
        }

        public override string Description
        {
            get { return "Does something."; }
        }

        public override Guid UniqueId
        {
            get { return new Guid("0D0B21FD-7146-4986-8EDC-C26D786AD570"); }
        }

        public override bool Schedulable
        {
            get { return true; }
        }

        public JobBuilder GetJobToRun()
        {
            return JobBuilder.Create<DoesSomethingJob>();
        }
    }
}
