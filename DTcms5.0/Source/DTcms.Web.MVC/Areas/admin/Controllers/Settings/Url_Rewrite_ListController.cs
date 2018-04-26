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
    public class Url_Rewrite_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Settings/url_rewrite_list.cshtml";
       protected string channel = string.Empty;
       protected string type = string.Empty;

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("sys_url_rewrite", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          this.channel = DTRequest.GetQueryString("channel");
          this.type = DTRequest.GetQueryString("type");
          ViewBag.Channel = this.channel;
          ViewBag.Type = this.type;
       }
        //
        // GET: /admin/Url_Rewrite/

       public ActionResult Index() {
          TreeBind();
          RptBind(this.channel, this.type);
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitDelete() {
          ChkAdminLevel("sys_url_rewrite", DTEnums.ActionEnum.Delete.ToString()); //检查权限
          DTcms.BLL.url_rewrite bll = new DTcms.BLL.url_rewrite();
          JObject jobject = JObject.Parse(Request.Form["json"]);
          JToken list = jobject["list"];
          foreach (JToken item in list) {
             bll.Remove("name", item["name"].ToString());
          }
          AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除URL配置信息"); //记录日志
          string script = JscriptMsg("URL配置删除成功！", "index");
          return Content(script);
       }

       #region 绑定频道=================================
       private void TreeBind() {
          DTcms.BLL.site_channel bll = new DTcms.BLL.site_channel();
          DataTable dt = bll.GetList(0, "", "sort_id asc,id desc").Tables[0];
          List<SelectListItem> channelList = new List<SelectListItem>();
          channelList.Add(new SelectListItem() { Text = "所有频道", Value = "" });
          foreach (DataRow dr in dt.Rows) {
             channelList.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["name"].ToString() });
          }
          ViewData["channelList"] = channelList;
          // 
          List<SelectListItem> typeList = new List<SelectListItem>(){
             new SelectListItem(){ Text="所有类型", Value=""},
             new SelectListItem(){ Text="首页", Value="index"},
             new SelectListItem(){ Text="列表页", Value="list"},
             new SelectListItem(){ Text="栏目页", Value="category"},
             new SelectListItem(){ Text="详细页", Value="detail"},
             new SelectListItem(){ Text="插件页", Value="plugin"},
             new SelectListItem(){ Text="其他页", Value="other"}
          };
          ViewData["typeList"] = typeList;
       }
       #endregion

       #region 绑定数据=================================
       private void RptBind(string _channel, string _type) {
          List<DTcms.Model.url_rewrite> list = new DTcms.BLL.url_rewrite().GetList(_channel, _type);
          ViewData["list"] = list;
       }
       #endregion
    }
}
