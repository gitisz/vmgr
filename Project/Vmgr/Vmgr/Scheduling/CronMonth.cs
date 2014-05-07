using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Scheduling
{
    public class CronMonth : Cron
    {
        public bool IsMod { get; set; }

        public int Index { get; set; }

        public string WkDay { get; set; }

        public double Occur { get; set; }

        public double Day { get; set; }

        public double Month { get; set; }

        public int Hour { get; set; }

        public int Minute { get; set; }

        public int Second { get; set; }

        public CronMonth()
        {
        }

        public CronMonth(string expression)
        {
            string[] exp = expression.Split(' ');
            string[] sec = exp[0].Split('/');
            string[] min = exp[1].Split('/');
            string[] hrs = exp[2].Split('/');

            if (sec.Length == 2)
                this.Second = int.Parse(sec[1]);
            else
                this.Second = int.Parse(sec[0]);

            if (min.Length == 2)
                this.Minute = int.Parse(min[1]);
            else
                this.Minute = int.Parse(min[0]);

            if (hrs.Length == 2)
                this.Hour = int.Parse(hrs[1]);
            else
                this.Hour = int.Parse(hrs[0]);

            if (exp[3] == "?")
            {
                this.IsMod = true;
                this.Index = int.Parse(exp[5].Split('#')[1]);
                this.WkDay = exp[5].Split('#')[0];
                this.Occur = double.Parse(exp[4].Split('/')[1]);
            }
            else
            {
                this.IsMod = false;
                this.Day = double.Parse(exp[3]);
                this.Month = double.Parse(exp[4].Split('/')[1]);
            }
        }
    }
}
