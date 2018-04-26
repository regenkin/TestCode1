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
using Kinfar.Frame.Common;
using Kinfar.Frame.Bridge;
using Kinfar.Frame.Operate.Base.EnumDef;
using System.Linq.Expressions;
using Kinfar.Frame.Operate.Base.TempModel;
using Kinfar.Frame.Model.EnumSpace;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using Kinfar.Frame.Base;
using Kinfar.Frame.Model.Sys;

namespace Kinfar.Frame.Operate.Base.OperateHandle
{
    #region 枚举定义

    /// <summary>
    /// 模块操作类型
    /// </summary>
    public enum ModelRecordOperateType
    {
        /// <summary>
        /// 新增
        /// </summary>
        Add = 0,

        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 1,

        /// <summary>
        /// 删除
        /// </summary>
        Del = 2,

        /// <summary>
        /// 查看
        /// </summary>
        View = 3,
    }

    /// <summary>
    /// 接口操作类型
    /// </summary>
    enum OperateInterfaceType
    {
        ModelOperate = 0,
        GridOperate = 1,
        FormOperate = 2,
        TreeOperate = 3,
        PermissionOperate = 4,
        FlowOperate = 5,
        UIDraw = 6,
        GridSearch = 7
    }

    #endregion

    /// <summary>
    /// 操作处理接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IModelOperateHandle<T> where T : class
    {
        #region 实体操作接口

        /// <summary>
        /// 单个实体操作完成后的处理，针对新增保存后、删除后、修改保存后
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="t">实体对象</param>
        /// <param name="result">操作结果，成功或失败</param>
        /// <param name="currUser">当前用户</param>
        /// <param name="otherParams">其他参数</param>
        void OperateCompeletedHandle(ModelRecordOperateType operateType, T t, bool result, UserInfo currUser, object[] otherParams = null);

        /// <summary>
        /// 单个实体操作前验证或处理，针对新增保存前、删除前、修改保存前
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="t">操作对象</param>
        /// <param name="errMsg">错误信息</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns>是否通过验证</returns>
        bool BeforeOperateVerifyOrHandle(ModelRecordOperateType operateType, T t, out string errMsg, object[] otherParams = null);

        /// <summary>
        /// 多个实体操作完成后的处理，针对新增保存后、删除后、修改保存后
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="ts">实体记录集合</param>
        /// <param name="result">操作结果，成功或失败</param>
        /// <param name="currUser">当前用户</param>
        /// <param name="otherParams">其他参数</param>
        void OperateCompeletedHandles(ModelRecordOperateType operateType, List<T> ts, bool result, UserInfo currUser, object[] otherParams = null);

        /// <summary>
        /// 多个实体操作前验证或处理，针对新增保存前、删除前、修改保存前
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="ts">操作对象集合</param>
        /// <param name="errMsg">错误信息</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns>是否通过验证</returns>
        bool BeforeOperateVerifyOrHandles(ModelRecordOperateType operateType, List<T> ts, out string errMsg, object[] otherParams = null);

        #endregion
    }

    /// <summary>
    /// 网格相关操作处理接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGridOperateHandle<T> where T : class
    {
        #region 网格相关接口

        /// <summary>
        /// 网格参数设置
        /// </summary>
        /// <param name="gridType">网格类型</param>
        /// <param name="gridParams">网格参数对象</param>
        void GridParamsSet(DataGridType gridType, GridParams gridParams);

        /// <summary>
        /// 返回分页网格数据前对数据处理
        /// </summary>
        /// <param name="data">处理前的网格数据</param>
        /// <param name="otherParams">其他参数</param>
        void PageGridDataHandle(List<T> data, object[] otherParams = null);

        /// <summary>
        /// 返回网格过滤条件
        /// </summary>
        /// <param name="where">where条件语句</param>
        /// <param name="gridType">网格类型</param>
        /// <param name="condition">条件参数</param>
        /// <param name="initModule">原始模块（弹出选择外键模块数据的初始模块），弹出列表用到</param>
        /// <param name="initField">原始字段（弹出选择外键模块数据的初始字段），弹出列表用到</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns>返回条件表达式</returns>
        Expression<Func<T, bool>> GetGridFilterCondition(out string where, DataGridType gridType, Dictionary<string, string> condition = null, string initModule = null, string initField = null, Dictionary<string, string> otherParams = null);

        /// <summary>
        /// 网格按钮操作验证
        /// </summary>
        /// <param name="buttonText">按钮显示名称</param>
        /// <param name="ids">操作的记录Id集合</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns></returns>
        string GridButtonOperateVerify(string buttonText, List<Guid> ids, object[] otherParams = null);

        #endregion
    }

