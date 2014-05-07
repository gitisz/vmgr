using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Vmgr.Data.Biz.MetaData;
using System.ServiceModel.Activation;
using Vmgr.Plugins;

namespace Vmgr.Operations
{
    [ServiceKnownType(typeof(StatusOperation))]
    [ServiceContract(Namespace = "Vmgr.Operations")]
    public interface IStatusOperation
    {
        /// <summary>
        /// A call to the service to check if it is online.  This method always returns true.
        /// </summary>
        [OperationContract]
        bool GetStatus();

        /// <summary>
        /// A call to the service to get all application domains currently running.
        /// </summary>
        [OperationContract]
        IList<AppDomainMetaData> GetAppDomains();

        /// <summary>
        /// A call to the service to get all plugins loaded in memory for a particular domain.
        /// </summary>
        [OperationContract]
        [ServiceKnownType(typeof(PluginMetaData))]
        IList<IPlugin> GetPluginsByDomain(string domainName);
    }
}
