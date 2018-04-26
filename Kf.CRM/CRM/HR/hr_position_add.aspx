<%@ Page Language="C#" AutoEventWireup="true"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head  >
    <title></title>
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />    
    <link href="../CSS/input.css" rel="stylesheet" type="text/css" />

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>    
    
    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            if (getparastr("pid")) {
                loadForm(getparastr("pid"));
            }


        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&id=" + getparastr("pid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../data/hr_position.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_position").val(obj.position_name);
                    $("#T_order").val(obj.position_order);
                    $("#T_level").val(obj.position_level);
                }
            });
        }
        
    </script>
</head>
<body style="margin:5px 5px 5px 5px">
    <form id="form1" onsubmit="return false">
    <div>
        <table  align="left" border="0" cellpadding="3" cellspacing="1" 
            style="background: #fff; width:320px;">
            
            <tr>
                <td  width="65px"  ><div align="left" style="width: 61px">
                    职务名称：</div></td>
                <td   >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_position" name="T_position" ltype="text"   ligerui="{width:180}" validate="{required:true}" />
                    </div>                    
                </td>
            </tr>
            
            <tr>
                <td  width="65px"  >职务级别：</td>
                <td   >
                        <input type="text" id="T_level" name="T_level"   ltype='spinner' ligerui="{type:'int',width:180}" value="20" validate="{required:true}" /></td>
            </tr>
            <tr>
                <td  >
                
                    <div align="left" style="width: 62px">行号：</div></td>
                <td  >
                    <div style="float:left; height: 20px;">
                        <input type="text" id="T_order" name="T_order"   ltype='spinner' ligerui="{type:'int',width:180}" value="20" validate="{required:true}" />
                    </div>
                </td>
            </tr>
            <tr>
                <td  colspan="2" >
                
                    &nbsp;</td>
            </tr>
            </table>
    </div>
    </form>
</body>
</html>
