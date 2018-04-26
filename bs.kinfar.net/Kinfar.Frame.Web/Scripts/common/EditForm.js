var id = GetLocalQueryString("id"); //记录Id
var page = GetLocalQueryString("page"); //页面类型标识
var toDoTaskId = GetLocalQueryString("todoId"); //待办任务ID

//初始化
$(function () {
    //checkbox控件处理
    $("input[type=checkbox]").click(function () {
        if ($(this).attr("checked")) {
            $(this).attr("value", "1");
        }
        else {
            $(this).attr("value", "0");
        }
    });
    //html标签上存在这个类会导致滚动条无法下拉到最底部，一直处于闪烁状态
    $("html").removeClass("panel-fit");
    //明细弹出编辑页面数据处理
    var ff = GetLocalQueryString("ff"); //主从编辑页面弹出明细编辑标识
    if (ff == 1) { //是从主从编辑页面弹出明细编辑页面
        var detailModuleId = GetLocalQueryString("moduleId"); //明细模块Id
        var pmode = GetLocalQueryString("pmode"); //父页面编辑模式
        var gridId = "grid_" + detailModuleId;
        var iframe = null;
        if (pmode == 2) { //弹出框
            iframe = top.getParentDialogFrame(); //取父弹出框iframe
        }
        else { //tab方式
            iframe = top.getCurrentTabFrame();
        }
        if (page == "edit") { //编辑页面
            //从明细编辑网格加载数据
            var row = iframe[0].contentWindow.GetSelectRow(gridId);
            var form = $("#mainform");
            var formFields = GetFormFields();
            JsonToForm(row, form, formFields); //数据绑定到表单
        }
        var fg = GetLocalQueryString("fg"); //主网格页面的明细网格或附属模块网格标识
        var obj = null;
        if (fg == 1) { //加载明细模块或附属模块对应主模块选择行字段值
            obj = iframe[0].contentWindow.GetSelectRowTitleKeyValue()
        }
        else { //加载对应主表的外键字段值
            obj = iframe[0].contentWindow.GetTitleKeyValue();
        }
        if (obj) {
            var tempText = $("#" + obj.foreignFieldName).textbox("getText");
            if (obj.recordId) { //针对查看页面，明细新增
                $("#" + obj.foreignFieldName).textbox("setValue", obj.recordId);
            }
            if (!tempText) { //没有给对应的主表外键字段赋值时
                $("#" + obj.foreignFieldName).textbox("setText", obj.value);
            }
        }
    }
    else {
        //外键字段处理
        $("input[foreignField='1']").each(function () {
            var controlId = $(this).attr("Id");
            var control = $("#" + controlId);
            var textValue = $(this).attr("textValue");
            //设置外键字段的text值
            control.textbox("setText", textValue);
            //父模块外键字段标识
            var isParentForeignField = $(this).attr("isParentForeignField");
            if (isParentForeignField == "1") { //父模块外键字段没有初始化值是允许编辑
                var value = control.textbox("getValue"); //字段值
                if (!value) {
                    control.textbox("enable");
                }
            }
        });
    }
    //表单字段智能提示处理
    var formFields = GetFormFields();
    if (formFields && formFields.length > 0) {
        var moduleId = GetLocalQueryString("moduleId"); //模块Id
        $.each(formFields, function (i, obj) {
            if (obj.ForeignModuleName && obj.ForeignTitleKey && obj.ControlType == 8) { //外键字段
                var fieldDom = $('#' + obj.Sys_FieldName);
                var tempDom = fieldDom.next('span').find('input.textbox-text');
                FieldBindAutoCompleted(tempDom, moduleId, obj.Sys_FieldName, null, function (dom, item, fieldName, paramObj) {
                    var oldValue = paramObj.textbox("getValue");
                    tempDom.blur(function () {
                        var valueField = 'value'; //值字段名
                        var textField = 'text'; //文本字段名
                        paramObj.textbox("setValue", item[valueField]);
                        paramObj.textbox("setText", item[textField]);
                        OnFieldSelect(item, fieldName, valueField, textField); //触发字段选择事件
                        if (oldValue != item[valueField]) { //触发字段值改变事件
                            OnFieldValueChanged({ moduleId: moduleId }, fieldName, item[valueField], oldValue);
                        }
                    });
                }, fieldDom, "autoFlag=1");
            }
        });
    }
    //自定义表单加载完成事件
    if (typeof (OverFormLoadCompleted) == "function") {
        OverFormLoadCompleted();
    }
});

