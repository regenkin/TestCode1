<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/core.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        var checked_id = [];
        var checked_name = [];
        $(function () {
            var emp_ids = getparastr("emp_ids");
            if (emp_ids) {
                var ids = new Array();
                ids = emp_ids.split(";");
                for (var i = 0; i < ids.length - 1; i++) {
                    checked_id.push(ids[i]);
                }
            }
            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: 'ID', name: 'ID',  width: 50 },
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
                url: "../data/hr_employee.ashx?Action=grid",
                width: '100%',
                height: '100%',
                isChecked: f_isChecked,
                onCheckRow: f_onCheckRow,
                //title: "员工列表",
                heightDiff: -1
            });
            


        });

        function f_select() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getCheckedRows();
            //alert(rows);
            return rows;
        }

        function findCheckedCustomer(CustomerID) {
            for (var i = 0; i < checked_id.length; i++) {
                if (checked_id[i] == "e" + CustomerID)
                    return i;
            }
            return -1;
        }
        function findCheckedName(CustomerName) {
            for (var i = 0; i < checked_name.length; i++) {
                if (checked_name[i] == CustomerName)
                    return i;
            }
            return -1;
        }
        function addCheckedCustomer(CustomerID, CustomerName) {
            if (findCheckedCustomer(CustomerID) == -1) {
                checked_id.push("e" + CustomerID);
            }
            if (findCheckedName(CustomerName) == -1) {
                checked_name.push(CustomerName);
            }
        }
        function removeCheckedCustomer(CustomerID) {
            var i = findCheckedCustomer(CustomerID);
            if (i == -1) return;
            checked_id.splice(i, 1);
            checked_name.splice(i, 1);
        }
        function f_isChecked(rowdata) {
            if (findCheckedCustomer(rowdata.ID) == -1) {
                removeCheckedCustomer(rowdata.ID)
                return false;
            }
            else {
                addCheckedCustomer(rowdata.ID, rowdata.name);
                return true;
            }
        }
        function f_onCheckRow(checked, data) {
            if (checked)
                addCheckedCustomer(data.ID, data.name);
            else
                removeCheckedCustomer(data.ID, data.name);
        }
        function f_getChecked() {
            //alert(checked_id.join(';')+checked_name.join(';'));
            var ids = checked_id.join(';') + ";";
            var names = checked_name.join(';') + ";";
            var emp_info = [];
            emp_info.push({
                emp_ids: ids,
                emp_list: names
            })
            return emp_info;
        }
    </script>

</head>
<body>

    <form id="form1" onsubmit="return false">
        <div>
            
            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </form>


</body>
</html>
