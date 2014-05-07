using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Scheduling
{
    public class CronMinute : Cron
    {
        public int Second { get; set; }

        public int Minute { get; set; }

        public bool IsSimple { get; set; }

        public CronMinute()
        {
        }

        public CronMinute(string expression)
        {
            string[] exp = expression.Split(' ');
            string[] sec = exp[0].Split('/');
            string[] min = exp[1].Split('/');

            if (sec.Length == 2)
                this.Second = int.Parse(sec[1]);
            else
                this.Second = int.Parse(sec[0]);

            if (min.Length == 2)
            {
                this.IsSimple = true;
                this.Minute = int.Parse(min[1]);
            }
            else
            {
                this.IsSimple = false;
                this.Minute = int.Parse(min[0]);
            }
        }
    }
}
