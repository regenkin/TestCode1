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

namespace DTcms.Web.MVC.Areas.admin.Controllers.weixin {
   public class Response_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/weixin/response_list.cshtml";

      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected string keywords = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("weixin_response_content", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");
         this.pageSize = DTRequest.GetQueryIntValue("pagesize", 0); //每页数量
         if (this.pageSize > 0) {
            Utils.WriteCookie("weixin_response_content_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
         }
         else {
            this.pageSize = GetPageSize(10);
         }
         ViewBag.Keywords = this.keywords;
         ViewBag.PageSize = this.pageSize;
         ViewBag.This = this;
      }
      //
      // GET: /admin/Response_List/

      public ActionResult Index() {
         RptBind("id>0" + CombSqlTxt(this.keywords), "add_time desc,id desc");
         return View(WEB_VIEW);
      }

      //批量删除
      [HttpPost]
      public ActionResult SubmitDelete() {
         ChkAdminLevel("weixin_response_content", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0; //成功数量
         int errorCount = 0; //失败数量
         DTcms.BLL.weixin_response_content bll = new DTcms.BLL.weixin_response_content();
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
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "删除微信请求回复" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
         return Content(script);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         this.page = DTRequest.GetQueryInt("page", 1);
         BLL.weixin_response_content bll = new BLL.weixin_response_content();
         DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
         DataTable list = null;
         if (ds.Tables.Count > 0) {
            list = ds.Tables[0];
         }
         else {
            list = new DataTable();
         }
         ViewData["list"] = list;

         string pageUrl = Utils.CombUrlTxt("index", "keywords={0}&page={1}", this.keywords, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
         ViewBag.Page = this.page;
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(string _keywords) {
         StringBuilder strTemp = new StringBuilder();
         _keywords = _keywords.Replace("'", "");
         if (!string.IsNullOrEmpty(_keywords)) {
            strTemp.Append(" and (openid like  '%" + _keywords + "%' or request_content like '%" + _keywords + "%' or reponse_content like '%" + _keywords + "%')");
         }
         return strTemp.ToString();
      }
      #endregion

      #region 返回每页数量=============================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("weixin_response_content_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion

      #region 返回类别名称=============================
      public string TypeStr(object type) {
         string str = "";
         string typeStr = (Utils.ObjectToStr(type)).ToLower();
         switch (typeStr) {
            case "text":
               str = "文本";
               break;
            case "event":
               str = "事件";
               break;
            case "txtpic":
               str = "图文";
               break;
            case "music":
               str = "语音";
               break;
            default:
               str = string.Empty;
               break;
         }
         return str;
      }
      #endregion
   }
}
