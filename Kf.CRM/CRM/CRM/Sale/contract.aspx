<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../../CSS/core.css" rel="stylesheet" type="text/css" />
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
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
        var manager;
        var manager1;
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                   { display: '���', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    { display: '��ͬ���', name: 'Serialnumber', width: 80 },
                    {
                        display: '��ͬ����', name: 'Contract_name', width: 200, align: 'left', render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view(5," + item.id + ")>";
                            if (item.Contract_name)
                                html += item.Contract_name;
                            html += "</a>";
                            return html;
                        }
                    },
                    {
                        display: '�ͻ�����', name: 'Customer_name', width: 200, render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view(1," + item.Customer_id + ")>";
                            if (item.Customer_name)
                                html += item.Customer_name;
                            html += "</a>";
                            return html;
                        }
                    },
                    {
                        display: '��ͬ���(��)', name: 'Contract_amount', width: 120, align: 'right', render: function (item) {
                            return toMoney(item.Contract_amount);
                        }
                    },
                    { display: '��������', name: 'C_depname', width: 100 },
                    { display: '����Ա��', name: 'C_empname', width: 80 },
                    {
                        display: '��ͬ����', name: 'End_date', width: 110, render: function (item) {
                            var diff = DateDiff(item.End_date);
                            if (diff < 0)
                                return "<div style='color:#0030ff'>�ѵ���</div>";
                            else if (diff <= 30)
                                return "<div style='color:#f00;font-weight:bold;'>����" + diff + "�쵽��</div>";
                            else if (diff <= 60)
                                return "<div style='color:#ffa800'>����" + diff + "�쵽��</div>";
                            else return item.End_date;

                        }
                    },
                    { display: 'ǩ������', name: 'Sign_date', width: 90 }

                ],
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "../../data/Crm_contract.ashx?Action=grid&rnd=" + Math.random(),
                width: '100%', height: '100%',
                heightDiff: -1,               
                detail: {
                    onShowDetail: function (r, p) {
                        for (var n in r) {
                            if (r[n] == null) r[n] = "";
                        }
                        $(p).append(
                            "<table class='bodytable0'  style='width:99%;margin:5px'>" +
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>ǩԼ�ˣ�</td><td height='23'  width='15%'>" + r.Customer_Contractor + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>�ҷ�ǩԼ��</td><td height='23'  width='15%'>" + r.Our_Contractor_name + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>��ʼʱ�䣺</td><td height='23'  width='15%'>" + r.Start_date + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>����ʱ�䣺</td><td height='23'  width='15%'>" + r.End_date + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>��Ҫ���</td><td height='23' colspan='7'>" + r.Main_Content.replace(/\n/g, "<br />") + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>��ע��</td><td height='23'  colspan='7'>" + r.Remarks + "</td>" +
                                "</tr>" +
                            "</table>");
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
            //toolbar1();

        });
        function DateDiff(sDate) {    //sDate1��sDate2��2006-12-18��ʽ  
            var oDate1, oDate2, iDays
            oDate1 = new Date()    //ת��Ϊ12-18-2006��ʽ  
            aDate = sDate.split("-")
            oDate2 = new Date(aDate[1] + '-' + aDate[2] + '-' + aDate[0])
            iDays = parseInt((oDate2 - oDate1) / 1000 / 60 / 60 / 24)    //�����ĺ�����ת��Ϊ����  
            return iDays
        }
        function toolbar() {
            $.getJSON("../../data/toolbar.ashx?Action=GetSys&mid=35&rnd=" + Math.random(), function (data, textStatus) {
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
            var e = $('#employee').ligerComboBox({ width: 96 });
            var f = $('#department').ligerComboBox({
                width: 97,
                selectBoxWidth: 240,
                selectBoxHeight: 200,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                tree: {
                    url: '../../../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    //parentIDFieldName: 'pid',
                    checkbox: false
                },
                onSelected: function (newvalue) {
                    $.get("../../../data/hr_employee.ashx?Action=combo&did=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
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

            manager.GetDataByURL("../../../data/Crm_contract.ashx?" + serchtxt);
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
                            text: '����', onclick: function (item, dialog) {
                                f_save(item, dialog);
                            }
                        },
                        {
                            text: '�ر�', onclick: function (item, dialog) {
                                dialog.frame.flush_attachment();
                                setTimeout(function(){ dialog.close()},100);
                            }
                        }
                ], isResize: true, showToggle: true, timeParmName: 'a', allowClose: false
            };
            activeDialog = top.jQuery.ligerDialog.open(dialogOptions);
        }

        function add() {
            f_openWindow("CRM/sale/contract_add.aspx", "������ͬ", 867, 490);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('CRM/sale/contract_add.aspx?cid=' + row.id, "�޸ĺ�ͬ", 867, 490);
            }
            else {
                $.ligerDialog.warn('��ѡ���У�');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("��ͬɾ���޷��ָ���ȷ��ɾ����", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "../../data/Crm_contract.ashx", type: "POST",
                            data: { Action: "del", id: row.id, rnd: Math.random() },
                            success: function (responseText) {
                                if (responseText == "true") {
                                    f_reload();
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


        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "../../data/Crm_contract.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        $.ligerDialog.closeWaitting();
                        f_reload();
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }

        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };


    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <div id="toolbar"></div>
        
        <div id="grid">
            <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
            <div id="toolbar1"></div>
            <div id="maingrid5" style="margin: -1px -1px;"></div>
        </div>


    </form>
    <div class="az">
        <form id='serchform'>
            <table style='width: 750px' class="bodytable1">
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�ͻ����ƣ�</div>
                    </td>
                    <td>
                        <input type='text' id='company' name='company' ltype='text' ligerui='{width:120}' /></td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>ǩ�����ڣ�</div>
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
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>��ͬ����</div>
                    </td>
                    <td>
                        <input id='contact' name="contact" type='text' ltype='text' ligerui='{width:120}' /></td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>ҵ��Ա��</div>
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
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>
