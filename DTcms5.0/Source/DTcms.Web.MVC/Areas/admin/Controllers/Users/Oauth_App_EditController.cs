using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Oauth_App_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Users/Oauth_App_Edit.cshtml";
       private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
       private int id = 0;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_oauth", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          this.action = DTRequest.GetQueryString("action");
          if (!string.IsNullOrEmpty(this.action) && this.action == DTEnums.ActionEnum.Edit.ToString()) {
             this.id = DTRequest.GetQueryInt("id");
             if (this.id == 0) {
                JscriptMsg("传输参数不正确！", "back");
                filterContext.Result = View(EDIT_RESULT_VIEW);
                return;
             }
             if (!new DTcms.BLL.oauth_app().Exists(this.id)) {
                JscriptMsg("记录不存在或已被删除！", "back");
                filterContext.Result = View(EDIT_RESULT_VIEW);
                return;
             }
          }
          ViewBag.Action = this.action;
          ViewBag.Id = this.id.ToString();
       }
        //
        // GET: /admin/Oauth_App_Edit/

        public ActionResult Index()
        {
           if (action == DTEnums.ActionEnum.Edit.ToString()) {
              ShowInfo(this.id);
           }
           else {
              DTcms.Model.oauth_app model = new DTcms.Model.oauth_app();
              model.sort_id = 99;
              ViewData["model"] = model;
           }
            return View(WEB_VIEW);
        }

        [HttpPost]
        public ActionResult SubmitSave() {
           if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
              ChkAdminLevel("user_oauth", DTEnums.ActionEnum.Edit.ToString()); //检查权限
              if (!DoEdit(this.id)) {
                 JscriptMsg("保存过程中发生错误！", string.Empty);
              }
              else {
                 JscriptMsg("修改站点OAuth应用成功！", "../oauth_app_list/index");
              }
           }
           else //添加
            {
              ChkAdminLevel("user_oauth", DTEnums.ActionEnum.Add.ToString()); //检查权限
              if (!DoAdd()) {
                 JscriptMsg("保存过程中发生错误！", string.Empty);
              }
              else {
                 JscriptMsg("添加站点OAuth应用成功！", "../oauth_app_list/index");
              }
           }
           return View(EDIT_RESULT_VIEW);
        }

        #region 赋值操作=================================
        private void ShowInfo(int _id) {
           //赋值实体
           BLL.oauth_app bll = new BLL.oauth_app();
           Model.oauth_app model = bll.GetModel(_id);
           ViewData["model"] = model;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd() {
           bool result = false;
           Model.oauth_app model = new Model.oauth_app();
           BLL.oauth_app bll = new BLL.oauth_app();

           model.title = Request.Form["txtTitle"].Trim();
           model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
           model.is_lock = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 0 : 1;
           model.api_path = Request.Form["txtApiPath"].Trim();
           model.img_url = Request.Form["txtImgUrl"].Trim();
           model.remark = Request.Form["txtRemark"];
           if (bll.Add(model) > 0) {
              AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加OAuth信息:" + model.title); //记录日志
              result = true;
           }
           return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id) {
           bool result = false;
           Model.oauth_app model = new Model.oauth_app();
           BLL.oauth_app bll = new BLL.oauth_app();

           model.title = Request.Form["txtTitle"].Trim();
           model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
           model.is_lock = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 0 : 1;
           model.api_path = Request.Form["txtApiPath"].Trim();
           model.img_url = Request.Form["txtImgUrl"].Trim();
           model.remark = Request.Form["txtRemark"];
           if (bll.Update(model)) {
              AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改OAuth信息:" + model.title); //记录日志
              result = true;
           }

           return result;
        }
        #endregion
    }
}
