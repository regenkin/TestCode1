using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class FeedbackController : DTcms.Web.MVC.UI.Controllers.BaseController {
      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected string keywords = string.Empty;

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         if (int.TryParse(DTRequest.GetQueryString("pageNum"), out this.pageSize)) {
            if (this.pageSize > 0) {
               Utils.WriteCookie("feedback_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
            }
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }
         this.page = DTRequest.GetQueryInt("page", 1);
         //绑定页码
         ViewBag.txtPageNum = this.pageSize.ToString();
         ViewBag.Page = this.page.ToString();
         ViewBag.This = this;
      }
      //
      // GET: /Web/Feedback/

      public ActionResult Index() {
         return View(ViewName);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         string action = DTRequest.GetQueryString("action");
         if (action != DTEnums.ActionEnum.Add.ToString().ToLower()) {
            return Content("{\"status\":0, \"msg\":\"对不起，网站传输参数有误！\"}");
         }
         string result = string.Empty;
         StringBuilder builder = new StringBuilder();
         DTcms.BLL.feedback feedback = new DTcms.BLL.feedback();
         DTcms.Model.feedback model = new DTcms.Model.feedback();
         string queryString = DTRequest.GetQueryString("site");
         string formString = DTRequest.GetFormString("txtCode");
         string str3 = DTRequest.GetFormString("txtTitle");
         string str4 = DTRequest.GetFormString("txtContent");
         string str5 = DTRequest.GetFormString("txtUserName");
         string str6 = DTRequest.GetFormString("txtUserTel");
         string str7 = DTRequest.GetFormString("txtUserQQ");
         string str8 = DTRequest.GetFormString("txtUserEmail");
         if (string.IsNullOrEmpty(queryString)) {
            result = "{\"status\":0, \"msg\":\"对不起，网站传输参数有误！\"}";
         }
         else if (string.IsNullOrEmpty(formString)) {
            result = "{\"status\":0, \"msg\":\"对不起，请输入验证码！\"}";
         }
         else if (System.Web.HttpContext.Current.Session[DTKeys.SESSION_CODE] == null) {
            result = "{\"status\":0, \"msg\":\"对不起，验证码已过期！\"}";
         }
         else if (formString.ToLower() != System.Web.HttpContext.Current.Session[DTKeys.SESSION_CODE].ToString().ToLower()) {
            result = "{\"status\":0, \"msg\":\"验证码与系统的不一致！\"}";
         }
         else if (string.IsNullOrEmpty(str4)) {
            result = "{\"status\": 0, \"msg\": \"对不起，请输入留言的内容！\"}";
         }
         else {
            model.site_path = Utils.DropHTML(queryString);
            model.title = Utils.DropHTML(str3);
            model.content = Utils.ToHtml(str4);
            model.user_name = Utils.DropHTML(str5);
            model.user_tel = Utils.DropHTML(str6);
            model.user_qq = Utils.DropHTML(str7);
            model.user_email = Utils.DropHTML(str8);
            model.add_time = DateTime.Now;
            model.is_lock = 1;
            if (feedback.Add(model) > 0) {
               result = "{\"status\": 1, \"msg\": \"恭喜您，留言提交成功！\"}";
            }
            else {
               result = "{\"status\": 0, \"msg\": \"对不起，保存过程中发生错误！\"}";
            }
         }
         return Content(result);
      }

      #region 返回每页数量=============================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("feedback_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion

      public DataTable get_feedback_list(int top, string strwhere) {
         DataTable table = new DataTable();
         string strWhere = "is_lock=0";
         if (!string.IsNullOrEmpty(strwhere)) {
            strWhere = strWhere + " and " + strwhere;
         }
         return new DTcms.BLL.feedback().GetList(top, strWhere).Tables[0];
      }

      public DataTable get_feedback_list(int page_size, int page_index, string strwhere, out int totalcount) {
         DataTable table = new DataTable();
         string strWhere = "is_lock=0";
         if (!string.IsNullOrEmpty(strwhere)) {
            strWhere = strWhere + " and " + strwhere;
         }
         return new DTcms.BLL.feedback().GetList(page_size, page_index, strWhere, "add_time desc", out totalcount).Tables[0];
      }
   }
}
