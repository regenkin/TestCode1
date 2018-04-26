//通用控制器名
var CommonController = {
    Data_Controller: "Data",
    Async_Data_Controller: "DataAsync",
    System_Controller: "System",
    Async_System_Controller: "SystemAsync",
    Annex_Controller: "Annex",
    Async_Annex_Controller: "AnnexAsync",
    User_Controller: "User",
    Async_User_Controller: "UserAsync",
    Bpm_Controller: "Bpm",
    Async_Bpm_Controller: "BpmAsync"
};

$(function () {

});

/*----------------系统公共函数-----------------------*/
//dom: 如:$("#tt"),
//url: 获取tree数据路径
//params：其他参数
function LoadTree(dom, url, params) {
    var treeParams = {
        url: url,
        loadFilter: function (data) {
            if (typeof (TreeLoadFilter) == "function") {
                return TreeLoadFilter(data, dom);
            }
            if (typeof (data) == 'string') {
                var tempData = eval("(" + data + ")");
                return tempData;
            }
            else {
                arr = [];
                arr.push(data);
                return arr;
            }
        },
        onClick: function (node) {
            if (typeof (TreeNodeOnClick) == "function") {
                TreeNodeOnClick(node, dom);
                return;
            }
        },
        onExpand: function (node) {
            if (typeof (TreeNodeExpand) == "function") {
                TreeNodeExpand(node, dom);
                return;
            }
        },
        onCollapse: function (node) {
            if (typeof (TreeNodeCollapse) == "function") {
                TreeNodeCollapse(node, dom);
                return;
            }
        },
        onLoadSuccess: function (node, data) {
            if (typeof (TreeOnLoadSuccess) == "function") {
                TreeOnLoadSuccess(node, data, dom);
                return;
            }
        },
        onBeforeSelect: function (node) {
            if (typeof (TreeOnBeforeSelect) == "function") {
                return TreeOnBeforeSelect(node, dom);
            }
            return true;
        }
    };
    if (params) {
        if (params.method) {
            treeParams.method = params.method;
        }
        if (params.animate) {
            treeParams.animate = params.animate;
        }
        if (params.checkbox) {
            treeParams.checkbox = params.checkbox;
        }
        if (params.cascadeCheck) {
            treeParams.cascadeCheck = params.cascadeCheck;
        }
        if (params.onlyLeafCheck) {
            treeParams.onlyLeafCheck = params.onlyLeafCheck;
        }
        if (params.lines) {
            treeParams.lines = params.lines;
        }
        if (params.dnd) {
            treeParams.dnd = params.dnd;
        }
        if (params.data) {
            treeParams.data = params.data;
        }
        if (params.queryParams) {
            treeParams.queryParams = params.queryParams;
        }
        if (params.formatter) {
            treeParams.formatter = params.formatter;
        }
        if (params.onDblClick) {
            treeParams.onDblClick = params.onDblClick;
        }
        if (params.onBeforeLoad) {
            treeParams.onBeforeLoad = params.onBeforeLoad;
        }
        if (params.onLoadError) {
            treeParams.onLoadError = params.onLoadError;
        }
        if (params.onBeforeExpand) {
            treeParams.onBeforeExpand = params.onBeforeExpand;
        }
        if (params.onBeforeCollapse) {
            treeParams.onBeforeCollapse = params.onBeforeCollapse;
        }
        if (params.onBeforeCheck) {
            treeParams.onBeforeCheck = params.onBeforeCheck;
        }
        if (params.onCheck) {
            treeParams.onCheck = params.onCheck;
        }
        if (params.onSelect) {
            treeParams.onSelect = params.onSelect;
        }
        if (params.onContextMenu) {
            treeParams.onContextMenu = params.onContextMenu;
        }
        if (params.onBeforeDrag) {
            treeParams.onBeforeDrag = params.onBeforeDrag;
        }
        if (params.onStartDrag) {
            treeParams.onStartDrag = params.onStartDrag;
        }
        if (params.onStopDrag) {
            treeParams.onStopDrag = params.onStopDrag;
        }
        if (params.onDragEnter) {
            treeParams.onDragEnter = params.onDragEnter;
        }
        if (params.onDragOver) {
            treeParams.onDragOver = params.onDragOver;
        }
        if (params.onDragLeave) {
            treeParams.onDragLeave = params.onDragLeave;
        }
        if (params.onBeforeDrop) {
            treeParams.onBeforeDrop = params.onBeforeDrop;
        }
        if (params.onDrop) {
            treeParams.onDrop = params.onDrop;
        }
        if (params.onBeforeEdit) {
            treeParams.onBeforeEdit = params.onBeforeEdit;
        }
        if (params.onAfterEdit) {
            treeParams.onAfterEdit = params.onAfterEdit;
        }
        if (params.onCancelEdit) {
            treeParams.onCancelEdit = params.onCancelEdit;
        }
    }
    dom.tree(treeParams);
}

