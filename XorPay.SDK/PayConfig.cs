using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XorPay.SDK
{
    /// <summary>
    /// 支付配置类
    /// </summary>
    public class PayConfig
    {
        /// <summary>
        /// 域名
        /// </summary>
        public string protocol = "";

        public PayConfig()
        {
            protocol = string.IsNullOrWhiteSpace(protocol) ? $"http{(HttpContext.Current.Request.IsSecureConnection ? "s" : "")}://{HttpContext.Current.Request.Url.Authority.Trim('/')}" : "";
            notify_url = $"{protocol}{notify_url}";
            return_url = $"{protocol}{return_url}";
            cancel_url = $"{protocol}{return_url}";
            string x_aid = System.Configuration.ConfigurationManager.AppSettings["xorpay_aid"];
            string x_appsecert = System.Configuration.ConfigurationManager.AppSettings["xorpay_appsecert"];
            if (!string.IsNullOrWhiteSpace(x_aid))
            {
                aid = x_aid;
            }
            if (!string.IsNullOrWhiteSpace(x_appsecert))
            {
                app_secret = x_appsecert;
            }
        }

        /// <summary>
        /// aid
        /// </summary>
        public static string aid = "";

        /// <summary>
        /// app_secret
        /// </summary>
        public static string app_secret = "";

        /// <summary>
        /// 回调通知地址
        /// </summary>
        public string notify_url = "/page/notify_url.aspx";

        /// <summary>
        /// 支付成功跳转地址
        /// </summary>
        public string return_url = "/page/return_url.aspx";

        /// <summary>
        /// 取消支付跳转地址
        /// </summary>
        public string cancel_url = "/page/cancel_url.aspx";

    }
}
