<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title></title>
    <link href="../../lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="../../CSS/input.css" rel="stylesheet" />
    <meta http-equiv="X-UA-Compatible" content="ie=8 chrome=1" />
    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerForm.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerComboBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerRadio.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerSpinner.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTextBox.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDateEditor.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerCheckBox.js" type="text/javascript"></script>

    <script src="../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>

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

            loadForm(getparastr("cid"));

            $('#T_employee').ligerComboBox({ width: 196, onBeforeOpen: f_selectContact });
            $("#T_company").attr("validate", "{ required: true, remote: remote, messages: { required: '������ͻ���', remote: '�˿ͻ��Ѵ���!' } }");

        })
        function f_save() {
            if ($(form1).valid()) {
                var sendtxt = "&Action=save&id=" + getparastr("cid");
                return $("form :input").fieldSerialize() + sendtxt;
            }
        }
        var a; var b; var c; var d; var e; var f; var g; var h; var i;

        function loadForm(oaid) {
            
            $.ajax({
                type: "GET",
                url: "../../data/crm_customer.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                data: { Action: 'form', cid: oaid, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == "null" || obj[n] == null)
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String ���캯��
                    $("#T_company").val(obj.Customer);
                    $("#T_customername").val(obj.contact);
                    $("#T_address").val(obj.address);
                    $("#T_qq").val(obj.QQ);
                    $("#T_mobil").val(obj.mobil);
                    $("#T_tel").val(obj.tel);
                    $("#T_fax").val(obj.fax);
                    $("#T_Website").val(obj.site);
                    $("#T_email").val(obj.email);
                    $("#T_descript").val(obj.DesCripe);
                    $("#T_remarks").val(obj.Remarks);
                    $("#T_contact_dep").val(obj.contact_dep);
                    $("#T_contact_position").val(obj.contact_position);
                    $("#T_company_tel").val(obj.tel);

                    if (obj.Department && obj.Employee) {
                        fillemp(obj.Department, obj.Department_id, obj.Employee, obj.Employee_id);
                        $("#tr_contact0,#tr_contact1,#tr_contact2,#tr_contact3,#tr_contact4,#tr_contact5").remove();
                    }
                    else {
                        $.getJSON("../../data/hr_employee.ashx?Action=form&id=epu&rnd=" + Math.random(), function (result) {
                            var obj = eval(result);
                            for (var n in obj) {
                                if (obj[n] == null)
                                    obj[n] = "";
                            }
                            fillemp(obj.dname, obj.d_id, obj.name, obj.ID);
                            
                        })                        
                    }

                    $('#T_customertype').ligerComboBox({ width: 97, url: "../../data/Param_SysParam.ashx?Action=combo&parentid=1&rnd=" + Math.random(), emptyText: '���գ�', initValue: obj.CustomerType_id });
                    $('#T_customerlevel').ligerComboBox({ width: 96, url: "../../data/Param_SysParam.ashx?Action=combo&parentid=2&rnd=" + Math.random(), emptyText: '���գ�', initValue: obj.CustomerLevel_id });
                    $('#T_CustomerSource').ligerComboBox({ width: 196, url: "../../data/Param_SysParam.ashx?Action=combo&parentid=3&rnd=" + Math.random(), emptyText: '���գ�', initValue: obj.CustomerSource_id });
                    $("#T_industry").ligerComboBox({ width: 196, url: "../../data/Param_SysParam.ashx?Action=combo&parentid=8&rnd=" + Math.random(), emptyText: '���գ�', initValue: obj.industry_id });
                    $("#T_private").ligerGetComboBoxManager().selectValue(obj.privatecustomer);

                    b = $('#T_City').ligerComboBox({ width: 96, emptyText: '���գ�' });
                    c = $('#T_Provinces').ligerComboBox({
                        width: 97,
                        initValue: obj.Provinces_id,
                        url: "../../data/Param_City.ashx?Action=combo1&rnd=" + Math.random(),
                        onSelected: function (newvalue, newtext) {
                            if (!newvalue)
                                newvalue = -1;
                            $.get("../../data/Param_City.ashx?Action=combo2&pid=" + newvalue + "&rnd=" + Math.random(), function (data, textStatus) {
                                b.setData(eval(data));
                                b.selectValue(obj.City_id);
                            });
                        }, emptyText: '���գ�'
                    });


                }
            });
        }
        function f_selectContact() {
            top.$.ligerDialog.open({
                zindex: 9003,
                title: 'ѡ��Ա��', width: 850, height: 400, url: "hr/Getemp_Auth.aspx?auth=1", buttons: [
                    { text: 'ȷ��', onclick: f_selectContactOK },
                    { text: 'ȡ��', onclick: f_selectContactCancel }
                ]
            });
            return false;
        }
        function f_selectContactOK(item, dialog) {
            var data = dialog.frame.f_select();
            if (!data) {
                alert('��ѡ��Ա��!');
                return;
            }

            fillemp(data.dname, data.d_id, data.name, data.ID);

            dialog.close();
        }
        function fillemp(dep,depid,emp,empid)
        {
            $("#T_employee").val("��" + dep + "��" + emp);
            $("#T_employee1").val(emp);
            $("#T_employee_val").val(empid);
            $("#T_dep").val(dep);
            $("#T_dep_val").val(depid);
        }
        function f_selectContactCancel(item, dialog) {
            dialog.close();
        }
        function remote() {
            var url = "../../data/CRM_Customer.ashx?Action=validate&T_cid=" + getparastr("cid") + "&rnd=" + Math.random();
            return url;
        }

    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <table style="width: 600px; margin: 5px;" class='bodytable1'>
            <tr>
                <td colspan="4" class="table_title1">������Ϣ</td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">��˾���ƣ�</div>
                </td>
                <td>
                    <input type="text" id="T_company" name="T_company" ltype="text" ligerui="{width:196}" validate="{required:true}" /></td>
                <td>
                    <div style="width: 80px; text-align: right; float: right">��˾��ַ��</div>
                </td>
                <td>
                    <input id="T_Website" name="T_Website" type="text" ltype="text" ligerui="{width:196}" validate="{required:false}" /></td>
            </tr>
            <tr>
                <td>

                    <div style="width: 80px; text-align: right; float: right">������ҵ��</div>

                </td>
                <td>
                    <input type="text" id="T_industry" name="T_industry" /></td>
                <td>
                    <div style="width: 80px; text-align: right; float: right">����������</div>
                </td>
                <td>
                    <div style="width: 100px; float: left">
                        <input id="T_Provinces" name="T_Provinces" type="text" style="width: 96px;" />
                    </div>
                    <div style="width: 98px; float: left">
                        <input id="T_City" name="T_City" type="text" style="width: 96px;" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>

                    <div style="width: 80px; text-align: right; float: right">��˾�绰��</div>

                </td>
                <td>

                    <input id="T_company_tel" name="T_company_tel" type="text" ltype="text" ligerui="{width:196}" validate="{required:true}" /></td>
                <td>
                    <div style="width: 80px; text-align: right; float: right">���棺</div>
                </td>
                <td>
                    <input id="T_fax" name="T_fax" type="text" ltype="text" ligerui="{width:196}" /></td>
            </tr>
            <tr>
                <td>

                    <div style="width: 80px; text-align: right; float: right">��˾��ַ��</div>

                </td>
                <td colspan="3">

                    <input type="text" id="T_address" name="T_address" ltype="text" ligerui="{width:495}" /></td>
            </tr>
            <tr id="tr_contact0">
                <td colspan="4" class="table_title1">����ϵ��</td>
            </tr>
            <tr id="tr_contact1">
                <td>
                    <div style="width: 80px; text-align: right; float: right">��ϵ�ˣ�</div>
                </td>
                <td>
                    <%--<input type="text" id="T_customername" name="T_customername" style="width: 96px;" ltype="text" ligerui="{width:196}" validate="{required:true}" />--%>
                    <div style="width: 140px; float: left">
                        <input id="T_customername" name="T_customername" type="text" ltype="text" ligerui="{width:136}" style="width: 146px" validate="{required:true}" />
                    </div>
                    <div style="width: 58px; float: left">
                        <input type="text" id="T_sex" name="T_sex" style="width: 56px" ltype="select" ligerui="{width:56,data:[{id:'����',text:'����'},{id:'Ůʿ',text:'Ůʿ'}]}" validate="{required:true}" />
                    </div>
                </td>
                <td>
                    <div style="width: 80px; text-align: right; float: right">���š�ְ��</div>
                </td>
                <td>
                    <div style="width: 100px; float: left">
                        <input type="text" id="T_contact_dep" name="T_contact_dep" ltype="text" ligerui="{width:96}" />
                    </div>
                    <div style="width: 98px; float: left">
                        <input type="text" id="T_contact_position" name="T_contact_position" ltype="text" ligerui="{width:96}" />
                    </div>
                </td>
            </tr>
            <tr id="tr_contact2">
                <td>
                    <div style="width: 80px; text-align: right; float: right">�����ʼ���</div>
                </td>
                <td>
                    <input id="T_email" name="T_email" type="text" ltype="text" ligerui="{width:196}" validate="{required:false,email:true}" /></td>
                <td>
                    <div style="width: 80px; text-align: right; float: right">QQ��</div>
                </td>
                <td>

                    <input type="text" id="T_qq" name="T_qq" ltype="text" ligerui="{width:196}" /></td>
            </tr>
            <tr id="tr_contact3">
                <td>

                    <div style="width: 80px; text-align: right; float: right">��ϵ�绰��</div>

                </td>
                <td>

                    <input id="T_tel" name="T_tel" type="text" ltype="text" ligerui="{width:196}" /></td>
                <td>

                    <div style="width: 80px; text-align: right; float: right">�ֻ���</div>

                </td>
                <td>
                    <input id="T_mobil" name="T_mobil" type="text" ltype="text" ligerui="{width:196}" /></td>
            </tr>
            <tr id="tr_contact4">
                <td>
                    <div style="width: 80px; text-align: right; float: right">���ã�</div>
                </td>
                <td colspan="3">

                    <input id="T_hobby" name="T_hobby" type="text" ltype="text" ligerui="{width:495}" /></td>
            </tr>
            <tr id="tr_contact5">
                <td>
                    <div style="width: 80px; text-align: right; float: right">��ע��</div>
                </td>
                <td colspan="3">
                    <input id="T_contact_remarks" name="T_contact_remarks" type="text" ltype="text" ligerui="{width:495}" /></td>
            </tr>
            <tr>
                <td colspan="4" class="table_title1">����</td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">�ͻ����ͣ�</div>
                </td>
                <td>
                    <div style="width: 100px; float: left">
                        <input id="T_customertype" name="T_customertype" type="text" style="width: 96px" />
                    </div>
                    <div style="width: 98px; float: left">
                        <input id="T_customerlevel" name="T_customerlevel" type="text" style="width: 96px" />
                    </div>
                </td>
                <td>
                    <div style="width: 80px; text-align: right; float: right">�ͻ���Դ��</div>
                </td>
                <td>
                    <input id="T_CustomerSource" name="T_CustomerSource" type="text" />
                </td>
            </tr>

            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">�ͻ�������</div>
                </td>
                <td colspan="3">
                    <input id="T_descript" name="T_descript" type="text" ltype="text" ligerui="{width:495}" /></td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">��&nbsp; ע��</div>
                </td>
                <td colspan="3">
                    <input id="T_remarks" name="T_remarks" type="text" ltype="text" ligerui="{width:495}" /></td>
            </tr>
            <tr>
                <td colspan="4" class="table_title1">����</td>
            </tr>
            <tr>
                <td>
                    <div style="width: 80px; text-align: right; float: right">״̬��</div>
                </td>
                <td>
                    <input id="T_private" name="T_private" type="text" ltype="select" ligerui="{width:196,data:[{id:'˽��',text:'˽��'},{id:'����',text:'����'}]}" validate="{required:true}" /></td>
                <td>
                    <div style="width: 80px; text-align: right; float: right">ҵ��Ա��</div>
                </td>
                <td>
                    <input id="T_employee" name="T_employee" type="text" validate="{required:true}" style="width: 196px" />
                    <input id="T_employee1" name="T_employee1" type="hidden" />
                    <input id="T_dep_val" name="T_dep_val" type="hidden" />
                    <input id="T_dep" name="T_dep" type="hidden" />
                </td>
            </tr>
        </table>


    </form>
</body>
</html>