//将常规JSON数据转换成树状数据
//treeData:json数据
//idField:id字段名
//pIdField:父id字段名
//childrenField:children字段名
function ToTreeData(treeData, idField, pIdField, childrenField) {
    var arr = [], hash = {}, len = (treeData || []).length;
    for (var i = 0; i < len; i++) {
        hash[treeData[i][idField]] = treeData[i];
    }
    for (var j = 0; j < len; j++) {
        var node = treeData[j], hashNodes = hash[node[pIdField]];
        if (hashNodes) {
            !hashNodes[childrenField] && (hashNodes[childrenField] = []);
            hashNodes[childrenField].push(node);
        } else {
            arr.push(node);
        }
    }
    return arr;
};

//网格按钮操作前验证
//moduleName:模块名称
//buttonText:操作按钮显示名称
//ids:操作记录集合,多个以逗号分隔
//backFun:验证回调函数
function GridOperateVerify(moduleName, buttonText, ids, backFun) {
    //客户端验证
    var errMsg = null;
    //调用自定义网格按钮操作客户端验证方法
    if (typeof (OverrideGridOperateVerify) == "function") {
        errMsg = OverrideGridOperateVerify(moduleName, buttonText, ids);
        if (errMsg && errMsg.length > 0) { //验证不通过
            if (typeof (backFun) == "function") {
                backFun(errMsg);
                return;
            }
        }
    }
    //服务端验证
    $.ajax({
        type: "post",
        url: '/' + CommonController.Async_Data_Controller + '/GridOperateVerify.html',
        data: { moduleName: moduleName, buttonText: escape(buttonText), ids: ids },
        success: function (result) {
            if (typeof (backFun) == "function") {
                backFun(result.Message);
            }
        },
        error: function (err) {
            if (typeof (backFun) == "function") {
                backFun("服务端调用异常！");
            }
        },
        dataType: "json"
    });
}

//弹出框、弹出树选择数据，外键选择数据
//obj:外键输入框对象
//backFun:回调函数
function SelectDialogData(obj, backFun) {
    var url = $(obj).attr("url");
    if (!url) return;
    var isTree = $(obj).attr('isTree') == '1';
    url += "&r=" + Math.random(); //弹出框页面url
    var textField = isTree ? "Name" : $(obj).attr("textField"); //文本字段
    var valueField = isTree ? "Id" : $(obj).attr("valueField"); //值字段
    var title = "选择数据";
    var isMutiSelect = $(obj).attr('ms') == '1'; //是否多选
    var foreignModuleName = $(obj).attr("foreignModuleName"); //外键模块
    if (foreignModuleName) {
        title = "选择" + foreignModuleName;
    }
    var toolbar = [{
        text: "选择",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            var foreignModuleId = $(obj).attr("foreignModuleId"); //外键模块Id
            var win = top.getCurrentDialogFrame()[0].contentWindow;
            var rows = isTree ? win.GetSelectData() : (isMutiSelect ? win.GetSelectRows("grid_" + foreignModuleId) :
                      win.GetSelectRow("grid_" + foreignModuleId));
            if (!rows || (isMutiSelect && (!rows || rows.length == 0))) {
                top.showAlertMsg("提示", '请至少选择一条记录！', 'info');
                return;
            }
            if (typeof (backFun) != "function") { //通用处理
                var fieldName = $(obj).attr("id");
                var txtControl = $("#" + fieldName);
                var oldValue = txtControl.textbox("getValue");
                var vf = '';
                var tf = '';
                if (isMutiSelect) { //多选
                    for (var i = 0; i < rows.length; i++) {
                        if (vf) {
                            vf += ',';
                            tf += ',';
                        }
                        vf += rows[i][valueField] ? rows[i][valueField] : '';
                        tf += rows[i][textField] ? rows[i][textField] : '';
                    }
                }
                else {
                    vf = rows[valueField];
                    tf = rows[textField];
                }
                txtControl.textbox("setValue", vf);
                txtControl.textbox("setText", tf);
                if (typeof (OnFieldSelect) == "function") { //触发字段选择事件
                    OnFieldSelect(rows, fieldName, valueField, textField);
                }
                if (typeof (OnFieldValueChanged) == "function" && oldValue != vf) { //触发字段值改变事件
                    var moduleId = GetLocalQueryString("moduleId");
                    OnFieldValueChanged({ moduleId: moduleId }, fieldName, vf, oldValue);
                }
            }
            else { //有自定义函数时调用自定义函数
                backFun(rows, valueField, textField);
            }
            top.closeDialog();
        }
    }, {
        text: '关闭',
        iconCls: "eu-icon-close",
        handler: function (e) {
            top.closeDialog();
        }
    }];
    var w = isTree ? 500 : 800;
    var h = isTree ? 510 : 430;
    top.openDialog(title, url, toolbar, w, h, 'eu-icon-search');
}

