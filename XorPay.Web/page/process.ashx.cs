using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using XorPay.SDK;

namespace XorPay.Web.page
{
    /// <summary>
    /// process 的摘要说明
    /// </summary>
    public partial class process : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string action = PayRequest.GetQueryString("action");
            switch (action)
            {
                case "paypc"://扫码支付
                    paypcGetCode(context);
                    break;
                case "paywap"://微信收银台
                    paywapGetInfo(context);
                    break;
                case "payjs"://微信jsapi
                    payjsGetInfo(context);
                    break;
                case "query"://订单查询
                    query(context);
                    break;
                case "queryOrerNo"://订单状态查询
                    queryOrerNo(context);
                    break;
                case "refund"://退款
                    refund(context);
                    break;

            }
        }

        #region 订单退款 + void refund(HttpContext context)
        /// <summary>
        /// 订单退款
        /// </summary>
        /// <param name="context"></param>
        private void refund(HttpContext context)
        {
            string aoid = PayRequest.GetFormString("aoid");
            string errorMsg = "";
            if (string.IsNullOrWhiteSpace(aoid))
            {
                errorMsg = "XorPay平台订单号不能为空";
            }
            float price = PayRequest.GetFormFloat("price", 0f);
            if (price <= 0f)
            {
                errorMsg = "退款金额必须大于0";
            }
            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                {
                    status = 0,
                    msg = errorMsg
                }));
                return;
            }
            string data_info = "";
            try
            {
                string strInfo = PayCore.Refund(aoid, price);
                if (!string.IsNullOrWhiteSpace(strInfo))
                {
                    statusInfo info = JsonHelper.JSONToObject<statusInfo>(strInfo);
                    if (info != null)
                    {
                        data_info = PayCore.GetDictValue(PayModel.refundStatusDict, info.status);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                {
                    status = 0,
                    msg = ex.Message
                }));
                return;
            }
            context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
            {
                status = 1,
                msg = "请求成功",
                data = data_info
            }));
        }
        #endregion

        #region 订单查询 + void query(HttpContext context)
        /// <summary>
        /// 订单查询
        /// </summary>
        /// <param name="context"></param>
        private void query(HttpContext context)
        {
            string aoid = PayRequest.GetFormString("aoid");
            if (string.IsNullOrWhiteSpace(aoid))
            {
                context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                {
                    status = 0,
                    msg = "XorPay平台订单号不能为空"
                }));
                return;
            }
            string data_info = "";
            try
            {
                string strInfo = PayRequest.SendRequest("https://xorpay.com/api/query/" + aoid, "", "GET", "UTF-8");
                if (!string.IsNullOrWhiteSpace(strInfo))
                {
                    statusInfo info = JsonHelper.JSONToObject<statusInfo>(strInfo);
                    if (info != null)
                    {
                        data_info = PayCore.GetDictValue(PayModel.queryStatusDict, info.status);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                {
                    status = 0,
                    msg = ex.Message
                }));
                return;
            }
            context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
            {
                status = 1,
                msg = "请求成功",
                data = data_info
            }));
        }
        #endregion

        #region 获取订单支付状态 + void queryOrerNo(HttpContext context)
        /// <summary>
        /// 获取订单支付状态
        /// </summary>
        /// <param name="context"></param>
        private void queryOrerNo(HttpContext context)
        {
            string order_no = PayRequest.GetQueryString("order_no");
            if (string.IsNullOrWhiteSpace(order_no))
            {
                context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                {
                    status = 0,
                    msg = "订单号不能为空"
                }));
                return;
            }
            string aoid = Orders.GetAoidByOrderNo(order_no);
            if (!string.IsNullOrWhiteSpace(aoid))
            {
                try
                {
                    string strInfo = PayRequest.SendRequest("https://xorpay.com/api/query/" + aoid, "", "GET", "UTF-8");
                    if (!string.IsNullOrWhiteSpace(strInfo))
                    {
                        statusInfo info = JsonHelper.JSONToObject<statusInfo>(strInfo);
                        if (info != null && (info.status == "payed" || info.status == "success"))
                        {
                            context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                            {
                                status = 1,
                                msg = "订单已支付"
                            }));
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Error(ex.Message);
                }
            }
            context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
            {
                status = 0,
                msg = ""
            }));
        }
        #endregion

        #region 获取扫码支付信息 + void paypcGetCode(HttpContext context)
        /// <summary>
        /// 获取扫码支付信息
        /// </summary>
        /// <param name="context"></param>
        private void paypcGetCode(HttpContext context)
        {
            string name = PayRequest.GetFormString("name");
            string pay_type = PayRequest.GetFormString("pay_type");
            float price = PayRequest.GetFormFloat("price", 0f);
            string order_id = PayRequest.GetFormString("order_id");
            string notify_url = PayRequest.GetFormString("notify_url");
            string order_uid = PayRequest.GetFormString("order_uid");
            string more = PayRequest.GetFormString("more");
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pay_type) || price <= 0f || string.IsNullOrWhiteSpace(order_id) || string.IsNullOrWhiteSpace(notify_url))
            {
                context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                {
                    status = 0,
                    msg = "参数信息获取失败"
                }));
                return;
            }
            string qr = "";
            string errormsg = "";
            if (Orders.Exists(order_id))
            {
                qr = Orders.GetQR(order_id);
                if (!string.IsNullOrWhiteSpace(qr))
                {
                    context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                    {
                        status = 1,
                        msg = "订单已存在",
                        data = qr
                    }));
                    return;
                }
            }
            try
            {
                PayRequestModel payRequest = new PayRequestModel
                {
                    name = name,
                    pay_type = pay_type,
                    price = price,
                    order_id = order_id,
                    notify_url = notify_url,
                    order_uid = order_uid,
                    more = more
                };
                string jsonStr = PayCore.GetPayInfo(payRequest);
                if (!string.IsNullOrWhiteSpace(jsonStr))
                {
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
                else
                {
                    errormsg = "请求失败，请检查aid与app_secret是否正确配置，以及XorPay后台是否正常";
                }
                if (!string.IsNullOrWhiteSpace(errormsg))
                {
                    context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                    {
                        status = 0,
                        msg = errormsg
                    }));
                    return;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                {
                    status = 0,
                    msg = ex.Message
                }));
                return;
            }
            context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
            {
                status = 1,
                msg = "请求成功",
                data = qr
            }));
        }
        #endregion

        #region 获取微信收银台跳转链接 + void paywapGetInfo(HttpContext context)
        /// <summary>
        /// 获取微信收银台跳转链接
        /// </summary>
        /// <param name="context"></param>
        private void paywapGetInfo(HttpContext context)
        {
            string name = PayRequest.GetFormString("name");
            string pay_type = PayRequest.GetFormString("pay_type");
            float price = PayRequest.GetFormFloat("price", 0f);
            string order_id = PayRequest.GetFormString("order_id");
            string notify_url = PayRequest.GetFormString("notify_url");
            string cancel_url = PayRequest.GetFormString("cancel_url");
            string return_url = PayRequest.GetFormString("return_url");
            string order_uid = PayRequest.GetFormString("order_uid");
            string more = PayRequest.GetFormString("more");
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pay_type) || price <= 0f || string.IsNullOrWhiteSpace(order_id) || string.IsNullOrWhiteSpace(notify_url))
            {
                context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                {
                    status = 0,
                    msg = "参数信息获取失败"
                }));
                return;
            }
            string qr;
            if (Orders.Exists(order_id))
            {
                qr = Orders.GetQR(order_id);
                if (!string.IsNullOrWhiteSpace(qr))
                {
                    context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                    {
                        status = 1,
                        msg = "订单已存在",
                        data = qr
                    }));
                    return;
                }
            }
            PayRequestModel requestModel = new PayRequestModel
            {
                cancel_url = cancel_url,
                more = more,
                name = name,
                notify_url = notify_url,
                order_id = order_id,
                order_uid = order_uid,
                pay_type = pay_type,
                price = price,
                return_url = return_url
            };
            qr = PayCore.GetWXPayUrl(requestModel);
            if (!Orders.Add(requestModel, "", qr))
            {
                context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                {
                    status = 0,
                    msg = "添加失败"
                }));
                return;
            }
            context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
            {
                status = 1,
                msg = "请求成功",
                data = qr
            }));
        }
        #endregion

        #region 获取jsapi支付参数 + void payjsGetInfo(HttpContext context)
        /// <summary>
        /// 获取jsapi支付参数
        /// </summary>
        /// <param name="context"></param>
        private void payjsGetInfo(HttpContext context)
        {
            string name = PayRequest.GetFormString("name");
            string pay_type = PayRequest.GetFormString("pay_type");
            float price = PayRequest.GetFormFloat("price", 0f);
            string order_id = PayRequest.GetFormString("order_id");
            string notify_url = PayRequest.GetFormString("notify_url");
            string order_uid = PayRequest.GetFormString("order_uid");
            string more = PayRequest.GetFormString("more");
            string open_id = PayRequest.GetFormString("openid");

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(pay_type) || price <= 0f || string.IsNullOrWhiteSpace(order_id) || string.IsNullOrWhiteSpace(notify_url) || string.IsNullOrWhiteSpace(open_id))
            {
                context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                {
                    status = 0,
                    msg = "参数信息获取失败"
                }));
                return;
            }

            if (Orders.Exists(order_id))
            {
                string jsApiInfo = Orders.GetJsApiInfo(order_id);
                if (!string.IsNullOrWhiteSpace(jsApiInfo))
                {
                    context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                    {
                        status = 1,
                        msg = "订单已存在",
                        data = order_id
                    }));
                    return;
                }
            }

            string errormsg = "请求失败";

            try
            {
                PayRequestModel payRequest = new PayRequestModel
                {
                    name = name,
                    pay_type = pay_type,
                    price = price,
                    order_id = order_id,
                    notify_url = notify_url,
                    order_uid = order_uid,
                    more = more,
                    openid = open_id
                };

                string jsonStr = PayCore.GetPayInfo(payRequest);
                if (!string.IsNullOrWhiteSpace(jsonStr))
                {
                    JsPayResponse model = JsonHelper.JSONToObject<JsPayResponse>(jsonStr);
                    if (model != null)
                    {
                        if (model.status == "ok")
                        {
                            if (model.info != null)
                            {
                                var jsapiInfo = JsonHelper.ObjectToJSON(model.info);
                                if(Orders.Add(payRequest, model.aoid, "", jsapiInfo))
                                {
                                    context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                                    {
                                        status = 1,
                                        msg = "请求成功",
                                        data = order_id
                                    }));
                                    return;
                                }
                            }
                        }
                        else
                        {
                            errormsg = PayCore.GetDictValue(PayModel.payStatusDict, model.status);
                        }
                    }
                }
                else
                {
                    errormsg = "请求失败，请检查aid与app_secret是否正确配置，以及XorPay后台是否正常";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex.Message);
                context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
                {
                    status = 0,
                    msg = ex.Message
                }));
                return;
            }
            context.Response.Write(JsonHelper.ObjectToJSON(new JsonData<string>
            {
                status = 0,
                msg = errormsg
            }));
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}