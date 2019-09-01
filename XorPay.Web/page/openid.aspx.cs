using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XorPay.SDK;

namespace XorPay.Web.page
{
    public partial class openid : System.Web.UI.Page
    {
        protected string open_id = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            open_id = PayRequest.GetQueryString("openid");
        }
    }
}