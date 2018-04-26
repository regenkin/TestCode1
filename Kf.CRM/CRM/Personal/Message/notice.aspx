<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />

    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" /> 
    <link href="../../CSS/input.css" rel="stylesheet" />
    <link href="../../ueditor1_2_5_1-utf8-net/themes/default/css/ueditor.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTab.js" type="text/javascript"></script>

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
    <script src="../../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {

            initLayout();
            $(window).resize(function () {
                initLayout();
            });
            $("#layout1").ligerLayout({ bottomHeight: '50%', allowBottomResize: false, height: '100%' });
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '���', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    {
                        display: '�������', name: 'notice_title', width: 460, align: 'left', render: function (item) {
                            return "<a href='javascript:void(0)' onclick=view(" + item.id + ")>" + item.notice_title + "</a>";
                        }
                    },
                    { display: '��������', name: 'dep_name', width: 100 },
                    { display: '������', name: 'create_name', width: 100 },

                    {
                        display: '����ʱ��', name: 'notice_time', width: 90, render: function (item) {
                            return formatTimebytype(item.notice_time, 'yyyy-MM-dd');
                        }
                    },
                    {
                        display: '�鿴', render: function (item) {
                            return "<a href='#' onclick=view(" + item.id + ")>�鿴</a>";
                        }
                    }

                ],
                //fixedCellHeight:false,
                onSelectRow: function (data, rowindex, rowobj) {
                    $("#framecenter").html(myHTMLDeCode(data.notice_content));
                },
                dataAction: 'server', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "../../data/public_notice.ashx?Action=grid&rnd=" + Math.random(),
                //title:'�����б�',
                width: '100%', height: '100%',
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
            toolbar();
        });

        function toolbar() {
            $.getJSON("../../data/toolbar.ashx?Action=GetSys&mid=14&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                var items = [];
                var arr = data.Items;
                for (var i = 0; i < arr.length; i++) {
                    arr[i].icon = "../../" + arr[i].icon;
                    items.push(arr[i]);
                }
                items.push({ type: 'textbox', id: 'sstart', text: 'ʱ�䣺' });
                items.push({ type: 'textbox', id: 'sdend', text: '' });
                items.push({ type: 'textbox', id: 'stext', text: '���⣺' });
                items.push({ type: 'button', text: '����', icon: '../../images/search.gif', disable: true, click: function () { doserch() } });
                items.push({ type: 'button', text: '����', icon: '../../images/edit.gif', disable: true, click: function () { doclear() } });
                $("#toolbar").ligerToolBar({ items: items });
                menu = $.ligerMenu({ width: 120, items: getMenuItems(data) });

                $("#maingrid4").ligerGetGridManager().onResize();
                $("#sstart").ligerDateEditor({  width: 100 })
                $("#sdend").ligerDateEditor({ width: 100 })
                $("#stext").ligerTextBox({ width: 200, nullText: "����ؼ�������" })
            });
        }

        function doserch() {
            var sendtxt = "&Action=grid&rnd=" + Math.random();
            var serchtxt = $("#form1 :input").fieldSerialize() + sendtxt;
            //alert(serchtxt);           
            var manager = $("#maingrid4").ligerGetGridManager();

            manager.GetDataByURL("../../data/public_notice.ashx?" + serchtxt);
        }
        function doclear() {
            //var serchtxt = $("#serchform :input").reset();
            $("#form1").each(function () {
                this.reset();
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
                ], isResize: true, showToggle: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }

        //�鿴
        function view(title_id) {
            var dialogOptions = {
                width: 770, height: 510, title: "�鿴����", url: 'personal/message/notice_view.aspx?nid=' + title_id, buttons: [
                        {
                            text: '�ر�', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a'
            };
            activeDialog = parent.jQuery.ligerDialog.open(dialogOptions);
        }

        function add() {
            f_openWindow("personal/message/notice_add.aspx", "��������", 770, 500);
        }

        function edit() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                f_openWindow('personal/message/notice_add.aspx?nid=' + row.id, "�޸Ĺ���", 770, 500);
            }
            else {
                $.ligerDialog.warn('��ѡ���У�');
            }
        }

        function del() {
            var manager = $("#maingrid4").ligerGetGridManager();
            var row = manager.getSelectedRow();
            if (row) {
                $.ligerDialog.confirm("ɾ�����ָܻ���\n��ȷ��Ҫɾ����", function (yes) {
                    if (yes) {
                        $.ajax({
                            url: "../../data/public_notice.ashx", type: "POST",
                            data: { Action: "del", id: row.id, rnd: Math.random() },
                            success: function (responseText) {
                                if (responseText == "true") {
                                    f_reload();
                                }
                                else if (responseText == "auth") {
                                    top.$.ligerDialog.error('Ȩ�޲�����');
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
                $.ligerDialog.warn("��ѡ��ͻ�");
            }
        }
        function f_save(item, dialog) {
            var issave = dialog.frame.f_save();
            if (issave) {
                dialog.close();
                top.$.ligerDialog.waitting('���ݱ�����,���Ժ�...');
                $.ajax({
                    url: "../../data/public_notice.ashx", type: "POST",
                    data: issave,
                    success: function (responseText) {
                        top.$.ligerDialog.closeWaitting();
                        f_reload();
                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('����ʧ�ܣ�');
                    }
                });

            }
        }


        function f_reload() {
            $("#maingrid4").ligerGetGridManager().loadData(true);
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
