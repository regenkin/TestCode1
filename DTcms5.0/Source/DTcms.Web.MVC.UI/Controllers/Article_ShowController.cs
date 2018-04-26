using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.UI.Controllers {
   public class Article_ShowController : DTcms.Web.MVC.UI.Controllers.BaseController {
      protected string channel = string.Empty; //频道名称
      public int id;
      public string page = string.Empty;
      public DTcms.Model.article model = new DTcms.Model.article();

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         id = DTRequest.GetQueryInt("id");
         page = DTRequest.GetQueryString("page");
         DTcms.BLL.article bll = new DTcms.BLL.article();

         if (id > 0) {
            //如果ID获取到，将使用ID
            if (!bll.ArticleExists(channel, id)) {
               filterContext.Result = Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，您要浏览的页面不存在或已删除！")));
               return;
            }
            model = bll.ArticleModel(channel, id);
         }
         else if (!string.IsNullOrEmpty(page)) {
            //否则检查设置的别名
            if (!bll.ArticleExists(channel, page)) {
               filterContext.Result = Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，您要浏览的页面不存在或已删除！")));
               return;
            }
            model = bll.ArticleModel(channel, page);
         }
         else {
            return;
         }
         //跳转URL
         if (model.link_url != null) {
            model.link_url = model.link_url.Trim();
         }
         if (!string.IsNullOrEmpty(model.link_url)) {
            filterContext.Result = Redirect(model.link_url);
         }
         ViewData["model"] = model;
         ViewBag.This = this;
      }


      /// <summary>
      /// 获取上一条下一条的链接
      /// </summary>
      /// <param name="urlkey">urlkey</param>
      /// <param name="type">-1代表上一条，1代表下一条</param>
      /// <param name="defaultvalue">默认文本</param>
      /// <param name="callIndex">是否使用别名，0使用ID，1使用别名</param>
      /// <returns>A链接</returns>
      public string get_prevandnext_article(string urlkey, int type, string defaultvalue, int callIndex) {
         string symbol = (type == -1 ? "<" : ">");
         DTcms.BLL.article bll = new DTcms.BLL.article();
         string str = string.Empty;
         str = " and category_id=" + model.category_id;

         string orderby = type == -1 ? "id desc" : "id asc";
         DataSet ds = bll.ArticleList(channel, 1, "channel_id=" + model.channel_id + " " + str + " and status=0 and Id" + symbol + id, orderby);
         if (ds == null || ds.Tables[0].Rows.Count <= 0) {
            return defaultvalue;
         }
         if (callIndex == 1 && !string.IsNullOrEmpty(ds.Tables[0].Rows[0]["call_index"].ToString())) {
            return "<a href=\"" + linkurl(urlkey, ds.Tables[0].Rows[0]["call_index"].ToString()) + "\">" + ds.Tables[0].Rows[0]["title"] + "</a>";
         }
         return "<a href=\"" + linkurl(urlkey, ds.Tables[0].Rows[0]["id"].ToString()) + "\">" + ds.Tables[0].Rows[0]["title"] + "</a>";
      }
   }
}
