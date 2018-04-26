<%@ Page Language="C#" AutoEventWireup="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../../CSS/core.css" rel="stylesheet" type="text/css" />
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>

    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
   <script src="../../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../lib/json.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>

    <script src="../../JS/echarts-all.js" type="text/javascript"></script>
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
                    { display: '编号', width: 40, render: function (item, i) { return i + 1; } },
                    {
                        display: '员工名字', name: 'yy', width: 120, render: function (item) {
                            if (!item.yy)
                                item.yy = "【系统】未分类";
                            return item.yy;
                        }
                    },
                    {
                        display: '时间一', name: 'dt1', width: 120, render: function (item) {
                            if (typeof (item.dt1) == "number" && item.dt1 != "0")
                                return item.dt1;
                            else
                                return "0";
                        }
                    },
                    {
                        display: '时间二', name: 'dt2', width: 120, render: function (item) {
                            if (typeof (item.dt2) == "number" && item.dt2 != "0")
                                return item.dt2;
                            else
                                return "0";
                        }
                    },
                    {
                        display: '增幅', name: 'm3', width: 120, render: function (item) {
                            var dt1, dt2;
                            if (typeof (item.dt1) == "number" && item.dt1 != "0")
                                dt1 = item.dt1;
                            else
                                dt1 = 0;
                            if (typeof (item.dt2) == "number" && item.dt2 != "0")
                                dt2 = item.dt2;
                            else
                                dt2 = 0;
                            if (dt1 > dt2)
                                return "↓ " + (dt2 - dt1);
                            else
                                return "↑ " + (dt2 - dt1);
                        }
                    },
                    {
                        display: '比例', name: 'm4', width: 120, render: function (item) {
                            var dt1, dt2;
                            if (typeof (item.dt1) == "number" && item.dt1 != "0")
                                dt1 = item.dt1;
                            else
                                dt1 = 0;
                            if (typeof (item.dt2) == "number" && item.dt2 != "0")
                                dt2 = item.dt2;
                            else
                                dt2 = 0;
                            if (dt1 == 0)
                                return "--";
                            if (dt1 > dt2)
                                return "↓ " + Math.round((dt2 - dt1) * 100 / dt1) + " %";
                            else
                                return "↑ " + Math.round((dt2 - dt1) * 100 / dt1) + " %";
                        }
                    }
                ],
                url: '../../data/Crm_contract.ashx',
                usePager: false,
                //dataAction: 'local', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                width: '100%', height: '100%',
                title: "年度统计表",
                heightDiff: -6
            });

            var items = [];
            items.push({ type: 'textbox', id: 'tree1', text: '部门：' });
            items.push({ type: 'textbox', id: 'year1', text: '时间1：' });
            items.push({ type: 'textbox', id: 'month1', text: '年' });
            items.push({ type: 'text', text: '月' });
            items.push({ type: 'text', text: '  -  ' });
            items.push({ type: 'textbox', id: 'year2', text: '时间2：' });
            items.push({ type: 'textbox', id: 'month2', text: '年' });
            items.push({ type: 'text', text: '月' });
            items.push({ type: 'line' });

            items.push({ type: 'button', text: '统计', icon: '../../images/search.gif', disable: true, click: function () { doserch() } });
            items.push({ type: 'button', text: '重置', icon: '../../images/edit.gif', disable: true, click: function () { $("#serchform").each(function () { this.reset(); $(".l-selected").removeClass("l-selected"); }); } });

            $("#toolbar").ligerToolBar({
                items: items
            });

            initSelect();

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height() - 350);
            $("#maingrid5").ligerGetGridManager().onResize();

            var d = new Date();
            var nowYear = +d.getFullYear();
            var nowMonth = d.getMonth();
            var syearData = [], smonthDate = [];
            for (var i = nowYear; i >= nowYear - 20; i--) {
                syearData.push({ 'text': i, 'id': i });
            }
            for (var i = 1; i <= 12; i++) {
                smonthDate.push({ 'text': i, 'id': i });
            }
            $("#year1").ligerComboBox({ width: 60, data: syearData, initValue: nowYear - 1 });
            $("#year2").ligerComboBox({ width: 60, data: syearData, initValue: nowYear });
            $("#month1").ligerComboBox({ width: 60, data: smonthDate, initValue: nowMonth });
            $("#month2").ligerComboBox({ width: 60, data: smonthDate, initValue: nowMonth });

            //doserch();
        });

        function initSelect() {

            var d = new Date();
            var nowYear = +d.getFullYear();
            var syearData = [];
            for (var i = nowYear; i >= nowYear - 20; i--) {
                syearData.push({ 'text': i, 'id': i });
            }

            $('#tree1').ligerComboBox({
                width: 200,
                selectBoxWidth: 240,
                selectBoxHeight: 200,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: true,
                //checkbox: true,
                tree: {
                    url: '../../data/Base.ashx?Action=getUserTree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    //parentIDFieldName: 'pid',
                    checkbox: true
                },
                onSelected: function (newvalue, newtext, newid) {

                    //alert($("#tree1_val").val())

                }
            });





        }

        function test(GridData) {
            var series = [];
            var legend = [];

            var dc1 = 0, dc2 = 0;
            for (var i = 0; i < GridData.Rows.length; i++) {
                var yy = GridData.Rows[i].yy;
                if (!yy)
                    yy = "未分类";

                var dt1 = typeof (GridData.Rows[i].dt1) != 'number' ? 0 : GridData.Rows[i].dt1;
                var dt2 = typeof (GridData.Rows[i].dt2) != 'number' ? 0 : GridData.Rows[i].dt2;

                series.push({ "name": yy, "type": "bar", "data": [dt1, dt2] });
                dc1 += dt1; dc2 += dt2;
                legend.push(yy);
            }

            series.push({ "name": '总计', "type": "line", "data": [dc1, dc2] });
            legend.push("总计");
            var myChart = echarts.init(document.getElementById('graph'));

            var option = {
                tooltip: {
                    show: true
                },
                legend: {
                    data: legend
                },
                xAxis: [
                    {
                        type: 'category',
                        data: [
                            $("#year1").val() + "年" + $("#month1").val() + "月",
                            $("#year2").val() + "年" + $("#month2").val() + "月"
                        ]
                    }
                ],
                yAxis: [
                    {
                        type: 'value'
                    }
                ],
                series: series,

                grid: {
                    x: 40,
                    y: 20,
                    x2: 10
                }
            };
            // 为echarts对象加载数据 
            myChart.setOption(option);
            $("#graph").css({ "filter": "alpha(opacity=100)", "background": "#fff" });
        }
        function doserch() {
            var sendtxt = "&Action=Compared_empcuscontract&idlist=" + $("#tree1_val").val() + "&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);
            var manager = $("#maingrid5").ligerGetGridManager();

            top.$.ligerDialog.waitting('数据查询中,请稍候...');
            $.ajax({
                url: "../../data/Crm_contract.ashx", type: "POST",
                data: serchtxt,
                dataType: 'json',
                beforeSend: function () {
                    manager.showData({ Rows: [], Total: 0 });
                },
                success: function (responseText) {
                    manager.setURL("../../data/Crm_contract.ashx?" + serchtxt);
                    manager.showData(responseText);
                    top.$.ligerDialog.closeWaitting();
                    test(responseText);
                },
                error: function () {
                    top.$.ligerDialog.closeWaitting();
                    top.$.ligerDialog.error('查询失败！请检查查询项。');
                }
            });
            manager.changeHeaderText('dt1', $("#year1").val() + "年" + $("#month1").val() + "月");
            manager.changeHeaderText('dt2', $("#year2").val() + "年" + $("#month2").val() + "月");
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
            <div id="graph" style="height: 280px; margin: 5px;"></div>
            
            <div id="maingrid5" style="margin: -1px -1px;"></div>
        </div>
    </form>


</body>
</html>
