using System;
using System.Collections.Generic;
using System.Text;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Util
{
    public class WebApiUrl
    {
        //public const string API = "http://webapi.kinfar.net";
        public const string API = "http://localhost:900";
        #region KfCenter
        /// <summary>
        /// 获取数据中心账套分页数据
        /// </summary>
        public const string API_KfCenter_KfActSet_GetPageData = API+"/api/KfCenter/KfActSet/GetPageData";
        /// <summary>
        /// 保存
        /// </summary>
        public const string API_KfCenter_KfActSet_Save = API+"/api/KfCenter/KfActSet/Save";
        #endregion
        #region OAth
        /// <summary>
        /// 获取全局令牌 {"appid":"","appsecret",""}
        /// </summary>
        public const string API_OAth_Account_GetGlobalToken = API + "/api/OAth/Account/GetGlobalToken/";
        #endregion
    }
}
