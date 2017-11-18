# Wechat-SDK
C#版本的微信sdk处理。

-------
#### ` 如果对你有帮助，恳请给作者累积一个大保健的机会，欢迎扫码`
![](http://ww1.sinaimg.cn/large/79462090ly1flmcqgz0xwj21080q2anh.jpg)

首先请修改SiteSettings.cs里面的参数

## JSSDK
[官方jssdk文档](http://mp.weixin.qq.com/wiki/7/1c97470084b73f8e224fe6d9bab1625b.html)
微信jssdk签名认证，JSSDK.cs里面修改参数：
```
private static string wxAppId = "*****";
private static string wxAppSecret = "*****";
```
然后页面直接调用：
```
JSSDK.getSignPackage()或者JSSDK.getSignPackage(url)

Hashtable ht = JSSDK.getSignPackage();
string appId = ht["appId"].ToString();
string nonceStr = ht["nonceStr"].ToString();
string timestamp = ht["timestamp"].ToString();
string signature = ht["signature"].ToString();
```
然后根据获取的参数进行微信jssdk签名


## WXOauth.cs 微信认证和获取用户信息
[官方授权文档](http://mp.weixin.qq.com/wiki/9/01f711493b5a02f24b04365ac5d8fd95.html)
```
//获取token和openid信息
OAuth_Token Model = Get_token(code);
//获取微信用户信息
OAuthUser user = Get_UserInfo(Model.access_token, Model.openid);
```


