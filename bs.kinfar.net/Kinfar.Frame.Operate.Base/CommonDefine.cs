/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // 版权所有
        // 开发者：Kinfar.
        // Email：kinfar@foxmail.net
        // QQ：3133119519
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinfar.Frame.Operate.Base
{
    /// <summary>
    /// 常用变量定义
    /// </summary>
    public static class CommonDefine
    {
        /// <summary>
        /// 基类字段集合
        /// </summary>
        public static readonly List<string> BaseEntityFields = new List<string>() { "CreateUserId", "ModifyUserId", "CreateUserName", "ModifyUserName", "CreateDate", "ModifyDate", "IsDeleted", "DeleteTime", "IsDraft", "OrgId" };

        /// <summary>
        /// 包含Id的基类字段集合
        /// </summary>
        public static readonly List<string> BaseEntityFieldsContainId = new List<string>() { "Id", "CreateUserId", "ModifyUserId", "CreateUserName", "ModifyUserName", "CreateDate", "ModifyDate", "IsDeleted", "DeleteTime", "IsDraft", "OrgId" };

        /// <summary>
        /// 不需要更新字段集合
        /// </summary>
        public static readonly List<string> NoUpdateFields = new List<string>() { "CreateUserId", "CreateUserName", "CreateDate", "IsDeleted", "DeleteTime", "IsDraft" };

        /// <summary>
        /// 网格通用按钮
        /// </summary>
        public static readonly List<string> GridCommonBtns = new List<string>() { "新增", "编辑", "删除", "查看", "导入", "导出", "复制", "批量编辑", "打印" };
    }
}
