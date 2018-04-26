using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class IndexController : DTcms.Web.MVC.UI.Controllers.BaseController {
      //
      // GET: /Web/Index/
      public ActionResult Index() {
         ViewBag.Controller = this;
         return View(ViewName);
      }
   }
}
