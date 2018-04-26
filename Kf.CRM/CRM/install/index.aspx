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
    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>  
    <script type="text/javascript">
          $(function () {
           
            $("#btn_next").click(function () {
                window.location.href = "step2.aspx";
            });             
        })
    </script>
    <style type="text/css">
        img { border: none; }
        .text { border: #d2e2f2 1px solid; height: 19px; }
        body { BACKGROUND: url(../images/login/loginbackground1.jpg) repeat-x; font-size: 12px; }
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
                        <h2>欢迎使用KFCRM</h2>

                        <table class="auto-style1">
                            <tr>
                                <td style="border-bottom: 1px solid #d2e2f2; width: 400px;">
                                    <p class="l-log-content">
                                        作者: kinfar
                                    </p>
                                    <p class="l-log-content">
                                        QQ: 313321669
                                    </p>
                                    <p class="l-log-content">
                                        交流QQ群1群: <a href="" target="_blank">191647516</a>
                                    </p>
                                    <p class="l-log-content">
                                        官方网站：<a href="http://www.kinfar.com/)" target="_blank">http://www.kinfar.net</a>
                                    </p>
                                    <p class="l-log-content">
                                        演示地址：<a href="http://crm.kinfar.com/" target="_blank">http://crm.kinfar.com</a>
                                    </p>
                                    <p class="l-log-content">
                                        官方论坛：<a href="http://bbs.ligerui.com/" target="_blank">http://bbs.kinfar.com</a>
                                    </p>
                                </td>
                                <td style="border-bottom: 1px solid #d2e2f2;">
                                    <img src="../images/logo/logo.png" width="234" alt="Kf crm" /></td>
                            </tr>
                        </table>
                        <p>
                            <span style="WHITE-SPACE: normal; TEXT-TRANSFORM: none; WORD-SPACING: 0px; FLOAT: none; COLOR: rgb(0,0,0); FONT: 12px/25px 宋体,SimSun; DISPLAY: inline !important; LETTER-SPACING: normal; TEXT-INDENT: 0px; font-size-adjust: none; font-stretch: normal; -webkit-text-stroke-width: 0px">&nbsp; KFCRM是一款开源免费的客户关系管理系统，是为帮助企业快速成长发展而开发的一款优秀的CRM客户关系管理系统，能帮助您管理客户与销售，能协同进行工作，并能方便的进行二次开发与扩展，是您企业信息化进程最佳的选择。</span>
                        </p>
                        <p>
                            &nbsp;&nbsp; KFCRM能在手机和平板等移动设备上使用，对于业务员来说，非常方便。&nbsp; &nbsp;
                        </p>
                        <p>
                            &nbsp;&nbsp; 如需进行<strong>二次开发</strong>，请联系我们。比如<span class="auto-style2"><strong>审批工作流，OA，进销存</strong></span>等功能。&nbsp;
                        </p>
                    </td>
                </tr>
                <tr>
                    <td height="145" style="text-align: center;">
                        <input type="button" value="同意" style="width: 80px; height: 25px;" id="btn_next" />
                    </td>
                </tr>
            </table>
        </div>
    </form>

</body>

</html>
