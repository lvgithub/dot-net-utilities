using System;
using System.Web;
using System.Threading;
using System.Diagnostics;

namespace Core.Common
{
    /// <summary>
    /// 系统操作相关的公共类
    /// </summary>    
    public static class SysHelper
    {
        #region 获取文件相对路径映射的物理路径
        /// <summary>
        /// 获取文件相对路径映射的物理路径
        /// </summary>
        /// <param name="virtualPath">文件的相对路径</param>        
        public static string GetPath(string virtualPath)
        {

            return HttpContext.Current.Server.MapPath(virtualPath);

        }
        #endregion

       

        #region 获取指定调用层级的方法名
        /// <summary>
        /// 获取指定调用层级的方法名
        /// </summary>
        /// <param name="level">调用的层数</param>        
        public static string GetMethodName(int level)
        {
            //创建一个堆栈跟踪
            StackTrace trace = new StackTrace();

            //获取指定调用层级的方法名
            return trace.GetFrame(level).GetMethod().Name;
        }
        #endregion

        

        #region 获取换行字符
        /// <summary>
        /// 获取换行字符
        /// </summary>
        public static string NewLine
        {
            get
            {
                return Environment.NewLine;
            }
        }
        #endregion

        #region 获取当前应用程序域
        /// <summary>
        /// 获取当前应用程序域
        /// </summary>
        public static AppDomain CurrentAppDomain
        {
            get
            {
                return Thread.GetDomain();
            }
        }
        #endregion
       
        #region 获取Windows Form应用程序的名字
        /// <summary>
        /// 获取当前Windows Form应用程序的名字,不包括.exe
        /// </summary>
        public static string WinFormName
        {
            get
            {
                //获取应用程序的完全路径
                string appPath = "";//MediaTypeNames.Application.ExecutablePath;

                //找到最后一个'\'的位置
                int beginIndex = appPath.LastIndexOf(@"\");

                //找到".exe"的位置
                int endIndex = appPath.ToLower().LastIndexOf(".exe");

                //返回Windows Form应用程序的名字
                return appPath.Substring(beginIndex + 1, endIndex - beginIndex - 1);
            }
        }
        #endregion

    }
}
