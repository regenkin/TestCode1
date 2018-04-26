<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title></title>
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            

            $("#T_product_category").ligerComboBox({
                width: 150,
                selectBoxWidth: 150,
                selectBoxHeight: 150,
                valueField: 'id',
                textField: 'text',
                initValue: getparastr("categoryid"),
                treeLeafOnly: false,
                tree: {
                    url: '../../data/crm_product_category.ashx?Action=tree&rnd=' + Math.random(),
                    //onSelect: onSelect,
                    idFieldName: 'id',
                    valueField: 'text',
                    usericon: 'd_icon',
                    checkbox: false,
                    itemopen: false
                }
            });

            if (getparastr("pid")) {
                loadForm(getparastr("pid"));
            }
        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&pid=" + getparastr("pid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "get",
                url: "../../data/Crm_product.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', pid: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_product_name").val(obj.product_name);
                    $("#T_product_unit").val(obj.unit);
                    $("#T_specifications").val(obj.specifications);
                    $("#T_remarks").val(obj.remarks);
                    $("#T_price").val(toMoney(obj.price));

                    $("#T_product_category").ligerGetComboBoxManager().selectValue(obj.category_id);

                }
            });
        }

        function set_tomoney(value) {
            $("#T_price").val(toMoney(value));
        }
       


    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">     
        <table align="left" border="0" cellpadding="3" cellspacing="1" style="width: 462px;">
            <tr>
                <td>
                    <div align="left" style="width:60px">产品名称：</div>
                </td>
                <td colspan="3">
                    <input type='text' id="T_product_name" name="T_product_name" ltype="text" ligerui="{width:390}" validate="{required:true}" /></td>
            </tr>

            <tr>
                <td>
                    <div align="left" style="width: 60px">产品类别：</div>
                </td>
                <td>
                    <input type="text" id="T_product_category" name="T_product_category" validate="{required:true}"  ligerui="{width:390}" /></td>
                <td>
                    <div align="left" style="width: 60px">单位：</div>
                </td>
                <td>
                    <input type='text' id="T_product_unit" name="T_product_unit" ltype='text' ligerui="{width:160}" /></td>
            </tr>

            <tr>
                <td>
                    <div align="left" style="width: 60px">价格(元)：</div>
                </td>
                <td>
                    <input type="text" id="T_price" name="T_price" value="0.00" ltype='text' onchange="set_tomoney(this.value)" style="text-align:right" ligerui="{width:150,number:true}" validate="{required:true}" /></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>

            <tr>
                <td>
                    <div align="left" style="width: 60px">产品规格：</div>
                </td>
                <td colspan="3">
                    <input type='text' id="T_specifications" name="T_specifications" ltype="text" ligerui="{width:390}" /></td>
            </tr>

            <tr>
                <td style="vertical-align: top">
                    <div align="left" style="width: 54px">备注：</div>
                </td>
                <td colspan="3">
                    <textarea cols="100" id="T_remarks" name="T_remarks" rows="3" class="l-textarea" style="width: 388px"></textarea></td>
            </tr>

        </table>
    </form>
</body>
</html>
