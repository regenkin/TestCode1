using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class GoodsController : DTcms.Web.MVC.UI.Controllers.ArticleController {
      protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
         base.Initialize(requestContext);
         this.channel = "goods";
      }

      //
      // GET: /Web/Goods/
      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
