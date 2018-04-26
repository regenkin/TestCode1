using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.weixin {
   public class Sound_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/weixin/sound_edit.cshtml";

      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("weixin_response_sound", DTEnums.ActionEnum.View.ToString()); //检查权限
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
            if (!new BLL.weixin_request_rule().Exists(this.id)) {
               JscriptMsg("记录不存在或已删除！", "back");
               filterContext.Result = result;
               return;
            }
         }
         ViewBag.Id = this.id.ToString();
         ViewBag.Action = this.action;
      }
      //
      // GET: /admin/Sound_Edit/

      public ActionResult Index() {
         TreeBind();
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ShowInfo(this.id);
         }
         else {
            DTcms.Model.weixin_request_rule model = new Model.weixin_request_rule();
            model.sort_id = 99;
            ViewData["model"] = model;
         }
         return View(WEB_VIEW);
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

      [HttpPost, ValidateInput(false)]
      public ActionResult SubmitSave(string txtContent) {
         ActionResult result = View(EDIT_RESULT_VIEW);
         string errmsg = string.Empty;
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("weixin_response_sound", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id, txtContent.Trim(), out errmsg)) {
               ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            JscriptMsg("修改语音回复成功！", "../sound_list/index");
         }
         else //添加
            {
            ChkAdminLevel("weixin_response_sound", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd(txtContent.Trim(), out errmsg)) {
               ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            JscriptMsg("添加语音回复成功！", "../sound_list/index");
         }
         JscriptMsg("保存错误：" + errmsg, string.Empty);
         return result;
      }

      #region 增加操作=================================
      private bool DoAdd(string txtContent, out string errmsg) {
         Model.weixin_request_rule model = new Model.weixin_request_rule();
         BLL.weixin_request_rule bll = new BLL.weixin_request_rule();

         model.name = "语音回复";
         model.request_type = 1;
         model.response_type = 3;
         model.account_id = Utils.StrToInt(Request.Form["ddlAccountId"], 0);
         model.keywords = Request.Form["txtKeywords"].Trim();
         model.is_like_query = Utils.StrToInt(Request.Form["rblIsLikeQuery"], 0);
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
         List<Model.weixin_request_content> ls = new List<Model.weixin_request_content>();
         //上传素材到微信服务器
         string mediaId = new API.Weixin.Common.CRMComm().UploadForeverMedia(model.account_id, Utils.GetMapPath(Request.Form["txtImgUrl"].Trim()), out errmsg);

         ls.Add(new Model.weixin_request_content() { 
            account_id = model.account_id, 
            title = Request.Form["txtTitle"].Trim(),
            img_url = Request.Form["txtImgUrl"].Trim(),
            media_id = mediaId,
            media_url = Request.Form["txtMediaUrl"].Trim(),
            meida_hd_url = Request.Form["txtMediaUrl"].Trim(),
            content = txtContent 
         });
         model.contents = ls;

         if (bll.Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加微信语音回复，关健字:" + model.keywords); //记录日志
            errmsg = string.Empty;
            return true;
         }
         errmsg = "保存过程中出错！";
         return false;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id, string txtContent, out string errmsg) {
         API.Weixin.Common.CRMComm wxComm = new API.Weixin.Common.CRMComm();
         BLL.weixin_request_rule bll = new BLL.weixin_request_rule();
         Model.weixin_request_rule model = bll.GetModel(_id);

         model.account_id = Utils.StrToInt(Request.Form["ddlAccountId"], 0);
         model.keywords = Request.Form["txtKeywords"].Trim();
         model.is_like_query = Utils.StrToInt(Request.Form["rblIsLikeQuery"], 0);
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);
         //判断是否需要重新上传素材
         string mediaId = model.contents[0].media_id;
         if (model.contents[0].img_url != Request.Form["txtImgUrl"].Trim()) {
            if (!string.IsNullOrEmpty(mediaId)) {
               wxComm.DeleteForeverMedia(model.account_id, mediaId, out errmsg); //删除永久素材
            }
            mediaId = wxComm.UploadForeverMedia(model.account_id, Utils.GetMapPath(Request.Form["txtImgUrl"].Trim()), out errmsg); //重新上传素材
         }

         List<Model.weixin_request_content> ls = new List<Model.weixin_request_content>();
         ls.Add(new Model.weixin_request_content() {
            account_id = model.account_id,
            rule_id = model.id,
            title = Request.Form["txtTitle"].Trim(),
            img_url = Request.Form["txtImgUrl"].Trim(),
            media_id = mediaId,
            media_url = Request.Form["txtMediaUrl"].Trim(),
            meida_hd_url = Request.Form["txtMediaUrl"].Trim(),
            content = txtContent.Trim()
         });
         model.contents = ls;

         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改微信语音回复，关健字:" + model.keywords); //记录日志
            errmsg = string.Empty;
            return true;
         }
         errmsg = "保存过程中出错！";
         return false;
      }
      #endregion
   }
}
