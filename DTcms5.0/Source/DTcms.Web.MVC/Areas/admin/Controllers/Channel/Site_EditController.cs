using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public partial class Site_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Channel/site_edit.cshtml";
      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;
      private DTcms.BLL.navigation bll_navigation = new DTcms.BLL.navigation();

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("sys_site_manage", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         this.id = DTRequest.GetQueryInt("id");
         this.action = DTRequest.GetQueryString("action");
         if (!string.IsNullOrEmpty(this.action) && this.action == DTEnums.ActionEnum.Edit.ToString()) {
            if (this.id == 0) {
               JscriptMsg("传输参数不正确！", "back");
               filterContext.Result = result;
               return;
            }
            if (!new DTcms.BLL.sites().Exists(this.id)) {
               JscriptMsg("记录不存在或已被删除！", "back");
               filterContext.Result = result;
               return;
            }
         }
         ViewBag.Action = this.action;
         ViewBag.Id = this.id.ToString();
      }
      //
      // GET: /admin/Site_Edit/
      public ActionResult Index() {
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ShowInfo(this.id);
         }
         else {
            ViewData["model"] = new DTcms.Model.sites();
         }
         SiteBind(this.id); //绑定站点
         return View(WEB_VIEW);
      }

      //保存
      public ActionResult SubmitSave() {
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("sys_site_manage", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id)) {
               JscriptMsg("保存过程中发生错误！", "");
            }
            else {
               JscriptMsg("修改站点信息成功！", "../site_list/index", "parent.loadMenuTree");
            }
         }
         else //添加
            {
            ChkAdminLevel("sys_site_manage", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd()) {
               JscriptMsg("保存过程中发生错误！", "");
            }
            else {
               JscriptMsg("添加站点信息成功！", "../site_list/index", "parent.loadMenuTree");
            }
         }
         return View(EDIT_RESULT_VIEW);
      }

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
         DTcms.BLL.sites bll = new DTcms.BLL.sites();
         DTcms.Model.sites model = bll.GetModel(_id);
         ViewData["model"] = model;
      }
      #endregion

      #region 绑定站点=================================
      private void SiteBind(int _id) {
         string strWhere = string.Empty;
         if (_id > 0) {
            strWhere = "is_lock=0 and parent_id=0 and id<>" + _id;
         }
         else {
            strWhere = "is_lock=0 and parent_id=0";
         }
         BLL.sites bll = new BLL.sites();
         DataTable dt = bll.GetList(0, strWhere, "sort_id asc,id desc").Tables[0];
         List<SelectListItem> selectListItems = new List<SelectListItem>();
         selectListItems.Add(new SelectListItem() { Text = "不基于其它站点", Value = "0" });
         foreach (DataRow dr in dt.Rows) {
            selectListItems.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
         }
         ViewData["selectListItems"] = selectListItems;
      }
      #endregion

      #region 增加操作=================================
      private bool DoAdd() {
         DTcms.Model.sites model = new DTcms.Model.sites();
         DTcms.BLL.sites bll = new DTcms.BLL.sites();

         model.parent_id = int.Parse(Request.Form["ddlParentId"]);
         model.title = Request.Form["txtTitle"].Trim();
         model.build_path = Request.Form["txtBuildPath"].Trim();
         model.domain = Request.Form["txtDomain"].Trim();
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
         model.is_default = Request.Form["cbIsDefault"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.is_default = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.site_type = int.Parse(Request.Form["rblSiteType"]);
         model.name = Request.Form["txtName"].Trim();
         model.logo = Request.Form["txtLogo"].Trim();
         model.company = Request.Form["txtCompany"].Trim();
         model.address = Request.Form["txtAddress"].Trim();
         model.tel = Request.Form["txtTel"].Trim();
         model.fax = Request.Form["txtFax"].Trim();
         model.email = Request.Form["txtEmail"].Trim();
         model.crod = Request.Form["txtCrod"].Trim();
         model.seo_title = Request.Form["txtSeoTitle"].Trim();
         model.seo_keyword = Request.Form["txtSeoKeyword"].Trim();
         model.seo_description = Utils.DropHTML(Request.Form["txtSeoDescription"]);
         model.copyright = Request.Form["txtCopyright"].Trim();
         if (bll.Add(model) > 0) {
            //更新一下域名缓存
            CacheHelper.Remove(DTKeys.CACHE_SITE_HTTP_DOMAIN);
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加站点:" + model.title); //记录日志
            return true;
         }

         return false;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id) {
         bool result = false;
         DTcms.BLL.sites bll = new DTcms.BLL.sites();
         DTcms.Model.sites model = bll.GetModel(_id);

         model.parent_id = int.Parse(Request.Form["ddlParentId"]);
         model.title = Request.Form["txtTitle"].Trim();
         model.build_path = Request.Form["txtBuildPath"].Trim();
         model.domain = Request.Form["txtDomain"].Trim();
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
         model.is_default = Request.Form["cbIsDefault"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.is_lock = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.site_type = int.Parse(Request.Form["rblSiteType"]);
         model.name = Request.Form["txtName"].Trim();
         model.logo = Request.Form["txtLogo"].Trim();
         model.company = Request.Form["txtCompany"].Trim();
         model.address = Request.Form["txtAddress"].Trim();
         model.tel = Request.Form["txtTel"].Trim();
         model.fax = Request.Form["txtFax"].Trim();
         model.email = Request.Form["txtEmail"].Trim();
         model.crod = Request.Form["txtCrod"].Trim();
         model.seo_title = Request.Form["txtSeoTitle"].Trim();
         model.seo_keyword = Request.Form["txtSeoKeyword"].Trim();
         model.seo_description = Utils.DropHTML(Request.Form["txtSeoDescription"]);
         model.copyright = Request.Form["txtCopyright"].Trim();
         if (bll.Update(model)) {
            //更新一下域名缓存
            CacheHelper.Remove(DTKeys.CACHE_SITE_HTTP_DOMAIN);
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改站点:" + model.title); //记录日志
            result = true;
         }

         return result;
      }
      #endregion

      #region 复制通道信息
      private int CopyChannel(DTcms.Model.sites sourceSite, DTcms.Model.sites targetSite) {
         int result = 0;
         DTcms.BLL.site_channel bll_channel = new DTcms.BLL.site_channel();
         DataTable channelList = bll_channel.GetList(99, "site_id=" + sourceSite.id.ToString(), "id").Tables[0];
         foreach (DataRow dr in channelList.Rows) {
            // 获取通道数据实体(包含扩展字段信息)
            DTcms.Model.site_channel sourceChannel = bll_channel.GetModel((int)dr["id"]);
            // 新通道赋值
            DTcms.Model.site_channel newChannel = new DTcms.Model.site_channel();
            newChannel.is_albums = sourceChannel.is_albums;
            newChannel.is_attach = sourceChannel.is_attach;
            newChannel.is_spec = sourceChannel.is_spec;
            if (sourceChannel.name.IndexOf("_") >= 0) {
               string[] strs = sourceChannel.name.Split('_');
               newChannel.name = targetSite.build_path + "_" + strs[strs.Length -1];
            }
            else {
               newChannel.name = targetSite.build_path + "_" + sourceChannel.name;
            }
            newChannel.site_id = targetSite.id;
            newChannel.sort_id = sourceChannel.sort_id;
            newChannel.title = sourceChannel.title;
            if (sourceChannel.channel_fields != null) {
               //扩展字段赋值
               newChannel.channel_fields = new List<DTcms.Model.site_channel_field>();
               foreach (DTcms.Model.site_channel_field item in sourceChannel.channel_fields) {
                  newChannel.channel_fields.Add(new DTcms.Model.site_channel_field { channel_id = newChannel.id, field_id = item.field_id });
               }
            }
            bll_channel.Add(newChannel);
            result++;
         }
         return result;
      }
      #endregion

      private DTcms.Model.sites GetSystemSite() {
         DTcms.BLL.sites bll = new DTcms.BLL.sites();
         DataRow siteRow = bll.GetList(1, "", "id").Tables[0].Rows[0];
         return bll.GetModel((int)siteRow["id"]);
      }
   }
}
