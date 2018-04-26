<%@ Page Language="C#" AutoEventWireup="true" Inherits="Login" %>

<%@ Register src="Script/SubScript.ascx" tagname="SubScript" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title>登录</title>
    
	<uc1:SubScript ID="SubScript1" runat="server" />
	
	<style type="text/css">

        html
        {
	        overflow: hidden;
	        border:0px;
        }
        
		body
		{
			padding: 0px;
			margin: 0px;
	        border:0px;
		}
		.div_bk
		{
			background: url("images/login_bk.png" ) repeat-x;
			width: 580px;
			height: 380px;
			padding: 10px 10px 10px 10px;
		}
		.div_logo
		{
			background: url("images/login_logo.png" ) no-repeat;
			width: 140px;
			height: 60px;
		}
		#txtUser, #txtuser_reg,#txtemail_reg,#txtnick_reg
		{
			position: absolute;
			font-size: 12px;
			padding: 5px;
			font-family: Courier New;
			border: solid 1px #7F9DB9;
		}
		#txtpwd, #txtpwd_reg, #txtpwd_confirm_reg
		{
			position: absolute;
			font-size: 14px;
			padding: 5px;
			font-family: 宋体;
			border: solid 1px #7F9DB9;
		}
		label
		{
			font-size: 12px;
			font-family: 宋体;
		}
		#login, #register
		{
			font-family: 宋体;
			font-size: 12px;
			height: 12px;
			padding: 4px 8px 4px 8px;
		}
		.link
		{
			font-family: 宋体;
			font-size:12px;
			position: absolute;
			color:#229ACD;
			cursor:pointer;
			text-align:right;
			text-decoration:none;
		}
		.link:hover
		{
			text-decoration:underline;
		}
	</style>

	<script language="javascript" type="text/javascript">
	
	document.onkeydown=function()
	{
		if(event.keyCode==116 || (event.ctrlKey && event.keyCode==82))
		{
			event.keyCode=0; 
			event.returnValue=false;
			return false;
		}
		if(event.keyCode == 70 && event.ctrlKey && !event.altKey && !event.shiftKey)
		{
			event.keyCode=0; 
			event.returnValue=false;
			return false;
		}
		
	}
	</script>

	<script language="javascript" type="text/javascript">
	function init()
	{
		document.getElementById("download_link").style.display = ClientMode ? "none" : "block";
		
		try
		{
			document.getElementById("txtUser").focus();
		}
		catch(ex)
		{
		}
		
		if(IMCore.Params["name"]!=undefined)
		{
		    document.getElementById("txtUser").readOnly = true;
		    document.getElementById("txtUser").value = IMCore.Params["name"];
			document.getElementById("txtpwd").focus();
		}
		
		var s = document.getElementById("status").value;
		if(s=="login")
		{			
			CurrentWindow.Completed();
			IMCore.OutputPanel.MoveEx("",10000,10000,true);
			IMCore.OutputPanel.Show();
		
			IMCore.OutputPanel.Load(
				IMCore.GetPageUrl("Output.aspx"),
				function()
				{	
					IMCore.OutputPanel.Hide();
					
					if(!ClientMode)
					{
					    //IMCore.OutputPanel.MoveEx("",0,0,true);
					    //IMCore.OutputPanel.Show();
					}
			
					var login_data = IMCore.Utility.ParseJson(document.getElementById("data").value);
					if (login_data != undefined && login_data != null && login_data.UserInfo != null) {
					    IMCore.Session.InitService(
                            login_data.UserInfo.Name,
                            login_data.UserInfo,
                            document.cookie,
                            document.getElementById("sessionId").value
                        );
					    IMCore.Taskbar.Show();
					    IMCore.Session.GetGlobal("SingletonForm").ShowFriendForm();
					    CurrentWindow.Close();
					}
				}
			);
		}
		else if(s=="error")
		{
			CurrentWindow.Completed();
			IMCore.Utility.ShowError(document.getElementById("data").value);
		}
	}
	
	function login_onclick() 
	{
		var s = document.getElementById("txtUser").value;
		if(s=="") 
		{
			IMCore.Utility.ShowWarning("请输入用户名!");
			return false;
		}
		return true;
	}

	function ShowLoginPage()
	{
		document.getElementById("div_reg").style.display = "none";
		document.getElementById("div_login").style.display = "block";
	}
	
	function ShowRegPage()
	{
		document.getElementById("div_reg").style.display = "block";
		document.getElementById("div_login").style.display = "none";
	}

	function match(reg,str)
	{
		reg.lastIndex=0;
		var ft = reg.exec(str);
		return (ft!=null && ft.length==1 && ft[0]==str) 
	}
	
	function register_onclick()
	{
	    var nameReg=/[a-zA-Z0-9_]+/ig;
	    var pwdReg=/[a-zA-Z0-9]+/ig;
	    
		if(document.getElementById("txtuser_reg").value == "") 
		{
			IMCore.Utility.ShowWarning("请输入用户名!");
			return false;
		}
	    
	    if(!match(nameReg,document.getElementById("txtuser_reg").value))
	    {
	        IMCore.Utility.ShowWarning("用户名格式不正确（用户名只能包含英文字符，数字和下划线）！");
	        return false;
	    }
		
	    if(document.getElementById("txtuser_reg").value.length<4)
	    {
	        IMCore.Utility.ShowWarning("用户名必须4个字符以上！");
	        return false;
	    }
	    
		if(document.getElementById("txtnick_reg").value == "") 
		{
			IMCore.Utility.ShowWarning("请输入昵称!");
			return false;
		}
		
		if(document.getElementById("txtpwd_reg").value == "") 
		{
			IMCore.Utility.ShowWarning("请输入密码!");
			return false;
		}
		
	    if(!match(pwdReg,document.getElementById("txtpwd_reg").value))
	    {
	        IMCore.Utility.ShowWarning("密码格式不正确（密码只能包含英文字符和数字）！");
	        return false;
	    }
	    
	    if(document.getElementById("txtpwd_reg").value.length<6)
	    {
	        IMCore.Utility.ShowWarning("密码必须6个字符以上！");
	        return false;
	    }
		
		if(document.getElementById("txtpwd_confirm_reg").value != document.getElementById("txtpwd_reg").value) 
		{
			IMCore.Utility.ShowWarning("两次输入密码不一致!");
			return false;
		}
		
		if(document.getElementById("txtemail_reg").value == "") 
		{
			IMCore.Utility.ShowWarning("请输入电子邮箱!");
			return false;
		}
		
		return true;
	}
	function form1_onsubmit()
	{
		CurrentWindow.Waiting("");
		return true;
	}
	</script>

