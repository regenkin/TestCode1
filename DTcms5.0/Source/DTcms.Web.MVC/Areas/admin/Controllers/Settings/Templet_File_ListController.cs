using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Templet_File_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Settings/Templet_File_List.cshtml";
       protected string skinName = string.Empty; //模板目录

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("sys_site_templet", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          ActionResult result = View(EDIT_RESULT_VIEW);
          skinName = DTRequest.GetQueryString("skin");
          if (string.IsNullOrEmpty(skinName)) {
             JscriptMsg("传输参数不正确！", "back");
             filterContext.Result = result;
             return;
          }
          if (skinName.IndexOf("..") != -1) {
             JscriptMsg("模板目录名有误！", "back");
             return;
          }
          ViewBag.SkinName = skinName;
       }
        //
        // GET: /admin/Templet_File_List/

       public ActionResult Index() {
          RptBind(skinName);
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitDelete() {
          ChkAdminLevel("sys_site_templet", DTEnums.ActionEnum.Delete.ToString()); //检查权限
          JObject jobject = JObject.Parse(Request.Form["json"]);
          JToken record = jobject["list"];
          foreach (JToken item in record) {
             FileHelper.DeleteFile("/templates/" + this.skinName + "/" + item["name"]);
          }
          AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除模板文件，模板:" + this.skinName);//记录日志
          string script = JscriptMsg("文件删除成功！", Utils.CombUrlTxt("index", "skin={0}", this.skinName));
          return Content(script);
       }

       #region 数据绑定=================================
       private void RptBind(string skin_name) {
          DataTable dt = new DataTable();
          dt.Columns.Add("name", Type.GetType("System.String"));
          dt.Columns.Add("skinname", Type.GetType("System.String"));
          dt.Columns.Add("creationtime", Type.GetType("System.String"));
          dt.Columns.Add("updatetime", Type.GetType("System.String"));

          DirectoryInfo dirInfo = new DirectoryInfo(Utils.GetMapPath(@"~/templates/" + skin_name));
          foreach (FileInfo file in dirInfo.GetFiles()) {
             if (file.Name != "about.xml" && file.Name != "about.png") {
                DataRow dr = dt.NewRow();
                dr["name"] = file.Name;
                dr["skinname"] = skin_name;
                dr["creationtime"] = file.CreationTime;
                dr["updatetime"] = file.LastWriteTime;
                dt.Rows.Add(dr);
             }
          }
          ViewData["list"] = dt;
       }
       #endregion
    }
}
