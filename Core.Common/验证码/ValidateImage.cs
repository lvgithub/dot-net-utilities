/**********************************************
 * 类作用：   验证码类
 * 建立人：   abaal
 * 建立时间： 2008-09-03 
 * Copyright (C) 2007-2008 abaal
 * All rights reserved
 * http://blog.csdn.net/abaal888
 ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Security.Cryptography;

namespace Core.Common
{
    /// <summary>
    /// 验证码类
    /// </summary>
    public class ValidateImage
    {
        /// <summary>
        /// 要显示的文字
        /// </summary>
        public string Text
        {
            get { return this.text; }
        }
        /// <summary>
        /// 图片
        /// </summary>
        public Bitmap Image
        {
            get { return this.image; }
        }
        /// <summary>
        /// 宽度
        /// </summary>
        public int Width
        {
            get { return this.width; }
        }
        /// <summary>
        /// 高度
        /// </summary>
        public int Height
        {
            get { return this.height; }
        }

        private string text;
        private int width;
        private int height;
        private Bitmap image;

        private static byte[] randb = new byte[4];
        private static Random rand = new Random();

        public ValidateImage()
        {
            text = rand.Next(1000, 9999).ToString();
            System.Web.HttpContext.Current.Session["CheckCode"] = text;
            this.width = (int)Math.Ceiling(text.Length * 16.5);
            this.height = 30;

            GenerateImage();
            System.Web.HttpContext.Current.Response.ContentType = "image/pjpeg";
            Image.Save(System.Web.HttpContext.Current.Response.OutputStream, ImageFormat.Jpeg);

        }


        public static bool Validate(string input)
        {
            return System.Web.HttpContext.Current.Session["CheckCode"] != null && System.Web.HttpContext.Current.Session["CheckCode"].ToString().Equals(input);
        }

        ~ValidateImage()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                this.image.Dispose();
        }
        private FontFamily[] fonts = {
										 new FontFamily("Times New Roman"),
										 new FontFamily("Georgia"),
										 new FontFamily("Arial"),
										 new FontFamily("Comic Sans MS")
									 };

        public int Next(int max)
        {
            return rand.Next(max);
        }

        /// <summary>
        /// 生成验证码图片

        /// </summary>
        private void GenerateImage()
        {
            Bitmap bitmap = new Bitmap(this.width, this.height, PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bitmap);
            Rectangle rect = new Rectangle(0, 0, this.width, this.height);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            g.Clear(Color.White);

            //int emSize = Next(3) + 15;//(int)((this.width - 20) * 2 / text.Length);
            //int emSize = (int)((this.width - 20) * 2 / text.Length);

            int emSize = 12;
            FontFamily family = fonts[Next(fonts.Length - 1)];
            Font font = new Font(family, emSize, FontStyle.Bold);

            SizeF measured = new SizeF(0, 0);
            SizeF workingSize = new SizeF(this.width, this.height);
            while (emSize > 2 && (measured = g.MeasureString(text, font)).Width > workingSize.Width || measured.Height > workingSize.Height)
            {
                font.Dispose();
                font = new Font(family, emSize -= 2);
            }

            SolidBrush drawBrush = new SolidBrush(Color.FromArgb(Next(100), Next(80), Next(80)));
            for (int x = 0; x < 1; x++)
            {
                Pen linePen = new Pen(Color.FromArgb(Next(150), Next(100), Next(100)), 1);
                g.DrawLine(linePen, new PointF(0.0F + Next(2), 0.0F + Next(this.height)), new PointF(0.0F + Next(this.width), 0.0F + Next(this.height - 10)));
            }

            for (int x = 0; x < this.text.Length; x++)
            {
                drawBrush.Color = Color.FromArgb(Next(150) + 20, Next(150) + 20, Next(150) + 20);
                PointF drawPoint = new PointF(0.0F + Next(5) + x * 15, 2.0F + Next(4));
                g.DrawString(this.text[x].ToString(), font, drawBrush, drawPoint);
            }

            double distort = rand.Next(5, 10) * (Next(10) == 1 ? 1 : -1);

            using (Bitmap copy = (Bitmap)bitmap.Clone())
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int newX = (int)(x + (distort * Math.Sin(Math.PI * y / 84.0)));
                        int newY = (int)(y + (distort * Math.Cos(Math.PI * x / 54.0)));
                        if (newX < 0 || newX >= width) newX = 0;
                        if (newY < 0 || newY >= height) newY = 0;
                        bitmap.SetPixel(x, y, copy.GetPixel(newX, newY));
                    }
                }
            }


            //g.DrawRectangle(new Pen(Color.Silver), 0, 0, bitmap.Width - 1, bitmap.Height - 1);

            font.Dispose();
            drawBrush.Dispose();
            g.Dispose();

            this.image = bitmap;
        }
    }
}
