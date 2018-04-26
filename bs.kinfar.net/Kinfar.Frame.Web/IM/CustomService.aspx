<%@ Page Language="C#" AutoEventWireup="true"Inherits="Kinfar.Frame.IMWeb.Lesktop_CustomService" %>

<%@ Register src="Script/ServiceScript.ascx" tagname="ServiceScript" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>客服平台</title>
    
    <uc1:ServiceScript ID="Script1" runat="server" />
    
	<script language="javascript" type="text/javascript">

	window.onload = function()
	{
		StartService();
	}
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    <input id="data_json" type="hidden" runat="server" />
    </form>
</body>
</html>
