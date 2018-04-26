﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Order {
   public class Express_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Order/express_list.cshtml";
      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected string keywords = string.Empty;
      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("order_express", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");
         if (int.TryParse(DTRequest.GetQueryString("pageNum"), out this.pageSize)) {
            if (this.pageSize > 0) {
               Utils.WriteCookie("express_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
            }
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }
         ViewBag.Keywords = this.keywords;
         ViewBag.PageNum = this.pageSize.ToString();
         ViewBag.Page = this.page.ToString();
      }
      //
      // GET: /admin/Express_List/

      public ActionResult Index() {
         RptBind("id>0" + CombSqlTxt(keywords), "sort_id asc,id desc");
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ChkAdminLevel("order_express", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         DTcms.BLL.express bll = new DTcms.BLL.express();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = Convert.ToInt32(item["id"].ToString());
            int sortId = Convert.ToInt32(item["sortId"].ToString());
            bll.UpdateField(id, "sort_id=" + sortId.ToString());
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存配送方式排序"); //记录日志
         string script = JscriptMsg("保存排序成功！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
         return Content(script);
      }

      [HttpPost]
      public ActionResult SubmitDelete() {
         ChkAdminLevel("order_express", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0;
         int errorCount = 0;
         DTcms.BLL.express bll = new DTcms.BLL.express();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = Convert.ToInt32(item["id"].ToString());
            if (bll.Delete(id)) {
               sucCount += 1;
            }
            else {
               errorCount += 1;
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除配送方式成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
         return Content(script);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         this.page = DTRequest.GetQueryInt("page", 1);
         DTcms.BLL.express bll = new DTcms.BLL.express();
         DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
         if (ds.Tables.Count > 0) {
            ViewData["list"] = ds.Tables[0];
         }
         string pageUrl = Utils.CombUrlTxt("index", "keywords={0}&page={1}", this.keywords, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(string _keywords) {
         StringBuilder strTemp = new StringBuilder();
         _keywords = _keywords.Replace("'", "");
         if (!string.IsNullOrEmpty(_keywords)) {
            strTemp.Append(" and (title like  '%" + _keywords + "%' or express_code like '%" + _keywords + "%')");
         }

         return strTemp.ToString();
      }
      #endregion

      #region 返回每页数量=============================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("express_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion
   }
}
