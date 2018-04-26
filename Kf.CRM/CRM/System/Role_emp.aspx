<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/core.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../JS/Toolbar.js" type="text/javascript"></script>
    <script src="../lib/json2.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var str1 = getparastr("rid");
            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: 'ID', name: 'ID', type: 'int', width: 50 },
                    { display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    { display: '名字', name: 'name' },
                    { display: '性别', name: 'sex', width: 50 },
                    { display: '部门', name: 'dname' },
                    { display: '职务', name: 'zhiwu' }
                ],
                checkbox: true,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "../data/Sys_role_emp.ashx?Action=get&role_id=" + str1,
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: -1
            });
            var items = [];
            items.push({ type: 'button', text: '添加', icon: '../../images/add.gif', disable: true, click: function () { add_emp(); } });
            items.push({ type: 'button', text: '移除', icon: '../../images/delete.gif', disable: true, click: function () { remove_emp(); } });
            items.push({ type: 'textbox', id: 'stext', text: '姓名：' });
            items.push({ type: 'button', text: '搜索', icon: '../images/search.gif', disable: true, click: function () { doserch() } });
            $("#serchbar1").ligerToolBar({
                items: items
            })
            $("#stext").ligerTextBox({ width: 200 })
        });


        function add_emp() {
            top.$.ligerDialog.open({
                title: '选择联系人', width: 600, height: 300, url: 'system/getemp.aspx?role_id=' + getparastr("rid"), buttons: [
                    { text: '确定', onclick: f_selectContactOK },
                    { text: '取消', onclick: function (item, dialog) { dialog.close(); } }
                ], zindex: 9003
            });
            return false;
        }

        function f_selectContactOK(item, dialog) {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = dialog.frame.f_select();
            if (rows.length < 1) {
                alert('请选择行!');
                return;
            }
            var rid = "";
            for (var i = 0; i < rows.length - 1; i++) {
                rid += rows[i].ID + ",";
            }
            rid += rows[rows.length - 1].ID;
            $.ajax({
                type: "POST",
                url: "../data/Sys_role_emp.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: "add", empids: rid, role_id: getparastr("rid") }, /* 注意参数的格式和名称 */
                success: function (result) {
                    manager.loadData(true);
                }
            });
            //alert(rows.length);
            dialog.close();
        }

        function remove_emp() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getCheckedRows();
            if (row.length < 1) {
                $.ligerDialog.warn("请选择行！", "提示");
                return;
            }
            $.ligerDialog.confirm("此操作不可恢复，确定删除？", function (yes) {
                if (!yes) {
                    alert();
                }
                else {
                    var rid = "";
                    for (var i = 0; i < row.length - 1; i++) {
                        rid += row[i].ID + ",";
                    }
                    rid += row[row.length - 1].ID;
                    $.ajax({
                        type: "POST",
                        url: "../data/Sys_role_emp.ashx", /* 注意后面的名字对应CS的方法名称 */
                        data: { Action: "remove", empids: rid, role_id: getparastr("rid") }, /* 注意参数的格式和名称 */
                        success: function (result) {
                            manager.loadData(true);
                            $(".l-checked").removeClass("l-checked");
                        }
                    });
                }
            })
        }
        //查询
        function doserch() {
            var sendtxt = "&Action=get&role_id=" + getparastr("rid") + "&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);           
            var manager = $("#maingrid4").ligerGetGridManager();

            manager.setURL("../data/Sys_role_emp.ashx?" + serchtxt);
            manager.loadData(true);
        }

    </script>

</head>
<body>

    <form id="form1" onsubmit="return false">
        <div>
            <div id="serchbar1"></div>

            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>

</body>
</html>
