using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XorPay.SDK
{
    /// <summary>
    /// 支付配置类
    /// </summary>
    public class PayConfig
    {
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
        public const string notify_url = "/page/notify_url.aspx";

        /// <summary>
        /// 支付成功跳转地址
        /// </summary>
        public const string return_url = "/page/return_url.aspx";

        /// <summary>
        /// 取消支付跳转地址
        /// </summary>
        public const string cancel_url = "/page/cancel_url.aspx";

    }
}
