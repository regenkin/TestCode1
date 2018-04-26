/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // 版权所有
        // 开发者：Kinfar.
        // Email：kinfar@foxmail.net
        // QQ：3133119519
//----------------------------------------------------------------*/

using Kinfar.Frame.Model.Sys;
using Kinfar.Frame.Operate.Base.EnumDef;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Kinfar.Frame.Operate.Base.OperateHandle.Implement
{
    /// <summary>
    /// 菜单操作事件
    /// </summary>
    class Sys_MenuOperateHandle : IGridOperateHandle<Sys_Menu>
    {
        public void GridParamsSet(DataGridType gridType, TempModel.GridParams gridParams)
        {
        }

        /// <summary>
        /// 网格数据处理
        /// </summary>
        /// <param name="data"></param>
        /// <param name="otherParams"></param>
        public void PageGridDataHandle(List<Sys_Menu> data, object[] otherParams = null)
        {
            if (data != null)
            {
                foreach (Sys_Menu menu in data)
                {
                    string url = SystemOperate.GetIconUrl(menu.Icon);
                    if (!string.IsNullOrEmpty(url))
                    {
                        menu.Icon = string.Format("<img src=\"{0}\" />", url);
                    }
                }
            }
        }

        public Expression<Func<Sys_Menu, bool>> GetGridFilterCondition(out string where, DataGridType gridType, Dictionary<string, string> condition = null, string initModule = null, string initField = null, Dictionary<string, string> otherParams = null)
        {
            where = string.Empty;
            return null;
        }

        public string GridButtonOperateVerify(string buttonText, List<Guid> ids, object[] otherParams = null)
        {
            return string.Empty;
        }
    }
}
