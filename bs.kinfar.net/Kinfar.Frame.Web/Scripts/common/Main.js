//初始化
$(function () {
    initTab();
    initMenu();
    initSet();
});

/*-------------------------私有方法-----------------------------*/
//初始化Tab
function initTab() {
    //双击关闭TAB选项卡
    $(".tabs-inner").live('dblclick', function () {
        var title = $(this).children(".tabs-closable").text();
        CloseTabByTitle(null, title);
    });
    //为选项卡绑定右键
    $(".tabs-inner").live('contextmenu', function (e) {
        var title = $(this).children(".tabs-closable").text();
        if (title) {
            $('#mm').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
            $('#mm').data("currtab", title);
            $('#tabs').tabs('select', title);
        }
        return false;
    });
    //绑定右键菜单事件
    //刷新
    $('#mm-tabupdate').click(function () {
        UpdateSelectedTab(null);
    });
    //关闭当前
    $('#mm-tabclose').click(function () {
        var t = $('#mm').data("currtab");
        CloseTabByTitle(null, t);
    });
    //全部关闭
    $('#mm-tabcloseall').click(function () {
        $('.tabs-inner span').each(function (i, n) {
            var t = $(n).text();
            CloseTabByTitle(null, t);
        });
    });
    //关闭除当前之外的TAB
    $('#mm-tabcloseother').click(function () {
        $('#mm-tabcloseright').click();
        $('#mm-tabcloseleft').click();
    });
    //关闭当前右侧的TAB
    $('#mm-tabcloseright').click(function () {
        var nextall = $('.tabs-selected').nextAll();
        if (nextall.length == 0) {
            return false;
        }
        nextall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            CloseTabByTitle(null, t);
            //if (t != "工作台") {
            //    $('#tabs').tabs('close', t);
            //}
        });
        return false;
    });
    //关闭当前左侧的TAB
    $('#mm-tabcloseleft').click(function () {
        var prevall = $('.tabs-selected').prevAll();
        if (prevall.length == 0) {
            return false;
        }
        prevall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            CloseTabByTitle(null, t);
            //if (t != "工作台") {
            //    $('#tabs').tabs('close', t);
            //}
        });
        return false;
    });
    $('#deskIframe').attr('src', $('#deskIframe').attr('url'));
}

//菜单初始化
function initMenu() {
    var loadPanel = function (panel) {
        var ul = panel.find("ul:first");
        var cls = ul.attr("class");
        if (!!cls == false || cls.indexOf("tree") < 0) {
            var menuId = ul.attr("menuId");
            var url = '/DataAsync/GetTreeByNode.html?moduleName=' + encodeURI("菜单管理") + '&noRoot=1&parentId=' + menuId;
            var attrUrl = ul.attr("url");
            if (attrUrl != null && attrUrl != undefined && attrUrl != "" && attrUrl != "null") {
                url = attrUrl + "?menuId=" + menuId;
            }
            LoadTree(ul, url);
        }
    }
    //加载菜单
    $("#leftMenu").accordion({
        onSelect: function (title, index) {
            var panel = $(this).accordion("getPanel", title);
            loadPanel(panel);
        }
    });
    //加载第一个菜单面板树
    var panel = $("#leftMenu").accordion("getSelected");
    if (panel) {
        loadPanel(panel);
    }
}

