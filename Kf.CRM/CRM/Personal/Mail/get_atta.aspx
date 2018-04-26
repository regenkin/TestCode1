<%@ Page Language="C#" AutoEventWireup="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
   
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '��', width: 50, render: function (item, i) { return i + 1; } },
                    { display: '��ļ��', name: 'file_name', width: 400, align: 'left' },
                    {
                        display: '����', name: 'receive_name', width: 100, render: function (item) {
                            //return "<a href='../../file/" + item.real_name + "'>����</a>";
                            return "<a href='javascript:void(0)' onclick='javascript:window.open(\"../../file/" + item.real_name + "\")'>����</a>";
                        }
                    }
                ],
                url: "../../data/mail.ashx?Action=Get_atta_list&mail_id=" + getparastr("mail_id"),
                width: "552px",
                height: "355px",
                pageSize: 30,
                checkbox: false
            });
        })

    </script>
</head>
<body style="margin: 0;overflow:hidden;">
    <form id="form1" onsubmit="return false">



        <div id="maingrid4" style="margin: -1px;"></div>


    </form>

</body>
</html>

