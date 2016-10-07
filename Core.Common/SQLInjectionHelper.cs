using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Core.Common
{
    /// <summary>
    /// SQL注入帮助
    /// </summary>
    public class SQLInjectionHelper
    {
        private const string StrKeyWord = @".*(select|insert|delete|from|count(|drop table|update|truncate|asc(|mid(|char(|xp_cmdshell|exec master|netlocalgroup administrators|:|net user|""|or|and).*";
        private const string StrRegex = @"[-|;|,|/|(|)|[|]|}|{|%|@|*|!|']";

        /// <summary>
        /// 获取Post的数据
        /// </summary>
        public static bool ValidUrlPostData()
        {
            bool result = false;

            for (int i = 0; i < HttpContext.Current.Request.Form.Count; i++)
            {
                result = ValidData(HttpContext.Current.Request.Form[i].ToString());
                if (result)
                {
                    LogHelper.Info("检测出POST恶意数据: 【" + HttpContext.Current.Request.Form[i].ToString() + "】 URL: 【" + HttpContext.Current.Request.RawUrl + "】来源: 【" + HttpContext.Current.Request.UserHostAddress + "】");
                    break;
                }//如果检测存在漏洞
            }
            return result;
        }

        /// <summary>
        /// 获取QueryString中的数据
        /// </summary>
        public static bool ValidUrlGetData()
        {
            bool result = false;

            for (int i = 0; i < HttpContext.Current.Request.QueryString.Count; i++)
            {
                result = ValidData(HttpContext.Current.Request.QueryString[i].ToString());
                if (result)
                {
                    LogHelper.Info("检测出GET恶意数据: 【" + HttpContext.Current.Request.QueryString[i].ToString() + "】 URL: 【" + HttpContext.Current.Request.RawUrl + "】来源: 【" + HttpContext.Current.Request.UserHostAddress + "】");
                    break;
                }//如果检测存在漏洞
            }
            return result;
        }

        /// <summary>
        /// 验证是否存在注入代码
        /// </summary>
        /// <param name="inputData"></param>
        public static bool ValidData(string inputData)
        {
            //里面定义恶意字符集合
            //验证inputData是否包含恶意集合
            if (Regex.IsMatch(inputData.ToLower(), GetRegexString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取正则表达式
        /// </summary>
        /// <param name="queryConditions"></param>
        /// <returns></returns>
        private static string GetRegexString()
        {
            //构造SQL的注入关键字符
            string[] strBadChar =
        {
            "and"
            ,"exec"
            ,"insert"
            ,"select"
            ,"delete"
            ,"update"
            ,"count"
            ,"from"
            ,"drop"
            ,"asc"
            ,"char"
            ,"or"
            ,"%"
            ,";"
            ,":"
            ,"\'"
            ,"\""
            ,"-"
            ,"chr"
            ,"mid"
            ,"master"
            ,"truncate"
            ,"char"
            ,"declare"
            ,"SiteName"
            ,"net user"
            ,"xp_cmdshell"
            ,"/add"
            ,"exec master.dbo.xp_cmdshell"
            ,"net localgroup administrators"
        };

            //构造正则表达式
            string str_Regex = ".*(";
            for (int i = 0; i < strBadChar.Length - 1; i++)
            {
                str_Regex += strBadChar[i] + "|";
            }
            str_Regex += strBadChar[strBadChar.Length - 1] + ").*";

            return str_Regex;
        }


        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary> SQL注入等安全验证
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }

        #region 检测客户的输入中是否有危险字符串
        /// <summary>
        /// 检测客户输入的字符串是否有效,并将原始字符串修改为有效字符串或空字符串。
        /// 当检测到客户的输入中有攻击性危险字符串,则返回false,有效返回true。
        /// </summary>
        /// <param name="input">要检测的字符串</param>
        public static bool IsValidInput(ref string input)
        {
            try
            {

                //替换单引号
                input = input.Replace("'", "''").Trim();

                //检测攻击性危险字符串
                string testString = "and |or |exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
                string[] testArray = testString.Split('|');
                foreach (string testStr in testArray)
                {
                    if (input.ToLower().IndexOf(testStr) != -1)
                    {
                        //检测到攻击字符串,清空传入的值
                        input = "";
                        return false;
                    }
                }

                //未检测到攻击字符串
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
