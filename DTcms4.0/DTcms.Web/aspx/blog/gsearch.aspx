<%@ Page Language="C#" AutoEventWireup="true" Inherits="DTcms.Web.UI.Page.article_list" ValidateRequest="false" %>
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
	const string channel = "goods";
	const int pagesize = 20;

	templateBuilder.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\r\n<title>");
	templateBuilder.Append(Utils.ObjectToStr(model.title));
	templateBuilder.Append(" - ");
	templateBuilder.Append(Utils.ObjectToStr(site.name));
	templateBuilder.Append("</title>\r\n<meta name=\"keywords\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(model.seo_keywords));
	templateBuilder.Append("\" />\r\n<meta name=\"description\" content=\"");
	templateBuilder.Append(Utils.ObjectToStr(model.seo_description));
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


	templateBuilder.Append("\r\n<!--/Header-->\r\n\r\n<div class=\"section clearfix\">\r\n\r\n  <div class=\"wrapper auto clearfix\">\r\n    ");
	string category_nav = get_category_menu("goods_list", category_id);

	templateBuilder.Append("\r\n    <div class=\"curr-nav\">当前位置：<a href=\"");
	templateBuilder.Append(linkurl("index"));

	templateBuilder.Append("\">首页</a> &gt; <a href=\"");
	templateBuilder.Append(linkurl("goods"));

	templateBuilder.Append("\">购物商城</a>");
	templateBuilder.Append(Utils.ObjectToStr(category_nav));
	templateBuilder.Append("</div>\r\n    <!--C#代码-->\r\n    ");
	      string orderby="add_time desc,id asc";
	      string strBy=DTRequest.GetQueryString("orderby");
	      if(strBy=="click"){
	        orderby="click desc";
	      }else if(strBy=="min"){
	        orderby="sell_price asc";
	      }else if(strBy=="max"){
	        orderby="sell_price desc";
	      }
	      string strwhere="status=0";
	      int minPrice=DTRequest.GetQueryInt("min_price",0);
	      if(minPrice>0){
	        strwhere+="and sell_price>="+minPrice;
	      }
	      int maxPrice=DTRequest.GetQueryInt("max_price",0);
	      if(maxPrice>0){
	        strwhere+="and sell_price<="+maxPrice;
	      }
	      Dictionary<string,string> dicSpecIds=new Dictionary<string,string>();
	      for (int i = 0; i < Request.QueryString.AllKeys.Length; i++)
	      {
	        string paramKey=Request.QueryString.GetKey(i).ToString();
	        int paramValue=Utils.StrToInt(Request.QueryString[i].ToString(),0);
	        if(paramKey.StartsWith("s_") && paramValue>0)
	        {
	          dicSpecIds.Add(paramKey,paramValue.ToString());
	        }
	      }
	    

	templateBuilder.Append("\r\n    <!--/C#代码-->\r\n    <div class=\"screen-box\">\r\n      <dl>\r\n        <dt>分类：</dt>\r\n        <dd>\r\n          ");
	if (category_id==0)
	{

	templateBuilder.Append("\r\n            <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch"));

	templateBuilder.Append("\">全部</a>\r\n          ");
	}
	else
	{

	templateBuilder.Append("\r\n            <a href=\"");
	templateBuilder.Append(linkurl("gsearch"));

	templateBuilder.Append("\">全部</a>\r\n          ");
	}	//end for if

	DataTable categoryList = get_category_child_list(channel,0);

	foreach(DataRow cdr in categoryList.Rows)
	{

	if (category_id==Utils.StrToInt(Utils.ObjectToStr(cdr["id"]), 0))
	{

	templateBuilder.Append("\r\n              <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+Utils.ObjectToStr(cdr["id"])));

	templateBuilder.Append("\">" + Utils.ObjectToStr(cdr["title"]) + "</a>\r\n            ");
	}
	else
	{

	templateBuilder.Append("\r\n              <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+Utils.ObjectToStr(cdr["id"])));

	templateBuilder.Append("\">" + Utils.ObjectToStr(cdr["title"]) + "</a>\r\n            ");
	}	//end for if

	}	//end for if

	templateBuilder.Append("\r\n        </dd>\r\n      </dl>\r\n      \r\n      <!--规格列表-->\r\n      ");
	DataTable specList = get_article_spec_parent(channel, model.id);

	foreach(DataRow dr1 in specList.Rows)
	{

	templateBuilder.Append("\r\n      <dl>\r\n        <dt>" + Utils.ObjectToStr(dr1["title"]) + "：</dt>\r\n        <dd>\r\n          ");
	DataTable subList = get_article_spec_child(Utils.StrToInt(Utils.ObjectToStr(dr1["id"]), 0));

	string tempKey = "s_"+Utils.ObjectToStr(dr1["id"]);

	if (!dicSpecIds.ContainsKey(tempKey))
	{

	templateBuilder.Append("\r\n          <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby="+strBy+get_article_spec_param(dicSpecIds,"s_"+Utils.ObjectToStr(dr1["id"])+"=0")));

	templateBuilder.Append("\">全部</a>\r\n          ");
	}
	else
	{

	templateBuilder.Append("\r\n          <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby="+strBy+get_article_spec_param(dicSpecIds,"s_"+Utils.ObjectToStr(dr1["id"])+"=0")));

	templateBuilder.Append("\">全部</a>\r\n          ");
	}	//end for if

	foreach(DataRow dr2 in subList.Rows)
	{

	if (dicSpecIds.ContainsValue(Utils.ObjectToStr(dr2["id"])))
	{

	templateBuilder.Append("\r\n              <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby="+strBy+get_article_spec_param(dicSpecIds,"s_"+Utils.ObjectToStr(dr1["id"])+"="+Utils.ObjectToStr(dr2["id"]))));

	templateBuilder.Append("\">" + Utils.ObjectToStr(dr2["title"]) + "</a>\r\n            ");
	}
	else
	{

	templateBuilder.Append("\r\n              <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby="+strBy+get_article_spec_param(dicSpecIds,"s_"+Utils.ObjectToStr(dr1["id"])+"="+Utils.ObjectToStr(dr2["id"]))));

	templateBuilder.Append("\">" + Utils.ObjectToStr(dr2["title"]) + "</a>\r\n            ");
	}	//end for if

	}	//end for if

	templateBuilder.Append("\r\n        </dd>\r\n      </dl>\r\n      ");
	}	//end for if

	templateBuilder.Append("\r\n      <!--/规格列表-->\r\n      \r\n      <!--价格区间-->\r\n      <dl>\r\n        <dt>价格：</dt>\r\n        <dd>\r\n          ");
	if (minPrice==0&&maxPrice==0)
	{

	templateBuilder.Append("\r\n          <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">全部</a>\r\n          ");
	}
	else
	{

	templateBuilder.Append("\r\n          <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">全部</a>\r\n          ");
	}	//end for if

	if (minPrice==0&&maxPrice==100)
	{

	templateBuilder.Append("\r\n          <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price=0&max_price=100&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">0-100元</a>\r\n          ");
	}
	else
	{

	templateBuilder.Append("\r\n          <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price=0&max_price=100&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">0-100元</a>\r\n          ");
	}	//end for if

	if (minPrice==101&&maxPrice==500)
	{

	templateBuilder.Append("\r\n          <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price=101&max_price=500&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">101-500元</a>\r\n          ");
	}
	else
	{

	templateBuilder.Append("\r\n          <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price=101&max_price=500&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">101-500元</a>\r\n          ");
	}	//end for if

	if (minPrice==501&&maxPrice==1000)
	{

	templateBuilder.Append("\r\n          <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price=501&max_price=1000&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">501-1000元</a>\r\n          ");
	}
	else
	{

	templateBuilder.Append("\r\n          <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price=501&max_price=1000&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">501-1000元</a>\r\n          ");
	}	//end for if

	if (minPrice==1001&&maxPrice==2000)
	{

	templateBuilder.Append("\r\n          <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price=1001&max_price=2000&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">1001-2000元</a>\r\n          ");
	}
	else
	{

	templateBuilder.Append("\r\n          <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price=1001&max_price=2000&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">1001-2000元</a>\r\n          ");
	}	//end for if

	if (minPrice==2000&&maxPrice==0)
	{

	templateBuilder.Append("\r\n          <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price=2000&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">2000以上</a>\r\n          ");
	}
	else
	{

	templateBuilder.Append("\r\n          <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price=2000&orderby="+strBy+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">2000以上</a>\r\n          ");
	}	//end for if

	templateBuilder.Append("\r\n        </dd>\r\n      </dl>\r\n      <!--价格区间-->\r\n    </div>\r\n    \r\n    <div class=\"line15\"></div>\r\n    <!--列表排序-->\r\n    <div class=\"sort-box\">\r\n      ");
	if (strBy==""||strBy=="time")
	{

	templateBuilder.Append("\r\n      <a class=\"first selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby=time"+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">上架时间</a>\r\n      ");
	}
	else
	{

	templateBuilder.Append("\r\n      <a class=\"first\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby=time"+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">上架时间</a>\r\n      ");
	}	//end for if

	if (strBy=="click")
	{

	templateBuilder.Append("\r\n      <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby=click"+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">按人气</a>\r\n      ");
	}
	else
	{

	templateBuilder.Append("\r\n      <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby=click"+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">按人气</a>\r\n      ");
	}	//end for if

	if (strBy=="max")
	{

	templateBuilder.Append("\r\n      <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby=max"+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">价格从高到低</a>\r\n      ");
	}
	else
	{

	templateBuilder.Append("\r\n      <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby=max"+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">价格从高到低</a>\r\n      ");
	}	//end for if

	if (strBy=="min")
	{

	templateBuilder.Append("\r\n      <a class=\"selected\" href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby=min"+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">价格从低到高</a>\r\n      ");
	}
	else
	{

	templateBuilder.Append("\r\n      <a href=\"");
	templateBuilder.Append(linkurl("gsearch","?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby=min"+get_article_spec_param(dicSpecIds,"")));

	templateBuilder.Append("\">价格从低到高</a>\r\n      ");
	}	//end for if

	templateBuilder.Append("\r\n    </div>\r\n    <!--/列表排序-->\r\n    \r\n    <div class=\"img-list2\">\r\n      <ul>\r\n        ");
	DataTable goodsList = get_article_list(channel,category_id,dicSpecIds,pagesize,page,strwhere,orderby,out totalcount);

	templateBuilder.Append("<!--数据-->\r\n        ");
	 pagelist = Utils.OutPageList(pagesize, page, totalcount, linkurl("gsearch", "?category_id="+category_id+"&min_price="+minPrice+"&max_price="+maxPrice+"&orderby="+strBy+"&page=__id__"+get_article_spec_param(dicSpecIds,"")), 8);

	templateBuilder.Append("<!--分页-->\r\n        ");
	foreach(DataRow dr in goodsList.Rows)
	{

	templateBuilder.Append("\r\n        <li>\r\n          <div class=\"wrap-box\">\r\n            <div class=\"img-box\">\r\n              <a title=\"" + Utils.ObjectToStr(dr["title"]) + "\" href=\"");
	templateBuilder.Append(linkurl("goods_show",Utils.ObjectToStr(dr["id"])));

	templateBuilder.Append("\">\r\n                ");
	if (Utils.ObjectToStr(dr["is_red"])=="1")
	{

	templateBuilder.Append("\r\n                <div class=\"abs-txt\">推荐</div>\r\n                ");
	}	//end for if

	templateBuilder.Append("\r\n                <img src=\"" + Utils.ObjectToStr(dr["img_url"]) + "\" />\r\n              </a>\r\n            </div>\r\n            <div class=\"info\">\r\n              <h3><a title=\"" + Utils.ObjectToStr(dr["title"]) + "\" href=\"");
	templateBuilder.Append(linkurl("goods_show",Utils.ObjectToStr(dr["id"])));

	templateBuilder.Append("\">" + Utils.ObjectToStr(dr["title"]) + "</a></h3>\r\n              <div class=\"col\">\r\n                <b>￥" + Utils.ObjectToStr(dr["sell_price"]) + "</b>元\r\n              </div>\r\n              <div class=\"col\">\r\n                <i>库存：" + Utils.ObjectToStr(dr["stock_quantity"]) + "件</i>\r\n                市场价：<s>" + Utils.ObjectToStr(dr["market_price"]) + "</s>\r\n              </div>\r\n            </div>\r\n          </div>\r\n        </li>\r\n        ");
	}	//end for if

	if (goodsList.Rows.Count==0)
	{

	templateBuilder.Append("\r\n        <div class=\"nodata\">暂时无法找到您想要的商品！</div>\r\n        ");
	}	//end for if

	templateBuilder.Append("\r\n      </ul>\r\n    </div>\r\n    \r\n    <!--页码列表-->\r\n    <div class=\"page-box\">\r\n      <div class=\"digg\">");
	templateBuilder.Append(Utils.ObjectToStr(pagelist));
	templateBuilder.Append("</div>\r\n    </div>\r\n    <!--/页码列表-->\r\n    \r\n  </div>\r\n</div>\r\n\r\n<!--Footer-->\r\n");

	templateBuilder.Append("<div class=\"foot m-bg text-center col-md-12\">Copyright &copy; 2007-2016 kinfar.net studio,All Rights Reserved.</p></div>\r\n");


	templateBuilder.Append("\r\n<!--/Footer-->\r\n</body>\r\n</html>");
	Response.Write(templateBuilder.ToString());
}
</script>
