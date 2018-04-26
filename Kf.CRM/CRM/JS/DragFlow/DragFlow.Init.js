/// <reference path="jquery-1.4.1-vsdoc.js" />
/*
*  DragFlow初始化类，主要包括：
*  1、初始化流程默认显示样式；
*  2、解析流程步骤及步骤关系；
*  3、设置所有元素的可拖拽性、目标节点、源节点等
*  4、设置右键菜单；
*  Author:limq
*  date:2012.12.08
*/
var stateMachineConnector;
(function () {

    window.DragFlow = {

        init: function () {
            // jsplum默认样式
            jsPlumb.importDefaults({
                DragOptions: { cursor: "pointer", zIndex: 2000 },
                HoverClass: "connector-hover",
                HoverPaintStyle: { strokeStyle: "#7ec3d9" }
                ,
                PaintStyle: {
                    lineWidth: 1,
                    strokeStyle: "black"
                },
                Overlays: [["PlainArrow", { location: 1, width: 10, length: 12 }]]
            });

            var connectorStrokeColor = "rgba(50, 50, 200, 1)",
				connectorHighlightStrokeColor = "rgba(180, 180, 200, 1)",
				hoverPaintStyle = { strokeStyle: "#7ec3d9" }; 		// hover paint style is merged on normal style, so you 
            // don't necessarily need to specify a lineWidth



            stateMachineConnector = {
                connector: "Flowchart",
                paintStyle: { lineWidth: 1, strokeStyle: "black" },
                hoverPaintStyle: { strokeStyle: "#dbe300" },
                endpoint: "Blank",
                anchor: "Continuous",
                overlays: [["PlainArrow", { location: 1, width: 10, length: 12 }]]
            };
            //jsPlumb.connect({ source: "flow_start", target: "flow_end" }, stateMachineConnector);


            //double click on any connection 
            //jsPlumb.bind("dblclick", function (connection, originalEvent) { alert("double click on connection from " + connection.sourceId + " to " + connection.targetId); });
            //single click on any endpoint
            //jsPlumb.bind("endpointClick", function (endpoint, originalEvent) { alert("click on endpoint on element " + endpoint.elementId); });
            //context menu (right click) on any component.
            //jsPlumb.bind("contextmenu", function (component, originalEvent) {
            //    alert("context menu on component " + component.id);
            //    originalEvent.preventDefault();
            //    return false;
            //});
            // 链接创建成功事件，在此事件中可以加入增加链接的逻辑
            //jsPlumb.bind("jsPlumbConnection", function (conn) {
            //    conn.connection.getOverlay("label").setLabel(conn.connection.id);
            //});
            // 链接双击事件，在此事件中加入删除链接的逻辑
            jsPlumb.bind("dblclick", function (c) {
                //jsPlumb.detach(c);
            });

            //DragFlow.initEndpoints("");
            // 设置所有节点为连接目标节点
            //jsPlumb.makeTarget(jsPlumb.getSelector(".component"), {
            //    dropOptions: { hoverClass: "dragHover" },
            //    anchor: "Continuous",
            //    endpoint: "Blank"
            //    //anchor:"TopCenter"			
            //});


            // make all .window divs draggable
            jsPlumb.draggable(jsPlumb.getSelector(".component"));

            //            $(jsPlumb.getSelector(".window")).draggable(
            //            {
            //                start: function (event, ui)
            //                {

            //                },
            //                drag: function ()
            //                {


            //                },
            //                stop: function (event, ui)
            //                {
            //                    var $movingDiv = $(ui.helper[0]);
            //                    var realtop = $movingDiv.position().top;
            //                    //                    alert(realtop);
            //                    // 重新画
            //                    jsPlumb.repaintEverything();
            //                }
            //            }
            //        );

            // 设置所有节点的右键菜单
            DragFlow.makecontextmenu("", true);


        }
    };



})();
