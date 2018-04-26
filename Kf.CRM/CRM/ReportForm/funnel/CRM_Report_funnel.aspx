<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
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

    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>

    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../lib/json.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>

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
                    { display: '选项', name: 'CustomerType', width: 120 },
                    { display: '统计', name: 'cc', width: 80, align: 'right' },
                    {
                        display: '预期比例', name: 'rate1', width: 150, render: function (item) {
                            var html = "<div style='background:#f00;float:right;text-align:right;";
                            html += "width:" + item.rate1 + "'>";
                            html += item.rate1;
                            html += "</div>";
                            return html;
                        }
                    },
                    {
                        display: '实际比例', name: 'rate2', width: 150, render: function (item) {
                            var html = "<div style='background:#0f0;text-align:left;";
                            html += "width:" + item.rate2 + "'>";
                            html += item.rate2;
                            html += "</div>";
                            return html;
                        }
                    }
                ],
                url: '../../data/Reports_CRM.ashx',
                usePager: false,
                //dataAction: 'local', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                width: '100%', height: '100%',
                title: "年度统计表",
                heightDiff: -6
            });

            $("#toolbar").ligerToolBar({
                items: [
                    { type: 'textbox', id: 'stype', text: '统计项:' },
                    { type: 'textbox', id: "syear", text: "年度：" },
                    { type: 'button', text: '统计', icon: '../../images/search.gif', disable: true, click: function () { doserch(); } },
                    { type: 'button', text: '重置', icon: '../../images/edit.gif', disable: true, click: function () { $("#serchform").each(function () { this.reset(); $(".l-selected").removeClass("l-selected"); }); } }
                ]
                //激活哪个
            });
            initSelect();

            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height() - 350);


            //$("#maingrid5").ligerGetGridManager().onResize();
            doserch();
        });

        function initSelect() {
            $("#stype").ligerComboBox({
                width: 200,
                isMultiSelect: true,
                url: "../../data/param_sysparam.ashx?Action=combo&parentid=1&rnd=" + Math.random()
            })
            var d = new Date();
            var nowYear = +d.getFullYear();
            var syearData = [];
            for (var i = nowYear; i >= nowYear - 20; i--) {
                syearData.push({ 'text': i, 'id': i });
            }
            $("#syear").ligerComboBox({
                width: 100,
                data: syearData,
                initValue: nowYear

            })

        }

        function test(GridData) {
            var series = [], series1 = [];
            var legend = [];

            var total = GridData.Rows.length;
            var count = 0;
            for (var i = 0; i < GridData.Rows.length; i++) {
                var row = GridData.Rows[i];
                count = count + row.cc;
                var CustomerType = GridData.Rows[i].CustomerType;
                series.push({ name: row.CustomerType, value: ((total - i) * 100 / total).toFixed(2) });
                GridData.Rows[i].rate1 = ((total - i) * 100 / total).toFixed(2) + "%";
                legend.push(CustomerType);
            }
            for (var i = 0; i < GridData.Rows.length; i++) {
                var row = GridData.Rows[i];
                var CustomerType = GridData.Rows[i].CustomerType;
                series1.push({ name: row.CustomerType, value: (row.cc * 100 / count).toFixed(2) });
                GridData.Rows[i].rate2 = (row.cc * 100 / count).toFixed(2) + "%";
            }

            var manager = $("#maingrid5").ligerGetGridManager();
            manager.showData(GridData);

            var myChart = echarts.init(document.getElementById('graph'));

            var option = {
                title: {
                    text: '漏斗图'
                },

                legend: {
                    data: legend
                },
                calculable: true,
                series: [
                    {
                        name: '预期',
                        type: 'funnel',
                        x: '10%',
                        width: '40%',
                        itemStyle: {
                            normal: {
                                label: {
                                    formatter: '{b}'
                                },
                                labelLine: {
                                    show: false
                                }
                            },
                            emphasis: {
                                label: {
                                    position: 'inside',
                                    formatter: '{b} : {c}%'
                                }
                            }
                        },
                        data: series
                    }
                    ,
                    {
                        name: '实际',
                        type: 'funnel',
                        x: '10%',
                        width: '40%',
                        maxSize: '80%',
                        itemStyle: {
                            normal: {
                                borderColor: '#fff',
                                borderWidth: 2,
                                label: {
                                    position: 'inside',
                                    formatter: '{c}%',
                                    textStyle: {
                                        color: '#fff'
                                    }
                                }
                            },
                            emphasis: {
                                label: {
                                    position: 'inside',
                                    formatter: '{b} 实际 : {c}%'
                                }
                            }
                        },
                        data: series1
                    }
                ]
            };

            // 为echarts对象加载数据 
            myChart.setOption(option);
            $("#graph").css({ "filter": "alpha(opacity=100)", "background": "#fff" });
        }
        function doserch() {

            var sendtxt = "&Action=Funnel&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);
            var manager = $("#maingrid5").ligerGetGridManager();

            top.$.ligerDialog.waitting('数据查询中,请稍候...');
            $.ajax({
                url: "../../data/Reports_CRM.ashx", type: "POST",
                data: serchtxt,
                dataType: 'json',
                beforeSend: function () {
                    manager.showData({ Rows: [], Total: 0 });
                },
                success: function (responseText) {

                    top.$.ligerDialog.closeWaitting();
                    test(responseText);
                },
                error: function () {
                    top.$.ligerDialog.closeWaitting();
                    top.$.ligerDialog.error('查询失败！请检查查询项。');
                }
            });
            //test();
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
