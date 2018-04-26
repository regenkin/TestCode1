using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Order {

   public class Order_ConfigController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Order/order_config.cshtml";
      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("order_config", DTEnums.ActionEnum.View.ToString()); //检查权限
      }
      //
      // GET: /admin/Order_Config/

      public ActionResult Index() {
         ShowInfo();
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ActionResult result = View(EDIT_RESULT_VIEW);
         ChkAdminLevel("order_config", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         DTcms.BLL.orderconfig bll = new DTcms.BLL.orderconfig();
         DTcms.Model.orderconfig model = bll.loadConfig();
         try {
            model.anonymous = Request.Form["anonymous"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
            model.taxtype = Utils.StrToInt(Request.Form["taxtype"], 1);
            model.taxamount = Utils.StrToDecimal(Request.Form["taxamount"].Trim(), 0);
            model.confirmmsg = Utils.StrToInt(Request.Form["confirmmsg"], 0);
            model.confirmcallindex = Request.Form["confirmcallindex"];
            model.expressmsg = Utils.StrToInt(Request.Form["expressmsg"], 0);
            model.expresscallindex = Request.Form["expresscallindex"];
            model.completemsg = Utils.StrToInt(Request.Form["completemsg"], 0);
            model.completecallindex = Request.Form["completecallindex"];

            bll.saveConifg(model);
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改订单配置信息"); //记录日志
            ViewBag.ClientScript = JscriptMsg("修改订单配置成功！", "../order_config/index");
         }
         catch {
            ViewBag.ClientScript = JscriptMsg("文件写入失败，请检查是否有权限！", string.Empty);
         }
         return result;
      }

      #region 赋值操作=================================
      private void ShowInfo() {
         DTcms.BLL.orderconfig bll = new DTcms.BLL.orderconfig();
         DTcms.Model.orderconfig model = bll.loadConfig();
         ViewData["model"] = model;
      }
      #endregion
   }
}
