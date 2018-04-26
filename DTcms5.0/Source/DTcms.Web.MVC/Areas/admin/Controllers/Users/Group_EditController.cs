using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Users
{
    public class Group_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private string viewurl = "~/Areas/admin/Views/Users/Group_Edit.cshtml";
       private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
       private int id = 0;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_group", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          ActionResult result = View(EDIT_RESULT_VIEW);
          //取到操作类型
          string _action = DTRequest.GetQueryString("action");
          if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
             this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
             this.id = DTRequest.GetQueryInt("id");
             if (this.id == 0) {
                JscriptMsg("传输参数不正确！", "back");
                filterContext.Result = result;
                return;
             }
             if (!new DTcms.BLL.user_groups().Exists(this.id)) {
                JscriptMsg("用户组不存在或已被删除！", "back");
                filterContext.Result = result;
                return;
             }
          }
          ViewBag.Action = this.action;
          ViewBag.Id = this.id.ToString();
       }
        //
        // GET: /admin/Group_Edit/

       public ActionResult Index() {
          if (action == DTEnums.ActionEnum.Edit.ToString()) {
             ShowInfo(this.id);
          }
          else {
             DTcms.Model.user_groups model = new DTcms.Model.user_groups();
             ViewData["model"] = model;
          }
          return View(viewurl);
       }

        #region 赋值操作=================================
        private void ShowInfo(int _id) {
           DTcms.BLL.user_groups bll = new DTcms.BLL.user_groups();
           DTcms.Model.user_groups model = bll.GetModel(_id);
           ViewData["model"] = model;
        }
        #endregion

        #region 增加操作=================================
        private int DoAdd(bool isLock, bool isDefault, bool isUpgrade) {
           int result = 0;
           DTcms.Model.user_groups model = new DTcms.Model.user_groups();
           DTcms.BLL.user_groups bll = new DTcms.BLL.user_groups();

           model.title = Request.Form["txtTitle"].Trim();
           model.is_lock = isLock ? 1 : 0;
           model.is_default = isDefault ? 1 : 0;
           model.is_upgrade = isUpgrade ? 1 : 0;
           model.grade = int.Parse(Request.Form["txtGrade"].Trim());
           model.upgrade_exp = int.Parse(Request.Form["txtUpgradeExp"].Trim());
           model.amount = decimal.Parse(Request.Form["txtAmount"].Trim());
           model.point = int.Parse(Request.Form["txtPoint"].Trim());
           model.discount = int.Parse(Request.Form["txtDiscount"].Trim());
           result = bll.Add(model);
           if ( result > 0) {
              AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加用户组:" + model.title); //记录日志
           }
           return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id, bool isLock, bool isDefault, bool isUpgrade) {
           bool result = false;
           DTcms.BLL.user_groups bll = new DTcms.BLL.user_groups();
           DTcms.Model.user_groups model = bll.GetModel(_id);

           model.title = Request.Form["txtTitle"].Trim();
           model.is_lock = isLock ? 1 : 0;
           model.is_default = isDefault ? 1 : 0;
           model.is_upgrade = isUpgrade ? 1 : 0;
           model.grade = int.Parse(Request.Form["txtGrade"].Trim());
           model.upgrade_exp = int.Parse(Request.Form["txtUpgradeExp"].Trim());
           model.amount = decimal.Parse(Request.Form["txtAmount"].Trim());
           model.point = int.Parse(Request.Form["txtPoint"].Trim());
           model.discount = int.Parse(Request.Form["txtDiscount"].Trim());
           if (bll.Update(model)) {
              AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改用户组:" + model.title); //记录日志
              result = true;
           }

           return result;
        }
        #endregion


        // 提交保存
        [HttpPost]
        public ActionResult SubmitSave(bool rblIsLock, bool rblIsDefault, bool rblIsUpgrade) {
           action = Request.Form["action"];
           this.id = int.Parse(Request.Form["id"]);
           if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
              ChkAdminLevel("user_group", DTEnums.ActionEnum.Edit.ToString()); //检查权限
              if (!DoEdit(this.id, rblIsLock, rblIsDefault, rblIsUpgrade)) {
                 JscriptMsg("保存过程中发生错误！", "");
              }
              else {
                 JscriptMsg("修改用户组成功！", "../group_list/index");
              }
           }
           else //添加
            {
              ChkAdminLevel("user_group", DTEnums.ActionEnum.Add.ToString()); //检查权限
              this.id = DoAdd(rblIsLock, rblIsDefault, rblIsUpgrade);
              if (id <= 0) {
                 JscriptMsg("保存过程中发生错误！", "");
              }
              else {
                 JscriptMsg("添加会员组成功！", "../group_list/index");
              }
           }
           return View(EDIT_RESULT_VIEW);
        }
    }
}
