using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace WechatSDK
{
    /// <summary>
    /// 用户授权and获取用户信息
    /// </summary>
    public class WXOauth
    {
        public void GenerOauth(string code) {
            OAuth_Token Model = Get_token(code);
            OAuthUser user = Get_UserInfo(Model.access_token, Model.openid);
        }

        /// 获得Token  
        /// <summary>
        /// 获得Token  
        /// </summary>
        /// <param name="Code"></param>
        /// <returns></returns>
        protected OAuth_Token Get_token(string Code)
        {
            string Str = GetJson("https://api.weixin.qq.com/sns/oauth2/access_token?appid=" + SiteSettings.wxAppId + "&secret=" + SiteSettings.wxAppSecret + "&code=" + Code + "&grant_type=authorization_code");
            OAuth_Token Oauth_Token_Model = JsonHelper.ParseFromJson<OAuth_Token>(Str);
            return Oauth_Token_Model;
        }

        /// 获得用户信息 
        /// <summary>
        /// 获得用户信息
        /// </summary>
        /// <param name="REFRESH_TOKEN"></param>
        /// <param name="OPENID"></param>
        /// <returns></returns>
        protected OAuthUser Get_UserInfo(string REFRESH_TOKEN, string OPENID)
        {
            // Response.Write("获得用户信息REFRESH_TOKEN:" + REFRESH_TOKEN + "||OPENID:" + OPENID);  
            string Str = GetJson("https://api.weixin.qq.com/sns/userinfo?access_token=" + REFRESH_TOKEN + "&openid=" + OPENID);
            OAuthUser OAuthUser_Model = JsonHelper.ParseFromJson<OAuthUser>(Str);
            return OAuthUser_Model;
        }

        protected string GetJson(string url)
        {
            WebClient wc = new WebClient();
            wc.Credentials = CredentialCache.DefaultCredentials;
            wc.Encoding = Encoding.UTF8;
            string returnText = wc.DownloadString(url);
            return returnText;
        }
    }


}