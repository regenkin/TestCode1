
//主键字段格式化
//value:字段值
//row:行对象
//index:行号 
//moduleName:当前模块信息
//fieldName:当前字段名称
//paramsObj:字段参数对象
//otherParams：模块其他参数
function TitleKeyFormatter(value, row, index, moduleName, fieldName, paramsObj, otherParams) {
    var v = value != undefined && value != null ? value : "";
    if (typeof (OverTitleKeyFormatter) == 'function') {
        v = OverTitleKeyFormatter(value, row, index, moduleName, fieldName, paramsObj, otherParams);
        if (v == null || v == undefined) v = "";
        return v;
    }
    var recordId = row["Id"];
    if (v.length > 0 && otherParams && otherParams.length > 0) {
        var obj = eval("(" + decodeURIComponent(otherParams) + ")");
        var clickMethod = "ViewRecord(this)"; //默认点击标识字段值链接查看记录
        var isDraft = GetLocalQueryString("draft"); //是否我的草稿列表页
        if (parseInt(isDraft) == 1) { //我的草稿列表时，点击标识字段链接进入编辑表单
            clickMethod = "Edit(this)";
        }
        v = "<a href='#' recycle='" + obj.recycle + "' recordId='" + recordId + "' moduleId='" + obj.moduleId + "' moduleName='" + moduleName + "' titleKey='" + obj.titleKey + "' titleKeyDisplay='" + obj.titleKeyDisplay + "' editMode='" + obj.editMode + "' editWidth='" + obj.editWidth + "' editHeight='" + obj.editHeight + "' onclick='" + clickMethod + "'>" + v + "</a>";
    }
    if (paramsObj && paramsObj.length > 0) {
        var obj = eval("(" + decodeURIComponent(paramsObj) + ")");
        var editFlag = "<img class='editFlag' gridId='" + obj.gridId + "' moduleId='" + obj.moduleId + "' fieldName='" + obj.fieldName + "' recordId='" + recordId + "' fieldDisplay='" + obj.fieldDisplay + "' fieldWidth='" + obj.fieldWidth + "' oldValue='" + value + "' onclick='EditField(this)' style='margin-left:5px;cursor:pointer;display:none;' src='/Css/icons/docEdit.png' />"
        return "<span>" + v + editFlag + "</span>";
    }
    return v;
}

//外键字段格式化
//value:字段值
//row:行对象
//index:行号 
//moduleName:当前模块名称
//fieldName:当前字段名称
//foreignModuleName:外键模块名称
//paramsObj:字段参数对象
//otherForeignParams:外键模块其他参数
function ForeignKeyFormatter(value, row, index, moduleName, fieldName, foreignModuleName, paramsObj, otherForeignParams) {
    var v = value != undefined && value != null ? value : "";
    if (typeof (OverForeignKeyFormatter) == 'function') {
        v = OverForeignKeyFormatter(value, row, index, moduleName, fieldName, foreignModuleName, paramsObj, otherForeignParams);
        if (v == null || v == undefined) v = "";
        return v;
    }
    if (v.length > 0 && otherForeignParams && otherForeignParams.length > 0) {
        var obj = eval("(" + decodeURIComponent(otherForeignParams) + ")");
        var foreignRecordId = row[fieldName];
        var foreignNameField = fieldName.substr(0, fieldName.length - 2) + "Name";
        var titleKeyValue = row[foreignNameField];
        v = "<a href='#' recordId='" + foreignRecordId + "' titleKeyValue='" + titleKeyValue + "' moduleId='" + obj.moduleId + "' moduleName='" + foreignModuleName + "' titleKey='" + obj.titleKey + "' titleKeyDisplay='" + obj.titleKeyDisplay + "' editMode='" + obj.editMode + "' editWidth='" + obj.editWidth + "' editHeight='" + obj.editHeight + "' onclick='ViewRecord(this)'>" + v + "</a>";
    }
    if (paramsObj && paramsObj.length > 0) {
        var obj = eval("(" + decodeURIComponent(paramsObj) + ")");
        var editFlag = "<img class='editFlag' gridId='" + obj.gridId + "' moduleId='" + obj.moduleId + "' fieldName='" + obj.fieldName + "' recordId='" + row["Id"] + "' fieldDisplay='" + obj.fieldDisplay + "' fieldWidth='" + obj.fieldWidth + "' oldValue='" + value + "' onclick='EditField(this)' style='margin-left:5px;cursor:pointer;display:none;' src='/Css/icons/docEdit.png' />"
        return "<span>" + v + editFlag + "</span>";
    }
    return v;
}

