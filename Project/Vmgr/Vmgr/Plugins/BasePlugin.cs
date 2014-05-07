using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Plugins
{
    public abstract class BasePlugin : IPlugin
    {
        /// <summary>
        /// The plugin name.  This value should be unique and should be hard coded in the plugin.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// A description of the plugin.  This value should be hard coded in the plugin.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// A unique ID.
        /// </summary>
        public abstract Guid UniqueId { get; }

        /// <summary>
        /// Determines if the plugin is schedulable.
        /// </summary>
        public abstract bool Schedulable { get; }

        /// <summary>
        /// Fires when a plugin is loaded.
        /// </summary>
        public virtual void Loaded()
        {
        }

        /// <summary>
        /// Fires when a plugin is unloaded from the AppDomain.
        /// </summary>
        public virtual void UnLoaded()
        {
        }
    }
}
