var moduleId = GetLocalQueryString("moduleId");
var moduleName = GetLocalQueryString("moduleName");
var isMutiSelect = GetLocalQueryString("ms") == '1';

$(function () {
    //加载组织树
    $("#tree").tree({
        checkbox: isMutiSelect, //显示复选框
        cascadeCheck: true,//定义是否层叠选中状态
        url: '/' + CommonController.Async_Data_Controller + '/GetTreeByNode.html?moduleId=' + moduleId + '&moduleName=' + moduleName,
        loadFilter: function (data) {
            if (data == null) return data;
            var lastData = null;
            if (typeof (data) == 'string') {
                var tempData = eval("(" + data + ")");
                lastData = tempData;
            }
            else {
                arr = [];
                arr.push(data);
                lastData = arr;
            }
            if (typeof (OverDialogTreeLoadFilter) == "function") {
                lastData = OverDialogTreeLoadFilter(lastData, moduleName, moduleId);
            }
            return lastData;
        },
        onLoadSuccess: function () {
            $("#tree").tree("collapseAll");
            var roots = $("#tree").tree("getRoots"); //展开所有根结点
            if (roots && roots.length > 0) {
                $.each(roots, function (i, root) {
                    $("#tree").tree("expand", root.target);
                });
            }
        },
        onSelect: function (node) {
            var item = $("#selectedNodeList").find("span[targetid='" + node.target.id + "']");
            if (item.length <= 0) {
                AddSelectedNode(node);
            }
        },
        onCheck: function (node, checked) {
            var dom = $("#tree");
            var item = $("#selectedNodeList").find("span[targetid='" + node.target.id + "']");
            //选择叶节点
            var flag = dom.tree("isLeaf", node.target);
            if (flag) {
                if (checked && item.length <= 0) {
                    AddSelectedNode(node);
                }
                else if (!checked && item.length > 0) {
                    item.parent().remove();
                }
            }//选择非叶节点
            else {
                var children = dom.tree("getChildren", node.target);
                $(children).each(function () {
                    var cnode = $(this)[0];
                    var it = $("#selectedNodeList").find("span[targetid='" + cnode.target.id + "']");
                    if (cnode.checked && it.length <= 0) {
                        AddSelectedNode(cnode);
                    }
                    else if (!cnode.checked && it.length > 0) {
                        it.parent().remove();
                    }
                });
            }
        }
    });
});

//搜索节点
function SearchNode(value) {
    var dom = $("#tree");
    var target = $("#tree li span.tree-title:contains('" + value + "')").parent();
    if (target.length <= 0) {
        top.showMsg("提示", "未找到任何相关节点");
        return;
    }
    $(target).each(function () {
        var tt = $(this);
        var parentNode = dom.tree("getParent", tt);
        while (parentNode != null) {
            if (parentNode != null) {
                dom.tree("expand", parentNode.target);
                parentNode = dom.tree("getParent", parentNode.target);
            }
        }
    });
    dom.tree("select", target);
}

//已设置或选择添加记录
//node:节点
function AddSelectedNode(node) {
    if (!node) return;
    var nodeList = $("#selectedNodeList");
    if (!isMutiSelect) nodeList.html('');
    var dom = document.createDocumentFragment();
    var span = document.createElement("span");
    var title = "删除"
    var clickMethod = "UnSelect(this)";
    var spClass = "attaDelete";
    var spText = "删除";
    $(span).attr("class", "attaItem");
    var a = document.createElement("a");
    $(a).attr("href", "javascript:void(0);");
    $(a).text(node.text);
    $(a).attr("dataId", node.id);
    var sp = document.createElement("span");
    $(sp).attr("class", spClass);
    $(sp).attr("title", title);
    $(sp).attr("onclick", clickMethod);
    $(sp).attr("targetId", node.target.id);
    $(sp).text(spText);
    span.appendChild(a);
    span.appendChild(sp);
    dom.appendChild(span);
    nodeList[0].appendChild(dom);
}

//取消选择
function UnSelect(obj) {
    var targetId = $(obj).attr("targetId");
    var target = $("#" + targetId);
    var dom = $("#tree");
    dom.tree("uncheck", target);
    $(obj).parent("span.attaItem").remove();
}

//获取已选数据
//isMutiSelect:是否多选
function GetSelectData() {
    var data = [];
    $("#selectedNodeList a").each(function (i, item) {
        var dataId = $(this).attr("dataId");
        if (dataId) {
            var dataText = $(this).text();
            var obj = { Id: dataId, Name: dataText };
            data.push(obj);
        }
    });
    return isMutiSelect ? data : data[0];
}