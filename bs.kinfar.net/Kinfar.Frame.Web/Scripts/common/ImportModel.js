
//下载导入模板
function DownImportTemp(obj) {
    var moduleId = $(obj).attr("moduleId");
    var downLoadUrl = '/' + CommonController.Async_Data_Controller + '/DownImportTemplate.html?moduleId=' + moduleId;
    window.open(downLoadUrl);
}

//上传模板数据文件
//moduleId:模块Id
//backFun:上传成功后的回调函数
function UploadTempData(backFun) {
    var file = $('#file').val();
    if (!file) {
        top.showAlertMsg("提示", "请选择要上传的文件！", "info");
        return;
    }
    $("#form_Import").ajaxSubmit({
        type: "post",
        url: "/Annex/UploadTempImportFile.html",
        beforeSubmit: function () {
            top.openWaitDialog('处理中...');
        },
        success: function (result) {
            top.closeWaitDialog();
            if (!result.Success) {
                top.showAlertMsg('提示', result.Message, 'info');
            }
            else {
                if (typeof (backFun) == "function") {
                    backFun(result.FilePath);
                }
            }
        },
        error: function (err) {
            top.showAlertMsg('提示', '数据上传失败，服务端异常！', 'error', function () {
                top.closeWaitDialog();
            });
        },
        dataType: "json"
    });
}