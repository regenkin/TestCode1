<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="ie=8" />
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" />

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>

    <script src="../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>

    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>

    <script type="text/javascript" charset="utf-8" src="../../ueditor1_2_5_1-utf8-net/editor_config.js"></script>
    <script type="text/javascript" src="../../ueditor1_2_5_1-utf8-net/editor_all.js"></script>
    <script type="text/javascript" src="../../ueditor1_2_5_1-utf8-net/lang/zh-cn/zh-cn.js"></script>
    <link href="../../ueditor1_2_5_1-utf8-net/themes/default/css/ueditor.css" rel="stylesheet" />

    <link href="../../CSS/swfupload.css" rel="stylesheet" />
    <script type="text/javascript" src="../../js/swfupload/swfupload.js"></script>
    <script type="text/javascript" src="../../js/swfupload/swfupload.queue.js"></script>
    <script type="text/javascript" src="../../js/swfupload/fileprogress.js"></script>
    <script type="text/javascript" src="../../js/swfupload/filegroupprogress.js"></script>
    <script type="text/javascript" src="../../js/swfupload/handlers.js"></script>

    <script type="text/javascript">
        var swfu;
        $(function () {
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            $("form").ligerForm();

            UE.getEditor('editor', {
                initialFrameWidth: 750,
                initialFrameHeight: 260,
                toolbars: [
               [
                'bold', 'italic', 'underline', '|',
                'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|',
                'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
                'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
               ]
                ],
                autoHeightEnabled: false,
                initialContent: '�ʼ����'
            });

            initcombo();
            flush_attachment();

            initmail();
            $("#atta").bind("click", function () {
                top.$.ligerDialog.open({ title: '��ʼ�', url: "../../CRM/mail/get_atta.aspx?mail_id=" + getparastr("mail_id"), width: 580, height: 400, zindex: 9003 });
            })
        });

        function initmail() {
            var mail_id = getparastr("mail_id");
            if (mail_id && mail_id != "null") {
                $.ajax({
                    type: "GET",
                    url: "../../data/mail.ashx", /* �ע���������ֶ�ӦCS�ķ����� */
                    data: { Action: 'view', nid: mail_id, isView: "view", rnd: Math.random() }, /* �ע�����ĸ�ʽ���� */
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        var obj = eval(result);
                        for (var n in obj) {
                            
                        }

                        //alert(obj.constructor); //String ƹ��캯�
                        $("#mail_title").val("�ת��:" + obj.mail_title);
                        var content = "<br/>";
                        content += "<div style='background-color:#ddd'>";
                        content += "    <div>------------------&nbsp;ԭʼ�ʼ&nbsp;------------------</div>";
                        content += "    <div>";
                        content += "        <div><b>���:</b>&nbsp;" + obj.create_name + ";</div>";
                        content += "        <div><b>˷���ʱ�:</b>&nbsp;" + obj.create_time + "</div>";
                        content += "        <div><b>����:</b>&nbsp;" + obj.mail_title + "</div>";
                        content += "    </div>";
                        content += "</div>";
                        content += "<br/>";
                        UE.getEditor('editor').setContent(content + myHTMLDeCode(obj.mail_content));
                        $("#atta_count").text(obj.atta_count);
                    }
                });
            }
            else {
                alert("�ϵͳ�쳣��");
                top.$.ligerDialog.close();
            }
        }
        function initcombo() {
            $("#receiver").ligerComboBox({ width: 500, onBeforeOpen: f_selectContact1 })
            $("#CC").ligerComboBox({ width: 500, onBeforeOpen: f_selectContact2 })
            $("#Blindpeople").ligerComboBox({ width: 500, onBeforeOpen: f_selectContact3 })
        }
        function f_selectContact1() {
            f_selecttype("receiver");
            return false;
        }
        function f_selectContact2() {
            f_selecttype1("CC");
            return false;
        }
        function f_selectContact3() {
            f_selecttype2("Blindpeople");
            return false;
        }
        function f_selecttype(id) {
            top.$.ligerDialog.open({
                zindex: 9003,
                title: 'ѡ���ϵ�', width: 600, height: 350, url: 'hr/GetMultEmp.aspx?emp_ids=' + $("#receiver_val").val(), buttons: [
                    { text: '�ȷ��', onclick: function (item, dialog) { f_selectContactOK(item, dialog, id) } },
                    { text: 'ȡ�', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
        }
        function f_selecttype1(id) {
            top.$.ligerDialog.open({
                zindex: 9003,
                title: '�ѡ���ϵ�', width: 600, height: 350, url: 'hr/GetMultEmp.aspx?emp_ids=' + $("#CC_val").val(), buttons: [
                    { text: '�ȷ��', onclick: function (item, dialog) { f_selectContactOK(item, dialog, id) } },
                    { text: 'ȡ�', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
        }
        function f_selecttype2(id) {
            top.$.ligerDialog.open({
                zindex: 9003,
                title: '�ѡ���ϵ�', width: 600, height: 350, url: 'hr/GetMultEmp.aspx?emp_ids=' + $("#Blindpeople_val").val(), buttons: [
                    { text: '�ȷ��', onclick: function (item, dialog) { f_selectContactOK(item, dialog, id) } },
                    { text: 'ȡ�', onclick: function (item, dialog) { dialog.close(); } }
                ]
            });
        }
        function f_selectContactOK(item, dialog, id) {
            var data = dialog.frame.f_getChecked();
            if (data.length < 1) {
                alert('���ѡ���!');
                return;
            }
            switch (id) {
                case "receiver":
                    $("#receiver").val(data[0].emp_list);
                    $("#receiver_val").val(data[0].emp_ids);
                    break;
                case "CC":
                    $("#CC").val(data[0].emp_list);
                    $("#CC_val").val(data[0].emp_ids);
                    break;
                case "Blindpeople":
                    $("#Blindpeople").val(data[0].emp_list);
                    $("#Blindpeople_val").val(data[0].emp_ids);
            }

            
            dialog.close();
            
            
        }


        function f_selectContactCancel(item, dialog) {
            dialog.close();
        }


        function f_save() {
            if ($(form1).valid()) {
                var arr = [];
                arr.push(UE.getEditor('editor').getContent());
                var sendtxt = "&Action=save&forward_id=" + getparastr("mail_id") + "&mail_content=" + escape(arr) + "&page_id=" + getparastr("a") + "&filecount=" + $("#atta_count").text();
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        var filecount = 0;

        function mail_attachment(file_id, file_name, real_name) {
            //(file_id + ":" + file_name + ":" + real_name+":"+getparastr("a"));
            $.ajax({
                url: "../../data/mail.ashx", type: "POST",
                data:
                {
                    Action: "insert_attachment",
                    file_id: file_id,
                    file_name: file_name,
                    real_name: real_name,
                    page_id: getparastr("a")
                },
                success: function (responseText) {
                    filecount += 1;
                }
            });
        }
        function del_attachment(file_id) {
            $.ajax({
                url: "../../data/mail.ashx", type: "POST",
                data:
                {
                    Action: "del_attachment",
                    file_id: file_id,
                    page_id: getparastr("a")
                },
                success: function (responseText) {
                    filecount -= 1;
                }
            });
        }
        function flush_attachment() {
            $.ajax({
                url: "../../data/mail.ashx", type: "POST",
                data:
                {
                    Action: "flush_attachment",
                    page_id: getparastr("a")
                },
                success: function (responseText) {
                    filecount = 0;
                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <div style="position: relative; z-index: 9999">


            <table style="width: 752px;margin:3px;" class="bodytable3">
                <tr>
                    <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                        <div style="width: 68px; text-align: right;">��ռ��ˣ�</div>
                    </td>
                    <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                        <input type="text" id="receiver" name="receiver" validate="{required:true}" />
                    </td>
                </tr>
                <tr>
                    <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                        <div style="width: 68px; text-align: right;">���ͣ�</div>
                    </td>
                    <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                        <input type="text" id="CC" name="CC" />
                    </td>

                </tr>
                <tr>
                    <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                        <div style="width: 68px; text-align: right;">���ͣ�</div>
                    </td>
                    <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                        <input type="text" id="Blindpeople" name="Blindpeople" />
                    </td>
                </tr>
                <tr>

                    <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                        <div style="width: 68px; text-align: right;">���⣺</div>
                    </td>
                    <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                        <input type="text" id="mail_title" name="mail_title" ltype="text" ligerui="{width:500}" validate="{required:true}" />
                    </td>
                </tr>
                <tr>
                    <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                        <div style="width: 40px; float: left">���</div>
                        <div style="width: 30px; float: left;">
                            <img src='../../images/IsAccessary.gif' />
                            <span id="atta_count"></span>
                        </div>
                    </td>
                    <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                        <input id="atta" type="button" style="width: 80px; height: 20px;" value="�鿴���" />
                    </td>
                </tr>
            </table>

        </div>

        <table>
            <tr>
                <td>
                    <textarea id="editor" style="width: 640px;"></textarea>

                </td>
            </tr>
        </table>
    </form>
</body>
</html>
