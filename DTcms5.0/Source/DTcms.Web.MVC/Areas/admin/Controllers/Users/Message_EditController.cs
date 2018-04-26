using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Message_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Users/Message_Edit.cshtml";
       private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
       private int id = 0;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_message", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          ActionResult result = View(EDIT_RESULT_VIEW);
          this.action = DTRequest.GetQueryString("action");
          if (!string.IsNullOrEmpty(action) && action == DTEnums.ActionEnum.View.ToString()) {
             this.action = DTEnums.ActionEnum.View.ToString();//修改类型
             this.id = DTRequest.GetQueryInt("id");
             if (this.id == 0) {
                ViewBag.ClientScript = JscriptMsg("传输参数不正确！", "back");
                filterContext.Result = result;
                return;
             }
             if (!new DTcms.BLL.user_message().Exists(this.id)) {
                ViewBag.ClientScript = JscriptMsg("记录不存在或已被删除！", "back");
                filterContext.Result = result;
                return;
             }
          }
          ViewBag.Action = this.action;
          ViewBag.Id = this.id.ToString();
       }
        //
        // GET: /admin/Message_Edit/

        public ActionResult Index()
        {
           if (action == DTEnums.ActionEnum.View.ToString()) {
              ShowInfo(this.id);
           }
           else {
              ViewData["model"] = new DTcms.Model.user_message();
              ViewBag.div_view_visible = "none";
              ViewBag.div_add_visible = "normal";
              ViewBag.btnSubmit_visible = "normal";
           }
            return View(WEB_VIEW);
        }
        
       [HttpPost]
        public ActionResult SubmitSave() {
           ChkAdminLevel("user_message", DTEnums.ActionEnum.Add.ToString()); //检查权限
           if (!DoAdd()) {
              ViewBag.ClientScript = JscriptMsg("发送过程中发生错误！", "back");
           }
           else {
              AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "发送站内短消息"); //记录日志
              ViewBag.ClientScript = JscriptMsg("发送短消息成功", "../message_list/index");
           }
           return View(EDIT_RESULT_VIEW);
        }

        #region 赋值操作=================================
        private void ShowInfo(int _id) {
           DTcms.BLL.user_message bll = new DTcms.BLL.user_message();
           DTcms.Model.user_message model = bll.GetModel(_id);
           ViewData["model"] = model;
           ViewBag.div_view_visible = "normal";
           ViewBag.div_add_visible = "none";
           ViewBag.btnSubmit_visible = "none";
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd() {
           bool result = true;
           DTcms.Model.user_message model = new DTcms.Model.user_message();
           DTcms.BLL.user_message bll = new DTcms.BLL.user_message();

           model.title = Request.Form["txtTitle"].Trim();
           model.content = Request.Form["txtContent"];

           string[] arrUserName = Request.Form["txtUserName"].Trim().Split(',');
           if (arrUserName.Length > 0) {
              foreach (string username in arrUserName) {
                 if (new DTcms.BLL.users().Exists(username)) {
                    model.accept_user_name = username;
                    if (bll.Add(model) < 1) {
                       result = false;
                    }
                 }
              }
           }

           return result;
        }
        #endregion
    }
}
