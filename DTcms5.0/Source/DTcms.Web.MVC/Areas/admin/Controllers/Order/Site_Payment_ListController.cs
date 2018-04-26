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

namespace DTcms.Web.Mvc.Areas.admin.Controllers.Order {
    public class Site_Payment_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
      private const string WEB_VIEW = "~/Areas/admin/Views/Order/site_payment_list.cshtml";

      protected int totalCount;
      protected int page;
      protected int pageSize;
      protected int site_id;
      protected string keywords = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("site_order_payment", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");
         this.site_id = DTRequest.GetQueryInt("site_id");
         if (this.pageSize > 0) {
            if (this.pageSize != GetPageSize(10))
               Utils.WriteCookie("payment_page_size", "DTcmsPage", this.pageSize.ToString(), 43200);
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }
         ViewBag.Keywords = this.keywords;
         ViewBag.PageSize = this.pageSize;
         ViewBag.site_id = site_id;
      }
      //
      // GET: /admin/Payment_List/

      public ActionResult Index() {
         SiteBind(); //绑定站点
         RptBind("id>0" + CombSqlTxt(this.site_id, keywords), "sort_id asc,id desc");
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ChkAdminLevel("site_order_payment", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         DTcms.BLL.site_payment bll = new DTcms.BLL.site_payment();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            int sortId = int.Parse(item["sortId"].ToString());
            bll.UpdateField(id, "sort_id=" + sortId.ToString());
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存支付方式排序"); //记录日志
         string script = JscriptMsg("保存排序成功！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
         return Content(script);
      }

      /// <summary>
      /// 批量删除
      /// </summary>
      /// <returns></returns>
      public ActionResult SubmitDelete(string json) {
          ChkAdminLevel("site_order_payment", DTEnums.ActionEnum.Delete.ToString());//检查权限
         int sucCount = 0;
         int errorCount = 0;
         BLL.site_payment bll = new BLL.site_payment();
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
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除支付方式成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "site_id={0}&keywords={1}", this.site_id.ToString(), this.keywords));
         return Content(script);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         DTcms.BLL.site_payment bll = new DTcms.BLL.site_payment();
         DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
         if (ds.Tables.Count > 0) {
            ViewData["list"] = ds.Tables[0];
         }
         string pageUrl = Utils.CombUrlTxt("index", "site_id={0}&keywords={1}&page={2}", this.site_id.ToString(), this.keywords, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
      }
      #endregion

      #region 绑定站点=================================
      private void SiteBind() {
         DTcms.BLL.sites bll = new DTcms.BLL.sites();
         DataTable dt = bll.GetList(0, "", "sort_id asc,id desc").Tables[0];
         List<SelectListItem> siteList = new List<SelectListItem>() {
            new SelectListItem(){ Text="所有站点...", Value="" }
         };
         foreach (DataRow dr in dt.Rows) {
            siteList.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
         }
         ViewData["siteListItems"] = siteList;
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
         if (int.TryParse(Utils.GetCookie("site_payment_list_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion
   }
}
