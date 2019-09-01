<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="openid.aspx.cs" Inherits="XorPay.Web.page.openid" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="renderer" content="webkit">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="icon" href="favicon.ico" type="image/x-icon">
    <title>获取OpenId</title>
    <link rel="stylesheet" href="/layui/css/layui.css" />
    <link rel="stylesheet" href="/layui/css/common.css" />
</head>
<body style="background: #fff">

    <div class="container">

        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 10px;">
            <legend>OpenId</legend>
        </fieldset>

        <form class="layui-form layui-form-pane" id="layform" data-url="" action="" lay-filter="layform">

            <blockquote class="layui-elem-quote" id="statusTips">
                <%=open_id %>&nbsp;&nbsp;<span class="layui-text"><a href="/index.aspx">返回首页</a></span>
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

            function layerMsg(title, icon_num, hash) {
                icon_num = icon_num || 0;
                layer.msg(title, { icon: icon_num, time: 1000, shade: 0.3 }, function () {
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
