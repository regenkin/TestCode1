<%@ Page Language="C#" AutoEventWireup="true" %>

<%
    //刷新静态方法缓存  
    CRM.Data.install inss = new CRM.Data.install();
    string filename = Server.MapPath("/conn.config");
    inss.CheckConfig(filename);
    string filename1 = Server.MapPath("/Web.config");
    inss.CheckConfig(filename1);

    //判断是否已配置
    CRM.Data.install ins = new CRM.Data.install();
    int configed = ins.configed();

    if (configed == 1)
    {
       Response.Redirect( "remind.aspx");
    }    
   
%>


<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3c.org/TR/1999/REC-html401-19991224/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="ie=8" http-equiv="X-UA-Compatible">
    <meta http-equiv="content-type" content="text/html; charset=gb2312">
    <title>KFCRM-安装</title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-dialog.css" rel="stylesheet" />
    <link href="../CSS/input.css" rel="stylesheet" />
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btn_retry").click(function () { check(); })
            $("#btn_next").click(function () { window.location.href = "step3.aspx"; })
            check();
        });
        function check() {
            $.ajax({
                type: 'post',
                url: '../Data/install.ashx',
                data: [
                { name: 'Action', value: 'initcheck' },
                { name: 'rnd', value: Math.random() }
                ],
                success: function (result) {
                    var arr = new Array();
                    arr = result.split(",");
                    if (arr[0] == 1) $("#Span0").text("通过！"); else $("#Span0").text("失败！");
                    if (arr[1] == 1) $("#Span1").text("通过！"); else $("#Span1").text("失败！");
                    if (arr[2] == 1) $("#Span2").text("通过！"); else $("#Span2").text("失败！");
                    if (arr[3] == 0) $("#Span3").text("通过！"); else $("#Span3").text("失败，已配置！");
                    $("span").css({ "font-weight": "bolder","color":"#f00" });
                    if (arr[0]==1 && arr[1]==1 && arr[2]==1&&arr[3]==0) {
                        $("#btn_next").attr("disabled", "");
                    }
                    else {
                        $("#btn_next").attr("disabled", "disabled");
                    }
                },
                error: function () {
                    alert("检测失败");
                }
            });
        }
    </script>
    <style type="text/css">
        span { font-weight:bolder;}
        img { border: none; }
        .text { border: #d2e2f2 1px solid; height: 19px; }
        body { BACKGROUND: url(../images/login/loginbackground1.jpg) repeat-x; font-size: 12px; }
        .auto-style1 { color: #FF0000; }
    </style>
    <script type="text/javascript">
        if (top.location != self.location) top.location = self.location;
    </script>
</head>
<body>
    <form id="form1" name="form1">

        <div style="width: 731px; margin-left: 50px; margin-top: 100px;">
            <table class="bodytable3" id="Table1" width="732" height="358" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <h2>欢迎使用KFCRM</h2>

                        <table>
                            <tr>
                                <td style="width: 400px;">
                                    <p>
                                        <span class="auto-style1"><strong>Step2</strong></span>:现在对您的运行环境进行检测，以确认您的环境符合要求.
                                    </p>
                                    <p>
                                        <font color="red">注意:</font>如果出现目录或文件没有写入和删除权限情况,请选择该目录或文件-&gt;右键属性-&gt;安全-&gt;添加, 在&quot;输入对象名称来选择&quot;中输入&quot;Network Service&quot;,点击&quot;确定&quot;.选择&quot;组或用户名称&quot;中&quot;Network Service&quot;用户组,在下面 &quot;Network Service&quot;的权限中勾选&quot;修改&quot;的&quot;允许&quot;复选框,点击&quot;确定&quot;后再次重新刷新本页面继续.
                                    </p>
                                </td>
                                <td style="width: 300px; text-align: center;">
                                    <img src="../images/logo/logo.png" width="234" alt="Kinfar crm" /></td>
                            </tr>
                        </table>
                        <table class="bodytable0" style="width: 100%; margin: -1px">

                            <tr>
                                <td height="23" width="50%" class="table_label">conn.config文件是否可写：</td>
                                <td height="23">
                                    <span id="Span0"></span>
                                </td>
                            </tr>
                            <tr>
                                <td height="23" class="table_label">install文件夹是否可写：</td>
                                <td height="23">
                                    <span id="Span1"></span>
                                </td>
                            </tr>
                            <tr>
                                <td height="23" class="table_label">App_Data文件夹是否可写：</td>
                                <td height="23">
                                    <span id="Span2"></span>
                                </td>
                            </tr> 
                            <tr>
                                <td height="23" class="table_label">是否已经配置完毕：</td>
                                <td height="23">
                                    <span id="Span3"></span>
                                </td>
                            </tr>
                            <tr>
                                <td height="23" colspan="2">注：如已经配置完毕，您需要重新配置，请在conn.config文件里，把“<span class="auto-style1">CompleteConfig</span>”节配置为<span class="auto-style1">false</span></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="145" style="text-align: center;">
                        <input type="button" value="重新检测" style="width: 80px; height: 25px;" id="btn_retry" />
                        <input type="button" value="下一步" style="width: 80px; height: 25px;" id="btn_next" disabled="disabled" />
                    </td>
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
