using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Web;

namespace Core.IO
{
    /// <summary>
    /// 文件操作操作类
    /// 1、获取一个文件的长度
    /// 1.1获取一个文件的长度,单位为Byte GetFileSize(string filePath)
    /// 1.2获取一个文件的长度,单位为KB   GetFileSizeKB(string filePath)
    /// 1.3获取一个文件的长度,单位为MB   GetFileSizeMB(string filePath)
    /// 2、对文件内容操作 
    /// 2.1写文件  WriteFile(string Path, string Strings)
    /// 2.2读文件 ReadFile(string Path)
    /// 2.3向文本文件中写入内容 WriteText(string filePath, string content)
    /// 2.4向文本文件的尾部追加内容AppendText(string filePath, string content)
    /// 2.5将源文件的内容复制到目标文件中Copy(string sourceFilePath, string destFilePath)
    /// 2.6 备份文件  BackupFile(string sourceFileName, string destFileName, bool overwrite)
    /// 2.7 恢复文件  RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
    /// 3、检测文件是否存在
    /// 3.1 检测指定目录是否存在IsExistDirectory(string directoryPath)
    /// 3.2 检测指定文件是否存在,如果存在则返回true IsExistFile(string filePath)
    /// 3.3检测指定目录是否为空
    /// 3.4检测指定目录中是否存在指定的文件
    /// 4、获取指定目录、文件列表
    /// 4.1获取指定目录中所有文件列表GetFileNames(string directoryPath)
    /// 4.2获取指定目录及子目录中所有文件列表GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
    /// 4.3获取指定目录及子目录中所有子目录列表 GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
    /// 4.4获取指定目录中所有子目录列表GetDirectories(string directoryPath)
    /// 4.5读取文件列表 GetFileItems(string path)
    /// 5、创建目录及文件
    /// 5.1创建目录   CreateDir(string dir)
    /// 5.2创建一个目录  CreateDirectory(string directoryPath)
    /// 5.3创建文件      CreateFile(string dir, string pagestr)
    /// 5.4创建一个文件  CreateFile(string filePath)
    /// 5.5创建一个文件,并将字节流写入文件  CreateFile(string filePath, byte[] buffer)
    /// 5.6检查文件,如果文件不存在则创建  ExistsFile(string FilePath)
    /// 5.7创建一个零字节临时文件   CreateTempZeroByteFile()
    /// 5.8创建一个随机文件名，不创建文件本身 GetRandomFileName()
    /// 6、删除文件及目录
    /// 6.1删除目录 DeleteDir(string dir)
    /// 6.2删除文件 DeleteFile(string file)
    /// 6.3删除指定目录及其所有子目录   DeleteDirectory(string directoryPath)
    /// 6.4删除指定文件夹对应其他文件夹里的文件 DeleteFolderFiles(string varFromDirectory, string varToDirectory)
    /// 7、清空目录及文件内容
    /// 7.1清空指定目录下所有文件及子目录,但该目录依然保存. ClearDirectory(string directoryPath)
    /// 7.2清空文件内容  ClearFile(string filePath)
    /// 8、复制、移动
    /// 8.1移动文件(剪贴--粘贴)  MoveFile(string dir1, string dir2)
    /// 8.2复制文件       CopyFile(string dir1, string dir2)
    /// 8.3复制文件夹(递归)    CopyFolder(string varFromDirectory, string varToDirectory)
    /// 8.4将文件移动到指定目录   Move(string sourceFilePath, string descDirectoryPath)       
    /// 8.5复制文件参考方法,页面中引用
    /// 8.6指定文件夹下面的所有内容copy到目标文件夹下面
    /// 9、文件路径
    /// 9.1从文件的绝对路径中获取文件名( 包含扩展名 )
    /// 9.2从文件的绝对路径中获取文件名( 不包含扩展名 )
    /// 9.3从文件的绝对路径中获取扩展名
    /// 9.4取得文件后缀名
    /// 10、文件类型其他操作
    /// 10.1文件是否存在或无权访问 FileIsExist(string path)
    /// 10.2文件是否只读 FileIsReadOnly(string fullpath)
    /// 10.3设置文件是否只读   SetFileReadonly(string fullpath, bool flag)
    /// 10.4取文件名    GetFileName(string fullpath, bool removeExt)
    /// 10.5 取文件创建时间   GetFileCreateTime(string fullpath)
    /// 10.6 取文件最后存储时间  GetLastWriteTime(string fullpath)
    /// 10.7判断两个文件的哈希值是否一致  CompareFilesHash(string fileName1, string fileName2)
    /// 10.8获取文本文件的行数  GetLineCount(string filePath)
    /// 10.9获取指定文件详细属性   GetFileAttibe(string filePath)
    /// 11.1获取文件夹大小      GetDirectoryLength(string dirPath)
    /// </summary>
   
    public class FileHelper
    {
        
