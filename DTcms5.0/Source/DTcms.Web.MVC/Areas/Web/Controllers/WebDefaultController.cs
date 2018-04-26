using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class WebDefaultController : DTcms.Web.MVC.UI.Controllers.BaseController {
      //
      // GET: /Web/WebDefault/

      public ActionResult Index() {
         return Redirect(linkurl("index.html"));
      }
   }
}
