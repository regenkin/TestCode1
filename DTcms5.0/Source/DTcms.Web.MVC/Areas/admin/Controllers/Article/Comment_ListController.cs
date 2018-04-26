using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Comment_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       protected const string WEB_VIEW = "~/Areas/admin/Views/Article/comment_list.cshtml";
       protected int channel_id;
       protected int totalCount;
       protected int page;
       protected int pageSize;

       protected string channel_name = string.Empty; //频道名称
       protected string property = string.Empty;
       protected string keywords = string.Empty;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("channel_" + this.channel_name + "_comment", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          ActionResult result = View(EDIT_RESULT_VIEW);
          this.channel_id = DTRequest.GetQueryInt("channel_id");
          this.channel_name = new DTcms.BLL.site_channel().GetChannelName(this.channel_id); //取得频道名称
          this.property = DTRequest.GetQueryString("property");
          this.keywords = DTRequest.GetQueryString("keywords");
          if (int.TryParse(DTRequest.GetQueryString("pagesize"), out this.pageSize)) {
             if (this.pageSize > 0) {
                Utils.WriteCookie("article_comment_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
             }
          }
          else {
             this.pageSize = GetPageSize(10); //每页数量
          }
          if (channel_id == 0) {
             JscriptMsg("频道参数不正确！", "back");
             filterContext.Result = result;
             return;
          }
          ViewBag.ChannelId = this.channel_id;
          ViewBag.Property = this.property;
          ViewBag.Keywords = this.keywords;
          ViewBag.PageSize = this.pageSize.ToString();
       }
        //
        // GET: /admin/Comment_List/

       public ActionResult Index() {
          RptBind("channel_id=" + this.channel_id + CombSqlTxt(this.keywords, this.property), "add_time desc");
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitAudit() {
          ChkAdminLevel("channel_" + this.channel_name + "_comment", DTEnums.ActionEnum.Audit.ToString()); //检查权限
          DTcms.BLL.article_comment bll = new DTcms.BLL.article_comment();
          JObject jobject = JObject.Parse(Request.Form["json"]);
          JToken list = jobject["list"];
          foreach (JToken item in list) {
             int id = int.Parse(item["id"].ToString());
             bll.UpdateField(id, "is_lock=0");
          }
          AddAdminLog(DTEnums.ActionEnum.Audit.ToString(), "审核" + this.channel_name + "频道评论信息"); //记录日志
          string script = JscriptMsg("审核通过成功！", Utils.CombUrlTxt("index", "channel_id={0}&keywords={1}&property={2}",
              this.channel_id.ToString(), this.keywords, this.property));
          return Content(script);
       }

       [HttpPost]
       public ActionResult SubmitDelete() {
          ChkAdminLevel("channel_" + this.channel_name + "_comment", DTEnums.ActionEnum.Delete.ToString()); //检查权限
          int sucCount = 0;
          int errorCount = 0;
          DTcms.BLL.article_comment bll = new DTcms.BLL.article_comment();
          JObject jobject = JObject.Parse(Request.Form["json"]);
          JToken list = jobject["list"];
          foreach (JToken item in list) {
             int id = int.Parse(item["id"].ToString());
             if (bll.Delete(id)) {
                sucCount += 1;
             }
             else {
                errorCount += 1;
             }
          }
          AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除" + this.channel_name + "频道评论成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
          string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
              Utils.CombUrlTxt("index", "channel_id={0}&keywords={1}&property={2}", this.channel_id.ToString(), this.keywords, this.property));
          return Content(script);
       }

        #region 数据绑定=================================
        private void RptBind(string _strWhere, string _orderby) {
           this.page = DTRequest.GetQueryInt("page", 1);
           DTcms.BLL.article_comment bll = new DTcms.BLL.article_comment();
           DataTable list = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount).Tables[0];
           ViewData["list"] = list;
           string pageUrl = Utils.CombUrlTxt("index", "channel_id={0}&keywords={1}&property={2}&page={3}",
               this.channel_id.ToString(), this.keywords, this.property, "__id__");
           ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords, string _property) {
           StringBuilder strTemp = new StringBuilder();
           _keywords = _keywords.Replace("'", "");
           if (!string.IsNullOrEmpty(_keywords)) {
              strTemp.Append(" and (user_name like '%" + _keywords + "%' or content like '%" + _keywords + "%')");
           }
           if (!string.IsNullOrEmpty(_property)) {
              switch (_property) {
                 case "isLock":
                    strTemp.Append(" and is_lock=1");
                    break;
                 case "unLock":
                    strTemp.Append(" and is_lock=0");
                    break;
              }
           }
           return strTemp.ToString();
        }
        #endregion

        #region 返回评论每页数量=========================
        private int GetPageSize(int _default_size) {
           int _pagesize;
           if (int.TryParse(Utils.GetCookie("article_comment_page_size", "DTcmsPage"), out _pagesize)) {
              if (_pagesize > 0) {
                 return _pagesize;
              }
           }
           return _default_size;
        }
        #endregion
    }
}
