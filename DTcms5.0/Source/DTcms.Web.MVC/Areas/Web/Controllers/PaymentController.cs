using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;
using DTcms.Web.MVC.UI.Controllers;

namespace DTcms.Web.MVC.Areas.Web.Controllers
{
   public class PaymentController : DTcms.Web.MVC.UI.Controllers.BaseController {
      public string action = string.Empty;
      public string order_no = string.Empty;
      public string order_type = string.Empty;
      public decimal order_amount = 0;

      public DTcms.Model.orderconfig orderConfig = new DTcms.BLL.orderconfig().loadConfig(); //订单配置
      public DTcms.Model.users userModel; //用户
      public DTcms.Model.orders orderModel; //购物
      public DTcms.Model.user_recharge rechargeModel; //充值
      public DTcms.Model.payment payModel; //支付

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         //取得处事类型
         action = DTRequest.GetString("action");
         order_no = DTRequest.GetString("order_no");
         if (order_no.ToUpper().StartsWith("R")) //充值订单
            {
            order_type = DTEnums.AmountTypeEnum.Recharge.ToString().ToLower();
         }
         else if (order_no.ToUpper().StartsWith("B")) //商品订单
            {
            order_type = DTEnums.AmountTypeEnum.BuyGoods.ToString().ToLower();
         }

         switch (action) {
            case "confirm":
               if (string.IsNullOrEmpty(action) || string.IsNullOrEmpty(order_no)) {
                  Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，URL传输参数有误！")));
                  return;
               }
               //是否需要支持匿名购物
               userModel = new BaseController().GetUserInfo(); //取得用户登录信息
               if (orderConfig.anonymous == 0 || order_no.ToUpper().StartsWith("R")) {
                  if (userModel == null) {
                     //用户未登录
                     Response.Redirect(linkurl("payment", "?action=login"));
                     return;
                  }
               }
               else if (userModel == null) {
                  userModel = new DTcms.Model.users();
               }
               //检查订单的类型(充值或购物)
               if (order_no.ToUpper().StartsWith("R")) //充值订单
                    {
                  rechargeModel = new DTcms.BLL.user_recharge().GetModel(order_no);
                  if (rechargeModel == null) {
                     Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，订单号不存在或已删除！")));
                     return;
                  }
                  //检查订单号是否已支付
                  if (rechargeModel.status == 1) {
                     Response.Redirect(linkurl("payment", "?action=succeed&order_no=" + rechargeModel.recharge_no));
                     return;
                  }
                  //检查支付方式
                  payModel = new DTcms.BLL.payment().GetModel(rechargeModel.payment_id);
                  if (payModel == null) {
                     Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，支付方式不存在或已删除！")));
                     return;
                  }
                  //检查是否线上支付
                  if (payModel.type == 2) {
                     Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，账户充值不允许线下支付！")));
                     return;
                  }
                  order_amount = rechargeModel.amount; //订单金额
               }
               else if (order_no.ToUpper().StartsWith("B")) //商品订单
                    {
                  //检查订单是否存在
                  orderModel = new DTcms.BLL.orders().GetModel(order_no);
                  if (orderModel == null) {
                     Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，订单号不存在或已删除！")));
                     return;
                  }
                  //检查是否已支付过
                  if (orderModel.payment_status == 2) {
                     Response.Redirect(linkurl("payment", "?action=succeed&order_no=" + orderModel.order_no));
                     return;
                  }
                  //检查支付方式
                  payModel = new DTcms.BLL.payment().GetModel(orderModel.payment_id);
                  if (payModel == null) {
                     Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，支付方式不存在或已删除！")));
                     return;
                  }
                  //检查是否线下付款
                  if (orderModel.payment_status == 0) {
                     Response.Redirect(linkurl("payment", "?action=succeed&order_no=" + orderModel.order_no));
                     return;
                  }
                  //检查是否积分换购，直接跳转成功页面
                  if (orderModel.order_amount == 0) {
                     //修改订单状态
                     bool result = new DTcms.BLL.orders().UpdateField(orderModel.order_no, "status=2,payment_status=2,payment_time='" + DateTime.Now + "'");
                     if (!result) {
                        Response.Redirect(linkurl("payment", "?action=error"));
                        return;
                     }
                     Response.Redirect(linkurl("payment", "?action=succeed&order_no=" + orderModel.order_no));
                     return;
                  }
                  order_amount = orderModel.order_amount; //订单金额
               }
               else {
                  Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，找不到您要提交的订单类型！")));
                  return;
               }
               break;
            case "succeed":
               //检查订单的类型(充值或购物)
               if (order_no.ToUpper().StartsWith("R")) //充值订单
                    {
                  rechargeModel = new DTcms.BLL.user_recharge().GetModel(order_no);
                  if (rechargeModel == null) {
                     Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，订单号不存在或已删除！")));
                     return;
                  }

               }
               else if (order_no.ToUpper().StartsWith("B")) //商品订单
                    {
                  orderModel = new DTcms.BLL.orders().GetModel(order_no);
                  if (orderModel == null) {
                     Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，订单号不存在或已删除！")));
                     return;
                  }
               }
               else {
                  Response.Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("出错啦，找不到您要提交的订单类型！")));
                  return;
               }
               break;
         }
         ViewBag.Action = action;
         ViewBag.OrderNo = order_no;
         ViewBag.OrderType = order_type;
         ViewBag.OrderAmount = order_amount.ToString();
         ViewBag.This = this;
      }
      //
      // GET: /Web/Article/

      public ActionResult Index() {
         return View(ViewName);
      }

    }
}
