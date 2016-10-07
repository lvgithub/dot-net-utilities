using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Html
{
     public class HtmlTools
    {
         #region 获得发表日期、编码
         public static DateTime GetCreateDate(string sContent, string sRegex)
         {
             DateTime dt = System.DateTime.Now;

             Regex re = new Regex(sRegex, RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline);
             Match mc = re.Match(sContent);
             if (mc.Success)
             {
                 try
                 {
                     int iYear = int.Parse(mc.Groups["Year"].Value);
                     int iMonth = int.Parse(mc.Groups["Month"].Value);
                     int iDay = int.Parse(mc.Groups["Day"].Value);
                     int iHour = dt.Hour;
                     int iMinute = dt.Minute;

                     string sHour = mc.Groups["Hour"].Value;
                     string sMintue = mc.Groups["Mintue"].Value;

                     if (sHour != "")
                         iHour = int.Parse(sHour);
                     if (sMintue != "")
                         iMinute = int.Parse(sMintue);

                     dt = new DateTime(iYear, iMonth, iDay, iHour, iMinute, 0);
                 }
                 catch { }
             }
             return dt;
         }
         #endregion 获得发表日期

    }
}
