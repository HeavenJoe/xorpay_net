<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="query.aspx.cs" Inherits="XorPay.Web.page.query" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="renderer" content="webkit">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="icon" href="favicon.ico" type="image/x-icon">
    <title>订单查询</title>
    <link rel="stylesheet" href="/layui/css/layui.css" />
    <link rel="stylesheet" href="/layui/css/common.css" />
</head>
<body style="background: #fff">

    <div class="container">

        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 10px;">
            <legend>订单状态查询</legend>
        </fieldset>

        <form class="layui-form layui-form-pane" id="layform" data-url="" action="" lay-filter="layform">

            <div class="layui-form-item">
                <label class="layui-form-label">平台订单号</label>
                <div class="layui-input-block">
                    <input type="text" name="aoid" lay-verify="required" value="" autocomplete="off" placeholder="XorPay平台订单号" class="layui-input" />
                </div>
            </div>

            <div class="layui-form-item">
                <div class="layui-input-block" style="margin-left: 0;">
                    <button class="layui-btn layui-btn-normal layui-btn-fluid" lay-submit="" lay-filter="laysub">立即查询</button>
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

                $.post('/page/process.ashx?action=query', data.field, function (res) {
                    console.log(res);
                    var info = eval('(' + res + ')');
                    if (info.status == 1) {
                        $("#statusTips").html(info.data).show();
                    }
                    else {
                        layerMsg(info.msg, 5, function () {
                            return false;
                        },2000);
                    }

                });

                return false;
            });

            function layerMsg(title, icon_num, hash, _time) {
                icon_num = icon_num || 0;
                layer.msg(title, { icon: icon_num, time: _time || 1000, shade: 0.3, shadeClose: true}, function () {
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
