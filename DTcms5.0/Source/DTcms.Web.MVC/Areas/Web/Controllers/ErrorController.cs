using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class ErrorController : DTcms.Web.MVC.UI.Controllers.BaseController {
      public string msg = string.Empty;

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         msg = Utils.ToHtml(DTRequest.GetQueryString("msg"));
         ViewBag.Msg = this.msg;
      }
      //
      // GET: /Web/Article/

      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
