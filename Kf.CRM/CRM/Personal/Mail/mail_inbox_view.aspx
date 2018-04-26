<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" />
    <link href="../../CSS/input.css" rel="stylesheet" />
    <script type="text/javascript" src="../../lib/jquery/jquery-1.3.2.min.js"></script>
    <script type="text/javascript" src="../../lib/ligerUI/js/plugins/ligerDialog.js"></script>
    <script type="text/javascript" src="../../lib/ligerUI/js/plugins/ligerTextBox.js"></script>
    <script type="text/javascript" src="../../JS/KFCRM.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#mail_receive").ligerTextBox({ width: 570, disabled: true });
            $("#mail_cc").ligerTextBox({ width: 570, disabled: true });
            $.ajax({
                type: "GET",
                url: "../../data/mail.ashx", /* ע���������ֶ�ӦCS�ķ����� */
                data: { Action: 'view', nid: getparastr("nid"), isView: "view", rnd: Math.random() }, /* �ע�����ĸ�ʽ���� */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        
                    }

                    //alert(obj.constructor); //String ƹ��캯�
                    $("#mail_title").html(obj.mail_title);
                    $("#mail_sender").html(obj.create_name);
                    $("#mail_content").html(myHTMLDeCode(obj.mail_content));
                    $("#atta_count").text(obj.atta_count);
                }
            });
            $.ajax({
                type: "GET",
                url: "../../data/mail.ashx", /* �ע���������ֶ�ӦCS�ķ����� */
                data: { Action: 'get_receive_list', mail_id: getparastr("nid"), rnd: Math.random() }, /* �ע�����ĸ�ʽ���� */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    var rows = obj.Rows;
                    //alert(obj.Rows.length);
                    //for (var n in obj) {
                    //    
                    //}

                    //alert(obj.constructor); //String ƹ��캯�
                    var receive_list = "";
                    var cc_list = "";
                    for (var n in rows) {
                        if (rows[n].recive_type_id == 1) {
                            receive_list += rows[n].receive_name + ";";
                        }
                        else if (rows[n].recive_type_id == 2) {
                            cc_list += rows[n].receive_name + ";";
                        }
                    }
                    $("#mail_receive").val(receive_list);
                    $("#mail_cc").val(cc_list);
                }
            });
            $("#more_recieve").bind("click", function () {
                f_alert($("#mail_receive").val(), "��ռ��");
            })
            $("#more_cc").bind("click", function () {
                f_alert($("#mail_cc").val(), "˳��");
            })
            $("#atta").bind("click", function () {
                $.ligerDialog.open({ title: '��ʼ�', url: "get_atta.aspx?mail_id=" + getparastr("nid"), width: 580, height: 400 });
            })
        })
        function f_alert(content, title) {
            $.ligerDialog.alert(content, title, 'none');
        }
        function getAtta() {
            var grid = {
                columns: [
                    { display: '���', width: 50, render: function (item, i) { return i + 1; } },
                    { display: '��ļ��', name: 'file_name', width: 400, align: 'left' },
                    {
                        display: '����', name: 'receive_name', width: 100, render: function (item) {
                            //return "<a href='../../file/" + item.real_name + "'>����</a>";
                            return "<a href='javascript:void(0)' onclick='javascript:window.open(\"../../file/" + item.real_name + "\")'>����</a>";
                        }
                    }
                ],
                url: "../../data/mail.ashx?Action=Get_atta_list&mail_id=" + getparastr("nid"),
                pageSize: 30,
                checkbox: false
            };
            return grid;
        }

        /* ajax������ļ 
        @url: ��ļurl�·��
        */

        function download_file(url) {
            if (typeof (download_file.iframe) == "undefined") {
                var iframe = document.createElement("iframe");
                download_file.iframe = iframe;
                document.body.appendChild(download_file.iframe);
            }
            // alert(download_file.iframe);
            var url = '../../file/' + filename;

            download_file.iframe.src = url;
            download_file.iframe.style.display = "none";
        }


    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">


        <table style="margin:2px;border-collapse: collapse;width:738px;">
            <tr>
                <td colspan="2" class="table_title" style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                   <div style="width:32px;float:left"><img src="../../images/icons/32X32/email.gif" width="32" height="32" /></div>
                    <div id="mail_title" style="text-align: left; font-size: x-large; font-weight: bold;width:690px; float:left;"></div>
                </td>
            </tr>
            <tr>
                <td style="width: 80px; border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">�����ˣ�</td>
                <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px; padding: 2px; line-height: 22px;">
                    <div id="mail_sender" style="text-align: left; font-size: 12px;"></div>
                </td>
            </tr>
            <tr>
                <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">�����ˣ�</td>
                <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                    <div style="width: 572px; float: left">
                        <input id="mail_receive" type="text" />

                    </div>
                    <div style="width: 50px; float: left; padding-left: 2px;">
                        <input type="button" value="��" id="more_recieve">
                    </div>
                </td>
            </tr>
            <tr>
                <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">೭�ͣ�</td>
                <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                    <div style="width: 572px; float: left">
                        <input id="mail_cc" type="text" />
                    </div>
                    <div style="width: 50px; float: left; padding-left: 2px;">
                        <input type="button" value="��" id="more_cc">
                    </div>
                </td>
            </tr>
            <tr>
                <td style="border: 1px solid #b6d6e6; padding: 2px; line-height: 22px;">
                    <div style="width: 40px; float: left">฽�</div>
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
        <div id="mail_content" style="height: 305px; overflow-y: scroll; margin: 2px;border:1px solid #b6d6e6;"></div>





    </form>
</body>
</html>
