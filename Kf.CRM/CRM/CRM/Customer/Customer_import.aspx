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
                alert("可以上传，请点击确定按钮开始导入。");
            }
            else {
                $("#btn_up").attr("disabled", "disabled");
                alert("您选择的文件似乎是错误的，请认真检查。");
            }
        }
        function update()
        {
            var filename = $("#upload").val();
            $.ligerDialog.waitting('数据保存中,请稍候...');
            $("#form1").ajaxSubmit({
                type: "post",
                url: "../../data/upload.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'cus_import', filename: filename, rnd: Math.random() }, /* 注意参数的格式和名称 */
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
                            $.ligerDialog.error('操作失败！');
                        }
                    });
                   //
                },
                error: function () {
                    $.ligerDialog.closeWaitting();
                    $.ligerDialog.error('操作失败！');
                }
            });
            
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 502px; margin: 5px;" class='bodytable1'>
            <tr>
                <td class="table_title1">操作方法：</td>
            </tr>
            <tr>
                <td >
                    1、下载模板：<a href="../../file/template/customer_template.xls">客户导入模板</a><br />
                    2、根据模板格式认真填写，请不要修改模板结构及模板文件名称，并在sheet1里填写完整。<br />
                    3、点击“浏览”，选择填写好的模板，点击“上传并导入”。<br />
                    4、导入的数据并不一定能完全成功，如果错误，请联系KFCRM官方技术人员。<br />
                    </td>
            </tr>
            <tr>
                <td class="table_title1">操作：</td>
            </tr>
            <tr>
                <td >
                    <input name="upload" type="file" id="upload" onchange="checkpath()" style="width: 250px; height: 21px;" runat="server" /> 
                     <input type="button" id="btn_up" value="上传并导入" style="width: 80px; height: 21px;" disabled="disabled" onclick="update()"/>
                </td>
            </tr>
            </table>


    </form>
</body>
</html>
