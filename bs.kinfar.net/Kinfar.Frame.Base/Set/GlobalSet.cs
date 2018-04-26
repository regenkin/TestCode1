using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinfar.Frame.Base.Set
{
    /// <summary>
    /// 员工用户名默认配置规则
    /// </summary>
    public enum UserNameAndEmpConfigRule
    {
        /// <summary>
        /// 以工号默认为用户名
        /// </summary>
        EmpCode = 0,

        /// <summary>
        /// 以邮箱前缀默认为用户名
        /// </summary>
        EmailPre = 1,

        /// <summary>
        /// 以邮箱默认为用户名
        /// </summary>
        Email = 2,

        /// <summary>
        /// 以手机号默认为用户名
        /// </summary>
        Mobile = 3,
    }

    /// <summary>
    /// 全局设置
    /// </summary>
    public class GlobalSet
    {
        private static UserNameAndEmpConfigRule _empUserNameConfigRule = UserNameAndEmpConfigRule.EmpCode;
        /// <summary>
        /// 员工用户名默认配置规则
        /// </summary>
        public static UserNameAndEmpConfigRule EmpUserNameConfigRule
        {
            get { return _empUserNameConfigRule; }
            set { _empUserNameConfigRule = value; }
        }

        private static bool _isAllowOtherConfigRuleLogin = true;
        /// <summary>
        /// 是否允许除当前配置规则以外的其他方式登录系统
        /// </summary>
        public static bool IsAllowOtherConfigRuleLogin
        {
            get { return _isAllowOtherConfigRuleLogin; }
            set { _isAllowOtherConfigRuleLogin = value; }
        }

        private static int _exportDataPagingSize = 500;
        /// <summary>
        /// 导出数据分页大小，默认500
        /// </summary>
        public static int ExportDataPagingSize
        {
            get { return _exportDataPagingSize; }
            set 
            {
                if (value > 500)
                {
                    _exportDataPagingSize = value;
                }
            }
        }

        private static bool _isShowStyleBtn = true;
        /// <summary>
        /// 是否显示样式按钮
        /// </summary>
        public static bool IsShowStyleBtn
        {
            get { return GlobalSet._isShowStyleBtn; }
            set { GlobalSet._isShowStyleBtn = value; }
        }

        private static bool _isStartLoadCache = true;
        /// <summary>
        /// 启动程序时是否加载所有缓存数据
        /// </summary>
        public static bool IsStartLoadCache
        {
            get { return GlobalSet._isStartLoadCache; }
        }
    }
}
