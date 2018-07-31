using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class UserAddress_EditController : DTcms.Web.MVC.UI.Controllers.UserController {
      protected string action;
      protected int id;
      protected Model.user_addr_book model = new Model.user_addr_book();

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         action = DTRequest.GetQueryString("action");
         id = DTRequest.GetQueryInt("id");
         if (action.ToLower() == DTEnums.ActionEnum.Edit.ToString().ToLower()) {
            BLL.user_addr_book bll = new BLL.user_addr_book();
            if (!bll.Exists(id)) {
               filterContext.Result = Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错了，您要浏览的页面不存在或已删除！")));
               return;
            }
            model = bll.GetModel(id);
            if (model.user_id != userModel.id) {
               filterContext.Result = Redirect(linkurl("error", "error.aspx?msg=" + Utils.UrlEncode("出错了，您所要修改的并非自己的地址！")));
               return;
            }
         }
      }
      //
      // GET: /Web/UserAddress_Edit/

      public ActionResult Index() {
         return View(ViewName);
      }

   }
}
