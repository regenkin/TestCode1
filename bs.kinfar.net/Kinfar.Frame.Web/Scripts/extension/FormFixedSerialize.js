//表单序列化扩展
//修改$("form:first").serializeArray(); 中某一名称的checkbox一个均未选择， 取不到name错误
$.fn.extend({
    "fixedSerializeArray": function () {
        var data = $(this).serializeArray();
        var $chks = $(this).find("#mainContent :checkbox:not(:checked)");    //取得所有未选中的checkbox
        if ($chks.length == 0) {
            return data;
        }
        $chks.each(function () {
            var chkName = $(this).attr("name");
            if (chkName) {
                data.push({ name: chkName, value: $(this).val() });
            }
        });
        return data;
    }
});
