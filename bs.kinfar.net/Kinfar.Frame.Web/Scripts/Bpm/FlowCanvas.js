
var gooFlowObj = null; //流程对象
var workflowId = GetLocalQueryString("workflowId"); //当前流程ID
var workName = decodeURI(GetLocalQueryString("name")); //当前流程名称

//初始化
$(document).ready(function () {
    //流程画面对象初始化
    if (workName) $("#gooFlowDom").attr('title', workName);
    var property = {
        width: GetBodyWidth(), height: GetBodyHeight(),
        toolBtns: ["start round", "end round", "task round"],
        haveHead: true, headBtns: ["new", "import", "export", "save", "undo", "redo", "reload"],//如果haveHead=true，则定义HEAD区的按钮
        haveTool: true, haveGroup: false, useOperStack: true
    };
    gooFlowObj = $.createGooFlow($("#gooFlowDom"), property);
    var remark = { cursor: "选择指针", start: "开始结点", "end": "结束结点", "task": "任务结点" };
    gooFlowObj.setNodeRemarks(remark);
    //删除节点
    gooFlowObj.onItemDel = function (id, type) {
        top.showConfirmMsg('提示', '确定要删除该单元吗?', function (action) {
            if (action) {
                if (type == 'node') {
                    gooFlowObj.delNode(id, true);
                }
                else if (type == 'line') {
                    gooFlowObj.delLine(id, true);
                }
            }
        });
        return false;
    };
    //新建流程
    gooFlowObj.onBtnNewClick = function () {
        parent.AddNewFlow();
    };
    //导入流程
    gooFlowObj.onBtnImportClick = function () { };
    //导出流程
    gooFlowObj.onBtnExportClick = function () {

    };
    //保存流程
    gooFlowObj.onBtnSaveClick = function () {
        if (workflowId) {
            var startNodeNum = $("div[id^='gooFlowDom_start']").length;
            var endNodeNum = $("div[id^='gooFlowDom_end']").length;
            if (startNodeNum > 1 || endNodeNum > 1) {
                top.showMsg('提示', '只能有一个开始结点和一个结束结点！');
                return;
            }
            if (startNodeNum == 0 || endNodeNum == 0) {
                top.showMsg('提示', '必须有一个开始结点和一个结束结点！');
                return;
            }
            var url = '/' + CommonController.Async_Bpm_Controller + '/UpdateWorkflowChart.html';
            var workLines = [];
            var workNodes = [];
            $("div[id^='gooFlowDom_task'],g[id^='gooFlowDom_line'],div[id^='gooFlowDom_start'],div[id^='gooFlowDom_end']").each(function (i, item) {
                var tagId = $(this).attr('id');
                var params = $(this).attr('params');
                var paramObj = null;
                if (params != undefined && params != null && params.length > 0) {
                    paramObj = JSON.parse(decodeURI(params));
                }
                if (paramObj == null)
                    paramObj = { TagId: tagId };
                else
                    paramObj.TagId = tagId;
                if (tagId.indexOf('gooFlowDom_line') > -1) { //连接线
                    var line = gooFlowObj.getItemInfo(tagId, 'line');
                    paramObj.FromTagId = line['from'];
                    paramObj.ToTagId = line['to'];
                    paramObj.M = line['M'];
                    paramObj.Note = line['name'];
                    workLines.push(paramObj);
                }
                else { //普通结点
                    var node = gooFlowObj.getItemInfo(tagId, 'node');
                    paramObj.Name = node['name'];
                    paramObj.Top = node['top'];
                    paramObj.Left = node['left'];
                    paramObj.Width = node['width'];
                    paramObj.Height = node['height'];
                    if (tagId.indexOf('gooFlowDom_start') > -1)
                        paramObj.WorkNodeType = 0;
                    else if (tagId.indexOf('gooFlowDom_end') > -1)
                        paramObj.WorkNodeType = 1;
                    else
                        paramObj.WorkNodeType = 2;
                    workNodes.push(paramObj);
                }
            });
            if (workNodes.length == 0) {
                top.showMsg('提示', '请至少添加一个流程节点！');
                return;
            }
            var workflow = { Id: workflowId, WorkNodes: workNodes, WorkLines: workLines };
            var params = { workflowJson: $.base64.encode(escape(JSON.stringify(workflow))) };
            ExecuteCommonAjax(url, params, null, true);
        }
    };
    //重新加载流程
    gooFlowObj.onFreshClick = function () {
        window.location.reload(true);
    };
    //结点参数设置
    var NodeParamsSet = function (tagId) {
        var nodeDom = $('#' + tagId);
        var nodeName = nodeDom.find('.span').text();
        var url = "/" + CommonController.Bpm_Controller + "/NodeParamSet.html?workflowId=" + workflowId + "&tagId=" + tagId + "&name=" + encodeURI(nodeName);
        top.openOkCancelDialog('结点参数设置－【' + nodeName + '】', url, 650, 480, function (iframe, backFun) {
            //获取节点参数
            var nodeParams = iframe.contentWindow.GetNodeParams();
            nodeDom.attr('params', encodeURI(JSON.stringify(nodeParams)));
            gooFlowObj.setName(tagId, nodeParams.Name, 'node');
            if (typeof (backFun) == "function")
                backFun(true);
        }, null, function (divDomId, iframe) {
            var divDom = top.$('#' + divDomId);
            var paramsTemp = nodeDom.attr('params');
            if (paramsTemp != undefined && paramsTemp) {
                divDom.attr('params', paramsTemp);
            }
            else {
                divDom.removeAttr('params');
            }
        });
    };
    //连线参数设置
    var LineParamsSet = function (tagId) {
        var lineDom = $('#' + tagId);
        var url = "/" + CommonController.Bpm_Controller + "/LineParamSet.html?workflowId=" + workflowId + "&tagId=" + tagId;
        top.openOkCancelDialog('连线参数设置', url, 650, 450, function (iframe, backFun) {
            //获取节点参数
            var lineParams = iframe.contentWindow.GetLineParams();
            lineDom.attr('params', encodeURI(JSON.stringify(lineParams)));
            gooFlowObj.setName(tagId, lineParams.Note, 'line');
            if (typeof (backFun) == "function")
                backFun(true);
        }, null, function (divDomId, iframe) {
            var divDom = top.$('#' + divDomId);
            var paramsTemp = lineDom.attr('params');
            if (paramsTemp != undefined && paramsTemp) {
                divDom.attr('params', paramsTemp);
            }
            else {
                divDom.removeAttr('params');
            }
        });
    };
    //结点双击事件
    gooFlowObj.onItemNodeDbClick = function (id) {
        NodeParamsSet(id);
    };
    //连线双击事件
    gooFlowObj.onItemLineDbClick = function (id) {
        LineParamsSet(id);
    };
    //绑定结点、连线右键菜单
    $("div[id^='gooFlowDom_task'],g[id^='gooFlowDom_line']").live('contextmenu', function (e) {
        var id = $(this).attr('id');
        if (id.indexOf('gooFlowDom_line') > -1) {
            var line = gooFlowObj.getItemInfo(id, 'line');
            if (line['from'].indexOf('gooFlowDom_start') > -1 ||
               line['to'].indexOf('gooFlowDom_end') > -1)
                return; //连接开始结点和结束结点的连线不显示右键菜单
        }
        $('#mm-chart').menu('show', {
            left: e.pageX,
            top: e.pageY
        });
        $('#mm-chart').data('tagId', id);
        return false;
    });
    //参数设置
    $('#mm-set').click(function (e) {
        var tagId = $('#mm-chart').data('tagId');
        if (tagId.indexOf('gooFlowDom_task') > -1) {
            NodeParamsSet(tagId);
        }
        else {
            LineParamsSet(tagId);
        }
    });
    //加载流程图
    $.get('/' + CommonController.Async_Bpm_Controller + '/LoadWorkflowChart.html', { workflowId: workflowId }, function (data) {
        if (data && data.FlowData) {
            gooFlowObj.loadData(data.FlowData);
            if (data.NodeParams) {
                for (var tagId in data.NodeParams) {
                    var nodeDom = $('#' + tagId);
                    nodeDom.attr('params', encodeURI(JSON.stringify(data.NodeParams[tagId])));
                }
            }
            if (data.LineParams) {
                for (var tagId in data.LineParams) {
                    var lineDom = $('#' + tagId);
                    lineDom.attr('params', encodeURI(JSON.stringify(data.LineParams[tagId])));
                }
            }
        }
    }, 'json');
});
