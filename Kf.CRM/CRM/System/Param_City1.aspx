<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>


    <script src="../JS/KFCRM.js" type="text/javascript"></script>

    <script src="../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {

            $("#tree1").ligerTree({
                url: '../data/Param_City.ashx?Action=tree&rnd=' + Math.random(),
                idFieldName: 'id',
                parentIDFieldName: 'pid',
                checkbox: false,
                itemopen: false
            }); 
            treemanager = $("#tree1").ligerGetTreeManager();  

            toolbar();

            $(window).resize(function () {
                resize();
            });
            
        });
        function resize()
        {
            var height = $(window).height();
            $("#treediv").height(height - $("#toolbar").height());
        }
        function toolbar() {
            $.getJSON("../data/toolbar.ashx?Action=GetSys&mid=84&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({ type: 'button', text: 'ȫ��չ��', icon: '../../images/folder-open.gif', disable: true, click: function () { expandAll(1); } })
                items.push({ type: 'button', text: 'ȫ���۵�', icon: '../../images/folder-closed.gif', disable: true, click: function () { collapseAll(0); } })
                $("#toolbar").ligerToolBar({
                    items: items

                });
                resize();
            });
        }
        function collapseAll() {
            treemanager.collapseAll();
        }
        function expandAll() {
            treemanager.expandAll();
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
                ], isResize: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
            //activeDialog.css({ top: top.$(".l-layout-center").height() + 81 });
            //activeDialog.animate({ top: top.$(".l-layout-center").height() + 81 - height });            
        }


        function edit() {
            var notes = treemanager.getSelected();
            if (notes != null && notes != undefined) {
                f_openWindow('system/Param_City_add.aspx?pid=' + notes.data.id, "��������", 620, 280);
            }
            else {
                $.ligerDialog.warn('��ѡ���ţ�');
            }
        }
        function add() {
            f_openWindow('system/Param_City_add.aspx', "��������", 620, 280);
        }

        function del() {
            var manager = $("#tree1").ligerGetTreeManager();
            var node = manager.getSelected();
            if (node) {
                if (manager.isChildren(node)) {
                    $.ligerDialog.error("�����¼����ţ�������ɾ����");
                }
                else {
                    $.ligerDialog.confirm("��ɫɾ�����ָܻ���\n��ȷ��Ҫ�Ƴ���", function (yes) {
                        if (yes) {
                            $.ajax({
                                url: "../data/hr_department.ashx", type: "POST",
                                data: { Action: "AdvanceDelete", id: node.data.id, rnd: Math.random() },
                                success: function (responseText) {
                                    top.$.ligerDialog.closeWaitting();
                                    if (responseText == "true") {
                                        treereload();
                                    }
                                    else if (responseText == "false:emp") {
                                        top.$.ligerDialog.error('ɾ��ʧ�ܣ��˲����º���Ա����¼��');
                                    }
                                    else if (responseText == "false:post") {
                                        top.$.ligerDialog.error('ɾ��ʧ�ܣ��˲����º��и�λ��Ϣ��');
                                    }
                                    else {
                                        top.$.ligerDialog.error('ɾ��ʧ�ܣ�');
                                    }

                                },
                                error: function () {
                                    top.$.ligerDialog.closeWaitting();
                                    top.$.ligerDialog.error('ɾ��ʧ�ܣ�');
                                }
                            });
                        }
                    })
                }

            } else {
                $.ligerDialog.warn("��ѡ���ţ�");
            }
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "../data/hr_department.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        top.$.ligerDialog.closeWaitting();
                        treereload();

                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }
        }

        function treereload() {
            treemanager = $("#tree1").ligerGetTreeManager();
            treemanager.clear();
            treemanager.FlushData();
        }
    </script>
</head>
<body style="padding: 0px">
    <form id="form1" onsubmit="return false">
        <div id="toolbar"></div>

        <div id="treediv" style="float: left;  overflow-y: scroll;width:100%;">
            <ul id="tree1"></ul>
        </div>

 
    </form>
</body>
</html>
