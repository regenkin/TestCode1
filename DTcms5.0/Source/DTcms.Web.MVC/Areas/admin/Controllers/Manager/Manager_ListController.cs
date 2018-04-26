using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Manager_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Manager/Manager_List.cshtml";
       protected int totalCount;
       protected int page;
       protected int pageSize;

       protected string keywords = string.Empty;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("manager_list", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          this.keywords = DTRequest.GetQueryString("keywords");
          if (int.TryParse(Request.Form["pageNum"], out pageSize)) {
             if (pageSize > 0) {
                Utils.WriteCookie("manager_page_size", "DTcmsPage", pageSize.ToString(), 14400);
             }
          }
          else {
             this.pageSize = GetPageSize(10); //每页数量
          }
          ViewBag.Keywords = this.keywords;
          ViewBag.PageNum = pageSize.ToString();
       }

        //
        // GET: /admin/Manager_List/
        public ActionResult Index()
        {
          DTcms.Model.manager model = GetAdminInfo(); //取得当前管理员信息
          RptBind("role_type>=" + model.role_type + CombSqlTxt(keywords), "add_time asc,id desc");
            return View(WEB_VIEW);
        }
        
        [HttpPost]
        public ActionResult SubmitSave(string json) {
           ChkAdminLevel("manager_list", DTEnums.ActionEnum.Delete.ToString()); //检查权限
           int sucCount = 0;
           int errorCount = 0;
           DTcms.BLL.manager bll = new DTcms.BLL.manager();
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
           AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除管理员" + sucCount + "条，失败" + errorCount + "条"); //记录日志
           JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
           return View(EDIT_RESULT_VIEW);
        }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby) {
           this.page = DTRequest.GetQueryInt("page", 1);
           DTcms.BLL.manager bll = new DTcms.BLL.manager();
           ViewData["list"] = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount).Tables[0];
           string pageUrl = Utils.CombUrlTxt("index", "keywords={0}&page={1}", this.keywords, "__id__");
           ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords) {
           StringBuilder strTemp = new StringBuilder();
           _keywords = _keywords.Replace("'", "");
           if (!string.IsNullOrEmpty(_keywords)) {
              strTemp.Append(" and (user_name like  '%" + _keywords + "%' or real_name like '%" + _keywords + "%' or email like '%" + _keywords + "%')");
           }
           return strTemp.ToString();
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size) {
           int _pagesize;
           if (int.TryParse(Utils.GetCookie("manager_page_size", "DTcmsPage"), out _pagesize)) {
              if (_pagesize > 0) {
                 return _pagesize;
              }
           }
           return _default_size;
        }
        #endregion
    }
}
