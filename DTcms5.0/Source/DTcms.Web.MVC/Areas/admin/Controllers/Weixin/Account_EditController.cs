using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;
using DTcms.Model;

namespace DTcms.Web.MVC.Areas.admin.Controllers.weixin {
   public class Account_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/weixin/account_edit.cshtml";

      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("weixin_account_manage", DTEnums.ActionEnum.View.ToString()); //检查权限
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
            if (!new BLL.weixin_account().Exists(this.id)) {
               JscriptMsg("记录不存在或已删除！", "back");
               filterContext.Result = result;
               return;
            }
         }
         ViewBag.Action = this.action;
         ViewBag.Id = this.id;
      }

      //
      // GET: /admin/Account_Edit/
      public ActionResult Index() {
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ShowInfo(this.id);
         }
         else {
            DTcms.Model.weixin_account model = new weixin_account();
            model.sort_id = 99;
            ViewData["model"] = model;
         }
         return View(WEB_VIEW);
      }

      [HttpPost, ValidateInput(false)]
      public ActionResult SubmitSave() {
         ActionResult result = View(EDIT_RESULT_VIEW);
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ChkAdminLevel("weixin_account_manage", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id)) {
               JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("修改公众账户成功！", "../account_list/index");
         }
         else //添加
            {
            ChkAdminLevel("weixin_account_manage", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd()) {
               JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("添加公众账户成功！", "../account_list/index");
         }
         return result;
      }

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
         BLL.weixin_account bll = new BLL.weixin_account();
         Model.weixin_account model = bll.GetModel(_id);
         ViewData["model"] = model;
      }
      #endregion

      #region 增加操作=================================
      private bool DoAdd() {
         bool result = false;
         Model.weixin_account model = new Model.weixin_account();
         BLL.weixin_account bll = new BLL.weixin_account();

         model.name = Request.Form["txtName"].Trim();
         model.originalid = Request.Form["txtOriginalId"].Trim();
         model.wxcode = Request.Form["txtWxCode"].Trim();
         model.token = Request.Form["txtToKen"].Trim();
         model.appid = Request.Form["txtAppId"].Trim();
         model.appsecret = Request.Form["txtAppSecret"].Trim();
         model.is_push = Request.Form["cbIsPush"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         if (bll.Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "增加公众账户:" + model.name); //记录日志
            result = true;
         }
         return result;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id) {
         bool result = false;
         BLL.weixin_account bll = new BLL.weixin_account();
         Model.weixin_account model = bll.GetModel(_id);

         model.name = Request.Form["txtName"].Trim();
         model.originalid = Request.Form["txtOriginalId"].Trim();
         model.wxcode = Request.Form["txtWxCode"].Trim();
         model.token = Request.Form["txtToKen"].Trim();
         model.appid = Request.Form["txtAppId"].Trim();
         model.appsecret = Request.Form["txtAppSecret"].Trim();
         model.is_push = Request.Form["cbIsPush"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改公众账户:" + model.name); //记录日志
            result = true;
         }
         return result;
      }
      #endregion
   }
}
