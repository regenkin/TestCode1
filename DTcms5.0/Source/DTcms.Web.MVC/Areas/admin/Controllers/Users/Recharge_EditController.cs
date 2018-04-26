using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class Recharge_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Users/Recharge_Edit.cshtml";
      private string username = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("user_recharge_log", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.username = DTRequest.GetQueryString("username"); //获得用户名
         ViewBag.UserName = this.username;
         string rechargeNo = Utils.GetOrderNumber();//随机生成订单号
         ViewBag.RechargeNo = rechargeNo;
      }
      //
      // GET: /admin/Recharge_Edit/

      public ActionResult Index() {
         TreeBind("type=1"); //绑定支付方式
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ChkAdminLevel("user_recharge_log", DTEnums.ActionEnum.Add.ToString()); //检查权限
         if (!DoAdd()) {
            ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", "");
         }
         else {
            ViewBag.ClientScript = JscriptMsg("会员充值成功！", "../recharge_list/index");
         }
         return View(EDIT_RESULT_VIEW);
      }

      #region 绑定支付方式=============================
      private void TreeBind(string strWhere) {
         DTcms.BLL.payment bll = new DTcms.BLL.payment();
         DataTable dt = bll.GetList(0, strWhere, "sort_id asc,id asc").Tables[0];
         List<SelectListItem> list = new List<SelectListItem>();
         list.Add(new SelectListItem() { Value = "", Text = "请选择支付方式" });
         foreach (DataRow dr in dt.Rows) {
            list.Add(new SelectListItem() { Value = dr["id"].ToString(), Text = dr["title"].ToString() });
         }
         ViewData["list"] = list;
      }
      #endregion

      #region 增加操作=================================
      private bool DoAdd() {
         DTcms.Model.users userModel = new DTcms.BLL.users().GetModel(Request.Form["txtUserName"].Trim());
         if (userModel == null) {
            return false;
         }

         bool result = false;
         DTcms.Model.user_recharge model = new DTcms.Model.user_recharge();
         DTcms.BLL.user_recharge bll = new DTcms.BLL.user_recharge();

         model.user_id = userModel.id;
         model.user_name = userModel.user_name;
         model.recharge_no = "R" + Request.Form["txtRechargeNo"].Trim(); //订单号R开头为充值订单
         model.payment_id = Utils.StrToInt(Request.Form["ddlPaymentId"], 0);
         model.amount = Utils.StrToDecimal(Request.Form["txtAmount"].Trim(), 0);
         model.status = 1;
         model.add_time = DateTime.Now;
         model.complete_time = DateTime.Now;

         if (bll.Recharge(model)) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "给会员：" + model.user_name + "，充值:" + model.amount + "元"); //记录日志
            result = true;
         }
         return result;
      }
      #endregion
   }
}
