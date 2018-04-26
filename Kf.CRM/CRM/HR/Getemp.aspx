<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/core.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {   
            $("#maingrid4").ligerGrid({
                columns: [
                    //{ display: 'ID', name: 'ID', type: 'int', width: 50 },
                    { display: '���', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    { display: '����', name: 'name' },
                    { display: '�Ա�', name: 'sex', width: 50 },
                    { display: '����', name: 'dname' },
                    { display: 'ְ��', name: 'zhiwu' }
                ],
                checkbox: false,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "../data/hr_employee.ashx?Action=grid",
                width: '100%',
                height: '100%',
                //title: "Ա���б�",
                heightDiff: 0
            });
            toolbar();
        });

        function f_select() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getSelectedRow();
            //alert(rows);
            return rows;
        }

        function toolbar() {            
                var items = [];                
                items.push({ type: 'button', text: '����Ա��', icon: '../images/icon/11.png', disable: true, click: function () { addemp() } });
                items.push({ type: 'textbox', id: 'stext', text: '������' });
                items.push({ type: 'button', text: '����', icon: '../images/search.gif', disable: true, click: function () { doserch() } });

                $("#toolbar").ligerToolBar({
                    items: items

                });  
            
                $("#stext").ligerTextBox({ width: 200, nullText: "������������" })
                $("#maingrid4").ligerGetGridManager().onResize();
           
        }
        //��ѯ
        function doserch() {
            var sendtxt = "&Action=grid&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);           
            var manager = $("#maingrid4").ligerGetGridManager();

            manager.setURL("../data/hr_employee.ashx?" + serchtxt);
            manager.loadData(true);
        }
        var activeDialog = null;
        function f_openWindow(url, title, width, height) {
            var dialogOptions = {
                width: width, height: height, title: title, url: url, buttons: [
                        { text: '����', onclick: f_save },
                        {
                            text: '�ر�', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ] ,zindex:9005,isResize: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }

       
        function addemp() {
            f_openWindow("hr/QuickAddEmp.aspx", "�����˺�", 730, 430);
        }
    
        function f_save(item, dialog) {             
                var issave = dialog.frame.f_save();                 
                if (issave) {
                    dialog.close();
                    $.ajax({
                        url: "../data/hr_employee.ashx", type: "POST",
                        data: issave + "&PostData=[]",
                        success: function (responseText) {                            
                            f_reload();
                        },
                        error: function () {                            
                            alert('����ʧ�ܣ�');
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
    <div>
        <div id="toolbar"></div> 
        <div id="maingrid4" style="margin:-1px; "></div>   
    </div>     
  </form>

 
</body>
</html>
