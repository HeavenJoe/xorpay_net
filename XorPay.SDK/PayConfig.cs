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
        }

        /// <summary>
        /// aid
        /// </summary>
        public const string aid = "";

        /// <summary>
        /// app_secret
        /// </summary>
        public const string app_secret = "";

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
