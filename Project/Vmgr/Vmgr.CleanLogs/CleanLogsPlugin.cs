using System;
using Vmgr.Plugins;
using Quartz;

namespace Vmgr.CleanLogs
{
    [Serializable]
    public class CleanLogsPlugin : BasePlugin, IPluginJob
    {
        public override string Name
        {
            get { return "V-Manager Clean Logs Plugin"; }
        }

        public override string Description
        {
            get { return "Removes logs after a period of time."; }
        }

        public override Guid UniqueId
        {
            get { return new Guid("ed4f297f-17e7-41c0-bd89-cd3dc4c4c869"); }
        }

        public override bool Schedulable
        {
            get { return true; }
        }

        public JobBuilder GetJobToRun()
        {
            return JobBuilder.Create<CleanLogsPluginJob>();
        }
    }
}
