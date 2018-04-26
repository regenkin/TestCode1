<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../CSS/core.css" rel="stylesheet" type="text/css" />
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerResizable.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
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
                    { display: '��ͬ���', name: 'Serialnumber', width: 140 },
                    { display: 'Ա��', name: 'emp_name', width: 100 },
                    { display: '����', name: 'dep_name', width: 100 },
                    { display: '��λ', name: 'post_name', width: 100 },
                    { display: 'ְ��', name: 'position_name', width: 100 },
                    {
                        display: '¼������', name: 'create_date', width: 90, render: function (item) {
                            return formatTimebytype(item.create_date, "yyyy-MM-dd");
                        }
                    }

                ],
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "../data/C_hr_contract.ashx?Action=grid&rnd=" + Math.random(),
                width: '100%', height: '100%',
                heightDiff: -1,
               
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());

            $('form').ligerForm();
            initSerchForm();
            toolbar();
            //toolbar1();

        });

        var view_auth = false;
        function toolbar() {
            $.getJSON("../data/toolbar.ashx?Action=GetSys&mid=27&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    if (arr[i].text == '�鿴' && arr[i].disable)
                        view_auth = true;
                    items.push(arr[i]);
                }
                items.push({
                    type: 'textbox',
                    id: 'stext',
                    text: '������'
                });
                items.push({
                    type: 'button',
                    text: '����',
                    icon: '../images/search.gif',
                    disable: true,
                    click: function () {
                        doserch();
                    }
                });
                items.push({
                    type: 'button',
                    text: '����',
                    icon: '../images/search.gif',
                    disable: true,
                    click: function () {
                        doclear();
                    }
                });
                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });



                
                $("#maingrid4").ligerGetGridManager().onResize();
                $("#stext").ligerTextBox({ width: 200, nullText: "����ؼ�������" })
            });
        }
        function initSerchForm() {
            var e = $('#employee').ligerComboBox({ width: 96 });
            var f = $('#department').ligerComboBox({
                width: 97,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                tree: {
                    url: '../../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    parentIDFieldName: 'pid',
                    checkbox: false
                },
                onSelected: function (newvalue) {
                    $.get("../../data/hr_employee.ashx?Action=combo&did=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        e.setData(eval(data));
                    });
                }
            });
        }
        function doserch() {
            var sendtxt = "&Action=grid&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);
            var manager = $("#maingrid4").ligerGetGridManager();

            manager.setURL("../../data/C_hr_contract.ashx?" + serchtxt);
            manager.loadData(true);
        }
        function doclear() {
            //var serchtxt = $("#serchform :input").reset();
            $("#form1").each(function () {
                this.reset();
            });
        }
        $(document).keydown(function (e) {
            if (e.keyCode == 13 && e.target.applyligerui) {
                doserch();
            }
        });

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
            activeDialog = top.jQuery.ligerDialog.open(dialogOptions);
        }

        function view() {
            if (view_auth) {
                var manager = $("#maingrid4").ligerGetGridManager();
                var row = manager.getSelectedRow();
                if (row) {
                    parent.jQuery.ligerDialog.open({ width: 770, height: 490, title: "��ͬ����", url: "HR/hr_contract_add.aspx?cid=" + row.id, buttons: [{ text: '�ر�', onclick: function (item, dialog) { dialog.close(); } }] });
                } else {
                    $.ligerDialog.warn('��ѡ���У�');
                }
            }
            else {
                $.ligerDialog.warn('Ȩ�޲�����');
            }

        }
        function add() {
            f_openWindow("hr/hr_contract_add.aspx", "������ͬ", 770, 490);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {

                f_openWindow('hr/hr_contract_add.aspx?cid=' + row.id, "�޸ĺ�ͬ", 770, 490);
               
            }
            else {
                $.ligerDialog.warn('��ѡ���У�');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {      
                $.ligerDialog.confirm("ɾ�����ָܻ���\n��ȷ��Ҫɾ����", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "../data/C_hr_contract.ashx", type: "POST",
                            data: { Action: "AdvanceDelete", id: row.id, rnd: Math.random() },
                            success: function (responseText) {
                                if (responseText == "true") {
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
                $.ligerDialog.warn("��ѡ���У�");
            }
        }


        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "../data/C_hr_contract.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        $.ligerDialog.closeWaitting();
                        f_reload();
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }

        }
        function f_reload() {
            $("#maingrid4").ligerGetGridManager().loadData(true);
        };


    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">

        
        <div style="position: relative; z-index: 9999">
            <div id="toolbar"></div>
        </div>

        <div id="grid">
            <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
        </div>

    </form>

</body>
</html>
