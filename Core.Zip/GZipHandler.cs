using System;
using System.Web;
using System.IO;
using System.Text;
using System.IO.Compression;

namespace Core.Zip
{
    public class GZipHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpContext Context = HttpContext.Current;
            HttpRequest Request = Context.Request;
            HttpResponse Response = Context.Response;

            string AcceptEncoding = Request.Headers["Accept-Encoding"];
            // *** Start by checking whether GZip is supported by client

            bool UseGZip = false;

            if (!string.IsNullOrEmpty(AcceptEncoding) &&

                AcceptEncoding.ToLower().IndexOf("gzip") > -1)

                UseGZip = true;

            // *** Create a cachekey and check whether it exists

            string CacheKey = Request.QueryString.ToString() + UseGZip.ToString();

            byte[] Output = Context.Cache[CacheKey] as byte[];

            if (Output != null)
            {
                // *** Yup - read cache and send to client
                SendOutput(Output, UseGZip);
                return;
            }

            // *** Load the script file 

            string Script = "";


            StreamReader sr = new StreamReader(context.Server.MapPath(Request["src"]));

            Script = sr.ReadToEnd();


            // *** Now we're ready to create out output

            // *** Don't GZip unless at least 8k

            if (UseGZip && Script.Length > 6000)

                Output = GZipMemory(Script);

            else
            {

                Output = Encoding.ASCII.GetBytes(Script);

                UseGZip = false;

            }
            // *** Add into the cache with one day

            Context.Cache.Add(CacheKey, Output, null, DateTime.UtcNow.AddDays(1), TimeSpan.Zero, System.Web.Caching.CacheItemPriority.High, null);
            // *** Write out to Response object with appropriate Client Cache settings

            this.SendOutput(Output, UseGZip);
        }
        /// <summary>
        /// Sends the output to the client using appropriate cache settings.
        /// Content should be already encoded and ready to be sent as binary.
        /// </summary>
        /// <param name="Output"></param>
        /// <param name="UseGZip"></param>
        private void SendOutput(byte[] Output, bool UseGZip)
        {

            HttpResponse Response = HttpContext.Current.Response;
            Response.ContentType = "application/x-javascript";
            if (UseGZip)
                Response.AppendHeader("Content-Encoding", "gzip");
            //if (!HttpContext.Current.IsDebuggingEnabled)
            // {

            Response.ExpiresAbsolute = DateTime.UtcNow.AddYears(1);
            Response.Cache.SetLastModified(DateTime.UtcNow);
            Response.Cache.SetCacheability(HttpCacheability.Public);
            // }

            Response.BinaryWrite(Output);
            Response.End();

        }


        /// <summary>
        /// Takes a binary input buffer and GZip encodes the input
        /// </summary>
        /// <param name="Buffer"></param>
        /// <returns></returns>

        public static byte[] GZipMemory(byte[] Buffer)
        {

            MemoryStream ms = new MemoryStream();
            GZipStream GZip = new GZipStream(ms, CompressionMode.Compress);
            GZip.Write(Buffer, 0, Buffer.Length);
            GZip.Close();
            byte[] Result = ms.ToArray();
            ms.Close();
            return Result;

        }
       
        public static byte[] GZipMemory(string Input)
        {
            return GZipMemory(Encoding.ASCII.GetBytes(Input));

        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}