using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Web.MVC.UI.Controllers;
using DTcms.Common;

namespace DTcms.Web.MVC.Areas.Web.Controllers {
   public class ShoppingController : DTcms.Web.MVC.UI.Controllers.BaseController {
      public string goodsJsonValue = string.Empty;
      public DTcms.Model.users userModel;
      public List<DTcms.Model.cart_items> goodsList = new List<DTcms.Model.cart_items>();
      public DTcms.Model.cart_total goodsTotal = new DTcms.Model.cart_total();
      public DTcms.Model.orderconfig orderConfig = new DTcms.BLL.orderconfig().loadConfig();

      protected override void OnActionExecuting(ActionExecutingContext filterContext) {
         base.OnActionExecuting(filterContext);
         goodsJsonValue = Utils.GetCookie(DTKeys.COOKIE_SHOPPING_BUY); //获取商品JSON数据
         int group_id = 0; //会员组ID
         userModel = GetUserInfo(); //获取会员信息
         if (userModel == null) {
            //如果不支持匿名购物则跳转到登录页面
            if (orderConfig.anonymous == 0) {
               filterContext.Result = Redirect(linkurl("login")); //自动跳转URL
               return;
            }
         }
         else {
            group_id = userModel.group_id;
         }
         //获取商品列表
         if (string.IsNullOrEmpty(goodsJsonValue)) {
            filterContext.Result = Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("对不起，无法获取您要购买的商品！")));
            return;
         }
         try {
            List<DTcms.Model.cart_keys> ls = (List<DTcms.Model.cart_keys>)JsonHelper.JSONToObject<List<DTcms.Model.cart_keys>>(goodsJsonValue);
            goodsList = ShopCart.ToList(ls, group_id); //商品列表
            goodsTotal = ShopCart.GetTotal(goodsList); //商品统计
         }
         catch {
            filterContext.Result = Redirect(linkurl("error", "?msg=" + Utils.UrlEncode("对不起，商品的传输参数有误！")));
            return;
         }
         ViewData["userModel"] = userModel;
         ViewData["goodsList"] = goodsList;
         ViewData["goodsTotal"] = goodsTotal;
         ViewData["orderConfig"] = orderConfig;
      }
      //
      // GET: /Web/Article/

      public ActionResult Index() {
         return View(ViewName);
      }
   }
}
