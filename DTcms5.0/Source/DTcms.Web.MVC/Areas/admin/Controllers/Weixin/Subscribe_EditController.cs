using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.weixin {
   public class Subscribe_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/weixin/subscribe_edit.cshtml";

      private string action = string.Empty; //消息类型
      private int id = 0; //公众账户ID

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.action = DTRequest.GetQueryString("action"); //获取消息类型
         this.id = DTRequest.GetQueryInt("id", 0); //获取公众账户ID
         if (action.ToLower() == "default") {
            ChkAdminLevel("weixin_subscribe_default", DTEnums.ActionEnum.View.ToString()); //检查权限
            ViewBag.RequestType = "0";//默认回复
            ViewBag.Position = "默认回复";
         }
         else if (action.ToLower() == "cancel") {
            ChkAdminLevel("weixin_subscribe_cancel", DTEnums.ActionEnum.View.ToString()); //检查权限
            ViewBag.RequestType = "7";//取消关注
            ViewBag.Position = "默认回复";
         }
         else {
            ChkAdminLevel("weixin_subscribe_subscribe", DTEnums.ActionEnum.View.ToString()); //检查权限
            ViewBag.RequestType = "6";//关注回复
            ViewBag.Position = "默认回复";
         }
         ViewBag.Id = this.id;
         ViewBag.Action = this.action;
      }

      //
      // GET: /admin/Subscribe_Edit/
      public ActionResult Index() {
         TreeBind(); //绑定公众账户
         if (this.id > 0) {
            ShowInfo(this.id);
         }
         return View(WEB_VIEW);
      }

      [HttpPost, ValidateInput(false)]
      public ActionResult SubmitSave(string txtContent) {
         ActionResult result = View(EDIT_RESULT_VIEW);
         //检查权限
         switch (this.action.ToLower()) {
            case "default":
               ChkAdminLevel("weixin_subscribe_default", DTEnums.ActionEnum.Edit.ToString());
               break;
            case "cancel":
               ChkAdminLevel("weixin_subscribe_cancel", DTEnums.ActionEnum.Edit.ToString());
               break;
            default:
               ChkAdminLevel("weixin_subscribe_subscribe", DTEnums.ActionEnum.Edit.ToString());
               break;
         }
         //开始保存数据
         string ruleName = string.Empty; //规格名称
         int ruleId = Utils.StrToInt(Request.Form["hideId"], 0); //规则ID
         int requestType = int.Parse(Request.Form["hideRequestType"]);//请求的类别
         int responseType = int.Parse(Request.Form["rblResponseType"]); //回复的类别
         if (requestType == 6) {
            ruleName = "关注时的触发内容";
         }
         else if (requestType == 0) {
            ruleName = "默认回复内容";
         }
         else if (requestType == 7) {
            ruleName = "取消关注时的触发内容";
         }
         Model.weixin_request_rule model = new BLL.weixin_request_rule().GetModel(ruleId);
         if (model == null) {
            model = new Model.weixin_request_rule();
         }
         model.account_id = this.id;
         model.name = ruleName;
         model.request_type = requestType;
         model.is_default = 0;
         model.add_time = DateTime.Now;

         if (responseType == 0) //纯文本
            {
            if (txtContent.Trim().Length == 0) {
               JscriptMsg("回复内容不能为空！", string.Empty);
               return result;
            }
            model.response_type = 1;//回复的类型:文本1，图文2，语音3，视频4,第三方接口5
            List<Model.weixin_request_content> ls = new List<Model.weixin_request_content>();
            ls.Add(new Model.weixin_request_content() { account_id = this.id, rule_id = ruleId, content = txtContent.Trim() });
            model.contents = ls;
         }
         else if (Request.Form["rblResponseType"] == "1") //图文
            {
            model.response_type = 2;//回复的类型:文本1，图文2，语音3，视频4,第三方接口5

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
                     modelt.account_id = this.id;
                     modelt.rule_id = ruleId;
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
         }
         else if (Request.Form["rblResponseType"] == "2") //语音
            {
            if (Request.Form["txtSoundTitle"].Trim().Length == 0) {
               JscriptMsg("语音标题不能为空！", string.Empty);
               return result;
            }
            if (Request.Form["txtSoundUrl"].Trim().Length == 0) {
               JscriptMsg("文件地址不能为空！", string.Empty);
               return result;
            }
            model.response_type = 3;//回复的类型:文本1，图文2，语音3，视频4,第三方接口5
            List<Model.weixin_request_content> ls = new List<Model.weixin_request_content>();
            ls.Add(new Model.weixin_request_content() {
               account_id = this.id,
               rule_id = ruleId,
               title = Request.Form["txtSoundTitle"].Trim(),
               media_url = Request.Form["txtSoundUrl"].Trim(),
               content = Request.Form["txtSoundContent"].Trim()
            });
            model.contents = ls;
         }
         //判断是新增还是修改
         if (model.id > 0 && new BLL.weixin_request_rule().Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "新增微信" + ruleName); //记录日志
            JscriptMsg("编辑" + ruleName + "成功！", "index?action=" + this.action + "&id=" + this.id);
            return result;
         }
         else if (new BLL.weixin_request_rule().Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "编辑微信" + ruleName); //记录日志
            JscriptMsg("新增" + ruleName + "成功！", "index?action=" + this.action + "&id=" + this.id);
            return result;
         }
         JscriptMsg("保存" + ruleName + "失败！", "back");
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
      private void ShowInfo(int account_id) {
         string requestType = DTRequest.GetQueryString("hideRequestType");
         Model.weixin_request_rule ruleModel = new BLL.weixin_request_rule().GetModel(account_id, Utils.StrToInt(requestType, 0)); //获取规则
         ViewData["model"] = ruleModel;
      }
      #endregion

   }
}
