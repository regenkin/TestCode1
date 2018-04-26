$(function () {
    var gridObj = $('#tb_approvalList');
    //并行结点处理
    var mergeCount = $('#div_ApprovalList').attr('mergeCount');
    var mergeIndex = $('#div_ApprovalList').attr('mergeIndex');
    if (mergeCount != null && mergeCount.length > 0 && mergeIndex != null && mergeIndex.length > 0) {
        var token1 = mergeCount.split(',');
        var token2 = mergeIndex.split(',');
        if (token1.length == token2.length) {
            var merges = [];
            for (var i = 0; i < token1.length; i++) {
                var tbIndex = token2[i];
                var rowspan = token1[i];
                gridObj.datagrid('mergeCells', {
                    index: tbIndex,
                    field: 'NodeName',
                    rowspan: rowspan
                });
                gridObj.datagrid('mergeCells', {
                    index: tbIndex,
                    field: 'NextNodeName',
                    rowspan: rowspan
                });
                gridObj.datagrid('mergeCells', {
                    index: tbIndex,
                    field: 'NextHandler',
                    rowspan: rowspan
                });
            }
        }
    }
    $.parser.parse('#approvalDiv');
});