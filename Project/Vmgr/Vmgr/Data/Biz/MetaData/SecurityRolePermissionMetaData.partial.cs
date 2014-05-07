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
	
    public partial class SecurityRolePermissionMetaData : IMetaData
    {	
		[Key]
        public virtual System.Int32 SecurityRolePermissionId { get; set; }
        public virtual System.Boolean Active { get; set; }
        public virtual System.DateTime CreateDate { get; set; }
        public virtual System.String CreateUser { get; set; }
        public virtual System.Int32 SecurityPermissionId { get; set; }
        public virtual System.Int32 SecurityRoleId { get; set; }
        public virtual System.DateTime ?  UpdateDate { get; set; }
        public virtual System.String UpdateUser { get; set; }
		
		//	Mapped fields on related field.
		
		[DataMember]
		public System.String SecurityPermissionDescription { get; set; }
		[DataMember]
		public System.String SecurityPermissionName { get; set; }
		[DataMember]
		public System.String SecurityRoleName { get; set; }
	}
}
