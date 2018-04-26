/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // 版权所有
        // 开发者：Kinfar.
        // Email：kinfar@foxmail.net
        // QQ：3133119519
//----------------------------------------------------------------*/

using Kinfar.Frame.Operate.Base.EnumDef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinfar.Frame.Operate.Base.TempModel
{
    /// <summary>
    /// 表单按钮
    /// </summary>
    public class FormButton
    {
        /// <summary>
        /// 标签Id
        /// </summary>
        public string TagId { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayText { get; set; }

        /// <summary>
        /// 按钮图标类型
        /// </summary>
        public ButtonIconType IconType { get; set; }

        /// <summary>
        /// 按钮调用方法
        /// </summary>
        public string ClickMethod { get; set; }

        /// <summary>
        /// 按钮图标
        /// </summary>
        public string Icon { get; set; }
    }
}
