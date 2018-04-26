using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class GSearchController : DTcms.Web.MVC.UI.Controllers.Article_ListController {
      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
