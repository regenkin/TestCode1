using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Order {
   public class Express_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Order/express_edit.cshtml";
      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;
      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("order_express", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         this.action = DTRequest.GetQueryString("action");
         if (!string.IsNullOrEmpty(this.action) && this.action == DTEnums.ActionEnum.Edit.ToString()) {
            this.id = DTRequest.GetQueryInt("id");
            if (this.id == 0) {
               ViewBag.ClientScript = JscriptMsg("传输参数不正确！", "back");
               filterContext.Result = result;
               return;
            }
            if (!new DTcms.BLL.express().Exists(this.id)) {
               ViewBag.ClientScript = JscriptMsg("记录不存在或已被删除！", "back");
               filterContext.Result = result;
               return;
            }
         }
         ViewBag.Action = this.action;
         ViewBag.Id = this.id.ToString();
      }
      //
      // GET: /admin/Express_Edit/

      public ActionResult Index() {
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ShowInfo(this.id);
         }
         else {
            DTcms.Model.express model = new DTcms.Model.express();
            ViewData["model"] = model;
         }
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ActionResult result = View(EDIT_RESULT_VIEW);
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("order_express", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id)) {
               ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("修改物流配送成功！", "../express_list/index");
         }
         else //添加
            {
            ChkAdminLevel("order_express", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd()) {
               ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("添加物流配送成功！", "../express_list/index");
         }
         return result;
      }

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
         DTcms.BLL.express bll = new DTcms.BLL.express();
         DTcms.Model.express model = bll.GetModel(_id);
         ViewData["model"] = model;
      }
      #endregion

      #region 增加操作=================================
      private bool DoAdd() {
         DTcms.Model.express model = new DTcms.Model.express();
         DTcms.BLL.express bll = new DTcms.BLL.express();

         model.title = Request.Form["txtTitle"].Trim();
         model.express_code = Request.Form["txtExpressCode"].Trim();
         model.express_fee = Utils.StrToDecimal(Request.Form["txtExpressFee"].Trim(), 0);
         model.website = Request.Form["txtWebSite"].Trim();
         model.remark = Utils.ToHtml(Request.Form["txtRemark"]);
         model.is_lock = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 0 : 1;
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         if (bll.Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加配送方式:" + model.title); //记录日志
            return true;
         }
         return false;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id) {
         bool result = false;
         DTcms.BLL.express bll = new DTcms.BLL.express();
         DTcms.Model.express model = bll.GetModel(_id);

         model.title = Request.Form["txtTitle"].Trim();
         model.express_code = Request.Form["txtExpressCode"].Trim();
         model.express_fee = Utils.StrToDecimal(Request.Form["txtExpressFee"].Trim(), 0);
         model.website = Request.Form["txtWebSite"].Trim();
         model.remark = Utils.ToHtml(Request.Form["txtRemark"]);
         model.is_lock = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 0 : 1;
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改配送方式:" + model.title); //记录日志
            result = true;
         }

         return result;
      }
      #endregion
   }
}
