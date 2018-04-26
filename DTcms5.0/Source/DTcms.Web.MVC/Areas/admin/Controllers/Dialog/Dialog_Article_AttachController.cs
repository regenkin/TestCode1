using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Dialog {
   public class Dialog_Article_AttachController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Dialog/dialog_article_attach.cshtml";
      //
      // GET: /admin/Dialog_Attach/
      public ActionResult Index() {
         return View(WEB_VIEW);
      }
   }
}