//枚举字段格式化
//value:字段值
//row:行对象
//index:行号 
//moduleName:当前模块名称
//fieldName:当前字段名称
//enumData:枚举数据
//paramsObj:字段参数对象
function EnumFieldFormatter(value, row, index, moduleName, fieldName, enumData, paramsObj) {
    var v = value != undefined && value != null ? value : "";
    if (typeof (OverEnumFieldFormatter) == 'function') {
        v = OverEnumFieldFormatter(value, row, index, moduleName, fieldName, enumData, paramsObj);
        if (v == null || v == undefined) v = "";
        return v;
    }
    else {
        if ((value || value == 0) && enumData && enumData.length > 0) {
            var dic = eval("(" + decodeURIComponent(enumData) + ")");
            if (dic && dic.length > 0) {
                for (var i = 0; i < dic.length; i++) {
                    var tempV = dic[i].Id;
                    var n = dic[i].Name;
                    if (value == tempV && tempV != "") {
                        v = n;
                        break;
                    }
                }
            }
        }
    }
    if (paramsObj && paramsObj.length > 0) {
        var obj = eval("(" + decodeURIComponent(paramsObj) + ")");
        var editFlag = "<img class='editFlag' gridId='" + obj.gridId + "' moduleId='" + obj.moduleId + "' fieldName='" + obj.fieldName + "' recordId='" + row["Id"] + "' fieldDisplay='" + obj.fieldDisplay + "' fieldWidth='" + obj.fieldWidth + "' oldValue='" + value + "' onclick='EditField(this)' style='margin-left:5px;cursor:pointer;display:none;' src='/Css/icons/docEdit.png' />"
        return "<span>" + v + editFlag + "</span>";
    }
    return v;
}

//字典绑定字段格式化
//value:字段值
//row:行对象
//index:行号 
//moduleName:当前模块名称
//fieldName:当前字段名称
//dicData:字典数据
//paramsObj:字段参数对象
function DicFieldFormatter(value, row, index, moduleName, fieldName, dicData, paramsObj) {
    var v = value != undefined && value != null ? value : "";
    if (typeof (OverDicFieldFormatter) == 'function') {
        v = OverDicFieldFormatter(value, row, index, moduleName, fieldName, dicData, paramsObj);
        if (v == null || v == undefined) v = "";
        return v;
    }
    else {
        if ((value || value == 0) && dicData && dicData.length > 0) {
            var dic = eval("(" + decodeURIComponent(dicData) + ")");
            if (dic && dic.length > 0) {
                for (var i = 0; i < dic.length; i++) {
                    var tempV = dic[i].Id;
                    var n = dic[i].Name;
                    if (value == tempV && tempV != "") {
                        v = n;
                        break;
                    }
                }
            }
        }
    }
    if (paramsObj && paramsObj.length > 0) {
        var obj = eval("(" + decodeURIComponent(paramsObj) + ")");
        var editFlag = "<img class='editFlag' gridId='" + obj.gridId + "' moduleId='" + obj.moduleId + "' fieldName='" + obj.fieldName + "' recordId='" + row["Id"] + "' fieldDisplay='" + obj.fieldDisplay + "' fieldWidth='" + obj.fieldWidth + "' oldValue='" + value + "' onclick='EditField(this)' style='margin-left:5px;cursor:pointer;display:none;' src='/Css/icons/docEdit.png' />"
        return "<span>" + v + editFlag + "</span>";
    }
    return v;
}

