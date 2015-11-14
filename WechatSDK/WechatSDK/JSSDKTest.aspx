<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JSSDKTest.aspx.cs" Inherits="WechatSDK.JSSDKTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            appId = '<%=appId %>'<br />
            nonceStr = '<%=nonceStr %>'<br />
            signature = '<%=signature %>'<br />
            timestamp = '<%=timestamp %>'<br />
        </div>
        <script type="text/javascript" src="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.2.min.js"></script>
        <script src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js" type="text/javascript"></script>
        <script>
            var appId = '<%=appId %>'
               , nonceStr = '<%=nonceStr %>'
                , signature = '<%=signature %>'
                , timestamp = '<%=timestamp %>';
            wx.config({
                debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                appId: appId, // 必填，公众号的唯一标识
                timestamp: timestamp, // 必填，生成签名的时间戳
                nonceStr: nonceStr, // 必填，生成签名的随机串
                signature: signature, // 必填，签名，见附录1
                jsApiList: ['getLocation'] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
            });

            wx.ready(function () {
                wx.getLocation({
                    success: function (res) {
                        sessionStorage.setItem("latitude", res.latitude);
                        sessionStorage.setItem("longitude", res.longitude);
                    }
                });
            });
        </script>
    </form>
</body>
</html>