//弹出框选择模块数据
function SelectModuleData(moduleIdOrName, backFun) {
    if (!moduleIdOrName) {
        top.showMsg('提示', "模块Id和模块名称至少要传递一个！");
        return;
    }
    var url = null;
    var title = "选择数据";
    if (typeof (moduleIdOrName) == "string" && moduleIdOrName.length < 36) {
        url = "/Page/Grid.html?page=fdGrid&moduleName=" + encodeURI(moduleIdOrName);
        title = "选择" + moduleIdOrName;
    }
    else {
        url = "/Page/Grid.html?page=fdGrid&moduleId=" + moduleIdOrName;
    }
    url += "&r=" + Math.random(); //弹出框页面url
    var toolbar = [{
        text: "选择",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            var row = top.getCurrentDialogFrame()[0].contentWindow.GetSelectRowByGridIdPrefix("grid_");
            if (typeof (backFun) == "function") { //有自定义函数时调用自定义函数
                backFun(row);
            }
            top.closeDialog();
        }
    }, {
        text: '关闭',
        iconCls: "eu-icon-close",
        handler: function (e) {
            top.closeDialog();
        }
    }];
    top.openDialog(title, url, toolbar, 800, 430, 'eu-icon-search');
}

//选择模块树
//moduleIdOrName:模块Id或模块名称
//url:自定义数据加载url
//isMutiSelect:是否多选
//backFun:回调函数
function SelectModuleTree(moduleIdOrName, url, isMutiSelect, backFun) {
    if (!moduleIdOrName) {
        top.showMsg('提示', "模块Id和模块名称至少要传递一个！");
        return;
    }
    var dataUrl = url;
    var title = "选择数据";
    if (typeof (moduleIdOrName) == "string" && moduleIdOrName.length < 36) {
        if (!dataUrl || dataUrl.length == 0) {
            dataUrl = "/Page/DialogTree.html?moduleName=" + encodeURI(moduleIdOrName);
        }
        title = "选择" + moduleIdOrName;
    }
    else {
        if (!dataUrl || dataUrl.length == 0) {
            dataUrl = "/Page/DialogTree.html?moduleId=" + moduleIdOrName;
        }
    }
    if (isMutiSelect) dataUrl += "&ms=1";
    dataUrl += "&r=" + Math.random(); //弹出框页面url
    var toolbar = [{
        text: "选择",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            var row = top.getCurrentDialogFrame()[0].contentWindow.GetSelectData();
            if (isMutiSelect) {
                if (row.length == 0) {
                    top.showMsg('提示', '请至少选择一项！');
                    return;
                }
            }
            else {
                if (row == null) {
                    top.showMsg('提示', '请选择一项！');
                    return;
                }
            }
            if (typeof (backFun) == "function") { //有自定义函数时调用自定义函数
                backFun(row);
            }
            top.closeDialog();
        }
    }, {
        text: '关闭',
        iconCls: "eu-icon-close",
        handler: function (e) {
            top.closeDialog();
        }
    }];
    top.openDialog(title, dataUrl, toolbar, 500, 510, 'eu-icon-search');
}

