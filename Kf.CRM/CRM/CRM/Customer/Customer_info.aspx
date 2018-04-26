<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=8 chrome=1" />

    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/core.css" rel="stylesheet" type="text/css" />

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

    <script src="../../lib/ligerUI/js/plugins/ligerTab.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        var g1, g2, g3, g4, g5;
        $(function () {
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            $("form").ligerForm();
            grid();
            $('#navtab1').ligerTab({
                onAfterSelectTabItem: function (tabid) {
                    switch (tabid) {
                        case "nav_contact": grid(1);
                            break;
                        case "nav_follow": grid(2);
                            break;
                        case "nav_order": grid(3);
                            break;
                        case "nav_construct": grid(4);
                            break;
                        case "nav_payment": grid(5);
                            break;
                    }
                }
            });

            if (getparastr("cid")) {
                loadForm(getparastr("cid"));
            }
        })

        function grid(id) {
            switch (id) {
                case 1:
                    $.getJSON("../../data/CRM_Contact.ashx?Action=grid&customerid=" + getparastr("cid"), function (data) {
                        var rows = data.Rows;
                        if (rows.length < 1) {
                            $("#maingrid6").append("<span>无联系人信息</span>");
                            return;
                        }

                        for (var i = 0; i < rows.length; i++) {
                            var r = rows[i];
                            for (var n in r) {
                                if (r[n] == "null" || r[n] == null)
                                    r[n] = "";
                            }
                            $("#maingrid6").append(
                            "<table class='bodytable0'  style='margin:5px 5px 0 5px;width:710px;'>" +
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>联系人：</td><td height='23' width='15%'  >" + r.C_name + "【" + r.C_sex + "】" + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>部门：</td><td height='23' width='15%' >" + r.C_department + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>职务：</td><td height='23' width='15%'  >" + r.C_position + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>生日：</td><td height='23'  >" + r.C_birthday + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td height='23' class='table_label'>手机：</td><td height='23' >" + r.C_mob + "</td>" +
                                    "<td height='23' class='table_label'>电话：</td><td height='23'  >" + r.C_tel + "</td>" +
                                   "<td height='23' class='table_label'>QQ：</td><td height='23'  >" + r.C_QQ + "</td>" +
                                   "<td height='23' class='table_label'>Email：</td><td height='23'  >" + r.C_email + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td height='23'  class='table_label'>爱好：</td><td height='23' colspan='7'  >" + r.C_hobby + "</td>" +
                                "</tr>" +
                                 "<tr>" +
                                    "<td height='23'  class='table_label'>备注：</td><td height='23' colspan='7'  >" + r.C_remarks + "</td>" +
                                "</tr>" +
                                 "<tr>" +
                                    "<td height='23'  class='table_label'>地址：</td><td height='23' colspan='7'  >" + r.C_add + "</td>" +
                                "</tr>" +
                            "</table>");
                        }

                    });                  
                    break;
                case 2:
                    g2 = $("#maingrid8").ligerGrid({
                        columns: [
                                { display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                                {
                                    display: '跟进内容', name: 'Follow', align: 'left', width: 390, render: function (item, i) {
                                        return "<div class='abc'>" + item.Follow + "</div>";
                                    }
                                },
                                {
                                    display: '跟进时间', name: 'Follow_date', width: 130, render: function (item) {
                                        return formatTimebytype(item.Follow_date, 'yyyy-MM-dd hh:mm');
                                    }
                                },
                                { display: '跟进方式', name: 'Follow_Type', width: 60 },
                                {
                                    display: '跟进人', width: 100, render: function (item) {
                                        return item.department_name + "." + item.employee_name;
                                    }
                                }
                        ],
                        onAfterShowData: function (grid) {
                            $(".abc").hover(function (e) {
                                $(this).ligerTip({ content: $(this).text(), width: 200, distanceX: event.clientX - $(this).offset().left - $(this).width() + 15 });
                            }, function (e) {
                                $(this).ligerHideTip(e);
                            });
                        },
                        title: "跟进信息",
                        dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                        url: "../../data/CRM_Follow.ashx?Action=grid&customer_id=" + getparastr("cid"),
                        width: '744', height: '416',
                        heightDiff: -1
                    });
                    break;
                case 3:
                    g3 = $("#maingrid35").ligerGrid({
                        columns: [
                           { display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                            { display: '订单编号', name: 'Serialnumber', width: 140, hide: true },
                            { display: '客户', name: 'Customer_name', width: 260, align: 'left' },
                            {
                                display: '成交人员', width: 100, render: function (item) {
                                    return item.F_dep_name + "." + item.F_emp_name;
                                }
                            },
                            { display: '订单状态', name: 'Order_status', width: 100 },
                            { display: '订单金额（￥）', name: 'Order_amount', width: 100 },
                            {
                                display: '成交时间', name: 'Order_date', width: 90, render: function (item) {
                                    return formatTimebytype(item.Order_date, 'yyyy-MM-dd');
                                }
                            }

                        ],
                        title: '订单详情',
                        dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                        url: "../../data/Crm_order.ashx?Action=gridbycustomerid&customerid=" + getparastr("cid") + "&rnd=" + Math.random(),
                        width: '744', height: '416',
                        heightDiff: -1,
                        detail: {
                            onShowDetail: function (r, p) {
                                for (var n in r) {
                                    if (r[n] == null) r[n] = "";
                                }
                                var grid = document.createElement('div');
                                $(p).append(grid);
                                $(grid).css('margin', 3).ligerGrid({
                                    columns: [
                                            { display: '序号', width: 30, render: function (item, i) { return i + 1; } },
                                            { display: '产品名', name: 'product_name', width: 120 },
                                            {
                                                display: '单价', name: 'price', width: 80, type: 'float', render: function (item) {
                                                    return toMoney(item.price);
                                                }
                                            },
                                            { display: '数量', name: 'quantity', width: 40, type: 'int' },
                                            { display: '单位', name: 'unit', width: 40 },
                                            {
                                                display: '总价', name: 'amount', width: 100, type: 'float', render: function (item) {
                                                    return toMoney(item.amount) + "元";
                                                }
                                            }

                                    ],
                                    //selectRowButtonOnly: true,
                                    usePager: false,
                                    checkbox: true,
                                    url: "../../data/Crm_order_details.ashx?Action=grid&orderid=" + r.id,
                                    width: '99%', height: '100',
                                    heightDiff: -1
                                })

                            }
                        }
                    });
                    break;
                case 4:
                    g4 = $("#maingrid36").ligerGrid({
                        columns: [
                            { display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                            { display: '合同编号', name: 'Serialnumber', width: 140, hide: true },
                            { display: '合同名称', name: 'Contract_name', width: 200, align: 'left' },
                            { display: '客户名称', name: 'Customer_name', width: 150 },
                            {
                                display: '合同金额(￥)', name: 'Contract_amount', width: 120, align: 'right', render: function (item) {
                                    return toMoney(item.Contract_amount);
                                }
                            },
                            {
                                display: '合同归属', width: 100, render: function (item) {
                                    return item.C_depname + "." + item.C_empname;
                                }
                            },

                            { display: '签订日期', name: 'Sign_date', width: 90 }

                        ],
                        title: '合同记录',
                        dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                        url: "../../data/Crm_contract.ashx?Action=grid&cid=" + getparastr("cid") + "&rnd=" + Math.random(),
                        width: '744', height: '416',
                        heightDiff: -1,
                        detail: {
                            onShowDetail: function (r, p) {
                                for (var n in r) {
                                    if (r[n] == null) r[n] = "";
                                }
                                $(p).append(
                                    "<table class='bodytable0'  style='width:99%;margin:5px'>" +
                                        "<tr>" +
                                            "<td height='23' width='10%' class='table_label'>签约人：</td><td height='23'  width='15%'>" + r.Customer_Contractor + "</td>" +
                                            "<td height='23' width='10%' class='table_label'>我方签约：</td><td height='23'  width='15%'>" + r.Our_Contractor_name + "</td>" +
                                            "<td height='23' width='10%' class='table_label'>开始时间：</td><td height='23'  width='15%'>" + r.Start_date + "</td>" +
                                            "<td height='23' width='10%' class='table_label'>结束时间：</td><td height='23'  width='15%'>" + r.End_date + "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td height='23' width='10%' class='table_label'>主要条款：</td><td height='23' colspan='7'>" + r.Main_Content.replace(/\n/g, "<br />") + "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td height='23' width='10%' class='table_label'>备注：</td><td height='23'  colspan='7'>" + r.Remarks + "</td>" +
                                        "</tr>" +
                                    "</table>");
                            }
                        }
                    });
                    break;
                case 5:
                    g5 = $("#maingrid37").ligerGrid({
                        columns: [
                            { display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                            { display: '客户名', name: 'Customer_name', width: 140 },
                            { display: '凭证号码', name: 'Receive_num', width: 140 },
                            { display: '付款方式', name: 'Pay_type', width: 100 },
                            {
                                display: '收款金额(￥)', name: 'Receive_amount', width: 120, align: 'right', render: function (item) {
                                    return toMoney(item.Receive_amount);
                                }
                            },
                            {
                                display: '收款人', width: 100, render: function (item) {
                                    return item.C_depname + "." + item.C_empname;
                                }
                            },
                            {
                                display: '收款日期', name: 'Receive_date', width: 90, render: function (item) {
                                    return formatTimebytype(item.Receive_date, 'yyyy-MM-dd');
                                }
                            },
                            { display: '录入人', name: 'create_name', width: 90 }

                        ],
                        title: '收款纪录',
                        dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                        //checkbox:true,
                        url: "../../data/CRM_receive.ashx?Action=grid&customerid=" + getparastr("cid") + "&rnd=" + Math.random(),
                        width: '744', height: '416',
                        heightDiff: -1
                    });
                    break;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../../data/crm_customer.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', cid: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == null || obj[n] == "null")
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_company").val(obj.Customer);
                    $("#T_company0").val(obj.industry);
                    $("#T_address").val(obj.address);
                    $("#T_Website").html("<a href='"+obj.site+"' target='_blank'>"+obj.site+"</a>");
                    $("#T_fax").val(obj.fax);
                    $("#T_descript").val(obj.DesCripe);
                    $("#T_remarks").val(obj.Remarks);
                    $("#T_company_tel").val(obj.tel);

                    $("#T_Provinces").val(obj.Provinces);
                    $("#T_City").val(obj.City);
                    $("#T_customertype").val(obj.CustomerType);
                    $("#T_customerlevel").val(obj.CustomerLevel);
                    $("#T_CustomerSource").val(obj.CustomerSource);
                    $("#T_private").val(obj.privatecustomer);
                    $("#T_department").val(obj.Department);
                    $("#T_employee").val(obj.Employee);

                    var site = obj.site;
                    var strRegex = "^((https|http|ftp)?://)";
             
                    var re = new RegExp(strRegex);
                    if (!re.test(site))                    
                        site = "http://" + site;
                    $("#T_Website").html("<a href='" + site + "' target='_blank'>" + obj.site + "</a>");
                }
            });

        }

    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <div id="navtab1" style="width: 743px; overflow: hidden; border: none;">
            <div tabid="nav_base" title="基本信息" style="height: 415px">
                <table style="width: 600px; margin: 5px;" class='bodytable1'>
                    <tr>
                        <td colspan="4" class="table_title1">基本信息</td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">公司名称：</div>
                        </td>
                        <td>
                            <input type="text" id="T_company" name="T_company" ltype="text" ligerui="{width:196}" validate="{required:true}" /></td>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">公司网址：</div>
                        </td>
                        <td>
                            <div id="T_Website" name="T_Website"></div></td>
                    </tr>
                    <tr>
                        <td>

                            <div style="width: 80px; text-align: right; float: right">所属行业：</div>

                        </td>
                        <td>
                            <input type="text" id="T_company0" name="T_company0" ltype="text" ligerui="{width:196}" validate="{required:true}" /></td>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">所属地区：</div>
                        </td>
                        <td>
                            <div style="width: 100px; float: left">
                                <input id="T_Provinces" name="T_Provinces" type="text" style="width: 96px;" ltype="text" ligerui="{width:96}" />
                            </div>
                            <div style="width: 98px; float: left">
                                <input id="T_City" name="T_City" type="text" style="width: 96px;" ltype="text" ligerui="{width:96}" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>

                            <div style="width: 80px; text-align: right; float: right">公司电话：</div>

                        </td>
                        <td>

                            <input id="T_company_tel" name="T_company_tel" type="text" ltype="text" ligerui="{width:196}" validate="{required:true}" /></td>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">传真：</div>
                        </td>
                        <td>
                            <input id="T_fax" name="T_fax" type="text" ltype="text" ligerui="{width:196}" /></td>
                    </tr>
                    <tr>
                        <td>

                            <div style="width: 80px; text-align: right; float: right">公司地址：</div>

                        </td>
                        <td colspan="3">

                            <input type="text" id="T_address" name="T_address" ltype="text" ligerui="{width:495}" /></td>
                    </tr>


                    <tr>
                        <td colspan="4" class="table_title1">其他</td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">客户类型：</div>
                        </td>
                        <td>
                            <div style="width: 100px; float: left">
                                <input id="T_customertype" name="T_customertype" type="text" style="width: 96px" ltype="text" ligerui="{width:96}" />
                            </div>
                            <div style="width: 98px; float: left">
                                <input id="T_customerlevel" name="T_customerlevel" type="text" style="width: 96px" ltype="text" ligerui="{width:96}" />
                            </div>
                        </td>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">客户来源：</div>
                        </td>
                        <td>
                            <input id="T_CustomerSource" name="T_CustomerSource" type="text" ltype="text" ligerui="{width:196}" />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">客户描述：</div>
                        </td>
                        <td colspan="3">
                            <input id="T_descript" name="T_descript" type="text" ltype="text" ligerui="{width:495}" /></td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">备&nbsp; 注：</div>
                        </td>
                        <td colspan="3">
                            <input id="T_remarks" name="T_remarks" type="text" ltype="text" ligerui="{width:495}" /></td>
                    </tr>
                    <tr>
                        <td colspan="4" class="table_title1">归属</td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">状态：</div>
                        </td>
                        <td>
                            <input id="T_private" name="T_private" type="text" ltype="text" ligerui="{width:196 }" validate="{required:true}" /></td>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">业务员：</div>
                        </td>
                        <td>
                            <div style="width: 100px; float: left">
                                <input id="T_department" name="T_department" type="text" validate="{required:true}" style="width: 97px" ltype="text" ligerui="{width:96}" />
                            </div>
                            <div style="width: 98px; float: left">
                                <input id="T_employee" name="T_employee" type="text" validate="{required:true}" style="width: 96px" ltype="text" ligerui="{width:96}" />
                            </div>
                        </td>
                    </tr>
                    <%--<tr>
                <td colspan="4">
                    <div id="toolbar" style="width: 585px;"></div>
                    <div id="maingrid4" style="margin: -1px;"></div>
                </td>
            </tr>--%>
                </table>
            </div>
            <div tabid="nav_contact" title="联系人" style="height: 417px;overflow-y:scroll;">
                <div id="maingrid6" style="margin: -1px;"></div>
            </div>
            <div tabid="nav_follow" title="跟进信息" style="height: 417px">
                <div id="maingrid8" style="margin: -1px;"></div>
            </div>
            <div tabid="nav_order" title="订单详情" style="height: 417px">
                <div id="maingrid35" style="margin: -1px;"></div>
            </div>
            <div tabid="nav_construct" title="合同信息" style="height: 417px">
                <div id="maingrid36" style="margin: -1px;"></div>
            </div>
            <div tabid="nav_payment" title="收款记录" style="height: 417px">
                <div id="maingrid37" style="margin: -1px;"></div>
            </div>
        </div>
    </form>
</body>
</html>
