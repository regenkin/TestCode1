using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Role_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Manager/Role_Edit.cshtml";
       private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
       private int id = 0;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("manager_role", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          ActionResult result= View(EDIT_RESULT_VIEW);
          string _action = DTRequest.GetQueryString("action");
          this.id = DTRequest.GetQueryInt("id");

          if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
             this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
             if (this.id == 0) {
                ViewBag.ClientScript = JscriptMsg("传输参数不正确！", "back");
                filterContext.Result = result; 
                return;
             }
             if (!new DTcms.BLL.manager_role().Exists(this.id)) {
                ViewBag.ClientScript = JscriptMsg("角色不存在或已被删除！", "back");
                filterContext.Result = result;
                return;
             }
          }
          ViewBag.Action = this.action;
          ViewBag.Id = this.id.ToString();
       }
        //
        // GET: /admin/Role_Edit/

        public ActionResult Index()
        {
          RoleTypeBind(); //绑定角色类型
          NavBind(); //绑定导航
          if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
             ShowInfo(this.id);
          }
          else {
             ViewData["model"] = new DTcms.Model.manager_role();
          }
            return View(WEB_VIEW);
        }

        [HttpPost]
        public ActionResult SubmitSave() {
           string result = string.Empty;
           if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
              ChkAdminLevel("manager_role", DTEnums.ActionEnum.Edit.ToString()); //检查权限
              if (!DoEdit(this.id)) {
                 result = JscriptMsg("保存过程中发生错误！", "back");
                 return Content(result);
              }
              result = JscriptMsg("修改管理角色成功！", "../role_list/index");
           }
           else //添加
            {
              ChkAdminLevel("manager_role", DTEnums.ActionEnum.Add.ToString()); //检查权限
              if (!DoAdd()) {
                 ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", "back");
                 return Content(result);
              }
              result = JscriptMsg("添加管理角色成功！", "../role_list/index");
           }
           return Content(result);
        }

        #region 角色类型=================================
        private void RoleTypeBind() {
           DTcms.Model.manager model = GetAdminInfo();
           List<SelectListItem> roleList = new List<SelectListItem>();
           roleList.Add(new SelectListItem() { Text = "请选择类型...", Value = "" });
           if (model.role_type < 2) {
              roleList.Add(new SelectListItem() { Text = "超级用户", Value = "1" });
           }
           roleList.Add(new SelectListItem() { Text = "系统用户", Value = "2" });
           ViewData["roleList"] = roleList;
        }
        #endregion

        #region 导航菜单=================================
        private void NavBind() {
           DTcms.BLL.navigation bll = new DTcms.BLL.navigation();
           DataTable dt = bll.GetList(0, DTEnums.NavigationEnum.System.ToString());
           ViewData["list"] = dt;
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id) {
           DTcms.BLL.manager_role bll = new DTcms.BLL.manager_role();
           DTcms.Model.manager_role model = bll.GetModel(_id);
           ViewData["model"] = model;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd() {
           bool result = false;
           DTcms.Model.manager_role model = new DTcms.Model.manager_role();
           DTcms.BLL.manager_role bll = new DTcms.BLL.manager_role();

           model.role_name = DTRequest.GetQueryString("role_name");
           model.role_type = int.Parse(DTRequest.GetQueryString("role_type"));
           string json = DTRequest.GetFormString("json");
           JObject jobject = JObject.Parse(json);
           JToken record = jobject["list"];
           //管理权限
           List<DTcms.Model.manager_role_value> ls = new List<DTcms.Model.manager_role_value>();
           foreach (JToken item in record) {
              JArray action_types = JArray.FromObject(item["action_type"]);
              foreach (JToken action in action_types) {
                 ls.Add(new DTcms.Model.manager_role_value { nav_name = item["name"].ToString(), action_type = action.ToString() });
              }
           }
           model.manager_role_values = ls;

           if (bll.Add(model) > 0) {
              AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加管理角色:" + model.role_name); //记录日志
              result = true;
           }
           return result;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id) {
           bool result = false;
           DTcms.BLL.manager_role bll = new DTcms.BLL.manager_role();
           DTcms.Model.manager_role model = bll.GetModel(_id);
           
           model.role_name = DTRequest.GetQueryString("role_name");
           model.role_type = int.Parse(DTRequest.GetQueryString("role_type"));
           string json = DTRequest.GetFormString("json");
           JObject jobject = JObject.Parse(json);
           JToken record = jobject["list"];
           //管理权限
           List<DTcms.Model.manager_role_value> ls = new List<DTcms.Model.manager_role_value>();
           foreach (JToken item in record) {
              JArray action_types = JArray.FromObject(item["action_type"]);
              foreach (JToken action in action_types) {
                 ls.Add(new DTcms.Model.manager_role_value { nav_name = item["name"].ToString(), action_type = action.ToString() });
              }
           }
           model.manager_role_values = ls;

           if (bll.Update(model)) {
              AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改管理角色:" + model.role_name); //记录日志
              result = true;
           }
           return result;
        }
        #endregion
    }
}
