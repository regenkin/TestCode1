using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class Attribute_Field_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Channel/attribute_field_edit.cshtml";
      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;
      private string control_type = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel(" sys_channel_field", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         this.action = DTRequest.GetQueryString("action");
         this.control_type = DTRequest.GetQueryString("control_type");
         if (!string.IsNullOrEmpty(this.action) && this.action == DTEnums.ActionEnum.Edit.ToString()) {
            this.id = DTRequest.GetQueryInt("id");
            if (this.id == 0) {
               ViewBag.ClientScript = JscriptMsg("传输参数不正确！", "back");
               filterContext.Result = result;
               return;
            }
            if (!new DTcms.BLL.article_attribute_field().Exists(this.id)) {
               ViewBag.ClientScript = JscriptMsg("记录不存在或已被删除！", "back");
               filterContext.Result = result;
               return;
            }
         }
         ViewBag.Action = this.action;
         ViewBag.Id = this.id.ToString();
         ViewBag.ControlType = this.control_type;
      }
      //
      // GET: /admin/Attribute_Field_Edit/

      public ActionResult Index() {
         RptBind();
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ShowInfo(this.id);
         }
         else {
            DTcms.Model.article_attribute_field model = new DTcms.Model.article_attribute_field();
            model.control_type = this.control_type;
            ViewData["model"] = model;
         }
         return View(WEB_VIEW);
      }

      public ActionResult SubmitSave() {
         ActionResult result = View(EDIT_RESULT_VIEW);
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("sys_channel_field", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id)) {
               ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", "back");
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("修改扩展字段成功！", "../attribute_field_list/index");
         }
         else //添加
            {
            ChkAdminLevel("sys_channel_field", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd()) {
               ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", "back");
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("添加扩展字段成功！", "../attribute_field_list/index");
         }
         return result;
      }

      #region 数据绑定=================================
      private void RptBind() {
         List<SelectListItem> typeList = new List<SelectListItem>() {
              new SelectListItem(){ Text="所有类型", Value="" },
              new SelectListItem(){ Text="单行文本", Value="single-text" },
              new SelectListItem(){ Text="多行文本", Value="multi-text" },
              new SelectListItem(){ Text="编辑器", Value="editor" },
              new SelectListItem(){ Text="图片上传", Value="images" },
              new SelectListItem(){ Text="视频上传", Value="video" },
              new SelectListItem(){ Text="数字", Value="number" },
              new SelectListItem(){ Text="复选框", Value="checkbox" },
              new SelectListItem(){ Text="多项单选", Value="multi-radio" },
              new SelectListItem(){ Text="多项多选", Value="multi-checkbox" }
           };
         ViewData["typeList"] = typeList;

         List<SelectListItem> placeList = new List<SelectListItem>() {
            new SelectListItem(){ Text="0", Value="无小数点" },
            new SelectListItem(){ Text="1", Value="一位" },
            new SelectListItem(){ Text="2", Value="二位" },
            new SelectListItem(){ Text="3", Value="三位" },
            new SelectListItem(){ Text="4", Value="四位" },
            new SelectListItem(){ Text="5", Value="五位" },
         };
         ViewData["placeList"] = placeList;
      }
      #endregion

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
         DTcms.BLL.article_attribute_field bll = new DTcms.BLL.article_attribute_field();
         DTcms.Model.article_attribute_field model = bll.GetModel(_id);
         ViewData["model"] = model;
         //设置字段列明为只读
         ViewBag.ClientScript = "$('#txtName').attr('disabled', 'disabled').removeAttr('ajaxurl').removeAttr('datatype');";
      }
      #endregion

      #region 增加操作=================================
      private bool DoAdd() {
         bool result = false;
         DTcms.Model.article_attribute_field model = new DTcms.Model.article_attribute_field();
         DTcms.BLL.article_attribute_field bll = new DTcms.BLL.article_attribute_field();

         model.control_type = Request.Form["ddlControlType"];
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
         model.name = Request.Form["txtName"].Trim();
         model.title = Request.Form["txtTitle"];
         model.is_required = Request.Form["cbIsRequired"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.default_value = Request.Form["txtDefaultValue"].Trim();
         model.valid_tip_msg = Request.Form["txtValidTipMsg"].Trim();
         switch (model.control_type) {
            case "single-text":
               model.is_password = Request.Form["cbIsPassword"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
               model.valid_pattern = Request.Form["txtValidPattern"].Trim();
               break;
            case "multi-text":
               model.is_html = Request.Form["cbIsHtml"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
               model.valid_pattern = Request.Form["txtValidPattern"].Trim();
               break;
            case "editor":
               model.editor_type = Utils.StrToInt(Request.Form["rblEditorType"], 0);
               model.data_type = Request.Form["rblDataType"];
               break;
            case "multi-radio":
               model.data_length = Utils.StrToInt(Request.Form["txtDataLength"].Trim(), 0);
               model.item_option = Request.Form["txtItemOption"].Trim();
               //model.valid_error_msg = Request.Form["txtValidErrorMsg"].Trim();
               break;
            case "multi-checkbox":
               model.data_length = Utils.StrToInt(Request.Form["txtDataLength"].Trim(), 0);
               model.item_option = Request.Form["txtItemOption"].Trim();
               //model.valid_error_msg = Request.Form["txtValidErrorMsg"].Trim();
               break;
            case "number":
               model.data_place = Utils.StrToInt(Request.Form["ddlDataPlace"], 0);
               break;
            case "images":
               model.valid_pattern = Request.Form["txtValidPattern"].Trim();
               break;
            case "video":
               model.valid_pattern = Request.Form["txtValidPattern"].Trim();
               break;
            case "checkbox":
               model.valid_error_msg = Request.Form["txtValidErrorMsg"].Trim();
               break;
         }
         if (bll.Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加扩展字段:" + model.title); //记录日志
            result = true;
         }
         return result;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id) {
         bool result = false;
         DTcms.BLL.article_attribute_field bll = new DTcms.BLL.article_attribute_field();
         DTcms.Model.article_attribute_field model = bll.GetModel(_id);

         if (model.is_sys == 0) {
            model.control_type = Request.Form["ddlControlType"];
         }
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
         model.title = Request.Form["txtTitle"];
         model.is_required = Request.Form["cbIsRequired"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
         model.default_value = Request.Form["txtDefaultValue"].Trim();
         model.valid_tip_msg = Request.Form["txtValidTipMsg"].Trim();
         switch (model.control_type) {
            case "single-text":
               model.is_password = Request.Form["cbIsPassword"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
               model.valid_pattern = Request.Form["txtValidPattern"].Trim();
               break;
            case "multi-text":
               model.is_html = Request.Form["cbIsHtml"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
               model.valid_pattern = Request.Form["txtValidPattern"].Trim();
               break;
            case "editor":
               model.editor_type = Utils.StrToInt(Request.Form["rblEditorType"], 0);
               if (model.is_sys == 0) {
                  model.data_type = Request.Form["rblDataType"];
               }
               break;
            case "multi-radio":
               if (model.is_sys == 0) {
                  model.data_length = Utils.StrToInt(Request.Form["txtDataLength"].Trim(), 0);
               }
               model.item_option = Request.Form["txtItemOption"].Trim();
               //model.valid_error_msg = Request.Form["txtValidErrorMsg"].Trim();
               break;
            case "multi-checkbox":
               if (model.is_sys == 0) {
                  model.data_length = Utils.StrToInt(Request.Form["txtDataLength"].Trim(), 0);
               }
               model.item_option = Request.Form["txtItemOption"].Trim();
               //model.valid_error_msg = Request.Form["txtValidErrorMsg"].Trim();
               break;
            case "number":
               if (model.is_sys == 0) {
                  model.data_place = Utils.StrToInt(Request.Form["ddlDataPlace"], 0);
               }
               break;
            case "images":
               model.valid_pattern = Request.Form["txtValidPattern"].Trim();
               break;
            case "video":
               model.valid_pattern = Request.Form["txtValidPattern"].Trim();
               break;
            case "checkbox":
               model.valid_error_msg = Request.Form["txtValidErrorMsg"].Trim();
               break;
         }
         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "修改扩展字段:" + model.title); //记录日志
            result = true;
         }

         return result;
      }
      #endregion
   }
}
