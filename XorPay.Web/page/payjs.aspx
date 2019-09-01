<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="payjs.aspx.cs" Inherits="XorPay.Web.page.payjs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="renderer" content="webkit">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="icon" href="favicon.ico" type="image/x-icon">
    <title>移动端支付</title>
    <link rel="stylesheet" href="/layui/css/layui.css" />
    <link rel="stylesheet" href="/layui/css/common.css" />
</head>
<body style="background: #fff">

    <div class="container">

        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 10px;">
            <legend>JSAPI 网页支付</legend>
        </fieldset>

        <div id="imgDiv" style="display: none; text-align: center">
            <div id="imgCanvas"></div>
            <blockquote class="layui-elem-quote" id="pay_info">
                提示：JSAPI（在微信客户端打开）体验支付流程
            </blockquote>
        </div>
        <form class="layui-form layui-form-pane" id="layform" data-url="" action="" lay-filter="layform">
            <div class="layui-form-item">
                <label class="layui-form-label">商品名称</label>
                <div class="layui-input-block">
                    <input type="text" name="name" lay-verify="required" value="测试商品" autocomplete="off" placeholder="请输入商品名称" class="layui-input" />
                </div>
            </div>
            <div class="layui-form-item">
                <label class="layui-form-label">商品价格</label>
                <div class="layui-input-block">
                    <input type="text" name="price" lay-verify="required|price" value="1.00" autocomplete="off" placeholder="￥ 大于1的数字,如1.00" class="layui-input" />
                </div>
            </div>

            <div class="layui-form-item">
                <label class="layui-form-label">订单号</label>
                <div class="layui-input-block">
                    <input type="text" name="order_id" lay-verify="required" value="" autocomplete="off" placeholder="请输入唯一订单号" class="layui-input" />
                </div>
            </div>

            <div class="layui-form-item">
                <label class="layui-form-label">通知地址</label>
                <div class="layui-input-block">
                    <input type="text" name="notify_url" lay-verify="required" value="<%=notify_url %>" autocomplete="off" placeholder="notify_url(异步通知地址)" class="layui-input" />
                </div>
            </div>

            <div class="layui-form-item">
                <label class="layui-form-label">订单用户</label>
                <div class="layui-input-block">
                    <input type="text" name="order_uid" value="" autocomplete="off" placeholder="用户标识信息，非必填" class="layui-input" />
                </div>
            </div>

            <div class="layui-form-item">
                <label class="layui-form-label">其他信息</label>
                <div class="layui-input-block">
                    <input type="text" name="more" value="" autocomplete="off" placeholder="订单其他信息，非必填" class="layui-input" />
                </div>
            </div>

            <div class="layui-form-item">
                <div class="layui-input-block" style="margin-left: 0;">
                    <input type="hidden" name="pay_type" value="jsapi" />
                    <input type="hidden" name="openid" value="<%=open_id %>" />
                    <button class="layui-btn layui-btn-normal layui-btn-fluid" lay-submit="" lay-filter="laysub">立即提交</button>
                </div>
            </div>
        </form>
    </div>

    <script type="text/javascript" src="/layui/layui.js"></script>
    <script>
        layui.use(['layer', 'form', 'jquery'], function () {
            var layer = layui.layer
                , form = layui.form
                , $ = layui.jquery
                ;


            $("input[name=price]").on("blur", function () {
                var this_val = $(this).val() || '';
                if (this_val != '') {
                    if (!new RegExp(/^([1-9]\d*|0)(\.\d{1,2})?$/).test(this_val)) {
                        $(this).val('');
                    } else if (parseFloat(this_val || 0) < 1) {
                        $(this).val('');
                    }
                }
            });

            $("input[name=order_id]").val("B" + randomNumber());

            /* 监听提交 */
            form.on('submit(laysub)', function (data) {

                $.post('/page/process.ashx?action=payjs', data.field, function (res) {
                    console.log(res);
                    var info = eval('(' + res + ')');
                    if (info.status == 1) {

                        if (info.data && info.data != '') {
                            parent.location.href = '/page/jsapipay.aspx?order_no=' + info.data;
                        }

                    }
                    else {
                        layerMsg(info.msg, 5, function () {
                            return false;
                        }, 3000);
                    }

                });

                return false;
            });

            function randomNumber() {
                const now = new Date()
                let month = (now.getMonth() + 1) > 9 ? (now.getMonth() + 1) : "0" + (now.getMonth() + 1);
                let day = now.getDate() > 9 ? now.getDate() : "0" + now.getDate();
                let hour = now.getHours() > 9 ? now.getHours() : "0" + now.getHours();
                let minutes = now.getMinutes() > 9 ? now.getMinutes() : "0" + now.getMinutes();
                let seconds = now.getSeconds()
                return now.getFullYear().toString() + month.toString() + day + hour + minutes + seconds + (Math.round(Math.random() * 89 + 100)).toString()
            }

            function layerMsg(title, icon_num, hash, _time) {
                icon_num = icon_num || 0;
                layer.msg(title, { icon: icon_num, time: _time || 1000, shade: 0.3, shadeClose: true }, function () {
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
