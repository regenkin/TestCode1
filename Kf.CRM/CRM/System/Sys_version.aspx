<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache,must-revalidate" />
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../JS/Toolbar.js" type="text/javascript"></script>
    <link href="../CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/core.css" rel="stylesheet" type="text/css" />
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script type="text/javascript">
        var sys_id = "",sys_name="",sys_version="";
        $(function () {
            $.ajax({
                type: "GET",
                url: "../data/sys_info.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                data: { Action: 'grid', rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    var rows = obj.Rows;
                    for (var i = 0; i < rows.length;i++) {
                        if (rows[i].sys_value == "null" || rows[i].sys_value == null) {
                            rows[i].sys_value = " ";
                        }
                    }
                    sys_id = rows[0].sys_value;
                    sys_name = rows[1].sys_value;
                    sys_version = rows[3].sys_value;
                    //alert(obj.constructor); //String ���캯��
                    $("#Label1").text(sys_version);
                    $("#Label2").attr(rows[4].sys_value);
                }
            });
        });
        function checkup()
        {
            var T_name="";
            if ($("#T_send").attr("checked"))
            {
                T_name=sys_name;
            }
            $.ajax({
                type: "GET",
                url: "http://server.kinfar.net/data/crm_version.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                data: { Action: 'getversion',T_guid:sys_id,T_name:T_name, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    if (result.version_name == sys_version) {
                        $.ligerDialog.success("�Ѿ������°汾������������");
                    }
                    else {
                        $.ligerDialog.warn("��"+result.version_name+"�汾���Ը��£���ǰ�� <a herf='http://www.kinfar.net/down'>KFCRM����</a> ���ظ��¡�")
                    }
                }
            })
        }

    </script>
</head>
<body style="padding: 0px">
    <form id="form1">
        <table class="bodytable0" style="width: 100%; margin: -1px">

            <tr>
                <td height="23" width="150" class="title" colspan="2" style="border-top: none;">ϵͳ��Ϣ</td>
            </tr>

            <tr>
                <td height="23" width="150" class="table_label">��ǰ�汾�ţ�</td>
                <td height="23">
                    <span id="Label1">1</span>
                </td>
            </tr>
            <tr>
                <td height="23" class="table_label">��ǰ�汾��Ϣ��</td>
                <td height="23">
                    <span id="Label2"></span>
                </td>
            </tr>
            <tr>
                <td height="23" class="table_label">�û�����ƻ���</td>
                <td height="23">
                    <input id="T_send" type="checkbox" checked="checked" />������ҵ��</td>
            </tr>
            <tr>
                <td height="23" class="table_label">&nbsp;</td>
                <td height="23">
                    <span id="Label4"><input type="button" value="������" style="width:80px;height:22px;" onclick="checkup()"></span>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
