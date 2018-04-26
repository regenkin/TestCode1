<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../../lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="../../JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.ajax({
                type: "GET",
                url: "../../data/public_news.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'form', nid: getparastr("nid"), rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    for (var n in obj) {
                        
                    }

                    //alert(obj.constructor); //String 构造函数
                    $("#news_title").html(obj.news_title);
                    $("#fly_title").html(formatTimebytype(obj.news_time, 'yyyy-MM-dd') + "　　　编辑：" + obj.create_name);
                    $("#news_content").html(myHTMLDeCode(obj.news_content));
                }
            });
        })
    </script>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <div id="news_title" style="text-align:center;font-size:xx-large;font-weight:bold;"></div>
                <br />
                <div id="fly_title"style="text-align:center;font-size:12px;border-bottom:1px solid #aaa;vertical-align:middle;height:27px;"></div>

                <div id="news_content"></div>
    </form>
</body>
</html>
