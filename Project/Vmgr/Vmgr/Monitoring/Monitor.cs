using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Vmgr.Monitoring
{
    [DataContract(Name = "Monitor")]
    public class Monitor : IMonitor
    {
        [DataMember]
        public string PackageUniqueId { get; set; }
        [DataMember]
        public double CpuUtilization { get; set; }
        [DataMember]
        public double MemoryUtilization { get; set; }
        [DataMember]
        public double MonitoringTotalProcessorTime { get; set; }
        [DataMember]
        public double AvgMonitoringTotalProcessorTime { get; set; }
        [DataMember]
        public double MonitoringSurvivedMemorySize { get; set; }
        [DataMember]
        public DateTime Date { get; set; }
        [DataMember]
        public int Seconds { get; set; }
    }
}
