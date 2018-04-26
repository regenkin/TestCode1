using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.UI.Controllers {
   public class ArticleController : DTcms.Web.MVC.UI.Controllers.BaseController {
      protected string channel = string.Empty; //频道名称

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ViewBag.This = this;
      }
   }
}
