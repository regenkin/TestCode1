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

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public class Attribute_Field_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Channel/attribute_field_list.cshtml";
      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected string control_type = string.Empty;
      protected string keywords = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("sys_channel_field", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.control_type = DTRequest.GetQueryString("control_type");
         this.keywords = DTRequest.GetQueryString("keywords");
         if (int.TryParse(DTRequest.GetQueryString("pageNum"), out pageSize)) {
            if (pageSize > 0) {
               Utils.WriteCookie("attribute_field_page_size", "DTcmsPage", pageSize.ToString(), 43200);
            }
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }
         ViewBag.Keywords = this.keywords;
         ViewBag.PageNum = this.pageSize.ToString();
         ViewBag.ControlType = this.control_type;
      }
      //
      // GET: /admin/Attribute_Field/

      public ActionResult Index() {
         RptBind("id>0" + CombSqlTxt(this.control_type, this.keywords), "is_sys desc,sort_id asc,id desc");
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ChkAdminLevel("sys_channel_field", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         DTcms.BLL.article_attribute_field bll = new DTcms.BLL.article_attribute_field();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = Convert.ToInt32(item["id"].ToString());
            int sortId = Convert.ToInt32(item["sortId"].ToString());
            bll.UpdateField(id, "sort_id=" + sortId.ToString());
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存扩展字段排序"); //记录日志
         string script = JscriptMsg("保存排序成功！", Utils.CombUrlTxt("index", "control_type={0}&keywords={1}", this.control_type, this.keywords));
         return Content(script);
      }

      [HttpPost]
      public ActionResult SubmitDelete() {
         ChkAdminLevel("sys_channel_field", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0; //成功数量
         int errorCount = 0; //失败数量
         DTcms.BLL.article_attribute_field bll = new DTcms.BLL.article_attribute_field();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = Convert.ToInt32(item["id"].ToString());
            if (bll.Delete(id)) {
               sucCount++;
            }
            else {
               errorCount++;
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除扩展字段成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "control_type={0}&keywords={1}", this.control_type, this.keywords));
         return Content(script);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
         this.page = DTRequest.GetQueryInt("page", 1);
         DTcms.BLL.article_attribute_field bll = new DTcms.BLL.article_attribute_field();
         DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
         ViewData["list"] = ds;
         string pageUrl = Utils.CombUrlTxt("index", "control_type={0}&keywords={1}&page={2}", this.control_type, this.keywords, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);

         List<SelectListItem> typeList = new List<SelectListItem>() {
              new SelectListItem(){ Text="所有类型", Value="" },
              new SelectListItem(){ Text="单行文本", Value="single-text" },
              new SelectListItem(){ Text="多行文本", Value="multi-text" },
              new SelectListItem(){ Text="编辑器", Value="editor" },
              new SelectListItem(){ Text="图片上传", Value="images" },
              new SelectListItem(){ Text="视频上传", Value="video" },
              new SelectListItem(){ Text="数字", Value="number" },
              new SelectListItem(){ Text="复选框", Value="checkbox" },
              new SelectListItem(){ Text="多项单选", Value="multi-radio" },
              new SelectListItem(){ Text="多项多选", Value="multi-checkbox" }
           };
         ViewData["typeList"] = typeList;
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(string _control_type, string _keywords) {
         StringBuilder strTemp = new StringBuilder();
         if (!string.IsNullOrEmpty(_control_type)) {
            strTemp.Append(" and control_type='" + _control_type + "'");
         }
         _keywords = _keywords.Replace("'", "");
         if (!string.IsNullOrEmpty(_keywords)) {
            strTemp.Append(" and (name like  '%" + _keywords + "%' or title like '%" + _keywords + "%')");
         }

         return strTemp.ToString();
      }
      #endregion

      #region 返回每页数量=============================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("attribute_field_page_size", "DTcmsPage"), out _pagesize)) {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion
   }
}
