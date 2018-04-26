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

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class User_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Users/user_list.cshtml";
       protected int totalCount;
       protected int page;
       protected int pageSize;

       protected int site_id;
       protected int group_id;
       protected string start_time = string.Empty;
       protected string end_time = string.Empty;
       protected string keywords = string.Empty;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_list", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          this.site_id = DTRequest.GetQueryInt("site_id");
          this.group_id = DTRequest.GetQueryInt("group_id");
          this.keywords = DTRequest.GetQueryString("keywords");
          this.start_time = DTRequest.GetQueryString("start_time");
          this.end_time = DTRequest.GetQueryString("end_time");

          this.pageSize = DTRequest.GetFormIntValue("pagesize", 0);
          if (this.pageSize > 0) {
             Utils.WriteCookie("user_list_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
          }
          else {
             this.pageSize = GetPageSize(10); //每页数量
          }
          // 绑定页面数据
          ViewBag.PageSize = this.pageSize;
          ViewBag.Keywords = this.keywords;
          ViewBag.GroupId = this.group_id;
          ViewBag.StartTime = this.start_time;
          ViewBag.EndTime = this.end_time;
          ViewBag.SiteId = this.site_id;
       }

        //
        // GET: /admin/User_List/

       public ActionResult Index() {
          SiteBind(); //绑定站点
          TreeBind("is_lock=0"); //绑定类别
          RptBind("id>0" + CombSqlTxt(this.site_id, this.group_id, this.start_time, this.end_time, this.keywords), "reg_time desc,id desc");
          return View(WEB_VIEW);
       }

       #region 绑定站点=================================
       private void SiteBind() {
          BLL.sites bll = new BLL.sites();
          DataTable dt = bll.GetList(0, "is_lock=0", "sort_id asc,id desc").Tables[0];
          List<SelectListItem> siteListItems = new List<SelectListItem>(){
            new SelectListItem(){ Text="请选择站点...", Value=""}
         };
          foreach (DataRow dr in dt.Rows) {
             siteListItems.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
          }
          ViewData["siteListItems"] = siteListItems;
       }
       #endregion

       #region 绑定组别=================================
       private void TreeBind(string strWhere) {
          DTcms.BLL.user_groups bll = new DTcms.BLL.user_groups();
          DataTable dt = bll.GetList(0, strWhere, "grade asc,id asc").Tables[0];
          List<SelectListItem> groupListItems = new List<SelectListItem>();
          groupListItems.Add(new SelectListItem() { Text = "请选择组别...", Value = "" });
          foreach (DataRow dr in dt.Rows) {
             groupListItems.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
          }
          ViewData["groupListItems"] = groupListItems;
       }
       #endregion

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby) {
          this.page = DTRequest.GetQueryInt("page", 1);
          BLL.users bll = new BLL.users();
          DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
          DataTable list = null;
          if (ds.Tables.Count > 0) {
             list = ds.Tables[0];
          }
          else {
             list = new DataTable();
          }
          ViewData["list"] = list;
          string pageUrl = Utils.CombUrlTxt("../user_list/index", "site_id={0}&group_id={1}&start_time={2}&end_time={3}&keywords={4}&page={5}",
              this.site_id.ToString(), this.group_id.ToString(), this.start_time, this.end_time, this.keywords, "__id__");
          ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(int _site_id, int _group_id, string _start_time, string _end_time, string _keywords) {
           StringBuilder strTemp = new StringBuilder();
           if (_site_id > 0) {
              strTemp.Append(" and site_id=" + _site_id);
           }
           if (_group_id > 0) {
              strTemp.Append(" and group_id=" + _group_id);
           }
           _start_time = _start_time.Replace("'", "");
           if (!string.IsNullOrEmpty(_start_time)) {
              strTemp.Append(" and datediff(d,reg_time,'" + _start_time + "')<=0");
           }
           _end_time = _end_time.Replace("'", "");
           if (!string.IsNullOrEmpty(_end_time)) {
              strTemp.Append(" and datediff(d,reg_time,'" + _end_time + "')>=0");
           }
           _keywords = _keywords.Replace("'", "");
           if (!string.IsNullOrEmpty(_keywords)) {
              strTemp.Append(" and (user_name like '%" + _keywords + "%' or mobile like '%" + _keywords + "%' or email like '%" + _keywords + "%' or nick_name like '%" + _keywords + "%')");
           }
           return strTemp.ToString();
        }
        #endregion

        #region 返回用户每页数量=========================
        private int GetPageSize(int _default_size) {
           int _pagesize;
           if (int.TryParse(Utils.GetCookie("user_list_page_size", "DTcmsPage"), out _pagesize)) {
              if (_pagesize > 0) {
                 return _pagesize;
              }
           }
           return _default_size;
        }
        #endregion

        //批量删除
        [HttpPost]
        public ActionResult SubmitDelete(string json) {
           ChkAdminLevel("user_list", DTEnums.ActionEnum.Delete.ToString()); //检查权限
           int sucCount = 0;
           int errorCount = 0;
           DTcms.BLL.users bll = new DTcms.BLL.users();
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
           AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除用户" + sucCount + "条，失败" + errorCount + "条"); //记录日志
           string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("user_list.aspx",
                           "site_id={0}&group_id={1}&start_time={2}&end_time={3}&keywords={4}",
                           this.site_id.ToString(), this.group_id.ToString(), this.start_time, this.end_time, this.keywords));
           return Content(script);
        }
    }
}
