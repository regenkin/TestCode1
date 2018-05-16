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
using DTcms.Web.Mvc.Plugin.KfCenter.BLL;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
    public class KfCenter_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/plugins/kfcenter/Areas/admin/Views/kfcenter_list.cshtml";
      protected int totalCount;
      protected int page;
      protected int pageSize;

      protected string keywords = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("plugin_kfcenter", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");
         if (int.TryParse(DTRequest.GetQueryString("pagesize"), out this.pageSize))
         {
            if (this.pageSize > 0) {
                Utils.WriteCookie("kfcenter_page_size", "DTcmsPage", this.pageSize.ToString(), 14400);
            }
         }
         else {
            this.pageSize = GetPageSize(10); //每页数量
         }
         this.page = DTRequest.GetQueryInt("page", 1);
         //绑定查询关键字
         ViewBag.Keywords = this.keywords;
         //绑定页码
         ViewBag.PageSize = this.pageSize.ToString();
         ViewBag.Page = this.page.ToString();
      }
      //
      // GET: /admin/KfCenter_List/

      public ActionResult Index() {
         RptBind("id>0" + CombSqlTxt(this.keywords), "id desc");
         return View(WEB_VIEW);
      }

      /// <summary>
      /// 设置分页数量
      /// </summary>
      /// <returns></returns>
      public ActionResult PageNumTextChanged() {
         string url = Utils.CombUrlTxt(WEB_VIEW, "keywords={0}", this.keywords);
         RptBind("id>0" + CombSqlTxt(this.keywords), "id desc");
         return View(url);
      }

      [HttpPost]
      /// <summary>
      /// 提交保存
      /// </summary>
      /// <returns></returns>
      public ActionResult SubmitSave(string json) {
         ChkAdminLevel("sys_site_manage", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         DTcms.BLL.sites bll = new DTcms.BLL.sites();
         JObject jobject = JObject.Parse(json);
         JToken record = jobject["list"];
         foreach (JToken item in record) {
            int id;
            if (!int.TryParse(item["id"].ToString(), out id))
               id = -1;
            int sortId;
            if (!int.TryParse(item["id"].ToString(), out sortId))
               sortId = 99;
            bll.UpdateSort(id, sortId);
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存站点排序"); //记录日志
         string script = JscriptMsg("保存排序成功！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords), "parent.loadMenuTree");
         return Content(script);
      }

      /// <summary>
      /// 批量删除
      /// </summary>
      /// <returns></returns>
      public ActionResult SubmitDelete(string json) {
         ChkAdminLevel("sys_site_manage", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0;
         int errorCount = 0;
         DTcms.BLL.sites bll = new DTcms.BLL.sites();
         JObject jobject = JObject.Parse(json);
         JToken record = jobject["list"];
         int id;
         foreach (JToken item in record) {
            if (int.TryParse(item["id"].ToString(), out id)) {
               //检查该分类下是否还有频道
               int channelCount = new DTcms.BLL.site_channel().GetCount("site_id=" + id);
               if (channelCount > 0) {
                  errorCount += 1;
                  continue;
               }
               DTcms.Model.sites model = bll.GetModel(id);
               //删除成功后对应的目录及文件
               if (bll.Delete(id)) {
                  sucCount += 1;
                  FileHelper.DeleteDirectory(sysConfig.webpath + DTKeys_Extension.DIRECTORY_REWRITE_MVC + "/" + model.build_path);
                  FileHelper.DeleteDirectory(sysConfig.webpath + DTKeys.DIRECTORY_REWRITE_HTML + "/" + model.build_path);
               }
               else {
                  errorCount += 1;
               }
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除站点成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords), "parent.loadMenuTree");
         return Content(script);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere, string _orderby) {
          DTcms.Web.Mvc.Plugin.KfCenter.BLL.kfActSetBLL<DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActSet> bll = new DTcms.Web.Mvc.Plugin.KfCenter.BLL.kfActSetBLL<DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActSet>();
         DataSet ds = bll.GetList(this.pageSize, this.page, _strWhere, _orderby, out this.totalCount);
         DataTable list = null;
         if (ds.Tables.Count > 0) {
            list = ds.Tables[0];
         }
         else {
            list = new DataTable();
         }
         ViewData["list"] = list;
         string pageUrl = Utils.CombUrlTxt("index", "keywords={0}&page={1}", this.keywords, "__id__");
         ViewBag.PageContent = Utils.OutPageList(this.pageSize, this.page, this.totalCount, pageUrl, 8);
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(string _keywords) {
         StringBuilder strTemp = new StringBuilder();
         _keywords = _keywords.Replace("'", "");
         if (!string.IsNullOrEmpty(_keywords)) {
             strTemp.Append(" and (ActsetNum like  '%" + _keywords + "%' or ActSetName like  '%" + _keywords + "%' or ActsetDBName like '%" + _keywords + "%' or CreateDate like '%" + _keywords + "%')");
         }
         return strTemp.ToString();
      }
      #endregion

      #region 返回每页数量=============================
      private int GetPageSize(int _default_size) {
         int _pagesize;
         if (int.TryParse(Utils.GetCookie("kfcenter_page_size", "DTcmsPage"), out _pagesize))
         {
            if (_pagesize > 0) {
               return _pagesize;
            }
         }
         return _default_size;
      }
      #endregion
   }
}
