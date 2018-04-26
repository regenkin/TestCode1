using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class RegisterController : DTcms.Web.MVC.UI.Controllers.BaseController {
      public string action = string.Empty;
      public string username = string.Empty;

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         action = DTRequest.GetQueryString("action");
         username = DTRequest.GetQueryString("username");
         username = Utils.DropHTML(username);
         //检查是否关闭会员注册服务
         if (action == "" && uconfig.regstatus == 0) {
            Response.Redirect(linkurl("register", "?action=close"));
            return;
         }
         //Email验证
         if (action == "checkmail") {
            string code = DTRequest.GetQueryString("code");
            DTcms.BLL.user_code bll = new DTcms.BLL.user_code();
            DTcms.Model.user_code model = bll.GetModel(code);
            if (model == null) //返回出错
                {
               filterContext.Result = Redirect(linkurl("register", "?action=checkerror"));
               return;
            }
            //修改申请码状态
            model.status = 1;
            bll.Update(model);
            //修改用户状态
            new DTcms.BLL.users().UpdateField(model.user_id, "status=0");
         }
         ViewBag.Action = this.action;
         ViewBag.UserName = username;
      }
      //
      // GET: /Web/Register/

      public ActionResult Index() {
         return View(ViewName);
      }

   }
}
