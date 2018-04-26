using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Collections;
using System.Data;
using System.Text;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class User_SmsController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Users/User_Sms.cshtml";
       string mobiles = string.Empty;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_sms", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          mobiles = DTRequest.GetString("mobiles");
          ViewBag.Mobiles = mobiles;
       }
        //
        // GET: /admin/User_Sms/

       public ActionResult Index() {
          ShowInfo(mobiles);
          TreeBind("is_lock=0"); //绑定类别
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitSave() {
          ActionResult result = View(EDIT_RESULT_VIEW);
          ChkAdminLevel("user_sms", DTEnums.ActionEnum.Add.ToString()); //检查权限
          //检查短信内容
          if (Request.Form["txtSmsContent"].Trim() == "") {
             JscriptMsg("请输入短信内容！", "back");
             return result;
          }
          //检查发送类型
          if (Request.Form["rblSmsType"] == "1") {
             if (Request.Form["txtMobileNumbers"].Trim() == "") {
                JscriptMsg("请输入手机号码！", "back");
                return result;
             }
             //开始发送短信
             string msg = string.Empty;
             bool _result = new DTcms.BLL.sms_message().Send(Request.Form["txtMobileNumbers"].Trim(), Request.Form["txtSmsContent"].Trim(), 2, out msg);
             if (_result) {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "发送手机短信"); //记录日志
                JscriptMsg(msg, "../user_list/index");
                return result;
             }
             JscriptMsg(msg, "");
             return result;
          }
          else {
             ArrayList al = new ArrayList();
             DTcms.BLL.user_groups bll = new DTcms.BLL.user_groups();
             DataTable dt = bll.GetList(0, "is_lock=0", "grade asc,id asc").Tables[0];
             foreach (DataRow dr in dt.Rows) {
                string v = Request.Form["cblGroupId_" + dr["id"].ToString()];
                if (v.ToLower().IndexOf("true") >= 0)
                   al.Add(dr["id"].ToString());
             }
             if (al.Count < 1) {
                JscriptMsg("请选择会员组别！", "back");
                return result;
             }
             string _mobiles = GetGroupMobile(al);
             //开始发送短信
             string msg = string.Empty;
             bool _result = new DTcms.BLL.sms_message().Send(_mobiles, Request.Form["txtSmsContent"].Trim(), 2, out msg);
             if (_result) {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "发送手机短信"); //记录日志
                JscriptMsg(msg, "index");
                return result;
             }
             JscriptMsg(msg, "back");
             return result;
          }
       }

       #region 绑定类别=================================
       private void TreeBind(string strWhere) {
          DTcms.BLL.user_groups bll = new DTcms.BLL.user_groups();
          DataTable dt = bll.GetList(0, strWhere, "grade asc,id asc").Tables[0];
          Dictionary<string, string> dict = new Dictionary<string, string>();
          foreach (DataRow dr in dt.Rows) {
             dict.Add(dr["title"].ToString(), dr["id"].ToString());
          }
          ViewData["dict"] = dict;
       }
       #endregion

       #region 赋值操作=================================
       private void ShowInfo(string _mobiles) {
          if (!string.IsNullOrEmpty(_mobiles)) {
             ViewBag.SmsType = "1";
             ViewBag.MobileNumbers = _mobiles;
          }
          else {
             ViewBag.SmsType = "2";
          }
       }
       #endregion

       #region 返回会员组所有手机号码===================
       private string GetGroupMobile(ArrayList al) {
          StringBuilder str = new StringBuilder();
          foreach (Object obj in al) {
             DataTable dt = new DTcms.BLL.users().GetList(0, "group_id=" + Convert.ToInt32(obj), "reg_time desc,id desc").Tables[0];
             foreach (DataRow dr in dt.Rows) {
                if (!string.IsNullOrEmpty(dr["mobile"].ToString())) {
                   str.Append(dr["mobile"].ToString() + ",");
                }
             }
          }
          return Utils.DelLastComma(str.ToString());
       }
       #endregion
    }
}
