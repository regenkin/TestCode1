using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class Link_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/plugins/link/Areas/admin/Views/link_edit.cshtml";
      private string action = string.Empty;
      private int id = 0;
      private DTcms.Web.Plugin.Link.BLL.link bll = new DTcms.Web.Plugin.Link.BLL.link();

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("plugin_link", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         //取到操作类型
         string _action = DTRequest.GetQueryString("action");
         if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
            this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
            this.id = DTRequest.GetQueryInt("id");
            if (this.id == 0) {
               JscriptMsg("传输参数不正确！", "back");
               filterContext.Result = result;
            }
            if (!bll.Exists(this.id)) {
               JscriptMsg("内容不存在或已被删除！", "back");
               filterContext.Result = result;
            }
         }
         else {
            this.id = -1;
         }
         ViewBag.Action = this.action;
         ViewBag.Id = this.id.ToString();
      }
      //
      // GET: /admin/Link_Edit/

      public ActionResult Index() {
         TreeBind();
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ShowInfo(this.id);
         }
         else {
            DTcms.Web.Plugin.Link.Model.link model = new DTcms.Web.Plugin.Link.Model.link();
            ViewData["model"] = model;
         }
         return View(WEB_VIEW);
      }

      [HttpPost, ValidateInput(false)]
      public ActionResult SubmitSave(string txtContent) {
         ActionResult result = View(EDIT_RESULT_VIEW);
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("plugin_link", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id)) {
               ViewBag.ClientScript = JscriptMsg("保存过程中发生错误啦！", string.Empty);
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("修改链接成功！", "../link_list/index");
         }
         else //添加
            {
            ChkAdminLevel("plugin_link", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd()) {
               ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("添加链接成功！", "../link_list/index");
         }
         return result;
      }

      private void ShowInfo(int _id) {
         DTcms.Web.Plugin.Link.Model.link model = bll.GetModel(_id);
         ViewData["model"] = model;
      }

      private void TreeBind() {
         DTcms.BLL.sites bll_site = new DTcms.BLL.sites();
         DataTable dt = bll_site.GetList(0, "", "sort_id asc,id desc").Tables[0];
         List<SelectListItem> channelList = new List<SelectListItem>() {
            new SelectListItem(){ Text="请选择站点...", Value=""}
         };
         foreach (DataRow dr in dt.Rows) {
            channelList.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
         }
         ViewData["siteSelectItems"] = channelList;
      }

      private bool DoAdd() {
         bool result = false;
         DTcms.Web.Plugin.Link.Model.link model = new DTcms.Web.Plugin.Link.Model.link();
         model.site_id = Utils.StrToInt(Request.Form["ddlSiteId"], -1);
         model.title = Request.Form["txtTitle"].Trim();
         model.is_lock = Request.Form["rblIsLock"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.is_red = Request.Form["cbIsRed"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 0);
         model.user_name = Request.Form["txtUserName"].Trim();
         model.user_tel = Request.Form["txtUserTel"].Trim();
         model.email = Request.Form["txtEmail"].Trim();
         model.site_url = Request.Form["txtSiteUrl"].Trim();
         model.img_url = Request.Form["txtImgUrl"].Trim();
         model.is_image = 1;
         model.add_time = DateTime.Now;
         if (string.IsNullOrEmpty(model.img_url)) {
            model.is_image = 0;
         }
         if (bll.Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加友情链接：" + model.title);
            result = true;
         }
         return result;
      }

      private bool DoEdit(int _id) {
         bool result = false;
         DTcms.Web.Plugin.Link.Model.link model = bll.GetModel(_id);
         model.site_id = Utils.StrToInt(Request.Form["ddlSiteId"], -1);
         model.title = Request.Form["txtTitle"].Trim();
         model.is_lock = Request.Form["rblIsLock"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.is_red = Request.Form["cbIsRed"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 0);
         model.user_name = Request.Form["txtUserName"].Trim();
         model.user_tel = Request.Form["txtUserTel"].Trim();
         model.email = Request.Form["txtEmail"].Trim();
         model.site_url = Request.Form["txtSiteUrl"].Trim();
         model.img_url = Request.Form["txtImgUrl"].Trim();
         model.is_image = 1;
         model.add_time = DateTime.Now;
         if (string.IsNullOrEmpty(model.img_url)) {
            model.is_image = 0;
         }
         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改友情链接：" + model.title);
            result = true;
         }
         return result;
      }
   }
}
