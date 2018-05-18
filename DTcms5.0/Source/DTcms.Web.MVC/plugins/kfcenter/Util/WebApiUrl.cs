using System;
using System.Collections.Generic;
using System.Text;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Util
{
    public class WebApiUrl
    {
        #region KfCenter
        /// <summary>
        /// 获取数据中心账套分页数据
        /// </summary>
        public const string API_KfCenter_KfActSet_GetPageData = "http://localhost:900/api/KfCenter/KfActSet/GetPageData";
        #endregion
        #region OAth
        /// <summary>
        /// 获取全局令牌 {"appid":"","appsecret",""}
        /// </summary>
        public const string API_OAth_Account_GetGlobalToken = "http://localhost:900/api/OAth/Account/GetGlobalToken/";
        #endregion
    }
}
