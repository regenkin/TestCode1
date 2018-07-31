using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class Goods_ListController : DTcms.Web.MVC.UI.Controllers.Article_ListController {
      private string sortname;

      protected override void Initialize(System.Web.Routing.RequestContext requestContext) {
         base.Initialize(requestContext);
         this.channel = "goods";
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         sortname = DTRequest.GetQueryString("sort");
         ViewBag.SortName = sortname;
      }
      //
      // GET: /Web/Goods_List/
      public ActionResult Index() {
         // Article_List控制器处理page, category_id请求参数, 保存为ViewBag.Page,ViewBag.CategoryId
         return View(ViewName);
      }

      public ActionResult get_list_page() {
         return PartialView("~/Areas/Web/Views/" + ViewBag.TemplateSkin + "/goods_partial_list.cshtml");
      }
   }
}
