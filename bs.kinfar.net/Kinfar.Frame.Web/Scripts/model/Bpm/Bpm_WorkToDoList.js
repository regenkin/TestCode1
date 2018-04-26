
//重写主键格式化
function OverTitleKeyFormatter(value, row, index, moduleName, fieldName, paramsObj, otherParams) {
    var v = value != undefined && value != null ? value : "";
    if (fieldName == "Code" || fieldName == "Title") {
        return "<a href='jascript:void(0);' onclick=ViewRecord('" + row.Id + "','" + row.ModuleId + "','" + row.RecordId + "','" + row.Title + "') >" + value + "</a>";
    }
    return v;
}

//重写通用格式化
function OverGeneralFormatter(value, row, index, moduleName, fieldName, paramsObj) {
    if (fieldName == "Code" || fieldName == "Title") {
        return OverTitleKeyFormatter(value, row, index, moduleName, fieldName, paramsObj);
    }
    var v = value != undefined && value != null ? value : "";
    return v;
}

//重写外键格式化
function OverForeignKeyFormatter(value, row, index, moduleName, fieldName, foreignModuleName, paramsObj, otherForeignParams) {
    var v = value != undefined && value != null ? value : "";
    if (fieldName == "OrgM_EmpName") {
        return "<span>" + v + "</span>";
    }
    return v;
}

//查看记录
function ViewRecord(todoId, moduleId, recordId, title) {
    var url = "/Page/EditForm.html?page=edit&editMode=1&moduleId=" + moduleId + "&id=" + recordId + "&todoId=" + todoId;
    var currTabIndex = GetSelectTabIndex(); //当前grid网格页面的tabindex
    if (currTabIndex)
        url += "&tb=" + currTabIndex;
    url += "&r=" + Math.random();
    AddTab(null, title, url);
}