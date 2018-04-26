/// <reference path="jquery-1.4.1-vsdoc.js" />
/*
*  DragFlow JQuery方法类，主要包括：
*  1、初始化端点、设置目标节点、源节点；
*  2、设置右键菜单；
*  3、设置所有元素的可拖拽性、目标节点、源节点等
*  4、设置右键菜单；
*  Author:limq
*  date:2012.12.08
*/
(function ($) {
    // 初始化端点，把所有带有ep样式元素的父节点设置为源节点
    DragFlow.initEndpoints = function (nextColour) {
        $(".ep").each(function (i, e) {
            var p = $(e).parent();
            jsPlumb.makeSource($(e),
            {
                parent: p,
                //anchor:"BottomCenter",
                anchor: "Continuous",
                connector: ["Flowchart", { curviness: 20 }],
                connectorStyle: { strokeStyle: "black", lineWidth: 1 },
                endpoint: "Blank",
                maxConnections: 5,
                onMaxConnections: function (info, e) {
                    alert("Maximum connections (" + info.maxConnections + ") reached");
                }
            });
        });
    };
    // 设置元素为连接源节点
    DragFlow.makeSourceById = function (newid) {
        jsPlumb.makeSource($("#" + newid + "").children(".ep"),
                            {
                                parent: newid,
                                anchor: "Continuous",
                                connector: ["Flowchart", { curviness: 20 }],
                                connectorStyle: { strokeStyle: "black", lineWidth: 1 },
                                endpoint: "Blank",
                                maxConnections: 4,
                                onMaxConnections: function (info, e) {
                                    alert("最大连接数为 (" + info.maxConnections + ") ！！");
                                }
                            });
    }
    // 设置连接目标节点
    DragFlow.makeTargetById = function (newid) {
        jsPlumb.makeTarget(newid, {
            dropOptions: { hoverClass: "dragHover" },
            anchor: "Continuous",
            endpoint: "Blank"
            //anchor:"TopCenter"			
        });
    }
    // 设置元素右键菜单
    DragFlow.makecontextmenu = function (newid, isAllElements) {
        var els = $("#" + newid);
        if (isAllElements) {
            els = $(".component");
        }

        els.contextMenu('myMenu1',
             {
                 bindings:
                  {
                      'edit': function (t) {
                          // 弹出编辑页面
                          editNode(t);
                      },
                      'delete': function (t) {
                          // 执行删除操作
                          deletenote(t);
                      },
                      'insert_before': function (t) {
                          // 之前插入
                          insert_before(t);
                      },
                      'insert_next': function (t) {
                          // 之后插入
                          insert_next(t);
                      }
                  }
             });
    }




})(jQuery);