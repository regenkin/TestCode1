using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Dialog {
   public class Dialog_Article_SpecController : Controller {
      private const string WEB_VIEW = "~/Areas/admin/Views/Dialog/dialog_article_spec.cshtml";

      protected int channel_id; //频道ID

      //
      // GET: /admin/Dialog_Article_Spec/
      public ActionResult Index() {
         this.channel_id = DTRequest.GetQueryInt("channel_id"); //获取频道ID
         RptBind("parent_id=0 and channel_id=" + this.channel_id.ToString(), "sort_id asc,id desc");
         return View(WEB_VIEW);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         BLL.article_spec bll = new BLL.article_spec();
         DataSet ds = bll.GetList(0, _strWhere, _orderby);
         ViewData["list"] = ds.Tables.Count > 0 ? ds.Tables[0] : new DataTable();
      }
      #endregion
   }
}
