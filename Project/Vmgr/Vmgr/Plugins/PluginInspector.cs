using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Vmgr.Data.Biz.MetaData;
using Vmgr.Helpers;
using Vmgr.Packaging;

namespace Vmgr.Plugins
{
    public class PluginInspector : IPluginInspector
    {
        public IList<IPlugin> GetPlugins()
        {
            return PluginManager<IPlugin>.Manage.Plugins
                .Select(p => new PluginMetaData
                {
                    Name = p.Name,
                    Description = p.Description,
                    UniqueId = p.UniqueId,
                    Schedulable = p.Schedulable,
                } as IPlugin
                )
                .ToList()
                ;
        }
    }
}