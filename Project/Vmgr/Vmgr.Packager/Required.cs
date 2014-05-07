using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Packager
{
    public class Required : Attribute
    {
        public string Message { set; get; }

        public Required()
        {
        }

        public Required(string message)
        {
            this.Message = message;
        }
    }
}
