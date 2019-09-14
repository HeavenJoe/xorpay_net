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


            PayConfig payConfig = new PayConfig();

            notify_url = payConfig.notify_url;
            return_url = payConfig.return_url;
            cancel_url = payConfig.cancel_url;

        }
    }
}