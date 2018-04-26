using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Dialog_ChannelController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Dialog/dialog_channel.cshtml";
        //
        // GET: /admin/Dialog_Channel/

        public ActionResult Index()
        {
            return View(WEB_VIEW);
        }

    }
}
