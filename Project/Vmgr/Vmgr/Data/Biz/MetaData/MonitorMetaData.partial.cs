using System;
using System.Collections.Generic;
#if NET_40
using System.ComponentModel.DataAnnotations;
#else 
using Vmgr.CustomAttributes;
#endif
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Runtime.Serialization;
using Vmgr.Data.Biz;

namespace Vmgr.Data.Biz.MetaData
{
	
    public partial class MonitorMetaData : IMetaData
    {	
		[Key]
        public virtual System.Int32 MonitorId { get; set; }
        public virtual System.DateTime CreateDate { get; set; }
        public virtual System.String CreateUser { get; set; }
        public virtual System.Int32 PackageId { get; set; }
        public virtual System.DateTime ?  UpdateDate { get; set; }
        public virtual System.String UpdateUser { get; set; }
        public virtual System.String Username { get; set; }
		
		//	Mapped fields on related field.
		
		[DataMember]
		public System.Boolean PackageDeactivated { get; set; }
		[DataMember]
		public System.String PackageName { get; set; }
		[DataMember]
		public System.Guid PackageUniqueId { get; set; }
	}
}
