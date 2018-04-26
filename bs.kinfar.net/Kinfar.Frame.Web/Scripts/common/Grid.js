var page = GetLocalQueryString("page");
var searchParams = null; //当前搜索条件

$(function () {
    //列表左边树的top、left、bottom边框去掉
    $("#region_west").css('border-left-width', '0px')
                     .css('border-bottom-width', '0px');
    $("#region_west").prev().css('border-top-width', '0px')
                     .css('border-left-width', '0px');
    var color = top.getBorderColor();
    $("#regon_main .datagrid").css('border-right-color', color)
                    .css('border-right-style', 'solid')
                    .css('border-right-width', '1px');
    if ($("div[id^='region_south']").length > 0) { //有明细或附属模块
        $("#regon_main .datagrid")
                    .css('border-bottom-color', color)
                    .css('border-bottom-style', 'solid')
                    .css('border-bottom-width', '1px')
    }
    $('#detailTabs').css('border-top-color', color)
                    .css('border-top-style', 'solid')
                    .css('border-top-width', '1px')
    //搜索框智能提示绑定
    setTimeout(function () {
        $("input[id^='txtSearch']").each(function () {
            var domId = $(this).attr("id");
            var tempModuleId = $(this).attr("moduleId");
            var fieldName = $(this).searchbox("getName"); //搜索字段
            var tempDom = $(this).searchbox("textbox");
            FieldBindAutoCompleted(tempDom, tempModuleId, fieldName, null, function (dom, item) {
                var text = item["text"];
                var searchDom = $("#" + domId);
                searchDom.searchbox("setValue", text);
                SimpleSearch(searchDom, text, fieldName);
            });
        });
    }, 100);
});

//简单搜索
//obj:当前搜索dom对象
//value:输入值
//name:搜索字段名
function SimpleSearch(obj, value, name) {
    var treeDom = $("#gridTree"); //左侧树dom对象
    var o = {}; //搜索对象
    if (value) {
        o[name] = value;
    }
    var s = JSON.stringify(o);
    var params = { q: s };
    if (treeDom && treeDom.length > 0) { //存在左侧树
        //树结点字段
        var treeField = treeDom.attr("treeField");
        var node = treeDom.tree("getSelected");
        if (node && node.id != -1) {
            var oo = {};
            //oo[treeField] = node.id > 0 ? node.id : node.text;
            oo[treeField] = node.id === "undefined" ? node.text : node.id;
            params["condition"] = JSON.stringify(oo);
        }
    }
    var gridId = $(obj).attr("gridId");
    $("#" + gridId).datagrid("reload", params);
}

//高级搜索
function AdvanceSearch(obj) {
    var toolbar = [{
        text: "搜 索",
        iconCls: "eu-icon-search",
        handler: function (e) {
            var searchParam = top.getCurrentDialogFrame()[0].contentWindow.GetSearchParam();
            var s = JSON.stringify(searchParam);
            var params = { q: s };
            searchParams = params;
            var gridId = $(obj).attr("gridId");
            $("#" + gridId).datagrid("reload", params);
        }
    }, {
        text: '关 闭',
        iconCls: "eu-icon-close",
        handler: function (e) {
            top.closeDialog();
        }
    }];
    var moduleId = $(obj).attr("moduleId");
    var moduleName = $(obj).attr("moduleName");
    var viewId = $(obj).attr("viewId"); //视图Id
    var url = '/Page/AdvanceSearch.html?moduleId=' + moduleId + '&viewId=' + viewId;
    top.openDialog('高级搜索－' + moduleName, url, toolbar, 400, 380, 'eu-icon-search');
}

