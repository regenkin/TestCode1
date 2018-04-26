<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>KFCRM-最好的开源免费CRM</title>
    <meta http-equiv="X-UA-Compatible" content="ie=8 chrome=1" />

    <link href="lib/ligerUI/skins/ext/css/ligerui-all.css" rel="stylesheet" type="text/css" />
    <link href="CSS/Toolbar.css" rel="stylesheet" type="text/css" />
    <link href="CSS/core.css" rel="stylesheet" type="text/css" />
    <link href="lib/ligerUI/skins/ext/css/ligerui-fix.css" rel="stylesheet" type="text/css" />

    <script src="lib/jquery/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerDrag.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerResizable.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerLayout.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerTab.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerAccordion.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerDialog.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerGrid.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerMenu.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerTree.js" type="text/javascript"></script>
    <script src="lib/ligerUI/js/plugins/ligerToolBar.js" type="text/javascript"></script>
    <script src="JS/jquery.jclock.js" type="text/javascript"></script>

    <script src="JS/Toolbar.js" type="text/javascript"></script>
    <script src="JS/KFCRM.js" type="text/javascript"></script>
    <script type="text/javascript">
        var tab = null;
        var accordion = null;
        var accordion2 = null;
        var tree = null;
        var manager = null;
        $(function () {           
            setInterval("getUser()", 30000);
            $("#pageloading").height($(window).height());
            //布局
            $("#layout1").ligerLayout({ leftWidth: 190, rightWidth: 190, bottomHeight: 25, allowBottomResize: false, allowLeftResize: false, allowRightResize: false, height: '100%', onHeightChanged: f_heightChanged, isRightCollapse: true });
            var height = $(".l-layout-center").height();
            //Tab
            tab = $("#framecenter").ligerTab({ 
                height: height,
                dblClickToClose:true,
                showSwitch: true,       //显示切换窗口按钮
                showSwitchInTab: true //切换窗口按钮显示在最后一项 
            });

            //面板              
            accordion = $("#accordion1").ligerAccordion({ height: height - 25 });
            accordion2 = $("#accordion2").ligerAccordion({ height: height - 25 });
            f_according();

            //时间日期
            $('#jnkc').jclock({ withDate: true, withWeek: true });

            initLayout();
            $(window).resize(function () {
                initLayout();
            });
           

            $("#tree1").ligerTree({
                url: 'data/Base.ashx?Action=getUserTree&rnd=' + Math.random(),
                idFieldName: 'id',
                //parentIDFieldName: 'pid',
                usericon: 'd_icon',
                checkbox: false,
                itemopen: false
            });
            getsysinfo();
            getuserinfo();
            toolbar();
            remind();
            show_welcome();
        });
        function getsysinfo()
        {
            $.ajax({
                type: "GET",
                url: "data/sys_info.ashx", /* 注意后面的名字对应CS的方法名称 */
                data: { Action: 'getinfo', rnd: Math.random() }, /* 注意参数的格式和名称 */
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (result) {
                    var obj = eval(result);
                    var rows = obj.Rows;

                    document.title =rows[0].sys_value + "CRM客户关系管理系统-KFCRM";                    
                    $("#logo").attr("src", rows[1].sys_value);
                }
            });
        }
        function getuserinfo()
        {
            $.getJSON("data/Base.ashx?Action=GetUserInfo&rnd=" + Math.random(), function (data, textStatus) {
                //alert(data);
                $("#Username").html(data.name);
                if (data.title)
                    $("#userheader").attr("src", "/images/upload/portrait/" + data.title);
                else
                    $("#userheader").attr("src", "/images/icons/function_icon_set/user_48.png");
            });
        }
       
        function getUser() {
            $.ajax({
                url: "data/Base.ashx?Action=GetOnline&rnd=" + Math.random(),
                type: 'get',
                success: function () {
                    treemanager = $("#tree1").ligerGetTreeManager();
                    treemanager.FlushNodeIcon();
                },
                error: function () {
                    javascript: location.replace("login.aspx");
                }
            })
           
            remind();
        }
        function remind() {
            var now = new Date(), hour = now.getHours();
            if (hour > 4 && hour < 6) { $("#labelwelcome").html("凌晨好！") }
            else if (hour < 9) { $("#labelwelcome").html("早上好！") }
            else if (hour < 12) { $("#labelwelcome").html("上午好！") }
            else if (hour < 14) { $("#labelwelcome").html("中午好！") }
            else if (hour < 17) { $("#labelwelcome").html("下午好！") }
            else if (hour < 19) { $("#labelwelcome").html("傍晚好！") }
            else if (hour < 22) { $("#labelwelcome").html("晚上好！") }
            else { $("#labelwelcome").html("夜深了，注意休息！") }
        }

        function f_heightChanged(options) {
            if (tab)
                tab.addHeight(options.diff);
            if (accordion && options.middleHeight - 25 > 0)
                accordion.setHeight(options.middleHeight - 25);
            if (accordion2 && options.middleHeight - 25 > 0)
                accordion2.setHeight(options.middleHeight - 25);
        }
        function f_addTab(tabid, text, url) {
            tab.addTabItem({ tabid: tabid, text: text, url: url });
        }

        function onSelect(node) {
            if (!node.data.url) return;
            var tabid = $(node.target).attr("tabid");
            if (!tabid) {
                //tabid = new Date().getTime();
                tabid = node.data.id;
                $(node.target).attr("tabid", tabid)
            }
            f_addTab(tabid, node.data.text, node.data.url);
        }
        function changepwd() {
            var dialog = $.ligerDialog.open({
                url: "system/hr_changepwd.aspx", width: 480, height: 250, title: "修改密码", buttons: [
                        {
                            text: '保存', onclick: function (item, dialog) {
                                dialog.frame.f_save();
                            }
                        },
                        {
                            text: '关闭', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a'
            });
        }

        function logout() {
            $.ligerDialog.confirm('您确认要退出系统？', function (yes) {
                if (yes) {
                    $.ajax({
                        type: 'post', dataType: 'json',
                        url: 'Data/login.ashx',
                        data: [{ name: 'Action', value: 'logout' }],
                        success: function (result) {
                            javascript: location.replace("http://localhost:50039/Account/LogOff?redirecturl=" + location);
                        }
                    });
                }
            });


        }
        function toolbar() {
            $.getJSON("data/Sys_App.ashx?Action=GetSysApp&rnd=" + Math.random(), function (data, textStatus) {
                $("#toolbar").ligerToolBar({
                    background: false,
                    items: data.Items

                });
                //unView();
            });
            var toolbar = new Toolbar({renderTo: 'filters', items: [],
            filters: [
                { id: 'filter-home', title: '桌面', bodyStyle: 'filter-home', handler: function () { f_addTab('home') } },
                { id: 'filter-sett', title: '个人设置', bodyStyle: 'filter-person', handler: function () { personalinfoupdate() } },
                { id: 'filter-sett', title: '修改密码', bodyStyle: 'filter-sett', handler: function () { changepwd() } },
                { id: 'filter-help', title: '版权信息', bodyStyle: 'filter-help', handler: function () { show_copyright() } },
                { id: 'filter-theme', title: '系统信息', bodyStyle: 'filter-theme', handler: function () { show_welcome(1) } },
                { id: 'filter-out', title: '退出系统', bodyStyle: 'filter-out', handler: function () { logout() } }
            ]
            }); 
            toolbar.render();
        }
        function f_according(appid) {
            if (!appid) appid = 1;

            var mainmenu = $("#accordion1");
            mainmenu.empty();
            $.getJSON("Data/Base.ashx?Action=GetSysApp&appid=" + appid + "&rnd=" + Math.random(), function (menus) {
                $(menus).each(function (i, menu) {
                    var item = $('<div title="' + menu.Menu_name + '"><ul class="menulist"></ul></div>');

                    $(menu.children).each(function (j, submenu) {
                        var subitem = $('<li><img/><span></span><div class="menuitem-l"></div><div class="menuitem-r"></div></li>');
                        subitem.attr({
                            url: submenu.Menu_url,
                            tabid: "tabid" + submenu.Menu_id,
                            menuno: submenu.Menu_id
                        });
                        $("img", subitem).attr("src", submenu.Menu_icon || submenu.icon);
                        $("span", subitem).html(submenu.Menu_name || submenu.text);

                        $("ul:first", item).append(subitem);
                    });
                    mainmenu.append(item);
                });
                accordion.render();
                accordion.setHeight($(".l-layout-center").height() - 25);
            });

            $("#pageloading").fadeOut(800);



            //tabid计数器，保证tabid不会重复
            var tabidcounter = 0;
            //菜单初始化
            $("ul.menulist li").live('click', function () {
                var jitem = $(this);
                var tabid = jitem.attr("tabid");
                var url = jitem.attr("url");
                if (!url) return;
                if (!tabid) {
                    tabidcounter++;
                    tabid = "tabid" + tabidcounter;
                    jitem.attr("tabid", tabid);

                    //给url附加menuno
                    if (url.indexOf('?') > -1) url += "&";
                    else url += "?";
                    url += "MenuNo=" + jitem.attr("menuno");
                    jitem.attr("url", url);
                }
                //$("#mainframe").attr("src", url);                

                f_addTab(tabid, $("span:first", jitem).html(), url); if ($(this).hasClass("selected")) {
                    return;
                }
                else {
                    $(".selected").removeClass("selected");
                    $(this).addClass("selected");
                }

            }).live('mouseover', function () {
                var jitem = $(this);
                jitem.addClass("over");
            }).live('mouseout', function () {
                var jitem = $(this);
                jitem.removeClass("over");
            });

        }
        function show_welcome(item) {
            if (getCookie("kfcrm_show_wellcome") == 1 || item == 1) {
                var dialog = $.ligerDialog.open({
                    url: "welcome.htm", width: 800, height: 500, title: "欢迎使用KFCRM系统", buttons: [
                            {
                                text: '关闭', onclick: function (item, dialog) {
                                    dialog.close();
                                }
                            }
                    ], isResize: true, timeParmName: 'a'
                });
                SetCookie("kfcrm_show_wellcome", "2");
            }
        }
        function show_copyright() {
            var dialog = $.ligerDialog.open({
                url: "copyright.html", width: 800, height: 500, title: "版权信息", buttons: [
                        {
                            text: '关闭', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a'
            });
        }
        function personalinfoupdate() {
            var dialog = $.ligerDialog.open({
                url: "hr/emp_personal_update.aspx", width: 760, height: 300, title: "个人信息", buttons: [
                        {
                            text: '保存', onclick: function (item, dialog) {
                                dialog.frame.f_save();
                            }
                        },
                        {
                            text: '关闭', onclick: function (item, dialog) {
                                dialog.close();
                            }
                        }
                ], isResize: true, timeParmName: 'a'
            });
        }
        function flushiframegrid(tabid) {
            tab.flushiframegrid(tabid);
        }
    </script>
    <style type="text/css">
        /* 菜单列表 */
        .menulist { margin-left: 2px; margin-right: 2px; margin-top: 2px; text-align: left; color: #000; }
            .menulist li { height: 24px; line-height: 24px; padding-left: 24px; display: block; position: relative; cursor: pointer; text-align: left; }
                .menulist li img { position: absolute; left: 4px; top: 4px; width: 16px; height: 16px; }
                .menulist li.over, .menulist li.selected { background: url('Images/index/menuitem.gif') repeat-x 0px 0px; }
                    .menulist li.over .menuitem-l, .menulist li.selected .menuitem-l { background: url('Images/index/menuitem.gif') repeat-x 0px -24px; width: 2px; height: 24px; position: absolute; left: 0; top: 0; }
                    .menulist li.over .menuitem-r, .menulist li.selected .menuitem-r { background: url('Images/index/menuitem.gif') repeat-x -1px -24px; width: 2px; height: 24px; position: absolute; right: 0; top: 0; }
        #portrait { border-radius: 4px; box-shadow: 1px 1px 1px #111; position: absolute; width: 48px; height: 48px; right: 7px; top: 10px; background: #d2d2f2 /*url(images/icons/32X32/user.gif) no-repeat center center*/; border: 3px solid #fff; behavior: url(css/pie.htc); text-align: center; }
    </style>
</head>
<body>
    <form id="form1" onsubmit="return false">
        <div id="pageloading"></div>
        <div style="background: #d2e2f2; height: 74px; overflow: hidden;">
            <div style="height: 47px; margin: 0; padding: 0;">
                <div style="width: 278px; float: left;">
                    <a href="http://www.kinfar.net" target="_blank">
                        <img id="logo" alt="" src="Images/logo/KfCrm.png" style="height: 42px; margin-left: 5px; margin-top: 2px;" />
                    </a>
                </div>

                <div style="float: right; width: 220px; height: 47px; margin-right: 65px;">
                    <div style="width: 100%; height: 25px; text-align: right;">
                        <table style="width: 100%;">
                            <tr>
                                <td>
                                    <div id="jnkc" style="font-size: 12px; color: black; text-align: right;" />
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div style="width: 100%; height: 22px; line-height: 22px; vertical-align: bottom;">
                        <div id="labelwelcome" style="font-size: 12px; padding-right: 5px; width: 115px; margin-right: 80px; position: absolute; text-align: right;"></div>
                        <div id="Username" style="font-size: 12px; color: Red; padding-left: 18px; background: url(images/user.jpg) no-repeat left center; width: 80px; float: right;"></div>

                    </div>
                </div>

                <div id="portrait">
                    <%--<img id="user_headimg" src="imgage.gif" width='48' height='48' onerror="noheadimg" />--%>
                    <img id="userheader" width="48px" />
                </div>
            </div>
            <%--<div style="clear: both"></div>--%>
            <div style="margin: 0; padding: 0; background: url(images/headbg.gif); height: 28px; overflow: hidden; border-bottom: 1px solid #8db2e3; width: 100%;">
                <div id="toolbar" style="height: 27px; width: 780px; float: left; margin-top: 1px;"></div>
                <div id="filters" style="height: 27px; width: 200px; float: right; margin-right: 70px;"></div>
            </div>
        </div>


        <div id="layout1" style="width: 100%">

            <div position="left" title="功能菜单" id="accordion1">
            </div>
            <div position="center" id="framecenter" <%--title="首页"--%>>
                <div tabid="home" title="桌面" style="height: 300px">
                    <iframe frameborder="0" name="home" id="mainframe" src="personal/portal.aspx"></iframe>
                </div>
            </div>
            <div position="right" title="功能菜单" id="accordion2">
                <div title="在线用户" class="l-scroll">
                    <div id="onlineuser" style="margin: -1px">
                        <ul id="tree1"></ul>
                    </div>
                </div>
                <%--<div title="邮件列表" class="l-scroll">
                         
                    </div>
                    <div title="个人工作" class="l-scroll">
                         
                    </div>--%>
            </div>
            <div position="bottom">
                Copyright ? 2013-2020 kinfar.com All Rights Reserved
                <a href="http://www.kinfar.net" target="_blank">君飞CRM</a> QQ:313321669  v1.3.0  
            </div>
        </div>
    </form>
</body>
</html>
