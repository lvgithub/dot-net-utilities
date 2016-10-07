
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Core.Web
{
    public class UBB
    {
        public static string UbbToHtml(string ubb)
        {
            if (ubb == null) return null;
            //ubb = HttpUtility.HtmlEncode(ubb);
            Regex rex = new Regex(@"(\[Quote=([\s\S]*)\])(.[^\[]*)(\[\/Quote\])", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
            ubb = rex.Replace(ubb, "<fieldset><legend>$2</legend>$3</fieldset>");


            rex = new Regex(@"(\[b\])(.[^\[]*)(\[\/b\])", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
            ubb = rex.Replace(ubb, "<strong>$2</strong>");

            rex = new Regex(@"(\[i\])(.[^\[]*)(\[\/i\])", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
            ubb = rex.Replace(ubb, "<i>$2</i>");

            rex = new Regex(@"(\[u\])(.[^\[]*)(\[\/u\])", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
            ubb = rex.Replace(ubb, "<u>$2</u>");

            rex = new Regex(@"(\[img\])(.[^\[]*)(\[\/img\])", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
            ubb = rex.Replace(ubb, "<a href=\"$2\" target=\"_blank\"><img src=\"$2\" width=\"16\" height=\"16\" alt=\"查看原图\" border=\"0\" /></a>");

            rex = new Regex(@"(\[url\])(.[^\[]*)(\[\/url\])", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
            ubb = rex.Replace(ubb, "<a href=\"$2\" target=\"_blank\">$2</a>");
            rex = new Regex(@"(\[url\=)(.*?)\](.[^\[]*)(\[\/url\])", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
            ubb = rex.Replace(ubb, "<a href=\"$2\" target=\"_blank\">$3</a>");

            rex = new Regex(@"(\[email\])(.[^\[]*)(\[\/email\])", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
            ubb = rex.Replace(ubb, "<a href=\"mailto:$2\">$2</a>");

            rex = new Regex(@"(\[quote\])(.[^\[]*)(\[\/quote\])", RegexOptions.IgnoreCase | RegexOptions.IgnoreCase);
            ubb = rex.Replace(ubb, "<fieldset><legend>引用内容</legend>$2</fieldset>");

            //心情图片（待实现）
            string[] img = { "[:)]", "[#_#]", "[8*)]", "[:D]", "[:-)]", "[:P]", "[B_)]", "[B_I]", "[^_*]", "[:$]", "[:|]", "[:(]", "[:.(]", "[:_(]", "[):(]", "[:V]", "[*_*]", "[:^]", "[:?]", "[:!]", "[=:|]", "[:%]", "[:O]", "[:X]", "[|-)]", "[:Z]", "[:9]", "[:T]", "[:-*]", "[*_/]", "[:#|]", "[:69]", "[//shuang]", "[//qiang]", "[//ku]", "[//zan]", "[//heart]", "[//break]", "[//F]", "[//W]", "[//mail]", "[//strong]", "[//weak]", "[//share]", "[//phone]", "[//mobile]", "[//kiss]", "[//V]", "[//sun]", "[//moon]", "[//star]", "[(!)]", "[//TV]", "[//clock]", "[//gift]", "[//cash]", "[//coffee]", "[//rice]", "[//watermelon]", "[//tomato]", "[//pill]", "[//pig]", "[//football]", "[//shit]" };
            for (int i = 0; i < img.Length; i++)
            {
                ubb = ubb.Replace(img[i], "<img src=\"/images/em/" + i.ToString() + ".gif\" />");
            }
            //if (ubb.Length > 980)
            //{
            //    ubb = ubb.Substring(0, 980);
            //    if (ubb.LastIndexOf(">") > 950)
            //    {
            //        ubb = ubb.Substring(0, ubb.LastIndexOf(">") + 1);
            //    }
            //}
            return ubb;
        }
    }
}
