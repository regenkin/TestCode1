<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.article_show" ValidateRequest="false" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Text" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="DTcms.Common" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by DTcms Template Engine at 2016/4/2 23:27:30.
		本页面代码由DTcms模板引擎生成于 2016/4/2 23:27:30. 
	*/

	base.OnInit(e);
	StringBuilder templateBuilder = new StringBuilder(220000);
	const string channel = "www_blog";

	templateBuilder.Append("<!DOCTYPE html>\r\n<html>\r\n<head>\r\n<meta content=\"text/html;charset=utf-8\" http-equiv=\"Content-Type\" />\r\n<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\" />\r\n<meta name=\"viewport\" content=\"width=device-width, minimumscale=1.0, maximum-scale=1.0\" />\r\n<meta name=\"language\" content=\"zh-cn\" />\r\n<meta name=\"renderer\" content=\"webkit\" />\r\n<title>");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_title));
	templateBuilder.Append("</title>\r\n<meta name=\"keywords\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_keyword));
	templateBuilder.Append("\" />\r\n<meta name=\"description\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_description));
	templateBuilder.Append("\" />\r\n<script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/js/jquery.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n<link rel=\"shortcut icon\" href=\"/favicon.ico\" />\r\n<link href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("css/pagination.css\" rel=\"stylesheet\" />\r\n<link rel=\"stylesheet\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/css/bootstrap.min.css\">\r\n<!--[if lt IE 9]>\r\n<link rel=\"stylesheet\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/css/bootstrap.min_1.css\" />\r\n<script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/js/respond.min.js\"></");
	templateBuilder.Append("script>\r\n<script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/js/html5shiv.min.js\"></");
	templateBuilder.Append("script>\r\n<![endif]-->\r\n<link rel=\"stylesheet\" type=\"text/css\" href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/css/love.css\" />\r\n<script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/js/bootstrap.min.js\"></");
	templateBuilder.Append("script>\r\n</head>\r\n<body>\r\n<!--Header-->\r\n");

	templateBuilder.Append("<div style=\"width:100%;height:28px;line-height:28px;vertical-align:middle;background:#e7e7e7;color:green;position:fixed;left:0px;top:0px;z-index:9999;font-size:13px;\"  >\r\n     <div style=\"margin-left:20px;height:100%\">\r\n           <a style=\"width:150px;margin-left:20px;height:100%\" href=\"");
	templateBuilder.Append(linkurl("index"));

	templateBuilder.Append("\">君飞首页</a>\r\n           <a style=\"width:50px;margin-left:20px;height:100%\" href=\"");
	templateBuilder.Append(linkurl("www_blog_list",0));

	templateBuilder.Append("\">博文阅读</a>\r\n     </div>\r\n</div>");


	templateBuilder.Append("\r\n<!--/Header-->\r\n    <div class=\"container main\">\r\n        <div class=\"row\">\r\n            <div class=\"col-md-9\">\r\n                <div class=\"m-bg\">\r\n                    <div class=\"text-center border-dotted-b\">\r\n                        <h1>");
	templateBuilder.Append(Utils.ObjectToStr(model.title));
	templateBuilder.Append("</h1>\r\n                    </div>\r\n                    <div class=\"border-dotted-b padding-10\">\r\n                        <span class=\"g-bg glyphicon glyphicon-user margin-r-5\" aria-hidden=\"true\"></span>");
	templateBuilder.Append(get_article_field(model.id, "author").ToString());

	templateBuilder.Append("\r\n                        <span class=\"g-bg glyphicon glyphicon-calendar margin-l-10 margin-r-5\" aria-hidden=\"true\"></span>");
	templateBuilder.Append(Utils.ObjectToStr(model.add_time));
	templateBuilder.Append("\r\n                        <span class=\"g-bg glyphicon glyphicon-folder-close margin-l-10 margin-r-5\" aria-hidden=\"true\"></span>");
	templateBuilder.Append(get_category_title(model.category_id, "未分类").ToString());

	templateBuilder.Append("\r\n                         <span class=\"g-bg glyphicon glyphicon-eye-open margin-l-10 margin-r-5\" aria-hidden=\"true\"></span><script type=\"text/javascript\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("tools/submit_ajax.ashx?action=view_article_click&id=");
	templateBuilder.Append(Utils.ObjectToStr(model.id));
	templateBuilder.Append("&click=1\"></");
	templateBuilder.Append("script>次阅读\r\n                    </div>\r\n                    <div class=\"padding-10\">\r\n                        <div class=\"miaoshu\">&nbsp;");
	templateBuilder.Append(Utils.ObjectToStr(model.zhaiyao));
	templateBuilder.Append("</div>\r\n                        ");
	templateBuilder.Append(Utils.ObjectToStr(model.content));
	templateBuilder.Append("\r\n                   </div>\r\n                    <div class=\"border-dotted-t padding-10 text-right\">\r\n                        <span class=\"g-bg glyphicon glyphicon-tags margin-r-5\" aria-hidden=\"true\"></span>\r\n                        <a class=\"btn btn-xs btn-default margin-2\" href=\"#\">");
	templateBuilder.Append(Utils.ObjectToStr(model.tags));
	templateBuilder.Append("</a></div>\r\n                    <ul class=\"pager border-dotted-b padding-b-10\">\r\n                        <li class=\"previous\">");
	templateBuilder.Append(get_prevandnext_article("www_blog_content", -1, "<a href='javascript:'>无</a>", 0).ToString());

	templateBuilder.Append("</li>\r\n                        <li class=\"next\">");
	templateBuilder.Append(get_prevandnext_article("www_blog_content", 1, "<a href='javascript:'>无</a>", 0).ToString());

	templateBuilder.Append("</li>\r\n                    </ul>\r\n");
	int totalcount = 0;

	DataTable dtcomment = get_comment_list(model.id, 10, 1, "is_lock=0", out totalcount);

	int dr__loop__id=0;
	foreach(DataRow dr in dtcomment.Rows)
	{
		dr__loop__id++;


	templateBuilder.Append("\r\n  第");
	templateBuilder.Append(Utils.ObjectToStr(dr__loop__id));
	templateBuilder.Append("楼：" + Utils.ObjectToStr(dr["content"]) + "\r\n");
	}	//end for if

	templateBuilder.Append("\r\n                    <div class=\"bdsharebuttonbox bdshare-button-style0-24\">\r\n                        <a class=\"bds_more\" data-cmd=\"more\"></a>\r\n                        <a title=\"分享到QQ空间\" class=\"bds_qzone\" data-cmd=\"qzone\"></a>\r\n                        <a title=\"分享到新浪微博\" class=\"bds_tsina\" data-cmd=\"tsina\"></a>\r\n                        <a title=\"分享到腾讯微博\" class=\"bds_tqq\" data-cmd=\"tqq\"></a>\r\n                        <a title=\"分享到人人网\" class=\"bds_renren\" data-cmd=\"renren\"></a>\r\n                        <a title=\"分享到微信\" class=\"bds_weixin\" data-cmd=\"weixin\"></a>\r\n                    </div>\r\n                </div>\r\n            </div>\r\n            <div class=\"col-md-3\">\r\n                <!-- 右边栏开始 -->\r\n                <div class=\"post-sidebar margin-t-10\">\r\n                    <div class=\"panel panel-default\">\r\n                        <div class=\"panel-heading\">\r\n                            <h4 class=\"panel-title\"><span aria-hidden=\"true\" class=\"g-bg glyphicon glyphicon-list margin-r-5\"></span>目录</h4>\r\n                        </div>\r\n                        <ul class=\"list-group\">\r\n                            <li class=\"list-group-item b-t-r\">\r\n<a href=\"");
	templateBuilder.Append(linkurl("www_blog_list",0));

	templateBuilder.Append("\" class=\"btn btn-default width-80 btn-sm margin-b-5\" title=\"全部\">全部</a> \r\n");
	DataTable dt = get_category_list("www_blog", 0);

	foreach(DataRow dr in dt.Rows)
	{

	templateBuilder.Append("\r\n<a href=\"");
	templateBuilder.Append(linkurl("www_blog_list",Utils.ObjectToStr(dr["id"])));

	templateBuilder.Append("\" class=\"btn btn-default width-80 btn-sm margin-b-5\" title=\"" + Utils.ObjectToStr(dr["title"]) + "\">" + Utils.ObjectToStr(dr["title"]) + "</a> \r\n");
	}	//end for if

	templateBuilder.Append("\r\n                             </li>\r\n                        </ul>\r\n                    </div>\r\n                    <div class=\"panel panel-default\">\r\n                        <div class=\"panel-heading\">\r\n                            <h4 class=\"panel-title\">\r\n                                <span aria-hidden=\"true\" class=\"g-bg glyphicon glyphicon-list margin-r-5\"></span>\r\n                                推荐博文</h4>\r\n                        </div>\r\n                        <ul class=\"list-group\">\r\n");
	DataTable nlist = get_article_list("www_blog", model.category_id,9, "is_red=1","add_time desc");

	foreach(DataRow dr in nlist.Rows)
	{

	templateBuilder.Append("\r\n <li class=\"list-group-item b-t-r item-h clearfix\">\r\n          <div class=\"col-md-12\">\r\n               <a href=\"");
	templateBuilder.Append(linkurl("www_blog_content",Utils.ObjectToStr(dr["id"])));

	templateBuilder.Append("\" target=\"_blank\" title=\"" + Utils.ObjectToStr(dr["title"]) + "\">" + Utils.ObjectToStr(dr["title"]) + "</a>\r\n          </div>\r\n</li>\r\n");
	}	//end for if

	templateBuilder.Append("\r\n                        </ul>\r\n                    </div>\r\n                    <div class=\"panel panel-default\">\r\n                        <div class=\"panel-heading\">\r\n                            <h4 class=\"panel-title\">\r\n                                <span aria-hidden=\"true\" class=\"g-bg glyphicon glyphicon-list margin-r-5\"></span>最新文章</h4>\r\n                        </div>\r\n");
	DataTable lastlist = get_article_list("www_blog", model.category_id,9, "","add_time desc");

	foreach(DataRow dr in lastlist.Rows)
	{

	templateBuilder.Append("\r\n<ul class=\"list-group\">\r\n     <li class=\"list-group-item b-t-r item-h\"><span class=\"badge\">新</span><a href=\"");
	templateBuilder.Append(linkurl("www_blog_content",Utils.ObjectToStr(dr["id"])));

	templateBuilder.Append("\" target=\"_blank\" title=\"" + Utils.ObjectToStr(dr["title"]) + "\">" + Utils.ObjectToStr(dr["title"]) + "</a></li>\r\n</ul>\r\n");
	}	//end for if

	templateBuilder.Append("\r\n                    </div>\r\n                </div>\r\n                <!-- 右边栏结束 -->\r\n            </div>\r\n        </div>\r\n    </div>\r\n<!--Footer-->\r\n");

	templateBuilder.Append("<div class=\"foot m-bg text-center col-md-12\">Copyright &copy; 2007-2016 kinfar.net studio,All Rights Reserved.</p></div>\r\n");


	templateBuilder.Append("\r\n<!--/Footer-->\r\n<div class=\"top_scroll\" id=\"top_scroll\">\r\n<div id=\"_top\"><span aria-hidden=\"true\" class=\"glyphicon glyphicon-plane\"></span></div>\r\n<div id=\"_bottom\"><span aria-hidden=\"true\" class=\"glyphicon glyphicon-chevron-down\"></span></div>\r\n</div>\r\n</body>\r\n<script type=\"text/javascript\">\r\n$(function(){\r\n	//禁止无效链接\r\n	$('.disabled a').removeAttr('href');\r\n	//返回顶部\r\n	showScroll();\r\n	function showScroll(){\r\n		$(window).scroll( function() {\r\n			var scrollValue=$(window).scrollTop();\r\n			scrollValue > 100 ? $('div[class=top_scroll]').fadeIn():$('div[class=top_scroll]').fadeOut();\r\n		} );\r\n		$('#_top').click(function(){\r\n			$(\"html,body\").animate({scrollTop:0},200);\r\n		});\r\n		$('#_bottom').click(function(){\r\n			$(\"html,body\").animate({scrollTop:$('body').height()},200);\r\n		});\r\n	};\r\n	//右侧菜单\r\n	 if($('li').hasClass('li_bg')){\r\n	 	$('#accordion .in').removeClass('in');\r\n   		$('.li_bg').parent().parent().addClass('in');\r\n	}\r\n	//美化上传组件\r\n	$('input[type=file]').change(function() {\r\n		$('#tmp_file').val($(this).val());\r\n	});\r\n	//导航点击变色\r\n	$('.navbar-nav a[href$=\"post/index\"]').parent().addClass('active');\r\n	//TAB\r\n	$(\"#myTab a\").mouseover(function() {\r\n		$('#myTab .active').removeClass('active');\r\n		$(\"#myTabContent .active\").removeClass('active');\r\n		var data_id=$(this).attr('data-id');\r\n		$(this).parent().addClass('active');\r\n		$('#'+data_id).addClass('in active');\r\n	});\r\n});\r\n</");
	templateBuilder.Append("script>\r\n</html>\r\n");
	Response.Write(templateBuilder.ToString());
}
</script>
