using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Url_Rewrite_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Settings/url_rewrite_edit.cshtml";
       private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
       private string urlName = string.Empty;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("sys_url_rewrite", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          ActionResult result = View(EDIT_RESULT_VIEW);
          string _action = DTRequest.GetQueryString("action");
          if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
             this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
             this.urlName = DTRequest.GetQueryString("name");
             if (string.IsNullOrEmpty(this.urlName)) {
                ViewBag.ClientScript = JscriptMsg("传输参数不正确！", "back");
                filterContext.Result = result;
                return;
             }
          }
          ViewBag.Action = this.action;
          ViewBag.UrlName = this.urlName;
       }
        //
        // GET: /admin/Url_Rewrite_Edit/

       public ActionResult Index() {
          TreeBind(); //绑定频道
          if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
                {
             ShowInfo(urlName);
          }
          else {
             ViewData["model"] = new DTcms.Model.url_rewrite();
             ViewBag.ClientScript = "$('#txtName').attr('ajaxurl', '/tools/admin_ajax.ashx?action=urlrewrite_name_validate');";
          }
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitSave() {
          ActionResult result = View(EDIT_RESULT_VIEW);
          if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
             ChkAdminLevel("sys_url_rewrite", DTEnums.ActionEnum.Edit.ToString()); //检查权限
             if (!DoEdit(this.urlName)) {
                JscriptMsg("保存过程中发生错误！", "back");
                return result;
             }
             ViewBag.ClientScript = JscriptMsg("修改配置成功！", "../url_rewrite_list/index");
          }
          else //添加
            {
             ChkAdminLevel("sys_url_rewrite", DTEnums.ActionEnum.Add.ToString()); //检查权限
             if (!DoAdd()) {
                JscriptMsg("保存过程中发生错误！", "");
                return result;
             }
             JscriptMsg("添加配置成功！", "../url_rewrite_list/index");
          }
          return result;
       }

       #region 绑定频道=================================
       private void TreeBind() {
          DTcms.BLL.site_channel bll = new DTcms.BLL.site_channel();
          DataTable dt = bll.GetList(0, "", "sort_id asc,id desc").Tables[0];
          List<SelectListItem> channelSelectItems = new List<SelectListItem>(){
             new SelectListItem(){ Text="不属于频道", Value=""}
          };
          foreach (DataRow dr in dt.Rows) {
             channelSelectItems.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["name"].ToString() });
          }
          ViewData["channelSelectItems"] = channelSelectItems;
          // 
          List<SelectListItem> typeSelectItems = new List<SelectListItem>(){
             new SelectListItem(){ Text="所有类型", Value=""},
             new SelectListItem(){ Text="首页", Value="index"},
             new SelectListItem(){ Text="列表页", Value="list"},
             new SelectListItem(){ Text="栏目页", Value="category"},
             new SelectListItem(){ Text="详细页", Value="detail"},
             new SelectListItem(){ Text="插件页", Value="plugin"},
             new SelectListItem(){ Text="其他页", Value="other"}
          };
          ViewData["typeSelectItems"] = typeSelectItems;
       }
       #endregion

       #region 赋值操作=================================
       private void ShowInfo(string _urlName) {
          DTcms.BLL.url_rewrite bll = new DTcms.BLL.url_rewrite();
          DTcms.Model.url_rewrite model = bll.GetInfo(_urlName);
          ViewData["model"] = model;
       }
       #endregion

       #region 增加操作=================================
       private bool DoAdd() {
          DTcms.BLL.url_rewrite bll = new DTcms.BLL.url_rewrite();
          DTcms.Model.url_rewrite model = new DTcms.Model.url_rewrite();

          model.name = Request.Form["txtName"].Trim();
          model.type = Request.Form["ddlType"];
          model.channel = Request.Form["ddlChannel"];
          model.page = Request.Form["txtPage"].Trim();
          model.inherit = Request.Form["txtInherit"].Trim();
          model.templet = Request.Form["txtTemplet"].Trim();
          if (!string.IsNullOrEmpty(Request.Form["txtPageSize"].Trim())) {
             model.pagesize = Request.Form["txtPageSize"].Trim();
          }
          //添加URL重写节点
          List<DTcms.Model.url_rewrite_item> items = new List<DTcms.Model.url_rewrite_item>();
          string[] itemPathArr = Request.Form.GetValues("itemPath");
          string[] itemPatternArr = Request.Form.GetValues("itemPattern");
          string[] itemQuerystringArr = Request.Form.GetValues("itemQuerystring");
          if (itemPathArr != null && itemPatternArr != null && itemQuerystringArr != null) {
             for (int i = 0; i < itemPathArr.Length; i++) {
                items.Add(new DTcms.Model.url_rewrite_item { path = itemPathArr[i], pattern = itemPatternArr[i], querystring = itemQuerystringArr[i] });
             }
          }
          model.url_rewrite_items = items;

          if (bll.Add(model)) {
             AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加URL配置信息:" + model.name); //记录日志
             return true;
          }
          return false;
       }
       #endregion

       #region 修改操作=================================
       private bool DoEdit(string _urlName) {
          DTcms.BLL.url_rewrite bll = new DTcms.BLL.url_rewrite();
          DTcms.Model.url_rewrite model = bll.GetInfo(_urlName);

          model.type = Request.Form["ddlType"];
          model.channel = Request.Form["ddlChannel"];
          model.page = Request.Form["txtPage"].Trim();
          model.inherit = Request.Form["txtInherit"].Trim();
          model.templet = Request.Form["txtTemplet"].Trim();
          if (!string.IsNullOrEmpty(Request.Form["txtPageSize"].Trim())) {
             model.pagesize = Request.Form["txtPageSize"].Trim();
          }
          //添加URL重写节点
          List<DTcms.Model.url_rewrite_item> items = new List<DTcms.Model.url_rewrite_item>();
          string[] itemPathArr = Request.Form.GetValues("itemPath");
          string[] itemPatternArr = Request.Form.GetValues("itemPattern");
          string[] itemQuerystringArr = Request.Form.GetValues("itemQuerystring");
          if (itemPathArr != null && itemPatternArr != null && itemQuerystringArr != null) {
             for (int i = 0; i < itemPathArr.Length; i++) {
                items.Add(new DTcms.Model.url_rewrite_item { path = itemPathArr[i], pattern = itemPatternArr[i], querystring = itemQuerystringArr[i] });
             }
          }
          model.url_rewrite_items = items;

          if (bll.Edit(model)) {
             AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改URL配置信息:" + model.name); //记录日志
             return true;
          }
          return false;
       }
       #endregion
    }
}
