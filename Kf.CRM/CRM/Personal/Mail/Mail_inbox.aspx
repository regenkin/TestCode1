<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />   
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
    <script src="../../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerResizable.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        var manager;
        var manager1;
        $(function () {
            initLayout();

            $(window).resize(function () {
                initLayout();
                ;
            });

            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '��', width: 50, render: function (item, i) { return i + 1; } },
                    {
                        display: '�״̬', name: 'isView', width: 50, render: function (item) {
                            var html = "<img src='../../images/icon/";
                            if (item.isView == 0)
                                html += 47;
                            else
                                html += 96;
                            html += ".png' />";
                            return html;
                        }
                    },
                    {
                        display: '���', name: 'atta_count', width: 60, render: function (item) {
                            if (item.atta_count == 0) {
                                return "";
                            }
                            else {
                                var html = "<img src='../../images/IsAccessary.gif'/>"
                                html += item.atta_count;
                                return html;
                            }
                        }
                    },
                    {
                        display: '����', name: 'mail_title', width: 180,align:'left', render: function (item) {
                            return "<a href='javascript:void(0)' onclick=view(" + item.mail_id + ")>" + item.mail_title + "</a>";
                        }
                    },
                    { display: 'ⷢ���', name: 'sender_name', width: 100 },
                    {
                        display: '˷���ʱ�', name: 'sender_time', width: 150, render: function (item) {
                            return formatTimebytype(item.sender_time, 'yyyy-MM-dd hh:mm');
                        }
                    },
                    {
                        display: '��Ķ�ʱ�', name: 'view_time', width: 150, render: function (item) {
                            if (item.isView == 1)
                                return formatTimebytype(item.view_time, 'yyyy-MM-dd hh:mm:ss');
                        }
                    }
                ],
                dataAction: 'server', pageSize: 30,
                pageSizeOptions: [20, 30, 50, 100],
                url: "../../data/mail.ashx?Action=recive_grid&rnd=" + Math.random(),
                width: '100%',
                height: '100%',
                heightDiff: -1,
                onRClickToSelect: true,
                onContextmenu: function (parm, e) {
                    actionCustomerID = parm.data.id;
                    menu.show({ top: e.pageY, left: e.pageX });
                    return false;
                }
            });


            $("#grid").height(document.documentElement.clientHeight - $(".toolbar").height());
            $("#pageloading").fadeOut(400);

            $('form').ligerForm();
            //initSerchForm();
            toolbar();
        });

        
        function toolbar() {
            $.getJSON("../../data/toolbar.ashx?Action=GetSys&mid=78&rnd=" + Math.random(), function (data, textStatus) {
                
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    items.push(arr[i]);
                }

                items.push({
                    type: 'serchbtn',
                    text: '�߼����',
                    icon: '../../images/search.gif',
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

        //�߼�����
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

        //��ѯ
        function doserch() {
            var sendtxt = "&Action=recive_grid&rnd=" + Math.random();
            var serchtxt = $("#serchform: input").fieldSerialize() + sendtxt;
            $.ligerDialog.waitting('��ݲ�ѯ�У����Ժ...');
            var manager = $("#maingrid4").ligerGetGridManager();
            $.ajax({
                url: "../../data/mail.ashx", type: "POST",
                data: serchtxt,
                dataType: 'json',
                beforeSend: function () {
                    manager.showData({ Rows: [], Total: 0 });
                },
                success: function (responseText) {
                    manager.setURL("../../data/mail.ashx?" + serchtxt);
                    manager.showData(responseText);
                    $.ligerDialog.closeWaitting();
                },
                error: function () {
                    $.ligerDialog.closeWaitting();
                    $.ligerDialog.error('��ѯʧ�ܣ������ѯ�');
                }
            });
        }

        //����
        function doclear() {
            $("#serchform").each(function () {
                this.reset();
            });
        }


        //ó�ʼ��dialog
        var activeDialog = null;
        function f_openWindow(url, title, width, height) {
            var dialogOptions = {
                width: width, height: height, title: title, url:encodeURI(url), buttons: [
                    {
                        text: '���', onclick: function (item, dialog) {
                            f_save(item, dialog);
                        }
                    },
                    {
                        text: '͹ر', onclick: function (item, dialog) {
                            dialog.close();
                        }
                    }
                ], isResize: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }


        //dialog��ﱣ�水ť
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            //alert(issave);
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('��ݱ����У����Ժ...');
                $.ajax({
                    url: "../../data/mail.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        top.$.ligerDialog.closeWaitting();
                        f_reload();
                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('����ʧ�ܣ�');
                    }
                })
            }
        }

        //ˢ����
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        };

        //ݲ鿴���
        function view(title_id) {
            var dialogOptions = {
                width: 770, height: 510, title: "�鿴���", url: 'Personal/Mail/mail_inbox_view.aspx?nid=' + title_id, buttons: [
                        {
                            text: '�ر', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
            setTimeout(f_reload, 200);
        }


        //ջظ�
        function reply() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow("Personal/mail/mail_reply.aspx?mail_id=" + row.mail_id, "д�", 800, 480);
            }
            else {
                $.ligerDialog.warn("���ѡ��Ҫ�ظ����ʼ");
            }
        }

        $(document).keydown(function (e) {
            if (e.keyCode == 13 && e.target.applyligerui) {
                doserch();
            }
        });


        //�ת��
        function forward() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('Personal/mail/mail_forward.aspx?mail_id=' + row.mail_id, "ת��", 800, 520);
            } else {
                $.ligerDialog.warn('��ѡ���ʼ!');
            }
        }


        //�ɾ�
        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("�ɾ����ָܻ���\n��ȷ��Ҫɾ�",function(yes){
                if (yes) {
                    $.ajax({
                        url: "../../data/mail.ashx", type: "POST",
                        data: { Action: "AdvanceDelete", id: row.id, rnd: Math.random() },
                        success: function (responseText) {
                            if (responseText == "true") {
                                f_reload();

                                //f_followreload();
                                //f_foll
                            }
                            else {
                                top.$.ligerDialog.error('�ɾ��ʧ�ܣ�');
                            }

                        },
                        error: function () {
                            $.ligerDialog.error('ϵͳ���!����ϵ����Ա');
                        }
                    });
                }
                })
            }
            else {
                $.ligerDialog.warn("��ѡ���!");
            }
        }





    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <div id="toolbar"></div>
        <div class="l-loading" style="display: block" id="pageloading"></div>
        <div id="grid">
            <div id="maingrid4" style="margin: -1px; min-width: 800px;"></div>
        </div>


    </form>
    <div class="az">
        <form id='serchform'>
            <table style='width: 880px' class="bodytable1">
                <tr>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>з���ʱ�䣺</div>
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
                        <div style='width: 60px; text-align: right; float: right'>���    ⣺</div>
                    </td>
                    <td>
                        <input id='mail_title' name="mail_title" type='text' ltype='text' ligerui='{width:120}' />
                    </td>
                    <%--<td>
                        <div style='width: 60px; text-align: right; float: right'>״̬��</div>
                    </td>
                    <td>
                        <input type='text' id='mail_status' name='mail_status' ltype='text' ligerui='{width:167}' />
                    </td>--%>
                    
                    

                    

                </tr>
                <tr>
                    
                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�Ķ�ʱ�䣺</div>
                    </td>
                    <td>
                        <div style='width: 100px; float: left'>
                            <input type='text' id='startdate1' name='startdate1' ltype='date' ligerui='{width:97}' />
                        </div>
                        <div style='width: 98px; float: left'>
                            <input type='text' id='enddate1' name='enddate1' ltype='date' ligerui='{width:96}' />
                        </div>
                    </td>

                    <td>
                        <div style='width: 60px; text-align: right; float: right'>�����ˣ�</div>
                    </td>
                    <td>
                        <input type='text' id='sender' name='sender' ltype='text' ligerui='{width:120}' />
                    </td>

                    <td></td>
                    <td>
                        <input id='Button2' type='button' value='���' style='width: 80px; height: 24px'
                            onclick="doclear()" />
                        <input id='Button1' type='button' value='����' style='width: 80px; height: 24px' onclick="doserch()" />
                    </td>
                </tr>
            </table>
        </form>
    </div>
</body>
</html>

