<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>

    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            $("#T_newpwd").focus();
            $("form").ligerForm();


        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=allchangepwd&empid=" + getparastr("empid") + "&rnd=" + Math.random();
                var postdata = $("form :input").fieldSerialize() + sendtxt;
                $.ajax({
                    url: "../data/hr_employee.ashx", type: "POST",
                    data: postdata,
                    success: function (responseText) {
                        parent.$.ligerDialog.close();
                        alert('修改成功!')
                    },
                    error: function () {
                        alert('修改失败!');
                    }
                });
            }
        }

    </script>
</head>
<body style="margin: 5px 5px 5px 5px">
    <form id="form1" onsubmit="return false">

        <div>
            <table class="bodytable0" border="0" cellpadding="3" cellspacing="1"
                style="background: #fff; width: 280px;">

                <tr>
                    <td height="23" width="70px">
                        <div align="right" style="width: 61px">
                            新密码：
                        </div>
                    </td>
                    <td height="23">
                        <div style="float: left; height: 20px;">
                            <input type="password" id="T_newpwd" name="T_newpwd" ligerui="{width:180}"
                                validate="{required:true,minlength:4,maxlength:25,messages:{required:'请输入新密码'}}" ltype="password" />
                        </div>

                    </td>
                </tr>

                <tr>
                    <td height="23" width="70px">
                        <div align="right" style="width: 62px">确认密码：</div>

                    </td>
                    <td height="23">
                        <div style="float: left; height: 20px;">
                            <input type="password" id="T_confime" name="T_confime" ligerui="{width:180}"
                                validate="{required:true,minlength:4,maxlength:25,equalTo:'#T_newpwd',equalTo:'#T_newpwd',messages:{required:'请再次输入新密码'}}" ltype="password" />
                        </div>

                    </td>
                </tr>

                <tr>
                    <td height="23" width="70px"></td>
                    <td height="23">&nbsp;</td>
                </tr>

            </table>
        </div>
    </form>
</body>
</html>