//初始化其他设置
function initSet() {
    //退出系统
    $('#btnLogout').click(function () {
        showConfirmMsg("系统提示", "您确定要退出本次登录吗", function (ok) {
            if (ok) top.location.href = '/User/Logout.html';
        });
    });
    //个人设置
    $('#btnPersonalSet').click(function () {
        var toolbar = [{
            text: "保 存",
            iconCls: "eu-icon-save",
            handler: function (e) {
                var theme = top.getCurrentDialogFrame()[0].contentWindow.GetTheme();
                if (theme != null) {
                    $('.easyuiTheme').attr('href', '/Scripts/jquery-easyui/themes/' + theme + '/easyui.css');
                }
                top.closeDialog();
            }
        }, {
            text: '关 闭',
            iconCls: "eu-icon-close",
            handler: function (e) {
                top.closeDialog();
            }
        }];
        openDialog("个人设置", '/Page/PersonalSet.html', toolbar);
    });
    //修改密码
    $('#btnChangePwd').click(function () {
        var toolbar = [{
            text: "确 定",
            iconCls: "eu-icon-ok",
            handler: function (e) {
                top.getCurrentDialogFrame()[0].contentWindow.SaveModifyPwd(function () {
                    top.showMsg("提示", "密码修改成功！", function () {
                        top.closeDialog();
                    });
                });
            }
        }, {
            text: '取 消',
            iconCls: "eu-icon-close",
            handler: function (e) {
                top.closeDialog();
            }
        }];
        openDialog("修改密码", '/Page/ChangePwd.html', toolbar, 480, 250, 'eu-icon-changePwd');
    });
    //系统配置
    $('#btnWebConfig').click(function () {
        var toolbar = [{
            text: "保 存",
            iconCls: "eu-icon-save",
            handler: function (e) {
            }
        }, {
            text: '关 闭',
            iconCls: "eu-icon-close",
            handler: function (e) {
                top.closeDialog();
            }
        }];
        openDialog("系统配置", '/Page/WebConfig.html', toolbar);
    });
    //邮件
    $('#btnEmail').click(function () {
        window.open('/Page/EmailIndex.html');
    });
}

//设置快捷菜单
function SetQuckMenu() {
    var toolbar = [{
        text: "确 定",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            top.openWaitDialog('快捷菜单设置保存中...');
            top.getCurrentDialogFrame()[0].contentWindow.SaveUserQuckMenus(function (result) {
                if (result && result.Success) {
                    top.showMsg("提示", "快捷菜单设置成功！", function () {
                        top.closeWaitDialog();
                        top.closeDialog();
                        //重新加载用户快捷菜单
                        $.post('/' + CommonController.Async_System_Controller + '/ReloadUserQuckMenus.html', function (html) {
                            $('#quickOpToolbar').html(html);
                            $.parser.parse('#quickOpToolbar');
                        }, "html");
                    });
                }
                else {
                    top.showAlertMsg("提示", result.Message, "info", function () {
                        top.closeWaitDialog();
                    });
                }
            });
        }
    }, {
        text: '取 消',
        iconCls: "eu-icon-close",
        handler: function (e) {
            top.closeDialog();
        }
    }];
    openDialog("快捷菜单设置", '/Page/AddQuckMenu.html', toolbar, 630, 500, 'eu-p2-icon-tag_blue_add');
}

//单击菜单
function TreeNodeOnClick(node, dom) {
    var title = node.text;
    if (!node.children) { //子菜单
        if (!node.attribute) return;
        var url = node.attribute.url;
        if (url) { //自定义url菜单
            if (url.indexOf("?") > -1)
                url += "&";
            else
                url += "?"
            url += 'mId=' + node.id;
            if (node.attribute.obj && node.attribute.obj.isNewWinOpen) { //新窗口中打开
                window.open(url);
            }
            else { //在框架的标签页中打开
                AddTab(null, title, url);
            }
        }
        else { //通用模块菜单
            var moduleId = node.attribute.obj.moduleId;
            var moduleName = node.attribute.obj.moduleName;
            if (node.attribute.obj && (moduleId || moduleName)) {
                var gridUrl = "/Page/Grid.html?page=grid";
                if (moduleId) {
                    gridUrl += "&moduleId=" + moduleId;
                }
                else if (moduleName) {
                    gridUrl += "&moduleName=" + moduleName;
                }
                gridUrl += "&r=" + Math.random();
                AddTab(null, title, gridUrl);
            }
        }
    }
    else { //文件夹菜单
        $(dom).tree('toggle', node.target);
    }
}

//菜单加载成功
function TreeOnLoadSuccess(node, data, dom) {
    $(dom).tree("collapseAll"); //全部收缩
}
/*------------------------------------------------------------------*/

/*----------------------公有方法------------------------------------*/
//获取当前用户信息
function getUserInfo() {
    var json = decodeURIComponent($('#userInfo').val());
    if (json != null && json.length > 0) {
        var user = JSON.parse(json);
        return user;
    }
    return null;
}

//最大
function maximizeTab() {
    $("body").layout("collapse", "north");
    $("body").layout("collapse", "west");
    $("#ttb_max").hide();
    $("#ttb_min").show();
}

