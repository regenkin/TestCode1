<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.oauth_login" ValidateRequest="false" %>
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

	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n<title>合作网站登录 - ");
	templateBuilder.Append(Utils.ObjectToStr(site.name));
	templateBuilder.Append("</title>\r\n<meta name=\"keywords\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_keyword));
	templateBuilder.Append("\" />\r\n<meta name=\"description\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_description));
	templateBuilder.Append("\" />\r\n<link href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/artdialog/ui-dialog.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n<link href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("css/validate.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n<link href=\"");
	templateBuilder.Append("/templates/blog");
	templateBuilder.Append("/css/style.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n<script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/jquery/jquery-1.11.2.min.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/jquery/jquery.form.min.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/jquery/Validform_v5.3.2_min.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/artdialog/dialog-plus-min.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append("/templates/blog");
	templateBuilder.Append("/js/common.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\">\r\n$(function(){\r\n	tabs('#oauthTabs','click');\r\n	//初始化表单1\r\n	AjaxInitForm('#bindForm', '#btnSubmit1', 0, '#turl');\r\n	//初始化表单2\r\n	AjaxInitForm('#oauthForm', '#btnSubmit2', 0, '#turl');\r\n});\r\n</");
	templateBuilder.Append("script>\r\n</head>\r\n\r\n<body>\r\n<!--Header-->\r\n");


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


	templateBuilder.Append("\r\n<!--/Header-->\r\n\r\n<div class=\"main-box\">\r\n  <div class=\"section clearfix\">\r\n    <div class=\"main-tit\">\r\n      <h2>合作网站登录</h2>\r\n    </div>\r\n    <div id=\"oauthTabs\" class=\"inner-box\">\r\n      <!--选项卡-->\r\n      <div id=\"tabHead\" class=\"tab-head\">\r\n        <ul>\r\n          <li>\r\n            <a class=\"selected\" href=\"javascript:;\">绑定已有的账号</a>\r\n          </li>\r\n          <li>\r\n            <a href=\"javascript:;\">创建新的账号</a>\r\n          </li>\r\n        </ul>\r\n      </div>\r\n      <!--/选项卡-->\r\n      \r\n      <!--选项内容-->\r\n      <form name=\"bindForm\" id=\"bindForm\" url=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("tools/submit_ajax.ashx?action=user_oauth_bind\">\r\n      <div class=\"dl-list tab-content\" style=\"display:block;\">\r\n        <dl>\r\n          <dt>用户名：</dt>\r\n          <dd><input name=\"txtUserName\" type=\"text\" class=\"input txt\" datatype=\"*\"  nullmsg=\"请输入用户名\" sucmsg=\" \" /></dd>\r\n        </dl>\r\n        <dl>\r\n          <dt>密　码：</dt>\r\n          <dd><input name=\"txtPassword\" type=\"password\" class=\"input txt\" datatype=\"*6-20\"  nullmsg=\"请输入密码\" sucmsg=\" \" /></dd>\r\n        </dl>\r\n        <dl>\r\n          <dt></dt>\r\n          <dd><input id=\"btnSubmit1\" name=\"btnSubmit1\" type=\"submit\" class=\"btn\" value=\"立即绑定\" /></dd>\r\n        </dl>\r\n      </div>\r\n      </form>\r\n      \r\n      <form name=\"oauthForm\" id=\"oauthForm\" url=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("tools/submit_ajax.ashx?action=user_oauth_register\">\r\n      <div class=\"dl-list tab-content\">\r\n        <dl>\r\n          <dt>电子邮箱：</dt>\r\n          <dd><input name=\"txtEmail\" type=\"text\" class=\"input txt\" datatype=\"e\"  nullmsg=\"请输入邮箱地址\" sucmsg=\" \" /></dd>\r\n        </dl>\r\n        <dl>\r\n          <dt>手机号码：</dt>\r\n          <dd><input name=\"txtMobile\" type=\"text\" class=\"input txt\" datatype=\"m\"  nullmsg=\"请输入手机号码\" sucmsg=\" \" /></dd>\r\n        </dl>\r\n        <dl>\r\n          <dt>登录密码：</dt>\r\n          <dd><input name=\"txtPassword\" type=\"password\" class=\"input txt\" datatype=\"*6-20\"  nullmsg=\"请设置密码\" sucmsg=\" \" /></dd>\r\n        </dl>\r\n        <dl>\r\n          <dt></dt>\r\n          <dd><input id=\"btnSubmit2\" name=\"btnSubmit1\" type=\"submit\" class=\"btn\" value=\"创建新账户\" /></dd>\r\n        </dl>\r\n      </div>\r\n      </form>\r\n      <!--/选项内容-->\r\n      <input id=\"turl\" name=\"turl\" type=\"hidden\" value=\"");
	templateBuilder.Append(Utils.ObjectToStr(turl));
	templateBuilder.Append("\" />\r\n    </div>\r\n    \r\n  </div>\r\n</div>\r\n\r\n<!--Footer-->\r\n");

	templateBuilder.Append("<div class=\"foot m-bg text-center col-md-12\">Copyright &copy; 2007-2016 kinfar.net studio,All Rights Reserved.</p></div>\r\n");


	templateBuilder.Append("\r\n<!--/Footer-->\r\n</body>\r\n</html>");
	Response.Write(templateBuilder.ToString());
}
</script>
