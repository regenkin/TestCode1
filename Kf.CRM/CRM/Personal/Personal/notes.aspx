<%@ Page Language="C#" AutoEventWireup="true" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=GB2312" />
    <title>�ҵı�ǩֽ</title>

    <link rel="stylesheet" type="text/css" href="../../css/styles.css" />
    <link rel="stylesheet" type="text/css" href="../../css/jquery.fancybox-1.2.6.css" media="screen" />
   
    <script type="text/javascript" src="../../lib/jquery/jquery-1.5.2.min.js"></script>
    <script type="text/javascript"  src="../../JS/DragFlow/jquery-ui.min.js"></script>
    <script type="text/javascript" src="../../js/jquery.fancybox-1.2.6.pack.js"></script>
    <script type="text/javascript" src="../../js/script.js" charset="gb2312"></script> 
    <script type="text/javascript" src="../../JS/KFCRM.js"></script>
    <script type="text/javascript">
        $(function () {
            $.ajax({
                type: "get",
                url: "../../data/Personal_notes.ashx", /* ע���������ֶ�ӦCS�ķ������� */
                data: { Action: 'Get', rnd: Math.random() }, /* ע������ĸ�ʽ������ */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) { 
                    var obj = eval(result);
                    //alert(obj.constructor); //String ���캯��
                    for (var i = 0; i < obj.length ; i++) {
                        CreateItem(obj[i].id, obj[i].note_content, obj[i].note_color, obj[i].xyz);
                    }
                    initDel();
                }
            });

            initHeight();
            $(window).resize(function () {
                initHeight();
            });
        })
        function initHeight() {
            var h = document.documentElement.clientHeight;
            $("#main").height(h - 90);
        }
        function initDel()
        {
            $('.delbtn').click(
                function () {
                    if (confirm("��ȷ��Ҫɾ����ģ�飿")) {
                        $(this).parent().hide();
                        $.post('../../data/Personal_notes.ashx', {
                            Action: 'delete',
                            id: $(this).attr('noteid')
                        });
                    }
                }
            )
        }
        function CreateItem(id, content, color, xyz) {
            var arrstr = new Array();
            arrstr = xyz.split(",");
            var left = arrstr[0];
            var top = arrstr[1];
            var zindex = arrstr[2];

            var notes = "";
            notes += "<div   class='note " + color + "' style='left:" + left + "px; top:" + top + "px; z-index:" + zindex + "'>";
            notes += "<div class='delbtn' noteid=" + id + ">x</div>";
            notes += content;
            notes += "<span class='data'>" + id + "</span>";
            notes += "</div>";
            $("#main").append($(notes));
            make_draggable($('.note'));
        }
    </script>
</head>

<body>

    <h1>�ҵı�ǩֽ</h1>
    <h2><a href="">&nbsp;</a></h2>



    <div id="main">
        <a id="addButton" class="green-button" href="add_note.aspx">������ǩ</a>
    </div>




</body>
</html>



