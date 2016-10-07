using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core.Common;
using Common;

namespace DotNetUtilities
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(Tools.AreObjectsEqual("123456","654321"));
          
           Response.Write("<br />");
          // Response.Write(Tools.GetPhysicalPath());

           Response.Write("<br />");
           //CNDateHelper cnd=new CNDateHelper(DateTime.Now);
           ////Response.Write(cnd.GanZhiDateString);
           ////Response.Write("<br />");
           ////Response.Write(cnd.GanZhiDayString);
           ////Response.Write("<br />");
           ////Response.Write(cnd.GanZhiMonthString);
           ////Response.Write("<br />");
           ////Response.Write(cnd.GanZhiYearString);
           //Response.Write("<br />");
           //Response.Write(cnd.Animal);
           //Response.Write("<br />"); 
           // Response.Write(cnd.AnimalString);
           //Response.Write("<br />");
            // ZipHelper.EnZip("D:\\学习语言10条.txt" ,"学习语言10条.txt","E:\\学习语言10条.rar");
            //ZipHelper.DeZip("E:\\学习语言10条.rar","F:\\");
            //SharpZip.PackFiles("E:\\学习语言10条.rar", "D:\\文章");
            //ClassZip.Zip("D:\\文章", "E:\\学习语言16条.rar",6);
            //ClassZip.UnZip("E:\\学习语言16条.rar", "F:\\文章1");

            // GZipUtil.CompressFile("D:\\学习语言10条.txt", "D:\\学习语言10条.rar");
            //  Response.Write(GZipUtil.Uncompress (GZipUtil.Compress("123456789")));
            //Response.Write(TypeParse.StrToInt("1", 2));
            //Response.Write("<br />");
            //Response.Write(ConvertHelper.ToInt32("1", 2));
        }
    }
}