using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Vmgr.Data.Biz.MetaData
{
    [DataContract(Name = "AssemblyMetaData")]
    public class AssemblyMetaData
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string CompanyName { get; set; }
        [DataMember]
        public string AssemblyVersion { get; set; }
        [DataMember]
        public string AssemblyFileVersion { get; set; }
    }
}
