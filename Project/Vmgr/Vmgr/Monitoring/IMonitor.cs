using System;

namespace Vmgr.Monitoring
{
    public interface IMonitor
    {
        double AvgMonitoringTotalProcessorTime { get; set; }
        double CpuUtilization { get; set; }
        DateTime Date { get; set; }
        double MemoryUtilization { get; set; }
        double MonitoringSurvivedMemorySize { get; set; }
        double MonitoringTotalProcessorTime { get; set; }
        string PackageUniqueId { get; set; }
        int Seconds { get; set; }
    }
}
