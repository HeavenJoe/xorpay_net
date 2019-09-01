<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="XorPay.Web.index" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="renderer" content="webkit">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="shortcut icon" href="/favicon.ico" />
    <title>XorPay支付（.Net版本）</title>
    <link rel="stylesheet" href="/layui/css/layui.css" />
    <link rel="stylesheet" href="/layui/css/common.css" />
</head>
<body>

    <div class="container">

        <fieldset class="layui-elem-field layui-field-title" style="text-align: center;">
            <legend style="font-size: 20px;">XorPay支付（.Net版本）</legend>
        </fieldset>

        <blockquote class="layui-elem-quote">
            提示：以下为XorPay的.Net版本演示Demo,
            <span class="layui-text"><a href="https://xorpay.jinliniuan.com" target="_blank">点击查看线上版本演示</a></span>
        </blockquote>

        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
            <legend>扫码支付</legend>
        </fieldset>

        <div class="layui-row">
            <div class="layui-col-xs5">
                <button type="button" class="layui-btn layui-btn-fluid lay-href-btn not-full lay-pc-pay" data-href="/page/paypc.aspx?pay_type=native">微信支付</button>
            </div>
            <div class="layui-col-xs5" style="float: right">
                <button type="button" class="layui-btn layui-btn-normal layui-btn-fluid lay-href-btn not-full lay-pc-pay" data-href="/page/paypc.aspx?pay_type=alipay">支付宝支付</button>
            </div>
        </div>

        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
            <legend>移动支付</legend>
        </fieldset>

        <div class="layui-row">
            <div class="layui-col-xs5">
                <button type="button" class="layui-btn layui-btn-fluid lay-href-btn not-full lay-pc-pay" data-href="/page/paywap.aspx?pay_type=jsapi">微信收银台支付</button>
            </div>
            <div class="layui-col-xs5" style="float: right">
                <button type="button" class="layui-btn layui-btn-fluid not-full lay-href-btn not-full lay-pc-pay" id="jspay-btn">微信JSAPI支付</button>
            </div>
        </div>

        <div class="layui-row" style="margin-top: 15px;">
            <div class="layui-col-xs5">
                <button type="button" class="layui-btn layui-btn-normal layui-btn-fluid lay-href-btn not-full lay-pc-pay" data-href="/page/paywap.aspx?pay_type=alipay">支付宝支付</button>
            </div>
            <div class="layui-col-xs5" style="float: right">
                <a class="layui-btn layui-btn-primary layui-btn-fluid not-full" href="<%=openid_callback %>">获取OPENID</a>
            </div>
        </div>

        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
            <legend>查询退款</legend>
        </fieldset>

        <div class="layui-row">
            <div class="layui-col-xs5">
                <button type="button" class="layui-btn layui-btn-warm layui-btn-fluid lay-href-btn not-full" data-href="/page/query.aspx">订单查询</button>
            </div>
            <div class="layui-col-xs5" style="float: right">
                <button type="button" class="layui-btn layui-btn-danger layui-btn-fluid lay-href-btn not-full" data-href="/page/refund.aspx">订单退款</button>
            </div>
        </div>

        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
            <legend>相关资料</legend>
        </fieldset>

        <div class="layui-row">
            <div class="layui-col-xs5" style="text-align: center">
                <div class="layui-text">
                    <a class="lay-href-btn" data-href="https://xorpay.com/doc/" href="javascript:void(0)">XorPay平台介绍</a>
                </div>
            </div>
            <div class="layui-col-xs5" style="float: right; text-align: center">
                <div class="layui-text">
                    <a class="lay-href-btn" data-href="https://xorpay.com/doc/faq.html" href="javascript:void(0)">FAQ常见问题</a>
                </div>
            </div>
        </div>

        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 20px;">
            <legend>联系方式</legend>
        </fieldset>

        <div class="layui-row">
            <div class="layui-col-xs12">
                <div class="layui-text" style="text-align: center">
                    <p class="layui-text"><a href="javascript:void(0)">QQ/WX：350178646</a></p>
                    <br />
                    <p class="layui-text"><a href="javascript:void(0)">邮箱：350178646@qq.com</a></p>
                    <br />
                </div>
            </div>
        </div>

    </div>

    <script type="text/javascript" src="/layui/layui.js"></script>
    <script>
        layui.use(['layer', 'form', 'jquery'], function () {
            var layer = layui.layer
                , form = layui.form
                , $ = layui.jquery
                , device = layui.device()
                ;


            $(function () {

                if (<%=check_config%>== 0) {

                    layerMsg('请先配置XorPay.SDK文件夹下PayConfig.cs文件中的aid与app_secret', 0, function () {
                        return false;
                    });
                }

                $(".lay-href-btn").on("click", function () {
                    var href = $(this).data('href');
                    var title = $(this).text() || '信息';
                    var areaArray = ['350px', '400px'];
                    if ($(this).hasClass("lay-pc-pay")) {
                        areaArray = ['350px', '500px'];
                    }
                    if (href && href != '') {
                        var z_index = layer.open({
                            type: 2,
                            title: title,
                            content: href,
                            shade: [0.8, '#393D49'],
                            area: areaArray,
                            maxmin: false
                        });
                        if (!$(this).hasClass("not-full")) {
                            layer.full(z_index);
                        }
                    }
                });

                var open_id = '<%=open_id%>';

                if (open_id && open_id != '') {
                    $("#jspay-btn").data("href", "/page/payjs.aspx?pay_type=jsapi&open_id=" + open_id).click();
                }

                $("#jspay-btn").on("click", function () {
                    if (!open_id || open_id == '') {
                        location.href = '<%=jsapi_callback%>';
                    }
                });


            });


            function layerMsg(title, icon_num, hash, _time) {
                icon_num = icon_num || 0;
                layer.msg(title, { icon: icon_num, time: _time || 0, shade: 0.3, shadeClose: true }, function () {
                    if (hash) {
                        if (typeof (hash) === "function") {
                            hash();
                        } else {
                            location.hash = hash;
                        }
                    }
                });
            }

        });
    </script>
</body>
</html>