//图标控件的选择图标事件
function SelectIcon(obj) {
    var toolbar = [{
        id: 'btnOk',
        text: "确 定",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            var iconInfo = top.getCurrentDialogFrame()[0].contentWindow.GetSelectedIcon();
            if (iconInfo) { //获取到了图标信息
                var controlId = $(obj).attr("iconControlId");
                $("#" + controlId).val(iconInfo.StyleClassName);
                $("#img_" + controlId).attr("src", iconInfo.ImgUrl);
                top.closeDialog();
            }
        }
    }, {
        id: 'btnClose',
        text: '关 闭',
        iconCls: "eu-icon-close",
        handler: function (e) {
            top.closeDialog();
        }
    }];
    var url = '/Page/IconSelect.html?&r=' + Math.random();
    top.openDialog('选择图标', url, toolbar, 900, 560, 'eu-icon-grid');
}

//图片上传控件的选择图片事件
function SelectImage(obj) {
    var controlId = $(obj).attr("imgControlId");
    var tempFun = function (imgName) {
        var iframe = $("#iframe_" + controlId); //当前图片上传控件的iframe
        //开始上传到临时目录
        iframe[0].contentWindow.UploadImage(function () { //上传前
            $("#a_" + controlId).linkbutton('disable');
        }, function (result) { //上传完成后
            if (result && result.Success) {
                var filePath = result.FilePath;
                $("#img_" + controlId).attr("src", filePath);
                $("#" + controlId).val(filePath);
            }
            $("#a_" + controlId).linkbutton('enable');
        }, imgName);
    }
    //自定义验证方法
    if (typeof (ImgUploadControlVerify) == "function") {
        ImgUploadControlVerify(controlId, function (imgName) { //验证通过后可返回自定义图片文件名执行上传
            tempFun(imgName);
        });
    }
    else {
        tempFun();
    }
}

//图片上传控件iframe加载完成
//fieldName:当前字段名
function ImgUploadIframeLoaded(fieldName) {
    $("#a_" + fieldName).linkbutton('enable');
}

//调整网格大小
//gridId:网格domId
//w:调整后宽
//h:调整后高
function ResizeGrid(gridId, w, h) {
    var width = parseInt(w);
    var height = parseInt(h);
    if (gridId && (width > 0 || height > 0)) {
        var gridObj = $("#" + gridId);
        var obj = null;
        if (width > 0 && height > 0) {
            obj = { width: width, height: height };
        }
        else if (width > 0) {
            obj = { width: width };
        }
        else {
            obj = { height: height };
        }
        gridObj.datagrid("resize", obj);
    }
}

//json对象绑定到form表单中
//json:JSON数据
//form:表单对象
//formFields:表单字段对象集合
function JsonToForm(json, form, formFields) {
    if (!json || !formFields || !form) return;
    $.each(json, function (key, value) {
        var field = null;
        $.each(formFields, function (i, obj) {
            if (obj.Sys_FieldName == key) {
                field = obj;
                return;
            }
        });
        if (field != null) {
            var controlType = field.ControlType; //字段控件类型
            if (controlType == 0 || controlType == 12 || controlType == 15 || controlType == 100) { //文本框或文本域或密码框或标签框
                $("#" + key, form).textbox('setValue', value);
            }
            else if (controlType == 1 || controlType == 2) { //单选checkbox或多选checkbox
                $("#" + key, form).attr("checked", value);
                $("#" + key, form).attr("value", value ? 1 : 0);
            }
            else if (controlType == 3) { //下拉列表
                $("#" + key, form).combobox('setValue', value);
            }
            else if (controlType == 6 || controlType == 7) { //浮点数值或整形数值
                $("#" + key, form).numberbox('setValue', value);
            }
            else if (controlType == 8) { //外键弹出列表框
                if (key.substr(key.length - 2) == "Id") {
                    $("#" + key, form).textbox('setValue', value);
                }
                else {
                    var controlName = key.substr(0, key.length - 4) + "Id";
                    $("#" + controlName, form).textbox('setText', value);
                }
            }
            else if (controlType == 9) { //单选框组
            }
            else if (controlType == 10) { //日期框
                $("#" + key, form).datebox('setValue', value);
            }
            else if (controlType == 11) { //日期时间框
                $("#" + key, form).datetimebox('setValue', value);
            }
            else if (controlType == 13) { //富文本框
            }
            else if (controlType == 14) { //文本框列表
            }
        }
    });
}