//多选checkbox格式化
//value:字段值
//row:行对象
//index:行号 
//moduleName:当前模块名称
//fieldName:当前字段名称
//defaultTexts:checkbox的text显示名称
//paramsObj:字段参数对象
function MutiCheckBoxFormatter(value, row, index, moduleName, fieldName, defaultTexts, paramsObj) {
    var v = value != undefined && value != null ? value : "";
    if (typeof (OverMutiCheckBoxFormatter) == 'function') {
        v = OverMutiCheckBoxFormatter(value, row, index, moduleName, fieldName, defaultTexts, paramsObj);
        if (v == null || v == undefined) v = "";
        return v;
    }
    else if (v.length > 0 && defaultTexts != undefined && defaultTexts != null && defaultTexts.length > 0) {
        var token1 = v.split(',');
        var token2 = defaultTexts.split(',');
        if (token1.length == token2.length) {
            var str = '';
            for (var i = 0; i < token1.length; i++) {
                if (token1[i] != "1") continue;
                str += token2[i] + ',';
            }
            if (str.length > 0) {
                str = str.substr(0, str.length - 1);
                return "<span title='" + str + "'>" + str + "</span>";
            }
        }
    }
    return v;
}

//日期字段格式化
//value:字段值
//row:行对象
//index:行号 
//moduleName:当前模块名称
//fieldName:当前字段名称
//format:格式化字符串
//paramsObj:字段参数对象
function DateFormatter(value, row, index, moduleName, fieldName, format, paramsObj) {
    var v = value != undefined && value != null ? value : "";
    if (typeof (OverDateFormatter) == 'function') {
        v = OverDateFormatter(value, row, index, moduleName, fieldName, format, paramsObj);
        if (v == null || v == undefined) v = "";
        return v;
    }
    if (paramsObj && paramsObj.length > 0) {
        var obj = eval("(" + decodeURIComponent(paramsObj) + ")");
        var editFlag = "<img class='editFlag' gridId='" + obj.gridId + "' moduleId='" + obj.moduleId + "' fieldName='" + obj.fieldName + "' recordId='" + row["Id"] + "' fieldDisplay='" + obj.fieldDisplay + "' fieldWidth='" + obj.fieldWidth + "' oldValue='" + value + "' onclick='EditField(this)' style='margin-left:5px;cursor:pointer;display:none;' src='/Css/icons/docEdit.png' />"
        return "<span>" + v + editFlag + "</span>";
    }
    return v;
}

//布尔型字段格式化
//value:字段值
//row:行对象
//index:行号 
//moduleName:当前模块名称
//fieldName:当前字段名称
//paramsObj:字段参数对象
function BoolFormatter(value, row, index, moduleName, fieldName, paramsObj) {
    var v = (value == true || value == "true" || value == "True") ? "是" : "否";;
    if (typeof (OverBoolFormatter) == 'function') {
        v = OverBoolFormatter(value, row, index, moduleName, fieldName, paramsObj);
        if (v == null || v == undefined) v = "";
        return v;
    }
    if (paramsObj && paramsObj.length > 0) {
        var obj = eval("(" + decodeURIComponent(paramsObj) + ")");
        var editFlag = "<img class='editFlag' gridId='" + obj.gridId + "' moduleId='" + obj.moduleId + "' fieldName='" + obj.fieldName + "' recordId='" + row["Id"] + "' fieldDisplay='" + obj.fieldDisplay + "' fieldWidth='" + obj.fieldWidth + "' oldValue='" + value + "' onclick='EditField(this)' style='margin-left:5px;margin-top:5px;cursor:pointer;display:none;' src='/Css/icons/docEdit.png' />"
        return "<span>" + v + editFlag + "</span>";
    }
    return v;
}

