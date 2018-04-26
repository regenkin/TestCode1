using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Model;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Dialog
{
    public class Dialog_PrintController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Dialog/dialog_print.cshtml";
       private string order_no = string.Empty;
       protected DTcms.Model.orders model = new DTcms.Model.orders();
       protected DTcms.Model.manager adminModel = new DTcms.Model.manager();
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
          ShowInfo(order_no);
       }
        //
        // GET: /admin/Dialog_Print/

        public ActionResult Index()
        {
            return View(WEB_VIEW);
        }

        #region 赋值操作=================================
        private void ShowInfo(string _order_no) {
           DTcms.BLL.orders bll = new DTcms.BLL.orders();
           model = bll.GetModel(_order_no);
           adminModel = GetAdminInfo();
           ViewData["adminModel"] = adminModel;
           ViewData["model"] = model;
        }
        #endregion
    }
}
