<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache,must-revalidate" />
    <script type="text/javascript" src="../js/jquery.min.js"></script>
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () { });

        function f_save() {
            var filename = $("#File1").val();
            if (filename && filename != "null" && filename != "" && filename != null) {
                var location = "file:///" + $("#File1").val();
                var type = location.substr(location.lastIndexOf(".")).toLowerCase();
                if (type == ".jpg" || type == ".gif" || type == ".jpeg" || type == ".bmp" || type == ".png") {

                    if (confirm("�ϴ����滻��ԭlogo��\n��ȷ��Ҫ�滻��")) {
                        $("#form1").ajaxSubmit({
                            type: "post",
                            url: "../data/sys_info.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                            data: { Action: 'logo', filename: filename, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                            contentType: "application/json; charset=utf-8",
                            dataType: "text",
                            beforeSend: function (xmlHttp) {
                                xmlHttp.setRequestHeader("If-Modified-Since", "0");
                                xmlHttp.setRequestHeader("Cache-Control", "no-cache");
                            },
                            success: function (result) {
                                setTimeout(function () { top.$.ligerDialog.close(); },100);
                            }
                        });
                    }
                }
                else {
                    alert("��ѡ��ͼƬ�����ļ���");
                }
            }
            else {
                alert("��ѡ��ͼƬ��");
            }
        }
       

    </script>
</head>
<body style="padding: 0px">
    <form runat="server" id="form1">
        <table class="bodytable0" style="width: 99%; margin: 2px">
            <tr>
                <td height="23">
                    <div style="width: 300px; float: left">
                        <input id="File1" type="file" runat="server" validate="{required:true}" style="height: 21px; width: 295px;" />
                    </div>
                    
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
