using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class Comment_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Article/comment_edit.cshtml";
      private int id = 0;
      private string channel_name = string.Empty; //频道名称
      protected DTcms.Model.article_comment model = new DTcms.Model.article_comment();

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         this.id = DTRequest.GetQueryInt("id");
         if (id == 0) {
            JscriptMsg("传输参数不正确！", "back");
            filterContext.Result = result;
            return;
         }
         if (!new DTcms.BLL.article_comment().Exists(this.id)) {
            JscriptMsg("记录不存在或已删除！", "back");
            filterContext.Result = result;
            return;
         }
         ViewBag.Id = this.id.ToString();
      }
      //
      // GET: /admin/Comment_Edit/

      public ActionResult Index() {
         ShowInfo();
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ChkAdminLevel("channel_" + this.channel_name + "_comment", DTEnums.ActionEnum.Reply.ToString()); //检查权限
         DTcms.BLL.article_comment bll = new DTcms.BLL.article_comment();
         model.is_reply = 1;
         model.reply_content = Utils.ToHtml(Request.Form["txtReContent"]);
         model.is_lock = int.Parse(Request.Form["rblIsLock"]);
         model.reply_time = DateTime.Now;
         bll.Update(model);
         AddAdminLog(DTEnums.ActionEnum.Reply.ToString(), "回复" + this.channel_name + "频道评论ID:" + model.id); //记录日志
         JscriptMsg("评论回复成功！", "../comment_list/index?channel_id=" + model.channel_id);
         return View(EDIT_RESULT_VIEW);
      }

      #region 赋值操作=================================
      private void ShowInfo() {
         this.model = new DTcms.BLL.article_comment().GetModel(this.id); //取得评论实体
         this.channel_name = new DTcms.BLL.site_channel().GetChannelName(model.channel_id); //取得频道名称
         ViewData["model"] = this.model;
      }
      #endregion
   }
}
