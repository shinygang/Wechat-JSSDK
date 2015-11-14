using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WechatSDK
{
    public class OAuth_Token
    {
        public OAuth_Token()
        {
            //  
            //TODO: 在此处添加构造函数逻辑  
            //  
        }
        //access_token  网页授权接口调用凭证,注意：此access_token与基础支持的access_token不同  
        //expires_in    access_token接口调用凭证超时时间，单位（秒）  
        //refresh_token 用户刷新access_token  
        //openid    用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID  
        //scope 用户授权的作用域，使用逗号（,）分隔  
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string refresh_token { get; set; }
        public string openid { get; set; }
        public string scope { get; set; }
    }
}