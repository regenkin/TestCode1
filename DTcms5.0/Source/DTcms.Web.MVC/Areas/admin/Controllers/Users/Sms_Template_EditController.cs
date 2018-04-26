using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Sms_Template_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Users/Sms_Template_Edit.cshtml";
       private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
       private int id = 0;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_sms_template", DTEnums.ActionEnum.View.ToString()); //检查权限
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
             if (!new DTcms.BLL.sms_template().Exists(this.id)) {
                ViewBag.ClientScript = JscriptMsg("记录不存在或已被删除！", "back");
                filterContext.Result = result;
                return;
             }
          }
          ViewBag.Action = this.action;
          ViewBag.Id = this.id.ToString();
       }

        //
        // GET: /admin/Sms_Template_Edit/

       public ActionResult Index() {
          if (action == DTEnums.ActionEnum.Edit.ToString()) {
             ShowInfo(this.id);
          }
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitSave() {
          if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
             ChkAdminLevel("user_sms_template", DTEnums.ActionEnum.Edit.ToString()); //检查权限
             if (!DoEdit(this.id)) {
                ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", string.Empty);
             }
             else {
                ViewBag.ClientScript = JscriptMsg("修改短信模板成功！", "../sms_template_list/index");
             }
          }
          else //添加
            {
             ChkAdminLevel("user_sms_template", DTEnums.ActionEnum.Add.ToString()); //检查权限
             if (!DoAdd()) {
                ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", string.Empty);
             }
             else {
                ViewBag.ClientScript = JscriptMsg("添加短信模板成功！", "../sms_template_list/index");
             }
          }
          return View(EDIT_RESULT_VIEW);
       }

       #region 赋值操作=================================
       private void ShowInfo(int _id) {
          DTcms.BLL.sms_template bll = new DTcms.BLL.sms_template();
          DTcms.Model.sms_template model = bll.GetModel(_id);
          ViewData["model"] = model;
       }
       #endregion

       #region 增加操作=================================
       private bool DoAdd() {
          DTcms.Model.sms_template model = new DTcms.Model.sms_template();
          DTcms.BLL.sms_template bll = new DTcms.BLL.sms_template();

          model.title = Request.Form["txtTitle"].Trim();
          model.call_index = Request.Form["txtCallIndex"].Trim();
          model.content = Request.Form["txtContent"];

          if (bll.Add(model) > 0) {
             AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加短信模板:" + model.title); //记录日志
             return true;
          }
          return false;
       }
       #endregion

       #region 修改操作=================================
       private bool DoEdit(int _id) {
          bool result = false;
          DTcms.BLL.sms_template bll = new DTcms.BLL.sms_template();
          DTcms.Model.sms_template model = bll.GetModel(_id);

          model.title = Request.Form["txtTitle"].Trim();
          model.call_index = Request.Form["txtCallIndex"].Trim();
          model.content = Request.Form["txtContent"];

          if (bll.Update(model)) {
             AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改短信模板:" + model.title); //记录日志
             result = true;
          }

          return result;
       }
       #endregion

    }
}
