using System;
using System.Web.UI.HtmlControls;
using System.IO;

namespace Core.IO
{
    /// <summary>
    /// HtmlInputFileControl 的摘要说明。上传类
    /// </summary>
    public class HtmlInputFileControl
    {
      

        #region IsAllowedExtension是否允许该扩展名上传

        public static bool IsAllowedExtension(HtmlInputFile hifile)
        {
            string strOldFilePath = "", strExtension = "";

            //允许上传的扩展名，可以改成从配置文件中读出
            string[] arrExtension = { ".gif", ".GIF", ".JPG", ".jpg", ".JPEG", ".BMP", ".PNG", ".jpeg", ".bmp", ".png" };

            if (hifile.PostedFile.FileName != string.Empty)
            {
                strOldFilePath = hifile.PostedFile.FileName;
                //取得上传文件的扩展名
                strExtension = strOldFilePath.Substring(strOldFilePath.LastIndexOf("."));
                //判断该扩展名是否合法
                for (int i = 0; i < arrExtension.Length; i++)
                {
                    if (strExtension.Equals(arrExtension[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        #endregion

        #region IsAllowedLength判断上传文件大小是否超过最大值
        public static bool IsAllowedLength(HtmlInputFile hifile)
        {
            //允许上传文件大小的最大值,可以保存在xml文件中,单位为KB
            int i = 512;
            //如果上传文件的大小超过最大值,返回flase,否则返回true.
            if (hifile.PostedFile.ContentLength > i * 512)
            {
                return false;
            }
            return true;
        }
        #endregion

        #region SaveFile上传文件并返回文件名
        public static string SaveFile(HtmlInputFile hifile, string strAbsolutePath)
        {
            string strOldFilePath = "", strExtension = "", strNewFileName = "";

            if (hifile.PostedFile.FileName != string.Empty)
            {
                strOldFilePath = hifile.PostedFile.FileName;
                //取得上传文件的扩展名
                strExtension = strOldFilePath.Substring(strOldFilePath.LastIndexOf("."));
                //文件上传后的命名
                strNewFileName = GetUniqueString() + strExtension;
                if (strAbsolutePath.LastIndexOf("\\") == strAbsolutePath.Length)
                {
                    hifile.PostedFile.SaveAs(strAbsolutePath + strNewFileName);
                }
                else
                {
                    hifile.PostedFile.SaveAs(strAbsolutePath + "\\" + strNewFileName);
                }
            }
            return strNewFileName;
        }
        #endregion

        #region CoverFile重新上传文件，删除原有文件
        public static void CoverFile(HtmlInputFile ffFile, string strAbsolutePath, string strOldFileName)
        {
            //获得新文件名
            string strNewFileName = GetUniqueString();

            if (ffFile.PostedFile.FileName != string.Empty)
            {
                //旧图片不为空时先删除旧图片
                if (strOldFileName != string.Empty)
                {
                    DeleteFile(strAbsolutePath, strOldFileName);
                }
                SaveFile(ffFile, strAbsolutePath);
            }
        }
        #endregion

        #region DeleteFile删除指定文件
        public static void DeleteFile(string strAbsolutePath, string strFileName)
        {
            if (strAbsolutePath.LastIndexOf("\\") == strAbsolutePath.Length)
            {
                if (File.Exists(strAbsolutePath + strFileName))
                {
                    File.Delete(strAbsolutePath + strFileName);
                }
            }
            else
            {
                if (File.Exists(strAbsolutePath + "\\" + strFileName))
                {
                    File.Delete(strAbsolutePath + "\\" + strFileName);
                }
            }
        }
        #endregion

  
        #region 获取一个不重复的文件名
        /// <summary>
        /// 获取一个不重复的文件名
        /// </summary>
        /// <returns></returns>
        public static string GetUniqueString()
        {
            return DateTime.Now.ToString("yyyyMMddhhmmss");
        }
        #endregion


    }
}
