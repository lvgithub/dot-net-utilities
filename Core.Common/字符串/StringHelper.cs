using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Common
{
    /// <summary>
    /// 1.字符串操作类
    /// 1.1 字符串截取替换
    /// 1.1.1 截取过长字符串,指定长度其他用...代替
    /// 1.1.2 自定义的替换字符串函数
    /// 1.2 字符串分割
    ///     分割字符串 按着指定分隔符分割字符串
    /// 1.3 字符串删除
    /// 1.3.1 删除最后结尾的一个逗号
    /// 1.3.2 删除最后结尾的指定字符后的字符
    /// 1.3.3 删除指定的字符串结尾
    /// 1.4 字符串长度
    /// 1.4.1 得到字符串长度，一个汉字长度为2
    ///       获取某一字符串在字符串中出现的次数
    /// 1.5 字符串其他操作
    ///      清理字符串
    ///      获得干净,无非法字符的字符串
    ///      获取拆分符右边的字符串
    ///      获取拆分符左边的字符串
    ///      格式化字符串为SQL语句字段
    /// 2. 全角半角
    ///     ToSBC(string input)转全角的函数(SBC case)半角转全角
    ///     ToDBC(string input)转半角的函数(SBC case)全角转半角
    /// 3. 字符串加密,编码
    ///     GetMD5(string s)根据配置对指定字符串进行 MD5 加密
    /// 4. 字符串数组
    ///    GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
    ///    判断指定字符串在指定字符串数组中的位置
    /// 5. 字符串验证是否是纯数字
    /// 6.字符串样式
    /// 7.快速验证一个字符串是否符合指定的正则表达式
    /// 8.判断对象是否为空
    
    /// 10.其他操作
    ///    获得唯一的字符串
    ///    统计char出现在string中的次数
    /// </summary>
    public class StringHelper
    {
        #region 1.字符串常用操作

        #region 1.1字符串截取替换

        #region 1.1.1 截取过长字符串,指定长度其他用...代替
        /// <summary>
        /// 截取过长字符串,指定长度其他用...代替
        /// </summary>
        public static string Cut(string Content, int length)
        {
            return Cut(Content, length, "...");
        }
        /// <summary>
        /// 截取字符串
        /// </summary>
        public static string Cut(string Content, int length, string addstr)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            int intLength = 0;
            string strString = "";
            byte[] s = ascii.GetBytes(Content);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                {
                    intLength += 2;
                }
                else
                {
                    intLength += 1;
                }

                try
                {
                    strString += Content.Substring(i, 1);
                }
                catch
                {
                    break;
                }

                if (intLength > length)
                {
                    break;
                }
            }
            //如果截过则加上半个省略号
            byte[] mybyte = System.Text.Encoding.Default.GetBytes(Content);
            if (mybyte.Length > length)
            {
                strString += addstr;
            }
            return strString;
        }
        #region 截取字符串优化版
        //  /// <summary>
        //  /// 截取字符串优化版
        //  /// </summary>
        //  /// <param name="str">所要截取的字符串</param>
        //  /// <param name="length">截取字符串的长度</param>
        //  /// <returns></returns>
        //  public static string CutString(string str, int length, bool appendText)
        //  {

        //      Regex regex = new Regex("[\u4e00-\u9fa5]+", RegexOptions.Compiled);
        //      char[] stringChar = str.ToCharArray();
        //      StringBuilder sb = new StringBuilder();
        //      int nLength = 0;
        //      bool isCut = false;
        //      for (int i = 0; i < stringChar.Length; i++)
        //      {
        //          if (regex.IsMatch((stringChar[i]).ToString()))
        //          {
        //              sb.Append(stringChar[i]);
        //              nLength += 2;
        //          }
        //          else
        //          {
        //              sb.Append(stringChar[i]);
        //              nLength = nLength + 1;
        //          }

        //          if (nLength > length)
        //          {
        //              isCut = true;
        //              break;
        //          }
        //      }
        //      if (isCut)
        //          if (appendText)
        //          {
        //              return sb.ToString() + "...";
        //          }
        //          else
        //          {
        //              return sb.ToString();
        //          }
        //      else
        //          return sb.ToString();

        //  }

