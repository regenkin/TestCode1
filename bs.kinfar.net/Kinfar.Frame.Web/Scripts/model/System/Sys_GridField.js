var page = GetLocalQueryString("page"); //页面类型标识

//表单加载完成后
function OverFormLoadCompleted() {
    if (page == "add" || page=="edit") {
        //设置模块字段的下拉数据源
        var viewId = $('#Sys_GridId').textbox('getValue');
        if (viewId) { //根据当前表单视图绑定字段信息
            var flag = 4;
            if (page == "edit") flag = 5;
            var url = '/' + CommonController.Async_System_Controller + '/LoadFields.html?flag=' + flag + '&viewId=' + viewId;
            $('#Sys_FieldId').combobox('reload', url);
            if (page == "edit") {
                var value = $('#Sys_FieldId').combobox('getValue');
                $('#Sys_FieldId').combobox('setValue', value);
            }
        }
    }
}

//重写字段选择事件
function OverOnFieldSelect(record, fieldName, valueField, textField) {
    if (page == "add") {
        if (fieldName == 'Sys_GridId') { //选择字段后加载依赖字段数据源
            var viewId = $('#Sys_GridId').textbox('getValue');
            var url = '/' + CommonController.Async_System_Controller + '/LoadFields.html?flag=4&viewId=' + viewId;
            $('#Sys_FieldId').combobox('clear').combobox('reload', url);
        }
    }
    if (page == "add" || page == "edit") {
        if (fieldName == 'Sys_FieldId') { //选择字段后，显示名称随着变
            $('#Display').textbox('setValue', $('#Sys_FieldId').combobox('getText'));
        }
    }
}

////格式化重写
//function OverForeignKeyFormatter(value, row, index, moduleName, fieldName, foreignModuleName, paramsObj) {
//    if (moduleName == '视图字段' && fieldName == 'Sys_FieldId') { //模块字段
//        return row['Display'];
//    }
//    return value ? value : '';
//}