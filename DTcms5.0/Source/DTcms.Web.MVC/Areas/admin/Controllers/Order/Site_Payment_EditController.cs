using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using DTcms.Common;

namespace DTcms.Web.Mvc.Areas.admin.Controllers.Order {
    public class Site_Payment_EditController : DTcms.Web.MVC.UI.Controllers.ManageController
    {
      private const string WEB_VIEW = "~/Areas/admin/Views/Order/site_payment_edit.cshtml";
      private string action = DTEnums.ActionEnum.Add.ToString(); //操作类型
      private int id = 0;
      protected DTcms.Model.site_payment model = new DTcms.Model.site_payment();

      protected PaymentKeys paymentKeys = new PaymentKeys();

      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("order_site_payment", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         string _action = DTRequest.GetQueryString("action");
         string _siteid=DTRequest.GetQueryString("siteid");
          string _paymentid=DTRequest.GetQueryString("paymentid");
         if (!string.IsNullOrEmpty(_action) && _action == DTEnums.ActionEnum.Edit.ToString()) {
            this.action = DTEnums.ActionEnum.Edit.ToString();//修改类型
            this.id = DTRequest.GetQueryInt("id");
            if (this.id == 0) {
               JscriptMsg("传输参数不正确！", "back");
               filterContext.Result = result;
               return;
            }
            if (!new BLL.payment().Exists(this.id)) {
               JscriptMsg("记录不存在或已被删除！", "back");
               filterContext.Result = result;
               return;
            }
         }
         //赋值实体
         //this.model = new BLL.site_payment().GetModel(this.id);
         ViewBag.Id = this.id.ToString();
         ViewBag.Action = this.action;
         ViewBag.siteid = _siteid;
         ViewBag.paymentid = _paymentid;
      }
      //
      // GET: /admin/Payment_Edit/

      public ActionResult Index() {
         ShowInfo(this.id);
         if (Utils.StrToInt(ViewBag.siteid, 0) > 0) model.site_id = Utils.StrToInt(ViewBag.siteid, 0);
         if (Utils.StrToInt(ViewBag.paymentid, 0) > 0) model.payment_id = Utils.StrToInt(ViewBag.paymentid, 0);
         SiteBind();//绑站点
         PaymentBind(model.site_id, model.payment_id); //绑定平台
         ShowFields(model.payment_id); //显示字段
         ViewData["model"] = model;
         ViewData["PaymentKeys"] = paymentKeys;
         return View(WEB_VIEW);
      }

      [HttpPost]
      //[ValidateInput(false)]
      public ActionResult SubmitSave() {
         ActionResult result = View(EDIT_RESULT_VIEW);
         if (action == DTEnums.ActionEnum.Edit.ToString()) //修改
            {
            ChkAdminLevel("order_site_payment", DTEnums.ActionEnum.Edit.ToString()); //检查权限
            if (!DoEdit(this.id)) {
               JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            JscriptMsg("修改站点支付方式成功！", "../site_payment_list/index");
         }
         else //添加
            {
                ChkAdminLevel("order_site_payment", DTEnums.ActionEnum.Add.ToString()); //检查权限
            if (!DoAdd()) {
               JscriptMsg("保存过程中发生错误！", string.Empty);
               return result;
            }
            JscriptMsg("添加站点支付方式成功！", "../site_payment_list/index");
         }
         return result;
      }

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
          if (_id > 0)
          {
              DTcms.BLL.site_payment bll = new DTcms.BLL.site_payment();
              model = bll.GetModel(_id);
          }
      }
      #endregion

