<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.search" ValidateRequest="false" %>
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

	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n<title>站内搜索 - ");
	templateBuilder.Append(Utils.ObjectToStr(site.name));
	templateBuilder.Append("</title>\r\n<meta name=\"keywords\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_keyword));
	templateBuilder.Append("\" />\r\n<meta name=\"description\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(site.seo_description));
	templateBuilder.Append("\" />\r\n<link href=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("css/pagination.css\" rel=\"stylesheet\" />\r\n<link href=\"");
	templateBuilder.Append("/templates/blog");
	templateBuilder.Append("/css/style.css\" rel=\"stylesheet\" type=\"text/css\" />\r\n<script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("scripts/jquery/jquery-1.11.2.min.js\"></");
	templateBuilder.Append("script>\r\n<script type=\"text/javascript\" charset=\"utf-8\" src=\"");
	templateBuilder.Append("/templates/blog");
	templateBuilder.Append("/js/common.js\"></");
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


	templateBuilder.Append("\r\n<!--/Header-->\r\n\r\n<div class=\"section clearfix\">\r\n  <!--右边-->\r\n  <div class=\"list-right\">\r\n    <div class=\"sidebar-box\">\r\n      <div class=\"line30\"></div>\r\n      <h3>热门标签</h3>\r\n      <div class=\"tags-box\">\r\n        ");
	DataTable tagsList = get_article_tags(0, "is_red=1");

	foreach(DataRow dr in tagsList.Rows)
	{

	templateBuilder.Append("\r\n          <a href=\"");
	templateBuilder.Append(linkurl("search","?tags="+Utils.ObjectToStr(dr["title"])));

	templateBuilder.Append("\">" + Utils.ObjectToStr(dr["title"]) + "<i>(" + Utils.ObjectToStr(dr["count"]) + ")</i></a>\r\n        ");
	}	//end for if

	templateBuilder.Append("\r\n      </div>\r\n      \r\n      <div class=\"line20\"></div>\r\n      <h3>推荐资讯</h3>\r\n      <div class=\"focus-list\">\r\n        <ul>\r\n          ");
	DataTable redNews = get_article_list("news", 0, 4, "status=0 and is_red=1 and img_url<>''");

	foreach(DataRow dr in redNews.Rows)
	{

	templateBuilder.Append("\r\n          <li>\r\n            <a title=\"" + Utils.ObjectToStr(dr["title"]) + "\" href=\"");
	templateBuilder.Append(linkurl("news_show",Utils.ObjectToStr(dr["id"])));

	templateBuilder.Append("\">\r\n              <b><img src=\"" + Utils.ObjectToStr(dr["img_url"]) + "\" /></b>\r\n              <span>" + Utils.ObjectToStr(dr["title"]) + "</span>\r\n            </a>\r\n          </li>\r\n          ");
	}	//end for if

	templateBuilder.Append("\r\n        </ul>\r\n      </div>\r\n      \r\n    </div>\r\n  </div>\r\n  <!--/右边-->\r\n  \r\n  <!--左边-->\r\n  <div class=\"list-auto\">\r\n    <!--取得一个DataTable-->\r\n    ");
	DataTable list = get_search_list(15, out totalcount);

	templateBuilder.Append("\r\n    <!--取得分页页码列表-->\r\n    ");
	if (tags!="")
	{

	 pagelist = get_page_link(15, page, totalcount, linkurl("search","?channel=" + channel + "&tags=" + Server.UrlEncode(tags) + "&page=__id__"));

	}
	else
	{

	 pagelist = get_page_link(15, page, totalcount, linkurl("search","?channel=" + channel + "&keyword=" + Server.UrlEncode(keyword) + "&page=__id__"));

	}	//end for if

	templateBuilder.Append("\r\n    <div class=\"ntitle\">\r\n      <h2><a>站内搜索</a></h2>\r\n      ");
	if (tags!="")
	{

	templateBuilder.Append("\r\n        <i>查询Tags标签： <b class=\"blue\">");
	templateBuilder.Append(Utils.ObjectToStr(tags));
	templateBuilder.Append("</b> ，共有 <b class=\"red\">");
	templateBuilder.Append(Utils.ObjectToStr(totalcount));
	templateBuilder.Append("</b> 条记录</i>\r\n      ");
	}
	else
	{

	templateBuilder.Append("\r\n        <i>搜索关健字： <b class=\"blue\">");
	templateBuilder.Append(Utils.ObjectToStr(keyword));
	templateBuilder.Append("</b> ，共有 <b class=\"red\">");
	templateBuilder.Append(Utils.ObjectToStr(totalcount));
	templateBuilder.Append("</b> 条记录</i>\r\n      ");
	}	//end for if

	templateBuilder.Append("\r\n    </div>\r\n    <ul class=\"n-list\">\r\n      ");
	foreach(DataRow dr in list.Rows)
	{

	templateBuilder.Append("\r\n      <li>\r\n        <h2><a target=\"_blank\" href=\"" + Utils.ObjectToStr(dr["link_url"]) + "\">" + Utils.ObjectToStr(dr["title"]) + "</a></h2>\r\n        <div class=\"note\">\r\n          ");
	if (Utils.ObjectToStr(dr["img_url"])!="")
	{

	templateBuilder.Append("\r\n          <b><img src=\"" + Utils.ObjectToStr(dr["img_url"]) + "\" /></b>\r\n          ");
	}	//end for if

	templateBuilder.Append("\r\n          <p>" + Utils.ObjectToStr(dr["remark"]) + "</p>\r\n          <div class=\"info\">\r\n            <span class=\"time\">" + Utils.ObjectToStr(dr["add_time"]) + "</span>\r\n            <span class=\"comm\"><script type=\"text/javascript\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("tools/submit_ajax.ashx?action=view_comment_count&id=" + Utils.ObjectToStr(dr["id"]) + "\"></");
	templateBuilder.Append("script>人评论</span>\r\n            <span class=\"view\"><script type=\"text/javascript\" src=\"");
	templateBuilder.Append(Utils.ObjectToStr(config.webpath));
	templateBuilder.Append("tools/submit_ajax.ashx?action=view_article_click&id=" + Utils.ObjectToStr(dr["id"]) + "\"></");
	templateBuilder.Append("script>次浏览</span>\r\n          </div>\r\n        </div>\r\n      </li>\r\n      ");
	}	//end for if

	if (totalcount==0)
	{

	templateBuilder.Append("\r\n      <div class=\"nodata\">很抱歉，目前尚未查找到符合条件的信息！</div>\r\n      ");
	}	//end for if

	templateBuilder.Append("\r\n    </ul>\r\n    \r\n    <!--页码列表-->\r\n    <div class=\"page-box\">\r\n      <div class=\"digg\">");
	templateBuilder.Append(Utils.ObjectToStr(pagelist));
	templateBuilder.Append("</div>\r\n    </div>\r\n    <!--/页码列表-->\r\n  </div>\r\n  <!--/左边-->\r\n\r\n</div>\r\n\r\n<!--Footer-->\r\n");

	templateBuilder.Append("<div class=\"foot m-bg text-center col-md-12\">Copyright &copy; 2007-2016 kinfar.net studio,All Rights Reserved.</p></div>\r\n");


	templateBuilder.Append("\r\n<!--/Footer-->\r\n</body>\r\n</html>");
	Response.Write(templateBuilder.ToString());
}
</script>
