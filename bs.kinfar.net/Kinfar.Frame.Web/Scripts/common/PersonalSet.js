﻿$(function () {
    var themes = [
        groupField: 'group',
        onLoadSuccess: function () {
            var cookieTheme = GetCookie('theme');
            if (cookieTheme == undefined || cookieTheme == null)
                cookieTheme = 'default';
            $(this).combobox('setValue', cookieTheme);
        }
    });
});

//获取设置样式
function GetTheme() {
    var cookieTheme = GetCookie('theme');
    var theme = $('#cb-theme').combobox('getValue');
    if (theme != cookieTheme) {
        SetCookie('theme', theme);
        return theme;
    }
    return null;
}