<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="application/msword; charset=utf-8" />
    <title></title>


</head>
<body style="margin: 0; padding: 0; overflow: hidden;">
    <div id="pageloading">
        <div style="margin: 180px auto; text-align: center;">文档努力加载中... ...</div>
    </div>
    <iframe id="view" frameborder='0' style="width: 100%; margin: 0;"></iframe>
</body>
</html>
<link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
<script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
<script src="../JS/KFCRM.js" type="text/javascript"></script>
<script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
<script type="text/javascript">
    $(function () { 
        $.ajax({
            type: "GET",
            url: "../data/CRM_contract_attachment.ashx", /* 注意后面的名字对应CS的方法名称 */
            data: { Action: 'get_realname', page_id: getparastr("page_id"), realname: getparastr("realname"), filename: getparastr("filename"), rnd: Math.random() }, /* 注意参数的格式和名称 */
            contentType: "application/msword; charset=utf-8",
            dataType: "text",
            success: function (result) {
                //alert(obj.constructor); //String 构造函数
                $("#view").attr("src", "../file/contract/" + result);
                $("#pageloading").fadeOut(800);
            }
        }); 
    })
   
</script>
