using System;
using XorPay.SDK;

namespace XorPay.Web.page
{
    public partial class payjs : System.Web.UI.Page
    {
        protected string open_id = "", pay_text = "", notify_url = "", return_url = "", cancel_url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            open_id = PayRequest.GetQueryString("open_id");

            notify_url = PayConfig.notify_url.StartsWith("http") ? PayConfig.notify_url : "http://" + Request.Url.Authority.Trim('/') + PayConfig.notify_url;
            return_url = PayConfig.notify_url.StartsWith("http") ? PayConfig.return_url : "http://" + Request.Url.Authority.Trim('/') + PayConfig.return_url;
            cancel_url = PayConfig.notify_url.StartsWith("http") ? PayConfig.cancel_url : "http://" + Request.Url.Authority.Trim('/') + PayConfig.cancel_url;
        }
    }
}