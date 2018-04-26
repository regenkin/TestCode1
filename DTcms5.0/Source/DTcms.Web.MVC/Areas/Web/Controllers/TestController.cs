using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class TestController : DTcms.Web.MVC.UI.Controllers.BaseController {
      //
      // GET: /Web/Test/

      public ActionResult Index() {
         
         return View(ViewName);
      }

   }
}