        #region 1、获取一个文件的长度
        /// <summary>
        /// 1.1获取一个文件的长度,单位为Byte
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static int GetFileSize(string filePath)
        {
            //创建一个文件对象
            FileInfo fi = new FileInfo(filePath);

            //获取文件的大小
            return (int)fi.Length;
        }

        /// <summary>
        /// 1.2获取一个文件的长度,单位为KB
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        public static double GetFileSizeKB(string filePath)
        {
            //创建一个文件对象
            FileInfo fi = new FileInfo(filePath);

            //获取文件的大小
            return ToDouble(Convert.ToDouble(fi.Length) / 1024,1);
        }

        /// <summary>
        /// 1.3获取一个文件的长度,单位为MB
        /// </summary>
        /// <param name="filePath">文件的路径</param>
        public static double GetFileSizeMB(string filePath)
        {
            //创建一个文件对象
            FileInfo fi = new FileInfo(filePath);

            //获取文件的大小
            return ToDouble(Convert.ToDouble(fi.Length) / 1024 / 1024, 1);
        }
        #region     ToDouble 
        private static double ToDouble(object data, double defValue)
        {
            //如果为空则返回默认值
            if (data == null || Convert.IsDBNull(data))
            {
                return defValue;
            }

            try
            {
                return Convert.ToDouble(data);
            }
            catch
            {
                return defValue;
            }

        }
        #endregion
        #endregion 获取一个文件的长度

        #region 2、对文件内容操作

        #region 2.1写文件
        /// <summary>
        /// 2.1写文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Strings">文件内容</param>
        public static void WriteFile(string Path, string Strings)
        {

            if (!File.Exists(Path))
            {
                FileStream f = File.Create(Path);
                f.Close();
                f.Dispose();
            }
            StreamWriter f2 = new StreamWriter(Path, true, System.Text.Encoding.UTF8);
            f2.WriteLine(Strings);
            f2.Close();
            f2.Dispose();


        }
        #endregion

