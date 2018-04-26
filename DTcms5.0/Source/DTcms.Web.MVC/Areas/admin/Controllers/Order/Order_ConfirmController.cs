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
   public class Order_ConfirmController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Order/order_confirm.cshtml";
      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected string keywords = string.Empty;
      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("order_list", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");
         if (int.TryParse(DTRequest.GetQueryString("pageNum"), out this.pageSize)) {
            if (this.pageSize > 0) {
               Utils.WriteCookie("order_confirm_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
            }
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }
         ViewBag.Keywords = this.keywords;
         ViewBag.PageNum = this.pageSize.ToString();
      }
      //
      // GET: /admin/Order_Confirm/

      public ActionResult Index() {
         RptBind("status=1" + CombSqlTxt(this.keywords), "add_time desc,id desc");
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ChkAdminLevel("order_list", DTEnums.ActionEnum.Confirm.ToString()); //检查权限
         int sucCount = 0;
         int errorCount = 0;
         DTcms.BLL.orders bll = new DTcms.BLL.orders();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            DTcms.Model.orders model = bll.GetModel(id);
            if (model != null) {
               //检查订单是否使用在线支付方式
               if (model.payment_status > 0) {
                  //在线支付方式
                  model.payment_status = 2; //标志已付款
                  model.payment_time = DateTime.Now; //支付时间
                  model.status = 2; // 订单为确认状态
                  model.confirm_time = DateTime.Now; //确认时间
               }
               else {
                  //线下支付方式
                  model.status = 2; // 订单为确认状态
                  model.confirm_time = DateTime.Now; //确认时间
               }
               if (bll.Update(model)) {
                  sucCount++;
               }
               else {
                  errorCount++;
               }
            }
            else {
               errorCount++;
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Confirm.ToString(), "确认订单成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("确认成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
         return Content(script);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         this.page = DTRequest.GetQueryInt("page", 1);
         DTcms.BLL.orders bll = new DTcms.BLL.orders();
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
            strTemp.Append(" and (order_no like '%" + _keywords + "%' or user_name like '%" + _keywords + "%' or accept_name like '%" + _keywords + "%')");
         }
         return strTemp.ToString();
      }
      #endregion

      #region 返回用户每页数量=========================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("order_confirm_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion
   }
}
