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
   public class Order_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Order/order_list.cshtml";
      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected int status;
      protected int payment_status;
      protected int express_status;
      protected string keywords = string.Empty;
      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("order_list", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.status = DTRequest.GetQueryInt("status");
         this.payment_status = DTRequest.GetQueryInt("payment_status");
         this.express_status = DTRequest.GetQueryInt("express_status");
         this.keywords = DTRequest.GetQueryString("keywords");
         if (int.TryParse(DTRequest.GetQueryString("pageNum"), out this.pageSize)) {
            if (this.pageSize > 0) {
               Utils.WriteCookie("order_list_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
            }
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }
         ViewBag.Status = this.status.ToString();
         ViewBag.PaymentStatus = this.payment_status.ToString();
         ViewBag.ExpressStatus = this.express_status.ToString();
         ViewBag.Keywords = this.keywords;
         ViewBag.PageNum = this.pageSize.ToString();
      }
      //
      // GET: /admin/Order_List/

      public ActionResult Index() {
         RptBind("id>0" + CombSqlTxt(this.status, this.payment_status, this.express_status, this.keywords), "status asc,add_time desc,id desc");
         return View(WEB_VIEW);
      }

      public ActionResult SubmitDelete() {
         ChkAdminLevel("order_list", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0;
         int errorCount = 0;
         DTcms.BLL.orders bll = new DTcms.BLL.orders();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = Convert.ToInt32(item["id"]);
            if (bll.Delete(id)) {
               sucCount += 1;
            }
            else {
               errorCount += 1;
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除订单成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "status={0}&payment_status={1}&express_status={2}&keywords={3}",
             this.status.ToString(), this.payment_status.ToString(), this.express_status.ToString(), this.keywords));
         return Content(script);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         this.page = DTRequest.GetQueryInt("page", 1);
         DTcms.BLL.orders bll = new DTcms.BLL.orders();
         DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
         ViewData["list"] = ds;
         string pageUrl = Utils.CombUrlTxt("index", "status={0}&payment_status={1}&express_status={2}&keywords={3}&page={4}",
             this.status.ToString(), this.payment_status.ToString(), this.express_status.ToString(), this.keywords, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);

         List<SelectListItem> statusList = new List<SelectListItem>(){
            new SelectListItem(){ Text="订单状态", Value="1" },
            new SelectListItem(){ Text="已生成", Value="2" },
            new SelectListItem(){ Text="已确认", Value="3" },
            new SelectListItem(){ Text="已完成", Value="4" },
            new SelectListItem(){ Text="已取消", Value="5" },
            new SelectListItem(){ Text="已作废", Value="6" }
         };
         ViewData["statusList"] = statusList;
         List<SelectListItem> payStatusList = new List<SelectListItem>() {
            new SelectListItem(){ Text="支付状态", Value="0" },
            new SelectListItem(){ Text="待支付", Value="1" },
            new SelectListItem(){ Text="已支付", Value="2" }
         };
         ViewData["paymentStatusList"] = payStatusList;
         List<SelectListItem> expressStatusList = new List<SelectListItem>() {
            new SelectListItem(){ Text="发货状态", Value="0" },
            new SelectListItem(){ Text="待发货", Value="1" },
            new SelectListItem(){ Text="已发货", Value="2" }
         };
         ViewData["expressStatusList"] = expressStatusList;
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(int _status, int _payment_status, int _express_status, string _keywords) {
         StringBuilder strTemp = new StringBuilder();
         if (_status > 0) {
            strTemp.Append(" and status=" + _status);
         }
         if (_payment_status > 0) {
            strTemp.Append(" and payment_status=" + _payment_status);
         }
         if (_express_status > 0) {
            strTemp.Append(" and express_status=" + _express_status);
         }
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
         if (int.TryParse(Utils.GetCookie("order_list_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion
   }
}
