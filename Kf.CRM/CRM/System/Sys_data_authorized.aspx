<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=8" />

    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../lib/json.js" type="text/javascript"></script>

    <script type="text/javascript">

        $(function () {
            $.metadata.setType("attr", "validate");
            KFCRM.validate($("#form1"));

            initSelect(); //初始化

            loadform(getparastr("rid")); //加载
        })

        function loadform(role_id) {
            //../data/Sys_data_authority.ashx?Action=get&Role_id=' + getparastr("rid")
            $.ajax({
                type: "POST", dataType: 'json',
                url: "../data/Sys_data_authority.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: "get", Role_id: role_id, rnd: Math.random() }, /* 注意参数的格式和名称 */
                success: function (result) {
                    var data = result["Rows"];
                    $("#customer_view").ligerGetComboBoxManager().selectValue(data[0].Sys_view);
                    $("#customer_add").ligerGetComboBoxManager().selectValue(data[0].Sys_add);
                    $("#customer_del").ligerGetComboBoxManager().selectValue(data[0].Sys_del);

                    $("#follow_view").ligerGetComboBoxManager().selectValue(data[1].Sys_view);
                    $("#follow_del").ligerGetComboBoxManager().selectValue(data[1].Sys_del);

                    $("#order_view").ligerGetComboBoxManager().selectValue(data[2].Sys_view);
                    $("#order_del").ligerGetComboBoxManager().selectValue(data[2].Sys_del);

                    $("#contract_view").ligerGetComboBoxManager().selectValue(data[3].Sys_view);
                    $("#contract_del").ligerGetComboBoxManager().selectValue(data[3].Sys_del);
                }
            });
        }

        var data1 = [{ id: 1, text: '本人' }, { id: 2, text: '本部' }, { id: 3, text: '本部及下级' }, { id: 4, text: '全部' }];
        var data2 = [{ id: 1, text: '本人' }, { id: 2, text: '本部' }, { id: 3, text: '本部及下级' }];
        var data3 = [{ id: 1, text: '本人' }, { id: 2, text: '本部' }];
        var data4 = [{ id: 1, text: '本人' }];

        var a, b, c, d, e;
        function initSelect() {
            a = $("#customer_add").ligerComboBox({ width: 100 });
            b = $("#customer_del").ligerComboBox({ width: 100 });
            c = $("#follow_del").ligerComboBox({ width: 100 });
            d = $("#order_del").ligerComboBox({ width: 100 });
            e = $("#contract_del").ligerComboBox({ width: 100 });

            $("#customer_view").ligerComboBox({
                width: 100,
                data: data1,
                initValue: 1,
                onSelected: function (newvalue, newtext, newid) {
                    if (newvalue == 1) {
                        a.setData(data4); a.selectValue(1);
                        b.setData(data4); b.selectValue(1);

                    }
                    else if (newvalue == 2) {
                        a.setData(data3); a.selectValue(2);
                        b.setData(data3); b.selectValue(2);
                    }
                    else if (newvalue == 3) {
                        a.setData(data2); a.selectValue(3);
                        b.setData(data2); b.selectValue(3);
                    }
                    else {
                        a.setData(data1); a.selectValue(4);
                        b.setData(data1); b.selectValue(4);
                    }

                }
            });
            $("#follow_view").ligerComboBox({
                width: 100,
                data: data1,
                initValue: 1,
                onSelected: function (newvalue, newtext, newid) {
                    if (newvalue == 1) {
                        c.setData(data4); c.selectValue(1);
                    }
                    else if (newvalue == 2) {
                        c.setData(data3); c.selectValue(2);
                    }
                    else if (newvalue == 3) {
                        c.setData(data2); c.selectValue(3);
                    }
                    else {
                        c.setData(data1); c.selectValue(4);
                    }

                }
            });
            $("#order_view").ligerComboBox({
                width: 100,
                data: data1,
                initValue: 1,
                onSelected: function (newvalue, newtext, newid) {
                    if (newvalue == 1) {
                        d.setData(data4); d.selectValue(1);
                    }
                    else if (newvalue == 2) {
                        d.setData(data3); d.selectValue(2);
                    }
                    else if (newvalue == 3) {
                        d.setData(data2); d.selectValue(3);
                    }
                    else if (newvalue == 4) {
                        d.setData(data1); d.selectValue(4);
                    }

                }
            });
            $("#contract_view").ligerComboBox({
                width: 100,
                data: data1,
                initValue: 1,
                onSelected: function (newvalue, newtext, newid) {
                    if (newvalue == 1) {
                        e.setData(data4); e.selectValue(1);
                    }
                    else if (newvalue == 2) {
                        e.setData(data3); e.selectValue(2);
                    }
                    else if (newvalue == 3) {
                        e.setData(data2); e.selectValue(3);
                    }
                    else{
                        e.setData(data1); e.selectValue(4);
                    }

                }
            });
        }
        function f_save() {
            if ($("#form1").valid()) {
                var str1 = getparastr("rid");

                var postdata = [];
                postdata.push({
                    option_id: 1,
                    Sys_option: "客户管理",
                    Sys_view: $("#customer_view_val").val(),
                    Sys_add: $("#customer_add_val").val(),
                    Sys_edit: "4",
                    Sys_del: $("#customer_del_val").val()
                });
                postdata.push({
                    option_id: 2,
                    Sys_option: "跟进管理",
                    Sys_view: $("#follow_view_val").val(),
                    Sys_add: 1,
                    Sys_edit: "4",
                    Sys_del: $("#follow_del_val").val()
                });
                postdata.push({
                    option_id: 3,
                    Sys_option: "订单管理",
                    Sys_view: $("#order_view_val").val(),
                    Sys_add: $("#order_view_val").val(),
                    Sys_edit: "4",
                    Sys_del: $("#order_del_val").val()
                });
                postdata.push({
                    option_id: 4,
                    Sys_option: "合同管理",
                    Sys_view: $("#contract_view_val").val(),
                    Sys_add: $("#contract_view_val").val(),
                    Sys_edit: "4",
                    Sys_del: $("#contract_del_val").val()
                });

                //alert(JSON.stringify(postdata));

                $.ajax({
                    type: "POST",
                    url: "../data/Sys_data_authority.ashx", /* 注意后面的名字对应CS的方法名称 */
                    data: { Action: "save", rid: str1, savestring: JSON.stringify(postdata) }, /* 注意参数的格式和名称 */
                    success: function (result) {
                        setTimeout(function () { parent.$.ligerDialog.close(); }, 100);
                    }
                });
            }
        }

    </script>



