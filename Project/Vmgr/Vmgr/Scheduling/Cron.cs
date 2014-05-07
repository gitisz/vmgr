using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;

namespace Vmgr.Scheduling
{
    /// <summary>
    /// Creating the following cron expressions was made possible with the tremendous help of http://www.cronmaker.com/.
    /// </summary>
    public class Cron
    {
        #region PRIVATE PROPERTIES

        #endregion

        #region PROTECTED PROPERTIES

        #endregion

        #region PUBLIC PROPERTIES

        #endregion

        #region CTOR

        public Cron()
        {
        }

        #endregion

        #region PRIVATE METHODS

        private string getMinutes(CronMinute minute)
        {
            return string.Format("{0}/{1}", 0, minute.Minute);
        }

        private string getMinutes(CronHour hour)
        {
            if (hour.IsSimple)
                return string.Format("{0}", 0);
            else
                return string.Format("{0}", hour.Minute);
        }

        private string getHours(CronHour hour)
        {
            if (hour.IsSimple)
                return string.Format("{0}/{1}", 0, hour.Hour);
            else
                return string.Format("{0}", hour.Hour);
        }

        private string getDays(CronDay day)
        {
            if (day.IsSimple)
                return string.Format("1/{0}", day.Day);
            else
                return string.Format("{0}", "?");
        }

        private string getModifier(CronDay day)
        {
            return string.Format("{0}", "MON-FRI");
        }

        private string getModifier(CronWeek week)
        {
            return string.Format("{0}", string.Join(",", week.WkDays));
        }

        private string getModifier(CronMonth month)
        {
            if (!month.IsMod)
                throw new ApplicationException("The month must have a modifier.");

            return string.Format("{0}#{1}", month.WkDay, month.Index);
        }

        private string getModifier(CronYear year)
        {
            return string.Format("{0}#{1}", year.WkDay, year.Index);
        }

        #endregion

        #region PROTECTED METHODS

        #endregion

        #region PUBLIC METHODS

        public string BuildExpression(CronMinute minute)
        {
            string minutes = this.getMinutes(minute);

            return string.Format("0 {0} * 1/1 * ? *", minutes);
        }

        public string BuildExpression(CronHour hour)
        {
            string minutes = this.getMinutes(hour);
            string hours = this.getHours(hour);

            if (hour.IsSimple)
                return string.Format("0 0 {0} 1/1 * ? *", hours);
            else
                return string.Format("0 {0} {1} 1/1 * ? *", minutes, hours);
        }

        public string BuildExpression(CronDay day)
        {
            string days = this.getDays(day);
            string modifier = this.getModifier(day);

            if (day.IsSimple)
                return string.Format("0 {0} {1} {2} * ? *", day.Minute, day.Hour, days);
            else
                return string.Format("0 {0} {1} {2} * {3} *", day.Minute, day.Hour, days, modifier);
        }

        public string BuildExpression(CronWeek week)
        {
            //0 0 12 ? * MON,FRI,SAT *
            string modifier = this.getModifier(week);

            return string.Format("0 {0} {1} ? * {2} *", week.Minute, week.Hour, modifier);
        }

        public string BuildExpression(CronMonth month)
        {
            if (month.IsMod)
            {
                return string.Format("0 {0} {1} ? 1/{2} {3} *", month.Minute, month.Hour, month.Occur, this.getModifier(month));
            }
            else
                return string.Format("0 {0} {1} {2} 1/{3} ? *", month.Minute, month.Hour, month.Day, month.Month);
        }

        public string BuildExpression(CronYear year)
        {
            string modifier = this.getModifier(year);

            if (year.IsMod)
                return string.Format("0 {0} {1} ? {2} {3} *", year.Minute, year.Hour, year.WkMonth, modifier);
            else
                return string.Format("0 {0} {1} {2} {3} ? *", year.Minute, year.Hour, year.Day, year.Month);
        }

        public static string GenerateDateString(DateTime value)
        {
            return string.Format("{0:dddd, MMMM} {1:dd}, {0:yyyy}, at {0:hh:mm: tt}", value, ToOrdinal(value.Day));
        }

        public static string ToOrdinal(int number)
        {
            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return number.ToString() + "<sup style=\"\">th</sup>";
            }

            switch (number % 10)
            {
                case 1:
                    return number.ToString() + "<sup style=\"\">st</sup>";
                case 2:
                    return number.ToString() + "<sup style=\"\">nd</sup>";
                case 3:
                    return number.ToString() + "<sup style=\"\">rd</sup>";
                default:
                    return number.ToString() + "<sup style=\"\">th</sup>";
            }
        }

        public static bool TryParse(string expression, out CronSecond cronSecond)
        {
            bool result = false;

            cronSecond = new CronSecond();

            result = CronExpression.IsValidExpression(expression);

            if (result)
            {
                try
                {
                    cronSecond = new CronSecond(expression);
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool TryParse(string expression, out CronMinute cronMinute)
        {
            bool result = false;

            cronMinute = new CronMinute();

            result = CronExpression.IsValidExpression(expression);

            if (result)
            {
                try
                {
                    cronMinute = new CronMinute(expression);
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool TryParse(string expression, out CronHour cronHour)
        {
            bool result = false;

            cronHour = new CronHour();

            result = CronExpression.IsValidExpression(expression);

            if (result)
            {
                try
                {
                    cronHour = new CronHour(expression);
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool TryParse(string expression, out CronDay cronDay)
        {
            bool result = false;

            cronDay = new CronDay();

            result = CronExpression.IsValidExpression(expression);

            if (result)
            {
                try
                {
                    cronDay = new CronDay(expression);
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool TryParse(string expression, out CronWeek cronWeek)
        {
            bool result = false;

            cronWeek = new CronWeek();

            result = CronExpression.IsValidExpression(expression);

            if (result)
            {
                try
                {
                    cronWeek = new CronWeek(expression);
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool TryParse(string expression, out CronMonth cronMonth)
        {
            bool result = false;

            cronMonth = new CronMonth();

            result = CronExpression.IsValidExpression(expression);

            if (result)
            {
                try
                {
                    cronMonth = new CronMonth(expression);
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        public static bool TryParse(string expression, out CronYear cronYear)
        {
            bool result = false;

            cronYear = new CronYear();

            result = CronExpression.IsValidExpression(expression);

            if (result)
            {
                try
                {
                    cronYear = new CronYear(expression);
                }
                catch
                {
                    result = false;
                }
            }

            return result;
        }

        #endregion

        #region EVENTS

        #endregion
    }
}
