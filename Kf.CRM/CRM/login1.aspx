<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3c.org/TR/1999/REC-html401-19991224/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="ie=8" http-equiv="X-UA-Compatible">
    <META http-equiv="content-type" content="text/html; charset=gb2312">
    <title>KFCRM-登录</title>
    <link href="lib/ligerUI/skins/ext/css/ligerui-dialog.css" rel="stylesheet" />
    <script src="lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="JS/jquery.md5.js" type="text/javascript"></script>
    <script src="JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            delCookie("kfcrm_show_wellcome")
            if (getCookie("kfcrm_uid") && getCookie("kfcrm_uid") != null)
                $("#T_uid").val(getCookie("kfcrm_uid"))

            var FromUrl = getQueryStringByName("FromUrl");
            if (!FromUrl) {
                FromUrl = encodeURIComponent("main.aspx");
            }
            $(document).keydown(function (e) {
                if (e.keyCode == 13 ) {
                    dologin();
                }
            });
            $("#login").click(function () {
                dologin();
            });
            $("#reset").click(function () {
                $(":input", "#form1").not(":button,:submit:reset:hidden").val("");
            });
            function dologin() {
                var uid = $("#T_uid").val();
                var pwd = $("#T_pwd").val();
                var validate = $("#T_validate").val();
                if (validate == "") {
                    alert("验证码不能为空！");
                    $("#T_validate").focus();
                    return;
                }
                else if (validate.length != 4) {
                    alert("验证码错误！");
                    $("#T_validate").focus();
                    return;
                }

                if (uid == "") {
                    alert("账号不能为空！");
                    $("#T_uid").focus();
                    return;
                }
                if (pwd == "") {
                    alert("密码不能为空！");
                    $("#T_pwd").focus();
                    return;
                }


                $.ajax({
                    type: 'post', dataType: 'json',
                    url: 'Data/login.ashx',
                    data: [
                    { name: 'Action', value: 'login' },
                    { name: 'username', value: uid },
                    { name: 'password', value: $.md5( pwd )},
                    { name: 'validate', value: validate },
                    { name: 'rnd', value: Math.random() }
                    ],
                    success: function (result) {
                        if (typeof (result) == "number") {
                            switch (result) {
                                case 0:
                                    alert("验证码错误！");
                                    $("#validate").click();
                                    $("#T_validate").val("");
                                    $("#T_validate").focus();
                                    break;
                                case 1:
                                    alert("用户名或密码错误！");
                                    $("#T_pwd").focus();
                                    break;
                                case 2:
                                    SetCookie("kfcrm_uid", uid, 30);
                                    SetCookie("kfcrm_show_wellcome", "1");
                                    location.href = decodeURIComponent(FromUrl);
                                    break;
                                case 3:
                                    alert("账户异常，请联系管理员！");
                                    break;
                                case 4:
                                    alert("账户已限制登录！");
                                    break;
                            }
                        }
                        else {
                            alert('登陆失败,账号或密码有误!');
                            $("#password").focus();
                            return;
                        }
                    },
                    error: function () {
                        $("#validate").click();
                        alert('发生系统错误,请与系统管理员联系!');
                    },
                    beforeSend: function () {
                        $.ligerDialog.waitting("正在登陆中,请稍后...");
                        $("#lgoin").attr("disabled", true);
                    },
                    complete: function () {
                        $.ligerDialog.closeWaitting();
                        $("#login").attr("disabled", false);
                    }
                });
            }
        })
       

    </script>
    <style type="text/css">
        img { border:none;}
        .text { border: #d2e2f2 1px solid; height: 19px; }
        body { BACKGROUND: url(images/login/loginbackground1.jpg) repeat-x; }
        .auto-style1 { font-size: small; }
    </style>
    <script type="text/javascript">
        if (top.location != self.location) top.location = self.location;
    </script>
</head>
<body>
    <form id="form1" name="form1">
        <div style="width: 731px; margin-left: 50px; margin-top: 200px;">
            <table id="__01" width="732" height="388" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td rowspan="6">
                      <a href="http://www.kinfar.net" target="_blank">    <img src="images/logo/logo.png" width="234" alt="KFCRM"/></a></td>
                    <td rowspan="14" style="background: url(' images/login/login_02.gif') no-repeat; width: 200px; height: 279px;"></td>
                    <td colspan="7" width="187" height="54"></td>
                    <td rowspan="6" background="images/login/login_04.gif" width="110" height="134"></td>
                    <td width="1" height="54"></td>
                </tr>
                <tr>
                    <td rowspan="9" width="6" height="180"></td>
                    <td colspan="5" width="163" height="9"></td>
                    <td rowspan="9" width="18" height="180"></td>
                    <td width="1" height="9"></td>
                </tr>
                <tr>
                    <td colspan="5" width="163" height="22">
                        <input id="T_uid" name="T_uid" type="text" style="width: 160px;" class="text" />
                    </td>
                    <td width="1" height="22"></td>
                </tr>
                <tr>
                    <td colspan="5" background="images/login_09.jpg" width="163" height="13"></td>
                    <td width="1" height="13"></td>
                </tr>
                <tr>
                    <td colspan="5" width="163" height="23">
                        <input id="T_pwd" name="T_pwd" type="password" style="width: 160px" class="text" /></td>
                    <td width="1" height="23"></td>
                </tr>
                <tr>
                    <td colspan="5" rowspan="2" width="163" height="14"></td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td rowspan="8" width="234" height="145"></td>
                    <td rowspan="8" width="110" height="145"></td>
                    <td width="1" height="1"></td>
                </tr>
                <tr>
                    <td colspan="3" width="87" height="23">
                        <input id="T_validate" name="T_validte" type="text" style="width: 80px" class="text" /></td>
                    <td colspan="2" background="images/login_15.gif" width="76" height="23">
                        <img id="validate" onclick="this.src=this.src+'?'" src="ValidateCode.aspx" style="cursor: pointer; border: 1px solid #ddd" alt="看不清楚，换一张" title="看不清楚，换一张" />
                    </td>
                    <td width="1" height="23"></td>
                </tr>
                <tr>
                    <td colspan="5" width="163" height="47"></td>
                    <td width="1" height="47"></td>
                </tr>
                <tr>
                    <td colspan="5" width="163" height="29" class="auto-style1">演示用户:admin 密码admin</td>
                    <td width="1" height="29"></td>
                </tr>
                <tr>
                    <td colspan="5" background="images/login_18.gif" width="97" rowspan="3">
                        <input id="login" type="button" value="登录" style="width: 80px; height: 28px;" /></td>
                    <td colspan="2" rowspan="3" background="images/login_19.gif" width="90" style="height: 60px">
                        <input id="reset" type="button" value="重置" style="width: 80px; height: 28px;" /></td>
                    <td width="1" height="3"></td>
                </tr>
                <tr>
                    <td width="1" height="22"></td>
                </tr>
                <tr>
                    <td width="1" height="5"></td>
                </tr>
                <tr>
                    <td colspan="7" width="187" height="15"></td>
                    <td width="1" height="15"></td>
                </tr>
                <tr>
                    <td colspan="10" background="images/login_25.gif" width="731" height="108"></td>
                    <td width="1" height="108"></td>
                </tr>
                <tr>
                    <td width="234" height="1"></td>
                    <td width="200" height="1"></td>
                    <td width="6" height="1"></td>
                    <td width="4" height="1"></td>
                    <td width="51" height="1"></td>
                    <td width="32" height="1"></td>
                    <td width="4" height="1"></td>
                    <td width="72" height="1"></td>
                    <td width="18" height="1"></td>
                    <td width="110" height="1"></td>
                    <td></td>
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
