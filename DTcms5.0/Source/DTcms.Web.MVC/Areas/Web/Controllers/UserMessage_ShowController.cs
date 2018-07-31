using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class UserMessage_ShowController : DTcms.Web.MVC.UI.Controllers.UserController {
      public int id;
      public DTcms.Model.user_message model = new DTcms.Model.user_message();

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         id = DTRequest.GetQueryInt("id");
         ViewBag.Id = this.id.ToString();
      }
      //
      // GET: /Web/Article/

      public ActionResult Index() {
         DTcms.BLL.user_message bll = new DTcms.BLL.user_message();
         if (!bll.Exists(id)) {
            return Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错了，您要浏览的页面不存在或已删除！")));
         }
         model = bll.GetModel(id);
         if (model.accept_user_name != userModel.user_name && model.post_user_name != userModel.user_name) {
            return Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错了，您所查看的并非自己的短消息！")));
         }
         //设为已阅读状态
         bll.UpdateField(id, "is_read=1,read_time='" + DateTime.Now + "'");
         ViewData["model"] = model;
         return View(ViewName);
      }

   }
}
