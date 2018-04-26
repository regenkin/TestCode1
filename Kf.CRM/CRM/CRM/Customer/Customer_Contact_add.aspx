<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>

    <script src="../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../JS/Toolbar.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            $("form").ligerForm();

            $('#T_company').ligerComboBox({
                width: 495,
                onBeforeOpen: f_selectContact
            });

            if (getparastr("cid")) {
                loadForm(getparastr("cid"));
            }
            if (getparastr("Customer_id")) {
                $.ajax({
                    type: "GET",
                    url: "../../data/crm_customer.ashx", /* 注意后面的名字对应CS的方法名称 */
                    data: { Action: 'form', cid: getparastr("Customer_id"), rnd: Math.random() }, /* 注意参数的格式和名称 */
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var obj = eval(result);
                        for (var n in obj) {
                            if (obj[n] == null)
                                obj[n] = "";
                        }
                        //alert(obj.constructor); //String 构造函数
                        $("#T_company").val(obj.Customer);
                        $("#T_company_val").val(obj.id);
                        $("#T_company").ligerGetComboBoxManager().setReadOnly();
                    }
                });
            }
        })
        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&contact_id=" + getparastr("cid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../../data/CRM_Contact.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', contact_id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_company_val").val(obj.C_customerid);
                    $("#T_company").val(obj.C_customername);
                    $("#T_contact").val(obj.C_name);
                    if (obj.C_birthday)
                        $("#T_birthday").val(obj.C_birthday.split(" ")[0]);
                    $("#T_dep").val(obj.C_department);
                    $("#T_position").val(obj.C_position);
                    $("#T_tel").val(obj.C_tel);
                    $("#T_mobil").val(obj.C_mob);
                    $("#T_fax").val(obj.C_fax);
                    $("#T_email").val(obj.C_email);
                    $("#T_qq").val(obj.C_QQ);
                    $("#T_add").val(obj.C_add);
                    $("#T_hobby").val(obj.C_hobby);
                    $("#T_remarks").val(obj.C_remarks);

                    $("#T_sex").ligerGetComboBoxManager().selectValue(obj.C_sex);
                }
            });

        }
        function f_selectContact() {
            $.ligerDialog.open({
                title: '选择联系人', width: 650, height: 300, url: 'getcustomer.aspx', buttons: [
                    { text: '确定', onclick: f_selectContactOK },
                    { text: '取消', onclick: f_selectContactCancel }
                ]
            });
            return false;
        }
        function f_selectContactOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择行!');
                return;
            }
            $("#T_company").val(data.Customer);
            $("#T_company_val").val(data.id);
            dialog.close();
        }
        function f_selectContactCancel(item, dialog) {
            dialog.close();
        }
        function remotesite() {
            var url = "../../data/CRM_Customer.ashx?Action=validate&T_cid=" + getparastr("cid") + "&rnd=" + Math.random();
            return url;
        }

    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">

        <table style="width: 600px; margin: 5px;" class="bodytable1">
            <tr>
                <td colspan="4" class="table_title1">基本信息</td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">公司名称：</div>
                </td>
                <td colspan="3">
                    <input type="text" id="T_company" name="T_company" validate="{required:true}" /></td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">联系人：</div>
                </td>
                <td>
                    <div style="width: 140px; float: left">
                        <input id="T_contact" name="T_contact" type="text" ltype="text" ligerui="{width:136}" style="width: 146px" validate="{required:true}" />
                    </div>
                    <div style="width: 58px; float: left">
                        <input type="text" id="T_sex" name="T_sex" style="width: 56px" ltype="select" ligerui="{width:56,data:[{id:'先生',text:'先生'},{id:'女士',text:'女士'}]}" validate="{required:true}" />
                    </div>
                </td>
                <td>
                    <div style="width: 80px; text-align: right; float: right">生日：</div>
                </td>
                <td>
                    <input id="T_birthday" name="T_birthday" type="text" ltype="date" ligerui="{width:196}" /></td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">部门：</div>
                </td>
                <td>
                    <input type="text" id="T_dep" name="T_dep" ltype="text" ligerui="{width:196}" /></td>
                <td>
                    <div style="width: 80px; text-align: right; float: right">职务：</div>
                </td>
                <td>
                    <input type="text" id="T_position" name="T_position" ltype="text" ligerui="{width:196}" />
                </td>
            </tr>
            <tr>
                <td class="table_title1" colspan="4">联系方式</td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">电话：</div>
                </td>
                <td>
                    <input id="T_tel" name="T_tel" type="text" ltype="text" ligerui="{width:196}" />
                </td>
                <td>
                    <div style="width: 80px; text-align: right; float: right">手机：</div>
                </td>
                <td>
                    <input id="T_mobil" name="T_mobil" type="text" ltype="text" ligerui="{width:196}" />
                </td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">传真：</div>
                </td>
                <td>
                    <input id="T_fax" name="T_fax" type="text" ltype="text" ligerui="{width:196}" /></td>
                <td>
                    <div style="width: 80px; text-align: right; float: right">Email：</div>
                </td>
                <td>
                    <input id="T_email" name="T_email" type="text" ltype="text" ligerui="{width:196}" validate="{required:false,email:true}" /></td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">QQ：</div>
                </td>
                <td>
                    <input id="T_qq" name="T_qq" type="text" ltype="text" ligerui="{width:196}" validate="{required:false ,digits:true}" /></td>
                <td>&nbsp;</td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">地址：</div>
                </td>
                <td colspan="3">
                    <input id="T_add" name="T_add" type="text" ltype="text" ligerui="{width:495}" /></td>
            </tr>
            <tr>
                <td class="table_title1" colspan="4">其他</td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">爱好：</div>
                </td>
                <td colspan="3">

                    <input id="T_hobby" name="T_hobby" type="text" ltype="text" ligerui="{width:495}" /></td>
    
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">备注：</div>
                </td>
                <td colspan="3">
                    <input id="T_remarks" name="T_remarks" type="text" ltype="text" ligerui="{width:495}" /></td>
            </tr>

        </table>

    </form>
</body>
</html>
