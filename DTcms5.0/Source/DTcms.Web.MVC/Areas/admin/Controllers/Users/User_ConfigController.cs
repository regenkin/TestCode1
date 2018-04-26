using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class User_ConfigController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Users/User_Config.cshtml";
      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("user_config", DTEnums.ActionEnum.View.ToString()); //检查权限
      }
      //
      // GET: /admin/User_Config/

      public ActionResult Index() {
         ShowInfo();
         return View(WEB_VIEW);
      }

      [HttpPost, ValidateInput(false)]
      public ActionResult SubmitSave(string regrulestxt) {
         ChkAdminLevel("user_config", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         DTcms.BLL.userconfig bll = new DTcms.BLL.userconfig();
         DTcms.Model.userconfig model = bll.loadConfig();

         try {
            model.regstatus = Utils.StrToInt(Request.Form["regstatus"], 0);
            model.regmsgstatus = Utils.StrToInt(Request.Form["regmsgstatus"], 0);
            model.regmsgtxt = Request.Form["regmsgtxt"];
            model.regkeywords = Request.Form["regkeywords"].Trim();
            model.regctrl = Utils.StrToInt(Request.Form["regctrl"].Trim(), 0);
            model.regsmsexpired = Utils.StrToInt(Request.Form["regsmsexpired"].Trim(), 0);
            model.regemailexpired = Utils.StrToInt(Request.Form["regemailexpired"].Trim(), 0);
            model.regverify = Request.Form["regverify"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
            model.mobilelogin = Request.Form["mobilelogin"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
            model.emaillogin = Request.Form["emaillogin"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
            model.regrules = Request.Form["regrules"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
            model.regrulestxt = regrulestxt;

            model.invitecodeexpired = Utils.StrToInt(Request.Form["invitecodeexpired"].Trim(), 1);
            model.invitecodecount = Utils.StrToInt(Request.Form["invitecodecount"].Trim(), 0);
            model.invitecodenum = Utils.StrToInt(Request.Form["invitecodenum"].Trim(), 0);
            model.pointcashrate = Utils.StrToDecimal(Request.Form["pointcashrate"].Trim(), 0);
            model.pointinvitenum = Utils.StrToInt(Request.Form["pointinvitenum"].Trim(), 0);
            model.pointloginnum = Utils.StrToInt(Request.Form["pointloginnum"].Trim(), 0);
            bll.saveConifg(model);
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改用户配置信息"); //记录日志
            JscriptMsg("修改用户配置成功！", "index");
         }
         catch {
            JscriptMsg("文件写入失败，请检查是否有权限！", "back");
         }
         return View(EDIT_RESULT_VIEW);
      }

      #region 赋值操作=================================
      private void ShowInfo() {
         DTcms.BLL.userconfig bll = new DTcms.BLL.userconfig();
         DTcms.Model.userconfig model = bll.loadConfig();
         ViewData["model"] = model;
      }
      #endregion
   }
}
