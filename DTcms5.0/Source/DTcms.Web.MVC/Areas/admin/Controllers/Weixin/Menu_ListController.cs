using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.API.Weixin.Common;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.weixin {
   public class Menu_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/weixin/menu_list.cshtml";

      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected string keywords = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("weixin_custom_menu", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");

         this.pageSize = DTRequest.GetQueryIntValue("pagesize", 0); //每页数量
         if (this.pageSize > 0) {
            Utils.WriteCookie("weixin_menu_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
         }
         else {
            this.pageSize = GetPageSize(10);
         }
         ViewBag.Keywords = this.keywords;
         ViewBag.PageSize = this.pageSize;
      }
      //
      // GET: /admin/Menu_List/

      public ActionResult Index() {
         RptBind("id>0" + CombSqlTxt(this.keywords), "sort_id asc,id desc");
         return View(WEB_VIEW);
      }

      public ActionResult SubmitDelete() {
         ChkAdminLevel("weixin_custom_menu", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         CRMComm cpp = new CRMComm();
         BLL.weixin_account bll = new BLL.weixin_account();
         int sucCount = 0; //成功数量
         int errorCount = 0; //失败数量
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            Model.weixin_account model = new BLL.weixin_account().GetModel(id);
            if (model != null) {
               string error = string.Empty; //定义错误
               string token = cpp.GetAccessToken(id, out error); //获取最新的AccessToken
               if (string.IsNullOrEmpty(error)) {
                  var result = Senparc.Weixin.MP.CommonAPIs.CommonApi.DeleteMenu(token);
                  if ((int)result.errcode == 0) {
                     sucCount += 1;
                  }
                  else {
                     errorCount += 1;
                  }
               }
               else {
                  errorCount += 1;
               }
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "清空公众平台自定义菜单成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
         return Content(script);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         this.page = DTRequest.GetQueryInt("page", 1);
         BLL.weixin_account bll = new BLL.weixin_account();
         DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
         DataTable list = null;
         if (ds.Tables.Count > 0) {
            list = ds.Tables[0];
         }
         else {
            list = new DataTable();
         }
         ViewData["list"] = list;

         string pageUrl = Utils.CombUrlTxt("../account_list/index", "keywords={0}&page={1}", this.keywords, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
         ViewBag.Page = this.page;
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(string _keywords) {
         StringBuilder strTemp = new StringBuilder();
         _keywords = _keywords.Replace("'", "");
         if (!string.IsNullOrEmpty(_keywords)) {
            strTemp.Append(" and (name like  '%" + _keywords + "%' or wxcode like '%" + _keywords + "%')");
         }
         return strTemp.ToString();
      }
      #endregion

      #region 返回每页数量=============================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("weixin_menu_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion

   }
}
