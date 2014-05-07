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
	
    public partial class SecurityPermissionMetaData : IMetaData
    {	
		[Key]
        public virtual System.Int32 SecurityPermissionId { get; set; }
        public virtual System.String Description { get; set; }
        public virtual System.String Name { get; set; }
		
		//	Mapped fields on related field.
		
	}
}
