using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Admin.Controllers
{
   public class IndexController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
      protected Model.manager admin_info;//管理员信息
        //
        // GET: /Index/
        public ActionResult Index()
        {
           admin_info = GetAdminInfo();
           ViewData["admin_info"] = admin_info;
           return View("~/Areas/admin/Views/Index.cshtml");
        }

        public ActionResult LogOff() {
           Session[DTKeys.SESSION_ADMIN_INFO] = null;
           Utils.WriteCookie("AdminName", "DTcms", -14400);
           Utils.WriteCookie("AdminPwd", "DTcms", -14400);
           return RedirectToAction("Index", "Login");
        }
    }
}
