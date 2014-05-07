using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Vmgr.Data.Biz.MetaData;

namespace Vmgr.Packaging
{
    [ServiceKnownType(typeof(PackageMetaData))]
    public interface IPackage
    {
        /// <summary>
        /// The plugin name.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// A description of the plugin.  This value should be hard coded in the plugin.
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// A unique ID.
        /// </summary>
        Guid UniqueId { get; set; }

        /// <summary>
        /// The content of the package.
        /// </summary>
        byte [] Package { get; set; }
    }
}
