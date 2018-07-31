using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class Content_ShowController : DTcms.Web.MVC.UI.Controllers.Article_ShowController {
      //
      // GET: /Web/Content_Show/

      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
