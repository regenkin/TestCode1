<%@ Page Language="C#" AutoEventWireup="true" Inherits="ChatForm" %>

<%@ Register src="Script/SubScript.ascx" tagname="SubScript" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
	<uc1:SubScript ID="SubScript1" runat="server" />
	
	<script language="javascript" type="text/javascript">
	
	var Params = (function()
	{
		var pairs = window.location.search.substr(1, window.location.search.length - 1).split("&");
		var params = {};
		for (var i in pairs)
		{
			var vs = pairs[i].split("=");
			params[vs[0]] = vs[1];
		}
		return params;
	})();
	
	var Controls = null;
	var Mangement = null;
	var Common = null;
	var ChatPanel = null;
	
	function AddMessage(message)
	{
		if(ChatPanel != null) ChatPanel.AddMessage(message);
	}
	
	function init()
	{
		CurrentWindow.Waiting("");
		IMCore.InitUI(
			function()
			{
				IMCore.LoadModules(
					function()
				    {
				        CurrentWindow.Completed();
				        Controls = IMCore.GetModule("Controls.js");
				        WebIM = IMCore.GetModule("WebIM.js");

				        var data = IMCore.Utility.ParseJson(document.getElementById("data").value);

				        var config = {
				            Left: 0, Top: 0, Width: Desktop.GetClientWidth(), Height: Desktop.GetClientHeight(),
				            Parent: Desktop,
				            AnchorStyle: Controls.AnchorStyle.All,
				            Peer: data.Peer,
				            User: data.User
				        }

				        ChatPanel = data.Peer.Type == 0 ? new WebIM.UserChatForm(config) : new WebIM.GroupChatForm(config);
				        CurrentWindow.SetTitle(String.format("{0}({1})", data.Peer.Nickname, data.Peer.Name));
				        CurrentWindow.GetTag().OnFormCreated.Call(ChatPanel);
					},
					function(ex)
					{
						IMCore.Utility.ShowError(ex.toString());
					},
					["WebIM.js" ,"Common.js"]
				);
			}
		);
	}
    </script>
</head>
<body>
	<input id="data" name="data" runat="server" type="hidden" />
</body>
</html>
