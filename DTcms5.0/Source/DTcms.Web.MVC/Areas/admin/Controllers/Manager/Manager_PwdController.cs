using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Manager_PwdController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Manager/Manager_Pwd.cshtml";
        //
        // GET: /admin/Manager_Pwd/

       public ActionResult Index() {
          DTcms.Model.manager model = GetAdminInfo();
          ShowInfo(model.id);
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitSave() {
          ActionResult result = View(EDIT_RESULT_VIEW);
          DTcms.BLL.manager bll = new DTcms.BLL.manager();
          DTcms.Model.manager model = GetAdminInfo();

          if (DESEncrypt.Encrypt(Request.Form["txtOldPassword"].Trim(), model.salt) != model.password) {
             ViewBag.ClientScript = JscriptMsg("旧密码不正确！", "back");
             return result;
          }
          if (Request.Form["txtPassword"].Trim() != Request.Form["txtPassword1"].Trim()) {
             ViewBag.ClientScript = JscriptMsg("两次密码不一致！", "back");
             return result;
          }
          model.password = DESEncrypt.Encrypt(Request.Form["txtPassword"].Trim(), model.salt);
          model.real_name = Request.Form["txtRealName"].Trim();
          model.telephone = Request.Form["txtTelephone"].Trim();
          model.email = Request.Form["txtEmail"].Trim();

          if (!bll.Update(model)) {
             ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", "back");
             return result;
          }
          Session[DTKeys.SESSION_ADMIN_INFO] = null;
          ViewBag.ClientScript = JscriptMsg("密码修改成功！", "index");
          return result;
       }

       #region 赋值操作=================================
       private void ShowInfo(int _id) {
          DTcms.BLL.manager bll = new DTcms.BLL.manager();
          DTcms.Model.manager model = bll.GetModel(_id);
          ViewData["model"] = model;
       }
       #endregion
    }
}
