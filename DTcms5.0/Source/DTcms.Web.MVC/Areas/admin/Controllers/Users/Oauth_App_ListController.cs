﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Oauth_App_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Users/Oauth_App_List.cshtml";
       protected int totalCount;
       protected int page;
       protected int pageSize;

       protected string keywords = string.Empty;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_oauth", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          this.keywords = DTRequest.GetQueryString("keywords");
          if (int.TryParse(DTRequest.GetQueryString("pagesize"), out this.pageSize)) {
             if (this.pageSize > 0) {
                Utils.WriteCookie("oauth_app_list_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
             }
          }
          else {
             this.pageSize = GetPageSize(10); //每页数量
          }
          ViewBag.Keywords = this.keywords;
          ViewBag.PageNum = this.pageSize;
       }
        //
        // GET: /admin/Aauth_App_List/

       public ActionResult Index() {
          RptBind("id>0" + CombSqlTxt(keywords), "sort_id asc,id desc");
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitSave(string json) {
          ChkAdminLevel("user_oauth", DTEnums.ActionEnum.Delete.ToString()); //检查权限
          DTcms.BLL.oauth_app bll = new DTcms.BLL.oauth_app();
          JObject jobject = JObject.Parse(json);
          JToken record = jobject["list"];
          foreach (JToken item in record) {
             int id;
             if (!int.TryParse(item["id"].ToString(), out id))
                id = -1;
             int sortId;
             if (!int.TryParse(item["sortId"].ToString(), out sortId))
                sortId = 99;
             bll.UpdateField(id, "sort_id=" + sortId.ToString());
          }
          AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存OAuth配置排序"); //记录日志
          string script = JscriptMsg("保存排序成功！", "index");
          return Content(script);
       }

       [HttpPost]
       public ActionResult SubmitDelete(string json) {
          ChkAdminLevel("user_oauth", DTEnums.ActionEnum.Delete.ToString()); //检查权限
          int sucCount = 0;
          int errorCount = 0;
          DTcms.BLL.oauth_app bll = new DTcms.BLL.oauth_app();
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
          AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除OAuth配置成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
          string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
          return Content(script);
       }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby) {
           this.page = DTRequest.GetQueryInt("page", 1);
           DTcms.BLL.oauth_app bll = new DTcms.BLL.oauth_app();
           DataTable list = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount).Tables[0];
           //修正asp.net路径为MVC路径
           foreach (DataRow dr in list.Rows) {
              dr["img_url"] = dr["img_url"].ToString().Replace("images", "Content");
           }
           ViewData["list"] = list;
           string pageUrl = Utils.CombUrlTxt("index", "keywords={0}&page={1}", this.keywords, "__id__");
           ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords) {
           StringBuilder strTemp = new StringBuilder();
           _keywords = _keywords.Replace("'", "");
           if (!string.IsNullOrEmpty(_keywords)) {
              strTemp.Append(" and title like  '%" + _keywords + "%'");
           }

           return strTemp.ToString();
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size) {
           int _pagesize;
           if (int.TryParse(Utils.GetCookie("oauth_app_list_page_size", "DTcmsPage"), out _pagesize)) {
              if (_pagesize > 0) {
                 return _pagesize;
              }
           }
           return _default_size;
        }
        #endregion
    }
}
