using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    /// <summary>
    /// 时间处理工具
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// 根据所给时间计算出该时间所在的周是那一年的第几周
        /// </summary>
        /// <param name="dt">参考时间</param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime dt)
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            int weekOfYear = gc.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            return weekOfYear;
        }


        /// <summary>
        /// 根据一年中的第几周获取该周的开始日期与结束日期
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="weekNumber">周：> 0 的正整数</param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static Tuple<DateTime, DateTime> GetFirstEndDayOfWeek(int year, int weekNumber)
        {

            System.Globalization.Calendar calendar = System.Globalization.CultureInfo.CurrentCulture.Calendar;
            DateTime firstOfYear = new DateTime(year, 1, 1, calendar);
            DateTime targetDay = calendar.AddWeeks(firstOfYear, weekNumber - 1);
            DayOfWeek firstDayOfWeek = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;

            while (targetDay.DayOfWeek != firstDayOfWeek)
            {
                targetDay = targetDay.AddDays(-1);
            }

            return Tuple.Create<DateTime, DateTime>(targetDay, targetDay.AddDays(6));
        }

        /// <summary>
        /// 根据 年 和 月 获取该月的开始日期与结束日期
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月：> 0 的正整数</param>
        /// <returns></returns>
        public static Tuple<DateTime, DateTime> GetFirstEndDayOfMonth(int year, int month)
        {
            DateTime start = new DateTime(year, month, 1);
            DateTime end = start.AddMonths(1).AddDays(-1);
            return Tuple.Create<DateTime, DateTime>(start, end);
        }

        /// <summary>
        /// 计算指定年份内有多少周
        /// </summary>
        /// <param name="strYear">年</param>
        /// <returns></returns>
        public static int GetYearWeekCount(int strYear)
        {
            int weeks = GetWeekOfYear(DateTime.Parse(strYear.ToString() + "-12-31"));
            System.DateTime firstDay = DateTime.Parse((strYear + 1).ToString() + "-01-01");//计算第二年第一天
            int k = Convert.ToInt32(firstDay.DayOfWeek);//得到第二年的第一天是周几 
            if (k > 4)
            {
                //若当年的第一天是过了周四的，那么第一天所在的周是属于上一年的；否则属于当年
                return weeks;
            }
            else
            {
                return weeks - 1;
            }

        }
    }
}