      #region 显示相关字段=============================
      private void ShowFields(int _payment_id) {
         Model.payment payModel = new BLL.payment().GetModel(_payment_id);
         if (payModel == null) {
            return;
         }
          switch (payModel.api_path.ToLower()) {
             case "alipaypc":
             case "alipaymb":
                 paymentKeys.txtKey1_visible = true;
                 paymentKeys.txtKey1_title = "支付宝账号";
                 paymentKeys.txtKey1_tip = "*签约支付宝账号或卖家支付宝帐户";
                 paymentKeys.txtKey2_visible = true;
                 paymentKeys.txtKey2_title = "合作者身份(partner ID)";
                 paymentKeys.txtKey2_tip = "*合作身份者ID，以2088开头由16位纯数字组成的字符串";
                 paymentKeys.txtKey3_visible = true;
                 paymentKeys.txtKey3_title = "交易安全校验码(key)";
                 paymentKeys.txtKey3_tip = "*交易安全检验码，由数字和字母组成的32位字符串";
                 paymentKeys.txtKey4_visible = false;
                //txtKey4.Text = string.Empty;
                break;
             case "tenpaypc":
                paymentKeys.txtKey1_visible = true;
                paymentKeys.txtKey1_title = "财付通商户号";
                paymentKeys.txtKey1_tip = "*财付通商家服务商户号";
                paymentKeys.txtKey2_visible = true;
                paymentKeys.txtKey2_title = "财付通密钥";
                paymentKeys.txtKey2_tip = "*财付通商家服务密钥";
                paymentKeys.txtKey3_visible = false;
                //txtKey3 = string.Empty;
                paymentKeys.txtKey4_visible = false;
                ///txtKey4.Text = string.Empty;
                break;
             case "chinabankpc":
                paymentKeys.txtKey1_visible = true;
                paymentKeys.txtKey1_title = "商户编号";
                paymentKeys.txtKey1_tip = "*网银在线商户编号";
                paymentKeys.txtKey2_visible = true;
                paymentKeys.txtKey2_title = "MD5校验码";
                paymentKeys.txtKey2_tip = "*网银在线MD5校验码";
                paymentKeys.txtKey3_visible = false;
                //txtKey3.Text = string.Empty;
                paymentKeys.txtKey4_visible = false;
                //txtKey4.Text = string.Empty;
                break;
             case "wxapipay":
                paymentKeys.txtKey1_visible = true;
                paymentKeys.txtKey1_title = "商户号";
                paymentKeys.txtKey1_tip = "*微信支付的商户号";
                paymentKeys.txtKey2_visible = true;
                paymentKeys.txtKey2_title = "支付密钥";
                paymentKeys.txtKey2_tip = "*商户支付密钥，进入微信支付【账户设置】【API安全】设置";
                paymentKeys.txtKey3_visible = true;
                paymentKeys.txtKey3_title = "AppID";
                paymentKeys.txtKey3_tip = "*微信公众号的AppID(应用ID)";
                paymentKeys.txtKey4_visible = true;
                paymentKeys.txtKey4_title = "AppSecret";
                paymentKeys.txtKey4_tip = "*微信公众号的AppSecret(应用密钥)";
                break;
             case "wxnatpay":
                paymentKeys.txtKey1_visible = true;
                paymentKeys.txtKey1_title = "商户号";
                paymentKeys.txtKey1_tip = "*微信支付的商户号";
                paymentKeys.txtKey2_visible = true;
                paymentKeys.txtKey2_title = "支付密钥";
                paymentKeys.txtKey2_tip = "*商户支付密钥，进入微信支付【账户设置】【API安全】设置";
                paymentKeys.txtKey3_visible = true;
                paymentKeys.txtKey3_title = "AppID";
                paymentKeys.txtKey3_tip = "*微信公众号的AppID(应用ID)";
                paymentKeys.txtKey4_visible = false;
                //txtKey4.Text = string.Empty;
                break;
             default:
                paymentKeys.txtKey1_visible = false;
                paymentKeys.txtKey2_visible = false;
                paymentKeys.txtKey3_visible = false;
                paymentKeys.txtKey4_visible = false;
                break;
          }
      }
      #endregion