//绑定字段值
//formField:表单字段
//value:value值
//text:text值
//dom:控件，为空时根据字段名称取dom
function BindFieldValue(formField, value, text, dom) {
    if (field != null) {
        var fieldName = formField.Sys_FieldName;
        var tempDom = dom ? dom : $("#" + fieldName);
        if (formField.ForeignModuleName) {
            var controlName = fieldName.substr(0, fieldName.length - 4) + "Id";
            tempDom = $("#" + controlName);
        }
        var controlType = field.ControlType; //字段控件类型
        if (controlType == 0 || controlType == 12 || controlType == 15 || controlType == 100) { //文本框或文本域或密码框或标签框
            tempDom.textbox('setValue', value);
        }
        else if (controlType == 1 || controlType == 2) { //单选checkbox或多选checkbox
            tempDom.attr("checked", value).attr("value", value ? 1 : 0);
        }
        else if (controlType == 3) { //下拉列表
            tempDom.combobox('setValue', value);
        }
        else if (controlType == 6 || controlType == 7) { //浮点数值或整形数值
            tempDom.numberbox('setValue', value);
        }
        else if (controlType == 8) { //外键弹出列表框
            tempDom.textbox('setValue', value);
            tempDom.textbox('setText', value);
        }
        else if (controlType == 9) { //单选框组
        }
        else if (controlType == 10) { //日期框
            tempDom.datebox('setValue', value);
        }
        else if (controlType == 11) { //日期时间框
            tempDom.datetimebox('setValue', value);
        }
        else if (controlType == 13) { //富文本框
        }
        else if (controlType == 14) { //文本框列表
        }
    }
    else {
        dom.val(value);
    }
}

//获取当前表单字段
function GetFormFields() {
    try {
        var formFields = eval("(" + decodeURIComponent($("#hd_formFields").val()) + ")");
        return formFields;
    } catch (err) { }
    return null;
}

//获取字段的表单字段信息
//fieldName:字段名
function GetFormField(fieldName) {
    var formFields = GetFormFields();
    if (formFields != null) {
        var field = null;
        $.each(formFields, function (i, obj) {
            if (obj.Sys_FieldName == fieldName) {
                field = obj;
                return;
            }
        });
        return field;
    }
    return null;
}

//获取titleKey字段
function GetTitleKeyField() {
    var formFields = GetFormFields();
    if (formFields != null) {
        var field = null;
        $.each(formFields, function (i, obj) {
            if (obj.IsTitleKey == true) {
                field = obj;
                return;
            }
        });
        return field;
    }
    return null;
}

//编辑字段值
//obj：控件对象
function EditField(obj) {
    //有自定义字段编辑方法则先调用自定义否则调用通用
    if (typeof (OverEditField) == "function") {
        OverEditField(obj);
        return;
    }
    var moduleId = $(obj).attr("moduleId"); //模块Id
    var fieldName = $(obj).attr("fieldName"); //字段名称
    var recordId = $(obj).attr("recordId"); //记录Id
    var fieldDisplay = $(obj).attr("fieldDisplay"); //字段显示名称
    var fieldWidth = parseInt($(obj).attr("fieldWidth")) + 100; //字段宽度
    var oldValue = $(obj).attr("oldValue"); //原始字段值
    if (fieldWidth < 400) fieldWidth = 400;
    var toolbar = [{
        id: 'btnOk',
        text: "确 定",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            var fieldValue = top.getCurrentDialogFrame()[0].contentWindow.GetUpdateFieldValue();
            if (fieldValue == oldValue) {
                top.closeDialog();
                return;
            }
            var msgTitle = "字段更新提示";
            $.ajax({
                type: 'post',
                url: '/' + CommonController.Async_Data_Controller + '/UpdateField.html',
                data: { moduleId: moduleId, recordId: recordId, fieldName: fieldName, fieldValue: fieldValue },
                dataType: "json",
                beforeSend: function () {
                    top.openWaitDialog('处理中...');
                },
                success: function (result) {
                    if (result.Success) {
                        top.closeDialog();
                        top.showMsg(msgTitle, '字段更新成功！', function () {
                            top.closeWaitDialog();
                            var gridId = $(obj).attr("gridId");
                            if (gridId) { //网格页面
                                RefreshGrid(gridId); //刷新网格
                            }
                            else { //查看页面
                                $("#span_" + fieldName).text(result.FieldDisplayValue);
                            }
                        });
                    }
                    else {
                        top.showAlertMsg(msgTitle, result.Message, "info", function () {
                            top.closeWaitDialog();
                        });
                    }
                },
                error: function (err) {
                    top.showAlertMsg(msgTitle, "字段更新失败，服务端异常！", "error", function () {
                        top.closeWaitDialog();
                    });
                }
            });
        }
    }, {
        id: 'btnClose',
        text: '关 闭',
        iconCls: "eu-icon-close",
        handler: function (e) {
            top.closeDialog();
        }
    }];
    var url = '/Page/EditField.html?page=editField&moduleId=' + moduleId + '&fieldName=' + fieldName + '&recordId=' + recordId + '&r=' + Math.random();
    top.openDialog('编辑字段－' + fieldDisplay, url, toolbar, fieldWidth, 280, 'eu-icon-docEdit');
}

