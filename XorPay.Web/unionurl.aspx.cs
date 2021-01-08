using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using XorPay.SDK;

namespace XorPay.Web
{
    public partial class unionurl : System.Web.UI.Page
    {
        protected string qr = "",errormsg = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            string userAgent = Request.UserAgent;

            string order_no = "B" + DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(10, 99);

            float amount = PayRequest.GetQueryFloat("amount", 1f);

            PayConfig payConfig = new PayConfig();

            string jsapi_callback = $"https://xorpay.com/api/openid/{PayConfig.aid}?callback={HttpUtility.UrlEncode(payConfig.protocol + "/unionurl.aspx?pay_type=jsapi&amount=" + amount)}";

            string open_id = PayRequest.GetQueryString("openid");

            if (Orders.Exists(order_no))
            {
                qr = Orders.GetQR(order_no);
                if (!string.IsNullOrWhiteSpace(qr))
                {
                    return;
                }
            }

            string pay_type = "alipay";

            if (userAgent.ToLower().Contains("alipayclient"))
            {
                pay_type = "alipay";
            }
            else
            {
                pay_type = "native";
                if (string.IsNullOrWhiteSpace(open_id))
                {
                    Response.Redirect(jsapi_callback);
                    return;
                }
            }

            PayRequestModel payRequest = new PayRequestModel
            {
                name = "统一支付",
                pay_type = pay_type,
                price = 1f,
                order_id = order_no,
                notify_url = payConfig.notify_url,
                order_uid = "union_test",
                more = "统一支付test",
                openid = open_id
            };

            try
            {
                if (pay_type == "native")//微信支付
                {
                    string wxUrl = PayCore.GetWXPayUrl(payRequest);
                    if (!string.IsNullOrWhiteSpace(wxUrl))
                    {
                        qr = wxUrl;
                    }
                }
                else//默认支付宝
                {
                    string jsonStr = PayCore.GetPayInfo(payRequest);
                    CodePayResponse model = JsonHelper.JSONToObject<CodePayResponse>(jsonStr);
                    if (model != null)
                    {
                        if (model.status == "ok")
                        {
                            qr = ((model.info != null) ? model.info.qr : "");
                            Orders.Add(payRequest, model.aoid, qr);
                        }
                        else
                        {
                            errormsg = PayCore.GetDictValue(PayModel.payStatusDict, model.status);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errormsg = "系统繁忙...";
                LogHelper.Error(ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errormsg))
            {
                Response.Write(errormsg);
            }

        }
    }
}