///// <summary>
        ///// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        ///// </summary>
        ///// <param name="p_SrcString">要检查的字符串</param>
        ///// <param name="p_Length">指定长度</param>
        ///// <param name="p_TailString">用于替换的字符串</param>
        ///// <returns>截取后的字符串</returns>
        //public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        //{
        //    return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        //}

        ///// <summary>
        ///// 取指定长度的字符串
        ///// </summary>
        ///// <param name="p_SrcString">要检查的字符串</param>
        ///// <param name="p_StartIndex">起始位置</param>
        ///// <param name="p_Length">指定长度</param>
        ///// <param name="p_TailString">用于替换的字符串</param>
        ///// <returns>截取后的字符串</returns>
        //public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        //{
        //    string myResult = p_SrcString;

        //    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
        //    if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") ||
        //        System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
        //    {
        //        //当截取的起始位置超出字段串长度时
        //        if (p_StartIndex >= p_SrcString.Length)
        //        {
        //            return "";
        //        }
        //        else
        //        {
        //            return p_SrcString.Substring(p_StartIndex,
        //                                           ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
        //        }
        //    }


        //    if (p_Length >= 0)
        //    {
        //        byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

        //        //当字符串长度大于起始位置
        //        if (bsSrcString.Length > p_StartIndex)
        //        {
        //            int p_EndIndex = bsSrcString.Length;

        //            //当要截取的长度在字符串的有效长度范围内
        //            if (bsSrcString.Length > (p_StartIndex + p_Length))
        //            {
        //                p_EndIndex = p_Length + p_StartIndex;
        //            }
        //            else
        //            {   //当不在有效范围内时,只取到字符串的结尾

        //                p_Length = bsSrcString.Length - p_StartIndex;
        //                p_TailString = "";
        //            }



        //            int nRealLength = p_Length;
        //            int[] anResultFlag = new int[p_Length];
        //            byte[] bsResult = null;

        //            int nFlag = 0;
        //            for (int i = p_StartIndex; i < p_EndIndex; i++)
        //            {

        //                if (bsSrcString[i] > 127)
        //                {
        //                    nFlag++;
        //                    if (nFlag == 3)
        //                    {
        //                        nFlag = 1;
        //                    }
        //                }
        //                else
        //                {
        //                    nFlag = 0;
        //                }

        //                anResultFlag[i] = nFlag;
        //            }

        //            if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
        //            {
        //                nRealLength = p_Length + 1;
        //            }

        //            bsResult = new byte[nRealLength];

        //            Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

        //            myResult = Encoding.Default.GetString(bsResult);

        //            myResult = myResult + p_TailString;
        //        }
        //    }

        //    return myResult;
        //}
        #endregion

        #endregion   
    
        #region 1.1.2 自定义的替换字符串函数
        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        /// <param name="SourceString">字符串</param>
        /// <param name="SearchString">替换前字符串</param>
        /// <param name="ReplaceString">替换后字符串</param>
        /// <param name="IsCaseInsensetive"></param>
        /// <returns></returns>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }
        

        #endregion

        #endregion 1.字符串截取替换

        #region 1.2 字符串分割

        /// <summary>
        /// 把字符串串 按照 逗号, 分割 换为数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new Char[] { ',' });
        }

        /// <summary>
        /// 分割字符串 按着指定分隔符分割字符串
        /// </summary>
        /// <param name="strContent">字符串</param>
        /// <param name="strSplit">分隔符</param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit)
        {
            string[] strArray = null;
            if ((strContent != null) && (strContent != ""))
            {
                strArray = new Regex(strSplit).Split(strContent);
            }
            return strArray;
            #region 分割字符串第2种
            ////第2种
            //if (strContent.IndexOf(strSplit) < 0)
            //{
            //    string[] tmp = { strContent };
            //    return tmp;
            //}
            //return strArray = Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            #endregion
            #region 分割字符串第3种
            ////第3种
            //if (!Regex.IsMatch(strContent, strSplit))
            //{
            //    string[] tmp = { strContent };
            //    return tmp;
            //}
            //return strArray = Regex.Split(strContent, strSplit, RegexOptions.IgnoreCase);
            #endregion
        }

        /// <summary>
        /// 分割字符串 按着指定分隔符分割字符串 返回指定个数数组
        /// </summary>
        /// <param name="strContent">字符串</param>
        /// <param name="strSplit">分隔符</param>
        /// <param name="p_3">数组个数</param>
        /// <returns></returns>
        public static string[] SplitString(string strContent, string strSplit, int p_3)
        {
            string[] result = new string[p_3];

            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < p_3; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }


        #endregion 2.字符串分割

        #region 1.3 字符串删除
        #region 1.3.1 删除最后结尾的一个逗号
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            if (str.IndexOf(",") == -1)
            {
                return str;
            }
            return str.Substring(0, str.LastIndexOf(","));
        }
        #endregion
        #region 1.3.2 删除最后结尾的指定字符后的字符
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            //int iLast = sOrg.LastIndexOf(sLast);
            //if (iLast > 0)
            //    return sOrg.Substring(0, iLast);
            //else
            //    return sOrg;
            return str.Substring(0, str.LastIndexOf(strchar));
        }

        #endregion
        #region 1.3.3 删除指定的字符串结尾

        /// <summary>
        /// 删除指定的字符串结尾
        /// </summary>
        /// <param name="sOrg">字符串</param>
        /// <param name="sEnd">指定结尾字符串</param>
        /// <returns></returns>
        public static string RemoveEndWith(string sOrg, string sEnd)
        {
            if (sOrg.EndsWith(sEnd))
                sOrg = sOrg.Remove(sOrg.IndexOf(sEnd), sEnd.Length);
            return sOrg;
        }
        // public static string Remove(string sourceString, string removedString)
        //{
        //    try
        //    {
        //        if (sourceString.IndexOf(removedString) < 0)
        //            throw new Exception("原字符串中不包含移除字符串！");
        //        string result = sourceString;
        //        int lengthOfSourceString = sourceString.Length;
        //        int lengthOfRemovedString = removedString.Length;
        //        int startIndex = lengthOfSourceString - lengthOfRemovedString;
        //        string tempSubString = sourceString.Substring(startIndex);
        //        if (tempSubString.ToUpper() == removedString.ToUpper())
        //        {
        //            result = sourceString.Remove(startIndex, lengthOfRemovedString);
        //        }
        //        return result;
        //    }
        //    catch
        //    {
        //        return sourceString;
        //    }
        //}
        #endregion

        #endregion 3.字符串删除

        #region 1.4 字符串长度
        #region 1.4.1 得到字符串长度，一个汉字长度为2
        /// <summary>
        /// 得到字符串长度，一个汉字长度为2
        /// </summary>
        /// <param name="inputString">参数字符串</param>
        /// <returns></returns>
        public static int StrLength(string inputString)
        {
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();
            int tempLen = 0;
            byte[] s = ascii.GetBytes(inputString);
            for (int i = 0; i < s.Length; i++)
            {
                if ((int)s[i] == 63)
                    tempLen += 2;
                else
                    tempLen += 1;
            }
            return tempLen;
        }
        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }
       

        /// <summary>
        /// 按字节数取出字符串的长度
        /// </summary>
        /// <param name="strTmp">要计算的字符串</param>
        /// <returns>字符串的字节数</returns>
        public static int GetByteCount(string strTmp)
        {
            int intCharCount = 0;
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (System.Text.UTF8Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    intCharCount = intCharCount + 2;
                }
                else
                {
                    intCharCount = intCharCount + 1;
                }
            }
            return intCharCount;
        }

        /// <summary>
        /// 按字节数要在字符串的位置
        /// </summary>
        /// <param name="intIns">字符串的位置</param>
        /// <param name="strTmp">要计算的字符串</param>
        /// <returns>字节的位置</returns>
        public static int GetByteIndex(int intIns, string strTmp)
        {
            int intReIns = 0;
            if (strTmp.Trim() == "")
            {
                return intIns;
            }
            for (int i = 0; i < strTmp.Length; i++)
            {
                if (System.Text.UTF8Encoding.UTF8.GetByteCount(strTmp.Substring(i, 1)) == 3)
                {
                    intReIns = intReIns + 2;
                }
                else
                {
                    intReIns = intReIns + 1;
                }
                if (intReIns >= intIns)
                {
                    intReIns = i + 1;
                    break;
                }
            }
            return intReIns;
        }
        
        #endregion
        /// <summary>
        /// 获取某一字符串在字符串中出现的次数
        /// </summary>
        public static int GetStringCount(string sourceString, string findString)
        {
            int count = 0;
            int findStringLength = findString.Length;
            string subString = sourceString;

            while (subString.IndexOf(findString) >= 0)
            {
                subString = subString.Substring(subString.IndexOf(findString) + findStringLength);
                count += 1;
            }
            return count;
        }

        //截取长度,num是英文字母的总数，一个中文算两个英文
        public static string GetLetter(string str, int iNum, bool bAddDot)
        {
            if (str == null || iNum <= 0) return "";

            if (str.Length < iNum && str.Length * 2 < iNum)
            {
                return str;
            }

            string sContent = str;
            int iTmp = iNum;

            char[] arrC;
            if (sContent.Length >= iTmp) //防止因为中文的原因使ToCharArray溢出
            {
                arrC = str.ToCharArray(0, iTmp);
            }
            else
            {
                arrC = str.ToCharArray(0, sContent.Length);
            }

            int i = 0;
            int iLength = 0;
            foreach (char ch in arrC)
            {
                iLength++;

                int k = (int)ch;
                if (k > 127 || k < 0)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }

                if (i > iTmp)
                {
                    iLength--;
                    break;
                }
                else if (i == iTmp)
                {
                    break;
                }
            }

            if (iLength < str.Length && bAddDot)
                sContent = sContent.Substring(0, iLength - 3) + "...";
            else
                sContent = sContent.Substring(0, iLength);

            return sContent;
        }
        #endregion 

        #region 1.5 字符串其他操作
        /// <summary>
        /// 清理字符串
        /// </summary>
        public static string CleanInput(string strIn)
        {
            return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", "");
        }

        //获得干净,无非法字符的字符串
        public static string GetCleanJsString(string str)
        {
            str = str.Replace("\"", "“");
            str = str.Replace("'", "”");
            str = str.Replace("\\", "\\\\");
            Regex re = new Regex(@"\r|\n|\t", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace);
            str = re.Replace(str, " ");

            return str;
        }
        /// <summary>
        /// 获取拆分符右边的字符串
        /// </summary>
        /// <param name="sourceString">字符串</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns></returns>
        public static string RightSplit(string sourceString, char splitChar)
        {
            string result = null;
            string[] tempString = sourceString.Split(splitChar);
            if (tempString.Length > 0)
            {
                result = tempString[tempString.Length - 1].ToString();
            }
            return result;
        }

        /// <summary>
        /// 获取拆分符左边的字符串
        /// </summary>
        /// <param name="sourceString">字符串</param>
        /// <param name="splitChar">分隔符</param>
        /// <returns></returns>
        public static string LeftSplit(string sourceString, char splitChar)
        {
            string result = null;
            string[] tempString = sourceString.Split(splitChar);
            if (tempString.Length > 0)
            {
                result = tempString[0].ToString();
            }
            return result;
        }

        /// <summary>
        /// 格式化字符串为SQL语句字段
        /// </summary>
        /// <param name="fldList"></param>
        /// <returns></returns>
        public static string GetSQLFildList(string fldList)
        {
            if (fldList == null)
                return "*";
            if (fldList.Trim() == "")
                return "*";
            if (fldList.Trim() == "*")
                return "*";
            //先去掉空格，[]符号
            string strTemp = fldList;
            strTemp = strTemp.Replace(" ", "");
            strTemp = strTemp.Replace("[", "").Replace("]", "");
            //为防止使用保留字，给所有字段加上[]
            strTemp = "[" + strTemp + "]";
            strTemp = strTemp.Replace('，', ',');
            strTemp = strTemp.Replace(",", "],[");
            return strTemp;
        }
        #endregion

        #endregion 字符串常用操作

        #region  2.全角半角
        /// <summary>
        /// 转全角的函数(SBC case)半角转全角
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ConvertToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            //for (int i = 0; i < c.Length; i++)
            //{
            //    byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
            //    if (b.Length == 2)
            //    {
            //        if (b[1] == 0)
            //        {
            //            b[0] = (byte)(b[0] - 32);
            //            b[1] = 255;
            //            c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
            //        }
            //    }
            //}
            return new string(c);
        }
        /// <summary>
        ///  转半角的函数(SBC case)全角转半角
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ConvertToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            //for (int i = 0; i < c.Length; i++)
            //{
            //    byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
            //    if (b.Length == 2)
            //    {
            //        if (b[1] == 255)
            //        {
            //            b[0] = (byte)(b[0] + 32);
            //            b[1] = 0;
            //            c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
            //        }
            //    }
            //}
            return new string(c);
        }
        #endregion  全角半角

        #region 3.字符串加密,编码
        #region 根据配置对指定字符串进行 MD5 加密
        /// <summary>
        /// 根据配置对指定字符串进行 MD5 加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string GetMD5(string s)
        {
            //md5加密
            s = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(s, "md5").ToString();
            return s;//.ToLower().Substring(8, 16);
        }
        #endregion

        #endregion 

        #region 4.字符串数组
        #region 判断指定字符串在指定字符串数组中的位置

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }

            }
            return -1;
        }


        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetInArrayID(string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }
        #endregion

        #endregion 字符串数组

        #region  5.字符串验证是否是纯数字
        
        #region 获取正确的Id，如果不是正整数，返回0
        /// <summary>
        /// 获取正确的Id，如果不是正整数，返回0
        /// </summary>
        /// <param name="_value"></param>
        /// <returns>返回正确的整数ID，失败返回0</returns>
        public static int StrToId(string _value)
        {
            if (IsNumberId(_value))
                return int.Parse(_value);
            else
                return 0;
        }
        #endregion
   
        #region 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。
        /// <summary>
        /// 检查一个字符串是否是纯数字构成的，一般用于查询字符串参数的有效性验证。(0除外)
        /// </summary>
        /// <param name="_value">需验证的字符串。。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool IsNumberId(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", _value);
        }
        #endregion
        #endregion

        #region 6.字符串样式
        #region 将字符串样式转换为纯字符串
        /// <summary>
        ///  将字符串样式转换为纯字符串
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            string RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                string NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }
        #endregion

        #region 将字符串转换为新样式
        /// <summary>
        /// 将字符串转换为新样式
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="NewStyle"></param>
        /// <param name="SplitString"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public static string GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)
        {
            string ReturnValue = "";
            //如果输入空值，返回空，并给出错误提示
            if (StrList == null)
            {
                ReturnValue = "";
                Error = "请输入需要划分格式的字符串";
            }
            else
            {
                //检查传入的字符串长度和样式是否匹配,如果不匹配，则说明使用错误。给出错误信息并返回空值
                int strListLength = StrList.Length;
                int NewStyleLength = GetCleanStyle(NewStyle, SplitString).Length;
                if (strListLength != NewStyleLength)
                {
                    ReturnValue = "";
                    Error = "样式格式的长度与输入的字符长度不符，请重新输入";
                }
                else
                {
                    //检查新样式中分隔符的位置
                    string Lengstr = "";
                    for (int i = 0; i < NewStyle.Length; i++)
                    {
                        if (NewStyle.Substring(i, 1) == SplitString)
                        {
                            Lengstr = Lengstr + "," + i;
                        }
                    }
                    if (Lengstr != "")
                    {
                        Lengstr = Lengstr.Substring(1);
                    }
                    //将分隔符放在新样式中的位置
                    string[] str = Lengstr.Split(',');
                    foreach (string bb in str)
                    {
                        StrList = StrList.Insert(int.Parse(bb), SplitString);
                    }
                    //给出最后的结果
                    ReturnValue = StrList;
                    //因为是正常的输出，没有错误
                    Error = "";
                }
            }
            return ReturnValue;
        }
        #endregion
        #endregion

        #region 7.快速验证一个字符串是否符合指定的正则表达式
        /// <summary>
        /// 快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="_express">正则表达式的内容。</param>
        /// <param name="_value">需验证的字符串。</param>
        /// <returns>是否合法的bool值。</returns>
        public static bool QuickValidate(string _express, string _value)
        {
            if (_value == null) return false;
            Regex myRegex = new Regex(_express);
            if (_value.Length == 0)
            {
                return false;
            }
            return myRegex.IsMatch(_value);
        }
        #endregion
    
        #region 8.判断对象是否为空
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
   
        

       
        #region 其他操作


        /// <summary>
        /// 简介：获得唯一的字符串
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueString()
        {
            Random rand = new Random();
            return ((int)(rand.NextDouble() * 10000)).ToString() + DateTime.Now.Ticks.ToString();
        }

        /// <summary>
        /// 统计char出现在string中的次数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="chr">字符</param>
        /// <returns></returns>
        public static int CharCount(string str, char chr)
        {
            int i = 0;
            for (int j = 0; j < str.Length; j++)
            {
                if (str[j] == chr)
                {
                    i++;
                }
            }

            return i;
        }


        #endregion 其他字符串操作


      
  
    }
}
