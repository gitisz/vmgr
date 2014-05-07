using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.CustomAttributes
{
    /// <summary>
    /// If upgraded to .NET4 this should be replaced with System.ComponentModel.DataAnnotations.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class KeyAttribute : System.Attribute
    {
        public KeyAttribute()
        {
        }
    }
}
