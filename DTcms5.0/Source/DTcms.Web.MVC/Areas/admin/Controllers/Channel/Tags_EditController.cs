using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Channel {
   public class Tags_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Channel/tags_edit.cshtml";
      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;

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
            if (!new BLL.article_tags().Exists(this.id)) {
               JscriptMsg("记录不存在或已删除！", "back");
               filterContext.Result = result;
               return;
            }
         }
         ViewBag.Id = this.id;
         ViewBag.Action = this.action;
      }
      //
      // GET: /admin/Tags_Edit/
      public ActionResult Index() {
         ChkAdminLevel("sys_article_tags", DTEnums.ActionEnum.View.ToString()); //检查权限
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            //修改
            ShowInfo(this.id);
         }
         else {
            Model.article_tags model = new Model.article_tags();
            model.sort_id = 99;
            ViewData["model"] = model;
         }
         return View(WEB_VIEW);
      }

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
         BLL.article_tags bll = new BLL.article_tags();
         Model.article_tags model = bll.GetModel(_id);
         ViewData["model"] = model;
      }
      #endregion

      [HttpPost]
      public ActionResult SubmitSave() {
         ActionResult result = View(EDIT_RESULT_VIEW);
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("sys_article_tags", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id)) {
               JscriptMsg("保存过程中发生错误！", "back");
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("修改Tags标签成功！", "../tags_list/index");
         }
         else //添加
            {
            ChkAdminLevel("sys_article_tags", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd()) {
               JscriptMsg("保存过程中发生错误！", "back");
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("添加Tags标签成功！", "../tags_list/index");
         }
         return result;
      }

      #region 增加操作=================================
      private bool DoAdd() {
         bool result = false;
         Model.article_tags model = new Model.article_tags();
         BLL.article_tags bll = new BLL.article_tags();

         model.title = Request.Form["txtTitle"].Trim();
         model.is_red = Request.Form["cbIsRed"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         if (bll.Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加Tags标签:" + model.title); //记录日志
            result = true;
         }
         return result;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id) {
         bool result = false;
         BLL.article_tags bll = new BLL.article_tags();
         Model.article_tags model = bll.GetModel(_id);

         model.title = Request.Form["txtTitle"].Trim();
         model.is_red = Request.Form["cbIsRed"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改Tags标签:" + model.title); //记录日志
            result = true;
         }
         return result;
      }
      #endregion
   }
}
