<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="../../CSS/core.css" rel="stylesheet" type="text/css" />
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
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
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
                    {
                        display: '��˾����', name: 'Customer', width: 200, align: 'left', render: function (item) {
                            var html = "<a href='javascript:void(0)' onclick=view(1," + item.id + ")>";
                            if (item.Customer)
                                html += item.Customer;
                            html += "</a>";
                            return html;
                        }
                    },
                    { display: '�绰', name: 'tel', width: 120, align: 'right' },
                    { display: '�ͻ�����', name: 'CustomerType', width: 80 },
                    { display: '�ͻ����', name: 'CustomerLevel', width: 80 },
                    { display: '�ͻ���Դ', name: 'CustomerSource', width: 80 },
                    { display: 'ʡ��', name: 'Provinces', width: 80 },
                    { display: '����', name: 'City', width: 80 },
                    { display: '������ҵ', name: 'industry', width: 80 },
                    { display: '����', name: 'Department', width: 80 },
                    { display: 'Ա��', name: 'Employee', width: 80 },
                    { display: '��Դ״̬', name: 'privatecustomer', width: 60 },
                    {
                        display: '������', name: 'lastfollow', width: 90, render: function (item) {
                            var lastfollow = formatTimebytype(item.lastfollow, 'yyyy-MM-dd');
                            if (lastfollow == "1900-01-01")
                                lastfollow = "";
                            return lastfollow;
                        }
                    },
                    {
                        display: '����ʱ��', name: 'Create_date', width: 90, render: function (item) {
                            var Create_date = formatTimebytype(item.Create_date, 'yyyy-MM-dd');
                            return Create_date;
                        }
                    }
                ],
                onBeforeShowData: function (grid, data) {
                    startTime = new Date();
                },
                //fixedCellHeight:false,
                onSelectRow: function (data, rowindex, rowobj) {
                    var manager = $("#maingrid5").ligerGetGridManager();
                    manager.showData({ Rows: [], Total: 0 });
                    var url = "../../data/CRM_Follow.ashx?Action=grid&customer_id=" + data.id;
                    manager.GetDataByURL(url);
                },
                rowtype: "CustomerType",
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "../../data/crm_customer.ashx?Action=grid&rnd=" + Math.random(),
                width: '100%', height: '65%',
                heightDiff: -1,
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                },
                onAfterShowData: function (grid) {
                    $("tr[rowtype='�ѳɽ�']").addClass("l-treeleve1").removeClass("l-grid-row-alt");
                    var nowTime = new Date();
                    //alert('�������ݺ�ʱ��' + (nowTime - startTime));
                },
                detail: {
                    onShowDetail: function (r, p) {
                        for (var n in r) {
                            if (r[n] == null) r[n] = "";
                        }
                        var grid = document.createElement('div');
                        $(p).append(grid);
                        $(grid).css('margin', 3).ligerGrid({
                            columns: [
                            { display: '�к�', width: 50, render: function (item, i) { return i + 1; } },
                            { display: '��ϵ��', name: 'C_name', width: 100 },
                            { display: 'ְ��', name: 'C_position', width: 100 },
                            { display: '�Ա�', name: 'C_sex', width: 50 },
                            //{ display: '������˾', name: 'C_companyname', width: 180 },
                            { display: '�ֻ�', name: 'C_mob', width: 120 },
                            { display: 'QQ', name: 'C_QQ', width: 100 },
                            { display: 'Email', name: 'C_email', width: 180 }
                            ],
                            usePager: false,
                            checkbox: true,
                            url: "../../data/CRM_Contact.ashx?Action=grid&customerid=" + r.id,
                            width: '1022px', height: '100px',
                            heightDiff: 0
                        })

                    }
                }
            });
            $("#maingrid5").ligerGrid({
                columns: [
                        { display: '���', width: 40, render: function (item, i) { return i + 1; } },
                        {
                            display: '��������', name: 'Follow', align: 'left', width: 590, render: function (item) {
                                var html = "<div class='abc'><a href='javascript:void(0)' onclick=view(2," + item.id + ")>";
                                if (item.Follow)
                                    html += item.Follow;
                                html += "</a></div>";
                                return html;
                            }
                        },
                        {
                            display: '����ʱ��', name: 'Follow_date', width: 140, render: function (item) {
                                return formatTimebytype(item.Follow_date, 'yyyy-MM-dd hh:mm');
                            }
                        },
                        { display: '������ʽ', name: 'Follow_Type', width: 60 },
                        {
                            display: '������', name: '', width: 180, render: function (item) {
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
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                //checkbox:true,
                url: "../../data/CRM_Follow.ashx?Action=grid&customer_id=0",
                width: '100%', height: '100%',
                //title: "������Ϣ",
                heightDiff: -1,
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu1.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());
            $('form').ligerForm();
            toolbar();
        });

        function toolbar() {
            $.getJSON("../../data/toolbar.ashx?Action=GetSys&mid=4&rnd=" + Math.random(), function (data, textStatus) {
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

            });
            $.getJSON("../../data/toolbar.ashx?Action=GetSys&mid=6&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);
                }
                $("#toolbar1").ligerToolBar({
                    items: items
                });
                menu1 = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#maingrid4").ligerGetGridManager().onResize();
                $("#maingrid5").ligerGetGridManager().onResize();
            });
        }
        function initSerchForm() {
            var a = $('#T_City').ligerComboBox({ width: 96, emptyText: '���գ�' });
            var b = $('#T_Provinces').ligerComboBox({
                width: 97,

                url: "../../data/Param_City.ashx?Action=combo1&rnd=" + Math.random(),
                onSelected: function (newvalue) {
                    $.get("../../data/Param_City.ashx?Action=combo2&pid=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        a.setData(eval(data));
                    });
                }, emptyText: '���գ�'
            });
            $('#customertype').ligerComboBox({ width: 97, emptyText: '���գ�', url: "../../data/Param_SysParam.ashx?Action=combo&parentid=1&rnd=" + Math.random() });
            $('#customerlevel').ligerComboBox({ width: 96, emptyText: '���գ�', url: "../../data/Param_SysParam.ashx?Action=combo&parentid=2&rnd=" + Math.random() });
            $('#cus_sourse').ligerComboBox({ width: 196, emptyText: '���գ�', url: "../../data/Param_SysParam.ashx?Action=combo&parentid=3&rnd=" + Math.random() });
            $('#industry').ligerComboBox({ width: 120, emptyText: '���գ�', url: "../../data/Param_SysParam.ashx?Action=combo&parentid=8&rnd=" + Math.random() });
            var e = $('#employee').ligerComboBox({ width: 96, emptyText: '���գ�' });
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
                $("#maingrid5").ligerGetGridManager().onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                $("#maingrid4").ligerGetGridManager().onResize();
                $("#maingrid5").ligerGetGridManager().onResize();
            }
            $("#company").focus();
        }
        $(document).keydown(function (e) {
            if (e.keyCode == 13 && e.target.applyligerui) {
                doserch();
            }
        });
        function doserch() {
            var sendtxt = "&Action=grid&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);           
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.GetDataByURL("../../data/crm_customer.ashx?" + serchtxt);

        }
        function doclear() {
            //var serchtxt = $("#serchform :input").reset();
            $("#serchform").each(function () {
                this.reset();
                $(".l-selected").removeClass("l-selected");
            });
        }

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
                                dialog.close();
                            }
                        }
                ], isResize: true, showToggle: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }

        function toimport() {
            var dialogOptions = {
                width: 540, height: 295, title: '�ͻ�����', url: 'crm/customer/customer_import.aspx', buttons: [
                        {
                            text: '�ر�', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }

        function add() {
            f_openWindow("CRM/Customer/Customer_add.aspx", "�����ͻ�", 770, 495);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('CRM/Customer/Customer_add.aspx?cid=' + row.id, "�޸Ŀͻ�", 770, 490);
            }
            else {
                $.ligerDialog.warn('��ѡ���У�');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("ȷ��ɾ����", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "../../data/CRM_Customer.ashx", type: "POST",
                            data: { Action: "AdvanceDelete", id: row.id, rnd: Math.random() },
                            success: function (responseText) {
                                if (responseText == "true") {
                                    f_reload();
                                    f_followreload();
                                }
                                else if (responseText == "delfalse") {
                                    top.$.ligerDialog.error('Ȩ�޲�����ɾ��ʧ�ܣ�');
                                }
                                else if (responseText == "false") {
                                    top.$.ligerDialog.error('δ֪����ɾ��ʧ�ܣ�');
                                }
                                else {
                                    top.$.ligerDialog.warn('�˿ͻ��º��� ' + responseText + '��ɾ��ʧ�ܣ������Ƚ���Щ����ɾ����');
                                }

                            },
                            error: function () {
                                top.$.ligerDialog.error('ɾ��ʧ�ܣ�');
                            }
                        });
                    }
                });
            }
            else {
                $.ligerDialog.warn("��ѡ��ͻ�");
            }
        }
        function toexport() {
            var sendtxt = "&Action=ToExcel&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            var url = "../../data/crm_customer.ashx?" + serchtxt;

            window.open(url);
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();

                $.ajax({
                    url: "../../data/CRM_Customer.ashx", type: "POST",
                    data: issave,
                    beforesend: function () {
                        top.$.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                    },
                    success: function (responseText) {

                        f_reload();
                        top.flushiframegrid("tabid5");
                    },
                    error: function () {
                        top.$.ligerDialog.error('����ʧ�ܣ�');
                    },
                    complete: function () {
                        top.$.ligerDialog.closeWaitting();
                    }
                });

            }
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };

        //follow
        function follow_openWindow(url, title, width, height) {
            var dialogOptions = {
                width: width, height: height, title: title, url: url, buttons: [
                        {
                            text: '����', onclick: function (item, dialog) {
                                f_savefollow(item, dialog);
                            }
                        },
                        {
                            text: '�ر�', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, showToggle: true, timeParmName: 'b'
            };
            activeDialog1 = top.jQuery.ligerDialog.open(dialogOptions);
        }
        function addfollow() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                follow_openWindow("CRM/Customer/Customer_follow_add.aspx?cid=" + row.id, "��������", 530, 400);
            } else {
                $.ligerDialog.warn('��ѡ��ͻ���');
            }
        }
        function editfollow() {
            var manager = $("#maingrid5").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                follow_openWindow('CRM/Customer/Customer_follow_add.aspx?fid=' + row.id + "&cid=" + row.Customer_id, "�޸ĸ���", 530, 400);
            } else {
                $.ligerDialog.warn('��ѡ�������');
            }
        }
        function delfollow() {
            var manager = $("#maingrid5").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("����ɾ���޷��ָ���ȷ��ɾ����", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "../../data/CRM_Follow.ashx", type: "POST",
                            data: { Action: "del", id: row.id, rnd: Math.random() },
                            success: function (responseText) {
                                if (responseText == "true") {
                                    f_followreload();
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
                $.ligerDialog.warn("��ѡ�����");
            }
        }
        function f_savefollow(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "../../data/CRM_Follow.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        $.ligerDialog.closeWaitting();
                        f_followreload();
                        f_reload();
                        top.flushiframegrid("tabid6");
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }
        }
        function f_followreload() {
            var manager = $("#maingrid5").ligerGetGridManager();
            manager.loadData(true);
            top.flushiframegrid("tabid6");
        };
    </script>
    <style type="text/css">
        .l-treeleve1 {
            background: yellow;
        }

        .l-treeleve2 {
            background: yellow;
        }

        .l-treeleve3 {
            background: #eee;
        }
    </style>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <div id="toolbar"></div>

        <div id="grid">
            <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
            <div id="toolbar1"></div>
            <div id="Div1" style="position: relative;">
                <div id="maingrid5" style="margin: -1px -1px;"></div>
            </div>
        </div>


    </form>
    <div class="az">
        <form id='serchform'>
            <table style='width: 960px' class="bodytable1">
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>��˾���ƣ�</div>
                    </td>
                    <td>
                        <input type='text' id='company' name='company' ltype='text' ligerui='{width:120}' /></td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�ͻ����ͣ�</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='customertype' name='customertype' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='customerlevel' name='customerlevel' />
                        </div>
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>¼��ʱ�䣺</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate' name='startdate' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate' name='enddate' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>
                    <td>
                        <input id='keyword' name="keyword" type='text' ltype='text' ligerui='{width:196, nullText: "����ؼ���������ַ����������ע"}' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>������ҵ��</div>
                    </td>
                    <td>
                        <input id='industry' name="industry" type='text' /></td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>����������</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='T_Provinces' name='T_Provinces' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='T_City' name='T_City' />
                        </div>
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>��������</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startfollow' name='startfollow' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='endfollow' name='endfollow' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>

                </tr>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�绰��</div>
                    </td>
                    <td>
                        <input type='text' id='tel' name='tel' ltype='text' ligerui='{width:120}' />
                    </td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�ͻ���Դ��</div>
                    </td>
                    <td>
                        <input type='text' id='cus_sourse' name='cus_sourse' />
                    </td>
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
                    <td>

                        <input id='Button2' type='button' value='����' style='width: 80px; height: 24px'
                            onclick="doclear()" />
                        <input id='Button1' type='button' value='����' style='width: 80px; height: 24px' onclick="doserch()" />
                    </td>
                </tr>
            </table>
        </form>
    </div>

    <form id='toexcel'></form>
</body>
</html>
