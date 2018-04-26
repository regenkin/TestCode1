using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.weixin {
   public class Text_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/weixin/text_edit.cshtml";

      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("weixin_response_text", DTEnums.ActionEnum.View.ToString()); //检查权限
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
            }
            if (!new BLL.weixin_request_rule().Exists(this.id)) {
               JscriptMsg("记录不存在或已删除！", "back");
               filterContext.Result = result;
            }
         }
         ViewBag.Action = this.action;
         ViewBag.Id = this.id.ToString();
      }
      //
      // GET: /admin/Text_Edit/

      public ActionResult Index() {
         TreeBind();
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ShowInfo(this.id);
         }
         else {
            DTcms.Model.weixin_request_rule model = new DTcms.Model.weixin_request_rule();
            ViewData["model"] = model;
         }
         return View(WEB_VIEW);
      }

      [HttpPost, ValidateInput(false)]
      public ActionResult SubmitSave(string txtContent) {
         ActionResult result = View(EDIT_RESULT_VIEW);
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("weixin_response_text", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id, txtContent)) {
               JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("修改文本回复成功！", "../text_list/index");
         }
         else //添加
            {
            ChkAdminLevel("weixin_response_text", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd(txtContent)) {
               JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("添加文本回复成功！", "../text_list/index");
         }
         return result;
      }

      #region 绑定公众账户=============================
      private void TreeBind() {
         BLL.weixin_account bll = new BLL.weixin_account();
         DataTable dt = bll.GetList(0, "", "sort_id asc,id desc").Tables[0];
         List<SelectListItem> weixinListItems = new List<SelectListItem>() {
            new SelectListItem(){ Text="请选择公众账户", Value=""}
         };
         foreach (DataRow dr in dt.Rows) {
            weixinListItems.Add(new SelectListItem() { Text = dr["name"].ToString(), Value = dr["id"].ToString() });
         }
         ViewData["weixinListItems"] = weixinListItems;
      }
      #endregion

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
         BLL.weixin_request_rule bll = new BLL.weixin_request_rule();
         Model.weixin_request_rule model = bll.GetModel(_id);
         ViewData["model"] = model;
      }
      #endregion

      #region 增加操作=================================
      private bool DoAdd(string txtContent) {
         bool result = false;
         Model.weixin_request_rule model = new Model.weixin_request_rule();
         BLL.weixin_request_rule bll = new BLL.weixin_request_rule();

         model.name = "文本回复";
         model.request_type = 1;
         model.response_type = 1;
         model.account_id = Utils.StrToInt(Request.Form["ddlAccountId"], 0);
         model.keywords = Request.Form["txtKeywords"].Trim();
         model.is_like_query = Utils.StrToInt(Request.Form["rblIsLikeQuery"], 0);
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
         List<Model.weixin_request_content> ls = new List<Model.weixin_request_content>();
         ls.Add(new Model.weixin_request_content() { account_id = model.account_id, content = txtContent.Trim() });
         model.contents = ls;

         if (bll.Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加微信文本回复，关健字:" + model.keywords); //记录日志
            result = true;
         }
         return result;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id, string txtContent) {
         bool result = false;
         BLL.weixin_request_rule bll = new BLL.weixin_request_rule();
         Model.weixin_request_rule model = bll.GetModel(_id);

         model.account_id = Utils.StrToInt(Request.Form["ddlAccountId"], 0);
         model.keywords = Request.Form["txtKeywords"].Trim();
         model.is_like_query = Utils.StrToInt(Request.Form["rblIsLikeQuery"], 0);
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
         List<Model.weixin_request_content> ls = new List<Model.weixin_request_content>();
         ls.Add(new Model.weixin_request_content() { account_id = model.account_id, rule_id = model.id, content = txtContent.Trim() });
         model.contents = ls;

         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改微信文本回复，关健字:" + model.keywords); //记录日志
            result = true;
         }
         return result;
      }
      #endregion
   }
}
