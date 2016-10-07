using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Core.Common
{
    public class DateTimeHelper
    {
        private DateTime dt = DateTime.Now;
        
        /// <summary>
        /// 获取某一年有多少周
        /// </summary>
        /// <param name="year">年份</param>
        /// <returns>该年周数</returns>
        public static int GetWeekAmount(int year)
        {
            var end = new DateTime(year, 12, 31); //该年最后一天
            var gc = new GregorianCalendar();
            return gc.GetWeekOfYear(end, CalendarWeekRule.FirstDay, DayOfWeek.Monday); //该年星期数
        }

        /// <summary>
        /// 返回年度第几个星期   默认星期日是第一天
        /// </summary>
        /// <param name="date">时间</param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime date)
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            return gc.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Sunday);
        }

        /// <summary>
        /// 返回年度第几个星期
        /// </summary>
        /// <param name="date">时间</param>
        /// <param name="week">一周的开始日期</param>
        /// <returns></returns>
        public static int WeekOfYear(DateTime date, DayOfWeek week)
        {
            System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
            return gc.GetWeekOfYear(date, System.Globalization.CalendarWeekRule.FirstDay, week);
        }


        /// <summary>
        /// 得到一年中的某周的起始日和截止日
        /// 年 nYear
        /// 周数 nNumWeek
        /// 周始 out dtWeekStart
        /// 周终 out dtWeekeEnd
        /// </summary>
        /// <param name="nYear">年份</param>
        /// <param name="nNumWeek">第几周</param>
        /// <param name="dtWeekStart">开始日期</param>
        /// <param name="dtWeekeEnd">结束日期</param>
        public static void GetWeekTime(int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekeEnd)
        {
            DateTime dt = new DateTime(nYear, 1, 1);
            dt = dt + new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
            dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
            dtWeekeEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1);
        }

        /// <summary>
        /// 得到一年中的某周的起始日和截止日    周一到周五  工作日
        /// </summary>
        /// <param name="nYear">年份</param>
        /// <param name="nNumWeek">第几周</param>
        /// <param name="dtWeekStart">开始日期</param>
        /// <param name="dtWeekeEnd">结束日期</param>
        public static void GetWeekWorkTime(int nYear, int nNumWeek, out DateTime dtWeekStart, out DateTime dtWeekeEnd)
        {
            DateTime dt = new DateTime(nYear, 1, 1);
            dt = dt + new TimeSpan((nNumWeek - 1) * 7, 0, 0, 0);
            dtWeekStart = dt.AddDays(-(int)dt.DayOfWeek + (int)DayOfWeek.Monday);
            dtWeekeEnd = dt.AddDays((int)DayOfWeek.Saturday - (int)dt.DayOfWeek + 1).AddDays(-2);
        }

        #region P/Invoke 设置本地时间

        [DllImport("kernel32.dll")]
        private static extern bool SetLocalTime(ref SYSTEMTIME time);

        [StructLayout(LayoutKind.Sequential)]
        private struct SYSTEMTIME
        {
            public short year;
            public short month;
            public short dayOfWeek;
            public short day;
            public short hour;
            public short minute;
            public short second;
            public short milliseconds;
        }

        /// <summary>
        /// 设置本地计算机时间
        /// </summary>
        /// <param name="dt">DateTime对象</param>
        public static void SetLocalTime(DateTime dt)
        {
            SYSTEMTIME st;

            st.year = (short)dt.Year;
            st.month = (short)dt.Month;
            st.dayOfWeek = (short)dt.DayOfWeek;
            st.day = (short)dt.Day;
            st.hour = (short)dt.Hour;
            st.minute = (short)dt.Minute;
            st.second = (short)dt.Second;
            st.milliseconds = (short)dt.Millisecond;

            SetLocalTime(ref st);
        }

        #endregion

        #region 获取网络时间
         ///<summary>
        /// 获取中国国家授时中心网络服务器时间发布的当前时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetChineseDateTime()
        {
            DateTime res = DateTime.MinValue;
            //try
            //{
            //    string url = "http://www.time.ac.cn/stime.asp";
            //    HttpHelper helper = new HttpHelper();
            //    helper.Encoding = Encoding.Default;
            //    string html = helper.GetHtml(url);
            //    string patDt = @"\d{4}年\d{1,2}月\d{1,2}日";
            //    string patHr = @"hrs\s+=\s+\d{1,2}";
            //    string patMn = @"min\s+=\s+\d{1,2}";
            //    string patSc = @"sec\s+=\s+\d{1,2}";
            //    Regex regDt = new Regex(patDt);
            //    Regex regHr = new Regex(patHr);
            //    Regex regMn = new Regex(patMn);
            //    Regex regSc = new Regex(patSc);

            //    res = DateTime.Parse(regDt.Match(html).Value);
            //    int hr = GetInt(regHr.Match(html).Value, false);
            //    int mn = GetInt(regMn.Match(html).Value, false);
            //    int sc = GetInt(regSc.Match(html).Value, false);
            //    res = res.AddHours(hr).AddMinutes(mn).AddSeconds(sc);
            //}
            //catch { }
            return res;
        }

        /// <summary>
        /// 从指定的字符串中获取整数
        /// </summary>
        /// <param name="origin">原始的字符串</param>
        /// <param name="fullMatch">是否完全匹配，若为false，则返回字符串中的第一个整数数字</param>
        /// <returns>整数数字</returns>
        private static int GetInt(string origin, bool fullMatch)
        {
            if (string.IsNullOrEmpty(origin))
            {
                return 0;
            }
            origin = origin.Trim();
            if (!fullMatch)
            {
                string pat = @"-?\d+";
                Regex reg = new Regex(pat);
                origin = reg.Match(origin.Trim()).Value;
            }
            int res = 0;
            int.TryParse(origin, out res);
            return res;
        }
        #endregion

        #region 类实例方法
     
        public DateTimeHelper(DateTime dateTime)
        {
            dt = dateTime;
        }
        public DateTimeHelper(string dateTime)
        {
            dt = DateTime.Parse(dateTime);
        }
        /// <summary>
        /// 哪天
        /// </summary>
        /// <param name="days">7天前:-7 7天后:+7</param>
        /// <returns></returns>
        public string GetTheDay(int? days)
        {
            int day = days ?? 0;
            return dt.AddDays(day).ToShortDateString();
        }

        /// <summary>
        /// 周日
        /// </summary>
        /// <param name="weeks">上周-1 下周+1 本周0</param>
        /// <returns></returns>
        public string GetSunday(int? weeks)
        {
            int week = weeks ?? 0;
            return dt.AddDays(Convert.ToDouble((0 - Convert.ToInt16(dt.DayOfWeek))) + 7 * week).ToShortDateString();
        }
        /// <summary>
        /// 周六
        /// </summary>
        /// <param name="weeks">上周-1 下周+1 本周0</param>
        /// <returns></returns>
        public string GetSaturday(int? weeks)
        {
            int week = weeks ?? 0;
            return dt.AddDays(Convert.ToDouble((6 - Convert.ToInt16(dt.DayOfWeek))) + 7 * week).ToShortDateString();
        }
        /// <summary>
        /// 月第一天
        /// </summary>
        /// <param name="months">上月-1 本月0 下月1</param>
        /// <returns></returns>
        public string GetFirstDayOfMonth(int? months)
        {
            int month = months ?? 0;
            return DateTime.Parse(DateTime.Now.ToString("yyyy-MM-01")).AddMonths(month).ToShortDateString();
        }
        /// <summary>
        /// 月最后一天
        /// </summary>
        /// <param name="months">上月0 本月1 下月2</param>
        /// <returns></returns>
        public string GetLastDayOfMonth(int? months)
        {
            int month = months ?? 0;
            return DateTime.Parse(dt.ToString("yyyy-MM-01")).AddMonths(month).AddDays(-1).ToShortDateString();
        }
        /// <summary>
        /// 年度第一天
        /// </summary>
        /// <param name="years">上年度-1 下年度+1</param>
        /// <returns></returns>
        public string GetFirstDayOfYear(int? years)
        {
            int year = years ?? 0;
            return DateTime.Parse(dt.ToString("yyyy-01-01")).AddYears(year).ToShortDateString();
        }
        /// <summary>
        /// 年度最后一天
        /// </summary>
        /// <param name="years">上年度0 本年度1 下年度2</param>
        /// <returns></returns>
        public string GetLastDayOfYear(int? years)
        {
            int year = years ?? 0;
            return DateTime.Parse(dt.ToString("yyyy-01-01")).AddYears(year).AddDays(-1).ToShortDateString();
        }
        /// <summary>
        /// 季度第一天
        /// </summary>
        /// <param name="quarters">上季度-1 下季度+1</param>
        /// <returns></returns>
        public string GetFirstDayOfQuarter(int? quarters)
        {
            int quarter = quarters ?? 0;
            return dt.AddMonths(quarter * 3 - ((dt.Month - 1) % 3)).ToString("yyyy-MM-01");
        }
        /// <summary>
        /// 季度最后一天
        /// </summary>
        /// <param name="quarters">上季度0 本季度1 下季度2</param>
        /// <returns></returns>
        public string GetLastDayOfQuarter(int? quarters)
        {
            int quarter = quarters ?? 0;
            return DateTime.Parse(dt.AddMonths(quarter * 3 - ((dt.Month - 1) % 3)).ToString("yyyy-MM-01")).AddDays(-1).ToShortDateString();
        }
        /// <summary>
        /// 中文星期
        /// </summary>
        /// <returns></returns>
        public string GetDayOfWeekCN()
        {
            string[] Day = new string[] { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
            return Day[Convert.ToInt16(dt.DayOfWeek)];
        }
        /// <summary>
        /// 获取星期数字形式,周一开始
        /// </summary>
        /// <returns></returns>
        public int GetDayOfWeekNum()
        {
            int day = (Convert.ToInt16(dt.DayOfWeek) == 0) ? 7 : Convert.ToInt16(dt.DayOfWeek);
            return day;
        } 
        #endregion

        
    }
}
