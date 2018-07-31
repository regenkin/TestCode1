using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class UserAddressController : DTcms.Web.MVC.UI.Controllers.UserController {
      protected int page;         //当前页码
      protected int totalcount;   //OUT数据总数

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         page = DTRequest.GetQueryInt("page", 1);
      }
      //
      // GET: /Web/UserAddress/

      public ActionResult Index() {
         return View(ViewName);
      }

   }
}
