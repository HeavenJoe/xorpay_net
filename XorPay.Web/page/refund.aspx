<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="refund.aspx.cs" Inherits="XorPay.Web.page.refund" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="renderer" content="webkit">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="icon" href="favicon.ico" type="image/x-icon">
    <title>订单退款</title>
    <link rel="stylesheet" href="/layui/css/layui.css" />
    <link rel="stylesheet" href="/layui/css/common.css" />
</head>
<body style="background: #fff">

    <div class="container">

        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 10px;">
            <legend>订单退款</legend>
        </fieldset>

        <form class="layui-form layui-form-pane" id="layform" data-url="" action="" lay-filter="layform">

            <div class="layui-form-item">
                <label class="layui-form-label">平台订单号</label>
                <div class="layui-input-block">
                    <input type="text" name="aoid" lay-verify="required" value="" autocomplete="off" placeholder="XorPay平台订单号" class="layui-input" />
                </div>
            </div>

            <div class="layui-form-item">
                <label class="layui-form-label">退款金额</label>
                <div class="layui-input-block">
                    <input type="text" name="price" lay-verify="required|price" value="" autocomplete="off" placeholder="￥ 退款金额" class="layui-input" />
                </div>
            </div>

            <div class="layui-form-item">
                <div class="layui-input-block" style="margin-left: 0;">
                    <button class="layui-btn layui-btn-normal layui-btn-fluid" lay-submit="" lay-filter="laysub">提交退款</button>
                </div>
            </div>

            <blockquote class="layui-elem-quote" id="statusTips" style="display: none">
            </blockquote>

        </form>
    </div>

    <script type="text/javascript" src="/layui/layui.js"></script>
    <script>
        layui.use(['layer', 'form', 'jquery'], function () {
            var layer = layui.layer
                , form = layui.form
                , $ = layui.jquery
                ;

            /* 监听提交 */
            form.on('submit(laysub)', function (data) {

                $.post('/page/process.ashx?action=refund', data.field, function (res) {
                    console.log(res);
                    var info = eval('(' + res + ')');
                    if (info.status == 1) {
                        $("#statusTips").html(info.data).show();
                    }
                    else {
                        layerMsg(info.msg, 5, function () {
                            return false;
                        }, 3000);
                    }

                });

                return false;
            });

            $("input[name=price]").on("blur", function () {
                var this_val = $(this).val() || '';
                if (this_val != '') {
                    if (!new RegExp(/^([1-9]\d*|0)(\.\d{1,2})?$/).test(this_val)) {
                        $(this).val('');
                    } else if (parseFloat(this_val || 0) <= 0) {
                        $(this).val('');
                    }
                }
            });

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
