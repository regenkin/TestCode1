using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.UI.Controllers {
   public class CategoryController : DTcms.Web.MVC.UI.Controllers.BaseController {
      public int category_id; //类别ID

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         category_id = DTRequest.GetQueryInt("category_id");
         ViewBag.CategoryId = this.category_id.ToString();
      }
      //
      // GET: /Web/Article/
      public ActionResult Index() {
         return View(ViewName);
      }

   }
}
