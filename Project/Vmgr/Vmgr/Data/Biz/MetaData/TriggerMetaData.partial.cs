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
	
    public partial class TriggerMetaData : IMetaData
    {	
		[Key]
        public virtual System.Int32 TriggerId { get; set; }
        public virtual System.DateTime CreateDate { get; set; }
        public virtual System.String CreateUser { get; set; }
        public virtual System.DateTime ?  Ended { get; set; }
        public virtual System.Int32 JobId { get; set; }
        public virtual System.Boolean ?  Mayfire { get; set; }
        public virtual System.Boolean ?  Misfire { get; set; }
        public virtual System.DateTime ?  Nextfire { get; set; }
        public virtual System.DateTime ?  Previousfire { get; set; }
        public virtual System.DateTime ?  Started { get; set; }
        public virtual System.String TriggerKey { get; set; }
        public virtual System.String TriggerKeyGroup { get; set; }
        public virtual System.Int32 TriggerStatusTypeId { get; set; }
        public virtual System.DateTime ?  UpdateDate { get; set; }
        public virtual System.String UpdateUser { get; set; }
		
		//	Mapped fields on related field.
		
	}
}