//表单保存
//obj：按钮对象
//backFun:保存成功后的回调函数
//isAddNew:保存成功后是否继续新增
//isDraft:是否为草稿
function Save(obj, backFun, isAddNew, isDraft) {
    var form = $("#mainform");
    var flag = form.form("validate");
    if (!flag) return;
    var msgTitle = '保存提示';
    var successMsg = '保存成功';
    var isFlowPage = false; //流程表单页面
    //保存方法
    var ExecuteSave = function () {
        //有自定义保存方法则先调用自定义方法否则调用通用
        if (typeof (OverSave) == "function") {
            OverSave(obj, backFun, isAddNew, isDraft);
            return;
        }
        var editMode = $(obj).attr("editMode"); //当前编辑模式
        if (editMode)
            editMode = parseInt(editMode);
        else
            editMode = 1; //默认标签编辑模式
        var detail = $(obj).attr("detail"); //是否明细编辑页面
        if (detail && detail == "true")
            detail = true;
        else
            detail = false;
        var tempModuleId = $(obj).attr("moduleId") || GetLocalQueryString("moduleId");
        var tempModuleName = $(obj).attr("moduleName") || decodeURIComponent(GetLocalQueryString("moduleName"));
        if (!detail) { //正常编辑保存
            var data = form.fixedSerializeArray();
            if (id) {
                data.push({ name: "Id", value: id });
            }
            //查看页面新增功能处理
            var ff = GetLocalQueryString("ff"); //主从编辑页面弹出明细编辑标识
            var fgFlag = GetLocalQueryString("fg"); //是否是主网格下方明细或附属模块新增标识
            if (ff == 1) { //是从主从查看页面弹出明细新增页面
                //加载对应主表的外键字段值
                var pmode = GetLocalQueryString("pmode"); //父页面编辑模式
                var iframe = null;
                if (pmode == 2) { //弹出框
                    iframe = top.getParentDialogFrame(); //取父弹出框iframe
                }
                else { //tab方式
                    iframe = top.getCurrentTabFrame();
                }
                var tempObj = iframe[0].contentWindow.GetTitleKeyValue();
                if (tempObj && tempObj.recordId && tempObj.foreignFieldName) { //主表记录Id
                    data.push({ name: tempObj.foreignFieldName, value: tempObj.recordId });
                }
                else {
                    top.showAlertMsg(msgTitle, "对应的主表外键字段值获取失败！", "info");
                    return;
                }
            }
            //调用主模块自定义数据处理函数
            if (typeof (OverMainModuleDataHandleBeforeSaved) == "function") {
                OverMainModuleDataHandleBeforeSaved(data);
            }
            //组装表单数据对象
            var release = parseInt($(obj).attr("release")) == 1;
            var formObject = { ModuleId: tempModuleId, ModuleName: tempModuleName, ModuleData: data, IsDraft: isDraft ? true : false, IsReleaseDraft: release };
            var hasDetail = $("div[id^='regon_']").length > 0; //是否有明细模块
            if (hasDetail) { //有明细模块
                //组装明细数据
                var details = [];
                $("div[id^='regon_']").each(function () {
                    var moduleDatas = [];
                    //明细模块Id
                    var detailModuleId = $(this).attr("moduleId");
                    //明细模块名称
                    var detailModuleName = $(this).attr("moduleName");
                    var detailGridId = 'grid_';
                    if (detailModuleId) detailGridId = 'grid_' + detailModuleId;
                    //明细结束编辑
                    EndEditAllRows(detailGridId);
                    //组装数据
                    var gridObj = $("#" + detailGridId);
                    var rows = gridObj.datagrid("getRows");
                    for (var i = 0; i < rows.length; i++) {
                        var row = rows[i]; //一条明细数据对象
                        //调用明细模块自定义数据处理函数
                        if (typeof (OverDetailModuleDataHandleBeforeSaved) == "function") {
                            OverDetailModuleDataHandleBeforeSaved(row);
                        }
                        //数据组装
                        var detailDatas = [];
                        for (var p in row) {
                            detailDatas.push({ name: p, value: row[p] });
                        }
                        moduleDatas.push(JSON.stringify(detailDatas));
                    }
                    if (moduleDatas.length > 0) {
                        details.push({ ModuleId: detailModuleId, ModuleName: detailModuleName, ModuleDatas: moduleDatas });
                    }
                });
                if (details.length > 0) { //有明细数据
                    formObject["Details"] = details;
                }
            }
            //流程处理
            var btnTagId = $(obj).attr('id');
            if (btnTagId != null && btnTagId.length > 0 && btnTagId.indexOf('flowBtn_') > -1) {
                var flowBtnId = btnTagId.replace('flowBtn_', '');
                formObject.OpFlowBtnId = flowBtnId;
                isFlowPage = true;
                if (toDoTaskId != undefined && toDoTaskId != null && toDoTaskId.length > 0) {
                    formObject.ToDoTaskId = toDoTaskId;
                    formObject.ApprovalOpinions = $('#txt_approvalOpinions').textbox('getValue');
                    msgTitle = '审批流程提示';
                    successMsg = '流程操作成功！';
                }
                else {
                    msgTitle = '发起流程提示';
                    successMsg = '流程发起成功！';
                }
                var returnNodeId = $(obj).attr('returnNodeId'); //针对回退时回退结点
                var directHandler = $(obj).attr('directHandler'); //针对指派时指派人id
                if (returnNodeId != undefined && returnNodeId != null && returnNodeId.length > 0) {
                    formObject.ReturnNodeId = returnNodeId;
                }
                if (directHandler != undefined && directHandler != null && directHandler.length > 0) {
                    formObject.DirectHandler = directHandler;
                    msgTitle = '流程指派提示';
                    successMsg = '流程指派成功！';
                }
            }
            //开始保存
            var url = "/" + CommonController.Async_Data_Controller + "/SaveData.html";
            $.ajax({
                type: "post",
                url: url,
                data: { formData: $.base64.encode(escape(JSON.stringify(formObject))) },
                beforeSend: function () {
                    top.openWaitDialog('拼命处理中...');
                },
                success: function (result) {
                    if (result.Success) { //保存成功
                        if (editMode == 1) { //tab编辑模式
                            var recordId = result.RecordId;
                            var tempFun = function () {
                                top.showMsg(msgTitle, successMsg, function () {
                                    //保存成功后的回调函数
                                    top.closeWaitDialog();
                                    if (typeof (backFun) == "function") {
                                        backFun(result);
                                    }
                                    //自定义保存完成事件处理函数
                                    if (typeof (OverAfterSaveCompleted) == "function") {
                                        OverAfterSaveCompleted(result);
                                    }
                                    //刷新网格
                                    var tb = GetLocalQueryString("tb"); //网格对应的tabindex
                                    if (tb && parseInt(tb) > 0) {
                                        var tempIframe = top.getTabFrame(parseInt(tb));
                                        if (tempIframe.length > 0) {
                                            tempIframe[0].contentWindow.RefreshGrid(gridId);
                                        }
                                    }
                                    if (!isFlowPage) { //非流程表单页面
                                        var fp = GetLocalQueryString("fp");
                                        if (fp == 'grid') { //来自网格页面
                                            var tab = GetSelectedTab();
                                            if (isAddNew) { //保存后新增
                                                var addUrl = "/Page/EditForm.html?page=add&moduleId=" + tempModuleId + "&r=" + Math.random();
                                                var title = "新增" + tempModuleName;
                                                //跳转到新增页面
                                                UpdateTab(null, tab, addUrl, title);
                                            }
                                            else {
                                                var viewUrl = "/Page/ViewForm.html?page=view&moduleId=" + tempModuleId + "&id=" + recordId;
                                                viewUrl += "&mode=" + editMode + "&r=" + Math.random();
                                                //跳转到查看页面
                                                var title = tab.panel('options').title.replace("编辑", "查看").replace("新增", "查看");
                                                UpdateTab(null, tab, viewUrl, title);
                                            }
                                        }
                                    }
                                    else { //流程表单页面
                                        //关闭当前tab
                                        CloseTab();
                                    }
                                });
                            }
                            if (recordId != undefined && recordId != null && typeof (SaveFormAttach) == "function") { //有附件需要保存
                                SaveFormAttach(tempModuleId, recordId, function () {
                                    tempFun();
                                });
                            }
                            else {
                                tempFun();
                            }
                        }
                        else if (editMode == 2 || editMode == 4) { //弹出框编辑模式或网格表单编辑模式
                            var recordId = result.RecordId;
                            var pmode = GetLocalQueryString("pmode"); //父页面编辑模式
                            var gridId = $(obj).attr("gridId");
                            var iframe = null;
                            if (pmode == 2) { //父页面为弹出框
                                iframe = top.getCurrentDialogFrame(); //取当前弹出框iframe
                            }
                            else { //tab方式
                                iframe = top.getCurrentTabFrame();
                            }
                            var tempFun = function () {
                                if (editMode == 2 || (editMode == 4 && !id)) { //弹出框或网格表单编辑新增弹出框页面
                                    top.closeDialog();
                                }
                                if (editMode == 2 || (editMode == 4 && !id)) {
                                    //刷新当前grid
                                    iframe[0].contentWindow.RefreshGrid(gridId);
                                }
                                //关闭对话框
                                top.showMsg(msgTitle, successMsg, function () {
                                    //保存成功后的回调函数
                                    top.closeWaitDialog();
                                    if (typeof (backFun) == "function") {
                                        backFun(result);
                                    }
                                    //自定义保存完成事件处理函数
                                    if (typeof (OverAfterSaveCompleted) == "function") {
                                        OverAfterSaveCompleted(result);
                                    }
                                    if (!isFlowPage) { //非流程表单页面
                                        var fp = GetLocalQueryString("fp");
                                        if (fp == 'grid') { //来自网格页面
                                            if (editMode == 4 && id) { //网格表单编辑时
                                                //刷新当前grid
                                                iframe[0].contentWindow.RefreshGrid(gridId);
                                            }
                                            if (isAddNew) { //保存后新增，继续弹出新增对话框
                                                var dom = iframe.contents().find("#btnAdd");
                                                if (dom.length > 0) { //新增按钮在工具栏上
                                                    dom.click();
                                                }
                                                else { //新增按钮在网格内
                                                    var tempDom = iframe.contents().find("div[id^='rowOperateDiv_'").eq(0);
                                                    var tag = tempDom.attr("Id").replace("rowOperateDiv_", "");
                                                    iframe[0].contentWindow.Add(iframe.contents().find("#btnAdd_" + tag));
                                                }
                                            }
                                        }
                                    }
                                });
                            }
                            if (recordId != undefined && recordId != null && typeof (SaveFormAttach) == "function") { //有附件需要保存
                                SaveFormAttach(tempModuleId, recordId, function () {
                                    tempFun();
                                });
                            }
                            else {
                                tempFun();
                            }
                        }
                    }
                    else {
                        if (result.RecordId != undefined && result.RecordId != null && result.RecordId != '00000000-0000-0000-0000-000000000000')
                            id = result.RecordId;
                        top.showAlertMsg(msgTitle, result.Message, "info", function () {
                            top.closeWaitDialog();
                        });
                    }
                },
                error: function (err) {
                    top.showAlertMsg(msgTitle, '数据保存失败，服务器异常！', "error", function () {
                        top.closeWaitDialog();
                    });
                },
                dataType: "json"
            });
        }
        else { //明细编辑保存
            if (editMode == 2) { //弹出框编辑模式
                var gridId = "grid_" + tempModuleId;
                var row = GetFormData(true);
                var pmode = GetLocalQueryString("pmode"); //父页面编辑模式
                var iframe = null;
                if (pmode == 2) { //父页面为弹出框
                    iframe = top.getParentDialogFrame(); //取父弹出框iframe
                }
                else { //tab方式
                    iframe = top.getCurrentTabFrame();
                }
                if (page == "add") {
                    iframe[0].contentWindow.AppendRow(gridId, row);
                    top.closeDialog();
                }
                else if (page == "edit") {
                    var rowIndex = iframe[0].contentWindow.GetSelectRowIndex(gridId);
                    iframe[0].contentWindow.UpdateRow(gridId, rowIndex, row);
                    top.closeDialog();
                }
                if (isAddNew) { //继续新增
                    var dom = iframe.contents().find("div[id^='regon_'] a[id^='btnAdd']");
                    dom.click();
                }
            }
        }
    }
    //有自定义保存前验证方法
    if (typeof (OverBeforeSaveVerify) == "function") {
        //调用后执行回调函数返回验证异常信息
        OverBeforeSaveVerify(function (errMsg) {
            if (errMsg && errMsg.length > 0) { //验证不通过
                top.showAlertMsg(msgTitle, errMsg, "info");
                return;
            }
            else {
                ExecuteSave();
            }
        });
    }
    else {
        ExecuteSave();
    }
}