    /// <summary>
    /// 网格搜索接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IGridSearchHandle<T> where T : class
    {
        #region 网格搜索接口

        /// <summary>
        /// 重写多字段搜索结果
        /// </summary>
        /// <param name="q">搜索键值对</param>
        /// <param name="whereSql">过滤条件语句</param>
        /// <returns></returns>
        List<ConditionItem> GetSeachResults(Dictionary<string, string> q, out string whereSql);

        /// <summary>
        /// 重写单字段搜索结果
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">字段值</param>
        /// <param name="whereSql">过滤条件语句</param>
        /// <returns></returns>
        ConditionItem GetSearchResult(string fieldName, object value, out string whereSql);

        #endregion
    }

    /// <summary>
    /// 表单操作处理接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFormOperateHandle<T> where T : class
    {
        #region 表单相关接口

        /// <summary>
        /// 返回表单数据前对表单数据进行处理
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <param name="formType">表单类型</param>
        void FormDataHandle(T t, FormTypeEnum formType);

        /// <summary>
        /// 取表单页面按钮
        /// </summary>
        /// <param name="formType">表单类型</param>
        /// <param name="buttons">原有表单按钮</param>
        /// <param name="isAdd">是否新增页面</param>
        /// <param name="isDraft">是否草稿</param>
        /// <returns></returns>
        List<FormButton> GetFormButtons(FormTypeEnum formType, List<FormButton> buttons, bool isAdd = false, bool isDraft = false);

        /// <summary>
        /// 获取表单工具标签按钮集合
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="formType">表单类型</param>
        /// <param name="isAdd">是否新增页面</param>
        /// <returns></returns>
        List<FormToolTag> GetFormToolTags(FormTypeEnum formType, List<FormToolTag> tags, bool isAdd = false);

        /// <summary>
        /// 获取智能提示下拉面板显示值，各模块通过重写此方法可以任意设置下拉显示格式
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <param name="initModule">针对编辑表单时，原始模块</param>
        /// <param name="initField">针对编辑表单时，原始字段</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns></returns>
        string GetAutoCompleteDisplay(T t, string initModule, string initField, object[] otherParams = null);

        /// <summary>
        /// 重写表单明细保存数据
        /// </summary>
        /// <param name="detailObj">明细表单数据对象</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>返回是否执行了自定义明细保存方法</returns>
        bool OverSaveDetailData(DetailFormObject detailObj, out string errMsg);

        #endregion
    }

    /// <summary>
    /// 权限操作处理接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPermissionHandle<T> where T : class
    {
        #region 权限相关接口
        /// <summary>
        /// 权限过滤条件表达式，如x=>x.Name=="name"
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="filterWhere">权限过滤SQL语句，如 Name='name'</param>
        /// <param name="queryCache">是否从缓存中查询</param>
        /// <returns></returns>
        Expression<Func<T, bool>> GetPermissionExp(Guid userId, out string filterWhere, bool queryCache);

        /// <summary>
        /// 是否有记录的操作（查看，更新、删除）权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="t">单据对象</param>
        /// <param name="type">类型，0-查看，1-更新，2-删除</param>
        /// <returns></returns>
        bool HasRecordOperatePermission(Guid userId, T t, int type);
        #endregion
    }

    /// <summary>
    /// 流程操作处理接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFlowOperateHandle<T> where T : class
    {
        #region 流程相关接口

        /// <summary>
        /// 流程是否转向
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="currNodeName">当前流程结点名称</param>
        /// <param name="nextNodeName">下一流程结点名称</param>
        /// <returns></returns>
        bool IsFlowTurn(Guid id, string currNodeName, string nextNodeName);

        /// <summary>
        /// 流程操作完成后事件处理
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="currNodeName">当前流程结点名称</param>
        /// <param name="currUser">当前处理人</param>
        /// <param name="isOpSuccess">是否操作成功</param>
        /// <param name="workAction">流程操作动作，同意、拒绝、。。。</param>
        /// <param name="flowStatus">流程状态</param>
        void AfterFlowOperateCompleted(Guid id, string currNodeName, UserInfo currUser, bool isOpSuccess, WorkActionEnum workAction, WorkFlowStatusEnum flowStatus);

        /// <summary>
        /// 获取结点处理人
        /// </summary>
        /// <param name="recordId">记录ID</param>
        /// <param name="nodeName">结点名称</param>
        /// <param name="workFlowInstId">流程实例ID</param>
        /// <param name="currUser">当前用户</param>
        /// <returns></returns>
        List<Guid> GetNodeHandler(Guid recordId, string nodeName, Guid workFlowInstId, UserInfo currUser);

        #endregion
    }

    /// <summary>
    /// 树操作处理接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITreeOperateHandle<T> where T : class
    {
        #region 树节点相关接口

        /// <summary>
        /// 树节点处理
        /// </summary>
        /// <param name="node">节点对象</param>
        /// <returns></returns>
        void TreeNodeHandle(TreeNode node);

        #endregion
    }

