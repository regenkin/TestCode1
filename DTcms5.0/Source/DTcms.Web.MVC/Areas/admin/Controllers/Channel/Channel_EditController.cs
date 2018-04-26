using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class Channel_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Channel/channel_edit.cshtml";
      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         string _action = DTRequest.GetQueryString("action");

         if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
            this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
            this.id = DTRequest.GetQueryInt("id");
            if (this.id == 0) {
               JscriptMsg("传输参数不正确！", "back");
               filterContext.Result = result;
               return;
            }
            if (!new DTcms.BLL.site_channel().Exists(this.id)) {
               JscriptMsg("记录不存在或已删除！", "back");
               filterContext.Result = result;
               return;
            }
         }
         ViewBag.Action = this.action;
         ViewBag.Id = this.id.ToString();
      }
      //
      // GET: /admin/Channel_Edit/

      public ActionResult Index() {
         SiteBind(); //绑定站点
         FieldBind(); //绑定扩展字段
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ShowInfo(this.id);
         }
         else {
            DTcms.Model.site_channel model = new DTcms.Model.site_channel();
            ViewData["model"] = model;
            ViewBag.ClientSctipt = "$('#txtName').attr('ajaxurl','/tools/admin_ajax.ashx?action=channel_name_validate');";
            ViewData["urlList"] = new List<Model.url_rewrite>();
         }
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ActionResult result = View(EDIT_RESULT_VIEW);
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id)) {
               JscriptMsg("保存过程中发生错误！", "back");
               return result;
            }
            JscriptMsg("修改频道成功！", "../channel_list/index", "parent.loadMenuTree");
         }
         else //添加
            {
            ChkAdminLevel("sys_channel_manage", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd()) {
               JscriptMsg("保存过程中发生错误！", "back");
               return result;
            }
            JscriptMsg("添加频道成功！", "../channel_list/index", "parent.loadMenuTree");
         }
         return result;
      }

      #region 返回页面继承类===========================
      private string GetInherit(string page_type) {
         string result = "";
         switch (page_type) {
            case "index":
               result = "DTcms.Web.UI.Page.article";
               break;
            case "category":
               result = "DTcms.Web.UI.Page.category";
               break;
            case "list":
               result = "DTcms.Web.UI.Page.article_list";
               break;
            case "detail":
               result = "DTcms.Web.UI.Page.article_show";
               break;
         }
         return result;
      }
      #endregion

      #region 绑定站点=================================
      private void SiteBind() {
         DTcms.BLL.sites bll = new DTcms.BLL.sites();
         DataTable dt = bll.GetList(0, "is_lock=0", "sort_id asc,id desc").Tables[0];
         List<SelectListItem> siteList = new List<SelectListItem>() {
            new SelectListItem(){ Text="请选择站点...", Value="" }
         };
         foreach (DataRow dr in dt.Rows) {
            siteList.Add(new SelectListItem(){ Text=dr["title"].ToString(), Value=dr["id"].ToString() });
         }
         ViewData["siteListItems"] = siteList;
      }
      #endregion

      #region 绑定扩展字段=============================
      private void FieldBind() {
         DTcms.BLL.article_attribute_field bll = new DTcms.BLL.article_attribute_field();
         DataTable dt = bll.GetList(0, "", "is_sys desc,sort_id asc,id desc").Tables[0];
         ViewData["fieldList"] = dt;
      }
      #endregion

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
         DTcms.BLL.site_channel bll = new DTcms.BLL.site_channel();
         DTcms.Model.site_channel model = bll.GetModel(_id);
         ViewData["model"] = model;
         //绑定URL配置列表
         List<DTcms.Model.url_rewrite> url_list = new DTcms.BLL.url_rewrite().GetList(model.name);
         if (url_list == null) {
            url_list = new List<Model.url_rewrite>();
         }
         ViewData["urlList"] = url_list;
      }
      #endregion

      #region 增加操作=================================
      private bool DoAdd() {
         DTcms.Model.site_channel model = new DTcms.Model.site_channel();
         DTcms.BLL.site_channel bll = new DTcms.BLL.site_channel();

         model.site_id = Utils.StrToInt(Request.Form["ddlSiteId"], 0);
         model.name = Request.Form["txtName"].Trim();
         model.title = Request.Form["txtTitle"].Trim();
         model.is_albums = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 0 : 1;
         model.is_albums = Request.Form["cbIsComment"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.is_albums = Request.Form["cbIsAlbums"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.is_attach = Request.Form["cbIsAttach"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.is_spec = Request.Form["cbIsSpec"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         //添加频道扩展字段
         DTcms.BLL.article_attribute_field bll_field = new DTcms.BLL.article_attribute_field();
         DataTable dt = bll_field.GetList(0, "", "sort_id asc,id desc").Tables[0];
         List<DTcms.Model.site_channel_field> ls = new List<DTcms.Model.site_channel_field>();
         for (int i = 0; i < dt.Rows.Count; i++) {
            DataRow dr = dt.Rows[i];
            string id = "cblAttributeField_" + i.ToString();
            //如果Html的Checkbox控件加入了Value属性,选中状态会把value内容传过来而不是true
            if (Request.Form.GetValues(id).Length > 1) {
               ls.Add(new DTcms.Model.site_channel_field { channel_id = model.id, field_id = (int)dr["id"] });
            }
         }
         model.channel_fields = ls;

         if (bll.Add(model) < 1) {
            return false;
         }

         #region 保存URL配置
         DTcms.BLL.url_rewrite urlBll = new DTcms.BLL.url_rewrite();
         urlBll.Remove("channel", model.name); //先删除
         string[] itemTypeArr = Request.Form.GetValues("item_type");
         string[] itemNameArr = Request.Form.GetValues("item_name");
         string[] itemPageArr = Request.Form.GetValues("item_page");
         string[] itemTempletArr = Request.Form.GetValues("item_templet");
         string[] itemPageSizeArr = Request.Form.GetValues("item_pagesize");
         string[] itemRewriteArr = Request.Form.GetValues("item_rewrite");

         if (itemTypeArr != null && itemNameArr != null && itemPageArr != null && itemTempletArr != null && itemPageSizeArr != null && itemRewriteArr != null) {
            if ((itemTypeArr.Length == itemNameArr.Length) && (itemNameArr.Length == itemPageArr.Length) && (itemPageArr.Length == itemTempletArr.Length)
                && (itemTempletArr.Length == itemPageSizeArr.Length) && (itemPageSizeArr.Length == itemRewriteArr.Length)) {
               for (int i = 0; i < itemTypeArr.Length; i++) {
                  DTcms.Model.url_rewrite urlModel = new DTcms.Model.url_rewrite();
                  urlModel.name = itemNameArr[i].Trim();
                  urlModel.type = itemTypeArr[i].Trim();
                  urlModel.page = itemPageArr[i].Trim();
                  urlModel.inherit = GetInherit(urlModel.type);
                  urlModel.templet = itemTempletArr[i].Trim();
                  if (Utils.StrToInt(itemPageSizeArr[i].Trim(), 0) > 0) {
                     urlModel.pagesize = itemPageSizeArr[i].Trim();
                  }
                  urlModel.channel = model.name;

                  List<DTcms.Model.url_rewrite_item> itemLs = new List<DTcms.Model.url_rewrite_item>();
                  string[] urlRewriteArr = itemRewriteArr[i].Split('&'); //分解URL重写字符串
                  for (int j = 0; j < urlRewriteArr.Length; j++) {
                     string[] urlItemArr = urlRewriteArr[j].Split(',');
                     if (urlItemArr.Length == 3) {
                        itemLs.Add(new DTcms.Model.url_rewrite_item { path = urlItemArr[0], pattern = urlItemArr[1], querystring = urlItemArr[2] });
                     }
                  }
                  urlModel.url_rewrite_items = itemLs;
                  urlBll.Add(urlModel);
               }
            }
         }
         #endregion

         AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加频道" + model.title); //记录日志
         return true;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id) {
         
         DTcms.BLL.site_channel bll = new DTcms.BLL.site_channel();
         DTcms.Model.site_channel model = bll.GetModel(_id);
         string old_name = model.name;
         model.site_id = Utils.StrToInt(Request.Form["ddlSiteId"], 0);
         model.name = Request.Form["txtName"].Trim();
         model.title = Request.Form["txtTitle"].Trim();
         model.is_albums = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 0 : 1;
         model.is_albums = Request.Form["cbIsComment"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.is_albums = Request.Form["cbIsAlbums"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.is_attach = Request.Form["cbIsAttach"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.is_spec = Request.Form["cbIsSpec"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         //添加频道扩展字段
         DTcms.BLL.article_attribute_field bll_field = new DTcms.BLL.article_attribute_field();
         DataTable dt = bll_field.GetList(0, "", "sort_id asc,id desc").Tables[0];
         List<DTcms.Model.site_channel_field> ls = new List<DTcms.Model.site_channel_field>();
         string[] vals = Request.Form.GetValues("cblAttributeField_1");
         for(int i=0; i<dt.Rows.Count; i++) {
            DataRow dr = dt.Rows[i];
            string id = "cblAttributeField_" + i.ToString();
            //如果Html的Checkbox控件加入了Value属性,选中状态会把value内容传过来而不是true
            if (Request.Form.GetValues(id).Length > 1) {
               ls.Add(new DTcms.Model.site_channel_field { channel_id = model.id, field_id = (int)dr["id"] });
            }
         }
         model.channel_fields = ls;
         if (!bll.Update(model)) {
            return false;
         }

         #region 保存URL配置
         DTcms.BLL.url_rewrite urlBll = new DTcms.BLL.url_rewrite();
         urlBll.Remove("channel", old_name); //先删除旧的
         string[] itemTypeArr = Request.Form.GetValues("item_type");
         string[] itemNameArr = Request.Form.GetValues("item_name");
         string[] itemPageArr = Request.Form.GetValues("item_page");
         string[] itemTempletArr = Request.Form.GetValues("item_templet");
         string[] itemPageSizeArr = Request.Form.GetValues("item_pagesize");
         string[] itemRewriteArr = Request.Form.GetValues("item_rewrite");

         if (itemTypeArr != null && itemNameArr != null && itemPageArr != null && itemTempletArr != null && itemPageSizeArr != null && itemRewriteArr != null) {
            if ((itemTypeArr.Length == itemNameArr.Length) && (itemNameArr.Length == itemPageArr.Length) && (itemPageArr.Length == itemTempletArr.Length)
                && (itemTempletArr.Length == itemPageSizeArr.Length) && (itemPageSizeArr.Length == itemRewriteArr.Length)) {
               for (int i = 0; i < itemTypeArr.Length; i++) {
                  DTcms.Model.url_rewrite urlModel = new DTcms.Model.url_rewrite();
                  urlModel.name = itemNameArr[i].Trim();
                  urlModel.type = itemTypeArr[i].Trim();
                  urlModel.page = itemPageArr[i].Trim();
                  urlModel.inherit = GetInherit(urlModel.type);
                  urlModel.templet = itemTempletArr[i].Trim();
                  if (Utils.StrToInt(itemPageSizeArr[i].Trim(), 0) > 0) {
                     urlModel.pagesize = itemPageSizeArr[i].Trim();
                  }
                  urlModel.channel = model.name;

                  List<DTcms.Model.url_rewrite_item> itemLs = new List<DTcms.Model.url_rewrite_item>();
                  string[] urlRewriteArr = itemRewriteArr[i].Split('&'); //分解URL重写字符串
                  for (int j = 0; j < urlRewriteArr.Length; j++) {
                     string[] urlItemArr = urlRewriteArr[j].Split(',');
                     if (urlItemArr.Length == 3) {
                        itemLs.Add(new DTcms.Model.url_rewrite_item { path = urlItemArr[0], pattern = urlItemArr[1], querystring = urlItemArr[2] });
                     }
                  }
                  urlModel.url_rewrite_items = itemLs;
                  urlBll.Add(urlModel);
               }
            }
         }
         #endregion

         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改频道" + model.title); //记录日志
         return true;
      }
      #endregion
   }
}
