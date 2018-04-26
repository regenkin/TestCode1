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
   public class Role_ListController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Manager/Role_List.cshtml";
      protected string keywords = string.Empty;

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("manager_role", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         this.keywords = DTRequest.GetQueryString("keywords");
         ViewBag.Keywords = this.keywords;
      }
      //
      // GET: /admin/Role_List/

      public ActionResult Index() {
         DTcms.Model.manager model = GetAdminInfo(); //取得当前管理员信息
         RptBind("role_type>=" + model.role_type + CombSqlTxt(this.keywords));
         return View(WEB_VIEW);
      }

      [HttpPost]
      public ActionResult SubmitSave(string json) {
         ChkAdminLevel("manager_role", DTEnums.ActionEnum.Delete.ToString()); //检查权限
         int sucCount = 0; //成功数量
         int errorCount = 0; //失败数量
         DTcms.BLL.manager_role bll = new DTcms.BLL.manager_role();
         JObject jobject = JObject.Parse(json);
         JToken record = jobject["list"];
         int id;
         foreach (JToken item in record) {
            if (int.TryParse(item["id"].ToString(), out id)) {
               if (bll.Delete(id)) {
                  sucCount += 1;
               }
               else {
                  errorCount += 1;
               }
            }
         }
         AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除管理角色" + sucCount + "条，失败" + errorCount + "条"); //记录日志
         ViewBag.ClientScript = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！", Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
         return View(EDIT_RESULT_VIEW);
      }

      #region 数据绑定=================================
      private void RptBind(string _strWhere) {
         DTcms.BLL.manager_role bll = new DTcms.BLL.manager_role();
         DataTable list = bll.GetList(_strWhere).Tables[0];
         ViewData["list"] = list;
      }
      #endregion

      #region 组合SQL查询语句==========================
      protected string CombSqlTxt(string _keywords) {
         StringBuilder strTemp = new StringBuilder();
         _keywords = _keywords.Replace("'", "");
         if (!string.IsNullOrEmpty(_keywords)) {
            strTemp.Append(" and role_name like '%" + _keywords + "%'");
         }

         return strTemp.ToString();
      }
      #endregion

      #region 返回角色类型名称=========================
      protected string GetTypeName(int role_type) {
         string str = "";
         switch (role_type) {
            case 1:
               str = "超级用户";
               break;
            default:
               str = "系统用户";
               break;
         }
         return str;
      }
      #endregion
   }
}