</head>
<body>
	<form id="form1" runat="server" onsubmit="return form1_onsubmit();">
	<div id="div_login" class="div_bk">
		<div class="div_logo">
		</div>
		<label for="txtUser" style="position: absolute; top: 146px; left: 171px; text-align: right;">
			用户名：
		</label>
		<input id="txtUser" name="txtuser" runat="server" type="text" style="top: 140px; left: 229px; width: 258px;
			height: 14px;" />
		<label for="txtpwd" style="position: absolute; top: 186px; left: 160px; text-align: right;">
			登录密码：
		</label>
		<input id="txtpwd" name="txtpwd" runat="server" type="password" style="top: 180px; left: 229px;
			width: 258px; height: 14px;"/>
		
		<a class="link" id="download_link" href="client-1.1.0.11.zip" style="top: 240px; left: 355px; width: 65px; height: 14px;">下载桌面版</a>
		
		<a class="link" href="javascript:ShowRegPage();" style="top: 240px; left: 425px; width: 65px; height: 14px;">注册新账户</a>
		
		<input id="login" name="login" type="submit" value="登 录" style="position: absolute;
			top: 367px; left: 521px; height: 26px; width: 67px;" onclick="return login_onclick()" />
	</div>
	<div id="div_reg" class="div_bk" style="display: none">
		<div class="div_logo">
		</div>
		
		<label for="txtuser_reg" style="position: absolute; top: 106px; left: 171px; text-align: right;">
			用户名：
		</label>
		<input id="txtuser_reg" name = "txtuser_reg" runat="server" type="text"  style="top: 100px; left: 229px; width: 258px;
			height: 14px;" />
		
		<label for="txtnick_reg" style="position: absolute; top: 146px; left: 171px; text-align: right;">
			昵&nbsp;&nbsp;称：
		</label>
		<input id="txtnick_reg" name = "txtnick_reg" runat="server" type="text"  style="top: 140px; left: 229px; width: 258px;
			height: 14px;" />
		
		<label for="txtpwd_reg" style="position: absolute; top: 186px; left: 160px; text-align: right;">
			登录密码：
		</label>
		<input id="txtpwd_reg" name = "txtpwd_reg" runat="server" type="password" style="top: 180px; left: 229px;
			width: 258px; height: 14px;" />
		
		<label for="txtpwd_confirm_reg" style="position: absolute; top: 226px; left: 160px; text-align: right;">
			密码确认：
		</label>
		<input id="txtpwd_confirm_reg" name="txtpwd_confirm_reg" runat="server" type="password" style="top: 220px; left: 229px;
			width: 258px; height: 14px;" />
		
		<label for="txtemail_reg" style="position: absolute; top: 266px; left: 160px; text-align: right;">
			电子邮件：
		</label>
		<input id="txtemail_reg" name="txtemail_reg" runat="server" type="text"  style="top: 260px; left: 229px; width: 258px;
			height: 14px;" />
			
		<a class="link" href="javascript:ShowLoginPage();" style="top: 320px; left: 410px; width: 80px; height: 14px;">返回登录页面</a>
		
		<input id="register" name="register" type="submit" value="注 册" style="position: absolute;
			top: 367px; left: 521px; height: 26px; width: 67px;" onclick="return register_onclick()" />
	</div>
	<input id="status" name="status" runat="server" type="hidden" value="none" />
	<input id="data" name="data" runat="server" type="hidden" value="" />
	<input id="sessionId" name="sessionId" runat="server" type="hidden" value="" />
	</form>
</body>
</html>
