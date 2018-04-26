
//表单加载完成后
function OverFormLoadCompleted() {
    var parentBtn = $('#ParentId').combobox('getValue');
    if (page == "edit" && !parentBtn) {
        var sysModuleId = $('#Sys_ModuleId').combobox('getValue');
        var url = '/' + CommonController.Async_System_Controller + '/LoadViewButtons.html?moduleId=' + sysModuleId;
        $('#ParentId').combobox('clear').combobox('reload', url);
    }
}

//重写字段选择事件
function OverOnFieldSelect(record, fieldName, valueField, textField) {
    if (fieldName == "Sys_ModuleId") { //新增时选择模块后加载该模块对应的按钮
        var url = '/' + CommonController.Async_System_Controller + '/LoadViewButtons.html?moduleId=' + record[valueField];
        $('#ParentId').combobox('clear').combobox('reload', url);
    }
    else if (fieldName == 'ParentId') { //选择上级按钮
        if (page == "edit") { //编辑页面不能将当前按钮设置为当前按钮上级
            var selectParentBtn = $('#ParentId').combobox('getValue');
            if (selectParentBtn == id) {
                top.showAlertMsg('提示', '不能将自己设置为上级！', 'info', function () {
                    $('#ParentId').combobox('clear');
                });
            }
        }
    }
}

//重写字段值发生改变事件
function OverOnFieldValueChanged(moduleInfo, fieldName, newValue, oldValue) {
    if (fieldName == "ButtonText" && page == "add") { //按钮显示名称
        $.getScript("/Scripts/jquery-plug/jquery.hz2py-min.js", function () {
            var py = $('#ButtonText').toPinyin();
            $("#ButtonTagId").textbox('setText', 'btn' + py);
        });
    }
}

//添加常用按钮
function AddCommonBtn() {
    var moduleId = 0;
    var row = GetSelectRow(); //获取选中行
    if (row) {
        moduleId = row["Sys_ModuleId"];
    }
    var toolbar = [{
        id: 'btnOk',
        text: "确 定",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            var iframe = top.getCurrentDialogFrame();
            var data = iframe[0].contentWindow.GetData();
            if (!data.moduleId) {
                top.showAlertMsg('提示', '请选择模块！', 'info');
                return;
            }
            $.ajax({
                type: 'post',
                dataType: 'json',
                url: '/' + CommonController.Async_System_Controller + '/SaveCommonBtn.html',
                data: { moduleId: data.moduleId, indexs: data.selectItems },
                beforeSend: function () {
                    top.openWaitDialog('正在处理中...');
                },
                success: function (result) {
                    if (!result) return;
                    if (result.Success) {
                        top.showMsg('提示', '常用按钮添加/移除成功！', function () {
                            top.closeWaitDialog();
                            top.closeDialog();
                            RefreshGrid();
                        });
                    }
                    else {
                        top.showAlertMsg('提示', result.Message, "info", function () {
                            top.closeWaitDialog();
                        });
                    }
                },
                error: function () {
                    top.showAlertMsg('提示', '常用按钮添加/移除失败，服务器异常！', "error", function () {
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
    var url = '/Page/AddCommonBtn.html?moduleId=' + moduleId + '&r=' + Math.random();
    top.openDialog('添加/移除常用按钮', url, toolbar, 400, 250, 'eu-p2-icon-add_other');
}