# Wechat-SDK
C#版本的微信sdk处理。

## JSSDK
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