//提交流程
//obj：按钮对象
function SubmitFlow(obj) {
    top.showConfirmMsg('流程提交', '确定要提交流程吗？', function (action) {
        if (action) {
            Save(obj);
        }
    });
}

//审批流程，同意、拒绝
//obj：按钮对象
function ApprovalFlow(obj) {
    var btnText = $(obj).find('span.l-btn-text').text();
    top.showConfirmMsg('审批确认', '确定要' + btnText + '吗？', function (action) {
        if (action) {
            Save(obj);
        }
    });
}

//回退流程，针对流程回退
//obj：按钮对象
function BackFlow(obj) {
    var btnText = $(obj).find('span.l-btn-text').text();
    ExecuteCommonAjax('/' + CommonController.Async_Bpm_Controller + '/LoadBackNode.html', { toDoTaskId: toDoTaskId }, function (result) {
        if (result != null && result.html != null) {
            top.openOkCancelDialog('选择回退结点', result.html, 270, 200, function (iframe, backFun) {
                var returnNode = top.$("input[name='backNodes']").val();
                $(obj).attr('returnNodeId', returnNode);
                top.showConfirmMsg('审批确认', '确定要' + btnText + '吗？', function (action) {
                    if (action) {
                        if (typeof (backFun) == "function")
                            backFun(true);
                        Save(obj);
                    }
                });
            });
        }
    }, false, true);
}

