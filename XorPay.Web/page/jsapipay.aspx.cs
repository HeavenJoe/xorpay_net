using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XorPay.SDK;

namespace XorPay.Web.page
{
    public partial class jsapipay : System.Web.UI.Page
    {
        protected string wxJsApiParam = "{}", return_url="", cancel_url="";

        protected void Page_Load(object sender, EventArgs e)
        {
            string order_no = PayRequest.GetQueryString("order_no");
            if (!string.IsNullOrWhiteSpace(order_no))
            {
                var jsapiInfo = Orders.GetJsApiInfo(order_no);
                wxJsApiParam = string.IsNullOrWhiteSpace(jsapiInfo) ? "{}" : jsapiInfo;
            }

            return_url = PayConfig.notify_url.StartsWith("http") ? PayConfig.return_url : "http://" + Request.Url.Authority.Trim('/') + PayConfig.return_url;
            cancel_url = PayConfig.notify_url.StartsWith("http") ? PayConfig.cancel_url : "http://" + Request.Url.Authority.Trim('/') + PayConfig.cancel_url;

        }
    }
}