<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            var realname = getparastr("realname");
            if (realname && realname!=undefined && realname!="undefined") {
                $("#view").attr("src", "../file/contract/" + realname);
            }
            else {
                $.ajax({
                    type: "GET",
                    url: "../data/CRM_contract_attachment.ashx", /* 注意后面的名字对应CS的方法名称 */
                    data: { Action: 'get_realname', page_id: getparastr("page_id"), filename: getparastr("filename"), rnd: Math.random() }, /* 注意参数的格式和名称 */
                    contentType: "application/json; charset=utf-8",
                    dataType: "text",
                    success: function (result) {                          
                        //alert(obj.constructor); //String 构造函数
                        if (result == "sucess:false") {
                            $("#view").remove();
                            $.ligerDialog.warn("系统错误！找不到地址");
                        }
                        else {                             
                            $("#view").attr("src", "../file/contract/" + result);
                        } 
                    }
                });
            }
        })
    </script>
</head>
<body style="overflow: scroll;">
    <img id="view" />
</body>
</html>
