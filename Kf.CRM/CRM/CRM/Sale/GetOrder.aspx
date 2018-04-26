<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title></title>
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
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            if (getparastr("orderid")) {
                loadForm(getparastr("orderid"));
            }
            
            $("#maingrid4").ligerGrid({
                columns: [
                        //{ display: '���', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                        { display: '��Ʒ��', name: 'product_name', width: 120 },
                        {
                            display: '����', name: 'price', width: 80, type: 'float',align:'right', render: function (item) {
                                return toMoney(item.price);
                            }
                        },
                        { display: '����', name: 'quantity', width: 40, type: 'int' },
                        { display: '��λ', name: 'unit', width: 40 },
                        {
                            display: '�ܼ�', name: 'amount', width: 100, type: 'float',align:'right', render: function (item) {
                                return toMoney(item.amount) + "Ԫ";
                            }
                        }

                ],
                //selectRowButtonOnly: true,
                usePager: false,
                checkbox: true,
                url: "../../data/Crm_order_details.ashx?Action=grid&orderid=" + getparastr("orderid"),
                width: '456px', height: '150px',
                heightDiff: 0
            })

        });

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../../data/Crm_order.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                data: { Action: 'form', orderid: oaid, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {                        
                        if (obj[n] == null || obj[n] == "null" || obj[n] == undefined)
                            obj[n] = "";
                    }
                    //alert(UrlDecode("2013%2F4%2F14%200%3A00%3A00"));
                    //alert(obj.constructor); //String ���캯��
                    $("#T_Customer").val(obj.Customer_name);

                    $("#T_date").val(UrlDecode(obj.Order_date).split(" ")[0]);
                    $("#T_details").val(obj.Order_details);
                    $("#T_amount").val(toMoney( obj.Order_amount));

                    $("#T_department").val(obj.C_dep_name);
                    $("#T_employee").val(obj.C_emp_name);
                    $("#T_department1").val(obj.F_dep_name)
                    $("#T_employee1").val(obj.F_emp_name);
                    $("#T_status").val(obj.Order_status);
                    $("#T_paytype").val(obj.pay_type);
                }
            });
        }
    </script>

</head>
<body style="margin: 0">
    <form id="form1" onsubmit="return false">
        <div>
            <table style="width: 600px; margin: 5px;" class='bodytable1'>
                <tr>
                    <td colspan="4" class="table_title1">������Ϣ</td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        �ͻ���
                                    </div>
                                </td>
                                <td colspan="3">
                                    <input type="text" id="T_Customer" name="T_Customer"  ltype="text" ligerui="{width:457,disabled:true}"  /></td>
                            </tr>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        �ͻ�������
                                    </div>
                                </td>
                                <td>
                                    <div style="width: 100px; float: left">
                                        <input id="T_department" name="T_department" type="text" ltype="text" ligerui="{width:97,disabled:true}" style="width: 97px" />
                                    </div>
                                    <div style="width: 98px; float: left">
                                        <input id="T_employee" name="T_employee" type="text" ltype="text" ligerui="{width:96,disabled:true}"  style="width: 96px" />
                                    </div>
                                </td>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        �ɽ�ʱ�䣺
                                    </div>
                                </td>
                                <td>
                                    <input type="text" id="T_date" name="T_date" ltype="text"   ligerui="{width:182,disabled:true}" /></td>
                            </tr>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        �ٳ���Ա��
                                    </div>
                                </td>
                                <td>
                                    <div style="width: 100px; float: left">
                                        <input id="T_department1" name="T_department1" type="text" ltype="text"   ligerui="{width:97,disabled:true}" style="width: 97px" />
                                    </div>
                                    <div style="width: 98px; float: left">
                                        <input id="T_employee1" name="T_employee1" type="text" ltype="text"  ligerui="{width:96,disabled:true}"  style="width: 96px" />
                                    </div>
                                </td>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        ����״̬��
                                    </div>
                                </td>
                                <td>

                                    <input type="text" id="T_status" name="T_status" ltype="text"  ligerui="{width:182,disabled:true}" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <div align="right" style="width: 62px">�������飺</div>
                                </td>
                                <td colspan="3">
                                    <textarea cols="100" id="T_details" name="T_details" rows="3" class="l-textarea" style="width: 455px"></textarea></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="table_title1">������Ʒ</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table>
                            <tr>
                                <td>
                                    <div align="right" style="width: 62px">��Ʒ��</div>
                                </td>
                                <td colspan="3">
                                    <div id="maingrid4" style="margin: -1px;"></div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div align="right" style="width: 62px">֧����ʽ��</div>
                                </td>
                                <td>
                                    <input type="text" id="T_paytype" name="T_paytype" ltype="text"   ligerui="{width:182,disabled:true}"/>
                                </td>
                                <td>
                                    <div align="right" style="width: 62px">��</div>
                                </td>
                                <td>
                                    <input type="text" id="T_amount" name="T_amount" ltype="text" style="text-align:right" value="0"  ligerui="{width:182,disabled:true}" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>






        </div>
    </form>

</body>
</html>
