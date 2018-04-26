/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // 版权所有
        // 开发者：Kinfar.
        // Email：kinfar@foxmail.net
        // QQ：3133119519
//----------------------------------------------------------------*/

using Kinfar.Frame.Cache.Factory;
using Kinfar.Frame.Cache.Factory.Provider;
using Kinfar.Frame.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Security;

namespace Kinfar.Frame.Base
{
    /// <summary>
    /// 用户类
    /// </summary>
    public sealed class UserInfo
    {
        #region 当前账户

        /// <summary>
        /// 互斥锁
        /// </summary>
        private static object locker = new object();

        /// <summary>
        /// 当前账户信息
        /// </summary>
        public static UserInfo CurrentUserInfo
        {
            get
            {
                return GetCurretnUser(null);
            }
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        /// <param name="context">上下文对象</param>
        /// <returns></returns>
        public static UserInfo GetCurretnUser(HttpContext context)
        {
            lock (locker)
            {
                HttpContext tempContext = context != null ? context : ApplicationObject.CurrentHttpContext;
                FormsIdentity identity = tempContext.User.Identity as FormsIdentity;
                if (identity != null)
                {
                    string[] token = identity.Ticket.UserData.Split("___".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    //取用户基本信息
                    UserInfo userInfo = JsonHelper.Deserialize<UserInfo>(token[0]);
                    if (token.Length > 1)
                    {
                        //取用户扩展信息
                        Type extendType = null;
                        ICacheProvider cacheFactory = CacheFactory.GetCacheInstance(CacheProviderType.LOCALMEMORYCACHE);
                        if (cacheFactory == null || cacheFactory.Get<Type>("UserExtendType") == null)
                        {
                            string binPath = Globals.GetBinPath();
                            string dllPath = string.Format(@"{0}{1}", binPath, token[2]);
                            if (File.Exists(dllPath))
                            {
                                Assembly assembly = Assembly.LoadFrom(dllPath);
                                extendType = assembly.GetTypes().Where(x => x.Name == token[3]).FirstOrDefault();
                                if (extendType != null)
                                {
                                    cacheFactory.Set<Type>("UserExtendType", extendType); //扩展对象类型存入缓存
                                }
                            }
                        }
                        else //扩展对象类型从缓存中取
                        {
                            extendType = cacheFactory.Get<Type>("UserExtendType");
                        }
                        if (extendType != null)
                        {
                            //反序列化用户扩展对象
                            object obj = Globals.Deserialize(extendType, token[1]);
                            userInfo.ExtendUserObject = obj as UserExtendBase;
                        }
                    }
                    return userInfo;
                }
                return null;
            }
        }

        #endregion

        #region 构造函数

        public UserInfo()
        {
            ClientBrowserWidth = 0;
            ClientBrowserHeight = 0;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户别名
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// 所属组织
        /// </summary>
        public Guid? OrganizationId { get; set; }

        /// <summary>
        /// 客户端IP
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// 角色ID集合
        /// </summary>
        public List<Guid?> RoleIds { get; set; }

        /// <summary>
        /// 员工ID
        /// </summary>
        public Guid? EmpId { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmpName { get; set; }

        /// <summary>
        /// 员工编号
        /// </summary>
        public string EmpCode { get; set; }

        /// <summary>
        /// 角色名称集合
        /// </summary>
        public List<string> RoleNames { get; set; }

        /// <summary>
        /// 扩展用户对象
        /// </summary>
        public UserExtendBase ExtendUserObject { get; set; }

        #region 客户端参数
        /// <summary>
        /// 客户端浏览器可见区域宽
        /// </summary>
        public int ClientBrowserWidth { get; set; }

        /// <summary>
        /// 客户端浏览器可见区域高
        /// </summary>
        public int ClientBrowserHeight { get; set; }
        #endregion

        #endregion

        #region 静态方法

        /// <summary>
        /// 获取当前用户别名
        /// </summary>
        /// <returns></returns>
        public static string GetUserAliasName()
        {
            if (string.IsNullOrWhiteSpace(CurrentUserInfo.AliasName))
            {
                return CurrentUserInfo.UserName;
            }
            return CurrentUserInfo.AliasName;
        }

        /// <summary>
        /// 当前用户是否为超级管理员
        /// </summary>
        /// <returns></returns>
        public static bool IsSuperAdmin()
        {
            return CurrentUserInfo != null && CurrentUserInfo.UserName == "admin";
        }

        #endregion

        #region 常量

        /// <summary>
        /// 账号过期时间（分钟）
        /// </summary>
        public const int ACCOUNT_EXPIRATION_TIME = 30;

        #endregion
    }
}
