/// <reference path="jquery-1.4.1-vsdoc.js" />
/*
*  DragFlow��ʼ���࣬��Ҫ������
*  1����ʼ������Ĭ����ʾ��ʽ��
*  2���������̲��輰�����ϵ��
*  3����������Ԫ�صĿ���ק�ԡ�Ŀ��ڵ㡢Դ�ڵ��
*  4�������Ҽ��˵���
*  Author:limq
*  date:2012.12.08
*/
var stateMachineConnector;
(function () {

    window.DragFlow = {

        init: function () {
            // jsplumĬ����ʽ
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
            // ���Ӵ����ɹ��¼����ڴ��¼��п��Լ����������ӵ��߼�
            //jsPlumb.bind("jsPlumbConnection", function (conn) {
            //    conn.connection.getOverlay("label").setLabel(conn.connection.id);
            //});
            // ����˫���¼����ڴ��¼��м���ɾ�����ӵ��߼�
            jsPlumb.bind("dblclick", function (c) {
                //jsPlumb.detach(c);
            });

            //DragFlow.initEndpoints("");
            // �������нڵ�Ϊ����Ŀ��ڵ�
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
            //                    // ���»�
            //                    jsPlumb.repaintEverything();
            //                }
            //            }
            //        );

            // �������нڵ���Ҽ��˵�
            DragFlow.makecontextmenu("", true);


        }
    };



})();
