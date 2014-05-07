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
	
    public partial class LogMetaData : IMetaData
    {	
		[Key]
        public virtual System.Int32 Id { get; set; }
        public virtual System.String ApplicationName { get; set; }
        public virtual System.String CorrelationId { get; set; }
        public virtual System.DateTime CreateDate { get; set; }
        public virtual System.String CreateUser { get; set; }
        public virtual System.String Exception { get; set; }
        public virtual System.String Level { get; set; }
        public virtual System.String Logger { get; set; }
        public virtual System.String Message { get; set; }
        public virtual System.String Server { get; set; }
        public virtual System.String Thread { get; set; }
        public virtual System.String ThreadId { get; set; }
		
		//	Mapped fields on related field.
		
	}
}