//还原
function restoreTab() {
    $("body").layout("expand", "north");
    $("body").layout("expand", "west");
    $("#ttb_min").hide();
    $("#ttb_max").show();
    //var iframe = getCurrentTabFrame();
    //iframe.contentWindow.location.reload();
}

//调整页面布局
function parseLayout() {
    var panel = top.$("body").layout('panel', 'center');
    var w = panel.panel('options').width;
    panel.panel('resize', { width: w - 1 });
    panel.panel('resize', { width: w });
}

//取边框颜色
function getBorderColor() {
    var color = top.$("#tabs .tabs-header").css('border-bottom-color');
    return color;
}

//获取当前tab的iframe
function getCurrentTabFrame() {
    var tab = GetSelectedTab();
    var iframe = tab.find("iframe:first");
    return iframe;
}

//获取tab中的第一个iframe
//tabIndexOrTitle:tab索引或标题
function getTabFrame(tabIndexOrTitle) {
    var tab = GetTab(null, tabIndexOrTitle);
    var iframe = tab.find("iframe:first");
    return iframe;
}

//获取当前tab的frame内的dom对象
//domId:domId
function getCurrentTabFrameDom(domId) {
    var tab = GetSelectedTab();
    var iframe = tab.find("iframe:first");
    return iframe.contents().find("#" + domId);
}

//获取当前tab的frmae内的dom对象
//selector:选择器
function getCurrentTabFrameDomBySelector(selector) {
    var tab = GetSelectedTab();
    var iframe = tab.find("iframe:first");
    return iframe.contents().find(selector);
}

//获取当前tab的frame内的某个标签内的dom对象
//tag:标签
//startDomId:Id以startDomId开始的
function getCurrentTabFrameSomeDom(tag, startDomId) {
    var tab = GetSelectedTab();
    var iframe = tab.find("iframe:first");
    return iframe.contents().find(tag + "[id^='" + startDomId + "']");
}

//获取当前对话框的iframe
//divId:指定div
function getCurrentDialogFrame(divId) {
    if (divId != undefined && divId != null && divId.length > 0) {
        return top.$("#" + divId).find("iframe:first")
    }
    var no = 0;
    for (var i = 0; i < dialogNum.length; i++) {
        if (dialogNum[i] > no)
            no = dialogNum[i];
    }
    if (no == 0) no = 1;
    var iframe = top.$("#main_dialog" + no).find("iframe:first");
    return iframe;
}

//获取前一个对话框的iframe
function getParentDialogFrame() {
    var maxNo = 0; //当前对话框编号
    for (var i = 0; i < dialogNum.length; i++) {
        if (dialogNum[i] > maxNo)
            maxNo = dialogNum[i];
    }
    var no = maxNo - 1; //前一个对话框编号
    if (no < 1) return null;
    var iframe = top.$("#main_dialog" + no).find("iframe:first");
    return iframe;
}

//获取中间内容区宽度
function getContentAreaWidth() {
    return $("#tabs").tabs("options").width;
}

//获取中间区域高度
function getContentAreaHeight() {
    return $("#tabs").tabs("options").height;
}

