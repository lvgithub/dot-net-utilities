using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Core.Common
{
    public class TypeTools
    {
        #region 判断对象是否为空
        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNullOrEmpty<T>(T data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
        public static bool IsNullOrEmpty(object data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }
        #endregion

        #region 数值类型验证

        /// <summary>
        /// int有效性
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        static public bool IsValidInt(string val)
        {
            return Regex.IsMatch(val, @"^[1-9]\d*\.?[0]*$");
        }
       
       

        #region 验证是否为数字


        /// <summary>
        /// 验证是否为数字
        /// </summary>
        /// <param name="number">要验证的数字</param>        
        public static bool IsNumber(string number)
        {
            //如果为空，认为验证不合格
            if (IsNullOrEmpty(number))
            {
                return false;
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            string pattern = @"^[0-9]+[0-9]*[.]?[0-9]*$";

            //验证
            return Regex.IsMatch(number, pattern);
        }
        #endregion

        #region 验证是否为整数
        /// <summary>
        /// 验证是否为整数 如果为空，认为验证不合格 返回false
        /// </summary>
        /// <param name="number">要验证的整数</param>        
        public static bool IsInt(string number)
        {
            //如果为空，认为验证不合格
            if (IsNullOrEmpty(number))
            {
                return false;
            }
            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            string pattern = @"^[0-9]+[0-9]*$";

            //验证
            return Regex.IsMatch(number, pattern);
        }
        #endregion

        #region 判断对象是否为Int32类型的数字
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(object Expression)
        {
            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

        //public static bool IsNumeric(object o)
        //{
        //    if (o is Int16 ||
        //        o is Int32 ||
        //        o is Int64 ||
        //        o is Decimal ||
        //        o is Double ||
        //        o is Byte ||
        //        o is SByte ||
        //        o is Single ||
        //        o is UInt16 ||
        //        o is UInt32 ||
        //        o is UInt64)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
        #endregion

        #region 判断对象是否为Double类型的数字
        /// <summary>
        /// 判断对象是否为Double类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsDouble(object Expression)
        {
            if (Expression != null)
            {
                return Regex.IsMatch(Expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }
        #endregion

        #region 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;

        }
        #endregion

        #endregion

        #region 数字字符串检查




        /// <summary>
        /// 是否数字字符串 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsNumberSign(string inputData)
        {
            Regex RegNumberSign = new Regex("^[+-]?[0-9]+$");
            Match m = RegNumberSign.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimal(string inputData)
        {
            Regex RegDecimal = new Regex("^[0-9]+[.]?[0-9]+$");
            Match m = RegDecimal.Match(inputData);
            return m.Success;
        }
        /// <summary>
        /// 是否是浮点数 可带正负号
        /// </summary>
        /// <param name="inputData">输入字符串</param>
        /// <returns></returns>
        public static bool IsDecimalSign(string inputData)
        {
            Regex RegDecimalSign = new Regex("^[+-]?[0-9]+[.]?[0-9]+$"); //等价于^[+-]?\d+[.]?\d+$
            Match m = RegDecimalSign.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 字符串类型验证

        #region 判断是否为base64字符串
        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }
        #endregion

        #region 中文检测

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        #endregion

        #endregion

        #region 字符串功能验证

        /// <summary>
        /// 是否包涵制定的字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="stringarray"></param>
        /// <param name="strsplit"></param>
        /// <returns></returns>
        public static bool IsCompriseStr(string str, string stringarray, string strsplit)
        {
            if (stringarray == "" || stringarray == null)
            {
                return false;
            }

            str = str.ToLower();
            string[] stringArray = StringHelper.SplitString(stringarray.ToLower(), strsplit);
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (str.IndexOf(stringArray[i]) > -1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return StringHelper.GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray)
        {
            return InArray(str, StringHelper.SplitString(stringarray, ","), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit)
        {
            return InArray(str, StringHelper.SplitString(stringarray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(string str, string stringarray, string strsplit, bool caseInsensetive)
        {
            return InArray(str, StringHelper.SplitString(stringarray, strsplit), caseInsensetive);
        }


        #endregion

        #region 逻辑判断类

        /// <summary>
        /// 空值显示替换
        /// </summary>
        /// <param name="objOrigin">来源对象</param>
        /// <param name="strOriginal">来源对象拷贝或处理对象</param>
        /// <param name="strReplace">替换对象</param>
        /// <returns>相应文本输出</returns>
        public static string GetEmptyShow(object objOrigin, object strOriginal, string strReplace)
        {
            string strRet = string.Empty;
            strRet = (objOrigin != null && objOrigin.ToString().Trim() != string.Empty) ? strOriginal.ToString() : strReplace;
            return strRet;
        }

        /// <summary>
        /// 获取满足条件的相应值
        /// </summary>
        /// <param name="Expression">布尔表达式</param>
        /// <param name="strTrue">为真时返回的字符</param>
        /// <param name="strFalse">为假时返回的字符</param>
        /// <returns>相应条件的字符值</returns>
        public static string GetWhich(bool Expression, string strTrue, string strFalse)
        {
            return (Expression == true) ? strTrue : strFalse;
        }

        /// <summary>
        /// 表单选择判断
        /// </summary>
        /// <param name="objQuery">来源对象</param>
        /// <param name="strMatch">匹配项</param>
        /// <param name="allowNull">可以为Null对象</param>
        /// <param name="strValue">返回文本输出</param>
        /// <returns>相应文本输出</returns>
        public static string GetDefaultValue(object objQuery, string strMatch, bool allowNull, string strValue)
        {
            string strRet = string.Empty;
            if (objQuery == null)
            {
                if (allowNull == true) strRet = strValue;
            }
            else
            {
                if ((objQuery.ToString() == strMatch) ||
                    (objQuery.ToString() == string.Empty && allowNull == true))
                {
                    strRet = strValue;
                }
            }
            return strRet;
        }

        #endregion 

        #region 数据操作
        /// <summary>
        /// 去除字符串的所有空格。
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>字符串</returns>
        public static string StringTrimAll(string text)
        {
            string _text = text;
            string returnText = String.Empty;
            char[] chars = _text.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i].ToString() != string.Empty)
                    returnText += chars[i].ToString();
            }
            return returnText;
        }

        /// <summary>
        /// 去除数值字符串的所有空格。
        /// </summary>
        /// <param name="numricString">数值字符串</param>
        /// <returns>String</returns>
        public static string NumricTrimAll(string numricString)
        {
            string text = numricString.Trim();
            string returnText = String.Empty;
            //char[] chars = text.ToCharArray();
            //for (int i = 0; i < chars.Length; i++)
            //{
            //    if (chars[i].ToString() == "+" || chars[i].ToString() == "-" || IsDouble(chars[i].ToString()))
            //        returnText += chars[i].ToString();
            //}
            return returnText;
        }

        /// <summary>
        /// 在数组中查找匹配对象类型
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="obj">对象</param>
        /// <returns>Boolean</returns>
        public static bool ArrayFind(Array array, object obj)
        {
            bool b = false;
            foreach (object obj1 in array)
            {
                if (obj.Equals(obj1))
                {
                    b = true;
                    break;
                }
            }
            return b;
        }

        /// <summary>
        /// 在数组中查找匹配字符串
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="obj">对象</param>
        /// <param name="unUpLower">是否忽略大小写</param>
        /// <returns>Boolean</returns>
        public static bool ArrayFind(Array array, string obj, bool unUpLower)
        {
            bool b = false;
            foreach (string obj1 in array)
            {
                if (!unUpLower)
                {
                    if (obj.Trim().Equals(obj1.ToString().Trim()))
                    {
                        b = true;
                        break;
                    }
                }
                else
                {
                    if (obj.Trim().ToUpper().Equals(obj1.ToString().Trim().ToUpper()))
                    {
                        b = true;
                        break;
                    }
                }
            }
            return b;
        }



        /// <summary>
        /// 判断两个字节数组是否具有相同值.
        /// </summary>
        /// <param name="bytea">字节1</param>
        /// <param name="byteb">字节2</param>
        /// <returns>Boolean</returns>
        public static bool CompareByteArray(byte[] bytea, byte[] byteb)
        {
            if (null == bytea || null == byteb)
            {
                return false;
            }
            else
            {
                int aLength = bytea.Length;
                int bLength = byteb.Length;
                if (aLength != bLength)
                    return false;
                else
                {
                    bool compare = true;
                    for (int index = 0; index < aLength; index++)
                    {
                        if (bytea[index].CompareTo(byteb[index]) != 0)
                        {
                            compare = false;
                            break;
                        }
                    }
                    return compare;
                }
            }
        }


        /// <summary>
        /// 日期智能生成。
        /// </summary>
        /// <param name="inputText">字符串</param>
        /// <returns>DateTime</returns>
        public static string BuildDate(string inputText)
        {
            try
            {
                return DateTime.Parse(inputText).ToShortDateString();
            }
            catch
            {
                string text = NumricTrimAll(inputText);
                string year = DateTime.Now.Year.ToString();
                string month = DateTime.Now.Month.ToString();
                string day = DateTime.Now.Day.ToString();
                int length = text.Length;
                if (length == 0)
                    return String.Empty;
                else
                {
                    if (length <= 2)
                        day = text;
                    else if (length <= 4)
                    {
                        month = text.Substring(0, 2);
                        day = text.Substring(2, length - 2);
                    }
                    else if (length <= 6)
                    {
                        year = text.Substring(0, 4);
                        month = text.Substring(4, length - 4);
                    }
                    else if (length > 6)
                    {
                        year = text.Substring(0, 4);
                        month = text.Substring(4, 2);
                        day = text.Substring(6, length - 6);
                    }
                    try
                    {
                        return DateTime.Parse(year + "-" + month + "-" + day).ToShortDateString();
                    }
                    catch
                    {
                        return String.Empty;
                    }
                }
            }
        }



        /// <summary>
        /// 查找文件中是否存在匹配行。
        /// </summary>
        /// <param name="fi">目标文件.</param>
        /// <param name="lineText">要查找的行文本.</param>
        /// <param name="lowerUpper">是否区分大小写.</param>
        /// <returns>Boolean</returns>
        public static bool FindLineTextFromFile(FileInfo fi, string lineText, bool lowerUpper)
        {
            bool b = false;
            try
            {
                if (fi.Exists)
                {
                    StreamReader sr = new StreamReader(fi.FullName);
                    string g = "";
                    do
                    {
                        g = sr.ReadLine();
                        if (lowerUpper)
                        {
                            if (g.Trim() == lineText.Trim())
                            {
                                b = true;
                                break;
                            }
                        }
                        else
                        {
                            if (g.Trim().ToLower() == lineText.Trim().ToLower())
                            {
                                b = true;
                                break;
                            }
                        }
                    }
                    while (sr.Peek() != -1);
                    sr.Close();
                }
            }
            catch
            { b = false; }
            return b;
        }


        /// <summary>
        /// 判断父子级关系是否正确。
        /// </summary>
        /// <param name="table">数据表。</param>
        /// <param name="columnName">子键列名。</param>
        /// <param name="parentColumnName">父键列名。</param>
        /// <param name="inputString">子键值。</param>
        /// <param name="compareString">父键值。</param>
        /// <returns></returns>
        public static bool IsRightParent(DataTable table, string columnName, string parentColumnName, string inputString, string compareString)
        {
            ArrayList array = new ArrayList();
            SearchChild(array, table, columnName, parentColumnName, inputString, compareString);
            return array.Count == 0;
        }


        // 内部方法
        private static void SearchChild(ArrayList array, DataTable table, string columnName, string parentColumnName, string inputString, string compareString)
        {
            DataView view = new DataView(table);
            view.RowFilter = parentColumnName + "='" + inputString.Replace("'", "''") + "'";//找出所有的子类。
            //查找表中的数据的ID是否与compareString相等，相等返回 false;不相等继续迭代。
            for (int index = 0; index < view.Count; index++)
            {
                if ((view[index][columnName]).ToString().ToLower() == compareString.Trim().ToLower())
                {
                    array.Add("1");
                    break;
                }
                else
                {
                    // SearchChild(array, table, columnName, parentColumnName, ToObjectString(view[index][columnName]), compareString);
                }
            }
        }

        #endregion


        #region 日期类型格式验证
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

        /// <summary>
        /// 判断字符串是否是yy-mm-dd字符串
        /// </summary>
        /// <param name="str">待判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsDateString(string str)
        {
            return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
        }

        /// <summary>是否日期</summary>
        /// <param name="strInput">输入字符串</param>
        /// <returns>true/false</returns>
        public static bool isDate(string strInput)
        {
            string datestr = strInput;
            string year, month, day;
            string[] c = { "/", "-", "." };
            string cs = "";
            for (int i = 0; i < c.Length; i++)
            {
                if (datestr.IndexOf(c[i]) > 0)
                {
                    cs = c[i];
                    break;
                }

            };

            if (cs != "")
            {
                year = datestr.Substring(0, datestr.IndexOf(cs));
                if (year.Length != 4) { return false; };
                datestr = datestr.Substring(datestr.IndexOf(cs) + 1);

                month = datestr.Substring(0, datestr.IndexOf(cs));
                if ((month.Length != 2) || (Convert.ToInt16(month) > 12))
                { return false; };
                datestr = datestr.Substring(datestr.IndexOf(cs) + 1);

                day = datestr;
                if ((day.Length != 2) || (Convert.ToInt16(day) > 31)) { return false; };

                return checkDatePart(year, month, day);
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 检查年月日是否合法
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="part"></param>
        /// <returns></returns>
        private static bool checkDatePart(string year, string month, string day)
        {
            int iyear = Convert.ToInt16(year);
            int imonth = Convert.ToInt16(month);
            int iday = Convert.ToInt16(day);
            if (iyear > 2099 || iyear < 1900) { return false; }
            if (imonth > 12 || imonth < 1) { return false; }
            if (iday > DateUtil.GetDaysOfMonth(iyear, imonth) || iday < 1) { return false; };
            return true;
        }



        /// <summary>
        /// 判断输入的字符是否为日期
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsDate(string strValue)
        {
            return Regex.IsMatch(strValue, @"^((\d{2}(([02468][048])|([13579][26]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|([1-2][0-9])))))|(\d{2}(([02468][1235679])|([13579][01345789]))[\-\/\s]?((((0?[13578])|(1[02]))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(3[01])))|(((0?[469])|(11))[\-\/\s]?((0?[1-9])|([1-2][0-9])|(30)))|(0?2[\-\/\s]?((0?[1-9])|(1[0-9])|(2[0-8]))))))");
        }

        /// <summary>
        /// 判断输入的字符是否为日期,如2004-07-12 14:25|||1900-01-01 00:00|||9999-12-31 23:59
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool IsDateHourMinute(string strValue)
        {
            return Regex.IsMatch(strValue, @"^(19[0-9]{2}|[2-9][0-9]{3})-((0(1|3|5|7|8)|10|12)-(0[1-9]|1[0-9]|2[0-9]|3[0-1])|(0(4|6|9)|11)-(0[1-9]|1[0-9]|2[0-9]|30)|(02)-(0[1-9]|1[0-9]|2[0-9]))\x20(0[0-9]|1[0-9]|2[0-3])(:[0-5][0-9]){1}$");
        }
    
        #region 验证日期是否合法
        /// <summary>
        /// 验证日期是否合法,对不规则的作了简单处理
        /// </summary>
        /// <param name="date">日期</param>
        public static bool IsDate(ref string date)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(date))
            {
                return true;
            }

            //清除要验证字符串中的空格
            date = date.Trim();

            //替换\
            date = date.Replace(@"\", "-");
            //替换/
            date = date.Replace(@"/", "-");

            //如果查找到汉字"今",则认为是当前日期
            if (date.IndexOf("今") != -1)
            {
                date = DateTime.Now.ToString();
            }

            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch
            {
                //如果日期字符串中存在非数字，则返回false
                if (!IsInt(date))
                {
                    return false;
                }

                #region 对纯数字进行解析
                //对8位纯数字进行解析
                if (date.Length == 8)
                {
                    //获取年月日
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }

                //对6位纯数字进行解析
                if (date.Length == 6)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }

                //对5位纯数字进行解析
                if (date.Length == 5)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 1);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }

                //对4位纯数字进行解析
                if (date.Length == 4)
                {
                    //获取年
                    string year = date.Substring(0, 4);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }
                #endregion

                return false;
            }
        }
        #endregion

        #endregion

        #region 合并数组

        /// <summary>
        /// 用 , 分割 合并数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string Join<T>(IList<T> list)
        {
            return Join<T>(list, ",");
        }

        /// <summary>
        /// 用 , 分割 合并数组
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string Join(IList<string> list)
        {
            return Join<string>(list, ",");
        }

        /// <summary>
        /// 用 , 分割 合并数组
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string Join(IList<string> list, string c)
        {
            return Join<string>(list, c);
        }


        /// <summary>
        /// 合并数组 分割符
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">数组列</param>
        /// <param name="c">分隔符</param>
        /// <returns></returns>
        public static string Join<T>(IList<T> list, string c)
        {
            if (null != list && list.Count > 0)
            {
                StringBuilder text = new StringBuilder();
                foreach (T t in list)
                {
                    text.Append(c);
                    text.Append(t.ToString());
                }

                if (!string.IsNullOrEmpty(c))
                    return text.ToString().Substring(c.Length);

                return text.ToString();
            }

            return string.Empty;
        }


        #endregion

        #region 身份证号码验证
        ///// <summary>
        ///// 验证身份证是否合法  15 和  18位两种
        ///// </summary>
        ///// <param name="idCard">要验证的身份证</param>
        //public static bool IsIdCard(string idCard)
        //{
        //    if (string.IsNullOrEmpty(idCard))
        //    {
        //        return false;
        //    }

        //    if (idCard.Length == 15)
        //    {
        //        return Regex.IsMatch(idCard, @"^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$");
        //    }
        //    else if (idCard.Length == 18)
        //    {
        //        return Regex.IsMatch(idCard, @"^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])((\d{4})|\d{3}[A-Z])$", RegexOptions.IgnoreCase);
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        /// <summary>
        /// 验证身份证号码
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns>验证成功为True，否则为False</returns>
        public static bool CheckIDCard(string Id)
        {
            if (Id.Length == 18)
            {
                bool check = CheckIDCard18(Id);
                return check;
            }
            else if (Id.Length == 15)
            {
                bool check = CheckIDCard15(Id);
                return check;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 验证15位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard18(string Id)
        {
            long n = 0;
            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = Id.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
            {
                return false;//校验码验证
            }
            return true;//符合GB11643-1999标准
        }

        /// <summary>
        /// 验证18位身份证号
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns>验证成功为True，否则为False</returns>
        private static bool CheckIDCard15(string Id)
        {
            long n = 0;
            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
            {
                return false;//数字验证
            }
            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }
            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
            DateTime time = new DateTime();
            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }
            return true;//符合15位身份证标准
        }

        #endregion

        #region  验证EMail是否合法

        /// <summary> 
        /// 检查邮件正确性 
        /// </summary> 
        /// <param name="inputEmail">输入的邮件地址</param> 
        /// <returns>返回BOOL值</returns> 
        public static bool IsEmail(string inputEmail)
        {
            //string strRegex = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
            //string strRegex = "^[\\w-]+@[\\w-]+\\.(com|net|org|edu|mil|tv|biz|info)$";//w 英文字母或数字的字符串，和 [a-zA-Z0-9] 语法一样 

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }
        #endregion

        #region IP地址验证

        #region 验证IP地址是否合法
        /// <summary>
        /// 验证IP地址是否合法
        /// </summary>
        /// <param name="ip">要验证的IP地址</param>        
        public static bool IsIP(string ip)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(ip))
            {
                return true;
            }
            //清除要验证字符串中的空格
            ip = ip.Trim();
            //模式字符串
            string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            //验证
            return Regex.IsMatch(ip, pattern);
            //return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");

        }

        #endregion

        #region 返回指定IP是否在指定的IP数组所限定的范围内
        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool InIPArray(string ip, string[] iparray)
        {

            string[] userip = StringHelper.SplitString(ip, @".");
            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = StringHelper.SplitString(iparray[ipIndex], @".");
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*")
                    {
                        return true;
                    }

                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i])
                        {
                            r++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                }
                if (r == 4)
                {
                    return true;
                }


            }
            return false;

        }
        #endregion

        #endregion


        /// <summary>
        /// 检查Request查询字符串的键值，是否是数字，最大长度限制
        /// </summary>
        /// <param name="req">Request</param>
        /// <param name="inputKey">Request的键值</param>
        /// <param name="maxLen">最大长度</param>
        /// <returns>返回Request查询字符串</returns>
        public static string FetchInputDigit(HttpRequest req, string inputKey, int maxLen)
        {
            string retVal = string.Empty;
            if (inputKey != null && inputKey != string.Empty)
            {
                retVal = req.QueryString[inputKey];
                if (null == retVal)
                    retVal = req.Form[inputKey];
                if (null != retVal)
                {
                    retVal = SqlText(retVal, maxLen);
                    if (!IsNumber(retVal))
                        retVal = string.Empty;
                }
            }
            if (retVal == null)
                retVal = string.Empty;
            return retVal;
        }
        public static string SqlText(string sqlInput, int maxLength)
        {
            if (sqlInput != null && sqlInput != string.Empty)
            {
                sqlInput = sqlInput.Trim();
                if (sqlInput.Length > maxLength)//按最大长度截取字符串
                    sqlInput = sqlInput.Substring(0, maxLength);
            }
            return sqlInput;
        }
    }
}