    /// <summary>
    /// UI绘制接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IUIDrawHandle<T> where T : class
    {
        #region UI绘制接口

        /// <summary>
        /// 返回网格页面HTML
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="gridType">网格类型</param>
        /// <param name="condition">过滤条件</param>
        /// <param name="where">where过滤条件</param>
        /// <param name="viewId">视图Id</param>
        /// <param name="initModule">针对表单弹出外键选择框，表单原始模块</param>
        /// <param name="initField">针对表单外键弹出框，表单原始字段</param>
        /// <param name="otherParams">其他参数</param>
        /// <param name="detailCopy">明细复制</param>
        /// <param name="filterFields">过滤字段</param>
        /// <param name="menuId">菜单ID，从哪个菜单进来的</param>
        /// <returns></returns>
        string GetGridHTML(DataGridType gridType = DataGridType.MainGrid, string condition = null, string where = null, Guid? viewId = null, string initModule = null, string initField = null, Dictionary<string, object> otherParams = null, bool detailCopy = false, List<string> filterFields = null, Guid? menuId = null);

        /// <summary>
        /// 获取简单搜索HTML
        /// </summary>
        /// <param name="searchFields">可搜索字段</param>
        /// <param name="gridType">网格类型</param>
        /// <param name="condition">过滤条件</param>
        /// <param name="where">where过滤条件</param>
        /// <param name="viewId">视图Id</param>
        /// <param name="initModule">针对表单弹出外键选择框，表单原始模块</param>
        /// <param name="initField">针对表单外键弹出框，表单原始字段</param>
        /// <returns></returns>
        string GetSimpleSearchHTML(List<Sys_GridField> searchFields, DataGridType gridType = DataGridType.MainGrid, string condition = null, string where = null, Guid? viewId = null, string initModule = null, string initField = null);

        /// <summary>
        /// 获取编辑表单HTML
        /// </summary>
        /// <param name="id">记录Id</param>
        /// <param name="gridId">为网格表单编辑模式的网格Id</param>
        /// <param name="copyId">复制时被复制的记录Id</param>
        /// <param name="showTip">是否显示表单tip按钮</param>
        /// <param name="todoTaskId">待办任务ID</param>
        /// <param name="formId">指定表单ID</param>
        string GetEditFormHTML(Guid? id, string gridId = null, Guid? copyId = null, bool showTip = false, Guid? todoTaskId = null, Guid? formId = null);

        /// <summary>
        /// 获取编辑表单明细编辑网格HTML
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="detailTopDisplay">明细是否顶部显示</param>
        /// <param name="copyId">复制记录ID</param>
        /// <returns></returns>
        string GetEditDetailHTML(Guid? id, out bool detailTopDisplay, Guid? copyId = null);

        /// <summary>
        /// 获取查看表单HTML
        /// </summary>
        /// <param name="id">记录Id</param>
        /// <param name="gridId">为网格表单查看模式的网格Id</param>
        /// <param name="fromEditPageFlag">从编辑页面点击查看按钮标识</param>
        /// <param name="isRecycle">是否来自回收站</param>
        /// <param name="showTip">是否显示表单tip按钮</param>
        /// <param name="formId">指定表单ID</param>
        /// <returns></returns>
        string GetViewFormHTML(Guid id, string gridId = null, string fromEditPageFlag = null, bool isRecycle = false, bool showTip = false, Guid? formId = null);

        /// <summary>
        /// 获取查看表单明细查看网格HTML
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="detailTopDisplay">明细是否顶部显示</param>
        /// <returns></returns>
        string GetViewDetailHTML(Guid? id, out bool detailTopDisplay);

        #endregion
    }

