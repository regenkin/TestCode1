<%@ Page Language="C#" AutoEventWireup="true" Inherits="Upload" %>

<%@ Register src="Script/SubScript.ascx" tagname="SubScript" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    
	<uc1:SubScript ID="SubScript1" runat="server" />
	
    <style type="text/css">
    	html
    	{
    		padding:0px;
    		margin:0px;
			overflow: hidden;
			border:0px;
    	}
    	body
    	{
    		padding:6px;
    		margin:0px;
			background-color:#EFF0F2;
			overflow: hidden;
			font-family:宋体;
			font-size:12px;
			border:0px;
    	}
    	#UploadFile
    	{
    		width:100%;
    	}
    	#BtnOk
    	{
			font-family: 宋体;
			font-size: 12px;
			height: 24px;
    	}
    </style>
    
    
	<script language="javascript" type="text/javascript">
	function init()
	{
		var data_str = document.getElementById("data").value;
		if(data_str != "")
		{
			var data = IMCore.Utility.ParseJson(data_str);
			if(data.Result) CurrentWindow.GetTag().AfterUpload(data.Path);
			else IMCore.Utility.ShowError(data.Exception.toString());
		}
	}
	function BtnOk_onclick()
	{
	    if(document.getElementById("UploadFile").value=="")
	    {
	        IMCore.Utility.ShowWarning("请选择要上传的文件！");
	        return false;
	    }
	    CurrentWindow.Waiting("正在上传文件...");
	}

    </script>
</head>
<body>
    <form id="form1" runat="server">
	<div><asp:FileUpload ID="UploadFile" runat="server" /></div>
	<div style="text-align:right; margin-top:12px;"><input id="BtnOk" name="BtnOk" type="submit" value="确 定" onclick="return BtnOk_onclick()" /></div>
	<input runat="server" type="hidden" id="data" name="data" />
    </form>
</body>
</html>
