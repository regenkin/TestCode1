using Kinfar.Frame.Base;
using Kinfar.Frame.Common;
using Kinfar.Frame.Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinfar.Frame.Operate.Base.OperateHandle.Implement
{
    class Sys_FieldOperateHandle : IModelOperateHandle<Sys_Field>, IGridOperateHandle<Sys_Field>, IFormOperateHandle<Sys_Field>
    {
        /// <summary>
        /// 字段操作完成后
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="t">字段对象</param>
        /// <param name="result">操作结果</param>
        /// <param name="currUser">当前用户</param>
        /// <param name="otherParams"></param>
        public void OperateCompeletedHandle(ModelRecordOperateType operateType, Sys_Field t, bool result, UserInfo currUser, object[] otherParams = null)
        {
            if (operateType == ModelRecordOperateType.Del && result)
            {
                //字段信息删除后删除相应的表单字段和列表字段
                string errMsg = string.Empty;
                //删除列表字段
                Sys_Module gridFieldModule = SystemOperate.GetModuleByTableName(typeof(Sys_GridField).Name);
                CommonOperate.DeleteRecordsByExpression<Sys_GridField>(x => x.Sys_FieldId == t.Id, out errMsg, gridFieldModule.IsEnabledRecycle);
                //删除表单字段
                Sys_Module formFieldModule = SystemOperate.GetModuleByTableName(typeof(Sys_FormField).Name);
                CommonOperate.DeleteRecordsByExpression<Sys_FormField>(x => x.Sys_FieldId == t.Id, out errMsg, formFieldModule.IsEnabledRecycle);
            }
        }

        /// <summary>
        /// 操作前事件
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="t">对象</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns></returns>
        public bool BeforeOperateVerifyOrHandle(ModelRecordOperateType operateType, Sys_Field t, out string errMsg, object[] otherParams = null)
        {
            errMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// 字段操作完成后
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="ts">操作对象集合</param>
        /// <param name="result">操作结果</param>
        /// <param name="currUser">当前用户</param>
        /// <param name="otherParams">其他参数</param>
        public void OperateCompeletedHandles(ModelRecordOperateType operateType, List<Sys_Field> ts, bool result, UserInfo currUser, object[] otherParams = null)
        {
            if (operateType == ModelRecordOperateType.Del && result)
            {
                //字段信息删除后删除相应的表单字段和列表字段
                string errMsg = string.Empty;
                Sys_Module gridFieldModule = SystemOperate.GetModuleByTableName(typeof(Sys_GridField).Name);
                Sys_Module formFieldModule = SystemOperate.GetModuleByTableName(typeof(Sys_FormField).Name);
                foreach (Sys_Field t in ts)
                {
                    //删除列表字段
                    CommonOperate.DeleteRecordsByExpression<Sys_GridField>(x => x.Sys_FieldId == t.Id, out errMsg, gridFieldModule.IsEnabledRecycle);
                    //删除表单字段
                    CommonOperate.DeleteRecordsByExpression<Sys_FormField>(x => x.Sys_FieldId == t.Id, out errMsg, formFieldModule.IsEnabledRecycle);
                }
            }
        }

        /// <summary>
        /// 操作前事件
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="ts">对象集合</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns></returns>
        public bool BeforeOperateVerifyOrHandles(ModelRecordOperateType operateType, List<Sys_Field> ts, out string errMsg, object[] otherParams = null)
        {
            errMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// 网格参数设置
        /// </summary>
        /// <param name="gridType">网格类型</param>
        /// <param name="gridParams">网格参数对象</param>
        public void GridParamsSet(EnumDef.DataGridType gridType, TempModel.GridParams gridParams)
        {
        }

        /// <summary>
        /// 网格数据处理
        /// </summary>
        /// <param name="data">数据集合</param>
        /// <param name="otherParams">其他参数</param>
        public void PageGridDataHandle(List<Sys_Field> data, object[] otherParams = null)
        {
            if (data != null && data.Count > 0)
            {
                foreach (Sys_Field field in data)
                {
                    if (!field.Sys_ModuleId.HasValue) continue;
                    string tableName = SystemOperate.GetModuleTableNameById(field.Sys_ModuleId.Value);
                    Dictionary<string, string> dic = ToolOperate.GetDbColumnInfo(tableName, field.Name);
                    field.DbType = dic["ColumnType"].ObjToStr();
                    field.DbLen = dic["Length"].ObjToInt();
                }
            }
        }

        /// <summary>
        /// 网格数据过滤条件
        /// </summary>
        /// <returns></returns>
        public System.Linq.Expressions.Expression<Func<Sys_Field, bool>> GetGridFilterCondition(out string where, EnumDef.DataGridType gridType, Dictionary<string, string> condition = null, string initModule = null, string initField = null, Dictionary<string, string> otherParams = null)
        {
            where = string.Empty;
            return null;
        }

        /// <summary>
        /// 网格按钮操作前验证
        /// </summary>
        /// <returns></returns>
        public string GridButtonOperateVerify(string buttonText, List<Guid> ids, object[] otherParams = null)
        {
            return string.Empty;
        }

        /// <summary>
        /// 表单数据处理
        /// </summary>
        /// <param name="t">对象</param>
        /// <param name="formType">表单类型</param>
        public void FormDataHandle(Sys_Field t, Model.EnumSpace.FormTypeEnum formType)
        {
            if (t != null && t.Sys_ModuleId.HasValue)
            {
                string tableName = SystemOperate.GetModuleTableNameById(t.Sys_ModuleId.Value);
                Dictionary<string, string> dic = ToolOperate.GetDbColumnInfo(tableName, t.Name);
                t.DbType = dic["ColumnType"].ObjToStr();
                t.DbLen = dic["Length"].ObjToInt();
            }
        }

        /// <summary>
        /// 获取表单按钮
        /// </summary>
        /// <returns></returns>
        public List<TempModel.FormButton> GetFormButtons(Model.EnumSpace.FormTypeEnum formType, List<TempModel.FormButton> buttons, bool isAdd = false, bool isDraft = false)
        {
            return buttons;
        }

        /// <summary>
        /// 获取表单工具标签项
        /// </summary>
        /// <returns></returns>
        public List<TempModel.FormToolTag> GetFormToolTags(Model.EnumSpace.FormTypeEnum formType, List<TempModel.FormToolTag> tags, bool isAdd = false)
        {
            return tags;
        }

        /// <summary>
        /// 获取表单自动完成显示格式
        /// </summary>
        /// <returns></returns>
        public string GetAutoCompleteDisplay(Sys_Field t, string initModule, string initField, object[] otherParams = null)
        {
            return string.Empty;
        }

        /// <summary>
        /// 是否重写保存明细
        /// </summary>
        /// <returns></returns>
        public bool OverSaveDetailData(TempModel.DetailFormObject detailObj, out string errMsg)
        {
            errMsg = string.Empty;
            return false;
        }
    }
}
