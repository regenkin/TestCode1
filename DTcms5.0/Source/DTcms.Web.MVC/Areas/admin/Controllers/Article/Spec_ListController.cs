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

namespace DTcms.Web.MVC.Areas.admin.Controllers.Channel {
   public class Spec_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Article/spec_list.cshtml";
      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected string keywords = string.Empty;
      protected int channel_id;
      protected string channel_name = string.Empty; //频道名称

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("channel_" + this.channel_name + "_spec", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         this.keywords = DTRequest.GetQueryString("keywords");
         this.page = DTRequest.GetQueryInt("page", 1);
         this.channel_id = DTRequest.GetQueryInt("channel_id");
         this.channel_name = new BLL.site_channel().GetChannelName(this.channel_id); //取得频道名称
         this.pageSize = DTRequest.GetQueryInt("pagesize", 0);//每页数量
         if (this.pageSize > 0) {
            Utils.WriteCookie("channel_spec_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
         }
         else {
            this.pageSize = GetPageSize(10);
         }
         if (this.channel_id == 0) {
            JscriptMsg("频道参数不正确！", "back");
            filterContext.Result = result;
            return;
         }
         ViewBag.Keywords = this.keywords;
         ViewBag.PageSize = this.pageSize;
         ViewBag.Page = this.page.ToString();
         ViewBag.ChannelId = this.channel_id;
         ViewBag.ChannelName = this.channel_name;
      }

      //
      // GET: /admin/Spec_List/
      public ActionResult Index() {
         RptBind("parent_id=0 and channel_id=" + channel_id.ToString() + CombSqlTxt(keywords), "sort_id asc,id desc");
         return View(WEB_VIEW);
      }

      [HttpPost]
      /// <summary>
      /// 提交保存
      /// </summary>
      /// <returns></returns>
      public ActionResult SubmitSave(string json) {
         ChkAdminLevel("channel_" + this.channel_name + "_spec", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         DTcms.BLL.article_spec bll = new DTcms.BLL.article_spec();
         JObject jobject = JObject.Parse(json);
         JToken record = jobject["list"];
         foreach (JToken item in record) {
            int id;
            if (!int.TryParse(item["id"].ToString(), out id))
               id = -1;
            int sortId;
            if (!int.TryParse(item["sortId"].ToString(), out sortId))
               sortId = 99;
            bll.UpdateField(id, "sort_id=" + sortId.ToString());
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存商品规格排序"); //记录日志
         string script = JscriptMsg("保存排序成功！", Utils.CombUrlTxt("index", "channel_id={0}&keywords={1}", this.channel_id.ToString(), this.keywords));
         return Content(script);
      }

      /// <summary>
      /// 批量删除
      /// </summary>
      /// <returns></returns>
      public ActionResult SubmitDelete(string json) {
         ChkAdminLevel("channel_" + this.channel_name + "_spec", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0;
         int errorCount = 0;
         DTcms.BLL.article_spec bll = new DTcms.BLL.article_spec();
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
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除商品规格" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除规格成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "channel_id={0}&keywords={1}", this.channel_id.ToString(), this.keywords));
         return Content(script);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         this.page = DTRequest.GetQueryInt("page", 1);
         BLL.article_spec bll = new BLL.article_spec();
         DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
         DataTable list = null;
         if (ds.Tables.Count > 0) {
            list = ds.Tables[0];
         }
         ViewData["list"] = list;
         string pageUrl = Utils.CombUrlTxt("index", "channel_id={0}&keywords={1}&page={2}", this.channel_id.ToString(), this.keywords, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(string _keywords) {
         StringBuilder strTemp = new StringBuilder();
         _keywords = _keywords.Replace("'", "");
         if (!string.IsNullOrEmpty(_keywords)) {
            strTemp.Append(" and title like  '%" + _keywords + "%'");
         }

         return strTemp.ToString();
      }
      #endregion

      #region 返回每页数量=============================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("channel_spec_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion
   }
}
