$(function () {

    var router = new Router({
        container: '#container',
        enterTimeout: 0,
        leaveTimeout: 0
    });

    // grid
    var home = {
        url: '/',
        className: 'home',
        render: function () {
            return '<h1>老子是home</h1>'+
                '<a href = "#/button" class="weui_grid" >'+
                '<div class="weui_grid_icon" >'+
                    '<i class="icon icon_button"></i>'+
                '</div >'+
                '<p class="weui_grid_label" > Button</p> '+
            '</a>';
        }
    };

    // button
    var button = {
        url: '/button',
        className: 'button',
        render: function () {
            return '<h1>老子是buttom</h1>' +
                '<a href = "#/" class="weui_grid" >' +
                '<div class="weui_grid_icon" >' +
                '<i class="icon icon_button"></i>' +
                '</div >' +
                '<p class="weui_grid_label" > home</p> ' +
                '</a>';
        }

    };

    router.push(home)
        .push(button)
        .setDefault('/')
        .init();


    // .container 设置了 overflow 属性, 导致 Android 手机下输入框获取焦点时, 输入法挡住输入框的 bug
    // 相关 issue: https://github.com/weui/weui/issues/15
    // 解决方法:
    // 0. .container 去掉 overflow 属性, 但此 demo 下会引发别的问题
    // 1. 参考 http://stackoverflow.com/questions/23757345/android-does-not-correctly-scroll-on-input-focus-if-not-body-element
    //    Android 手机下, input 或 textarea 元素聚焦时, 主动滚一把
    if (/Android/gi.test(navigator.userAgent)) {
        window.addEventListener('resize', function () {
            if (document.activeElement.tagName == 'INPUT' || document.activeElement.tagName == 'TEXTAREA') {
                window.setTimeout(function () {
                    document.activeElement.scrollIntoViewIfNeeded();
                }, 0);
            }
        })
    }
});
