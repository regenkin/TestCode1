using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Users
{
    public class Site_Oauth_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Users/Site_Oauth_Edit.cshtml";
       private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
       private Model.site_oauth model;
       private int id = 0;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_oauth", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          string _action = DTRequest.GetQueryString("action");

          if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
             this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
             this.id = DTRequest.GetQueryInt("id");
             if (this.id == 0) {
                JscriptMsg("传输参数不正确！", "back");
                return;
             }
             if (!new BLL.site_oauth().Exists(this.id)) {
                JscriptMsg("记录不存在或已被删除！", "back");
                return;
             }
             //赋值实体
             this.model = new BLL.site_oauth().GetModel(this.id);
          }
          ViewBag.Action = this.action;
          ViewBag.Id = this.id;
       }
        //
        // GET: /admin/Site_Oauth_Edit/

        public ActionResult Index()
        {
          SiteBind(); //绑定站点
          if (action == DTEnums.ActionEnum.Edit.ToString())                 {
             //修改
             ShowInfo(this.id);
          }
            return View(WEB_VIEW);
        }

        [HttpPost]
        public ActionResult SubmitSave() {
           if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
              ChkAdminLevel("user_oauth", DTEnums.ActionEnum.Edit.ToString()); //检查权限
              if (!DoEdit(this.id)) {
                 JscriptMsg("保存过程中发生错误！", string.Empty);
              }
              else {
                 JscriptMsg("修改站点OAuth列表成功！", "../site_oauth_list/index");
              }
           }
           else //添加
            {
              ChkAdminLevel("user_oauth", DTEnums.ActionEnum.Add.ToString()); //检查权限
              if (!DoAdd()) {
                 JscriptMsg("保存过程中发生错误！", string.Empty);
              }
              else {
                 JscriptMsg("添加站点OAuth列表成功！", "../site_oauth_list/index");
              }
           }
           return View(EDIT_RESULT_VIEW);
        }

        #region 绑定站点=================================
        private void SiteBind() {
           DTcms.BLL.sites bll = new DTcms.BLL.sites();
           DataTable dt = bll.GetList(0, "is_lock=0", "sort_id asc,id desc").Tables[0];
           List<SelectListItem> siteList = new List<SelectListItem>() {
            new SelectListItem(){ Text="请选择站点...", Value="" }
         };
           foreach (DataRow dr in dt.Rows) {
              siteList.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
           }
           ViewData["siteListItems"] = siteList;
        }
        #endregion

        #region 绑定应用=================================
        private void OauthBind(int _site_id, int _oauth_id) {
           if (_site_id > 0) {
              DataTable dt = new BLL.oauth_app().GetList(_site_id, _oauth_id).Tables[0];
              List<SelectListItem> appList = new List<SelectListItem>() {
                 new SelectListItem(){ Text="请选择应用...", Value=""}
              };
              foreach (DataRow dr in dt.Rows) {
                 appList.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
              }
              ViewData["appListItems"] = appList;
           }
           else {
              List<SelectListItem> appList = new List<SelectListItem>() {
                 new SelectListItem(){ Text="请选择站点...", Value=""}
              };
              ViewData["appListItems"] = appList;
           }
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id) {
           OauthBind(model.site_id, model.oauth_id); //绑定应用
           ViewData["model"] = this.model;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd() {
           bool result = false;
           this.model = new Model.site_oauth();
           BLL.site_oauth bll = new BLL.site_oauth();

           model.site_id = Utils.StrToInt(Request.Form["ddlSiteId"], 0);
           model.oauth_id = Utils.StrToInt(Request.Form["ddlOauthId"], 0);
           model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
           model.title = Request.Form["txtTitle"].Trim();
           model.app_id = Request.Form["txtAppId"].Trim();
           model.app_key = Request.Form["txtAppKey"].Trim();
           if (bll.Add(model) > 0) {
              AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加站点OAuth列表:" + model.title); //记录日志
              result = true;
           }
           return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id) {
           bool result = false;
           BLL.site_oauth bll = new BLL.site_oauth();

           model.site_id = Utils.StrToInt(Request.Form["ddlSiteId"], 0);
           model.oauth_id = Utils.StrToInt(Request.Form["ddlOauthId"], 0);
           model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
           model.title = Request.Form["txtTitle"].Trim();
           model.app_id = Request.Form["txtAppId"].Trim();
           model.app_key = Request.Form["txtAppKey"].Trim();
           if (bll.Update(model)) {
              AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改站点OAuth列表:" + model.title); //记录日志
              result = true;
           }

           return result;
        }
        #endregion
    }
}