</head>
<body>
    <form id="form1" onsubmit="return false">
        <div>
            <table style="width: 500px; margin: 5px;" class="bodytable0">
                <tr>
                    <td class="l-messagebox-btn-inner">权限名称</td>
                    <td class="l-messagebox-btn-inner" style="width: 25%;">查看</td>
                    <td class="l-messagebox-btn-inner" style="width: 25%;">新增</td>
                    <td class="l-messagebox-btn-inner" style="width: 25%;">删除</td>
                </tr>
                <tr>
                    <td class="l-messagebox-btn-inner">客户管理</td>
                    <td class="l-messagebox-btn-inner">
                        <input type="text" id="customer_view" name="customer_view"></td>
                    <td class="l-messagebox-btn-inner">
                        <input type="text" id="customer_add" name="customer_add"></td>
                    <td class="l-messagebox-btn-inner">
                        <input type="text" id="customer_del" name="customer_del"></td>
                </tr>
                <tr>
                    <td class="l-messagebox-btn-inner" style="width: 90px;">跟进管理</td>
                    <td class="l-messagebox-btn-inner">
                        <input type="text" id="follow_view" name="follow_view"></td>
                    <td class="l-messagebox-btn-inner">本人</td>
                    <td class="l-messagebox-btn-inner">
                        <input type="text" id="follow_del" name="follow_del"></td>
                </tr>
                <tr>
                    <td class="l-messagebox-btn-inner" style="width: 90px;">订单管理</td>
                    <td class="l-messagebox-btn-inner">
                        <input type="text" id="order_view" name="order_view"></td>
                    <td class="l-messagebox-btn-inner">同查看</td>
                    <td class="l-messagebox-btn-inner">
                        <input type="text" id="order_del" name="order_del"></td>
                </tr>
                <tr>
                    <td class="l-messagebox-btn-inner">合同管理</td>
                    <td class="l-messagebox-btn-inner">
                        <input type="text" id="contract_view" name="contract_view"></td>
                    <td class="l-messagebox-btn-inner">同查看</td>
                    <td class="l-messagebox-btn-inner">
                        <input type="text" id="contract_del" name="contract_del"></td>
                </tr>
            </table>

        </div>
    </form>



</body>
</html>
