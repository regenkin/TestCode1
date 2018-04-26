<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>

    <script type="text/javascript">

        var manager = "";
        var treemanager;
        $(function () {
            $("#layout1").ligerLayout({ leftWidth: 150, allowLeftResize: false, allowLeftCollapse: true, space: 2 });
            $("#tree1").ligerTree({
                //url: '../data/Sys_Menu.ashx?Action=GetSysApp&rnd=' + Math.random(),
                url: '../data/Sys_App.ashx?Action=GetAppList&rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                usericon: 'App_icon',
                checkbox: false,
                itemopen: false
            });

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            toolbar();

            $("#maingrid").ligerGrid({
                columns: [
                    { display: 'ID', name: 'Menu_id', type: 'int', width: 50 },
                    { display: '菜单名', name: 'Menu_name', align: 'left' },
                    { display: '链接地址', name: 'Menu_url', align: 'left', width: 300 },
                    {
                        display: '图标', name: 'Menu_icon', width: 50, render: function (item) {
                            return "<img style='width:16px;height:16px;margin-top:4px;' src='../" + item.Menu_icon + "'/>"
                        }
                    },
                    //{ display: '响应事件', name: 'Menu_handler' },
                    { display: '排序', name: 'Menu_order', width: 50 }

                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                tree: { columnName: 'Menu_name' },
                url: "../data/Sys_Menu.ashx?Action=GetMenu&parentid=-1",
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                },
                width: '100%',
                height: '100%',
                heightDiff: 1,
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

          


        });
        function toolbar() {
            $.getJSON("../data/toolbar.ashx?Action=GetSys&mid=76&rnd=" + Math.random(), function (data, textStatus) {
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    items.push(arr[i]);
                }

                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });
                
                $("#maingrid").ligerGetGridManager().onResize();
            });
        }

        function onSelect(note) {
            var manager = $("#maingrid").ligerGetGridManager();
            manager.showData({ Rows: [], Total: 0 });
            var url = "../data/Sys_Menu.ashx?Action=GetMenu&appid=" + note.data.id + "&rnd=" + Math.random();
            manager.GetDataByURL(url);
        }

        var activeDialog = null;
        function f_openWindow(url, title, width, height) {
            var dialogOptions = {
                width: width, height: height, title: title, url: url, buttons: [
                        {
                            text: '保存', onclick: function (item, dialog) {
                                f_save(item, dialog);
                            }
                        },
                        {
                            text: '关闭', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }


        function edit() {
            var row = $("#maingrid").ligerGetGridManager().getSelectedRow();
            var notes = $("#tree1").ligerGetTreeManager().getSelected();
            if (row != null && row != undefined && notes != null && notes != undefined) {
                f_openWindow('System/Sys_Menu_add.aspx?menuid=' + row.Menu_id + '&appid=' + notes.data.id, "修改目录", 530, 380);
            }
            else {
                $.ligerDialog.warn('请选择目录！');
            }
        }
        function add() {
            var notes = $("#tree1").ligerGetTreeManager().getSelected();
            if (notes != null && notes != undefined) {
                f_openWindow('System/Sys_Menu_add.aspx?appid=' + notes.data.id, "新增目录", 530, 380);
            }
            else {
                $.ligerDialog.warn('请选择主菜单目录！');
            }
        }

        function del() {
            var manager = $("#maingrid").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("删除后不能恢复，\n您确定要删除？", function (yes) {
                    if (yes) {
                        $.ajax({
                            type: "POST",
                            url: "../data/Sys_Menu.ashx",
                            data: { Action: 'del', menuid: row.Menu_id },
                            success: function (result) {
                                treereload();
                            }
                        });
                    }
                })
            } else {
                $.ligerDialog.warn("请选择行");
            }
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "../data/Sys_Menu.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        top.$.ligerDialog.closeWaitting();
                        treereload();

                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('操作失败！');
                    }
                });

            }
        }

        function treereload() {
            var manager = $("#maingrid").ligerGetGridManager();
            manager.loadData(true);
        }
    </script>
</head>
<body style="padding: 0px">
    <form id="form1" onsubmit="return false">
        
        <div id="layout1" style="margin-top: -1px; margin-left: -1px">
            <div position="left" title="主菜单模块">
                <div id="treediv" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto;">
                    <ul id="tree1"></ul>
                </div>
            </div>
            <div position="center" title="子菜单">
                <div id="toolbar"></div>
                <div id="maingrid" style="margin-top: -1px; margin-left: -1px"></div>
            </div>
        </div>
    </form>
</body>
</html>
