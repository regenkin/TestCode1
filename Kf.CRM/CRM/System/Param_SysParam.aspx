<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
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
                url: '../data/Param_SysParam.ashx?Action=GetApp&rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                parentIDFieldName: 'pid',
                checkbox: false,
                itemopen: false,
                onSuccess: function () {
                    $(".l-first div:first").click();
                }
            });

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid").ligerGrid({
                columns: [
                    { display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    //{ display: 'pid', name: 'parentid', width: 50 },
                    { display: '参数名', name: 'params_name' },
                    { display: '排序', name: 'params_order', width: 50 }

                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],

                url: "../data/Sys_Menu.ashx?Action=GetParams&parentid=-1",
                width: '100%',
                height: '100%',
                heightDiff: 0,
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });
            toolbar();
            


        });
        function toolbar() {
            $.getJSON("../data/toolbar.ashx?Action=GetSys&mid=43&rnd=" + Math.random(), function (data, textStatus) {
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
                
                $("#maingrid").ligerGetGridManager().onResize();
            });

        }


        function onSelect(note) {
            var manager = $("#maingrid").ligerGetGridManager();
            manager.showData({ Rows: [], Total: 0 });
            var url = "../data/Param_SysParam.ashx?Action=GetParams&parentid=" + note.data.id + "&rnd=" + Math.random();
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
                ], isResize: true, showToggle: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }


        function edit() {
            var row = $("#maingrid").ligerGetGridManager().getSelectedRow();
            if (row != null && row != undefined) {
                f_openWindow('System/Param_SysParam_add.aspx?paramid=' + row.id, "修改参数", 430, 280);
            }
            else {
                $.ligerDialog.warn('请选择参数！');
            }
        }
        function add() {
            var notes = $("#tree1").ligerGetTreeManager().getSelected();
            if (notes != null && notes != undefined) {
                f_openWindow('System/Param_SysParam_add.aspx?parentid=' + notes.data.id, "新增参数", 430, 280);
            }
            else {
                $.ligerDialog.warn('请选择参数类别！');
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
                            url: "../data/Param_SysParam.ashx",
                            data: { Action: 'del', paramid: row.id, parentid: row.parentid },
                            success: function (result) {
                                if (result == "true") {
                                    treereload();
                                }
                                else if (result == "false")
                                {
                                    $.ligerDialog.warn("删除失败！");
                                }
                                else if (result == "false:data")
                                {
                                    $.ligerDialog.warn("含有关联数据，不允许删除！");
                                }
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
                    url: "../data/Param_SysParam.ashx", type: "POST",
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
<body style="padding: 0px;overflow:hidden;">
    <form id="form1" onsubmit="return false">
        
        <div id="layout1" style="margin-top: -1px; margin-left: -1px">
            <div position="left" title="参数类别">
                <div id="treediv" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto;">
                    <ul id="tree1"></ul>
                </div>
            </div>
            <div position="center" title="参数信息">
                <div id="toolbar"></div>
                <div id="maingrid" style="margin-top: -1px; margin-left: -1px"></div>
            </div>
        </div>
    </form>
</body>
</html>