//指派流程，专门针对流程指派
//obj：按钮对象
function DirectFlow(obj) {
    var btnText = $(obj).find('span.l-btn-text').text();
    SelectModuleTree('员工管理', null, false, function (row) {
        var empId = row['Id'];
        $(obj).attr('directHandler', empId);
        top.showConfirmMsg('审批确认', '确定要' + btnText + '蚂？', function (action) {
            if (action) {
                Save(obj);
            }
        });
    });
}

//获取表单数据
//handleForeignField:是否处理外键字段
function GetFormData(handleForeignField) {
    var form = $("#mainform");
    var data = form.fixedSerializeArray();
    var formData = {};
    $.each(data, function (index) {
        var name = this['name'];
        var value = this["value"];
        if (!formData[name]) {
            formData[name] = value;
        }
        if (handleForeignField) {
            var field = GetFormField(name);
            if (field != null && field.ForeignModuleName.length > 0) { //是外键字段
                var textName = name.substr(0, name.length - 2) + "Name";
                var textValue = $("#" + name, form).textbox("getText");
                formData[textName] = textValue;
            }
        }
    });
    return formData;
}

//获取表单的titlekey字段值
function GetTitleKeyValue() {
    var field = GetTitleKeyField();
    if (field && field.Sys_FieldName) {
        var name = field.Sys_FieldName;
        var value = $("#" + name).textbox('getValue');
        return { name: name, value: value, foreignFieldName: field.ForignFieldName };
    }
    return null;
}

