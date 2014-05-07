using System;
using System.Threading;
using Quartz;
using VmgrLogger = Vmgr.Data.Biz.Logging;

namespace Vmgr.TestFailingPlugin
{
    [DisallowConcurrentExecutionAttribute]
    public class PluginTestFailJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //  The job to run
            VmgrLogger.Logger.Logs.Log(string.Format("Executed job {0} at {1}.", context.JobDetail.Key.Name, DateTime.Now.ToString()), VmgrLogger.LogType.Info);

            System.Threading.Thread.Sleep(20000);

            //	Your code goes here.
            throw new Exception("This plugin is throwing an unhandled exception.");
        }
    }
}
