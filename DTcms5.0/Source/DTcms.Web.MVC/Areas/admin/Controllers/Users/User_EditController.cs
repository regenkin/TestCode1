using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class User_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Users/User_Edit.cshtml";
      string defaultpassword = "0|0|0|0"; //默认显示密码
      protected string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("user_list", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ViewBag.defaultpassowrd = defaultpassword;
         string _action = DTRequest.GetQueryString("action");
         if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
            this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
            this.id = DTRequest.GetQueryInt("id");
            if (this.id == 0) {
               JscriptMsg("传输参数不正确！", "back");
               filterContext.Result = View(EDIT_RESULT_VIEW);
               return;
            }
            if (!new DTcms.BLL.users().Exists(this.id)) {
               JscriptMsg("信息不存在或已被删除！", "back");
               filterContext.Result = View(EDIT_RESULT_VIEW);
               return;
            }
         }
         ViewBag.Id = this.id;
         ViewBag.Action = this.action;
      }
      //
      // GET: /admin/User_Edit/

      public ActionResult Index() {
         SiteBind(); //绑定站点
         TreeBind("is_lock=0"); //绑定组别
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ShowInfo(this.id);
         }
         else {
            DTcms.Model.users model = new DTcms.Model.users();
            model.amount = 0;
            model.point = 0;
            model.exp = 0;
            model.reg_time = DateTime.Now;
            model.reg_ip = DTRequest.GetIP();
            model.birthday = new DateTime(1900, 1, 1);
            ViewData["model"] = model;
         }
         return View(WEB_VIEW);
      }

      #region 绑定站点=================================
      private void SiteBind() {
         BLL.sites bll = new BLL.sites();
         DataTable dt = bll.GetList(0, "is_lock=0", "sort_id asc,id desc").Tables[0];
         List<SelectListItem> siteListItems = new List<SelectListItem>(){
            new SelectListItem(){ Text="请选择站点...", Value=""}
         };
         foreach (DataRow dr in dt.Rows) {
            siteListItems.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
         }
         ViewData["siteListItems"] = siteListItems;
      }
      #endregion

      #region 绑定组别=================================
      private void TreeBind(string strWhere) {
         DTcms.BLL.user_groups bll = new DTcms.BLL.user_groups();
         DataTable dt = bll.GetList(0, strWhere, "grade asc,id asc").Tables[0];
         List<SelectListItem> groupListItems = new List<SelectListItem>();
         groupListItems.Add(new SelectListItem() { Text = "请选择组别...", Value = "" });
         foreach (DataRow dr in dt.Rows) {
            groupListItems.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
         }
         ViewData["groupListItems"] = groupListItems;
      }
      #endregion

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
         DTcms.BLL.users bll = new DTcms.BLL.users();
         DTcms.Model.users model = bll.GetModel(_id);
         if(!string.IsNullOrEmpty(model.password)){
            model.password = defaultpassword;
         }
         ViewData["model"] = model;
         //查找最近登录信息
         DTcms.Model.user_login_log logModel = new DTcms.BLL.user_login_log().GetLastModel(model.user_name);
         ViewData["logModel"] = logModel;
      }
      #endregion

      #region 增加操作=================================
      private bool DoAdd() {
         bool result = false;
         DTcms.Model.users model = new DTcms.Model.users();
         DTcms.BLL.users bll = new DTcms.BLL.users();

         model.site_id = int.Parse(Request.Form["ddlSiteId"]);
         model.group_id = int.Parse(Request.Form["ddlGroupId"]);
         model.status = int.Parse(Request.Form["rblStatus"]);
         //检测用户名是否重复
         if (bll.Exists(Request.Form["txtUserName"].Trim())) {
            return false;
         }
         model.user_name = Utils.DropHTML(Request.Form["txtUserName"].Trim());
         //获得6位的salt加密字符串
         model.salt = Utils.GetCheckCode(6);
         //以随机生成的6位字符串做为密钥加密
         model.password = DESEncrypt.Encrypt(Request.Form["txtPassword"].Trim(), model.salt);
         model.email = Utils.DropHTML(Request.Form["txtEmail"]);
         model.nick_name = Utils.DropHTML(Request.Form["txtNickName"]);
         model.avatar = Utils.DropHTML(Request.Form["txtAvatar"]);
         model.sex = Request.Form["rblSex"];
         DateTime _birthday;
         if (DateTime.TryParse(Request.Form["txtBirthday"].Trim(), out _birthday)) {
            model.birthday = _birthday;
         }
         model.telphone = Utils.DropHTML(Request.Form["txtTelphone"].Trim());
         model.mobile = Utils.DropHTML(Request.Form["txtMobile"].Trim());
         model.qq = Utils.DropHTML(Request.Form["txtQQ"]);
         model.msn = Utils.DropHTML(Request.Form["txtMsn"]);
         model.address = Utils.DropHTML(Request.Form["txtAddress"].Trim());
         model.amount = decimal.Parse(Request.Form["txtAmount"].Trim());
         model.point = int.Parse(Request.Form["txtPoint"].Trim());
         model.exp = int.Parse(Request.Form["txtExp"].Trim());
         model.reg_time = DateTime.Now;
         model.reg_ip = DTRequest.GetIP();

         if (bll.Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加用户:" + model.user_name); //记录日志
            result = true;
         }
         return result;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id) {
         bool result = false;
         DTcms.BLL.users bll = new DTcms.BLL.users();
         DTcms.Model.users model = bll.GetModel(_id);

         model.site_id = int.Parse(Request.Form["ddlSiteId"]);
         model.group_id = int.Parse(Request.Form["ddlGroupId"]);
         model.status = int.Parse(Request.Form["rblStatus"]);
         //判断密码是否更改
         if (Request.Form["txtPassword"].Trim() != defaultpassword) {
            //获取用户已生成的salt作为密钥加密
            model.password = DESEncrypt.Encrypt(Request.Form["txtPassword"].Trim(), model.salt);
         }
         model.email = Utils.DropHTML(Request.Form["txtEmail"]);
         model.nick_name = Utils.DropHTML(Request.Form["txtNickName"]);
         model.avatar = Utils.DropHTML(Request.Form["txtAvatar"]);
         model.sex = Request.Form["rblSex"];
         DateTime _birthday;
         if (DateTime.TryParse(Request.Form["txtBirthday"].Trim(), out _birthday)) {
            model.birthday = _birthday;
         }
         model.telphone = Utils.DropHTML(Request.Form["txtTelphone"].Trim());
         model.mobile = Utils.DropHTML(Request.Form["txtMobile"].Trim());
         model.qq = Utils.DropHTML(Request.Form["txtQQ"]);
         model.msn = Utils.DropHTML(Request.Form["txtMsn"]);
         model.address = Utils.DropHTML(Request.Form["txtAddress"].Trim());
         model.amount = Utils.StrToDecimal(Request.Form["txtAmount"].Trim(), 0);
         model.point = Utils.StrToInt(Request.Form["txtPoint"].Trim(), 0);
         model.exp = Utils.StrToInt(Request.Form["txtExp"].Trim(), 0);

         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改用户信息:" + model.user_name); //记录日志
            result = true;
         }
         return result;
      }
      #endregion

      //保存
      [HttpPost]
      public ActionResult SubmitSave() {
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("user_list", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id)) {
               JscriptMsg("保存过程中发生错误！", "");
            }
            else {
               JscriptMsg("修改会员成功！", "../user_list/index");
            }
         }
         else //添加
            {
            ChkAdminLevel("user_list", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd()) {
               JscriptMsg("保存过程中发生错误！", "");
            }
            else {
               JscriptMsg("添加会员成功！", "../user_list/index");
            }
         }
         return View(EDIT_RESULT_VIEW);
      }
   }
}
