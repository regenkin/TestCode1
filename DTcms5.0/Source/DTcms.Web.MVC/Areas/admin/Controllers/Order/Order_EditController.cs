using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.admin.Controllers.Order {
   public class Order_EditController : DTcms.Web.MVC.UI.Controllers.ManageController {
      private const string WEB_VIEW = "~/Areas/admin/Views/Order/order_edit.cshtml";
      private int id = 0;
      protected DTcms.Model.orders model = new DTcms.Model.orders();
      protected override void OnAuthorization(AuthorizationContext filterContext) {
         base.OnAuthorization(filterContext);
         ChkAdminLevel("order_list", DTEnums.ActionEnum.View.ToString()); //检查权限
      }

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         ActionResult result = View(EDIT_RESULT_VIEW);
         this.id = DTRequest.GetQueryInt("id");
         if (this.id == 0) {
            ViewBag.ClientScript = JscriptMsg("传输参数不正确！", "back");
            filterContext.Result = result;
            return;
         }
         if (!new DTcms.BLL.orders().Exists(this.id)) {
            ViewBag.ClientScript = JscriptMsg("记录不存在或已被删除！", "back");
            filterContext.Result = result;
            return;
         }
         ViewBag.Id = this.id.ToString();
      }
      //
      // GET: /admin/Order_Edit/

      public ActionResult Index() {
         ShowInfo(this.id);
         return View(WEB_VIEW);
      }

      #region 赋值操作=================================
      private void ShowInfo(int _id) {
         DTcms.BLL.orders bll = new DTcms.BLL.orders();
         model = bll.GetModel(_id);
         ViewData["model"] = model;
         //获得会员信息
         if (model.user_id > 0) {
            DTcms.Model.users user_info = new DTcms.BLL.users().GetModel(model.user_id);
            ViewData["userModel"] = user_info;
            if (user_info != null) {
               DTcms.Model.user_groups group_info = new DTcms.BLL.user_groups().GetModel(user_info.group_id);
               ViewData["groupModel"] = group_info;
            }
            /*//根据订单状态，显示各类操作按钮
            switch (model.status) {
               case 1: //如果是线下支付，支付状态为0，如果是线上支付，支付成功后会自动改变订单状态为已确认
                  if (model.payment_status > 0) {
                     //确认付款、取消订单、修改收货按钮显示
                     btnPayment.Visible = btnCancel.Visible = btnEditAcceptInfo.Visible = true;
                  }
                  else {
                     //确认订单、取消订单、修改收货按钮显示
                     btnConfirm.Visible = btnCancel.Visible = btnEditAcceptInfo.Visible = true;
                  }
                  //修改订单备注、修改商品总金额、修改配送费用、修改支付手续费、修改发票税金按钮显示
                  btnEditRemark.Visible = btnEditRealAmount.Visible = btnEditExpressFee.Visible = btnEditPaymentFee.Visible = btnEditInvoiceTaxes.Visible = true;
                  break;
               case 2: //如果订单为已确认状态，则进入发货状态
                  if (model.express_status == 1) {
                     //确认发货、取消订单、修改收货信息按钮显示
                     btnExpress.Visible = btnCancel.Visible = btnEditAcceptInfo.Visible = true;
                  }
                  else if (model.express_status == 2) {
                     //完成订单、取消订单按钮可见
                     btnComplete.Visible = btnCancel.Visible = true;
                  }
                  //修改订单备注按钮可见
                  btnEditRemark.Visible = true;
                  break;
               case 3:
                  //作废订单、修改订单备注按钮可见
                  btnInvalid.Visible = btnEditRemark.Visible = true;
                  break;
            }*/
            //根据订单状态和物流单号跟踪物流信息
            if (model.express_status == 2 && model.express_no.Trim().Length > 0) {
               DTcms.Model.express modelt = new DTcms.BLL.express().GetModel(model.express_id);
               DTcms.Model.orderconfig orderConfig = new DTcms.BLL.orderconfig().loadConfig();
            }

         }
      #endregion
      }
   }
}