//下拉框、下拉列表、下拉树的下拉数据加载成功事件
//fieldName:字段名
//valueField:值字段
//textField:显示字段
function OnFieldLoadSuccess(fieldName, valueField, textField) {
    //调用模块自定义事件
    if (typeof (OverOnFieldLoadSuccess) == "function") {
        OverOnFieldLoadSuccess(fieldName, valueField, textField);
    }
}

//下拉框、下拉列表、下拉树的下拉数据加载失败事件
//fieldName:字段名
//valueField:值字段
//textField:显示字段
//arguments:在数据加载失败的时候触发，arguments参数和jQuery的$.ajax()函数里面的'error'回调函数的参数相同。
function OnLoadError(fieldName, valueField, textField, arguments) {
    //调用模块自定义事件
    if (typeof (OverOnLoadError) == "function") {
        OnLoadError(fieldName, valueField, textField, arguments);
    }
}

//下拉框、下拉列表、下拉树数据项选择事件
//record:选择的项
//fieldName:字段名
//valueField:值字段
//textField:显示字段
function OnFieldSelect(record, fieldName, valueField, textField) {
    //调用模块自定义事件
    if (typeof (OverOnFieldSelect) == "function") {
        OverOnFieldSelect(record, fieldName, valueField, textField);
    }
}

