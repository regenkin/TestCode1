 <%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=8" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '���', width: 30, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return rowindex + 1; } },
                    { display: '����', name: 'City', align: 'left', width: 300 },
                    { display: '����', name: 'City_order',width:50 }
                ],
                checkBox:true,
                dataAction: 'local', pageSize: 10, pageSizeOptions: [5, 10, 20],
                url: "../data/Param_City.ashx?Action=treegrid",
                width: '100%', height: '100%',
                tree: { columnName: 'City' },
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                },
                heightDiff: -1
            });

            var manager = $("#maingrid4").ligerGetGridManager();
            //manager.onResize();
            toolbar();


        });
        function toolbar() {

            $.getJSON("../data/toolbar.ashx?Action=GetSys&mid=84&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({ type: 'button', text: 'ȫ��չ��', icon: '../images/folder-open.gif', disable: true, click: function () { treegridexpand(1); } })
                items.push({ type: 'button', text: 'ȫ���۵�', icon: '../images/folder-closed.gif', disable: true, click: function () { treegridexpand(0); } })
                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#maingrid4").ligerGetGridManager().onResize();
            });
        }
        function treegridexpand(status) {
            var manager = $("#maingrid4").ligerGetGridManager();
            if (status == "0") {
                manager.collapseAll();
            } else {
                manager.expandAll();
            }

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


        function add() {
            f_openWindow("System/Param_City_add.aspx", "����", 390, 200);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('System/Param_City_add.aspx?pid=' + row.id, "�޸�", 390, 200);
            } else {
                top.$.ligerDialog.error('��ѡ���У�');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();

            if (manager.hasChildren(row))
            {
                $.ligerDialog.warn('�����¼���������ɾ����');
                return;
            }

            if (row) {
                $.ligerDialog.confirm("ȷ��ɾ����", function (yes) {
                    if (yes) {
                        $.ajax({
                            type: "POST",
                            url: "../data/Param_City.ashx",
                            data: { Action: 'del', id: row.id },
                            success: function (result) {
                                if (result == "false:parent")
                                    $.ligerDialog.error('�����¼���������ɾ����');
                                else if (result == "false:customer")
                                    $.ligerDialog.error('���пͻ���������ɾ����');
                                else
                                    f_reload();
                            }
                        });
                    }
                })
            }
            else {
                $.ligerDialog.warn("��ѡ����");
            }
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "../data/Param_City.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        top.$.ligerDialog.closeWaitting();
                        f_reload();
                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('����ʧ�ܣ�');
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
    <form id="mainform" onsubmit="return false">
        <div id="toolbar"></div>

        <div id="maingrid4" style="margin: -1px;"></div>
    </form>
</body>
</html>
