using System;
using System.Security.Cryptography;  
using System.Text;
using System.IO;

namespace Common
{
    public class Encrypt
    {
        public Encrypt()
        {
        }

        //密钥
        private static byte[] arrDESKey = new byte[] {42, 16, 93, 156, 78, 4, 218, 32};
        private static byte[] arrDESIV = new byte[] {55, 103, 246, 79, 36, 99, 167, 3};

        /// <summary>
        /// 加密。
        /// </summary>
        /// <param name="m_Need_Encode_String"></param>
        /// <returns></returns>
        public static string Encode(string m_Need_Encode_String)
        {
            if (m_Need_Encode_String == null)
            {
                throw new Exception("Error: \n源字符串为空！！");
            }
            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            MemoryStream objMemoryStream = new MemoryStream();
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateEncryptor(arrDESKey, arrDESIV),
                CryptoStreamMode.Write);
            StreamWriter objStreamWriter = new StreamWriter(objCryptoStream);
            objStreamWriter.Write(m_Need_Encode_String);
            objStreamWriter.Flush();
            objCryptoStream.FlushFinalBlock();
            objMemoryStream.Flush();
            return Convert.ToBase64String(objMemoryStream.GetBuffer(), 0, (int) objMemoryStream.Length);
        }

        /// <summary>
        /// 解密。
        /// </summary>
        /// <param name="m_Need_Encode_String"></param>
        /// <returns></returns>
        public static string Decode(string m_Need_Encode_String)
        {
            if (m_Need_Encode_String == null)
            {
                throw new Exception("Error: \n源字符串为空！！");
            }
            DESCryptoServiceProvider objDES = new DESCryptoServiceProvider();
            byte[] arrInput = Convert.FromBase64String(m_Need_Encode_String);
            MemoryStream objMemoryStream = new MemoryStream(arrInput);
            CryptoStream objCryptoStream = new CryptoStream(objMemoryStream, objDES.CreateDecryptor(arrDESKey, arrDESIV),
                CryptoStreamMode.Read);
            StreamReader objStreamReader = new StreamReader(objCryptoStream);
            return objStreamReader.ReadToEnd();
        }

        /// <summary>
        /// md5
        /// </summary>
        /// <param name="encypStr"></param>
        /// <returns></returns>
        public static string Md5(string encypStr)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();
            byte[] inputBye;
            byte[] outputBye;
            inputBye = System.Text.Encoding.ASCII.GetBytes(encypStr);
            outputBye = m5.ComputeHash(inputBye);
            retStr = Convert.ToBase64String(outputBye);
            return (retStr);
        }

        

        

        #region TripleDES加密

        /// <summary>
        /// TripleDES加密
        /// </summary>
        public static string TripleDESEncrypting(string strSource)
        {
            try
            {
                byte[] bytIn = Encoding.Default.GetBytes(strSource);
                byte[] key =
                {
                    42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119,
                    35, 184, 197
                }; //定义密钥
                byte[] IV = {55, 103, 246, 79, 36, 99, 167, 3}; //定义偏移量
                TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
                TripleDES.IV = IV;
                TripleDES.Key = key;
                ICryptoTransform encrypto = TripleDES.CreateEncryptor();
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(bytIn, 0, bytIn.Length);
                cs.FlushFinalBlock();
                byte[] bytOut = ms.ToArray();
                return System.Convert.ToBase64String(bytOut);
            }
            catch (Exception ex)
            {
                throw new Exception("加密时候出现错误!错误提示:\n" + ex.Message);
            }
        }

        #endregion

        #region TripleDES解密

        /// <summary>
        /// TripleDES解密
        /// </summary>
        public static string TripleDESDecrypting(string Source)
        {
            try
            {
                byte[] bytIn = System.Convert.FromBase64String(Source);
                byte[] key =
                {
                    42, 16, 93, 156, 78, 4, 218, 32, 15, 167, 44, 80, 26, 20, 155, 112, 2, 94, 11, 204, 119,
                    35, 184, 197
                }; //定义密钥
                byte[] IV = {55, 103, 246, 79, 36, 99, 167, 3}; //定义偏移量
                TripleDESCryptoServiceProvider TripleDES = new TripleDESCryptoServiceProvider();
                TripleDES.IV = IV;
                TripleDES.Key = key;
                ICryptoTransform encrypto = TripleDES.CreateDecryptor();
                System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);
                StreamReader strd = new StreamReader(cs, Encoding.Default);
                return strd.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("解密时候出现错误!错误提示:\n" + ex.Message);
            }
        }

        #endregion
    }
}