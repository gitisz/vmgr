using System;
using System.Threading;
using Quartz;
using VmgrLogger = Vmgr.Data.Biz.Logging;

namespace Vmgr.TestLongRunningPlugin
{
    [DisallowConcurrentExecutionAttribute]
    public class PluginLongRunningJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //  The job to run
            VmgrLogger.Logger.Logs.Log(string.Format("Executed job {0} at {1}.", context.JobDetail.Key.Name, DateTime.Now.ToString()), VmgrLogger.LogType.Info);

            //	Your code goes here.
            System.Threading.Thread.Sleep(20000);

            VmgrLogger.Logger.Logs.Log(string.Format("Completed job {0} at {1}.", context.JobDetail.Key.Name, DateTime.Now.ToString()), VmgrLogger.LogType.Info);
        }
    }
}
