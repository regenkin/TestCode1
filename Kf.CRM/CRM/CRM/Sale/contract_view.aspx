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

    <script src="../../lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>

    <script src="../../lib/jquery-validation/jquery.validate.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/jquery.metadata.js" type="text/javascript"></script>
    <script src="../../lib/jquery-validation/messages_cn.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/common.js" type="text/javascript"></script>
    <script src="../../lib/ligerUI/js/plugins/ligerTip.js" type="text/javascript"></script>
    <script src="../../lib/jquery.form.js" type="text/javascript"></script>

    <script src="../../lib/json2.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>


    <script type="text/javascript">
        $(function () {
            $.metadata.setType("attr", "validate");
            KFCRM.validate($("#form1"));

            //$("#T_Contract_name").focus();
            $("form").ligerForm();

            if (getparastr("cid")) {
                loadForm(getparastr("cid"));
            }
           
        });



        function loadForm(oaid) {
            $.ajax({
                type: "GET",
                url: "../../data/Crm_contract.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                data: { Action: 'form', cid: oaid, rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        if (obj[n] == null || obj[n] == "null")
                            obj[n] = "";
                    }
                    //alert(obj.constructor); //String ���캯��
                    $("#T_Customer").val(obj.Customer_name);
                    $("#T_Customer_val").val(obj.Customer_id);
                    $("#T_contract_num").val(obj.Serialnumber);
                    $("#T_contract_name").val(obj.Contract_name);
                    $("#T_contract_amount").val(toMoney(obj.Contract_amount));
                    $("#T_pay_cycle").val(obj.Pay_cycle);
                    $("#T_contractor").val(obj.Customer_Contractor);

                    $("#T_start_date").val(obj.Start_date);
                    $("#T_end_date").val(obj.End_date);
                    $("#T_contract_date").val(obj.Sign_date);

                    $("#T_content").val(obj.Main_Content);
                    $("#T_remarks").val(obj.Remarks);

                    $("#c_emp_view").val("��" + obj.C_depname + "��" + obj.C_empname);
                    $("#f_emp_view").val("��" + obj.Our_Contractor_depname + "��" + obj.Our_Contractor_name);
                }
            });
            $("#maingrid4").ligerGrid({
                columns: [
                    { display: '���', width: 50, render: function (rowData, rowindex, value, column, rowid, page, pagesize) { return (page - 1) * pagesize + rowindex + 1; } },
                    { display: '�ļ���', name: 'file_name', width: 500, align: 'left' },
                    {
                        display: '�ļ���С', name: 'file_size', width: 100, align: 'right', render: function (item) {
                            return formatUnits(item.file_size);
                        }
                    },
                    {
                        display: '����', width: 120, render: function (item, i) {
                            var div = "";

                            div += ' <a href="javascript:void(0)" onclick="view_attachment(\'' + item.file_name + '\',\'' + item.real_name + '\')">�鿴</a>     ';
                            div += ' <a href="javascript:void(0)" onclick="down_attachment(\'' + item.real_name + '\')">����</a>    ';

                            return div;
                        }
                    }


                ],
                dataAction: 'local', pageSize: 30, pageSizeOptions: [20, 30, 50, 100],
                url: "../../data/CRM_contract_attachment.ashx?Action=get_attachment&contract_id=" + oaid + "&rnd=" + Math.random(),
                width: '810', height: '99%',
                heightDiff: -1,
            })
        }


        function set_tomoney(value) {
            $("#T_contract_amount").val(toMoney(value));
        }

        function view_attachment(filename, realname) {
            //alert(fileid + ":" + filename + ":" + realname);
            var filetype = null;
            if (filename) {
                var type = filename.substr(filename.lastIndexOf(".")).toLowerCase();

                switch (type) {
                    case ".jpg": case ".gif": case ".jpeg": case ".bmp": case ".png":
                        url = "file/view_image.aspx?page_id=" + getparastr("a") + "&filename=" + filename + "&realname=" + realname;
                        f_openWindow(url, "���鿴��" + filename, 1000, 500);
                        break;
                    case ".doc": case ".docx":
                        //case ".xls": case ".xlsx": case ".ppt": case ".pptx":
                        viewoffice(filename, realname);
                        break;
                    default:
                        $.ligerDialog.warn('�ݲ��ṩ"' + type + '"�����ļ�������Ԥ�����ܡ�');
                }
            }
        }
       
        function down_attachment(filename) {
            window.open("../../file/contract/" + filename);
        }
        function viewoffice(filename, realname) {
            $.ligerDialog.waitting('�ĵ�Ŭ��������,���Ժ�...');
            $.ajax({
                type: "GET",
                url: "../../data/CRM_contract_attachment.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                data: {
                    Action: 'get_office',
                    page_id: getparastr("a"),
                    filename: filename,
                    realname: realname,
                    rnd: Math.random()
                }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "text",
                success: function (result) {
                    $.ligerDialog.closeWaitting();
                    //alert(obj.constructor); //String ���캯��
                    if (result == "sucess:false") {
                        $.ligerDialog.warn("ϵͳ�����Ҳ�����ַ");
                    }
                    else {
                        //$("#view").attr("src", "../file/contract/" + result);
                        url = "../../file/contract" + result;
                        f_openWindow(url, "���鿴��" + filename, 1000, 500);
                    }
                }
            });
        }
        var activeDialog = null;
        function f_openWindow(url, title, width, height) {
            var dialogOptions = {
                width: width, height: height, title: title, url: url, buttons: [
                        {
                            text: '�ر�', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], zindex: 9003, isResize: true, timeParmName: 'a'
            };
            activeDialog = top.jQuery.ligerDialog.open(dialogOptions);
        }
        function f_reload() {
            var manager = $("#maingrid4").ligerGetGridManager();
            manager.loadData(true);
        }
    </script>

</head>
<body style="margin: 0;">
    <form id="form1" onsubmit="return false">

        <table style="width: 810px; margin: 5px;" class='bodytable2'>
            <tr>
                <td colspan="6" class="table_title1">��ͬ������Ϣ</td>
            </tr>
            <tr>

                <td width="72px">
                    <div align="right" style="width: 61px">
                        �ͻ���
                    </div>
                </td>
                <td>
                    <input type="text" id="T_Customer" name="T_Customer" ltype="text" ligerui="{width:182}" validate="{required:true}" /></td>
                <td>
                    <div align="right" style="width: 61px">
                        ��ͬ��ţ�
                    </div>
                </td>
                <td>
                    <input type="text" id="T_contract_num" name="T_contract_num" ltype="text" ligerui="{width:182}" validate="{required:true}" /></td>
                <td>
                    <div align="right" style="width: 61px">
                        ��ͬ��
                    </div>
                </td>
                <td>
                    <input type="text" id="T_contract_amount" name="T_contract_amount" style="text-align: right" ltype="text" ligerui="{width:182,number: true}" onchange="set_tomoney(this.value)" validate="{required:true}" /></td>
            </tr>
            <tr>
                <td width="62px">
                    <div align="right" style="width: 61px">
                        ��ͬ���ƣ�
                    </div>
                </td>
                <td colspan="3">
                    <input type="text" id="T_contract_name" name="T_contract_name" ltype="text" ligerui="{width:440}" validate="{required:true}" />
                </td>
                <td>
                    <div align="right" style="width: 61px">
                        ����������
                    </div>
                </td>
                <td>
                    <input type="text" id="T_pay_cycle" name="T_pay_cycle" ltype="spinner" ligerui="{width:182,type:'int',isNegative:false}" validate="{required:true}" /></td>
            </tr>
            <tr>
                <td width="62px">
                    <div align="right" style="width: 61px">
                        ��ʼʱ�䣺
                    </div>
                </td>
                <td>
                    <input type="text" id="T_start_date" name="T_start_date" ltype="date" validate="{required:true}" ligerui="{width:182}" /></td>
                <td width="62px">
                    <div align="right" style="width: 61px">
                        ����ʱ�䣺
                    </div>
                </td>
                <td>
                    <input type="text" id="T_end_date" name="T_end_date" ltype="date" validate="{required:true}" ligerui="{width:182}" /></td>
                <td>
                    <div align="right" style="width: 61px">
                        ǩ��ʱ�䣺
                    </div>
                </td>
                <td>
                    <input type="text" id="T_contract_date" name="T_contract_date" ltype="date" validate="{required:true}" ligerui="{width:182}" /></td>
            </tr>
            <tr>
                <td width="62px">
                    <div align="right" style="width: 61px">
                        �Է�ǩԼ��
                    </div>
                </td>
                <td>
                    <input type="text" id="T_contractor" name="T_contractor" ltype="text" ligerui="{width:182}" validate="{required:true}" /></td>
                <td width="62px">
                    <div align="right" style="width: 61px">
                        �ҷ�ǩԼ��
                    </div>
                </td>
                <td>
                    <input id="f_emp_view" name="f_emp_view" type="text" validate="{required:true}" ltype="text" ligerui="{width:182}" />
                </td>
                <td>
                    <div align="right" style="width: 61px">
                        �ͻ�������
                    </div>
                </td>
                <td>
                    <input id="c_emp_view" name="c_emp_view" type="text" ltype="text" ligerui="{width:182,disabled:true}" validate="{required:true}" />
                </td>
            </tr>
            <tr>
                <td colspan="6" class="table_title1">��ͬ����</td>
            </tr>
            <tr>

                <td width="72px">
                    <div align="right" style="width: 62px">��Ҫ���</div>
                </td>
                <td colspan="3">
                    <textarea cols="100" id="T_content" name="T_content" rows="5" class="l-textarea" style="width: 440px"></textarea></td>
                <td>
                    <div align="right" style="width: 62px">��ע��</div>
                </td>
                <td>
                    <textarea cols="100" id="T_remarks" name="T_remarks" rows="5" class="l-textarea" style="width: 182px"></textarea></td>
            </tr>

        </table>
    </form>
    <table style="width: 810px; margin: 5px;" class='bodytable2'>
        <tr>
            <td class="table_title1">
               ����
            </td>
        </tr>
    </table>
    <div id="maingrid4" style="margin: 5px;"></div>
</body>
</html>
