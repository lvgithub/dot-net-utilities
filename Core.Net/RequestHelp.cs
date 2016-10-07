using System;
using System.Web;



public class RequestHelp
{
    /// <summary>
    /// 判断当前页面是否接收到了Post请求
    /// </summary>
    /// <returns>是否接收到了Post请求</returns>
    public static bool IsPost()
    {
        return HttpContext.Current.Request.HttpMethod.Equals("POST");
    }
    /// <summary>
    /// 判断当前页面是否接收到了Get请求
    /// </summary>
    /// <returns>是否接收到了Get请求</returns>
    public static bool IsGet()
    {
        return HttpContext.Current.Request.HttpMethod.Equals("GET");
    }

    /// <summary>
    /// 返回指定的服务器变量信息
    /// </summary>
    /// <param name="strName">服务器变量名</param>
    /// <returns>服务器变量信息</returns>
    public static string GetServerString(string strName)
    {
        //
        if (HttpContext.Current.Request.ServerVariables[strName] == null)
        {
            return "";
        }
        return HttpContext.Current.Request.ServerVariables[strName].ToString();
    }

    /// <summary>
    /// 返回上一个页面的地址
    /// </summary>
    /// <returns>上一个页面的地址</returns>
    public static string GetUrlReferrer()
    {
        string retVal = null;

        try
        {
            retVal = HttpContext.Current.Request.UrlReferrer.ToString();
        }
        catch { }

        if (retVal == null)
            return "";

        return retVal;

    }

    /// <summary>
    /// 得到当前完整主机头
    /// </summary>
    /// <returns></returns>
    public static string GetCurrentFullHost()
    {
        HttpRequest request = System.Web.HttpContext.Current.Request;
        if (!request.Url.IsDefaultPort)
        {
            return string.Format("{0}:{1}", request.Url.Host, request.Url.Port.ToString());
        }
        return request.Url.Host;
    }

    /// <summary>
    /// 得到主机头
    /// </summary>
    /// <returns></returns>
    public static string GetHost()
    {
        return HttpContext.Current.Request.Url.Host;
    }


    /// <summary>
    /// 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
    /// </summary>
    /// <returns>原始 URL</returns>
    public static string GetRawUrl()
    {
        return HttpContext.Current.Request.RawUrl;
    }

    /// <summary>
    /// 判断当前访问是否来自浏览器软件
    /// </summary>
    /// <returns>当前访问是否来自浏览器软件</returns>
    public static bool IsBrowserGet()
    {
        string[] BrowserName = { "ie", "opera", "netscape", "mozilla", "konqueror", "firefox" };
        string curBrowser = HttpContext.Current.Request.Browser.Type.ToLower();
        for (int i = 0; i < BrowserName.Length; i++)
        {
            if (curBrowser.IndexOf(BrowserName[i]) >= 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 返回当前页面是否是跨站提交
    /// </summary>
    /// <returns>当前页面是否是跨站提交</returns>
    public static bool IsCrossSitePost()
    {

        // 如果不是提交则为true
        if (!RequestHelp.IsPost())
        {
            return true;
        }
        return IsCrossSitePost(RequestHelp.GetUrlReferrer(), RequestHelp.GetHost());
    }

    /// <summary>
    /// 判断是否是跨站提交
    /// </summary>
    /// <param name="urlReferrer">上个页面地址</param>
    /// <param name="host">论坛url</param>
    /// <returns></returns>
    public static bool IsCrossSitePost(string urlReferrer, string host)
    {
        if (urlReferrer.Length < 7)
        {
            return true;
        }
        Uri u = new Uri(urlReferrer);
        return u.Host != host;
    }

    /// <summary>
    /// 判断是否来自搜索引擎链接
    /// </summary>
    /// <returns>是否来自搜索引擎链接</returns>
    public static bool IsSearchEnginesGet()
    {
        if (HttpContext.Current.Request.UrlReferrer == null)
        {
            return false;
        }
        string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
        string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
        for (int i = 0; i < SearchEngine.Length; i++)
        {
            if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获得当前完整Url地址
    /// </summary>
    /// <returns>当前完整Url地址</returns>
    public static string GetUrl()
    {
        return HttpContext.Current.Request.Url.ToString();
    }


    /// <summary>
    /// 获得指定Url参数的值
    /// </summary>
    /// <param name="strName">Url参数</param>
    /// <returns>Url参数的值</returns>
    public static string GetQueryString(string strName)
    {

        if (HttpContext.Current.Request.QueryString[strName] == null)
        {
            return String.Empty;
        }
        return HttpContext.Current.Request.QueryString[strName];

    }
    /// <summary>
    /// 获得当前页面的名称
    /// </summary>
    /// <returns>当前页面的名称</returns>
    public static string GetPageName()
    {
        string[] urlArr = HttpContext.Current.Request.Url.AbsolutePath.Split('/');
        return urlArr[urlArr.Length - 1].ToLower();
    }

    /// <summary>
    /// 返回表单或Url参数的总个数
    /// </summary>
    /// <returns></returns>
    public static int GetParamCount()
    {
        return HttpContext.Current.Request.Form.Count + HttpContext.Current.Request.QueryString.Count;
    }


    /// <summary>
    /// 获得指定表单参数的值
    /// </summary>
    /// <param name="strName">表单参数</param>
    /// <returns>表单参数的值</returns>
    public static string GetFormString(string strName)
    {
        if (HttpContext.Current.Request.Form[strName] == null)
        {
            return "";
        }
        return HttpContext.Current.Request.Form[strName];
    }

    /// <summary>
    /// 获得Url或表单参数的值, 先判断Url参数是否为空字符串, 如为True则返回表单参数的值
    /// </summary>
    /// <param name="strName">参数</param>
    /// <returns>Url或表单参数的值</returns>
    public static string GetString(string strName)
    {
        if ("".Equals(GetQueryString(strName)))
        {
            return GetFormString(strName);
        }
        else
        {
            return GetQueryString(strName);
        }
    }



}

