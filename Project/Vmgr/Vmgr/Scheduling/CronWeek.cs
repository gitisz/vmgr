using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vmgr.Scheduling
{
    public class CronWeek : Cron
    {
        public string[] WkDays { get; set; }

        public int Hour { get; set; }

        public int Minute { get; set; }

        public int Second { get; set; }

        public CronWeek()
        {
        }

        public CronWeek(string expression)
        {
            string[] exp = expression.Split(' ');
            string[] sec = exp[0].Split('/');
            string[] min = exp[1].Split('/');
            string[] hrs = exp[2].Split('/');
            string[] wkd = exp[5].Split(',');

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

            this.WkDays = wkd;
        }
    }
}
