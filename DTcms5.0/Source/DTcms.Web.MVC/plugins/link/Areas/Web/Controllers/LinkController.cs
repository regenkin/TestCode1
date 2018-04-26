using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers
{
    public class LinkController : DTcms.Web.MVC.UI.Controllers.BaseController
    {
       protected int totalCount;
       protected int page;
       protected int pageSize;

       protected string keywords = string.Empty;

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          if (int.TryParse(DTRequest.GetQueryString("pagesize"), out this.pageSize)) {
             if (this.pageSize > 0) {
                Utils.WriteCookie("link_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
             }
          }
          else {
             this.pageSize = GetPageSize(10); //每页数量
          }
          this.page = DTRequest.GetQueryInt("page", 1);
          //绑定页码
          ViewBag.PageSize = this.pageSize.ToString();
          ViewBag.Page = this.page.ToString();
          ViewBag.This = this;
       }
       //
       // GET: /Web/Link/

       public ActionResult Index() {
          return View(ViewName);
       }


       #region 返回每页数量=============================
       private int GetPageSize(int _default_size) {
          int _pagesize;
          if (int.TryParse(Utils.GetCookie("link_page_size", "DTcmsPage"), out _pagesize)) {
             if (_pagesize > 0) {
                return _pagesize;
             }
          }
          return _default_size;
       }
       #endregion

       public DataTable get_link_list(int top, string strwhere) {
          DataTable table = new DataTable();
          string strWhere = "is_lock=0";
          if (!string.IsNullOrEmpty(strwhere)) {
             strWhere = strWhere + " and " + strwhere;
          }
          return new DTcms.Web.Plugin.Link.BLL.link().GetList(top, strWhere).Tables[0];
       }

       public DataTable get_link_list(int page_size, int page_index, string strwhere, out int totalcount) {
          DataTable table = new DataTable();
          string strWhere = "is_lock=0";
          if (!string.IsNullOrEmpty(strwhere)) {
             strWhere = strWhere + " and " + strwhere;
          }
          return new DTcms.Web.Plugin.Link.BLL.link().GetList(page_size, page_index, strWhere, "sort_id asc,add_time desc", out totalcount).Tables[0];
       }
    }
}
