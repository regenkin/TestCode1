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
   public class User_AuditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string VIEWURL = "~/Areas/admin/Views/Users/User_Audit.cshtml";
      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected string keywords = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("user_audit", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");
         this.page = DTRequest.GetQueryInt("page", 1);
         this.pageSize = DTRequest.GetFormIntValue("pagesize", 0);
         if (this.pageSize > 0) {
            Utils.WriteCookie("user_audit_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }

         ViewBag.Keywords = this.keywords;
         ViewBag.PageSize = this.pageSize.ToString();
      }

      //
      // GET: /admin/User_Audit/
      public ActionResult Index() {
         RptBind("status>0 and status<3" + CombSqlTxt(this.keywords), "reg_time desc,id desc");
         return View(VIEWURL);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         DTcms.BLL.users bll = new DTcms.BLL.users();
         DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
         DataTable list = null;
         if (ds.Tables.Count > 0) {
            list = ds.Tables[0];
         }
         else {
            list = new DataTable();
         }
         ViewData["list"] = list;
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, Utils.CombUrlTxt("index", "keywords={0}&page={1}", this.keywords, "__id__"), 8);
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(string _keywords) {
         StringBuilder strTemp = new StringBuilder();
         _keywords = _keywords.Replace("'", "");
         if (!string.IsNullOrEmpty(_keywords)) {
            strTemp.Append(" and (user_name like '%" + _keywords + "%' or mobile like '%" + _keywords + "%' or email like '%" + _keywords + "%' or nick_name like '%" + _keywords + "%')");
         }
         return strTemp.ToString();
      }
      #endregion

      #region 返回用户每页数量=========================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("user_audit_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion

      //审核通过
      [HttpPost]
      public ActionResult SumbitAudit(string json) {
         ChkAdminLevel("user_audit", DTEnums.ActionEnum.Audit.ToString()); //检查权限
         int sucCount = 0;
         int errorCount = 0;
         DTcms.BLL.users bll = new DTcms.BLL.users();
         //批量删除
         JObject jobject = JObject.Parse(json);
         JToken record = jobject["list"];
         int id;
         foreach (JToken item in record) {
            if (int.TryParse(item["id"].ToString(), out id)) {
               if (bll.UpdateField(id, "status=0") > 0) {
                  sucCount += 1;
               }
               else {
                  errorCount += 1;
               }
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Audit.ToString(), "审核会员成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         JscriptMsg("审核通过" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
         return View(EDIT_RESULT_VIEW);
      }

      //审核通过
      [HttpPost]
      public ActionResult SumbitDelete(string json) {
         ChkAdminLevel("user_list", DTEnums.ActionEnum.Audit.ToString()); //检查权限
         int sucCount = 0;
         int errorCount = 0;
         DTcms.BLL.users bll = new DTcms.BLL.users();
         //批量删除
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
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除用户" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
             Utils.CombUrlTxt("index", "keywords={0}", this.keywords)); return View(EDIT_RESULT_VIEW);
      }
   }
}
