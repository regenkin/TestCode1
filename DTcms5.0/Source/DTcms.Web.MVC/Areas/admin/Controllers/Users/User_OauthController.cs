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
   public class User_OauthController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Users/user_oauth.cshtml";

      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected string keywords = string.Empty;

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");
         this.pageSize = DTRequest.GetFormIntValue("pagesize", 0);
         if (this.pageSize > 0) {
            Utils.WriteCookie("user_oauth_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }
         ViewBag.Keywords = this.keywords;
         ViewBag.PageSize = this.pageSize;
      }
      //
      // GET: /admin/User_Oauth/

      public ActionResult Index() {
         ChkAdminLevel("user_oauth", DTEnums.ActionEnum.View.ToString()); //检查权限
         RptBind("id>0" + CombSqlTxt(keywords), "add_time desc,id desc");
         return View(WEB_VIEW);
      }

      //批量删除
      [HttpPost]
      public ActionResult SubmitDelete(string json) {
         ChkAdminLevel("user_oauth", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0;
         int errorCount = 0;
         DTcms.BLL.user_oauth bll = new DTcms.BLL.user_oauth();
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
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除OAuth授权成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
         return Content(script);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         this.page = DTRequest.GetQueryInt("page", 1);
         BLL.user_oauth bll = new BLL.user_oauth();
         DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
         DataTable list = null;
         if (ds.Tables.Count > 0) {
            list = ds.Tables[0];
         }
         else {
            list = new DataTable();
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
            strTemp.Append(" and (user_name like '%" + _keywords + "%' or oauth_name like '%" + _keywords + "%')");
         }

         return strTemp.ToString();
      }
      #endregion

      #region 返回每页数量=============================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("user_oauth_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion
   }
}
