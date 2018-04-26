/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // 版权所有
        // 开发者：Kinfar.
        // Email：kinfar@foxmail.net
        // QQ：3133119519
//----------------------------------------------------------------*/

using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Data;
using Kinfar.Frame.Common;
using Kinfar.Frame.Operate.Base.OperateHandle;
using Kinfar.Frame.Operate.Base.TempModel;
using Kinfar.Frame.Common.PubDefine;
using Kinfar.Frame.Bridge;
using Kinfar.Frame.IBLL.Base;
using Kinfar.Frame.Common.Model;
using Kinfar.Frame.Model.EnumSpace;
using Kinfar.Frame.Model.Sys;
using Kinfar.Frame.Operate.Base.ConditionChange;
using Kinfar.Frame.Operate.Base.EnumDef;
using System.Web;
using System.Text;
using Kinfar.Frame.EntityBase.Attr;
using Kinfar.Frame.Base;
using System.IO;
using FluentValidation.Results;
using Kinfar.Frame.Office;
using Kinfar.Frame.EntityBase;
using ServiceStack.DataAnnotations;

namespace Kinfar.Frame.Operate.Base
{
    /// <summary>
    /// 通用操作类
    /// </summary>
    public static class CommonOperate
    {
        #region 临时处理类

        /// <summary>
        /// 临时操作类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        class TempOperate<T> where T : class
        {
            #region 属性处理

            /// <summary>
            /// 设置实体所有外键字段的TitleKey字段的值或导航属性值
            /// </summary>
            /// <param name="t">实体对象</param>
            /// <param name="references">是否加载关联对象数据（导航属性）</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            private void SetModelForeignNameFieldValue(T t, bool references = false, string connString = null, DatabaseType? dbType = null)
            {
                if (t == null) return;
                Type modelType = typeof(T);
                //开始处理
                PropertyInfo[] ps = modelType.GetProperties();
                string errMsg = string.Empty;
                foreach (PropertyInfo p in ps)
                {
                    if (CommonDefine.BaseEntityFields.Contains(p.Name)) continue;
                    FieldConfigAttribute fieldAttr = (FieldConfigAttribute)Attribute.GetCustomAttribute(p, typeof(FieldConfigAttribute));
                    if (fieldAttr == null || string.IsNullOrEmpty(fieldAttr.ForeignModuleName)) continue;
                    //取外键模块类型
                    Type foreighModelType = CommonOperate.GetModelTypeByModuleName(fieldAttr.ForeignModuleName);
                    if (foreighModelType == null) continue;
                    ModuleConfigAttribute moduleConfig = ((ModuleConfigAttribute)(Attribute.GetCustomAttribute(foreighModelType, typeof(ModuleConfigAttribute))));
                    if (moduleConfig == null || string.IsNullOrEmpty(moduleConfig.TitleKey)) return;
                    //外键Id值
                    Guid foreignKeyId = p.GetValue(t, null).ObjToGuid();
                    if (foreignKeyId == Guid.Empty) continue;
                    //导航属性字段名称
                    string navFieldName = p.Name.Substring(0, p.Name.Length - 2);
                    PropertyInfo navProperty = ps.Where(x => x.Name == navFieldName).FirstOrDefault();
                    //如果存在导航属性则不加载外键Name字段值
                    if (navProperty != null && navProperty.PropertyType.IsClass && navProperty.PropertyType.Name == foreighModelType.Name)
                    {
                        if (references && navFieldName == "Parent") //Parent导航属性处理
                        {
                            object navObj = CommonOperate.GetEntityById(foreighModelType.Name, foreignKeyId, out errMsg, false, connString, dbType);
                            navProperty.SetValue(t, navObj, null);
                        }
                        continue;
                    }
                    //外键的Name字段名称
                    string foreignNameField = navFieldName + "Name";
                    //外键Name字段属性
                    PropertyInfo temp = ps.Where(x => x.Name == foreignNameField).FirstOrDefault();
                    if (temp == null) continue;
                    string sql = string.Format("SELECT {0} FROM {1} WHERE Id='{2}'", moduleConfig.TitleKey, ModelConfigHelper.GetModuleTableName(foreighModelType), foreignKeyId);
                    object value = CommonOperate.ExecuteScale(out errMsg, sql, null, connString, dbType);
                    temp.SetValue(t, value, null);
                }
            }

            /// <summary>
            /// 设置实体集合的对象的外键字段的TitleKey字段的值或导航属性值
            /// </summary>
            /// <param name="ts">实体对象集合</param>
            /// <param name="references">是否加载关联对象数据（导航属性）</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            private void SetModelListForeignNameFieldValue(List<T> ts, bool references = false, string connString = null, DatabaseType? dbType = null)
            {
                if (ts != null && ts.Count > 0)
                {
                    foreach (T t in ts)
                    {
                        SetModelForeignNameFieldValue(t, references, connString, dbType);
                    }
                }
            }

            #endregion

            #region 查询记录

            /// <summary>
            /// 获取实体记录
            /// </summary>
            /// <param name="id">记录Id</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="references">是否加载关联对象数据（导航属性）</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public T GetEntityById(Guid id, out string errMsg, bool references = false, string connString = null, DatabaseType? dbType = null)
            {
                errMsg = string.Empty;
                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "GetEntityById");
                if (methodCallType != MethodCallTypeEnum.CommonMethod)
                {
                    object[] args = new object[] { errMsg, id, references, connString };
                    object obj = ReflectExecuteBLLMethod(typeof(T), "GetEntityById", args, dbType);
                    errMsg = args[0].ObjToStr();
                    T t = obj as T;
                    SetModelForeignNameFieldValue(t, references, connString, dbType);
                    return t;
                }
                else
                {
                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                    T t = bll.GetEntityById(out errMsg, id, references, connString);
                    SetModelForeignNameFieldValue(t, references, connString, dbType);
                    return t;
                }
            }

            /// <summary>
            /// 获取实体记录
            /// </summary>
            /// <param name="expression">条件表达式</param>
            /// <param name="whereSql">SQL条件语句</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="references">是否加载关联对象数据（导航属性）</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public T GetEntity(Expression<Func<T, bool>> expression, string whereSql, out string errMsg, bool references = false, string connString = null, DatabaseType? dbType = null)
            {
                errMsg = string.Empty;
                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "GetEntity");
                if (methodCallType != MethodCallTypeEnum.CommonMethod)
                {
                    object[] args = new object[] { errMsg, expression, whereSql, references, connString };
                    object obj = ReflectExecuteBLLMethod(typeof(T), "GetEntity", args, dbType);
                    errMsg = args[0].ObjToStr();
                    T t = obj as T;
                    SetModelForeignNameFieldValue(t, references, connString, dbType);
                    return t;
                }
                else
                {
                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                    T t = bll.GetEntity(out errMsg, expression, whereSql, references, connString);
                    SetModelForeignNameFieldValue(t, references, connString, dbType);
                    return t;
                }
            }

