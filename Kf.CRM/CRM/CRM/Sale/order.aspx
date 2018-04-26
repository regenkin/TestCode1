<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerResizable.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '���', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    { display: '�������', name: 'Serialnumber', width: 140, hide: true },
                    {
                        display: '�鿴',  width: 60, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view(4," + item.id + ")>�鿴</a>";
                            return html;
                        }
                    },
                    {
                        display: '�ͻ�', name: 'Customer_name', width: 260, align: 'left', render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view(1," + item.Customer_id + ")>";
                            if (item.Customer_name)
                                html += item.Customer_name;
                            html += "</a>";
                            return html;
                        }
                    },
                    { display: '�ɽ�����', name: 'F_dep_name', width: 80 },
                    { display: '�ɽ���Ա', name: 'F_emp_name', width: 80 },
                    { display: '����״̬', name: 'Order_status', width: 70 },
                    {
                        display: '����������', name: 'Order_amount', width: 100, align: 'right', render: function (item) {
                            return "<div style='color:#135294'>" + toMoney(item.Order_amount) + "</div>";
                        }
                    },
                   {
                       display: '�����ܶ����', name: 'receive_money', width: 100, align: 'right', render: function (item) {
                           return "<div style='color:#135294'>" + toMoney(item.receive_money) + "</div>";
                       }
                   },
                   {
                       display: 'δ��������', name: 'arrears_money', width: 100, align: 'right', render: function (item) {
                           return "<div style='color:#135294'>" + toMoney(item.arrears_money) + "</div>";
                       }
                   },
                   {
                       display: '�ѿ�Ʊ�����', name: 'invoice_money', width: 100, align: 'right', render: function (item) {
                           return "<div style='color:#135294'>" + toMoney(item.invoice_money) + "</div>";
                       }
                   },
                    {
                        display: '�ɽ�ʱ��', name: 'Order_date', width: 90, render: function (item) {
                            return formatTimebytype(item.Order_date, 'yyyy-MM-dd');
                        }
                    }

                ],
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "../../data/CRM_order.ashx?Action=grid&rnd=" + Math.random(),
                width: '100%', height: '100%',
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
                                    { display: '���', width: 30, render: function (item, i) { return i + 1; } },
                                    { display: '��Ʒ��', name: 'product_name', width: 120 },
                                    {
                                        display: '����', name: 'price', width: 80, type: 'float', align: 'right', render: function (item) {
                                            return toMoney(item.price);
                                        }
                                    },
                                    { display: '����', name: 'quantity', width: 40, type: 'int' },
                                    { display: '��λ', name: 'unit', width: 40 },
                                    {
                                        display: '�ܼ�', name: 'amount', width: 100, type: 'float', align: 'right', render: function (item) {
                                            return toMoney(item.amount) + "Ԫ";
                                        }
                                    }

                            ],
                            //selectRowButtonOnly: true,
                            usePager: false,
                            checkbox: true,
                            url: "../../data/CRM_order_details.ashx?Action=grid&orderid=" + r.id,
                            width: '99%', height: '100',
                            heightDiff: 0
                        })

                    }
                },
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());
            $('form').ligerForm();
            toolbar();
        });

        function toolbar() {
            $.getJSON("../../data/toolbar.ashx?Action=GetSys&mid=34&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {                   
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);
                }

                items.push({
                    type: 'serchbtn',
                    text: '�߼�����',
                    icon: '../../images/search.gif',
                    disable: true,
                    click: function () {
                        serchpanel();
                    }
                });
                $("#toolbar").ligerToolBar({
                    items: items
                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#maingrid4").ligerGetGridManager().onResize();
            });
        }
        function initSerchForm() {
            var d = $('#contact').ligerComboBox({ width: 120, url: "../../data/Param_SysParam.ashx?Action=combo&parentid=6&rnd=" + Math.random() });
            var e = $('#employee').ligerComboBox({ width: 96 });
            var f = $('#department').ligerComboBox({
                width: 97,
                selectBoxWidth: 240,
                selectBoxHeight: 200,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                tree: {
                    url: '../../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    //parentIDFieldName: 'pid',
                    checkbox: false
                },
                onSelected: function (newvalue) {
                    $.get("../../data/hr_employee.ashx?Action=combo&did=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        e.setData(eval(data));
                    });
                }
            });
        }
        function serchpanel() {
            initSerchForm();
            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                $("#maingrid4").ligerGetGridManager().onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                $("#maingrid4").ligerGetGridManager().onResize();
            }
        }
        function doserch() {
            var sendtxt = "&Action=grid&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);

            var manager = $("#maingrid4").ligerGetGridManager();

            manager.GetDataByURL("../../data/CRM_order.ashx?" + serchtxt);
        }
        function doclear() {
            //var serchtxt = $("#serchform :input").reset();
            $("#serchform").each(function () {
                this.reset();
                $(".l-selected").removeClass("l-selected");
            });
        }
        $(document).keydown(function (e) {
            if (e.keyCode == 13 && e.target.applyligerui) {
                doserch();
            }
        });
        var activeDialog = null;
        function f_openWindow(url, title, width, height) {
            var dialogOptions = {
                width: width, height: height, title: title, url: url, buttons: [
                        {
                            text: '����', onclick:f_save
                        },
                        {
                            text: '�ر�', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, showToggle: true, timeParmName: 'a'
            };
            activeDialog = top.jQuery.ligerDialog.open(dialogOptions);
        }

       
        function add() {
            f_openWindow("CRM/sale/order_add.aspx", "��������", 770, 490);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('CRM/sale/order_add.aspx?orderid=' + row.id, "�޸Ķ���", 770, 490);
            }
            else {
                $.ligerDialog.warn('��ѡ���У�');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("����ɾ���޷��ָ���ȷ��ɾ����", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "../../data/CRM_order.ashx", type: "POST",
                            data: { Action: "del", id: row.id, rnd: Math.random() },
                            success: function (responseText) {
                                if (responseText == "true") {
                                    f_reload();
                                }
                                else if (responseText == "false:receive") {
                                    top.$.ligerDialog.error('�˶����º����տ���Ϣ��������ɾ����');
                                }
                                else if (responseText == "false:invoice") {
                                    top.$.ligerDialog.error('�˶����º��з�Ʊ��Ϣ��������ɾ����');
                                }
                                else {
                                    top.$.ligerDialog.error('ɾ��ʧ�ܣ�');
                                }

                            },
                            error: function () {
                                top.$.ligerDialog.error('ɾ��ʧ�ܣ�');
                            }
                        });
                    }
                })
            }
            else {
                $.ligerDialog.warn("��ѡ��ͻ�");
            }
        }

        function f_check(item, dialog) {
            
            setTimeout(function (item, dialog) { f_save(item, dialog) }, 100);
        }
        function f_save(item, dialog) {
            dialog.frame.f_check();
            var issave = dialog.frame.f_save();
            var postdata = dialog.frame.f_postdata();
            var cansave = dialog.frame.f_checkquantity();
            var postnum = dialog.frame.f_postnum();
            //alert(postdata);

            if (cansave) {
                if (issave) {
                    if (postnum > 0) {
                        dialog.close();
                        $.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                        $.ajax({
                            url: "../../data/CRM_order.ashx", type: "POST",
                            data: issave + "&Postdata=" + postdata,
                            success: function (responseText) {
                                if (responseText == "{success:success}") {
                                    $.ligerDialog.closeWaitting();
                                    f_reload();
                                }
                                else {
                                    $.ligerDialog.closeWaitting();
                                    $.ligerDialog.error('δ֪����,����ϵ����Ա!');
                                }
                            },
                            error: function () {
                                $.ligerDialog.closeWaitting();
                                $.ligerDialog.error('����ʧ�ܣ�');
                            }
                        });
                    }
                    else {
                        top.$.ligerDialog.warn('��Ʒ����Ϊ�գ�');
                        return;
                    }

                }
            }
            else {
                top.$.ligerDialog.error('��������Ϊ��0����գ�', "", "", 9005);
            }
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
            top.flushiframegrid("tabid36");
            top.flushiframegrid("tabid37");
        };

    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <div id="toolbar"></div>

        <div id="grid">
            <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
        </div>


    </form>
    <div class="az">
        <form id='serchform'>
            <table style='width: 720px' class="bodytable1">
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>��˾���ƣ�</div>
                    </td>
                    <td>
                        <input type='text' id='company' name='company' ltype='text' ligerui='{width:120}' /></td>


                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�ɽ�ʱ�䣺</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate' name='startdate' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate' name='enddate' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>����״̬��</div>
                    </td>
                    <td>
                        <input id='contact' name="contact" type='text' /></td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�ɽ���Ա��</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='department' name='department' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='employee' name='employee' />
                        </div>
                    </td>
                    <td></td>
                    <td>
                        <input id='Button2' type='button' value='����' style='width: 80px; height: 24px' onclick="doclear()" />
                        <input id='Button1' type='button' value='����' style='width: 80px; height: 24px' onclick="doserch()" />
                    </td>
                </tr>

            </table>
        </form>
    </div>
</body>
</html>
