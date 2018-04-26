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
    public class Message_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Users/Message_List.cshtml";
       protected int totalCount;
       protected int page;
       protected int pageSize;

       protected int type_id;
       protected string keywords = string.Empty;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_message", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          this.type_id = DTRequest.GetQueryInt("type_id");
          this.keywords = DTRequest.GetQueryString("keywords");
          if (int.TryParse(DTRequest.GetQueryString("pageNum"), out pageSize)) {
             if (pageSize > 0) {
                Utils.WriteCookie("message_list_page_size", "DTcmsPage", pageSize.ToString(), 14400);
             }
          }
          else {
             this.pageSize = GetPageSize(10); //每页数量
          }
          ViewBag.type_id = this.type_id.ToString();
          ViewBag.Keywords = this.keywords;
          ViewBag.PageNum = this.pageSize;
       }

        //
        // GET: /admin/Message_List/

       public ActionResult Index() {
          RptBind("id>0" + CombSqlTxt(this.type_id, this.keywords), "post_time desc,id desc");
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitDelete(string json) {
          ChkAdminLevel("user_message", DTEnums.ActionEnum.Delete.ToString()); //检查权限
          int sucCount = 0;
          int errorCount = 0;
          DTcms.BLL.user_message bll = new DTcms.BLL.user_message();
          JObject jobject = JObject.Parse(json);
          JToken record = jobject["list"];
          int id;
          foreach (JToken item in record) {
             if (int.TryParse(item["id"].ToString(), out id)) {
                if (bll.Delete(id)) {
                   sucCount += 1;
                }
                else {
                   errorCount += 1;
                }
             }
          }
          AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除站内短消息成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
          string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
              Utils.CombUrlTxt("message_list.aspx", "type_id={0}&keywords={1}", this.type_id.ToString(), this.keywords));
          return Content(script);
       }

       #region 数据绑定=================================
       private void RptBind(string _strWhere, string _orderby) {
          DTcms.BLL.user_message bll = new DTcms.BLL.user_message();
          ViewData["list"] = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount).Tables[0];

          string pageUrl = Utils.CombUrlTxt("message_list.aspx", "type_id={0}&keywords={1}&page={2}",
              this.type_id.ToString(), this.keywords, "__id__");
          ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
       }
       #endregion

       #region 组合SQL查询语句==========================
       protected string CombSqlTxt(int _type_id, string _keywords) {
          StringBuilder strTemp = new StringBuilder();
          if (_type_id > 0) {
             strTemp.Append(" and type=" + _type_id);
          }
          _keywords = _keywords.Replace("'", "");
          if (!string.IsNullOrEmpty(_keywords)) {
             strTemp.Append(" and (accept_user_name='" + _keywords + "' or post_user_name like '%" + _keywords + "%' or title like '%" + _keywords + "%')");
          }
          return strTemp.ToString();
       }
       #endregion

       #region 返回用户每页数量=========================
       private int GetPageSize(int _default_size) {
          int _pagesize;
          if (int.TryParse(Utils.GetCookie("message_list_page_size", "DTcmsPage"), out _pagesize)) {
             if (_pagesize > 0) {
                return _pagesize;
             }
          }
          return _default_size;
       }
       #endregion
    }
}
