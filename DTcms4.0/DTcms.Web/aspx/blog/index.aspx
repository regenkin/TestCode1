<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.index" ValidateRequest="false" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Text" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="DTcms.Common" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by DTcms Template Engine at 2016/4/2 23:21:54.
		本页面代码由DTcms模板引擎生成于 2016/4/2 23:21:54. 
	*/

	base.OnInit(e);
	StringBuilder templateBuilder = new StringBuilder(220000);

	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\"><head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n<title>");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_title));
	templateBuilder.Append("</title>\r\n<meta name=\"keywords\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_keyword));
	templateBuilder.Append("\" />\r\n<meta name=\"description\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_description));
	templateBuilder.Append("\" />\r\n<link href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/css/main.css\" rel=\"stylesheet\" type=\"text/css\" >\r\n<script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/js/jquery-1.8.3.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n<script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/js/jquery.event.drag-1.5.min.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n<script src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/js/jquery.touchSlider.js\" type=\"text/javascript\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\">\r\n        $(function () {\r\n//splid\r\n$(\".main_visual\").hover(function () {\r\n                $(\"#btn_prev,#btn_next\").fadeIn()\r\n            }, function () {\r\n                $(\"#btn_prev,#btn_next\").fadeOut()\r\n            });\r\n\r\n            $dragBln = false;\r\n\r\n            $(\".main_image\").touchSlider({\r\n                flexible: true,\r\n                speed: 200,\r\n                btn_prev: $(\"#btn_prev\"),\r\n                btn_next: $(\"#btn_next\"),\r\n                paging: $(\".flicking_con a\"),\r\n                counter: function (e) {\r\n                    $(\".flicking_con a\").removeClass(\"on\").eq(e.current - 1).addClass(\"on\");\r\n                }\r\n            });\r\n\r\n            $(\".main_image\").bind(\"mousedown\", function () {\r\n                $dragBln = false;\r\n            });\r\n\r\n            $(\".main_image\").bind(\"dragstart\", function () {\r\n                $dragBln = true;\r\n            });\r\n\r\n            $(\".main_image a\").click(function () {\r\n                if ($dragBln) {\r\n                    return false;\r\n                }\r\n            });\r\n\r\n            timer = setInterval(function () {\r\n                $(\"#btn_next\").click();\r\n            }, 5000);\r\n\r\n            $(\".main_visual\").hover(function () {\r\n                clearInterval(timer);\r\n            }, function () {\r\n                timer = setInterval(function () {\r\n                    $(\"#btn_next\").click();\r\n                }, 5000);\r\n            });\r\n\r\n            $(\".main_image\").bind(\"touchstart\", function () {\r\n                clearInterval(timer);\r\n            }).bind(\"touchend\", function () {\r\n                timer = setInterval(function () {\r\n                    $(\"#btn_next\").click();\r\n                }, 5000);\r\n            });\r\n\r\nvar $hoverBtn = $(\".h_defualt\");\r\n			var $defualt = $(this).find(\".mbbox2\");\r\n\r\n            $hoverBtn.hover(function () {\r\n                $(this).find(\".mbbox2\").stop().animate({ marginTop: -240 }, \"fast\");\r\n            }, function () {\r\n                $(this).find(\".mbbox2\").stop().animate({ marginTop: 0 }, \"fast\");\r\n            })\r\n\r\n        });\r\n    </");
	templateBuilder.Append("script>\r\n</head>\r\n<body>\r\n<!--Header-->\r\n");


	templateBuilder.Append("<div style=\"width:100%;height:28px;line-height:28px;vertical-align:middle;background:#e7e7e7;color:green;position:fixed;left:0px;top:0px;z-index:9999;font-size:13px;\"  >\r\n     <div style=\"margin-left:20px;height:100%\">\r\n           <a style=\"width:150px;margin-left:20px;height:100%\" href=\"");
	templateBuilder.Append(linkurl("index"));

	templateBuilder.Append("\">君飞首页</a>\r\n           <a style=\"width:50px;margin-left:20px;height:100%\" href=\"");
	templateBuilder.Append(linkurl("www_blog_list",0));

	templateBuilder.Append("\">博文阅读</a>\r\n     </div>\r\n</div>");


	templateBuilder.Append("\r\n<div style=\"width:100%;height:28px;\"></div>\r\n<div class=\"header\" style=\"\">\r\n<div class=\"content\">\r\n <span></span>\r\n <div class=\"menu\">\r\n");
	string _url="/"+System.IO.Path.GetFileName(Request.PhysicalPath);
	

	templateBuilder.Append("\r\n  <a href=\"");
	templateBuilder.Append(linkurl("index"));

	templateBuilder.Append("\" ");
	if (_url==linkurl("index"))
	{

	templateBuilder.Append(" class=\"ved\" ");
	}	//end for if

	templateBuilder.Append(">首页</a>\r\n<a href=\"");
	templateBuilder.Append(linkurl("www_product"));

	templateBuilder.Append("\" ");
	if (_url==linkurl("www_product"))
	{

	templateBuilder.Append(" class=\"ved\" ");
	}	//end for if

	templateBuilder.Append(">产品服务</a>\r\n<a href=\"");
	templateBuilder.Append(linkurl("www_case"));

	templateBuilder.Append("\" ");
	if (_url==linkurl("www_case"))
	{

	templateBuilder.Append(" class=\"ved\" ");
	}	//end for if

	templateBuilder.Append(">案例</a>\r\n<a href=\"");
	templateBuilder.Append(linkurl("www_about"));

	templateBuilder.Append("\" ");
	if (_url==linkurl("www_about"))
	{

	templateBuilder.Append(" class=\"ved\" ");
	}	//end for if

	templateBuilder.Append(">关于我们</a>\r\n </div>\r\n </div>\r\n</div>");


	templateBuilder.Append("\r\n<!--/Header-->\r\n<div class=\"content\">\r\n    <div class=\"main_visual\">\r\n	<div class=\"flicking_con\">\r\n		<a href=\"#\">1</a>\r\n		<a href=\"#\">2</a>\r\n		<a href=\"#\">3</a>\r\n		<a href=\"#\">4</a>\r\n	</div>\r\n	<div class=\"main_image\">\r\n		<ul>\r\n			<li><span class=\"img_3\"></span></li>\r\n			<li><span class=\"img_4\"></span></li>\r\n			<li><span class=\"img_1\"></span></li>\r\n			<li><span class=\"img_2\"></span></li>\r\n		</ul>\r\n		<a href=\"javascript:;\" id=\"btn_prev\"></a>\r\n		<a href=\"javascript:;\" id=\"btn_next\"></a>\r\n	</div>\r\n</div>\r\n  <!--区域二-->\r\n  <div id=\"art2\" class=\"corbox clearfloat\"  style=\"background-color:#fcfcfc\">\r\n    <div class=\"box2 clearfloat\">\r\n     <div class=\"title clearfloat\">\r\n      <h3>君飞工作室</h3>\r\n      <p>专注互联网软件 开发 设计 服务</p>\r\n     </div>\r\n     <ul>\r\n      <li class=\"h_defualt\" style=\"cursor:pointer\">\r\n       <div class=\"mbbox3\">\r\n       <img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/picture/btwo_ico1.png\" /><span>软件定制服务</span><p>企业信息化软件定制开发</p>\r\n       </div>\r\n      <div class=\"mbbox2\">\r\n       <p>软件开发服务<br />软件技术支持与维护<br />软件二次开发合作</p>\r\n       <span><b>软件定制服务</b></span>\r\n      </div>\r\n      </li>\r\n      <li class=\"h_defualt\" style=\"cursor:pointer\">\r\n       <div class=\"mbbox3\" >\r\n       <img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/picture/btwo_ico2.png\" /><span>网站建设</span><p>专注原创定制网站建设</p>\r\n       </div>\r\n      <div class=\"mbbox2\">\r\n       <p>整站制作、SEO优化<br />模板定制<br />网页前后端设计</p>\r\n       <span><b>定制网站建设</b></span>\r\n      </div>\r\n      </li>\r\n      <li class=\"h_defualt\"  style=\"cursor:pointer\">\r\n       <div class=\"mbbox3\" >\r\n       <img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/picture/btwo_ico3.png\" /><span>专业平面设计</span><p>无创意、不设计</p>\r\n       </div>\r\n      <div class=\"mbbox2\">\r\n       <p>形象LOGO<br />广告彩页<br />宣传画册</p>\r\n       <span><b>无创意、不设计</b></span>\r\n      </div>\r\n      </li>\r\n      <li class=\"h_defualt\">\r\n       <div class=\"mbbox3\">\r\n       <img src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/picture/btwo_ico4.png\" /><span>KF开发平台</span><p>快速构建企业应用</p>\r\n       </div>\r\n      <div class=\"mbbox2\">\r\n       <p>面向业务应用的管理软件开发平台<br />快速地开发应用<br />快速地对软件系统进行调整<br />降低了软件开发、实施和维护成本<br />良好性能和易用性的富客户端</p>\r\n       <span><b>KF.net Framework开发平台</b></span>\r\n      </div>\r\n      </li>\r\n     </ul>\r\n    </div>\r\n  </div>\r\n \r\n</div>\r\n\r\n<!--Footer-->\r\n");

	templateBuilder.Append("<div class=\"foot m-bg text-center col-md-12\">Copyright &copy; 2007-2016 kinfar.net studio,All Rights Reserved.</p></div>\r\n");


	templateBuilder.Append("\r\n<!--/Footer-->\r\n</body>\r\n</html>");
	Response.Write(templateBuilder.ToString());
}
</script>
