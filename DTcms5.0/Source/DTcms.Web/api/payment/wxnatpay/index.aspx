<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="DTcms.Web.api.payment.wxnatpay.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<title>微信扫码支付</title>
<meta http-equiv="content-type" content="text/html;image/gif;charset=utf-8"/>
<meta name="viewport" content="width=device-width, initial-scale=1" />
<script type="text/javascript" charset="utf-8" src="../../../scripts/jquery/jquery-1.11.2.min.js"></script>
<script type="text/javascript">
$(function(){
    sendPost(); //调用监听事件
});
//监听订单支付状态
function sendPost() {
    //发送AJAX请求
    $.ajax({
        url: "return_url.aspx",
        type: "POST",
        timeout: 30000,
        data: { "order_no": "<%=order_no%>" },
        dataType: "json",
        success: function (data, type) {
            if (data.status == 1) {
                $("#tips").show();
                setTimeout(function () {
                    location.href = data.url; //支付成功后跳转
                }, 1000);
            } else {
                time();
            }
        }
    });
}
function time() {
    setTimeout(function () {
        sendPost();
    }, 5000);
}
</script>
<style type="text/css">
body{margin:0;padding:0;}
h2{display:block;text-align:center;line-height:42px;height:42px; font-size:20px;color:#fff;background:#333;}
p{ margin:30px auto; text-align:center; font-family:"Microsoft Yahei";}
p.price{color:#333;font-size:24px;}
#tips{ display:none; }
p span.tip-box{ margin:0 auto; display:inline-block; color:#71b83d; font-size:16px; padding-left:20px; background:url(../../../css/valid_icons.png) no-repeat -40px -20px;}
</style>

</head>

<body>
<form id="form1" runat="server">
<h2>微信扫码支付</h2>
<p><asp:Image ID="imgQRCode" runat="server" style="width:200px;height:200px;"/></p>
<p class="price">￥<asp:Literal ID="litText" runat="server" /></p>
<p id="tips"><span class="tip-box">已支付成功，正在跳转...</span></p>
</form>
</body>
</html>
