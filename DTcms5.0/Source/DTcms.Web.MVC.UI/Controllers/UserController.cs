using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Configuration;
using DTcms.Common;

namespace DTcms.Web.MVC.UI.Controllers {
   public partial class UserController : BaseController {
      public DTcms.Model.users userModel;
      public DTcms.Model.user_groups groupModel;

      protected override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         if (!IsUserLogin()) {
            //跳转URL
            filterContext.Result = Redirect(linkurl("login"));
            return;
         }
         //获得登录用户信息
         userModel = GetUserInfo();
         groupModel = new DTcms.BLL.user_groups().GetModel(userModel.group_id);
         if (groupModel == null) {
            groupModel = new DTcms.Model.user_groups();
         }
         ViewData["userModel"] = userModel;
         ViewData["groupModel"] = groupModel;
      }
   }
}