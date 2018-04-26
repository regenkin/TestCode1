using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class Feedback_ReplyController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/plugins/feedback/Areas/admin/Views/feedback_reply.cshtml";
      private int id = 0;
      private DTcms.Web.Plugin.Feedback.BLL.feedback bll = new DTcms.Web.Plugin.Feedback.BLL.feedback();

      protected override void OnAuthorization(AuthorizationContext filterContext){
 	      base.OnAuthorization(filterContext);
         ChkAdminLevel("plugin_feedback", DTEnums.ActionEnum.View.ToString());
      }
      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         this.id = DTRequest.GetQueryInt("id");
         if (this.id == 0) {
            JscriptMsg("传输参数不正确！", "back");
            filterContext.Result = result;
            return;
         }
         else if (!bll.Exists(this.id)) {
            JscriptMsg("信息不存在或已被删除！", "back");
            filterContext.Result = result;
            return;
         }
         ViewBag.Id = this.id.ToString();
      }
      //
      // GET: /admin/Reply/

      public ActionResult Index() {
         ShowInfo(this.id);
         return View(WEB_VIEW);
      }

      [HttpPost, ValidateInput(false)]
      public ActionResult SubmitSave(string txtReContent){
          ChkAdminLevel("plugin_feedback", DTEnums.ActionEnum.Reply.ToString());
          DTcms.Web.Plugin.Feedback.Model.feedback model = bll.GetModel(this.id);
          model.reply_content = Utils.ToHtml(txtReContent);
          model.reply_time = DateTime.Now;
          bll.Update(model);
          AddAdminLog(DTEnums.ActionEnum.Reply.ToString(), "回复留言插件内容：" + model.title);
          ViewBag.ClientScript = JscriptMsg("留言回复成功！", "../feedback_list/index");
         return View(EDIT_RESULT_VIEW);
      }

      private void ShowInfo(int _id){
         DTcms.Web.Plugin.Feedback.Model.feedback model = bll.GetModel(_id);
         ViewData["model"] = model;
      }
   }
}
