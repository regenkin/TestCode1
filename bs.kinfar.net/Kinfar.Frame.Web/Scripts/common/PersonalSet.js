$(function () {
    var themes = [			{ value: 'default', text: 'Default', group: 'Base' },			{ value: 'gray', text: 'Gray', group: 'Base' },			{ value: 'metro', text: 'Metro', group: 'Base' },			{ value: 'bootstrap', text: 'Bootstrap', group: 'Base' },			{ value: 'black', text: 'Black', group: 'Base' },			{ value: 'metro-blue', text: 'Metro Blue', group: 'Metro' },			{ value: 'metro-gray', text: 'Metro Gray', group: 'Metro' },			{ value: 'metro-green', text: 'Metro Green', group: 'Metro' },			{ value: 'metro-orange', text: 'Metro Orange', group: 'Metro' },			{ value: 'metro-red', text: 'Metro Red', group: 'Metro' },			{ value: 'ui-cupertino', text: 'Cupertino', group: 'UI' },			{ value: 'ui-dark-hive', text: 'Dark Hive', group: 'UI' },			{ value: 'ui-pepper-grinder', text: 'Pepper Grinder', group: 'UI' },			{ value: 'ui-sunny', text: 'Sunny', group: 'UI' }    ];    $('#cb-theme').combobox({
        groupField: 'group',        data: themes,        editable: false,        panelHeight: 'auto',
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