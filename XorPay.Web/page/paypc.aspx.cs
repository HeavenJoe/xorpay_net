using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XorPay.SDK;

namespace XorPay.Web.page
{
    public partial class paypc : System.Web.UI.Page
    {
        protected string pay_type = "", pay_text = "", notify_url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            pay_type = PayRequest.GetQueryString("pay_type");
            if (pay_type == "alipay")
            {
                pay_text = "支付宝";
            }
            else if (pay_type == "native")
            {
                pay_text = "微信";
            }

            notify_url = PayConfig.notify_url.StartsWith("http") ? PayConfig.notify_url : "http://" + Request.Url.Authority.Trim('/') + PayConfig.notify_url;
        }
    }
}