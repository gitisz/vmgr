using Quartz;
using Quartz.Impl.Calendar;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Vmgr.Scheduling;

namespace Vmgr
{
    public static class CommonExtensions
    {
        public static void Move<T>(this IList<T> list, int oldIndex, int newIndex)
        {
            var item = list[oldIndex];

            list.RemoveAt(oldIndex);

            list.Insert(newIndex, item);
        }

        public static void AddRange<T>(this IList<T> list, IEnumerable<T> items)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (items == null)
                throw new ArgumentNullException("items");
            foreach (T item in items)
                list.Add(item);
        }

        public static IList<T> CopiedKeys<T>(this IOrderedDictionary dictionary)
        {
            T[] keys = new T[dictionary.Count];
            dictionary.Keys.CopyTo(keys, 0);

            return keys.ToList<T>();
        }

        public static IList<T> CopiedValues<T>(this IOrderedDictionary dictionary)
        {
            T[] keys = new T[dictionary.Count];
            dictionary.Values.CopyTo(keys, 0);

            return keys.ToList<T>();
        }

        public static int GetIndex<T>(this IEnumerable<T> enumerable, T t)
        {
            int idx = 0;
            foreach (T item in enumerable)
            {
                if (t.Equals(item))
                    break;
                idx++;

            }

            return idx;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static Color HexToColor(this string hex, string def)
        {
            if (string.IsNullOrEmpty(def))
                def = "#FFFFFF";

            if (string.IsNullOrEmpty(hex))
                hex = def;

            return Color.FromArgb(int.Parse(hex.Replace("#", ""), NumberStyles.HexNumber));
        }

        public static string ColorToHex(this Color color)
        {
            return System.Drawing.ColorTranslator.ToHtml(color);
        }

        public static string ToPascalCase(this string source)
        {
            //if nothing is proivided throw a null argument exception
            if (source == null) throw new ArgumentNullException("source", "Null text cannot be converted!");

            if (source.Length == 0) return source;

            //split the provided string into an array of words
            string[] words = source.Split(' ');

            //loop through each word in the array
            for (int i = 0; i < words.Length; i++)
            {
                //if the current word is greater than 1 character long
                if (words[i].Length > 0)
                {
                    //grab the current word
                    string word = words[i];

                    //convert the first letter in the word to uppercase
                    char firstLetter = char.ToUpper(word[0]);

                    //concantenate the uppercase letter to the rest of the word
                    words[i] = firstLetter + word.Substring(1);
                }
            }

            //return the converted text
            return string.Join(string.Empty, words);
        }

        public static string HumanizeString(this string source)
        {
            StringBuilder builder = new StringBuilder();
            char c = '\0';
            foreach (char ch2 in source)
            {
                if (char.IsLower(c) && char.IsUpper(ch2))
                {
                    builder.Append(' ');
                }
                builder.Append(ch2);
                c = ch2;
            }
            return builder.ToString();
        }

        public static string HumanizeString(this string source, string delimiter)
        {
            StringBuilder builder = new StringBuilder();
            char c = '\0';
            foreach (char ch2 in source)
            {
                if (char.IsLower(c) && char.IsUpper(ch2))
                {
                    builder.Append(' ');
                    builder.Append(delimiter);
                    builder.Append(' ');
                }
                builder.Append(ch2);
                c = ch2;
            }
            return builder.ToString();
        }

        public static string ToUniqueName(this Guid guid)
        {
            return Regex.Replace(Convert.ToBase64String(guid.ToByteArray()).ToString(), @"[^\w\s]", string.Empty);
        }

        public static Color HexToColor(this string hexValue)
        {
            try
            {
                hexValue = hexValue.Replace("#", "");
                byte position = 0;
                byte alpha = System.Convert.ToByte("ff", 16);

                if (hexValue.Length == 8)
                {
                    // get the alpha channel value
                    alpha = System.Convert.ToByte(hexValue.Substring(position, 2), 16);
                    position = 2;
                }

                // get the red value
                byte red = System.Convert.ToByte(hexValue.Substring(position, 2), 16);
                position += 2;

                // get the green value
                byte green = System.Convert.ToByte(hexValue.Substring(position, 2), 16);
                position += 2;

                // get the blue value
                byte blue = System.Convert.ToByte(hexValue.Substring(position, 2), 16);

                // create the Color object
                Color color = Color.FromArgb(alpha, red, green, blue);

                // create the SolidColorBrush object
                return color;
            }
            catch
            {
                return Color.FromArgb(255, 251, 237, 187);
            }
        }

        static char[] hexDigits = {
         '0', '1', '2', '3', '4', '5', '6', '7',
         '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

        public static string ColorToHexString(this Color color)
        {
            byte[] bytes = new byte[3];
            bytes[0] = color.R;
            bytes[1] = color.G;
            bytes[2] = color.B;

            char[] chars = new char[bytes.Length * 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }

            return "#" + new string(chars);
        }

        public static int ToInt32(this Guid source)
        {
            byte[] gb = source.ToByteArray();

            return BitConverter.ToInt32(gb, 0);
        }

        public static bool Contains(this string source, string str, StringComparison comparison)
        {
            return source.IndexOf(str, comparison) >= 0;
        }

        public static string ToSlug(this string s)
        {
            string input = s;
            input = Regex.Replace(input, "[^a-zA-Z0-9]", "-");
            input = input.Replace("----", "-");
            input = input.Replace("---", "-");
            input = input.Replace("--", "-");
            return input.ToLower();
        }

        public static T ChangeType<T>(this object value)
        {
            Type conversionType = typeof(T);

            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null || (conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)) && Convert.ToString(value) == ""))
                {
                    return default(T);
                }
                conversionType = Nullable.GetUnderlyingType(conversionType);
            }

            return (T)Convert.ChangeType(value, conversionType);
        }

        public static T? ToNullable<T>(this object s) where T : struct
        {
            object value = null;

            if (s == null)
                return null;

            Type t = typeof(T);

            if (s is string)
            {
                if (string.IsNullOrEmpty(s as string))
                    return null;
            }

            if (t == typeof(int))
            {
                int i;

                if (int.TryParse(s.ToString(), out i))
                    value = i;
                else
                    return null;
            }

            if (t == typeof(double))
            {
                value = double.Parse(s.ToString());
            }

            if (t == typeof(long))
            {
                value = long.Parse(s.ToString());
            }

            if (t == typeof(decimal))
            {
                value = decimal.Parse(s.ToString());
            }

            if (t == typeof(bool))
            {
                bool b;
                if (bool.TryParse(s.ToString(), out b))
                    value = b;
                else
                    return null;
            }

            if (t == typeof(DateTime))
            {
                DateTime d;
                if (DateTime.TryParse(s.ToString(), out d))
                    value = d;
                else
                    return null;
            }

            if (t == typeof(Guid))
            {
                try
                {
                    value = new Guid(s.ToString());
                }
                catch
                {
                    value = Guid.Empty;
                }
            }

            //  etc...
            return (T)value;
        }

        public static string RecurseInnerException(this Exception ex)
        {
            return ex.RecurseInnerException("");
        }

        public static string RecurseInnerException(this Exception ex, string msg)
        {
            if (ex.InnerException != null)
                msg = ex.InnerException.RecurseInnerException(ex.Message);

            msg += string.Format("\r\nEXCEPTION MESSAGE: {0}"
                , ex.Message
                );

            return msg;
        }

        public static string EncodeJsString(this string s)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"");
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            sb.Append("\"");

            return sb.ToString();
        }

        public static void CopyTo(this Stream source, Stream target, int memSize)
        {
            byte[] buffer = new byte[memSize];
            int bytesRead = 0;
            int buffSize = buffer.Length;

            while ((bytesRead = source.Read(buffer, 0, buffSize)) > 0)
                target.Write(buffer, 0, bytesRead);
        }

        public static IEnumerable<KeyValuePair<string, string>> ToPairs(this NameValueCollection collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException("collection");
            }

            return collection.Cast<string>().Select(key => new KeyValuePair<string, string>(key, collection[key]));
        }

        public static string ToKey(this string value, string suffix)
        {
            return string.Format("{0}_{1}", value.ToSlug().ToUpper().Replace("-", "_"), suffix);
        }

        public static string RemoveDomain(this string value)
        {
            if (value.IndexOf("\\") > 0)
                return value.Substring(value.IndexOf("\\") + 1);

            return value;
        }

    }

    public static class XmlExtensions
    {
        public static string GetElementValue(this XElement element)
        {
            if (element == null)
                return string.Empty;

            return element.Value.Trim();
        }

        public static string GetXml(this XmlReader reader)
        {
            StringBuilder sb = new StringBuilder();

            if (reader != null)
            {
                while (reader.Read())
                    sb.AppendLine(reader.ReadOuterXml());
            }

            return sb.ToString();
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            System.Globalization.CultureInfo ci = System.Threading.Thread.CurrentThread.CurrentCulture;
            DayOfWeek fdow = ci.DateTimeFormat.FirstDayOfWeek;
            return DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - fdow));
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, 1);
        }

        public static DateTime EndOfMonth(this DateTime dt)
        {
            return dt.StartOfMonth().AddMonths(1).AddDays(-1);
        }

        public static string ToFormattedDateTimeString(this DateTime dt)
        {
            string dateTimeFormat = "MM/dd/yyyy HH:mm:ss";

            return dt.ToString(dateTimeFormat);
        }

        public static string ToFormattedDateString(this DateTime dt)
        {
            string dateTimeFormat = "MM/dd/yyyy";

            return dt.ToString(dateTimeFormat);
        }
    }

    public static class SchedulerExtensions
    {
        /// <summary>
        /// The original ITrigger implementations don't correctly calculate next fire time based upon a supplied calendar.  This 
        /// is because the internals of these implementations make calls to their public method to perform other calculations.  This 
        /// extension provides a correct calculation without interfering with the source.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="afterTimeUtc"></param>
        /// <param name="cal"></param>
        /// <returns></returns>
        public static DateTimeOffset? GetFireTimeAfter(this ITrigger t, DateTimeOffset? afterTimeUtc, ICalendar cal)
        {
            DateTimeOffset? d = t.GetFireTimeAfter(afterTimeUtc);

            while (d.HasValue && cal != null && !cal.IsTimeIncluded(d.Value))
            {
                d = t.GetFireTimeAfter(d);

                if (!d.HasValue)
                {
                    break;
                }

                int maxYear = DateTime.Now.AddYears(100).Year;

                // avoid infinite loop
                if (d.Value.Year > maxYear)
                {
                    return null;
                }
            }

            return d;
        }

        public static int WeekNumber(this DateTime date, DayOfWeek dayOfWeek)
        {
            GregorianCalendar cal = new GregorianCalendar(GregorianCalendarTypes.Localized);
            return cal.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, dayOfWeek);
        }

        public static AnnualCalendar ExcludeDays(this AnnualCalendar calendar, DateTime date, int weekInterval, DayOfWeek startWeekOfDay)
        {
            //  Nothing to do if there is not skipping interval.
            if (weekInterval == 1)
                return calendar;

            //  Begin at the next week.
            int week = 0;

            //  Exclude days for the next 100 years.
            while (week < 5200)
            {
                week++;

                DateTime start = date.AddDays(7 * week).StartOfWeek(startWeekOfDay);

                if (week % weekInterval != 0)
                {
                    for (int i = 0; i < 7; i++)
                        calendar.SetDayExcluded(start.AddDays(i), true);
                }
            }

            return calendar;
        }

        public static string GetPrimaryScheduleText(this ISchedule schedule)
        {
            string result = string.Empty;

            if (CronExpression.IsValidExpression(schedule.RecurrenceRule))
            {
                switch ((RecurrenceType)schedule.RecurrenceTypeId)
                {
                    case RecurrenceType.Minutely:

                        CronMinute cronMinute = new CronMinute();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronMinute))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        result = string.Format("This schedule will start at {0}, and will be invoked every {1} minutes."
                            , schedule.Start
                            , cronMinute.Minute
                            );

                        result = string.Format("{0} {1}."
                            , result
                            , schedule.End.HasValue ? string.Format("This schedule will end at {0}", schedule.End) : "This schedule will run indefinitely"
                            );

                        break;

                    case RecurrenceType.Hourly:

                        CronHour cronHour = new CronHour();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronHour))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronHour.IsSimple)
                        {
                            result = string.Format("This schedule will start at {0}, and will be invoked every {1} hours."
                                , schedule.Start
                                , cronHour.Hour
                                );
                        }
                        else
                        {
                            result = string.Format("This schedule will start at {0}, and will be invoked every day at {1: hh:mm tt}."
                                , schedule.Start
                                , new DateTime(
                                    DateTime.Now.Year,
                                    DateTime.Now.Month,
                                    DateTime.Now.Day,
                                    cronHour.Hour,
                                    cronHour.Minute,
                                    0
                                    )
                                );
                        }

                        result = string.Format("{0} {1}."
                            , result
                            , schedule.End.HasValue ? string.Format("This schedule will end at {0}", schedule.End) : "This schedule will run indefinitely"
                            );

                        break;

                    case RecurrenceType.Daily:

                        CronDay cronDay = new CronDay();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronDay))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronDay.IsSimple)
                        {
                            result = string.Format("This schedule will start at {0}, and will be invoked every {1} day(s) at {2: hh:mm tt}."
                                , schedule.Start
                                , cronDay.Day
                                , new DateTime(
                                    DateTime.Now.Year,
                                    DateTime.Now.Month,
                                    DateTime.Now.Day,
                                    cronDay.Hour,
                                    cronDay.Minute,
                                    0
                                    )
                                );
                        }
                        else
                        {
                            result = string.Format("This schedule will start at {0}, and will be invoked daily, Monday through Friday at {1: hh:mm tt}."
                                , schedule.Start
                                , new DateTime(
                                    DateTime.Now.Year,
                                    DateTime.Now.Month,
                                    DateTime.Now.Day,
                                    cronDay.Hour,
                                    cronDay.Minute,
                                    0
                                    )
                                );
                        }

                        result = string.Format("{0} {1}."
                            , result
                            , schedule.End.HasValue ? string.Format("This schedule will end at {0}", schedule.End) : "This schedule will run indefinitely"
                            );

                        break;

                    case RecurrenceType.Weekly:

                        CronWeek cronWeek = new CronWeek();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronWeek))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        IList<string> days = new List<string> { };

                        if (cronWeek.WkDays.Contains("MON"))
                            days.Add("Monday");

                        if (cronWeek.WkDays.Contains("TUE"))
                            days.Add("Tuesday");

                        if (cronWeek.WkDays.Contains("WED"))
                            days.Add("Wednesday");

                        if (cronWeek.WkDays.Contains("THU"))
                            days.Add("Thursday");

                        if (cronWeek.WkDays.Contains("FRI"))
                            days.Add("Friday");

                        if (cronWeek.WkDays.Contains("SAT"))
                            days.Add("Saturday");

                        if (cronWeek.WkDays.Contains("SUN"))
                            days.Add("Sunday");

                        result = string.Format("This schedule will start at {0}, and will be invoked weekly on the following week days: {1}, at {2: hh:mm tt}."
                            , schedule.Start
                            , string.Join(", ", days.ToArray())
                            , new DateTime(
                                DateTime.Now.Year,
                                DateTime.Now.Month,
                                DateTime.Now.Day,
                                cronWeek.Hour,
                                cronWeek.Minute,
                                0
                                )
                            );

                        result = string.Format("{0} {1}."
                            , result
                            , schedule.End.HasValue ? string.Format("This schedule will end at {0}", schedule.End) : "This schedule will run indefinitely"
                            );

                        break;

                    case RecurrenceType.Monthly:

                        CronMonth cronMonth = new CronMonth();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronMonth))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        IList<string> indices = new List<string> 
                            { 
                                "first", 
                                "second", 
                                "third", 
                                "fourth", 
                            }
                        ;

                        IDictionary<string, string> wkDays = new Dictionary<string, string> 
                            { 
                                {"MON", "Monday"},
                                {"TUE", "Tuesday"},
                                {"WED", "Wednesday"},
                                {"THU", "Thursday"},
                                {"FRI", "Friday"},
                                {"SAT", "Saturday"},
                                {"SUN", "Sunday"},
                            };

                        if (cronMonth.IsMod)
                        {
                            result = string.Format("This schedule will start at {0}, and will be invoked on the {1} {2}, of every {3} month(s), at {4: hh:mm tt}."
                                , schedule.Start
                                , indices[cronMonth.Index - 1]
                                , wkDays
                                    .Where(w => w.Key == cronMonth.WkDay)
                                    .Select(k => k.Value)
                                    .FirstOrDefault()
                                , cronMonth.Occur
                                , new DateTime(
                                    DateTime.Now.Year,
                                    DateTime.Now.Month,
                                    DateTime.Now.Day,
                                    cronMonth.Hour,
                                    cronMonth.Minute,
                                    0
                                    )
                                );
                        }
                        else
                        {
                            result = string.Format("This schedule will start at {0}, and will be invoked on the {1} day, of every {2} month, at {3: hh:mm tt}."
                                , schedule.Start
                                , Cron.ToOrdinal((int)cronMonth.Day)
                                , Cron.ToOrdinal((int)cronMonth.Month)
                                , new DateTime(
                                    DateTime.Now.Year,
                                    DateTime.Now.Month,
                                    DateTime.Now.Day,
                                    cronMonth.Hour,
                                    cronMonth.Minute,
                                    0
                                    )
                                );
                        }

                        result = string.Format("{0} {1}."
                            , result
                            , schedule.End.HasValue ? string.Format("This schedule will end at {0}", schedule.End) : "This schedule will run indefinitely"
                            );

                        break;

                    case RecurrenceType.Yearly:

                        CronYear cronYear = new CronYear();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronYear))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        IList<string> months = new List<string> 
                            { 
                                "January", 
                                "February", 
                                "March", 
                                "April", 
                                "May", 
                                "June", 
                                "July", 
                                "August", 
                                "September", 
                                "October", 
                                "November", 
                                "December", 
                            }
                        ;

                        IList<string> yrIndices = new List<string> 
                            { 
                                "first", 
                                "second", 
                                "third", 
                                "fourth", 
                            }
                        ;

                        IDictionary<string, string> yrWkDays = new Dictionary<string, string> 
                            { 
                                {"MON", "Monday"},
                                {"TUE", "Tuesday"},
                                {"WED", "Wednesday"},
                                {"THU", "Thursday"},
                                {"FRI", "Friday"},
                                {"SAT", "Saturday"},
                                {"SUN", "Sunday"},
                            };


                        if (cronYear.IsMod)
                        {
                            result = string.Format("This schedule will start at {0}, and will be invoked yearly on the {1} {2}, of {3}, at {4: hh:mm tt}."
                                , schedule.Start
                                , yrIndices[cronYear.Index - 1]
                                , yrWkDays
                                    .Where(w => w.Key == cronYear.WkDay)
                                    .Select(k => k.Value)
                                    .FirstOrDefault()
                                , months[cronYear.WkMonth - 1]
                                , new DateTime(
                                    DateTime.Now.Year,
                                    DateTime.Now.Month,
                                    DateTime.Now.Day,
                                    cronYear.Hour,
                                    cronYear.Minute,
                                    0
                                    )
                                );
                        }
                        else
                        {
                            result = string.Format("This schedule will start at {0}, and will be invoked every year on {1} {2}, at {3: hh:mm tt}."
                                , schedule.Start
                                , months[int.Parse(cronYear.Month) - 1]
                                , Cron.ToOrdinal((int)cronYear.Day)
                                , new DateTime(
                                    DateTime.Now.Year,
                                    DateTime.Now.Month,
                                    DateTime.Now.Day,
                                    cronYear.Hour,
                                    cronYear.Minute,
                                    0
                                    )
                                );
                        }

                        result = string.Format("{0} {1}."
                            , result
                            , schedule.End.HasValue ? string.Format("This schedule will end at {0}", schedule.End) : "This schedule will run indefinitely"
                            );

                        break;
                }
            }

            return result;
        }

        public static string GetSecondaryScheduleText(this ISchedule schedule)
        {
            string result = string.Empty;

            CronExpression cronExpression = new CronExpression(schedule.RecurrenceRule);
            DateTimeOffset? next = cronExpression.GetNextValidTimeAfter(DateTime.Now);

            if (next.HasValue)
            {
                DateTime nextAnticipated = next.Value.LocalDateTime;

                if (next.Value.LocalDateTime < schedule.Start)
                    nextAnticipated = schedule.Start;

                switch ((RecurrenceType)schedule.RecurrenceTypeId)
                {
                    case RecurrenceType.Minutely:

                        CronMinute cronMinute = new CronMinute();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronMinute))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronMinute.IsSimple)
                        {
                            nextAnticipated = schedule.Start;

                            while (nextAnticipated < DateTime.Now)
                            {
                                nextAnticipated = nextAnticipated.AddMinutes(cronMinute.Minute);
                            }
                        }

                        break;

                    case RecurrenceType.Hourly:

                        CronHour cronHour = new CronHour();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronHour))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronHour.IsSimple)
                        {
                            nextAnticipated = schedule.Start;

                            while (nextAnticipated < DateTime.Now)
                            {
                                nextAnticipated = nextAnticipated.AddHours(cronHour.Hour);
                            }
                        }

                        break;

                    case RecurrenceType.Daily:
                        break;
                    case RecurrenceType.Weekly:
                        break;
                    case RecurrenceType.Monthly:
                        break;
                    case RecurrenceType.Yearly:
                        break;
                    default:
                        break;
                }

                result = string.Format("{0}"
                    , Cron.GenerateDateString(nextAnticipated)
                    )
                    ;

            }

            return result;
        }

        public static string GetAnticipatedScheduleText(this ISchedule schedule)
        {
            string result = string.Empty;

            CronExpression cronExpression = new CronExpression(schedule.RecurrenceRule);
            DateTimeOffset? next = cronExpression.GetNextValidTimeAfter(DateTime.Now);

            if (next.HasValue)
            {
                DateTime nextAnticipated = next.Value.LocalDateTime;

                if (next.Value.LocalDateTime < schedule.Start)
                    nextAnticipated = schedule.Start;

                switch ((RecurrenceType)schedule.RecurrenceTypeId)
                {
                    case RecurrenceType.Minutely:

                        CronMinute cronMinute = new CronMinute();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronMinute))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronMinute.IsSimple)
                        {
                            nextAnticipated = schedule.Start;

                            while (nextAnticipated < DateTime.Now)
                            {
                                nextAnticipated = nextAnticipated.AddMinutes(cronMinute.Minute);
                            }
                        }

                        break;

                    case RecurrenceType.Hourly:

                        CronHour cronHour = new CronHour();

                        if (!Cron.TryParse(schedule.RecurrenceRule, out cronHour))
                            throw new ApplicationException("Unable to parse expresion to Cron.");

                        if (cronHour.IsSimple)
                        {
                            nextAnticipated = schedule.Start;

                            while (nextAnticipated < DateTime.Now)
                            {
                                nextAnticipated = nextAnticipated.AddHours(cronHour.Hour);
                            }
                        }

                        break;

                    case RecurrenceType.Daily:
                        break;
                    case RecurrenceType.Weekly:
                        break;
                    case RecurrenceType.Monthly:
                        break;
                    case RecurrenceType.Yearly:
                        break;
                    default:
                        break;
                }

                result = string.Format("{0}<br /><span>The next anticipated time this schedule will fire is <span style=\"color: #2D5594;\">{1}</span></span>"
                    , result
                    , Cron.GenerateDateString(nextAnticipated)
                    )
                    ;

                int count = 0;

                while (count < 2)
                {
                    switch ((RecurrenceType)schedule.RecurrenceTypeId)
                    {
                        case RecurrenceType.Minutely:

                            CronMinute cronMinute = new CronMinute();

                            if (!Cron.TryParse(schedule.RecurrenceRule, out cronMinute))
                                throw new ApplicationException("Unable to parse expresion to Cron.");

                            if (cronMinute.IsSimple)
                            {
                                next = nextAnticipated.AddMinutes(cronMinute.Minute);
                            }
                            else
                                next = cronExpression.GetNextValidTimeAfter(nextAnticipated);

                            break;

                        case RecurrenceType.Hourly:

                            CronHour cronHour = new CronHour();

                            if (!Cron.TryParse(schedule.RecurrenceRule, out cronHour))
                                throw new ApplicationException("Unable to parse expresion to Cron.");

                            if (cronHour.IsSimple)
                            {
                                next = nextAnticipated.AddHours(cronHour.Hour);
                            }
                            else
                                next = cronExpression.GetNextValidTimeAfter(nextAnticipated);

                            break;

                        case RecurrenceType.Daily:
                        case RecurrenceType.Weekly:
                        case RecurrenceType.Monthly:
                        case RecurrenceType.Yearly:

                            next = cronExpression.GetNextValidTimeAfter(nextAnticipated);

                            break;
                    }

                    if (next.HasValue)
                    {
                        if (next.Value.LocalDateTime > schedule.Start)
                        {
                            nextAnticipated = next.Value.LocalDateTime;
                        }
                    }

                    result = string.Format("{0} <br />&nbsp;&nbsp;&nbsp; <span>- followed by another <span style=\"color: #2D5594;\">{1}</span></span>"
                        , result
                        , Cron.GenerateDateString(nextAnticipated)
                        )
                        ;

                    count++;
                }
            }

            return result += ".";
        }
    }

    public static class MathExtensions
    {
        public static double Mean(this IList<double> values)
        {
            return values.Count == 0 ? 0 : values.Mean(0, values.Count);
        }

        public static double Mean(this IList<double> values, int start, int end)
        {
            double s = 0;

            for (int i = start; i < end; i++)
            {
                s += values[i];
            }

            return s / (end - start);
        }

        public static double Variance(this IList<double> values)
        {
            return values.Variance(values.Mean(), 0, values.Count);
        }

        public static double Variance(this IList<double> values, double mean)
        {
            return values.Variance(mean, 0, values.Count);
        }

        public static double Variance(this IList<double> values, double mean, int start, int end)
        {
            double variance = 0;

            for (int i = start; i < end; i++)
            {
                variance += Math.Pow((values[i] - mean), 2);
            }

            int n = end - start;
            if (start > 0) n -= 1;

            return variance / (n);
        }

        public static double StandardDeviation(this IList<double> values)
        {
            return values.Count == 0 ? 0 : values.StandardDeviation(0, values.Count);
        }

        public static double StandardDeviation(this IList<double> values, int start, int end)
        {
            double mean = values.Mean(start, end);
            double variance = values.Variance(mean, start, end);

            return Math.Sqrt(variance);
        }
    }
}