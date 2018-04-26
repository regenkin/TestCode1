<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" />
    <meta http-equiv="X-UA-Compatible" content="ie=8 chrome=1" />
    <script src="../../lib/jquery/jquery-1.5.2.min.js" type="text/javascript"></script> 
    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>  
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        function checkpath()
        {
            var path = $("#upload").val();
            var filename = path.substr(path.lastIndexOf("\\")).toLowerCase();
            // alert(filename);
            if (filename == "\\customer_template.xls") {
                $("#btn_up").attr("disabled", "");
                alert("�����ϴ�������ȷ����ť��ʼ���롣");
            }
            else {
                $("#btn_up").attr("disabled", "disabled");
                alert("��ѡ����ļ��ƺ��Ǵ���ģ��������顣");
            }
        }
        function update()
        {
            var filename = $("#upload").val();
            $.ligerDialog.waitting('���ݱ�����,���Ժ�...');
            $("#form1").ajaxSubmit({
                type: "post",
                url: "../../data/upload.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                data: { Action: 'cus_import', filename: filename, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: function (result) {
                    
                    $.ajax({
                        url: "../../data/CRM_Customer.ashx", type: "POST",
                        data: { Action:"import",file:result,rnd:Math.random() },
                        success: function (responseText) {
                            $.ligerDialog.closeWaitting();
                            top.frames["tabid4"].f_reload();
                            setTimeout(function () { top.$.ligerDialog.close() }, 100);
                            
                        },
                        error: function () {
                            $.ligerDialog.closeWaitting();
                            $.ligerDialog.error('����ʧ�ܣ�');
                        }
                    });
                   //
                },
                error: function () {
                    $.ligerDialog.closeWaitting();
                    $.ligerDialog.error('����ʧ�ܣ�');
                }
            });
            
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 502px; margin: 5px;" class='bodytable1'>
            <tr>
                <td class="table_title1">����������</td>
            </tr>
            <tr>
                <td >
                    1������ģ�壺<a href="../../file/template/customer_template.xls">�ͻ�����ģ��</a><br />
                    2������ģ���ʽ������д���벻Ҫ�޸�ģ��ṹ��ģ���ļ����ƣ�����sheet1����д������<br />
                    3��������������ѡ����д�õ�ģ�壬������ϴ������롱��<br />
                    4����������ݲ���һ������ȫ�ɹ��������������ϵKFCRM�ٷ�������Ա��<br />
                    </td>
            </tr>
            <tr>
                <td class="table_title1">������</td>
            </tr>
            <tr>
                <td >
                    <input name="upload" type="file" id="upload" onchange="checkpath()" style="width: 250px; height: 21px;" runat="server" /> 
                     <input type="button" id="btn_up" value="�ϴ�������" style="width: 80px; height: 21px;" disabled="disabled" onclick="update()"/>
                </td>
            </tr>
            </table>


    </form>
</body>
</html>
