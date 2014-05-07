using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Messaging
{
    public class UploadProgress
    {
        public bool IsFaulted { get; set; }
        public string Message { get; set; }
        public string Group { get; set; }
        public int PrimaryTotal { get; set; }
        public int PrimaryValue { get; set; }
        public int SecondaryTotal { get; set; }
        public int SecondaryValue { get; set; }

    }
}
