using System;
using System.Xml.Serialization;

namespace Vmgr.Packaging
{
    [Serializable]
    public class AssemblyItem
    {
        // Properties
        [XmlAttribute("location")]
        public string Location { get; set; }
    }
}
