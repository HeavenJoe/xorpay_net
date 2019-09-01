using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XorPay.SDK
{

    /// <summary>
    /// 相关信息
    /// </summary>
    public class PayModel
    {
        /// <summary>
        /// 下单返回状态
        /// </summary>
        public static Dictionary<string, string> payStatusDict = new Dictionary<string, string> {
            { "ok","成功" },
            { "missing_argument","缺少参数"},
            { "app_off","账号被冻结"},
            { "aid_not_exist","aid不存在"},
            { "pay_type_error","支付类型错误"},
            { "sign_error","签名错误"},
            { "order_payed","订单已支付"},
            { "order_expire","订单过期"},
            { "wechat_api_error","微信服务器错误"},
            { "fee_error","余额不足"},
            { "order_exist","同一订单，参数不同"},
            { "no_contract","未签约"},
            { "no_alipay_contract","未签约支付宝"}
        };

        /// <summary>
        /// 订单查询返回状态
        /// </summary>
        public static Dictionary<string, string> queryStatusDict = new Dictionary<string, string> {
            { "not_exist","订单不存在" },
            { "new","订单未支付"},
            { "payed","订单已支付，未通知成功"},
            { "fee_error","手续费扣除失败"},
            { "success","订单已支付，通知成功"},
            { "expire","订单过期"}
        };

        /// <summary>
        /// 订单退款返回状态
        /// </summary>
        public static Dictionary<string, string> refundStatusDict = new Dictionary<string, string> {
            { "ok","成功" },
            { "order_error","订单状态不是success/或者订单不存在" },
            { "price_error","退款金额大于收款金额"},
            { "sign_error","签名错误"},
            { "alipay_api_error","支付宝接口错误"}
        };

    }

    #region 支付请求实体信息 + PayRequestModel
    /// <summary>
    /// 支付请求信息
    /// </summary>
    public class PayRequestModel
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 支付类型，native
        /// </summary>
        public string pay_type { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public float price { get; set; }

        /// <summary>
        /// 平台订单号，需要唯一
        /// </summary>
        public string order_id { get; set; }

        /// <summary>
        /// 订单用户如: abc@def.com
        /// </summary>
        public string order_uid { get; set; }

        /// <summary>
        /// 回调地址
        /// </summary>
        public string notify_url { get; set; }

        /// <summary>
        /// 支付成功跳转地址
        /// </summary>
        public string return_url { get; set; }

        /// <summary>
        /// 支付取消跳转地址
        /// </summary>
        public string cancel_url { get; set; }

        /// <summary>
        /// 订单其他信息，回调时原样传回
        /// </summary>
        public string more { get; set; }

        /// <summary>
        /// 用户openid
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 订单过期秒数，默认7200
        /// </summary>
        public int expire { get; set; } = 7200;

        /// <summary>
        /// 将参数按name + pay_type + price + order_id + notify_url + app secret顺序拼接后MD5(纯 value 拼接，不要包含 + 号)
        /// </summary>
        public string sign { get; set; }
    }
    #endregion

    #region 支付请求响应实体信息
    /// <summary>
    /// 支付响应信息
    /// </summary>
    public class CodePayResponse
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// XorPay 平台统一订单号
        /// </summary>
        public string aoid { get; set; }

        /// <summary>
        /// 订单过期秒数
        /// </summary>
        public int expire_in { get; set; }

        /// <summary>
        /// qr 支付二维码
        /// </summary>
        public code_info info { get; set; }

    }

    public class code_info
    {
        public string qr { get; set; }
    }

    public class JsPayResponse
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// XorPay 平台统一订单号
        /// </summary>
        public string aoid { get; set; }

        /// <summary>
        /// 订单过期秒数
        /// </summary>
        public int expire_in { get; set; }

        /// <summary>
        /// 支付参数信息
        /// </summary>
        public pay_info info { get; set; }

    }

    public class pay_info
    {
        /// <summary>
        /// 公众号appid
        /// </summary>
        public string appId { get; set; }

        /// <summary>
        /// 时间戳，自1970年以来的秒数
        /// </summary>
        public string timeStamp { get; set; }

        /// <summary>
        /// 随机串
        /// </summary>
        public string nonceStr { get; set; }

        /// <summary>
        /// 预支付id
        /// </summary>
        public string package { get; set; }

        /// <summary>
        /// 微信签名方式
        /// </summary>
        public string signType { get; set; }

        /// <summary>
        /// 微信签名
        /// </summary>
        public string paySign { get; set; }

    }

    #endregion

    #region 回调通知detail信息 + PayNotifyDetail
    /// <summary>
    /// 回调通知detail信息
    /// </summary>
    public class PayNotifyDetail
    {
        /// <summary>
        /// 渠道流水号
        /// </summary>
        public string transaction_id { get; set; }

        /// <summary>
        /// 用户付款方式
        /// </summary>
        public string bank_type { get; set; }

        /// <summary>
        /// 消费者
        /// </summary>
        public string buyer { get; set; }

    } 
    #endregion

    #region 查询/退款请求响应信息 +  class statusInfo
    /// <summary>
    /// 查询/退款请求响应信息
    /// </summary>
    public class statusInfo
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string info { get; set; }
    }
    #endregion

}
