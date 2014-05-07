using System;
using System.Threading;
using Quartz;
using VmgrLogger = Vmgr.Data.Biz.Logging;

namespace $rootnamespace$
{
    [DisallowConcurrentExecutionAttribute]
    public class $rootname$Job : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //  The job to run
            VmgrLogger.Logger.Logs.Log(string.Format("Executed job {0} at {1}.", context.JobDetail.Key.Name, DateTime.Now.ToString()), VmgrLogger.LogType.Info);

			//	Your code goes here.

            VmgrLogger.Logger.Logs.Log(string.Format("Completed job {0} at {1}.", context.JobDetail.Key.Name, DateTime.Now.ToString()), VmgrLogger.LogType.Info);
        }
    }
}
