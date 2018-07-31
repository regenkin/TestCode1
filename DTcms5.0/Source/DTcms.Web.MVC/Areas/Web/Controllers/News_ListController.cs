using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class News_ListController : DTcms.Web.MVC.UI.Controllers.Article_ListController {
      protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
         base.Initialize(requestContext);
         this.channel = "news";
      }
      //
      // GET: /Web/Article/
      public ActionResult Index() {
         return View(ViewName);
      }

      public ActionResult get_list_page() {
         string str = ViewName;
         return PartialView("~/Areas/Web/Views/" + ViewBag.TemplateSkin + "/news_partial_list.cshtml");
      }
   }
}
