<%@ Page Language="C#" AutoEventWireup="true" Inherits="FloatForm" %>

<%@ Register src="Script/SubScript.ascx" tagname="SubScript" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    
	<uc1:SubScript ID="SubScript1" runat="server" />
	
	<style type="text/css">
	#message
	{
		left:0px;
		top:0px;
		position:absolute;
		width:250px;
		padding:6px;
		line-height:1.5em;
	}
	a
	{
		color:Blue;
		cursor:pointer;
		text-decoration:none;
		margin-left:6px;
		margin-right:6px;
	}
	a:hover
	{
		text-decoration:underline;
		
	}
	.afr_info
	{
		padding: 4px; 
		color:#666666;
	}
	</style>
	
	<script language="javascript" type="text/javascript">
	
	function init()
	{
	}
	
	function Login()
	{
	    IMCore.Login(false);
	    CurrentWindow.Close();
	}
	
	function Restart()
	{
	    if(window.external != undefined && window.external.RestartApplication != undefined) window.external.RestartApplication();
	}
	
	function RefreshPage()
	{
	    top.window.navigate(top.window.location.toString());
	}
	
	function AddFriend()
	{
		IMCore.LoadModules(
			function()
			{
				IMCore.GetModule("Common.js").AddFriend(
					function(result, ex)
					{
						if(!result) IMCore.Utility.ShowError(ex);
						CurrentWindow.Close();
					},
					Data.Peer.Name
				);
			},
			function(ex)
			{
				IMCore.Utility.ShowError(ex);
			},
			["Common.js"]
		);
	}
	
	function AddToGroup()
	{
		IMCore.LoadModules(
			function()
			{
				IMCore.GetModule("Common.js").AddToGroup(
					function(result, ex)
					{
						if(!result) IMCore.Utility.ShowError(ex);
						CurrentWindow.Close();
					},
					Data.User.Name,
					Data.Group.Name
				);
			},
			function(ex)
			{
				IMCore.Utility.ShowError(ex);
			},
			["Common.js"]
		);
	}
	
	function Ingore()
	{
		CurrentWindow.Close();
	}
	
	var Data = null;
	
	function CreateMessageText(json)
	{
		Data = IMCore.Utility.ParseJson(json);
		if(Data.Type == "AddFriendRequest")
		{
			return String.format(
				"<div style='width: 100%;'>" +
					"{0}(\"{1}\") 请求添加您为好友，验证信息:" +
					"<div class='afr_info'>{2}</div>" +
					"<div style='width: 100%; text-align:right;'><a onclick='return AddFriend();'>加为好友</a><a onclick='return Ingore();'>忽略</a></div>" +
				"</div>",
				Data.Peer.Nickname, Data.Peer.Name, IMCore.Utility.ReplaceHtml(Data.Info)
			);
		}
		else if(Data.Type == "AddGroupRequest")
		{
			return String.format(
				"<div style='width: 100%;'>" +
					"{0}(\"{1}\") 请求加入群 {2}(\"{3}\")，验证信息:" +
					"<div class='afr_info'>{4}</div>" +
					"<div style='width: 100%; text-align:right;'><a onclick='return AddToGroup();'>同意加入</a><a onclick='return Ingore();'>忽略</a></div>" +
				"</div>",
				Data.User.Nickname, Data.User.Name, 
				Data.Group.Nickname, Data.Group.Name, 
				IMCore.Utility.ReplaceHtml(Data.Info)
			);
		}
		else if(Data.Type == "DeleteFriendNotify")
		{
			IMCore.Session.GetGlobal("FriendsInfoCache").Refresh();
			if(IMCore.Session.GetUserName().toUpperCase() == Data.User.Name.toUpperCase())
			{
				return String.format(
					"您已将 {0}({1}) 从好友中删除！",
					Data.Peer.Nickname,Data.Peer.Name
				);
			}
			else
			{
				return String.format(
					"{0}({1}) 已将您从好友中删除！",
					Data.User.Nickname,Data.User.Name
				);
			}

		}
		else if (Data.Type == "AddToGroupNotify")
		{
			IMCore.Session.GetGlobal("FriendsInfoCache").Refresh();
			if(IMCore.Session.GetUserInfo().ID == Data.User.ID)
			{
				return String.format(
					"你已经加入群{0}({1})！",
					Data.Group.Nickname, Data.Group.Name
				);
			}
			else
			{
				return String.format(
					"{0}({1}) 已加入群 {2}({3})！",
					Data.User.Nickname, Data.User.Name,
					Data.Group.Nickname, Data.Group.Name
				);
			}
		}
		else if (Data.Type == "ExitGroupNotify")
		{
			IMCore.Session.GetGlobal("FriendsInfoCache").Refresh();
			if(IMCore.Session.GetUserInfo().ID == Data.User.ID)
			{
				return String.format(
					"你已经离开群{0}({1})！",
					Data.Group.Nickname, Data.Group.Name
				);
			}
			else
			{
				return String.format(
					"{0}({1}) 已离开群 {2}({3})！",
					Data.User.Nickname, Data.User.Name,
					Data.Group.Nickname, Data.Group.Name
				);
			}
		}
		else if (Data.Type == "DeletetGroupNotify")
		{
			IMCore.Session.GetGlobal("FriendsInfoCache").Refresh();
			return String.format(
				"群{0}({1})已解散！",
				Data.Group.Nickname, Data.Group.Name
			);
		}
		else if(Data.Type == "AddFriendNotify")
		{
			IMCore.Session.GetGlobal("FriendsInfoCache").Refresh();
			if(IMCore.Session.GetUserName().toUpperCase() == Data.User.Name.toUpperCase())
			{
				return String.format(
					"您已将添加 {0}({1}) 为好友！",
					Data.Peer.Nickname,Data.Peer.Name
				);
			}
			else
			{
				return String.format(
					"{0}({1}) 已将添加您为好友！",
					Data.User.Nickname,Data.User.Name
				);
			}
		}
		else if(Data.Type == "UnauthorizedException")
		{
			return String.format("服务器验证您的身份时发生错误，请<a href='javascript:Login()'>重新登录</a>");
		}
		else if(Data.Type == "IncompatibleException")
		{
		    if(ClientMode) return String.format("服务器已升级，请<a href='javascript:Restart()'>重新启动应用程序</a>！");
		    else return String.format("服务器已升级，请<a href='javascript:RefreshPage()'>刷新页面</a>！");
		}
		else
		{
			return json;
		}
	}
		
	function ShowMessage(json, type)
	{
		if(type == undefined) type = "text";
		
		var msg_div = document.getElementById("message");
		msg_div.innerHTML = type == "text" ? json : CreateMessageText(json);
		
		var height = Math.max(msg_div.offsetHeight, 80);
		
		CurrentWindow.Resize(250 + 6 * 2 + 6 *2 , height + 18 + 6 * 2 + 6 *2);
	}
	</script>
</head>
<body>
    <div id="message"></div>
</body>
</html>
