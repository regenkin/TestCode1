<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../../CSS/input.css" rel="stylesheet" />
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />

    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>
    <script src="../../JS/Toolbar.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            $("form").ligerForm();
            
    
            loadForm(getparastr("fid"));
         

            remind();
            $("#remind").click(function () {
                remind();
            });
        })
        function remind()
        {
            if ($("#remind").attr("checked") == true || $(this).attr("checked") == "checked") {
                $("#tr1").show();
                $("#tr2").show();
                $("#tr3").show();
                $("#tr4").show();

                $("#T_starttime").rules("add", { required: true });
                $("#T_endtime").rules("add", { required: true });
                $("#T_content").rules("add", { required: true });
            }
            else {
                $("#tr1").hide();
                $("#tr2").hide();
                $("#tr3").hide();
                $("#tr4").hide();

                $("#T_starttime").rules("add", { required: false });
                $("#T_endtime").rules("add", { required: false });
                $("#T_content").rules("add", { required: false });
            }
            //$(this).parents("tr").next("tr").toggle();           
        }
        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&fid=" + getparastr("fid") + "&cid=" + getparastr("cid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../../data/CRM_Follow.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', fid: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_follow").val(obj.Follow);
                    $("#T_followtype").ligerComboBox({ width: 196, initValue: obj.Follow_Type_id, url: "../../data/Param_SysParam.ashx?Action=combo&parentid=4&rnd=" + Math.random() });
                }
            });

        }
    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <table class="bodytable1" style="width: 500px; margin: 2px;">
            <tr>
                <td class="table_title1" colspan="2">跟进</td>
            </tr>
            <tr>

                <td style="width: 85px">
                    <div style="width: 80px; text-align: right; float: right">跟进方式：</div>
                </td>
                <td>
                    <input type="text" id="T_followtype" name="T_followtype" validate="{required:true}" /></td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">跟进内容：</div>
                </td>
                <td>
                    <textarea id="T_follow" cols="6" name="T_follow" class="l-textarea" style="width: 350px;" rows="6" validate="{required:true}"></textarea></td>

            </tr>
            <tr>
                <td class="table_title1" colspan="2" id="remindtr">
                    <div style="width:40px;float:left">提醒</div>
                    <div style="width:40px;float:left">
                        <input type="checkbox" id="remind"/>
                    </div>
                    </td>
            </tr>
            <tr id="tr1">

                <td style="width: 85px">
                    <div style="width: 80px; text-align: right; float: right">注：</div>
                </td>
                <td>提醒内容将出现在日程里面</td>
            </tr>
            <tr id="tr2">

                <td style="width: 85px">
                    <div style="width: 80px; text-align: right; float: right">全天提醒：</div>
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <input type="radio" value="True" name="allday" /></td>
                            <td>是 </td>
                            <td>
                                <input type="radio" value="False" name="allday" checked="checked" /></td>
                            <td>否 </td>

                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="tr3">

                <td style="width: 85px">
                    <div style="width: 80px; text-align: right; float: right">提醒时间：</div>
                </td>
                <td>
                    <div style="float: left; width: 175px;">
                        <input type="text" id="T_starttime" name="T_starttime" ltype="date" ligerui="{width:172,showTime:true}" /></div>
                    <div style="float: left; width: 175px;">
                        <input type="text" id="T_endtime" name="T_endtime" ltype="date" ligerui="{width:176,showTime:true}" /></div>
                </td>
            </tr>
            <tr id="tr4">
                <td>
                    <div style="width: 80px; text-align: right; float: right">提醒内容：</div>
                </td>
                <td>
                    <textarea id="T_content" cols="5" name="T_content" class="l-textarea" style="width: 350px;" rows="3"></textarea></td>

            </tr>
        </table>



    </form>
</body>
</html>
