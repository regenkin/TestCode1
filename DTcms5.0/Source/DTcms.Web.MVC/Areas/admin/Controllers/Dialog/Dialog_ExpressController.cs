using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Dialog {
   public class Dialog_ExpressController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Dialog/dialog_express.cshtml";
      private string order_no = string.Empty;

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         order_no = DTRequest.GetQueryString("order_no");
         if (order_no == "") {
            JscriptMsg("传输参数不正确！", "back");
            return;
         }
         if (!new DTcms.BLL.orders().Exists(order_no)) {
            JscriptMsg("订单不存在或已被删除！", "back");
            return;
         }
         ViewBag.OrderNo = this.order_no;
      }
      //
      // GET: /admin/Dialog_Express/

      public ActionResult Index() {
         ShowInfo(order_no);
         return View(WEB_VIEW);
      }

      #region 赋值操作=================================
      private void ShowInfo(string _order_no) {
         DTcms.BLL.orders bll = new DTcms.BLL.orders();
         DTcms.Model.orders model = bll.GetModel(_order_no);
         ViewData["model"] = model;
         DTcms.BLL.express bll2 = new DTcms.BLL.express();
         DataTable dt = bll2.GetList(10,"","").Tables[0];
         List<SelectListItem> expressListItems = new List<SelectListItem>() {
            new SelectListItem(){ Text="请选择配送方式", Value="" }
         };
         foreach (DataRow dr in dt.Rows) {
            expressListItems.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
         }
         ViewData["expressListItems"] = expressListItems;
      }
      #endregion
   }
}
