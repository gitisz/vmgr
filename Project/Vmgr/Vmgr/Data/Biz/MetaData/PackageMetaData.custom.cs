using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Vmgr.Packaging;
using Vmgr.Plugins;

namespace Vmgr.Data.Biz.MetaData
{
    [Serializable]
    public partial class PackageMetaData : IPackage
    {
        [DataMember]
        public IList<IPlugin> Plugins { get; set; }
    }
}
