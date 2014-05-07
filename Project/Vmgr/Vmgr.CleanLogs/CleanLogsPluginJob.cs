using System;
using System.Linq;
using System.Threading;
using Quartz;
using VmgrLogger = Vmgr.Data.Biz.Logging;
using Vmgr.Data.Biz;
using Vmgr.Data.Biz.MetaData;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Vmgr.CleanLogs
{
    [DisallowConcurrentExecutionAttribute]
    public class CleanLogsPluginJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //  The job to run
            VmgrLogger.Logger.Logs.Log(string.Format("Executed job {0} at {1}.", context.JobDetail.Key.Name, DateTime.Now.ToString()), VmgrLogger.LogType.Info);

            try
            {
                int days = 30;

                int.TryParse(Vmgr.Configuration.Settings.GetSetting("VMGR_CLEAN_LOG_DAYS", true), out days);

                //	Your code goes here.
                using (AppService app = new AppService())
                {
                    app.Adapter.CommandTimeOut = 120;
                    app.Delete(days);
                }
            
                VmgrLogger.SmtpLogger.Logs.Log("DELETED V-MANAGER LOGS", VmgrLogger.LogType.Info, "Vmgr.CleanLogs");

            }
            catch (Exception ex)
            {
                VmgrLogger.SmtpLogger.Logs.Log(ex, VmgrLogger.LogType.Error, "Vmgr.CleanLogs");
            }

            VmgrLogger.Logger.Logs.Log(string.Format("Completed job {0} at {1}.", context.JobDetail.Key.Name, DateTime.Now.ToString()), VmgrLogger.LogType.Info);
        }
    }
}
