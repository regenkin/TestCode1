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
        /// 返回配送列表
        /// </summary>
        /// <param name="top">显示条数</param>
        /// <param name="strwhere">查询条件</param>
        /// <returns>DataTable</returns>
        public DataTable get_express_list(int top, string strwhere)
        {
            DataTable dt = new DataTable();
            string _where = "is_lock=0";
            if (!string.IsNullOrEmpty(strwhere))
            {
                _where += " and " + strwhere;
            }
            dt = new DTcms.BLL.express().GetList(top, _where, "sort_id asc,id desc").Tables[0];
            return dt;
        }

        /// <summary>
        /// 返回配送方式的标题
        /// </summary>
        /// <param name="payment_id">ID</param>
        /// <returns>String</returns>
        public string get_express_title(int express_id)
        {
           return new DTcms.BLL.express().GetTitle(express_id);
        }

    }
}
