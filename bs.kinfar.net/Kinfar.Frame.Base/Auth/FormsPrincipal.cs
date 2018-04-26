/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // 版权所有
        // 开发者：Kinfar.
        // Email：kinfar@foxmail.net
        // QQ：3133119519
//----------------------------------------------------------------*/

using Kinfar.Frame.Common;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Kinfar.Frame.Base
{
    /// <summary>
    /// 表单认证
    /// </summary>
    public sealed class FormsPrincipal : IPrincipal
    {
        private readonly IIdentity _identity;
        private readonly UserInfo _userData;

        public FormsPrincipal(FormsAuthenticationTicket ticket, UserInfo userData)
        {
            if (ticket == null)
                throw new ArgumentNullException("ticket");
            if (userData == null)
                throw new ArgumentNullException("userData");

            _identity = new FormsIdentity(ticket);
            _userData = userData;
        }

        public UserInfo UserData
        {
            get { return _userData; }
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            return true;
        }

        /// <summary>
        /// 执行用户登录操作
        /// </summary>
        /// <param name="loginName">登录名</param>
        /// <param name="userData">与登录名相关的用户信息</param>
        /// <param name="expiration">登录Cookie的过期时间，单位：分钟。</param>
        public static void Login(string loginName, UserInfo userData, int expiration)
        {
            if (string.IsNullOrEmpty(loginName))
                throw new ArgumentNullException("loginName");
            if (userData == null)
                throw new ArgumentNullException("userData");

            // 1. 把需要保存的用户数据转成一个字符串。
            string userExtendData = string.Empty; //用户扩展信息
            string extendTypeAssembly = string.Empty; //扩展对象类型程序集
            string extendTypeName = string.Empty; //扩展对象类型名称
            if (userData.ExtendUserObject != null)
            {
                userExtendData = JsonHelper.Serialize(userData.ExtendUserObject);
                extendTypeAssembly = userData.ExtendUserObject.GetType().Module.Name;
                extendTypeName = userData.ExtendUserObject.GetType().Name;
                userData.ExtendUserObject = null;
            }
            string userInfoData = JsonHelper.Serialize(userData); //用户基本信息
            string data = userInfoData;
            if (!string.IsNullOrEmpty(userExtendData))
            {
                data += string.Format("___{0}___{1}___{2}", userExtendData, extendTypeAssembly, extendTypeName);
            }

            // 2. 创建一个FormsAuthenticationTicket，它包含登录名以及额外的用户数据。
            var ticket = new FormsAuthenticationTicket(
                2, loginName, DateTime.Now, DateTime.Now.AddDays(1), true, data);

            // 3. 加密Ticket，变成一个加密的字符串。
            var cookieValue = FormsAuthentication.Encrypt(ticket);

            // 4. 根据加密结果创建登录Cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieValue)
            {
                HttpOnly = true,
                Secure = FormsAuthentication.RequireSSL,
                Domain = FormsAuthentication.CookieDomain,
                Path = FormsAuthentication.FormsCookiePath
            };
            if (expiration > 0)
                cookie.Expires = DateTime.Now.AddMinutes(expiration);

            HttpContext context = ApplicationObject.CurrentHttpContext;
            if (context == null)
                throw new InvalidOperationException();

            // 5. 写登录Cookie
            context.Response.Cookies.Remove(cookie.Name);
            context.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 退出
        /// </summary>
        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 根据HttpContext对象设置用户标识对象
        /// </summary>
        /// <param name="context"></param>
        public static void TrySetUserInfo(HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            // 1. 读登录Cookie
            HttpCookie cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null || string.IsNullOrEmpty(cookie.Value))
            {
                return;
            }
            try
            {
                UserInfo userData = null;
                // 2. 解密Cookie值，获取FormsAuthenticationTicket对象
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                if (ticket != null && !string.IsNullOrEmpty(ticket.UserData))
                {  // 3. 还原用户数据
                    string[] token = ticket.UserData.Split("___".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    userData = JsonHelper.Deserialize<UserInfo>(token[0]);
                    if (token.Length > 1)
                    {
                        Assembly assembly = Assembly.Load(token[2]);
                        Type extendType = assembly.GetTypes().Where(x => x.Name == token[3]).FirstOrDefault();
                        object obj = Globals.Deserialize(extendType, token[1]);
                        userData.ExtendUserObject = obj as UserExtendBase;
                    }
                }
                if (ticket != null && userData != null)
                {  // 4. 构造我们的MyFormsPrincipal实例，重新给context.User赋值。
                    context.User = new FormsPrincipal(ticket, userData);
                }
            }
            catch { /* 有异常也不要抛出，防止攻击者试探。 */ }
        }
    }
}