//保存当前打开对话框编号Id
var dialogNum = [];
//打开数据保存对话框
//title:对话框标题
//urlOrContent:对话框内容页面或者html内容
//toolbar:对话框底部工具栏
/*---底部工具栏格式--------
 [{
        text: "保存",
        iconCls: "eu-icon-save",
        handler: function (e) {
        }
    }, {
        text: '关闭',
        iconCls: "eu-icon-close",
        handler: function (e) {
            top.closeDialog();
        }
  }];
-------------------------*/
//width:对话框宽度，默认500
//height:对话框高度，默认300
//icon:对话框图标,默认为搜索编辑图标
//openBackFun:打开后的回调函数
function openDialog(title, urlOrContent, toolbar, width, height, icon, openBackFun) {
    if (!toolbar || toolbar.length == 0) {
        showAlertMsg("错误", "弹框按钮未定义，至少要添加一个关闭按钮！", "error");
        return;
    }
    var maxNo = 0;
    for (var i = 0; i < dialogNum.length; i++) {
        if (dialogNum[i] > maxNo)
            maxNo = dialogNum[i];
    }
    if (maxNo >= 10) {
        showAlertMsg("错误", "弹框资源已耗尽，请先释放其他弹出框资源！", "error");
        return;
    }
    var no = maxNo + 1;
    dialogNum.push(no);
    var dialogTagId = 'main_dialog' + no;
    var div = $("#" + dialogTagId);
    var content = urlOrContent && urlOrContent.indexOf(".html") > -1 ? CreateIFrame(urlOrContent) : urlOrContent;
    var iframe = urlOrContent && urlOrContent.indexOf(".html") > -1 ? top.getCurrentDialogFrame()[0] : null;
    div.dialog({
        minimizable: false,
        maximizable: true,
        closed: false,
        closable: true,
        modal: true,
        draggable: true,
        resizable: true,
        cache: false,
        maximized: false,
        title: title,
        iconCls: icon ? icon : "eu-icon-edit",
        content: content,
        width: parseInt(width) > 0 ? parseInt(width) : 500,
        height: parseInt(height) > 0 ? parseInt(height) : 300,
        buttons: toolbar,
        onOpen: function () {
            $('div.panel-tool a.panel-tool-close').click(function () {
                top.closeDialog();
            });
            if (urlOrContent && urlOrContent.indexOf(".html") < 0) {
                $.parser.parse("#" + dialogTagId);
            }
            if (top.isShowStyleBtn()) {
                setTimeout(function () {
                    var divBtnPar = top.$('#' + dialogTagId).parent();
                    var dgBtn = top.$("div.dialog-button a", divBtnPar);
                    dgBtn.each(function (i, item) {
                        var c = 'c' + (i + 1);
                        if (c != undefined && c != null && c.length > 0) {
                            $(item).addClass(c);
                        }
                    });
                    $.parser.parse('#' + dialogTagId);
                }, 50);
            }
            if (typeof (openBackFun) == "function") {
                openBackFun(dialogTagId, iframe); //回调函数
            }
        }
    });
}

//关闭对话框
function closeDialog() {
    var no = dialogNum.pop();
    var div = $("#main_dialog" + no);
    div.dialog("close");
}

//打开确定关闭对话框
//title:对话框标题
//urlOrContent:对话框内容页面或者html内容
//width:对话框宽度，默认500
//height:对话框高度，默认300
//okHandleFun:点击确定后的事件
//cancelHandleFun:点击关闭后的事件
//openBackFun:打开后的回调函数
function openOkCancelDialog(title, urlOrContent, width, height, okHandleFun, cancelHandleFun, openBackFun) {
    var toolbar = [{
        id: 'btnOk',
        text: "确 定",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            var iframe = urlOrContent && urlOrContent.indexOf(".html") > -1 ? top.getCurrentDialogFrame()[0] : null;
            if (typeof (okHandleFun) == "function") {
                okHandleFun(iframe, function (action) {
                    if (action) {
                        top.closeDialog();
                    }
                });
            }
        }
    }, {
        id: 'btnClose',
        text: '关 闭',
        iconCls: "eu-icon-close",
        handler: function (e) {
            top.closeDialog();
            if (typeof (cancelHandleFun) == "function") {
                cancelHandleFun();
            }
        }
    }];
    top.openDialog(title, urlOrContent, toolbar, width, height, 'eu-icon-cog', openBackFun);
}

//移除顶层弹出框按钮
//btnId:按钮ID
function removeTopDialogBtn(btnId) {
    var maxNo = 0;
    for (var i = 0; i < dialogNum.length; i++) {
        if (dialogNum[i] > maxNo)
            maxNo = dialogNum[i];
    }
    var div = $("#main_dialog" + maxNo);
    div.parent().find('#' + btnId).remove();
}

//移除顶层弹出框按钮
//btnId:id以些开头的按钮
function removeTopDialogBtnBySomeId(btnId) {
    var maxNo = 0;
    for (var i = 0; i < dialogNum.length; i++) {
        if (dialogNum[i] > maxNo)
            maxNo = dialogNum[i];
    }
    var div = $("#main_dialog" + maxNo);
    div.parent().find("a[id^='" + btnId + "']").remove();
}

