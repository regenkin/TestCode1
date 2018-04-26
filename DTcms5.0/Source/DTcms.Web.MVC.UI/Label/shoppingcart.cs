using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.Mvc;

namespace DTcms.Web.MVC.UI.Controllers
{
    public partial class BaseController : Controller
    {
        /// <summary>
        /// 返回购物车商品总数
        /// </summary>
        /// <returns>Int</returns>
        public int get_cart_quantity()
        {
           return ShopCart.GetQuantityCount();
        }

        /// <summary>
        /// 返回购物车列表
        /// </summary>
        /// <returns>IList</returns>
        public List<DTcms.Model.cart_items> get_cart_list()
        {
            int group_id = 0;
            DTcms.Model.users userModel = GetUserInfo();
            if (userModel != null)
            {
                group_id = userModel.group_id;
            }
            List<DTcms.Model.cart_items> ls = ShopCart.GetList(group_id);
            if (ls == null)
            {
               ls = new List<DTcms.Model.cart_items>();
            }
            return ls;
        }

    }
}
