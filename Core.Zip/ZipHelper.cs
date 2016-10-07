using System;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;

namespace Core.Zip
{
    /// <summary>
    /// 用系统WinRar进行压缩和解压缩
    /// </summary>
    public class ZipHelper
    {
        #region 私有变量
        static String the_rar; //WinRAR.exe 的完整路径 
        static RegistryKey the_Reg; //注册表键 
        static Object the_Obj; //键值 
        static String the_Info;  //cmd命令值
        static ProcessStartInfo the_StartInfo;
        static Process the_Process;
        #endregion

        //64位系统
        //此时会提示：未将对象引用设置为对象的实例 
        //解决办法：修改注册表，添加如下项：
        //HKEY_CLASSES_ROOT\Applications\WinRAR.exe\Shell\Open\Command 
        //值为："C:\Program Files (x86)\WinRAR\WinRAR.exe" "%1"

        #region 调用外部RAR解压缩
        private static string rarRegPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe";

        /// <summary>
        /// 是否安装了Winrar
        /// </summary>
        /// <returns></returns>
        public static bool Exists()
        {
            RegistryKey the_Reg = Registry.LocalMachine.OpenSubKey(rarRegPath);
            return !string.IsNullOrEmpty(the_Reg.GetValue("").ToString());
        }



        #endregion
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="path">要压缩文件路径</param>
        /// <param name="rarPath">要压缩的文件名</param>
        /// <param name="rarName">压缩的文件路径</param>
        public static void EnZip(string path, string rarName, string rarPath)
        {
            //   bool flag = false;
            try
            {
                the_Reg = Registry.ClassesRoot.OpenSubKey(@"Applications\WinRAR.exe\Shell\Open\Command");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                the_rar = the_rar.Substring(1, the_rar.Length - 7);
                // Directory.CreateDirectory(path);
                //
                //the_Info = " a    " + rarName + "  " + @"C:Test70821.txt"; //文件压缩
                the_Info = " a   " + rarPath + "  " + path;
                #region 命令参数
                //// 1
                ////压缩即文件夹及其下文件
                //the_Info = " a    " + rarName + "  " + path + "  -r";              
                //// 2
                ////压缩即文件夹及其下文件 设置压缩方式为 .zip
                //the_Info = " a -afzip  " + rarName + "  " + path;  
                //// 3
                ////压缩文件夹及其下文件 直接设定为free.zip
                //the_Info = " a -r  " + rarName + "  " + path;
                //// 4
                ////搬迁压缩即文件夹及其下文件原文件将不存在
                //the_Info = " m  " + rarName + "  " + path;
                //// 5
                ////压缩即文件  直接设定为free.zip 只有文件 而没有文件夹
                //the_Info = " a -ep  " + rarName + "  " + path;
                //// 6
                ////加密压缩即文件夹及其下文件 密码为123456 注意参数间不要空格
                //the_Info = " a -p123456  " + rarName + "  " + path;
                #endregion


                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //打包文件存放目录
                the_StartInfo.WorkingDirectory = rarName;
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
                //if (the_Process.HasExited)
                //{
                //    flag = true;
                //}

                the_Process.Close();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            // return flag;
        }

        /// <summary>
        /// 解压缩
        /// </summary>
        /// <param name="zipname">要解压的文件名</param>
        /// <param name="zippath">要解压的文件路径</param>
        public static void DeZip(string zipname, string zippath)
        {
            try
            {
                the_Reg = Registry.ClassesRoot.OpenSubKey(@"Applications\WinRar.exe\Shell\Open\Command");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_Reg.Close();
                the_rar = the_rar.Substring(1, the_rar.Length - 7);
                the_Info = " X " + zipname + " " + zippath;
                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                the_Process.WaitForExit();
  
                the_Process.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }


}
