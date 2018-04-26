using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class LoginController : DTcms.Web.MVC.UI.Controllers.BaseController {
      protected string turl = string.Empty;
      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         turl = linkurl("usercenter", "index");
         if (System.Web.HttpContext.Current.Request.Url != null && System.Web.HttpContext.Current.Request.UrlReferrer != null) {
            string currUrl = System.Web.HttpContext.Current.Request.Url.ToString().ToLower(); //当前页面
            string refUrl = System.Web.HttpContext.Current.Request.UrlReferrer.ToString().ToLower(); //上一页面
            string regPath = linkurl("register").ToLower(); //注册页面
            if (currUrl != refUrl && refUrl.IndexOf(regPath) == -1) {
               turl = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            }
         }
         Utils.WriteCookie(DTKeys.COOKIE_URL_REFERRER, turl); //记住上一页面
         ViewBag.turl = turl;
         DTcms.Model.users model = GetUserInfo();
         if (model != null) {
            //写入登录日志
            //new DTcms.BLL.user_login_log().Add(model.id, model.user_name, "自动登录");
            //自动登录,跳转URL
            //System.Web.HttpContext.Current.Response.Redirect(turl);
            filterContext.Result = Redirect(turl);
         }
      }

      //
      // GET: /Web/Login/
      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
