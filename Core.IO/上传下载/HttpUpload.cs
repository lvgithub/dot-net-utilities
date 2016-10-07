using System;
using System.Web;

namespace Core.IO
{
    public class HttpUpload
    {
        /// <summary>
        /// 判断是否有上传的文件
        /// </summary>
        /// <returns>是否有上传的文件</returns>
        public static bool IsPostFile()
        {
            for (int i = 0; i < HttpContext.Current.Request.Files.Count; i++)
            {
                if (HttpContext.Current.Request.Files[i].FileName != "")
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 保存用户上传的文件
        /// </summary>
        /// <param name="path">保存路径</param>
        public static void SaveRequestFile(string path)
        {
            if (HttpContext.Current.Request.Files.Count > 0)
            {
                HttpContext.Current.Request.Files[0].SaveAs(path);
            }
        }

        /// <summary>
        /// 保存上传的文件
        /// </summary>
        /// <param name="MaxAllowFileCount">最大允许的上传文件个数</param>
        /// <param name="MaxAllowFileSize">最大允许的文件长度(单位: KB)</param>
        /// <param name="AllowFileExtName">允许的文件扩展名, 以string[]形式提供</param>
        /// <param name="AllowFileType">允许的文件类型, 以string[]形式提供</param>
        /// <param name="Dir">目录</param>
        /// <returns></returns>
        public static string[] SaveRequestFiles(int MaxAllowFileCount, int MaxAllowFileSize, string[] AllowFileExtName, string[] AllowFileType, string Dir)
        {
            string[] attachmentinfo = new string[MaxAllowFileCount];
            int fcount = Math.Min(MaxAllowFileCount, HttpContext.Current.Request.Files.Count);
            for (int i = 0; i < fcount; i++)
            {
                string filename = HttpContext.Current.Request.Files[i].FileName;
                string fileextname = filename.Substring(filename.LastIndexOf("."));
                string filetype = HttpContext.Current.Request.Files[i].ContentType;
                int filesize = HttpContext.Current.Request.Files[i].ContentLength;
                // 判断 文件扩展名/文件大小/文件类型 是否符合要求
                if (InArray(fileextname, AllowFileExtName) && 
                    (filesize <= MaxAllowFileSize * 1024) &&
                    InArray(filetype, AllowFileType))
                {
                    HttpContext.Current.Request.Files[i].SaveAs(Dir + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.TickCount.ToString() + fileextname);
                    attachmentinfo[i] = Dir + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + Environment.TickCount.ToString() + fileextname;
                }
            }
            return attachmentinfo;

        }

        #region
        

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        private static bool InArray(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        private static bool InArray(string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }


        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        private static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
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

        #endregion


    }
}
