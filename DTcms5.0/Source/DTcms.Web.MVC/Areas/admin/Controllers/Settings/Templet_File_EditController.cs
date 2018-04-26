using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Templet_File_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Settings/templet_file_edit.cshtml";
       protected string filePath; //文件路径
       protected string pathName; //模板目录
       protected string fileName; //文件名称

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("sys_site_templet", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          ActionResult result = View(EDIT_RESULT_VIEW);
          pathName = DTRequest.GetQueryString("path");
          fileName = DTRequest.GetQueryString("filename");

          if (string.IsNullOrEmpty(pathName) || string.IsNullOrEmpty(fileName)) {
             JscriptMsg("传输参数不正确！", "back");
             filterContext.Result = result;
             return;
          }
          filePath = Utils.GetMapPath(@"~/templates/" + pathName.Replace(".", "") + "/" + fileName.Replace("/", ""));
          if (!System.IO.File.Exists(filePath)) {
             JscriptMsg("该文件不存在！", "back");
             filterContext.Result = result;
             return;
          }
          ViewBag.PathName = pathName;
          ViewBag.FileName = fileName;
       }
        //
        // GET: /admin/Templet_File_Edit/

       public ActionResult Index() {
          ShowInfo(filePath);
          return View(WEB_VIEW);
       }

       [ValidateInput(false)]
       public ActionResult SubmitSave(string txtContent) {
          ActionResult result = View(EDIT_RESULT_VIEW);
          ChkAdminLevel("sys_site_templet", DTEnums.ActionEnum.Edit.ToString()); //检查权限
          if (!System.IO.File.Exists(filePath)) {
             JscriptMsg("模板文件不存在！", "back");
             return result;
          }
          using (FileStream fs = new FileStream(this.filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)) {
             Byte[] info = Encoding.UTF8.GetBytes(txtContent);
             fs.Write(info, 0, info.Length);
             fs.Close();
          }
          AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改模板文件:" + this.fileName);//记录日志
          JscriptMsg("模板保存成功！", Utils.CombUrlTxt("../templet_file_list/index", "skin={0}", this.pathName));
          return result;
       }

       #region 赋值操作=================================
       private void ShowInfo(string _path) {
          using (StreamReader objReader = new StreamReader(_path, Encoding.UTF8)) {
             ViewBag.Content = objReader.ReadToEnd();
          }
       }
       #endregion
    }
}
