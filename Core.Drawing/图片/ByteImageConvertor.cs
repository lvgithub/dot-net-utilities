using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Core.Drawing
{
    /// <summary>
    /// This class provides the utility method to the convertion between byte array and image.
    /// </summary>
    public sealed class ByteImageConvertor
    {
        private ByteImageConvertor()
        {
        }

        /// <summary>
        /// Converts the PO's byte array to VO's image.
        /// </summary>
        /// <param name="bytes">The byte array in PO.</param>
        /// <returns>The Image object.</returns>
        public static Image ByteToImage(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }

            Image image = null;
            using (MemoryStream stream = new MemoryStream())
            {
                stream.Write(bytes, 0, bytes.Length);
                image = Image.FromStream(stream);
            }
            return image;
        }

        /// <summary>
        /// Converts the VO's image member to PO's bytes.
        /// </summary>
        /// <param name="image">The Image object in VO.</param>
        /// <returns>The byte array.</returns>
        public static byte[] ImageToByte(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            byte[] bytes = null;
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, ImageFormat.Jpeg);
                bytes = stream.ToArray();
            }
            return bytes;
        }
    }
}