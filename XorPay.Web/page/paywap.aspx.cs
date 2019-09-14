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

            PayConfig payConfig = new PayConfig();

            notify_url = payConfig.notify_url;
            return_url = payConfig.return_url;
            cancel_url = payConfig.cancel_url;
        }
    }
}