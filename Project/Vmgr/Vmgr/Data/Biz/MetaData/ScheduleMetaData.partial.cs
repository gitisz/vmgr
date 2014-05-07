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
	
	[DataContract (Name = "ScheduleMetaData")]
    public partial class ScheduleMetaData : IMetaData
    {	
		[Key]
		[DataMember]
        public virtual System.Int32 ScheduleId { get; set; }
		[DataMember]
        public virtual System.DateTime CreateDate { get; set; }
		[DataMember]
        public virtual System.String CreateUser { get; set; }
		[DataMember]
        public virtual System.Boolean Deactivated { get; set; }
		[DataMember]
        public virtual System.String Description { get; set; }
		[DataMember]
        public virtual System.DateTime ?  End { get; set; }
        public virtual System.String Exclusions { get; set; }
		[DataMember]
        public virtual System.Int32 MisfireInstruction { get; set; }
		[DataMember]
        public virtual System.String Name { get; set; }
		[DataMember]
        public virtual System.Int32 ?  PluginId { get; set; }
		[DataMember]
        public virtual System.String RecurrenceRule { get; set; }
		[DataMember]
        public virtual System.Int32 RecurrenceTypeId { get; set; }
		[DataMember]
        public virtual System.DateTime Start { get; set; }
		[DataMember]
        public virtual System.Guid UniqueId { get; set; }
		[DataMember]
        public virtual System.DateTime ?  UpdateDate { get; set; }
		[DataMember]
        public virtual System.String UpdateUser { get; set; }
		
		//	Mapped fields on related field.
		
		[DataMember]
		public System.Guid PluginUniqueId { get; set; }
	}
}
