using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Sms_Template_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Users/Sms_Template_List.cshtml";
       protected int totalCount;
       protected int page;
       protected int pageSize;
       protected string keywords = string.Empty;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_sms_template", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          this.keywords = DTRequest.GetQueryString("keywords");
          this.page = DTRequest.GetQueryInt("page", 1);
          if (int.TryParse(DTRequest.GetQueryString("pageNum"), out this.pageSize)) {
             if (this.pageSize > 0) {
                Utils.WriteCookie("sms_template_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
             }
          }
          else {
             this.pageSize = GetPageSize(10);
          }
          ViewBag.Keywords = this.keywords;
          ViewBag.PageNum = this.pageSize;
       }
        //
        // GET: /admin/Sms_Template_List/

        public ActionResult Index()
        {
           RptBind("id>0" + CombSqlTxt(keywords), "id asc");
            return View(WEB_VIEW);
        }

        [HttpPost]
        public ActionResult SubmitDelete(string json) {
           string script = string.Empty;
           int sucCount = 0;
           int errorCount = 0;
           DTcms.BLL.sms_template bll = new DTcms.BLL.sms_template();
           //批量删除
           JObject jobject = JObject.Parse(json);
           JToken record = jobject["list"];
           int id;
           foreach (JToken item in record) {
              if (int.TryParse(item["id"].ToString(), out id)) {
                 if (bll.Delete(id)) {
                    sucCount += 1;
                 }
                 else {
                    errorCount += 1;
                 }
              }
           }
           AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除短信模板成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
           script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("sms_template_list.aspx", "keywords={0}", this.keywords));
           return Content(script);
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby) {
           DTcms.BLL.sms_template bll = new DTcms.BLL.sms_template();
           ViewData["list"] = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount).Tables[0];

           string pageUrl = Utils.CombUrlTxt("../sms_template_list/index", "keywords={0}&page={1}", this.keywords, "__id__");
           ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords) {
           StringBuilder strTemp = new StringBuilder();
           _keywords = _keywords.Replace("'", "");
           if (!string.IsNullOrEmpty(_keywords)) {
              strTemp.Append(" and (title like  '%" + _keywords + "%' or content like '%" + _keywords + "%')");
           }

           return strTemp.ToString();
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size) {
           int _pagesize;
           if (int.TryParse(Utils.GetCookie("sms_template_page_size", "DTcmsPage"), out _pagesize)) {
              if (_pagesize > 0) {
                 return _pagesize;
              }
           }
           return _default_size;
        }
        #endregion
    }
}
