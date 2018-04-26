/**
* jQuery ligerUI 1.1.0.1
* 
* Author leoxie [ gd_star@163.com ] 
* 
*/
//dialog 图片文件夹的路径 针对于IE6设置
var ligerDialogImagePath = "../../lib/ligerUI/skins/ext/images/dialog/";
(function ($) {
    //manager base
    $.ligerui = $.ligerui || {};
    $.ligerui.addManager = function (dom, manager) {
        if (dom.id == undefined || dom.id == "")
            dom.id = "ligerui" + (1000 + $.ligerui.ManagerCount);
        $.ligerui.ManagerCount++;
        $.ligerui.Managers[dom.id] = manager;
        dom.applyligerui = true;
    };
    $.ligerui.getManager = function (domArr) {
        if (domArr.length == 0) return null;
        return $.ligerui.Managers[domArr[0].id];
    };
    $.ligerui.Managers = $.ligerui.Managers || {};
    $.ligerui.ManagerCount = $.ligerui.ManagerCount || 0;

    $.ligerDefaults = $.ligerDefaults || {};
    $.ligerDefaults.Dialog = {
        cls: null,       //给dialog附加css class
        id: null,        //给dialog附加id
        buttons: null, //按钮集合 
        isDrag: true,   //是否拖动
        width: 300,     //宽度
        height: null,   //高度，默认自适应 
        content: '',    //内容
        target: null,   //目标对象，指定它将以appendTo()的方式载入
        url: null,      //目标页url，默认以iframe的方式载入
        load: false,     //是否以load()的方式加载目标页的内容
        type: 'none',   //类型 warn、success、error、question
        left: null,     //位置left
        top: null,      //位置top
        modal: true,    //是否模态对话框
        name: null,     //创建iframe时 作为iframe的name和id 
        isResize: false, // 是否调整大小
        allowClose: true, //允许关闭
        opener: null,
        timeParmName: null,  //是否给URL后面加上值为new Date().getTime()的参数，如果需要指定一个参数名即可
        closeWhenEnter: null, //回车时是否关闭dialog
        isHidden: false,        //关闭对话框时是否只是隐藏，还是销毁对话框
        zindex: 9000,
        showToggle: false,                          //是否显示收缩窗口按钮
        slide:true
    };
    $.ligerDefaults.DialogString = {
        titleMessage: '提示',                     //提示文本标题
        waittingMessage: '正在等待中,请稍候...'
    };

    //dialog manager design
    $.ligerManagers = $.ligerManagers || {};
    $.ligerManagers.Dialog = function (options, po) {
        this.options = options;
        this.po = po;
    };
    $.ligerManagers.Dialog.prototype = {
        //按下回车
        enter: function () {
            var g = this; var po = this.po;
            var isClose;
            if (po.closeWhenEnter != undefined) {
                isClose = po.closeWhenEnter;
            }
            else if (po.type == "warn" || po.type == "error" || po.type == "success" || po.type == "question") {
                isClose = true;
            }
            if (isClose) {
                g.close();
            }
        },
        esc: function () {

        },
        close: function () {
            var g = this; var po = this.po, p = this.options;
            if (g.frame) {
                $(g.frame.document).ready(function () {                    
                    g.dialog.remove();
                });
            }
            else {

                g.dialog.remove();
            }
            if (g.windowMask) {
                if (p.isHidden) g.windowMask.hide();
                else g.windowMask.remove();
            }
            
                $('body').unbind('keydown.dialog');
        },
        hidden: function () {
            var g = this; var po = this.po;

                    if (g.windowMask) g.windowMask.hide();
                       // po.removeWindowMask();
                    g.dialog.hide();               
        },
        show: function () {
            var g = this, po = this.po, p = this.options;
            if (g.windowMask)
                g.windowMask.show()
            else if (p.modal)
                po.applyWindowMask();
            g.dialog.show();
        },
        url: function (url) {
            var g = this, po = this.po, p = this.options;
            p.url = url;
            if (g.jiframe)
                g.jiframe.attr("src", p.url);
        },
        setContent: function (content) {
            var g = this, po = this.po, p = this.options;
            p.content = content;
            $(".l-dialog-content", g.dialog.body).html(p.content);
        },
        //展开 收缩
        toggle: function () {
            var g = this, p = this.options;
            if (!g.wintoggle) return;
            if (g.wintoggle.hasClass("l-dialog-extend"))
                g.extend();
            else
                g.collapse();
        },

        //收缩
        collapse: function () {
            var g = this, p = this.options;
            if (!g.wintoggle) return;
            if (p.slide)
                g.dialog.content.animate({ height: 1 }, p.slide);
            else
                g.dialog.content.height(1);
        },

        //展开
        extend: function () {
            var g = this, p = this.options;
            if (!g.wintoggle) return;
            var contentHeight = g._height - g._borderY - g.dialog.buttons.outerHeight();
            if (p.slide)
                g.dialog.content.animate({ height: g.dialog.content.currentheight }, p.slide);
            else
                g.dialog.content.height(g.dialog.content.currentheight);
        },
        setShowToggle: function (value) {
            var g = this, p = this.options;
            if (value) {
                if (!g.wintoggle) {
                    g.wintoggle = $('<div class="l-dialog-winbtn l-dialog-collapse"></div>').appendTo(g.dialog.winbtns)
                   .hover(function () {                  
                       if ($(this).hasClass("l-dialog-extend"))
                           $(this).addClass("l-dialog-extend-over");
                       else
                           $(this).addClass("l-dialog-collapse-over");
                   }, function () {
                       $(this).removeClass("l-dialog-extend-over l-dialog-collapse-over");
                   }).click(function () {                      
                       if (g.wintoggle.hasClass("l-dialog-extend")) {                       
                           g.wintoggle.removeClass("l-dialog-extend");
                           g.extend();
                       }
                       else {                 
                           g.wintoggle.addClass("l-dialog-extend");
                           g.collapse();
                       }
                   });
                }
            }
            else if (g.wintoggle) {
                g.wintoggle.remove();
                g.wintoggle = null;
            }
        },
        updateBtnsWidth: function () {
            var g = this;
            var btnscount = $(">div", g.dialog.winbtns).length;
            g.dialog.winbtns.width(22 * btnscount);
        }
    };
    $.ligerManagers.Dialog.prototype.hide = $.ligerManagers.Dialog.prototype.hidden;

    ///	<param name="$" type="jQuery"></param>
    $.ligerDialog = {};
    $.ligerDialog.open = function (p) {
        p = $.extend({}, $.ligerDefaults.Dialog, $.ligerDefaults.DialogString, p || {});
        var zindex = p.zindex;
        var po = {
            applyWindowMask: function () {
                //$(".l-window-mask").remove();
                if (g.windowMask)
                    g.windowMask.remove();
                g.windowMask = g.dialog.WindowMask = $("<div class='l-window-mask' style='display: block;'></div>");
                g.dialog.WindowMask.height($(window).height() + $(window).scrollTop()).appendTo('body');
                g.dialog.WindowMask.css({ zIndex: zindex });
            },
            removeWindowMask: function () {
                //$(".l-window-mask").remove();
                g.dialog.WindowMask.remove();
            },
            applyDrag: function () {
                if ($.fn.ligerDrag)
                    g.dialog.ligerDrag({ handler: '.l-dialog-title',breakout:true });
            },
            applyResize: function () {
                if ($.fn.ligerResizable) {
                    g.dialog.ligerResizable({
                        onStopResize: function (current, e) {
                            var top = 0;
                            var left = 0;
                            if (!isNaN(parseInt(g.dialog.css('top'))))
                                top = parseInt(g.dialog.css('top'));
                            if (!isNaN(parseInt(g.dialog.css('left'))))
                                left = parseInt(g.dialog.css('left'));
                            if (current.diffTop != undefined) {
                                g.dialog.css({
                                    top: top + current.diffTop,
                                    left: left + current.diffLeft
                                });
                                g.dialog.body.css({
                                    width: current.newWidth - 26
                                });
                                $(".l-dialog-content", g.dialog.body).height(current.newHeight - 46 - $(".l-dialog-buttons", g.dialog).height());
                                g.dialog.content.currentheight = g.dialog.content.height();
                            }
                            return false;
                        }
                    });
                }
            },
            setImage: function () {
                if (p.type) {
                    if (p.type == 'success' || p.type == 'donne' || p.type == 'ok') {
                        $(".l-dialog-image", g.dialog).addClass("l-dialog-image-donne").show();
                        $(".l-dialog-content", g.dialog).css({ paddingLeft: 64, paddingBottom: 30,paddingRight: 5 });
                    }
                    else if (p.type == 'error') {
                        $(".l-dialog-image", g.dialog).addClass("l-dialog-image-error").show();
                        $(".l-dialog-content", g.dialog).css({ paddingLeft: 64, paddingBottom: 30, paddingRight: 5 });
                    }
                    else if (p.type == 'warn') {
                        $(".l-dialog-image", g.dialog).addClass("l-dialog-image-warn").show();
                        $(".l-dialog-content", g.dialog).css({ paddingLeft: 64, paddingBottom: 30, paddingRight: 5 });
                    }
                    else if (p.type == 'question') {
                        $(".l-dialog-image", g.dialog).addClass("l-dialog-image-question").show();
                        $(".l-dialog-content", g.dialog).css({ paddingLeft: 64, paddingBottom: 40, paddingRight: 5 });
                    }
                    else if (p.type == 'waitting') {
                        $(".l-dialog-image", g.dialog).addClass("l-dialog-image-waitting").show();
                        $(".l-dialog-image", g.dialog).css({ 'margin-left': 14 });
                    }
                }
            }
        };
        //public Object
        var g = new $.ligerManagers.Dialog(p, po);
        g.dialog = $('<div class="l-dialog"><table class="l-dialog-table" cellpadding="0" cellspacing="0" border="0"><tbody><tr><td class="l-dialog-tl"></td><td class="l-dialog-tc"><div class="l-dialog-tc-inner"><div class="l-dialog-icon"></div><div class="l-dialog-title"></div><div class="l-dialog-winbtns"><div class="l-dialog-winbtn l-dialog-close"></div></div></div></td><td class="l-dialog-tr"></td></tr><tr><td class="l-dialog-cl"></td><td class="l-dialog-cc"><div class="l-dialog-body"><div class="l-dialog-image"></div> <div class="l-dialog-content"></div><div class="l-dialog-buttons"><div class="l-dialog-buttons-inner"></div></td><td class="l-dialog-cr"></td></tr><tr><td class="l-dialog-bl"></td><td class="l-dialog-bc"></td><td class="l-dialog-br"></td></tr></tbody></table></div>');
        $('body').append(g.dialog);
        g.dialog.body = $(".l-dialog-body:first", g.dialog);
        if (p.allowClose == false) $(".l-dialog-close", g.dialog).remove();
        g.dialog.body = $(".l-dialog-body:first", g.dialog);
        g.dialog.header = $(".l-dialog-tc-inner:first", g.dialog);
        g.dialog.winbtns = $(".l-dialog-winbtns:first", g.dialog.header);
        g.dialog.buttons = $(".l-dialog-buttons:first", g.dialog);
        g.dialog.content = $(".l-dialog-content:first", g.dialog);
        if (p.target || p.url || p.type == "none") p.type = null;
        if (p.cls) g.dialog.addClass(p.cls);
        if (p.id) g.dialog.attr("id", p.id);
        if (p.showToggle) g.setShowToggle(true);


        //设置锁定屏幕、拖动支持 和设置图片
        if (p.modal)
            po.applyWindowMask();
        if (p.isDrag)
            po.applyDrag();
        if (p.isResize)
            po.applyResize();
        if (p.type)
            po.setImage();
        else {
            $(".l-dialog-image", g.dialog).remove();
            $(".l-dialog-content", g.dialog.body).addClass("l-dialog-content-noimage");
        }
        //设置主体内容
        if (p.target) {
            $(".l-dialog-content", g.dialog.body).prepend(p.target);
        }
        else if (p.url) {
            if (p.timeParmName) {
                p.url += p.url.indexOf('?') == -1 ? "?" : "&";
                p.url += p.timeParmName + "=" + new Date().getTime();
            }
            g.jiframe = $("<iframe frameborder='0'></iframe>");
            var framename = p.name ? p.name : "ligerwindow" + new Date().getTime();
            g.jiframe.attr("name", framename);
            g.jiframe.attr("id", framename);
            $(".l-dialog-content", g.dialog.body).prepend(g.jiframe);
            $(".l-dialog-content", g.dialog.body).addClass("l-dialog-content-nopadding");
            setTimeout(function () {
                g.jiframe.attr("src", p.url);
                g.frame = window.frames[g.jiframe.attr("name")];
            }, 0);
        }
        else if (p.content) {
            $(".l-dialog-content", g.dialog.body).html(p.content);
        }
        if (p.opener) g.dialog.opener = p.opener;
        //设置按钮
        if (p.buttons) {
            $(p.buttons).each(function (i, item) {
                var btn = $('<div class="l-dialog-btn"><div class="l-dialog-btn-l"></div><div class="l-dialog-btn-r"></div><div class="l-dialog-btn-inner"></div></div>');
                $(".l-dialog-btn-inner", btn).html(item.text);
                $(".l-dialog-buttons-inner", g.dialog.body).prepend(btn);
                item.width && btn.width(item.width);
                item.onclick && btn.click(function () { item.onclick(item, g, i) });
            });
        } else {
            $(".l-dialog-buttons", g.dialog).remove();
        }
        $(".l-dialog-buttons-inner", g.dialog).append("<div class='l-clear'></div>");

        //设置参数属性
        p.width && g.dialog.body.width(p.width - 26);
        if (p.height) {
            $(".l-dialog-content", g.dialog.body).height(p.height - 46 - $(".l-dialog-buttons", g.dialog).height());
            g.dialog.content.currentheight = p.height - 46 - $(".l-dialog-buttons", g.dialog).height();            
        }
        p.title = p.title || p.titleMessage;
        p.title && $(".l-dialog-title", g.dialog).html(p.title);
        $(".l-dialog-title", g.dialog).bind("selectstart", function () { return false; });


        //设置事件
        $(".l-dialog-btn", g.dialog.body).hover(function () {
            $(this).addClass("l-dialog-btn-over");
        }, function () {
            $(this).removeClass("l-dialog-btn-over");
        });
        $(".l-dialog-tc .l-dialog-close", g.dialog).hover(function () {
            $(this).addClass("l-dialog-close-over");
        }, function () {
            $(this).removeClass("l-dialog-close-over");
        }).click(function () {
            if (p.isHidden)
                g.hidden();
            else
                g.close();
        });

       
        //位置初始化
        var left = 0;
        var top = 0;
        var width = p.width || g.dialog.width();
        if (p.left != null) left = p.left;
        else left = 0.5 * ($(window).width() - width);
        if (p.top != null) top = p.top;
        else top = 0.5 * ($(window).height() - g.dialog.height()) + $(window).scrollTop() - 10;
        if (left < 0) left = 0;
        if (top < 0) top = 0;
        g.dialog.css({ left: left, top: top });
        g.dialog.css({ zIndex: Math.abs(p.zindex) + 1 });
        g.dialog.show();
        g.updateBtnsWidth();

        $('body').bind('keydown.dialog', function (e) {
            var key = e.which;
            if (key == 13) {
                g.enter();
            }
            else if (key == 27) {
                g.esc();
            }
        });
        $.ligerui.addManager(g.dialog[0], g);
        return g;
    };
    $.ligerDialog.close = function () {
        $(".l-dialog,.l-window-mask").remove();
    };
    $.ligerDialog.show = function (p) {
        if ($(".l-dialog").length > 0) {
            $(".l-dialog,.l-window-mask").show();
            return;
        }
        return $.ligerDialog.open(p);
    };
    $.ligerDialog.hide = function () {
        $(".l-dialog,.l-window-mask").hide();
    };
    $.ligerDialog.alert = function (content, title, type, callback, zindex) {
        content = content || "";
        if (typeof (title) == "function") {
            callback = title;
            type = null;
        }
        else if (typeof (type) == "function") {
            callback = type;
        }
        var btnclick = function (item, Dialog, index) {
            Dialog.close();
            if (callback)
                callback(item, Dialog, index);
        };
        p = {
            content: content,
            buttons: [{ text: '确定', onclick: btnclick }]
        };
        if (typeof (title) == "string" && title != "") p.title = title;
        if (typeof (type) == "string" && type != "") p.type = type;
        p.zindex = zindex || 9000;
        $.ligerDialog.open(p);
    };

    $.ligerDialog.confirm = function (content, title, callback, zindex) {
        if (typeof (title) == "function") {
            callback = title;
            type = null;
        }
        var btnclick = function (item, Dialog) {
            Dialog.close();
            if (callback) {
                callback(item.type == 'ok');
            }
        };
        p = {
            type: 'question',
            content: content,
            buttons: [{ text: '是', onclick: btnclick, type: 'ok' }, { text: '否', onclick: btnclick, type: 'no' }]
        };
        if (typeof (title) == "string" && title != "") p.title = title;
        $.ligerDialog.open(p);
    };
    $.ligerDialog.warning = function (content, title, callback, zindex) {
        if (typeof (title) == "function") {
            callback = title;
            type = null;
        }
        var btnclick = function (item, Dialog) {
            Dialog.close();
            if (callback) {
                callback(item.type);
            }
        };
        p = {
            type: 'question',
            content: content,
            buttons: [{ text: '是', onclick: btnclick, type: 'yes' }, { text: '否', onclick: btnclick, type: 'no' }, { text: '取消', onclick: btnclick, type: 'cancel' }]
        };
        if (typeof (title) == "string" && title != "") p.title = title;
        p.zindex = zindex;
        $.ligerDialog.open(p);
    };
    $.ligerDialog.waitting = function (title, zindex) {
        title = title || $.ligerDefaults.Dialog.waittingMessage;
        zindex = zindex || 9000;
        $.ligerDialog.open({ cls: 'l-dialog-waittingdialog', title:  title, type: 'waitting', content: '<div style="padding:4px"></div>', allowClose: false, zindex: zindex });
    };
    $.ligerDialog.closeWaitting = function () {
        $(".l-dialog-waittingdialog,.l-window-mask").remove();
    };
    $.ligerDialog.success = function (content, title, onBtnClick, zindex) {
        $.ligerDialog.alert(content, title, 'success', onBtnClick, zindex);
    };
    $.ligerDialog.error = function (content, title, onBtnClick, zindex) {
        $.ligerDialog.alert(content, title, 'error', onBtnClick, zindex);
    };
    $.ligerDialog.warn = function (content, title, onBtnClick, zindex) {
        $.ligerDialog.alert(content, title, 'warn', onBtnClick, zindex);
    };
    $.ligerDialog.question = function (content, title, zindex) {
        $.ligerDialog.alert(content, title, 'question', zindex);
    }


    $.ligerDialog.prompt = function (title, value, multi, callback) {
        var target = $('<input type="text" class="l-dialog-inputtext"/>');
        if (typeof (multi) == "function") {
            callback = multi;
        }
        if (typeof (value) == "function") {
            callback = value;
        }
        else if (typeof (value) == "boolean") {
            multi = value;
        }
        if (typeof (multi) == "boolean" && multi) {
            target = $('<textarea class="l-dialog-textarea"></textarea>');
        }
        if (typeof (value) == "string" || typeof (value) == "int") {
            target.val(value);
        }
        var btnclick = function (item, Dialog, index) {
            Dialog.close();
            if (callback) {
                callback(item.type == 'yes', target.val());
            }
        }
        p = {
            title: title,
            target: target,
            width: 320,
            buttons: [{ text: '确定', onclick: btnclick, type: 'yes' }, { text: '取消', onclick: btnclick, type: 'cancel' }]
        };
        return $.ligerDialog.open(p);
    };


})(jQuery);