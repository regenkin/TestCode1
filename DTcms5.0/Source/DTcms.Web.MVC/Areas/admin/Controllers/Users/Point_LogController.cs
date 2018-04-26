﻿using System;
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
    public class Point_LogController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Users/Point_Log.cshtml";
       protected int totalCount;
       protected int page;
       protected int pageSize;

       protected string keywords = string.Empty;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_point_log", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          this.keywords = DTRequest.GetQueryString("keywords");
          if (int.TryParse(DTRequest.GetQueryString("pageNum").Trim(), out pageSize)) {
             if (pageSize > 0) {
                Utils.WriteCookie("user_point_log_page_size", "DTcmsPage", pageSize.ToString(), 14400);
             }
          }
          else {
             this.pageSize = GetPageSize(10); //每页数量
          }
          ViewBag.Keywords = this.keywords;
          ViewBag.PageNum = this.pageSize.ToString();
       }
        //
        // GET: /admin/Point_Log/

       public ActionResult Index() {
          RptBind("id>0" + CombSqlTxt(keywords), "add_time desc,id desc");
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitDelete(string json) {
          ChkAdminLevel("user_point_log", DTEnums.ActionEnum.Delete.ToString()); //检查权限
          int sucCount = 0;
          int errorCount = 0;
          DTcms.BLL.user_point_log bll = new DTcms.BLL.user_point_log();
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
          AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除积分日志成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
          ViewBag.ClientScript = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("point_log.aspx", "keywords={0}", this.keywords));
          return View(EDIT_RESULT_VIEW);
       }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby) {
           this.page = DTRequest.GetQueryInt("page", 1);
           DTcms.BLL.user_point_log bll = new DTcms.BLL.user_point_log();
           DataTable list = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount).Tables[0];
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
              strTemp.Append(" and (user_name='" + _keywords + "' or remark like '%" + _keywords + "%')");
           }

           return strTemp.ToString();
        }
        #endregion

        #region 返回每页数量=============================
        private int GetPageSize(int _default_size) {
           int _pagesize;
           if (int.TryParse(Utils.GetCookie("user_point_log_page_size", "DTcmsPage"), out _pagesize)) {
              if (_pagesize > 0) {
                 return _pagesize;
              }
           }
           return _default_size;
        }
        #endregion
    }
}
