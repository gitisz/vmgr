using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vmgr.Data.Biz;
using Vmgr.Plugins;

namespace Vmgr.Monitoring
{
    public class MonitoringManager : IMonitoring
    {
        public IMonitor GetMonitor()
        {
            return PluginManager<IPlugin>.Manage.Monitors
                .LastOrDefault()
                ;
        }

        public void TryStart()
        {
#if NET_40
            using (AppService app = new AppService())
            {
                int monitors = app.GetMonitors()
                    .Where(m => m.PackageUniqueId == new Guid(AppDomain.CurrentDomain.FriendlyName))
                    .Count()
                    ;

                if (monitors > 0)
                {
                    if (PluginManager<IPlugin>.Manage.IsMonitoring == false)
                        PluginManager<IPlugin>.Manage.MonitorTimer.Start();
                }
            }
#endif
        }

        public void TryStop()
        {
#if NET_40
            using (AppService app = new AppService())
            {
                int monitors = app.GetMonitors()
                    .Where(m => m.PackageUniqueId == new Guid(AppDomain.CurrentDomain.FriendlyName))
                    .Count()
                    ;

                //  Only try to stop monitoring if there are really no more people monitoring a package.
                if (monitors == 0)
                {
                    PluginManager<IPlugin>.Manage.MonitorTimer.Stop();
                    PluginManager<IPlugin>.Manage.IsMonitoring = false;
                    PluginManager<IPlugin>.Manage.Monitors.Clear();
                }
            }
#endif
        }
    }
}
