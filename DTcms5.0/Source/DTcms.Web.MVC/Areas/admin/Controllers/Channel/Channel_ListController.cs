using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class Channel_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Channel/channel_list.cshtml";
      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected int site_id;
      protected string keywords = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.site_id = DTRequest.GetQueryInt("site_id");
         this.keywords = DTRequest.GetQueryString("keywords");
         if (int.TryParse(DTRequest.GetQueryString("pagesize"), out this.pageSize)) {
            if (this.pageSize > 0) {
               Utils.WriteCookie("channel_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
            }
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }
         ViewBag.Keywords = this.keywords;
         ViewBag.SiteId = this.site_id.ToString();
         ViewBag.PageSize = this.pageSize;
         ViewBag.Page = this.page.ToString();
      }
      //
      // GET: /admin/Channel_List/

      public ActionResult Index() {
         SiteBind(); //绑定站点
         RptBind("id>0" + CombSqlTxt(site_id, keywords), "sort_id asc,id desc");
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         DTcms.BLL.site_channel bll = new DTcms.BLL.site_channel();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            int sortId = int.Parse(item["sortId"].ToString());
            bll.UpdateSort(id, sortId);
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存频道排序"); //记录日志
         string script = JscriptMsg("保存排序成功！", Utils.CombUrlTxt("index", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords), "parent.loadMenuTree");
         return Content(script);
      }

      [HttpPost]
      public ActionResult SubmitDelete() {
         ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0;
         int errorCount = 0;
         DTcms.BLL.site_channel bll = new DTcms.BLL.site_channel();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            //检查该频道下是否还有文章
            int articleCount = 0;
            try {
               articleCount = new DTcms.BLL.article().GetCount(id, "");
            }
            catch { articleCount = 0; }
            if (articleCount > 0) {
               errorCount += 1;
               continue;
            }
            DTcms.Model.site_channel model = bll.GetModel(id);
            if (bll.Delete(id)) {
               sucCount += 1;
               //删除URL配置
               new DTcms.BLL.url_rewrite().Remove("channel", model.name);
            }
            else {
               errorCount += 1;
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除频道成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
             Utils.CombUrlTxt("index", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords), "parent.loadMenuTree");
         return Content(script);
      }

      #region 绑定类别=================================
      private void SiteBind() {
         DTcms.BLL.sites bll = new DTcms.BLL.sites();
         DataTable dt = bll.GetList(0, "", "sort_id asc,id desc").Tables[0];
         List<SelectListItem> siteList = new List<SelectListItem>() {
            new SelectListItem(){ Text="所有站点", Value="" }
         };
         foreach (DataRow dr in dt.Rows) {
            siteList.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
         }
         ViewData["siteListItems"] = siteList;
      }
      #endregion

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         this.page = DTRequest.GetQueryInt("page", 1);
         DTcms.BLL.site_channel bll = new DTcms.BLL.site_channel();
         DataTable dt = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount).Tables[0];
         ViewData["list"] = dt;
         string pageUrl = Utils.CombUrlTxt("index", "site_id={0}&keywords={1}&page={2}", this.site_id.ToString(), this.keywords, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(int _site_id, string _keywords) {
         StringBuilder strTemp = new StringBuilder();
         if (_site_id > 0) {
            strTemp.Append(" and site_id=" + _site_id);
         }
         _keywords = _keywords.Replace("'", "");
         if (!string.IsNullOrEmpty(_keywords)) {
            strTemp.Append(" and (name like  '%" + _keywords + "%' or title like '%" + _keywords + "%')");
         }

         return strTemp.ToString();
      }
      #endregion

      #region 返回每页数量=============================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("channel_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion
   }
}
