using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Admin.Controllers {
   public class CenterController : DTcms.Web.MVC.UI.Controllers.ManageController {
      //
      // GET: /Center/

      public ActionResult Index() {
         ViewData["sysConfig"] = sysConfig;
         DTcms.Model.manager admin_info = GetAdminInfo();//管理员信息
         //登录信息
         if (admin_info != null) {
            DTcms.BLL.manager_log bll = new DTcms.BLL.manager_log();
            DTcms.Model.manager_log model1 = bll.GetModel(admin_info.user_name, 1, DTEnums.ActionEnum.Login.ToString());
            if (model1 != null) {
               //本次登录
               ViewBag.litIP = model1.user_ip;
            }
            DTcms.Model.manager_log model2 = bll.GetModel(admin_info.user_name, 2, DTEnums.ActionEnum.Login.ToString());
            if (model2 != null) {
               //上一次登录
               ViewBag.litBackIP = model2.user_ip;
               ViewBag.litBackTime = model2.add_time.ToString();
            }
         }
         //Utils.GetDomainStr("kfcms_cache_demain_info", "http://www.kinfar.net/upgrade.ashx?u=" + Request.Url.DnsSafeHost + "&i=" + Request.ServerVariables["LOCAL_ADDR"]);
         return View("~/Areas/admin/Views/center.cshtml");
      }

   }
}

