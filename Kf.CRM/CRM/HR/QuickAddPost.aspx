<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js"></script>

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

            $("#T_depname").ligerComboBox({
                width: 150,
                selectBoxWidth: 150,
                selectBoxHeight: 150,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                initValue: getparastr("depid"),
                readonly: false,
                tree: {
                    url: '../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',                    
                    checkbox: false
                }
            })


            $("#T_position").ligerComboBox({
                width: 150,
                selectBoxWidth: 150,
                selectBoxHeight: 150,
                valueField: 'id',
                textField: 'text',
                url: "../data/hr_position.ashx?Action=combo",
                onSelected: positionselected
            });
        });

        function positionselected(newval) {
            //alert(newval);
            //$("#T_position_leavel").val(newval);
            $.ajax({
                type: "GET",
                url: "../data/hr_position.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'getlevel', position_id: newval, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                success: function (result) {
                    $("#T_position_leavel").val(result);
                }
            });
        }

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&T_emp=&T_emp_val=&postid=" + getparastr("postid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }


    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">

        <table align="left" border="0" cellpadding="3" cellspacing="1" style="width: 462px;">

            <tr>
                <td>
                    <div align="left" style="width: 60px">岗位名称：</div>
                </td>
                <td>
                    <input type='text' id="T_postname" name="T_postname" ltype="text" ligerui="{width:150}" validate="{required:true}" /></td>
                <td>
                    <div align="left" style="width: 60px">部门名称：</div>
                </td>
                <td>
                    <input type="text" id="T_depname" name="T_depname" validate="{required:true}" />
                </td>
            </tr>
            <tr>
                <td>
                    <div align="left" style="width: 60px">岗位级别：</div>
                </td>
                <td>
                    <input type="text" id="T_position" name="T_position" validate="{required:true}" /></td>
                <td>
                    <div align="left" style="width: 60px">级别排序：</div>
                </td>
                <td>
                    <input type='text' id="T_position_leavel" name="T_position_leavel" ltype='text' ligerui="{width:150,disabled:true}" value="20" validate="{required:true}" /></td>
            </tr>
            
            <tr>
                <td style="vertical-align: top">
                    <div align="left" style="width: 54px">描述：</div>
                </td>
                <td colspan="3">
                    <input type="text" id="T_descript" name="T_descript" ltype="text" ligerui="{width:390}" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
