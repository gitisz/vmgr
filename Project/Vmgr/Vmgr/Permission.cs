using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr
{
    public enum Permission
    {
        PageDashboard = 1,
        PagePackageManager = 2,
        PageScheduler = 3,
        PageJob = 4,
        PageLogs = 5,
        PageSecurity = 6,

        EditPackage = 1000,
        MovePackage = 1001,
        PausePackage = 1002,
        DeletePackage = 1003,

        EditSchedule = 1100,
        PauseSchedule = 1101,
        DeleteSchedule = 1102,

        Data = 2000,
        Data2 = 2001,
    }
}
