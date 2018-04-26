using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Manager_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Manager/Manager_Edit.cshtml";
       string defaultpassword = "0|0|0|0"; //默认显示密码
       private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
       private int id = 0;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("manager_list", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          ActionResult result = View(EDIT_RESULT_VIEW);
          string action = DTRequest.GetQueryString("action");
          if (!string.IsNullOrEmpty(action) && action == DTEnums.ActionEnum.Edit.ToString()) {
             this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
             if (!int.TryParse(Request.QueryString["id"] as string, out this.id)) {
                ViewBag.ClientScript = JscriptMsg("传输参数不正确！", "back");
                filterContext.Result = result;
                return;
             }
             if (!new DTcms.BLL.manager().Exists(this.id)) {
                ViewBag.ClientScript = JscriptMsg("记录不存在或已被删除！", "back");
                filterContext.Result = result;
                return;
             }
          }
          ViewBag.Action = action;
          ViewBag.DefaultPassword = defaultpassword;
          ViewBag.Id = this.id.ToString();
       }
        //
        // GET: /admin/Manager_Edit/

        public ActionResult Index()
        {
          DTcms.Model.manager model = GetAdminInfo(); //取得管理员信息
          RoleBind(model.role_type);
          if (action == DTEnums.ActionEnum.Edit.ToString()) {
             ShowInfo(this.id);
          }
          else {
             ViewData["model"] = new DTcms.Model.manager();
          }
            return View(WEB_VIEW);
        }

        [HttpPost]
        public ActionResult SubmitSave() {
           ActionResult result = View(EDIT_RESULT_VIEW);
           if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
              ChkAdminLevel("manager_list", DTEnums.ActionEnum.Edit.ToString()); //检查权限
              if (!DoEdit(this.id)) {
                 ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", "back");
                 return result;
              }
              ViewBag.ClientScript = JscriptMsg("修改管理员信息成功！", "../manager_list/index");
           }
           else //添加
            {
              ChkAdminLevel("manager_list", DTEnums.ActionEnum.Add.ToString()); //检查权限
              if (!DoAdd()) {
                 ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", "back");
                 return result;
              }
              ViewBag.ClientScript = JscriptMsg("添加管理员信息成功！", "../manager_list/index");
           }
           return result;
        }

        #region 角色类型=================================
        private void RoleBind(int role_type) {
           DTcms.BLL.manager_role bll = new DTcms.BLL.manager_role();
           DataTable dt = bll.GetList("").Tables[0];
           List<SelectListItem> list = new List<SelectListItem>();
           list.Add(new SelectListItem() { Text = "请选择角色...", Value = "" });
           foreach (DataRow dr in dt.Rows) {
              if (Convert.ToInt32(dr["role_type"]) >= role_type) {
                 list.Add(new SelectListItem() { Text = dr["role_name"].ToString(), Value = dr["id"].ToString() });
              }
           }
           ViewData["list"] = list;
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id) {
           DTcms.BLL.manager bll = new DTcms.BLL.manager();
           DTcms.Model.manager model = bll.GetModel(_id);
           ViewData["model"] = model;
           ViewBag.ClientScript = "$('#txtUserName').removeAttr('ajaxurl').attr('disabled','disabled');";
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd() {
           DTcms.Model.manager model = new DTcms.Model.manager();
           DTcms.BLL.manager bll = new DTcms.BLL.manager();
           model.role_id = int.Parse(Request.Form["ddlRoleId"]);
           model.role_type = new DTcms.BLL.manager_role().GetModel(model.role_id).role_type;
           if (Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0) {
              model.is_lock = 0;
           }
           else {
              model.is_lock = 1;
           }
           //检测用户名是否重复
           if (bll.Exists(Request.Form["txtUserName"].Trim())) {
              return false;
           }
           model.user_name = Request.Form["txtUserName"].Trim();
           //获得6位的salt加密字符串
           model.salt = Utils.GetCheckCode(6);
           //以随机生成的6位字符串做为密钥加密
           model.password = DESEncrypt.Encrypt(Request.Form["txtPassword"].Trim(), model.salt);
           model.real_name = Request.Form["txtRealName"].Trim();
           model.telephone = Request.Form["txtTelephone"].Trim();
           model.email = Request.Form["txtEmail"].Trim();
           model.add_time = DateTime.Now;

           if (bll.Add(model) > 0) {
              AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加管理员:" + model.user_name); //记录日志
              return true;
           }
           return false;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id) {
           bool result = false;
           DTcms.BLL.manager bll = new DTcms.BLL.manager();
           DTcms.Model.manager model = bll.GetModel(_id);

           model.role_id = int.Parse(Request.Form["ddlRoleId"]);
           model.role_type = new DTcms.BLL.manager_role().GetModel(model.role_id).role_type;
           if (Request.Form["cbIsLock"].ToLower().IndexOf("true") >= 0) {
              model.is_lock = 0;
           }
           else {
              model.is_lock = 1;
           }
           //判断密码是否更改
           if (Request.Form["txtPassword"].Trim() != defaultpassword) {
              //获取用户已生成的salt作为密钥加密
              model.password = DESEncrypt.Encrypt(Request.Form["txtPassword"].Trim(), model.salt);
           }
           model.real_name = Request.Form["txtRealName"].Trim();
           model.telephone = Request.Form["txtTelephone"].Trim();
           model.email = Request.Form["txtEmail"].Trim();

           if (bll.Update(model)) {
              AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改管理员:" + model.user_name); //记录日志
              result = true;
           }

           return result;
        }
        #endregion
    }
}
