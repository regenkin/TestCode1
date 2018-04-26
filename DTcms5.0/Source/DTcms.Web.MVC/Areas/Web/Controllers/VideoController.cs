using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class VideoController : DTcms.Web.MVC.UI.Controllers.ArticleController {
      protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
         base.Initialize(requestContext);
         this.channel = "video";
      }

      //
      // GET: /Web/Video/
      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
