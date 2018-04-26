using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Data;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Templet_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Settings/Templet_List.cshtml";
       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("sys_site_templet", DTEnums.ActionEnum.View.ToString()); //检查权限
       }
        //
        // GET: /admin/Templet_List/

       public ActionResult Index() {
          TreeBind(); //绑定站点
          RptBind(); //绑定模板
          return View(WEB_VIEW);
       }

       [HttpPost]
       public ActionResult SubmitBuild() {
          string script = string.Empty;
          ChkAdminLevel("sys_site_templet", DTEnums.ActionEnum.Build.ToString()); //检查权限
          string sitePath = DTRequest.GetQueryString("sitePath");
          if (sitePath == "") {
             script = JscriptMsg("请选择生成的站点", "back");
             return Content(script);
          }
          JObject jobject = JObject.Parse(Request.Form["json"]);
          JToken record = jobject["list"];
          foreach (JToken item in record) {
             string skinName = item["skinName"].ToString();
             MarkTemplates(sitePath, skinName);
             //修改当前频道分类当前模板名
             new DTcms.BLL.sites().UpdateField(sitePath, "templet_path='" + skinName + "'");
             //更新一下域名缓存
             CacheHelper.Remove(DTKeys.CACHE_SITE_HTTP_DOMAIN);
             AddAdminLog(DTEnums.ActionEnum.Build.ToString(), "生成模板:" + skinName);//记录日志
             script = JscriptMsg("生成模板成功！", "Index");
             return Content(script);
          }
          script = JscriptMsg("请选择生成的模板！", "back");
          return Content(script);
       }

       #region 绑定站点=================================
       private void TreeBind() {
          DTcms.BLL.sites bll = new DTcms.BLL.sites();
          DataTable dt = bll.GetList(0, "", "sort_id asc,id desc").Tables[0];
          List<SelectListItem> siteList = new List<SelectListItem>();
          siteList.Add(new SelectListItem() { Text = "生成模板到", Value = "" });
          foreach (DataRow dr in dt.Rows) {
             if (string.IsNullOrEmpty(dr["templet_path"].ToString())) {
                siteList.Add(new SelectListItem() { Text = "├ " + dr["title"].ToString(), Value = dr["build_path"].ToString() });
             }
             else {
                siteList.Add(new SelectListItem() { Text = "├ " + dr["title"].ToString() + "(当前模板：" + dr["templet_path"].ToString() + ")", Value = dr["build_path"].ToString() });
             }
          }
          ViewData["siteListItems"] = siteList;
       }
       #endregion

       #region 数据绑定=================================
       private void RptBind() {
          DataTable dt = new DataTable();
          dt.Columns.Add("skinname", Type.GetType("System.String"));
          dt.Columns.Add("name", Type.GetType("System.String"));
          dt.Columns.Add("img", Type.GetType("System.String"));
          dt.Columns.Add("author", Type.GetType("System.String"));
          dt.Columns.Add("createdate", Type.GetType("System.String"));
          dt.Columns.Add("version", Type.GetType("System.String"));
          dt.Columns.Add("fordntver", Type.GetType("System.String"));

          DirectoryInfo dirInfo = new DirectoryInfo(Utils.GetMapPath("~/templates/"));
          foreach (DirectoryInfo dir in dirInfo.GetDirectories()) {
             DataRow dr = dt.NewRow();
             DTcms.Model.template model = GetInfo(dir.FullName);
             if (model != null) {
                dr["skinname"] = dir.Name;  //文件夹名称
                dr["name"] = model.name;    // 模板名称
                dr["img"] = "~/templates/" + dir.Name + "/about.png";   // 模板图片
                dr["author"] = model.author;    //作者
                dr["createdate"] = model.createdate;    //创建日期
                dr["version"] = model.version;  //模板版本
                dr["fordntver"] = model.fordntver;  //适用的版本
                dt.Rows.Add(dr);
             }
          }
          ViewData["list"] = dt;
       }
       #endregion

       #region 读取模板配置信息=========================
       /// <summary>
       /// 从模板说明文件中获得模板说明信息
       /// </summary>
       /// <param name="xmlPath">模板路径(不包含文件名)</param>
       /// <returns>模板说明信息</returns>
       private DTcms.Model.template GetInfo(string xmlPath) {
          DTcms.Model.template model = new DTcms.Model.template();
          ///存放关于信息的文件 about.xml是否存在,不存在返回空串
          if (!System.IO.File.Exists(xmlPath + @"\about.xml")) {
             return null;
          }
          try {
             XmlNodeList xnList = XmlHelper.ReadNodes(xmlPath + @"\about.xml", "about");
             foreach (XmlNode n in xnList) {
                if (n.NodeType != XmlNodeType.Comment && n.Name.ToLower() == "template") {
                   model.name = n.Attributes["name"] != null ? n.Attributes["name"].Value.ToString() : "";
                   model.author = n.Attributes["author"] != null ? n.Attributes["author"].Value.ToString() : "";
                   model.createdate = n.Attributes["createdate"] != null ? n.Attributes["createdate"].Value.ToString() : "";
                   model.version = n.Attributes["version"] != null ? n.Attributes["version"].Value.ToString() : "";
                   model.fordntver = n.Attributes["fordntver"] != null ? n.Attributes["fordntver"].Value.ToString() : "";
                }
             }
          }
          catch {
             return null;
          }
          return model;
       }
       #endregion

       #region 全部生成模板=============================
       /// <summary>
       /// 生成全部模板
       /// </summary>
       private void MarkTemplates(string buildPath, string skinName) {
          //获得URL配置列表
          DTcms.BLL.url_rewrite bll = new DTcms.BLL.url_rewrite();
          List<DTcms.Model.url_rewrite> ls = bll.GetList("");
          //设置文件夹路径
          string webViewPath = Utils.GetMapPath(string.Format("{0}Areas/Web/Views/{1}/", sysConfig.webpath, skinName));
          string webTempletPath = Utils.GetMapPath(string.Format("{0}templates/{1}/", sysConfig.webpath, skinName));
          //删除分站下原有视图文件
          if (Directory.Exists(webViewPath)) {
             Directory.Delete(webViewPath, true);
          }
          //复制网页模板文件夹
          DirectoryCopy(webTempletPath, webViewPath);
          //复制网页模板共享文件夹(Shared)下的所有文件
          DirectoryCopy(webTempletPath + "Shared\\", webViewPath + "Shared\\");
       }

       /// <summary>
       /// 复制文件夹
       /// </summary>
       /// <param name="sourceDirectory">源文件夹路径</param>
       /// <param name="targetDirectory">目标文件夹路径</param>
       private void DirectoryCopy(string sourceDirectory, string targetDirectory) {
          if (!Directory.Exists(sourceDirectory)) {
             return;
          }
          if (!Directory.Exists(targetDirectory)) {
             Directory.CreateDirectory(targetDirectory);
          }
          DirectoryInfo sourceInfo = new DirectoryInfo(sourceDirectory);
          System.IO.FileInfo[] filesInfo = sourceInfo.GetFiles();
          foreach(System.IO.FileInfo fileInfo in filesInfo){
             System.IO.File.Copy(sourceDirectory + fileInfo.Name, targetDirectory + fileInfo.Name, true);
          }
       }

       private string GetTemplate(string templateFullName, string viewFullPath, string viewPage) {
          if (!System.IO.File.Exists(templateFullName)) {
             return "";
          }
          System.Text.StringBuilder template = new System.Text.StringBuilder();
              
          //开始读取模板文件
          using(StreamReader reader = new StreamReader(templateFullName, System.Text.Encoding.UTF8)){
             template.Append(reader.ReadToEnd());
          }
          string viewFullName = viewFullPath + viewPage;
          if(!Directory.Exists(viewFullPath)){
             Directory.CreateDirectory(viewFullPath);
          }
          System.IO.File.WriteAllText(viewFullName, template.ToString(), System.Text.Encoding.UTF8);
          return template.ToString();
       }
       #endregion
    }
}
