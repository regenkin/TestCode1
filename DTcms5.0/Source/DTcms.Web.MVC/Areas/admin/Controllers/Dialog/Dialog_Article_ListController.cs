using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using DTcms.Common;
using Newtonsoft.Json;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Dialog
{
   public class Dialog_Article_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
      protected readonly DTcms.BLL.article bll_article = new DTcms.BLL.article();
      protected readonly DTcms.BLL.article_link bll_article_link = new DTcms.BLL.article_link();
      protected readonly DTcms.BLL.site_channel bll_channel = new DTcms.BLL.site_channel();

      private const string WEB_VIEW = "~/Areas/admin/Views/Dialog/dialog_article_list.cshtml";
      private const string PARTIAL_WEB_VIEW = "~/Areas/admin/Views/Dialog/dialog_article_partial_list.cshtml";
      protected string channelName;
      protected int articleId = -1;

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
      }
      //
      // GET: /admin/Dialog_ArticleList/

      public ActionResult Index() {
         //获取内容
         channelName = DTRequest.GetQueryString("channel_name");
         articleId = DTRequest.GetQueryInt("id", -1);
         DTcms.Model.article model = bll_article.ArticleModel(channelName,articleId);
         ViewData["model"] = model;
         //获取类别
         DTcms.Model.site_channel channel = bll_channel.GetModel(model.channel_id);
         ViewData["channelModel"] = channel;
         return View(WEB_VIEW);
      }

      #region 客户端获取JSON数据
      /// <summary>
      /// 获取所属指定站点的通道数据
      /// </summary>
      /// <param name="site_id">站点Id</param>
      /// <returns></returns>
      public string GetJsonChannelList(int site_id) {
         string result = "[]";
         DTcms.BLL.site_channel bll_channel = new DTcms.BLL.site_channel();
         DataSet ds = bll_channel.GetList(0, "site_id=" + site_id.ToString(), "sort_id");
         if(ds.Tables.Count > 0){
            result = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0]);
         }
         return result;
      }

      /// <summary>
      /// 获取所属指定通道的类别数据
      /// </summary>
      /// <param name="channel_id"></param>
      /// <returns></returns>
      public string GetJsonCategoryList(int channel_id) {
         string result = "[]";
         DTcms.BLL.article_category bll_category = new DTcms.BLL.article_category();
         DataTable dt = bll_category.GetChildList(99, 0, channel_id);
         if (dt.Rows.Count > 0) {
            result = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
         }
         return result;
      }

      public ActionResult GetCategoryOptionHtml() {
         StringBuilder sb = new StringBuilder();
         int _channel_id = DTRequest.GetQueryInt("channel_id", -1);
         DTcms.BLL.article_category bll = new DTcms.BLL.article_category();
         DataTable dt = bll.GetList(0, _channel_id);
         sb.Append("<option value='' selected='selected'>所有类别</option>");
         foreach (DataRow dr in dt.Rows) {
            string Id = dr["id"].ToString();
            int ClassLayer = int.Parse(dr["class_layer"].ToString());
            string Title = dr["title"].ToString().Trim();

            if (ClassLayer == 1) {
               sb.Append("<option value='" + Id + "'>" + Title + "</option>");
            }
            else {
               Title = "├ " + Title;
               Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
               sb.Append("<option value='" + Id + "'>" + Title + "</option>");
            }
         }
         return Content(sb.ToString());
      }

      /// <summary>
      /// 获取内容分页视图
      /// </summary>
      /// <returns></returns>
      public ActionResult GetPartialList() {
         int _site_id = DTRequest.GetQueryInt("site_id", -1);
         int _channel_id = DTRequest.GetQueryInt("channel_id", -1);
         int _category_id = DTRequest.GetQueryInt("category_id", -1);
         int _pagesize = DTRequest.GetQueryInt("pagesize", 10);
         int _page = DTRequest.GetQueryInt("page", 0);
         string _keywords = HttpUtility.HtmlDecode(DTRequest.GetQueryString("keywords"));
         string _property = DTRequest.GetQueryString("property");
         int _totalcount;
         
         string strWhere = "id>0" + CombSqlTxt(_keywords, _property);
         DataSet ds = bll_article.GetList(_channel_id, _category_id, _pagesize, _page, strWhere , "sort_id,add_time", out _totalcount);
         DataTable dt = null;
         if (ds.Tables.Count > 0)
            dt = ds.Tables[0];
         ViewData["list"] = dt;
         ViewBag.TotalCount = _totalcount;
         ViewBag.SiteId = _site_id;
         ViewBag.ChannelId = _channel_id;
         ViewBag.CategoryId = _category_id;
         ViewBag.PageNum = _page;
         ViewBag.PageSize = _pagesize;
         string pageUrl = "javascript:getPageList(__id__)";
         ViewBag.PageContent = Utils.OutPageList(_pagesize, _page, _totalcount, pageUrl, 8);
         return PartialView(PARTIAL_WEB_VIEW);
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(string _keywords, string _property) {
         StringBuilder strTemp = new StringBuilder();
         _keywords = _keywords.Replace("'", "");
         if (!string.IsNullOrEmpty(_keywords)) {
            strTemp.Append(" and title like '%" + _keywords + "%'");
         }
         if (!string.IsNullOrEmpty(_property)) {
            switch (_property) {
               case "isLock":
                  strTemp.Append(" and status=1");
                  break;
               case "unIsLock":
                  strTemp.Append(" and status=0");
                  break;
               case "isMsg":
                  strTemp.Append(" and is_msg=1");
                  break;
               case "isTop":
                  strTemp.Append(" and is_top=1");
                  break;
               case "isRed":
                  strTemp.Append(" and is_red=1");
                  break;
               case "isHot":
                  strTemp.Append(" and is_hot=1");
                  break;
               case "isSlide":
                  strTemp.Append(" and is_slide=1");
                  break;
            }
         }
         return strTemp.ToString();
      }
      #endregion
   }
}
