using System;
using System.Web.UI;
using XorPay.SDK;

namespace XorPay.Web.page
{
    public partial class notify_url : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string aoid = PayRequest.GetFormString("aoid");
            string detail = PayRequest.GetFormString("detail");
            string more = PayRequest.GetFormString("more");
            string order_id = PayRequest.GetFormString("order_id");
            string pay_price = PayRequest.GetFormString("pay_price");
            string pay_time = PayRequest.GetFormString("pay_time");
            string sign = PayRequest.GetFormString("sign");

            LogHelper.Info("----------订单回调通知接收参数----------");
            LogHelper.Info($"aoid:{aoid},detail:{detail},more:{more},order_id:{order_id},pay_price:{pay_price},pay_time:{pay_time},sign:{sign}");

            //验证签名
            if (!string.IsNullOrWhiteSpace(sign))
            {
                string parameters = $"{aoid}{order_id}{pay_price}{pay_time}{PayConfig.app_secret}";

                //签名验证通过
                if ((PayCore.Md5Hash(parameters, false) ?? "").ToLower() == sign.ToLower())
                {
                    //是否存在该订单
                    if (!Orders.Exists(order_id))
                    {
                        base.Response.Write("oid_not_exist");
                        return;
                    }

                    /*判断订单是否已支付，避免业务重复处理，以及订单金额与支付金额是否一致等
                     ---------根据自身业务进行操作校验----------------*/

                    //更新订单状态
                    if (Orders.Update($"aoid='{aoid}',pay_price='{pay_price}',pay_time='{pay_time}'", $"order_no='{order_id}'"))
                    {
                        //业务处理成功后输出 ok/success ，XorPay停止通知
                        Response.Write("success");
                        return;
                    }
                }
            }
            Response.Write("failed");
        }
    }
}
