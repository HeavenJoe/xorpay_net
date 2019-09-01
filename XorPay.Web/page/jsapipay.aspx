<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="jsapipay.aspx.cs" Inherits="XorPay.Web.page.jsapipay" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>jsapi原生微信支付</title>
    <script type="text/javascript">
        //调用微信JS api 支付
        function jsApiCall() {
            WeixinJSBridge.invoke(
                'getBrandWCPayRequest',
            <%=wxJsApiParam%>, //josn串
            function (res) {
                WeixinJSBridge.log(res.err_msg);
                if (res.err_msg == "get_brand_wcpay_request:ok") {
                    window.location.href = "<%=return_url%>"; //成功后跳转到提示页面
                }
                else if (res.err_msg == "get_brand_wcpay_request:cancel" || res.err_msg == "get_brand_wcpay_request:fail") {
                    window.location.href = "<%=cancel_url%>"; //取消后跳转到取消或支付失败页面
                }

            }
        );
        }
        function callpay() {
            if (typeof WeixinJSBridge == "undefined") {
                if (document.addEventListener) {
                    document.addEventListener('WeixinJSBridgeReady', jsApiCall, false);
                } else if (document.attachEvent) {
                    document.attachEvent('WeixinJSBridgeReady', jsApiCall);
                    document.attachEvent('onWeixinJSBridgeReady', jsApiCall);
                }
            } else {
                jsApiCall();
            }
        }
        //页面加载完成后执行函数
        window.onload = callpay;
    </script>
</head>

<body>
</body>
</html>
