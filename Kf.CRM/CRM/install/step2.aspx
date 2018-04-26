<%@ Page Language="C#" AutoEventWireup="true" %>

<%
    //ˢ�¾�̬��������  
    CRM.Data.install inss = new CRM.Data.install();
    string filename = Server.MapPath("/conn.config");
    inss.CheckConfig(filename);
    string filename1 = Server.MapPath("/Web.config");
    inss.CheckConfig(filename1);

    //�ж��Ƿ�������
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
    <title>KFCRM-��װ</title>
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
                    if (arr[0] == 1) $("#Span0").text("ͨ����"); else $("#Span0").text("ʧ�ܣ�");
                    if (arr[1] == 1) $("#Span1").text("ͨ����"); else $("#Span1").text("ʧ�ܣ�");
                    if (arr[2] == 1) $("#Span2").text("ͨ����"); else $("#Span2").text("ʧ�ܣ�");
                    if (arr[3] == 0) $("#Span3").text("ͨ����"); else $("#Span3").text("ʧ�ܣ������ã�");
                    $("span").css({ "font-weight": "bolder","color":"#f00" });
                    if (arr[0]==1 && arr[1]==1 && arr[2]==1&&arr[3]==0) {
                        $("#btn_next").attr("disabled", "");
                    }
                    else {
                        $("#btn_next").attr("disabled", "disabled");
                    }
                },
                error: function () {
                    alert("���ʧ��");
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
                        <h2>��ӭʹ��KFCRM</h2>

                        <table>
                            <tr>
                                <td style="width: 400px;">
                                    <p>
                                        <span class="auto-style1"><strong>Step2</strong></span>:���ڶ��������л������м�⣬��ȷ�����Ļ�������Ҫ��.
                                    </p>
                                    <p>
                                        <font color="red">ע��:</font>�������Ŀ¼���ļ�û��д���ɾ��Ȩ�����,��ѡ���Ŀ¼���ļ�-&gt;�Ҽ�����-&gt;��ȫ-&gt;���, ��&quot;�������������ѡ��&quot;������&quot;Network Service&quot;,���&quot;ȷ��&quot;.ѡ��&quot;����û�����&quot;��&quot;Network Service&quot;�û���,������ &quot;Network Service&quot;��Ȩ���й�ѡ&quot;�޸�&quot;��&quot;����&quot;��ѡ��,���&quot;ȷ��&quot;���ٴ�����ˢ�±�ҳ�����.
                                    </p>
                                </td>
                                <td style="width: 300px; text-align: center;">
                                    <img src="../images/logo/logo.png" width="234" alt="Kinfar crm" /></td>
                            </tr>
                        </table>
                        <table class="bodytable0" style="width: 100%; margin: -1px">

                            <tr>
                                <td height="23" width="50%" class="table_label">conn.config�ļ��Ƿ��д��</td>
                                <td height="23">
                                    <span id="Span0"></span>
                                </td>
                            </tr>
                            <tr>
                                <td height="23" class="table_label">install�ļ����Ƿ��д��</td>
                                <td height="23">
                                    <span id="Span1"></span>
                                </td>
                            </tr>
                            <tr>
                                <td height="23" class="table_label">App_Data�ļ����Ƿ��д��</td>
                                <td height="23">
                                    <span id="Span2"></span>
                                </td>
                            </tr> 
                            <tr>
                                <td height="23" class="table_label">�Ƿ��Ѿ�������ϣ�</td>
                                <td height="23">
                                    <span id="Span3"></span>
                                </td>
                            </tr>
                            <tr>
                                <td height="23" colspan="2">ע�����Ѿ�������ϣ�����Ҫ�������ã�����conn.config�ļ���ѡ�<span class="auto-style1">CompleteConfig</span>��������Ϊ<span class="auto-style1">false</span></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="145" style="text-align: center;">
                        <input type="button" value="���¼��" style="width: 80px; height: 25px;" id="btn_retry" />
                        <input type="button" value="��һ��" style="width: 80px; height: 25px;" id="btn_next" disabled="disabled" />
                    </td>
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