      #region 绑定站点=================================
      private void SiteBind() {
         DTcms.BLL.sites bll = new DTcms.BLL.sites();
         DataTable dt = bll.GetList(0, "is_lock=0", "sort_id asc,id desc").Tables[0];
         List<SelectListItem> siteList = new List<SelectListItem>() {
            new SelectListItem(){ Text="请选择站点...", Value="" }
         };
         foreach (DataRow dr in dt.Rows) {
            siteList.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
         }
         ViewData["siteListItems"] = siteList;

         //DTcms.BLL.payment bllPay = new DTcms.BLL.payment();
         //DataTable dtPay = bllPay.GetList(0, "is_lock=0", "sort_id asc,id desc").Tables[0];
         //List<SelectListItem> paysiteList = new List<SelectListItem>() {
         //   new SelectListItem(){ Text="请选择支付平台...", Value="" }
         //};
         //foreach (DataRow dr in dtPay.Rows)
         //{
         //    paysiteList.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
         //}
         //ViewData["PaysiteList"] = paysiteList;
      }
      #endregion

      #region 绑定平台=================================
      private void PaymentBind(int _site_id, int _payment_id) {
         List<SelectListItem> listItems = new List<SelectListItem>();
         if (_site_id > 0) {
            DataTable dt = new BLL.payment().GetList(_site_id, _payment_id).Tables[0];
            listItems.Add(  new SelectListItem(){ Text="请选择平台...", Value="" });
            foreach (DataRow dr in dt.Rows) {
               listItems.Add(new SelectListItem() { Text = dr["title"].ToString(), Value = dr["id"].ToString() });
            }
         }
         else {
             listItems.Add(new SelectListItem() { Text = "请选择平台...", Value = "" });
         }
         ViewData["PaysiteList"] = listItems;
      }
      #endregion

      #region 增加操作=================================
      private bool DoAdd() {
         bool result = false;
         this.model = new Model.site_payment();
         BLL.site_payment bll = new BLL.site_payment();
         model.site_id = Utils.StrToInt(Request.Form["ddlSiteId"],0);
         model.payment_id = Utils.StrToInt(Request.Form["ddlPaymentId"], 0);
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"], 99);
         model.title = Request.Form["txtTitle"].Trim();
         model.key1 = Request.Form["txtKey1"]??"";
         model.key2 = Request.Form["txtKey2"] ?? "";
         model.key3 = Request.Form["txtKey3"] ?? "";
         model.key4 = Request.Form["txtKey4"] ?? "";
         if (bll.Add(model) > 0) {
             AddAdminLog(DTEnums.ActionEnum.Add.ToString(), "添加站点支付方式:" + model.title); //记录日志
            result = true;
         }
         return result;
      }
      #endregion

      #region 修改操作=================================
      private bool DoEdit(int _id) {
         bool result = false;
         BLL.site_payment bll = new BLL.site_payment();
         this.model = bll.GetModel(_id);
         model.site_id = Utils.StrToInt(Request.Form["ddlSiteId"],0);
         model.payment_id = Utils.StrToInt(Request.Form["ddlPaymentId"], 0);
         model.sort_id = Utils.StrToInt(Request.Form["txtSortId"], 99);
         model.title = Request.Form["txtTitle"].Trim();
         model.key1 = Request.Form["txtKey1"] ?? "";
         model.key2 = Request.Form["txtKey2"] ?? "";
         model.key3 = Request.Form["txtKey3"] ?? "";
         model.key4 = Request.Form["txtKey4"] ?? "";
         if (bll.Update(model)) {
            AddAdminLog(DTEnums.ActionEnum.Edit.ToString(), "修改站点支付方式:" + model.title); //记录日志
            result = true;
         }
         return result;
      }
      #endregion
   }

    public class PaymentKeys
    {
        public PaymentKeys() { }
        public bool txtKey1_visible = false;
        public string txtKey1_title = "";
        public string txtKey1_tip = "";
        public bool txtKey2_visible = false;
        public string txtKey2_title = "";
        public string txtKey2_tip = "";
        public bool txtKey3_visible = false;
        public string txtKey3_title = "";
        public string txtKey3_tip = "";
        public bool txtKey4_visible = false;
        public string txtKey4_title = "";
        public string txtKey4_tip = "";
    }
}
