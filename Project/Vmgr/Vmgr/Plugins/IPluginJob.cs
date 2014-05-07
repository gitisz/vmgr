using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Plugins
{
    public interface IPluginJob : IPlugin
    {
        /// <summary>
        /// The Quartz Job that should be run according to the configured schedule.
        /// </summary>
        JobBuilder GetJobToRun();
    }
}
