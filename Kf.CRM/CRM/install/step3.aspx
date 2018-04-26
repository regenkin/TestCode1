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

    <link href="../CSS/input.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" />
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../lib/json2.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            $("form").ligerForm();
            $("#btn_test").click(function () { check(); })
            $("#btn_next").click(function () { runconfig() })
        });
        function check() {
            if ($(form1).valid()) {
                var sendtxt = $("form :input").fieldSerialize() + "&Action=checkconnect&rnd=" + Math.random();
                $.ajax({
                    type: 'post',
                    url: '../Data/install.ashx',
                    data: sendtxt,
                    success: function (result) {
                        if (result == "false:connect") {
                            $.ligerDialog.error("���Ӵ�����������������û��������룡")
                        }
                        else if (result == "false:configed") {
                            $.ligerDialog.error('���ʧ�ܣ�ϵͳ�Ѿ����ù��������������ã�����conn.config�ļ����CompleteConfig������Ϊfalse��');
                        }
                        else if (result == "warn:dbname") {
                            $.ligerDialog.warn('�Ѵ�����Ϊ"' + $("#t_db_name").val() + '"�����ݿ�,�뱣֤�����ݿ�����KFCRM����ر�<br/>�����ӳɹ������Կ�ʼ���á�');
                            $("#btn_next").attr("disabled", "");
                        }
                        else if (result == "false:dbfile") {
                            $.ligerDialog.error('App_Data�ļ������Ѵ�����Ϊ"' + $("#t_db_name").val() + '.mdf"���ļ�');
                        }
                        else if (result == "success") {
                            $.ligerDialog.success("���ӳɹ������Կ�ʼ���á�");
                            $("#btn_next").attr("disabled", "");
                        }
                        else {
                            $.ligerDialog.error("ϵͳ����");
                        }
                        $("#btn_test").attr("disabled", "");

                    },
                    error: function () {
                        alert("���ʧ��");
                    },
                    beforeSend: function () {
                        $("#btn_test").attr("disabled", "disabled");
                    }

                });

            }
            else {
                $("#btn_next").attr("disabled", "disabled");
            }  
        }
        function runconfig() {
            if ($(form1).valid()) {
                var sendtxt = $("form :input").fieldSerialize() + "&Action=startconfig&rnd=" + Math.random();
                $.ajax({
                    type: 'post',
                    url: '../Data/install.ashx',
                    data: sendtxt,
                    beforeSend: function () {
                        $.ligerDialog.waitting("ϵͳ������,���Ժ�...");
                        $("#btn_next").attr("disabled", "disabled");
                    },
                    success: function (result) {                           
                        window.location.href = "success.aspx";
                    },
                    error: function () {
                        $.ligerDialog.closeWaitting();
                        $.ligerDialog.error("����ʧ�ܣ�ϵͳ����<br/>1��������˺��Ƿ��д������ݿ��Ȩ�ޡ�<br/>2�����Ѵ������ݿ⣬�뱣֤�����ݿ�����KFCRM����ر�<br/>3����ϵKFCRM�ٷ�������Ա�����");
                    }  
                });  
            }
            else {
                $("#btn_next").attr("disabled", "disabled");
            }
        }
        function btn_reset()
        {
            $("#btn_next").attr("disabled", "disabled");
        }
    </script>
    <style type="text/css">
        span { font-weight: bolder; }
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
                                        <span class="auto-style1"><strong>Step3:</strong></span>���ڶ��������л����������á�
                                    </p>
                                </td>
                                <td style="width: 300px; text-align: center;">
                                    <img src="../images/logo/logo.png" width="234" alt="Kinfar crm" /></td>
                            </tr>
                        </table>
                        <table class="bodytable0" style="width: 100%; margin: -1px; line-height: 25px;">

                            <tr>
                                <td class="table_title" colspan="3" height="25">���ݿ����ã�</td>
                            </tr>

                            <tr>
                                <td width="150px" class="table_label">����������</td>
                                <td height="25">
                                    <input type="text" id="t_name" name="t_name" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" />
                                </td>
                                <td height="25">�磺.\sqlexpress������SQL��������</td>
                            </tr>
                            <tr>
                                <td class="table_label">��¼����</td>
                                <td height="25">
                                    <input type="text" id="t_uid" name="t_uid" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" />
                                </td>
                                <td height="25">�磺sa������SQL��½�˺ţ��벻Ҫ��windows��½��֤</td>
                            </tr>
                            <tr>
                                <td class="table_label">���룺</td>
                                <td height="25">
                                    <input type="text" id="t_pwd" name="t_pwd" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" />
                                </td>
                                <td height="25">���ݿ��˺ŵ�����</td>
                            </tr>
                            <tr>
                                <td class="table_label">�Ƿ���ܣ�</td>
                                <td height="25">
                                    <input type="text" id="t_Encrypt" name="t_Encrypt" ltype="select" ligerui="{width:200,data:[{'text':'����',id:0},{'text':'������',id:1}],initValue:0}" validate="{required:true}" /></td>
                                <td height="25">�����ַ����Ƿ����</td>
                            </tr>
                            <tr>
                                <td class="table_label">���ݿ�����</td>
                                <td height="25">
                                    <input type="text" id="t_db_name" name="t_db_name" ltype="text" ligerui="{width:200}" validate="{required:true}" onchange="btn_reset()" /></td>
                                <td height="25">���ݿ����ƣ����Ѵ��������ݿ⣬����д�ѽ��õ����ݿ�����</td>
                            </tr>
                            <tr>
                                <td colspan="3">1���粻֪��д������ʹٷ���̳Ѱ�������<a href="http://bbs.kinfar.net" target="_blank">http://bbs.kinfar.net</a><br />
                                    2��������������ã�������������Է����ݶ�ʧ��</td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td height="145" style="text-align: center;">&nbsp;
                        <input type="button" value="��������" style="width: 80px; height: 25px;" id="btn_test" />&nbsp;
                        <input type="button" value="��ʼ��װ" style="width: 80px; height: 25px;" id="btn_next" disabled="disabled" /></td>
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