    /// <summary>
    /// 操作处理工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class OperateHandleFactory<T> where T : class
    {
        #region 私有方法

        /// <summary>
        /// 获取操作对象实体
        /// </summary>
        /// <returns></returns>
        internal object GetOperateHandleInstance(OperateInterfaceType interfaceType)
        {
            List<Type> types = BridgeObject.GetCustomerOperateHandleTypes();
            if (types.Count > 0)
            {
                object obj = null;
                foreach (Type type in types)
                {
                    try
                    {
                        switch (interfaceType)
                        {
                            case OperateInterfaceType.ModelOperate:
                                obj = (IModelOperateHandle<T>)Activator.CreateInstance(type);
                                break;
                            case OperateInterfaceType.GridOperate:
                                obj = (IGridOperateHandle<T>)Activator.CreateInstance(type);
                                break;
                            case OperateInterfaceType.GridSearch:
                                obj = (IGridSearchHandle<T>)Activator.CreateInstance(type);
                                break;
                            case OperateInterfaceType.FormOperate:
                                obj = (IFormOperateHandle<T>)Activator.CreateInstance(type);
                                break;
                            case OperateInterfaceType.TreeOperate:
                                obj = (ITreeOperateHandle<T>)Activator.CreateInstance(type);
                                break;
                            case OperateInterfaceType.PermissionOperate:
                                obj = (IPermissionHandle<T>)Activator.CreateInstance(type);
                                break;
                            case OperateInterfaceType.FlowOperate:
                                obj = (IFlowOperateHandle<T>)Activator.CreateInstance(type);
                                break;
                            case OperateInterfaceType.UIDraw:
                                obj = (IUIDrawHandle<T>)Activator.CreateInstance(type);
                                break;
                        }
                        if (obj != null) return obj;
                    }
                    catch
                    { }
                }
                return obj;
            }
            return null;
        }

        #endregion

        #region 实体操作

        /// <summary>
        /// 单个实体操作前验证或处理
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="t">操作对象</param>
        /// <param name="errMsg">错误信息</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns>是否通过验证</returns>
        internal bool BeforeOperateVerifyOrHandle(ModelRecordOperateType operateType, T t, out string errMsg, object[] otherParams = null)
        {
            errMsg = string.Empty;
            object instance = GetOperateHandleInstance(OperateInterfaceType.ModelOperate);
            if (instance != null)
            {
                return ((IModelOperateHandle<T>)instance).BeforeOperateVerifyOrHandle(operateType, t, out errMsg, otherParams);
            }
            return true;
        }

        /// <summary>
        /// 单个实体操作完成处理
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="t">实体对象</param>
        /// <param name="result">操作结果</param>
        /// <param name="currUser">当前用户</param>
        /// <param name="otherParams">其他参数</param>
        internal void OperateCompeletedHandle(ModelRecordOperateType operateType, T t, bool result, UserInfo currUser, object[] otherParams = null)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.ModelOperate);
            if (instance != null)
            {
                //异步处理
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        ((IModelOperateHandle<T>)instance).OperateCompeletedHandle(operateType, t, result, currUser, otherParams);
                    }
                    catch { }
                });
            }
        }

        /// <summary>
        /// 多个实体操作前验证或处理
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="ts">操作对象集合</param>
        /// <param name="errMsg">错误信息</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns>是否通过验证</returns>
        internal bool BeforeOperateVerifyOrHandles(ModelRecordOperateType operateType, List<T> ts, out string errMsg, object[] otherParams = null)
        {
            errMsg = string.Empty;
            object instance = GetOperateHandleInstance(OperateInterfaceType.ModelOperate);
            if (instance != null)
            {
                return ((IModelOperateHandle<T>)instance).BeforeOperateVerifyOrHandles(operateType, ts, out errMsg, otherParams);
            }
            return true;
        }

        /// <summary>
        /// 多个实体操作完成处理
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="ts">实体对象集合</param>
        /// <param name="result">操作结果</param>
        /// <param name="currUser">当前用户</param>
        /// <param name="otherParams">其他参数</param>
        internal void OperateCompeletedHandles(ModelRecordOperateType operateType, List<T> ts, bool result, UserInfo currUser, object[] otherParams = null)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.ModelOperate);
            if (instance != null)
            {
                //异步处理
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        ((IModelOperateHandle<T>)instance).OperateCompeletedHandles(operateType, ts, result, currUser, otherParams);
                    }
                    catch { }
                });
            }
        }

        #endregion

        #region 网格相关

        /// <summary>
        /// 网格参数设置
        /// </summary>
        /// <param name="gridType">网格类型</param>
        /// <param name="gridParams">网格参数对象</param>
        internal void GridParamsSet(DataGridType gridType, GridParams gridParams)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.GridOperate);
            if (instance != null)
            {
                ((IGridOperateHandle<T>)instance).GridParamsSet(gridType, gridParams);
            }
        }

        /// <summary>
        /// 返回分页网格数据前对数据处理
        /// </summary>
        /// <param name="data">处理前的网格数据</param>
        /// <param name="otherParams">其他参数</param>
        internal void PageGridDataHandle(List<T> data, object[] otherParams = null)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.GridOperate);
            if (instance != null)
            {
                ((IGridOperateHandle<T>)instance).PageGridDataHandle(data, otherParams);
            }
        }

        /// <summary>
        /// 返回网格过滤条件
        /// </summary>
        /// <param name="where">where条件语句</param>
        /// <param name="gridType">网格类型</param>
        /// <param name="condition">条件参数</param>
        /// <param name="initModule">原始模块（弹出选择外键模块数据的初始模块），弹出列表用到</param>
        /// <param name="initField">原始字段（弹出选择外键模块数据的初始字段），弹出列表用到</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns>返回条件表达式</returns>
        internal Expression<Func<T, bool>> GetGridFilterCondition(out string where, DataGridType gridType, Dictionary<string, string> condition = null, string initModule = null, string initField = null, Dictionary<string, string> otherParams = null)
        {
            where = string.Empty;
            object instance = GetOperateHandleInstance(OperateInterfaceType.GridOperate);
            if (instance != null)
            {
                return ((IGridOperateHandle<T>)instance).GetGridFilterCondition(out where, gridType, condition, initModule, initField, otherParams);
            }
            return null;
        }

        /// <summary>
        /// 网格按钮操作验证
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <param name="buttonText">按钮显示名称</param>
        /// <param name="ids">操作的记录Id集合</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns>验证通过，返回空字符串，验证不通过，返回验证提示信息</returns>
        internal string GridButtonOperateVerify(Guid moduleId, string buttonText, List<Guid> ids, object[] otherParams = null)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.GridOperate);
            if (instance != null)
            {
                return ((IGridOperateHandle<T>)instance).GridButtonOperateVerify(buttonText, ids, otherParams);
            }
            if (buttonText == "编辑" && BpmOperate.IsEnabledWorkflow(moduleId))
            {
                StringBuilder sb = new StringBuilder();
                string titleKeyDisplay = SystemOperate.GetModuleTitleKeyDisplay(moduleId);
                foreach (Guid id in ids)
                {
                    WorkFlowStatusEnum flowStatus = BpmOperate.GetRecordFlowStatus(moduleId, id);
                    if (flowStatus != WorkFlowStatusEnum.NoStatus)
                    {
                        string titleKeyValue = CommonOperate.GetModelTitleKeyValue(moduleId, id);
                        string msg = string.Format("【Id】=【{0}】", id);
                        if (!string.IsNullOrEmpty(titleKeyDisplay))
                            msg += string.Format(",【{0}】=【{1}】", titleKeyDisplay, titleKeyValue);
                        msg += string.Format("的记录已进入流程审批阶段，无法编辑！", buttonText);
                        sb.AppendLine(msg);
                    }
                }
                return sb.ToString();
            }
            return string.Empty;
        }

        #endregion

        #region 网格搜索

        /// <summary>
        /// 重写多字段搜索结果
        /// </summary>
        /// <param name="q">搜索键值对</param>
        /// <param name="whereSql">过滤条件语句</param>
        /// <returns></returns>
        internal List<ConditionItem> GetSearchResults(Dictionary<string, string> q, out string whereSql)
        {
            whereSql = string.Empty;
            object instance = GetOperateHandleInstance(OperateInterfaceType.GridSearch);
            if (instance != null)
            {
                return ((IGridSearchHandle<T>)instance).GetSeachResults(q, out whereSql);
            }
            return null;
        }

        /// <summary>
        /// 重写单字段搜索结果
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">字段值</param>
        /// <param name="whereSql">过滤条件语句</param>
        /// <returns></returns>
        internal ConditionItem GetSearchResult(string fieldName, object value, out string whereSql)
        {
            whereSql = string.Empty;
            object instance = GetOperateHandleInstance(OperateInterfaceType.GridSearch);
            if (instance != null)
            {
                return ((IGridSearchHandle<T>)instance).GetSearchResult(fieldName, value, out whereSql);
            }
            return null;
        }

        #endregion

        #region 表单相关

        /// <summary>
        /// 返回表单数据前对表单数据进行处理
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <param name="formType">表单类型</param>
        internal void FormDataHandle(T t, FormTypeEnum formType)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.FormOperate);
            if (instance != null)
            {
                ((IFormOperateHandle<T>)instance).FormDataHandle(t, formType);
            }
        }

        /// <summary>
        /// 取表单页面按钮
        /// </summary>
        /// <param name="formType">表单类型</param>
        /// <param name="buttons">原有表单按钮</param>
        /// <param name="isAdd">是否新增页面</param>
        /// <param name="isDraft">是否草稿</param>
        /// <returns></returns>
        public List<FormButton> GetFormButtons(FormTypeEnum formType, List<FormButton> buttons, bool isAdd = false, bool isDraft = false)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.FormOperate);
            if (instance != null)
            {
                return ((IFormOperateHandle<T>)instance).GetFormButtons(formType, buttons, isAdd, isDraft);
            }
            return new List<FormButton>();
        }

        /// <summary>
        /// 获取表单工具标签按钮集合
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="formType">表单类型</param>
        /// <param name="isAdd">是否新增页面</param>
        /// <returns></returns>
        public List<FormToolTag> GetFormToolTags(FormTypeEnum formType, List<FormToolTag> tags, bool isAdd = false)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.FormOperate);
            if (instance != null)
            {
                return ((IFormOperateHandle<T>)instance).GetFormToolTags(formType, tags, isAdd);
            }
            return new List<FormToolTag>();
        }

        /// <summary>
        /// 获取智能提示下拉面板显示值，各模块通过重写此方法可以任意设置下拉显示格式
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <param name="initModule">针对编辑表单时，原始模块</param>
        /// <param name="initField">针对编辑表单时，原始字段</param>
        /// <param name="otherParams">其他参数</param>
        /// <returns></returns>
        public string GetAutoCompleteDisplay(T t, string initModule, string initField, object[] otherParams = null)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.FormOperate);
            if (instance != null)
            {
                return ((IFormOperateHandle<T>)instance).GetAutoCompleteDisplay(t, initModule, initField, otherParams);
            }
            return null;
        }

        /// <summary>
        /// 重写表单明细保存数据
        /// </summary>
        /// <param name="detailObj">明细表单数据对象</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>返回是否执行了自定义方法</returns>
        public bool OverSaveDetailData(DetailFormObject detailObj, out string errMsg)
        {
            errMsg = string.Empty;
            object instance = GetOperateHandleInstance(OperateInterfaceType.FormOperate);
            if (instance != null)
            {
                return ((IFormOperateHandle<T>)instance).OverSaveDetailData(detailObj, out errMsg);
            }
            return false;
        }

        #endregion

        #region 树节点相关

        /// <summary>
        /// 树节点处理
        /// </summary>
        /// <param name="node">节点对象</param>
        /// <returns></returns>
        internal void TreeNodeHandle(TreeNode node)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.TreeOperate);
            if (instance != null)
            {
                ((ITreeOperateHandle<T>)instance).TreeNodeHandle(node);
            }
        }

        #endregion

        #region 权限相关

        /// <summary>
        /// 权限过滤条件表达式，如x=>x.Name=="name"
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="filterWhere">权限过滤SQL语句，如 Name='name'</param>
        /// <param name="queryCache">是否从缓存中查询</param>
        /// <returns></returns>
        internal Expression<Func<T, bool>> GetPermissionExp(Guid userId, out string filterWhere, bool queryCache)
        {
            filterWhere = string.Empty;
            if (UserOperate.IsSuperAdmin(userId)) return null;
            object instance = GetOperateHandleInstance(OperateInterfaceType.PermissionOperate);
            if (instance != null)
            {
                return ((IPermissionHandle<T>)instance).GetPermissionExp(userId, out filterWhere, queryCache);
            }
            return null;
        }

        /// <summary>
        /// 是否有记录的操作（查看，更新、删除）权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="t">单据对象</param>
        /// <param name="type">类型，0-查看，1-更新，2-删除</param>
        /// <returns></returns>
        internal bool HasRecordOperatePermission(Guid userId, T t, int type)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.PermissionOperate);
            if (instance != null)
            {
                return ((IPermissionHandle<T>)instance).HasRecordOperatePermission(userId, t, type);
            }
            return true;
        }

        #endregion

        #region 流程相关

        /// <summary>
        /// 流程是否转向
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="currNodeName">当前流程结点名称</param>
        /// <param name="nextNodeName">下一流程结点名称</param>
        /// <returns></returns>
        internal bool IsFlowTurn(Guid id, string currNodeName, string nextNodeName)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.FlowOperate);
            if (instance != null)
            {
                return ((IFlowOperateHandle<T>)instance).IsFlowTurn(id, currNodeName, nextNodeName);
            }
            return false;
        }

        /// <summary>
        /// 流程操作完成后事件处理
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="currNodeName">当前流程结点名称</param>
        /// <param name="currUser">当前处理人</param>
        /// <param name="isOpSuccess">是否操作成功</param>
        /// <param name="workAction">流程操作动作，同意、拒绝、。。。</param>
        /// <param name="flowStatus">流程状态</param>
        internal void AfterFlowOperateCompleted(Guid id, string currNodeName, UserInfo currUser, bool isOpSuccess, WorkActionEnum workAction, WorkFlowStatusEnum flowStatus)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.FlowOperate);
            if (instance != null)
            {
                //异步处理
                Task.Factory.StartNew(() =>
                {
                    try
                    {
                        ((IFlowOperateHandle<T>)instance).AfterFlowOperateCompleted(id, currNodeName, currUser, isOpSuccess, workAction, flowStatus);
                    }
                    catch { }
                });
            }
        }

        /// <summary>
        /// 获取结点处理人
        /// </summary>
        /// <param name="recordId">记录ID</param>
        /// <param name="nodeName">结点名称</param>
        /// <param name="workFlowInstId">流程实例ID</param>
        /// <param name="currUser">当前用户</param>
        /// <returns></returns>
        internal List<Guid> GetNodeHandler(Guid recordId, string nodeName, Guid workFlowInstId, UserInfo currUser)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.FlowOperate);
            if (instance != null)
            {
                try
                {
                    return ((IFlowOperateHandle<T>)instance).GetNodeHandler(recordId, nodeName, workFlowInstId, currUser);
                }
                catch { }
            }
            return new List<Guid>();
        }

        #endregion

        #region UI绘制

        /// <summary>
        /// 返回网格页面HTML
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="gridType">网格类型</param>
        /// <param name="condition">过滤条件</param>
        /// <param name="where">where过滤条件</param>
        /// <param name="viewId">视图Id</param>
        /// <param name="initModule">针对表单弹出外键选择框，表单原始模块</param>
        /// <param name="initField">针对表单外键弹出框，表单原始字段</param>
        /// <param name="otherParams">其他参数</param>
        /// <param name="detailCopy">明细复制</param>
        /// <param name="filterFields">过滤字段</param>
        /// <param name="menuId">菜单ID，从哪个菜单进来的</param>
        /// <returns></returns>
        internal string GetGridHTML(DataGridType gridType = DataGridType.MainGrid, string condition = null, string where = null, Guid? viewId = null, string initModule = null, string initField = null, Dictionary<string, object> otherParams = null, bool detailCopy = false, List<string> filterFields = null, Guid? menuId = null)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.UIDraw);
            if (instance != null)
            {
                return ((IUIDrawHandle<T>)instance).GetGridHTML(gridType, condition, where, viewId, initModule, initField, otherParams, detailCopy, filterFields, menuId);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取简单搜索HTML
        /// </summary>
        /// <param name="searchFields">可搜索字段</param>
        /// <param name="gridType">网格类型</param>
        /// <param name="condition">过滤条件</param>
        /// <param name="where">where过滤条件</param>
        /// <param name="viewId">视图Id</param>
        /// <param name="initModule">针对表单弹出外键选择框，表单原始模块</param>
        /// <param name="initField">针对表单外键弹出框，表单原始字段</param>
        /// <returns></returns>
        internal string GetSimpleSearchHTML(List<Sys_GridField> searchFields, DataGridType gridType = DataGridType.MainGrid, string condition = null, string where = null, Guid? viewId = null, string initModule = null, string initField = null)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.UIDraw);
            if (instance != null)
            {
                return ((IUIDrawHandle<T>)instance).GetSimpleSearchHTML(searchFields, gridType, condition, where, viewId, initModule, initField);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取编辑表单HTML
        /// </summary>
        /// <param name="id">记录Id</param>
        /// <param name="gridId">为网格表单编辑模式的网格Id</param>
        /// <param name="copyId">复制时被复制的记录Id</param>
        /// <param name="showTip">是否显示表单tip按钮</param>
        /// <param name="todoTaskId">待办任务ID</param>
        /// <param name="formId">指定表单ID</param>
        internal string GetEditFormHTML(Guid? id, string gridId = null, Guid? copyId = null, bool showTip = false, Guid? todoTaskId = null, Guid? formId = null)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.UIDraw);
            if (instance != null)
            {
                return ((IUIDrawHandle<T>)instance).GetEditFormHTML(id, gridId, copyId, showTip, todoTaskId, formId);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取编辑表单明细编辑网格HTML
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="detailTopDisplay">明细是否顶部显示</param>
        /// <param name="copyId">复制记录ID</param>
        /// <returns></returns>
        internal string GetEditDetailHTML(Guid? id, out bool detailTopDisplay, Guid? copyId = null)
        {
            detailTopDisplay = false;
            object instance = GetOperateHandleInstance(OperateInterfaceType.UIDraw);
            if (instance != null)
            {
                return ((IUIDrawHandle<T>)instance).GetEditDetailHTML(id, out detailTopDisplay, copyId);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取查看表单HTML
        /// </summary>
        /// <param name="id">记录Id</param>
        /// <param name="gridId">为网格表单查看模式的网格Id</param>
        /// <param name="fromEditPageFlag">从编辑页面点击查看按钮标识</param>
        /// <param name="isRecycle">是否来自回收站</param>
        /// <param name="showTip">是否显示表单tip按钮</param>
        /// <param name="formId">指定表单ID</param>
        /// <returns></returns>
        internal string GetViewFormHTML(Guid id, string gridId = null, string fromEditPageFlag = null, bool isRecycle = false, bool showTip = false, Guid? formId = null)
        {
            object instance = GetOperateHandleInstance(OperateInterfaceType.UIDraw);
            if (instance != null)
            {
                return ((IUIDrawHandle<T>)instance).GetViewFormHTML(id, gridId, fromEditPageFlag, isRecycle, showTip, formId);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取查看表单明细查看网格HTML
        /// </summary>
        /// <param name="id">记录ID</param>
        /// <param name="detailTopDisplay">明细是否顶部显示</param>
        /// <returns></returns>
        internal string GetViewDetailHTML(Guid? id, out bool detailTopDisplay)
        {
            detailTopDisplay = false;
            object instance = GetOperateHandleInstance(OperateInterfaceType.UIDraw);
            if (instance != null)
            {
                return ((IUIDrawHandle<T>)instance).GetViewDetailHTML(id, out detailTopDisplay);
            }
            return string.Empty;
        }

        #endregion
    }

    /// <summary>
    /// 用户操作处理接口
    /// </summary>
    public interface IUserOperateHandle
    {
        /// <summary>
        /// 登录成功后
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="request">request</param>
        /// <param name="response">response</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="expires">有效时间（分钟）</param>
        void AfterLoginSuccess(HttpSessionStateBase session, HttpRequestBase request, HttpResponseBase response, string username, string pwd, int expires);

        /// <summary>
        /// 用户注册后操作处理
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="nickName">昵称</param>
        void AfterRegiterUser(string username, string pwd, string nickName = null);

        /// <summary>
        /// 用户修改密码后操作处理
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        void AfterChangePwd(string username, string oldPwd, string newPwd);

        /// <summary>
        /// 删除用户后处理操作
        /// </summary>
        /// <param name="username"></param>
        void AfterDeleteUser(string username);
    }

    /// <summary>
    /// 用户操作处理
    /// </summary>
    internal class UserOperateHandleFactory
    {
        /// <summary>
        /// 获取通用操作接口对象
        /// </summary>
        /// <returns></returns>
        public List<IUserOperateHandle> GetUserOperateHandleInstances()
        {
            string operateDll = "Kinfar.Frame.Operate.Base"; //默认操作层dll
            //取自定义操作类类型
            string customerOperateDll = WebConfigHelper.GetAppSettingValue("Operate");
            if (!string.IsNullOrEmpty(customerOperateDll))
            {
                operateDll += string.Format(",{0}", customerOperateDll);
            }
            List<Type> list = new List<Type>();
            string[] token = operateDll.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (token.Length > 0)
            {
                foreach (string str in token)
                {
                    list.AddRange(BridgeObject.GetTypesByDLL(str));
                }
            }
            List<Type> types = list.Where(x => x.Name == "UserOperateHandle").ToList();
            List<IUserOperateHandle> operates = new List<IUserOperateHandle>();
            if (types != null && types.Count > 0)
            {
                foreach (Type type in types)
                {
                    Type[] interfaces = type.GetInterfaces();
                    if (interfaces.Length > 0 && interfaces.Where(x => x.Name == "IUserOperateHandle").FirstOrDefault() != null)
                    {
                        try
                        {
                            IUserOperateHandle obj = (IUserOperateHandle)Activator.CreateInstance(type);
                            if (obj != null) operates.Add(obj);
                        }
                        catch
                        { }
                    }
                }
            }
            return operates;
        }

        /// <summary>
        /// 登录成功后操作处理
        /// </summary>
        /// <param name="session">session</param>
        /// <param name="request">request</param>
        /// <param name="response">response</param>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="expires">有效时间（分钟）</param>
        public void AfterLoginSuccess(HttpSessionStateBase session, HttpRequestBase request, HttpResponseBase response, string username, string pwd, int expires)
        {
            List<IUserOperateHandle> instances = GetUserOperateHandleInstances();
            if (instances.Count > 0)
            {
                foreach (IUserOperateHandle instance in instances)
                {
                    instance.AfterLoginSuccess(session, request, response, username, pwd, expires);
                }
            }
        }

        /// <summary>
        /// 用户注册后操作处理
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="pwd">密码</param>
        /// <param name="nickName">昵称</param>
        public void AfterRegiterUser(string username, string pwd, string nickName = null)
        {
            List<IUserOperateHandle> instances = GetUserOperateHandleInstances();
            if (instances.Count > 0)
            {
                foreach (IUserOperateHandle instance in instances)
                {
                    try
                    {
                        instance.AfterRegiterUser(username, pwd, nickName);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// 用户修改密码后操作处理
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="oldPwd">旧密码</param>
        /// <param name="newPwd">新密码</param>
        public void AfterChangePwd(string username, string oldPwd, string newPwd)
        {
            List<IUserOperateHandle> instances = GetUserOperateHandleInstances();
            if (instances.Count > 0)
            {
                foreach (IUserOperateHandle instance in instances)
                {
                    instance.AfterChangePwd(username, oldPwd, newPwd);
                }
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="username"></param>
        public void AfterDeleteUser(string username)
        {
            List<IUserOperateHandle> instances = GetUserOperateHandleInstances();
            if (instances.Count > 0)
            {
                foreach (IUserOperateHandle instance in instances)
                {
                    instance.AfterDeleteUser(username);
                }
            }
        }
    }
}
