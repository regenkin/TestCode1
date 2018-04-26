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
                    { display: '序号', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    {
                        display: '操作类型', name: 'C_name', width: 100, render: function (item) {
                            switch (item.batch_type)
                            {
                                case "customer": return "批量转客户"; break;
                                case "order": return "批量转订单"; break;
                                default: return "未知类型"; break;
                            }
                        }
                    },
                     {
                         display: '转出人', width: 200, render: function (item, i) {
                             return "【" + item.o_dep + "】" + item.o_emp;
                         }
                     },
                     {
                         display: '→', width: 50, render: function (item, i) {
                             return "→";
                         }
                     },
                      {
                          display: '转入人', width: 200, render: function (item, i) {
                              return "【" + item.c_dep + "】" + item.c_emp;
                          }
                      },
                    //{ display: '原部门', name: 'o_dep', width: 100 },
                    //{ display: '原员工', name: 'o_emp', width: 100 },
                    //{ display: '现部门', name: 'c_dep', width: 100 },
                    //{ display: '现员工', name: 'c_emp', width: 100 },
                    { display: '数量', name: 'b_count', width: 50 },
                    { display: '操作人', name: 'create_name', widht: 100 },
                    {
                        display: '操作时间', name: 'create_date', width: 180, render: function (item) {
                            return formatTimebytype(item.create_date, 'yyyy-MM-dd hh:mm:ss');
                        }
                    }
                ],
                dataAction: 'server',
                pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "../../data/tool_batch.ashx?Action=grid",
                width: '100%',
                height: '100%',
                //title: "员工列表",
                heightDiff: -1,
                onRClickToSelect: true
            });


            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());

            $('form').ligerForm();
            toolbar();

        });

        function toolbar() {
            $.getJSON("../../data/toolbar.ashx?Action=GetSys&mid=80&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);
                }
                
                $("#toolbar").ligerToolBar({
                    items: items
                });                

                manager = $("#maingrid4").ligerGetGridManager();
                manager.onResize();
            });
        }
        var activeDialog = null;
        function f_openWindow(url, title, width, height) {
            var dialogOptions = {
                width: width, height: height, title: title, url: url, buttons: [
                        {
                            text: '保存', onclick: function (item, dialog) {
                                f_save(item, dialog);
                            }
                        },
                        {
                            text: '关闭', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a', allowClose: false
            };
            activeDialog = top.jQuery.ligerDialog.open(dialogOptions);
        }

        function batch_cus() {
            f_openWindow("toolbar/batch/batch_add.aspx?type=customer", "批量转客源", 720, 400);
        }
        function batch_order() {
            f_openWindow("toolbar/batch/batch_add.aspx?type=customer", "批量转客源", 400, 400);
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            var check = dialog.frame.f_check();

            if (check==true) {
                if (issave) {
                    dialog.close();
                    $.ligerDialog.confirm("批量转出后无法恢复，确定操作？", function (yes) {
                        if (yes) {                            
                            $.ligerDialog.waitting('数据保存中,请稍候...');
                            $.ajax({
                                url: "../../data/tool_batch.ashx", type: "POST",
                                data: issave,
                                success: function (responseText) {
                                    $.ligerDialog.closeWaitting();
                                    f_reload();
                                },
                                error: function () {
                                    $.ligerDialog.closeWaitting();
                                    $.ligerDialog.error('操作失败！');
                                }
                            });
                        }
                    })
                }
            }
            else if(check==false) {
                alert("转出人与转入人不能是同一个人！");
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
</body>
</html>