//字段值改变事件
//moduleInfo:模块信息
//fieldName:字段名
//newValue:改变后的值
//oldValue:改变前的值
function OnFieldValueChanged(moduleInfo, fieldName, newValue, oldValue) {
    var linkFields = $('#' + fieldName).attr('linkFields');
    if (linkFields && linkFields.length > 0) { //该字段存在值关联字段
        var token = linkFields.split(',');
        for (var i = 0; i < token.length; i++) {
            var tempDom = $('#' + token[i]);
            if (!tempDom.textbox('getText')) { //新增或编辑时，如果值关联字段为空
                tempDom.textbox('setText', newValue);
            }
        }
    }
    //调用模块自定义事件
    if (typeof (OverOnFieldValueChanged) == "function") {
        OverOnFieldValueChanged(moduleInfo, fieldName, newValue, oldValue);
    }
}

//下拉框、下拉列表、下拉树数据过滤
//fieldName:字段名
//valueField:值字段
//textField:显示字段
//data:当前数据
//parentDom:下拉树时父节点DOM对象
function LoadFilter(fieldName, valueField, textField, data, parentDom) {
    //调用模块自定义事件
    if (typeof (OverLoadFilter) == "function") {
        return OverLoadFilter(fieldName, valueField, textField, data, parentDom);
    }
    if (data && data.length > 0) {
        if (typeof (data) == "string") {
            var tempData = eval("(" + data + ")");
            arr = [];
            arr.push(tempData);
            return arr;
        }
        else {
            return data;
        }
    }
    return null;
}

//编辑表单的标签选择事件
//title:标签页面标题
//index://标签序号
function OnEditFormTabSelect(title, index) {
    if (typeof (OverOnEditFormTabSelect) == "function") {
        OverOnEditFormTabSelect(title, index);
    }
}

//下拉树自动完成
//q:用户输入字符
//fieldName:字段名称
function QueryComboTree(q, fieldName) {
    if (typeof (OverQueryComboTree) == "function") {
        OverQueryComboTree(q, fieldName);
    }
    else {
        var tree = $('#' + fieldName).combotree('tree');
        tree.tree("search", q);
    }
}

