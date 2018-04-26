using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers
{
    public class Category_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
       private const string WEB_VIEW = "~/Areas/admin/Views/Article/category_edit.cshtml";
       private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
       protected string channel_name = string.Empty; //频道名称
       protected int channel_id;
       private int id = 0;
       protected Model.site_channel channelModel = new Model.site_channel(); //频道实体

       protected override void OnAuthorization(AuthorizationContext filterContext) {
          base.OnAuthorization(filterContext);
          ChkAdminLevel("channel_" + this.channel_name + "_category", DTEnums.ActionEnum.View.ToString()); //检查权限
       }

       protected override void OnActionExecuting(ActionExecutingContext filterContext) {
          base.OnActionExecuting(filterContext);
          ActionResult result = View(EDIT_RESULT_VIEW);
          string _action = DTRequest.GetQueryString("action");
          this.channel_id = DTRequest.GetQueryInt("channel_id");
          this.id = DTRequest.GetQueryInt("id");

          if (this.channel_id == 0) {
             JscriptMsg("频道参数不正确！", "back");
             filterContext.Result = result;
             return;
          }
          this.channel_name = new DTcms.BLL.site_channel().GetChannelName(this.channel_id); //取得频道名称
          if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
             this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
             if (this.id == 0) {
                JscriptMsg("传输参数不正确！", "back");
                filterContext.Result = result;
                return;
             }
             if (!new DTcms.BLL.article_category().Exists(this.id)) {
                JscriptMsg("类别不存在或已被删除！", "back");
                filterContext.Result = result;
                return;
             }
          }
          ViewBag.Id = this.id.ToString();
          ViewBag.Action = this.action;
          ViewBag.ChannelId = this.channel_id.ToString();
          ViewData["channelModel"] = channelModel;
       }
        //
        // GET: /admin/Category_Edit/

       public ActionResult Index() {
          TreeBind(this.channel_id); //绑定类别
          if (action == DTEnums.ActionEnum.Edit.ToString()) {
             ShowInfo(this.id);
          }
          else {
             DTcms.Model.article_category model = new DTcms.Model.article_category();
             model.parent_id = this.id;
             model.sort_id = 99;
             ViewData["model"] = model;
          }
          return View(WEB_VIEW);
       }

        [HttpPost, ValidateInput(false)]
        public ActionResult SubmitSave(string txtContent) {
           ActionResult result = View(EDIT_RESULT_VIEW);
           if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
              ChkAdminLevel("channel_" + this.channel_name + "_category", DTEnums.ActionEnum.Edit.ToString()); //检查权限
              if (!DoEdit(this.id, txtContent)) {

                 JscriptMsg("保存过程中发生错误！", "");
                 return result;
              }
              ViewBag.ClientScript = JscriptMsg("修改类别成功！", "../category_list/index?channel_id=" + channel_id);
           }
           else //添加
            {
              ChkAdminLevel("channel_" + this.channel_name + "_category", DTEnums.ActionEnum.Add.ToString()); //检查权限
              if (!DoAdd(txtContent)) {
                 JscriptMsg("保存过程中发生错误！", "");
                 return result;
              }
              JscriptMsg("添加类别成功！", "../category_list/index?channel_id=" + channel_id);
           }
           return result;
        }

        #region 绑定类别=================================
        private void TreeBind(int _channel_id) {
           DTcms.BLL.article_category bll = new DTcms.BLL.article_category();
           DataTable dt = bll.GetList(0, _channel_id);
           List<SelectListItem> parentList = new List<SelectListItem>() {
              new SelectListItem(){ Text="无父级类别", Value="0" }
           };
           foreach (DataRow dr in dt.Rows) {
              string Id = dr["id"].ToString();
              int ClassLayer = int.Parse(dr["class_layer"].ToString());
              string Title = dr["title"].ToString().Trim();

              if (ClassLayer == 1) {
                 parentList.Add(new SelectListItem() { Text = Title, Value = Id });
              }
              else {
                 Title = "├ " + Title;
                 Title = Utils.StringOfChar(ClassLayer - 1, "　") + Title;
                 parentList.Add(new SelectListItem() { Text = Title, Value = Id });
              }
           }
           ViewData["selectListItems"] = parentList;
        }
        #endregion

        #region 赋值操作=================================
        private void ShowInfo(int _id) {
           DTcms.BLL.article_category bll = new DTcms.BLL.article_category();
           DTcms.Model.article_category model = bll.GetModel(_id);
           ViewData["model"] = model;
           //绑定类别规格
           DataSet ds = new BLL.article_spec().GetCategorySpecList(model.id);
           DataTable list = null;
           if (ds.Tables.Count > 0) {
              list = ds.Tables[0];
           }
           else {
              list = new DataTable();
           }
           ViewData["list"] = list;
        }
        #endregion

        #region 增加操作=================================
        private bool DoAdd(string txtContent) {
           try {
              DTcms.Model.article_category model = new DTcms.Model.article_category();
              DTcms.BLL.article_category bll = new DTcms.BLL.article_category();
              model.site_id = channelModel.site_id;
              model.channel_id = this.channel_id;
              model.call_index = Request.Form["txtCallIndex"].Trim();
              model.title = Request.Form["txtTitle"].Trim();
              model.parent_id = int.Parse(Request.Form["ddlParentId"]);
              model.sort_id = int.Parse(Request.Form["txtSortId"].Trim());
              model.seo_title = Request.Form["txtSeoTitle"];
              model.seo_keywords = Request.Form["txtSeoKeywords"];
              model.seo_description = Request.Form["txtSeoDescription."];
              model.link_url = Request.Form["txtLinkUrl"].Trim();
              model.img_url = Request.Form["txtImgUrl"].Trim();
              model.content = txtContent;

              #region 保存规格====================
              string[] specIdArr = Request.Form.GetValues("hide_spec_id");
              if (specIdArr != null && specIdArr.Length > 0) {
                 List<Model.article_category_spec> ls = new List<Model.article_category_spec>();
                 for (int i = 0; i < specIdArr.Length; i++) {
                    int specId = Utils.StrToInt(specIdArr[i], 0);
                    ls.Add(new Model.article_category_spec { spec_id = specId });
                 }
                 model.category_specs = ls;
              }
              #endregion
              if (bll.Add(model) > 0) {
                 AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加" + this.channel_name + "频道栏目分类:" + model.title); //记录日志
                 return true;
              }
           }
           catch {
              return false;
           }
           return false;
        }
        #endregion

        #region 修改操作=================================
        private bool DoEdit(int _id, string txtContent) {
           try {
              DTcms.BLL.article_category bll = new DTcms.BLL.article_category();
              DTcms.Model.article_category model = bll.GetModel(_id);

              int parentId = int.Parse(Request.Form["ddlParentId"]);
              model.site_id = channelModel.site_id;
              model.channel_id = this.channel_id;
              model.call_index = Request.Form["txtCallIndex"].Trim();
              model.title = Request.Form["txtTitle"].Trim();
              //如果选择的父ID不是自己,则更改
              if (parentId != model.id) {
                 model.parent_id = parentId;
              }
              model.sort_id = int.Parse(Request.Form["txtSortId"].Trim());
              model.seo_title = Request.Form["txtSeoTitle"];
              model.seo_keywords = Request.Form["txtSeoKeywords"];
              model.seo_description = Request.Form["txtSeoDescription"];
              model.link_url = Request.Form["txtLinkUrl"].Trim();
              model.img_url = Request.Form["txtImgUrl"].Trim();
              model.content = txtContent;

              #region 保存规格====================
              if (model.category_specs != null) {
                 model.category_specs.Clear();
              }
              string[] specIdArr = Request.Form.GetValues("hide_spec_id");
              if (specIdArr != null && specIdArr.Length > 0) {
                 List<Model.article_category_spec> ls = new List<Model.article_category_spec>();
                 for (int i = 0; i < specIdArr.Length; i++) {
                    int specId = Utils.StrToInt(specIdArr[i], 0);
                    ls.Add(new Model.article_category_spec { category_id = model.id, spec_id = specId });
                 }
                 model.category_specs = ls;
              }
              #endregion
              if (bll.Update(model)) {
                 AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改" + this.channel_name + "频道栏目分类:" + model.title); //记录日志
                 return true;
              }
           }
           catch {
              return false;
           }
           return false;
        }
        #endregion
    }
}
