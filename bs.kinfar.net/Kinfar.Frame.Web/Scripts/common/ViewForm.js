var id = GetLocalQueryString("id"); //记录Id

//通用查看表单js
$(function () {
    //html标签上存在这个类会导致滚动条无法下拉到最底部，一直处于闪烁状态
    //$("html").removeClass("panel-fit");
    $("a[id^='btnEditField_']").attr("title", "单击编辑字段值");
    $("#detailTab .datagrid-wrap").css('border-top-width', '0px');
    var ff = GetLocalQueryString("ff"); //主从编辑页面弹出明细查看标识
    if (ff == "true") { //是从主从编辑页面弹出明细查看页面
        //从编辑网格加载数据
        var detailModuleId = GetLocalQueryString("moduleId"); //明细模块Id
        var pmode = GetLocalQueryString("pmode"); //父页面编辑模式
        var gridId = "grid_" + detailModuleId;
        //取当前选中行字段文本显示值
        var iframe = null;
        if (pmode == 2) { //弹出框
            iframe = top.getParentDialogFrame(); //取父弹出框iframe
        }
        else { //tab方式
            iframe = top.getCurrentTabFrame();
        }
        var row = iframe[0].contentWindow.GetSelectRowDisplayValue(gridId);
        //绑定数据
        $.each(row, function (key, value) {
            var field = GetFormField(key);
            if (field.ForeignModuleName) {
                if (key.length > 4 && key.substr(key.length - 4) == "Name") {
                    var realName = key.substr(0, key.length - 4) + "Id";
                    $("#span_" + realName).text(value);
                }
            }
            else {
                $("#span_" + key).text(value);
            }
        });
        //加载对应主表的外键字段显示值
        var obj = iframe[0].contentWindow.GetTitleKeyValue();
        if (obj) {
            $("#span_" + obj.foreignFieldName).text(obj.value);
        }
    }
    if (typeof (OverFormLoadCompleted) == "function") {
        OverFormLoadCompleted();
    }
});

//编辑按钮事件
function ToEdit(obj) {
    var editMode = parseInt($(obj).attr("editMode"));
    if (editMode == 1) { //tab标签编辑模式
        var id = GetLocalQueryString("id");
        var tempModuleId = $(obj).attr("moduleId");
        var tempModuleName = $(obj).attr("moduleName");
        var title = "编辑" + tempModuleName;
        var titleKeyValue = $(obj).attr("titleKeyValue");
        if (titleKeyValue) {
            title = title + "－" + titleKeyValue;
        }
        var editUrl = "/Page/EditForm.html?page=edit&moduleId=" + tempModuleId + "&id=" + id + "&r=" + Math.random();
        //跳转到编辑页面
        var tab = GetSelectedTab();
        UpdateTab(null, tab, editUrl, title);
    }
    else if (editMode == 2) { //弹出框编辑模式
        top.closeDialog();
        var dom = top.getCurrentTabFrameDom("btnEdit");
        if (dom.length > 0) { //编辑按钮在工具栏上
            dom.click();
        }
    }
}

//获取表单的titlekey字段值
function GetTitleKeyValue() {
    var field = GetTitleKeyField();
    if (field && field.Sys_FieldName) {
        var name = field.Sys_FieldName;
        var value = $("#span_" + name).text();
        return { name: name, value: value, foreignFieldName: field.ForignFieldName, recordId: id };
    }
    return null;
}

//打印表单
function PrintForm(obj) {
    alert('打印表单');
}