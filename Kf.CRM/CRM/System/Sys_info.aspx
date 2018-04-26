
<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />
    
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>   
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $("#T_name").ligerTextBox({
                width: 200,
                disabled: true
            });
            loadform();
           

            $("#btn_name").click(function () {
                var manager = $("#T_name").ligerGetTextBoxManager();
                if ($(this).val() == "修改")
                {
                    $("#btn_name").val("保存");                    
                    manager.setEnabled();

                }
                else if ($(this).val() == "保存")
                {
                    $("#btn_name").val("修改");
                    manager.setDisabled();

                    if ($("#T_name").val() != "")
                    {
                        $.ajax({
                            url: "../data/sys_info.ashx", type: "POST",
                            data: {
                                Action: "up",         
                                T_name: $("#T_name").val()
                            },
                            success: function (responseText) {
                                $.ligerDialog.success("更新成功！");
                            },
                            error: function () {        
                                $.ligerDialog.error('操作失败！');
                            }
                        });
                    }
                }
            })

            $("#btn_logo").click(function () {
                top.$.ligerDialog.open({
                    width: 400, height: 80, url: "System/sys_info_add.aspx", title: "Logo修改", buttons: [
                        {
                            text: '保存', onclick: function (item, dialog) {
                                dialog.frame.f_save();
                                setTimeout(loadform,200);
                            }
                        },
                        {
                            text: '关闭', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                    ]
                });
            })
        });
        function loadform()
        {
            $.ajax({
                type: "GET",
                url: "../data/sys_info.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'grid', rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    var rows = obj.Rows;

                    //alert(obj.constructor); //String 构造函数
                    $("#T_name").val(rows[1].sys_value);
                    $("#logo").attr("src", "../" + rows[2].sys_value);
                }
            });
        }

    </script>
</head>
<body style="padding: 0px">
    <form runat="server" id="form1">
        <table class="bodytable0" style="width: 100%; margin: -1px">

            <tr>
                <td height="23" width="150" class="title" colspan="3" style="border-top:none;">系统信息</td>
            </tr>

            <tr>
                <td height="23" width="150" class="table_label">公司名称：</td>
                <td height="23">
                    <input type="text" name="T_name" id="T_name" />
                </td>
                <td height="23">
                     
                    <input type="button" value="修改" style="width:80px;height:22px;" id="btn_name"/></td>
            </tr>
            <tr>
                <td height="23" class="table_label">公司log：</td>
                <td height="23">
                    <img id="logo" style="height: 54px" />
                </td>
                <td height="23">
                     
                    <input type="button" value="修改" style="width:80px;height:22px;" id="btn_logo"/></td>
            </tr>
            </table>
    </form>
</body>
</html>
