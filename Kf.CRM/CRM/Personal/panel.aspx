<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title> 
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" /> 
    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>   
    <script src="../../lib/ligerUI/js/plugins/ligerPanel.js" type="text/javascript"></script>
    <script type="text/javascript">
        var manager;
        $(function ()
        {
            $("#panel1-1").ligerPanel({
                title : '����'
            });
            $("#panel1-2").ligerPanel({
                title: '����'
            });
            $("#panel2").ligerPanel({
                title: '��',
                width: 700,
                height : 500,
                url : '../form/form3.htm'
            });
        }); 
    </script>

</head>
<body style="padding:10px">
        <div style="width:100%;">
            <div id="panel1-1" style="float:left">
            ����������111��������
            </div>
             <div id="panel1-2" style="float:left; margin-left:10px;">
            ����������222��������
        </div>
            <div class="l-clear"></div>
        </div>
        <div id="panel2" style=" margin-top:10px; clear:both;"> 
        </div>
</body>
</html>
