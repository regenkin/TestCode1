using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Dialog {
   public class Dialog_MapController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Dialog/dialog_map.cshtml";
      //
      // GET: /admin/Dialog_Map/

      public ActionResult Index() {
         return View(WEB_VIEW);
      }

   }
}