            /// <summary>
            /// 获取实体记录集合 
            /// </summary>
            /// <param name="errMsg">异常信息</param>
            /// <param name="expression">条件表达式</param>
            /// <param name="whereSql">SQL条件语句</param>
            /// <param name="permissionFilter">是否进行权限过滤</param>
            /// <param name="orderFields">排序字段</param>
            /// <param name="isDescs">是否降序排列</param>
            /// <param name="top">取前几条记录</param>
            /// <param name="references">是否加载关联对象数据（导航属性）</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public List<T> GetEntities(out string errMsg, Expression<Func<T, bool>> expression = null, string whereSql = null, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, int? top = null, bool references = false, string connString = null, DatabaseType? dbType = null)
            {
                errMsg = string.Empty;
                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "GetEntities");
                if (methodCallType != MethodCallTypeEnum.CommonMethod)
                {
                    object[] args = new object[] { errMsg, expression, whereSql, permissionFilter, orderFields, isDescs, top, references, connString };
                    object obj = ReflectExecuteBLLMethod(typeof(T), "GetEntities", args, dbType);
                    errMsg = args[0].ObjToStr();
                    List<T> list = obj as List<T>;
                    SetModelListForeignNameFieldValue(list, references, connString, dbType);
                    return list;
                }
                else
                {
                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                    List<T> list = bll.GetEntities(out errMsg, expression, whereSql, permissionFilter, orderFields, isDescs, top, references, connString);
                    SetModelListForeignNameFieldValue(list, references, connString, dbType);
                    return list;
                }
            }

            /// <summary>
            /// 获取实体分页记录
            /// </summary>
            /// <param name="errMsg">异常信息</param>
            /// <param name="pageInfo">分页信息</param>
            /// <param name="permissionFilter">是否进行权限过滤</param>
            /// <param name="expression">条件表达式</param>
            /// <param name="whereSql">SQL条件语句</param>
            /// <param name="references">是否加载关联对象数据（导航属性）</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns>返回实体集合</returns>
            public List<T> GetPageEntities(out string errMsg, PageInfo pageInfo, bool permissionFilter = true, Expression<Func<T, bool>> expression = null, string whereSql = null, bool references = false, string connString = null, DatabaseType? dbType = null)
            {
                errMsg = string.Empty;
                long totalCount = 0;
                List<string> orderFields = string.IsNullOrEmpty(pageInfo.sortname) ? null : pageInfo.sortname.Split(",".ToCharArray()).ToList();
                List<bool> isDescs = string.IsNullOrEmpty(pageInfo.sortorder) ? null : pageInfo.sortorder.Split(",".ToCharArray()).Select(x => x == "desc").ToList();
                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "GetPageEntities");
                if (methodCallType != MethodCallTypeEnum.CommonMethod)
                {
                    object[] args = new object[] { totalCount, errMsg, permissionFilter, pageInfo.page, pageInfo.pagesize, orderFields, isDescs, expression, whereSql, references, connString };
                    object obj = ReflectExecuteBLLMethod(typeof(T), "GetPageEntities", args, dbType);
                    errMsg = args[1].ObjToStr();
                    pageInfo.totalCount = args[0].ObjToLong();
                    List<T> list = obj as List<T>;
                    //导航属性处理
                    SetModelListForeignNameFieldValue(list, references, connString, dbType);
                    //网格数据处理
                    new OperateHandleFactory<T>().PageGridDataHandle(list);
                    return list;
                }
                else
                {
                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                    List<T> list = bll.GetPageEntities(out totalCount, out errMsg, permissionFilter, pageInfo.page, pageInfo.pagesize, orderFields, isDescs, expression, whereSql, references, connString);
                    pageInfo.totalCount = totalCount;
                    //导航属性处理
                    SetModelListForeignNameFieldValue(list, references, connString, dbType);
                    //网格数据处理
                    new OperateHandleFactory<T>().PageGridDataHandle(list);
                    return list;
                }
            }

            /// <summary>
            /// 根据字段获取实体记录集合
            /// </summary>
            /// <param name="fieldName">字段名称</param>
            /// <param name="fieldValue">字段值</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="permissionFilter">是否进行权限过滤</param>
            /// <param name="orderFields">排序字段</param>
            /// <param name="isDescs">是否降序</param>
            /// <param name="top">加载前多少条记录</param>
            /// <param name="references">是否加载关联对象数据（导航属性）</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public List<T> GetEntitiesByField(string fieldName, object fieldValue, out string errMsg, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, int? top = null, bool references = false, string connString = null, DatabaseType? dbType = null)
            {
                errMsg = string.Empty;
                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "GetEntitiesByField");
                if (methodCallType != MethodCallTypeEnum.CommonMethod)
                {
                    object[] args = new object[] { errMsg, fieldName, fieldValue, permissionFilter, orderFields, isDescs, top, references, connString };
                    object obj = ReflectExecuteBLLMethod(typeof(T), "GetEntitiesByField", args, dbType);
                    errMsg = args[0].ObjToStr();
                    List<T> list = obj as List<T>;
                    SetModelListForeignNameFieldValue(list, references, connString, dbType);
                    return list;
                }
                else
                {
                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                    List<T> list = bll.GetEntitiesByField(out errMsg, fieldName, fieldValue, permissionFilter, orderFields, isDescs, top, references, connString);
                    SetModelListForeignNameFieldValue(list, references, connString, dbType);
                    return list;
                }
            }

            /// <summary>
            /// 根据字段获取单条记录
            /// </summary>
            /// <param name="fieldName">字段名称</param>
            /// <param name="fieldValue">字段值</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="permissionFilter">是否进行权限过滤</param>
            /// <param name="orderFields">排序字段</param>
            /// <param name="isDescs">是否降序</param>
            /// <param name="references">是否加载关联对象数据（导航属性）</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public T GetEntitiesByFieldOne(string fieldName, object fieldValue, out string errMsg, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, bool references = false, string connString = null, DatabaseType? dbType = null)
            {
                List<T> list = GetEntitiesByField(fieldName, fieldValue, out errMsg, permissionFilter, orderFields, isDescs, null, references, connString, dbType);
                return list.FirstOrDefault();
            }

            /// <summary>
            /// 获取实体记录数
            /// </summary>
            /// <param name="errMsg">错误信息</param>
            /// <param name="permissionFilter">是否进行权限过滤</param>
            /// <param name="expression">条件表达式</param>
            /// <param name="whereSql">SQL条件语句</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public long Count(out string errMsg, bool permissionFilter = true, Expression<Func<T, bool>> expression = null, string whereSql = null, string connString = null, DatabaseType? dbType = null)
            {
                errMsg = string.Empty;
                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "Count");
                if (methodCallType != MethodCallTypeEnum.CommonMethod)
                {
                    object[] args = new object[] { errMsg, permissionFilter, expression, whereSql, connString };
                    object obj = ReflectExecuteBLLMethod(typeof(T), "Count", args, dbType);
                    errMsg = args[0].ObjToStr();
                    return obj.ObjToLong();
                }
                else
                {
                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                    return bll.Count(out errMsg, permissionFilter, expression, whereSql, connString);
                }
            }

            /// <summary>
            /// 加载实体对象的关联对象（导航属性）
            /// </summary>
            /// <param name="instance">实体对象</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            public void LoadReferences(T instance, out string errMsg, string connString = null, DatabaseType? dbType = null)
            {
                errMsg = string.Empty;
                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "LoadReferences");
                if (methodCallType != MethodCallTypeEnum.CommonMethod)
                {
                    object[] args = new object[] { instance, errMsg, connString };
                    object obj = ReflectExecuteBLLMethod(typeof(T), "LoadReferences", args, dbType);
                    errMsg = args[1].ObjToStr();
                }
                else
                {
                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                    bll.LoadReferences(instance, out errMsg, connString);
                }
            }

            /// <summary>
            /// 加载关联对象（导航属性）
            /// </summary>
            /// <param name="instances">实体对象集合</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            public void LoadListReferences(List<T> instances, out string errMsg, string connString = null, DatabaseType? dbType = null)
            {
                errMsg = string.Empty;
                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "LoadListReferences");
                if (methodCallType != MethodCallTypeEnum.CommonMethod)
                {
                    object[] args = new object[] { instances, errMsg, connString };
                    object obj = ReflectExecuteBLLMethod(typeof(T), "LoadListReferences", args, dbType);
                    errMsg = args[1].ObjToStr();
                }
                else
                {
                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                    bll.LoadListReferences(instances, out errMsg, connString);
                }
            }

            #endregion

            #region 增删改

            /// <summary>
            /// 操作单条记录
            /// </summary>
            /// <param name="t">实体对象</param>
            /// <param name="operateType">操作类型</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="fieldNames">更新时用到，要更新的字段</param>
            /// <param name="permissionValidate">是否进行权限验证，针对编辑删除</param>
            /// <param name="references">是否保存关联对象数据（导航属性）</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <param name="transConn">事务连接对象</param>
            /// <returns>如果是新增返回新增后记录Id,否则成功返回1，失败返回0</returns>
            public Guid OperateRecord(T t, ModelRecordOperateType operateType, out string errMsg, List<string> fieldNames = null, bool permissionValidate = true, bool references = false, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
            {
                errMsg = string.Empty;
                //操作前验证
                bool verifyResult = new OperateHandleFactory<T>().BeforeOperateVerifyOrHandle(operateType, t, out errMsg);
                if (!verifyResult) //验证不通过
                {
                    return Guid.Empty;
                }
                PropertyInfo pId = typeof(T).GetProperty("Id");
                Guid id = pId.GetValue(t, null).ObjToGuid();
                Guid rs = Guid.Empty;
                switch (operateType)
                {
                    case ModelRecordOperateType.Add:
                        {
                            #region 新增
                            Guid recordId = Guid.Empty;
                            MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "AddEntity");
                            if (methodCallType != MethodCallTypeEnum.CommonMethod)
                            {
                                object[] args = new object[] { t, errMsg, references, connString, transConn };
                                object obj = ReflectExecuteBLLMethod(typeof(T), "AddEntity", args, dbType);
                                errMsg = args[1].ObjToStr();
                                recordId = obj.ObjToGuid();
                            }
                            else
                            {
                                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                                recordId = bll.AddEntity(t, out errMsg, references, connString, transConn);
                            }
                            PropertyInfo p = typeof(T).GetProperty("Id");
                            p.SetValue(t, recordId, null);
                            id = recordId;
                            rs = recordId;
                            #endregion
                        }
                        break;
                    case ModelRecordOperateType.Edit:
                        {
                            #region 编辑
                            MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "UpdateEntity");
                            if (methodCallType != MethodCallTypeEnum.CommonMethod)
                            {
                                object[] args = new object[] { t, errMsg, fieldNames, permissionValidate, references, connString, transConn };
                                object obj = ReflectExecuteBLLMethod(typeof(T), "UpdateEntity", args, dbType);
                                errMsg = args[1].ObjToStr();
                                rs = obj.ObjToBool() ? id : Guid.Empty;
                            }
                            else
                            {
                                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                                rs = bll.UpdateEntity(t, out errMsg, fieldNames, permissionValidate, references, connString, transConn) ? id : Guid.Empty;
                            }
                            #endregion
                        }
                        break;
                    case ModelRecordOperateType.Del:
                        {
                            #region 删除
                            Type modelType = typeof(T);
                            string tableName = modelType.Name;
                            Sys_Module module = SystemOperate.GetModuleByTableName(tableName);
                            PropertyInfo pIsDelete = modelType.GetProperty("IsDeleted");
                            PropertyInfo pDeleteTime = modelType.GetProperty("DeleteTime");
                            //先判断系统是否启用软删除
                            if (module.IsEnabledRecycle) //软删除
                            {
                                pIsDelete.SetValue(t, true, null);
                                pDeleteTime.SetValue(t, DateTime.Now, null);
                                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "UpdateFields");
                                if (methodCallType != MethodCallTypeEnum.CommonMethod) //自定义更新字段方法
                                {
                                    object[] args = new object[] { new List<T>() { t }, errMsg, new List<string>() { "IsDeleted", "DeleteTime" }, connString, transConn };
                                    object obj = ReflectExecuteBLLMethod(typeof(T), "UpdateFields", args, dbType);
                                    errMsg = args[1].ObjToStr();
                                    rs = obj.ObjToBool() ? id : Guid.Empty;
                                }
                                else
                                {
                                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                                    bool result = bll.UpdateFields(new List<T>() { t }, out errMsg, new List<string>() { "IsDeleted", "DeleteTime" }, connString, transConn);
                                    rs = result ? id : Guid.Empty;
                                }
                            }
                            else //硬删除
                            {
                                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "DeleteEntityByIds");
                                if (methodCallType != MethodCallTypeEnum.CommonMethod) //自定义删除方法
                                {
                                    object[] args = new object[] { new List<Guid>() { id }, errMsg, permissionValidate, connString, transConn };
                                    object obj = ReflectExecuteBLLMethod(typeof(T), "DeleteEntityByIds", args, dbType);
                                    errMsg = args[1].ObjToStr();
                                    rs = obj.ObjToBool() ? id : Guid.Empty;
                                }
                                else
                                {
                                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                                    bool result = bll.DeleteEntityByIds(new List<Guid>() { id }, out errMsg, permissionValidate, connString, transConn);
                                    rs = result ? id : Guid.Empty;
                                }
                            }
                            #endregion
                        }
                        break;
                }
                //执行操作完成事件
                new OperateHandleFactory<T>().OperateCompeletedHandle(operateType, t, rs != Guid.Empty, UserInfo.CurrentUserInfo);
                return rs;
            }

            /// <summary>
            /// 操作记录集合
            /// </summary>
            /// <param name="ts">记录集合</param>
            /// <param name="operateType">操作类型</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="permissionValidate">是否进行权限验证，针对编辑删除</param>
            /// <param name="references">是否保存关联对象数据（导航属性）</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <param name="transConn">事务连接对象</param>
            /// <returns>返回更新结果</returns>
            public bool OperateRecords(List<T> ts, ModelRecordOperateType operateType, out string errMsg, bool permissionValidate = true, bool references = false, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
            {
                errMsg = string.Empty;
                //操作前验证
                bool verifyResult = new OperateHandleFactory<T>().BeforeOperateVerifyOrHandles(operateType, ts, out errMsg);
                if (!verifyResult) //验证不通过
                {
                    return false;
                }
                bool rs = false;
                switch (operateType)
                {
                    case ModelRecordOperateType.Add:
                        {
                            #region 新增
                            MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "AddEntities");
                            if (methodCallType != MethodCallTypeEnum.CommonMethod)
                            {
                                object[] args = new object[] { ts, errMsg, references, connString, transConn };
                                object obj = ReflectExecuteBLLMethod(typeof(T), "AddEntities", args, dbType);
                                errMsg = args[1].ObjToStr();
                                rs = obj.ObjToBool();
                            }
                            else
                            {
                                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                                rs = bll.AddEntities(ts, out errMsg, references, connString, transConn);
                            }
                            #endregion
                        }
                        break;
                    case ModelRecordOperateType.Edit:
                        {
                            #region 编辑
                            MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "UpdateEntities");
                            if (methodCallType != MethodCallTypeEnum.CommonMethod)
                            {
                                object[] args = new object[] { ts, errMsg, permissionValidate, references, connString, transConn };
                                object obj = ReflectExecuteBLLMethod(typeof(T), "UpdateEntities", args, dbType);
                                errMsg = args[1].ObjToStr();
                                rs = obj.ObjToBool();
                            }
                            else
                            {
                                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                                rs = bll.UpdateEntities(ts, out errMsg, permissionValidate, references, connString, transConn);
                            }
                            #endregion
                        }
                        break;
                    case ModelRecordOperateType.Del:
                        {
                            #region 删除
                            List<Guid> listId = new List<Guid>();
                            Type modelType = typeof(T);
                            string tableName = modelType.Name;
                            Sys_Module module = SystemOperate.GetModuleByTableName(tableName);
                            PropertyInfo pIsDelete = modelType.GetProperty("IsDeleted");
                            PropertyInfo pDeleteTime = modelType.GetProperty("DeleteTime");
                            PropertyInfo pId = modelType.GetProperty("Id");
                            foreach (T t in ts)
                            {
                                Guid id = pId.GetValue(t, null).ObjToGuid();
                                listId.Add(id);
                                if (module.IsEnabledRecycle)
                                {
                                    pIsDelete.SetValue(t, true, null);
                                    pDeleteTime.SetValue(t, DateTime.Now, null);
                                }
                            }
                            //先判断系统是否启用软删除
                            if (module.IsEnabledRecycle) //软删除
                            {
                                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "UpdateFields");
                                if (methodCallType != MethodCallTypeEnum.CommonMethod) //自定义更新字段方法
                                {
                                    object[] args = new object[] { ts, errMsg, new List<string>() { "IsDeleted", "DeleteTime" }, connString, transConn };
                                    object obj = ReflectExecuteBLLMethod(typeof(T), "UpdateFields", args, dbType);
                                    errMsg = args[1].ObjToStr();
                                    rs = obj.ObjToBool();
                                }
                                else
                                {
                                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                                    rs = bll.UpdateFields(ts, out errMsg, new List<string>() { "IsDeleted", "DeleteTime" }, connString, transConn);
                                }
                            }
                            else //硬删除
                            {
                                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "DeleteEntityByIds");
                                if (methodCallType != MethodCallTypeEnum.CommonMethod) //自定义删除方法
                                {
                                    object[] args = new object[] { listId, errMsg, permissionValidate, connString, transConn };
                                    object obj = ReflectExecuteBLLMethod(typeof(T), "DeleteEntityByIds", args, dbType);
                                    errMsg = args[1].ObjToStr();
                                    rs = obj.ObjToBool();
                                }
                                else
                                {
                                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                                    rs = bll.DeleteEntityByIds(listId, out errMsg, permissionValidate, connString, transConn);
                                }
                            }
                            #endregion
                        }
                        break;
                }
                //执行操作完成后事件
                new OperateHandleFactory<T>().OperateCompeletedHandles(operateType, ts, rs, UserInfo.CurrentUserInfo);
                return rs;
            }

            /// <summary>
            /// 根据条件表达式更新记录
            ///  UpdateEntityByExpression(new { FirstName = "JJ" }, p => p.LastName == "Hendrix");
            ///  UPDATE "Person" SET "FirstName" = 'JJ' WHERE ("LastName" = 'Hendrix')
            /// </summary>
            /// <param name="obj">要更新的值对象</param>
            /// <param name="expression">条件表达式</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <param name="transConn">事务连接对象</param>
            /// <returns></returns>
            public bool UpdateRecordsByExpression(object obj, Expression<Func<T, bool>> expression, out string errMsg, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
            {
                errMsg = string.Empty;
                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "UpdateEntityByExpression");
                if (methodCallType != MethodCallTypeEnum.CommonMethod)
                {
                    object[] args = new object[] { obj, expression, errMsg, connString, transConn };
                    object objBll = ReflectExecuteBLLMethod(typeof(T), "UpdateEntityByExpression", args, dbType);
                    errMsg = args[2].ObjToStr();
                    return objBll.ObjToBool();
                }
                else
                {
                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                    return bll.UpdateEntityByExpression(obj, expression, out errMsg, connString, transConn);
                }
            }

            /// <summary>
            /// 删除记录
            /// </summary>
            /// <param name="ids">id集合</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="isSoftDel">是否软删除</param>
            /// <param name="permissionValidate">是否进行权限验证，针对硬删除</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <param name="transConn">事务连接对象</param>
            /// <returns></returns>
            public bool DeleteRecords(IEnumerable ids, out string errMsg, bool isSoftDel = false, bool permissionValidate = true, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
            {
                errMsg = string.Empty;
                Type modelType = typeof(T);
                PropertyInfo pIsDelete = modelType.GetProperty("IsDeleted");
                PropertyInfo pDeleteTime = modelType.GetProperty("DeleteTime");
                List<T> ts = new List<T>();
                Guid moduleId = SystemOperate.GetModuleIdByTableName(modelType.Name);
                foreach (object id in ids)
                {
                    object t = CommonOperate.GetEntityById(moduleId, id.ObjToGuid(), out errMsg);
                    if (t == null) continue;
                    if (isSoftDel)
                    {
                        pIsDelete.SetValue(t, true, null);
                        pDeleteTime.SetValue(t, DateTime.Now, null);
                    }
                    ts.Add(t as T);
                }
                //操作前验证
                bool verifyResult = new OperateHandleFactory<T>().BeforeOperateVerifyOrHandles(ModelRecordOperateType.Del, ts, out errMsg);
                if (!verifyResult) //验证不通过
                {
                    return false;
                }
                bool rs = false;
                //先判断系统是否启用软删除
                if (isSoftDel) //软删除
                {
                    MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "UpdateFields");
                    if (methodCallType != MethodCallTypeEnum.CommonMethod) //自定义更新字段方法
                    {
                        object[] args = new object[] { ts, errMsg, new List<string>() { "IsDeleted", "DeleteTime" }, connString, transConn };
                        object obj = ReflectExecuteBLLMethod(typeof(T), "UpdateFields", args, dbType);
                        errMsg = args[1].ObjToStr();
                        rs = obj.ObjToBool();
                    }
                    else
                    {
                        IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                        rs = bll.UpdateFields(ts, out errMsg, new List<string>() { "IsDeleted", "DeleteTime" }, connString, transConn);
                    }
                }
                else //硬删除
                {
                    MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "DeleteEntityByIds");
                    if (methodCallType != MethodCallTypeEnum.CommonMethod) //自定义删除方法
                    {
                        object[] args = new object[] { ids, errMsg, permissionValidate, connString, transConn };
                        object obj = ReflectExecuteBLLMethod(typeof(T), "DeleteEntityByIds", args, dbType);
                        errMsg = args[1].ObjToStr();
                        rs = obj.ObjToBool();
                    }
                    else
                    {
                        IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                        rs = bll.DeleteEntityByIds(ids, out errMsg, permissionValidate, connString, transConn);
                    }
                }
                //执行操作完成事件
                new OperateHandleFactory<T>().OperateCompeletedHandles(ModelRecordOperateType.Del, ts, rs, UserInfo.CurrentUserInfo);
                return rs;
            }

            /// <summary>
            /// 根据条件删除记录
            /// </summary>
            /// <param name="expression">条件表达式</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="isSoftDel">是否软删除</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <param name="transConn">事务连接对象</param>
            /// <returns></returns>
            public bool DeleteRecordsByExpression(Expression<Func<T, bool>> expression, out string errMsg, bool isSoftDel = false, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
            {
                errMsg = string.Empty;
                //先判断系统是否启用软删除
                if (isSoftDel) //软删除
                {
                    object objArg = new { IsDeleted = true, DeleteTime = DateTime.Now };
                    MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "UpdateEntityByExpression");
                    if (methodCallType != MethodCallTypeEnum.CommonMethod) //自定义更新字段方法
                    {
                        object[] args = new object[] { objArg, expression, errMsg, connString, transConn };
                        object obj = ReflectExecuteBLLMethod(typeof(T), "UpdateEntityByExpression", args, dbType);
                        errMsg = args[2].ObjToStr();
                        return obj.ObjToBool();
                    }
                    else
                    {
                        IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                        return bll.UpdateEntityByExpression(objArg, expression, out errMsg, connString, transConn);
                    }
                }
                else //硬删除
                {
                    MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "DeleteEntity");
                    if (methodCallType != MethodCallTypeEnum.CommonMethod) //自定义删除方法
                    {
                        object[] args = new object[] { expression, errMsg, connString, transConn };
                        object obj = ReflectExecuteBLLMethod(typeof(T), "DeleteEntity", args);
                        errMsg = args[1].ObjToStr();
                        return obj.ObjToBool();
                    }
                    else
                    {
                        IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>();
                        return bll.DeleteEntity(expression, out errMsg, connString, transConn);
                    }
                }
            }

            #endregion

            #region 通用树

            /// <summary>
            /// 获取树结点
            /// </summary>
            /// <param name="parentId">父记录Id</param>
            /// <param name="attributeParams">扩展属性处理方法</param>
            /// <param name="pIdFieldName">父Id字段名</param>
            /// <param name="textName">节点显示字段名</param>
            /// <param name="sortField">排序字段名</param>
            /// <param name="icon">图标类</param>
            /// <param name="expression">过滤表达式</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public TreeNode GetTreeNode(Guid? parentId, Action<T, TreeAttributes> attributeParams = null, string pIdFieldName = "ParentId", string textName = "Name", string sortField = "Id", string icon = null, Expression<Func<T, bool>> expression = null, string connString = null, DatabaseType? dbType = null)
            {
                string errMsg = string.Empty;
                string pidName = string.IsNullOrEmpty(pIdFieldName) ? "ParentId" : pIdFieldName;
                string txtName = string.IsNullOrEmpty(textName) ? "Name" : textName;
                T root = null; //根结点
                List<T> childs = GetAllChildNodesData(parentId, pidName, sortField, expression, connString, dbType); //子结点集合
                if (childs != null) childs = childs.ToList();
                PropertyInfo pId = typeof(T).GetProperty("Id");
                PropertyInfo pIsDeleted = typeof(T).GetProperty("IsDeleted");
                PropertyInfo pIsDraft = typeof(T).GetProperty("IsDraft");
                if (!parentId.HasValue || parentId.Value == Guid.Empty) //没有指定父结点时
                {
                    //找出所有父结点Id为Guid.Empty或null的
                    List<T> tempList = new List<T>();
                    List<T> tmpTs1 = GetEntitiesByField<T>(pidName, null, out errMsg, true, new List<string>() { sortField }, new List<bool>() { false }, null, false, connString, dbType);
                    if (tmpTs1 != null && tmpTs1.Count > 0)
                    {
                        foreach (T t in tmpTs1) //排序已删除和草稿
                        {
                            if (pIsDeleted != null && pIsDeleted.GetValue(t, null).ObjToBool())
                                continue;
                            if (pIsDraft != null && pIsDraft.GetValue(t, null).ObjToBool())
                                continue;
                            tempList.Add(t);
                            T removeT = null; //要移除的对象
                            foreach (T tt in childs)
                            {
                                if (pId.GetValue(t, null).ObjToGuid() == pId.GetValue(tt, null).ObjToGuid())
                                {
                                    removeT = tt;
                                    break;
                                }
                            }
                            if (removeT != null)
                            {
                                childs.Remove(removeT);
                            }
                        }
                    }
                    List<T> tmpTs2 = GetEntitiesByField<T>(pidName, Guid.Empty, out errMsg, true, new List<string>() { sortField }, new List<bool>() { false }, null, false, connString, dbType);
                    if (tmpTs2 != null && tmpTs2.Count > 0)
                    {
                        foreach (T t in tmpTs2) //排序已删除和草稿
                        {
                            if (pIsDeleted != null && pIsDeleted.GetValue(t, null).ObjToBool())
                                continue;
                            if (pIsDraft != null && pIsDraft.GetValue(t, null).ObjToBool())
                                continue;
                            tempList.Add(t);
                            T removeT = null; //要移除的对象
                            foreach (T tt in childs)
                            {
                                if (pId.GetValue(t, null).ObjToGuid() == pId.GetValue(tt, null).ObjToGuid())
                                {
                                    removeT = tt;
                                    break;
                                }
                            }
                            if (removeT != null)
                            {
                                childs.Remove(removeT);
                            }
                        }
                    }
                    if (tempList.Count > 1) //存在为Guid.Empty或null的父结点，并且有多个
                    {
                        object obj = Activator.CreateInstance(typeof(T)); //再创建一个根结点
                        PropertyInfo nameProperty = typeof(T).GetProperty(txtName);
                        nameProperty.SetValue(obj, "根结点", null);
                        PropertyInfo pidProperty = typeof(T).GetProperty(pidName);
                        tempList.ForEach(x => { pidProperty.SetValue(x, Guid.Empty, null); }); //将多个为0或null的父结点指向根结点
                        childs.AddRange(tempList);
                        root = obj as T;
                    }
                    else
                    {
                        root = tempList.FirstOrDefault();
                    }
                }
                else
                {
                    root = GetEntityById(parentId.Value, out errMsg, false, connString, dbType);
                    if (root != null)
                    {
                        if ((pIsDeleted != null && pIsDeleted.GetValue(root, null).ObjToBool()) || (pIsDraft != null && pIsDraft.GetValue(root, null).ObjToBool())) //不存在根结点
                        {
                            object obj = Activator.CreateInstance(typeof(T)); //再创建一个根结点
                            PropertyInfo nameProperty = typeof(T).GetProperty(txtName);
                            nameProperty.SetValue(obj, "根结点", null);
                            PropertyInfo pidProperty = typeof(T).GetProperty(pidName);
                            root = obj as T;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                var tree = GetTree(childs, root, attributeParams, pidName, txtName, icon);
                return tree;
            }

            /// <summary>
            /// 获取树结构模块所有的子结点记录
            /// </summary>
            /// <param name="parentId">父记录Id</param>
            /// <param name="pIdFieldName">父Id字段名</param>
            /// <param name="sortField">排序字段名</param>
            /// <param name="expression">过滤表达式</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public List<T> GetAllChildNodesData(Guid? parentId, string pIdFieldName = "ParentId", string sortField = "Id", Expression<Func<T, bool>> expression = null, string connString = null, DatabaseType? dbType = null)
            {
                List<T> list = new List<T>();
                string pidName = string.IsNullOrEmpty(pIdFieldName) ? "ParentId" : pIdFieldName;
                //找直接子结点
                List<T> listNodes = GetChildNodesData(parentId, pidName, sortField, expression, connString, dbType);
                //根节点特殊处理
                List<T> tempNodes = new List<T>();
                if (parentId == null)
                {
                    tempNodes = GetChildNodesData(Guid.Empty, pidName, sortField, expression, connString, dbType);
                }
                if (parentId == Guid.Empty)
                {
                    tempNodes = GetChildNodesData(null, pidName, sortField, expression, connString, dbType);
                }
                if (tempNodes != null) listNodes.AddRange(tempNodes);
                foreach (T node in listNodes)
                {
                    PropertyInfo pid = typeof(T).GetProperty("Id");
                    Guid tempId = pid.GetValue(node, null).ObjToGuid();
                    list.Add(node);
                    list.AddRange(GetAllChildNodesData(tempId, pidName, sortField, expression, connString, dbType));
                }
                return list;
            }

            /// <summary>
            /// 获取树结构模块直接子结点记录
            /// </summary>
            /// <param name="parentId">父记录Id</param>
            /// <param name="pIdFieldName">父Id字段名</param>
            /// <param name="sortField">排序字段名</param>
            /// <param name="expression">过滤表达式</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public List<T> GetChildNodesData(Guid? parentId, string pIdFieldName = "ParentId", string sortField = "Id", Expression<Func<T, bool>> expression = null, string connString = null, DatabaseType? dbType = null)
            {
                string errMsg = string.Empty;
                string pidName = string.IsNullOrEmpty(pIdFieldName) ? "ParentId" : pIdFieldName;
                MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "GetEntitiesByField");
                PropertyInfo pIsDeleted = typeof(T).GetProperty("IsDeleted");
                PropertyInfo pIsDraft = typeof(T).GetProperty("IsDraft");
                if (methodCallType != MethodCallTypeEnum.CommonMethod)
                {
                    object[] args = new object[] { errMsg, pidName, parentId, true, sortField, false, null, false, connString };
                    object obj = ReflectExecuteBLLMethod(typeof(T), "GetEntitiesByField", args, dbType);
                    errMsg = args[0].ToString();
                    List<T> list = obj as List<T>;
                    if (expression != null) list = list.Where(expression.Compile()).ToList();
                    List<T> tempList = new List<T>();
                    foreach (T t in list) //排序已删除和草稿
                    {
                        if (pIsDeleted != null && pIsDeleted.GetValue(t, null).ObjToBool())
                            continue;
                        if (pIsDraft != null && pIsDraft.GetValue(t, null).ObjToBool())
                            continue;
                        tempList.Add(t);
                    }
                    return tempList;
                }
                else
                {
                    IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                    List<T> list = bll.GetEntitiesByField(out errMsg, pidName, parentId, true, new List<string>() { sortField }, new List<bool>() { false }, null, false, connString);
                    if (expression != null) list = list.Where(expression.Compile()).ToList();
                    List<T> tempList = new List<T>();
                    foreach (T t in list) //排序已删除和草稿
                    {
                        if (pIsDeleted != null && pIsDeleted.GetValue(t, null).ObjToBool())
                            continue;
                        if (pIsDraft != null && pIsDraft.GetValue(t, null).ObjToBool())
                            continue;
                        tempList.Add(t);
                    }
                    return tempList;
                }
            }

            #endregion

            #region 事务操作

            /// <summary>
            /// 执行事务
            /// </summary>
            /// <param name="transactionObjects">事务对象集合</param>
            /// <param name="errMsg">异常信息</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            public void ExecuteTransaction(List<TransactionModel<T>> transactionObjects, out string errMsg, string connString = null, DatabaseType? dbType = null)
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                bll.ExecuteTransaction(transactionObjects, out errMsg, connString);
            }

            #endregion

            #region 查询条件

            /// <summary>
            /// 获取Lamda表达式的查询条件
            /// </summary>
            /// <param name="conditionItems">条件集合</param>
            /// <returns></returns>
            public Expression<Func<T, bool>> GetQueryCondition(List<ConditionItem> conditionItems)
            {
                return new QueryableSearcher<T>().GetQueryCondition(conditionItems);
            }

            /// <summary>
            /// 获取查询条件
            /// </summary>
            /// <param name="dicCondition">条件集合</param>
            /// <returns></returns>
            public Expression<Func<T, bool>> GetQueryConditionDic(Dictionary<string, string> dicCondition)
            {
                return new QueryableSearcher<T>().GetQueryCondition(dicCondition);
            }

            /// <summary>
            /// 返回网格过滤条件
            /// </summary>
            /// <param name="whereSql">过滤条件语句</param>
            /// <param name="q">搜索条件</param>
            /// <param name="gridType">网格类型</param>
            /// <param name="condition">条件参数</param>
            /// <param name="initModule">原始模块（弹出选择外键模块数据的初始模块），弹出列表用到</param>
            /// <param name="initField">原始字段（弹出选择外键模块数据的初始字段），弹出列表用到</param>
            /// <param name="otherParams">其他参数</param>
            /// <param name="viewId">当前网格视图Id</param>
            /// <returns>返回条件表达式</returns>
            public Expression<Func<T, bool>> GetGridFilterCondition(ref string whereSql, Dictionary<string, string> q, DataGridType gridType, Dictionary<string, string> condition = null, string initModule = null, string initField = null, Dictionary<string, string> otherParams = null, Guid? viewId = null)
            {
                string errMsg = string.Empty;
                string tableName = typeof(T).Name;
                Sys_Module module = SystemOperate.GetModuleByTableName(tableName);
                Expression<Func<T, bool>> expression = null;
                if (module.IsEnabledDraft && gridType == DataGridType.MyDraftGrid) //草稿
                {
                    expression = GetQueryCondition<T>(new List<ConditionItem>() 
                    { 
                         new ConditionItem() { Field = "IsDeleted", Method = QueryMethod.Equal, Value = false }, 
                         new ConditionItem() { Field = "IsDraft", Method = QueryMethod.Equal, Value = true }, 
                         new ConditionItem() { Field = "CreateUserId", Method = QueryMethod.Equal, Value = UserInfo.CurrentUserInfo.UserId } 
                    });
                }
                else
                {
                    expression = GetQueryCondition<T>(new List<ConditionItem>() 
                    { 
                        new ConditionItem() { Field = "IsDeleted", Method = QueryMethod.Equal, Value = gridType == DataGridType.RecycleGrid ? true : false },
                        new ConditionItem() { Field = "IsDraft", Method = QueryMethod.Equal, Value = false }, 
                    });
                }
                //调用自定义重写过滤条件
                string where = string.Empty;
                Expression<Func<T, bool>> conditionExp = new OperateHandleFactory<T>().GetGridFilterCondition(out where, gridType, condition, initModule, initField, otherParams);
                if (!string.IsNullOrWhiteSpace(where))
                {
                    if (!string.IsNullOrWhiteSpace(whereSql)) whereSql += " AND ";
                    whereSql += where;
                }
                //自定义过滤条件为空时系统自动解析过滤条件
                if (conditionExp == null && condition != null)
                {
                    #region 自定义过滤条件处理
                    List<ConditionItem> items = new List<ConditionItem>();
                    foreach (string fieldName in condition.Keys)
                    {
                        Sys_Field field = SystemOperate.GetFieldInfo(module.Id, fieldName);
                        if (field == null || condition[fieldName] == null) continue;
                        bool isTree = false;
                        Sys_Module foreignModule = SystemOperate.GetModuleByName(field.ForeignModuleName); //外键模块
                        Type foreignModelType = null;
                        if (foreignModule != null)
                        {
                            foreignModelType = CommonOperate.GetModelType(foreignModule.Id); //外键模块实体类型
                            if (foreignModelType.GetProperty("ParentId") != null) //外键模块是树型模块
                            {
                                isTree = true;
                            }
                        }
                        if (isTree)
                        {
                            string tempFieldName = fieldName;
                            string tempSql = string.Empty;
                            PropertyInfo treeValuePathProperty = foreignModelType.GetProperty("TreeValuePath");
                            if (fieldName == "ParentId") tempFieldName = "Id";
                            if (treeValuePathProperty == null) //不包含TreeValuePath字段循环取子结点Id
                            {
                                object tempList = CommonOperate.GetAllChildNodesData(foreignModule.Id, condition[fieldName].ObjToGuid());
                                PropertyInfo p = foreignModelType.GetProperty("Id");
                                List<string> tempIds = new List<string>();
                                if (condition[fieldName].ObjToGuid() != Guid.Empty)
                                    tempIds.Add(string.Format("{0}", condition[fieldName].ObjToGuid()));
                                    //Gary 屏蔽因两个单引号问题 2016-08-15 16:12
                                    //tempIds.Add(string.Format("'{0}'", condition[fieldName].ObjToGuid()));
                                foreach (object tempObj in (tempList as IEnumerable))
                                {
                                    Guid tempId = p.GetValue(tempObj, null).ObjToGuid();
                                    if (tempId != Guid.Empty && !tempIds.Contains(string.Format("{0}", tempId)))
                                        tempIds.Add(string.Format("{0}", tempId));
                                        //Gary 屏蔽因两个单引号问题 2016-08-15 16:12
                                        //tempIds.Add(string.Format("'{0}'", tempId));
                                }
                                string tempIdStr = string.Join("','", tempIds);
                                if (!string.IsNullOrEmpty(tempIdStr))
                                {
                                    tempSql = string.Format("{0} IN ('{1}')", tempFieldName, tempIdStr);
                                }
                            }
                            else //包含TreeValuePath字段
                            {
                                tempSql = string.Format("{0} IN (SELECT Id FROM {1} WHERE TreeValuePath LIKE '%{2}%')", tempFieldName, ModelConfigHelper.GetModuleTableName(foreignModelType), condition[fieldName]);
                            }
                            if (!string.IsNullOrWhiteSpace(whereSql)) whereSql += " AND ";
                            whereSql += tempSql;
                            continue;
                        }
                        else
                        {
                            items.Add(new ConditionItem() { Field = fieldName, Method = QueryMethod.Equal, Value = condition[fieldName] });
                        }
                    }
                    if (items.Count > 0)
                    {
                        conditionExp = GetQueryCondition<T>(items);
                    }
                    #endregion
                }
                if (conditionExp != null) expression = conditionExp;
                if (q != null && q.Count > 0)
                {
                    #region 搜索条件处理
                    List<ConditionItem> items = new List<ConditionItem>();
                    string tempQWhere=string.Empty;
                    items = new OperateHandleFactory<T>().GetSearchResults(q, out tempQWhere);
                    if ((items == null || items.Count == 0) && string.IsNullOrWhiteSpace(tempQWhere)) //不存在重写搜索结果
                    {
                        items = new List<ConditionItem>();
                        if (!q.Keys.Contains("Id"))
                        {
                            foreach (string fieldName in q.Keys)
                            {
                                if (q[fieldName] == null) continue;
                                string tempSingleQWhere = string.Empty;
                                ConditionItem tempSingleQItem = new OperateHandleFactory<T>().GetSearchResult(fieldName,q[fieldName], out tempSingleQWhere);
                                if (tempSingleQItem == null && string.IsNullOrWhiteSpace(tempSingleQWhere)) //不存在重写单个字段搜索接口
                                {
                                    Sys_Field sysField = SystemOperate.GetFieldInfo(module.Id, fieldName);
                                    if (sysField == null) //非当前模块字段
                                    {
                                        if (!viewId.HasValue) continue;
                                        //明细主模块或外键字段处理
                                        Sys_GridField gridField = SystemOperate.GetGridField(viewId.Value, fieldName);
                                        if (!gridField.Sys_FieldId.HasValue) continue;
                                        sysField = SystemOperate.GetFieldById(gridField.Sys_FieldId.Value);
                                        if (sysField == null) continue;
                                    }
                                    //外键字段搜索特殊处理
                                    Sys_Module foreignModule = SystemOperate.GetModuleByName(sysField.ForeignModuleName);
                                    Type foreignModelType = null; //外键模块类型
                                    Sys_FormField formField = SystemOperate.GetDefaultFormSingleField(module.Id, fieldName);
                                    Type fieldType = SystemOperate.GetFieldType(module.Id, fieldName);
                                    if (formField != null || fieldType != null) //当前模块字段
                                    {
                                        #region 外键字段
                                        if (foreignModule != null) //外键字段
                                        {
                                            foreignModelType = CommonOperate.GetModelType(foreignModule.Id); //外键模块实体类型
                                            string[] token = q[fieldName].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                            string innerWhere = string.Empty;
                                            foreach (string str in token)
                                            {
                                                if (string.IsNullOrEmpty(str)) continue;
                                                if (!string.IsNullOrEmpty(innerWhere)) innerWhere += " OR ";
                                                innerWhere += string.Format("{0} LIKE '%{1}%'", foreignModule.TitleKey, str);
                                            }
                                            if (!string.IsNullOrEmpty(innerWhere))
                                            {
                                                string tempWhereSql = string.Format("{0} IN(SELECT Id FROM {1} WHERE {2} IS NOT NULL AND ({3}))", fieldName, ModelConfigHelper.GetModuleTableName(foreignModelType), foreignModule.TitleKey, innerWhere);
                                                if (!string.IsNullOrWhiteSpace(whereSql)) whereSql += " AND ";
                                                whereSql += tempWhereSql;
                                            }
                                            continue;
                                        }
                                        #endregion
                                        #region 日期字段
                                        //日期搜索特殊处理
                                        if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?) || (formField != null && (formField.ControlTypeOfEnum == ControlTypeEnum.DateTimeBox || formField.ControlTypeOfEnum == ControlTypeEnum.DateBox)))
                                        {
                                            items.Add(new ConditionItem() { Field = fieldName, Method = QueryMethod.GreaterThanOrEqual, Value = q[fieldName] });
                                            string endFieldName = string.Format("{0}_End", fieldName); //结束日期字段
                                            if (q.Keys.Contains(endFieldName) && !string.IsNullOrWhiteSpace(q[endFieldName]))
                                            {
                                                items.Add(new ConditionItem() { Field = fieldName, Method = QueryMethod.LessThanOrEqual, Value = q[endFieldName] });
                                            }
                                        }
                                        #endregion
                                        #region 其他类型字段
                                        else //其他类型字段搜索处理
                                        {
                                            QueryMethod queryMethod = QueryMethod.Contains;
                                            if (fieldType == typeof(Int16) || fieldType == typeof(Int32) || fieldType == typeof(Int64) ||
                                                fieldType == typeof(Int16?) || fieldType == typeof(Int32?) || fieldType == typeof(Int64?) ||
                                                fieldType == typeof(float) || fieldType == typeof(Double) || fieldType == typeof(Decimal) ||
                                                fieldType == typeof(float?) || fieldType == typeof(Double?) || fieldType == typeof(Decimal?) ||
                                                (formField != null && (formField.ControlTypeOfEnum == ControlTypeEnum.SingleCheckBox ||
                                                formField.ControlTypeOfEnum == ControlTypeEnum.NumberBox ||
                                                formField.ControlTypeOfEnum == ControlTypeEnum.IntegerBox ||
                                                formField.ControlTypeOfEnum == ControlTypeEnum.ComboBox)))
                                            {
                                                queryMethod = QueryMethod.Equal;
                                            }
                                            string[] token = q[fieldName].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                            if (token.Length > 1) //多值搜索
                                            {
                                                string innerWhere = string.Empty;
                                                string methodChar = "LIKE";
                                                if (queryMethod == QueryMethod.Equal) methodChar = "=";
                                                foreach (string str in token)
                                                {
                                                    if (string.IsNullOrEmpty(str)) continue;
                                                    if (!string.IsNullOrEmpty(innerWhere)) innerWhere += " OR ";
                                                    string v = queryMethod == QueryMethod.Equal ? string.Format("'{0}'", str) : string.Format("'%{0}%'", str);
                                                    innerWhere += string.Format("{0} {1} {2}", fieldName, methodChar, v);
                                                }
                                                if (!string.IsNullOrEmpty(innerWhere))
                                                {
                                                    string tempWhereSql = string.Format("{0} IS NOT NULL AND ({1})", fieldName, innerWhere);
                                                    if (!string.IsNullOrWhiteSpace(whereSql)) whereSql += " AND ";
                                                    whereSql += tempWhereSql;
                                                }
                                            }
                                            else
                                            {
                                                items.Add(new ConditionItem() { Field = fieldName, Method = queryMethod, Value = q[fieldName] });
                                            }
                                        }
                                        #endregion
                                    }
                                    else //明细主模块或外键模块字段
                                    {
                                        if (!viewId.HasValue) continue;
                                        Type tempType = GetModelType(sysField.Sys_ModuleId.Value); //明细主模块或外键模块类型
                                        if (tempType == null) continue;
                                        fieldType = SystemOperate.GetFieldType(tempType, fieldName);
                                        formField = SystemOperate.GetDefaultFormSingleField(sysField);
                                        if (formField == null && fieldType == null) continue;
                                        Sys_Grid grid = SystemOperate.GetGrid(viewId.Value);
                                        if (grid == null) continue;
                                        //字段空格间隔时OR连接查询
                                        string[] token = q[fieldName].Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        if (token.Length == 0) continue;
                                        #region 外键字段
                                        if (foreignModule != null) //外键字段
                                        {
                                            foreignModelType = CommonOperate.GetModelType(foreignModule.Id); //外键模块实体类型
                                            string innerWhere = string.Empty;
                                            foreach (string str in token)
                                            {
                                                if (!string.IsNullOrEmpty(innerWhere)) innerWhere += " OR ";
                                                innerWhere += string.Format("{0} LIKE '%{1}%'", foreignModule.TitleKey, str);
                                            }
                                            if (!string.IsNullOrEmpty(innerWhere))
                                            {
                                                string tempWhereSql = string.Empty;
                                                string temp = string.Format("{0} IN(SELECT Id FROM {1} WHERE {2} IS NOT NULL AND ({3}))", fieldName, ModelConfigHelper.GetModuleTableName(foreignModelType), foreignModule.TitleKey, innerWhere);
                                                if (grid.GridTypeOfEnum == GridTypeEnum.ComprehensiveDetail) //综合明细视图
                                                {
                                                    if (!module.ParentId.HasValue) continue;
                                                    if (module.ParentId.Value == sysField.Sys_ModuleId.Value) //明细主模块
                                                    {
                                                        tempWhereSql = string.Format("{0} IN (SELECT Id FROM {1} WHERE {2})", string.Format("{0}Id", tempType.Name),
                                                            ModelConfigHelper.GetModuleTableName(tempType), temp);
                                                    }
                                                    else //主模块的外键模块
                                                    {
                                                        Type parentType = GetModelType(module.ParentId.Value);
                                                        tempWhereSql = string.Format("{0} IN (SELECT Id FROM {1} WHERE {2} IN (SELECT Id FROM {3} WHERE {4}))",
                                                              string.Format("{0}Id", parentType.Name), ModelConfigHelper.GetModuleTableName(parentType),
                                                              string.Format("{0}Id", tempType.Name), ModelConfigHelper.GetModuleTableName(tempType), temp);
                                                    }
                                                }
                                                else //外键模块
                                                {
                                                    tempWhereSql = string.Format("{0} IN (SELECT Id FROM {1} WHERE {2})", string.Format("{0}Id", tempType.Name),
                                                            ModelConfigHelper.GetModuleTableName(tempType), temp);
                                                }
                                                if (!string.IsNullOrEmpty(tempWhereSql))
                                                {
                                                    if (!string.IsNullOrWhiteSpace(whereSql)) whereSql += " AND ";
                                                    whereSql += string.Format("({0})", tempWhereSql);
                                                }
                                            }
                                            continue;
                                        }
                                        #endregion
                                        #region 日期字段
                                        //日期时间字段
                                        if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?) || (formField != null && (formField.ControlTypeOfEnum == ControlTypeEnum.DateTimeBox || formField.ControlTypeOfEnum == ControlTypeEnum.DateBox)))
                                        {
                                            string innerWhere = string.Empty;
                                            string endWhere = string.Empty;
                                            string endFieldName = string.Format("{0}_End", fieldName); //结束日期字段
                                            if (q.Keys.Contains(endFieldName) && !string.IsNullOrWhiteSpace(q[endFieldName]))
                                            {
                                                endWhere = string.Format(" AND {0}<='{1}'", fieldName, q[endFieldName]);
                                            }
                                            if (grid.GridTypeOfEnum == GridTypeEnum.ComprehensiveDetail) //综合明细视图
                                            {
                                                if (!module.ParentId.HasValue) continue;
                                                if (module.ParentId.Value == sysField.Sys_ModuleId.Value) //明细主模块
                                                {
                                                    innerWhere += string.Format("{0} IN (SELECT Id FROM {1} WHERE {2}>='{3}'{4})", string.Format("{0}Id", tempType.Name),
                                                        ModelConfigHelper.GetModuleTableName(tempType), fieldName, q[fieldName], endWhere);
                                                }
                                                else //主模块的外键模块
                                                {
                                                    Type parentType = GetModelType(module.ParentId.Value);
                                                    innerWhere += string.Format("{0} IN (SELECT Id FROM {1} WHERE {2} IN (SELECT Id FROM {3} WHERE {4}>='{5}'{6}))",
                                                          string.Format("{0}Id", parentType.Name), ModelConfigHelper.GetModuleTableName(parentType),
                                                          string.Format("{0}Id", tempType.Name), ModelConfigHelper.GetModuleTableName(tempType), fieldName, q[fieldName], endWhere);
                                                }
                                            }
                                            else //外键模块
                                            {
                                                innerWhere += string.Format("{0} IN (SELECT Id FROM {1} WHERE {2}>='{3}'{4})", string.Format("{0}Id", tempType.Name),
                                                        ModelConfigHelper.GetModuleTableName(tempType), fieldName, q[fieldName], endWhere);
                                            }
                                            if (!string.IsNullOrEmpty(innerWhere))
                                            {
                                                if (!string.IsNullOrWhiteSpace(whereSql)) whereSql += " AND ";
                                                whereSql += string.Format("({0})", innerWhere);
                                            }
                                        }
                                        #endregion
                                        #region 其他类型字段
                                        else //非日期字段
                                        {
                                            QueryMethod queryMethod = QueryMethod.Contains;
                                            if (fieldType == typeof(Int16) || fieldType == typeof(Int32) || fieldType == typeof(Int64) ||
                                                fieldType == typeof(Int16?) || fieldType == typeof(Int32?) || fieldType == typeof(Int64?) ||
                                                fieldType == typeof(float) || fieldType == typeof(Double) || fieldType == typeof(Decimal) ||
                                                fieldType == typeof(float?) || fieldType == typeof(Double?) || fieldType == typeof(Decimal?) ||
                                                (formField != null && (formField.ControlTypeOfEnum == ControlTypeEnum.SingleCheckBox ||
                                                formField.ControlTypeOfEnum == ControlTypeEnum.NumberBox ||
                                                formField.ControlTypeOfEnum == ControlTypeEnum.IntegerBox ||
                                                formField.ControlTypeOfEnum == ControlTypeEnum.ComboBox)))
                                            {
                                                queryMethod = QueryMethod.Equal;
                                            }
                                            string innerWhere = string.Empty;
                                            string methodChar = "LIKE";
                                            if (queryMethod == QueryMethod.Equal) methodChar = "=";
                                            foreach (string str in token)
                                            {
                                                if (!string.IsNullOrEmpty(innerWhere)) innerWhere += " OR ";
                                                string v = queryMethod == QueryMethod.Equal ? string.Format("'{0}'", str) : string.Format("'%{0}%'", str);
                                                if (grid.GridTypeOfEnum == GridTypeEnum.ComprehensiveDetail) //综合明细视图
                                                {
                                                    if (!module.ParentId.HasValue) continue;
                                                    if (module.ParentId.Value == sysField.Sys_ModuleId.Value) //明细主模块
                                                    {
                                                        innerWhere += string.Format("{0} IN (SELECT Id FROM {1} WHERE ({2} {3} {4}) AND {2} IS NOT NULL)", string.Format("{0}Id", tempType.Name),
                                                            ModelConfigHelper.GetModuleTableName(tempType), fieldName, methodChar, v);
                                                    }
                                                    else //主模块的外键模块
                                                    {
                                                        Type parentType = GetModelType(module.ParentId.Value);
                                                        innerWhere += string.Format("{0} IN (SELECT Id FROM {1} WHERE {2} IN (SELECT Id FROM {3} WHERE ({4} {5} {6}) AND {4} IS NOT NULL))",
                                                              string.Format("{0}Id", parentType.Name), ModelConfigHelper.GetModuleTableName(parentType),
                                                              string.Format("{0}Id", tempType.Name), ModelConfigHelper.GetModuleTableName(tempType), fieldName, methodChar, v);
                                                    }
                                                }
                                                else //外键模块
                                                {
                                                    innerWhere += string.Format("{0} IN (SELECT Id FROM {1} WHERE ({2} {3} {4}) AND {2} IS NOT NULL)", string.Format("{0}Id", tempType.Name),
                                                            ModelConfigHelper.GetModuleTableName(tempType), fieldName, methodChar, v);
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(innerWhere))
                                            {
                                                if (!string.IsNullOrWhiteSpace(whereSql)) whereSql += " AND ";
                                                whereSql += string.Format("({0})", innerWhere);
                                            }
                                        }
                                        #endregion
                                    }
                                }
                                else //存在重写单个字段搜索接口
                                {
                                    if (tempSingleQItem != null)
                                        items.Add(tempSingleQItem);
                                    if (!string.IsNullOrWhiteSpace(tempSingleQWhere))
                                    {
                                        if (!string.IsNullOrWhiteSpace(whereSql)) whereSql += " AND ";
                                        whereSql += tempSingleQWhere;
                                    }
                                }
                            }
                        }
                        else
                        {
                            items.Add(new ConditionItem() { Field = "Id", Method = QueryMethod.Equal, Value = q["Id"] });
                        }
                    }
                    else //存在重写搜索结果接口
                    {
                        if (!string.IsNullOrWhiteSpace(tempQWhere))
                        {
                            if (!string.IsNullOrWhiteSpace(whereSql)) whereSql += " AND ";
                            whereSql += tempQWhere;
                        }
                    }
                    if (items.Count > 0)
                    {
                        Expression<Func<T, bool>> searchExp = GetQueryCondition<T>(items);
                        if (expression == null) return searchExp;
                        if (searchExp != null)
                        {
                            return ExpressionExtension.And<T>(expression, searchExp);
                        }
                    }
                    #endregion
                }
                return expression;
            }

            /// <summary>
            /// 条件表达式合并，返回合并后的表达式
            /// </summary>
            /// <param name="expression1">lamda表达式１</param>
            /// <param name="expression2">lamda表达式２</param>
            /// <param name="isAnd">是否用And合并，否则用Or</param>
            /// <returns></returns>
            public Expression<Func<T, bool>> ConditionMerge(Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2, bool isAnd)
            {
                if (expression1 == null && expression2 == null) return null;
                if (expression1 != null && expression2 == null) return expression1;
                if (expression1 == null && expression2 != null) return expression2;
                if (isAnd)
                {
                    return ExpressionExtension.And<T>(expression1, expression2);
                }
                else
                {
                    return ExpressionExtension.Or<T>(expression1, expression2);
                }
            }

            /// <summary>
            /// 根据条件集合生成where条件语句
            /// </summary>
            /// <param name="items">条件集合</param>
            /// <param name="viewId">综合视图时，视图Id</param>
            /// <returns></returns>
            public string GetWhereSqlByCondition(List<ConditionItem> items, Guid? viewId = null)
            {
                string whereSql = string.Empty;
                if (items != null && items.Count > 0)
                {
                    string tableName = typeof(T).Name;
                    Sys_Module module = SystemOperate.GetModuleByTableName(tableName);
                    for (int i = 0; i < items.Count; i++)
                    {
                        ConditionItem item = items[i];
                        string fieldName = item.Field;
                        if (SystemOperate.IsForeignNameField(module.Id, item.Field))
                        {
                            fieldName = item.Field.Substring(0, item.Field.Length - 4) + "Id";
                        }
                        Sys_Field sysField = SystemOperate.GetFieldInfo(module.Id, fieldName);
                        if (sysField == null) //非当前模块字段
                        {
                            if (!viewId.HasValue) continue;
                            //明细主模块或外键字段处理
                            Sys_GridField gridField = SystemOperate.GetGridField(viewId.Value, fieldName);
                            if (!gridField.Sys_FieldId.HasValue) continue;
                            sysField = SystemOperate.GetFieldById(gridField.Sys_FieldId.Value);
                            if (sysField == null) continue;
                        }
                        if (!sysField.Sys_ModuleId.HasValue) continue;
                        string tempWhereSql = string.Empty;
                        Type fieldType = SystemOperate.GetFieldType(sysField.Sys_ModuleId.Value, fieldName);
                        //外键字段搜索特殊处理
                        Sys_Module foreignModule = SystemOperate.GetModuleByName(sysField.ForeignModuleName);
                        #region 外键字段或字符串字段
                        if (foreignModule != null || fieldType == typeof(String)) //外键字段或字符串字段
                        {
                            string innerWhere = string.Empty;
                            if (item.Method == QueryMethod.Equal)
                            {
                                if (item.Value == null)
                                    innerWhere = " IS NULL";
                                else
                                    innerWhere = string.Format("='{0}'", item.Value.ObjToStr());
                            }
                            else if (item.Method == QueryMethod.NotEqual)
                            {
                                if (item.Value == null)
                                    innerWhere = " IS NOT NULL";
                                else
                                    innerWhere = string.Format("!='{0}'", item.Value.ObjToStr());
                            }
                            else if (item.Method == QueryMethod.Contains)
                            {
                                innerWhere = string.Format(" LIKE '%{0}%'", item.Value.ObjToStr());
                            }
                            else if (item.Method == QueryMethod.StartsWith)
                            {
                                innerWhere = string.Format(" LIKE '{0}%'", item.Value.ObjToStr());
                            }
                            else if (item.Method == QueryMethod.EndsWith)
                            {
                                innerWhere = string.Format(" LIKE '%{0}'", item.Value.ObjToStr());
                            }
                            if (foreignModule != null) //外键字段
                            {
                                if (item.Value != null && item.Value.ObjToGuid() == Guid.Empty)
                                {
                                    Type foreignModelType = CommonOperate.GetModelType(foreignModule.Id); //外键模块实体类型
                                    tempWhereSql = string.Format("{0} IN(SELECT Id FROM {1} WHERE {2}{3})", fieldName, ModelConfigHelper.GetModuleTableName(foreignModelType), foreignModule.TitleKey, innerWhere);
                                }
                                else
                                {
                                    tempWhereSql = string.Format("{0}{1}", fieldName, innerWhere);
                                }
                            }
                            else
                            {
                                tempWhereSql = string.Format("{0}{1}", fieldName, innerWhere);
                            }
                        }
                        #endregion
                        #region 枚举、字典字段
                        else if (SystemOperate.IsEnumField(module.Id, fieldName) || SystemOperate.IsDictionaryBindField(module.Id, fieldName) ||
                            fieldType == typeof(Boolean) || fieldType == typeof(Boolean?))
                        {
                            if (item.Method == QueryMethod.Equal)
                                tempWhereSql = string.Format("{0}='{1}'", fieldName, item.Value.ObjToStr());
                            else
                                tempWhereSql = string.Format("{0}!='{1}'", fieldName, item.Value.ObjToStr());
                        }
                        #endregion
                        #region 日期字段
                        else if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?))
                        {
                            string fh = ">";
                            if (item.Method == QueryMethod.LessThan)
                                fh = "<";
                            else if (item.Method == QueryMethod.LessThanOrEqual)
                                fh = "<=";
                            else if (item.Method == QueryMethod.GreaterThan)
                                fh = ">";
                            else if (item.Method == QueryMethod.GreaterThanOrEqual)
                                fh = ">=";
                            tempWhereSql = string.Format("{0}{1}'{2}'", fieldName, fh, item.Value.ObjToStr());
                        }
                        #endregion
                        #region 数值型字段
                        else if (fieldType == typeof(Int16) || fieldType == typeof(Int16?) ||
                            fieldType == typeof(Int32) || fieldType == typeof(Int32?) ||
                            fieldType == typeof(Int64) || fieldType == typeof(Int64?) ||
                            fieldType == typeof(float) || fieldType == typeof(float?) ||
                            fieldType == typeof(Double) || fieldType == typeof(Double?) ||
                            fieldType == typeof(Decimal) || fieldType == typeof(Decimal?))
                        {
                            string fh = "=";
                            if (item.Method == QueryMethod.Equal)
                                fh = "=";
                            else if (item.Method == QueryMethod.NotEqual)
                                fh = "!=";
                            else if (item.Method == QueryMethod.LessThan)
                                fh = "<";
                            else if (item.Method == QueryMethod.LessThanOrEqual)
                                fh = "<=";
                            else if (item.Method == QueryMethod.GreaterThan)
                                fh = ">";
                            else if (item.Method == QueryMethod.GreaterThanOrEqual)
                                fh = ">=";
                            tempWhereSql = string.Format("{0}{1}'{2}'", fieldName, fh, item.Value.ObjToStr());
                        }
                        #endregion
                        #region 可空字段处理
                        if (foreignModule == null && fieldType.ToString().Contains("System.Nullable`1") &&
                            (item.Method == QueryMethod.Equal || item.Method == QueryMethod.NotEqual) && item.Value.ObjToStr() == string.Empty)
                        {
                            if (fieldType == typeof(String))
                            {
                                if (item.Method == QueryMethod.Equal)
                                    tempWhereSql = string.Format("({0}='' OR {0} IS NULL)", fieldName);
                                else
                                    tempWhereSql = string.Format("({0}!='' AND {0} IS NOT NULL)", fieldName);
                            }
                            else
                            {
                                tempWhereSql = string.Format("{0} {1}", fieldName, item.Method == QueryMethod.Equal ? "IS NULL" : "IS NOT NULL");
                            }
                        }
                        #endregion
                        #region 其他模块字段搜索处理
                        if (module.Id != sysField.Sys_ModuleId.Value) //非当前模块字段
                        {
                            Type tempType = GetModelType(sysField.Sys_ModuleId.Value); //明细主模块或外键模块类型
                            Sys_Grid grid = SystemOperate.GetGrid(viewId.Value);
                            if (grid != null)
                            {
                                if (grid.GridTypeOfEnum == GridTypeEnum.ComprehensiveDetail) //综合明细视图
                                {
                                    if (module.ParentId.Value == sysField.Sys_ModuleId.Value) //当前模块是明细模块，并且当前字段是明细父模块的字段
                                    {
                                        tempWhereSql = string.Format("{0} IN (SELECT Id FROM {1} WHERE {2})", string.Format("{0}Id", tempType.Name),
                                            ModelConfigHelper.GetModuleTableName(tempType), tempWhereSql);
                                    }
                                    else //当前字段是明细父模块的外键字段
                                    {
                                        Type parentType = GetModelType(module.ParentId.Value);
                                        tempWhereSql = string.Format("{0} IN (SELECT Id FROM {1} WHERE {2} IN (SELECT Id FROM {3} WHERE {4}))",
                                              string.Format("{0}Id", parentType.Name), ModelConfigHelper.GetModuleTableName(parentType),
                                              string.Format("{0}Id", tempType.Name), ModelConfigHelper.GetModuleTableName(tempType), tempWhereSql);
                                    }
                                }
                                else //综合视图
                                {
                                    tempWhereSql = string.Format("{0} IN (SELECT Id FROM {1} WHERE {2})", string.Format("{0}Id", tempType.Name),
                                            ModelConfigHelper.GetModuleTableName(tempType), tempWhereSql);
                                }
                            }
                        }
                        #endregion
                        if (i > 0)
                        {
                            if (items[i - 1].OrGroup != null && items[i - 1].OrGroup.ToLower() == "or")
                                whereSql += " OR ";
                            else
                                whereSql += " AND ";
                        }
                        whereSql += tempWhereSql;
                    }
                }
                return whereSql;
            }

            /// <summary>
            /// 将条件表达式转成where Sql语句
            /// </summary>
            /// <param name="expression">条件表达式</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public string ExpressionConditionToWhereSql(Expression<Func<T, bool>> expression, DatabaseType? dbType = null)
            {
                if (expression == null) return string.Empty;
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.ExpressionConditionToWhereSql(expression);
            }

            #endregion

            #region 其他

            /// <summary>
            /// 取表单数据
            /// </summary>
            /// <param name="id">记录Id</param>
            /// <param name="formType">表单类型</param>
            /// <param name="errMsg">异常信息</param>
            /// <returns></returns>
            public T GetFormData(Guid id, FormTypeEnum formType, out string errMsg)
            {
                T t = GetEntityById<T>(id, out errMsg);
                //表单数据处理
                new OperateHandleFactory<T>().FormDataHandle(t, formType);
                return t;
            }

            /// <summary>
            /// 清除当前模块缓存
            /// </summary>
            public void ClearCache()
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>();
                bll.ClearCache();
            }

            #endregion

            #region 数据库操作

            /// <summary>
            /// 创建数据表
            /// </summary>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns>创建成功返回空字符串，失败返回异常信息</returns>
            public string CreateTable(string connString = null, DatabaseType? dbType = null)
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.CreateTable(connString);
            }

            /// <summary>
            /// 删除数据表
            /// </summary>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns>删除成功返回空字符串，失败返回异常信息</returns>
            public string DropTable(string connString = null, DatabaseType? dbType = null)
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.DropTable(connString);
            }

            /// <summary>
            /// 修改数据表
            /// </summary>
            /// <param name="command">操作sql，如：[ALTER TABLE a] ADD Column b int</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns>成功返回空字符串，失败返回异常信息</returns>
            public string AlterTable(string command, string connString = null, DatabaseType? dbType = null)
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.AlterTable(command, connString);
            }

            /// <summary>
            /// 数据列是否存在
            /// </summary>
            /// <param name="fieldName">字段名称</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public bool ColumnIsExists(string fieldName, string connString = null, DatabaseType? dbType = null)
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.ColumnIsExists(fieldName, connString);
            }

            /// <summary>
            /// 增加数据列
            /// </summary>
            /// <param name="fieldName">字段名称</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns>成功返回空字符串，失败返回异常信息</returns>
            public string AddColumn(string fieldName, string connString = null, DatabaseType? dbType = null)
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.AddColumn(fieldName, connString);
            }

            /// <summary>
            /// 修改数据列
            /// </summary>
            /// <param name="fieldName">字段名称</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns>成功返回空字符串，失败返回异常信息</returns>
            public string AlterColumn(string fieldName, string connString = null, DatabaseType? dbType = null)
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.AlterColumn(fieldName, connString);
            }

            /// <summary>
            /// 修改列名
            /// </summary>
            /// <param name="fieldName">字段名称</param>
            /// <param name="oldFieldName">旧的字段名</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns>成功返回空字符串，失败返回异常信息</returns>
            public string ChangeColumnName(string fieldName, string oldFieldName, string connString = null, DatabaseType? dbType = null)
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.ChangeColumnName(fieldName, oldFieldName, connString);
            }

            /// <summary>
            /// 删除列
            /// </summary>
            /// <param name="columnName">列名</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public string DropColumn(string columnName, string connString = null, DatabaseType? dbType = null)
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.DropColumn(columnName, connString);
            }

            /// <summary>
            /// 创建索引
            /// </summary>
            /// <param name="field">字段</param>
            /// <param name="indexName">索引名</param>
            /// <param name="unique">是否唯一索引</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns>成功返回空字符串，失败返回异常信息</returns>
            public string CreateIndex(Expression<Func<T, object>> field, string indexName = null, bool unique = false, string connString = null, DatabaseType? dbType = null)
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.CreateTable(connString);
            }

            /// <summary>
            /// 删除索引
            /// </summary>
            /// <param name="indexName">索引名称</param>
            /// <param name="connString">数据库连接字符串</param>
            /// <param name="dbType">数据库类型</param>
            /// <returns></returns>
            public string DropIndex(string indexName, string connString = null, DatabaseType? dbType = null)
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.CreateTable(connString);
            }

            #endregion
        }

        #endregion

        #region 反射处理

        /// <summary>
        /// 反射执行业务层方法
        /// </summary>
        /// <param name="modelType">数据模型类型</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="args">参数</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static object ReflectExecuteBLLMethod(Type modelType, string methodName, object[] args, DatabaseType? dbType = null)
        {
            object bll = null;
            //调用业务层方法
            //先调用自定义业务层接口
            Type interfacBllType = BridgeObject.GetBLLInterfaceType(modelType);
            //没有自定义业务接口时调用通用业务接口
            if (interfacBllType == null)
            {
                Type bllTypeInit = typeof(IBaseBLL<>);
                interfacBllType = bllTypeInit.MakeGenericType(new Type[] { modelType });
            }
            //实例化业务接口对象
            bll = BridgeObject.Resolve(interfacBllType, dbType);
            //获取业务层获取模型对象方法
            MethodInfo method = bll.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            //反射执行方法
            object executedObj = method.Invoke(bll, args);
            return executedObj;
        }

        /// <summary>
        /// 反射执行TempOperate的方法
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        /// <returns>返回执行结果</returns>
        private static object ExecuteTempOperateReflectMethod(string tableName, string methodName, object[] args)
        {
            //根据tableName获取模块类型
            Type modelType = BridgeObject.GetModelType(tableName);
            Type tempType = typeof(TempOperate<>);
            Type relectType = tempType.MakeGenericType(new Type[] { modelType });
            //实例化对象
            object obj = Activator.CreateInstance(relectType);
            MethodInfo method = relectType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            //反射执行方法
            object executedObj = method.Invoke(obj, args);
            return executedObj;
        }

        /// <summary>
        /// 反射执行TempOperate的方法
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        /// <returns>返回执行结果</returns>
        private static object ExecuteTempOperateReflectMethod(Guid moduleId, string methodName, object[] args)
        {
            //根据moduleId获取模块类型
            Type modelType = SystemOperate.GetModelType(moduleId);
            Type tempType = typeof(TempOperate<>);
            Type relectType = tempType.MakeGenericType(new Type[] { modelType });
            //实例化对象
            object obj = Activator.CreateInstance(relectType);
            MethodInfo method = relectType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            //反射执行方法
            object executedObj = method.Invoke(obj, args);
            return executedObj;
        }

        /// <summary>
        /// 反射执行TempOperate的方法
        /// </summary>
        /// <param name="modelType">实体类型对象</param>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        /// <returns>返回执行结果</returns>
        private static object ExecuteTempOperateReflectMethod(Type modelType, string methodName, object[] args)
        {
            Type tempType = typeof(TempOperate<>);
            Type relectType = tempType.MakeGenericType(new Type[] { modelType });
            //实例化对象
            object obj = Activator.CreateInstance(relectType);
            MethodInfo method = relectType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
            //反射执行方法
            object executedObj = method.Invoke(obj, args);
            return executedObj;
        }

        /// <summary>
        /// 执行OperateHandleFactory类中自定义操作方法
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="methodName">方法名称</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object ExecuteCustomeOperateHandleMethod(Guid moduleId, string methodName, object[] args)
        {
            try
            {
                Type tempType = typeof(OperateHandleFactory<>);
                Type modelType = GetModelType(moduleId);
                Type relectType = tempType.MakeGenericType(new Type[] { modelType });
                //实例化对象
                object obj = Activator.CreateInstance(relectType);
                MethodInfo method = relectType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (method == null) return null;
                //反射执行方法
                object executedObj = method.Invoke(obj, args);
                return executedObj;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 反射执行用户操作处理方法
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <param name="args">参数</param>
        /// <returns></returns>
        public static object ExecuteUserOperateHandleMethod(string methodName, object[] args)
        {
            try
            {
                Type tempType = typeof(UserOperateHandleFactory);
                //实例化对象
                object obj = Activator.CreateInstance(tempType);
                MethodInfo method = tempType.GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
                if (method == null) return null;
                //反射执行方法
                object executedObj = method.Invoke(obj, args);
                return executedObj;
            }
            catch { }
            return null;
        }

        /// <summary>
        /// 获取枚举字段的描述、枚举值键值对，Dictionary中key为描述，value为枚举值
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetFieldEnumTypeList(Guid moduleId, string fieldName)
        {
            //根据moduleId获取模块类名
            Dictionary<string, string> dic = new Dictionary<string, string>();
            //获取数据模型类型
            Type modelType = SystemOperate.GetModelType(moduleId);
            if (modelType == null) return dic;
            //获取所有属性字段
            PropertyInfo[] propertys = modelType.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (propertys != null && propertys.Length > 0)
            {
                PropertyInfo field = propertys.Where(x => x.Name == string.Format("{0}OfEnum", fieldName)).FirstOrDefault();
                if (field == null) return null;
                return EnumHelper.GetEnumDescValue(field.PropertyType);
            }
            return dic;
        }

        /// <summary>
        /// 获取某模块字段值
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="model">模块数据</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static object GetModelFieldValueByModel(Guid moduleId, object model, string fieldName)
        {
            if (model == null || string.IsNullOrEmpty(fieldName)) return null;
            Type modelType = SystemOperate.GetModelType(moduleId);
            PropertyInfo pFieldName = modelType.GetProperty(fieldName);
            if (pFieldName != null)
            {
                return pFieldName.GetValue(model, null);
            }
            return null;
        }

        /// <summary>
        /// 获取记录的TitleKey值
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="id">记录Id</param>
        /// <returns></returns>
        public static string GetModelTitleKeyValue(Guid moduleId, Guid id)
        {
            string errMsg = string.Empty;
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module == null) return string.Empty;
            object model = CommonOperate.GetEntityById(moduleId, id, out errMsg);
            string titleKeyValue = string.Empty; //记录对应的titleKey值
            if (!string.IsNullOrEmpty(module.TitleKey))
            {
                titleKeyValue = CommonOperate.GetModelFieldValueByModel(moduleId, model, module.TitleKey).ObjToStr();
            }
            return titleKeyValue;
        }

        /// <summary>
        /// 获取实体类型
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static Type GetModelType(Guid moduleId)
        {
            string tableName = SystemOperate.GetModuleTableNameById(moduleId);
            return BridgeObject.GetModelType(tableName);
        }

        /// <summary>
        /// 获取某个实体的类型
        /// </summary>
        /// <param name="tableName">实体表名</param>
        /// <returns></returns>
        public static Type GetModelType(string tableName)
        {
            return BridgeObject.GetModelType(tableName);
        }

        /// <summary>
        /// 获取实体类型
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Type GetModelTypeByModuleName(string moduleName)
        {
            return BridgeObject.GetModelTypeByModuleName(moduleName);
        }

        /// <summary>
        /// 获取FluentValidation验证类型
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static Type GetFluentValidationModelType(Guid moduleId)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null)
                return BridgeObject.GetFluentValidationModelType(module.TableName);
            return null;
        }

        #endregion

        #region 查询记录

        /// <summary>
        /// 获取实体记录
        /// </summary>
        /// <param name="id">记录Id</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static T GetEntityById<T>(Guid id, out string errMsg, bool references = false, string connString = null, DatabaseType? dbType = null) where T : class
        {
            return new TempOperate<T>().GetEntityById(id, out errMsg, references, connString, dbType);
        }

        /// <summary>
        /// 根据Id获取实体
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="id">记录Id</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static object GetEntityById(string tableName, Guid id, out string errMsg, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { id, errMsg, references, connString, dbType };
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "GetEntityById", args);
            errMsg = args[1].ObjToStr();
            return obj;
        }

        /// <summary>
        /// 获取实体记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static T GetEntity<T>(Expression<Func<T, bool>> expression, string whereSql, out string errMsg, bool references = false, string connString = null, DatabaseType? dbType = null) where T : class
        {
            return new TempOperate<T>().GetEntity(expression, whereSql, out errMsg, references, connString, dbType);
        }

        /// <summary>
        /// 获取实体对象
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">条件语句</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static object GetEntity(string tableName, object expression, string whereSql, out string errMsg, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { expression, whereSql, errMsg, references, connString, dbType };
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "GetEntity", args);
            errMsg = args[2].ObjToStr();
            return obj;
        }

        /// <summary>
        /// 获取实体记录集合 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="errMsg">异常信息</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="orderFields">排序字段</param>
        /// <param name="isDescs">排序方式,是否降序排序</param>
        /// <param name="top">取top多少条</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static List<T> GetEntities<T>(out string errMsg, Expression<Func<T, bool>> expression = null, string whereSql = null, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, int? top = null, bool references = false, string connString = null, DatabaseType? dbType = null) where T : class
        {
            return new TempOperate<T>().GetEntities(out errMsg, expression, whereSql, permissionFilter, orderFields, isDescs, top, references, connString, dbType);
        }

        /// <summary>
        /// 获取实体记录集合 
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="orderFields">排序字段</param>
        /// <param name="isDescs">排序方式,是否降序排序</param>
        /// <param name="top">取top多少条</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static object GetEntities(string tableName, out string errMsg, object expression = null, string whereSql = null, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, int? top = null, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { errMsg, expression, whereSql, permissionFilter, orderFields, isDescs, top, references, connString, dbType };
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "GetEntities", args);
            errMsg = args[0].ObjToStr();
            return obj;
        }

        /// <summary>
        /// 获取分页实体记录集合
        /// </summary>
        /// <param name="errMsg">异常信息</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回实体集合</returns>
        public static List<T> GetPageEntities<T>(out string errMsg, PageInfo pageInfo, bool permissionFilter = true, Expression<Func<T, bool>> expression = null, string whereSql = null, bool references = false, string connString = null, DatabaseType? dbType = null) where T : class
        {
            return new TempOperate<T>().GetPageEntities(out errMsg, pageInfo, permissionFilter, expression, whereSql, references, connString, dbType);
        }

        /// <summary>
        /// 获取分页实体记录集合
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回实体集合</returns>
        public static object GetPageEntities(string tableName, out string errMsg, PageInfo pageInfo, bool permissionFilter = true, object expression = null, string whereSql = null, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { errMsg, pageInfo, permissionFilter, expression, whereSql, references, connString, dbType };
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "GetPageEntities", args);
            errMsg = args[0].ObjToStr();
            return obj;
        }

        /// <summary>
        /// 根据字段获取记录
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="orderFields">排序字段</param>
        /// <param name="isDescs">是否降序</param>
        /// <param name="top">取前多少条记录</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static List<T> GetEntitiesByField<T>(string fieldName, object fieldValue, out string errMsg, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, int? top = null, bool references = false, string connString = null, DatabaseType? dbType = null) where T : class
        {
            return new TempOperate<T>().GetEntitiesByField(fieldName, fieldValue, out errMsg, permissionFilter, orderFields, isDescs, top, references, connString, dbType);
        }

        /// <summary>
        /// 根据字段值获取实体记录
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="orderFields">排序字段</param>
        /// <param name="isDescs">是否降序</param>
        /// <param name="top">取前多少条记录</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static object GetEntitiesByField(string tableName, string fieldName, object fieldValue, out string errMsg, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, int? top = null, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { fieldName, fieldValue, errMsg, permissionFilter, orderFields, isDescs, top, references, connString, dbType };
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "GetEntitiesByField", args);
            errMsg = args[2].ObjToStr();
            return obj;
        }

        /// <summary>
        /// 根据字段获取单条记录
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="orderFields">排序字段</param>
        /// <param name="isDescs">是否降序</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static T GetEntitiesByFieldOne<T>(string fieldName, object fieldValue, out string errMsg, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, bool references = false, string connString = null, DatabaseType? dbType = null) where T : class
        {
            return new TempOperate<T>().GetEntitiesByFieldOne(fieldName, fieldValue, out errMsg, permissionFilter, orderFields, isDescs, references, connString, dbType);
        }

        /// <summary>
        /// 根据字段获取单条记录
        /// </summary>
        /// <param name="tableName">实体表名</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="orderFields">排序字段</param>
        /// <param name="isDescs">是否降序</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static object GetEntitiesByFieldOne(string tableName, string fieldName, object fieldValue, out string errMsg, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { fieldName, fieldValue, errMsg, permissionFilter, orderFields, isDescs, references, connString, dbType };
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "GetEntitiesByFieldOne", args);
            errMsg = args[2].ObjToStr();
            return obj;
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <typeparam name="T">实体类</typeparam>
        /// <param name="errMsg">错误信息</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static long Count<T>(out string errMsg, bool permissionFilter = true, Expression<Func<T, bool>> expression = null, string whereSql = null, string connString = null, DatabaseType? dbType = null) where T : class
        {
            return new TempOperate<T>().Count(out errMsg, permissionFilter, expression, whereSql, connString, dbType);
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="errMsg">错误信息</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static long Count(string tableName, out string errMsg, bool permissionFilter = true, object expression = null, string whereSql = null, string connString = null, DatabaseType? dbType = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { errMsg, permissionFilter, expression, whereSql, connString, dbType };
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "Count", args);
            errMsg = args[0].ObjToStr();
            return obj.ObjToLong();
        }

        /// <summary>
        /// 加载实体对象的关联对象（导航属性）
        /// </summary>
        /// <param name="instance">实体对象</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public static void LoadReferences<T>(T instance, out string errMsg, string connString = null, DatabaseType? dbType = null) where T : class
        {
            new TempOperate<T>().LoadReferences(instance, out errMsg, connString, dbType);
        }

        /// <summary>
        /// 加载实体对象的关联对象（导航属性）
        /// </summary>
        /// <param name="tableName">实体表名</param>
        /// <param name="instance">实体对象</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public static void LoadReferences(string tableName, object instance, out string errMsg, string connString = null, DatabaseType? dbType = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { instance, errMsg, connString, dbType };
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "LoadReferences", args);
            errMsg = args[1].ObjToStr();
        }

        /// <summary>
        /// 加载实体对象的关联对象（导航属性）
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="instance">实体对象</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public static void LoadReferences(Guid moduleId, object instance, out string errMsg, string connString = null, DatabaseType? dbType = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                LoadReferences(module.TableName, instance, out errMsg, connString, dbType);
            }
            errMsg = "模块不存在或模块表名为空！";
        }

        /// <summary>
        /// 加载关联对象（导航属性）
        /// </summary>
        /// <param name="instances">实体对象集合</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public static void LoadListReferences<T>(List<T> instances, out string errMsg, string connString = null, DatabaseType? dbType = null) where T : class
        {
            new TempOperate<T>().LoadListReferences(instances, out errMsg, connString, dbType);
        }

        /// <summary>
        /// 加载关联对象（导航属性）
        /// </summary>
        /// <param name="tableName">实体表名</param>
        /// <param name="instances">实体对象集合</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public static void LoadListReferences(string tableName, object instances, out string errMsg, string connString = null, DatabaseType? dbType = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { instances, errMsg, connString, dbType };
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "LoadListReferences", args);
            errMsg = args[1].ObjToStr();
        }

        /// <summary>
        /// 加载关联对象（导航属性）
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="instances">实体对象集合</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public static void LoadListReferences(Guid moduleId, object instances, out string errMsg, string connString = null, DatabaseType? dbType = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                LoadReferences(module.TableName, instances, out errMsg, connString, dbType);
            }
            errMsg = "模块不存在或模块表名为空！";
        }

        /// <summary>
        /// 获取模块记录
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="id">记录Id</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="references">是否加载关联对象（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回实体</returns>
        public static object GetEntityById(Guid moduleId, Guid id, out string errMsg, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return GetEntityById(module.TableName, id, out errMsg, references, connString, dbType);
            }
            errMsg = "模块不存在或模块表名为空！";
            return null;
        }

        /// <summary>
        /// 获取实体记录
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="references">是否加载关联对象（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回实体</returns>
        public static object GetEntity(Guid moduleId, object expression, string whereSql, out string errMsg, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return GetEntity(module.TableName, expression, whereSql, out errMsg, references, connString, dbType);
            }
            errMsg = "模块不存在或模块表名为空！";
            return null;
        }

        /// <summary>
        /// 获取模块记录集合
        /// </summary>
        /// <param name="errMsg">异常信息</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="orderFields">排序字段名</param>
        /// <param name="isDescs">是否降序排序</param>
        /// <param name="top">取前多少条数据</param>
        /// <param name="references">是否加载关联对象（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回实体集合</returns>
        public static object GetEntities(out string errMsg, Guid moduleId, object expression, string whereSql = null, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, int? top = null, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return GetEntities(module.TableName, out errMsg, expression, whereSql, permissionFilter, orderFields, isDescs, top, references, connString, dbType);
            }
            errMsg = "模块不存在或模块表名为空！";
            return null;
        }

        /// <summary>
        /// 根据字段获取实体记录
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="orderFields">排序字段</param>
        /// <param name="isDescs">是否降序</param>
        /// <param name="top">取前多少条记录</param>
        /// <param name="references">是否加载关联对象（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static object GetEntitiesByField(Guid moduleId, string fieldName, object fieldValue, out string errMsg, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, int? top = null, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return GetEntitiesByField(module.TableName, fieldName, fieldValue, out errMsg, permissionFilter, orderFields, isDescs, top, references, connString, dbType);
            }
            errMsg = "模块不存在或模块表名为空！";
            return null;
        }

        /// <summary>
        /// 根据字段获取单条记录
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="orderFields">排序字段</param>
        /// <param name="isDescs">是否降序</param>
        /// <param name="references">是否加载关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static object GetEntitiesByFieldOne(Guid moduleId, string fieldName, object fieldValue, out string errMsg, bool permissionFilter = true, List<string> orderFields = null, List<bool> isDescs = null, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return GetEntitiesByFieldOne(module.TableName, fieldName, fieldValue, out errMsg, permissionFilter, orderFields, isDescs, references, connString, dbType);
            }
            errMsg = "模块不存在或模块表名为空！";
            return null;
        }

        /// <summary>
        /// 获取分页记录
        /// </summary>
        /// <param name="errMsg">异常信息</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="pageInfo">分页信息</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="references">是否加载关联对象（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回实体集合</returns>
        public static object GetPageEntities(out string errMsg, Guid moduleId, PageInfo pageInfo, bool permissionFilter = true, object expression = null, string whereSql = null, bool references = false, string connString = null, DatabaseType? dbType = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return GetPageEntities(module.TableName, out errMsg, pageInfo, permissionFilter, expression, whereSql, references, connString, dbType);
            }
            errMsg = "模块不存在或模块表名为空！";
            return null;
        }

        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="permissionFilter">是否进行权限过滤</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static long Count(out string errMsg, Guid moduleId, bool permissionFilter = true, object expression = null, string whereSql = null, string connString = null, DatabaseType? dbType = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return Count(module.TableName, out errMsg, permissionFilter, expression, whereSql, connString, dbType);
            }
            errMsg = "模块不存在或模块表名为空！";
            return 0;
        }

        /// <summary>
        /// 获取实体某个字段值
        /// </summary>
        /// <param name="field">字段</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static object Scalar<T>(Expression<Func<T, object>> field, Expression<Func<T, bool>> expression, out string errMsg, string connString = null, DatabaseType? dbType = null) where T : class
        {
            errMsg = string.Empty;
            MethodCallTypeEnum methodCallType = BridgeObject.GetBLLMethodType(typeof(T), "Scalar");
            if (methodCallType != MethodCallTypeEnum.CommonMethod)
            {
                object[] args = new object[] { field, expression, errMsg, connString };
                object obj = ReflectExecuteBLLMethod(typeof(T), "Scalar", args, dbType);
                errMsg = args[2].ObjToStr();
                return obj;
            }
            else
            {
                IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
                return bll.Scalar(field, expression, out errMsg, connString);
            }
        }

        /// <summary>
        /// 获取不重复的列字段值
        /// </summary>
        /// <param name="errMsg">异常信息</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">列字段名</param>
        /// <returns></returns>
        public static Dictionary<string, string> GetColumnFieldValues(out string errMsg, Guid moduleId, string fieldName)
        {
            errMsg = string.Empty;
            try
            {
                Type modelType = GetModelType(moduleId);
                string tableName = ModelConfigHelper.GetModuleTableName(modelType);
                string sql = string.Empty;
                Sys_Module foreignModule = SystemOperate.GetForeignModule(moduleId, fieldName);
                if (foreignModule != null && !string.IsNullOrWhiteSpace(foreignModule.TitleKey)) //外键字段
                {
                    Type foreignModelType = CommonOperate.GetModelType(foreignModule.Id); //外键模块实体类型
                    sql = string.Format("SELECT DISTINCT Id,{0} FROM {1} WHERE Id IN (SELECT DISTINCT {2} FROM {3} WHERE IsDeleted=0)", foreignModule.TitleKey, ModelConfigHelper.GetModuleTableName(foreignModelType), fieldName, tableName);
                }
                else if (SystemOperate.IsDictionaryBindField(moduleId, fieldName)) //字典绑定字段
                {
                    string className = SystemOperate.GetBindDictonaryClass(moduleId, fieldName); //绑定的字典分类
                    sql = string.Format("SELECT DISTINCT Value,Name FROM Sys_Dictionary WHERE IsDeleted=0 AND ClassName='{0}' AND Value IN (SELECT DISTINCT {1} FROM {2} WHERE IsDeleted=0)", className, fieldName, tableName);
                }
                else
                {
                    sql = string.Format("SELECT DISTINCT {0} as Id,{0} as Name FROM {1} WHERE IsDeleted=0", fieldName, tableName);
                }
                DataTable dt = ExecuteQuery(out errMsg, sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = string.Empty;
                    dr[1] = "请选择";
                    dt.Rows.InsertAt(dr, 0);
                    if (SystemOperate.IsEnumField(moduleId, fieldName)) //枚举字段
                    {
                        return dt.AsEnumerable().Cast<DataRow>().ToDictionary(x => x[0].ObjToStr(), y => SystemOperate.GetEnumFieldDisplayText(moduleId, fieldName, y[0].ObjToStr()));
                    }
                    else
                    {
                        return dt.AsEnumerable().Cast<DataRow>().ToDictionary(x => x[0].ObjToStr(), y => y[1].ObjToStr());
                    }
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return new Dictionary<string, string>();
        }

        #endregion

        #region 增删改

        /// <summary>
        /// 操作单条实体记录
        /// </summary>
        /// <param name="t">实体对象</param>
        /// <param name="operateType">操作类型</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="fieldNames">更新时用到，要更新的字段</param>
        /// <param name="permissionValidate">是否进行权限验证，针对编辑删除</param>
        /// <param name="references">是否保存关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns>如果是新增返回新增后记录Id,否则成功返回1，失败返回0</returns>
        public static Guid OperateRecord<T>(T t, ModelRecordOperateType operateType, out string errMsg, List<string> fieldNames = null, bool permissionValidate = true, bool references = false, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null) where T : class
        {
            return new TempOperate<T>().OperateRecord(t, operateType, out errMsg, fieldNames, permissionValidate, references, connString, dbType, transConn);
        }

        /// <summary>
        /// 操作单条实体记录
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="t">实体对象</param>
        /// <param name="operateType">操作类型</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="fieldNames">更新时用到，要更新的字段</param>
        /// <param name="permissionValidate">是否进行权限验证，针对编辑删除</param>
        /// <param name="references">是否保存关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns>如果是新增返回新增后记录Id,否则成功返回1，失败返回0</returns>
        public static Guid OperateRecord(string tableName, object t, ModelRecordOperateType operateType, out string errMsg, List<string> fieldNames = null, bool permissionValidate = true, bool references = false, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { t, operateType, errMsg, fieldNames, permissionValidate, references, connString, dbType, transConn };
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "OperateRecord", args);
            errMsg = args[2].ObjToStr();
            return obj.ObjToGuid();
        }

        /// <summary>
        /// 操作实体记录集合
        /// </summary>
        /// <param name="ts">记录集合</param>
        /// <param name="operateType">操作类型</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="permissionValidate">是否进行权限验证，针对编辑删除</param>
        /// <param name="references">是否保存关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool OperateRecords<T>(List<T> ts, ModelRecordOperateType operateType, out string errMsg, bool permissionValidate = true, bool references = false, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null) where T : class
        {
            return new TempOperate<T>().OperateRecords(ts, operateType, out errMsg, permissionValidate, references, connString, dbType, transConn);
        }

        /// <summary>
        /// 操作实体记录集合
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="ts">记录集合</param>
        /// <param name="operateType">操作类型</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="permissionValidate">是否进行权限验证，针对编辑删除</param>
        /// <param name="references">是否保存关联对象数据（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool OperateRecords(string tableName, object ts, ModelRecordOperateType operateType, out string errMsg, bool permissionValidate = true, bool references = false, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { ts, operateType, errMsg, permissionValidate, references, connString, dbType, transConn };
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "OperateRecords", args);
            errMsg = args[2].ObjToStr();
            return obj.ObjToBool();
        }

        /// <summary>
        /// 根据条件表达式更新记录
        ///  UpdateEntityByExpression(new { FirstName = "JJ" }, p => p.LastName == "Hendrix");
        ///  UPDATE "Person" SET "FirstName" = 'JJ' WHERE ("LastName" = 'Hendrix')
        /// </summary>
        /// <param name="obj">要更新的值对象</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool UpdateRecordsByExpression<T>(object obj, Expression<Func<T, bool>> expression, out string errMsg, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null) where T : class
        {
            return new TempOperate<T>().UpdateRecordsByExpression(obj, expression, out errMsg, connString, dbType, transConn);
        }

        /// <summary>
        /// 根据条件表达式更新记录
        ///  UpdateEntityByExpression(new { FirstName = "JJ" }, p => p.LastName == "Hendrix");
        ///  UPDATE "Person" SET "FirstName" = 'JJ' WHERE ("LastName" = 'Hendrix')
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="updateValueObj">要更新的值对象</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool UpdateRecordsByExpression(string tableName, object updateValueObj, object expression, out string errMsg, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { updateValueObj, expression, errMsg, connString, dbType, transConn };
            //反射取数据
            object tempObj = ExecuteTempOperateReflectMethod(tableName, "UpdateRecordsByExpression", args);
            errMsg = args[2].ObjToStr();
            return tempObj.ObjToBool();
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="ids">id集合</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="isSoftDel">是否软删除</param>
        /// <param name="permissionValidate">是否进行权限验证，针对硬删除</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool DeleteRecords<T>(IEnumerable ids, out string errMsg, bool isSoftDel = false, bool permissionValidate = true, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null) where T : class
        {
            return new TempOperate<T>().DeleteRecords(ids, out errMsg, isSoftDel, permissionValidate, connString, dbType, transConn);
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="ids">id集合</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="isSoftDel">是否软删除</param>
        /// <param name="permissionValidate">是否进行权限验证，针对硬删除</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool DeleteRecords(string tableName, IEnumerable ids, out string errMsg, bool isSoftDel = false, bool permissionValidate = true, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { ids, errMsg, isSoftDel, permissionValidate, connString, dbType, transConn };
            //反射取数据
            object tempObj = ExecuteTempOperateReflectMethod(tableName, "DeleteRecords", args);
            errMsg = args[1].ObjToStr();
            return tempObj.ObjToBool();
        }

        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="isSoftDel">是否软删除</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool DeleteRecordsByExpression<T>(Expression<Func<T, bool>> expression, out string errMsg, bool isSoftDel = false, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null) where T : class
        {
            return new TempOperate<T>().DeleteRecordsByExpression(expression, out errMsg, isSoftDel, connString, dbType, transConn);
        }

        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="isSoftDel">是否软删除</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool DeleteRecordsByExpression(string tableName, object expression, out string errMsg, bool isSoftDel = false, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { expression, errMsg, isSoftDel, connString, dbType, transConn };
            //反射取数据
            object tempObj = ExecuteTempOperateReflectMethod(tableName, "DeleteRecordsByExpression", args);
            errMsg = args[1].ObjToStr();
            return tempObj.ObjToBool();
        }

        /// <summary>
        /// 操作单条记录
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="t">操作对象</param>
        /// <param name="operateType">操作类型</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="fieldNames">更新时用到，要更新的字段</param>
        /// <param name="permissionValidate">是否进行权限验证，针对编辑删除</param>
        /// <param name="references">是否加载关联对象（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns>如果是新增返回新增后记录Id,否则成功返回1，失败返回0</returns>
        public static Guid OperateRecord(Guid moduleId, object t, ModelRecordOperateType operateType, out string errMsg, List<string> fieldNames = null, bool permissionValidate = true, bool references = false, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return OperateRecord(module.TableName, t, operateType, out errMsg, fieldNames, permissionValidate, references, connString, dbType, transConn);
            }
            errMsg = "模块不存在或模块表名为空！";
            return Guid.Empty;
        }

        /// <summary>
        /// 操作记录集合
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="ts">对象集合</param>
        /// <param name="operateType">操作类型</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="permissionValidate">是否进行权限验证，针对编辑删除</param>
        /// <param name="references">是否加载关联对象（导航属性）</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns>返回结果</returns>
        public static bool OperateRecords(Guid moduleId, object ts, ModelRecordOperateType operateType, out string errMsg, bool permissionValidate = true, bool references = false, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return OperateRecords(module.TableName, ts, operateType, out errMsg, permissionValidate, references, connString, dbType, transConn);
            }
            errMsg = "模块不存在或模块表名为空！";
            return false;
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="ids">id集合</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="isSoftDel">是否软删除</param>
        /// <param name="permissionValidate">是否进行权限验证，针对硬删除</param>
        /// <param name="connString">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool DeleteRecords(Guid moduleId, IEnumerable ids, out string errMsg, bool isSoftDel = false, bool permissionValidate = true, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return DeleteRecords(module.TableName, ids, out errMsg, isSoftDel, permissionValidate, connString, dbType, transConn);
            }
            errMsg = "模块不存在或模块表名为空！";
            return false;
        }

        /// <summary>
        /// 根据条件表达式更新记录
        ///  UpdateEntityByExpression(new { FirstName = "JJ" }, p => p.LastName == "Hendrix");
        ///  UPDATE "Person" SET "FirstName" = 'JJ' WHERE ("LastName" = 'Hendrix')
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="obj">要更新的值对象</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool UpdateRecordsByExpression(Guid moduleId, object obj, object expression, out string errMsg, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return UpdateRecordsByExpression(module.TableName, obj, expression, out errMsg, connString, dbType, transConn);
            }
            errMsg = "模块不存在或模块表名为空！";
            return false;
        }

        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="isSoftDel">是否软删除</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool DeleteRecordsByExpression(Guid moduleId, object expression, out string errMsg, bool isSoftDel = true, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return DeleteRecordsByExpression(module.TableName, expression, out errMsg, isSoftDel, connString, dbType, transConn);
            }
            errMsg = "模块不存在或模块表名为空！";
            return false;
        }

        /// <summary>
        /// 更新字段值
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="recordId">记录Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns></returns>
        public static bool UpdateField(Guid moduleId, Guid recordId, string fieldName, string fieldValue, out string errMsg, IDbConnection transConn = null)
        {
            errMsg = string.Empty;
            Type modelType = GetModelType(moduleId);
            if (modelType == null)
            {
                errMsg = "模块不存在";
                return false;
            }
            PropertyInfo p = modelType.GetProperty(fieldName);
            if (p == null)
            {
                errMsg = "字段不存在";
                return false;
            }
            try
            {
                object model = Activator.CreateInstance(modelType);
                PropertyInfo idProperty = modelType.GetProperty("Id");
                idProperty.SetValue(model, recordId, null);
                p.SetValue(model, TypeUtil.ChangeType(fieldValue, p.PropertyType), null);
                Guid rs = CommonOperate.OperateRecord(moduleId, model, ModelRecordOperateType.Edit, out errMsg, new List<string>() { fieldName }, false, false, null, null, transConn);
                return rs != Guid.Empty;
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return false;
        }

        #endregion

        #region 事务操作

        /// <summary>
        /// 执行事务，针对T类型的对象的操作
        /// </summary>
        /// <param name="transactionObjects">事务对象集合</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public static void ExecuteTransaction<T>(List<TransactionModel<T>> transactionObjects, out string errMsg, string connString = null, DatabaseType? dbType = null) where T : class
        {
            new TempOperate<T>().ExecuteTransaction(transactionObjects, out errMsg, connString, dbType);
        }

        /// <summary>
        /// 执行事务，针对tableName表的操作
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="transactionObjects">事务对象集合</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public static void ExecuteTransaction(string tableName, object transactionObjects, out string errMsg, string connString = null, DatabaseType? dbType = null)
        {
            errMsg = string.Empty;
            object[] args = new object[] { transactionObjects, errMsg, connString, dbType };
            //反射执行
            object tempObj = ExecuteTempOperateReflectMethod(tableName, "ExecuteTransaction", args);
            errMsg = args[1].ObjToStr();
        }

        /// <summary>
        /// 执行事务，可以针对不同表的操作
        /// </summary>
        /// <param name="transactionObjects">事务扩展对象集合</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public static void ExecuteTransactionExtend(List<TransactionExtendModel> transactionObjects, out string errMsg, string connString = null, DatabaseType? dbType = null)
        {
            IBaseBLL<BaseEntity> bll = BridgeObject.Resolve<IBaseBLL<BaseEntity>>(dbType);
            bll.ExecuteTransactionExtend(transactionObjects, out errMsg, connString);
        }

        /// <summary>
        /// 执行事务
        /// </summary>
        /// <param name="transTask">事务处理函数</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        public static void TransactionHandle(TransactionTask transTask, out string errMsg, string connString = null, DatabaseType? dbType = null)
        {
            IBaseBLL<BaseEntity> bll = BridgeObject.Resolve<IBaseBLL<BaseEntity>>(dbType);
            bll.TransactionHandle(transTask, out errMsg, connString);
        }

        #endregion

        #region 通用树

        /// <summary>
        /// 获得通用树结构
        /// </summary>
        /// <param name="list">子节点</param>
        /// <param name="root">根节点</param>
        /// <param name="attributeParams">扩展属性处理方法</param>
        /// <param name="textName">节点显示字段</param>
        /// <param name="pIdFieldName">父Id字段名</param>
        /// <param name="icon">节点图标</param>
        /// <returns></returns>
        public static TreeNode GetTree<T>(IEnumerable<T> list, T root, Action<T, TreeAttributes> attributeParams = null, string pIdFieldName = "ParentId", string textName = "Name", string icon = null) where T : class
        {
            var node = TreeNode.Parse(root, (m1, n1) =>
            {
                PropertyInfo pid = typeof(T).GetProperty("Id");
                PropertyInfo ptextName = typeof(T).GetProperty(string.IsNullOrEmpty(textName) ? "Name" : textName);
                PropertyInfo pdisplay = textName == "Display" ? null : typeof(T).GetProperty("Display");
                PropertyInfo purl = typeof(T).GetProperty("Url");
                PropertyInfo picon = typeof(T).GetProperty("Icon");
                var objId = pid.GetValue(m1, null);
                var objName = ptextName.GetValue(m1, null);
                if (pdisplay != null && pdisplay.GetValue(m1, null) != null)
                {
                    objName = pdisplay.GetValue(m1, null).ObjToStr();
                }
                string strUrl = string.Empty;
                if (purl != null)
                {
                    var objUrl = purl.GetValue(m1, null);
                    strUrl = objUrl == null ? "" : objUrl.ToString();
                }
                string strIcon = icon;
                if (picon != null)
                {
                    var objIcon = picon.GetValue(m1, null);
                    strIcon = objIcon != null && objIcon.ToString() != "" ? objIcon.ToString() : icon;
                }
                string strCondition = string.Empty;
                n1.id = objId.ObjToGuid().ToString();
                n1.text = objName.ObjToStr();
                n1.iconCls = strIcon;
                n1.attribute = new TreeAttributes() { url = strUrl };
                if (attributeParams != null)
                {
                    attributeParams(m1, n1.attribute);
                }
                new OperateHandleFactory<T>().TreeNodeHandle(n1);
            }, m2 =>
            {
                List<T> tempList = new List<T>();
                foreach (T t in list)
                {
                    System.Reflection.PropertyInfo pparent = typeof(T).GetProperty(string.IsNullOrEmpty(pIdFieldName) ? "ParentId" : pIdFieldName);
                    var objParentId = pparent.GetValue(t, null);
                    string strParentId = objParentId == null ? "" : objParentId.ToString();
                    var objId = typeof(T).GetProperty("Id").GetValue(m2, null).ToString();
                    string strId = objId == null ? "" : objId.ToString();
                    if (string.IsNullOrWhiteSpace(strParentId) || string.IsNullOrWhiteSpace(objId))
                    {
                        continue;
                    }
                    if (strParentId == strId)
                    {
                        tempList.Add(t);
                    }
                }
                return tempList;
            });
            return node;
        }

        /// <summary>
        /// 获取树结点
        /// </summary>
        /// <param name="parentId">父记录Id</param>
        /// <param name="attributeParams">扩展属性处理方法</param>
        /// <param name="pIdFieldName">父Id字段名</param>
        /// <param name="textName">节点显示名称字段</param>
        /// <param name="sortField">排序字段名</param>
        /// <param name="icon">图标类</param>
        /// <param name="expression">过滤表达式</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static TreeNode GetTreeNode<T>(Guid? parentId, Action<T, TreeAttributes> attributeParams = null, string pIdFieldName = "ParentId", string textName = "Name", string sortField = "Id", string icon = null, Expression<Func<T, bool>> expression = null, string connString = null, DatabaseType? dbType = null) where T : class
        {
            return new TempOperate<T>().GetTreeNode(parentId, attributeParams, pIdFieldName, textName, sortField, icon, expression, connString, dbType);
        }

        /// <summary>
        /// 获取树结点
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="parentId">父记录Id</param>
        /// <param name="attributeParams">扩展属性处理方法</param>
        /// <param name="pIdFieldName">父Id字段名</param>
        /// <param name="textName">节点显示名称字段</param>
        /// <param name="sortField">排序字段名</param>
        /// <param name="icon">图标类</param>
        /// <param name="expression">过滤表达式</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static TreeNode GetTreeNode(string tableName, Guid? parentId, object attributeParams = null, string pIdFieldName = "ParentId", string textName = "Name", string sortField = "Id", string icon = null, object expression = null, string connString = null, DatabaseType? dbType = null)
        {
            //反射取数据
            object treeNode = ExecuteTempOperateReflectMethod(tableName, "GetTreeNode", new object[] { parentId, attributeParams, pIdFieldName, textName, sortField, icon, expression, connString, dbType });
            return treeNode as TreeNode;
        }

        /// <summary>
        /// 获取树结构模块所有的子结点记录
        /// </summary>
        /// <param name="parentId">父记录Id</param>
        /// <param name="pIdFieldName">父Id字段名</param>
        /// <param name="sortField">排序字段名</param>
        /// <param name="expression">过滤表达式</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回实体对象集合</returns>
        public static List<T> GetAllChildNodesData<T>(Guid? parentId, string pIdFieldName = "ParentId", string sortField = "Id", Expression<Func<T, bool>> expression = null, string connString = null, DatabaseType? dbType = null) where T : class
        {
            return new TempOperate<T>().GetAllChildNodesData(parentId, pIdFieldName, sortField, expression, connString, dbType);
        }

        /// <summary>
        /// 获取树结构模块所有的子结点记录
        /// </summary>
        /// <param name="tableName">实体表名</param>
        /// <param name="parentId">父记录Id</param>
        /// <param name="pIdFieldName">父Id字段名</param>
        /// <param name="sortField">排序字段名</param>
        /// <param name="expression">过滤表达式</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回实体对象集合</returns>
        public static object GetAllChildNodesData(string tableName, Guid? parentId, string pIdFieldName = "ParentId", string sortField = "Id", object expression = null, string connString = null, DatabaseType? dbType = null)
        {
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "GetAllChildNodesData", new object[] { parentId, pIdFieldName, sortField, expression, connString, dbType });
            return obj;
        }

        /// <summary>
        /// 获取树结构模块直接子结点记录
        /// </summary>
        /// <param name="parentId">父记录Id</param>
        /// <param name="pIdFieldName">父Id字段名</param>
        /// <param name="sortField">排序字段名</param>
        /// <param name="expression">过滤表达式</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回实体对象集合</returns>
        public static List<T> GetChildNodesData<T>(Guid? parentId, string pIdFieldName = "ParentId", string sortField = "Id", Expression<Func<T, bool>> expression = null, string connString = null, DatabaseType? dbType = null) where T : class
        {
            return new TempOperate<T>().GetChildNodesData(parentId, pIdFieldName, sortField, expression, connString, dbType);
        }

        /// <summary>
        /// 获取树结构模块直接子结点记录
        /// </summary>
        /// <param name="tableName">实体数据表名</param>
        /// <param name="parentId">父记录Id</param>
        /// <param name="pIdFieldName">父Id字段名</param>
        /// <param name="sortField">排序字段名</param>
        /// <param name="expression">过滤表达式</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>返回实体对象集合</returns>
        public static object GetChildNodesData(string tableName, Guid? parentId, string pIdFieldName = "ParentId", string sortField = "Id", object expression = null, string connString = null, DatabaseType? dbType = null)
        {
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(tableName, "GetChildNodesData", new object[] { parentId, pIdFieldName, sortField, expression, connString, dbType });
            return obj;
        }

        /// <summary>
        /// 获取某个模块的树结点
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="parentId">父记录Id</param>
        /// <param name="attributeParams">扩展属性处理方法</param>
        /// <param name="pIdFieldName">父Id字段名</param>
        /// <param name="textName">节点显示字段名</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="icon">图标样式类</param>
        /// <param name="expression">过滤表达式</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static TreeNode GetTreeNode(Guid moduleId, Guid parentId, object attributeParams = null, string pIdFieldName = "ParentId", string textName = "Name", string sortField = "Id", string icon = null, object expression = null, string connString = null, DatabaseType? dbType = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return GetTreeNode(module.TableName, parentId, attributeParams, pIdFieldName, textName, sortField, icon, expression, connString, dbType);
            }
            return null;
        }

        /// <summary>
        /// 获取所有的子结点记录
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="parentId">父记录Id</param>
        /// <param name="pIdFieldName">父Id字段名</param>
        /// <param name="sortField">排序字段</param>
        /// <param name="expression">过滤表达式</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static object GetAllChildNodesData(Guid moduleId, Guid parentId, string pIdFieldName = "ParentId", string sortField = "Id", object expression = null, string connString = null, DatabaseType? dbType = null)
        {
            Sys_Module module = SystemOperate.GetModuleById(moduleId);
            if (module != null && !string.IsNullOrEmpty(module.TableName))
            {
                return GetAllChildNodesData(module.TableName, parentId, pIdFieldName, sortField, expression, connString, dbType);
            }
            return null;
        }

        #endregion

        #region SQL操作

        /// <summary>
        /// 以SQL方式获取实体集合
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParams">sql参数</param>
        /// <param name="connString">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static List<T> GetEntitiesBySql<T>(out string errMsg, string sql, object[] sqlParams = null, string connString = null, DatabaseType? dbType = null) where T : class
        {
            IBaseBLL<T> bll = BridgeObject.Resolve<IBaseBLL<T>>(dbType);
            return bll.GetEntitiesBySql(out errMsg, sql, sqlParams, connString);
        }

        /// <summary>
        /// 以SQL方式执行查询
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParams">sql参数</param>
        /// <param name="connString">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static DataTable ExecuteQuery(out string errMsg, string sql, Dictionary<string, object> sqlParams = null, string connString = null, DatabaseType? dbType = null)
        {
            IBaseBLL<BaseEntity> bll = BridgeObject.Resolve<IBaseBLL<BaseEntity>>(dbType);
            return bll.ExecuteQuery(out errMsg, sql, sqlParams, connString);
        }

        /// <summary>
        /// 获取查询到的第一行第一列的数据
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <param name="sql">sql语句</param>
        /// <param name="sqlParams">sql参数</param>
        /// <param name="connString">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static object ExecuteScale(out string errMsg, string sql, object[] sqlParams = null, string connString = null, DatabaseType? dbType = null)
        {
            IBaseBLL<BaseEntity> bll = BridgeObject.Resolve<IBaseBLL<BaseEntity>>(dbType);
            return bll.ExecuteScale(out errMsg, sql, sqlParams, connString);
        }

        /// <summary>
        /// 执行增删改语句
        /// </summary>
        /// <param name="errMsg">错误信息</param>
        /// <param name="sql">sql</param>
        /// <param name="sqlParams">sql参数</param>
        /// <param name="connString">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public static int ExecuteNonQuery(out string errMsg, string sql, Dictionary<string, object> sqlParams = null, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            IBaseBLL<BaseEntity> bll = BridgeObject.Resolve<IBaseBLL<BaseEntity>>(dbType);
            return bll.ExecuteNonQuery(out errMsg, sql, sqlParams, connString, transConn);
        }

        /// <summary>
        /// 执行存储过程，针对非查询
        /// </summary>
        /// <param name="errMsg">异常信息</param>
        /// <param name="outParams">输出参数</param>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="inParams">输入参数 eg:new{Age=30}</param>
        /// <param name="connString">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns>返回受影响的行数</returns>
        public static int RunProcedureNoQuery(out string errMsg, ref Dictionary<string, object> outParams, string procedureName, object inParams = null, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            IBaseBLL<BaseEntity> bll = BridgeObject.Resolve<IBaseBLL<BaseEntity>>(dbType);
            return bll.RunProcedureNoQuery(out errMsg, ref outParams, procedureName, inParams, connString, transConn);
        }

        /// <summary>
        /// 执行存储过程，针对查询
        /// </summary>
        /// <param name="errMsg">异常信息</param>
        /// <param name="outParams">输出参数</param>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="inParams">输入参数 eg:new{Age=30}</param>
        /// <param name="connString">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <param name="transConn">事务连接对象</param>
        /// <returns>返回查询记录</returns>
        public static DataTable RunProcedure(out string errMsg, ref Dictionary<string, object> outParams, string procedureName, object inParams = null, string connString = null, DatabaseType? dbType = null, IDbConnection transConn = null)
        {
            IBaseBLL<BaseEntity> bll = BridgeObject.Resolve<IBaseBLL<BaseEntity>>(dbType);
            return bll.RunProcedure(out errMsg, ref outParams, procedureName, inParams, connString, transConn);
        }

        /// <summary>
        /// 存储过程分页查询
        /// </summary>
        /// <param name="errMsg">异常信息</param>
        /// <param name="total">总记录数</param>
        /// <param name="tableName">表名或SQL语句</param>
        /// <param name="fields">查询字段</param>
        /// <param name="where">条件</param>
        /// <param name="pageInfo">分页对象</param>
        /// <param name="connString">连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static DataTable PagingQueryByProcedure(out string errMsg, out long total, string tableName, string fields, string where, PageInfo pageInfo, string connString = null, DatabaseType? dbType = null)
        {
            errMsg = string.Empty;
            total = 0;
            PageInfo tempPageInfo = pageInfo == null ? PageInfo.GetDefaultPageInfo() : pageInfo;
            string procedureName = "[dbo].[GetPageTableByRowNumber]";
            Dictionary<string, object> outParams = new Dictionary<string, object>();
            outParams.Add("RecordCount", 0);
            string con = string.IsNullOrWhiteSpace(where) ? string.Empty : string.Format("AND {0}", where);
            object inParams = new { Field = fields, TableName = tableName, condition = con, OrderField = tempPageInfo.sortname, OrderType = tempPageInfo.sortorder == "asc" ? 1 : 0, pageindx = tempPageInfo.page, PageSize = tempPageInfo.pagesize };
            DataTable dt = RunProcedure(out errMsg, ref outParams, procedureName, inParams, connString, dbType);
            total = outParams[outParams.Keys.FirstOrDefault()].ObjToLong();
            return dt;
        }

        #endregion

        #region 通用保存

        //加锁静态变量
        private static object lockObj = new object();

        /// <summary>
        /// 通用保存（非事务方式）
        /// </summary>
        /// <param name="formDataObj">表单数据</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>返回操作结果，大于0成功，等于0失败</returns>
        public static Guid SaveRecord(FormDataObject formDataObj, out string errMsg)
        {
            errMsg = string.Empty;
            if (formDataObj == null)
            {
                errMsg = "表单数据对象为空！";
                return Guid.Empty;
            }
            if ((!formDataObj.ModuleId.HasValue || formDataObj.ModuleId == Guid.Empty) && string.IsNullOrEmpty(formDataObj.ModuleName))
            {
                errMsg = "没有传递主模块参数！";
                return Guid.Empty;
            }
            if (formDataObj.ModuleData == null || formDataObj.ModuleData.Count == 0)
            {
                errMsg = "没有模块表单数据！";
                return Guid.Empty;
            }
            Sys_Module module = null;
            Guid mainId = Guid.Empty; //主记录Id
            //主模块Id处理
            if (!formDataObj.ModuleId.HasValue) //未传递模块Id
            {
                //按模块名称取模块
                module = SystemOperate.GetModuleByName(formDataObj.ModuleName);
                if (module == null)
                {
                    errMsg = string.Format("模块【{0}】不存在！", formDataObj.ModuleName);
                    return Guid.Empty;
                }
                formDataObj.ModuleId = module.Id;
            }
            else //已传递模块Id
            {
                module = SystemOperate.GetModuleById(formDataObj.ModuleId.Value);
                formDataObj.ModuleName = module.Name;
            }
            //主模块类型
            Type modelType = SystemOperate.GetModelType(formDataObj.ModuleId.Value);
            //主模块字段属性
            //排除基类字段属性
            List<PropertyInfo> properties = modelType.GetProperties().Where(x => x.CanWrite && !CommonDefine.BaseEntityFieldsContainId.Contains(x.Name)).ToList();
            if (properties == null && properties.Count == 0)
            {
                errMsg = "模块属性集合为空！";
                return Guid.Empty;
            }
            List<string> propertyNames = properties.Select(x => x.Name).ToList();
            //实例化
            object mainModelObj = Activator.CreateInstance(modelType);
            ModelRecordOperateType operateType = ModelRecordOperateType.Add;
            NameValueObject IdObj = formDataObj.ModuleData.Where(x => x.name == "Id" && !string.IsNullOrEmpty(x.value) && x.value.ObjToGuid() != Guid.Empty).FirstOrDefault();
            operateType = IdObj == null || string.IsNullOrEmpty(IdObj.value) ? ModelRecordOperateType.Add : ModelRecordOperateType.Edit;
            if (IdObj != null && !string.IsNullOrEmpty(IdObj.value)) mainId = IdObj.value.ObjToGuid();
            if (operateType == ModelRecordOperateType.Edit)
            {
                if (UserInfo.CurrentUserInfo == null || ((!formDataObj.ToDoTaskId.HasValue || formDataObj.ToDoTaskId.Value == Guid.Empty) && !PermissionOperate.HasButtonPermission(UserInfo.CurrentUserInfo.UserId, module.Id, "编辑")))
                {
                    errMsg = string.Format("您没有模块【{0}】的编辑权限，如有疑问请联系管理员！", module.Name);
                    return Guid.Empty;
                }
                mainModelObj = GetEntityById(module.Id, mainId, out errMsg);
            }
            else
            {
                if (UserInfo.CurrentUserInfo == null || !PermissionOperate.HasButtonPermission(UserInfo.CurrentUserInfo.UserId, module.Id, "新增"))
                {
                    errMsg = string.Format("您没有模块【{0}】的新增权限，如有疑问请联系管理员！", module.Name);
                    return Guid.Empty;
                }
            }
            //基类字段赋值
            ModelCommonPropertySetValue(module.Id, mainModelObj, operateType == ModelRecordOperateType.Add);
            List<string> fieldNames = new List<string>();
            if (operateType == ModelRecordOperateType.Edit && formDataObj.IsReleaseDraft) //草稿发布
            {
                PropertyInfo draftProperty = modelType.GetProperty("IsDraft");
                if (draftProperty != null)
                {
                    draftProperty.SetValue(mainModelObj, false, null);
                    fieldNames.Add("IsDraft");
                }
            }
            else if (operateType == ModelRecordOperateType.Add && formDataObj.IsDraft && module.IsEnabledDraft) //保存草稿
            {
                PropertyInfo draftProperty = modelType.GetProperty("IsDraft");
                if (draftProperty != null)
                    draftProperty.SetValue(mainModelObj, true, null);
            }
            //模块对象赋值
            foreach (NameValueObject nv in formDataObj.ModuleData)
            {
                if (!propertyNames.Contains(nv.name)) continue;
                PropertyInfo p = properties.Where(x => x.Name == nv.name).FirstOrDefault();
                IgnoreAttribute ignoreAttr = (IgnoreAttribute)Attribute.GetCustomAttribute(p, typeof(IgnoreAttribute));
                if (ignoreAttr != null) continue;
                string tempValue = p.PropertyType == typeof(String) ? nv.value.ObjToStr().Trim() : nv.value;
                if (nv.name != "Id")
                {
                    #region 图片上传字段处理
                    Sys_FormField formField = SystemOperate.GetDefaultFormSingleField(module.Id, nv.name);
                    if (formField == null || formField.ControlTypeOfEnum == ControlTypeEnum.LabelBox) continue;
                    if (operateType == ModelRecordOperateType.Add && (!formField.IsAllowAdd.HasValue || !formField.IsAllowAdd.Value))
                        continue;
                    if (operateType == ModelRecordOperateType.Edit && (!formField.IsAllowEdit.HasValue || !formField.IsAllowEdit.Value))
                        continue;
                    if (formField.ControlTypeOfEnum == ControlTypeEnum.ImageUpload && !string.IsNullOrWhiteSpace(tempValue))
                    {
                        try
                        {
                            //图片上传控件处理真实的图片地址
                            //源路径
                            string sourcePath = Globals.GetWebDir() + tempValue.Substring(1, tempValue.Length - 1).Replace("/", "\\");
                            if (sourcePath.Contains("Upload\\Image\\ImgUploadControl")) //图片未发生改变时不更新该字段
                                continue;
                            //目标路径
                            string desPath = Globals.GetWebDir() + "Upload\\Image\\ImgUploadControl\\";
                            if (!Directory.Exists(desPath)) Directory.CreateDirectory(desPath);
                            desPath += Path.GetFileName(sourcePath);
                            File.Copy(sourcePath, desPath, true); //复制文件
                            tempValue = "/" + desPath.Replace(Globals.GetWebDir(), "").Replace("\\", "/");
                        }
                        catch { }
                    }
                    #endregion
                    fieldNames.Add(nv.name); //要更新的字段
                }
                p.SetValue(mainModelObj, TypeUtil.ChangeType(tempValue, p.PropertyType), null);
            }
            List<string> canOpFields = PermissionOperate.GetCanOperateFields(UserInfo.CurrentUserInfo.UserId, module.Id, operateType == ModelRecordOperateType.Add ? FieldPermissionTypeEnum.AddField : FieldPermissionTypeEnum.EditField);
            if (canOpFields.Count > 0 && !canOpFields.Contains("-1"))
            {
                fieldNames = fieldNames.Where(x => canOpFields.Contains(x)).ToList();
            }
            //基础验证
            string validateMsg = ModelVerify(module.Id, mainModelObj, fieldNames);
            if (!string.IsNullOrEmpty(validateMsg)) //验证不通过
            {
                errMsg = validateMsg;
                return Guid.Empty;
            }
            if (module.Name == "模块管理")
            {
                Sys_Module tempModule = mainModelObj as Sys_Module;
                bool createModule = false; //自定义模块保存处理标识
                //新增模块或编辑自定义模块时
                if (operateType == ModelRecordOperateType.Add)
                {
                    createModule = true;
                }
                else if (operateType == ModelRecordOperateType.Edit)
                {
                    Sys_Module initTempModule = SystemOperate.GetModuleById(mainId);
                    createModule = initTempModule.IsCustomerModule;
                    if (initTempModule.IsCustomerModule)
                    {
                        tempModule.Id = initTempModule.Id;
                        tempModule.Name = initTempModule.Name;
                        tempModule.TableName = initTempModule.TableName;
                    }
                }
                if (createModule) //自定义模块保存
                {
                    if (formDataObj.Details == null)
                    {
                        errMsg = "模块字段配置数据不能为空！";
                        return Guid.Empty;
                    }
                    errMsg = ToolOperate.CreateTempModule(tempModule, formDataObj.Details.FirstOrDefault());
                    return string.IsNullOrEmpty(errMsg) ? Guid.NewGuid() : Guid.Empty;
                }
            }
            Guid rs = Guid.Empty; //返回结果
            PropertyInfo billFieldProperty = null; //编码字段属性
            //判断是否有编码规则
            if (operateType == ModelRecordOperateType.Add && module.IsEnableCodeRule)
            {
                string billFieldName = SystemOperate.GetBillCodeFieldName(module); //编码字段
                billFieldProperty = properties.Where(x => x.Name == billFieldName && x.PropertyType == typeof(String)).FirstOrDefault();
            }
            if (billFieldProperty != null) //有编码规则是加锁
            {
                lock (lockObj) //取编码时加锁
                {
                    object tempValue = billFieldProperty.GetValue(mainModelObj, null); //原本字段值
                    //是否需要重新取编码
                    //前端没有赋值时取编码，或者前端编码已经赋值但编码是已经存在了
                    if (string.IsNullOrWhiteSpace(tempValue.ObjToStr()) ||
                        Count(out errMsg, module.Id, false, null, string.Format("{0}='{1}'", billFieldProperty.Name, tempValue.ObjToStr())) > 0) //需要重新取编码
                    {
                        string billCode = SystemOperate.GetBillCode(module); //取编码值
                        if (!string.IsNullOrEmpty(billCode)) //当前模块启用了编码规则
                        {
                            billFieldProperty.SetValue(mainModelObj, billCode, null);
                        }
                    }
                    //保存主模块数据
                    rs = OperateRecord(module.Id, mainModelObj, operateType, out errMsg, fieldNames, !formDataObj.ToDoTaskId.HasValue || formDataObj.ToDoTaskId.Value == Guid.Empty);
                }
            }
            else //没有编码规则
            {
                //保存主模块数据
                rs = OperateRecord(module.Id, mainModelObj, operateType, out errMsg, fieldNames, !formDataObj.ToDoTaskId.HasValue || formDataObj.ToDoTaskId.Value == Guid.Empty);
            }
            if (rs == Guid.Empty)
            {
                errMsg = string.Format("模块【{0}】数据保存失败，原因：{1}", formDataObj.ModuleName, errMsg);
                return rs;
            }
            if (operateType == ModelRecordOperateType.Add)
                mainId = rs;
            //明细模块处理
            if (formDataObj.Details != null && formDataObj.Details.Count > 0)
            {
                StringBuilder sbErr = new StringBuilder();
                foreach (DetailFormObject obj in formDataObj.Details)
                {
                    string tempMsg = string.Empty; //异常信息
                    object[] args = new object[] { obj, tempMsg }; //执行自定义保存方法参数
                    object isExecute = ExecuteCustomeOperateHandleMethod(module.Id, "OverSaveDetailData", args);
                    tempMsg = args[1].ObjToStr();
                    if (isExecute.ObjToBool()) //执行了自定义保存明细方法
                    {
                        continue;
                    }
                    Sys_Module detailModule = null;
                    //明细模块Id处理
                    if (!obj.ModuleId.HasValue) //明细模块Id未传递
                    {
                        detailModule = SystemOperate.GetModuleByName(obj.ModuleName);
                        if (detailModule == null)
                        {
                            sbErr.AppendFormat("明细模块【{0}】不存在，该明细模块数据保存失败！", obj.ModuleName);
                            continue;
                        }
                        obj.ModuleId = detailModule.Id;
                    }
                    else //明细模块名称处理
                    {
                        detailModule = SystemOperate.GetModuleById(obj.ModuleId.Value);
                        if (detailModule == null)
                        {
                            sbErr.AppendFormat("Id为【{0}】的明细模块不存在，该明细模块数据保存失败！", obj.ModuleId.Value);
                            continue;
                        }
                        obj.ModuleName = detailModule.Name;
                    }
                    //明细模块类型
                    Type detailModelType = SystemOperate.GetModelType(obj.ModuleId.Value);
                    //明细模块字段属性
                    List<PropertyInfo> detailProperties = detailModelType.GetProperties().Where(x => x.CanWrite && !CommonDefine.BaseEntityFieldsContainId.Contains(x.Name)).ToList();
                    if (detailProperties == null && detailProperties.Count == 0) continue;
                    List<string> detailPropertyNames = detailProperties.Select(x => x.Name).ToList();
                    //当前是编辑记录的记录Id
                    List<Guid> curIds = new List<Guid>();
                    //当前新增的记录Id
                    List<Guid> addIds = new List<Guid>();
                    foreach (string rowDataJson in obj.ModuleDatas)
                    {
                        if (string.IsNullOrEmpty(rowDataJson)) continue;
                        List<NameValueObject> nvs = JsonHelper.Deserialize<List<NameValueObject>>(rowDataJson);
                        //处理一条明细记录
                        NameValueObject detailIdObj = nvs.Where(x => x.name == "Id" && !string.IsNullOrEmpty(x.value) && Convert.ToInt64(x.value) > 0).FirstOrDefault();
                        ModelRecordOperateType detailOperateType = ModelRecordOperateType.Add;
                        if (detailIdObj != null)
                        {
                            curIds.Add(detailIdObj.value.ObjToGuid());
                            detailOperateType = ModelRecordOperateType.Edit;
                        }
                        //实例化
                        object detailModelObj = Activator.CreateInstance(detailModelType);
                        if (detailOperateType == ModelRecordOperateType.Edit)
                        {
                            detailModelObj = GetEntityById(detailModule.Id, detailIdObj.value.ObjToGuid(), out errMsg);
                        }
                        //基类字段赋值
                        ModelCommonPropertySetValue(detailModule.Id, detailModelObj, detailOperateType == ModelRecordOperateType.Add);
                        //要更新的字段
                        List<string> detailFieldNames = new List<string>();
                        //对象属性赋值
                        foreach (NameValueObject nv in nvs)
                        {
                            if (!detailPropertyNames.Contains(nv.name)) continue;
                            PropertyInfo p = detailProperties.Where(x => x.Name == nv.name).FirstOrDefault();
                            string tempValue = p.PropertyType == typeof(String) ? nv.value.ObjToStr().Trim() : nv.value;
                            #region 图片上传字段处理
                            if (nv.name != "Id")
                            {
                                Sys_FormField detailFormField = SystemOperate.GetDefaultFormSingleField(detailModule.Id, nv.name);
                                if (detailFormField == null || detailFormField.ControlTypeOfEnum == ControlTypeEnum.LabelBox) continue;
                                if (detailOperateType == ModelRecordOperateType.Add && (!detailFormField.IsAllowAdd.HasValue || !detailFormField.IsAllowAdd.Value))
                                    continue;
                                if (detailOperateType == ModelRecordOperateType.Edit && (!detailFormField.IsAllowEdit.HasValue || !detailFormField.IsAllowEdit.Value))
                                    continue;
                                if (detailFormField.ControlTypeOfEnum == ControlTypeEnum.ImageUpload && !string.IsNullOrWhiteSpace(tempValue))
                                {
                                    try
                                    {
                                        //图片上传控件处理真实的图片地址
                                        //源路径
                                        string sourcePath = Globals.GetWebDir() + tempValue.Substring(1, tempValue.Length - 1).Replace("/", "\\");
                                        if (sourcePath.Contains("Upload\\Image\\ImgUploadControl")) //图片未发生改变时不更新该字段
                                            continue;
                                        //目标路径
                                        string desPath = Globals.GetWebDir() + "Upload\\Image\\ImgUploadControl\\";
                                        if (!Directory.Exists(desPath)) Directory.CreateDirectory(desPath);
                                        desPath += Path.GetFileName(sourcePath);
                                        File.Copy(sourcePath, desPath, true); //复制文件
                                        tempValue = "/" + desPath.Replace(Globals.GetWebDir(), "").Replace("\\", "/");
                                    }
                                    catch { }
                                }
                                detailFieldNames.Add(nv.name); //要更新的字段
                            }
                            #endregion
                            p.SetValue(detailModelObj, TypeUtil.ChangeType(tempValue, p.PropertyType), null);
                        }
                        //新增时对外键模块是父模块的外键字段赋值
                        if (detailOperateType == ModelRecordOperateType.Add)
                        {
                            //字段赋值
                            List<Sys_Field> sysFields = CommonOperate.GetEntities<Sys_Field>(out errMsg, x => x.Sys_ModuleId == obj.ModuleId.Value && x.ForeignModuleName == formDataObj.ModuleName);
                            Sys_Field idField = sysFields.Where(x => x.Name.EndsWith("Id")).FirstOrDefault();
                            //外键Id字段赋值
                            PropertyInfo p1 = detailProperties.Where(x => x.Name == idField.Name).FirstOrDefault();
                            p1.SetValue(detailModelObj, mainId, null);
                            //外键Name字段赋值
                            PropertyInfo p2 = detailProperties.Where(x => x.Name == idField.Name.Replace("Id", "Name")).FirstOrDefault();
                            if (p2 != null && !string.IsNullOrEmpty(detailModule.TitleKey))
                            {
                                NameValueObject tempNv = formDataObj.ModuleData.Where(x => x.name == detailModule.TitleKey).FirstOrDefault();
                                if (tempNv != null && !string.IsNullOrEmpty(tempNv.value))
                                {
                                    p2.SetValue(detailModelObj, tempNv.value, null);
                                }
                            }
                        }
                        List<string> canOpDetailFields = PermissionOperate.GetCanOperateFields(UserInfo.CurrentUserInfo.UserId, detailModule.Id, detailOperateType == ModelRecordOperateType.Add ? FieldPermissionTypeEnum.AddField : FieldPermissionTypeEnum.EditField);
                        if (canOpDetailFields.Count > 0 && !canOpDetailFields.Contains("-1"))
                        {
                            detailFieldNames = detailFieldNames.Where(x => canOpDetailFields.Contains(x)).ToList();
                        }
                        //基础验证
                        string detailValidateMsg = ModelVerify(detailModule.Id, detailModelObj, detailFieldNames);
                        if (!string.IsNullOrEmpty(detailValidateMsg)) //验证不通过
                        {
                            sbErr.AppendLine(detailValidateMsg);
                            continue;
                        }
                        Guid detailRs = Guid.Empty; //返回结果
                        PropertyInfo tempBillFieldProperty = null; //编码字段属性
                        //判断是否有编码规则
                        if (detailOperateType == ModelRecordOperateType.Add && detailModule.IsEnableCodeRule)
                        {
                            string billFieldName = SystemOperate.GetBillCodeFieldName(detailModule); //编码字段
                            tempBillFieldProperty = detailProperties.Where(x => x.Name == billFieldName && x.PropertyType == typeof(String)).FirstOrDefault();
                        }
                        if (tempBillFieldProperty != null) //有编码规则是加锁
                        {
                            lock (lockObj) //取编码时加锁
                            {
                                object tempValue = tempBillFieldProperty.GetValue(detailModelObj, null); //原本字段值
                                //是否需要重新取编码
                                //前端没有赋值时取编码，或者前端编码已经赋值但编码是已经存在了
                                if (string.IsNullOrWhiteSpace(tempValue.ObjToStr()) ||
                                    Count(out errMsg, detailModule.Id, false, null, string.Format("{0}='{1}'", tempBillFieldProperty.Name, tempValue.ObjToStr())) > 0) //需要重新取编码
                                {
                                    string billCode = SystemOperate.GetBillCode(detailModule); //取编码值
                                    if (!string.IsNullOrEmpty(billCode)) //编码值不为空
                                    {
                                        tempBillFieldProperty.SetValue(detailModelObj, billCode, null);
                                    }
                                }
                                //保存明细数据
                                detailRs = OperateRecord(detailModule.Id, detailModelObj, detailOperateType, out errMsg, detailFieldNames);
                            }
                        }
                        else
                        {
                            //保存明细数据
                            detailRs = OperateRecord(detailModule.Id, detailModelObj, detailOperateType, out errMsg, detailFieldNames);
                        }
                        if (detailRs == Guid.Empty)
                        {
                            sbErr.AppendFormat("明细模块【{0}】数据保存失败，原因：{1}；", obj.ModuleName, errMsg);
                            continue;
                        }
                        if (detailOperateType == ModelRecordOperateType.Add)
                        {
                            addIds.Add(detailRs);
                        }
                    }
                    if (operateType == ModelRecordOperateType.Edit)
                    {
                        //找出需要删除的明细记录
                        //先找出主模块记录下该明细模块所有明细记录，将当前明细记录中不存在的明细记录删除
                        Sys_Field sysField = CommonOperate.GetEntity<Sys_Field>(x => x.Sys_ModuleId == obj.ModuleId.Value && x.ForeignModuleName == formDataObj.ModuleName && x.Name.EndsWith("Id"), null, out errMsg);
                        object allDetailRecords = GetEntitiesByField(obj.ModuleId.Value, sysField.Name, mainId, out errMsg);
                        PropertyInfo detailIdProperty = detailProperties.Where(x => x.Name == "Id").FirstOrDefault();
                        //获取要删除的明细记录
                        List<Guid> delIds = new List<Guid>();
                        foreach (object t in (allDetailRecords as IEnumerable))
                        {
                            Guid detailId = detailIdProperty.GetValue(t, null).ObjToGuid();
                            if (!curIds.Contains(detailId) && !addIds.Contains(detailId))
                            {
                                delIds.Add(detailId);
                            }
                        }
                        if (delIds.Count > 0) //存在要删除的明细记录
                        {
                            bool delRs = DeleteRecords(obj.ModuleId.Value, delIds, out errMsg);
                            if (!delRs)
                            {
                                sbErr.AppendFormat("明细模块【{0}】中需要被删除的记录（记录Id为【{1}】)删除失败，原因：{2}；", obj.ModuleName, string.Join(",", delIds), errMsg);
                            }
                        }
                    }
                }
                errMsg = sbErr.ToString();
            }
            if (rs != Guid.Empty && operateType == ModelRecordOperateType.Edit)
            {
                rs = IdObj.value.ObjToGuid();
            }
            //返回结果
            return rs;
        }

        #endregion

        #region 导入、导出等

        /// <summary>
        /// 通用数据导入
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="userId">当前用户Id</param>
        /// <param name="importFile">导入文件（绝对路径）</param>
        /// <returns>返回异常信息</returns>
        public static string ImportData(Guid moduleId, Guid userId, string importFile)
        {
            string errMsg = string.Empty;
            Type modelType = CommonOperate.GetModelType(moduleId);
            try
            {
                var fs = new FileStream(importFile, System.IO.FileMode.Open);
                DataTable dt = NPOI_ExcelHelper.RenderFromExcel(fs);
                List<string> fieldNames = dt != null && dt.Columns.Count > 0 ? dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList() : new List<string>();
                List<PropertyInfo> ps = modelType.GetProperties().Where(x => !fieldNames.Contains(x.Name)).ToList(); //需要赋值的属性
                Type listType = typeof(List<>).MakeGenericType(new Type[] { modelType });
                //导入数据前准备工作
                object addModels = null; //要新增的实体集合
                object updateModels = null; //要修改的实体集合
                PropertyInfo idProperty = modelType.GetProperty("Id");
                PropertyInfo codeFieldProperty = null; //编辑规则字段
                string billCodeField = SystemOperate.GetBillCodeFieldName(moduleId);
                if (!string.IsNullOrEmpty(billCodeField))
                    codeFieldProperty = modelType.GetProperty(billCodeField);
                int no = 0;
                //填充实体集合
                Func<object, string> action = (object model) =>
                {
                    string err = FluentVerify(moduleId, model); //FluentValidation验证
                    if (string.IsNullOrEmpty(err)) //验证通过
                    {
                        object existModel = null;
                        //主键字段验证
                        err = PrimaryKeyVerify(moduleId, model, out existModel);
                        if (string.IsNullOrEmpty(err)) //验证通过
                        {
                            //表单验证
                            Sys_Form form = SystemOperate.GetUserForm(userId, moduleId); //获取当前用户表单
                            List<Sys_FormField> formFields = SystemOperate.GetFormField(form.Id);
                            List<string> verifyFields = new List<string>(); //导入验证字段
                            foreach (Sys_FormField field in formFields)
                            {
                                if (field.Sys_FieldName == "Id" || field.IsVisible != true ||
                                   field.IsDeleted || field.IsAllowAdd != true)
                                    continue;
                                PropertyInfo p = modelType.GetProperty(field.Sys_FieldName);
                                if (p == null) continue;
                                verifyFields.Add(field.Sys_FieldName);
                            }
                            err = FormFieldVerify(moduleId, model, verifyFields);
                            if (!string.IsNullOrEmpty(err))
                            {
                                return err;
                            }
                        }
                        else //主键数据已存在
                        {
                            //把model中未赋值的字段赋值
                            if (existModel != null)
                            {
                                foreach (PropertyInfo p in ps)
                                {
                                    p.SetValue(model, p.GetValue(existModel, null), null);
                                }
                            }
                        }
                    }
                    else
                    {
                        return err;
                    }
                    Guid id = idProperty.GetValue(model, null).ObjToGuid();
                    //公共属性设置
                    ModelCommonPropertySetValue(moduleId, model, id == Guid.Empty);
                    if (id == Guid.Empty) //新增
                    {
                        if (codeFieldProperty != null)
                        {
                            no++;
                            string code = string.Format("{0}_{1}", SystemOperate.GetBillCode(moduleId), no);
                            codeFieldProperty.SetValue(model, code, null);
                        }
                        Globals.ExecuteReflectMethod(listType, "Add", new object[] { model }, ref addModels);
                    }
                    else //编辑
                    {
                        Globals.ExecuteReflectMethod(listType, "Add", new object[] { model }, ref updateModels);
                    }
                    return string.Empty;
                };
                ToolOperate.FillModels(moduleId, dt, out errMsg, action);
                if (string.IsNullOrEmpty(errMsg))
                {
                    //创建事务模型集合
                    List<TransactionExtendModel> tranList = new List<TransactionExtendModel>();
                    if (addModels != null)
                    {
                        tranList.Add(new TransactionExtendModel(modelType, DataOperateType.Add, addModels as IEnumerable));
                    }
                    if (updateModels != null)
                    {
                        tranList.Add(new TransactionExtendModel(modelType, DataOperateType.Edit, updateModels as IEnumerable));
                    }
                    //执行事务导入数据
                    ExecuteTransactionExtend(tranList, out errMsg);
                }
            }
            catch (Exception ex)
            {
                errMsg = string.Format("异常：{0}", ex.Message);
            }
            return errMsg;
        }

        /// <summary>
        /// 通用数据导出
        /// </summary>
        /// <param name="gridParams">网格参数</param>
        /// <param name="userId">当前用户Id</param>
        /// <param name="fileName">导出文件（绝对路径）</param>
        /// <returns>返回异常信息</returns>
        public static string ExportData(GridDataParmas gridParams, Guid userId, string fileName)
        {
            long total = 0;
            object data = GetGridData(gridParams, out total);
            string errMsg = string.Empty;
            DataTable dt = ToolOperate.FillToDataTable(gridParams.ModuleId, data, userId, out errMsg);
            if (dt == null) return errMsg;
            try
            {
                NPOI_ExcelBatchExport.ExportExcel(dt, fileName);
                int pageNum = 0; //总页数
                if (total % gridParams.PagingInfo.pagesize == 0)
                    pageNum = (int)(total / gridParams.PagingInfo.pagesize);
                else
                    pageNum = (int)(total / gridParams.PagingInfo.pagesize) + 1;
                for (int i = PageInfo.pageIndexStartNo + 1; i <= pageNum; i++)
                {
                    gridParams.PagingInfo.page = i;
                    data = GetGridData(gridParams, out total);
                    dt = ToolOperate.FillToDataTable(gridParams.ModuleId, data, userId, out errMsg);
                    NPOI_ExcelBatchExport.ExportExcel(dt, fileName);
                }
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return errMsg;
        }

        #endregion

        #region 数据保存验证

        /// <summary>
        /// 实体基础验证，包括FluentValidation验证、唯一性验证
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="model">实体对象</param>
        /// <param name="verifyFields">需要验证的字段</param>
        /// <returns>验证不通过返回验证错误信息，返回空字符串表示验证通过</returns>
        public static string ModelVerify(Guid moduleId, object model, List<string> verifyFields = null)
        {
            string errMsg = string.Empty;
            #region FluentValidation验证
            errMsg = FluentVerify(moduleId, model);
            if (!string.IsNullOrEmpty(errMsg))
                return errMsg;
            #endregion
            #region 主键验证
            object tempModel = null;
            errMsg = PrimaryKeyVerify(moduleId, model, out tempModel);
            if (!string.IsNullOrEmpty(errMsg))
                return errMsg;
            #endregion
            #region 表单字段验证
            errMsg = FormFieldVerify(moduleId, model, verifyFields);
            return errMsg;
            #endregion
        }

        /// <summary>
        /// FluentValidation验证
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="model">实体对象</param>
        /// <returns></returns>
        public static string FluentVerify(Guid moduleId, object model)
        {
            Type modelType = GetModelType(moduleId);
            //FluentValidation验证
            Type fluentValidationType = GetFluentValidationModelType(moduleId);
            if (fluentValidationType != null) //FluentValidation验证类型存在
            {
                //实例化验证对象
                object obj = Activator.CreateInstance(fluentValidationType);
                MethodInfo method = fluentValidationType.GetMethod("Validate", new Type[] { modelType });
                //反射执行方法
                object executedObj = method.Invoke(obj, new object[] { model });
                ValidationResult validateResult = executedObj as ValidationResult;
                if (validateResult != null && !validateResult.IsValid)
                {
                    string errMsg = string.Join(",", validateResult.Errors.Select(x => x.ErrorMessage));
                    return errMsg;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 主键唯一验证
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="model">实体对象</param>
        /// <param name="existModel">主键重复的实体对象</param>
        /// <returns></returns>
        public static string PrimaryKeyVerify(Guid moduleId, object model, out object existModel)
        {
            existModel = null;
            Type modelType = GetModelType(moduleId);
            PropertyInfo idProperty = modelType.GetProperty("Id");
            Guid id = idProperty.GetValue(model, null).ObjToGuid(); //当前记录Id
            //主键唯一性验证
            List<string> primaryKeyFields = SystemOperate.GetModulePrimaryKeyFields(moduleId);
            if (primaryKeyFields.Count > 0)
            {
                List<ConditionItem> conditonItems = new List<ConditionItem>();
                StringBuilder sb = new StringBuilder();
                foreach (string field in primaryKeyFields)
                {
                    PropertyInfo p = modelType.GetProperty(field);
                    if (p == null) continue;
                    object value = p.GetValue(model, null);
                    conditonItems.Add(new ConditionItem() { Field = field, Method = QueryMethod.Equal, Value = value });
                    if (sb.ToString() != string.Empty) sb.Append(",");
                    sb.AppendFormat("【{0}】=【{1}】", SystemOperate.GetFieldDisplay(moduleId, field), SystemOperate.GetFieldDisplayValue(moduleId, model, field));
                }
                if (conditonItems.Count > 0)
                {
                    if (id != Guid.Empty) //编辑时排除当前记录
                    {
                        conditonItems.Add(new ConditionItem() { Field = "Id", Method = QueryMethod.NotEqual, Value = id });
                    }
                    object exp = GetQueryCondition(moduleId, conditonItems);
                    string errMsg = string.Empty;
                    object tempModel = GetEntity(moduleId, exp, null, out errMsg);
                    if (tempModel != null)
                    {
                        existModel = tempModel;
                        return string.Format("{0}的记录已存在，请不要重复添加！", sb.ToString());
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 表单字段配置验证
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="model">实体对象</param>
        /// <param name="verifyFields">需要验证字段的字段集合</param>
        /// <returns></returns>
        public static string FormFieldVerify(Guid moduleId, object model, List<string> verifyFields)
        {
            Type modelType = GetModelType(moduleId);
            PropertyInfo idProperty = modelType.GetProperty("Id");
            Guid id = idProperty.GetValue(model, null).ObjToGuid(); //当前记录Id
            //表单字段验证
            if (verifyFields != null && verifyFields.Count > 0)
            {
                foreach (string field in verifyFields)
                {
                    PropertyInfo p = modelType.GetProperty(field);
                    if (p == null) continue;
                    object value = p.GetValue(model, null);
                    Sys_FormField formField = SystemOperate.GetDefaultFormSingleField(moduleId, field);
                    if (formField == null) continue;
                    string fieldDisplay = SystemOperate.GetFieldDisplay(moduleId, field);
                    #region 必填验证
                    if (formField.IsRequired.HasValue && formField.IsRequired.Value) //必填验证
                    {
                        ControlTypeEnum crlType = formField.ControlTypeOfEnum;
                        string requiredDes = crlType == ControlTypeEnum.ComboBox ||
                                          crlType == ControlTypeEnum.ComboGrid ||
                                          crlType == ControlTypeEnum.ComboTree ?
                                          "请选择" : "不能为空";
                        bool isForeignField = SystemOperate.IsForeignField(moduleId, field); //是否是外键字段
                        if (value == null || (isForeignField && value.ObjToGuid() == Guid.Empty))
                        {
                            return string.Format("【{0}】为必填字段，{1}！", fieldDisplay, requiredDes);
                        }
                    }
                    if (formField.ControlTypeOfEnum == ControlTypeEnum.IntegerBox ||
                        formField.ControlTypeOfEnum == ControlTypeEnum.NumberBox) //最大值、最小值验证
                    {
                        if (formField.MinValue.HasValue && value.ObjToDecimal() < formField.MinValue.Value ||
                            formField.MaxValue.HasValue && value.ObjToDecimal() > formField.MaxValue.Value)
                        {
                            return string.Format("【{0}】的值介于【{1}】~【{2}】之间！", fieldDisplay, formField.MinValue.Value, formField.MaxValue.Value);
                        }
                    }
                    if (p.PropertyType == typeof(String) && ((formField.MinCharLen.HasValue && formField.MinCharLen.Value > 0) || (formField.MaxCharLen.HasValue && formField.MaxCharLen.Value > 0))) //字符长度验证
                    {
                        if (value.ObjToStr().Length < formField.MinCharLen.Value ||
                            value.ObjToStr().Length > formField.MaxCharLen.Value)
                        {
                            return string.Format("【{0}】字符长度在【{1}】~【{2}】之间！", fieldDisplay, formField.MinCharLen.Value, formField.MaxCharLen.Value);
                        }
                    }
                    #endregion
                    #region 基本验证
                    if (formField.ValidateTypeOfEnum != ValidateTypeEnum.No && p.PropertyType == typeof(String))
                    {
                        //基本验证
                        switch (formField.ValidateTypeOfEnum)
                        {
                            case ValidateTypeEnum.email:
                                {
                                    bool rs = Validator.IsEmail(value.ObjToStr());
                                    if (!rs) return string.Format("【{0}】字段值无效，请输入正确的邮箱地址！", fieldDisplay);
                                }
                                break;
                            case ValidateTypeEnum.idCard: //身份证验证
                                {
                                    bool rs = Validator.IsIDCard(value.ObjToStr());
                                    if (!rs) return string.Format("【{0}】字段值无效，请输入正确的身份证号码！", fieldDisplay);
                                }
                                break;
                            case ValidateTypeEnum.intNum:
                                {
                                    bool rs = Validator.IsInteger(value.ObjToStr());
                                    if (!rs) return string.Format("【{0}】字段值无效，只能输入整数值！", fieldDisplay);
                                }
                                break;
                            case ValidateTypeEnum.floatNum:
                                {
                                    bool rs = Validator.IsNumber(value.ObjToStr());
                                    if (!rs) return string.Format("【{0}】字段值无效，只能输入数值！", fieldDisplay);
                                }
                                break;
                            case ValidateTypeEnum.faxno:
                            case ValidateTypeEnum.phone:
                                {
                                    bool rs = Validator.IsTelePhoneNumber(value.ObjToStr());
                                    if (!rs) return string.Format("【{0}】字段值无效，请输入正确的固定电话号码！", fieldDisplay);
                                }
                                break;
                            case ValidateTypeEnum.mobile:
                                {
                                    bool rs = Validator.IsMobilePhoneNumber(value.ObjToStr());
                                    if (!rs) return string.Format("【{0}】字段值无效，请输入正确的手机号码！", fieldDisplay);
                                }
                                break;
                            case ValidateTypeEnum.qq:
                                {
                                    bool rs = Validator.IsIntegerPositive(value.ObjToStr());
                                    if (!rs) return string.Format("【{0}】字段值无效，请输入正确的QQ号码！", fieldDisplay);
                                }
                                break;
                            case ValidateTypeEnum.url:
                                {
                                    bool rs = Validator.IsURL(value.ObjToStr());
                                    if (!rs) return string.Format("【{0}】字段值无效，请输入正确的URL！", fieldDisplay);
                                }
                                break;
                            case ValidateTypeEnum.zip:
                                {
                                    bool rs = Validator.IsZipCode(value.ObjToStr());
                                    if (!rs) return string.Format("【{0}】字段值无效，请输入正确的邮编！", fieldDisplay);
                                }
                                break;
                        }
                    }
                    #endregion
                    #region 字段唯一性验证
                    if (formField.IsUnique.HasValue && formField.IsUnique.Value) //唯一性验证
                    {
                        List<ConditionItem> conditonItems = new List<ConditionItem>() { new ConditionItem() { Field = field, Method = QueryMethod.Equal, Value = value } };
                        if (id != Guid.Empty) //编辑时排除当前记录
                        {
                            conditonItems.Add(new ConditionItem() { Field = "Id", Method = QueryMethod.NotEqual, Value = id });
                        }
                        object exp = GetQueryCondition(moduleId, conditonItems);
                        string errMsg = string.Empty;
                        object tempModel = GetEntity(moduleId, exp, null, out errMsg);
                        if (tempModel != null)
                        {
                            return string.Format("【{0}】=【{1}】的记录已存在，请不要重复添加！", fieldDisplay, SystemOperate.GetFieldDisplayValue(moduleId, model, formField));
                        }
                    }
                    #endregion
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 数据导入检查
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="userId">当前用户Id</param>
        /// <param name="importFile">导入文件（绝对路径）</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns>验证通过返回封装好的实体集合，否则返回空</returns>
        public static IEnumerable ImportCheck(Guid moduleId, Guid userId, string importFile, out string errMsg)
        {
            Type modelType = CommonOperate.GetModelType(moduleId);
            try
            {
                var fs = new FileStream(importFile, System.IO.FileMode.Open);
                DataTable dt = NPOI_ExcelHelper.RenderFromExcel(fs);
                List<string> fieldNames = dt != null && dt.Columns.Count > 0 ? dt.Columns.Cast<DataColumn>().Select(x => x.ColumnName).ToList() : new List<string>();
                List<PropertyInfo> ps = modelType.GetProperties().Where(x => !fieldNames.Contains(x.Name)).ToList(); //需要赋值的属性
                //填充实体集合
                Func<object, string> action = (object model) =>
                {
                    string err = FluentVerify(moduleId, model); //FluentValidation验证
                    if (string.IsNullOrEmpty(err)) //验证通过
                    {
                        object existModel = null;
                        //主键字段验证
                        err = PrimaryKeyVerify(moduleId, model, out existModel);
                        if (string.IsNullOrEmpty(err)) //验证通过
                        {
                            //表单验证
                            Sys_Form form = SystemOperate.GetUserForm(userId, moduleId); //获取当前用户表单
                            List<Sys_FormField> formFields = SystemOperate.GetFormField(form.Id);
                            List<string> verifyFields = new List<string>(); //导入验证字段
                            foreach (Sys_FormField field in formFields)
                            {
                                if (field.Sys_FieldName == "Id" || field.IsVisible != true ||
                                   field.IsDeleted || field.IsAllowAdd != true)
                                    continue;
                                PropertyInfo p = modelType.GetProperty(field.Sys_FieldName);
                                if (p == null) continue;
                                verifyFields.Add(field.Sys_FieldName);
                            }
                            err = FormFieldVerify(moduleId, model, verifyFields);
                            if (!string.IsNullOrEmpty(err))
                            {
                                return err;
                            }
                        }
                        else //主键数据已存在
                        {
                            //把model中未赋值的字段赋值
                            if (existModel != null)
                            {
                                foreach (PropertyInfo p in ps)
                                {
                                    p.SetValue(model, p.GetValue(existModel, null), null);
                                }
                            }
                        }
                    }
                    else
                    {
                        return err;
                    }
                    return string.Empty;
                };
                IEnumerable models = ToolOperate.FillModels(moduleId, dt, out errMsg, action);
                if (string.IsNullOrEmpty(errMsg))
                    return models;
            }
            catch (Exception ex)
            {
                errMsg = string.Format("异常：{0}", ex.Message);
            }
            return null;
        }

        #endregion

        #region 网格数据加载

        /// <summary>
        /// 获取客户端数据过滤条件
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, string> GetConditionDic(string condition)
        {
            Dictionary<string, string> clientCondition = null;
            if (!string.IsNullOrEmpty(condition))
            {
                try
                {
                    clientCondition = JsonHelper.Deserialize<Dictionary<string, string>>(condition);
                }
                catch
                {
                    condition = condition.Replace("{", "").Replace("}", "");
                    var arr = condition.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (arr.Count() > 0)
                    {
                        foreach (var item in arr)
                        {
                            var val = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                            if (val.Count() == 2)
                            {
                                if (clientCondition == null) 
                                    clientCondition = new Dictionary<string, string>();
                                if (!clientCondition.ContainsKey(val[0]))
                                {
                                    clientCondition.Add(val[0], val[1]);
                                }
                            }
                        }
                    }
                }
                if (clientCondition != null && clientCondition.Count > 0)
                {
                    foreach(string key in clientCondition.Keys)
                    {
                        string value = clientCondition[key];
                        if (value == "currEmp") //当前员工
                        {
                            clientCondition[key] = UserInfo.CurrentUserInfo.EmpId.ObjToStr();
                        }
                    }
                }
            }
            return clientCondition;
        }

        /// <summary>
        /// 获取网格数据加载条件
        /// </summary>
        /// <param name="gridDataParmas">网格数据加载参数</param>
        /// <param name="where">返回的where语句</param>
        /// <returns></returns>
        public static object GetGridCondition(GridDataParmas gridDataParmas, out string where)
        {
            where = string.Empty;
            if (gridDataParmas == null) return null;
            object conditionExpression = null; //最终条件
            //搜索条件
            Dictionary<string, string> searchDic = new Dictionary<string, string>();
            string q = gridDataParmas.Q; //搜索关键字
            string condition = gridDataParmas.Condition; //过滤条件
            //自定义条件反序列化
            Dictionary<string, string> customerCondition = new Dictionary<string, string>();
            try
            {
                if (gridDataParmas.GridType == DataGridType.FlowGrid)
                {
                    customerCondition.Add("Id", Guid.Empty.ToString());
                }
                customerCondition = GetConditionDic(condition);
            }
            catch { }
            //条件语句
            string whereSql = string.Empty;
            Type type = gridDataParmas.GetType(); //真实参数类型
            if (type == typeof(DialogGridDataParams)) //弹出网格参数类型
            {
                DialogGridDataParams dialogDataParams = gridDataParmas as DialogGridDataParams;
                searchDic = JsonHelper.Deserialize<Dictionary<string, string>>(q);
                //将自定义条件和搜索条件转成lamda表达式
                conditionExpression = GetGridFilterCondition(ref whereSql, gridDataParmas.ModuleId, searchDic, DataGridType.DialogGrid, customerCondition, dialogDataParams.InitModule, dialogDataParams.InitField, gridDataParmas.OtherParams, gridDataParmas.ViewId);
                //弹出框时
                object formFieldConditionExp = SystemOperate.GetFormFieldFilterCondition(ref whereSql, dialogDataParams.InitModule, dialogDataParams.InitField, gridDataParmas.ModuleId, dialogDataParams.RelyFieldsValue);
                //合并条件
                conditionExpression = ConditionMerge(gridDataParmas.ModuleId, conditionExpression, formFieldConditionExp);
            }
            else
            {
                if (type == typeof(AutoCompelteDataParams)) //自动完成数据参数类型
                {
                    AutoCompelteDataParams autoDataParams = gridDataParmas as AutoCompelteDataParams;
                    q = StringHelper.GetExpressionReplaceString(q);
                    searchDic.Add(autoDataParams.FieldName, q);
                }
                else //默认网格参数类型
                {
                    searchDic = JsonHelper.Deserialize<Dictionary<string, string>>(q);
                }
                //将自定义条件和搜索条件转成lamda表达式
                conditionExpression = GetGridFilterCondition(ref whereSql, gridDataParmas.ModuleId, searchDic, gridDataParmas.GridType, customerCondition, null, null, gridDataParmas.OtherParams, gridDataParmas.ViewId);
            }
            //复杂过滤条件
            if (gridDataParmas.CdItems != null && gridDataParmas.CdItems.Count > 0)
            {
                string tempWhere = GetWhereSqlByCondition(gridDataParmas.ModuleId, gridDataParmas.CdItems);
                if (!string.IsNullOrEmpty(tempWhere))
                {
                    if (string.IsNullOrEmpty(whereSql))
                        whereSql = tempWhere;
                    else
                        whereSql += string.Format(" AND {0}", tempWhere);
                }
            }
            //where语句条件
            if (!string.IsNullOrEmpty(gridDataParmas.WhereCon)) //存在where语句过滤
            {
                if (!string.IsNullOrEmpty(whereSql)) whereSql += " AND ";
                whereSql += gridDataParmas.WhereCon;
            }
            //网格过滤规则
            if (gridDataParmas.FilterRules != null && gridDataParmas.FilterRules.Count > 0)
            {
                string tempWhere = GetWhereSqlByCondition(gridDataParmas.ModuleId, gridDataParmas.FilterRules, gridDataParmas.ViewId);
                if (!string.IsNullOrEmpty(tempWhere))
                {
                    if (string.IsNullOrEmpty(whereSql))
                        whereSql = tempWhere;
                    else
                        whereSql += string.Format(" AND {0}", tempWhere);
                }
            }
            where = whereSql;
            return conditionExpression;
        }

        /// <summary>
        /// 通用网格数据加载
        /// </summary>
        /// <param name="gridDataParmas">网格数据参数</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        public static object GetGridData(GridDataParmas gridDataParmas, out long total)
        {
            total = 0; //初始化总记录数为0
            if (gridDataParmas == null) return null;
            if (gridDataParmas.ViewId.HasValue && gridDataParmas.ViewId.Value != Guid.Empty)
            {
                return GetGridDataBySql(gridDataParmas, out total);
            }
            List<string> allowCopyFields = new List<string>(); //允许复制的字段
            if (gridDataParmas.IsDetailCopy) //明细复制时
            {
                //清除Id的值，清除不允许复制的字段的值
                Sys_Form form = SystemOperate.GetUserForm(UserInfo.CurrentUserInfo.UserId, gridDataParmas.ModuleId);
                List<Sys_FormField> formFields = SystemOperate.GetFormField(form.Id);
                allowCopyFields = formFields.Where(x => x.IsAllowAdd.HasValue && x.IsAllowAdd.Value && x.IsAllowCopy.HasValue && x.IsAllowCopy.Value).Select(x => x.Sys_FieldName).ToList();
                if (allowCopyFields.Count == 0) return null;
            }
            string errMsg = string.Empty; //异常信息
            string whereSql = string.Empty; //条件语句
            object conditionExpression = GetGridCondition(gridDataParmas, out whereSql); //条件表达式
            //取数据
            object list = GetPageEntities(out errMsg, gridDataParmas.ModuleId, gridDataParmas.PagingInfo, true, conditionExpression, whereSql);
            if ((gridDataParmas.IsDetailCopy && allowCopyFields.Count > 0)) //明细复制并且有复制字段时或为树型网格时
            {
                Type modelType = GetModelType(gridDataParmas.ModuleId);
                PropertyInfo[] ps = modelType.GetProperties();
                foreach (object obj in (list as IEnumerable))
                {
                    foreach (PropertyInfo p in ps)
                    {
                        object value = p.GetValue(obj, null);
                        if (value == null || allowCopyFields.Contains(p.Name)) continue;
                        p.SetValue(obj, TypeUtil.ChangeType(null, p.PropertyType), null);
                    }
                }
            }
            total = gridDataParmas.PagingInfo.totalCount;
            return list;
        }

        /// <summary>
        /// 通用网格数据加载，SQL方式
        /// </summary>
        /// <param name="gridDataParmas">网格数据参数</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        public static object GetGridDataBySql(GridDataParmas gridDataParmas, out long total)
        {
            total = 0;
            if (gridDataParmas == null || !gridDataParmas.ViewId.HasValue) return null;
            #region 查询字段构造和构建匿名对象
            List<Sys_GridField> gridFields = SystemOperate.GetGridFields(gridDataParmas.ViewId.Value, true);
            if (gridFields.Count == 0) return null;
            //构造匿名对象
            Type t = ClassHelper.BuildType("TempClass");
            //查询字段集合，如果存在明细模块，只存储明细模块字段，否则存储主模块字段
            List<string> fns = new List<string>();
            //所有查询字段
            Dictionary<string, Sys_Field> queryFields = new Dictionary<string, Sys_Field>();
            Guid moduleId = gridDataParmas.ModuleId;
            Guid initModuleId = moduleId; //原始模块Id
            Type initModelType = GetModelType(initModuleId); //原始模块类型对象
            foreach (Sys_GridField gridField in gridFields)
            {
                if (!gridField.Sys_FieldId.HasValue) continue;
                Sys_Field sysField = SystemOperate.GetFieldById(gridField.Sys_FieldId.Value);
                if (sysField == null || !sysField.Sys_ModuleId.HasValue) continue;
                Type fieldType = SystemOperate.IsForeignNameField(sysField.Sys_ModuleId.Value, gridField.Sys_FieldName) ? typeof(String) :
                                SystemOperate.GetFieldType(sysField.Sys_ModuleId.Value, sysField.Name);
                if (fieldType == null) continue;
                if (initModuleId != sysField.Sys_ModuleId.Value && SystemOperate.GetParentModuleId(sysField.Sys_ModuleId.Value) == initModuleId) //带明细模块字段时，以明细模块表为主表
                {
                    moduleId = sysField.Sys_ModuleId.Value;
                    fns.Add(gridField.Sys_FieldName);
                }
                else if (sysField.Sys_ModuleId.Value == initModuleId && !gridDataParmas.IsComprehensiveDetailView) //本模块字段，非综合明细视图
                {
                    fns.Add(gridField.Sys_FieldName);
                }
                queryFields.Add(gridField.Sys_FieldName, sysField);
                //先定义两个属性。
                List<ClassHelper.CustPropertyInfo> lcpi = new List<ClassHelper.CustPropertyInfo>();
                ClassHelper.CustPropertyInfo cpi;
                cpi = new ClassHelper.CustPropertyInfo(fieldType.ToString(), gridField.Sys_FieldName);
                lcpi.Add(cpi);
                //再加入上面定义的两个属性到我们生成的类t。
                t = ClassHelper.AddProperty(t, lcpi);
            }
            if (fns.Count == 0) return null;
            gridDataParmas.ModuleId = moduleId;
            string tableName = SystemOperate.GetModuleTableNameById(gridDataParmas.ModuleId); //表名
            #endregion
            #region 查询条件构造
            string whereSql = string.Empty; //条件语句
            object conditionExpression = GetGridCondition(gridDataParmas, out whereSql); //条件表达式
            if (conditionExpression != null)
            {
                DbLinkArgs dbLinkArgs = ModelConfigHelper.GetLocalDbLinkArgs();
                //将条件表达式转化成条件语句
                string tempWhere = ExpressionConditionToWhereSql(gridDataParmas.ModuleId, conditionExpression, dbLinkArgs.DbType);
                if (!string.IsNullOrWhiteSpace(tempWhere))
                {
                    whereSql = string.IsNullOrEmpty(whereSql) ? tempWhere : string.Format("{0} AND {1}", whereSql, tempWhere);
                }
            }
            #endregion
            #region 取数据并处理
            string errMsg = string.Empty;
            DataTable dt = PagingQueryByProcedure(out errMsg, out total, tableName, "*", whereSql, gridDataParmas.PagingInfo);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<object> objList = new List<object>(); //数据容器
                foreach (DataRow dr in dt.Rows)
                {
                    object o = ClassHelper.CreateInstance(t);
                    foreach (string fieldName in queryFields.Keys)
                    {
                        Sys_Field sysField = queryFields[fieldName];
                        if (fns.Contains(fieldName)) //当前模块或明细模块
                        {
                            object value = string.Empty;
                            if (dt.Columns.Contains(fieldName))
                            {
                                value = dr[fieldName];
                            }
                            else if (!string.IsNullOrEmpty(sysField.ForeignModuleName) && fieldName.Length > 4 && fieldName.Substring(0, fieldName.Length - 4) + "Id" == sysField.Name) //外键Name字段
                            {
                                Guid id = dr[sysField.Name].ObjToGuid(); //外键Id
                                Sys_Module foreignModule = SystemOperate.GetForeignModule(sysField);
                                if (foreignModule != null && !string.IsNullOrWhiteSpace(foreignModule.TitleKey))
                                {
                                    object obj = GetEntityById(foreignModule.Id, id, out errMsg);
                                    Type foreignModeType = GetModelType(foreignModule.Id);
                                    PropertyInfo fp = foreignModeType.GetProperty(foreignModule.TitleKey);
                                    if (fp != null)
                                    {
                                        value = fp.GetValue(obj, null);
                                    }
                                }
                            }
                            ClassHelper.SetPropertyValue(o, fieldName, value);
                        }
                        else if (sysField.Sys_ModuleId.Value == initModuleId) //当前是主模块
                        {
                            Guid id = dr[initModelType.Name + "Id"].ObjToGuid();
                            object obj = GetEntityById(initModuleId, id, out errMsg);
                            if (obj != null)
                            {
                                PropertyInfo p = initModelType.GetProperty(fieldName);
                                ClassHelper.SetPropertyValue(o, fieldName, p.GetValue(obj, null));
                            }
                        }
                        else //主模块的外键模块
                        {
                            Type foreignModeType = GetModelType(sysField.Sys_ModuleId.Value);
                            Guid foreignId = Guid.Empty;
                            if (initModuleId != gridDataParmas.ModuleId) //有明细模块
                            {
                                Guid id = dr[initModelType.Name + "Id"].ObjToGuid(); //主模块记录Id
                                object obj = GetEntityById(initModuleId, id, out errMsg); //主模块记录
                                if (obj != null)
                                {
                                    PropertyInfo p = initModelType.GetProperty(foreignModeType.Name + "Id");
                                    foreignId = p.GetValue(obj, null).ObjToGuid(); //主模块对应的外键模块记录Id
                                }
                            }
                            else
                            {
                                foreignId = dr[foreignModeType.Name + "Id"].ObjToGuid();
                            }
                            if (foreignId != Guid.Empty)
                            {
                                object foreignObj = GetEntityById(sysField.Sys_ModuleId.Value, foreignId, out errMsg); //外键模块记录
                                if (foreignObj != null)
                                {
                                    PropertyInfo fp = foreignModeType.GetProperty(fieldName);
                                    ClassHelper.SetPropertyValue(o, fieldName, fp.GetValue(foreignObj, null));
                                }
                            }
                        }
                    }
                    objList.Add(o);
                }
                return objList;
            }
            return null;
            #endregion
        }

        #endregion

        #region 查询条件

        /// <summary>
        /// 获取Lamda表达式的查询条件
        /// </summary>
        /// <param name="conditionItems">条件集合</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetQueryCondition<T>(List<ConditionItem> conditionItems) where T : class
        {
            return new TempOperate<T>().GetQueryCondition(conditionItems);
        }

        /// <summary>
        /// 获取Lamda表达式的查询条件
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="conditionItems">条件集合</param>
        /// <returns></returns>
        public static object GetQueryCondition(Guid moduleId, List<ConditionItem> conditionItems)
        {
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(moduleId, "GetQueryCondition", new object[] { conditionItems });
            return obj;
        }

        /// <summary>
        /// 获取查询条件
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="dicCondition">条件集合</param>
        /// <returns></returns>
        public static object GetQueryCondition(Guid moduleId, Dictionary<string, string> dicCondition)
        {
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(moduleId, "GetQueryConditionDic", new object[] { dicCondition });
            return obj;
        }

        /// <summary>
        /// 获取Lamda表达式的查询条件
        /// </summary>
        /// <param name="moduleName">模块Id</param>
        /// <param name="conditionItems">条件集合</param>
        /// <returns></returns>
        public static object GetQueryCondition(string moduleName, List<ConditionItem> conditionItems)
        {
            string errMsg = string.Empty;
            Sys_Module module = SystemOperate.GetModuleByName(moduleName);
            if (module != null)
            {
                return GetQueryCondition(module.Id, conditionItems);
            }
            return null;
        }

        /// <summary>
        /// 条件表达式合并，返回合并后的表达式
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="expression1">lamda表达式１</param>
        /// <param name="expression2">lamda表达式２</param>
        /// <param name="isAnd">是否And合并</param>
        /// <returns></returns>
        public static object ConditionMerge(Guid moduleId, object expression1, object expression2, bool isAnd = true)
        {
            //反射取数据
            object obj = ExecuteTempOperateReflectMethod(moduleId, "ConditionMerge", new object[] { expression1, expression2, isAnd });
            return obj;
        }

        /// <summary>
        /// 返回网格过滤条件
        /// </summary>
        /// <param name="whereSql">sql条件语句</param>
        /// <param name="q">搜索条件</param>
        /// <param name="gridType">网格类型</param>
        /// <param name="condition">条件参数</param>
        /// <param name="initModule">原始模块（弹出选择外键模块数据的初始模块），弹出列表用到</param>
        /// <param name="initField">原始字段（弹出选择外键模块数据的初始字段），弹出列表用到</param>
        /// <param name="otherParams">其他参数</param>
        /// <param name="viewId">当前网格视图Id</param>
        /// <returns>返回条件表达式</returns>
        public static Expression<Func<T, bool>> GetGridFilterCondition<T>(ref string whereSql, Dictionary<string, string> q, DataGridType gridType, Dictionary<string, string> condition = null, string initModule = null, string initField = null, Dictionary<string, string> otherParams = null, Guid? viewId = null) where T : class
        {
            return new TempOperate<T>().GetGridFilterCondition(ref whereSql, q, gridType, condition, initModule, initField, otherParams);
        }

        /// <summary>
        /// 返回网格过滤条件
        /// </summary>
        /// <param name="whereSql">sql条件语句</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="q">搜索条件参数</param>
        /// <param name="gridType">网格类型</param>
        /// <param name="condition">条件参数</param>
        /// <param name="initModule">原始模块（弹出选择外键模块数据的初始模块），弹出列表用到</param>
        /// <param name="initField">原始字段（弹出选择外键模块数据的初始字段），弹出列表用到</param>
        /// <param name="otherParams">其他参数</param>
        /// <param name="viewId">当前网格视图Id</param>
        /// <returns>返回条件表达式</returns>
        public static object GetGridFilterCondition(ref string whereSql, Guid moduleId, Dictionary<string, string> q, DataGridType gridType, Dictionary<string, string> condition = null, string initModule = null, string initField = null, Dictionary<string, string> otherParams = null, Guid? viewId = null)
        {
            //反射取数据
            object[] args = new object[] { whereSql, q, gridType, condition, initModule, initField, otherParams, viewId };
            object obj = ExecuteTempOperateReflectMethod(moduleId, "GetGridFilterCondition", args);
            whereSql = args[0].ObjToStr();
            return obj;
        }

        /// <summary>
        /// 根据条件集合生成
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="items">条件集合</param>
        /// <param name="viewId">综合视图时，视图Id</param>
        /// <returns></returns>
        public static string GetWhereSqlByCondition(Guid moduleId, List<ConditionItem> items, Guid? viewId = null)
        {
            //反射取数据
            object[] args = new object[] { items, viewId };
            object obj = ExecuteTempOperateReflectMethod(moduleId, "GetWhereSqlByCondition", args);
            return obj.ObjToStr();
        }

        /// <summary>
        /// 将条件表达式转成where Sql语句
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="expression">条件表达式</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static string ExpressionConditionToWhereSql(Guid moduleId, object expression, DatabaseType? dbType = null)
        {
            //反射取数据
            object[] args = new object[] { expression, dbType };
            object obj = ExecuteTempOperateReflectMethod(moduleId, "ExpressionConditionToWhereSql", args);
            return obj.ObjToStr();
        }
        #endregion

        #region 其他

        /// <summary>
        /// 获取表单按钮
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="formType">表单类型</param>
        /// <param name="buttons">原有按钮</param>
        /// <param name="isAdd">是否新增页面</param>
        /// <param name="isDraft">是否草稿</param>
        /// <returns></returns>
        internal static List<FormButton> GetFormButtons(Guid moduleId, FormTypeEnum formType, List<FormButton> buttons, bool isAdd = false, bool isDraft = false)
        {
            //反射取数据
            object obj = ExecuteCustomeOperateHandleMethod(moduleId, "GetFormButtons", new object[] { formType, buttons, isAdd, isDraft });
            return obj as List<FormButton>;
        }

        /// <summary>
        /// 获取表单工具标签按钮集合
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="formType">表单类型</param>
        /// <param name="tags">原有标签</param>
        /// <param name="isAdd">是否新增页面</param>
        /// <returns></returns>
        internal static List<FormToolTag> GetFormToolTags(Guid moduleId, FormTypeEnum formType, List<FormToolTag> tags, bool isAdd = false)
        {
            //反射取数据
            object obj = ExecuteCustomeOperateHandleMethod(moduleId, "GetFormToolTags", new object[] { formType, tags, isAdd });
            return obj as List<FormToolTag>;
        }

        /// <summary>
        /// 视图按钮操作前验证
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="buttonText">按钮名称</param>
        /// <param name="ids">操作记录Id集合</param>
        /// <returns></returns>
        public static string GridOperateVerify(Guid moduleId, string buttonText, List<Guid> ids)
        {
            if (!PermissionOperate.HasButtonPermission(UserInfo.CurrentUserInfo.UserId, moduleId, buttonText))
            {
                return string.Format("您没有该模块的【{0}】权限，如有疑问请联系管理员！", buttonText);
            }
            if (buttonText == "编辑" || buttonText == "删除")
            {
                DataPermissionTypeEnum dataPermissionType = buttonText == "编辑" ? DataPermissionTypeEnum.EditData : DataPermissionTypeEnum.DelData;
                StringBuilder sb = new StringBuilder();
                string titleKeyDisplay = SystemOperate.GetModuleTitleKeyDisplay(moduleId);
                foreach (Guid id in ids)
                {
                    if (!PermissionOperate.UserHasOperateRecordPermission(UserInfo.CurrentUserInfo.UserId, moduleId, id, dataPermissionType))
                    {
                        string titleKeyValue = CommonOperate.GetModelTitleKeyValue(moduleId, id);
                        string msg = string.Format("您没有【{0}】=【{1}】对应记录的{2}权限", titleKeyDisplay, titleKeyValue, buttonText);
                        sb.AppendLine(msg);
                    }
                }
                if (sb.ToString().Length > 0)
                {
                    return sb.ToString();
                }
            }
            //反射取数据
            object obj = ExecuteCustomeOperateHandleMethod(moduleId, "GridOperateVerify", new object[] { buttonText, ids });
            return obj.ObjToStr();
        }

        /// <summary>
        /// 公共属性赋值，创建人、创建时间、修改人、修改时间等字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="model">实体对象</param>
        /// <param name="isAdd">是否新增</param>
        /// <returns></returns>
        public static void ModelCommonPropertySetValue(Guid moduleId, object model, bool isAdd)
        {
            Type modelType = GetModelType(moduleId);
            UserInfo currentUser = UserInfo.CurrentUserInfo;
            if (currentUser != null)
            {
                //修改人、修改日期赋值
                PropertyInfo pModifyUser = modelType.GetProperty("ModifyUserId");
                PropertyInfo pModifyUserName = modelType.GetProperty("ModifyUserName");
                PropertyInfo pModifyDate = modelType.GetProperty("ModifyDate");
                string userAliasName = UserInfo.GetUserAliasName();
                pModifyUser.SetValue(model, currentUser.UserId, null);
                pModifyUserName.SetValue(model, userAliasName, null);
                pModifyDate.SetValue(model, DateTime.Now, null);
                if (currentUser.OrganizationId.HasValue) //给组织Id赋值
                {
                    PropertyInfo pOrgId = modelType.GetProperty("OrgId"); //对象中原有组织Id
                    object oldOrgIdObj = pOrgId.GetValue(model, null);
                    if (oldOrgIdObj == null) //原组织Id为空时对象的组织Id取当前用户的组织Id
                    {
                        pOrgId.SetValue(model, currentUser.OrganizationId.Value, null);
                    }
                }
                if (isAdd) //添加
                {
                    //添加人、添加日期斌值
                    PropertyInfo pCreateUser = modelType.GetProperty("CreateUserId");
                    PropertyInfo pCreateUserName = modelType.GetProperty("CreateUserName");
                    PropertyInfo pCreateDate = modelType.GetProperty("CreateDate");
                    pCreateUser.SetValue(model, currentUser.UserId, null);
                    pCreateUserName.SetValue(model, userAliasName, null);
                    pCreateDate.SetValue(model, DateTime.Now, null);
                }
            }
        }

        /// <summary>
        /// 获取网格参数
        /// </summary>
        /// <param name="request">请求对象</param>
        /// <returns></returns>
        public static GridDataParmas GetGridDataParams(HttpRequestBase request)
        {
            //模块信息
            Sys_Module module = SystemOperate.GetModuleByRequest(request);
            if (module == null) return null;
            //分页信息
            PageInfo pageInfo = PageInfo.GetPageInfo(request);
            List<string> sortNames = pageInfo.sortname.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            if (sortNames != null && sortNames.Count > 0)
            {
                List<string> tempSortNames = new List<string>();
                foreach (string sortName in sortNames)
                {
                    if (SystemOperate.IsForeignNameField(module.Id, sortName))
                        tempSortNames.Add(sortName.Substring(0, sortName.Length - 4) + "Id");
                    else
                        tempSortNames.Add(sortName);
                }
                pageInfo.sortname = string.Join(",", tempSortNames);
            }
            //搜索条件
            string q = request["q"].ObjToStr().Trim();
            //自定义条件
            string condition = HttpUtility.UrlDecode(request["condition"].ObjToStr());
            //复杂条件集合
            string cdItemStr = HttpUtility.UrlDecode(request["cdItems"]).ObjToStr();
            List<ConditionItem> cdItems = new List<ConditionItem>();
            if (!string.IsNullOrEmpty(cdItemStr))
            {
                try
                {
                    cdItems = JsonHelper.Deserialize<List<ConditionItem>>(cdItemStr);
                }
                catch { }
            }
            //where条件语句，用Base64加密后传输
            string whereCon = string.Empty;
            try
            {
                string tempWhere = HttpUtility.UrlDecode(request["where"].ObjToStr());
                if (!string.IsNullOrWhiteSpace(tempWhere))
                {
                    whereCon = MySecurity.DecodeBase64(tempWhere);
                }
            }
            catch
            { }
            //弹出框的原始模块
            string initModule = HttpUtility.UrlDecode(request["initModule"].ObjToStr());
            //弹出框的原始字段
            string initField = request["initField"].ObjToStr();
            //弹出框的依赖字段值
            string relyFieldsValue = HttpUtility.UrlDecode(request["p_relyValues"]);
            //自动完成字段名
            string fieldName = request["fieldName"].ObjToStr(); //字段名
            //组装参数对象
            GridDataParmas gridParams = null;
            if (!string.IsNullOrWhiteSpace(initModule) && !string.IsNullOrWhiteSpace(initField)) //弹出框网格数据参数
            {
                gridParams = new DialogGridDataParams(module.Id, initModule, initField, pageInfo, q, condition, relyFieldsValue, cdItems, whereCon);
            }
            else if (!string.IsNullOrWhiteSpace(fieldName)) //自动完成数据参数
            {
                gridParams = new AutoCompelteDataParams(module.Id, fieldName, pageInfo, q, condition, cdItems, whereCon);
            }
            else
            {
                gridParams = new GridDataParmas(module.Id, pageInfo, q, condition, cdItems, whereCon);
            }
            try
            {
                string gt = request["gt"].ObjToStr();
                gridParams.GridType = (DataGridType)Enum.Parse(typeof(DataGridType), gt);
            }
            catch { }
            //其他参数
            Dictionary<string, object> dic = request.Params.ToDictionary().Where(x => x.Key.StartsWith("p_")).ToDictionary();
            if (dic != null && dic.Count > 0)
            {
                gridParams.OtherParams = new Dictionary<string, string>();
                foreach (string key in dic.Keys)
                {
                    gridParams.OtherParams.Add(key, dic[key].ObjToStr());
                }
            }
            //行过滤规则解析
            string filterRules = request["filterRules"].ObjToStr();
            if (!string.IsNullOrEmpty(filterRules))
            {
                try
                {
                    List<GridFilterRule> gridFilters = JsonHelper.Deserialize<List<GridFilterRule>>(filterRules);
                    List<ConditionItem> ruleItems = new List<ConditionItem>();
                    foreach (GridFilterRule rule in gridFilters)
                    {
                        QueryMethod method = QueryMethod.Equal;
                        string field = rule.field;
                        if (rule.op == FilterOpEnum.isnull || rule.op == FilterOpEnum.isnotnull)
                        {
                            method = rule.op == FilterOpEnum.isnull ? QueryMethod.Equal : QueryMethod.NotEqual;
                        }
                        else
                        {
                            method = (QueryMethod)Enum.Parse(typeof(QueryMethod), ((int)rule.op).ToString());
                        }
                        object value = rule.value;
                        if (rule.op == FilterOpEnum.isnull || rule.op == FilterOpEnum.isnotnull)
                        {
                            value = null;
                        }
                        if (SystemOperate.IsForeignNameField(module.Id, field))
                            field = field.Substring(0, field.Length - 4) + "Id";
                        ruleItems.Add(new ConditionItem() { Field = field, Method = method, Value = value });
                    }
                    gridParams.FilterRules = ruleItems;
                }
                catch { }
            }
            gridParams.ViewId = request["viewId"].ObjToGuidNull(); //综合视图Id
            gridParams.IsComprehensiveDetailView = request["dv"].ObjToInt() == 1; //综合明细视图
            gridParams.IsDetailCopy = request["copy"].ObjToInt() == 1; //是否明细复制
            gridParams.IsTreeGrid = request["tg"].ObjToInt() == 1; //是否为树型网格
            return gridParams;
        }

        /// <summary>
        /// 获取表单数据
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="id">记录Id</param>
        /// <param name="formType">表单类型</param>
        /// <returns></returns>
        public static object GetFormData(Guid moduleId, Guid id, FormTypeEnum formType)
        {
            string errMsg = string.Empty;
            object[] args = new object[] { id, formType, errMsg };
            object model = ExecuteTempOperateReflectMethod(moduleId, "GetFormData", args);
            errMsg = args[2].ObjToStr();
            return model;
        }

        #endregion

        #region 数据库操作

        /// <summary>
        /// 创建数据表
        /// </summary>
        /// <param name="modelType">实体类型对象</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>创建成功返回空字符串，失败返回异常信息</returns>
        public static string CreateTable(Type modelType, string connString = null, DatabaseType? dbType = null)
        {
            object obj = ExecuteTempOperateReflectMethod(modelType, "CreateTable", new object[] { connString, dbType });
            return obj.ObjToStr();
        }

        /// <summary>
        /// 删除数据表
        /// </summary>
        /// <param name="modelType">实体类型对象</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>删除成功返回空字符串，失败返回异常信息</returns>
        public static string DropTable(Type modelType, string connString = null, DatabaseType? dbType = null)
        {
            object obj = ExecuteTempOperateReflectMethod(modelType, "DropTable", new object[] { connString, dbType });
            return obj.ObjToStr();
        }

        /// <summary>
        /// 修改数据表
        /// </summary>
        /// <param name="modelType">实体类型对象</param>
        /// <param name="command">操作sql，如：[ALTER TABLE a] ADD Column b int</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>成功返回空字符串，失败返回异常信息</returns>
        public static string AlterTable(Type modelType, string command, string connString = null, DatabaseType? dbType = null)
        {
            object obj = ExecuteTempOperateReflectMethod(modelType, "AlterTable", new object[] { command, connString, dbType });
            return obj.ObjToStr();
        }

        /// <summary>
        /// 数据列是否存在
        /// </summary>
        /// <param name="modelType">实体类型对象</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static bool ColumnIsExists(Type modelType, string fieldName, string connString = null, DatabaseType? dbType = null)
        {
            object obj = ExecuteTempOperateReflectMethod(modelType, "ColumnIsExists", new object[] { fieldName, connString, dbType });
            return obj.ObjToBool();
        }

        /// <summary>
        /// 增加数据列
        /// </summary>
        /// <param name="modelType">实体类型对象</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>成功返回空字符串，失败返回异常信息</returns>
        public static string AddColumn(Type modelType, string fieldName, string connString = null, DatabaseType? dbType = null)
        {
            object obj = ExecuteTempOperateReflectMethod(modelType, "AddColumn", new object[] { fieldName, connString, dbType });
            return obj.ObjToStr();
        }

        /// <summary>
        /// 修改数据列
        /// </summary>
        /// <param name="modelType">实体类型对象</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>成功返回空字符串，失败返回异常信息</returns>
        public static string AlterColumn(Type modelType, string fieldName, string connString = null, DatabaseType? dbType = null)
        {
            object obj = ExecuteTempOperateReflectMethod(modelType, "AlterColumn", new object[] { fieldName, connString, dbType });
            return obj.ObjToStr();
        }

        /// <summary>
        /// 修改列名
        /// </summary>
        /// <param name="modelType">实体类型对象</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="oldFieldName">旧的字段名</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns>成功返回空字符串，失败返回异常信息</returns>
        public static string ChangeColumnName(Type modelType, string fieldName, string oldFieldName, string connString = null, DatabaseType? dbType = null)
        {
            object obj = ExecuteTempOperateReflectMethod(modelType, "ChangeColumnName", new object[] { fieldName, oldFieldName, connString, dbType });
            return obj.ObjToStr();
        }

        /// <summary>
        /// 删除列
        /// </summary>
        /// <param name="modelType">实体类型对象</param>
        /// <param name="columnName">列名</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static string DropColumn(Type modelType, string columnName, string connString = null, DatabaseType? dbType = null)
        {
            object obj = ExecuteTempOperateReflectMethod(modelType, "DropColumn", new object[] { columnName, connString, dbType });
            return obj.ObjToStr();
        }

        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="modelType">实体类型对象</param>
        /// <param name="field">字段,Expression<Func<T, object>> field</param>
        /// <param name="indexName">索引名</param>
        /// <param name="unique">是否唯一索引</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <returns>成功返回空字符串，失败返回异常信息</returns>
        public static string CreateIndex(Type modelType, object field, string indexName = null, bool unique = false, string connString = null, DatabaseType? dbType = null)
        {
            object obj = ExecuteTempOperateReflectMethod(modelType, "CreateIndex", new object[] { field, indexName, unique, connString, dbType });
            return obj.ObjToStr();
        }

        /// <summary>
        /// 删除索引
        /// </summary>
        /// <param name="modelType">实体类型对象</param>
        /// <param name="indexName">索引名称</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="dbType">数据库类型</param>
        /// <returns></returns>
        public static string DropIndex(Type modelType, string indexName, string connString = null, DatabaseType? dbType = null)
        {
            object obj = ExecuteTempOperateReflectMethod(modelType, "DropIndex", new object[] { indexName, connString, dbType });
            return obj.ObjToStr();
        }

        #endregion

        #region 缓存

        /// <summary>
        /// 清除当前模块缓存
        /// </summary>
        public static void ClearCache<T>() where T : class
        {
            new TempOperate<T>().ClearCache();
        }

        /// <summary>
        /// 清除模块缓存
        /// </summary>
        /// <param name="tableName">模块表名</param>
        public static void ClearCache(string tableName)
        {
            ExecuteTempOperateReflectMethod(tableName, "ClearCache", null);
        }

        /// <summary>
        /// 清除模块缓存
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        public static void ClearCache(Guid moduleId)
        {
            ExecuteTempOperateReflectMethod(moduleId, "ClearCache", null);
        }

        /// <summary>
        /// 清除所有缓存，不包含自定义缓存
        /// </summary>
        public static void ClearAllCache()
        {
            IBaseBLL<BaseEntity> bll = BridgeObject.Resolve<IBaseBLL<BaseEntity>>();
            bll.ClearAllCache();
        }

        /// <summary>
        /// 设置自定义缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="data">数据</param>
        public static void SetCustomerCache(string key, object data)
        {
            IBaseBLL<Sys_Module> bll = BridgeObject.Resolve<IBaseBLL<Sys_Module>>();
            bll.SetCustomerCache(key, data);
        }

        /// <summary>
        /// 移除自定义缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        public static void RemoveCustomerCache(string key)
        {
            IBaseBLL<Sys_Module> bll = BridgeObject.Resolve<IBaseBLL<Sys_Module>>();
            bll.RemoveCustomerCache(key);
        }

        #endregion
    }
}
