<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.article" ValidateRequest="false" %>
<%@ Import namespace="System.Collections.Generic" %>
<%@ Import namespace="System.Text" %>
<%@ Import namespace="System.Data" %>
<%@ Import namespace="DTcms.Common" %>

<script runat="server">
override protected void OnInit(EventArgs e)
{

	/* 
		This page was created by DTcms Template Engine at 2016/4/2 23:21:55.
		本页面代码由DTcms模板引擎生成于 2016/4/2 23:21:55. 
	*/

	base.OnInit(e);
	StringBuilder templateBuilder = new StringBuilder(220000);
	const string channel = "www_about";

	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\"><head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n<title>");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_title));
	templateBuilder.Append("</title>\r\n<meta name=\"keywords\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_keyword));
	templateBuilder.Append("\" />\r\n<meta name=\"description\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_description));
	templateBuilder.Append("\" />\r\n<link href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("templates/www/css/main.css\" rel=\"stylesheet\" type=\"text/css\" >\r\n</head>\r\n<body>\r\n<!--Header-->\r\n");


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


	templateBuilder.Append("\r\n<!--/Header-->\r\n<div style=\"height:20px\"></div>\r\n<div style=\"margin:0 auto;width:900px;\">\r\n<h2>君飞工作室</h2>\r\n<p>&nbsp; &nbsp;&nbsp; &nbsp;</p>\r\n<p style=\"line-height:40px;\">&nbsp; &nbsp;&nbsp; &nbsp;隶属深圳市天尊软件有限公司研发中心，主要负责互联网项目开发设计</p>\r\n\r\n<p>&nbsp; &nbsp;&nbsp; &nbsp;</p><p>&nbsp; &nbsp;&nbsp; &nbsp;</p>\r\n<img style=\"margin:0 auto;\" src=\"/upload/201603/18/201603181744019666.gif\"/>\r\n<p>&nbsp; &nbsp;</p><p>&nbsp; &nbsp;</p>\r\n\r\n<p style=\"line-height:40px\">&nbsp; &nbsp;&nbsp; &nbsp;深圳市天尊软件有限公司，是国内知名的企业管理信息化系统供应商，自成立以来以“致力行业管理，为顾客创造价值”为己任，专注于制造行业、贸易行业和地产行业的企业管理信息化研发和技术服务, 帮助企业利用信息化从根本上解决管理难题。 \r\n    公司现有十数名专业企业管理顾问，在各行业上积累了500家企业的实践经验和知识技巧，覆盖了研发、咨询/实施、销售、市场、管理、 财务等多个领域。</p>\r\n<p style=\"line-height:40px\">&nbsp; &nbsp;&nbsp; &nbsp; 公司目前主要产品有：T10系列制造业ERP系统、T10五金业ERP系统、T10注塑业ERP系统、T9系列商贸ERP系统、标准财务系统、售楼管理系统、云管理系统、手机APP。</p>\r\n\r\n<p>&nbsp; &nbsp;&nbsp; &nbsp;</p><p>&nbsp; &nbsp;&nbsp; &nbsp;</p>\r\n<img style=\"margin:0 auto;\" src=\"/upload/201603/18/201603181756531229.jpg\"/>\r\n<p>&nbsp; &nbsp;</p><p>&nbsp; &nbsp;</p>\r\n\r\n<p style=\"line-height:40px\"> &nbsp; &nbsp;&nbsp; &nbsp;服务体系及服务团队的培养一直是天尊软件的重要建设项目，经过长期的市场实践，有着丰富的上线经验，目前已拥有一套完善的实施服务体系，为用户提供本地化、个性化的增值服务，帮助企业最大化的创造价值。</p>\r\n\r\n<p>&nbsp; &nbsp;&nbsp; &nbsp;</p><p>&nbsp; &nbsp;&nbsp; &nbsp;</p>\r\n<img style=\"margin:0 auto;\" src=\"/upload/201603/18/201603181759009979.jpg\"/>\r\n<p>&nbsp; &nbsp;</p><p>&nbsp; &nbsp;</p>\r\n<p>&nbsp; &nbsp;</p>\r\n<p>&nbsp; &nbsp;</p>\r\n<p>&nbsp; &nbsp;</p>\r\n<p>&nbsp; &nbsp;</p>\r\n<p>&nbsp; &nbsp;</p>\r\n<p>&nbsp; &nbsp;</p>\r\n<p>&nbsp; &nbsp;</p>\r\n<p>&nbsp; &nbsp;</p>\r\n<p>&nbsp; &nbsp;</p>\r\n<p>&nbsp; &nbsp;</p>\r\n<p>&nbsp; &nbsp;</p>\r\n</div>\r\n<!--Footer-->\r\n");

	templateBuilder.Append("<div class=\"foot m-bg text-center col-md-12\">Copyright &copy; 2007-2016 kinfar.net studio,All Rights Reserved.</p></div>\r\n");


	templateBuilder.Append("\r\n<!--/Footer-->\r\n</body>\r\n</html>");
	Response.Write(templateBuilder.ToString());
}
</script>
