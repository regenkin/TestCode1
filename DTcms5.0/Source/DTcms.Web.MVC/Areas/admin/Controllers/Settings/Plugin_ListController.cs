using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Xml;
using DTcms.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Plugin_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Settings/Plugin_List.cshtml";

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("sys_plugin_config", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

        //
        // GET: /admin/Plugin_List/

       public ActionResult Index() {
          RptBind();
          return View(WEB_VIEW);
       }

       public ActionResult SubmitInstall() {
          ChkAdminLevel("sys_plugin_config", DTEnums.ActionEnum.Instal.ToString()); //检查权限
          //插件目录
          string pluginPath = Utils.GetMapPath("~/plugins/");
          DTcms.BLL.plugin_extension bll = new DTcms.BLL.plugin_extension();
          JObject jobject = JObject.Parse(Request.Form["json"]);
          JToken list = jobject["list"];
          foreach (JToken item in list) {
             string currDirName = item["id"].ToString();
             //是否未安装
             Model.plugin model = bll.GetInfo(pluginPath + currDirName + @"\");
             if (model.isload == 0) {
                //安装DLL
                string currPath = pluginPath + currDirName + @"\bin\";
                if (Directory.Exists(currPath)) {
                   string[] file = Directory.GetFiles(currPath);
                   foreach (string f in file) {
                      FileInfo info = new FileInfo(f);
                      //复制DLL文件
                      if (info.Extension.ToLower() == ".dll") {
                         //移动到站点目录下
                         string newFile = Utils.GetMapPath(sysConfig.webpath + @"bin\" + info.Name);
                         System.IO.File.Copy(info.FullName, newFile, true);
                      }
                   }
                }
                //执行SQL语句
                bll.ExeSqlStr(pluginPath + currDirName + @"\", @"plugin/install");
                //添加URL映射
                bll.AppendNodes(pluginPath + currDirName + @"\", @"plugin/urls");
                //添加后台导航记录
                bll.AppendMenuNodes(string.Format("{0}/", currDirName), pluginPath + currDirName + @"\", @"plugin/menu", "sys_plugin_manage");
                //遍历插件模板目录下的站点目录，生成模板
                DirectoryInfo tempPath = new DirectoryInfo(pluginPath + currDirName + @"\templet\"); //插件模板目录实例
                foreach (DirectoryInfo dirInfo in tempPath.GetDirectories()) {
                   //如果该站点目录存在则生成模板
                   if (Directory.Exists(Utils.GetMapPath(sysConfig.webpath + DTKeys_Extension.DIRECTORY_REWRITE_MVC + "/" + dirInfo.Name + "/"))) {
                      bll.MarkTemplet(sysConfig.webpath, "plugins/" + currDirName + "/templet", dirInfo.Name, pluginPath + currDirName + @"\", @"plugin/urls");

                   }
                }
                //修改plugins节点
                bll.UpdateNodeValue(pluginPath + currDirName + @"\", @"plugin/isload", "1");
             }
          }
          AddAdminLog(DTEnums.ActionEnum.Instal.ToString(), "安装插件"); //记录日志
          string script = JscriptMsg("插件安装成功！", "index", "parent.loadMenuTree");
          return Content(script);
       }

       public ActionResult SubmitUninstall() {
          ChkAdminLevel("sys_plugin_config", DTEnums.ActionEnum.UnLoad.ToString()); //检查权限
          //插件目录
          string pluginPath = Utils.GetMapPath("~/plugins/");
          DTcms.BLL.plugin_extension bll = new DTcms.BLL.plugin_extension();
          JObject jobject = JObject.Parse(Request.Form["json"]);
          JToken list = jobject["list"];
          foreach (JToken item in list) {
             string currDirName = item["id"].ToString();
             //是否已安装
             Model.plugin model = bll.GetInfo(pluginPath + currDirName + @"\");
             if (model.isload == 1) {
                string currPath = pluginPath + currDirName + @"/bin/";
                if (Directory.Exists(currPath)) {
                   string[] file = Directory.GetFiles(currPath);
                   foreach (string f in file) {
                      FileInfo info = new FileInfo(f);
                      //复制DLL文件
                      if (info.Extension.ToLower() == ".dll") {
                         //删除站点目录下DLL文件
                         string newFile = Utils.GetMapPath(sysConfig.webpath + @"bin/" + info.Name);
                         if (System.IO.File.Exists(newFile)) {
                            System.IO.File.Delete(newFile);
                         }
                      }
                   }

                }
                //执行SQL语句
                bll.ExeSqlStr(pluginPath + currDirName + @"\", @"plugin/uninstall");
                //删除URL映射
                bll.RemoveNodes(pluginPath + currDirName + @"\", @"plugin/urls");
                //删除后台导航记录
                bll.RemoveMenuNodes(pluginPath + currDirName + @"\", @"plugin/menu");
                //删除站点目录下的aspx文件
                RemoveTemplates(currDirName);
                //修改plugins节点
                bll.UpdateNodeValue(pluginPath + currDirName + @"\", @"plugin/isload", "0");
             }
          }
          AddAdminLog(DTEnums.ActionEnum.UnLoad.ToString(), "卸载插件"); //记录日志
          string script = JscriptMsg("插件卸载成功！", "index", "parent.loadMenuTree");
          return Content(script);
       }

       public ActionResult Remark() {
          string result = string.Empty;
          ChkAdminLevel("sys_plugin_config", DTEnums.ActionEnum.Build.ToString()); //检查权限
          //插件目录
          string pluginPath = Utils.GetMapPath("~/plugins/");
          DTcms.BLL.plugin_extension bll = new DTcms.BLL.plugin_extension();
          JObject jobject = JObject.Parse(Request.Form["json"]);
          JToken list = jobject["list"];
          foreach (JToken item in list) {
             string currDirName = item["id"].ToString();
             //是否已安装
             Model.plugin model = bll.GetInfo(pluginPath + currDirName + @"\");
             if (model.isload == 1) {
                //遍历插件模板目录下的站点目录
                DirectoryInfo tempPath = new DirectoryInfo(Utils.GetMapPath("~/plugins/" + currDirName + "/templet/")); //插件模板目录实例
                foreach (DirectoryInfo dirInfo in tempPath.GetDirectories()) {
                   //如果该站点目录存在则生成模板
                   if (Directory.Exists(Utils.GetMapPath(sysConfig.webpath + DTKeys.DIRECTORY_REWRITE_ASPX + "/" + dirInfo.Name + "/"))) {
                      bll.MarkTemplet(sysConfig.webpath, "plugins/" + currDirName + "/templet", dirInfo.Name, pluginPath + currDirName + @"\", @"plugin/urls");
                   }
                }
             }
             else {
                result = JscriptMsg("该插件尚未安装！", "index");
                return Content(result);
             }
          }

    
          AddAdminLog(DTEnums.ActionEnum.Build.ToString(), "生成插件模板"); //记录日志
          result = JscriptMsg("生成模板成功！", "index");
          return Content(result);
       }

        #region 插件列表绑定==============================
        private void RptBind() {
           List<DTcms.Model.plugin> lt = new List<DTcms.Model.plugin>();
           DTcms.BLL.plugin bll = new DTcms.BLL.plugin();
           List<DTcms.Model.plugin> list = bll.GetList(Utils.GetMapPath("~/plugins/"));
           ViewData["list"] = list;
        }
        #endregion

        #region 删除生成的ASPX文件========================
        private void RemoveTemplates(string dirName) {
           string fullPath = Utils.GetMapPath("~/plugins/" + dirName + "/"); //插件目录物理路径
           DirectoryInfo tempPath = new DirectoryInfo(fullPath + @"templet\"); //插件模板目录实例
           XmlNodeList xnList = XmlHelper.ReadNodes(fullPath + DTKeys.FILE_PLUGIN_XML_CONFING, "plugin/urls");
           if (xnList.Count > 0) {
              foreach (XmlElement xe in xnList) {
                 if (xe.NodeType != XmlNodeType.Comment && xe.Name.ToLower() == "rewrite" && xe.Attributes["page"] != null) {
                    if (xe.Attributes["name"] != null && xe.Attributes["type"] != null) {
                       //遍历插件模板目录下的站点目录
                       foreach (DirectoryInfo dirInfo in tempPath.GetDirectories()) {
                          //删除对应站点下的aspx文件
                          FileHelper.DeleteFile(sysConfig.webpath + DTKeys.DIRECTORY_REWRITE_ASPX + "/" + dirInfo.Name + "/" + xe.Attributes["page"].Value);
                       }
                    }
                 }
              }
           }
        }
        #endregion
    }
}
