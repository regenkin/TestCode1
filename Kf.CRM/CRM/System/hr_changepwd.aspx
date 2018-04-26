<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head  >
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

            //$("#T_Contract_name").focus();
            $("form").ligerForm();
        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=changepwd";
                var issave = $("form :input").fieldSerialize() + sendtxt;
                $.ajax({
                    url: "../data/hr_employee.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        if (responseText == "true") {
                            setTimeout(function () { parent.$.ligerDialog.close(); }, 100);
                        }
                        else {
                            $.ligerDialog.error('操作失败！请输入正确的原密码。');
                        }
                    },
                    error: function () {

                    }
                });
            }
        }
    </script>
</head>
<body style="margin:5px 5px 5px 5px">
    <form id="form1" onsubmit="return false">
                        
    <div>
        <table class="bodytable0" border="0" cellpadding="3" cellspacing="1" 
            style="background: #fff; width:280px;">
            
            <tr>
                <td height="23" width="70px" ><div align="right" style="width: 61px">
                    原密码：</div></td>
                <td height="23" >
                        <input type="password" id="T_oldpwd" name="T_oldpwd"    ltype="password" ligerui="{width:180}" 
                            validate="{required:true,minlength:4,maxlength:25,messages:{required:'请输入原密码'}}"  />
                    
                </td>
            </tr>

            <tr>
                <td height="23" width="70px" ><div align="right" style="width: 61px">
                    新密码：</div></td>
                <td height="23" >
                    <div style="float:left; height: 20px;">
                        <input type="password" id="T_newpwd"  name="T_newpwd"   ligerui="{width:180}" 
                             validate="{required:true,minlength:4,maxlength:25,messages:{required:'请输入新密码'}}" ltype="password"  />
                    </div>
                    
                </td>
            </tr>

             <tr>
                <td height="23" width="70px" >
                    <div align="right" style="width: 62px">确认密码：</div>
                    
                 </td>
                <td height="23" >
                    <div style="float:left; height: 20px;">
                        <input type="password" id="T_confime" name="T_confime"  ligerui="{width:180}" 
                             validate="{required:true,minlength:4,maxlength:25,equalTo:'#T_newpwd',equalTo:'#T_newpwd',messages:{required:'请再次输入新密码'}}" ltype="password" />
                    </div>
                    
                </td>
            </tr>

             <tr>
                <td height="23" width="70px" >
                    </td>
                <td height="23" >
                    &nbsp;</td>
            </tr>

             </table>
    </div>
    </form>
</body>
</html>
