


<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />    
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: 'ID', name: 'RoleID', type: 'int', width: 50 },
                    { display: '���', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    { display: '��ɫ��', name: 'RoleName' },
                    { display: '��ɫ����', name: 'RoleDscript', width: 450 },
                    { display: '����', name: 'RoleSort', width: 50 }
                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                enabledEdit: true,
                url: "../data/Sys_role.ashx?Action=grid",
                width: '100%',
                height: '100%',
                heightDiff: -1,
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });



            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            toolbar();
        });
        function toolbar() {
            $.getJSON("../data/toolbar.ashx?Action=GetSys&mid=31&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });
                $("#maingrid4").ligerGetGridManager().onResize();
            });
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.showData({ Rows: [], Total: 0 });
            var url = "../data/Sys_role.ashx?Action=grid";
            manager.GetDataByURL(url);
        };
        function authorized() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                top.$.ligerDialog.open({
                    width: 800, height: 500, showToggle: true, url: 'System/Sys_Role_authorized.aspx?Role_id=' + row.RoleID, title: "��Ȩ", buttons: [
                        {
                            text: '����', onclick: function (item, dialog) {
                                dialog.frame.f_save();
                            }
                        },
                        {
                            text: '�ر�', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                    ]
                });
            }
            else {
                $.ligerDialog.warn("��ѡ����");
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
            f_openWindow("System/Sys_Role_add.aspx", "������ɫ", 500, 300);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('System/Sys_Role_add.aspx?id=' + row.RoleID, "�޸Ľ�ɫ", 500, 300);
            } else {
                $.ligerDialog.warn("��ѡ����");
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("��ɫɾ�����ָܻ���\n��ȷ��Ҫ�Ƴ���", function (yes) {
                    if (yes) {
                        $.ajax({
                            type: "POST",
                            url: "../data/Sys_role.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                            data: { Action: "del", id: row.RoleID }, /* ע������ĸ�ʽ������ */
                            success: function (result) {
                                f_reload();
                            }
                        });
                    }
                })
            } else {
                $.ligerDialog.warn("��ѡ����");
            }
        }

        function role_emp() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                top.$.ligerDialog.open({
                    width: 700, height: 500, showToggle: true, url: 'system/Role_emp.aspx?rid=' + row.RoleID, title: "��ɫ��Ա����", buttons: [
                        //{ text: 'ȷ��', onclick: f_importOK },
                        { text: '�ر�', onclick: f_importCancel }
                    ]
                });
            } else {
                $.ligerDialog.warn("��ѡ����");
            }
        }
        function data_authorized() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                top.$.ligerDialog.open({
                    width: 600, height: 300, showToggle: true, url: 'system/Sys_data_authorized.aspx?rid=' + row.RoleID, title: "����Ȩ�޹���", buttons: [
                        { text: 'ȷ��', onclick: f_importOK },
                        { text: 'ȡ��', onclick: f_importCancel }
                    ]
                });
            } else {
                $.ligerDialog.warn("��ѡ����");
            }
        }
        function f_importOK(item, dialog) {
            var isSave = dialog.frame.f_save();
            if (isSave == "true") {
                dialog.close();
            }
        }
        function f_importCancel(item, dialog) {
            dialog.close();
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "../data/Sys_role.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        top.$.ligerDialog.closeWaitting();
                        //top.$.ligerDialog.success('�����ɹ���');
                        f_reload();

                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }
        }


    </script>
</head>
<body>
    <form id="mainform" onsubmit="return false">
        <div>
            <div id="toolbar"></div>
            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>
</body>
</html>
