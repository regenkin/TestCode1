using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Dialog {
   public class Dialog_Group_PriceController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Dialog/dialog_group_price.cshtml";
      //
      // GET: /admin/Dialog_Group_Price/

      public ActionResult Index() {
         RptBind("is_lock=0", "id asc");
         return View(WEB_VIEW);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         BLL.user_groups bll = new BLL.user_groups();
         DataSet ds = bll.GetList(0, _strWhere, _orderby);
         DataTable list = null;
         if (ds.Tables.Count > 0) {
            list = ds.Tables[0];
         }
         ViewData["list"] = list;
      }
      #endregion
   }
}
