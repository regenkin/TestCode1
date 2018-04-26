using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.weixin {
   public class Picture_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/weixin/picture_edit.cshtml";

      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("weixin_response_picture", DTEnums.ActionEnum.View.ToString()); //检查权限
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
         ViewBag.This = this;
         ViewBag.Action = this.action;
      }
      //
      // GET: /admin/Picture_Edit/
      public ActionResult Index() {
         TreeBind();
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ShowInfo(this.id);
         }
         else {
            Model.weixin_request_rule model = new Model.weixin_request_rule();
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
      public ActionResult SubmitSave() {
         ActionResult result = View(EDIT_RESULT_VIEW);
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("weixin_response_picture", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id)) {
               ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("修改图文回复成功！", "../picture_list/index");
         }
         else //添加
            {
            ChkAdminLevel("weixin_response_picture", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd()) {
               ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            ViewBag.ClientScript = JscriptMsg("添加图文回复成功！", "../picture_list/index");
         }
         return result;
      }

      #region 增加操作=================================
      private bool DoAdd() {
         bool result = false;
         Model.weixin_request_rule model = new Model.weixin_request_rule();
         BLL.weixin_request_rule bll = new BLL.weixin_request_rule();

         model.name = "图文回复";
         model.request_type = 1; //关键词回复
         model.response_type = 2; //回复的类型:文本1，图文2，语音3，视频4,第三方接口5
         model.account_id = Utils.StrToInt(Request.Form["ddlAccountId"], 0);
         model.keywords = Request.Form["txtKeywords"].Trim();
         model.is_like_query = Utils.StrToInt(Request.Form["rblIsLikeQuery"], 0);
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         #region 赋值规则图片
         string[] itemIdArr = Request.Form.GetValues("item_id");
         string[] itemTitleArr = Request.Form.GetValues("item_title");
         string[] itemContentArr = Request.Form.GetValues("item_content");
         string[] itemImgUrlArr = Request.Form.GetValues("item_imgurl");
         string[] itemLinkUrlArr = Request.Form.GetValues("item_linkurl");
         string[] itemSortIdArr = Request.Form.GetValues("item_sortid");
         if (itemIdArr != null && itemTitleArr != null && itemImgUrlArr != null && itemLinkUrlArr != null && itemSortIdArr != null && itemContentArr != null) {
            if ((itemIdArr.Length == itemTitleArr.Length) && (itemTitleArr.Length == itemImgUrlArr.Length) && (itemImgUrlArr.Length == itemLinkUrlArr.Length)
                && (itemLinkUrlArr.Length == itemSortIdArr.Length) && (itemSortIdArr.Length == itemContentArr.Length)) {
               List<Model.weixin_request_content> ls = new List<Model.weixin_request_content>();
               for (int i = 0; i < itemIdArr.Length; i++) {
                  Model.weixin_request_content modelt = new Model.weixin_request_content();
                  modelt.id = Utils.StrToInt(itemIdArr[i].Trim(), 0);
                  modelt.account_id = model.account_id;
                  modelt.title = itemTitleArr[i].Trim();
                  modelt.content = itemContentArr[i].Trim();
                  modelt.img_url = itemImgUrlArr[i].Trim();
                  modelt.link_url = itemLinkUrlArr[i].Trim();
                  modelt.sort_id = Utils.StrToInt(itemSortIdArr[i].Trim(), 99);
                  ls.Add(modelt);
               }
               model.contents = ls;
            }
         }
         else {
            model.contents = null;
         }
         #endregion

         if (bll.Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加微信图文回复，关健字:" + model.keywords); //记录日志
            result = true;
         }
         return result;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id) {
         bool result = false;
         BLL.weixin_request_rule bll = new BLL.weixin_request_rule();
         Model.weixin_request_rule model = bll.GetModel(_id);

         model.account_id = Utils.StrToInt(Request.Form["ddlAccountId"], 0);
         model.keywords = Request.Form["txtKeywords"].Trim();
         model.is_like_query = Utils.StrToInt(Request.Form["rblIsLikeQuery"], 0);
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         #region 赋值规则图片
         string[] itemIdArr = Request.Form.GetValues("item_id");
         string[] itemTitleArr = Request.Form.GetValues("item_title");
         string[] itemContentArr = Request.Form.GetValues("item_content");
         string[] itemImgUrlArr = Request.Form.GetValues("item_imgurl");
         string[] itemLinkUrlArr = Request.Form.GetValues("item_linkurl");
         string[] itemSortIdArr = Request.Form.GetValues("item_sortid");
         if (itemIdArr != null && itemTitleArr != null && itemImgUrlArr != null && itemLinkUrlArr != null && itemSortIdArr != null && itemContentArr != null) {
            if ((itemIdArr.Length == itemTitleArr.Length) && (itemTitleArr.Length == itemImgUrlArr.Length) && (itemImgUrlArr.Length == itemLinkUrlArr.Length)
                && (itemLinkUrlArr.Length == itemSortIdArr.Length) && (itemSortIdArr.Length == itemContentArr.Length)) {
               List<Model.weixin_request_content> ls = new List<Model.weixin_request_content>();
               for (int i = 0; i < itemIdArr.Length; i++) {
                  Model.weixin_request_content modelt = new Model.weixin_request_content();
                  modelt.id = Utils.StrToInt(itemIdArr[i].Trim(), 0);
                  modelt.rule_id = model.id;
                  modelt.account_id = model.account_id;
                  modelt.title = itemTitleArr[i].Trim();
                  modelt.content = itemContentArr[i].Trim();
                  modelt.img_url = itemImgUrlArr[i].Trim();
                  modelt.link_url = itemLinkUrlArr[i].Trim();
                  modelt.sort_id = Utils.StrToInt(itemSortIdArr[i].Trim(), 99);
                  ls.Add(modelt);
               }
               model.contents = ls;
            }
         }
         else {
            model.contents = null;
         }
         #endregion

         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改微信图文回复，关健字:" + model.keywords); //记录日志
            result = true;
         }
         return result;
      }
      #endregion
   }
}
