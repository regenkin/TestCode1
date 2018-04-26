using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Dialog
{
    public class Dialog_AcceptController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Dialog/dialog_accept.cshtml";
        //
        // GET: /admin/Dialog_Accept/

        public ActionResult Index()
        {
            return View(WEB_VIEW);
        }

    }
}
