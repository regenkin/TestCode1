using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class Link_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/plugins/link/Areas/admin/Views/link_list.cshtml";
      private string keywords;
      private int page;
      private int pageSize;
      private int totalCount;
      private DTcms.Web.Plugin.Link.BLL.link bll = new DTcms.Web.Plugin.Link.BLL.link();
      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("plugin_link", DTEnums.ActionEnum.Show.ToString());
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");
         this.pageSize = DTRequest.GetQueryInt("pageNum", 0);
         this.page = DTRequest.GetQueryInt("page", 1);
         if (this.pageSize > 0) {
            if (this.pageSize != GetPageSize(10)) {
               Utils.WriteCookie("link_page_size", "DTcmsPage", this.pageSize.ToString(), 43200);
            }
         }
         else {
            this.pageSize = GetPageSize(10);
         }
      }

      //
      // GET: /admin/Link_List/

      public ActionResult Index() {
         RptBind("id>0" + this.CombSqlTxt(keywords), "sort_id asc, add_time desc");
         return View(WEB_VIEW);
      }

      //设置状态
      [HttpPost]
      public ActionResult SetStatus() {
         int id = int.Parse(Request.Form["id"]);
         string commandName = Request.Form["cmdName"];

         DTcms.Web.Plugin.Link.Model.link model = bll.GetModel(id);
         switch (commandName) {
            case "lbtnIsRed":
               if (model.is_red == 1)
                  bll.UpdateField(id, "is_red=0");
               else
                  bll.UpdateField(id, "is_red=1");
               break;
         }
         return Content("1");
      }

      //保存排序
      [HttpPost]
      public ActionResult SubmitSave() {
         ChkAdminLevel("plugin_link", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            int sortId = int.Parse(item["sortId"].ToString());
            bll.UpdateField(id, "sort_id=" + sortId.ToString());
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存友情链接成功"); //记录日志
         string script = JscriptMsg("保存排序成功！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
         return Content(script);
      }

      //批量删除
      [HttpPost]
      public ActionResult SubmitDelete() {
         ChkAdminLevel("plugin_link", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0; //成功数量
         int errorCount = 0; //失败数量
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            if (bll.Delete(id)) {
               sucCount++;
            }
            else {
               errorCount++;
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "删除友情链接成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "keywords={0}",this.keywords));
         return Content(script);
      }

      //批量审核
      [HttpPost]
      public ActionResult SubmitAudit() {
         ChkAdminLevel("plugin_link", DTEnums.ActionEnum.Audit.ToString()); //检查权限
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            bll.UpdateField(id, "is_lock=0");
         }

         AddAdminLog(DTEnums.ActionEnum.Audit.ToString(), "审核友情链接"); //记录日志
         string script = JscriptMsg("批量审核成功！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords ));
         return Content(script);
      }

      private void RptBind(string _strWhere, string _orderby) {
         ViewBag.Keywords = this.keywords;
         ViewBag.PageNum = this.pageSize.ToString();
         ViewBag.Page = this.page.ToString();
         DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
         if (ds.Tables.Count > 0) {
            ViewData["list"] = ds.Tables[0];
         }
         string pageUrl = Utils.CombUrlTxt("index", "keywords={0}&page={1}", this.keywords, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
      }

      private int GetPageSize(int _default_size) {
         int _pageSize;
         if (!int.TryParse(Utils.GetCookie("link_page_size", "DTcmsPage"), out _pageSize)) {
            if (_pageSize > 0) {
               return _pageSize;
            }
         }
         return _default_size;
      }

      private string CombSqlTxt(string _keywords) {
         StringBuilder builder = new StringBuilder();
         _keywords = _keywords.Replace("'", "");
         if (!string.IsNullOrEmpty(_keywords)) {
            builder.Append(" and title link '%" + _keywords + "%'");
         }
         return builder.ToString();
      }
   }
}
