<%@ Control Language="C#" AutoEventWireup="true" Inherits="CommandCtrl" %>

<script type="text/javascript" language="javascript">
    if(window.IMCore == undefined)
{
    document.writeln('<script src="<%= Kinfar.Frame.IMCore.ServerImpl.Instance.ServiceUrl%>/<%= Kinfar.Frame.IMCore.ServerImpl.Instance.ResPath%>/Core/Common.js" type="text/javascript"><'+'/script>');
}
</script>

<script type="text/javascript" language="javascript">
	function DoCommand(command, data)
	{
		document.getElementById("<%= ClientID %>_command").value = command;
		document.getElementById("<%= ClientID %>_data").value = IMCore.Utility.RenderJson(data);
		document.getElementById("form1").submit();
	}

	function GetState()
	{
		if( <%= StateVarName %> == null)
		{
			<%= StateVarName %> = IMCore.Utility.ParseJson(document.getElementById("<%= ClientID %>_state_json").value);
		}
		return <%= StateVarName %>;
	}

	function ResetState(state)
	{
		<%= StateVarName %> = state;
		document.getElementById("<%= ClientID %>_state_json").value = IMCore.Utility.RenderJson(state);
	}
	
	var <%= StateVarName %> = null;
</script>

<input type="hidden" name="<%= ClientID %>_state_json" id="<%= ClientID %>_state_json" value="<%= StateJson %>" />
<input type="hidden" name="<%= ClientID %>_data" id="<%= ClientID %>_data" />
<input type="hidden" name="<%= ClientID %>_command" id="<%= ClientID %>_command" />

