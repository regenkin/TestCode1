using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class Oauth_LoginController : DTcms.Web.MVC.UI.Controllers.BaseController {
      public string turl = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         turl = Utils.GetCookie(DTKeys.COOKIE_URL_REFERRER);
         if (string.IsNullOrEmpty(turl) || turl == Request.Url.ToString().ToLower()) {
            turl = linkurl("usercenter", "index");
         }
         if (IsUserLogin()) {
            //自动登录,跳转URL
            Response.Redirect(turl);
            return;
         }
         //检查是否已授权
         if (System.Web.HttpContext.Current.Session["oauth_name"] == null || System.Web.HttpContext.Current.Session["oauth_access_token"] == null || Session["oauth_openid"] == null) {
            Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("登录失败，用户授权已过期，请重新登录！")));
            return;
         }
         DTcms.Model.user_oauth oauthModel = new DTcms.BLL.user_oauth().GetModel(System.Web.HttpContext.Current.Session["oauth_name"].ToString(), Session["oauth_openid"].ToString());
         if (oauthModel != null) {
            //检查用户是否存在
            DTcms.Model.users model = new DTcms.BLL.users().GetModel(oauthModel.user_name);
            if (model == null) {
               Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("登录失败，授权用户不存在或已被删除！")));
               return;
            }

            //记住登录状态，防止Session提前过期
            System.Web.HttpContext.Current.Session[DTKeys.SESSION_USER_INFO] = model;
            System.Web.HttpContext.Current.Session.Timeout = 45;
            Utils.WriteCookie(DTKeys.COOKIE_USER_NAME_REMEMBER, "DTcms", model.user_name);
            Utils.WriteCookie(DTKeys.COOKIE_USER_PWD_REMEMBER, "DTcms", model.password);
            //更新最新的Access Token
            oauthModel.oauth_access_token = System.Web.HttpContext.Current.Session["oauth_access_token"].ToString();
            new DTcms.BLL.user_oauth().Update(oauthModel);
            //自动登录,跳转URL
            Response.Redirect(turl);
            return;
         }
         ViewBag.turl = turl;
      }
      //
      // GET: /Web/Article/

      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