//字段绑定自动完成功能
//dom:绑定自动完成功能的输入控件dom
//moduleId:模块Id
//fieldName:需要显示、提示的字段名
//displayTemplate:显示模板html
//backFun:选择值后的回调函数
//paramObj：参数对象，可选参数
//urlParams:额外的url参数
/*
displayTemplate模板使用说明：
如：1.字段格式为{UserName},UserName字段为当前表字段
    <a href="#">{UserName}</a>
    2.字段格式为{用户管理.UserName},UserName字段为外键模块表字段，前面必须带上模块名称
    <a href="#">{用户管理.UserName}--{Des}</a>
*/
function FieldBindAutoCompleted(dom, moduleId, fieldName, displayTemplate, backFun, paramObj, urlParams) {
    dom.focus(function () {
        if (dom.attr("smartTips")) return;
        dom.attr("smartTips", "true"); //添加标识防止多次添加
        var autoUrl = "/" + CommonController.Async_Data_Controller + "/AutoComplete.html?fieldName=" + fieldName + "&moduleId=" + moduleId;
        if (urlParams && urlParams.length > 0) { //额外参数
            if (urlParams.indexOf("&") != 0) {
                urlParams = "&" + urlParams;
            }
            autoUrl += urlParams;
        }
        if (displayTemplate && displayTemplate.length > 0) { //显示模板
            autoUrl += "&template=" + escape(displayTemplate);
        }
        autoUrl += "&r=" + Math.random();
        AutoComplete(dom, autoUrl, function (item, dom, tempParamObj) {
            if (item == null) return;
            if (typeof (backFun) == "function") {
                backFun(dom, item, fieldName, tempParamObj);
            }
        }, false, paramObj);
    });
}

//字段绑定自动完成功能
//dom:绑定自动完成功能的输入控件dom
//moduleName:模块名称
//fieldName:需要显示、提示的字段名
//displayTemplate:显示模板html
//backFun:选择值后的回调函数
//paramObj：参数对象，可选参数
//urlParams:额外的url参数
function FieldBindAutoCompletedByModuleName(dom, moduleName, fieldName, displayTemplate, backFun, paramObj, urlParams) {
    dom.focus(function () {
        if (dom.attr("smartTips")) return;
        dom.attr("smartTips", "true"); //添加标识防止多次添加
        var autoUrl = "/" + CommonController.Async_Data_Controller + "/AutoComplete.html?fieldName=" + fieldName + "&moduleName=" + escape(moduleName);
        if (urlParams && urlParams.length > 0) { //额外参数
            if (urlParams.indexOf("&") != 0) {
                urlParams = "&" + urlParams;
            }
            autoUrl += urlParams;
        }
        if (displayTemplate && displayTemplate.length > 0) { //显示模板
            autoUrl += "&template=" + escape(displayTemplate);
        }
        autoUrl += "&r=" + Math.random();
        AutoComplete(dom, autoUrl, function (item, dom, tempParamObj) {
            if (item == null) return;
            if (typeof (backFun) == "function") {
                backFun(dom, item, fieldName, tempParamObj);
            }
        }, true, paramObj);
    });
}

