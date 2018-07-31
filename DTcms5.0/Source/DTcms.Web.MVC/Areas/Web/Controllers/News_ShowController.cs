using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class News_ShowController : DTcms.Web.MVC.UI.Controllers.Article_ShowController {
      protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
         base.Initialize(requestContext);
         this.channel = "news";
      }

      //
      // GET: /Web/News_Show/
      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
