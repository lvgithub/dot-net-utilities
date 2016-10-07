using System;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;


namespace Core.Zip
{
    /// <summary>
    /// 压缩文本、字节或者文件的压缩辅助类
    /// </summary>
    public class GZipUtil
    {
        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string Compress(string text)
        {
            // convert text to bytes
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            // get a stream
            MemoryStream ms = new MemoryStream();
            // get ready to zip up our stream
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                // compress the data into our buffer
                zip.Write(buffer, 0, buffer.Length);
            }
            // reset our position in compressed stream to the start
            ms.Position = 0;
            // get the compressed data
            byte[] compressed = ms.ToArray();
            ms.Read(compressed, 0, compressed.Length);
            // prepare final data with header that indicates length
            byte[] gzBuffer = new byte[compressed.Length + 4];
            //copy compressed data 4 bytes from start of final header
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            // copy header to first 4 bytes
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            // convert back to string and return
            return Convert.ToBase64String(gzBuffer);
        }

        /// <summary>
        /// 解压字符串
        /// </summary>
        /// <param name="compressedText"></param>
        /// <returns></returns>
        public static string Uncompress(string compressedText)
        {
            // get string as bytes
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            // prepare stream to do uncompression
            MemoryStream ms = new MemoryStream();
            // get the length of compressed data
            int msgLength = BitConverter.ToInt32(gzBuffer, 0);
            // uncompress everything besides the header
            ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
            // prepare final buffer for just uncompressed data
            byte[] buffer = new byte[msgLength];
            // reset our position in stream since we're starting over
            ms.Position = 0;
            // unzip the data through stream
            GZipStream zip = new GZipStream(ms, CompressionMode.Decompress);
            // do the unzip
            zip.Read(buffer, 0, buffer.Length);
            // convert back to string and return
            return Encoding.UTF8.GetString(buffer);
        }

        public static T GZip<T>(Stream stream, CompressionMode mode) where T : Stream
        {
            byte[] writeData = new byte[4096];
            T ms = default(T);
            using (Stream sg = new GZipStream(stream, mode))
            {
                while (true)
                {
                    Array.Clear(writeData, 0, writeData.Length);
                    int size = sg.Read(writeData, 0, writeData.Length);
                    if (size > 0)
                    {
                        ms.Write(writeData, 0, size);
                    }
                    else
                    {
                        break;
                    }
                }
                return ms;
            }
        }

        /// <summary>
        /// 压缩字节
        /// </summary>
        /// <param name="bytData"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] bytData)
        {
            using (MemoryStream stream = GZip<MemoryStream>(new MemoryStream(bytData), CompressionMode.Compress))
            {
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 解压字节
        /// </summary>
        /// <param name="bytData"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] bytData)
        {
            using (MemoryStream stream = GZip<MemoryStream>(new MemoryStream(bytData), CompressionMode.Decompress))
            {
                return stream.ToArray();
            }
        }

        /// <summary>
        /// 压缩Object对象到字节数组
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] ObjectToGZip(object obj)
        {
            byte[] byteTempArray = ObjectToByteArray(obj);
            return GZipUtil.Compress(byteTempArray);
        }

        /// <summary>
        /// 从压缩的字节数组转换到Object对象
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static object GZipToObject(byte[] byteArray)
        {
            byte[] byteTempArray = GZipUtil.Decompress(byteArray);
            return ByteArrayToObject(byteTempArray);
        }

        private static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        private static object ByteArrayToObject(byte[] byteArray)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                return formatter.Deserialize(stream);
            }
        }

        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="sourceFile">源文件</param>
        /// <param name="destinationFile">目标文件</param>
        public static void CompressFile(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException();
            }
            byte[] buffer = null;
            FileStream stream = null;
            FileStream stream2 = null;
            try
            {
                stream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);
                buffer = new byte[stream.Length];
                if (stream.Read(buffer, 0, buffer.Length) != buffer.Length)
                {
                    throw new ApplicationException();
                }
                stream2 = new FileStream(destinationFile, FileMode.OpenOrCreate, FileAccess.Write);
                new GZipStream(stream2, CompressionMode.Compress, true).Write(buffer, 0, buffer.Length);
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (stream2 != null)
                {
                    stream2.Close();
                }
            }
        }

        /// <summary>
        /// 解压文件
        /// </summary>
        /// <param name="sourceFile">源文件</param>
        /// <param name="destinationFile">目标文件</param>
        public static void DecompressFile(string sourceFile, string destinationFile)
        {
            if (!File.Exists(sourceFile))
            {
                throw new FileNotFoundException();
            }
            FileStream stream = null;
            FileStream stream2 = null;
            GZipStream stream3 = null;
            byte[] buffer = null;
            try
            {
                stream = new FileStream(sourceFile, FileMode.Open);
                stream3 = new GZipStream(stream, CompressionMode.Decompress, true);
                buffer = new byte[4];
                int num = ((int)stream.Length) - 4;
                stream.Position = num;
                stream.Read(buffer, 0, 4);
                stream.Position = 0L;
                byte[] buffer2 = new byte[BitConverter.ToInt32(buffer, 0) + 100];
                int offset = 0;
                int count = 0;
                while (true)
                {
                    int num5 = stream3.Read(buffer2, offset, 100);
                    if (num5 == 0)
                    {
                        break;
                    }
                    offset += num5;
                    count += num5;
                }
                stream2 = new FileStream(destinationFile, FileMode.Create);
                stream2.Write(buffer2, 0, count);
                stream2.Flush();
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
                if (stream3 != null)
                {
                    stream3.Close();
                }
                if (stream2 != null)
                {
                    stream2.Close();
                }
            }
        }

     
    }
}