//执行模块通用删除
//moduleIdOrName:模块id或模块名称
//ids:要删除的记录id,多个以逗号分隔,如，100,101,102
//isFromRecycle:是否来自回收站
//isHardDel:是否硬删除
//backFun:回调函数
function ExecuteCommonDelete(moduleIdOrName, ids, isFromRecycle, isHardDel, backFun) {
    var delUrl = "/" + CommonController.Async_Data_Controller + "/Delete.html?r=" + Math.random();
    if (isFromRecycle) { //来自回收站
        delUrl += "&recycle=1";
    }
    if (isHardDel) { //确认硬删除
        delUrl += "&isHardDel=1";
    }
    var msgTitle = "删除提示";
    var params = typeof (moduleIdOrName) == "string" && moduleIdOrName.length < 36 ? { moduleName: decodeURI(moduleIdOrName), ids: ids } :
                 { moduleId: moduleIdOrName, ids: ids };
    $.ajax({
        type: "post",
        url: delUrl,
        data: params,
        beforeSend: function () {
            top.openWaitDialog('正在删除...');
        },
        success: function (result) {
            if (result.Success) {
                top.showMsg(msgTitle, '数据删除成功！', function () {
                    top.closeWaitDialog();
                    if (typeof (backFun) == "function") {
                        backFun();
                    }
                });
            }
            else {
                top.showAlertMsg(msgTitle, result.Message, "info", function () {
                    top.closeWaitDialog();
                });
            }
        },
        error: function (err) {
            top.showAlertMsg(msgTitle, "操作失败，服务端异常！", "error", function () {
                top.closeWaitDialog();
            });
        },
        dataType: "json"
    });
}

//执行模块通用还原
//moduleIdOrName:模块id或模块名称
//ids:要删除的记录id,多个以逗号分隔,如，100,101,102
//backFun:回调函数
function ExecuteCommonRestore(moduleIdOrName, ids, backFun) {
    var msgTitle = "还原提示";
    var params = typeof (moduleIdOrName) == "string" && moduleIdOrName.length < 36 ? { moduleName: decodeURI(moduleIdOrName), ids: ids } :
                 { moduleId: moduleIdOrName, ids: ids };
    $.ajax({
        type: "post",
        url: "/" + CommonController.Async_Data_Controller + "/Restore.html?r=" + Math.random(),
        data: params,
        beforeSend: function () {
            top.openWaitDialog('正在还原...');
        },
        success: function (result) {
            if (result.Success) {
                top.showMsg(msgTitle, '数据还原成功！', function () {
                    top.closeWaitDialog();
                    if (typeof (backFun) == "function") {
                        backFun();
                    }
                });
            }
            else {
                top.showAlertMsg(msgTitle, result.Message, "info", function () {
                    top.closeWaitDialog();
                });
            }
        },
        error: function (err) {
            top.showAlertMsg(msgTitle, "操作失败，服务端异常！", "error", function () {
                top.closeWaitDialog();
            });
        },
        dataType: "json"
    });
}

//前一记录
function PreRecord(obj) {

}

//下一记录
function NextRecord(obj) {
}

/*-----------tabs操作相关-----------------------------------------------*/
//增加Tab，并触发onLoad回调函数
//dom:tab对应的dom，为空是为top.$("#tabs");
//title:标题
//url:url
//icon:图标
//onloadFun:加载完成后的回调函数
//repeatTitle:是否可重复title
//backFun:回调函数
function AddTab(dom, title, url, icon, onloadFun, repeatTitle, backFun) {
    var tt = dom || top.$("#tabs");
    var isExist = repeatTitle ? false : tt.tabs('exists', title);
    if (!isExist) {
        var optionParams = {
            title: title,
            border: false,
            content: CreateIFrame(url),
            closable: true,
            selected: true,
            icon: icon,
            loadingMessage: '正在加载中......',
            onClose: function (title, index) { //tab关闭后事件
                if (typeof (OverOnCloseTab) == "function")
                    OverOnCloseTab(title, index);
            },
            onBeforeClose: function () { //tab关闭前事件
                if (typeof (OverOnBeforeCloseTab) == "function")
                    return OverOnBeforeCloseTab();
                return true;
            }
        };
        if (typeof (onloadFun) == "function") {
            optionParams.onLoad = onloadFun;
        }
        tt.tabs('add', optionParams);
        if (typeof (backFun) == "function") {
            backFun();
        }
    }
    else {
        tt.tabs('select', title);
    }
}

