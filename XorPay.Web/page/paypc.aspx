<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="paypc.aspx.cs" Inherits="XorPay.Web.page.paypc" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="renderer" content="webkit">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link rel="icon" href="favicon.ico" type="image/x-icon">
    <title>扫码支付</title>
    <link rel="stylesheet" href="/layui/css/layui.css" />
    <link rel="stylesheet" href="/layui/css/common.css" />
</head>
<body style="background: #fff">

    <div class="container">

        <fieldset class="layui-elem-field layui-field-title" style="margin-top: 10px;">
            <legend>扫码支付（<%=pay_text %>）</legend>
        </fieldset>

        <div id="imgDiv" style="display: none; text-align: center">
            <div id="imgCanvas"></div>
            <blockquote class="layui-elem-quote" id="pay_info">
                提示：打开<%=pay_text %>扫码体验支付流程
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
                    <input type="hidden" name="pay_type" value="<%=pay_type %>" />
                    <button class="layui-btn layui-btn-normal layui-btn-fluid" lay-submit="" lay-filter="laysub">立即提交</button>
                </div>
            </div>
        </form>
    </div>

    <script type="text/javascript" src="/layui/layui.js"></script>
    <script type="text/javascript" src="/layui/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="/layui/jquery-qrcode.min.js"></script>
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

                $.post('/page/process.ashx?action=paypc', data.field, function (res) {
                    console.log(res);
                    var info = eval('(' + res + ')');
                    if (info.status == 1) {
                        $("#layform").hide().siblings("#imgDiv").show();
                        createCode("#imgCanvas", info.data);

                        //轮询获取订单支付状态，每隔5S查询一次
                        var interval = setInterval(function () {
                            $.get('/page/process.ashx?action=queryOrerNo', {
                                "order_no": data.field.order_id
                            }, function (get_res) {
                                var result = eval('(' + get_res + ')');
                                if (result && result.status == 1) {
                                    //支付成功
                                    $("#pay_info").html('<b style="color:#38b44a">' + result.msg + '</b>');

                                    clearInterval(interval);
                                }
                            });
                        }, 5000);

                    }
                    else {
                        layerMsg(info.msg, 5, function () {
                            return false;
                        }, 3000);
                    }

                });

                return false;
            });

            function createCode(obj, data) {
                $(obj).html("");
                $(obj).qrcode({
                    // render method: 'canvas', 'image' or 'div'
                    render: 'image',

                    // version range somewhere in 1 .. 40
                    minVersion: 1,
                    maxVersion: 40,

                    // error correction level: 'L', 'M', 'Q' or 'H'
                    ecLevel: 'H',

                    // offset in pixel if drawn onto existing canvas
                    left: 0,
                    top: 0,

                    // size in pixel
                    size: 300,

                    // code color or image element
                    fill: '#000',

                    // background color or image element, null for transparent background
                    background: '#fff',

                    // content
                    text: data,

                    // corner radius relative to module width: 0.0 .. 0.5
                    radius: 0.5,

                    // quiet zone in modules
                    quiet: 2,

                    // modes
                    // 0: normal
                    // 1: label strip
                    // 2: label box
                    // 3: image strip
                    // 4: image box
                    mode: 2,

                    mSize: 0.1,
                    mPosX: 0.5,
                    mPosY: 0.5,

                    label: '',
                    fontname: 'sans',
                    fontcolor: '#000',

                    image: null
                });
            }

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
