using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class UserMessageController : DTcms.Web.MVC.UI.Controllers.UserController {
      public string action = string.Empty;
      public int page;         //当前页码
      public int totalcount;   //OUT数据总数

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         action = DTRequest.GetQueryString("action");
         page = DTRequest.GetQueryInt("page", 1);
         ViewBag.Action = this.action;
         ViewBag.Page = this.page.ToString();
         ViewBag.This = this;
      }
      //
      // GET: /Web/UserMessage/

      public ActionResult Index() {
         return View(ViewName);
      }

   }
}
