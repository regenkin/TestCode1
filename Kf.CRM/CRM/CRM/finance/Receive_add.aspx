<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../../CSS/input.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />

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

    <script src="../../lib/json2.js" type="text/javascript"></script>

    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            if (getparastr("orderid")) {
                loadorder(getparastr("orderid"));
            }
            else {
                alert("系统无法在没有订单的情况下收款！");
                top.jQuery.ligerDialog.close();
            }
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();
            $('#T_employee').ligerComboBox({ width: 182, onBeforeOpen: f_selectContact });

            loadForm(getparastr("receiveid"));
        });


        function loadorder(orderid) {
            $.ajax({
                type: "GET",
                url: "../../data/Crm_order.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', orderid: orderid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_Customer").val(obj.Customer_name);
                    $("#T_Customer_val").val(obj.Customer_id);

                    $("#T_department0").val(obj.C_dep_name);
                    $("#T_employee0").val(obj.C_emp_name);

                    $("#T_order_amount").val(toMoney(obj.Order_amount));
                    $("#T_receive_amount").val(toMoney(obj.receive_money));
                    $("#T_arrears_amount").val(toMoney(obj.arrears_money));
                    $("#T_invoice_money").val(toMoney(obj.invoice_money));
                    $("#T_arrears_invoice").val(toMoney(obj.arrears_invoice));
                }
            });
        }
        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../../data/CRM_receive.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', receiveid: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String 构造函数                    
                    $("#T_invoice_amount").val(toMoney(obj.receive_real));
                    $("#T_invoice_num").val(obj.Receive_num);

                    $("#T_invoice_date").val(formatTimebytype(obj.Receive_date, "yyyy-MM-dd"));
                    $("#T_content").val(obj.remarks);

                    $("#T_receive_direction").ligerGetComboBoxManager().selectValue(obj.receive_direction_id);

                    if (obj.C_depid && obj.C_empid) {
                        fillemp(obj.C_depname, obj.C_depid, obj.C_empname, obj.C_empid);
                    }
                    else {
                        $.getJSON("../../data/hr_employee.ashx?Action=form&id=epu&rnd=" + Math.random(), function (result) {
                            var obj = eval(result);
                            for (var n in obj) {
                                if (obj[n] == null)
                                    obj[n] = "";
                            }
                            fillemp(obj.dname, obj.d_id, obj.name, obj.ID)
                        })
                    }

                    $('#T_invoice_type').ligerComboBox({ width: 182, initValue: obj.Pay_type_id, url: "../../data/Param_SysParam.ashx?Action=combo&parentid=5&rnd=" + Math.random() });
                }
            });
        }

        function set_tomoney(value) {
            $("#T_invoice_amount").val(toMoney(value));
        }

        function f_save() {
            if ($(form1).valid()) {
                if (parseFloat($("#T_invoice_amount").val().replace(/\$|\,/g, '')) > parseFloat($("#T_arrears_amount").val().replace(/\$|\,/g, '')) && $("#T_receive_direction").val() == "收款") {
                    alert('您的收款金额已经大于未收金额！');
                }
                var sendtxt = "&Action=save&receiveid=" + getparastr("receiveid") + "&orderid=" + getparastr("orderid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }
        function f_selectContact() {
            top.$.ligerDialog.open({
                zindex: 9003,
                title: '选择员工', width: 850, height: 400, url: "hr/Getemp_Auth.aspx?auth=0", buttons: [
                    { text: '确定', onclick: f_selectContactOK },
                    { text: '取消', onclick: f_selectContactCancel }
                ]
            });
            return false;
        }
        function f_selectContactOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择员工!');
                return;
            }
            fillemp(data.dname, data.d_id, data.name, data.ID);
            dialog.close();
        }
        function fillemp(dep, depid, emp, empid) {
            $("#T_employee").val("【" + dep + "】" + emp);
            $("#T_employee1").val(emp);
            $("#T_employee_val").val(empid);
            $("#T_dep").val(dep);
            $("#T_dep_val").val(depid);
        }
        function f_selectContactCancel(item, dialog) {
            dialog.close();
        }
    </script>

</head>
<body style="margin: 0">
    <form id="form1" onsubmit="return false">
        <div>
            <table style="width: 550px; margin: 5px;" class='bodytable1'>
                <tr>
                    <td class="table_title1" colspan="4">订单信息</td>
                </tr>

                <tr>
                    <td width="62px">
                        <div align="right" style="width: 61px">
                            客户：
                        </div>
                    </td>
                    <td colspan="3">
                        <input type="text" id="T_Customer" name="T_Customer" ltype="text" ligerui="{width: 450,disabled:true}" style="width: 452px;" /></td>
                </tr>
                <tr>
                    <td width="62px">
                        <div align="right" style="width: 61px">
                            应收金额：
                        </div>
                    </td>
                    <td>
                        <input type="text" id="T_order_amount" name="T_order_amount" ltype="text" ligerui="{width:182,disabled:true}" style="width: 452px; text-align: right" />
                    </td>
                    <td width="62px">
                        <div align="right" style="width: 61px">
                            已开票额：
                        </div>
                    </td>
                    <td>
                        <input type="text" id="T_invoice_money" name="T_invoice_money" style="text-align: right" ltype="text" ligerui="{width:182,disabled: true}" /></td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 61px">
                            已收金额：
                        </div>
                    </td>
                    <td>
                        <input type="text" id="T_receive_amount" name="T_receive_amount" style="text-align: right" ltype="text" ligerui="{width:182,disabled:true}" />
                    </td>
                    <td>
                        <div align="right" style="width: 61px">
                            未开票额：
                        </div>
                    </td>
                    <td>
                        <input type="text" id="T_arrears_invoice" name="T_receive_invoice" style="text-align: right" ltype="text" ligerui="{width:182,disabled:true}" /></td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 61px">
                            未收金额：
                        </div>
                    </td>
                    <td>

                        <input type="text" id="T_arrears_amount" name="T_arrears_amount" style="text-align: right" ltype="text" ligerui="{width:182,disabled:true}" />

                    </td>
                    <td>
                        <div align="right" style="width: 61px">
                            订单归属：
                        </div>
                    </td>
                    <td>
                        <div style="width: 100px; float: left">
                            <input id="T_department0" name="T_department0" type="text" ltype="text" ligerui="{width:96,disabled:true}" style="width: 97px" />
                        </div>
                        <div style="width: 98px; float: left">
                            <input id="T_employee0" name="T_employee0" type="text" ltype="text" ligerui="{width:82,disabled:true}" style="width: 96px" />
                        </div>
                    </td>
                </tr>

                <tr>
                    <td class="table_title1" colspan="4">收款信息</td>
                </tr>

                <tr>
                    <td width="62px">
                        <div align="right" style="width: 61px">
                            收款金额：
                        </div>
                    </td>
                    <td>
                        <input type="text" id="T_invoice_amount" name="T_invoice_amount" style="text-align: right" ltype="text" ligerui="{width:182,number: true}" onchange="set_tomoney(this.value)" validate="{required:true}" />
                    </td>

                    <td>
                        <div align="left" style="width: 60px">收款类别：</div>
                    </td>
                    <td>
                        <div style="width: 182px; float: left">
                            <input type="text" id="T_receive_direction" name="T_receive_direction" ltype="select" ligerui="{width:182,data:[{id:1,text:'收款'},{id:-1,text:'退款'}]}" validate="{required:true}" />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td width="62px">
                        <div align="right" style="width: 61px">
                            付款方式：
                        </div>
                    </td>
                    <td>
                        <input type="text" id="T_invoice_type" name="T_invoice_type" validate="{required:true}" ligerui="{width:182}" />

                    </td>

                    <td>
                        <div align="right" style="width: 61px">
                            凭证号码：
                        </div>
                    </td>
                    <td>
                        <input type="text" id="T_invoice_num" name="T_invoice_num" ltype="text" ligerui="{width:182}" validate="{required:true}" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <div align="right" style="width: 61px">
                            收款时间：
                        </div>
                    </td>
                    <td>
                        <input type="text" id="T_invoice_date" name="T_invoice_date" ltype="date" validate="{required:true}" ligerui="{width:182}" />

                    </td>


                    <td>
                        <div align="right" style="width: 61px">
                            收款人：
                        </div>
                    </td>
                    <td>

                        <input id="T_employee" name="T_employee" type="text" validate="{required:true}" />
                        <input id="T_employee1" name="T_employee1" type="hidden" />
                        <input id="T_dep_val" name="T_dep_val" type="hidden" />
                        <input id="T_dep" name="T_dep" type="hidden" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 61px">
                            收款内容：
                        </div>
                    </td>
                    <td colspan="3">

                        <textarea cols="100" id="T_content" name="T_content" rows="3" class="l-textarea" style="width: 448px" validate="{required:true}"></textarea></td>
                </tr>

                <tr>
                    <td width="62px">&nbsp;</td>
                    <td colspan="3">&nbsp;</td>
                </tr>

            </table>






        </div>
    </form>

</body>
</html>
