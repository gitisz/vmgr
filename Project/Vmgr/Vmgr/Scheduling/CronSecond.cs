using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Scheduling
{
    public class CronSecond : Cron
    {
        public bool IsSimple { get; private set; }

        public int Second { get; private set; }

        public CronSecond()
        {
        }

        public CronSecond(string expression)
        {
            string[] exp = expression.Split(' ');
            string[] sec = exp[0].Split('/');

            if (sec.Length == 2)
            {
                this.IsSimple = true;
                this.Second = int.Parse(sec[1]);
            }
            else
            {
                this.IsSimple = false;
                this.Second = int.Parse(sec[0]);
            }
        }
    }
}
