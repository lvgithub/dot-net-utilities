using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Common
{
    public class EmailHelper
    {
        #region  生成邮件验证码
        /// <summary>
        /// 生成邮件验证码
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static string CreateAuthStr(int len)
        {

            int number;
            StringBuilder checkCode = new StringBuilder();
            Random random = new Random();
            for (int i = 0; i < len; i++)
            {
                number = random.Next();

                if (number % 2 == 0)
                {
                    checkCode.Append((char)('0' + (char)(number % 10)));
                }
                else
                {
                    checkCode.Append((char)('A' + (char)(number % 26)));
                }
            }
            return checkCode.ToString();

        }
        #endregion

    }
}
