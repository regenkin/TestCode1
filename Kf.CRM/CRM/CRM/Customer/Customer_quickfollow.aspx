<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=7" />
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerResizable.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTab.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>

    <script src="../../lib/jquery.form.js" type="text/javascript"></script>

    <script src="../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script type="text/javascript">
        var manager;
        var manager1;
        $(function () {

            if (getparastr("customerid")) {
                loadForm(getparastr("customerid"));
            }
            $('#navtab1').ligerTab({
                onAfterSelectTabItem: function (tabid) {
                    switch (tabid) {
                        case "nav_contact":
                            $.getJSON("../../data/CRM_Contact.ashx?Action=grid&customerid=" + getparastr("customerid"), function (data) {
                                var rows = data.Rows;
                                for (var i = 0; i < rows.length; i++) {
                                    var r = rows[i];
                                    for (var n in r) {
                                        if (r[n] == "null" || r[n] == null )
                                            r[n] = "";
                                    }
                                    $("#maingrid6").append(
                                    "<table class='bodytable0'  style='margin:5px 5px 0 5px ;width:745px;'>" +
                                        "<tr>" +
                                            "<td height='23' width='10%' class='table_label'>��ϵ�ˣ�</td><td height='23' width='15%'  >" + r.C_name + "��" + r.C_sex + "��" + "</td>" +
                                            "<td height='23' width='10%' class='table_label'>���ţ�</td><td height='23' width='15%' >" + r.C_department + "</td>" +
                                            "<td height='23' width='10%' class='table_label'>ְ��</td><td height='23' width='15%'  >" + r.C_position + "</td>" +
                                            "<td height='23' width='10%' class='table_label'>���գ�</td><td height='23'  >" + r.C_birthday + "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td height='23' class='table_label'>�ֻ���</td><td height='23' >" + r.C_mob + "</td>" +
                                            "<td height='23' class='table_label'>�绰��</td><td height='23'  >" + r.C_tel + "</td>" +
                                           "<td height='23' class='table_label'>QQ��</td><td height='23'  >" + r.C_QQ + "</td>" +
                                           "<td height='23' class='table_label'>Email��</td><td height='23'  >" + r.C_email + "</td>" +
                                        "</tr>" +
                                        "<tr>" +
                                            "<td height='23'  class='table_label'>���ã�</td><td height='23' colspan='7'  >" + r.C_hobby + "</td>" +
                                        "</tr>" +
                                         "<tr>" +
                                            "<td height='23'  class='table_label'>��ע��</td><td height='23' colspan='7'  >" + r.C_remarks + "</td>" +
                                        "</tr>" +
                                         "<tr>" +
                                            "<td height='23'  class='table_label'>��ַ��</td><td height='23' colspan='7'  >" + r.C_add + "</td>" +
                                        "</tr>" +
                                    "</table>");
                                }

                            });
                            break;
                    }

                }
            });

            $("#maingrid5").ligerGrid({
                columns: [
                        { display: '���', width: 30, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                        {
                            display: '��������', name: 'Follow', align: 'left', width: 415, render: function (item) {
                                var html = "<div class='abc'>"
                                if (item.Follow)
                                    html += item.Follow;
                                html += "</div>";
                                return html;
                            }
                        },
                        {
                            display: '����ʱ��', name: 'Follow_date', width: 140, render: function (item) {
                                return formatTimebytype(item.Follow_date, 'yyyy-MM-dd hh:mm');
                            }
                        },
                        { display: '������ʽ', name: 'Follow_Type', width: 60 },
                        {
                            display: '������', name: '', width: 100, render: function (item) {
                                return item.department_name + "." + item.employee_name;
                            }
                        }
                ],
                onAfterShowData: function (grid) {
                    $(".abc").hover(function (e) {
                        $(this).ligerTip({ content: $(this).text(), width: 200, distanceX: event.clientX - $(this).offset().left - $(this).width() + 15 });
                    }, function (e) {
                        $(this).ligerHideTip(e);
                    });
                },
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                //checkbox:true,
                url: "../../data/CRM_Follow.ashx?Action=grid&customer_id=" + getparastr("customerid"),
                width: '100%', height: '160px',
                //title: "������Ϣ",
                heightDiff: -5,
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu1.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });


            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());

            $('form').ligerForm();
            toolbar();
            initLayout();
            $(window).resize(function () {
                initLayout();
            });
        });

        function toolbar() {
            $.getJSON("../../data/toolbar.ashx?Action=GetSys&mid=6&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);
                }
                $("#toolbar1").ligerToolBar({
                    items: items

                });
                menu1 = $.ligerMenu({
                    width: 120, items: getMenuItems(data)
                });

                $("#maingrid5").ligerGetGridManager().onResize();
            });
        }


        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../../data/crm_customer.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                data: { Action: 'form', cid: oaid, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);

                    //alert(obj.constructor); //String ���캯��
                    $("#T_company").val(obj.Customer);
                    $("#T_company0").val(obj.industry);
                    $("#T_address").val(obj.address);
                    $("#T_Website").html("<a href='" + obj.site + "' target='_blank'>" + obj.site + "</a>");
                    $("#T_fax").val(obj.fax);
                    $("#T_descript").val(obj.DesCripe);
                    $("#T_remarks").val(obj.Remarks);
                    $("#T_company_tel").val(obj.tel);

                    $("#T_Provinces").val(obj.Provinces);
                    $("#T_City").val(obj.City);
                    $("#T_customertype").val(obj.CustomerType);
                    $("#T_customerlevel").val(obj.CustomerLevel);
                    $("#T_CustomerSource").val(obj.CustomerSource);
                    $("#T_private").val(obj.privatecustomer);
                    $("#T_department").val(obj.Department);
                    $("#T_employee").val(obj.Employee);
                }
            });
        }
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
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }


        //follow
        function follow_openWindow(url, title, width, height) {
            var dialogOptions = {
                width: width, height: height, title: title, url: url, buttons: [
                        {
                            text: '����', onclick: function (item, dialog) {
                                f_savefollow(item, dialog);
                            }
                        },
                        {
                            text: '�ر�', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'b', zindex: 9003
            };
            activeDialog1 = top.jQuery.ligerDialog.open(dialogOptions);
        }
        function addfollow() {
            if (getparastr("customerid")) {
                follow_openWindow("CRM/Customer/Customer_follow_add.aspx?cid=" + getparastr("customerid"), "��������", 530, 400);
            } else {
                $.ligerDialog.warn('��ѡ��ͻ���');
            }
        }
        function editfollow() {
            var manager = $("#maingrid5").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                follow_openWindow('CRM/Customer/Customer_follow_add.aspx?fid=' + row.id + "&cid=" + getparastr("customerid"), "�޸ĸ���", 530, 400);
            } else {
                $.ligerDialog.warn('��ѡ�������');
            }
        }
        function delfollow() {
            var manager = $("#maingrid5").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("ȷ��ɾ����", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "../../data/CRM_Follow.ashx", type: "POST",
                            data: { Action: "AdvanceDelete", id: row.id, rnd: Math.random() },
                            success: function (responseText) {
                                if (responseText == "true") {
                                    f_followreload();
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
                $.ligerDialog.warn("��ѡ�����");
            }
        }
        function f_savefollow(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                $.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "../../data/CRM_Follow.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        $.ligerDialog.closeWaitting();
                        f_followreload();
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }
        }
        function f_followreload() {
            var manager = $("#maingrid5").ligerGetGridManager();
            manager.loadData(true);
        };
    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <div id="toolbar"></div>

        <div id="navtab1" style="width: 772px; overflow: hidden; border: none;">
            <div tabid="nav_base" title="������Ϣ" style="height: 292px;">
                <table style="width: 600px; margin: 5px;" class='bodytable1'>
                    <tr>
                        <td colspan="4" class="table_title1">������Ϣ</td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">��˾���ƣ�</div>
                        </td>
                        <td>
                            <input type="text" id="T_company" name="T_company" ltype="text" ligerui="{width:196}" validate="{required:true}" /></td>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">��˾��ַ��</div>
                        </td>
                        <td>
                            <div id="T_Website" name="T_Website"></div>
                        </td>
                    </tr>
                    <tr>
                        <td>

                            <div style="width: 80px; text-align: right; float: right">������ҵ��</div>

                        </td>
                        <td>
                            <input type="text" id="T_company0" name="T_company0" ltype="text" ligerui="{width:196}" validate="{required:true}" /></td>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">����������</div>
                        </td>
                        <td>
                            <div style="width: 100px; float: left">
                                <input id="T_Provinces" name="T_Provinces" type="text" style="width: 96px;" ltype="text" ligerui="{width:96}" />
                            </div>
                            <div style="width: 98px; float: left">
                                <input id="T_City" name="T_City" type="text" style="width: 96px;" ltype="text" ligerui="{width:96}" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>

                            <div style="width: 80px; text-align: right; float: right">��˾�绰��</div>

                        </td>
                        <td>

                            <input id="T_company_tel" name="T_company_tel" type="text" ltype="text" ligerui="{width:196}" validate="{required:true}" /></td>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">���棺</div>
                        </td>
                        <td>
                            <input id="T_fax" name="T_fax" type="text" ltype="text" ligerui="{width:196}" /></td>
                    </tr>
                    <tr>
                        <td>

                            <div style="width: 80px; text-align: right; float: right">��˾��ַ��</div>

                        </td>
                        <td colspan="3">

                            <input type="text" id="T_address" name="T_address" ltype="text" ligerui="{width:495}" /></td>
                    </tr>


                    <tr>
                        <td colspan="4" class="table_title1">����</td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">�ͻ����ͣ�</div>
                        </td>
                        <td>
                            <div style="width: 100px; float: left">
                                <input id="T_customertype" name="T_customertype" type="text" style="width: 96px" ltype="text" ligerui="{width:96}" />
                            </div>
                            <div style="width: 98px; float: left">
                                <input id="T_customerlevel" name="T_customerlevel" type="text" style="width: 96px" ltype="text" ligerui="{width:96}" />
                            </div>
                        </td>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">�ͻ���Դ��</div>
                        </td>
                        <td>
                            <input id="T_CustomerSource" name="T_CustomerSource" type="text" ltype="text" ligerui="{width:196}" />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">�ͻ�������</div>
                        </td>
                        <td colspan="3">
                            <input id="T_descript" name="T_descript" type="text" ltype="text" ligerui="{width:495}" /></td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">��&nbsp; ע��</div>
                        </td>
                        <td colspan="3">
                            <input id="T_remarks" name="T_remarks" type="text" ltype="text" ligerui="{width:495}" /></td>
                    </tr>
                    <tr>
                        <td colspan="4" class="table_title1">����</td>
                    </tr>
                    <tr>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">״̬��</div>
                        </td>
                        <td>
                            <input id="T_private" name="T_private" type="text" ltype="text" ligerui="{width:196 }" validate="{required:true}" /></td>
                        <td>
                            <div style="width: 80px; text-align: right; float: right">ҵ��Ա��</div>
                        </td>
                        <td>
                            <div style="width: 100px; float: left">
                                <input id="T_department" name="T_department" type="text" validate="{required:true}" style="width: 97px" ltype="text" ligerui="{width:96}" />
                            </div>
                            <div style="width: 98px; float: left">
                                <input id="T_employee" name="T_employee" type="text" validate="{required:true}" style="width: 96px" ltype="text" ligerui="{width:96}" />
                            </div>
                        </td>
                    </tr>
                    <%--<tr>
                <td colspan="4">
                    <div id="toolbar" style="width: 585px;"></div>
                    <div id="maingrid4" style="margin: -1px;"></div>
                </td>
            </tr>--%>
                </table>
            </div>
            <div tabid="nav_contact" title="��ϵ��" style="height: 292px; overflow-y: scroll;">
                <div id="maingrid6" style="margin: -1px;"></div>
            </div>
        </div>
        <div style="width: 765px; margin-left: 2px; border: 1px solid #98c0f4;">
            <div id="toolbar1"></div>
            <div id="maingrid5" style="margin: -1px -1px;"></div>
        </div>
</body>
</html>
