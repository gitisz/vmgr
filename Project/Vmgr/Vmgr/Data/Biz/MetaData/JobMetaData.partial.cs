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
	
    public partial class JobMetaData : IMetaData
    {	
		[Key]
        public virtual System.Int32 JobId { get; set; }
        public virtual System.DateTime CreateDate { get; set; }
        public virtual System.String CreateUser { get; set; }
        public virtual System.String JobKey { get; set; }
        public virtual System.String JobKeyGroup { get; set; }
        public virtual System.Int32 JobStatusTypeId { get; set; }
        public virtual System.Int32 ScheduleId { get; set; }
        public virtual System.DateTime ?  UpdateDate { get; set; }
        public virtual System.String UpdateUser { get; set; }
		
		//	Mapped fields on related field.
		
		[DataMember]
		public System.String ScheduleName { get; set; }
		[DataMember]
		public System.Guid ScheduleUniqueId { get; set; }
	}
}
