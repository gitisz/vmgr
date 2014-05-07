using System;
using System.Threading;
using Vmgr.Data.Biz.Logging;
using Quartz;

namespace Vmgr.TestPlugin
{
    [DisallowConcurrentExecutionAttribute]
    public class DoesSomethingJob : IJob
    {
        #region PRIVATE PROPERTIES

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        #endregion

        #region PRIVATE METHODS

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public void Execute(IJobExecutionContext context)
        {
            //  The job to run
            Logger.Logs.Log(string.Format("Executed job {0} at {1}.", context.JobDetail.Key.Name, DateTime.Now.ToString()), LogType.Info);

            //  Simulate doing something serious.
            Thread.Sleep(20000);

            Logger.Logs.Log(string.Format("Completed job {0} at {1}.", context.JobDetail.Key.Name, DateTime.Now.ToString()), LogType.Info);
        }

        #endregion

        #region EVENTS

        #endregion
    }
}
