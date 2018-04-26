using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.Admin.Controllers {
   public class ie6updateController : Controller {
      //
      // GET: /ie6update/

      public ActionResult Index() {
         return View("~/Areas/admin/Views/ie6update.cshtml");
      }
   }
}
