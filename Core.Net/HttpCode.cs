/*
 * 玄机网C# 基类库 Http请求类
 * 基础功能1:基于HttpWebRequest封装的 同步/异步 (Get/Post)
 * 基础功能2:基于Wininet系统API封装的 同步(Get/Post)
 * 基于以上功能实现的一键请求类,让你摆脱Cookie的困扰,让你解除对多线程的恐惧,. 
 * Coding by 君临
 * 09-27/2014
*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO.Compression;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Reflection;

namespace HttpCodeLib
{
    partial class HttpRequest
    {
        public const int DefaultTimeOutSpan = 30 * 1000;
        public HttpResults result = new HttpResults();
        //HttpWebRequest对象用来发起请求
        public HttpWebRequest request = null;
        //获取影响流的数据对象
        public HttpWebResponse response = null;
        //响应流对象
        public Stream streamResponse;
        public Action<HttpResults> callBack;
        public HttpItems objHttpCodeItem;
        public MemoryStream MemoryStream = new MemoryStream();
        public int m_semaphore = 0;
        //默认的编码
        public Encoding encoding;
    }
    /// <summary>
    /// Http连接操作帮助类 
    /// </summary>
    public class HttpHelpers
    {
        #region 预定义方法或者变更
        //默认的编码
        private Encoding encoding = Encoding.Default;
        //HttpWebRequest对象用来发起请求
        private HttpWebRequest request = null;
        //获取影响流的数据对象
        private HttpWebResponse response = null;
        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="strPostdata">传入的数据Post方式,get方式传NUll或者空字符串都可以</param>
        /// <returns>string类型的响应数据</returns>
        private HttpResults GetHttpRequestData(HttpItems objHttpItems)
        {
            //返回参数
            HttpResults result = new HttpResults();
            try
            {
                #region 得到请求的response
                result.CookieCollection = new CookieCollection();
                response = (HttpWebResponse)request.GetResponse();

                result.Header = response.Headers;
                if (response.Cookies != null)
                {
                    result.CookieCollection = response.Cookies;
                }
                if (response.Headers["set-cookie"] != null)
                {
                    result.Cookie = response.Headers["set-cookie"];
                }
                //处理返回值Container
                result.Container = objHttpItems.Container;
                MemoryStream _stream = new MemoryStream();
                //GZIIP处理
                if (response.ContentEncoding != null && response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                {
                    _stream = GetMemoryStream(new GZipStream(response.GetResponseStream(), CompressionMode.Decompress));
                }
                else
                {
                    _stream = GetMemoryStream(response.GetResponseStream());

                }
                //获取Byte
                byte[] RawResponse = _stream.ToArray();
                //是否返回Byte类型数据
                if (objHttpItems.ResultType == ResultType.Byte)
                {
                    result.ResultByte = RawResponse;
                    return result;
                }
                //无视编码
                if (encoding == null)
                {
                    string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
                    //<meta(.*?)charset([\s]?)=[^>](.*?)>
                    Match meta = Regex.Match(temp, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                    charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                    if (charter.Length > 0)
                    {
                        charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                        encoding = Encoding.GetEncoding(charter);
                    }
                    else
                    {

                        if (response.CharacterSet != null)
                        {
                            if (response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                            {
                                encoding = Encoding.GetEncoding("gbk");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(response.CharacterSet.Trim()))
                                {
                                    encoding = Encoding.UTF8;
                                }
                                else
                                {
                                    encoding = Encoding.GetEncoding(response.CharacterSet);
                                }
                            }
                        }
                    }
                }
                //得到返回的HTML
                try
                {
                    if (RawResponse.Length > 0)
                    {
                        result.Html = encoding.GetString(RawResponse);
                    }
                    else
                    {
                        result.Html = "";
                    }
                    _stream.Close();
                    response.Close();
                }
                catch
                {
                    return null;
                }
                //最后释放流


                #endregion


            }
            catch (WebException ex)
            {
                //这里是在发生异常时返回的错误信息
                result.Html = "String Error";
                response = (HttpWebResponse)ex.Response;
                return result;
            }
            if (objHttpItems.IsToLower)
            {
                result.Html = result.Html.ToLower();
            }
            return result;
        }
        private void AsyncResponseData(IAsyncResult result)
        {
            HttpRequest hrt = result.AsyncState as HttpRequest;
            if (System.Threading.Interlocked.Increment(ref hrt.m_semaphore) != 1)
                return;
            try
            {
                hrt.response = (HttpWebResponse)hrt.request.EndGetResponse(result);
                if (hrt.response.ContentEncoding != null && hrt.response.ContentEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase))
                {
                    hrt.streamResponse = new GZipStream(hrt.response.GetResponseStream(), CompressionMode.Decompress);
                }
                else
                {
                    hrt.streamResponse = hrt.response.GetResponseStream();
                }
                hrt.MemoryStream = GetMemoryStream(hrt.streamResponse);
                AsyncCallBackData(hrt);
            }
            catch (Exception ex)
            {
                hrt.result.Html = ex.Message;
                AsyncCallBackData(hrt);
            }
        }
        /// <summary>
        /// 无视编码
        /// </summary>
        /// <param name="hrt">请求参数</param>
        /// <param name="RawResponse">响应值</param>
        /// <returns></returns>
        HttpRequest GetEncoding(HttpRequest hrt, ref byte[] RawResponse)
        {
            if (hrt.encoding == null)
            {
                string temp = Encoding.Default.GetString(RawResponse, 0, RawResponse.Length);
                Match meta = Regex.Match(temp, "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                string charter = (meta.Groups.Count > 2) ? meta.Groups[2].Value : string.Empty;
                charter = charter.Replace("\"", string.Empty).Replace("'", string.Empty).Replace(";", string.Empty);
                if (charter.Length > 0)
                {
                    charter = charter.ToLower().Replace("iso-8859-1", "gbk");
                    hrt.encoding = Encoding.GetEncoding(charter);
                }
                else
                {

                    if (hrt.response.CharacterSet != null)
                    {
                        if (hrt.response.CharacterSet.ToLower().Trim() == "iso-8859-1")
                        {
                            hrt.encoding = Encoding.GetEncoding("gbk");
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(hrt.response.CharacterSet.Trim()))
                            {
                                hrt.encoding = Encoding.UTF8;
                            }
                            else
                            {
                                hrt.encoding = Encoding.GetEncoding(hrt.response.CharacterSet);
                            }
                        }
                    }
                }
            }
            return hrt;
        }
        /// <summary>
        /// 处理/解析数据方法
        /// </summary>
        /// <param name="hrt"></param>
        void AsyncCallBackData(HttpRequest hrt)
        {
            try
            {
                byte[] RawResponse = hrt.MemoryStream.ToArray();
                hrt.result.CookieCollection = hrt.response.Cookies;
                //无视编码
                hrt = GetEncoding(hrt, ref RawResponse);
                //是否返回Byte类型数据  
                if (hrt.objHttpCodeItem.ResultType == ResultType.Byte)
                {
                    hrt.result.ResultByte = RawResponse;
                }
                //得到返回的HTML
                try
                {
                    hrt.result.Html = Encoding.UTF8.GetString(RawResponse);
                    hrt.callBack.Invoke(hrt.result);
                }
                catch
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                hrt.result.Html = ex.Message;
                hrt.callBack.Invoke(hrt.result);
            }
        }
        /// <summary>
        /// 根据相传入的数据，得到相应页面数据
        /// </summary>
        /// <param name="strPostdata">传入的数据Post方式,get方式传NUll或者空字符串都可以</param>
        /// <returns>string类型的响应数据</returns>
        private void AsyncGetHttpRequestData(HttpItems objItems, Action<HttpResults> callBack)
        {
            HttpRequest hrt = new HttpRequest();
            SetRequest(objItems);
            hrt.objHttpCodeItem = objItems;
            hrt.request = request;
            hrt.callBack = callBack;
            try
            {

                IAsyncResult m_ar = hrt.request.BeginGetResponse(AsyncResponseData, hrt);
                System.Threading.ThreadPool.RegisterWaitForSingleObject(m_ar.AsyncWaitHandle,
                    TimeoutCallback, hrt, HttpRequest.DefaultTimeOutSpan, true);
            }
            catch (Exception ex)
            {
                hrt.result.Html = "TimeOut";
            }
        }
        /// <summary>
        /// 超时回调
        /// </summary>
        /// <param name="state"></param>
        /// <param name="timedOut"></param>
        void TimeoutCallback(object state, bool timedOut)
        {
            HttpRequest pa = state as HttpRequest;
            if (timedOut)
                if (System.Threading.Interlocked.Increment(ref pa.m_semaphore) == 1)
                    pa.result.Html = "TimeOut";
        }
        /// <summary>
        /// 4.0以下.net版本取数据使用
        /// </summary>
        /// <param name="streamResponse">流</param>
        private MemoryStream GetMemoryStream(Stream streamResponse)
        {
            MemoryStream _stream = new MemoryStream();
            int Length = 256;
            Byte[] buffer = new Byte[Length];
            int bytesRead = streamResponse.Read(buffer, 0, Length);
            // write the required bytes  
            while (bytesRead > 0)
            {
                _stream.Write(buffer, 0, bytesRead);
                bytesRead = streamResponse.Read(buffer, 0, Length);
            }
            return _stream;
        }

        /// <summary>
        /// 为请求准备参数
        /// </summary>
        ///<param name="objHttpItems">参数列表</param>
        /// <param name="_Encoding">读取数据时的编码方式</param>
        private void SetRequest(HttpItems objHttpItems)
        {

            // 验证证书
            SetCer(objHttpItems);
            //设置Header参数
            if (objHttpItems.Header != null)
            {
                try
                {
                    request.Headers = objHttpItems.Header;
                }
                catch
                {
                    return;
                }
            }
            if (objHttpItems.IsAjax)
            {
                request.Headers.Add("x-requested-with: XMLHttpRequest");
            }
            // 设置代理
            SetProxy(objHttpItems);
            //请求方式Get或者Post
            request.Method = objHttpItems.Method;
            request.Timeout = objHttpItems.Timeout;
            request.ReadWriteTimeout = objHttpItems.ReadWriteTimeout;
            //Accept
            request.Accept = objHttpItems.Accept;
            //ContentType返回类型
            request.ContentType = objHttpItems.ContentType;
            //UserAgent客户端的访问类型，包括浏览器版本和操作系统信息
            request.UserAgent = objHttpItems.UserAgent;
            // 编码
            SetEncoding(objHttpItems);
            //设置Cookie
            SetCookie(objHttpItems);
            //来源地址
            request.Referer = objHttpItems.Referer;
            //是否执行跳转功能
            request.AllowAutoRedirect = objHttpItems.Allowautoredirect;
            //设置Post数据
            SetPostData(objHttpItems);
            //设置最大连接
            if (objHttpItems.Connectionlimit > 0)
            {
                request.ServicePoint.ConnectionLimit = objHttpItems.Connectionlimit;
            }
        }
        /// <summary>
        /// 设置证书
        /// </summary>
        /// <param name="objHttpItems"></param>
        private void SetCer(HttpItems objHttpItems)
        {
            if (!string.IsNullOrEmpty(objHttpItems.CerPath))
            {
                //这一句一定要写在创建连接的前面。使用回调的方法进行证书验证。
                ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
                //初始化对像，并设置请求的URL地址
                request = (HttpWebRequest)WebRequest.Create(GetUrl(objHttpItems.URL));
                //创建证书文件
                X509Certificate objx509 = new X509Certificate(objHttpItems.CerPath);
                //添加到请求里
                request.ClientCertificates.Add(objx509);
            }
            else
            {
                //初始化对像，并设置请求的URL地址
                try
                {
                    request = (HttpWebRequest)WebRequest.Create(GetUrl(objHttpItems.URL));
                }
                catch
                {
                    return;
                }
            }
        }
        /// <summary>
        /// 设置编码
        /// </summary>
        /// <param name="objHttpItems">Http参数</param>
        private void SetEncoding(HttpItems objHttpItems)
        {
            if (string.IsNullOrEmpty(objHttpItems.Encoding) || objHttpItems.Encoding.ToLower().Trim() == "null")
            {
                //读取数据时的编码方式
                encoding = null;
            }
            else
            {
                //读取数据时的编码方式
                encoding = System.Text.Encoding.GetEncoding(objHttpItems.Encoding);
            }
        }
        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="objHttpItems">Http参数</param>
        private void SetCookie(HttpItems objHttpItems)
        {
            //获取当前的cookie

            if (!string.IsNullOrEmpty(objHttpItems.Cookie))
            {
                //Cookie
                request.Headers[HttpRequestHeader.Cookie] = objHttpItems.Cookie;
            }
            //设置Cookie

            if (objHttpItems.CookieCollection != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(objHttpItems.CookieCollection);
            }
            if (objHttpItems.Container != null)
            {
                request.CookieContainer = objHttpItems.Container;
            }
        }

        /// <summary>
        /// 设置Post数据
        /// </summary>
        /// <param name="objHttpItems">Http参数</param>
        private void SetPostData(HttpItems objHttpItems)
        {
            //验证在得到结果时是否有传入数据
            if (request.Method.Trim().ToLower().Contains("post"))
            {
                //写入Byte类型
                if (objHttpItems.PostDataType == PostDataType.Byte)
                {
                    //验证在得到结果时是否有传入数据
                    if (objHttpItems.PostdataByte != null && objHttpItems.PostdataByte.Length > 0)
                    {
                        request.ContentLength = objHttpItems.PostdataByte.Length;
                        request.GetRequestStream().Write(objHttpItems.PostdataByte, 0, objHttpItems.PostdataByte.Length);
                    }
                }//写入文件
                else if (objHttpItems.PostDataType == PostDataType.FilePath)
                {
                    StreamReader r = new StreamReader(objHttpItems.Postdata, encoding);
                    byte[] buffer = Encoding.Default.GetBytes(r.ReadToEnd());
                    r.Close();
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                else
                {
                    //验证在得到结果时是否有传入数据
                    if (!string.IsNullOrEmpty(objHttpItems.Postdata))
                    {
                        byte[] buffer = Encoding.Default.GetBytes(objHttpItems.Postdata);
                        request.ContentLength = buffer.Length;
                        request.GetRequestStream().Write(buffer, 0, buffer.Length);
                    }
                }
            }
        }
        /// <summary>
        /// 设置代理
        /// </summary>
        /// <param name="objHttpItems">参数对象</param>
        private void SetProxy(HttpItems objHttpItems)
        {
            if (string.IsNullOrEmpty(objHttpItems.ProxyUserName) && string.IsNullOrEmpty(objHttpItems.ProxyPwd) && string.IsNullOrEmpty(objHttpItems.ProxyIp))
            {
                //不需要设置
            }
            else
            {
                //设置代理服务器
                WebProxy myProxy = new WebProxy(objHttpItems.ProxyIp, false);
                //建议连接
                myProxy.Credentials = new NetworkCredential(objHttpItems.ProxyUserName, objHttpItems.ProxyPwd);
                //给当前请求对象
                request.Proxy = myProxy;
                //设置安全凭证
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
        }
        /// <summary>
        /// 回调验证证书问题
        /// </summary>
        /// <param name="sender">流对象</param>
        /// <param name="certificate">证书</param>
        /// <param name="chain">X509Chain</param>
        /// <param name="errors">SslPolicyErrors</param>
        /// <returns>bool</returns>
        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            // 总是接受    
            return true;
        }
        #endregion
        #region 普通类型
        /// <summary>    
        /// 传入一个正确或不正确的URl，返回正确的URL
        /// </summary>    
        /// <param name="URL">url</param>   
        /// <returns>
        /// </returns>
        public string GetUrl(string URL)
        {
            if (!(URL.Contains("http://") || URL.Contains("https://")))
            {
                URL = "http://" + URL;
            }
            return URL;
        }
        ///<summary>
        ///采用https协议访问网络,根据传入的URl地址，得到响应的数据字符串。
        ///</summary>
        ///<param name="objHttpItems">参数列表</param>
        ///<returns>String类型的数据</returns>
        public HttpResults GetHtml(HttpItems objHttpItems)
        {
            //准备参数
            SetRequest(objHttpItems);
            //调用专门读取数据的类
            return GetHttpRequestData(objHttpItems);
        }
        ///<summary>
        ///采用异步方式访问网络,根据传入的URl地址，得到响应的数据字符串。
        ///</summary>
        ///<param name="objHttpItems">参数列表</param>
        ///<returns>String类型的数据</returns>
        public void AsyncGetHtml(HttpItems objHttpItems, Action<HttpResults> callBack)
        {
            //调用专门读取数据的类
            AsyncGetHttpRequestData(objHttpItems, callBack);
        }
        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <param name="objHttpItems">参数列表</param>
        /// <returns>Img</returns>
        public Image GetImg(HttpResults hr)
        {

            return byteArrayToImage(hr.ResultByte);

        }
        /// <summary>
        /// 字节数组生成图片
        /// </summary>
        /// <param name="Bytes">字节数组</param>
        /// <returns>图片</returns>
        private Image byteArrayToImage(byte[] Bytes)
        {
            using (MemoryStream ms = new MemoryStream(Bytes))
            {
                Image outputImg = Image.FromStream(ms);
                return outputImg;
            }
        }
        #endregion
    }
    /// <summary>
    /// Http请求参考类 
    /// </summary>
    public class HttpItems
    {
        string _URL;
        /// <summary>
        /// 请求URL必须填写
        /// </summary>
        public string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }
        string _Method = "GET";
        /// <summary>
        /// 请求方式默认为GET方式
        /// </summary>
        public string Method
        {
            get { return _Method; }
            set { _Method = value; }
        }
        int _Timeout = 3000;
        /// <summary>
        /// 默认请求超时时间
        /// </summary>
        public int Timeout
        {
            get { return _Timeout; }
            set { _Timeout = value; }
        }
        int _ReadWriteTimeout = 30000;
        /// <summary>
        /// 默认写入Post数据超时间
        /// </summary>
        public int ReadWriteTimeout
        {
            get { return _ReadWriteTimeout; }
            set { _ReadWriteTimeout = value; }
        }
        string _Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";
        /// <summary>
        /// 请求标头值 默认为image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*
        /// </summary>
        public string Accept
        {
            get { return _Accept; }
            set { _Accept = value; }
        }
        string _ContentType = "application/x-www-form-urlencoded";
        /// <summary>
        /// 请求返回类型默认 application/x-www-form-urlencoded
        /// </summary>
        public string ContentType
        {
            get { return _ContentType; }
            set { _ContentType = value; }
        }
        string _UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0";
        /// <summary>
        /// 客户端访问信息默认Mozilla/5.0 (Windows NT 6.1; WOW64; rv:17.0) Gecko/20100101 Firefox/17.0
        /// </summary>
        public string UserAgent
        {
            get { return _UserAgent; }
            set { _UserAgent = value; }
        }
        string _Encoding = string.Empty;
        /// <summary>
        /// 返回数据编码默认为NUll,可以自动识别
        /// </summary>
        public string Encoding
        {
            get { return _Encoding; }
            set { _Encoding = value; }
        }
        private PostDataType _PostDataType = PostDataType.String;
        /// <summary>
        /// Post的数据类型
        /// </summary>
        public PostDataType PostDataType
        {
            get { return _PostDataType; }
            set { _PostDataType = value; }
        }
        string _Postdata;
        /// <summary>
        /// Post请求时要发送的字符串Post数据
        /// </summary>
        public string Postdata
        {
            get { return _Postdata; }
            set { _Postdata = value; }
        }
        private byte[] _PostdataByte = null;
        /// <summary>
        /// Post请求时要发送的Byte类型的Post数据
        /// </summary>
        public byte[] PostdataByte
        {
            get { return _PostdataByte; }
            set { _PostdataByte = value; }
        }
        CookieCollection cookiecollection = null;
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return cookiecollection; }
            set { cookiecollection = value; }
        }
        private CookieContainer _Container = null;
        /// <summary>
        /// 自动处理cookie
        /// </summary>
        public CookieContainer Container
        {
            get { return _Container; }
            set { _Container = value; }
        }



        string _Cookie = string.Empty;
        /// <summary>
        /// 请求时的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _Cookie; }
            set { _Cookie = value; }
        }
        string _Referer = string.Empty;
        /// <summary>
        /// 来源地址，上次访问地址
        /// </summary>
        public string Referer
        {
            get { return _Referer; }
            set { _Referer = value; }
        }
        string _CerPath = string.Empty;
        /// <summary>
        /// 证书绝对路径
        /// </summary>
        public string CerPath
        {
            get { return _CerPath; }
            set { _CerPath = value; }
        }
        private Boolean isToLower = false;
        /// <summary>
        /// 是否设置为全文小写
        /// </summary>
        public Boolean IsToLower
        {
            get { return isToLower; }
            set { isToLower = value; }
        }
        private Boolean isAjax = false;
        /// <summary>
        /// 是否增加异步请求头
        /// </summary>
        public Boolean IsAjax
        {
            get { return isAjax; }
            set { isAjax = value; }
        }

        private Boolean allowautoredirect = true;
        /// <summary>
        /// 支持跳转页面，查询结果将是跳转后的页面
        /// </summary>
        public Boolean Allowautoredirect
        {
            get { return allowautoredirect; }
            set { allowautoredirect = value; }
        }
        private int connectionlimit = 1024;
        /// <summary>
        /// 最大连接数
        /// </summary>
        public int Connectionlimit
        {
            get { return connectionlimit; }
            set { connectionlimit = value; }
        }
        private string proxyusername = string.Empty;
        /// <summary>
        /// 代理Proxy 服务器用户名
        /// </summary>
        public string ProxyUserName
        {
            get { return proxyusername; }
            set { proxyusername = value; }
        }
        private string proxypwd = string.Empty;
        /// <summary>
        /// 代理 服务器密码
        /// </summary>
        public string ProxyPwd
        {
            get { return proxypwd; }
            set { proxypwd = value; }
        }
        private string proxyip = string.Empty;
        /// <summary>
        /// 代理 服务IP
        /// </summary>
        public string ProxyIp
        {
            get { return proxyip; }
            set { proxyip = value; }
        }
        private ResultType resulttype = ResultType.String;
        /// <summary>
        /// 设置返回类型String和Byte
        /// </summary>
        public ResultType ResultType
        {
            get { return resulttype; }
            set { resulttype = value; }
        }
        private WebHeaderCollection header = new WebHeaderCollection();
        //header对象
        public WebHeaderCollection Header
        {
            get { return header; }
            set { header = value; }
        }
    }
    /// <summary>
    /// Http返回参数类
    /// </summary>
    public class HttpResults
    {
        CookieContainer _Container;
        /// <summary>
        /// 自动处理Cookie集合对象
        /// </summary>
        public CookieContainer Container
        {
            get { return _Container; }
            set { _Container = value; }
        }
        string _Cookie = string.Empty;
        /// <summary>
        /// Http请求返回的Cookie
        /// </summary>
        public string Cookie
        {
            get { return _Cookie; }
            set { _Cookie = value; }
        }
        CookieCollection cookiecollection = null;
        /// <summary>
        /// Cookie对象集合
        /// </summary>
        public CookieCollection CookieCollection
        {
            get { return cookiecollection; }
            set { cookiecollection = value; }
        }
        private string html = string.Empty;
        /// <summary>
        /// 返回的String类型数据 只有ResultType.String时才返回数据，其它情况为空
        /// </summary>
        public string Html
        {
            get { return html; }
            set { html = value; }
        }
        private byte[] resultbyte = null;
        /// <summary>
        /// 返回的Byte数组 只有ResultType.Byte时才返回数据，其它情况为空
        /// </summary>
        public byte[] ResultByte
        {
            get { return resultbyte; }
            set { resultbyte = value; }
        }
        private WebHeaderCollection header = new WebHeaderCollection();
        //header对象
        public WebHeaderCollection Header
        {
            get { return header; }
            set { header = value; }
        }
    }

    /// <summary>
    /// 返回类型
    /// </summary>
    public enum ResultType
    {
        String,//表示只返回字符串
        Byte//表示返回字符串和字节流
    }

    /// <summary>
    /// Post的数据格式默认为string
    /// </summary>
    public enum PostDataType
    {
        String,//字符串
        Byte,//字符串和字节流
        FilePath//表示传入的是文件
    }

    /// <summary>
    /// WinInet的方式请求数据
    /// </summary>
    public class Wininet
    {
        #region WininetAPI
        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        private static extern int InternetOpen(string strAppName, int ulAccessType, string strProxy, string strProxyBypass, int ulFlags);
        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        private static extern int InternetConnect(int ulSession, string strServer, int ulPort, string strUser, string strPassword, int ulService, int ulFlags, int ulContext);
        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        private static extern bool InternetCloseHandle(int ulSession);
        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        private static extern bool HttpAddRequestHeaders(int hRequest, string szHeasers, uint headersLen, uint modifiers);
        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        private static extern int HttpOpenRequest(int hConnect, string szVerb, string szURI, string szHttpVersion, string szReferer, string accetpType, long dwflags, int dwcontext);
        [DllImport("wininet.dll")]
        private static extern bool HttpSendRequestA(int hRequest, string szHeaders, int headersLen, string options, int optionsLen);
        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        private static extern bool InternetReadFile(int hRequest, byte[] pByte, int size, out int revSize);
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetGetCookieEx(string pchURL, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);
        #endregion

        #region 重载方法
        /// <summary>
        /// WinInet 方式GET
        /// </summary>
        /// <param name="Url">地址</param>
        /// <returns></returns>
        public string GetData(string Url)
        {
            using (MemoryStream ms = GetHtml(Url, ""))
            {
                if (ms != null)
                {
                    //无视编码
                    Match meta = Regex.Match(Encoding.Default.GetString(ms.ToArray()), "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                    string c = (meta.Groups.Count > 1) ? meta.Groups[2].Value.ToUpper().Trim() : string.Empty;
                    if (c.Length > 2)
                    {
                        if (c.IndexOf("UTF-8") != -1)
                        {
                            return Encoding.GetEncoding("UTF-8").GetString(ms.ToArray());
                        }
                    }
                    return Encoding.GetEncoding("GBK").GetString(ms.ToArray());
                }
                else
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// POST
        /// </summary>
        /// <param name="Url">地址</param>
        /// <param name="postData">提交数据</param>
        /// <returns></returns>
        public string PostData(string Url, string postData)
        {
            using (MemoryStream ms = GetHtml(Url, postData))
            {
                //无视编码
                Match meta = Regex.Match(Encoding.Default.GetString(ms.ToArray()), "<meta([^<]*)charset=([^<]*)[\"']", RegexOptions.IgnoreCase);
                string c = (meta.Groups.Count > 1) ? meta.Groups[2].Value.ToUpper().Trim() : string.Empty;
                if (c.Length > 2)
                {
                    if (c.IndexOf("UTF-8") != -1)
                    {
                        return Encoding.GetEncoding("UTF-8").GetString(ms.ToArray());
                    }
                }
                return Encoding.GetEncoding("GBK").GetString(ms.ToArray());
            }
        }
        /// <summary>
        /// GET（UTF-8）模式
        /// </summary>
        /// <param name="Url">地址</param>
        /// <returns></returns>
        public string GetUtf8(string Url)
        {
            using (MemoryStream ms = GetHtml(Url, ""))
            {
                return Encoding.GetEncoding("UTF-8").GetString(ms.ToArray());
            }
        }
        /// <summary>
        /// POST（UTF-8）
        /// </summary>
        /// <param name="Url">地址</param>
        /// <param name="postData">提交数据</param>
        /// <returns></returns>
        public string PostUtf8(string Url, string postData)
        {
            using (MemoryStream ms = GetHtml(Url, postData))
            {
                return Encoding.GetEncoding("UTF-8").GetString(ms.ToArray());
            }
        }
        /// <summary>
        /// 获取网页图片(Image)
        /// </summary>
        /// <param name="Url">图片地址</param>
        /// <returns></returns>
        public Image GetImage(string Url)
        {
            using (MemoryStream ms = GetHtml(Url, ""))
            {
                Image img = Image.FromStream(ms);
                return img;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 请求数据
        /// </summary>
        /// <param name="Url">请求地址</param>
        /// <param name="Postdata">提交的数据</param>
        /// <param name="Header">请求头</param>
        /// <returns></returns>
        private MemoryStream GetHtml(string Url, string Postdata, StringBuilder Header = null)
        {
            try
            {
                //声明部分变量
                Uri uri = new Uri(Url);
                string Method = "GET";
                if (Postdata != "")
                {
                    Method = "POST";
                }
                string UserAgent = "Mozilla/4.0 (compatible; MSIE 9.0; Windows NT 6.1; 125LA; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
                int hSession = InternetOpen(UserAgent, 1, "", "", 0);//会话句柄
                if (hSession == 0)
                {
                    InternetCloseHandle(hSession);
                    return null;//Internet句柄获取失败则返回
                }
                int hConnect = InternetConnect(hSession, uri.Host, uri.Port, "", "", 3, 0, 0);//连接句柄
                if (hConnect == 0)
                {
                    InternetCloseHandle(hConnect);
                    InternetCloseHandle(hSession);
                    return null;//Internet连接句柄获取失败则返回
                }
                //请求标记
                long gettype = -2147483632;
                if (Url.Substring(0, 5) == "https")
                {
                    gettype = -2139095024;
                }
                else
                {
                    gettype = -2147467248;
                }
                //取HTTP请求句柄
                int hRequest = HttpOpenRequest(hConnect, Method, uri.PathAndQuery, "HTTP/1.1", "", "", gettype, 0);//请求句柄
                if (hRequest == 0)
                {
                    InternetCloseHandle(hRequest);
                    InternetCloseHandle(hConnect);
                    InternetCloseHandle(hSession);
                    return null;//HTTP请求句柄获取失败则返回
                }
                //添加HTTP头
                StringBuilder sb = new StringBuilder();
                if (Header == null)
                {
                    sb.Append("Accept:text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8\r\n");
                    sb.Append("Content-Type:application/x-www-form-urlencoded\r\n");
                    sb.Append("Accept-Language:zh-cn\r\n");
                    sb.Append("Referer:" + Url);
                }
                else
                {
                    sb = Header;
                }
                //获取返回数据
                if (string.Equals(Method, "GET", StringComparison.OrdinalIgnoreCase))
                {
                    HttpSendRequestA(hRequest, sb.ToString(), sb.Length, "", 0);
                }
                else
                {
                    HttpSendRequestA(hRequest, sb.ToString(), sb.Length, Postdata, Postdata.Length);
                }
                //处理返回数据
                int revSize = 0;//计次
                byte[] bytes = new byte[1024];
                MemoryStream ms = new MemoryStream();
                while (true)
                {
                    bool readResult = InternetReadFile(hRequest, bytes, 1024, out revSize);
                    if (readResult && revSize > 0)
                    {
                        ms.Write(bytes, 0, revSize);
                    }
                    else
                    {
                        break;
                    }
                }
                InternetCloseHandle(hRequest);
                InternetCloseHandle(hConnect);
                InternetCloseHandle(hSession);
                return ms;
            }
            catch (Exception)
            {
                return null;
            }
        }
        #endregion

        #region 获取webbrowser的cookies
        /// <summary>
        /// 取出cookies
        /// </summary>
        /// <param name="url">完整的链接格式</param>
        /// <returns></returns>
        public string GetCookies(string url)
        {
            uint datasize = 256;
            StringBuilder cookieData = new StringBuilder((int)datasize);
            if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                    return null;

                cookieData = new StringBuilder((int)datasize);
                if (!InternetGetCookieEx(url, null, cookieData, ref datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return cookieData.ToString() + ";";
        }
        #endregion

        #region String与CookieContainer互转
        /// <summary>
        /// 遍历CookieContainer
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public List<Cookie> GetAllCookies(CookieContainer cc)
        {
            List<Cookie> lstCookies = new List<Cookie>();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });

            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }
            return lstCookies;

        }
        /// <summary>
        /// 将String转CookieContainer
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public CookieContainer StringToCookie(string url, string cookie)
        {
            string[] arrCookie = cookie.Split(';');
            CookieContainer cookie_container = new CookieContainer();    //加载Cookie
            foreach (string sCookie in arrCookie)
            {
                if (sCookie.IndexOf("expires") > 0)
                    continue;
                cookie_container.SetCookies(new Uri(url), sCookie);
            }
            return cookie_container;
        }
        /// <summary>
        /// 将CookieContainer转换为string类型
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public string CookieToString(CookieContainer cc)
        {
            System.Collections.Generic.List<Cookie> lstCookies = new System.Collections.Generic.List<Cookie>();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            StringBuilder sb = new StringBuilder();
            foreach (object pathList in table.Values)
            {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                    | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies)
                    {
                        sb.Append(c.Name).Append("=").Append(c.Value).Append(";");
                    }
            }
            return sb.ToString();
        }
        #endregion
    }

    /// <summary>
    /// 玄机网一键HTTP类库
    /// </summary>
    public class XJHTTP
    {
        HttpItems item = new HttpItems();
        HttpHelpers http = new HttpHelpers();
        Wininet wnet = new Wininet();
        HttpResults hr;
        /// <summary>
        /// 普通请求.直接返回标准结果
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <returns>返回结果</returns>
        public HttpResults GetHtml(string url)
        {
            item.URL = url;
            return http.GetHtml(item);
        }
        /// <summary>
        /// 普通请求.直接返回标准结果
        /// </summary>
        /// <param name="url">请求的URL</param>
        /// <param name="cc">当前Cookie</param>
        /// <returns></returns>
        public HttpResults GetHtml(string url, CookieContainer cc)
        {
            item.URL = url;
            item.Container = cc;
            return http.GetHtml(item);
        }
        /// <summary>
        ///  普通请求.直接返回标准结果
        /// </summary>
        /// <param name="picurl">图片请求地址</param>
        /// <param name="referer">上一次请求地址</param>
        /// <param name="cc">当前Cookie</param>
        /// <returns></returns>
        public HttpResults GetImage(string picurl, string referer, CookieContainer cc)
        {
            item.URL = picurl;
            item.Referer = referer;
            item.Container = cc;
            item.ResultType = ResultType.Byte;
            return http.GetHtml(item);
        }
        /// <summary>
        /// 普通请求.直接返回Image格式图像
        /// </summary>
        /// <param name="picurl">图片请求地址</param>
        /// <param name="referer">上一次请求地址</param>
        /// <param name="cc">当前Cookie</param>
        /// <returns></returns>
        public Image GetImageByImage(string picurl, string referer, CookieContainer cc)
        {
            item.URL = picurl;
            item.Referer = referer;
            item.Container = cc;
            item.ResultType = ResultType.Byte;
            return http.GetImg(http.GetHtml(item));
        }
        /// <summary>
        /// 普通请求.直接返回标准结果
        /// </summary>
        /// <param name="posturl">post地址</param>
        /// <param name="referer">上一次请求地址</param>
        /// <param name="postdata">请求数据</param>
        /// <param name="IsAjax">是否需要异步标识</param>
        /// <param name="cc">当前Cookie</param>
        /// <returns></returns>
        public HttpResults PostHtml(string posturl, string referer, string postdata, bool IsAjax, CookieContainer cc)
        {
            item.URL = posturl;
            item.Referer = referer;
            item.Method = "Post";
            item.IsAjax = IsAjax;
            item.ResultType = ResultType.String;
            item.Postdata = postdata;
            item.Container = cc;
            return http.GetHtml(item);
        }
        /// <summary>
        /// 获取当前请求所有Cookie
        /// </summary>
        /// <param name="items"></param>
        /// <returns>Cookie集合</returns>
        public List<Cookie> GetAllCookieByHttpItems(HttpItems items)
        {
            return wnet.GetAllCookies(items.Container);
        }
        /// <summary>
        /// 获取CookieContainer 中的所有对象
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public List<Cookie> GetAllCookie(CookieContainer cc)
        {
            return wnet.GetAllCookies(cc);
        }
        /// <summary>
        /// 将 CookieContainer 对象转换为字符串类型
        /// </summary>
        /// <param name="cc"></param>
        /// <returns></returns>
        public string CookieTostring(CookieContainer cc)
        {
            return wnet.CookieToString(cc);
        }
        /// <summary>
        /// 将文字Cookie转换为CookieContainer 对象
        /// </summary>
        /// <param name="url"></param>
        /// <param name="cookie"></param>
        /// <returns></returns>
        public CookieContainer StringToCookie(string url, string cookie)
        {
            return wnet.StringToCookie(url, cookie);
        }
        /// <summary>
        /// 从Wininet中获取Cookie对象
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string stringToCookieByWininet(string url)
        {
            return wnet.GetCookies(url);
        }

        /// <summary>
        /// 异步POST请求 通过回调返回结果
        /// </summary>
        /// <param name="objHttpItems">请求项</param>
        /// <param name="callBack">回调地址</param>
        public void AsyncPostHtml(HttpItems objHttpItems, Action<HttpResults> callBack)
        {
            http.AsyncGetHtml(objHttpItems, callBack);
        }
        /// <summary>
        /// 异步GET请求 通过回调返回结果
        /// </summary>
        /// <param name="objHttpItems">请求项</param>
        /// <param name="callBack">回调地址</param>
        public void AsyncGetHtml(HttpItems objHttpItems, Action<HttpResults> callBack)
        {
            http.AsyncGetHtml(objHttpItems, callBack);
        }
        /// <summary>
        /// WinInet方式GET请求  直接返回网页内容
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public string GetHtmlByWininet(string url)
        {
            return wnet.GetData(url);
        }
        /// <summary>
        /// WinInet方式GET请求(UTF8)  直接返回网页内容
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <returns></returns>
        public string GetHtmlByWininetUTF8(string url)
        {
            return wnet.GetUtf8(url);
        }
        /// <summary>
        /// WinInet方式POST请求  直接返回网页内容
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="postdata">提交内容</param>
        /// <returns></returns>
        public string POSTHtmlByWininet(string url, string postdata)
        {
            return wnet.PostData(url, postdata);
        }
        /// <summary>
        /// WinInet方式POST请求  直接返回网页内容
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <param name="postdata">提交内容</param>
        /// <returns></returns>
        public string POSTHtmlByWininetUTF8(string url, string postdata)
        {
            return wnet.GetUtf8(url);
        }
        /// <summary>
        /// WinInet方式请求 图片  直接返回Image
        /// </summary>
        /// <param name="url">提交地址</param>
        /// <returns></returns>
        public Image GetImageByWininet(string url)
        {
            return wnet.GetImage(url);
        }

        /// <summary>
        /// 获取JS时间戳 13位
        /// </summary>
        /// <returns></returns>
        public string GetTime()
        {
            Type obj = Type.GetTypeFromProgID("ScriptControl");
            if (obj == null) return null;
            object ScriptControl = Activator.CreateInstance(obj);
            obj.InvokeMember("Language", BindingFlags.SetProperty, null, ScriptControl, new object[] { "JScript" });
            string js = "function time(){return new Date().getTime()}";
            obj.InvokeMember("AddCode", BindingFlags.InvokeMethod, null, ScriptControl, new object[] { js });
            return obj.InvokeMember("Eval", BindingFlags.InvokeMethod, null, ScriptControl, new object[] { "time()" }).ToString();
        }
        /// <summary>  
        /// 获取时间戳 C# 10位 
        /// </summary>  
        /// <returns></returns>  
        public string GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }
    }

}
