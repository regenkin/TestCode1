using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.IO.Compression;
using System.Text;
using DTcms.Common;

namespace DTcms.Web.MVC.UI.Controllers
{
    public class ManageController : Controller
    {
       protected internal DTcms.Model.sysconfig sysConfig;
       public const string EDIT_RESULT_VIEW = "~/Areas/admin/Views/Shared/EditResult.cshtml";

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          sysConfig = new DTcms.BLL.sysconfig().loadConfig();
          ViewData["sysConfig"] = sysConfig;
          base.OnAuthorization(filterContext);
          if (!IsAdminLogin()) {
             filterContext.Result = Redirect(sysConfig.webpath + sysConfig.webmanagepath + "/login/index");
             return;
          }
       }

        #region 管理员
        public bool IsAdminLogin() {
           //如果Session为Null
           if (System.Web.HttpContext.Current.Session[DTKeys.SESSION_ADMIN_INFO] != null) {
              return true;
           }
           else {
              //检查Cookies
              string adminname = Utils.GetCookie("AdminName", "DTcms");
              string adminpwd = Utils.GetCookie("AdminPwd", "DTcms");
              if (adminname != "" && adminpwd != "") {
                 DTcms.BLL.manager bll = new DTcms.BLL.manager();
                 DTcms.Model.manager model = bll.GetModel(adminname, adminpwd);
                 if (model != null) {
                    System.Web.HttpContext.Current.Session[DTKeys.SESSION_ADMIN_INFO] = model;
                    return true;
                 }
              }
           }
           return false;
        }

        /// <summary>
        /// 取得管理员信息
        /// </summary>
        public DTcms.Model.manager GetAdminInfo() {
           if (IsAdminLogin()) {
              DTcms.Model.manager model = System.Web.HttpContext.Current.Session[DTKeys.SESSION_ADMIN_INFO] as DTcms.Model.manager;
              if (model != null) {
                 return model;
              }
           }
           return null;
        }

        /// <summary>
        /// 检查管理员权限
        /// </summary>
        /// <param name="nav_name">菜单名称</param>
        /// <param name="action_type">操作类型</param>
        public bool ChkAdminLevel(string nav_name, string action_type) {
           bool result = false;
           DTcms.Model.manager model = GetAdminInfo();
           if (model != null) {
              DTcms.BLL.manager_role bll = new DTcms.BLL.manager_role();
              result = bll.Exists(model.role_id, nav_name, action_type);
           }
           if (!result) {
              string msgbox = "parent.jsdialog(\"错误提示\", \"您没有管理该页面的权限，请勿非法进入！\", \"back\")";
              Response.Write("<script type=\"text/javascript\">" + msgbox + "</script>");
              Response.End();
           }
           return result;
        }

        /// <summary>
        /// 写入管理日志
        /// </summary>
        /// <param name="action_type"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public bool AddAdminLog(string action_type, string remark) {
           if (sysConfig.logstatus > 0) {
              DTcms.Model.manager model = GetAdminInfo();
              int newId = new DTcms.BLL.manager_log().Add(model.id, model.user_name, action_type, remark);
              if (newId > 0) {
                 return true;
              }
           }
           return false;
        }

        #endregion

        #region JS提示============================================
        /// <summary>
        /// 添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        public string JscriptMsg(string msgtitle, string url) {
           string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\")";
           ViewBag.ClientScript = msbox;
           return msbox;
        }
        /// <summary>
        /// 带回传函数的添加编辑删除提示
        /// </summary>
        /// <param name="msgtitle">提示文字</param>
        /// <param name="url">返回地址</param>
        /// <param name="callback">JS回调函数</param>
        public string JscriptMsg(string msgtitle, string url, string callback) {
           string msbox = "parent.jsprint(\"" + msgtitle + "\", \"" + url + "\", " + callback + ")";
           ViewBag.ClientScript = msbox;
           return msbox;
        }
        #endregion

       /// <summary>
       /// 压缩文本(未使用)
       /// </summary>
       /// <param name="text"></param>
       /// <returns></returns>
       [HttpPost, ValidateInput(false)]
        public ActionResult CompressText(string text) {
           // convert text to bytes  
           byte[] buffer = Encoding.UTF8.GetBytes(text);
           // get a stream  
           MemoryStream ms = new MemoryStream();
           // get ready to zip up our stream  
           using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true)) {
              // compress the data into our buffer  
              zip.Write(buffer, 0, buffer.Length);
           }
           // reset our position in compressed stream to the start  
           ms.Position = 0;
           // get the compressed data  
           byte[] compressed = ms.ToArray();
           ms.Read(compressed, 0, compressed.Length);
           // prepare final data with header that indicates length  
           byte[] gzBuffer = new byte[compressed.Length + 4];
           //copy compressed data 4 bytes from start of final header  
           System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
           // copy header to first 4 bytes  
           System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
           // convert back to string and return  
           string result = Convert.ToBase64String(gzBuffer);  
           return Content(result);
        }

       /// <summary>
       /// 解压缩文本(未使用)
       /// </summary>
       /// <param name="compressedText"></param>
       /// <returns></returns>
       [HttpPost, ValidateInput(false)]
        public ActionResult UncompressText(string compressedText) {
           // get string as bytes  
           byte[] gzBuffer = Convert.FromBase64String(compressedText);
           // prepare stream to do uncompression  
           MemoryStream ms = new MemoryStream();
           // get the length of compressed data  
           int msgLength = BitConverter.ToInt32(gzBuffer, 0);
           // uncompress everything besides the header  
           ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
           // prepare final buffer for just uncompressed data  
           byte[] buffer = new byte[msgLength];
           // reset our position in stream since we're starting over  
           ms.Position = 0;
           // unzip the data through stream  
           GZipStream zip = new GZipStream(ms, CompressionMode.Decompress);
           // do the unzip  
           zip.Read(buffer, 0, buffer.Length);
           // convert back to string and return  
           string result = Encoding.UTF8.GetString(buffer);
           return Content(result);
        }
    }
}
