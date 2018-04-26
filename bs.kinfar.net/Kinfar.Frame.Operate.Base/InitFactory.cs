using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kinfar.Frame.Common;
using Kinfar.Frame.Bridge;
using Kinfar.Frame.Model.Sys;

namespace Kinfar.Frame.Operate.Base
{
    /// <summary>
    /// 初始化工厂类
    /// </summary>
    public abstract class InitFactory
    {
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <returns></returns>
        internal static InitFactory GetInstance()
        {
            InitFactory factory = null;
            string operateDll = WebConfigHelper.GetAppSettingValue("Operate");
            if (!string.IsNullOrWhiteSpace(operateDll))
            {
                List<Type> types = BridgeObject.GetTypesByDLL(operateDll);
                if (types != null)
                {
                    Type type = types.Where(x => x.BaseType == typeof(InitFactory)).FirstOrDefault();
                    if (type != null)
                    {
                       object obj =  Activator.CreateInstance(type);
                       return obj as InitFactory;
                    }
                }
            }
            return factory;
        }

        /// <summary>
        /// 自定义初始化，包括菜单、模块、字段、字典等数据初始化
        /// </summary>
        public abstract void CustomerInit();
    }
}
