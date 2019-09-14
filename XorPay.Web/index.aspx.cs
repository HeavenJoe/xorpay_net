using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XorPay.SDK;

namespace XorPay.Web
{
    public partial class index : System.Web.UI.Page
    {
        protected string openid_callback = "", jsapi_callback = "";

        protected int check_config = 0;

        protected string open_id = "", unionurl = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            PayConfig payConfig = new PayConfig();

            openid_callback = $"https://xorpay.com/api/openid/{PayConfig.aid}?callback={HttpUtility.UrlEncode(payConfig.protocol + "/page/openid.aspx?pay=xorpay")}";

            jsapi_callback = $"https://xorpay.com/api/openid/{PayConfig.aid}?callback={HttpUtility.UrlEncode(payConfig.protocol + "/index.aspx?pay=xorpay")}";

            unionurl = $"{payConfig.protocol}/unionurl.aspx";

            check_config = string.IsNullOrWhiteSpace(PayConfig.aid) || string.IsNullOrWhiteSpace(PayConfig.app_secret) ? 0 : 1;

            open_id = PayRequest.GetQueryString("openid");
        }
    }
}