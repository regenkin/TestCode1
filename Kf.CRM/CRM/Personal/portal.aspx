<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerPanel.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerPortal.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../JS/echarts-all.js" type="text/javascript"></script>
    <script type="text/javascript">
        var manager;
        $(function () {
            manager = $("#portalMain").ligerPortal({
                draggable: false,
                rows: [
                    {
                        columns: [
                            {
                                width: '33%',
                                panels: [
                                    {
                                        title: '新闻',
                                        width: '99%',
                                        height: 200,
                                        content_id: 'c_news'
                                    }
                                ]
                            },
                        {
                            width: '33%',
                            panels: [
                                {
                                    title: '公告',
                                    width: '99%',
                                    height: 200,
                                    content_id: 'c_notice'
                                }
                            ]
                        },
                         {
                             width: '33%',
                             panels: [
                                 {
                                     title: '日程',
                                     width: '99%',
                                     height: 200,
                                     content_id: 'c_calendar'
                                 }
                             ]
                         }
                        ]
                    }
                    , {
                        columns: [{
                            width: '66%',
                            panels: [{
                                title: '统计年报',
                                width: '99.5%',
                                height: 200,
                                content_id: 'c_report'
                            }
                            ]
                        }, {
                            width: '33%',
                            panels: [{
                                title: '销售漏斗',
                                width: '99%',
                                height: 200,
                                content_id: 'c_funnel'
                            }
                            ]
                        }

                        ]
                    }
                     , {
                         columns: [
                             {
                                 width: '33%',
                                 panels: [{
                                     title: '便签',
                                     width: '99%',
                                     height: 200,
                                     content_id: 'c_note'
                                 }
                                 ]
                             }
                         ]
                     }
                ]
            });
            notice();
            todo();
            news();
            note();
            report();
            funnel();
        });
        function notice() {  //公告提醒
            $.getJSON("../data/public_notice.ashx?Action=noticeremind&rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>")
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/18.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='javascript:void(0)' onclick=\"window.top.f_addTab('tabid34','公告','Personal/Message/notice.aspx')\">" + obj.Rows[i].notice_title + "</a></div></td><td width='80'>" + formatTimebytype(obj.Rows[i].notice_time, 'yyyy-MM-dd') + "</td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_notice"))
            });
        }
        function todo() {  //日程安排提醒
            $.getJSON("../data/Personal_Calendar.ashx?Action=Today&rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>")
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/31.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='javascript:void(0)' onclick=\"window.top.f_addTab('tabid39','日程安排','personal/personal/Calendar.aspx')\">" + obj.Rows[i].Subject + "</a></div></td><td width='80'>" + formatTimebytype(obj.Rows[i].StartTime, 'yyyy-MM-dd') + "</td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_calendar"))
            });

        }
        function news() {  //新闻
            $.getJSON("../data/public_news.ashx?Action=newsremind&rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>")
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/10.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='javascript:void(0)' onclick=\"window.top.f_addTab('tabid33','新闻','personal/message/news.aspx')\">" + obj.Rows[i].news_title + "</a></div></td><td width='80'>" + formatTimebytype(obj.Rows[i].news_time, 'yyyy-MM-dd') + "</td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_news"))
            });

        }
        function note() {  //便签
            $.getJSON("../data/Personal_notes.ashx?Action=notesremind&rnd=" + Math.random(), function (data, textStatus) {
                var obj = eval(data);
                var table = $("<table style='width:100%'></table>")
                for (var i = 0; i < obj.Rows.length; i++) {
                    $("<tr><td style='width:25px;text-align:center;'><div style='height:18px;padding-top:5px;overflow:hidden;'><img src='../../images/icon/35.png'></div></td><td><div style='overflow:hidden;height:18px;'><a herf='javascript:void(0)' onclick=\"window.top.f_addTab('tabid38','便签','personal/personal/notes.aspx')\">" + obj.Rows[i].note_content

                        + "</a></div></td></tr>").appendTo(table);
                }
                table.addClass("bodytable2");
                table.appendTo($("#c_note"))
            });

        }
        function report() {
            $("#c_report").append("<div id='div_reports'></div>");
            var d = new Date();
            var nowYear = +d.getFullYear();
            var sendtxt = "&Action=CRM_Reports_year&stype=%E5%AE%A2%E6%88%B7%E7%B1%BB%E5%9E%8B&stype_val=CustomerType&syear_val=" + nowYear + "&rnd=" + Math.random();
            $.ajax({
                url: "../data/Reports_CRM.ashx", type: "POST",
                data: sendtxt,
                dataType: 'json',
                success: function (responseText) {
                    test(responseText);
                }
            });
            //test();

        }
        function test(GridData) {
            var series = [];
            var legend = [];
            var mc1 = 0; mc2 = 0; mc3 = 0; mc4 = 0; mc5 = 0; mc6 = 0; mc7 = 0; mc8 = 0; mc9 = 0; mc10 = 0; mc11 = 0; mc12 = 0;
            for (var i = 0; i < GridData.Rows.length; i++) {
                var flowtype = GridData.Rows[i].items;
                if (!flowtype)
                    flowtype = "未分类";
                var m1 = typeof (GridData.Rows[i].m1) != 'number' ? 0 : GridData.Rows[i].m1;
                var m2 = typeof (GridData.Rows[i].m2) != 'number' ? 0 : GridData.Rows[i].m2;
                var m3 = typeof (GridData.Rows[i].m3) != 'number' ? 0 : GridData.Rows[i].m3;
                var m4 = typeof (GridData.Rows[i].m4) != 'number' ? 0 : GridData.Rows[i].m4;
                var m5 = typeof (GridData.Rows[i].m5) != 'number' ? 0 : GridData.Rows[i].m5;
                var m6 = typeof (GridData.Rows[i].m6) != 'number' ? 0 : GridData.Rows[i].m6;
                var m7 = typeof (GridData.Rows[i].m7) != 'number' ? 0 : GridData.Rows[i].m7;
                var m8 = typeof (GridData.Rows[i].m8) != 'number' ? 0 : GridData.Rows[i].m8;
                var m9 = typeof (GridData.Rows[i].m9) != 'number' ? 0 : GridData.Rows[i].m9;
                var m10 = typeof (GridData.Rows[i].m10) != 'number' ? 0 : GridData.Rows[i].m10;
                var m11 = typeof (GridData.Rows[i].m11) != 'number' ? 0 : GridData.Rows[i].m11;
                var m12 = typeof (GridData.Rows[i].m12) != 'number' ? 0 : GridData.Rows[i].m12;

                series.push({ "name": flowtype, "type": "bar", "data": [m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12] });
                legend.push(flowtype);
                mc1 += m1; mc2 += m2; mc3 += m3; mc4 += m4; mc5 += m5; mc6 += m6; mc7 += m7; mc8 += m8; mc9 += m9; mc10 += m10; mc11 += m11; mc12 += m12;
            }
            series.push({ "name": "总计", "type": "line", "data": [mc1, mc2, mc3, mc4, mc5, mc6, mc7, mc8, mc9, mc10, mc11, mc12] });
            legend.push("总计");
            var myChart = echarts.init(document.getElementById('c_report'));

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
                        data: ["一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"]
                    }
                ],
                yAxis: [
                    {
                        type: 'value'
                    }
                ],
                series: series,
                dataZoom: {
                    show: true,
                    realtime: true,
                    start: 0,
                    end: 100
                },
                grid: {
                    x: 40,
                    y: 20,
                    x2: 10
                }
            };

            // 为echarts对象加载数据 
            myChart.setOption(option);
            $("#c_report").css({ "filter": "alpha(opacity=100)", "background": "#fff" });
        }
        function funnel() {
            var d = new Date();
            var nowYear = +d.getFullYear();
            var sendtxt = "&Action=Funnel&rnd=" + Math.random() + "&syear_val=" + nowYear;

            $.ajax({
                url: "../data/Reports_CRM.ashx", type: "POST",
                data: sendtxt,
                dataType: 'json',
                success: function (responseText) {
                    dofunnel(responseText);
                }
            });
        }
        function dofunnel(GridData) {
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

            var myChart = echarts.init(document.getElementById('c_funnel'));

            var option = {
                title: {
                    text: '销售漏斗图',
                    y:'bottom'
                },

                //legend: {
                //    data: legend
                //},
                calculable: true,
                series: [
                    {
                        name: '预期',
                        type: 'funnel',
                        x: '2%',
                        y: '2%',
                        width: '80%',
                        height: '95%',
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
                        x: '2%',
                        y: '2%',
                        width: '80%',
                        height: '95%',
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
            $("#c_funnel").css({ "filter": "alpha(opacity=100)", "background": "#fff" });
        }
    </script>
    <style type="text/css">
        .l-column-place { width: 99%; }
        .l-panel-place { width: 99%; }
        .l-portal .l-column .l-panel { margin-bottom: 0; }
    </style>
</head>
<body style="overflow-y: scroll;">
    <div style="padding: 5px;">
        <div style="width: 100%;" id="portalMain">
        </div>
    </div>
    <%--<input type="button" value="test" onclick="test()" />--%>
</body>
</html>
