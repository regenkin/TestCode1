using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Nav_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Settings/nav_list.cshtml";

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("sys_navigation", DTEnums.ActionEnum.View.ToString()); //检查权限
       }
        //
        // GET: /admin/Nav_List/

       public ActionResult Index() {
          RptBind();
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitSave() {
          ChkAdminLevel("sys_navigation", DTEnums.ActionEnum.Edit.ToString()); //检查权限
          DTcms.BLL.navigation bll = new DTcms.BLL.navigation();
          JObject jobject = JObject.Parse(Request.Form["json"]);
          JToken list = jobject["list"];
          foreach (JToken item in list) {
             int id = int.Parse(item["id"].ToString());
             int sortId = int.Parse(item["sortId"].ToString());
             bll.UpdateField(id, "sort_id=" + sortId);
          }
          AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存导航排序"); //记录日志
          string script = JscriptMsg("保存排序成功！", "index");
          return Content(script);
       }

       [HttpPost]
       public ActionResult SubmitDelete() {
          ChkAdminLevel("sys_navigation", DTEnums.ActionEnum.Delete.ToString()); //检查权限
          DTcms.BLL.navigation bll = new DTcms.BLL.navigation();
          JObject jobject = JObject.Parse(Request.Form["json"]);
          JToken list = jobject["list"];
          foreach (JToken item in list) {
             int id = int.Parse(item["id"].ToString());
             bll.Delete(id);
          }
          AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除导航菜单"); //记录日志
          string script = JscriptMsg("删除数据成功！", "index", "parent.loadMenuTree");
          return Content(script);
       }

        //数据绑定
        private void RptBind() {
           DTcms.BLL.navigation bll = new DTcms.BLL.navigation();
           DataTable dt = bll.GetList(0, DTEnums.NavigationEnum.System.ToString());
           ViewData["list"] = dt;
        }
    }
}
