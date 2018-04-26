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

namespace DTcms.Web.MVC.Areas.admin.Controllers.Users {
   public class Site_Oauth_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Users/Site_Oauth_List.cshtml";
      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected int site_id;
      protected string keywords = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("user_oauth", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");
         if (int.TryParse(DTRequest.GetQueryString("pagenum"), out this.pageSize)) {
            if (this.pageSize > 0) {
               Utils.WriteCookie("oauth_list_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
            }
         }
         else {
            this.pageSize = GetPageSize(10);
         }
         ViewBag.Keywores = this.keywords;
         ViewBag.PageSize = this.pageSize.ToString();
         ViewBag.SiteId = this.site_id;
      }

      //
      // GET: /admin/Site_Oauth_List/
      public ActionResult Index() {
         SiteBind(); //绑定站点
         RptBind("id>0" + CombSqlTxt(this.site_id, this.keywords), "sort_id asc,id desc");
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave(string json) {
         ChkAdminLevel("user_oauth", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         DTcms.BLL.oauth_app bll = new DTcms.BLL.oauth_app();
         JObject jobject = JObject.Parse(json);
         JToken record = jobject["list"];
         foreach (JToken item in record) {
            int id;
            if (!int.TryParse(item["id"].ToString(), out id))
               id = -1;
            int sortId;
            if (!int.TryParse(item["sortId"].ToString(), out sortId))
               sortId = 99;
            bll.UpdateField(id, "sort_id=" + sortId.ToString());
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存OAuth列表排序"); //记录日志
         string script = JscriptMsg("保存排序成功！", Utils.CombUrlTxt("site_oauth_list.aspx", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords));
         return Content(script);
      }

      [HttpPost]
      public ActionResult SubmitDelete(string json) {
         ChkAdminLevel("user_oauth", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0;
         int errorCount = 0;
         DTcms.BLL.site_oauth bll = new DTcms.BLL.site_oauth();
         JObject jobject = JObject.Parse(json);
         JToken record = jobject["list"];
         int id;
         foreach (JToken item in record) {
            if (int.TryParse(item["id"].ToString(), out id)) {
               if (bll.Delete(id)) {
                  sucCount += 1;
               }
               else {
                  errorCount += 1;
               }
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除OAuth列表成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords));
         return Content(script);
      }

      #region 绑定站点=================================
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
         DTcms.BLL.site_oauth bll = new DTcms.BLL.site_oauth();
         ViewData["list"] = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount).Tables[0];
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
            strTemp.Append(" and title like  '%" + _keywords + "%'");
         }

         return strTemp.ToString();
      }
      #endregion

      #region 返回每页数量=============================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("site_oauth_list_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion
   }
}
