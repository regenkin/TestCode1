using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DTcms.Common;
using DTcms.Web.MVC.UI;
using DTcms.Web.MVC.UI.Controllers;

namespace DTcms.Web.MVC.Areas.Web.Controllers
{
    /// <summary>
    /// 购物车
    /// </summary>
    public class CartController : DTcms.Web.MVC.UI.Controllers.BaseController
    {
        /// <summary>
        /// 购物车列表
        /// </summary>
        public List<DTcms.Model.cart_items> goodsList = new List<DTcms.Model.cart_items>();
        /// <summary>
        /// 购物画统计
        /// </summary>
        public DTcms.Model.cart_total goodsTotal = new DTcms.Model.cart_total();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            int group_id = 0; //会员组ID
            DTcms.Model.users userModel = GetUserInfo(); //会员信息
            if (userModel != null)
            {
                group_id = userModel.group_id; //如果是已登录则将会员组ID赋值
            }
            goodsList = ShopCart.GetList(group_id); //商品列表
            if (goodsList != null)
            {
                goodsTotal = ShopCart.GetTotal(goodsList); //商品统计
            }
            else
            {
                goodsList = new List<DTcms.Model.cart_items>();
                goodsTotal = new DTcms.Model.cart_total();
            }
            ViewData["goodsList"] = goodsList;
            ViewData["goodsTotal"] = goodsTotal;
        }

        //
        // GET: /Web/Cart/
        public ActionResult Index()
        {
            return View(ViewName);
        }
    }
}
