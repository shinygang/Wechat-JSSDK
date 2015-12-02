using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;

namespace WechatSDK
{
    public class JSSDK
    {
        private static string wxAppId = SiteSettings.wxAppId;
        private static string wxAppSecret = SiteSettings.wxAppSecret;
        /// 得到数据包，返回使用页面 
        /// <summary>
        /// 得到数据包，返回使用页面
        /// </summary>
        /// <returns></returns>
        public static Hashtable getSignPackage()
        {
            //AccessToken ace = Getaccess();
            string token = GetExistAccessToken();
            string JSTicketTicket = GetExistJSTicket();
            string url = HttpContext.Current.Request.Url.AbsoluteUri;
            string timestamp = Convert.ToString(ConvertDateTimeInt(DateTime.Now));
            string nonceStr = createNonceStr();


            // 这里参数的顺序要按照 key 值 ASCII 码升序排序  
            string rawstring = "JSTicket_ticket=" + JSTicketTicket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + url + "";

            string signature = FormsAuthentication.HashPasswordForStoringInConfigFile(rawstring, "SHA1").ToLower();
            //string signature = SHA1_Hash(rawstring);
            Hashtable signPackage = new Hashtable();
            signPackage.Add("appId", wxAppId);
            signPackage.Add("nonceStr", nonceStr);
            signPackage.Add("timestamp", timestamp);
            signPackage.Add("url", url);
            signPackage.Add("signature", signature);
            signPackage.Add("rawString", rawstring);
            signPackage.Add("JSTicketTicket", JSTicketTicket);

            return signPackage;
        }

        //得到数据包，返回使用页面  
        public static Hashtable getSignPackage(string url)
        {
            //AccessToken ace = Getaccess();
            string token = GetExistAccessToken();
            string JSTicketTicket = GetExistJSTicket();
            string timestamp = Convert.ToString(ConvertDateTimeInt(DateTime.Now));
            string nonceStr = createNonceStr();


            // 这里参数的顺序要按照 key 值 ASCII 码升序排序  
            string rawstring = "JSTicket_ticket=" + JSTicketTicket + "&noncestr=" + nonceStr + "&timestamp=" + timestamp + "&url=" + url + "";

            string signature = FormsAuthentication.HashPasswordForStoringInConfigFile(rawstring, "SHA1").ToLower();
            //string signature = SHA1_Hash(rawstring);
            Hashtable signPackage = new Hashtable();
            signPackage.Add("appId", wxAppId);
            signPackage.Add("nonceStr", nonceStr);
            signPackage.Add("timestamp", timestamp);
            signPackage.Add("url", url);
            signPackage.Add("signature", signature);
            signPackage.Add("rawString", rawstring);
            signPackage.Add("JSTicketTicket", JSTicketTicket);

            return signPackage;
        }

        /// 获取token,如果存在且没过期，则直接取token
        /// <summary>
        /// 获取token,如果存在且没过期，则直接取token
        /// </summary>
        /// <returns></returns>
        public static string GetExistAccessToken()
        {
            // 读取XML文件中的数据
            string filepath = System.Web.HttpContext.Current.Server.MapPath("/XMLToken.xml");
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader str = new StreamReader(fs, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            str.Dispose();
            fs.Close();
            fs.Dispose();
            string Token = xml.SelectSingleNode("xml").SelectSingleNode("AccessToken").InnerText;
            DateTime AccessTokenExpires = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("AccessExpires").InnerText);
            //如果token过期，则重新获取token
            if (DateTime.Now >= AccessTokenExpires)
            {
                AccessToken mode = Getaccess();
                //将token存到xml文件中，全局缓存
                xml.SelectSingleNode("xml").SelectSingleNode("AccessToken").InnerText = mode.access_token;
                DateTime _AccessTokenExpires = DateTime.Now.AddSeconds(mode.expires_in);
                xml.SelectSingleNode("xml").SelectSingleNode("AccessExpires").InnerText = _AccessTokenExpires.ToString();
                xml.Save(filepath);
                Token = mode.access_token;
            }
            return Token;
        }

        /// 获取jsapi_ticket,如果存在且没过期，则直接取jsapi_ticket
        /// <summary>
        /// 获取jsapi_ticket,如果存在且没过期，则直接取jsapi_ticket
        /// </summary>
        /// <returns></returns>
        public static string GetExistJSTicket()
        {
            // 读取XML文件中的数据
            string filepath = System.Web.HttpContext.Current.Server.MapPath("/XMLToken.xml");
            FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader str = new StreamReader(fs, System.Text.Encoding.UTF8);
            XmlDocument xml = new XmlDocument();
            xml.Load(str);
            str.Close();
            str.Dispose();
            fs.Close();
            fs.Dispose();
            string ticket = xml.SelectSingleNode("xml").SelectSingleNode("JSTicket").InnerText;
            DateTime AccessTokenExpires = Convert.ToDateTime(xml.SelectSingleNode("xml").SelectSingleNode("JSAccessExpires").InnerText);
            //如果jsapi_ticket过期，则重新获取token
            if (DateTime.Now >= AccessTokenExpires)
            {
                string Token = xml.SelectSingleNode("xml").SelectSingleNode("AccessToken").InnerText;
                JSTicket mode = getJSTicketTicket(Token);
                if (mode.ticket != null && mode.expires_in != null)
                {
                    //将jsapi_ticket存到xml文件中，全局缓存
                    xml.SelectSingleNode("xml").SelectSingleNode("JSTicket").InnerText = mode.ticket;
                    DateTime _AccessTokenExpires = DateTime.Now.AddSeconds(int.Parse(mode.expires_in));
                    xml.SelectSingleNode("xml").SelectSingleNode("JSAccessExpires").InnerText = _AccessTokenExpires.ToString();
                    xml.Save(filepath);
                    ticket = mode.ticket;
                }
                else
                {
                    ticket = "";
                }
            }
            return ticket;
        }

        /// <summary>
        /// 获取AccessToken_token
        /// </summary>
        /// <returns></returns>
        public static AccessToken Getaccess()
        {
            string Str = GetJson(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", wxAppId, wxAppSecret));
            AccessToken m = JsonHelper.ParseFromJson<AccessToken>(Str);
            return m;
        }

        /// 创建随机字符串  
        /// <summary>
        /// 创建随机字符串
        /// </summary>
        /// <returns></returns>
        private static string createNonceStr()
        {
            int length = 16;
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string str = "";
            Random rad = new Random();
            for (int i = 0; i < length; i++)
            {
                str += chars.Substring(rad.Next(0, chars.Length - 1), 1);
            }
            return str;
        }

        /// 获取jsapi_ticket
        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <param name="access"></param>
        /// <returns></returns>
        private static JSTicket getJSTicketTicket(string access)
        {
            string Str = GetJson("https://api.weixin.qq.com/cgi-bin/ticket/getticket?AccessToken_token=" + access + "&type=JSTicket");
            JSTicket jt = JsonConvert.DeserializeObject<JSTicket>(Str);
            return jt;
        }

        protected static string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);

            if (returnText.Contains("errcode"))
            {
                //可能发生错误  
            }
            //Response.Write(returnText);  
            return returnText;
        }

        //SHA1哈希加密算法  
        public static string SHA1_Hash(string str_sha1_in)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = System.Text.UTF8Encoding.Default.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.Replace("-", "").ToLower();
            return str_sha1_out;
        }

        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>double</returns>  
        public static int ConvertDateTimeInt(System.DateTime time)
        {
            int intResult = 0;
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            intResult = Convert.ToInt32((time - startTime).TotalSeconds);
            return intResult;
        }
    }
}