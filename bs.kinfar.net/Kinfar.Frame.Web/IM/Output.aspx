﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="System.Web.UI.Page" %>

<%@ Register src="Script/SubScript.ascx" tagname="SubScript" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>输出</title>
    
    <style type="text/css">
    body
    {
    	font-family:Courier New;
    	margin:6px;
    	padding:0px;
    	font-size:12px;
    	line-height:1.5em;
    }
    </style>
    
	<uc1:SubScript ID="SubScript1" runat="server" />
	
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

	function FormatNumber(num,length)
	{
		var s=num.toString();
		var zero="";
		for(var i=0;i<length-s.length;i++) zero+="0";
		return zero+s;
	}
	
	function DateToString(date)
	{
		return String.format(
			"{0}-{1}-{2} {3}:{4}:{5}",
			FormatNumber(date.getFullYear(),4),
			FormatNumber(date.getMonth()+1,2),
			FormatNumber(date.getDate(),2),
			FormatNumber(date.getHours(),2),
			FormatNumber(date.getMinutes(),2),
			FormatNumber(date.getSeconds(),2)
		);
	}
	
	var LogCount = 0;
	
	function Write(text)
	{
	    if(LogCount > 200)
	    {
	        document.body.removeChild(document.body.firstChild);
	        LogCount--;
	    }
		var dom = document.createElement("DIV");
		dom.style.whiteSpace = "nowrap";
		dom.innerHTML = String.format("[{0}] {1}", DateToString(new Date()), text);
		document.body.appendChild(dom);
		document.documentElement.scrollTop = document.documentElement.scrollHeight;
		LogCount++;
	}

	
	function init()
	{
		
	}
	</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
    </form>
</body>
</html>
