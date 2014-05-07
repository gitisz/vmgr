using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using Vmgr.Data.Biz.MetaData;

namespace Vmgr.Plugins
{
    [ServiceKnownType(typeof(PluginMetaData))]
    public interface IPlugin
    {
        /// <summary>
        /// The plugin name.  This value should be unique and should be hard coded in the plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// A description of the plugin.  This value should be hard coded in the plugin.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// A unique ID.
        /// </summary>
        Guid UniqueId { get; }

        /// <summary>
        /// Determines if the plugin is schedulable.
        /// </summary>
        bool Schedulable { get; }

        /// <summary>
        /// Fires when a plugin is loaded.
        /// </summary>
        void Loaded();

        /// <summary>
        /// Fires when a plugin is unloaded from the AppDomain.
        /// </summary>
        void UnLoaded();
    }
}
