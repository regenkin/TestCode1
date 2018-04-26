<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
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

            if (getparastr("id")) {
                loadForm(getparastr("id"));
            }
        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=SysSave&id=" + getparastr("id");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../data/Sys_role.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {

                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_role").val(obj.RoleName);
                    $("#T_RoleOrder").val(obj.RoleSort);
                    $("#T_Descript").val(obj.RoleDscript);

                }
            });
        }
    </script>
    <style type="text/css">
        .style1 {
            text-align: left;
        }
    </style>
    <script type="text/javascript">
        
    </script>
</head>
<body style="margin: 5px 5px 5px 5px">
    <form id="form1" onsubmit="return false">
        <div>
            <table class="bodytable0" align="center" border="0" cellpadding="3" cellspacing="1"
                style="background: #fff; width: 400px;">

                <tr>
                    <td height="23" width="70px" class="style1">
                        <div align="left" style="width: 61px">
                            角色名称：
                        </div>
                    </td>
                    <td height="23" class="style1">
                        <div style="float: left; height: 20px;">
                            <input type="text" id="T_role" name="T_role" ltype="text" ligerui="{width:250}"
                                validate="{required:true,maxlength:25,remote:'../../data/Sys_role.ashx?Action=Exist',messages:{required:'请输入角色名',remote:'角色名已经存在!'}}" />


                        </div>

                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">

                        <div align="left" style="width: 61px">
                            角色排序：
                        </div>
                    </td>
                    <td>
                        <div style="float: left; height: 20px;">
                            <input type="text" id="T_RoleOrder" name="T_RoleOrder" ltype='spinner' ligerui="{type:'int',width:250}" value="" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">

                        <div align="left" style="width: 62px">角色描述：</div>
                    </td>
                    <td>
                        <input type="text" id="T_Descript" name="T_Descript"
                            ligerui="{width:250}" ltype="text" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
