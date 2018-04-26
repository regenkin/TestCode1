<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
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

            if (getparastr("depid")) {
                loadForm(getparastr("depid"));
            }
            else {
                $("#T_parent").ligerComboBox({
                    width: 150,
                    selectBoxWidth: 150,
                    selectBoxHeight: 150,
                    valueField: 'id',
                    textField: 'text',
                    treeLeafOnly: false,
                    tree: {
                        onBeforeSelect: function (node) {
                            //alert(JSON.stringify(node.data));
                            if (node.data['d_icon'] == '../images/icon/88.png') {
                                $("#T_deptype").ligerGetComboBoxManager().setReadOnly();
                                $("#T_deptype").ligerGetComboBoxManager().selectValue("部门");
                            }
                            else if (node.data['d_icon'] == '../images/icon/61.png')
                            {
                                $("#T_deptype").ligerGetComboBoxManager().setUnReadOnly();
                            }
                            else {
                                $("#T_deptype").ligerGetComboBoxManager().setReadOnly();
                                $("#T_deptype").ligerGetComboBoxManager().selectValue("公司");
                            }
                        },
                        url: '../data/hr_department.ashx?Action=deptree&rnd=' + Math.random(),
                        usericon: 'd_icon',
                        idFieldName: 'id',
                        //parentIDFieldName: 'pid',
                        checkbox: false
                    }
                })
            }


        });

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&id=" + getparastr("depid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }

        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../data/hr_department.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', id: oaid, rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == null || obj[n] == "null")
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String 构造函数
                    $("#T_depname").val(obj.d_name);
                    $("#T_sort").val(obj.d_order);
                    $("#T_leader").val(obj.d_fuzeren);
                    $("#T_tel").val(obj.d_tel);
                    $("#T_email").val(obj.d_email);
                    $("#T_fax").val(obj.d_fax);
                    $("#T_add").val(obj.d_add);
                    $("#T_descript").val(obj.d_miaoshu);

                    $("#T_deptype").ligerGetComboBoxManager().selectValue(obj.d_type);
                    //$("#T_parent").ligerGetComboBoxManager().selectValue(obj.parentid);
                    $("#T_parent").ligerComboBox({
                        width: 150,
                        selectBoxWidth: 150,
                        selectBoxHeight: 150,
                        valueField: 'id',
                        textField: 'text',
                        treeLeafOnly: false,
                        initValue: obj.parentid,
                        tree: {
                            onSuccess: function () {
                                setTimeout(function () {
                                    var manager = $(".l-tree").ligerGetTreeManager();
                                    manager.remove($("#" + oaid));
                                }, 100);
                            },
                            onBeforeSelect: function (node) {
                                //alert(JSON.stringify(node.data));
                                if (node.data['d_icon'] == '../images/icon/88.png') {
                                    $("#T_deptype").ligerGetComboBoxManager().setReadOnly();
                                    $("#T_deptype").ligerGetComboBoxManager().selectValue("部门");
                                }
                                else if (node.data['d_icon'] == '../images/icon/61.png') {
                                    $("#T_deptype").ligerGetComboBoxManager().setUnReadOnly();
                                }
                                else {
                                    $("#T_deptype").ligerGetComboBoxManager().setReadOnly();
                                    $("#T_deptype").ligerGetComboBoxManager().selectValue("公司");
                                }

                            },
                            url: '../data/hr_department.ashx?Action=deptree&companyid=0&rnd=' + Math.random(),
                            usericon: 'd_icon',
                            idFieldName: 'id',
                            //parentIDFieldName: 'pid',
                            checkbox: false
                        }
                    })
                    if(obj.parentid==0)
                        $("#T_deptype").ligerGetComboBoxManager().setReadOnly();
                }
            });
        }

    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">

        <table align="left" border="0" cellpadding="3" cellspacing="1" style="width: 462px;">

            <tr>
                <td>
                    <div align="left" style="width: 60px">机构名称：</div>
                </td>
                <td>
                    <input type='text' id="T_depname" name="T_depname" ltype="text" ligerui="{width:150}" validate="{required:true}" /></td>
                <td>
                    <div align="left" style="width: 60px">上级机构：</div>
                </td>
                <td>
                    <input type="text" id="T_parent" name="T_parent" />
                </td>
            </tr>
            <tr>
                <td>
                    <div align="left" style="width: 60px">机构类型：</div>
                </td>
                <td>
                    <input type="text" id="T_deptype" name="T_deptype" ltype="select" ligerui="{width:150,data:[{id:'部门',text:'部门'},{id:'公司',text:'公司'}]}" /></td>
                <td>
                    <div align="left" style="width: 60px">部门排序：</div>
                </td>
                <td>
                    <input type='text' id="T_sort" name="T_sort" ltype='spinner' ligerui="{type:'int',width:150}" value="20" /></td>
            </tr>
            <tr>
                <td>
                    <div align="left" style="width: 60px">负责人：</div>
                </td>
                <td>
                    <input type='text' id="T_leader" name="T_leader" ltype="text" ligerui="{width:150}" /></td>
                <td>
                    <div align="left" style="width: 60px">电话：</div>
                </td>
                <td>
                    <input type='text' id="T_tel" name="T_tel" ltype="text" ligerui="{width:150}" /></td>
            </tr>
            <tr>
                <td>
                    <div align="left" style="width: 60px">邮箱：</div>
                </td>
                <td>
                    <input type='text' id="T_email" name="T_email" ltype="text" ligerui="{width:150}" validate="{required:false,email:true}" /></td>
                <td>
                    <div align="left" style="width: 70px">传真：</div>
                </td>
                <td>
                    <input type='text' id="T_fax" name="T_fax" ltype="text" ligerui="{width:150}" /></td>
            </tr>
            <tr>
                <td>
                    <div align="left" style="width: 54px">地址：</div>
                </td>
                <td colspan="3">
                    <input type='text' id="T_add" name="T_add" ltype="text" ligerui="{width:390}" /></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <div align="left" style="width: 54px">描述：</div>
                </td>
                <td colspan="3">
                    <input type="text" id="T_descript" name="T_descript" ltype="text" ligerui="{width:390}" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
