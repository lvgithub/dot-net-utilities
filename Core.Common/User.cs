/**********************************************
 * 类作用：   用户实用类
 * 建立人：   abaal
 * 建立时间： 2008-09-03 
 * Copyright (C) 2007-2008 abaal
 * All rights reserved
 * http://blog.csdn.net/abaal888
 ***********************************************/

using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;

namespace Svnhost.Common
{
    /// <summary>
    /// 用户实用类，自定义窗体身份验证时可以使用。
    /// </summary>
    public sealed class UserUtil
    {
        /// <summary>
        /// 用户登录方法
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="roles">用户角色</param>
        /// <param name="isPersistent">是否持久cookie</param>
        public static void Login(string username, string roles, bool isPersistent)
        {
            DateTime dt = isPersistent ? DateTime.Now.AddMinutes(99999) : DateTime.Now.AddMinutes(60);
            FormsAuthenticationTicket ticket =
                new FormsAuthenticationTicket(
                1, // 票据版本号
                username, // 票据持有者
                DateTime.Now, //分配票据的时间
                dt, // 失效时间
                isPersistent, // 需要用户的 cookie 
                roles, // 用户数据，这里其实就是用户的角色
              FormsAuthentication.FormsCookiePath);//cookie有效路径

            //使用机器码machine key加密cookie，为了安全传送
            string hash = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash); //加密之后的cookie

            //将cookie的失效时间设置为和票据tikets的失效时间一致 
            HttpCookie u_cookie = new HttpCookie("username", username);
            if (ticket.IsPersistent)
            {
                u_cookie.Expires = ticket.Expiration;
                cookie.Expires = ticket.Expiration;
            }

            //添加cookie到页面请求响应中
            HttpContext.Current.Response.Cookies.Add(cookie);
            HttpContext.Current.Response.Cookies.Add(u_cookie);
        }

        /// <summary>
        /// 用户退出方法
        /// </summary>
        public static void Logout()
        {
            HttpCookie cookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];

            if (cookie == null)
            {
                cookie = new HttpCookie(FormsAuthentication.FormsCookieName);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
            cookie.Expires = DateTime.Now.AddYears(-10);

            HttpCookie u_cookie = new HttpCookie("username", string.Empty);
            u_cookie.Expires = DateTime.Now.AddYears(-10);
            HttpContext.Current.Response.Cookies.Add(u_cookie);
        }
    }
}
