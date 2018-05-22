using System;
using System.Collections.Generic;
using System.Text;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Util
{
    public class WebApiUrl
    {
        public const string API = "http://localhost:900";
        //public const string API = "http://webapi.kinfar.net";

        #region KfCenter
        /// <summary>
        /// 获取数据中心账套分页数据
        /// </summary>
        public const string API_KfCenter_KfActSet_GetPageData = API+"/api/KfCenter/KfActSet/GetPageData";
        /// <summary>
        /// 判断账套是否存在
        /// </summary>
        public const string API_KfCenter_KfActSet_Exist = API + "/api/KfCenter/KfActSet/Exist";
        /// <summary>
        /// 获取账套实体
        /// </summary>
        public const string API_KfCenter_KfActSet_GetEntity = API + "/api/KfCenter/KfActSet/GetEntity";
        /// <summary>
        /// 账套信息保存
        /// </summary>
        public const string API_KfCenter_KfActSet_Save = API + "/api/KfCenter/KfActSet/Save";
        /// <summary>
        /// 测试数据库连接
        /// </summary>
        public const string API_KfCenter_KfActSet_TestActSetConnection = API + "/api/KfCenter/KfActSet/TestActSetConnection";
        //账套组
        /// <summary>
        /// 
        /// </summary>
        public const string API_KfCenter_KfActGroup_GetAllList = API + "/api/KfCenter/KfActGroup/GetAllList";
        #endregion
        #region OAth
        /// <summary>
        /// 获取全局令牌 {"appid":"","appsecret",""}
        /// </summary>
        public const string API_OAth_Account_GetGlobalToken = API + "/api/OAth/Account/GetGlobalToken/";
        #endregion
    }
}
