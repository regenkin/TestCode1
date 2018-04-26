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
   public class Category_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Article/category_list.cshtml";
      protected int channel_id;
      protected string channel_name = string.Empty; //频道名称

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         this.channel_id = DTRequest.GetQueryInt("channel_id");
         this.channel_name = new DTcms.BLL.site_channel().GetChannelName(this.channel_id); //取得频道名称
         if (this.channel_id == 0) {
            JscriptMsg("频道参数不正确！", "back");
            filterContext.Result = result;
            return;
         }
         ViewBag.ChannelId = this.channel_id.ToString();
      }
      //
      // GET: /admin/Category_List/

      public ActionResult Index() {
         ChkAdminLevel("channel_" + this.channel_name + "_category", DTEnums.ActionEnum.View.ToString()); //检查权限
         RptBind();
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave() {
         ChkAdminLevel("channel_" + this.channel_name + "_category", DTEnums.ActionEnum.Edit.ToString()); //检查权限
         DTcms.BLL.article_category bll = new DTcms.BLL.article_category();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            int sortId = int.Parse(item["sortId"].ToString());
            bll.UpdateField(id, "sort_id=" + sortId.ToString());
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "保存" + this.channel_name + "频道栏目分类排序"); //记录日志
         string script = JscriptMsg("保存排序成功！", Utils.CombUrlTxt("index", "channel_id={0}", this.channel_id.ToString()));
         return Content(script);
      }

      [HttpPost]
      public ActionResult SubmitDelete() {
         ChkAdminLevel("channel_" + this.channel_name + "_category", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         DTcms.BLL.article_category bll = new DTcms.BLL.article_category();
         JObject jobject = JObject.Parse(Request.Form["json"]);
         JToken list = jobject["list"];
         foreach (JToken item in list) {
            int id = int.Parse(item["id"].ToString());
            bll.Delete(id);
         }
         AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "删除" + this.channel_name + "频道栏目分类数据"); //记录日志
         string script = JscriptMsg("删除数据成功！", Utils.CombUrlTxt("index", "channel_id={0}", this.channel_id.ToString()));
         return Content(script);
      }

      //数据绑定
      private void RptBind() {
         DTcms.BLL.article_category bll = new DTcms.BLL.article_category();
         DataTable dt = bll.GetList(0, this.channel_id);
         ViewData["list"] = dt;
      }
   }
}
