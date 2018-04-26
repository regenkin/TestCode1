using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Admin.Controllers {
   public class LoginController : Controller {
      private const string WEB_VIEW = "~/Areas/admin/Views/Login.cshtml";

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ViewData["sysConfig"] = new DTcms.BLL.sysconfig().loadConfig();
      }

      //
      // GET: /Login/
      [HttpGet]
      [ActionName("Index")]
      public ActionResult Index() {
         ViewBag.msgtip = "请输入用户名和密码";
         return View("~/Areas/admin/Views/Login.cshtml");
      }

      [HttpPost]
      [ActionName("SubmitLogin")]
      public ActionResult Index(string txtUserName, string txtPassword) {
         ActionResult result = View(WEB_VIEW);
         string userName = Request.Form["txtUserName"];
         string userPwd = Request.Form["txtPassword"];

         if (userName.Equals("") || userPwd.Equals("")) {
            ViewBag.msgtip = "请输入用户名或密码";
            return result;
         }
         if (Session["AdminLoginSun"] == null) {
            Session["AdminLoginSun"] = 1;
         }
         else {
            Session["AdminLoginSun"] = Convert.ToInt32(Session["AdminLoginSun"]) + 1;
         }
         //判断登录错误次数
         if (Session["AdminLoginSun"] != null && Convert.ToInt32(Session["AdminLoginSun"]) > 5) {
            ViewBag.msgtip = "错误超过5次，关闭浏览器重新登录！";
            return result;
         }
         DTcms.BLL.manager bll = new DTcms.BLL.manager();
         DTcms.Model.manager model = bll.GetModel(userName, userPwd, true);
         if (model == null) {
            ViewBag.msgtip = "用户名或密码有误，请重试！";
            return result;
         }
         Session[DTKeys.SESSION_ADMIN_INFO] = model;
         Session.Timeout = 45;
         //写入登录日志
         Model.sysconfig sysConfig = new BLL.sysconfig().loadConfig();
         if (sysConfig.logstatus > 0) {
            new DTcms.BLL.manager_log().Add(model.id, model.user_name, DTEnums.ActionEnum.Login.ToString(), "用户登录");
         }
         //写入Cookies
         Utils.WriteCookie("DTRememberName", model.user_name, 14400);
         Utils.WriteCookie("AdminName", "DTcms", model.user_name);
         Utils.WriteCookie("AdminPwd", "DTcms", model.password);
         return Redirect("../index/index");
      }
   }
}
