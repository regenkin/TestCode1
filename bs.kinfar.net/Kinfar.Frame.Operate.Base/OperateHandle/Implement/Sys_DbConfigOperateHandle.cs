using Kinfar.Frame.Common.Model;
using Kinfar.Frame.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinfar.Frame.Operate.Base.OperateHandle.Implement
{
    /// <summary>
    /// 数据库配置操作处理类
    /// </summary>
    class Sys_DbConfigOperateHandle : IGridOperateHandle<Sys_DbConfig>, IFormOperateHandle<Sys_DbConfig>
    {
        public void GridParamsSet(EnumDef.DataGridType gridType, TempModel.GridParams gridParams)
        {
        }

        public void PageGridDataHandle(List<Sys_DbConfig> data, object[] otherParams = null)
        {
            if (data != null && data.Count > 0)
            {
                foreach (Sys_DbConfig t in data)
                {
                    TbIndexInfo tbIndexInfo = SystemOperate.GetTableIndexInfo(t.ModuleName);
                    if (tbIndexInfo != null)
                        t.CurrPageDensity = tbIndexInfo.FragmentationPercent;
                }
            }
        }

        public System.Linq.Expressions.Expression<Func<Sys_DbConfig, bool>> GetGridFilterCondition(out string where, EnumDef.DataGridType gridType, Dictionary<string, string> condition = null, string initModule = null, string initField = null, Dictionary<string, string> otherParams = null)
        {
            where = string.Empty;
            return null;
        }

        public string GridButtonOperateVerify(string buttonText, List<Guid> ids, object[] otherParams = null)
        {
            return string.Empty;
        }

        public void FormDataHandle(Sys_DbConfig t, Model.EnumSpace.FormTypeEnum formType)
        {
            if (t != null)
            {
                TbIndexInfo tbIndexInfo = SystemOperate.GetTableIndexInfo(t.ModuleName);
                if (tbIndexInfo != null)
                    t.CurrPageDensity = tbIndexInfo.FragmentationPercent;
            }
        }

        public List<TempModel.FormButton> GetFormButtons(Model.EnumSpace.FormTypeEnum formType, List<TempModel.FormButton> buttons, bool isAdd = false, bool isDraft = false)
        {
            return buttons;
        }

        public List<TempModel.FormToolTag> GetFormToolTags(Model.EnumSpace.FormTypeEnum formType, List<TempModel.FormToolTag> tags, bool isAdd = false)
        {
            return tags;
        }

        public string GetAutoCompleteDisplay(Sys_DbConfig t, string initModule, string initField, object[] otherParams = null)
        {
            return string.Empty;
        }

        public bool OverSaveDetailData(TempModel.DetailFormObject detailObj, out string errMsg)
        {
            errMsg = string.Empty;
            return false;
        }
    }
}