//关闭当前选中tab
//dom:tab对应的dom
function CloseTab(dom) {
    var tt = dom || top.$("#tabs");
    var index = tt.tabs('getTabIndex', tt.tabs('getSelected'));
    CloseTabByIndex(dom, index);
}

//关闭某个tab
//dom:tab对应的dom
function CloseTabItem(dom, tab) {
    var tt = dom || top.$("#tabs");
    var index = tt.tabs('getTabIndex', tab);
    CloseTabByIndex(dom, index);
}

//关闭tab
//dom:tab对应的dom
function CloseTabByIndex(dom, index) {
    var tt = dom || top.$("#tabs");
    if (index > 0) {
        tt.tabs('close', index);
    }
}

//关闭tab
//dom:tab对应的dom
function CloseTabByTitle(dom, title) {
    var tt = dom || top.$("#tabs");
    var tab = tt.tabs('getTab', title);
    CloseTabItem(dom, tab);
}

//选中tab
//dom:tab对应的dom
//title:tab标题
function SelectTab(dom, title) {
    var tt = dom || top.$("#tabs");
    tt.tabs('select', title);
}

//刷新tab
//dom:tab对应的dom
//url:新的链接地址
function RefreshTab(dom, url) {
    UpdateSelectedTab(dom, url);
}

//获取选中的tab
//dom:tab对应的dom
function GetSelectedTab(dom) {
    var tt = dom || top.$("#tabs");
    var tab = tt.tabs('getSelected');
    return tab;
}

//获取tabindex
//dom:tab对应的dom
//tab:标签对象
function GetTabIndex(dom, tab) {
    var tt = dom || top.$("#tabs");
    var tabIndex = tt.tabs('getTabIndex', tab);
    return tabIndex;
}

//dom:tab对应的dom
function GetSelectTabIndex(dom) {
    var tab = GetSelectedTab(dom);
    if (tab)
        return GetTabIndex(dom, tab);
    return 0;
}

//获取tab
//dom:tab对应的dom
//tabIndexOrTitle:tab索引或tab标题
function GetTab(dom, tabIndexOrTitle) {
    var tt = dom || top.$("#tabs");
    var tab = tt.tabs('getTab', tabIndexOrTitle);
    return tab;
}

//更新tab
//dom:tab对应的dom
//tab:要更新的tab对象
//url:tab的url
//title:新的标题
function UpdateTab(dom, tab, url, title) {
    var tt = dom || top.$("#tabs");
    var params = {};
    if (url) params.content = CreateIFrame(url);
    if (title) params.title = title;
    tt.tabs('update', {
        tab: tab,
        options: params
    });
}

//更新当前选中tab
//dom:tab对应的dom
//url:tab的url
//title:新的标题
function UpdateSelectedTab(dom, url, title) {
    var tt = dom || top.$("#tabs");
    var tab = tt.tabs('getSelected');
    var params = {};
    if (url) params.content = CreateIFrame(url);
    if (title) params.title = title;
    tt.tabs('update', {
        tab: tab,
        options: params
    });
}

/*----------------------------------------------------------------------*/
//执行ajax方法操作
//url:url
//params:url对应的参数
//successBakFun:操作成功后的回调函数
//isShowTip:是否显示提示
//isGet:是否get方式获取
function ExecuteCommonAjax(url, params, successBakFun, isShowTip, isGet) {
    var msgTitle = '操作提示';
    var method = isGet ? "get" : "post";
    $.ajax({
        type: method,
        url: url,
        data: params,
        dataType: "json",
        beforeSend: function () {
            if (isShowTip) {
                top.openWaitDialog('拼命处理中...');
            }
        },
        success: function (result) {
            if (isShowTip) {
                if (result.Success) {
                    top.showMsg(msgTitle, '操作成功！', function () {
                        top.closeWaitDialog();
                        if (typeof (successBakFun) == "function")
                            successBakFun(result);
                    });
                }
                else {
                    top.showAlertMsg(msgTitle, result.Message, "info", function () {
                        top.closeWaitDialog();
                    });
                }
            }
            else {
                if (typeof (successBakFun) == "function")
                    successBakFun(result);
            }
        },
        error: function (err) {
            if (isShowTip) {
                top.showAlertMsg(msgTitle, "操作失败，服务端异常！", "error", function () {
                    top.closeWaitDialog();
                });
            }
        }
    });
}