        #region 2.2读文件
        /// <summary>
        /// 2.2.1读文件
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string Path)
        {
            string s = "";
            if (!System.IO.File.Exists(Path))
                s = "不存在相应的目录";
            else
            {
                StreamReader f2 = new StreamReader(Path, System.Text.Encoding.GetEncoding("gb2312"));
                s = f2.ReadToEnd();
                f2.Close();
                f2.Dispose();
            }

            return s;
        }
        /// <summary>
        /// 2.2.2读取文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="encoding"></param>
        /// <param name="isCache"></param>
        /// <returns></returns>
        public static string ReadFile(string filename, Encoding encoding, bool isCache)
        {
            string body;
            if (isCache)
            {
                body = (string)HttpContext.Current.Cache[filename];
                if (body == null)
                {
                    body = ReadFile(filename, encoding, false);
                    HttpContext.Current.Cache.Add(filename, body, new System.Web.Caching.CacheDependency(filename), DateTime.MaxValue, TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
                }
            }
            else
            {
                using (StreamReader sr = new StreamReader(filename, encoding))
                {
                    body = sr.ReadToEnd();
                }
            }

            return body;
        }

        #endregion

        #region   2.3向文本文件中写入内容
        /// <summary>
        /// 2.3向文本文件中写入内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="content">写入的内容</param>
        public static void WriteText(string filePath, string content)
        {
            //向文件写入内容
            File.WriteAllText(filePath, content, Encoding.Default);
      
        }
        #endregion
   
        #region   2.4向文本文件的尾部追加内容
        /// <summary>
        /// 2.4向文本文件的尾部追加内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="content">写入的内容</param>
        public static void AppendText(string filePath, string content)
        {
            File.AppendAllText(filePath, content, Encoding.Default);
            //StreamWriter sw = File.AppendText(filePath);
            //sw.Write(content);
            //sw.Flush();
            //sw.Close();
            //sw.Dispose();
        }
        #endregion
     
        #region 2.5将现有文件的内容复制到新文件中
        /// <summary>
        /// 2.5将源文件的内容复制到目标文件中
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="destFilePath">目标文件的绝对路径</param>
        public static void Copy(string sourceFilePath, string destFilePath)
        {
            File.Copy(sourceFilePath, destFilePath, true);
        }
        #endregion
  
        #region  2.6备份文件

        /// <summary>
        /// 2.6备份文件
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!System.IO.File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "文件不存在！");
            }
            if (!overwrite && System.IO.File.Exists(destFileName))
            {
                return false;
            }
            try
            {
                System.IO.File.Copy(sourceFileName, destFileName, true);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
   
        #region  2.7 恢复文件
        /// <summary>
        ///2.7 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <param name="backupTargetFileName">要恢复文件再次备份的名称,如果为null,则不再备份恢复文件</param>
        /// <returns>操作是否成功</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!System.IO.File.Exists(backupFileName))
                {
                    throw new FileNotFoundException(backupFileName + "文件不存在！");
                }
                if (backupTargetFileName != null)
                {
                    if (!System.IO.File.Exists(targetFileName))
                    {
                        throw new FileNotFoundException(targetFileName + "文件不存在！无法备份此文件！");
                    }
                    else
                    {
                        System.IO.File.Copy(targetFileName, backupTargetFileName, true);
                    }
                }
                System.IO.File.Delete(targetFileName);
                System.IO.File.Copy(backupFileName, targetFileName);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }
        #endregion
        #endregion

        #region 3、检测指定文件目录是否存在

        #region  3.1检测指定目录是否存在

        /// <summary>
        /// 3.1检测指定目录是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string directoryPath)
        {
            return Directory.Exists(directoryPath);
        }
        #endregion

        #region  3.2检测指定文件是否存在,如果存在则返回true

        /// <summary>
        /// 3.2检测指定文件是否存在,如果存在则返回true
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static bool IsExistFile(string filePath)
        {
            return File.Exists(filePath);
        }
        #endregion
     
        #region 3.3检测指定目录是否为空
        /// <summary>
        /// 3.3检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //判断是否存在文件夹
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch
            {
                //这里记录日志
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return true;
            }
        }
        #endregion

        #region 3.4检测指定目录中是否存在指定的文件
        /// <summary>
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>        
        public static bool ContainFile(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param> 
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static bool ContainFile(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, true);

                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
            }
        }
        #endregion
        
        #endregion

        #region 4、获取指定目录、文件列表
     
        #region 4.1获取指定目录中的文件列表
        /// <summary>
        /// 4.1获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetFileNames(string directoryPath)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            //获取文件列表
            return Directory.GetFiles(directoryPath);
        }
        #endregion

        #region 4.2获取指定目录及子目录中所有文件列表
        /// <summary>
        /// 4.2获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常
            if (!IsExistDirectory(directoryPath))
            {
                throw new FileNotFoundException();
            }

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 4.3获取指定目录中的子目录列表
        /// <summary>
        /// 4.3获取指定目录及子目录中所有子目录列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetDirectories(string directoryPath, string searchPattern, bool isSearchChild)
        {
            try
            {
                if (isSearchChild)
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetDirectories(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion
   
        #region 4.4获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// <summary>
        /// 4.4获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 4.5 读取文件列表
       
        /// <summary>
        /// 4.5读取文件列表
        /// </summary>
        public static List<FileItem> GetFileItems(string path)
        {
            List<FileItem> list = new List<FileItem>();
            string[] files = Directory.GetFiles(path);
            foreach (string s in files)
            {
                FileItem item = new FileItem();
                FileInfo fi = new FileInfo(s);
                item.Name = fi.Name;
                item.FullName = fi.FullName;
                item.CreationDate = fi.CreationTime;
                item.IsFolder = true;
                item.Size = fi.Length;
                list.Add(item);
            }
            return list;
        }
        #endregion 获取指定目录、文件列表
        #endregion
    
        #region 5、创建目录及文件

        #region 创建目录
        /// <summary>
        /// 5.1创建目录
        /// </summary>
        /// <param name="dir">要创建的目录路径包括目录名</param>
        public static void CreateDir(string dir)
        {
            if (dir.Length == 0) return;
            if (!Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                Directory.CreateDirectory(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
        }

        /// <summary>
        /// 5.2创建一个目录
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static void CreateDirectory(string directoryPath)
        {
            //如果目录不存在则创建该目录
            if (!IsExistDirectory(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
        #endregion

        #region 创建文件
        /// <summary>
        /// 5.3创建文件
        /// </summary>
        /// <param name="dir">带后缀的文件名</param>
        /// <param name="pagestr">文件内容</param>
        public static void CreateFile(string dir, string pagestr)
        {
            dir = dir.Replace("/", "\\");
            if (dir.IndexOf("\\") > -1)
                CreateDir(dir.Substring(0, dir.LastIndexOf("\\")));
            StreamWriter sw = new StreamWriter(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir, false, Encoding.GetEncoding("GB2312"));
            sw.Write(pagestr);
            sw.Close();
        }
        #endregion

        #region 创建一个文件
        /// <summary>
        /// 5.4创建一个文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void CreateFile(string filePath)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象
                    FileInfo file = new FileInfo(filePath);
                    //创建文件  
                    FileStream fs = file.Create();
                   
                   // File.Create(filePath);
                    
                    //关闭文件流
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }
      
        /// <summary>
        /// 5.5创建一个文件,并将字节流写入文件
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="buffer">二进制流数据</param>
        public static void CreateFile(string filePath, byte[] buffer)
        {
            try
            {
                //如果文件不存在则创建该文件
                if (!IsExistFile(filePath))
                {
                    //创建一个FileInfo对象
                    FileInfo file = new FileInfo(filePath);
                    //创建文件
                    FileStream fs = file.Create();
                    //写入二进制流
                    fs.Write(buffer, 0, buffer.Length);
                    //关闭文件流
                    fs.Close();

                    ////创建文件
                    //using (FileStream fs = File.Create(filePath))
                    //{
                    //    //写入二进制流
                    //    fs.Write(buffer, 0, buffer.Length);

                    //}
                }
            }
            catch (Exception ex)
            {
                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                throw ex;
            }
        }
        #endregion
    
        #region 检查文件,如果文件不存在则创建
        /// <summary>
        /// 5.6检查文件,如果文件不存在则创建  
        /// </summary>
        /// <param name="FilePath">路径,包括文件名</param>
        public static void ExistsFile(string FilePath)
        {
            //if(!File.Exists(FilePath))    
            //File.Create(FilePath);    
            //以上写法会报错,详细解释请看下文.........   
            if (!File.Exists(FilePath))
            {
                FileStream fs = File.Create(FilePath);
                fs.Close();
            }
        }
        #endregion

        /// <summary>
        /// 5.7创建一个零字节临时文件
        /// </summary>
        /// <returns></returns>
        public static string CreateTempZeroByteFile()
        {
            return Path.GetTempFileName();
        }

        /// <summary>
        /// 5.8创建一个随机文件名，不创建文件本身
        /// </summary>
        /// <returns></returns>
        public static string GetRandomFileName()
        {
            return Path.GetRandomFileName();
        }
        #endregion

        #region  6、删除文件及目录
        #region 删除目录
        /// <summary>
        /// 6.1删除目录
        /// </summary>
        /// <param name="dir">要删除的目录路径和名称</param>
        public static void DeleteDir(string dir)
        {
            if (dir.Length == 0) return;
            if (Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir))
                Directory.Delete(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir);
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 6.2删除文件
        /// </summary>
        /// <param name="file">要删除的文件路径和名称</param>
        public static void DeleteFile(string file)
        {
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + file))
                File.Delete(HttpContext.Current.Request.PhysicalApplicationPath + file);
        }
        #endregion

        #region 删除指定目录
        /// <summary>
        /// 6.3删除指定目录及其所有子目录
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void DeleteDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                Directory.Delete(directoryPath, true);
            }
        }
        #endregion

        #region 删除指定文件夹对应其他文件夹里的文件
        /// <summary>
        /// 6.4删除指定文件夹对应其他文件夹里的文件
        /// </summary>
        /// <param name="varFromDirectory">指定文件夹路径</param>
        /// <param name="varToDirectory">对应其他文件夹路径</param>
        public static void DeleteFolderFiles(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    DeleteFolderFiles(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }


            string[] files = Directory.GetFiles(varFromDirectory);

            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Delete(varToDirectory + s.Substring(s.LastIndexOf("\\")));
                }
            }
        }
        #endregion
        #endregion

        #region 7、清空目录及文件内容
        
        #region 7.1清空指定目录
        /// <summary>
        /// 7.1清空指定目录下所有文件及子目录,但该目录依然保存.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        public static void ClearDirectory(string directoryPath)
        {
            if (IsExistDirectory(directoryPath))
            {
                //删除目录中所有的文件
                string[] fileNames = GetFileNames(directoryPath);
                for (int i = 0; i < fileNames.Length; i++)
                {
                    DeleteFile(fileNames[i]);
                }

                //删除目录中所有的子目录
                string[] directoryNames = GetDirectories(directoryPath);
                for (int i = 0; i < directoryNames.Length; i++)
                {
                    DeleteDirectory(directoryNames[i]);
                }
            }
        }
        #endregion

        #region 7.2清空文件内容
        /// <summary>
        /// 7.2清空文件内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void ClearFile(string filePath)
        {
            //删除文件
            File.Delete(filePath);

            //重新创建该文件
            CreateFile(filePath);
        }
        #endregion
        #endregion

        #region 8、复制、移动
       
        #region 8.1移动文件(剪贴--粘贴)
        /// <summary>
        /// 8.1移动文件(剪贴--粘贴)
        /// </summary>
        /// <param name="dir1">要移动的文件的路径及全名(包括后缀)</param>
        /// <param name="dir2">文件移动到新的位置,并指定新的文件名</param>
        public static void MoveFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
                File.Move(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1, HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2);
        }
        #endregion

        #region 8.2复制文件
        /// <summary>
        /// 8.2复制文件
        /// </summary>
        /// <param name="dir1">要复制的文件的路径已经全名(包括后缀)</param>
        /// <param name="dir2">目标位置,并指定新的文件名</param>
        public static void CopyFile(string dir1, string dir2)
        {
            dir1 = dir1.Replace("/", "\\");
            dir2 = dir2.Replace("/", "\\");
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1))
            {
                File.Copy(HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir1, HttpContext.Current.Request.PhysicalApplicationPath + "\\" + dir2, true);
            }
        }
        #endregion

        #region 8.3复制文件夹
        /// <summary>
        /// 8.3复制文件夹(递归)
        /// </summary>
        /// <param name="varFromDirectory">源文件夹路径</param>
        /// <param name="varToDirectory">目标文件夹路径</param>
        public static void CopyFolder(string varFromDirectory, string varToDirectory)
        {
            Directory.CreateDirectory(varToDirectory);

            if (!Directory.Exists(varFromDirectory)) return;

            string[] directories = Directory.GetDirectories(varFromDirectory);

            if (directories.Length > 0)
            {
                foreach (string d in directories)
                {
                    CopyFolder(d, varToDirectory + d.Substring(d.LastIndexOf("\\")));
                }
            }
            string[] files = Directory.GetFiles(varFromDirectory);
            if (files.Length > 0)
            {
                foreach (string s in files)
                {
                    File.Copy(s, varToDirectory + s.Substring(s.LastIndexOf("\\")), true);
                }
            }
        }
        #endregion

        #region 8.4将文件移动到指定目录
        /// <summary>
        /// 8.4将文件移动到指定目录
        /// </summary>
        /// <param name="sourceFilePath">需要移动的源文件的绝对路径</param>
        /// <param name="descDirectoryPath">移动到的目录的绝对路径</param>
        public static void Move(string sourceFilePath, string descDirectoryPath)
        {
            //获取源文件的名称
            string sourceFileName = GetFileName(sourceFilePath);

            if (Directory.Exists(descDirectoryPath))
            {
                //如果目标中存在同名文件,则删除
                if (IsExistFile(descDirectoryPath + "\\" + sourceFileName))
                {
                    DeleteFile(descDirectoryPath + "\\" + sourceFileName);
                }
                //将文件移动到指定目录
                File.Move(sourceFilePath, descDirectoryPath + "\\" + sourceFileName);
            }
        }
        #endregion

        #region 8.5复制文件参考方法,页面中引用

        /// <summary>
        /// 8.5复制文件参考方法,页面中引用
        /// </summary>
        /// <param name="cDir">新路径</param>
        /// <param name="TempId">模板引擎替换编号</param>
        public static void CopyFiles(string cDir, string TempId)
        {
            //if (Directory.Exists(Request.PhysicalApplicationPath + "\\Controls"))
            //{
            //    string TempStr = string.Empty;
            //    StreamWriter sw;
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Default.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Default.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Column.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\List.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Content.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\View.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\MoreDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\DissList.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\ShowDiss.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Diss.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Review.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Review.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //    if (File.Exists(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx"))
            //    {
            //        TempStr = File.ReadAllText(Request.PhysicalApplicationPath + "\\Controls\\Search.aspx");
            //        TempStr = TempStr.Replace("{$ChannelId$}", TempId);

            //        sw = new StreamWriter(Request.PhysicalApplicationPath + "\\" + cDir + "\\Search.aspx", false, System.Text.Encoding.GetEncoding("GB2312"));
            //        sw.Write(TempStr);
            //        sw.Close();
            //    }
            //}
        }

        #endregion

        #region 8.6将指定文件夹下面的所有内容copy到目标文件夹下面 果目标文件夹为只读属性就会报错。
        /// <summary>
        /// 8.6指定文件夹下面的所有内容copy到目标文件夹下面
        /// </summary>
        /// <param name="srcPath">原始路径</param>
        /// <param name="aimPath">目标文件夹</param>
        public static void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                //如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                //string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                //遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    //先当作目录处理如果存在这个目录就递归Copy该目录下面的文件

                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    //否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.ToString());
            }
        }
        #endregion
        #endregion

        #region  9、文件路径
        
        #region 9.1从文件的绝对路径中获取文件名( 包含扩展名 )
        /// <summary>
        /// 9.1从文件的绝对路径中获取文件名( 包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileName(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Name;
        }
        #endregion

        #region 9.2从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// <summary>
        /// 9.2从文件的绝对路径中获取文件名( 不包含扩展名 )
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileNameNoExtension(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Name.Split('.')[0];
        }
        #endregion

        #region 9.3从文件的绝对路径中获取扩展名
        /// <summary>
        /// 9.3从文件的绝对路径中获取扩展名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetExtension(string filePath)
        {
            //获取文件的名称
            FileInfo fi = new FileInfo(filePath);
            return fi.Extension;
        }
        #endregion

        #region 9.4取得文件后缀名
        /// <summary>
        /// 取后缀名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>.gif|.html格式</returns>
        public static string GetPostfixStr(string filename)
        {
            int start = filename.LastIndexOf(".");
            int length = filename.Length;
            string postfix = filename.Substring(start, length - start);
            return postfix;
        }  
        #endregion
        #endregion

        #region 10、文件类型其他操作

        #region  10.1文件是否存在或无权访问
        /// <summary>
        /// 10.1文件是否存在或无权访问
        /// </summary>
        /// <param name="path">相对路径或绝对路径</param>
        /// <returns>如果是目录也返回false</returns>
        public static bool FileIsExist(string path)
        {
            return File.Exists(path);
        }
        #endregion
   
        #region  10.2文件是否只读
        /// <summary>
        /// 10.2文件是否只读
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static bool FileIsReadOnly(string fullpath)
        {
            FileInfo file = new FileInfo(fullpath);
            if ((file.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
   
        #region  10.3设置文件是否只读

        /// <summary>
        /// 10.3设置文件是否只读
        /// </summary>
        /// <param name="fullpath"></param>
        /// <param name="flag">true表示只读，反之</param>
        public static void SetFileReadonly(string fullpath, bool flag)
        {
            FileInfo file = new FileInfo(fullpath);

            if (flag)
            {
                // 添加只读属性
                file.Attributes |= FileAttributes.ReadOnly;
            }
            else
            {
                // 移除只读属性
                file.Attributes &= ~FileAttributes.ReadOnly;
            }
        }
        #endregion
   
        #region  10.4 取文件名

        /// <summary>
        /// 10.4取文件名
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static string GetFileName(string fullpath, bool removeExt)
        {
            FileInfo fi = new FileInfo(fullpath);
            string name = fi.Name;
            if (removeExt)
            {
                name = name.Remove(name.IndexOf('.'));
            }
            return name;
        }
        #endregion
    
        #region 10.5 取文件创建时间
        /// <summary>
        /// 10.5取文件创建时间
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static DateTime GetFileCreateTime(string fullpath)
        {
            FileInfo fi = new FileInfo(fullpath);
            return fi.CreationTime;
        }
        #endregion
    
        #region  10.6 取文件最后存储时间

        /// <summary>
        ///10.6 取文件最后存储时间
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public static DateTime GetLastWriteTime(string fullpath)
        {
            FileInfo fi = new FileInfo(fullpath);
            return fi.LastWriteTime;
        }
        #endregion
    
        #region 10.7判断两个文件的哈希值是否一致

        /// <summary>
        /// 10.7判断两个文件的哈希值是否一致
        /// </summary>
        /// <param name="fileName1"></param>
        /// <param name="fileName2"></param>
        /// <returns></returns>
        public static bool CompareFilesHash(string fileName1, string fileName2)
        {
            using (HashAlgorithm hashAlg = HashAlgorithm.Create())
            {
                using (FileStream fs1 = new FileStream(fileName1, FileMode.Open), fs2 = new FileStream(fileName2, FileMode.Open))
                {
                    byte[] hashBytes1 = hashAlg.ComputeHash(fs1);
                    byte[] hashBytes2 = hashAlg.ComputeHash(fs2);

                    // 比较哈希码
                    return (BitConverter.ToString(hashBytes1) == BitConverter.ToString(hashBytes2));
                }
            }
        }
        #endregion
    
        #region 10.8获取文本文件的行数
        /// <summary>
        /// 10.8获取文本文件的行数
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetLineCount(string filePath)
        {
            //将文本文件的各行读到一个字符串数组中
            string[] rows = File.ReadAllLines(filePath);

            //返回行数
            return rows.Length;
        }
        #endregion
    
        #region 10.9获取指定文件详细属性
    
        /// <summary>
        /// 获取指定文件详细属性
        /// </summary>
        /// <param name="filePath">文件详细路径</param>
        /// <returns></returns>
        public static string GetFileAttibe(string filePath)
        {
            string str = "";
            FileInfo objFI = new FileInfo(filePath);
            str += "详细路径:" + objFI.FullName 
                + "<br>文件名称:" + objFI.Name 
                + "<br>文件长度:" + objFI.Length.ToString()
                + "字节<br>创建时间" + objFI.CreationTime.ToString()
                + "<br>最后访问时间:" + objFI.LastAccessTime.ToString() 
                + "<br>修改时间:" + objFI.LastWriteTime.ToString() 
                + "<br>所在目录:" + objFI.DirectoryName 
                + "<br>扩展名:" + objFI.Extension;
            return str;
        }
        #endregion
      
        #endregion

        #region 11、获取文件夹大小
        /// <summary>
        /// 11.1获取文件夹大小
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        }
        #endregion



    }
    [Serializable]
    public class FileItem
    {

        #region 私有字段
        private string _Name;
        private string _FullName;
        private DateTime _CreationDate;
        private bool _IsFolder;
        private long _Size;
        private DateTime _LastAccessDate;
        private DateTime _LastWriteDate;
        private int _FileCount;
        private int _SubFolderCount;
        #endregion

        #region 公有属性
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        /// <summary>
        /// 文件或目录的完整目录
        /// </summary>
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }

        /// <summary>
        ///  创建时间
        /// </summary>
        public DateTime CreationDate
        {
            get { return _CreationDate; }
            set { _CreationDate = value; }
        }

        public bool IsFolder
        {
            get { return _IsFolder; }
            set { _IsFolder = value; }
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long Size
        {
            get { return _Size; }
            set { _Size = value; }
        }

        /// <summary>
        /// 上次访问时间
        /// </summary>
        public DateTime LastAccessDate
        {
            get { return _LastAccessDate; }
            set { _LastAccessDate = value; }
        }

        /// <summary>
        /// 上次读写时间
        /// </summary>
        public DateTime LastWriteDate
        {
            get { return _LastWriteDate; }
            set { _LastWriteDate = value; }
        }

        /// <summary>
        /// 文件个数
        /// </summary>
        public int FileCount
        {
            get { return _FileCount; }
            set { _FileCount = value; }
        }

        /// <summary>
        /// 目录个数
        /// </summary>
        public int SubFolderCount
        {
            get { return _SubFolderCount; }
            set { _SubFolderCount = value; }
        }
        #endregion
    }
    /// <summary>
    /// 文件的编码类型
    /// 1、获取文件编码 GetEncoding(string filePath)
    /// 2、获取文件编码 GetEncoding(string filePath, Encoding defaultEncoding)
    /// </summary>
    public class FileEncoding
    {
        #region 获取文件的编码类型

        /// <summary>
        /// 获取文件编码
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string filePath)
        {
            return GetEncoding(filePath, Encoding.Default);
        }

        /// <summary>
        /// 获取文件编码
        /// </summary>
        /// <param name="filePath">文件绝对路径</param>
        /// <param name="defaultEncoding">找不到则返回这个默认编码</param>
        /// <returns></returns>
        public static Encoding GetEncoding(string filePath, Encoding defaultEncoding)
        {
            Encoding targetEncoding = defaultEncoding;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4))
            {
                if (fs != null && fs.Length >= 2)
                {
                    long pos = fs.Position;
                    fs.Position = 0;
                    int[] buffer = new int[4];
                    //long x = fs.Seek(0, SeekOrigin.Begin);
                    //fs.Read(buffer,0,4);
                    buffer[0] = fs.ReadByte();
                    buffer[1] = fs.ReadByte();
                    buffer[2] = fs.ReadByte();
                    buffer[3] = fs.ReadByte();

                    fs.Position = pos;

                    if (buffer[0] == 0xFE && buffer[1] == 0xFF)//UnicodeBe
                    {
                        targetEncoding = Encoding.BigEndianUnicode;
                    }
                    if (buffer[0] == 0xFF && buffer[1] == 0xFE)//Unicode
                    {
                        targetEncoding = Encoding.Unicode;
                    }
                    if (buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF)//UTF8
                    {
                        targetEncoding = Encoding.UTF8;
                    }
                }
            }

            return targetEncoding;
        }

        #endregion
    }
    /// <summary>
    /// Stream、byte[] 和 文件之间的转换
    /// 1、将流读取到缓冲区中StreamToBytes(Stream stream)
    /// 2、将 byte[] 转成 Stream；BytesToStream(byte[] bytes)
    /// 3、将 Stream 写入文件 StreamToFile(Stream stream, string fileName)
    /// 4、从文件读取 Stream  FileToStream(string fileName)
    /// 5、将文件读取到缓冲区中 FileToBytes(string filePath)
    /// 6、将文件读取到字符串中 FileToString(string filePath, Encoding encoding)
    /// 7、从嵌入资源中读取文件内容(e.g: xml) ReadFileFromEmbedded(string fileWholeName)
    /// </summary>
    public class FileStreamHelp
    {
        #region Stream、byte[] 和 文件之间的转换

        /// <summary>
        /// 将流读取到缓冲区中
        /// </summary>
        /// <param name="stream">原始流</param>
        public static byte[] StreamToBytes(Stream stream)
        {
            try
            {
                //创建缓冲区
                byte[] buffer = new byte[stream.Length];

                //读取流
                stream.Read(buffer, 0, Convert.ToInt32(stream.Length));

                //返回流
                return buffer;
            }
            catch (IOException ex)
            {
                throw ex;
            }
            finally
            {
                //关闭流
                stream.Close();
            }
        }

        /// <summary>
        /// 将 byte[] 转成 Stream
        /// </summary>
        public static Stream BytesToStream(byte[] bytes)
        {
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// 将 Stream 写入文件
        /// </summary>
        public static void StreamToFile(Stream stream, string fileName)
        {
            // 把 Stream 转换成 byte[]
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);
            // 把 byte[] 写入文件
            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            bw.Write(bytes);
            bw.Close();
            fs.Close();
        }

        /// <summary>
        /// 从文件读取 Stream
        /// </summary>
        public static Stream FileToStream(string fileName)
        {
            // 打开文件
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);
            return stream;
        }

        /// <summary>
        /// 将文件读取到缓冲区中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static byte[] FileToBytes(string filePath)
        {
            //获取文件的大小
            int fileSize = FileHelper.GetFileSize(filePath);

            //创建一个临时缓冲区
            byte[] buffer = new byte[fileSize];

            //创建一个文件流
            FileInfo fi = new FileInfo(filePath);
            FileStream fs = fi.Open(FileMode.Open);

            try
            {
                //将文件流读入缓冲区
                fs.Read(buffer, 0, fileSize);

                return buffer;
            }
            catch (IOException ex)
            {
                throw ex;
            }
            finally
            {
                //关闭文件流
                fs.Close();
            }
        }

        /// <summary>
        /// 将文件读取到字符串中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static string FileToString(string filePath)
        {
            return FileToString(filePath, Encoding.Default);
        }

        /// <summary>
        /// 将文件读取到字符串中
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        /// <param name="encoding">字符编码</param>
        public static string FileToString(string filePath, Encoding encoding)
        {
            try
            {
                //创建流读取器
                using (StreamReader reader = new StreamReader(filePath, encoding))
                {
                    //读取流
                    return reader.ReadToEnd();
                }
            }
            catch (IOException ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 从嵌入资源中读取文件内容(e.g: xml).
        /// </summary>
        /// <param name="fileWholeName">嵌入资源文件名，包括项目的命名空间.</param>
        /// <returns>资源中的文件内容.</returns>
        public static string ReadFileFromEmbedded(string fileWholeName)
        {
            string result = string.Empty;

            using (TextReader reader = new StreamReader(
                Assembly.GetExecutingAssembly().GetManifestResourceStream(fileWholeName)))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }

        #endregion
    }

    /// <summary>
    /// 常用的目录操作辅助类
    /// </summary>
    public class DirectoryUtil
    {
        #region 目录可写与空间计算

        /// <summary>
        ///检查目录是否可写，如果可以，返回True，否则False
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool IsWriteable(string path)
        {
            if (!Directory.Exists(path))
            {
                // if the directory is not exist
                try
                {
                    // if you can create a new directory, it's mean you have write right
                    Directory.CreateDirectory(path);
                }
                catch
                {
                    return false;
                }
            }


            try
            {
                string testFileName = ".test." + Guid.NewGuid().ToString().Substring(0, 5);
                string testFilePath = Path.Combine(path, testFileName);
                File.WriteAllLines(testFilePath, new string[] { "test" });
                File.Delete(testFilePath);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查磁盘是否有足够的可用空间
        /// </summary>
        /// <param name="path"></param>
        /// <param name="requiredSpace"></param>
        /// <returns></returns>
        public static bool IsDiskSpaceEnough(string path, ulong requiredSpace)
        {
            string root = Path.GetPathRoot(path);
            ulong freeSpace = GetFreeSpace(root);

            return requiredSpace <= freeSpace;
        }

        /// <summary>
        /// 获取驱动盘符的可用空间大小
        /// </summary>
        /// <param name="driveName">Direve name</param>
        /// <returns>free space (byte)</returns>
        public static ulong GetFreeSpace(string driveName)
        {
            ulong freefreeBytesAvailable = 0;
            try
            {
                DriveInfo drive = new DriveInfo(driveName);
                freefreeBytesAvailable = (ulong)drive.AvailableFreeSpace;
            }
            catch
            {

            }

            return freefreeBytesAvailable;
        }
        #endregion

        #region 目录操作

        /// <summary>
        /// 取系统目录
        /// </summary>
        /// <returns></returns>
        public static string GetSystemDirectory()
        {
            return System.Environment.SystemDirectory;
        }

        /// <summary>
        /// 取系统的特别目录
        /// </summary>
        /// <param name="folderType"></param>
        /// <returns></returns>
        public static string GetSpeicalFolder(Environment.SpecialFolder folderType)
        {
            return System.Environment.GetFolderPath(folderType);
        }

        /// <summary>
        /// 返回当前系统的临时目录
        /// </summary>
        /// <returns></returns>
        public static string GetTempPath()
        {
            return Path.GetTempPath();
        }

        /// <summary>
        /// 取当前目录
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }

        /// <summary>
        /// 读取目录列表
        /// </summary>
        public static List<FileItem> GetDirectoryItems(string path)
        {
            List<FileItem> list = new List<FileItem>();
            string[] folders = Directory.GetDirectories(path);
            foreach (string s in folders)
            {
                FileItem item = new FileItem();
                DirectoryInfo di = new DirectoryInfo(s);
                item.Name = di.Name;
                item.FullName = di.FullName;
                item.CreationDate = di.CreationTime;
                item.IsFolder = false;
                list.Add(item);
            }
            return list;
        }
   
        /// <summary>
        /// 设当前目录
        /// </summary>
        /// <param name="path"></param>
        public static void SetCurrentDirectory(string path)
        {
            Directory.SetCurrentDirectory(path);
        }

        /// <summary>
        /// 取路径中不充许存在的字符
        /// </summary>
        /// <returns></returns>
        public static char[] GetInvalidPathChars()
        {
            return Path.GetInvalidPathChars();
        }

        /// <summary>
        /// 取系统所有的逻辑驱动器
        /// </summary>
        /// <returns></returns>
        public static DriveInfo[] GetAllDrives()
        {
            return DriveInfo.GetDrives();
        }

        #endregion

    }

}