//显示消息
//title:标题
//msg:消息内容
//backFun:关闭后事件
//position:消息框位置,
//timeout:停留时间默认1s
/*position参数说明：
  'topLeft':左上角
  'topCenter':顶部中间
  'topRight':右上角
  'centerLeft':中间靠左
  'center':正中间
  'centerRight':中间靠右
  'bottomLeft':左下角
  'bottomCenter':底部中间
  'bottomRight':右下角
  为空或其他：正中间
*/
function showMsg(title, msg, backFun, position, timeout) {
    var params = {
        title: title,
        msg: msg,
        showType: 'fade'
    };
    if (typeof (backFun) == "function") {
        params.onClose = backFun;
    }
    if (timeout && parseInt(timeout) > 0) {
        params.timeout = parseInt(timeout) * 1000;
    }
    else {
        params.timeout = 1000;
    }
    if (position == 'topLeft') { //左上角
        params.style = {
            right: '',
            left: 0,
            top: document.body.scrollTop + document.documentElement.scrollTop,
            bottom: ''
        };
    }
    else if (position == 'topCenter') { //顶部中间
        params.style = {
            right: '',
            top: document.body.scrollTop + document.documentElement.scrollTop,
            bottom: ''
        };
    }
    else if (position == 'topRight') { //右上角
        params.style = {
            left: '',
            right: 0,
            top: document.body.scrollTop + document.documentElement.scrollTop,
            bottom: ''
        };
    }
    else if (position == 'centerLeft') { //中间靠左
        params.style = {
            left: 0,
            right: '',
            bottom: ''
        };
    }
    else if (position == 'center') { //正中间
        params.style = {
            right: '',
            bottom: ''
        };
    }
    else if (position == 'centerRight') { //中间靠右
        params.style = {
            left: '',
            right: 0,
            bottom: ''
        };
    }
    else if (position == 'bottomLeft') { //左下角
        params.style = {
            left: 0,
            right: '',
            top: '',
            bottom: -document.body.scrollTop - document.documentElement.scrollTop
        };
    }
    else if (position == 'bottomCenter') { //底部中间
        params.style = {
            right: '',
            top: '',
            bottom: -document.body.scrollTop - document.documentElement.scrollTop
        };
    }
    else if (position == 'bottomRight') { //右下角
        params.style = null;
    }
    else {
        params.style = {
            right: '',
            bottom: ''
        };
    }
    $.messager.show(params);
}

//显示一个包含“确定”和“取消”按钮的确认消息窗口
//title:在头部面板显示的标题文本
//msg:消息内容
//msgType:消息类型，有以下几种消息类型：
/*
info:普通消息
warning:警告消息
question:询问消息
error:错误消息
*/
//backFun:回调函数,当用户点击“确定”按钮的时侯将传递一个true值给回调函数，否则传递一个false值
function showAlertMsg(title, msg, msgType, backFun) {
    $.messager.alert(title, msg, msgType, backFun);
}

//显示一个用户可以输入文本的并且带“确定”和“取消”按钮的消息窗体。参数：
//title：在头部面板显示的标题文本。
//msg：显示的消息文本。
//backFun(ok): 在用户输入一个值参数的时候执行的回调函数。 
function showConfirmMsg(title, msg, backFun) {
    $.messager.confirm(title, msg, function (ok) {
        if (typeof (backFun) == "function") {
            backFun(ok);
        }
    });
}

//打开等待对话框
//title:标题
function openWaitDialog(title) {
    $.messager.progress({
        title: title && title.length > 0 ? title : '拼命处理中...',
        text: ''
    });
}

//关闭等待对话框
function closeWaitDialog() {
    try {
        $.messager.progress('close');
    } catch (err) { }
}

//弹出登录对话框
function openLoginDialog() {
    var toolbar = [{
        id: 'btnOk',
        text: "登录",
        iconCls: "eu-icon-ok",
        handler: function (e) {
            var iframe = $('#' + $(e.data.target).attr('divId')).find("iframe:first")[0];
            iframe.contentWindow.DoLogin();
        }
    }, {
        id: 'btnClose',
        text: '关闭',
        iconCls: "eu-icon-close",
        handler: function (e) {
            top.closeDialog();
        }
    }];
    top.openDialog('登录窗口', '/User/DialogLogin.html?noself=1', toolbar, 470, 300, 'eu-icon-password', function (divId) {
        $('#btnOk').attr('divId', divId);
    });
}

//是否显示样式按钮
function isShowStyleBtn() {
    var v = $('#isShowStyleBtn').val();
    return parseInt(v) == 1;
}
/*-----------------------------------------------------------------*/