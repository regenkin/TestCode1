<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />

    <link href="../../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />  
    <link href="../../../CSS/input.css" rel="stylesheet" />

    <script src="../../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerResizable.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../../../JS/KFCRM.js" type="text/javascript"></script>
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
                    { display: '��ϵ��', name: 'C_name', width: 100 },
                    { display: 'ְ��', name: 'C_position', width: 100 },
                    { display: '�Ա�', name: 'C_sex', width: 50 },
                    { display: '������˾', name: 'C_customername', width: 180 },
                    { display: '�ֻ�', name: 'C_mob', width: 120 },
                    { display: 'QQ', name: 'C_QQ', width: 100 },
                    { display: 'Email', name: 'C_email', widht: 100 },
                    {
                        display: 'ɾ��ʱ��', name: 'Delete_time', width: 180, render: function (item) {
                            return formatTimebytype(item.Delete_time, 'yyyy-MM-dd hh:mm:ss');
                        }
                    }
                ],
                checkbox: true,
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "../../../data/CRM_Contact.ashx?Action=grid&isdel=1",
                width: '100%',
                height: '100%',
                //title: "Ա���б�",
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

        function toolbar() {
            $.getJSON("../../../data/toolbar.ashx?Action=GetSys&mid=48&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../../" + arr[i].icon;
                    items.push(arr[i]);
                }

                items.push({
                    type: 'serchbtn',
                    text: '�߼�����',
                    icon: '../../../images/search.gif',
                    disable: true,
                    click: function () {
                        serchpanel();
                    }
                });
                $("#toolbar").ligerToolBar({
                    items: items

                });
                menu = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                manager = $("#maingrid4").ligerGetGridManager();
                manager.onResize();
            });
        }
        function initSerchForm() {

        }
        function serchpanel() {
            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                manager.onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                manager.onResize();
            }
        }
        
        function doserch() {
            var sendtxt = "&Action=grid&isdel=1&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);

            $.ligerDialog.waitting('���ݲ�ѯ��,���Ժ�...');
            var manager = $("#maingrid4").ligerGetGridManager();

            manager.setURL("../../../data/CRM_Contact.ashx?" + serchtxt);
            manager.loadData(true);
            $.ligerDialog.closeWaitting();

        }

        $(document).keydown(function (e) {
            if (e.keyCode == 13 && e.target.applyligerui) {
                doserch();
            }
        });

        function doclear() {
            //var serchtxt = $("#serchform :input").reset();
            $("#serchform").each(function () {
                this.reset();
                $(".l-selected").removeClass("l-selected");
            });
        }

        

        function regain() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getCheckedRows();
            if (row.length>0) {
                var rowid = "";
                for (var i = 0; i < row.length; i++) {
                    if (i == (row.length - 1)) {
                        rowid += row[i].id;
                    }
                    else {
                        rowid += row[i].id + ",";
                    }
                }
                $.ajax({
                    url: "../../../data/CRM_Contact.ashx", type: "POST",
                    data: { Action: "regain", idlist: rowid, rnd: Math.random() },
                    success: function (responseText) {
                        if (responseText == "true") {
                            f_reload();
                        }
                        else {
                            top.$.ligerDialog.error('�ָ�ʧ�ܣ�');
                        }

                    },
                    error: function () {
                        top.$.ligerDialog.error('�ָ�ʧ�ܣ�');
                    }
                });

            }
            else {
                $.ligerDialog.warn("��ѡ����ϵ��");
            }
        }


        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getCheckedRows();
            if (row.length > 0) {
                $.ligerDialog.confirm("ɾ�����ָܻ��������������\n��ȷ��Ҫɾ����", function (yes) {
                    if (yes) {
                        var rowid = "";
                        for (var i = 0; i < row.length; i++) {
                            if (i == (row.length - 1)) {
                                rowid += row[i].id;
                            }
                            else {
                                rowid += row[i].id + ",";
                            }
                        }
                        //alert(rowid);
                        $.ajax({
                            url: "../../../data/CRM_Contact.ashx", type: "POST",
                            data: { Action: "del", idlist: rowid, rnd: Math.random() },
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
                $.ligerDialog.warn("��ѡ����ϵ��");
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
        <div id="toolbar"></div>
        
        <div id="grid">
            <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
        </div>


    </form>
    <div class="az">
        <form id='serchform'>
            <table style='width: 840px' class="bodytable1">
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>��˾���ƣ�</div>
                    </td>
                    <td>
                        <input type='text' id='company' name='company' ltype='text' ligerui='{width:120}' />
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>��ϵ�ˣ�</div>
                    </td>
                    <td>
                        <input id='contact' name="contact" type='text' ltype='text' ligerui='{width:120}' />
                    </td>
                    <td>

                        <div style='width: 60px; text-align: right; float: right'>¼��ʱ�䣺</div>
                    </td>
                    <td>

                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate' name='startdate' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate' name='enddate' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�绰��</div>
                    </td>
                    <td>
                        <input type='text' id='tel' name='tel' ltype='text' ligerui='{width:120}' />
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>QQ��</div>
                    </td>
                    <td>
                        <input id='qq' name="qq" type='text' ltype='text' ligerui='{width:120}' />

                    </td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>ɾ��ʱ�䣺</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate_del' name='startdate_del' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate_del' name='enddate_del' ltype='date' ligerui='{width:96}' />
                        </div>

                    </td>
                    <td>
                        <input id='Button2' type='button' value='����' style='width: 80px; height: 24px'
                            onclick="doclear()" />
                        <input id='Button1' type='button' value='����' style='width: 80px; height: 24px' onclick="doserch()" />
                    </td>
                    <td></td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>
