<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../JS/Toolbar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script type="text/javascript" charset="utf-8">

        var manager = "";
        var treemanager;
        $(function () {
            $("#layout1").ligerLayout({ leftWidth: 250, allowLeftResize: false, allowLeftCollapse: true, space: 2, heightDiff: -30 });
            $("#tree1").ligerTree({
                url: '../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                onSelect: onSelect,
                idFieldName: 'id',
                //parentIDFieldName: 'pid',
                usericon: 'd_icon',
                checkbox: false,
                itemopen: false,
                onSuccess: function () {
                    $(".l-first div:first").click();
                }
            });

            treemanager = $("#tree1").ligerGetTreeManager();

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            
            toolbar();
            $("#pageloading").fadeOut(800);
        });
        function toolbar() {
            $.getJSON("../data/toolbar.ashx?Action=GetSys&mid=21&rnd=" + Math.random(), function (data, textStatus) {
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

            });
        }


        function onSelect(note) {
            $.get("../data/hr_department.ashx?Action=department&did=" + note.data.id + '&rdm=' + Math.random(), function (data, textStatus) {
                //alert(data);
                var arrstr = new Array();
                arrstr = data.split(",");

                $("#Label1").text(arrstr[1]);
                $("#Label2").text(arrstr[2]);
                $("#Label3").text(arrstr[3]);
                $("#Label4").text(arrstr[6]);
                $("#Label5").text(arrstr[4]);
                $("#Label6").text(arrstr[5]);
                $("#Label7").text(arrstr[7]);
                $("#Label8").text(arrstr[8]);
            });

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
            var notes = treemanager.getSelected();
            if (notes != null && notes != undefined) {
                f_openWindow('hr/hr_department_add.aspx?depid=' + notes.data.id, "修改部门", 620, 280);
            }
            else {
                $.ligerDialog.warn('请选择部门！');
            }
        }
        function add() {
            f_openWindow('hr/hr_department_add.aspx', "新增部门", 620, 280);
        }

        function del() {
            var manager = $("#tree1").ligerGetTreeManager();
            var node = manager.getSelected();
            if (node) {
                if (manager.isChildren(node)) {
                    $.ligerDialog.error("含有下级部门，不允许删除！");
                }
                else {
                    $.ligerDialog.confirm("部门删除后不能恢复，\n您确定要删除？", function (yes) {
                        if (yes) {
                            $.ajax({
                                url: "../data/hr_department.ashx", type: "POST",
                                data: { Action: "del", id: node.data.id, rnd: Math.random() },
                                success: function (responseText) {
                                    top.$.ligerDialog.closeWaitting();
                                    if (responseText == "true") {
                                        treereload();
                                    }
                                    else if (responseText == "false:first") {
                                        top.$.ligerDialog.error('删除失败！根目录不允许删除。');
                                    }
                                    else if (responseText == "false:emp") {
                                        top.$.ligerDialog.error('删除失败！此部门下含有员工记录。');
                                    }
                                    else if (responseText == "false:post") {
                                        top.$.ligerDialog.error('删除失败！此部门下含有岗位信息。');
                                    }
                                    else {
                                        top.$.ligerDialog.error('删除失败！');
                                    }

                                },
                                error: function () {
                                    top.$.ligerDialog.closeWaitting();
                                    top.$.ligerDialog.error('删除失败！');
                                }
                            });
                        }
                    })
                }

            } else {
                $.ligerDialog.warn("请选择部门！");
            }
        }

        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('数据保存中,请稍候...');
                $.ajax({
                    url: "../data/hr_department.ashx", type: "POST",
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
            treemanager = $("#tree1").ligerGetTreeManager();
            treemanager.clear();
            treemanager.FlushData();
        }
    </script>
</head>
<body style="padding: 0px; overflow: hidden;">
    <form id="form1" onsubmit="return false">
        <div id="toolbar"></div>
        <div id="layout1" style="margin: -1px">
            <div position="left" title="组织架构">
                <div id="treediv" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto;">
                    <ul id="tree1"></ul>
                </div>
            </div>
            <div position="center">

                <table class="bodytable0" style="width: 100%; margin: -1px">

                    <tr>
                        <td height="23" width="20%" class="table_label">名称：</td>
                        <td height="23">
                            <span id="Label1">1</span>
                        </td>
                    </tr>
                    <tr>
                        <td height="23" class="table_label">负责人：</td>
                        <td height="23">
                            <span id="Label2">2</span>
                        </td>
                    </tr>
                    <tr>
                        <td height="23" class="table_label">电话：</td>
                        <td height="23">
                            <span id="Label3">3</span>
                        </td>
                    </tr>
                    <tr>
                        <td height="23" class="table_label">邮箱：</td>
                        <td height="23">
                            <span id="Label4">4</span>
                        </td>
                    </tr>
                    <tr>
                        <td height="23" class="table_label">传真：</td>
                        <td height="23">
                            <span id="Label5">5</span>
                        </td>
                    </tr>
                    <tr>
                        <td height="23" class="table_label">地址：</td>
                        <td height="23">
                            <span id="Label6">6</span>
                        </td>
                    </tr>
                    <tr>
                        <td height="23" class="table_label">描述：</td>
                        <td height="23">
                            <span id="Label7">7</span>
                        </td>
                    </tr>
                    <tr>
                        <td height="23" class="table_label">排序：</td>
                        <td height="23">
                            <span id="Label8">8</span>
                        </td>
                    </tr>
                </table>


            </div>
        </div>



    </form>
</body>
</html>
