using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
   public class Builder_HtmlController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Settings/Builder_Html.cshtml";

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("sys_builder_html", DTEnums.ActionEnum.View.ToString()); //检查权限
      }
      //
      // GET: /admin/Builder_Html/

      public ActionResult Index() {
         RptBind();
         return View(WEB_VIEW);
      }

      #region 数据绑定=================================
      private void RptBind() {
         DTcms.BLL.sites bll = new DTcms.BLL.sites();
         DataTable list = bll.GetList(0, "is_lock=0", "sort_id asc,id desc").Tables[0];
         ViewData["list"] = list;
      }
      #endregion
   }
}
