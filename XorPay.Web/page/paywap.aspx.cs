using System;
using XorPay.SDK;

namespace XorPay.Web.page
{
    public partial class paywap : System.Web.UI.Page
    {
        protected string pay_type = "", pay_text = "", notify_url = "", return_url = "", cancel_url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            pay_type = PayRequest.GetQueryString("pay_type");
            if (pay_type == "alipay")
            {
                pay_text = "支付宝（在手机浏览器中打开）";
            }
            else if (pay_type == "jsapi")
            {
                pay_text = "微信（在微信客户端打开）";
            }

            notify_url = PayConfig.notify_url.StartsWith("http") ? PayConfig.notify_url : "http://" + Request.Url.Authority.Trim('/') + PayConfig.notify_url;
            return_url = PayConfig.notify_url.StartsWith("http") ? PayConfig.return_url : "http://" + Request.Url.Authority.Trim('/') + PayConfig.return_url;
            cancel_url = PayConfig.notify_url.StartsWith("http") ? PayConfig.cancel_url : "http://" + Request.Url.Authority.Trim('/') + PayConfig.cancel_url;
        }
    }
}