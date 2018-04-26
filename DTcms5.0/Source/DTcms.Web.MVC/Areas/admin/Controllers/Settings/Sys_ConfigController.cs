using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Admin.Controllers
{
   
    public partial class Sys_ConfigController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       string defaultpassword = "0|0|0|0"; //默认显示密码

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("sys_config", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

        //
        // GET: /Sys_Config/
       [ValidateInput(false)]
       public ActionResult Index() {
          ShowInfo();
          return View("~/Areas/admin/Views/settings/sys_config.cshtml");
       }

        #region 赋值操作=================================
        private void ShowInfo() {
           DTcms.BLL.sysconfig bll = new DTcms.BLL.sysconfig();
           DTcms.Model.sysconfig model = bll.loadConfig();
           if (string.IsNullOrEmpty(model.smspassword)) {
              model.smspassword = defaultpassword;
           }
           if (string.IsNullOrEmpty(model.emailpassword)) {
              model.emailpassword = defaultpassword;
           }
           ViewData["model"] = model;
           ViewBag.labSmsCount = GetSmsCount(); //取得短信数量
        }
        #endregion

        #region 获取短信数量=================================
        private string GetSmsCount() {
           string code = string.Empty;
           int count = new DTcms.BLL.sms_message().GetAccountQuantity(out code);
           if (code == "115") {
              return "查询出错：请完善账户信息";
           }
           else if (code != "100") {
              return "错误代码：" + code;
           }
           return count + " 条";
        }
        #endregion

        [HttpPost, ValidateInput(false)]
        /// <summary>
        /// 保存配置信息
        /// </summary>
        public ActionResult SubmitSave(string webcountcode, string webclosereason) {
           string result = string.Empty;
           ChkAdminLevel("sys_config", DTEnums.ActionEnum.Edit.ToString()); //检查权限
           DTcms.BLL.sysconfig bll = new DTcms.BLL.sysconfig();
           DTcms.Model.sysconfig model = bll.loadConfig();
           try {
              
              model.webname = Request.Form["webname"];
              model.weburl = Request.Form["weburl"];
              model.webcompany = Request.Form["webcompany"];
              model.webaddress = Request.Form["webaddress"];
              model.webtel = Request.Form["webtel"];
              model.webfax = Request.Form["webfax"];
              model.webmail = Request.Form["webmail"];
              model.webcrod = Request.Form["webcrod"];

              model.webpath = Request.Form["webpath"];
              model.webmanagepath = Request.Form["webmanagepath"];
              model.staticstatus = Utils.StrToInt(Request.Form["staticstatus"], 0);
              model.staticextension = Request.Form["staticextension"];
             
              model.memberstatus = Request.Form["memberstatus"].ToLower().IndexOf("true") >=0 ? 1 : 0;
              model.commentstatus = Request.Form["commentstatus"].ToLower().IndexOf("true") >=0 ? 1 : 0;
              model.logstatus = Request.Form["logstatus"].ToLower().IndexOf("true") >=0 ? 1 : 0;
              model.webstatus = Request.Form["webstatus"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
              
              model.webclosereason = webclosereason;
              model.webcountcode = webcountcode;

              model.smsapiurl = Request.Form["smsapiurl"];
              model.smsusername = Request.Form["smsusername"];
              //判断密码是否更改
              if (Request.Form["smspassword"] != "" && Request.Form["smspassword"] != defaultpassword) {
                 model.smspassword = Utils.MD5(Request.Form["smspassword"]);
              }

              model.emailsmtp = Request.Form["emailsmtp"];
              
              model.emailssl = Request.Form["emailssl"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
              
              model.emailport = Utils.StrToInt(Request.Form["emailport"], 25);
              model.emailfrom = Request.Form["emailfrom"];
              model.emailusername = Request.Form["emailusername"];
              //判断密码是否更改
              if (Request.Form["emailpassword"] != defaultpassword) {
                 model.emailpassword = DESEncrypt.Encrypt(Request.Form["emailpassword"], model.sysencryptstring);
              }

              model.emailnickname = Request.Form["emailnickname"];

              model.filepath = Request.Form["filepath"];
              model.filesave = Utils.StrToInt(Request.Form["filesave"], 2);
              model.fileextension = Request.Form["fileextension"];
              model.videoextension = Request.Form["videoextension"];
              model.attachsize = Utils.StrToInt(Request.Form["attachsize"], 0);
              model.videosize = Utils.StrToInt(Request.Form["videosize"], 0);
              model.imgsize = Utils.StrToInt(Request.Form["imgsize"], 0);
              model.imgmaxheight = Utils.StrToInt(Request.Form["imgmaxheight"], 0);
              model.imgmaxwidth = Utils.StrToInt(Request.Form["imgmaxwidth"], 0);
              model.thumbnailheight = Utils.StrToInt(Request.Form["thumbnailheight"], 0);
              model.thumbnailwidth = Utils.StrToInt(Request.Form["thumbnailwidth"], 0);
              model.thumbnailmode = Request.Form["thumbnailmode"];
              model.watermarktype = Utils.StrToInt(Request.Form["watermarktype"], 0);
              model.watermarkposition = Utils.StrToInt(Request.Form["watermarkposition"], 9);
              model.watermarkimgquality = Utils.StrToInt(Request.Form["watermarkimgquality"], 80);
              model.watermarkpic = Request.Form["watermarkpic"];
              model.watermarktransparency = Utils.StrToInt(Request.Form["watermarktransparency"], 5);
              model.watermarktext = Request.Form["watermarktext"];
              model.watermarkfont = Request.Form["watermarkfont"];
              model.watermarkfontsize = Utils.StrToInt(Request.Form["watermarkfontsize"], 12);

              model.fileserver = Request.Form["fileserver"];
              model.osssecretid = Request.Form["osssecretid"].Trim();
              model.osssecretkey = Request.Form["osssecretkey"].Trim();
              model.ossbucket = Request.Form["ossbucket"].Trim();
              model.ossendpoint = Request.Form["ossendpoint"].Trim();
              model.ossdomain = Request.Form["ossdomain"].Trim();
              bll.saveConifg(model);
              AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改系统配置信息"); //记录日志
              result = JscriptMsg("修改系统配置成功！", "index");
           }
           catch {
              result = JscriptMsg("文件写入失败，请检查文件夹权限！", "back");
           }
           return View(EDIT_RESULT_VIEW);
        }
    }
}
