<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="unionurl.aspx.cs" Inherits="XorPay.Web.unionurl" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="icon" href="favicon.ico" type="image/x-icon">
    <title>统一支付</title>
    <link rel="stylesheet" href="/layui/css/layui.css" />
    <script>
        var qr_url = '<%=qr%>';
        if (qr_url && qr_url != '') {
            location.href = qr_url;
        }
    </script>
</head>
<body>
    <script type="text/javascript" src="/layui/layui.js"></script>
    <script>
        layui.use(['layer'], function () {
            var layer = layui.layer;
            var index = layer.load(0, { shade: false });
        });
    </script>
</body>
</html>
