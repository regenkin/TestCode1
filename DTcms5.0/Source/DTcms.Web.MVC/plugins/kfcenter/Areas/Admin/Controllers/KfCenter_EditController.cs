using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;


namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class KfCenter_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
        private const string WEB_VIEW = "~/plugins/kfcenter/Areas/admin/Views/kfcenter_edit.cshtml";
        private string action = string.Empty;
        private int id = 0;
        private DTcms.Web.Mvc.Plugin.KfCenter.BLL.kfActSetBLL<DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActSet> bll = Mvc.Plugin.KfCenter.BLL.kfActSetBLL<Mvc.Plugin.KfCenter.Models.kfActSet>.Instance();

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            ChkAdminLevel("plugin_link", DTEnums.ActionEnum.View.ToString()); //检查权限
            LogHelper.ErrorLog("ErrorLog");
            LogHelper.DebugLog("DebugLog");
            LogHelper.InfoLog("InfoLog");
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ActionResult result = View(EDIT_RESULT_VIEW);
            //取到操作类型
            string _action = DTRequest.GetQueryString("action");
            if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString())
            {
                this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
                this.id = DTRequest.GetQueryInt("id");
                if (this.id == 0)
                {
                    JscriptMsg("传输参数不正确！", "back");
                    filterContext.Result = result;
                }
                if (bll.GetEntity(this.id) == null)
                {
                    JscriptMsg("内容不存在或已被删除！", "back");
                    filterContext.Result = result;
                }
            }
            else
            {
                this.id = -1;
            }
            ViewBag.Action = this.action;
            ViewBag.Id = this.id.ToString();
        }
        //
        // GET: /admin/Link_Edit/

        public ActionResult Index()
        {
            TreeBind();
            if (action == DTEnums.ActionEnum.Edit.ToString())
            {
                ShowInfo(this.id);
            }
            else
            {
                DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActSet model = bll.GetEntity(this.id);
                ViewData["model"] = model;
            }
            return View(WEB_VIEW);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult SubmitSave(string txtContent)
        {
            ActionResult result = View(EDIT_RESULT_VIEW);
            if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
                ChkAdminLevel("plugin_kfcenter", DTEnums.ActionEnum.Edit.ToString()); //检查权限
                if (!DoEdit(this.id))
                {
                    ViewBag.ClientScript = JscriptMsg("保存过程中发生错误啦！", string.Empty);
                    return result;
                }
                ViewBag.ClientScript = JscriptMsg("修改数据中心成功！", "../kfcenter_list/index");
            }
            else //添加
            {
                ChkAdminLevel("plugin_kfcenter", DTEnums.ActionEnum.Add.ToString()); //检查权限
                if (!DoAdd())
                {
                    ViewBag.ClientScript = JscriptMsg("保存过程中发生错误！", string.Empty);
                    return result;
                }
                ViewBag.ClientScript = JscriptMsg("添加数据中心成功！", "../kfcenter_list/index");
            }
            return result;
        }

        private void ShowInfo(int _id)
        {
            DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActSet model = bll.GetEntity(this.id);
            ViewData["model"] = model;
        }

        private void TreeBind()
        {
            DTcms.Web.Mvc.Plugin.KfCenter.BLL.kfActGroupBLL<DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActGroup> bll = Mvc.Plugin.KfCenter.BLL.kfActGroupBLL<Mvc.Plugin.KfCenter.Models.kfActGroup>.Instance();
            IList<DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActGroup> lsModels = bll.GetList();
            List<SelectListItem> lsAppSetGroup = new List<SelectListItem>() 
            {
                new SelectListItem(){ Text="请账套所属组...", Value=""}
            };
            foreach (DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActGroup m in lsModels)
            {
                lsAppSetGroup.Add(new SelectListItem() { Text = m.ActGroupName, Value = m.ActGroupNum});
            }
            ViewData["lsAppSetGroup"] = lsAppSetGroup;
        }

        private bool DoAdd()
        {
            bool result = false;
            DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActSet model = new DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActSet();
            ViewData["model"] = model;
            model.ActsetNum = Request.Form["txtActsetNum"].Trim();
            model.ActsetName = Request.Form["txtActsetName"].Trim();
            model.ActsetDBName = Request.Form["txtActsetDBName"].Trim();
            model.ActsetType = Request.Form["txtActSetType"].Trim();
            model.ActSetGroupKey = Request.Form["txtActSetGroupKey"].Trim();
            model.LoginType = Request.Form["txtLoginType"].Trim();
            model.LoginUserName = Request.Form["txtLoginUserName"].Trim();
            model.LoginPwd = Request.Form["txtLoginPwd"].Trim();
            model.DBServerName = Request.Form["txtDBServerName"].Trim();
            model.CreateDate = DateTime.Now;
            //model.NewBackUpDate
            model.DBGUID = Request.Form["txtDBGUID"].Trim();
            model.DBVersion = Request.Form["txtDBVersion"].Trim();
            model.Visible = Request.Form["txtVisible"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
            model.UIStyle = Utils.StrToInt(Request.Form["txtUIStyle"].Trim(), 1);
            model.LimitCount = Utils.StrToInt(Request.Form["txtLimitCount"], 0);
            model.DBMaxSize = Utils.StrToInt(Request.Form["txtLimitCount"], 0);
            model.EndDate = DateTime.Today.AddMonths(1);

            //model.site_id = Utils.StrToInt(Request.Form["ddlSiteId"], -1);
            //model.title = Request.Form["txtTitle"].Trim();
            //model.is_lock = Request.Form["rblIsLock"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
            //model.is_red = Request.Form["cbIsRed"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
            //model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 0);
            //model.user_name = Request.Form["txtUserName"].Trim();
            //model.user_tel = Request.Form["txtUserTel"].Trim();
            //model.email = Request.Form["txtEmail"].Trim();
            //model.site_url = Request.Form["txtSiteUrl"].Trim();
            //model.img_url = Request.Form["txtImgUrl"].Trim();
            //model.is_image = 1;
            //model.add_time = DateTime.Now;
            //if (string.IsNullOrEmpty(model.img_url))
            //{
            //    model.is_image = 0;
            //}
            if (bll.Insert(model))
            {
                AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加数据中心：" + model.ActsetNum);
                result = true;
            }
            return result;
        }

        private bool DoEdit(int _id)
        {
            bool result = false;
            DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActSet model = bll.GetEntity(_id);
            //model.ActsetNum = Request.Form["txtActsetNum"].Trim();
            //model.ActsetName = Request.Form["txtActsetName"].Trim();
            //model.ActsetDBName = Request.Form["txtActsetDBName"].Trim();
            //model.ActsetType = Request.Form["txtActSetType"].Trim();
            //model.ActSetGroupKey = Request.Form["txtActSetGroupKey"].Trim();
            //model.LoginType = Request.Form["txtLoginType"].Trim();
            //model.LoginUserName = Request.Form["txtLoginUserName"].Trim();
            //model.LoginPwd = Request.Form["txtLoginPwd"].Trim();
            //model.DBServerName = Request.Form["txtDBServerName"].Trim();
            //model.CreateDate = DateTime.Now;
            ////model.NewBackUpDate
            //model.DBGUID = Request.Form["txtDBGUID"].Trim();
            //model.DBVersion = Request.Form["txtDBVersion"].Trim();
            //model.Visible = Request.Form["txtVisible"].ToLower().IndexOf("true") >= 0 ? 1 : 0;
            //model.UIStyle = Utils.StrToInt(Request.Form["txtUIStyle"].Trim(), 1);
            //model.LimitCount = Utils.StrToInt(Request.Form["txtLimitCount"], 0);
            //model.DBMaxSize = Utils.StrToInt(Request.Form["txtLimitCount"], 0);
            //model.EndDate = DateTime.Today.AddMonths(1);
            if (bll.Update(model))
            {
                AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改数据中心：" + model.ActsetNum + "-" + model.ActsetName);
                result = true;
            }
            return result;
        }
    }
}
