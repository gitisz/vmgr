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
	
    public partial class ServerMetaData : IMetaData
    {	
		[Key]
        public virtual System.Int32 ServerId { get; set; }
        public virtual System.DateTime CreateDate { get; set; }
        public virtual System.String CreateUser { get; set; }
        public virtual System.String Description { get; set; }
        public virtual System.String Name { get; set; }
        public virtual System.String RTFqdn { get; set; }
        public virtual System.Int32 RTPort { get; set; }
        public virtual System.String RTProtocol { get; set; }
        public virtual System.Guid UniqueId { get; set; }
        public virtual System.DateTime ?  UpdateDate { get; set; }
        public virtual System.String UpdateUser { get; set; }
        public virtual System.String WSFqdn { get; set; }
        public virtual System.Int32 WSPort { get; set; }
        public virtual System.String WSProtocol { get; set; }
		
		//	Mapped fields on related field.
		
	}
}
