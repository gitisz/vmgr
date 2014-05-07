using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Vmgr.Monitoring
{
    [ServiceContract(Namespace = "http://Vmgr.Monitoring")]
    public interface IMonitoring
    {
        [OperationContract]
        [ServiceKnownType(typeof(Monitor))]
        IMonitor GetMonitor();

        [OperationContract]
        void TryStart();

        [OperationContract]
        void TryStop();
    }
}
