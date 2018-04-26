<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title></title>
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
            $("#T_uid").attr("validate", "{ required: true, username: true, remote: remote, messages: { required: '�������˺���', remote: '���˻�����!' } }");
            $.metadata.setType("attr", "validate");
            KFCRM.validate($(form1));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            $("#T_dep").ligerComboBox({
                width: 180,
                selectBoxWidth: 180,
                selectBoxHeight: 180,
                valueField: 'id',
                textField: 'text',
                treeLeafOnly: false,
                tree: {
                    url: '../data/hr_department.ashx?Action=tree&rnd=' + Math.random(),
                    idFieldName: 'id',
                    checkbox: false
                }
            })


        });


        function remote() {
            var url = "../data/hr_employee.ashx?Action=Exist&emp_id=" + getparastr("empid") + "&rnd=" + Math.random();
            return url;
        }

        function uploadimg() {
            top.$.ligerDialog.open({
                zindex: 9007,
                width: 800, height: 500,
                title: '�ϴ�ͷ��',
                url: 'hr/headimage.aspx',
                buttons: [
                {
                    text: '����', onclick: function (item, dialog) {
                        saveheadimg(item, dialog);
                    }
                },
                {
                    text: '�ر�', onclick: function (item, dialog) {
                        dialog.close();
                    }
                }
                ],
                isResize: true
            });
        }

        function saveheadimg(item, dialog) {
            var upfile = dialog.frame.f_save();
            //alert(upfile);
            if (upfile) {
                dialog.close();
                top.$.ligerDialog.waitting('���ݱ�����,���Ժ�...');

                $.ajax({
                    url: "../data/upload.ashx", type: "POST",
                    data: upfile,
                    success: function (responseText) {
                        $("#headurl").val(responseText);
                        $("#userheadimg").attr("src", "../images/upload/portrait/" + responseText);
                        top.$.ligerDialog.closeWaitting();
                    },
                    error: function () {
                        top.$.ligerDialog.closeWaitting();
                        top.$.ligerDialog.error('����ʧ�ܣ�');
                    }
                });
            }
        }

        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&id=" + getparastr("empid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }


    </script>

</head>
<body style="margin: 0; overflow: hidden">
    <form id="form1" onsubmit="return false">
        <div>
            <table>
                <tr>
                    <td width="62px">
                        <div align="right" style="width: 61px">
                            �˺ţ�
                        </div>
                    </td>
                    <td width="182px" colspan="3">
                        <input type="text" id="T_uid" name="T_uid" ltype="text" ligerui="{width:180}" />
                    </td>
                    <td width="62px">
                        <div align="right" style="width: 61px">
                            Email��
                        </div>
                    </td>
                    <td width="182px">
                        <input type="text" id="T_email" name="T_email" ltype="text" ligerui="{width:180}" validate="{required:false,email:true}" /></td>
                    <td width="132px" rowspan="5">
                        <img id="userheadimg" src="a.gif" onerror="noheadimg()" style="border-radius: 4px; box-shadow: 1px 1px 3px #111; width: 120px; height: 120px; margin-left: 5px; background: #d2d2f2; border: 3px solid #fff; behavior: url(../css/pie.htc);" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 62px">������</div>
                    </td>
                    <td colspan="3">
                        <input type="text" id="T_name" name="T_name" ltype="text" ligerui="{width:180}" validate="{required:true,minlength:1,maxlength:10,messages:{required:'����������',maxlength:'�����������ô���'}}" />
                    </td>
                    <td>
                        <div align="right" style="width: 62px">���գ�</div>
                    </td>
                    <td>
                        <input type="text" id="T_birthday" name="T_birthday" ltype="date" validate="{required:true}" ligerui="{width:180}" /></td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 62px">�Ա�</div>
                    </td>
                    <td width="50px">
                        <input type="text" id="T_sex" name="T_sex" style="width: 50px" ltype="select" ligerui="{width:50,data:[{id:'��',text:'��'},{id:'Ů',text:'Ů'}]}" validate="{required:true}" />
                    </td>
                    <td width="52">
                        <div align="right">״̬��</div>
                    </td>
                    <td width="80px">
                        <input type="text" id="T_status" name="T_status" style="width: 60px" ltype="select" ligerui="{width:65,data:[{id:'��ְ',text:'��ְ'},{id:'��ְ',text:'��ְ'}]}" /></td>
                    <td>
                        <div align="right" style="width: 61px">
                            ���֤��
                        </div>
                    </td>
                    <td>
                        <input type="text" id="T_idcard" name="T_idcard" ltype="text" ligerui="{width:180}" validate="{required:false,IdNumber:true,messages:{required:'���������֤����'}}" /></td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 61px">
                            �ֻ���
                        </div>
                    </td>
                    <td colspan="3">
                        <input type="text" id="T_tel" name="T_tel" ltype="text" ligerui="{width:180}" validate="{required:true,cellphone:true,messages:{required:'�������ֻ���'}}" />
                    </td>
                    <td>
                        <div align="right" style="width: 62px">��ְ���ڣ�</div>
                    </td>
                    <td>
                        <input type="text" id="T_entryDate" name="T_entryDate" ltype="date" ligerui="{width:180}" /></td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 61px">
                            ��ַ��
                        </div>
                    </td>
                    <td colspan="5">
                        <input type="text" id="T_Adress" name="T_Adress" ltype="text" ligerui="{width:446}" /></td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 62px">��ҵԺУ��</div>
                    </td>
                    <td colspan="5">
                        <input type="text" id="T_school" name="T_school" ltype="text" ligerui="{width:446}" /></td>
                    <td>
                        <div style="text-align: center">
                            <input type="button" value="�ϴ�ͷ��" style="width: 80px; height: 22px;" onclick="uploadimg()" />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 61px">
                            ѧ����
                        </div>
                    </td>
                    <td colspan="3">
                        <input type="text" id="T_edu" name="T_edu" ltype="text" ligerui="{width:180}" /></td>
                    <td>
                        <div align="right" style="width: 62px">רҵ��</div>
                    </td>
                    <td>
                        <input type="text" id="T_professional" name="T_professional" ltype="text" ligerui="{width:180}" /></td>
                    <td>
                        <input type="hidden" id="headurl" name="headurl" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div align="right" style="width: 62px">��ע��</div>
                    </td>
                    <td colspan="5">
                        <textarea cols="100" id="T_remarks" name="T_remarks" rows="3" class="l-textarea" style="width: 442px"></textarea></td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <div align="right" style="width: 62px">�ɵ�½��</div>
                                </td>
                                <td>
                                    <input type="radio" value="1" name="canlogin" checked="checked" /></td>
                                <td>�� </td>
                                <td>
                                    <input type="radio" value="0" name="canlogin" /></td>
                                <td>�� </td>

                            </tr>
                        </table>
                    </td>
                </tr>


            </table>
        </div>
    </form>
</body>
</html>
