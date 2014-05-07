using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Packaging;

namespace Vmgr.Plugins
{
    [ServiceKnownType(typeof(PluginInspector))]
    [ServiceContract(Namespace = "http://Vmgr.Plugins")]
    public interface IPluginInspector
    {
        [OperationContract]
        [ServiceKnownType(typeof(PluginMetaData))]
        IList<IPlugin> GetPlugins();
    }
}
