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

namespace DTcms.Web.MVC.Areas.admin.Controllers.Order {
   public class Payment_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Order/payment_list.cshtml";

      protected int totalCount;
      protected int page;
      protected int pageSize;
      protected string keywords = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("order_payment", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");
         if (this.pageSize > 0) {
            if (this.pageSize != GetPageSize(10))
               Utils.WriteCookie("payment_page_size", "DTcmsPage", this.pageSize.ToString(), 43200);
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }
         ViewBag.Keywords = this.keywords;
      }
      //
      // GET: /admin/Payment_List/

      public ActionResult Index() {
         RptBind("id>0" + CombSqlTxt(keywords), "sort_id asc,id desc");
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ChkAdminLevel("order_payment", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         DTcms.BLL.payment bll = new DTcms.BLL.payment();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            int sortId = int.Parse(item["sortId"].ToString());
            bll.UpdateField(id, "sort_id=" + sortId.ToString());
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存支付平台排序"); //记录日志
         string script = JscriptMsg("保存排序成功！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
         return Content(script);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         DTcms.BLL.payment bll = new DTcms.BLL.payment();
         DataSet ds = bll.GetList(0, _strWhere, _orderby);
         if (ds.Tables.Count > 0) {
            ViewData["list"] = ds.Tables[0];
         }
         string pageUrl = Utils.CombUrlTxt("../payment_list/index", "keywords={0}&page={1}", this.keywords, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(string _keywords) {
         StringBuilder strTemp = new StringBuilder();
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
         if (int.TryParse(Utils.GetCookie("payment_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion
   }
}
