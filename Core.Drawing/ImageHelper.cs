using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Core.Drawing
{
    class ImageHelper
    {     
        /// <summary>
        /// 改变原图片的格式，使其成为后缀名为strFormat的图片
        /// path是图片的地址
        /// </summary>
        /// <param name="path">原图片的地址</param>
        /// <param name="strFormat">修改后图片的格式</param>
        /// <param name="isDelSourcefile">是否删除源文件</param>
        public static void ChangeFormat(string path, string strFormat, bool isDelSourcefile)
        {
            if (isAPicFile(path) == false)
            {
                MessageBox.Show("非图片文件，请确定路径");
                return;
            }
            Image img = null;
            bool canGoOn = true;
            try
            {
                img = Image.FromFile(path);
            }
            catch (Exception)
            {
                MessageBox.Show("文件" + path + "未找到，请确认路径是否正确");
                canGoOn = false;
            }
            if (canGoOn)////已经打开成功
            {
                try
                {
                    img.Save(getDirectory(path) + "." + strFormat); //CAN
                }
                catch (Exception)
                {
                    canGoOn = false;
                }
                if (canGoOn)//修改格式成功
                {
                    try
                    {
                        img.Dispose();
                        if (isDelSourcefile)
                        {
                            File.Delete(path);
                        }
                    }
                    catch (Exception efail)
                    {
                        MessageBox.Show("文件" + path + "删除失败,错误原因：" + efail.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 改变图片尺寸的函数，原则上不会有变大的可能性，所以我会将其减小
        /// 而且会保存图片，并且决定时候删除原文件，否则保存时文件名要改动
        /// </summary>
        /// <param name="path">文件的路径</param>
        /// <param name="width">要修改的宽度</param>
        /// <param name="height">要修改的高度</param>
        /// <param name="isDelSourceFile">是否删除原文件</param>
        public static void ChangeSize(string path, int width, int height, bool isDelSourceFile)
        {
            if (isAPicFile(path) == false)
            {
                MessageBox.Show("非图片文件，请确定路径");
                return;
            }
            Image img = null;
            bool canGoOn = true;
            if (width <= 0 || height <= 0)
            {
                MessageBox.Show("尺寸错误，请不要使用非正整数尺寸！");
                return;
            }
            try
            {
                img = Image.FromFile(path);
            }
            catch (Exception)
            {
                MessageBox.Show("文件" + path + "未找到，请确认路径是否正确");
                canGoOn = false;
            }
            if (canGoOn)////已经打开成功
            {
                if (width > img.Width)
                {
                    width = img.Width;
                }
                if (height > img.Height)
                {
                    height = img.Height;
                }
                Bitmap bmp = new Bitmap(width, height);////Empty Bitmap
                Bitmap pic = (Bitmap)img;

                ///
                /// 缩小图片算法：
                /// 1，拿原长度比上要求的长度，得到一个double类型的比值 bi
                /// 2，依次将这个比值 bi 加 要求的长度次，每次得到的值取整，则这个整数就是要求的点！
                /// 3，将这些点填充到bmp变量里面，再将该变量强制转换成image格式，保存...
                ///
                double biOfWidth = ((double)pic.Width) / width;
                double biOfHeight = ((double)pic.Height) / height;
                int y = 0, x;
                double dy = 0, dx;
                for (int i = 0; i < height; i++)
                {
                    dy += biOfHeight;
                    y = (int)dy;
                    x = 0;
                    dx = 0;
                    for (int j = 0; j < width; j++)
                    {
                        dx += biOfWidth;
                        x = (int)dx;
                        try
                        {
                            bmp.SetPixel(j, i, pic.GetPixel(x - 1, y - 1));
                            //MessageBox.Show("" + j + " " + i + " " + x + " " + y);
                        }
                        catch (Exception) { MessageBox.Show("x=" + pic.Width); }
                    }
                }
                // img = (Image)pic;
                if (isDelSourceFile == true)
                {
                    img.Dispose();
                    File.Delete(path);
                    img = (Image)bmp;
                    img.Save(path);
                    MessageBox.Show("saved");
                }
                else
                {
                    img = (Image)bmp;
                    img.Save(getDirectory(path) + "_." + getLast(path));
                }
                pic.Dispose();
                bmp.Dispose();
                img.Dispose();
            }
        }
        /// <summary>
        /// 得到路径包含文件名但不包含后缀名的部分
        /// </summary>
        /// <param name="path">图片文件路径</param>
        /// <returns></returns>
        public static string getDirectory(string path)
        {
            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '.')
                {
                    return path.Substring(0, i);
                }
            }
            return null;
        }
        /// <summary>
        /// 得到文件路径的后缀名
        /// </summary>
        /// <param name="path">图片文件的路径</param>
        /// <returns></returns>
        public static string getLast(string path)
        {
            for (int i = path.Length - 1; i >= 0; i--)
            {
                if (path[i] == '.')
                {
                    return path.Substring(i + 1, path.Length - i - 1);
                }
            }
            return null;
        }
        /// <summary>
        /// 判断是否是否是一个图片文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool isAPicFile(string path)
        {
            string last = getLast(path);
            last = last.Trim();
            if (last.Equals("jpg") || last.Equals("jpeg") || last.Equals("png") || last.Equals("bmp"))
            {
                return true;
            }
            return false;
        }
    }
}
