using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Vmgr.Packaging
{
    [Serializable, XmlRoot("package")]
    public class Vmgx
    {
        // Properties
        [XmlArrayItem("assembly"), XmlArray("assemblies")]
        public List<AssemblyItem> Assemblies { get; set; }

        [XmlAttribute("description")]
        public virtual string Description { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("uniqueId")]
        public virtual Guid UniqueId { get; set; }
    }
}

