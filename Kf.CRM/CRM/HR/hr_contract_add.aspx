<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../CSS/input.css" rel="stylesheet" />
    <link href="../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />    

    <script src="../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>

    <script src="../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>

    <script src="../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../lib/jquery.form.js" type="text/javascript"></script>

    <script src="../lib/json2.js" type="text/javascript"></script>

    <script src="../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            $("#T_emp").ligerComboBox({
                width: 182,
                onBeforeOpen: f_selectContact
            })
            initcombo();
            if (getparastr("cid")) {
                loadForm(getparastr("cid"));
            }
        });
        
        function initcombo() {
            a = $('#T_post').ligerComboBox({  width: 182 });
            b = $('#T_dep').ligerComboBox({
                width: 182,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                //readonly: true,
                tree: {
                    url: '../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    parentIDFieldName: 'pid',
                    checkbox: false
                },
                onSelected: function (newvalue, newtext, newid) {
                    $.get("../data/hr_post.ashx?Action=combo&postid=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                        a.setData(eval(data));
                        a.selectValue(newid);
                    });
                }
            });
            c = $('#T_position').ligerComboBox({ width: 182,url:'../data/hr_position.ashx?Action=combo&rnd='+Math.random() });
        }
        function f_selectContact() {
            $.ligerDialog.open({
                title: '选择客户', width: 600, height: 350, url: 'Getemp.aspx', buttons: [
                    { text: '确定', onclick: f_selectContactOK },
                    { text: '取消', onclick: f_selectContactCancel }
                ]
            });
            return false;
        }
        function f_selectContactOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('请选择行!');
                return;
            }
            $("#T_emp").val(data.name);
            $("#T_emp_val").val(data.ID);
            $("#T_dep").ligerGetComboBoxManager().selectValue(data.d_id, data.postid);
            $('#T_position').ligerGetComboBoxManager().selectValue(data.zhiwuid)
            dialog.close();
        }
        function f_selectContactCancel(item, dialog) {
            dialog.close();
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../data/C_hr_contract.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', cid: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_contract_num").val(obj.Serialnumber);
                    $("#T_content").val(obj.Contract_content);
                    $("#T_remarks").val(obj.Remarks);

                    $("#T_start_date").val(obj.start_date.split(' ')[0]);
                    $("#T_end_date").val(obj.end_date.split(' ')[0]);
                     
                    $("#T_emp").val(obj.emp_name);
                    $("#T_emp_val").val(obj.emp_id);

                    $('#T_position').ligerGetComboBoxManager().selectValue(obj.position_id);
                    $("#T_dep").ligerGetComboBoxManager().selectValue(obj.dep_id, obj.post_id);
                }
            });
        }

        function remote1() {
            var url = "../data/hr_employee.ashx?Action=Exist&rnd=" + Math.random();
            return url;
        }

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&cid=" + getparastr("cid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }        
    </script>

</head>
<body style="margin: 0">
    <form id="form1" onsubmit="return false">
        <div>
            <table style="width: 620px; margin: 5px;" class='bodytable1'>
                <tr>
                    <td colspan="4" class="table_title1">合同基本信息</td>
                </tr>
                <tr>
                    <td>
                        <table width="560px">
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        合同编号：
                                    </div>
                                </td>
                                <td>
                                   <input type="text" id="T_contract_num" name="T_contract_num" ltype="text" ligerui="{width:182}" validate="{required:true}" /></td>
                                <td width="62px">
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        员工姓名：
                                    </div>
                                </td>
                                <td>
                                    <input type="text" id="T_emp" name="T_emp"  validate="{required:true}" /></td>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        职务：
                                    </div>
                                </td>
                                <td>
                                   <input type="text" id="T_position" name="T_position"  validate="{required:true}" /></td>
                            </tr>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        部门： </div>
                                </td>
                                <td>
                                    <input type="text" id="T_dep" name="T_dep" validate="{required:true}" /></td>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        岗位： </div>
                                </td>
                                <td>
                                    <input type="text" id="T_post" name="T_post"  validate="{required:true}" /></td>
                            </tr>
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        开始时间：
                                    </div>
                                </td>
                                <td>
                                   <input type="text" id="T_start_date" name="T_start_date" ltype="date" validate="{required:true}" ligerui="{width:182}" /></td>
                                <td width="62px">
                                    <div align="right" style="width: 61px">
                                        结束时间：
                                    </div>
                                </td>
                                <td>
                                    <input type="text" id="T_end_date" name="T_end_date" ltype="date" validate="{required:true}" ligerui="{width:182}" /></td>
                            </tr>
                            </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="table_title1">合同条款</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <table width="560px">
                            <tr>
                                <td width="62px">
                                    <div align="right" style="width: 62px">主要条款：</div>
                                </td>
                                <td>
                                    <textarea cols="100" id="T_content" name="T_content" rows="8" class="l-textarea" style="width: 455px"></textarea></td>
                            </tr>
                            <tr>
                                <td >
                                    <div align="right" style="width: 62px">备注：</div>
                                </td>
                                <td>
                                    <textarea cols="100" id="T_remarks" name="T_remarks" rows="3" class="l-textarea" style="width: 455px"></textarea></td>
                            </tr>
                            </table>
                    </td>
                </tr>
            </table>






        </div>
    </form>

</body>
</html>
