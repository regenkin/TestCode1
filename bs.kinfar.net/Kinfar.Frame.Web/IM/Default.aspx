<%@ Page Language="C#" AutoEventWireup="true" Inherits="Desktop" %>

<%@ Register src="Script/MainScript.ascx" tagname="MainScript" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IM</title>
    
    <uc1:MainScript ID="MainScript1" runat="server" />
    
	<script language="javascript" type="text/javascript">
	window.onload = function()
	{
		StartService();
	}
	</script>
</head>
<body>
	<form id="form1" runat="server">
	<div style="height:1000px;">
	
	</div>
	</form>
</body>
</html>
