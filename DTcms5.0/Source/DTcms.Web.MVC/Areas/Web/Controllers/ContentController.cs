using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class ContentController : DTcms.Web.MVC.UI.Controllers.Article_ShowController {
      protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
         base.Initialize(requestContext);
         this.channel = "content";
      }
      //
      // GET: /Web/Content_Show/

      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