//列表视图设置
function GridSet(obj) {
    var toolbar = [{
        id: 'btnOk',
        text: "确 定",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            top.closeDialog();
            var moduleId = $(obj).attr("moduleId");
            var gridId = $(obj).attr("gridId");
            var viewObj = top.getCurrentDialogFrame()[0].contentWindow.GetSelectGridView();
            var treeField = $("#btn_gridSet" + moduleId).attr("treeField");
            if (viewObj.TreeField == treeField) { //没有产生树显示字段变化时只切换列头
                ChangeGridView(moduleId, gridId, viewObj.ViewId); //切换视图
            }
            else { //树显示字段变化需要刷新整个页面
                var url = "/Page/Grid.html?moduleId=" + moduleId + "&viewId=" + viewObj.ViewId + "&r=" + Math.random();
                var iframe = $("iframe", GetSelectedTab());
                iframe.attr("src", url);
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
    var moduleId = $(obj).attr("moduleId");
    var moduleName = $(obj).attr("moduleName");
    var viewId = $(obj).attr("viewId");
    var url = '/Page/GridSet.html?moduleId=' + moduleId + '&viewId=' + viewId + '&r=' + Math.random();
    top.openDialog('选择视图－' + moduleName, url, toolbar, 415, 250, 'eu-icon-grid');
}

//附属模块设置
//obj:按钮dom对象
function AttachModuleSet(obj) {
    var moduleId = $(obj).attr('moduleId');
    var toolbar = [{
        id: 'btnOk',
        text: '确 定',
        iconCls: 'eu-icon-ok',
        handler: function (e) {
            var attachModuleInfos = top.getCurrentDialogFrame()[0].contentWindow.GetEnabledAttachModules();
            var json = escape(JSON.stringify(attachModuleInfos));
            $.ajax({
                type: 'post',
                url: '/' + CommonController.Async_System_Controller + '/SaveUserAttachModuleSet.html',
                data: { moduleId: moduleId, attachModuleInfo: json },
                dataType: 'json',
                beforeSend: function () {
                    top.openWaitDialog('数据保存中...');
                },
                success: function (result) {
                    top.closeWaitDialog();
                    if (result && result.Success) {
                        top.closeDialog();
                        //window.location.reload();
                    }
                    else {
                        top.showAlertMsg('提示', result.Message, 'info');
                    }
                },
                error: function (err) {
                    top.closeWaitDialog();
                    top.showAlertMsg('提示', '附属模块显示设置保存失败，服务端异常！', 'error');
                }
            });
        }
    }, {
        id: 'btnClose',
        text: '关 闭',
        iconCls: 'eu-icon-close',
        handler: function (e) {
            top.closeDialog();
        }
    }];
    var url = '/Page/AttachModuleSet.html?moduleId=' + moduleId + '&r=' + Math.random();
    top.openDialog('附属模块显示设置', url, toolbar, 600, 500, 'eu-icon-calendar');
}

//刷新网格
//gridId:网格Id
function RefreshGrid(gridId) {
    if (gridId && gridId.length > 0) {
        $("#" + gridId).datagrid("reload");
    }
    else {
        $("#mainGrid").datagrid("reload");
    }
}

//重新加载数据
//gridId:网格domId
//url:网格数据加载url
function ReloadData(gridId, url) {
    var gridDom = gridId && gridId.length > 0 ? $("#" + gridId) : $("#mainGrid");
    var options = gridDom.datagrid("options");
    options.url = url;
    gridDom.datagrid(options);
}

//获取选择的单条记录，针对主网格
//gridId:网格domId
function GetSelectRow(gridId) {
    if (gridId) {
        var row = $("#" + gridId).datagrid("getSelected");
        return row;
    }
    else {
        var row = $("#mainGrid").datagrid("getSelected");
        return row;
    }
    return null;
}

//获取选择的单条记录
//gridIdPrefix:网格domId前缀
function GetSelectRowByGridIdPrefix(gridIdPrefix) {
    var row = $("table[id^='" + gridIdPrefix + "']").datagrid("getSelected");
    return row;
}

//获取选中行的TitleKey的值
//gridId:网格domId
function GetSelectRowTitleKeyValue(gridId) {
    var regonObj = gridId && gridId != 'mainGrid' ? $("div[id^='regon_'][id!='regon_main']") : $("#regon_main");
    var titleKey = regonObj.attr("titleKey");
    if (titleKey) {
        var tableName = regonObj.attr("tableName");
        var row = GetSelectRow(gridId);
        if (row) {
            return { name: titleKey, value: row[titleKey], foreignFieldName: tableName + "Id", recordId: row["Id"] };
            return row[titleKey];
        }
    }
    return null;
}

//获取所有选择的记录，针对主网格
//gridId:网格domId
function GetSelectRows(gridId) {
    if (gridId) {
        var rows = $("#" + gridId).datagrid("getSelections");
        return rows;
    }
    else {
        var rows = $("#mainGrid").datagrid("getSelections");
        return rows;
    }
    return null;
}

//获取所有选择的记录
//gridIdPrefix:网格domId前缀
function GetSelectRowsByGridIdPrefix(gridIdPrefix) {
    var rows = $("table[id^='" + gridIdPrefix + "']").datagrid("getSelections");
    return rows;
}

//获取当前页所有行
//gridId:网格domId
function GetCurrentRows(gridId) {
    if (gridId) {
        var rows = $("#" + gridId).datagrid("getRows");
        return rows;
    }
    else {
        return $("#mainGrid").datagrid("getRows");
    }
}

//获取网格的行id前缀
//如：datagrid-row-r2-1-rowIndex中的‘datagrid-row-r2-1-’
//parentDom:网格所属父dom对象
function GetGridRowIdPrefix(parentDom) {
    var tr = parentDom && parentDom.length > 0 ?
        $("div.datagrid-view2 div.datagrid-body tr.datagrid-row", parentDom).eq(0) :
        $("div.datagrid-view2 div.datagrid-body tr.datagrid-row").eq(0);
    var tempId = tr.attr("id");
    var startIndex = tempId.lastIndexOf("-");
    return tempId.substr(0, startIndex + 1);
}

//获取网格选择行的字段文本显示值
function GetSelectRowDisplayValue(gridId) {
    var rowIndex = GetSelectRowIndex(gridId);
    var rowIdPrefix = GetGridRowIdPrefix();
    var tdDom = $("#" + rowIdPrefix + rowIndex + " td");
    var row = {};
    $.each(tdDom, function () {
        var fieldName = $(this).attr("field");
        var textValue = null;
        if (fieldName) {
            var span = $("span", $(this));
            if (span.length == 0) {
                var div = $("div", $(this));
                textValue = div.text();
            }
            else {
                textValue = span.text();
            }
            row[fieldName] = textValue;
        }
    });
    return row;
}

//获取选中行的行号
//gridId:网格domId
function GetSelectRowIndex(gridId) {
    var gridObj = $("#" + gridId);
    var row = gridObj.datagrid("getSelected");
    var rowIndex = gridObj.datagrid("getRowIndex", row);
    return rowIndex;
}

//根据记录Id取行号
//gridId:网格domId
//recordId:记录Id
function GetRowIndexByRecordId(gridId, recordId) {
    var gridObj = $("#" + gridId);
    var rows = gridObj.datagrid("getRows");
    for (var i = 0; i < rows.length; i++) {
        var tempRow = rows[i];
        if (recordId == tempRow["Id"]) {
            var rowIndex = gridObj.datagrid("getRowIndex", tempRow);
            return rowIndex;
        }
    }
    return -1;
}

//获取行号
//gridId:网格domId
//row:行对象
function GetRowIndexByRow(gridId, row) {
    var gridObj = $("#" + gridId);
    var rowIndex = gridObj.datagrid("getRowIndex", row);
    return rowIndex;
}

//获取网格编辑器中字段的控件
//rowIndex:行号
//fieldName:字段名
//parentDom:网格所属父dom对象
function GetGridEditorControl(rowIndex, fieldName, parentDom) {
    var idPrefix = GetGridRowIdPrefix(parentDom);
    var dom = $("#" + idPrefix + rowIndex + " td[field='" + fieldName + "']");
    var control = dom.find('input.datagrid-editable-input');
    if (control.length == 0) {
        control = dom.find("td input");
    }
    return control;
}

//获取网格编辑器中字段的控件
//gridId:网格Id
//rowIndex:行号
//fieldName:字段名
function GetGridEditorControl2(gridId, rowIndex, fieldName) {
    var tmpGridId = gridId ? gridId : 'mainGrid';
    var obj = $($('#' + tmpGridId).datagrid('getEditor', { index: rowIndex, field: fieldName }).target);
    return obj;
}

//获取网格编辑器中td
//rowIndex:行号
//fieldName:字段名
//parentDom:网格所属父dom对象
function GetGridEditorTdCell(rowIndex, fieldName, parentDom) {
    var idPrefix = GetGridRowIdPrefix(parentDom);
    var control = $("#" + idPrefix + rowIndex + " td[field='" + fieldName + "']");
    return control;
}

//启用单元格字段编辑
//gridId:网格domId
function EnableCellEdit(gridId) {
    var tempGridId = gridId && gridId.length > 0 ? gridId : 'mainGrid';
    $("#" + tempGridId).datagrid('enableCellEditing').datagrid('gotoCell', {
        index: 0,
        field: 'Id'
    });
}

//获取当前主网格选中记录的titlekey字段值，主网格下方明细或附属模块新增时用到
function GetTitleKeyValue() {
    var row = GetSelectRow();
    var forignFieldName = $('#regon_main').attr('tableName') + "Id";
    return { foreignFieldName: forignFieldName, recordId: row["Id"] };
}

//新增，兼容明细新增
//copyId:复制时被复制记录Id
function Add(obj, copyId) {
    var tempModuleId = $(obj).attr("moduleId");
    var gridId = $(obj).attr('gridId');
    var moduleName = $(obj).attr("moduleName");
    var title = "新增" + moduleName;
    var formUrl = $(obj).attr('formUrl'); //自定义表单页面URL
    if (formUrl == undefined || formUrl == null || formUrl.length == 0) { //通用新增页面
        var editMode = parseInt($(obj).attr("editMode")); //编辑模式
        var editPageUrl = "/Page/EditForm.html?page=add&moduleId=" + tempModuleId;
        var formId = $(obj).attr('formId'); //表单ID
        if (formId != undefined && formId != null && formId.length > 0)
            editPageUrl += "&formId=" + formId;
        editPageUrl += "&mode=" + editMode;
        editPageUrl += "&fp=grid";
        if (copyId) {
            editPageUrl += "&mode=" + editMode + "&copyId=" + copyId;
        }
        if (editMode == 2 && (page == "add" || page == "edit" || page == "view" ||
            (page == "grid" && $(obj).attr('gt') == '5'))) { //主从编辑页面或附属模块添加标识
            var parentMode = GetLocalQueryString("mode"); //父页面编辑模式
            editPageUrl += "&pmode=" + parentMode;
            editPageUrl += "&ff=1"; //标识来自表单页面
            if (page == "grid" && $(obj).attr('gt') == '5') { //从附属模块网格中单击新增时
                var row = GetSelectRow(); //获取当前主网格选中记录
                if (row == null) {
                    var mainModuleName = $('#regon_main').attr('moduleName');
                    top.showAlertMsg("提示", "请在主模块【" + mainModuleName + "】列表中选择一条记录！", "info"); //弹出提示信息
                    return;
                }
                editPageUrl += "&fg=1"; //标识来自附属网格中
            }
        }
        var currTabIndex = GetSelectTabIndex(); //当前grid网格页面的tabindex
        if (currTabIndex)
            editPageUrl += "&tb=" + currTabIndex;
        editPageUrl += "&r=" + Math.random();
        var gridObj = $("#" + gridId);
        //执行新增方法
        var ExecuteMethod = function () {
            //有自定义新增方法则先调用自定义否则调用通用
            if (typeof (OverAdd) == "function") {
                OverAdd(obj);
                return;
            }
            if (editMode == 2 || editMode == 4) { //弹出框编辑模式或网格编辑表单
                var editWidth = parseInt($(obj).attr("editWidth"));
                var editHeight = parseInt($(obj).attr("editHeight"));
                editWidth += 40; //加上padding相关
                editHeight += 35 + 50; //标题栏高度和按钮栏高度
                //加载弹出框操作按钮
                ExecuteCommonAjax('/' + CommonController.Async_System_Controller + '/LoadFormBtns.html', { moduleId: tempModuleId, formType: 0, editMode: editMode }, function (result) {
                    if (result && result.length > 0) {
                        var toolbar = result;
                        for (var i = 0; i < toolbar.length; i++) {
                            var tempHandler = toolbar[i].handler;
                            if (typeof (tempHandler) == 'string') {
                                toolbar[i].handler = eval('(' + tempHandler + ')');
                            }
                        }
                        top.openDialog(title, editPageUrl, toolbar, editWidth, editHeight, null, function (dialogDivId) {
                            setTimeout(function () {
                                var divBtnPar = top.$("#" + dialogDivId).parent();
                                var dgBtn = top.$("div.dialog-button a", divBtnPar);
                                if (page != "view") {
                                    dgBtn.attr("detail", toolbar[0].detail);
                                }
                                dgBtn.attr("editMode", editMode);
                                dgBtn.attr("moduleId", tempModuleId);
                                dgBtn.attr("moduleName", moduleName);
                                dgBtn.attr("gridId", gridId);
                            }, 50);
                        });
                    }
                }, false, true);
            }
            else if (editMode == 1) { //tab编辑模式
                AddTab(null, title, editPageUrl);
            }
            else if (editMode == 3) { //列表行编辑模式
                AddRow(gridId);
                InitRowOpBtns(gridId);
            }
        }
        if (!copyId) { //新增
            //先进行客户端和服务端验证
            GridOperateVerify(moduleName, "新增", null, function (errMsg) {
                if (errMsg && errMsg.length > 0) {
                    top.showAlertMsg("提示", errMsg, "info"); //弹出验证提示信息
                }
                else {
                    //调用通用方法
                    ExecuteMethod(); //执行新增
                }
            });
        }
        else { //复制
            ExecuteMethod(); //执行新增
        }
    }
    else { //自定义新增表单
        if (!copyId) { //新增
            //先进行客户端和服务端验证
            GridOperateVerify(moduleName, "新增", null, function (errMsg) {
                if (errMsg && errMsg.length > 0) {
                    top.showAlertMsg("提示", errMsg, "info"); //弹出验证提示信息
                }
                else {
                    AddTab(null, title, formUrl); //执行新增
                }
            });
        }
        else { //复制
            if (formUrl.indexOf('?') > -1) {
                formUrl += '&copyId=' + copyId;
            }
            else {
                formUrl += '?copyId=' + copyId;
            }
            AddTab(null, title, formUrl); //执行新增
        }
    }
}

//编辑，兼容明细编辑
function Edit(obj) {
    var tempModuleId = $(obj).attr("moduleId");
    var gridId = $(obj).attr('gridId');
    var gridObj = $("#" + gridId);
    var editMode = parseInt($(obj).attr("editMode")); //编辑模式
    var selectId = $(obj).attr("recordId"); //要编辑的记录Id
    var row = GetSelectRow(gridId); //获取选中行
    if (!row) { //没有选中行，从当前按钮中找对应的记录Id来得到选择行
        if (selectId) {
            var rows = gridObj.datagrid("getRows");
            for (var i = 0; i < rows.length; i++) {
                var tempRow = rows[i];
                if (selectId == tempRow["Id"]) {
                    row = tempRow;
                    break;
                }
            }
        }
    }
    if (!row) {
        top.showAlertMsg("提示", "请选择一条记录！", "info"); //弹出提示信息
        return;
    }
    if (selectId == undefined || !selectId)
        selectId = row["Id"];
    var moduleName = $(obj).attr("moduleName");
    var titleKey = $(obj).attr("titleKey"); //标记字段名
    var title = "编辑" + moduleName;
    if (titleKey) {
        title = title + "－" + row[titleKey];
    }
    var formUrl = $(obj).attr('formUrl'); //自定义表单页面URL
    if (formUrl == undefined || formUrl == null || formUrl.length == 0) { //通用新增页面
        var editPageUrl = "/Page/EditForm.html?page=edit&moduleId=" + tempModuleId;
        var formId = $(obj).attr('formId'); //表单ID
        if (formId != undefined && formId != null && formId.length > 0)
            editPageUrl += "&formId=" + formId;
        editPageUrl += "&tip=0";
        editPageUrl += "&mode=" + editMode;
        editPageUrl += "&fp=grid";
        if (editMode == 2 && (page == "add" || page == "edit" || page == "view")) { //主从编辑页面添加标识
            var parentMode = GetLocalQueryString("mode"); //父页面编辑模式
            editPageUrl += "&pmode=" + parentMode;
            if (page == "add" || page == "edit") {
                editPageUrl += "&ff=1"; //标识来自表单页面
            }
            else {
                editPageUrl += "&id=" + selectId;
            }
        }
        else {
            editPageUrl += "&id=" + selectId;
        }
        var todoId = $(obj).attr('todoId');
        if (todoId && todoId.length > 0) {
            editPageUrl += "&todoId=" + todoId;
        }
        var currTabIndex = GetSelectTabIndex(); //当前grid网格页面的tabindex
        if (currTabIndex)
            editPageUrl += "&tb=" + currTabIndex;
        editPageUrl += "&r=" + Math.random();
        //执行编辑方法
        var ExecuteMethod = function () {
            //有自定义编辑方法则先调用自定义否则调用通用
            if (typeof (OverEdit) == "function") {
                OverEdit(obj);
                return;
            }
            if (editMode == 2) { //弹出框编辑模式
                var editWidth = parseInt($(obj).attr("editWidth"));
                var editHeight = parseInt($(obj).attr("editHeight"));
                editWidth += 40; //加上padding相关
                editHeight += 35 + 50; //标题栏高度和按钮栏高度
                //加载弹出框操作按钮
                var params = { moduleId: tempModuleId, id: selectId, formType: 0, editMode: editMode };
                if (todoId && todoId.length > 0) {
                    params.todoId = todoId;
                }
                ExecuteCommonAjax('/' + CommonController.Async_System_Controller + '/LoadFormBtns.html', params, function (result) {
                    if (result && result.length > 0) {
                        var toolbar = result;
                        for (var i = 0; i < toolbar.length; i++) {
                            var tempHandler = toolbar[i].handler;
                            if (typeof (tempHandler) == 'string') {
                                toolbar[i].handler = eval('(' + tempHandler + ')');
                            }
                        }
                        top.openDialog(title, editPageUrl, toolbar, editWidth, editHeight, null, function (dialogDivId) {
                            setTimeout(function () {
                                var divBtnPar = top.$("#" + dialogDivId).parent();
                                var dgBtn = top.$("div.dialog-button a", divBtnPar);
                                if (page != "view") {
                                    dgBtn.attr("detail", toolbar[0].detail);
                                }
                                dgBtn.attr("editMode", editMode);
                                dgBtn.attr("moduleId", tempModuleId);
                                dgBtn.attr("moduleName", moduleName);
                                dgBtn.attr("gridId", gridId);
                            }, 50);
                        });
                    }
                }, false, true);
            }
            else if (editMode == 1) { //tab编辑模式
                AddTab(null, title, editPageUrl);
            }
            else if (editMode == 3) { //列表行编辑模式
                var rowIndex = gridObj.datagrid("getRowIndex", row);
                EditRow(gridId, rowIndex);
                var tag = tempModuleId + "_" + selectId;
                $("#rowOperateDiv_" + tag).hide();
                $("#rowOkDiv_" + tag).show();
                InitRowOpBtns(gridId);
            }
            else if (editMode == 4) { //网格编辑表单
                var rowIndex = gridObj.datagrid("getRowIndex", row);
                var expanderRowIdPrefix = GetExpanderRowIdPrefix();
                var rowExpanderIconDom = $("#" + expanderRowIdPrefix + rowIndex + " span.datagrid-row-expander");
                var formType = rowExpanderIconDom.attr("formType");
                if (rowExpanderIconDom.hasClass("datagrid-row-expand")) { //收缩状态
                    rowExpanderIconDom.attr("formType", "edit");
                    rowExpanderIconDom.click();
                }
                else { //已展开状态
                    if (formType != "edit") {
                        rowExpanderIconDom.attr("formType", "edit");
                        var editWidth = parseInt($(obj).attr("editWidth"));
                        var editHeight = parseInt($(obj).attr("editHeight"));
                        var option = { titleKey: titleKey, editWidth: editWidth, editHeight: editHeight };
                        ExpandGridRowForm(gridId, tempModuleId, moduleName, row, rowIndex, option);
                    }
                }
            }
        }
        var isDraft = GetLocalQueryString("draft"); //是否我的草稿列表页
        if (!isDraft) {
            //先进行客户端和服务端验证
            GridOperateVerify(moduleName, "编辑", selectId, function (errMsg) {
                if (errMsg && errMsg.length > 0) {
                    top.showAlertMsg("提示", errMsg, "info"); //弹出验证提示信息
                }
                else {
                    //调用通用方法
                    ExecuteMethod(); //执行编辑
                }
            });
        }
        else {
            ExecuteMethod(); //执行编辑
        }
    }
    else { //自定义编辑表单
        if (formUrl.indexOf('?') > -1) {
            formUrl += '&id=' + selectId;
        }
        else {
            formUrl += '?id=' + selectId;
        }
        AddTab(null, title, formUrl); //执行编辑
    }
}

//删除，兼之明细删除
function Delete(obj) {
    var tempModuleId = $(obj).attr("moduleId");
    var gridId = $(obj).attr('gridId');
    var gridObj = $("#" + gridId);
    var rows = GetSelectRows(gridId); //获取选中行
    if (!rows || rows.length == 0) { //没有选中行，从当前按钮中找对应的记录Id来得到选择行
        var selectId = $(obj).attr("recordId");
        var tempRows = gridObj.datagrid("getRows");
        for (var i = 0; i < tempRows.length; i++) {
            var tempRow = tempRows[i];
            if (selectId == tempRow["Id"]) {
                rows.push(tempRow);
                break;
            }
        }
    }
    if (!rows || rows.length == 0) {
        top.showAlertMsg("提示", "请至少选择一条记录！", "info"); //弹出提示信息
        return;
    }
    var isRecycle = parseInt($(obj).attr("recycle")) == 1; //是来自回收站
    var isHardDel = parseInt($(obj).attr("isHardDel")) == 1; //是否硬删除
    var msgTitle = "删除提示";
    var ExecuteDel = function () {
        //有自定义删除方法则先调用自定义否则调用通用
        if (typeof (OverDelete) == "function") {
            OverDelete(obj);
            return;
        }
        var moduleName = $(obj).attr("moduleName");
        var titleKey = $(obj).attr("titleKey");
        var titleKeyDisplay = $(obj).attr("titleKeyDisplay");
        var msg = "确定要删除";
        if (rows.length == 1 && titleKeyDisplay) {
            msg += titleKeyDisplay + "为【" + rows[0][titleKey] + "】的记录吗？";
        }
        else {
            msg += "选中的记录吗？";
        }
        if (isHardDel || isRecycle) {
            msg += "数据删除后不可恢复，请谨慎操作！";
        }
        top.showConfirmMsg(msgTitle, msg, function (action) {
            if (action) {
                if (page == "grid" || page == "view") { //主网格页面或主网格下方明细网格或者主从查看页面
                    var ids = "";
                    for (var i = 0; i < rows.length; i++) {
                        ids += rows[i]["Id"] + ",";
                    }
                    if (ids.length > 0) {
                        ids = ids.substr(0, ids.length - 1);
                    }
                    //先进行客户端和服务端验证
                    GridOperateVerify(moduleName, "删除", ids, function (errMsg) {
                        if (errMsg && errMsg.length > 0) {
                            top.showAlertMsg("提示", errMsg, "info"); //弹出验证提示信息
                        }
                        else {
                            ExecuteCommonDelete(tempModuleId, ids, isRecycle, isHardDel, function () {
                                //刷新列表
                                gridObj.datagrid("reload");
                            });
                        }
                    });
                }
                else if (page == "add" || page == "edit") { //编辑页面
                    $.each(rows, function (i, row) {
                        var rowIndex = gridObj.datagrid("getRowIndex", row);
                        DelRow(gridId, rowIndex);
                    });
                }
            }
        });
    }
    //有自定义删除前验证方法
    if (typeof (OverBeforeDeleteVerify) == "function") {
        //调用后执行回调函数返回验证异常信息
        OverBeforeDeleteVerify(function (errMsg) {
            if (errMsg && errMsg.length > 0) { //验证不通过
                top.showAlertMsg(msgTitle, errMsg, "info");
                return;
            }
            else {
                ExecuteDel();
            }
        });
    }
    else {
        ExecuteDel();
    }
}

//查看，兼之明细查看
function ViewRecord(obj) {
    var tempModuleId = $(obj).attr("moduleId");
    var gridId = $(obj).attr('gridId');
    var gridObj = $("#" + gridId);
    var editMode = parseInt($(obj).attr("editMode"));
    var titleKey = $(obj).attr("titleKey"); //标记字段名
    var selectId = $(obj).attr("recordId"); //记录Id
    var titleKeyValue = $(obj).attr("titleKeyValue"); //标记字段值
    if (!titleKeyValue || titleKeyValue.length == 0) {
        var row = GetSelectRow(gridId); //获取选中行
        if (!row) { //没有选中行，从当前按钮中找对应的记录Id来得到选择行
            if (selectId) {
                var rows = gridObj.datagrid("getRows");
                for (var i = 0; i < rows.length; i++) {
                    var tempRow = rows[i];
                    if (selectId == tempRow["Id"]) {
                        row = tempRow;
                        break;
                    }
                }
            }
        }
        if (!row) {
            top.showAlertMsg("提示", "请选择一条记录！", "info"); //弹出提示信息
            return;
        }
        if (!selectId)
            selectId = row["Id"];
        titleKeyValue = row[titleKey];
    }
    var moduleName = $(obj).attr("moduleName");
    var title = "查看" + moduleName;
    if (titleKey) {
        title = title + "－" + titleKeyValue;
    }
    var formUrl = $(obj).attr('formUrl'); //自定义表单页面URL
    if (formUrl == undefined || formUrl == null || formUrl.length == 0) { //通用新增页面
        var viewPageUrl = "/Page/ViewForm.html?page=view&moduleId=" + tempModuleId;
        viewPageUrl += "&tip=0";
        viewPageUrl += "&mode=" + editMode;
        var isEditDetailView = false; //是否编辑明细查看
        if (page == "add" || page == "edit" || page == "view") { //主从编辑页面添加标识
            var parentMode = GetLocalQueryString("mode"); //父页面编辑模式
            viewPageUrl += "&pmode=" + parentMode;
            if ((editMode == 2 || editMode == 3) && (page == "add" || page == "edit")) {
                viewPageUrl += "&ff=1"; //标识来自表单页面
                isEditDetailView = true;
            }
            else {
                viewPageUrl += "&id=" + selectId;
            }
        }
        else {
            viewPageUrl += "&id=" + selectId;
        }
        var recycle = $(obj).attr("recycle");
        if (parseInt(recycle) == 1) {
            viewPageUrl += "&recycle=1";
        }
        viewPageUrl += "&r=" + Math.random();
        //执行新增方法
        var ExecuteMethod = function () {
            //有自定义查看方法则先调用自定义否则调用通用
            if (typeof (OverViewRecord) == "function") {
                OverViewRecord(obj);
                return;
            }
            if (editMode == 2 || editMode == 3) { //弹出框编辑模式
                var editWidth = parseInt($(obj).attr("editWidth"));
                var editHeight = parseInt($(obj).attr("editHeight"));
                editWidth += 40; //加上padding相关
                editHeight += 35 + 50; //标题栏高度和按钮栏高度
                //加载弹出框操作按钮
                var params = { moduleId: tempModuleId, id: selectId, formType: 1, editMode: editMode };
                if (parseInt(recycle) == 1) {
                    params.recycle = 1;
                }
                ExecuteCommonAjax('/' + CommonController.Async_System_Controller + '/LoadFormBtns.html', params, function (result) {
                    if (result && result.length > 0) {
                        var toolbar = [];
                        for (var i = 0; i < result.length; i++) {
                            if (isEditDetailView && result[i].iconType == 1)
                                continue;
                            var tempHandler = result[i].handler;
                            if (typeof (tempHandler) == 'string') {
                                result[i].handler = eval('(' + tempHandler + ')');
                            }
                            toolbar.push(result[i]);
                        }
                        top.openDialog(title, viewPageUrl, toolbar, editWidth, editHeight, null, function (dialogDivId) {
                            setTimeout(function () {
                                var divBtnPar = top.$("#" + dialogDivId).parent();
                                var dgBtn = top.$("div.dialog-button a", divBtnPar);
                                if (page != "view") {
                                    dgBtn.attr("detail", toolbar[0].detail);
                                }
                                dgBtn.attr("editMode", editMode);
                                dgBtn.attr("moduleId", tempModuleId);
                                dgBtn.attr("moduleName", moduleName);
                                dgBtn.attr("gridId", gridId);
                            }, 50);
                        });
                    }
                }, false, true);
            }
            else if (editMode == 1 || editMode == 3) { //tab查看模式或列表行查看模式
                AddTab(null, title, viewPageUrl);
            }
            else if (editMode == 4) { //网格查看表单
                var rowIndex = gridObj.datagrid("getRowIndex", row);
                var expanderRowIdPrefix = GetExpanderRowIdPrefix();
                var rowExpanderIconDom = $("#" + expanderRowIdPrefix + rowIndex + " span.datagrid-row-expander");
                var formType = rowExpanderIconDom.attr("formType");
                if (rowExpanderIconDom.hasClass("datagrid-row-expand")) { //收缩状态
                    rowExpanderIconDom.attr("formType", "view");
                    rowExpanderIconDom.click();
                }
                else { //已展开状态
                    if (formType != "view") {
                        rowExpanderIconDom.attr("formType", "view");
                        var editWidth = parseInt($(obj).attr("editWidth"));
                        var editHeight = parseInt($(obj).attr("editHeight"));
                        var option = { titleKey: titleKey, editWidth: editWidth, editHeight: editHeight };
                        ExpandGridRowForm(gridId, tempModuleId, moduleName, row, rowIndex, option);
                    }
                }
            }
        }
        ExecuteMethod(); //执行查看
    }
    else { //自定义查看页面
        if (formUrl.indexOf('?') > -1) {
            formUrl += '&id=' + selectId;
        }
        else {
            formUrl += '?id=' + selectId;
        }
        AddTab(null, title, formUrl); //执行查看
    }
}

//批量编辑
function BatchEdit(obj) {
    //有自定义批量编辑方法则先调用自定义否则调用通用
    if (typeof (OverBatchEdit) == "function") {
        OverBatchEdit(obj);
        return;
    }
    var moduleId = $(obj).attr("moduleId");
    var moduleName = $(obj).attr("moduleName");
    var gridId = $(obj).attr('gridId');
    var selectRows = GetSelectRows(gridId); //当前选择行
    var selectRecords = selectRows.length; //选择记录数
    var pageRows = GetCurrentRows(gridId); //当前页所有行
    var pageRecords = pageRows.length; //当前页记录数
    var toolbar = [{
        id: 'btnOk',
        text: "确 定",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            top.getCurrentDialogFrame()[0].contentWindow.GetBatchEditData(function (updateRange, data) {
                if (data && data.length > 0) {
                    var ids = ""; //更新所有记录
                    var rows = null;
                    if (updateRange == "1") { //更新当前选中记录
                        if (selectRows == null || selectRows.length == 0) {
                            top.showAlertMsg("提示", "当前选中记录数为0！", "info");
                            return;
                        }
                        rows = selectRows;
                    }
                    else if (updateRange == "2") { //更新当前页面记录
                        if (pageRows == null || pageRows.length == 0) {
                            top.showAlertMsg("提示", "当前页面记录数为0！", "info");
                            return;
                        }
                        rows = pageRows;
                    }
                    if (rows != null) {
                        for (var i = 0; i < rows.length; i++) {
                            var row = rows[i];
                            ids += row["Id"] + ",";
                        }
                        ids = ids.substr(0, ids.length - 1);
                    }
                    $.ajax({
                        type: 'post',
                        url: '/' + CommonController.Async_Data_Controller + '/BatchUpdate.html',
                        data: { moduleId: moduleId, ids: ids, data: $.base64.encode(escape(JSON.stringify(data))) },
                        dataType: "json",
                        beforeSend: function () {
                            top.openWaitDialog('正在更新...');
                        },
                        success: function (result) {
                            top.closeWaitDialog();
                            if (result && result.Success) {
                                top.closeDialog();
                                RefreshGrid(gridId);
                            }
                            else {
                                top.showAlertMsg("提示", result.Message, "info");
                            }
                        },
                        error: function (err) {
                            top.closeWaitDialog();
                            top.showAlertMsg("提示", "批量更新失败，服务端异常！", "error");
                        }
                    });
                }
                else {
                    top.showAlertMsg("提示", "请至少选择一个变更字段并设置字段值！", "info");
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
    var url = '/Page/BatchEdit.html?moduleId=' + moduleId + '&selectRecords=' + selectRecords + '&pageRecords=' + pageRecords + '&r=' + Math.random();
    top.openDialog('批量编辑－' + moduleName, url, toolbar, 600, 450, 'eu-icon-edit');
}

//导入实体
function ImportModel(obj) {
    //有自定义导入实体方法则先调用自定义否则调用通用
    if (typeof (OverImportModel) == "function") {
        OverImportModel(obj);
        return;
    }
    var moduleId = $(obj).attr("moduleId");
    var moduleName = $(obj).attr("moduleName");
    var gridId = $(obj).attr('gridId');
    var toolbar = [{
        id: 'btnOk',
        text: "确 定",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            var iframe = top.getCurrentDialogFrame();
            iframe[0].contentWindow.UploadTempData(function (fileName) {
                //数据文件上传完成后开始导入数据
                $.ajax({
                    type: "post",
                    url: "/" + CommonController.Async_Data_Controller + "/ImportModelData.html",
                    data: { moduleId: moduleId, fileName: escape(fileName) },
                    dataType: "json",
                    beforeSend: function () {
                        //top.openWaitDialog('正在导入数据，请稍候...');
                    },
                    success: function (result) {
                        if (result.Success) {
                            //top.closeDialog();
                            top.showMsg('导入提示', "导入成功！", function () {
                                RefreshGrid(gridId);
                                //top.closeWaitDialog();
                            });
                        }
                        else {
                            top.showAlertMsg('导入提示', result.Message, 'info', function () {
                                //top.closeWaitDialog();
                            });
                        }
                    },
                    error: function (err) {
                        top.showAlertMsg('导入提示', "数据导入失败，服务器异常！", 'error', function () {
                            top.closeWaitDialog();
                        });
                    }
                });
            });
            top.showMsg('导入提示', '系统正在处理中，可能需要等待较长时间，等待期间您可以处理其他工作，在弹出提示信息前请不要重复点击！', function () {
                top.closeDialog();
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
    var url = '/Page/ImportModel.html?moduleId=' + moduleId + '&r=' + Math.random();
    top.openDialog('数据导入－' + moduleName, url, toolbar, 600, 300, 'eu-icon-export');
}

//导出数据
function ExportModel(obj) {
    //有自定义导出方法则先调用自定义否则调用通用
    if (typeof (OverExportModel) == "function") {
        OverExportModel(obj);
        return;
    }
    var moduleId = $(obj).attr("moduleId");
    var moduleName = $(obj).attr("moduleName");
    var gridId = $(obj).attr('gridId');
    var toolbar = [{
        id: 'btnOk',
        text: "导 出",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            var iframe = top.getCurrentDialogFrame();
            iframe[0].contentWindow.GetCondition(function (type, conditions) {
                var url = '/' + CommonController.Async_Data_Controller + '/ExportModelData.html';
                var data = { moduleId: moduleId };
                if (type == 0) {
                    data = searchParams ? searchParams : {};
                    data.moduleId = moduleId;
                }
                else if (type == 2) {
                    data = { moduleId: moduleId, cdItems: JSON.stringify(conditions) };
                }
                //开始导出数据
                $.ajax({
                    type: "post",
                    url: url,
                    data: data,
                    dataType: "json",
                    beforeSend: function () {
                        //top.openWaitDialog('正在导出数据，请稍候...');
                    },
                    success: function (result) {
                        if (result.Success) {
                            //top.closeWaitDialog();
                            //top.closeDialog();
                            window.open(result.DownUrl);
                        }
                        else {
                            top.showAlertMsg('导出提示', result.Message, 'info', function () {
                                top.closeWaitDialog();
                            });
                        }
                    },
                    error: function (err) {
                        top.showAlertMsg('导出提示', "【" + moduleName + "】数据导出失败，服务器异常！", 'error', function () {
                            top.closeWaitDialog();
                        });
                    }
                });
            });
            top.showMsg('导出提示', '系统正在处理中，可能需要等待较长时间，等待期间您可以处理其他工作，在弹出提示信息前请不要重复点击！', function () {
                top.closeDialog();
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
    var count = parseInt($('#btnExportModel').attr('total'));
    var url = '/Page/ExportModel.html?moduleId=' + moduleId + '&cc=' + count + '&r=' + Math.random();
    top.openDialog('数据导出－' + moduleName, url, toolbar, 600, 300, 'eu-icon-export');
}

//复制
function Copy(obj) {
    var tempModuleId = $(obj).attr("moduleId");
    var moduleName = $(obj).attr("moduleName");
    var gridId = $(obj).attr('gridId');
    var gridObj = $("#" + gridId);
    var editMode = parseInt($(obj).attr("editMode")); //编辑模式
    var row = GetSelectRow(gridId); //获取选中行
    if (!row) { //没有选中行，从当前按钮中找对应的记录Id来得到选择行
        var selectId = $(obj).attr("recordId"); //要编辑的记录Id
        var rows = gridObj.datagrid("getRows");
        for (var i = 0; i < rows.length; i++) {
            var tempRow = rows[i];
            if (selectId == tempRow["Id"]) {
                row = tempRow;
                break;
            }
        }
    }
    if (!row) {
        top.showAlertMsg("提示", "请选择一条记录！", "info"); //弹出提示信息
        return;
    }
    //先进行客户端和服务端验证
    GridOperateVerify(moduleName, "复制", null, function (errMsg) {
        if (errMsg && errMsg.length > 0) {
            top.showAlertMsg("提示", errMsg, "info"); //弹出验证提示信息
        }
        else {
            //调用通用方法
            Add(obj, row["Id"]); //执行复制
        }
    });
}

//打印
function PrintModel(obj) {

}

//转到回收站
function GoToRecycle(obj) {
    var moduleId = $(obj).attr("moduleId");
    var moduleName = $(obj).attr("moduleName");
    var url = "/Page/Grid.html?page=grid&moduleId=" + moduleId + "&recycle=1";
    var title = "回收站－" + moduleName;
    AddTab(null, title, url);
}

//还原数据
function Restore(obj) {
    var tempModuleId = $(obj).attr("moduleId");
    var gridId = "mainGrid";
    var gridObj = $("#" + gridId);
    var rows = GetSelectRows(gridId); //获取选中行
    if (!rows || rows.length == 0) { //没有选中行，从当前按钮中找对应的记录Id来得到选择行
        var selectId = $(obj).attr("recordId");
        var tempRows = gridObj.datagrid("getRows");
        for (var i = 0; i < tempRows.length; i++) {
            var tempRow = tempRows[i];
            if (selectId == tempRow["Id"]) {
                rows.push(tempRow);
                break;
            }
        }
    }
    if (!rows || rows.length == 0) {
        top.showAlertMsg("提示", "请至少选择一条记录！", "info"); //弹出提示信息
        return;
    }
    var msgTitle = "还原提示";
    var ExecuteRestore = function () {
        //有自定义删除方法则先调用自定义否则调用通用
        if (typeof (OverRestore) == "function") {
            OverRestore(obj);
            return;
        }
        var moduleName = $(obj).attr("moduleName");
        var titleKey = $(obj).attr("titleKey");
        var titleKeyDisplay = $(obj).attr("titleKeyDisplay");
        var msg = "确定要还原";
        if (rows.length == 1 && titleKeyDisplay) {
            msg += titleKeyDisplay + "为【" + rows[0][titleKey] + "】的记录吗？";
        }
        else {
            msg += "选中的记录吗？";
        }
        top.showConfirmMsg(msgTitle, msg, function (action) {
            if (action) {
                if (page == "grid") { //主网格页面
                    var ids = "";
                    for (var i = 0; i < rows.length; i++) {
                        ids += rows[i]["Id"] + ",";
                    }
                    if (ids.length > 0) {
                        ids = ids.substr(0, ids.length - 1);
                    }
                    //先进行客户端和服务端验证
                    GridOperateVerify(moduleName, "还原", ids, function (errMsg) {
                        if (errMsg && errMsg.length > 0) {
                            top.showAlertMsg("提示", errMsg, "info"); //弹出验证提示信息
                        }
                        else { //执行还原
                            ExecuteCommonRestore(tempModuleId, ids, function () {
                                //刷新列表
                                gridObj.datagrid("reload");
                            });
                        }
                    });
                }
            }
        });
    }
    //有自定义还原前验证方法
    if (typeof (OverBeforeRestoreVerify) == "function") {
        //调用后执行回调函数返回验证异常信息
        OverBeforeRestoreVerify(function (errMsg) {
            if (errMsg && errMsg.length > 0) { //验证不通过
                top.showAlertMsg(msgTitle, errMsg, "info");
                return;
            }
            else {
                ExecuteRestore();
            }
        });
    }
    else {
        ExecuteRestore();
    }
}

//转到草稿
function GoToDraft(obj) {
    var moduleId = $(obj).attr("moduleId");
    var moduleName = $(obj).attr("moduleName");
    var url = "/Page/Grid.html?page=grid&moduleId=" + moduleId + "&draft=1";
    var title = "我的草稿－" + moduleName;
    AddTab(null, title, url);
}

//添加新行
//gridId:网格domId
function AddRow(gridId) {
    var gridObj = $("#" + gridId);
    gridObj.datagrid('insertRow', { index: 0, row: {} });
    gridObj.datagrid('beginEdit', 0);
    //增加完成调用自定义事件
    if (typeof (OverAddRowCompeleted) == "function") {
        OverAddRowCompeleted(gridId, 0);
    }
}

//编辑行
//gridId:网格domId
//rowIndex:行号
function EditRow(gridId, rowIndex) {
    var gridObj = $("#" + gridId);
    gridObj.datagrid('beginEdit', rowIndex);
}

//结束编辑行
//gridId:网格domId
//rowIndex:行号
function EndEditRow(gridId, rowIndex) {
    var gridObj = $("#" + gridId);
    gridObj.datagrid('endEdit', rowIndex);
}

//结束编辑所有行
//gridId:网格domId
function EndEditAllRows(gridId) {
    var rows = GetCurrentRows(gridId);
    if (rows && rows.length > 0) {
        for (var i = 0; i < rows.length; i++) {
            var row = rows[i];
            var rowIndex = GetRowIndexByRow(gridId, row);
            EndEditRow(gridId, rowIndex);
        }
    }
}

//取消编辑行
//gridId:网格domId
//rowIndex:行号
function CancelEditRow(gridId, rowIndex) {
    var gridObj = $("#" + gridId);
    gridObj.datagrid('cancelEdit', rowIndex);
    gridObj.datagrid('deleteRow', rowIndex);
}

//插入行
//gridId:网格domId
//row:行记录
function AppendRow(gridId, row) {
    var gridObj = $("#" + gridId);
    gridObj.datagrid('appendRow', row);
}

//更新行
//gridId:网格domId
//rowIndex:行号
//row:行记录
function UpdateRow(gridId, rowIndex, row) {
    var gridObj = $("#" + gridId);
    gridObj.datagrid('updateRow', {
        index: rowIndex,
        row: row
    });
}

//删除行
//gridId:网格domId
//rowIndex:行号
function DelRow(gridId, rowIndex) {
    var gridObj = $("#" + gridId);
    gridObj.datagrid("deleteRow", rowIndex);
}

//移除当前所有行
//gridId:网格domId
function DelCurrentRows(gridId) {
    var rows = GetCurrentRows(gridId);
    var copyRows = [];
    for (var i = 0; i < rows.length; i++) {
        copyRows.push(rows[i]);
    }
    for (var i = 0; i < copyRows.length; i++) {
        var row = copyRows[i];
        var rowIndex = GetRowIndexByRow(gridId, row);
        DelRow(gridId, rowIndex);
    }
}

//移除当前所有选中行
//gridId:网格domId
function DelSelectRows(gridId) {
    var rows = GetSelectRows(gridId);
    var copyRows = [];
    for (var i = 0; i < rows.length; i++) {
        copyRows.push(rows[i]);
    }
    for (var i = 0; i < copyRows.length; i++) {
        var row = copyRows[i];
        var rowIndex = GetRowIndexByRow(gridId, row);
        DelRow(gridId, rowIndex);
    }
}

//表单数据保存完成事件
//result:保存成功后的结果对象
function FormDataSaveCompeleted(result) {
    var obj = $("#btnAdd");
    var linkAdd = obj.attr("linkAdd"); //外链
    if (linkAdd == "true") {
        var initField = obj.attr("initField"); //原始模块表单外链按钮左边的控件字段
        var linkField = obj.attr("linkField"); //外链模块的titleKey字段
        var initNameField = initField.replace("Id", "Name");
        var formData = top.$("#main_dialog").find("iframe")[0].contentWindow.GetFormData();
        $("#" + initField).val(result.RecordId); //值控件斌值
        $("#" + initNameField).val(formData[linkField]); //文本控件斌值
    }
}

//切换视图
//moduleId:模块Id
//gridId:网格domId
//viewId:视图Id
function ChangeGridView(moduleId, gridId, viewId) {
    //先加载视图字段
    $.ajax({
        type: 'post',
        url: '/' + CommonController.Async_System_Controller + '/ChangeGridView.html',
        data: { viewId: viewId },
        dataType: "json",
        beforeSend: function () {
            top.openWaitDialog('视图切换中...');
        },
        success: function (data) {
            top.closeWaitDialog();
            if (!data) {
                top.showAlertMsg("提示", "视图切换失败，视图不存在！", "info");
                return;
            }
            var gridDom = $("#" + gridId);
            var gridView = data.GridView;
            $("#btn_gridSet" + moduleId).attr("title", "单击可切换列表视图--当前视图：" + gridView.Name);
            $("#btn_gridSet" + moduleId).attr("viewId", gridView.Id);
            $("#btn_advanceSearch" + moduleId).attr("viewId", gridView.Id);
            if (gridView.AddFilterRow) { //启用了行过滤
                //先移除所有行过滤规则
                gridDom.datagrid("removeFilterRule");
            }
            //处理搜索字段
            var searchFields = data.SearchFields;
            var menuObj = $("#search_mm" + moduleId);
            //先移除之前所有字段项 menuObj.html("");
            menuObj.find("div").each(function () {
                menuObj.menu('removeItem', $(this));
            });
            //重新加载字段项
            $('#txtSearch').searchbox('reset').searchbox('clear');
            if (searchFields && searchFields.length > 0) { //重置搜索字段
                $.each(searchFields, function (i, item) {
                    menuObj.menu('appendItem', {
                        id: item.FieldName,
                        name: item.FieldName,
                        text: item.Display,
                        iconCls: ''
                    });
                });
                //默认选择第一个字段
                $('#txtSearch').searchbox('selectName', searchFields[0].FieldName);
            }
            //行过滤规则处理
            if (gridView.AddFilterRow) { //启用了行过滤
                if (data.RuleFilters) {
                    $('#ruleFilters').val(data.RuleFilters);
                }
                if (data.NoFilterFields) {
                    $('#ruleFilters').attr('noFilterFields', data.NoFilterFields);
                }
            }
            //字段处理
            var viewFields = data.ViewFields;
            if (viewFields && viewFields.length > 0) {
                //锁定字段
                var frozenList = [];
                var frozenArray = [];
                //显示字段
                var fieldList = [];
                var fieldArray = [];
                var groupField = null;
                for (var i = 0; i < viewFields.length; i++) {
                    var field = viewFields[i];
                    var formatter = null;
                    if (field.Formatter && field.Formatter.length > 0) {
                        formatter = eval("(" + field.Formatter + ")");
                    }
                    if (field.IsGroupField) {
                        groupField = field.Sys_FieldName;
                    }
                    if (field.IsFrozen || field.Sys_FieldName == "Id") {
                        frozenArray.push({ field: field.Sys_FieldName, title: field.Display, width: field.Width, hidden: !field.IsVisible, formatter: formatter, checkbox: field.Sys_FieldName == "Id", sortable: field.IsAllowSort });
                    }
                    else {
                        fieldArray.push({ field: field.Sys_FieldName, title: field.Display, width: field.Width, hidden: !field.IsVisible, formatter: formatter, sortable: field.IsAllowSort });
                    }
                }
                frozenList.push(frozenArray);
                fieldList.push(fieldArray);
                //重新加载字段
                var options = gridDom.datagrid("options");
                options.columns = fieldList;
                options.frozenColumns = frozenList;
                var gridUrl = $("#btn_gridSet" + moduleId).attr("gridUrl");
                if (!gridUrl)
                    gridUrl = options.url;
                if (gridView.GridType == 3 || gridView.GridType == 4) { //综合视图或综合明细视图
                    gridUrl += "&viewId=" + gridView.Id;
                }
                if (gridView.GridType == 4) { //综合明细视图
                    gridUrl += "&dv=1";
                    $("a[id^='btn_attach_set_']").hide();
                    $('#region_south').removeAttr("flag").attr("isDetailView", "true").html("").hide(); //附属网格不显示
                    $("#regon_main .datagrid").css('border-bottom-width', '0px');
                }
                else {
                    $("a[id^='btn_attach_set_']").show();
                    $('#region_south').removeAttr("isDetailView").removeAttr("flag").show();
                }
                if (groupField) {
                    options.view = groupview;
                    options.groupField = groupField.Sys_FieldName;
                    options.groupFormatter = function (value, rows) { return value + '(' + rows.length + ')'; };
                }
                options.url = gridUrl;
                options.loadFilter = function (data) { if (typeof (OverLoadFilter) == 'function') { return OverLoadFilter(data, gridId, moduleId, null); } else { return data; } }
                if (gridId == "mainGrid" && gridView.AddFilterRow) { //启用了行过滤功能
                    gridDom.attr("enableFilter", "true"); //启用过滤标识
                }
                gridDom.datagrid(options);
            }
        },
        error: function (err) {
            top.closeWaitDialog();
            top.showAlertMsg("提示", "视图加载失败，服务端异常！", "error");
        }
    });
}

//创建网格列头右键菜单
//e:系统参数
//targetField:鼠标右键点击的字段名称
//menuDomId:右键菜单容器domId
//gridId:网格domId
function CreateColumnContextMenu(e, targetField, menuDomId, gridId) {
    e.preventDefault();
    var colMenuDom = $("#" + menuDomId);
    var gridObj = $("#" + gridId);
    colMenuDom.menu({
        onClick: function (item) {
            if (item.iconCls == 'eu-icon-ok') {
                gridObj.datagrid('hideColumn', item.name);
                colMenuDom.menu('setIcon', {
                    target: item.target,
                    iconCls: 'eu-icon-empty'
                });
            } else {
                gridObj.datagrid('showColumn', item.name);
                colMenuDom.menu('setIcon', {
                    target: item.target,
                    iconCls: 'eu-icon-ok'
                });
            }
        }
    });
    var fields = gridObj.datagrid('getColumnFields');
    var noHideFields = gridObj.attr('noHideFields');//不允许隐藏的字段
    var token = noHideFields && noHideFields.length > 0 ? noHideFields.split(',') : null;
    for (var i = 0; i < fields.length; i++) {
        var field = fields[i];
        if (field == "Id") continue;
        if (token != null) {
            var flag = false;
            for (var j = 0; j < token.length; j++) {
                if (token[j] == field) {
                    flag = true;
                    break;
                }
            }
            if (flag) continue;
        }
        var col = gridObj.datagrid('getColumnOption', field);
        if (!!col.title) {
            var item = colMenuDom.menu("findItem", col.title);
            if (!item) {
                colMenuDom.menu('appendItem', {
                    text: col.title,
                    name: field,
                    iconCls: 'eu-icon-ok'
                });
            }
        }
    }
    colMenuDom.menu('show', { left: e.pageX, top: e.pageY });
}

//网格行展开事件，针对网格内明细模块展示
//gridId:当前网格domId
//moduleId:当前模块Id
//moduleName:模块名称
//row:行对象
//index:行索引编号,有值时为网格内展开，无值时为网格下方加载
function ExpandGridRow(gridId, moduleId, moduleName, row, index) {
    var id = row["Id"];
    var gridObj = $("#" + gridId);
    var mod = gridObj.datagrid('getRowDetail', index);
    var html = mod.html();
    if (html && html.length > 0) { //数据已加载
        return;
    }
    var tag = id;
    var data = { moduleId: moduleId, id: id };
    if (gridId == "mainGrid") {
        var treeDom = $("#gridTree"); //左侧树dom对象
        var hasTree = treeDom && treeDom.length > 0;
        if (hasTree) //存在左侧树
            data.hasTree = 1;
    }
    $.ajax({
        type: 'get',
        url: '/' + CommonController.Async_System_Controller + '/LoadInnerDetailModuleGrid.html',
        data: data,
        dataType: "html",
        success: function (html) {
            mod.html(html);
            $.parser.parse('#ddv_' + tag);
            var tt = $('#tt_' + tag);
            var mm = $('#tt_mm_' + tag);
            gridObj.datagrid('fixDetailRowHeight', index);
            //处理tab右键菜单和tab工具栏
            //为选项卡绑定右键
            $("#ddv_" + tag + " .tabs-inner").bind('contextmenu', function (e) {
                mm.menu('show', {
                    left: e.pageX,
                    top: e.pageY
                });
                var title = $(this).children(".tabs-closable").text();
                mm.data("currtab", title);
                tt.tabs('select', title);
                return false;
            });
            //绑定右键菜单事件，刷新
            $('#tt_mm_refresh_' + tag + ',#tt_a_refresh_' + tag).click(function () {
                var tab = tt.tabs('getSelected');
                var iframe = $('iframe', tab);
                var url = iframe.attr('src');
                if (url && url.length > 0) {
                    tt.tabs('update', {
                        tab: tab,
                        options: {
                            content: CreateIFrame(url)
                        }
                    });
                }
            });
        },
        error: function (err) {
            top.closeWaitDialog();
        }
    });
}

//获取expander图标的行id前缀
//如：datagrid-row-r2-1-rowIndex
//parentDom:网格所属父dom对象
function GetExpanderRowIdPrefix(parentDom) {
    var td = parentDom && parentDom.length > 0 ?
        $("div.datagrid-body-inner td[field='_expander']", parentDom).eq(0) :
        $("div.datagrid-body-inner td[field='_expander']").eq(0);
    var tempId = td.parent().attr("id");
    var startIndex = tempId.lastIndexOf("-");
    return tempId.substr(0, startIndex + 1);
}

//网格行展开事件，针对网格行内展开表单编辑
//gridId:当前网格domId
//moduleId:当前模块Id
//moduleName:模块名称
//row:行对象
//index:行索引编号,有值时为网格内展开编辑表单，无值时展开新增表单
//option:关联数据，包括titleKey,titleKeyDisplay,formWidth,formHeight
function ExpandGridRowForm(gridId, moduleId, moduleName, row, index, option) {
    var prefixId = GetExpanderRowIdPrefix();
    var rowExpanderIconDom = $("#" + prefixId + index + " span.datagrid-row-expander");
    var formType = rowExpanderIconDom.attr("formType");
    var gridObj = $("#" + gridId);
    var ddv = gridObj.datagrid('getRowDetail', index).find('div.ddv');
    var ddvHtml = ddv.html();
    if (ddvHtml.length > 0 && ddv.attr("formType") == formType) { //非第一次加载
        return;
    }
    var title = "";
    if (formType == "edit") {
        var url = "/Page/EditForm.html?moduleId=" + moduleId + "&gridId=" + gridId;
        url += "&page=edit&id=" + row["Id"];
        title = "编辑" + moduleName;
    }
    else {
        var url = "/Page/ViewForm.html?moduleId=" + moduleId + "&gridId=" + gridId;
        url += "&page=view&id=" + row["Id"];
        title = "查看" + moduleName;
    }
    if (option && option.titleKey) {
        title += "－" + row[option.titleKey];
    }
    url += "&r=" + Math.random();
    var h = 250;
    var w = 550;
    var editH = option.editHeight ? option.editHeight + 35 + 50 : 0;
    var editW = option.editWidth ? option.editWidth : 0;
    if (option && editH > 250) {
        h = editH > 550 ? 550 : editH;
    }
    if (option && editW > 550) {
        w = editW > 900 ? 900 : editW;
    }
    ddv.panel({
        title: title,
        height: h,
        width: w,
        content: CreateIFrame(url),
        onLoad: function () {
            gridObj.datagrid('fixDetailRowHeight', index);
        }
    });
    ddv.attr("formType", formType);
    gridObj.datagrid('fixDetailRowHeight', index);
}

//网格行编辑表单格式化
function GridFormDeailFormatter(gridId, moduleId, moduleName, index, row) {
    return "<div class=\"ddv\"></div>";
}

//主网格页面行内明细或附属视图模块标签时触发事件
//title:tab标题
//index:tab序号
function ExpandRowTabSelected(title, index) {
    var target = this;
    var panelDom = $(target).tabs('getSelected');
    var iframe = $("iframe", panelDom);
    var src = iframe.attr("src");
    var url = iframe.attr("url");
    if (url && (!src || src.length == 0)) {
        iframe.attr("src", url);
    }
}

//主网格页面展开选择附属明细模块标签时触发事件
//title:tab标题
//index:tab序号
function AttachTabSelected(title, index) {
    var target = this;
    var row = GetSelectRow();
    var tab = $(target).tabs('getSelected');
    LoadAttachModuleData(tab, row);
}

//加载附属模块数据
function LoadAttachModuleData(tab, row) {
    if (!tab || !row || !row["Id"]) return;
    var gridObj = tab.find("table[id^='grid_']");
    var currFlagId = gridObj.attr('flagId');
    if (currFlagId == row["Id"]) return;
    var url = gridObj.attr('baseUrl');
    var condition = GetQueryStringByUrl(url, 'condition');
    if (condition && condition.length > 0) {
        var replaceCondition = decodeURIComponent(condition).replace('{Id}', row["Id"]);
        url = url.replace(condition, encodeURIComponent(replaceCondition));
        ReloadData(gridObj.attr('id'), url);
        gridObj.attr('flagId', row["Id"]);
    }
}

//行选中事件
//girdId:网格domId
//moduleId:模块Id
//moduleName:模块名称
//rowIndex:行索引
//rowData:行数据
function OnSelect(rowIndex, rowData, gridId, moduleId, moduleName) {
    if (page == "grid" && gridId == "mainGrid") {
        //加载网格下方附属模块和明细模块
        var tabObj = $('#detailTabs');
        if (tabObj.length > 0) {
            var selectTab = tabObj.tabs('getSelected');
            LoadAttachModuleData(selectTab, rowData);
        }
    }
    if (typeof (OverOnSelect) == "function") {
        OverOnSelect(rowIndex, rowData, gridId, moduleId, moduleName);
    }
}

//网格数据加载完成
//data:数据
//girdId:网格domId
//moduleId:模块Id
//moduleName:模块名称
function OnLoadSuccess(data, gridId, moduleId, moduleName) {
    if (typeof (OverOnLoadSuccess) == 'function') {
        OverOnLoadSuccess(data, gridId, moduleId, moduleName);
    }
    //行操作按钮处理
    InitRowOpBtns(gridId);
    //明细网格
    if (page == 'add' || page == 'edit' || page == 'view') {
        var tabsDom = $("#detailTab");
        if (tabsDom.length > 0) {
            var w = tabsDom.width();
            ResizeGrid("grid_" + moduleId, w);
        }
    }
    $("img.editFlag").attr("title", "单击编辑字段值");
    //鼠标移入单元格时显示字段编辑图标
    $(".datagrid-row td").mouseover(function (e) {
        $("img.editFlag", $(this)).show();
    }).mouseout(function (e) {
        $("img.editFlag", $(this)).hide();
    });
    if (page == 'grid' && gridId == 'mainGrid') {
        $("input.datagrid-filter[type='checkbox']").click(function () {
            if ($(this).attr("checked")) {
                $(this).attr("value", "1");
            }
            else {
                $(this).attr("value", "0");
            }
        });
        if ($("#" + gridId).attr("enableFilter") == "true") { //启用了行过滤功能
            $("#" + gridId).removeAttr("enableFilter");
            EnableRowFilter();
        }
        //有附属模块或明细
        if ($("div[id^='region_south']").length > 0) {
            var rows = GetCurrentRows(gridId);
            if (rows && rows.length > 0) {
                $("#" + gridId).datagrid("selectRow", 0);
            }
        }
    }
    $('#btnExportModel').attr('total', data.total);
}

//行样式设置
//index:行号
//row:行对象
//girdId:网格domId
//moduleId:模块Id
//moduleName:模块名称
function OverRowStyler(index, row, gridId, moduleId, moduleName) {
    if (typeof (OverOnRowStyler) == 'function') {
        return OverOnRowStyler(index, row, gridId, moduleId, moduleName);
    }
}

//初始化行操作按钮（行编辑时）
//gridId:网格domId
function InitRowOpBtns(gridId) {
    //生成行操作按钮函数
    var createRowOpBtn = function () {
        $("div[id^='rowOperateDiv_'] a[rowOperateBtn='1']").each(function (i, item) {
            var moduleId = $(item).attr("moduleId");
            var recordId = $(item).attr("recordId"); //记录Id
            var editMode = parseInt($(item).attr("editMode"));
            var tag = moduleId + "_" + recordId;
            $(item).attr('gridId', gridId);
            $(item).linkbutton({
                iconCls: $(item).attr('icon'),
                onClick: function (e) {
                    var methodStr = $(item).attr('clickMethod');
                    if (methodStr != undefined && methodStr.length > 0) {
                        var tempMethodStr = methodStr.replace("(this)", "");
                        if (tempMethodStr == "Edit" && editMode == 3) {
                            $("#rowOperateDiv_" + tag).hide();
                            $("#rowOkDiv_" + tag).show();
                            createSaveBtn();
                            createCancelBtn();
                        }
                        else if (tempMethodStr == "Add" && editMode == 3) {
                            //一次只能添加一行，新增行保存后才能再增加
                            var hasNew = false;
                            var gridObj = $('#' + gridId);
                            var rows = gridObj.datagrid("getRows");
                            for (var i = 0; i < rows.length; i++) {
                                var row = rows[i];
                                if (!row["Id"]) {
                                    hasNew = true;
                                    break;
                                }
                            }
                            if (hasNew) { //存在新增行，返回
                                return;
                            }
                        }
                        eval('(' + methodStr + ')');
                        StopBubble(e);
                    }
                },
                plain: true
            });
            var btnText = $(item).attr('btnText');
            if (btnText != undefined && btnText) {
                $(item).tooltip({ content: btnText });
            }
        });
    }
    //生成保存按钮函数
    var createSaveBtn = function () {
        $("a[id^='rowOkBtn_']").each(function (i, item) {
            var moduleId = $(item).attr("moduleId");
            var moduleName = $(item).attr("moduleName");
            var recordId = $(item).attr("recordId"); //记录Id
            var tag = moduleId + "_" + recordId;
            $(item).attr('gridId', gridId);
            $(item).linkbutton({
                iconCls: 'eu-p2-icon-save',
                onClick: function (e) {
                    var rowIndex = 0; //行号
                    if (recordId > 0) {
                        rowIndex = GetRowIndexByRecordId(gridId, recordId);
                    }
                    EndEditRow(gridId, rowIndex); //结束编辑当前行
                    var gridObj = $('#' + gridId);
                    var row = null;
                    var rows = gridObj.datagrid('getChanges');
                    for (var i = 0; i < rows.length; i++) {
                        var tempRow = rows[i];
                        if (recordId == 0 && !tempRow["Id"]) { //新增行
                            row = tempRow;
                        }
                        else { //编辑行
                            if (tempRow["Id"] == recordId) {
                                row = tempRow;
                                break;
                            }
                        }
                    }
                    if (row != null) {
                        var data = [];
                        for (var p in row) {
                            data.push({ name: p, value: row[p] });
                        }
                        var msgTitle = '保存提示';
                        var formObject = { ModuleId: moduleId, ModuleName: moduleName, ModuleData: data };
                        alert(CommonController.Async_Data_Controller);
                        var url = "/" + CommonController.Async_Data_Controller + "/SaveData.html";
                        $.ajax({
                            type: "post",
                            url: url,
                            data: { formData: $.base64.encode(escape(JSON.stringify(formObject))) },
                            beforeSend: function () {
                                top.openWaitDialog('数据保存中...');
                            },
                            success: function (result) {
                                top.closeWaitDialog();
                                if (result.Success) {
                                    top.showMsg(msgTitle, '数据保存成功！', function () {
                                        if (recordId == 0) //新增时刷新网格
                                            RefreshGrid(gridId);
                                        $("#rowOkDiv_" + tag).hide();
                                        $("#rowOperateDiv_" + tag).show();
                                        createRowOpBtn();
                                    });
                                }
                                else {
                                    top.showAlertMsg(msgTitle, result.Message, "info");
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
                    StopBubble(e);
                },
                plain: true
            });
            $(item).tooltip({ content: '保存' });
        });
    }
    //生成取消按钮函数
    var createCancelBtn = function () {
        $("a[id^='rowCancelBtn_']").each(function (i, item) {
            var moduleId = $(item).attr("moduleId");
            var recordId = $(item).attr("recordId"); //记录Id
            var tag = moduleId + "_" + recordId;
            $(item).attr('gridId', gridId);
            $(item).linkbutton({
                iconCls: 'eu-p2-icon-cancel',
                onClick: function (e) {
                    var rowIndex = 0; //行号
                    if (recordId > 0) {
                        rowIndex = GetRowIndexByRecordId(gridId, recordId);
                    }
                    CancelEditRow(gridId, rowIndex);
                    $("#rowOkDiv_" + tag).hide();
                    $("#rowOperateDiv_" + tag).show();
                    createRowOpBtn();
                    StopBubble(e);
                },
                plain: true
            });
            $(item).tooltip({ content: '取消编辑' });
        });
    }
    //调用函数
    createRowOpBtn();
    createSaveBtn();
    createCancelBtn();
}

//单击左侧树结点事件
//node:节点对象
function GridTreeNodeClick(node) {
    var txtSearcher = $("#txtSearch");
    var name = txtSearcher.searchbox("getName"); //搜索字段
    var value = txtSearcher.searchbox("getValue"); //搜索值
    SimpleSearch(txtSearcher, value, name);
}

//网格左侧树加载成功
function GridTreeLoadSuccess(node, data) {
}

//网格左侧树加载过滤事件
//data:网格数据
function GridTreeLoadFilter(data) {
    if (typeof (data) == 'string') {
        var tempData = eval("(" + data + ")");
        arr = [];
        arr.push(tempData);
        return arr;
    }
    else if (data instanceof Array) { //是否为数组
        return data;
    }
    else { //为对象
        arr = [];
        arr.push(data);
        return arr;
    }
}
