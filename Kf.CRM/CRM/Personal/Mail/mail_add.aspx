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
                initialFrameWidth: 735,
                initialFrameHeight: 295,
                toolbars: [
               [
                'bold', 'italic', 'underline', '|',
                'justifyleft', 'justifycenter', 'justifyright', 'justifyjustify', '|',
                'rowspacingtop', 'rowspacingbottom', 'lineheight', '|',
                'customstyle', 'paragraph', 'fontfamily', 'fontsize', '|',
               ]
                ],
                autoHeightEnabled: false,
                initialContent: '�ʼ�����'
            });

            initcombo();
            swfupload();
            flush_attachment();
        });

        function swfupload() {
            var settings = {
                flash_url: "../../js/swfupload/swfupload.swf",
                upload_url: "../../data/upload.ashx?Action=Mult",
                post_params: {
                    "ASPSESSID": "<%=Session.SessionID %>"
                },
                file_size_limit: "10 MB",
                file_types: "*.*",
                file_types_description: "All Files",
                file_upload_limit: 100,
                file_queue_limit: 0,
                custom_settings: {

                    progressTarget: "divprogresscontainer",
                    progressGroupTarget: "divprogressGroup",

                    //progress object
                    container_css: "progressobj",
                    icoNormal_css: "IcoNormal",
                    icoWaiting_css: "IcoWaiting",
                    icoUpload_css: "IcoUpload",
                    fname_css: "fle ftt",
                    state_div_css: "statebarSmallDiv",
                    state_bar_css: "statebar",
                    percent_css: "ftt",
                    href_delete_css: "ftt",

                    //sum object
                    /*
                        ҳ���в�Ӧ������"cnt_"��ͷ������Ԫ��
                    */
                    s_cnt_progress: "cnt_progress",
                    s_cnt_span_text: "fle",
                    s_cnt_progress_statebar: "cnt_progress_statebar",
                    s_cnt_progress_percent: "cnt_progress_percent",
                    s_cnt_progress_uploaded: "cnt_progress_uploaded",
                    s_cnt_progress_size: "cnt_progress_size"
                },
                debug: false,

                // Button settings
                button_image_url: "images/TestImageNoText_65x29.png",
                button_width: "65",
                button_height: "29",
                button_placeholder_id: "spanButtonPlaceHolder",
                button_text: '<span class="theFont">��Ӹ���</span>',
                button_text_style: ".theFont { font-size: 12;color:#0068B7; }",
                button_text_left_padding: 12,
                button_text_top_padding: 3,

                // The event handler functions are defined in handlers.js
                file_queued_handler: fileQueued,
                file_queue_error_handler: fileQueueError,
                upload_start_handler: uploadStart,
                upload_progress_handler: uploadProgress,
                upload_error_handler: uploadError,
                upload_success_handler: uploadSuccess,
                upload_complete_handler: uploadComplete,
                file_dialog_complete_handler: fileDialogComplete
            };
        swfu = new SWFUpload(settings);
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
            title: 'ѡ����ϵ��', width: 600, height: 350, url: 'hr/GetMultEmp.aspx?emp_ids=' + $("receiver_val").val(), buttons: [
                { text: 'ȷ��', onclick: function (item, dialog) { f_selectContactOK(item, dialog, id) } },
                { text: 'ȡ��', onclick: function (item, dialog) { dialog.close(); } }
            ]
        });
    }

    function f_selecttype1(id) {
        top.$.ligerDialog.open({
            zindex: 9003,
            title: 'ѡ����ϵ��', width: 600, height: 350, url: 'hr/GetMultEmp.aspx?emp_ids=' + $("CC_val").val(), buttons: [
                { text: 'ȷ��', onclick: function (item, dialog) { f_selectContactOK(item, dialog, id) } },
                { text: 'ȡ��', onclick: function (item, dialog) { dialog.close(); } }
            ]
        });
    }

    function f_selecttype2(id) {
        top.$.ligerDialog.open({
            zindex: 9003,
            title: 'ѡ����ϵ��', width: 600, height: 350, url: 'hr/GetMultEmp.aspx?emp_ids=' + $("Blindpeople_val").val(), buttons: [
                { text: 'ȷ��', onclick: function (item, dialog) { f_selectContactOK(item, dialog, id) } },
                { text: 'ȡ��', onclick: function (item, dialog) { dialog.close(); } }
            ]
        });
    }
    function f_selectContactOK(item, dialog, id) {
        var data = dialog.frame.f_getChecked();
        if (data.length < 1) {
            alert('��ѡ����!');
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
        if (fg_uploads == fg_fileSizes) {
            if ($(form1).valid()) {
                var arr = [];
                arr.push(UE.getEditor('editor').getContent());
                var sendtxt = "&Action=save&nid=" + getparastr("nid") + "&mail_content=" + escape(arr) + "&page_id=" + getparastr("a") + "&filecount=" + filecount;
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }
        else {
            alert("��ȴ������ϴ���ɡ�");
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


            <table style="width: 650px" class="bodytable3">
                <tr>
                    <td>
                        <div style="width: 68px; text-align: right;">�ռ��ˣ�</div>
                    </td>
                    <td>
                        <input type="text" id="receiver" name="receiver" validate="{required:true}" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="width: 68px; text-align: right;">���ͣ�</div>
                    </td>
                    <td>
                        <input type="text" id="CC" name="CC" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <div style="width: 68px; text-align: right;">���ͣ�</div>
                    </td>
                    <td>
                        <input type="text" id="Blindpeople" name="Blindpeople" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <div style="width: 68px; text-align: right;">���⣺</div>
                    </td>
                    <td>
                        <input type="text" id="mail_title" name="mail_title" ltype="text" ligerui="{width:500}" validate="{required:true}" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="min-height: 29px;">
                        <span id="spanButtonPlaceHolder"></span>
                        <div id="divprogresscontainer"></div>
                        <div id="divprogressGroup"></div>
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
