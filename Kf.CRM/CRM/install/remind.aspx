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
       Response.Redirect( "../login.aspx");
    }
    else
    {
        Response.Redirect("../login1.aspx");
    }
   
 %>

<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN" "http://www.w3c.org/TR/1999/REC-html401-19991224/loose.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="ie=8" http-equiv="X-UA-Compatible">
    <meta http-equiv="content-type" content="text/html; charset=gb2312">
    <title>KFCRM-��װ</title>   
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>  
    <script type="text/javascript">
          $(function () {
           
            $("#btn_next").click(function () {
                window.location.href = "../login.aspx";
            });             
        })
    </script>
    <style type="text/css">
        img { border: none; }
        .text { border: #d2e2f2 1px solid; height: 19px; }
        body { BACKGROUND: url(../images/login/loginbackground1.jpg) repeat-x; font-size: 12px; }
        .auto-style1 { color: #FF0000; }
    
* { font-size: 12px; }
* { font-family: Verdana, Geneva, sans-serif; font-size: 12px; }
        .auto-style2 { font-weight: bolder; }
    </style>
    <script type="text/javascript">
        if (top.location != self.location) top.location = self.location;
    </script>
</head>
<body>
    <form id="form1" name="form1">
        <div style="width: 731px; margin-left: 50px; margin-top: 100px;">
            <table id="__01" width="732" height="358" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <h2>��ӭʹ��KFCRM</h2>

                        <table class="auto-style1">
                            <tr>
                                <td style="border-bottom: 1px solid #d2e2f2; width: 400px;">
                                    <p class="l-log-content">
                                        ����: Kinfar
                                    </p>
                                    <p class="l-log-content">
                                        QQ: 313321669
                                    </p>
                                    <p class="l-log-content">
                                        ����QQȺ1Ⱥ: <a href="#" target="_blank">191647516</a>
                                    </p>
                                    <p class="l-log-content">
                                        �ٷ���վ��<a href="http://www.kinfar.net/" target="_blank">http://www.kinfar.net</a>
                                    </p>
                                    <p class="l-log-content">
                                        ��ʾ��ַ��<a href="http://www.kinfar.net:8001/" target="_blank">http://www.kinfar.net:8001</a>
                                    </p>

                                </td>
                                <td style="border-bottom: 1px solid #d2e2f2;">
                                    <img src="../images/logo/KfCrm.png" width="234" alt="Kinfar crm" /></td>
                            </tr>
                        </table>
                        <p>
                            <span style="WHITE-SPACE: normal; TEXT-TRANSFORM: none; WORD-SPACING: 0px; FLOAT: none; COLOR: rgb(0,0,0); FONT: 12px/25px ����,SimSun; DISPLAY: inline !important; LETTER-SPACING: normal; TEXT-INDENT: 0px; font-size-adjust: none; font-stretch: normal; -webkit-text-stroke-width: 0px">&nbsp; ϵͳ</span>�Ѿ�������ϣ�����Ҫ�������ã�����conn.config�ļ���ѡ�<span class="auto-style2">CompleteConfig</span>��������Ϊ<span class="auto-style2">false</span></p>
                    </td>
                </tr>
                <tr>
                    <td height="145" style="text-align: center;">
                        <input type="button" value="ͬ��" style="width: 80px; height: 25px;" id="btn_next" />
                    </td>
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
