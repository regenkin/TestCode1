//初始化
$(function () {
    $('#pp').portal({ border: false });
    $('#pp').portal('resize');
    $('.easyui-tabs').css('border-style', 'none');
    //加载所有选中的标签数据
    $('.easyui-tabs').each(function (i, item) {
        var tab = $(item).tabs('getSelected');
        var iframe = tab.find('iframe');
        if (iframe && iframe.length > 0) {
            iframe.attr('src', iframe.attr('url'));
        }
    });
    $("iframe[hastab='0']").each(function (i, iframe) {
        $(iframe).attr('src', $(iframe).attr('url'));
    });
    $("div[id^='deskItem_']").each(function (i, tab) {
        $(tab).find("div[id^='deskPanel_']").each(function (i, item) {
            $(item).width($(tab).width());
        });
    });
    $.parser.parse();
});

//选择标签事件
function OnSelectTab(title, index, i) {
    var tab = $('#deskItem_' + i).tabs('getTab', index);
    var moreUrl = tab.find('input').attr('moreUrl');
    if (moreUrl && moreUrl.length > 0) {
        $('#tab-tools_' + i).find("a[name='more']").show();
    }
    else {
        $('#tab-tools_' + i).find("a[name='more']").hide();
    }
    var iframe = tab.find('iframe');
    if (iframe && iframe.length > 0) {
        var src = iframe.attr('src');
        if (!src) {
            iframe.attr('src', iframe.attr('url'));
        }
    }
}

//刷新
function Refresh(i, obj) {
    var panel = null;
    if ($(obj).hasClass('easyui-linkbutton')) { //tabs
        panel = $('#deskItem_' + i).tabs('getSelected');
    }
    else { //panel
        panel = $("div[id^='deskPanel_" + i + "']");
    }
    if (panel) {
        var iframe = panel.find('iframe');
        var url = iframe.attr('src');
        iframe.attr('src', url);
    }
}

//加载更多
function LoadMore(i, obj) {
    var panel = null;
    if ($(obj).hasClass('easyui-linkbutton')) { //tabs
        panel = $('#deskItem_' + i).tabs('getSelected');
    }
    else { //panel
        panel = $("div[id^='deskPanel_" + i + "']");
    }
    if (panel) {
        var moreUrl = panel.find('input').attr('moreUrl');
        var tabName = panel.find('input').attr('tabName');
        if (moreUrl && moreUrl.length > 0)
            //top.addTab(tabName, moreUrl);
            AddTab(null, tabName, moreUrl);
    }
}