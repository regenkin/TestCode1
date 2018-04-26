using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Nav_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Settings/nav_edit.cshtml";
       private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
       private int id = 0;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("sys_navigation", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          ActionResult result = View(EDIT_RESULT_VIEW);
          string _action = DTRequest.GetQueryString("action");
          this.id = DTRequest.GetQueryInt("id");

          if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
             this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
             if (this.id == 0) {
                JscriptMsg("传输参数不正确！", "back");
                filterContext.Result = result;
                return;
             }
             if (!new DTcms.BLL.navigation().Exists(this.id)) {
                JscriptMsg("导航不存在或已被删除！", "back");
                filterContext.Result = result;
                return;
             }
          }
          ViewBag.Action = this.action;
          ViewBag.Id = this.id.ToString();
       }
        //
        // GET: /admin/Nav_Edit/

       public ActionResult Index() {
          TreeBind(DTEnums.NavigationEnum.System.ToString()); //绑定导航菜单
          if (action == DTEnums.ActionEnum.Edit.ToString()) {
             ShowInfo(this.id);
          }
          else {
             ViewData["model"] = new DTcms.Model.navigation();
             ViewBag.ClientScript = "$('#txtName').attr('ajaxurl', '/tools/admin_ajax.ashx?action=navigation_validate');";
          }
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitSave() {
          ActionResult result = View(EDIT_RESULT_VIEW);
          if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
             ChkAdminLevel("sys_navigation", DTEnums.ActionEnum.Edit.ToString()); //检查权限
             if (!DoEdit(this.id)) {
                ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", "back");
                return result;
             }
             ViewBag.ClientScript = JscriptMsg("修改导航菜单成功！", "../nav_list/index", "parent.loadMenuTree");
          }
          else //添加
            {
             ChkAdminLevel("sys_navigation", DTEnums.ActionEnum.Add.ToString()); //检查权限
             if (!DoAdd()) {
                ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", "back");
                return result;
             }
             ViewBag.ClientScript = JscriptMsg("添加导航菜单成功！", "../nav_list/index", "parent.loadMenuTree");
          }
          return result;
       }

       #region 绑定导航菜单=============================
       private void TreeBind(string nav_type) {
          DTcms.BLL.navigation bll = new DTcms.BLL.navigation();
          DataTable dt = bll.GetList(0, nav_type);
          List<SelectListItem> parentList = new List<SelectListItem>();
          parentList.Add(new SelectListItem() { Text = "无父级导航", Value = "0" });
          foreach (DataRow dr in dt.Rows) {
             string Id = dr["id"].ToString();
             int ClassLayer = int.Parse(dr["class_layer"].ToString());
             string Title = dr["title"].ToString();
             if (ClassLayer == 1) {
                parentList.Add(new SelectListItem() { Text = Title, Value = Id });
             }
             else {
                Title = "├ " + Title;
                Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                parentList.Add(new SelectListItem() { Text = Title, Value = Id });
             }
          }
          ViewData["selectListItems"] = parentList;
       }
       #endregion

       #region 赋值操作=================================
       private void ShowInfo(int _id) {
          DTcms.BLL.navigation bll = new DTcms.BLL.navigation();
          DTcms.Model.navigation model = bll.GetModel(_id);
          ViewData["model"] = model;
          ViewBag.ClientScript = "$('#txtName').attr('ajaxurl', '/tools/admin_ajax.ashx?action=navigation_validate&old_name=" + Utils.UrlEncode(model.name) + "');";
       }
       #endregion

       #region 增加操作=================================
       private bool DoAdd() {
          try {
             DTcms.Model.navigation model = new DTcms.Model.navigation();
             DTcms.BLL.navigation bll = new DTcms.BLL.navigation();

             model.nav_type = DTEnums.NavigationEnum.System.ToString();
             model.name = Request.Form["txtName"].Trim();
             model.title = Request.Form["txtTitle"].Trim();
             model.sub_title = Request.Form["txtSubTitle"].Trim();
             model.icon_url = Request.Form["txtIconUrl"].Trim();
             model.link_url = Request.Form["txtLinkUrl"].Trim();
             model.sort_id = int.Parse(Request.Form["txtSortId"].Trim());
             model.is_lock = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
             model.remark = Request.Form["txtRemark"].Trim();
             model.parent_id = int.Parse(Request.Form["ddlParentId"]);

             //添加操作权限类型
             string action_type_str = string.Empty;
             foreach (KeyValuePair<string, string> kv in Utils.ActionType()) {
                if (Request.Form["cblActiontype_" + kv.Key].ToLower().IndexOf("true") >= 0) {
                   action_type_str += kv.Key + ",";
                }
             }
             model.action_type = Utils.DelLastComma(action_type_str);
             if (bll.Add(model) > 0) {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加导航菜单:" + model.title); //记录日志
                return true;
             }
          }
          catch {
             return false;
          }
          return false;
       }
       #endregion

       #region 修改操作=================================
       private bool DoEdit(int _id) {
          try {
             DTcms.BLL.navigation bll = new DTcms.BLL.navigation();
             DTcms.Model.navigation model = bll.GetModel(_id);

             model.name = Request.Form["txtName"].Trim();
             model.title = Request.Form["txtTitle"].Trim();
             model.sub_title = Request.Form["txtSubTitle"].Trim();
             model.icon_url = Request.Form["txtIconUrl"].Trim();
             model.link_url = Request.Form["txtLinkUrl"].Trim();
             model.sort_id = int.Parse(Request.Form["txtSortId"].Trim());
             model.is_lock = Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
             model.remark = Request.Form["txtRemark"].Trim();
             if (model.is_sys == 0) {
                int parentId = int.Parse(Request.Form["ddlParentId"]);
                //如果选择的父ID不是自己,则更改
                if (parentId != model.id) {
                   model.parent_id = parentId;
                }
             }

             //添加操作权限类型
             string action_type_str = string.Empty;
             foreach (KeyValuePair<string, string> kv in Utils.ActionType()) {
                if (Request.Form["cblActiontype_" + kv.Key].ToLower().IndexOf("true") >= 0) {
                   action_type_str += kv.Key + ",";
                }
             }
             model.action_type = Utils.DelLastComma(action_type_str);
             if (bll.Update(model)) {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "修改导航菜单:" + model.title); //记录日志
                return true;
             }
          }
          catch {
             return false;
          }
          return false;
       }
       #endregion
    }
}
