using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WechatSDK
{
    public partial class JSSDKTest : System.Web.UI.Page
    {
        public string appId = "";
        public string timestamp = "";
        public string nonceStr = "";
        public string signature = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                Hashtable ht = JSSDK.getSignPackage();
                appId = ht["appId"].ToString();
                nonceStr = ht["nonceStr"].ToString();
                timestamp = ht["timestamp"].ToString();
                signature = ht["signature"].ToString();
            }
        }
    }
}