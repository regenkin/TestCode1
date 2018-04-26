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
                itemopen: false,
                onSuccess: function () {
                    $(".l-first").click();
                }
            });

            treemanager = $("#tree1").ligerGetTreeManager();

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '���', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return rowid + 1; } },
                    { display: '�˵�����', name: 'Menu_name', align: 'left',width:190 },
                    {
                        display: '��ťȨ��', name: 'Menu_icon', align: 'left', width: 550, render: function (item) {
                            //var html = "<div style='vertical-align:middle;color: #FF0000;background:yellow'>";
                            var html = "<div style='vertical-align:middle;'>";
                            var returntxt = item.Sysroler;

                            var arrstr = new Array();
                            var arrstr1 = new Array();
                            var arrid = new Array();
                            var arrname = new Array();
                            arrstr = returntxt.split(",");

                            for (var j = 0; j < arrstr.length - 1; j++) {
                                arrstr1 = arrstr[j].split("|");
                                arrid = arrstr1[0];
                                arrname = arrstr1[1];
                                html += "<input type='checkbox' ";
                                html += "value='" + arrid + "' ";
                                html += " id='b" + arrid + "'";
                                html += ">";
                                html += arrname;
                                html += "&nbsp;&nbsp;";
                            }
                            html += "</div>";
                            return html;
                        }
                    }

                ],
                rowid: "Menu_id",
                onLoaded:f_setbtn,
                onCheckRow: f_onCheckRow,
                onCheckAllRow: f_onCheckAllRow,
                dataAction: 'local',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                enabledEdit: true,
                checkbox: true,
                url: "../data/System_role_data.ashx?action=treegrid",
                width: '100%', height: '100%',
                usePager: false,
                tree: { columnName: 'Menu_name' },
                heightDiff: -3
            });
            

            var manager = $("#maingrid4").ligerGetGridManager();
            manager.onResize();
            $("tbody> :checkbox").attr("checked", false);


        });

        function onSelect(note) {
            //��������
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.showData({ Rows: [], Total: 0 });
            var url = "../data/Sys_role.ashx?Action=treegrid&appid=" + note.data.id + '&rdm=' + Math.random();
            manager.GetDataByURL(url);            
            //ִ�в���
        }
        function f_setbtn()
        {
           
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.gridloading.hide();
            var note = treemanager.getSelected();
            if (!note) return;
            
            //��ȡȨ��
            var roleid = getparastr("Role_id");
            var savetext = "{role_id:'" + roleid + "',";
            savetext += "app:'" + note.data.id + "'}";

            $.ajax({
                type: 'post',
                url: "../data/Sys_role.ashx?Action=getauth&postdata=" + savetext + '&rdm=' + Math.random(),
                success: function (data) {
                    //alert(data);   
                    var arrstr = new Array();
                    var arrmenu = new Array();
                    var arrbtn = new Array();
                    arrstr = data.split("|");

                    arrmenu = arrstr[0].split(",");
                    for (var i = 0; i < arrmenu.length; i++) {
                        if (arrmenu[i].length > 0) {
                            manager.setCheck(arrmenu[i].replace("m", ""));
                        }
                    }

                    if (arrstr[1])
                        arrbtn = arrstr[1].split(",");
                    for (var j = 0; j < arrbtn.length; j++) {
                        if (arrbtn[j].length > 0) {
                            $("#" + arrbtn[j]).attr("checked", true);
                        }
                    }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    //f_error("��ɫ��δ��Ȩ��");
                }
            });
        }

        function f_save() {
            var notes = treemanager.getSelected();
            if (notes != null && notes != undefined) {
                var app = notes.data.id;

                var manager = $("#maingrid4").ligerGetGridManager();
                var rows = manager.getCheckedRows();
                var menu = "";
                $(rows).each(function () {
                    menu += "m" + this.Menu_id + ",";
                });

                var btn = "";
                $("input[type='checkbox']").each(function (i) {
                    if ($(this).attr("checked")) {
                        btn += $(this).attr("id") + ",";
                    }
                })


                var roleid = getparastr("Role_id");
                var savetext = "{role_id:'" + roleid + "',";
                savetext += "app:'a" + app + ",',";
                savetext += "menu:'" + menu + "',";
                savetext += "btn:'" + btn + "'}";

                //alert(savetext);

                f_saving();
                $.ajax({
                    type: 'post',
                    url: "../data/Sys_role.ashx?Action=saveauth&postdata=" + savetext + '&rdm=' + Math.random(),
                    success: function (data) {
                        //alert(data);
                        setTimeout(function () {
                            f_success();
                        }, 10);

                    },
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        f_error("��Ȩʧ�ܣ�");
                    }
                });
            }
            else {
                f_error("��ѡ��ģ�飡");
            }

        }
        function EditMenu() {
            $(":checkbox").attr("checked", true);
        }
        function DeleteMenu() {
            $(":checkbox").attr("checked", false);
            $("#_38").attr("checked", true);
        }
        function gridreload(Appid) {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.showData({ Rows: [], Total: 0 });
            //alert('onSelect:' + note.data.id);
            var url = "../data/Sys_Menu_data.aspx?action=treegrid&appid=" + Appid;
            manager.GetDataByURL(url);
        };

        function f_onCheckRow(checked, data) {
            var returntxt = data.Sysroler;
            var arrstr = new Array();
            var arrstr1 = new Array();
            var arrid = new Array();
            var arrname = new Array();
            arrstr = returntxt.split(",");

            if (checked) {
                for (var j = 0; j < arrstr.length; j++) {
                    arrstr1 = arrstr[j].split("|");
                    arrid = arrstr1[0];
                    $("#b" + arrid).attr("checked", true);
                }
                //parent
                var manager = $("#maingrid4").ligerGetGridManager();
                if (manager.isLeaf(data)) {
                    var row = manager.getParent(data);
                    if(row) manager.setCheck(row.Menu_id);
                }
                else
                {
                    var rows = data.children;
                    if (!rows) return;
                    $(rows).each(function (i,item) {
                        manager.setCheck(rows[i].Menu_id);
                        var returntxt = item.Sysroler;
                        var arrstr = new Array();
                        var arrstr1 = new Array();
                        var arrid = new Array();
                        var arrname = new Array();
                        arrstr = returntxt.split(",");


                        for (var j = 0; j < arrstr.length; j++) {
                            arrstr1 = arrstr[j].split("|");
                            arrid = arrstr1[0];
                            if (checked) {
                                $("#b" + arrid).attr("checked", true);
                            }
                            else {
                                $("#b" + arrid).attr("checked", false);
                            }
                        }
                    })
                }
            }
            else {
                for (var j = 0; j < arrstr.length; j++) {
                    arrstr1 = arrstr[j].split("|");
                    arrid = arrstr1[0];
                    $("#b" + arrid).attr("checked", false);
                }
                //children
                var manager = $("#maingrid4").ligerGetGridManager();
                if (!manager.isLeaf(data)) {
                    var rows = data.children;
                    $(rows).each(function (i) {
                        manager.setUnCheck(rows[i].Menu_id);
                    })
                }
            }


        }
        function f_onCheckAllRow(checked, grid) {
            var manager = $("#maingrid4").ligerGetGridManager();
            var rows = manager.getAllRows();
            if (rows.length > 0) {
                $(rows).each(function (i, item) {
                    //alert(i+";"+item.Sysroler)
                    var returntxt = item.Sysroler;
                    var arrstr = new Array();
                    var arrstr1 = new Array();
                    var arrid = new Array();
                    var arrname = new Array();
                    arrstr = returntxt.split(",");


                    for (var j = 0; j < arrstr.length; j++) {
                        arrstr1 = arrstr[j].split("|");
                        arrid = arrstr1[0];
                        if (checked) {
                            $("#b" + arrid).attr("checked", true);
                        }
                        else {
                            $("#b" + arrid).attr("checked", false);
                        }
                    }

                })
            }
        }
        function f_success() {
            setTimeout(function () {
                $.ligerDialog.confirm("�Ƿ�����༭", "����ɹ�", function (ok) {
                    $.ligerDialog.closeWaitting();
                    if (!ok) {
                        parent.$.ligerDialog.close();
                    }
                });
            }, 200);
        }
        function f_error(message) {
            $(document).ready(function () {
                $.ligerDialog.error(message);
            });
        }
        function f_saving() {
            $.ligerDialog.waitting("���ڱ�����...");
        }

    </script>

</head>
<body style="padding: 0px;overflow:hidden;">
    <div id="layout1" style="margin: -1px;">
        <div position="left" title="ϵͳĿ¼">
            <div id="treediv1" style="width: 250px; height: 100%; margin: -1px; float: left; border: 1px solid #ccc; overflow: auto;">
                <ul id="tree1"></ul>
            </div>
        </div>
        <div position="center">
            <div class="title">����</div>
            
            <div id="maingrid4" style="margin: -1px;"></div>
        </div>
    </div>
</body>
</html>
