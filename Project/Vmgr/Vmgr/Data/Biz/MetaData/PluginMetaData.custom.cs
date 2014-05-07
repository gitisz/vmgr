using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vmgr.Plugins;

namespace Vmgr.Data.Biz.MetaData
{
    [Serializable]
    public partial class PluginMetaData : IPlugin
    {
        public void Loaded()
        {
            throw new NotImplementedException();
        }

        public void UnLoaded()
        {
            throw new NotImplementedException();
        }
    }
}
