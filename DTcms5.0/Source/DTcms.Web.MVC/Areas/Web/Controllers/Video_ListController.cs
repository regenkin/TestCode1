using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class Video_ListController : DTcms.Web.MVC.UI.Controllers.Article_ListController {
      protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
         base.Initialize(requestContext);
         this.channel = "video";
      }

      //
      // GET: /Web/Video_List/
      public ActionResult Index() {
         return View(ViewName);
      }

      public ActionResult get_list_page() {
         return PartialView("~/Areas/Web/Views/" + ViewBag.TemplateSkin + "/video_partial_list.cshtml");
      }
   }
}
