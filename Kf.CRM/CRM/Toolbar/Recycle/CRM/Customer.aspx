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
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '���', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    {
                        display: '��˾����', name: 'Customer', width: 200, align: 'left', render: function (item) {
                            var html = "<div class='abc'>"
                            if (item.Customer)
                                html += item.Customer;
                            html += "</div>";
                            return html;
                        }
                    },
                    { display: '�绰', name: 'tel', width: 120, align: 'right' },
                    { display: '�ͻ�����', name: 'CustomerType', width: 80 },
                    { display: '�ͻ����', name: 'CustomerLevel', width: 80 },
                    { display: '�ͻ���Դ', name: 'CustomerSource', width: 80 },
                    {
                        display: 'ʡ��', name: 'CustomerFrom', width: 80, render: function (item) {
                            return item.Provinces + "." + item.City;
                        }
                    },
                    { display: '������ҵ', name: 'industry', width: 80 },
                    {
                        display: '�ͻ�����', name: '', width: 120, render: function (item) {
                            return item.Department + "." + item.Employee;
                        }
                    },
                    { display: '��Դ״̬', name: 'privatecustomer', width: 60 },
                    {
                        display: '������', name: 'lastfollow', width: 90, render: function (item) {
                            return formatTimebytype(item.lastfollow, 'yyyy-MM-dd');
                        }
                    },
                    {
                        display: 'ɾ��ʱ��', name: 'Delete_time', width: 180, render: function (item) {
                            return formatTimebytype(item.Delete_time, 'yyyy-MM-dd hh:mm:ss');
                        }
                    }

                ],
                rowtype: "CustomerType",
                checkbox: true,
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "../../../data/CRM_Customer.ashx?Action=grid&isdel=1&rnd=" + Math.random(),
                width: '100%', height: '100%',
                heightDiff: -1,
                onAfterShowData: function (grid) {
                    $("tr[rowtype='�ѳɽ�']").addClass("l-treeleve2").removeClass("l-grid-row-alt");
                },
                
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
                
            });
            $('form').ligerForm();
            initSerchForm();
            toolbar();
            //toolbar1();

        });

        function toolbar() {
            $.getJSON("../../../data/toolbar.ashx?Action=GetSys&mid=47&rnd=" + Math.random(), function (data, textStatus) {
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

                $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());
                $("#maingrid4").ligerGetGridManager().onResize();
                
            });
        }
        function initSerchForm() {
            var a = $('#T_City').ligerComboBox({ width: 96 });
            var b = $('#T_Provinces').ligerComboBox({
                width: 97,
                url: "../../../data/Param_City.ashx?Action=combo1&rnd=" + Math.random(),
                onSelected: function (newvalue) {
                    $.get("../../../data/Param_City.ashx?Action=combo2&pid=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        a.setData(eval(data));
                    });
                }
            });
            var c = $('#customertype').ligerComboBox({ width: 97, url: "../../../data/Param_SysParam.ashx?Action=combo&parentid=1&rnd=" + Math.random() });
            var d = $('#customerlevel').ligerComboBox({ width: 96, url: "../../../data/Param_SysParam.ashx?Action=combo&parentid=2&rnd=" + Math.random() });
            var e = $('#employee').ligerComboBox({ width: 96 });
            var f = $('#department').ligerComboBox({
                width: 97,
                selectBoxWidth: 240,
                selectBoxHeight: 200,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                tree: {
                    url: '../../../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    //parentIDFieldName: 'pid',
                    checkbox: false
                },
                onSelected: function (newvalue) {
                    $.get("../../../data/hr_employee.ashx?Action=combo&did=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        e.setData(eval(data));
                    });
                }
            });
        }
        function serchpanel() {
            if ($(".az").css("display") == "none") {
                $("#grid").css("margin-top", $(".az").height() + "px");
                $("#maingrid4").ligerGetGridManager().onResize();
            }
            else {
                $("#grid").css("margin-top", "0px");
                $("#maingrid4").ligerGetGridManager().onResize();
            }
        }
        $(document).keydown(function (e) {
            if (e.keyCode == 13 && e.target.applyligerui) {
                doserch();
            }
        });
        function doserch() {
            var sendtxt = "&Action=grid&isdel=1&rnd=" + Math.random();
            var serchtxt = $("#serchform :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);
            $.ligerDialog.waitting('���ݲ�ѯ��,���Ժ�...');
            var manager = $("#maingrid4").ligerGetGridManager();

            manager.setURL("../../../data/crm_customer.ashx?" + serchtxt);
            manager.loadData(true);
            $.ligerDialog.closeWaitting();
        }
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
                    url: "../../../data/CRM_Customer.ashx", type: "POST",
                    data: { Action: "regain", idlist: rowid, rnd: Math.random() },
                    success: function (responseText) {                          
                        f_reload();
                        top.$.ligerDialog.success(responseText);
                    },
                    error: function () {
                        top.$.ligerDialog.error('�ָ�ʧ�ܣ�');
                    }
                });

            }
            else {
                $.ligerDialog.warn("��ѡ��ͻ�");
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getCheckedRows();
            if (row.length>0) {
                $.ligerDialog.confirm("����ɾ�����ָܻ��������������<br/>��ȷ��Ҫɾ����", function (yes) {
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
                            url: "../../../data/CRM_Customer.ashx", type: "POST",
                            data: { Action: "del", idlist: rowid, rnd: Math.random() },
                            success: function (responseText) {
                                if (responseText == "delfalse" || responseText == "auth") {
                                    top.$.ligerDialog.error('Ȩ�޲�����ɾ��ʧ�ܣ�');                                     
                                }
                                else {
                                    f_reload();
                                    var arr = new Array();
                                    arr = responseText.split("|");
                                    if (arr.length > 0) {
                                        if(arr[1]>0)
                                            top.$.ligerDialog.error(arr[0] + "<br/>�볹��ɾ����Щ�ͻ��µ���ϵ�ˡ������������ͺ�ͬ���ٲ�����");
                                        else
                                            top.$.ligerDialog.success(arr[0]);
                                    }
                                    else {
                                        top.$.ligerDialog.error('ϵͳ����ɾ��ʧ�ܣ�');
                                    }
                                }
                            },
                            error: function () {
                                top.$.ligerDialog.error('ϵͳ����ɾ��ʧ�ܣ�');
                            }
                        });
                    }
                })
            }
            else {
                $.ligerDialog.warn("��ѡ��ͻ�");
            }
        }

        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };

    </script>
    <style type="text/css">
        .l-treeleve1 {
            background: orange;
        }

        .l-treeleve2 {
            background: yellow;
        }

        .l-treeleve3 {
            background: #eee;
        }
    </style>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <div id="toolbar"></div>
        
        <div id="grid">
            <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
            <div id="toolbar1"></div>
            <div id="maingrid5" style="margin: -1px -1px;"></div>
        </div>


    </form>
    <div class="az">
        <form id='serchform'>
            <table style='width: 1020px' class="bodytable1">
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>��˾���ƣ�</div>
                    </td>
                    <td>
                        <input type='text' id='company' name='company' ltype='text' ligerui='{width:120}' /></td>
                    
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�ͻ����ͣ�</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='customertype' name='customertype' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='customerlevel' name='customerlevel' />
                        </div>
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
                    
                </tr>
                <tr>
                      <td>
                        <div style='width: 60px; text-align: right; float: right'>������ҵ��</div>
                    </td>
                    <td>
                        <input id='industry' name="industry" type='text' ltype='text' ligerui='{width:120}' /></td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>����������</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='T_Provinces' name='T_Provinces' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='T_City' name='T_City' />
                        </div>
                    </td>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>��������</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startfollow' name='startfollow' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='endfollow' name='endfollow' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>ҵ��Ա��</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='department' name='department' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='employee' name='employee' />
                        </div>
                    </td>
                    
                    
                </tr>
                <tr>
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�绰��</div>
                    </td>
                    <td>
                        <input type='text' id='tel' name='tel' ltype='text' ligerui='{width:120}' />
                    </td>
                    
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>��˾��ַ��</div>
                    </td>
                    <td>
                        <input id='website' name="website" type='text' ltype='text' ligerui='{width:196}' /></td>
                    
                    <td></td>
                    
                    <td>
                        
                        <input id='Button2' type='button' value='����' style='width: 80px; height: 24px'
                            onclick="doclear()" />
                        <input id='Button1' type='button' value='����' style='width: 80px; height: 24px' onclick="doserch()" />
                    </td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>

