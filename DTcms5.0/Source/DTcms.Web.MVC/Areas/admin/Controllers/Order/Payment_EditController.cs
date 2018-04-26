using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Order {
   public class Payment_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Order/payment_edit.cshtml";
      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;
      protected DTcms.Model.payment model = new DTcms.Model.payment();
      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("order_payment", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         string _action = DTRequest.GetQueryString("action");

         if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
            this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
            this.id = DTRequest.GetQueryInt("id");
            if (this.id == 0) {
               JscriptMsg("传输参数不正确！", "back");
               filterContext.Result = result;
               return;
            }
            if (!new BLL.payment().Exists(this.id)) {
               JscriptMsg("记录不存在或已被删除！", "back");
               filterContext.Result = result;
               return;
            }
         }
         ViewBag.Id = this.id.ToString();
         ViewBag.Action = this.action;
      }
      //
      // GET: /admin/Payment_Edit/

      public ActionResult Index() {
         ShowInfo(this.id);
         return View(WEB_VIEW);
      }

      [HttpPost]
      [ValidateInput(false)]
      public ActionResult SubmitSave(string txtRemark) {
         ActionResult result = View(EDIT_RESULT_VIEW);
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("order_payment", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id, txtRemark)) {
               JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            JscriptMsg("修改支付平台成功！", "../payment_list/index");
         }
         else //添加
            {
            ChkAdminLevel("order_payment", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd(txtRemark)) {
               JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            JscriptMsg("添加支付平台成功！", "../payment_list/index");
         }
         return result;
      }

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
         DTcms.BLL.payment bll = new DTcms.BLL.payment();
         model = bll.GetModel(_id);
         if (model == null) model=new Model.payment();
         ViewData["model"] = model;
      }
      #endregion

      #region 增加操作=================================
      private bool DoAdd(string txtRemark) {
         bool result = false;
         Model.payment model = new Model.payment();
         BLL.payment bll = new BLL.payment();

         model.title = Request.Form["txtTitle"].Trim();
         model.img_url = Request.Form["txtImgUrl"].Trim();
         model.remark = txtRemark;
         model.is_lock = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 0 : 1;
         model.sort_id = int.Parse(Request.Form["txtSortId"].Trim());
         model.poundage_type = int.Parse(Request.Form["rblPoundageType"]);
         model.poundage_amount = decimal.Parse(Request.Form["txtPoundageAmount"].Trim());
         model.api_path = Request.Form["txtApiPath"].Trim();
         model.redirect_url = Request.Form["txtRedirectUrl"].Trim();
         model.return_url = Request.Form["txtReturnUrl"].Trim();
         model.notify_url = Request.Form["txtNotifyUrl"].Trim();

         if (bll.Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加支付平台:" + model.title); //记录日志
            result = true;
         }
         return result;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id, string txtRemark) {
         bool result = false;
         DTcms.BLL.payment bll = new DTcms.BLL.payment();
         DTcms.Model.payment model = bll.GetModel(_id);

         model.title = Request.Form["txtTitle"].Trim();
         model.img_url = Request.Form["txtImgUrl"].Trim();
         model.remark = txtRemark;
         model.is_lock = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 0 : 1;
         model.sort_id = int.Parse(Request.Form["txtSortId"].Trim());
         model.poundage_type = int.Parse(Request.Form["rblPoundageType"]);
         model.poundage_amount = decimal.Parse(Request.Form["txtPoundageAmount"].Trim());
         model.api_path = Request.Form["txtApiPath"].Trim();
         model.redirect_url = Request.Form["txtRedirectUrl"].Trim();
         model.return_url = Request.Form["txtReturnUrl"].Trim();
         model.notify_url = Request.Form["txtNotifyUrl"].Trim();


         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改支付平台:" + model.title); //记录日志
            result = true;
         }

         return result;
      }
      #endregion
   }
}
