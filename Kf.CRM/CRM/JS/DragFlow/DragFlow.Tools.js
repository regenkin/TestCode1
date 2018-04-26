/// <reference path="jquery-1.4.1-vsdoc.js" />

(function ($)
{
    $.fn.NewGUID = function (parentId, tab_class, content_class, ismouseover)
    {
        var guid = (G() + G() + "-" + G() + "-" + G() + "-" +

 G() + "-" + G() + G() + G()).toUpperCase();
        return guid;
    };
    function G()
    {

        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1)

    };

})(jQuery);