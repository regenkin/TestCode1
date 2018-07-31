using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;
using DTcms.Web.MVC.UI;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class UserCenterController : DTcms.Web.MVC.UI.Controllers.UserController {
      public string action = string.Empty;
      public string curr_login_ip = string.Empty;
      public string pre_login_ip = string.Empty;
      public string pre_login_time = string.Empty;
      public int total_order;
      public int total_msg;

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         action = DTRequest.GetQueryString("action");

         //获得最后登录日志
         DataTable dt = new DTcms.BLL.user_login_log().GetList(2, "user_name='" + userModel.user_name + "'", "id desc").Tables[0];
         if (dt.Rows.Count == 2) {
            curr_login_ip = dt.Rows[0]["login_ip"].ToString();
            pre_login_ip = dt.Rows[1]["login_ip"].ToString();
            pre_login_time = dt.Rows[1]["login_time"].ToString();
         }
         else if (dt.Rows.Count == 1) {
            curr_login_ip = dt.Rows[0]["login_ip"].ToString();
         }
         //未完成订单
         total_order = new DTcms.BLL.orders().GetCount("user_name='" + userModel.user_name + "' and status<3");
         //未读短信息
         total_msg = new DTcms.BLL.user_message().GetCount("accept_user_name='" + userModel.user_name + "' and is_read=0");

         //退出登录==========================================================
         if (action == "exit") {
            //清险Session
            System.Web.HttpContext.Current.Session[DTKeys.SESSION_USER_INFO] = null;
            //清除Cookies
            Utils.WriteCookie(DTKeys.COOKIE_USER_NAME_REMEMBER, "DTcms", -43200);
            Utils.WriteCookie(DTKeys.COOKIE_USER_PWD_REMEMBER, "DTcms", -43200);
            Utils.WriteCookie("UserName", "DTcms", -1);
            Utils.WriteCookie("Password", "DTcms", -1);
            //自动登录,跳转URL
            filterContext.Result = Redirect(linkurl("login"));
         }
         ViewBag.Action = this.action;
         ViewBag.This = this;
         ViewData["model"] = userModel;
         ViewData["groupModel"] = groupModel;
      }
      //
      // GET: /Web/UserCenter/

      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
