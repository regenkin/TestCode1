<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=8" />
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


            $("#maingrid5").ligerGrid({
                columns: [
                        { display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                        { display: '日志类型', name: 'EventType', width: 120 },
                        { display: '标识', name: 'EventID', width: 40 },
                        { display: '标题', name: 'EventTitle', width: 250 },
                        { display: '操作人', name: 'UserName', width: 120 },
                        {
                            display: '操作时间', name: 'EventDate', width: 180, render: function (item) {
                                return formatTime(item.EventDate);
                            }
                        },
                        { display: 'IP', name: 'IPStreet', width: 160 }

                ],
                onAfterShowData: function () {
                    $(".abc").hover(function (e) {
                        $(this).ligerTip({ content: $(this).html(), width: 150, distanceX: event.clientX - $(this).offset().left - $(this).width() + 15 });
                    }, function (e) {
                        $(this).ligerHideTip(e);
                    });
                },
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                //checkbox:true,
                url: "../data/Sys_log.ashx?Action=grid",
                width: '100%', height: '100%',
                heightDiff: -1,
               
               
                detail: {
                    onShowDetail: function (r, p) {
                        for (var n in r) {
                            if (r[n] == null) r[n] = "";
                        }
                        $(p).append(
                            "<table class='bodytable0'  style='width:99%;margin:5px'>" +
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>日志类型：</td><td height='23'  width='15%'>" + r.EventType + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>标识：</td><td height='23'  width='15%'>" + r.EventID + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>标题：</td><td height='23'  width='15%'>" + r.EventTitle + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>IP：</td><td height='23'  width='15%'>" + r.IPStreet + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>操作人：</td><td height='23' >" + r.UserName + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>原内容：</td><td height='23'  colspan='5'>" + r.Original_txt + "</td>" +
                                "</tr>" +
                                "<tr>" +
                                    "<td height='23' width='10%' class='table_label'>操作时间：</td><td height='23' >" + formatTimebytype(r.EventDate, 'yyyy-MM-dd') + "</td>" +
                                    "<td height='23' width='10%' class='table_label'>当前内容：</td><td height='23'  colspan='5'>" + r.Current_txt + "</td>" +

                                "</tr>" +
                            "</table>");
                    }
                }
            });
            $("#toolbar").ligerToolBar({
                items: [
                { type: 'textbox', id: 'stype', text: '类型：' },
                { type: 'textbox', id: 'sstart', text: '时间：' },
                { type: 'textbox', id: 'sdend', text: "" },
                { type: 'textbox', id: 'stext', text: '关键字：' },
                { type: 'button', text: '智能搜索', icon: '../../images/search.gif', disable: true, click: function () { doserch() } },
                { type: 'button', text: '重置', icon: '../../images/edit.gif', disable: true, click: function () { $("#serchform").each(function () { this.reset(); }); } }
                ]
            });



            $("#stype").ligerComboBox({ width: 100, url: "../data/Sys_log.ashx?Action=logtype" })
            $("#sstart").ligerDateEditor({ width: 100 })
            $("#sdend").ligerDateEditor({ width: 100 })
            $("#stext").ligerTextBox({ width: 200 })

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());
            

            $("#maingrid5").ligerGetGridManager().onResize();


        });
        function doserch() {
            var sendtxt = "&Action=grid&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);
            var manager = $("#maingrid5").ligerGetGridManager();

            manager.setURL("../data/Sys_log.ashx?" + serchtxt);
            manager.loadData(true);
        }
        function f_reload() {
            var manager = $("#maingrid5").ligerGetGridManager();
            manager.loadData(true);
        };


    </script>
</head>
<body>
    <div style="position: relative; z-index: 9999">
        <form id="serchform">
            <div id="toolbar"></div>
        </form>
    </div>

    <form id="form1" onsubmit="return false">
        <div id="griddiv">

            
            <div id="maingrid5" style="margin: -1px -1px;"></div>
        </div>
    </form>


</body>
</html>
