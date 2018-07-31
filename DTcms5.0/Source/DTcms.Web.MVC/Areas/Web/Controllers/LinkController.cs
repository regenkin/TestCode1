using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers
{
    public class LinkController : DTcms.Web.MVC.UI.Controllers.BaseController
    {
       protected int totalCount;
       protected int page;
       protected int pageSize;

       protected string keywords = string.Empty;

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          if (int.TryParse(DTRequest.GetQueryString("pageNum"), out this.pageSize)) {
             if (this.pageSize > 0) {
                Utils.WriteCookie("link_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
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
       // GET: /Web/Link/

       public ActionResult Index() {
          return View(ViewName);
       }

       [HttpPost]
       public ActionResult SubmitSave() {
          string result = string.Empty;
          string action = DTRequest.GetQueryString("action");
          if (action != DTEnums.ActionEnum.Add.ToString().ToLower()) {
             return Content("{\"status\":0, \"msg\":\"对不起，网站传输参数有误！\"}");
          }
          StringBuilder builder = new StringBuilder();
          DTcms.BLL.link link = new DTcms.BLL.link();
          DTcms.Model.link model = new DTcms.Model.link();
          string queryString = DTRequest.GetQueryString("site");
          string formString = DTRequest.GetFormString("txtCode");
          string str3 = DTRequest.GetFormString("txtTitle");
          string str4 = DTRequest.GetFormString("txtUserName");
          string str5 = DTRequest.GetFormString("txtUserTel");
          string str6 = DTRequest.GetFormString("txtEmail");
          string str7 = DTRequest.GetFormString("txtSiteUrl");
          string str8 = DTRequest.GetFormString("txtImgUrl");
          if (string.IsNullOrEmpty(queryString)) {
             result = "{\"status\":0, \"msg\":\"对不起，网站传输参数有误！\"}";
          }
          else if (string.IsNullOrEmpty(formString)) {
             result = "{\"status\":0, \"msg\":\"对不起，请输入验证码！\"}";
          }
          else if (System.Web.HttpContext.Current.Session[DTKeys.SESSION_CODE] == null) {
             result = "{\"status\":0, \"msg\":\"对不起，系验证码已过期！\"}";
          }
          else if (formString.ToLower() != System.Web.HttpContext.Current.Session[DTKeys.SESSION_CODE].ToString().ToLower()) {
             result = "{\"status\":0, \"msg\":\"验证码与系统的不一致！\"}";
          }
          else if (string.IsNullOrEmpty(str3)) {
             result = "{\"status\": 0, \"msg\": \"对不起，请输入网站标题！\"}";
          }
          else if (string.IsNullOrEmpty(str7)) {
             result = "{\"status\": 0, \"msg\": \"对不起，请输入网站网址！\"}";
          }
          else {
             model.site_path = Utils.DropHTML(queryString);
             model.title = Utils.DropHTML(str3);
             model.is_lock = 1;
             model.is_red = 0;
             model.user_name = Utils.DropHTML(str4);
             model.user_tel = Utils.DropHTML(str5);
             model.email = Utils.DropHTML(str6);
             model.site_url = Utils.DropHTML(str7);
             model.img_url = Utils.DropHTML(str8);
             model.is_image = 1;
             model.add_time = DateTime.Now;
             if (string.IsNullOrEmpty(model.img_url)) {
                model.is_image = 0;
             }
             if (link.Add(model) > 0) {
                result = "{\"status\": 1, \"msg\": \"恭喜您，提交成功！\"}";
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
          if (int.TryParse(Utils.GetCookie("link_page_size", "DTcmsPage"), out _pagesize)) {
             if (_pagesize > 0) {
                return _pagesize;
             }
          }
          return _default_size;
       }
       #endregion

       public DataTable get_link_list(int top, string strwhere) {
          DataTable table = new DataTable();
          string strWhere = "is_lock=0";
          if (!string.IsNullOrEmpty(strwhere)) {
             strWhere = strWhere + " and " + strwhere;
          }
          return new DTcms.BLL.link().GetList(top, strWhere).Tables[0];
       }

       public DataTable get_link_list(int page_size, int page_index, string strwhere, out int totalcount) {
          DataTable table = new DataTable();
          string strWhere = "is_lock=0";
          if (!string.IsNullOrEmpty(strwhere)) {
             strWhere = strWhere + " and " + strwhere;
          }
          return new DTcms.BLL.link().GetList(page_size, page_index, strWhere, "sort_id asc,add_time desc", out totalcount).Tables[0];
       }
    }
}
