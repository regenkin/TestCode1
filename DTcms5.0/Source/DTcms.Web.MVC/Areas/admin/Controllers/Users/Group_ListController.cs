using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Users
{
    public class Group_ListController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       public string keywords = string.Empty;
       private string viewurl = "~/Areas/admin/Views/Users/Group_List.cshtml";
       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("user_group", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          this.keywords = DTRequest.GetQueryString("keywords");
          ViewBag.Keywords = this.keywords;
       }

        //
        // GET: /admin/Group_List/
       public ActionResult Index() {
          RptBind("id>0" + CombSqlTxt(this.keywords));
          return View(viewurl);
       }

        #region 数据绑定=================================
        private void RptBind(string _strWhere) {
           DTcms.BLL.user_groups bll = new DTcms.BLL.user_groups();
           ViewData["list"] = bll.GetList(0, _strWhere, "grade asc,id asc").Tables[0];
        }
        #endregion

        #region 组合SQL查询语句==========================
        protected string CombSqlTxt(string _keywords) {
           StringBuilder strTemp = new StringBuilder();
           _keywords = _keywords.Replace("'", "");
           if (!string.IsNullOrEmpty(_keywords)) {
              strTemp.Append(" and title like '%" + _keywords + "%'");
           }
           return strTemp.ToString();
        }
        #endregion

        //批量删除
        public ActionResult SubmitDelete (string json) {
           ChkAdminLevel("user_group", DTEnums.ActionEnum.Delete.ToString()); //检查权限
           int sucCount = 0;
           int errorCount = 0;
           DTcms.BLL.user_groups bll = new DTcms.BLL.user_groups();
           //批量删除
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
           AddAdminLog(DTEnums.ActionEnum.Delete.ToString(), "删除用户组成功" + sucCount + "条，失败" + errorCount + "条"); //记录日志
           string script = JscriptMsg("删除成功" + sucCount + "条，失败" + errorCount + "条！",
               Utils.CombUrlTxt("index", "keywords={0}", this.keywords));
           return Content(script);
        }
    }
}
