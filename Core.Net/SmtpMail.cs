using System;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;
using System.Collections.Generic;

namespace Core.Net
{
    /// <summary>
    /// Smtp配置
    /// </summary>
    public class SmtpConfig
    {
        /// <summary>
        /// SMTP在web.config的默认配置
        /// </summary>
        public SmtpConfig()
        {
            setWebConfigBindding();
            ContentEncoding = Encoding.Default;
        }

        private void setWebConfigBindding()
        {
            try
            {
                MailSettingsSectionGroup sectionGroup = (MailSettingsSectionGroup)WebConfigurationManager.OpenWebConfiguration("~/").GetSectionGroup("system.net/mailSettings");
                if (sectionGroup == null)
                {
                    SmtpServer = "localhost";
                    Port = 25;
                    SSLConnect = false;
                }
                else
                {

                    if (sectionGroup.Smtp.Network.Host != "")
                    {
                        SmtpServer = sectionGroup.Smtp.Network.Host;
                    }
                    Port = sectionGroup.Smtp.Network.Port;
                    UserName = sectionGroup.Smtp.Network.UserName;
                    Password = sectionGroup.Smtp.Network.Password;

                    if (sectionGroup.Smtp.Network.DefaultCredentials == true)
                    {
                        Credentials = null;
                    }
                    else
                    {
                        Credentials = new NetworkCredential(UserName, Password);
                    }
                }
            }
            catch (Exception)
            {

            }

        }

        private string smtpServerField;
        /// <summary>
        /// 发送邮件服务器
        /// </summary>
        public string SmtpServer 
        {
            get { return smtpServerField; }
            set { smtpServerField = value; }
        }
        
        private int portField = 25;
        /// <summary>
        /// 服务器连接端口，默认为25。
        /// </summary>
        public int Port 
        {
        	get { return portField; }
        	set { portField = value; }
        }

        private string userNameField;
        /// <summary>
        /// 连接用户名
        /// </summary>
        public string UserName 
        { 
        	get { return userNameField; }
            set { userNameField = value; }
        }

        private string passwordField;
        /// <summary>
        /// 连接密码
        /// </summary>
        public string Password 
        { 
        	get { return passwordField; }
            set { passwordField = value; } 
        }
        
        private bool sslConnectField = false;
        /// <summary>
        /// 是否是安全套接字连接，默认为否。
        /// </summary>
        public bool SSLConnect 
        { 
        	get { return sslConnectField; }
            set { sslConnectField = value; } 
        }
        
        private Encoding contentEncodingField;
        /// <summary>
        /// 邮件内容编码
        /// </summary>
        public Encoding ContentEncoding 
        { 
        	get { return contentEncodingField; }
            set { contentEncodingField = value; } 
        }
        
        private NetworkCredential credentialsField;
        /// <summary>
        /// 访问凭据
        /// </summary>
        public NetworkCredential Credentials 
        { 
        	get { return credentialsField; }
            set { credentialsField = value; } 
        }
    }
    
    /// <summary>
    /// SMTP邮件发送
    /// </summary>
    public class SmtpMail
    {
        /// <summary>
        /// 发送HTML格式邮件(UTF8)
        /// </summary>
        public static string MailTo(SmtpConfig config,
            MailAddress AddrFrom, MailAddress AddrTo,
            MailAddressCollection cc, MailAddressCollection bCC,
            string Subject, string BodyContent, bool isHtml, List<Attachment> attC)
        {
            MailMessage msg = new MailMessage(AddrFrom, AddrTo);

            #region 抄送
            if (cc != null && cc.Count > 0)
            {
                foreach (MailAddress cAddr in cc)
                {
                    msg.CC.Add(cAddr);
                }
            }
            #endregion

            #region 密送
            if (bCC != null && bCC.Count > 0)
            {
                foreach (MailAddress cAddr in bCC)
                {
                    msg.Bcc.Add(cAddr);
                }
            }
            #endregion

            #region 附件列表
            if (attC != null && attC.Count > 0)
            {
                foreach (Attachment item in attC)
                {
                    msg.Attachments.Add(item);
                }
            }
            #endregion

            msg.IsBodyHtml = isHtml;
            msg.Priority = MailPriority.High;

            msg.Subject = Subject;
            msg.SubjectEncoding = config.ContentEncoding;
            msg.BodyEncoding = config.ContentEncoding;
            msg.Body = BodyContent;

            SmtpClient client = new SmtpClient(config.SmtpServer, config.Port);
            if (config.Credentials != null)
            {
                client.Credentials = config.Credentials;
            }
            client.EnableSsl = config.SSLConnect;

            try
            {
                client.Send(msg);
                return "0";
            }
            catch (Exception exp)
            {
                return exp.Message;
            }

        }


        /// <summary>
        /// 发送HTML格式邮件
        /// </summary>
        /// <param name="config">SMTP配置</param>
        /// <param name="AddrFrom">发件人邮箱</param>
        /// <param name="AddrTo">收件人邮箱</param>
        /// <param name="Subject">主题</param>
        /// <param name="BodyContent">内容</param>
        /// <returns></returns>
        public static string MailTo(SmtpConfig config,
            MailAddress AddrFrom, MailAddress AddrTo,
            string Subject, string BodyContent)
        {
            return MailTo(config, AddrFrom, AddrTo, null, null, Subject, BodyContent, true, null);
        }

    }
}
