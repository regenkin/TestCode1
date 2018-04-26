﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class Article_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Article/article_list.cshtml";
      protected int channel_id;
      protected int totalCount;
      protected int page;
      protected int pageSize;
      protected int site_id;
      protected int category_id;
      protected string channel_name = string.Empty;
      protected string property = string.Empty;
      protected string keywords = string.Empty;
      protected string prolistview = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("channel_" + this.channel_name + "_list", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         this.site_id = DTRequest.GetQueryInt("site_id");
         this.channel_id = DTRequest.GetQueryInt("channel_id");
         if (this.site_id <= 0) {
            DTcms.Model.site_channel channelModel = new DTcms.BLL.site_channel().GetModel(this.channel_id);
            this.site_id = channelModel.site_id;
         }
         this.category_id = DTRequest.GetQueryInt("category_id");
         this.keywords = DTRequest.GetQueryString("keywords");
         this.property = DTRequest.GetQueryString("property");
         this.pageSize = DTRequest.GetQueryInt("pagesize", 0);
         this.prolistview = DTRequest.GetQueryString("prolistview");
         this.page = DTRequest.GetQueryInt("page", 1);
         if (this.prolistview != "") {
            Utils.WriteCookie("article_list_view", this.prolistview, 14400);
         }
         else {
            this.prolistview = Utils.GetCookie("article_list_view"); //显示方式
            if (this.prolistview == "")
               this.prolistview = "Txt";
         }
         if (channel_id == 0) {
            JscriptMsg("频道参数不正确！", "back");
            filterContext.Result = result;
            return;
         }
         this.channel_name = new DTcms.BLL.site_channel().GetChannelName(this.channel_id); //取得频道名称
         if (this.pageSize > 0) {
            if (this.pageSize != GetPageSize(10))
               Utils.WriteCookie("article_page_size", "DTcmsPage", this.pageSize.ToString(), 43200);
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }

         //保存参数
         ViewBag.ChannelId = this.channel_id.ToString();
         ViewBag.CategoryId = this.category_id.ToString();
         ViewBag.Keywords = this.keywords;
         ViewBag.Property = this.property;
         ViewBag.ProListView = this.prolistview;
         ViewBag.PageSize = this.pageSize.ToString();
         ViewBag.Page = this.page.ToString();
      }
      //
      // GET: /admin/Article_List/

      public ActionResult Index() {
         TreeBind(this.channel_id); //绑定类别
         WeiXinBind(); //绑定微信公众号
         RptBind(this.channel_id, this.category_id, "id>0" + CombSqlTxt(this.keywords, this.property), "sort_id asc,add_time desc,id desc");
         return View(WEB_VIEW);
      }

      //设置文章状态
      [HttpPost]
      public ActionResult SetStatus() {
         int id = int.Parse(Request.Form["id"]);
         string commandName = Request.Form["cmdName"];
         DTcms.BLL.article bll = new DTcms.BLL.article();
         DTcms.Model.article model = bll.GetModel(this.channel_id, id);
         switch (commandName) {
            case "lbtnIsMsg":
               if (model.is_msg == 1)
                  bll.UpdateField(this.channel_id, id, "is_msg=0");
               else
                  bll.UpdateField(this.channel_id, id, "is_msg=1");
               break;
            case "lbtnIsTop":
               if (model.is_top == 1)
                  bll.UpdateField(this.channel_id, id, "is_top=0");
               else
                  bll.UpdateField(this.channel_id, id, "is_top=1");
               break;
            case "lbtnIsRed":
               if (model.is_red == 1)
                  bll.UpdateField(this.channel_id, id, "is_red=0");
               else
                  bll.UpdateField(this.channel_id, id, "is_red=1");
               break;
            case "lbtnIsHot":
               if (model.is_hot == 1)
                  bll.UpdateField(this.channel_id, id, "is_hot=0");
               else
                  bll.UpdateField(this.channel_id, id, "is_hot=1");
               break;
            case "lbtnIsSlide":
               if (model.is_slide == 1)
                  bll.UpdateField(this.channel_id, id, "is_slide=0");
               else
                  bll.UpdateField(this.channel_id, id, "is_slide=1");
               break;
         }
         return Content("1");
      }

      //保存排序
      [HttpPost]
      public ActionResult SubmitSave() {
         ChkAdminLevel("channel_" + this.channel_name + "_list", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         DTcms.BLL.article bll = new DTcms.BLL.article();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            int sortId = int.Parse(item["sortId"].ToString());
            bll.UpdateField(this.channel_id, id, "sort_id=" + sortId.ToString());
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存" + this.channel_name + "频道内容排序"); //记录日志
         string script = JscriptMsg("保存排序成功！", Utils.CombUrlTxt("index", "channel_id={0}&category_id={1}&keywords={2}&property={3}",
             this.channel_id.ToString(), this.category_id.ToString(), this.keywords, this.property));
         return Content(script);
      }

      //批量删除
      [HttpPost]
      public ActionResult SubmitDelete() {
         ChkAdminLevel("channel_" + this.channel_name + "_list", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0; //成功数量
         int errorCount = 0; //失败数量
         DTcms.BLL.article bll = new DTcms.BLL.article();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            if (bll.Delete(this.channel_id, id)) {
               sucCount++;
            }
            else {
               errorCount++;
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "删除" + this.channel_name + "频道内容成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "channel_id={0}&category_id={1}&keywords={2}&property={3}",
             this.channel_id.ToString(), this.category_id.ToString(), this.keywords, this.property));
         return Content(script);
      }

      //批量审核
      [HttpPost]
      public ActionResult SubmitAudit() {
         ChkAdminLevel("channel_" + this.channel_name + "_list", DTEnums.ActionEnum.Audit.ToString()); //检查权限
         DTcms.BLL.article bll = new DTcms.BLL.article();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            bll.UpdateField(this.channel_id, id, "status=0");
         }

         AddAdminLog(DTEnums.ActionEnum.Audit.ToString(), "审核" + this.channel_name + "频道内容信息"); //记录日志
         string script = JscriptMsg("批量审核成功！", Utils.CombUrlTxt("index", "channel_id={0}&category_id={1}&keywords={2}&property={3}",
             this.channel_id.ToString(), this.category_id.ToString(), this.keywords, this.property));
         return Content(script);
      }

      #region 绑定类别=================================
      private void TreeBind(int _channel_id) {
         DTcms.BLL.article_category bll = new DTcms.BLL.article_category();
         DataTable dt = bll.GetList(0, _channel_id);
         List<SelectListItem> categoryListItems = new List<SelectListItem>() {
              new SelectListItem(){ Text="所有类别", Value=""}
           };
         List<SelectListItem> moveListItems = new List<SelectListItem>() {
            new SelectListItem(){ Text="批量移动...", Value=""}
         };
         foreach (DataRow dr in dt.Rows) {
            string Id = dr["id"].ToString();
            int ClassLayer = int.Parse(dr["class_layer"].ToString());
            string Title = dr["title"].ToString().Trim();

            if (ClassLayer == 1) {
               categoryListItems.Add(new SelectListItem() { Text = Title, Value = Id });
               moveListItems.Add(new SelectListItem() { Text = Title, Value = Id });
            }
            else {
               Title = "├ " + Title;
               Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
               categoryListItems.Add(new SelectListItem() { Text = Title, Value = Id });
               moveListItems.Add(new SelectListItem() { Text = Title, Value = Id });
            }
         }
         ViewData["categoryListItems"] = categoryListItems;
         ViewData["moveListItems"] = moveListItems;

         // 
         List<SelectListItem> propertyList = new List<SelectListItem>(){
              new SelectListItem(){ Text = "所有属性", Value = ""},
              new SelectListItem(){ Text = "待审核", Value = "isLock"},
              new SelectListItem(){ Text = "已审核", Value = "unIsLock"},
              new SelectListItem(){ Text = "可评论", Value = "isMsg"},
              new SelectListItem(){ Text = "置顶", Value = "isTop"},
              new SelectListItem(){ Text = "推荐", Value = "idRed"},
              new SelectListItem(){ Text = "热门", Value = "isHot"},
              new SelectListItem(){ Text = "幻灯片", Value = "isSlide"},
           };
         ViewData["propertyList"] = propertyList;
      }
      #endregion

      #region 绑定微信公众号===========================
      private void WeiXinBind() {
         DataTable dt = new BLL.weixin_account().GetList(0, "is_push=1", "sort_id asc,id desc").Tables[0];
         List<SelectListItem> weixinListItems = new List<SelectListItem>() {
            new SelectListItem(){ Text="微信推送", Value="" }
         };
         foreach (DataRow dr in dt.Rows) {
            weixinListItems.Add(new SelectListItem() { Text = "├ " + dr["name"].ToString(), Value = dr["id"].ToString() });
         }
         ViewData["weixinListItems"] = weixinListItems;
      }
      #endregion

      #region 数据绑定=================================
      private void RptBind(int _channel_id, int _category_id, string _strWhere, string _orderby) {

         //图表或列表显示
         DTcms.BLL.article bll = new DTcms.BLL.article();
         DataTable list = bll.GetList(_channel_id, _category_id, this.pageSize, this.page, _strWhere, _orderby, out this.totalCount).Tables[0];
         ViewData["list"] = list;
         string pageUrl = Utils.CombUrlTxt("../article_list/index", "channel_id={0}&category_id={1}&keywords={2}&property={3}&page={4}",
             _channel_id.ToString(), _category_id.ToString(), this.keywords, this.property, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
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

      #region 返回图文每页数量=========================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("article_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion

      #region 替换内容图片路径=========================
      private string ReplaceImagesPath(string content) {
         if (string.IsNullOrEmpty(content)) {
            return string.Empty;
         }
         Regex reg = new Regex("IMG[^>]*?src\\s*=\\s*(?:\"(?<1>[^\"]*)\"|'(?<1>[^\']*)')", RegexOptions.IgnoreCase);
         MatchCollection m = reg.Matches(content);
         foreach (Match math in m) {
            string imgUri = math.Groups[1].Value;
            //如果是本地图片，则加上http://网址/网站安装目录/上传目录/文件名
            if (imgUri.StartsWith(sysConfig.webpath + sysConfig.filepath + "/")) {
               string newImgPath = "http://" +  Request.Url.Authority.ToLower() + imgUri;
               content = content.Replace(imgUri, newImgPath);
            }
         }
         return content;
      }
      #endregion

      #region 返回站点链接网址=========================
      private string GetLinkDomain() {
         Model.site_channel channelModel = new BLL.site_channel().GetModel(this.channel_id);
         Model.sites siteModel = new BLL.sites().GetModel(channelModel.site_id);
         if (siteModel == null || string.IsNullOrEmpty(siteModel.domain)) {
            return sysConfig.weburl;
         }
         return siteModel.domain;
      }
      #endregion
   }
}
