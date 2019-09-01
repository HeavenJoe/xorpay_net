using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace XorPay.SDK
{
    /// <summary>
    /// 支付核心类
    /// </summary>
    public class PayCore
    {
        #region 发起退款 + static string Refund(string aoid,float price)
        /// <summary>
        /// 发起退款
        /// </summary>
        /// <param name="aoid">XorPay平台订单号</param>
        /// <param name="price">退款金额</param>
        /// <returns></returns>
        public static string Refund(string aoid,float price)
        {
            string sign = Md5Hash($"{price}{PayConfig.app_secret}");
            string parameters = $"price={price}&sign={sign}";
            return PayRequest.SendRequest($"https://xorpay.com/api/refund/{aoid}", parameters, "POST");
        }
        #endregion

        #region 获取支付信息 + static string GetPayInfo(CodePayRequest requestModel)
        /// <summary>
        /// 获取支付二维码
        /// </summary>
        /// <returns></returns>
        public static string GetPayInfo(PayRequestModel requestModel)
        {
            string sign = Md5Hash($"{requestModel.name}{requestModel.pay_type}{requestModel.price}{requestModel.order_id}{requestModel.notify_url}{PayConfig.app_secret}");
            string parameters = $"name={requestModel.name}&pay_type={requestModel.pay_type}&price={requestModel.price}&order_id={requestModel.order_id}&sign={sign}&notify_url={HttpUtility.UrlEncode(requestModel.notify_url)}&order_uid={requestModel.order_uid}&more={requestModel.more}&expire={requestModel.expire}&openid={requestModel.openid}";
            return PayRequest.SendRequest($"https://xorpay.com/api/pay/{PayConfig.aid}", parameters, "POST");
        }
        #endregion

        #region 获取微信收银台跳转链接 + static string GetWXPayUrl(CodePayRequest requestModel)
        /// <summary>
        /// 获取微信收银台跳转链接
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        public static string GetWXPayUrl(PayRequestModel requestModel)
        {
            string sign = Md5Hash($"{requestModel.name}{requestModel.pay_type}{requestModel.price}{requestModel.order_id}{requestModel.notify_url}{PayConfig.app_secret}");
            string parameters = $"name={requestModel.name}&pay_type={requestModel.pay_type}&price={requestModel.price}&order_id={requestModel.order_id}&sign={sign}&notify_url={HttpUtility.UrlEncode(requestModel.notify_url)}&return_url={HttpUtility.UrlEncode(requestModel.return_url)}&cancel_url={HttpUtility.UrlEncode(requestModel.cancel_url)}&order_uid={requestModel.order_uid}&more={requestModel.more}&expire={requestModel.expire}";
            return $"https://xorpay.com/api/cashier/{PayConfig.aid}?{parameters}";
        }
        #endregion


        #region MD5加密 + static string Md5Hash(string input)
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Md5Hash(string input, bool is_16bit = false)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(input);
            MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();
            byte[] byteArr = MD5.ComputeHash(buffer);
            string md5Str = BitConverter.ToString(byteArr).Replace("-", "");
            return is_16bit ? md5Str.Substring(0, 16) : md5Str;
        }
        #endregion

        #region 根据字典key获取value值 + static string GetDictValue(Dictionary<string, string> dict, string key)
        /// <summary>
        /// 根据字典key获取value值
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetDictValue(Dictionary<string, string> dict, string key)
        {
            string default_value = "";
            if (dict != null && dict.ContainsKey(key))
            {
                dict.TryGetValue(key, out default_value);
            }
            return default_value;
        }
        #endregion
    }

}
