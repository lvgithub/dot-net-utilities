using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using Core.Common;

namespace DotNetUtilities.Commons
{
    public partial class WebString : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {

            //Response.Write(Encrypt.Encode("123456789"));
            //Response.Write("<br />");
            //Response.Write(Encrypt.Md5("123456789"));
            //Response.Write("<br />");
            //Response.Write(MySecurity.EncodeBase64("123456789"));

            //Response.Write("<br />");
            //Response.Write(MySecurity.SEncryptString("123456789"));

            //Response.Write("<br />");
            //Response.Write(Encrypt.TripleDESEncrypting("123456789"));
            //Response.Write("<br />");
            //Response.Write(DEncrypt.Encrypt("123456789"));
            //Response.Write("<br />");
            //Response.Write(DESEncrypt.Encrypt("123456789"));
         
            Response.Write("<br />");



            //把字符串转 按照 逗号, 分割 换为数据
            //  StringHelper.GetStrArray("qwe,asd,zxc,rty,fgh,vbn");
           //删除最后结尾的一个逗号
            //Response.Write(StringHelper.DelLastComma("非官方月黑风高，防御条约,"));

           //删除最后结尾的指定字符后的字符
           // Response.Write(StringHelper.DelLastChar("qweasdasdwqw", "asd"));

            //string str = "123456789搜索";
            //Response.Write(str.Substring(str.ToUpper().IndexOf("4")));
            
          string[] str  =StringHelper.SplitString("qwe,asd,zxc,rty,fgh,vbn,aer", ",");
           
           // StringHelper.SplitString("qwe,asd,zxc,rty,fgh,vbn", ",",2);

          DateTime dt1 = DateTime.Now;
          DateTime dt2 = DateTime.Now;
            dt1 = Convert.ToDateTime("2014-9-1");
            dt2 = Convert.ToDateTime("2014-9-3");

           // Response.Write(StringHelper.CharCount("qwe,asd,zxc,rty,fgh,vbn,aer", ','));
            
            // Response.Write( StringHelper.GetStringLength("123123撒旦"));

           // Response.Write(StringHtmlHelp.HtmlEncode("1234567890"));
         //   Response.Write("<br />");
         // //  Response.Write(StringHtmlHelp.HtmlDecode("123456789&nbsp0\r\n"));
         //   Response.Write("<br />");
         ////   Response.Write(StringHtmlHelp.HtmlEncode1("123456789&nbsp0\r\n"));
         //   Response.Write("<br />");
         // //  Response.Write(StringHtmlHelp.HtmlDecode1("123456789&nbsp0\r\n"));
         //   Response.Write("<br />");
         //  // Response.Write(StringHelper.UrlEncode("Default.aspx?asd=1&asdd=2"));
         //   Response.Write("<br />");
         // //  Response.Write(StringHelper.UrlDecode("Default.aspx%3fasd%3d1%26asdd%3d2"));
         //   Response.Write("<br />");
           // Response.Write(StringHelper.GetStringCount("qwe,asd,zxc,rty,fgh,vbn,aer", "a"));
            //Response.Write(StringHelper.GetLetter("4阿萨德对方是个回家后asddfg13213123asd干活",9,true));
          
          
            //Response.Write("<br />");
            //Response.Write(PinYin.GetFirstLetter("丁雪峰"));
            //Response.Write("<br />");
            //Response.Write(PinYin.GetCodstring("丁雪峰"));


           // Response.Write(BarCodeToHTML.get39("123456789", 10, 5));
           // Response.Write("<br />");
          //  Response.Write(BarCodeToHTML.getEAN13("123456789", 10, 5));

          
            //Response.Write(RandomHelper.GenerateCheckCode(5));
            //Response.Write("<br />");
            //Response.Write(RandomHelper.MakeRandomString(5));
            //Response.Write("<br />");
            //Response.Write(RandomHelper.MakeRandomString("abcdefg1234567890", 5));
            //Response.Write("<br />");
            //Response.Write(RandomHelper.RandomNum());
            //Response.Write("<br />");
            //Response.Write(RandomHelper.RandomNum(5));
          //  Response.Write("<br />");
            

        }
      
        protected void Button1_Click(object sender, EventArgs e)
        {
           //截取指定长度字符串
           this.Label1.Text=StringHelper.Cut("asd洒洒的asd武器飞哥asdf", 6);
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
           
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
           
        }

      


     

      

     
    }
}