//通用字段格式化
//value:字段值
//row:行对象
//index:行号 
//moduleName:当前模块名称
//fieldName:当前字段名称
//paramsObj:字段参数对象
function GeneralFormatter(value, row, index, moduleName, fieldName, paramsObj) {
    var v = value != undefined && value != null ? "<span title='" + value + "'>" + value + "</span>" : "";
    if (typeof (OverGeneralFormatter) == 'function') {
        v = OverGeneralFormatter(value, row, index, moduleName, fieldName, paramsObj);
        if (v == null || v == undefined) v = "";
        return v;
    }
    if (paramsObj && paramsObj.length > 0) {
        var obj = eval("(" + decodeURIComponent(paramsObj) + ")");
        var editFlag = "<img class='editFlag' gridId='" + obj.gridId + "' moduleId='" + obj.moduleId + "' fieldName='" + obj.fieldName + "' recordId='" + row["Id"] + "' fieldDisplay='" + obj.fieldDisplay + "' fieldWidth='" + obj.fieldWidth + "' oldValue='" + value + "' onclick='EditField(this)' style='margin-left:5px;cursor:pointer;display:none;' src='/Css/icons/docEdit.png' />"
        return v + editFlag;
    }
    return v;
}

//行操作按钮格式化
//value:字段值
//row:行对象
//index:行号 
//moduleId:当前模块ID
//moduleName:当前模块名称
function RowOperateBtnFormat(value, row, index, moduleId, moduleName) {
    var btnDom = $("#txtRowOperateBtn_" + moduleId);
    var btnJson = btnDom.val();
    if (btnJson && btnJson.length > 0) {
        btnJson = decodeURIComponent(btnJson);
        var btns = eval("(" + btnJson + ")");
        if (btns && btns.length > 0) {
            var titleKey = btnDom.attr("titleKey");
            var titleKeyDisplay = btnDom.attr("titleKeyDisplay");
            var editMode = parseInt(btnDom.attr("editMode"));
            var editWidth = btnDom.attr("editWidth");
            var editHeight = btnDom.attr("editHeight");
            var recordId = 0;
            if (row["Id"]) {
                recordId = row["Id"];
            }
            var tag = moduleId + "_" + recordId;
            var a = "<div id=\"rowOperateDiv_" + tag + "\" style=\"display:" + (editMode == 3 && recordId == 0 ? "none" : "block") + ";\">";
            var a_attr = "recordId=\"" + recordId + "\" moduleId=\"" + moduleId + "\" moduleName=\"" + moduleName + "\" titleKey=\"" + titleKey + "\" titleKeyDisplay=\"" + titleKeyDisplay + "\" editMode=\"" + editMode + "\" editWidth=\"" + editWidth + "\" editHeight=\"" + editHeight + "\"";
            $(btns).each(function (i, btn) {
                if (btn.ClickMethod == null) btn.ClickMethod = '';
                if (btn.ButtonIcon == null) btn.ButtonIcon = '';
                if (btn.ButtonText == null) btn.ButtonText = '';
                a += "<a rowOperateBtn=\"1\" id=\"" + btn.ButtonTagId + "_" + tag + "\" clickMethod=\"" + btn.ClickMethod + "\" icon=\"" + btn.ButtonIcon + "\" btnText=\"" + btn.ButtonText + "\" " + a_attr + "></a>";
            });
            a += "</div>";
            if (editMode == 3) {
                a += "<div id=\"rowOkDiv_" + tag + "\" style=\"display:" + (recordId == 0 ? "block" : "none") + ";\">";
                a += "<a id=\"rowOkBtn_" + tag + "\" " + a_attr + "></a>";
                a += "<a id=\"rowCancelBtn_" + tag + "\" " + a_attr + "></a>";
                a += "</div>";
            }
            return a;
        }
    }
    return value;
}
