<%@ Page Language="C#" AutoEventWireup="true" Inherits="Lesktop_Management_UpdateSelfInfo" %>

<%@ Register Src="../Script/SubScript.ascx" TagName="SubScript" TagPrefix="uc1" %>
<%@ Register src="../CommandCtrl.ascx" tagname="CommandCtrl" tagprefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <uc1:SubScript ID="SubScript1" runat="server" />

    <script language="javascript" type="text/javascript">

        var Controls = null;
        var Mangement = null;
        var Common = null;

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
					        Management = IMCore.GetModule("Management.js");

					        var config = {
					            Left: 0, Top: 0, Width: Desktop.GetClientWidth(), Height: Desktop.GetClientHeight(),
					            Parent: Desktop,
					            AnchorStyle: Controls.AnchorStyle.All
					        }

					        new Management.UpdateSelfInfo(config);
					        
                            if(GetState().Action == "ResetUserInfo")
                            {
                                IMCore.Session.ResetUserInfo(GetState().SelfInfo);
                            }
                            else if(GetState().Action == "Alert")
                            {
                                IMCore.Utility.ShowError(GetState().Message);
                            }
					    },
					    function(ex)
					    {
					        IMCore.Utility.ShowError(ex.toString());
					    },
					    ["Management.js"]
				    );
			    }
		    );
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" enctype="multipart/form-data">
    <div id="__lesktop_container">
        <uc2:CommandCtrl ID="CommandCtrl1" runat="server" />
    </div>
    </form>
</body>
</html>
