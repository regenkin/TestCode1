using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers {
   public partial class Spec_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Article/spec_edit.cshtml";
      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      protected string channel_name = string.Empty; //频道名称
      protected int channel_id;
      private int id = 0;

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW); 
         string _action = DTRequest.GetQueryString("action");
         this.channel_id = DTRequest.GetQueryInt("channel_id");
         if (this.channel_id == 0) {
            JscriptMsg("频道参数不正确！", "back");
            filterContext.Result = result;
            return;
         }
         this.channel_name = new BLL.site_channel().GetChannelName(this.channel_id); //取得频道名称
         this.id = DTRequest.GetQueryInt("id");
         if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
            this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
            if (this.id == 0) {
               JscriptMsg("传输参数不正确！", "back");
               filterContext.Result = result;
               return;
            }
            if (!new BLL.article_spec().Exists(this.id)) {
               JscriptMsg("记录不存在或已删除！", "back");
               filterContext.Result = result;
               return;
            }
         }
         ViewBag.ChannelId = this.channel_id;
         ViewBag.ChannelName = this.channel_name;
         ViewBag.Action = this.action;
         ViewBag.Id = this.id.ToString();
      }

      //
      // GET: /admin/Spec_Edit/
      public ActionResult Index() {
         ChkAdminLevel("channel_" + this.channel_name + "_spec", DTEnums.ActionEnum.View.ToString()); //检查权限
         if (action == DTEnums.ActionEnum.Edit.ToString()) {
            ShowInfo(this.id);
         }
         else {
            DTcms.Model.article_spec model = new DTcms.Model.article_spec();
            model.sort_id = 99;
            ViewData["model"] = model;
         }
         return View(WEB_VIEW);
      }

      //保存
      public ActionResult SubmitSave() {
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("channel_" + this.channel_name + "_spec", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id)) {
               JscriptMsg("保存过程中发生错误！", "");
            }
            else {
               JscriptMsg("修改规格成功！", "../spec_list/index");
            }
         }
         else //添加
            {
            ChkAdminLevel("channel_" + this.channel_name + "_spec", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd()) {
               JscriptMsg("保存过程中发生错误！", "");
            }
            else {
               JscriptMsg("添加规格成功！", "../spec_list/index");
            }
         }
         return View(EDIT_RESULT_VIEW);
      }

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
         BLL.article_spec bll = new BLL.article_spec();
         Model.article_spec model = bll.GetModel(_id);
         ViewData["model"] = model;
      }
      #endregion

      #region 增加操作=================================
      private bool DoAdd() {
         bool result = false;
         Model.article_spec model = new Model.article_spec();
         BLL.article_spec bll = new BLL.article_spec();

         model.channel_id = this.channel_id;
         model.title = Request.Form["txtTitle"].Trim();
         model.remark = Request.Form["txtRemark"].Trim();
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         #region 保存规格选项
         string[] itemIdArr = Request.Form.GetValues("item_id");
         string[] itemTitleArr = Request.Form.GetValues("item_title");
         string[] itemImgUrlArr = Request.Form.GetValues("item_imgurl");
         string[] itemSortIdArr = Request.Form.GetValues("item_sortid");
         if (itemIdArr != null && itemTitleArr != null && itemImgUrlArr != null && itemSortIdArr != null) {
            if ((itemIdArr.Length == itemTitleArr.Length) && (itemTitleArr.Length == itemImgUrlArr.Length) && (itemImgUrlArr.Length == itemSortIdArr.Length)) {
               List<Model.article_spec_value> ls = new List<Model.article_spec_value>();
               for (int i = 0; i < itemIdArr.Length; i++) {
                  Model.article_spec_value modelt = new Model.article_spec_value();
                  modelt.id = Utils.StrToInt(itemIdArr[i].Trim(), 0);
                  modelt.title = itemTitleArr[i].Trim();
                  modelt.img_url = itemImgUrlArr[i].Trim();
                  modelt.sort_id = Utils.StrToInt(itemSortIdArr[i].Trim(), 99);
                  ls.Add(modelt);
               }
               model.values = ls;
            }
         }
         #endregion

         if (bll.Add(model) > 0) {
            AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加商品规格:" + model.title); //记录日志
            result = true;
         }
         return result;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id) {
         bool result = false;
         BLL.article_spec bll = new BLL.article_spec();
         Model.article_spec model = bll.GetModel(_id);

         model.title = Request.Form["txtTitle"].Trim();
         model.remark = Request.Form["txtRemark"].Trim();
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"].Trim(), 99);

         #region 保存规格选项
         string[] itemIdArr = Request.Form.GetValues("item_id");
         string[] itemTitleArr = Request.Form.GetValues("item_title");
         string[] itemImgUrlArr = Request.Form.GetValues("item_imgurl");
         string[] itemSortIdArr = Request.Form.GetValues("item_sortid");
         if (itemIdArr != null && itemTitleArr != null && itemImgUrlArr != null && itemSortIdArr != null) {
            if ((itemIdArr.Length == itemTitleArr.Length) && (itemTitleArr.Length == itemImgUrlArr.Length) && (itemImgUrlArr.Length == itemSortIdArr.Length)) {
               List<Model.article_spec_value> ls = new List<Model.article_spec_value>();
               for (int i = 0; i < itemIdArr.Length; i++) {
                  Model.article_spec_value modelt = new Model.article_spec_value();
                  modelt.id = Utils.StrToInt(itemIdArr[i].Trim(), 0);
                  modelt.parent_id = model.id;
                  modelt.title = itemTitleArr[i].Trim();
                  modelt.img_url = itemImgUrlArr[i].Trim();
                  modelt.sort_id = Utils.StrToInt(itemSortIdArr[i].Trim(), 99);
                  ls.Add(modelt);
               }
               model.values = ls;
            }
         }
         #endregion

         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改商品规格:" + model.title); //记录日志
            result = true;
         }
         return result;
      }
      #endregion
   }
}
