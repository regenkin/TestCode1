using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class RepasswordController : DTcms.Web.MVC.UI.Controllers.BaseController {
      public string action;
      public string username = string.Empty;
      public string code = string.Empty;

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         action = DTRequest.GetQueryString("action");
         if (action == "mobile") {
            username = DTRequest.GetQueryString("username");
         }
         else if (action == "email") {
            code = DTRequest.GetQueryString("code");
            DTcms.Model.user_code model = new DTcms.BLL.user_code().GetModel(code);
            if (model == null) {
               Response.Redirect(linkurl("repassword", "?action=error"));
               return;
            }
            username = model.user_name;
         }
         ViewBag.UserName = this.username;
         ViewBag.Action = this.action;
         ViewBag.Code = this.code;
      }
      //
      // GET: /Web/Article/

      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
