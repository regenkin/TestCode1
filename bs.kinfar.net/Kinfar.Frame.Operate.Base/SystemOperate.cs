/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // 版权所有
        // 开发者：Kinfar.
        // Email：kinfar@foxmail.net
        // QQ：3133119519
//----------------------------------------------------------------*/

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections;
using Kinfar.Frame.Model.Sys;
using Kinfar.Frame.Common;
using Kinfar.Frame.Model.EnumSpace;
using Kinfar.Frame.Operate.Base.TempModel;
using Kinfar.Frame.Operate.Base.EnumDef;
using System.Text;
using Kinfar.Frame.Common.Model;
using Kinfar.Frame.Base;
using Kinfar.Frame.Common.PubDefine;
using System.IO;
using Kinfar.Frame.Model.Desktop;
using Kinfar.Frame.EntityBase;
using System.Data;

namespace Kinfar.Frame.Operate.Base
{
    /// <summary>
    /// 系统操作类
    /// </summary>
    public static class SystemOperate
    {
        #region 模块

        #region 基本

        #region 基本信息

        /// <summary>
        /// 取模块集合
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<Sys_Module> GetModules(Expression<Func<Sys_Module, bool>> expression = null)
        {
            string errMsg = string.Empty;
            return CommonOperate.GetEntities<Sys_Module>(out errMsg, expression);
        }

        /// <summary>
        /// 通过模块Id获取模块信息
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public static Sys_Module GetModuleById(Guid moduleId)
        {
            string errMsg = string.Empty;
            return CommonOperate.GetEntityById<Sys_Module>(moduleId, out errMsg);
        }

        /// <summary>
        /// 通过模块名称获取模块信息
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Sys_Module GetModuleByName(string moduleName)
        {
            if (string.IsNullOrWhiteSpace(moduleName)) return null;
            string errMsg = string.Empty;
            Sys_Module module = CommonOperate.GetEntity<Sys_Module>(x => x.Name == moduleName, string.Empty, out errMsg);
            return module;
        }

        /// <summary>
        /// 根据表名获取模块
        /// </summary>
        /// <param name="tableName">实体表名</param>
        /// <returns></returns>
        public static Sys_Module GetModuleByTableName(string tableName)
        {
            string errMsg = string.Empty;
            Sys_Module module = CommonOperate.GetEntity<Sys_Module>(x => x.TableName == tableName, string.Empty, out errMsg);
            return module;
        }

        /// <summary>
        /// 根据表名获取模块Id
        /// </summary>
        /// <param name="tableName">实体表名</param>
        /// <returns></returns>
        public static Guid GetModuleIdByTableName(string tableName)
        {
            Sys_Module module = GetModuleByTableName(tableName);
            if (module != null) return module.Id;
            return Guid.Empty;
        }

        /// <summary>
        /// 根据模块名称获取模块Id
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static Guid GetModuleIdByName(string moduleName)
        {
            string errMsg = string.Empty;
            Sys_Module module = CommonOperate.GetEntity<Sys_Module>(x => x.Name == moduleName, string.Empty, out errMsg);
            if (module != null)
            {
                return module.Id;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// 根据模块Id获取模块名称
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static string GetModuleNameById(Guid moduleId)
        {
            string errMsg = string.Empty;
            Sys_Module module = CommonOperate.GetEntityById<Sys_Module>(moduleId, out errMsg);
            if (module != null)
            {
                return module.Name;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取模块表名
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public static string GetModuleTableNameById(Guid moduleId)
        {
            Sys_Module module = GetModuleById(moduleId);
            if (module != null)
            {
                return module.TableName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取模块表名
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static string GetModuleTableNameByName(string moduleName)
        {
            Sys_Module module = GetModuleByName(moduleName);
            if (module != null)
            {
                return module.TableName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="request">request请求</param>
        /// <returns></returns>
        public static Sys_Module GetModuleByRequest(HttpRequestBase request)
        {
            Guid moduleId = request["moduleId"].ObjToGuid();
            string moduleName = HttpUtility.UrlDecode(request["moduleName"].ObjToStr());
            Sys_Module module = moduleId != Guid.Empty ? GetModuleById(moduleId) : GetModuleByName(moduleName);
            if (module == null)
            {
                string tableName = request["tableName"].ObjToStr();
                if (!string.IsNullOrWhiteSpace(tableName))
                {
                    module = GetModuleByTableName(tableName);
                }
            }
            return module;
        }

        /// <summary>
        /// 通过实体类型获取模块
        /// </summary>
        /// <param name="modelType">实体类型</param>
        /// <returns></returns>
        public static Guid GetModuleIdByModelType(Type modelType)
        {
            if (modelType != null)
                return GetModuleIdByTableName(modelType.Name);
            else
                return Guid.Empty;
        }

        #endregion

        /// <summary>
        /// 获取模块的Js文件路径，多个Js文件路径以逗号分隔
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static string GetModuleJsFilePath(Guid moduleId)
        {
            string jsPath = string.Empty;
            Sys_Module module = GetModuleById(moduleId);
            if (module != null)
            {
                if (!string.IsNullOrEmpty(module.OtherJs))
                {
                    jsPath += module.OtherJs;
                }
                if (!string.IsNullOrEmpty(module.StandardJsFolder))
                {
                    jsPath = string.Format("/Scripts/model/{0}/{1}.js", module.StandardJsFolder, module.TableName);
                }
            }
            return jsPath;
        }

        /// <summary>
        /// 获取模块的Js文件路径，多个Js文件路径以逗号分隔
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="folder">所属文件夹</param>
        /// <returns></returns>
        public static string GetModuleJsFilePath(Guid moduleId, string folder)
        {
            if (string.IsNullOrEmpty(folder))
            {
                return GetModuleJsFilePath(moduleId);
            }
            string jsPath = string.Empty;
            Sys_Module module = GetModuleById(moduleId);
            if (module != null)
            {
                if (!string.IsNullOrEmpty(module.OtherJs))
                {
                    jsPath += module.OtherJs;
                }
                if (!string.IsNullOrEmpty(module.StandardJsFolder))
                {
                    jsPath = string.Format("/Scripts/{0}/model/{1}/{2}.js", folder, module.StandardJsFolder, module.TableName);
                }
            }
            return jsPath;
        }

        #region 外键模块

        /// <summary>
        /// 取外键字段的外键模块
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static Sys_Module GetForeignModule(Guid moduleId, string fieldName)
        {
            Sys_Field field = GetFieldInfo(moduleId, fieldName);
            if (field != null)
            {
                Sys_Module module = GetModuleByName(field.ForeignModuleName);
                return module;
            }
            return null;
        }

        /// <summary>
        /// 获取外键模块
        /// </summary>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public static Sys_Module GetForeignModule(Sys_Field field)
        {
            if (field != null)
            {
                Sys_Module module = GetModuleByName(field.ForeignModuleName);
                return module;
            }
            return null;
        }

        /// <summary>
        /// 获取外键模块Id
        /// </summary>
        /// <param name="field">字段</param>
        /// <returns></returns>
        public static Guid GetForeignModuleId(Sys_Field field)
        {
            if (field != null)
            {
                Sys_Module module = GetModuleByName(field.ForeignModuleName);
                return module.Id;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// 获取字段外键模块名称
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns>返回该模块对应的外键模块名称</returns>
        public static string GetForeignModuleName(Guid moduleId, string fieldName)
        {
            Sys_Field field = GetFieldInfo(moduleId, fieldName);
            if (field != null)
            {
                return field.ForeignModuleName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取模块的所有外键模块（除用户管理模块）
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_Module> GetForeignModules(Guid moduleId)
        {
            List<Sys_Field> fields = SystemOperate.GetFieldInfos(moduleId);
            List<string> tempModuleNames = new List<string>();
            foreach (Sys_Field field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.ForeignModuleName))
                    continue;
                if (CommonDefine.BaseEntityFields.Contains(field.Name) || tempModuleNames.Contains(field.ForeignModuleName.Trim()))
                    continue;
                tempModuleNames.Add(field.ForeignModuleName.Trim());
            }
            List<Sys_Module> modules = new List<Sys_Module>();
            foreach (string moduleName in tempModuleNames)
            {
                Sys_Module tempModule = SystemOperate.GetModuleByName(moduleName);
                if (tempModule != null)
                    modules.Add(tempModule);
            }
            return modules;
        }

        /// <summary>
        /// 获取模块的所有外键模块（除用户管理模块）
        /// </summary>
        /// <param name="module">模块对象</param>
        /// <returns></returns>
        public static List<Sys_Module> GetForeignModules(Sys_Module module)
        {
            return GetForeignModules(module.Id);
        }

        /// <summary>
        /// 获取模块的所有字段外键模块不重复的外键模块，
        /// 即模块字段中不允许有两个字段是同一个外键模块
        /// 并且不包含用户管理外键模块
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_Module> GetNoRepeatForeignModules(Guid moduleId)
        {
            List<Sys_Field> fields = SystemOperate.GetFieldInfos(moduleId);
            List<string> tempModuleNames = new List<string>(); //可用外键模块名称集合
            List<string> removeModuleNames = new List<string>(); //需要移除的
            foreach (Sys_Field field in fields)
            {
                if (string.IsNullOrWhiteSpace(field.ForeignModuleName))
                    continue;
                if (CommonDefine.BaseEntityFields.Contains(field.Name))
                    continue;
                if (tempModuleNames.Contains(field.ForeignModuleName.Trim())) //已经包含了该模块
                {
                    if (!removeModuleNames.Contains(field.ForeignModuleName.Trim()))
                    {
                        removeModuleNames.Add(field.ForeignModuleName.Trim()); //添加到移除列表
                    }
                }
                else
                {
                    tempModuleNames.Add(field.ForeignModuleName.Trim());
                }
            }
            //移除重复项
            foreach (string name in removeModuleNames)
            {
                tempModuleNames.Remove(name);
            }
            List<Sys_Module> modules = new List<Sys_Module>();
            foreach (string moduleName in tempModuleNames)
            {
                Sys_Module tempModule = SystemOperate.GetModuleByName(moduleName);
                if (tempModule != null)
                    modules.Add(tempModule);
            }
            return modules;
        }

        #endregion

        #region 模块标记字段

        /// <summary>
        /// 获取模块的TitleKey
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public static string GetModuleTitleKey(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName)) return string.Empty;
            Sys_Module module = GetModuleByName(moduleName);
            if (module != null)
            {
                return module.TitleKey;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取模块的TitleKey
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static string GetModuleTitleKey(Guid moduleId)
        {
            Sys_Module module = GetModuleById(moduleId);
            if (module != null)
            {
                return module.TitleKey;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取模块的TitleKey的显示名称
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static string GetModuleTitleKeyDisplay(Guid moduleId)
        {
            Sys_Module module = GetModuleById(moduleId);
            return GetModuleTitleKeyDisplay(module);
        }

        /// <summary>
        /// 获取模块的TitleKey的显示名称
        /// </summary>
        /// <param name="module">模块对象</param>
        /// <returns></returns>
        public static string GetModuleTitleKeyDisplay(Sys_Module module)
        {
            if (module != null && !string.IsNullOrWhiteSpace(module.TitleKey))
            {
                Sys_Field field = GetFieldInfo(module.Id, module.TitleKey);
                if (field != null)
                {
                    return field.Display;
                }
            }
            return string.Empty;
        }

        #endregion

        #region 模块主键字段

        /// <summary>
        /// 取模块的主键字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<string> GetModulePrimaryKeyFields(Guid moduleId)
        {
            Sys_Module module = GetModuleById(moduleId);
            if (module != null && !string.IsNullOrWhiteSpace(module.PrimaryKeyFields))
            {
                return module.PrimaryKeyFields.Split(",".ToCharArray()).ToList();
            }
            return new List<string>();
        }

        #endregion

        #region 明细相关

        /// <summary>
        /// 是否为明细模块
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static bool IsDetailModule(Guid moduleId)
        {
            Sys_Module module = GetModuleById(moduleId);
            if (module != null)
            {
                return module.ParentId.HasValue && module.ParentId.Value != Guid.Empty;
            }
            return false;
        }

        /// <summary>
        /// 获取所有明细模块
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_Module> GetDetailModules(Guid moduleId)
        {
            string errMsg = string.Empty;
            Sys_Module module = GetModuleById(moduleId);
            if (module != null)
            {
                Guid parentId = module.Id;
                List<Sys_Module> detailModules = CommonOperate.GetEntities<Sys_Module>(out errMsg, x => x.ParentId == parentId, string.Empty, false, new List<string>() { "Sort" }, new List<bool>() { false });
                if (detailModules == null) detailModules = new List<Sys_Module>();
                return detailModules;
            }
            return new List<Sys_Module>();
        }

        /// <summary>
        /// 是否有明细模块
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static bool HasDetailModule(Guid moduleId)
        {
            List<Sys_Module> detailModules = GetDetailModules(moduleId);
            return detailModules != null && detailModules.Count > 0;
        }

        /// <summary>
        /// 获取父模块
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static Sys_Module GetParentModule(Guid moduleId)
        {
            Sys_Module module = GetModuleById(moduleId);
            if (module != null && module.ParentId.HasValue && module.ParentId.Value != Guid.Empty)
            {
                return GetModuleById(module.ParentId.Value);
            }
            return null;
        }

        /// <summary>
        /// 获取父模块
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public static Guid GetParentModuleId(Guid moduleId)
        {
            Sys_Module module = GetModuleById(moduleId);
            if (module != null && module.ParentId.HasValue && module.ParentId.Value != Guid.Empty)
            {
                Sys_Module parentModule = GetModuleById(module.ParentId.Value);
                if (parentModule != null)
                    return parentModule.Id;
            }
            return Guid.Empty;
        }

        #endregion

        /// <summary>
        /// 获取实体类型
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static Type GetModelType(Guid moduleId)
        {
            Sys_Module module = GetModuleById(moduleId);
            if (module != null)
            {
                return CommonOperate.GetModelType(module.TableName);
            }
            return null;
        }

        #endregion

        #region 附属模块

        /// <summary>
        /// 获取模块的附属模块集合
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_Module> GetAttachModules(Guid moduleId)
        {
            List<Sys_Module> list = new List<Sys_Module>();
            //取模块信息
            Sys_Module module = GetModuleById(moduleId);
            //取模块字段
            List<Sys_Field> fields = GetFields(x => x.Sys_ModuleId != moduleId && x.ForeignModuleName == module.Name && !x.IsDeleted);
            //取外键模块
            if (fields != null && fields.Count > 0)
            {
                fields = fields.Where(x => !CommonDefine.BaseEntityFields.Contains(x.Name)).ToList();
                List<Guid> hasAdd = new List<Guid>(); //已添加的moduleId
                foreach (Sys_Field field in fields)
                {
                    if (!field.Sys_ModuleId.HasValue) continue;
                    Sys_Module attachModule = GetModuleById(field.Sys_ModuleId.Value);
                    if (attachModule == null || hasAdd.Contains(attachModule.Id) || (attachModule.ParentId.HasValue && attachModule.ParentId.Value != Guid.Empty))
                        continue;
                    list.Add(attachModule);
                    hasAdd.Add(attachModule.Id);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取附属模块绑定集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_AttachModuleBind> GetAttachModuleBind(Guid userId, Guid moduleId)
        {
            Sys_Module currentModule = GetModuleById(moduleId);
            if (currentModule == null) return new List<Sys_AttachModuleBind>();
            string errMsg = string.Empty;
            List<Sys_AttachModuleBind> attachBinds = CommonOperate.GetEntities<Sys_AttachModuleBind>(out errMsg, x => x.Sys_UserId == userId && x.ModuleName == currentModule.Name && !x.IsDeleted, null, false, new List<string>() { "Sort" }, new List<bool>() { false });
            if (attachBinds == null) attachBinds = new List<Sys_AttachModuleBind>();
            return attachBinds;
        }

        /// <summary>
        /// 取用户绑定的附属模块集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_Module> GetUserBindAttachModules(Guid userId, Guid moduleId)
        {
            List<Sys_AttachModuleBind> attachBinds = GetAttachModuleBind(userId, moduleId).Where(x => x.IsValid).ToList();
            if (attachBinds.Count > 0)
            {
                List<Guid> attachModuleIds = attachBinds.Select(x => x.Sys_ModuleId.Value).ToList();
                List<Sys_Module> attachModules = GetAttachModules(moduleId);
                var tempList = attachModules.Where(x => attachModuleIds.Contains(x.Id)).ToList();
                return tempList;
            }
            return new List<Sys_Module>();
        }

        /// <summary>
        /// 获取用户绑定的附属模块集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="isInnerGrid">是否网格内绑定的附属模块</param>
        /// <returns></returns>
        public static List<Sys_Module> GetUserBindAttachModules(Guid userId, Guid moduleId, bool isInnerGrid)
        {
            List<Sys_AttachModuleBind> attachBinds = GetAttachModuleBind(userId, moduleId).Where(x => x.IsValid).ToList();
            if (isInnerGrid)
            {
                attachBinds = attachBinds.Where(x => x.AttachModuleInGrid).ToList();
            }
            else
            {
                attachBinds = attachBinds.Where(x => !x.AttachModuleInGrid).ToList();
            }
            if (attachBinds.Count > 0)
            {
                List<Guid> attachModuleIds = attachBinds.Select(x => x.Sys_ModuleId.Value).ToList();
                List<Sys_Module> attachModules = GetAttachModules(moduleId);
                var tempList = attachModules.Where(x => attachModuleIds.Contains(x.Id)).ToList();
                return tempList;
            }
            return new List<Sys_Module>();
        }

        /// <summary>
        /// 用户是否绑定了附属模块
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="isInnerGrid">是否网格内绑定的附属模块</param>
        /// <returns></returns>
        public static bool HasUserAttachModule(Guid userId, Guid moduleId, bool isInnerGrid)
        {
            return GetUserBindAttachModules(userId, moduleId, isInnerGrid).Count > 0;
        }

        #endregion

        #region 其他

        /// <summary>
        /// 根据视图Id获取模块Id
        /// </summary>
        /// <param name="viewId">视图Id</param>
        /// <returns></returns>
        public static Sys_Module GetModuleByViewId(Guid viewId)
        {
            Sys_Grid grid = GetGrid(viewId);
            if (grid != null && grid.Sys_ModuleId.HasValue)
                return GetModuleById(grid.Sys_ModuleId.Value);
            return null;
        }

        /// <summary>
        /// 删除模块相关的数据，删除对应的字段、表单、表单字段、列表、列表字段、列表按钮、字典绑定等信息
        /// </summary>
        /// <param name="t">模块对象</param>
        public static void DeleteModuleReferences(Sys_Module t)
        {
            if (t == null) return;
            string errMsg = string.Empty;
            List<Sys_Field> sysFields = SystemOperate.GetFieldInfos(t.Id);
            if (sysFields.Count > 0)
            {
                List<Guid?> sysFieldIds = sysFields.Select(x => (Guid?)x.Id).ToList();
                //删除字段信息
                CommonOperate.DeleteRecordsByExpression<Sys_Field>(x => x.Sys_ModuleId == t.Id, out errMsg);
                //删除表单
                List<Sys_Form> forms = CommonOperate.GetEntities<Sys_Form>(out errMsg, x => x.Sys_ModuleId == t.Id, null, false);
                List<Guid?> formIds = forms == null ? new List<Guid?>() : forms.Select(x => (Guid?)x.Id).ToList();
                CommonOperate.DeleteRecordsByExpression<Sys_Form>(x => x.Sys_ModuleId == t.Id, out errMsg);
                //删除角色表单
                CommonOperate.DeleteRecordsByExpression<Sys_RoleForm>(x => formIds.Contains(x.Sys_FormId), out errMsg);
                //删除表单字段
                CommonOperate.DeleteRecordsByExpression<Sys_FormField>(x => sysFieldIds.Contains(x.Sys_FieldId), out errMsg);
                //删除视图
                List<Sys_Grid> grids = CommonOperate.GetEntities<Sys_Grid>(out errMsg, x => x.Sys_ModuleId == t.Id, null, false);
                List<Guid?> gridIds = grids == null ? new List<Guid?>() : grids.Select(x => (Guid?)x.Id).ToList();
                CommonOperate.DeleteRecordsByExpression<Sys_Grid>(x => x.Sys_ModuleId == t.Id, out errMsg);
                //删除用户视图
                CommonOperate.DeleteRecordsByExpression<Sys_UserGrid>(x => gridIds.Contains(x.Sys_GridId), out errMsg);
                //删除列表字段
                CommonOperate.DeleteRecordsByExpression<Sys_GridField>(x => sysFieldIds.Contains(x.Sys_FieldId), out errMsg);
                //删除列表按钮
                CommonOperate.DeleteRecordsByExpression<Sys_GridButton>(x => x.Sys_ModuleId == t.Id, out errMsg);
                //删除字典绑定
                CommonOperate.DeleteRecordsByExpression<Sys_BindDictionary>(x => x.Sys_ModuleId == t.Id, out errMsg);
                //删除附件
                #region 删除附件的文件
                List<Sys_Attachment> attachments = CommonOperate.GetEntities<Sys_Attachment>(out errMsg, x => x.Sys_ModuleId == t.Id, null, false);
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (Sys_Attachment tempObj in attachments)
                    {
                        try
                        {
                            string tempFile = string.Format("{0}{1}", Globals.GetWebDir(), tempObj.FileUrl).Replace("/", "\\");
                            string tempPdfFile = string.Format("{0}{1}", Globals.GetWebDir(), tempObj.PdfUrl).Replace("/", "\\");
                            string tempSwfFile = string.Format("{0}{1}", Globals.GetWebDir(), tempObj.SwfUrl).Replace("/", "\\");
                            if (!string.IsNullOrEmpty(tempFile) && System.IO.File.Exists(tempFile))
                            {
                                System.IO.File.Delete(tempFile); //删除原文件
                            }
                            if (!string.IsNullOrEmpty(tempPdfFile) && System.IO.File.Exists(tempPdfFile))
                            {
                                System.IO.File.Delete(tempPdfFile); //删除ＰＤＦ文件
                            }
                            if (!string.IsNullOrEmpty(tempSwfFile) && System.IO.File.Exists(tempSwfFile))
                            {
                                System.IO.File.Delete(tempSwfFile);　//删除ＳＷＦ文件
                            }
                        }
                        catch { }
                    }
                }
                #endregion
                CommonOperate.DeleteRecordsByExpression<Sys_Attachment>(x => x.Sys_ModuleId == t.Id, out errMsg);
                //删除附属模块绑定
                CommonOperate.DeleteRecordsByExpression<Sys_AttachModuleBind>(x => x.Sys_ModuleId == t.Id || x.ModuleName == t.Name, out errMsg);
                //删除模块菜单
                CommonOperate.DeleteRecordsByExpression<Sys_Menu>(x => x.Sys_ModuleId == t.Id, out errMsg);
                //删除临时代码文件
                try
                {
                    string codeFile = string.Format("{0}Config\\TempModel\\{1}.code", Globals.GetWebDir(), t.TableName);
                    if (System.IO.File.Exists(codeFile))
                        System.IO.File.Delete(codeFile);
                    string dllFile = string.Format("{0}TempModel\\{1}.dll", Globals.GetBinPath(), t.TableName);
                    if (System.IO.File.Exists(dllFile))
                        System.IO.File.Delete(dllFile);
                }
                catch { }
            }
        }

        #endregion

        #endregion

        #region 菜单

        /// <summary>
        /// 获取顶级菜单
        /// </summary>
        /// <returns></returns>
        public static List<Sys_Menu> GetTopMenus()
        {
            string errMsg = string.Empty;
            List<Sys_Menu> list = CommonOperate.GetEntities<Sys_Menu>(out errMsg, x => (x.ParentId == null || x.ParentId == Guid.Empty) && x.IsValid && !x.IsDeleted, null, true, new List<string>() { "Sort" }, new List<bool>() { false });
            if (list == null) return new List<Sys_Menu>();
            return list;
        }

        /// <summary>
        /// 取子菜单
        /// </summary>
        /// <param name="menuId">父菜单Id</param>
        /// <param name="isDirect">是否直接子菜单，否时取所有子菜单</param>
        /// <param name="detailModuleToAdd">是否将明细模块添加到菜单中，默认为否</param>
        /// <returns></returns>
        public static List<Sys_Menu> GetChildMenus(Guid? menuId, bool isDirect = true, bool detailModuleToAdd = false)
        {
            string errMsg = string.Empty;
            List<Sys_Menu> listTemp = new List<Sys_Menu>();
            if (menuId.HasValue && menuId.Value != Guid.Empty)
            {
                listTemp = CommonOperate.GetEntities<Sys_Menu>(out errMsg, x => x.ParentId == menuId, null, true, new List<string>() { "Sort" }, new List<bool>() { false });
            }
            else
            {
                listTemp = CommonOperate.GetEntities<Sys_Menu>(out errMsg, x => x.ParentId == null || x.ParentId == Guid.Empty, null, true, new List<string>() { "Sort" }, new List<bool>() { false });
            }
            if (listTemp == null) listTemp = new List<Sys_Menu>();
            if (detailModuleToAdd) //需要将明细模块添加到菜单中
            {
                List<Guid> moduleIds = listTemp.Where(x => x.Sys_ModuleId.HasValue && x.Sys_ModuleId.Value != Guid.Empty).Select(x => x.Sys_ModuleId.Value).ToList();
                List<Sys_Menu> addMenus = new List<Sys_Menu>();
                foreach (Sys_Menu menu in listTemp)
                {
                    if (!menu.Sys_ModuleId.HasValue) continue;
                    Sys_Module module = GetModuleById(menu.Sys_ModuleId.Value);
                    if (module == null) continue;
                    List<Sys_Module> detailModules = GetDetailModules(module.Id);
                    foreach (Sys_Module detailModule in detailModules)
                    {
                        if (moduleIds.Contains(detailModule.Id))
                            continue;
                        Sys_Menu tempMenu = new Sys_Menu() { Id = detailModule.Id, Name = detailModule.Name, Display = detailModule.Name, Sort = menu.Sort, Sys_ModuleId = detailModule.Id, Icon = menu.Icon, IsLeaf = true, IsValid = true, ParentId = menu.ParentId };
                        addMenus.Add(tempMenu);
                    }
                }
                listTemp.AddRange(addMenus);
            }
            if (isDirect)
            {
                return listTemp;
            }
            List<Sys_Menu> list = new List<Sys_Menu>();
            foreach (Sys_Menu menu in listTemp)
            {
                list.Add(menu);
                list.AddRange(GetChildMenus(menu.Id, isDirect, detailModuleToAdd));
            }
            return list;
        }

        /// <summary>
        /// 获取模块对应的菜单
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static Sys_Menu GetMenuOfModule(Guid moduleId)
        {
            string errMsg = string.Empty;
            Sys_Menu menu = CommonOperate.GetEntity<Sys_Menu>(x => x.Sys_ModuleId == moduleId && !x.IsDeleted, null, out errMsg);
            return menu;
        }

        /// <summary>
        /// 获取用户快捷操作菜单
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static List<Sys_Menu> GetUserQuckMenus(Guid userId)
        {
            string errMsg = string.Empty;
            List<Sys_Menu> list = new List<Sys_Menu>();
            List<Sys_UserQuckMenu> quckMenus = CommonOperate.GetEntities<Sys_UserQuckMenu>(out errMsg, x => x.Sys_UserId == userId && x.Sys_MenuId != null && !x.IsDeleted, null, false, new List<string>() { "Sort" }, new List<bool>() { false });
            if (quckMenus != null && quckMenus.Count > 0)
            {
                foreach (Sys_UserQuckMenu quckMenu in quckMenus)
                {
                    Sys_Menu menu = CommonOperate.GetEntityById<Sys_Menu>(quckMenu.Sys_MenuId.Value, out errMsg);
                    if (menu == null) continue;
                    list.Add(menu);
                }
            }
            return list;
        }

        #endregion

        #region 字段

        /// <summary>
        /// 获取主键字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_Field> GetPrimaryKeyFields(Guid moduleId)
        {
            List<string> fieldNames = GetModulePrimaryKeyFields(moduleId);
            if (fieldNames.Count > 0)
            {
                string errMsg = string.Empty;
                List<Sys_Field> fields = CommonOperate.GetEntities<Sys_Field>(out errMsg, x => x.Sys_ModuleId == moduleId && fieldNames.Contains(x.Name));
                return fields;
            }
            return new List<Sys_Field>();
        }

        /// <summary>
        /// 根据Id获取字段信息
        /// </summary>
        /// <param name="id">字段Id</param>
        /// <returns></returns>
        public static Sys_Field GetFieldById(Guid id)
        {
            string errMsg = string.Empty;
            Sys_Field field = CommonOperate.GetEntityById<Sys_Field>(id, out errMsg);
            return field;
        }

        /// <summary>
        /// 获取字段信息
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<Sys_Field> GetFields(Expression<Func<Sys_Field, bool>> expression = null)
        {
            string errMsg = string.Empty;
            List<Sys_Field> fields = CommonOperate.GetEntities<Sys_Field>(out errMsg, expression);
            if (fields == null) fields = new List<Sys_Field>();
            return fields;
        }

        /// <summary>
        /// 获取模块字段信息
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_Field> GetFieldInfos(Guid moduleId)
        {
            string errMsg = string.Empty;
            List<Sys_Field> fields = CommonOperate.GetEntities<Sys_Field>(out errMsg, x => x.Sys_ModuleId == moduleId, null, false);
            if (fields == null) fields = new List<Sys_Field>();
            return fields;
        }

        /// <summary>
        /// 获取模块字段信息
        /// </summary>
        /// <param name="module">模块对象</param>
        /// <returns></returns>
        public static List<Sys_Field> GetFieldInfos(Sys_Module module)
        {
            return GetFieldInfos(module.Id);
        }

        /// <summary>
        /// 获取模块的某个字段信息
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static Sys_Field GetFieldInfo(Guid moduleId, string fieldName)
        {
            List<Sys_Field> fields = GetFieldInfos(moduleId);
            if (fields != null && fields.Count > 0)
            {
                return fields.Where(x => x.Name == fieldName).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 获取字段id
        /// </summary>
        /// <param name="moduleId">模块id</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static Guid GetFieldId(Guid moduleId, string fieldName)
        {
            Sys_Field field = GetFieldInfo(moduleId, fieldName);
            if (field != null) return field.Id;
            return Guid.Empty;
        }

        /// <summary>
        /// 通过字段显示名称获取字段信息
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="display">显示名称</param>
        /// <returns></returns>
        public static Sys_Field GetFieldByDisplay(Guid moduleId, string display)
        {
            string errMsg = string.Empty;
            Sys_Field field = CommonOperate.GetEntity<Sys_Field>(x => x.Sys_ModuleId == moduleId && x.Display == display, null, out errMsg);
            return field;
        }

        /// <summary>
        /// 判断是否为外键字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static bool IsForeignField(Guid moduleId, string fieldName)
        {
            Sys_Field field = GetFieldInfo(moduleId, fieldName);
            if (field != null)
            {
                Sys_Module module = GetModuleByName(field.ForeignModuleName);
                return module != null;
            }
            return false;
        }

        /// <summary>
        /// 判断是否外键字段
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static bool IsForeignField(Sys_Field field)
        {
            if (field != null)
            {
                Sys_Module module = GetModuleByName(field.ForeignModuleName);
                return module != null;
            }
            return false;
        }

        /// <summary>
        /// 是否外键Name字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">外键Name字段名称</param>
        /// <returns></returns>
        public static bool IsForeignNameField(Guid moduleId, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName)) return false;
            Sys_Field field = GetFieldInfo(moduleId, fieldName);
            if (field != null) return false;
            Type modelType = CommonOperate.GetModelType(moduleId);
            if (fieldName.Length > 4 && fieldName.EndsWith("Name"))
            {
                PropertyInfo p = modelType.GetProperty(fieldName);
                if (p != null)
                {
                    string tempFieldName = fieldName.Substring(0, fieldName.Length - 4) + "Id";
                    field = GetFieldInfo(moduleId, tempFieldName);
                    if (field != null)
                    {
                        Sys_Module module = GetModuleByName(field.ForeignModuleName);
                        return module != null;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 是否为枚举字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static bool IsEnumField(Guid moduleId, string fieldName)
        {
            Type modelType = GetModelType(moduleId);
            if (modelType == null) return false;
            PropertyInfo pEnum = modelType.GetProperty(string.Format("{0}OfEnum", fieldName));
            return pEnum != null && pEnum.PropertyType.IsEnum;
        }

        /// <summary>
        /// 是否为枚举字段
        /// </summary>
        /// <param name="modelType">模块类型</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static bool IsEnumField(Type modelType, string fieldName)
        {
            if (modelType == null) return false;
            PropertyInfo pEnum = modelType.GetProperty(string.Format("{0}OfEnum", fieldName));
            return pEnum != null && pEnum.PropertyType.IsEnum;
        }

        /// <summary>
        /// 是否字典绑定字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static bool IsDictionaryBindField(Guid moduleId, string fieldName)
        {
            string className = GetBindDictonaryClass(moduleId, fieldName);
            return !string.IsNullOrEmpty(className);
        }

        /// <summary>
        /// 获取字段的显示名称
        /// </summary>
        /// <param name="fieldId">字段Id</param>
        /// <returns></returns>
        public static string GetFieldDisplay(Guid fieldId)
        {
            Sys_Field field = GetFieldById(fieldId);
            if (field != null) return field.Display;
            return string.Empty;
        }

        /// <summary>
        /// 获取字段的显示名称
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static string GetFieldDisplay(Guid moduleId, string fieldName)
        {
            Sys_Field field = GetFieldInfo(moduleId, fieldName);
            if (field != null) return field.Display;
            if (IsForeignNameField(moduleId, fieldName))
            {
                string tempFieldName = fieldName.Substring(0, fieldName.Length - 4) + "Id";
                field = GetFieldInfo(moduleId, tempFieldName);
                if (field != null) return field.Display;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取字段的外键模块名称
        /// </summary>
        /// <param name="fieldId">字段Id</param>
        /// <returns></returns>
        public static string GetFieldForeignModuleName(Guid fieldId)
        {
            Sys_Field field = GetFieldById(fieldId);
            if (field != null) return field.ForeignModuleName;
            return string.Empty;
        }

        /// <summary>
        /// 获取字段的外键模块名称
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static string GetFieldForeignModuleName(Guid moduleId, string fieldName)
        {
            Sys_Field field = GetFieldInfo(moduleId, fieldName);
            if (field != null) return field.ForeignModuleName;
            return string.Empty;
        }

        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static Type GetFieldType(Guid moduleId, string fieldName)
        {
            Type modelType = CommonOperate.GetModelType(moduleId);
            if (modelType == null) return null;
            PropertyInfo p = modelType.GetProperty(fieldName);
            if (p == null) return null;
            return p.PropertyType;
        }

        /// <summary>
        /// 获取字段类型
        /// </summary>
        /// <param name="modelType">实体类型</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static Type GetFieldType(Type modelType, string fieldName)
        {
            if (modelType == null) return null;
            PropertyInfo p = modelType.GetProperty(fieldName);
            if (p == null) return null;
            return p.PropertyType;
        }

        /// <summary>
        /// 获取字段综合信息
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetFieldCommonInfo(Guid moduleId, string fieldName)
        {
            string foreignModuleName = null;
            object dicData = null;
            object enumData = null;
            int controlType = 0;
            string foreignTitleKey = null;
            string tempFieldName = fieldName;
            if (SystemOperate.IsForeignNameField(moduleId, fieldName))
                tempFieldName = fieldName.Substring(0, fieldName.Length - 4) + "Id";
            Type fieldType = SystemOperate.GetFieldType(moduleId, tempFieldName);
            Sys_Field sysField = SystemOperate.GetFieldInfo(moduleId, tempFieldName);
            if (sysField == null) return null;
            foreignModuleName = sysField.ForeignModuleName;
            Sys_Module foreignModule = SystemOperate.GetForeignModule(sysField);
            Sys_FormField formField = SystemOperate.GetDefaultFormSingleField(moduleId, tempFieldName);
            if (formField != null)
            {
                controlType = formField.ControlType;
                dicData = SystemOperate.DictionaryDataJson(moduleId, tempFieldName);
                enumData = SystemOperate.EnumFieldDataJson(moduleId, tempFieldName);
            }
            else if (CommonDefine.BaseEntityFields.Contains(tempFieldName))
            {
                if (foreignModule != null)
                    controlType = (int)ControlTypeEnum.DialogGrid;
                else if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?))
                    controlType = (int)ControlTypeEnum.DateTimeBox;
            }
            foreignTitleKey = foreignModule == null ? null : foreignModule.TitleKey;
            return new { FieldName = fieldName, ControlType = controlType, DicData = dicData, EnumData = enumData, ForeignModule = foreignModuleName, FieldType = fieldType.ToString(), ForeignTitleKey = foreignTitleKey };
        }

        #region 字段值

        /// <summary>
        /// 获取字段显示值
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="model">实体对象</param>
        /// <param name="sysField">字段信息</param>
        /// <param name="field">表单字段</param>
        /// <returns></returns>
        public static string GetFieldDisplayValue(Guid moduleId, object model, Sys_Field sysField, Sys_FormField field)
        {
            if (field == null || model == null || sysField == null) return string.Empty;
            object value = CommonOperate.GetModelFieldValueByModel(moduleId, model, sysField.Name);
            if (value == null) return string.Empty;
            string valueStr = value.ObjToStr();
            if (SystemOperate.IsForeignField(moduleId, sysField.Name)) //外键字段
            {
                string textFieldName = sysField.Name.Substring(0, sysField.Name.Length - 2) + "Name";
                valueStr = CommonOperate.GetModelFieldValueByModel(moduleId, model, textFieldName).ObjToStr();
                if (string.IsNullOrEmpty(valueStr)) //当前实体中不存在从数据库中取
                {
                    Sys_Module foreignModule = GetForeignModule(sysField);
                    valueStr = CommonOperate.GetModelTitleKeyValue(foreignModule.Id, value.ObjToGuid());
                }
            }
            else if (SystemOperate.IsEnumField(moduleId, sysField.Name)) //枚举字段
            {
                valueStr = SystemOperate.GetEnumFieldDisplayText(moduleId, sysField.Name, valueStr);
            }
            else if (SystemOperate.IsDictionaryBindField(moduleId, sysField.Name)) //字典绑定字段
            {
                valueStr = SystemOperate.GetDictionaryDisplayText(moduleId, sysField.Name, valueStr);
            }
            else if (field.ControlTypeOfEnum == ControlTypeEnum.DateBox ||
                field.ControlTypeOfEnum == ControlTypeEnum.DateTimeBox)
            {
                string dateFormat = field.ControlTypeOfEnum == ControlTypeEnum.DateBox ? "yyyy-MM-dd" : "yyyy-MM-dd HH:mm:ss";
                try
                {
                    valueStr = DateTime.Parse(valueStr).ToString(dateFormat);
                }
                catch { }
            }
            else
            {
                Type fieldType = SystemOperate.GetFieldType(moduleId, sysField.Name);
                if (fieldType == typeof(Boolean) || fieldType == typeof(Boolean?))
                {
                    if (valueStr.ToLower() == "true")
                        valueStr = "是";
                    else if (valueStr.ToLower() == "false")
                        valueStr = "否";
                }
            }
            return valueStr;
        }

        /// <summary>
        /// 获取字段显示值
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="model">实体对象</param>
        /// <param name="field">表单字段</param>
        /// <returns></returns>
        public static string GetFieldDisplayValue(Guid moduleId, object model, Sys_FormField field)
        {
            if (field == null || model == null) return string.Empty;
            Sys_Field sysField = SystemOperate.GetFieldById(field.Sys_FieldId.Value);
            return GetFieldDisplayValue(moduleId, model, sysField, field);
        }

        /// <summary>
        /// 获取字段显示值
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="recordId">记录Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static string GetFieldDisplayValue(Guid moduleId, Guid recordId, string fieldName)
        {
            string errMsg = string.Empty;
            object model = CommonOperate.GetEntityById(moduleId, recordId, out errMsg);
            Sys_FormField field = GetDefaultFormSingleField(moduleId, fieldName);
            return GetFieldDisplayValue(moduleId, model, field);
        }

        /// <summary>
        /// 获取字段显示值
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="model">实体对象</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static string GetFieldDisplayValue(Guid moduleId, object model, string fieldName)
        {
            Sys_FormField formField = GetDefaultFormSingleField(moduleId, fieldName);
            if (formField == null)
            {
                Type modelType = CommonOperate.GetModelType(moduleId);
                PropertyInfo p = modelType.GetProperty(fieldName);
                if (p == null) return string.Empty;
                return p.GetValue(model, null).ObjToStr();
            }
            return GetFieldDisplayValue(moduleId, model, formField);
        }

        #endregion

        #endregion

        #region 列表

        #region 视图字段

        /// <summary>
        /// 加载用户视图字段
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_GridField> GetUserGridFields(Guid userId, Guid moduleId)
        {
            Sys_Grid grid = GetUserDefaultGrid(userId, moduleId);
            return GetGridFields(grid);
        }

        /// <summary>
        /// 获取默认列表视图的字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="addForeignNameField">是否添加外键显示字段</param>
        /// <returns></returns>
        public static List<Sys_GridField> GetDefaultGridFields(Guid moduleId, bool addForeignNameField = true)
        {
            string errMsg = string.Empty;
            Sys_Grid grid = GetGrids(moduleId).Where(x => x.IsDefault).FirstOrDefault();
            if (grid == null) return new List<Sys_GridField>();
            List<Sys_GridField> gridFields = GetGridFields(grid, addForeignNameField);
            return gridFields;
        }

        /// <summary>
        /// 获取视图字段
        /// </summary>
        /// <param name="viewId">视图Id</param>
        /// <param name="addForeignNameField">是否自动添加外键Name网格字段</param>
        /// <returns></returns>
        public static List<Sys_GridField> GetGridFields(Guid viewId, bool addForeignNameField = true)
        {
            Sys_Grid grid = GetGrid(viewId);
            return GetGridFields(grid, addForeignNameField);
        }

        /// <summary>
        /// 获取网格字段
        /// </summary>
        /// <param name="viewId">视图Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static Sys_GridField GetGridField(Guid viewId, string fieldName)
        {
            string errMsg = string.Empty;
            return GetGridFields(viewId, false).Where(x => x.Sys_FieldName == fieldName).FirstOrDefault();
        }

        /// <summary>
        /// 获取默认网格字段
        /// </summary>
        /// <param name="sysField">字段信息</param>
        /// <returns></returns>
        public static Sys_GridField GetDefaultGridField(Sys_Field sysField)
        {
            if (sysField != null && sysField.Sys_ModuleId.HasValue && sysField.Sys_ModuleId.Value != Guid.Empty)
            {
                return GetDefaultGridFields(sysField.Sys_ModuleId.Value, false).Where(x => x.Sys_FieldId == sysField.Id).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 获取视图字段
        /// </summary>
        /// <param name="grid">视图对象</param>
        /// <param name="addForeignNameField">是否自动添加外键Name网格字段</param>
        /// <returns></returns>
        public static List<Sys_GridField> GetGridFields(Sys_Grid grid, bool addForeignNameField = true)
        {
            if (grid == null || !grid.Sys_ModuleId.HasValue)
                return new List<Sys_GridField>();
            string errMsg = string.Empty;
            //装载最终视图字段
            List<Sys_GridField> list = new List<Sys_GridField>();
            //获取视图字段
            List<Sys_GridField> gridFields = CommonOperate.GetEntities<Sys_GridField>(out errMsg, x => x.Sys_GridId == grid.Id && x.IsVisible && !x.IsDeleted, string.Empty, false, new List<string>() { "Sort" }, new List<bool>() { false });
            if (gridFields != null && gridFields.Count > 0)
            {
                //循环处理字段
                foreach (Sys_GridField field in gridFields)
                {
                    if (!field.Sys_FieldId.HasValue) continue;
                    if (!PermissionOperate.CanViewField(UserInfo.CurrentUserInfo.UserId, grid.Sys_ModuleId.Value, field.Sys_FieldName)) //没有字段权限
                    {
                        if (!field.IsFrozen) continue;
                        #region 复制字段
                        Sys_GridField tempGridField = new Sys_GridField();
                        tempGridField.Id = field.Id;
                        tempGridField.Sys_GridId = field.Sys_GridId;
                        tempGridField.Sys_GridName = field.Sys_GridName;
                        tempGridField.Sys_FieldId = field.Sys_FieldId;
                        tempGridField.Sys_FieldName = field.Sys_FieldName;
                        tempGridField.Display = field.Display;
                        tempGridField.MinWidth = field.MinWidth;
                        tempGridField.Width = field.Width;
                        tempGridField.IsFrozen = field.IsFrozen;
                        tempGridField.IsGroupField = field.IsGroupField;
                        tempGridField.IsVisible = false;
                        tempGridField.Sort = field.Sort;
                        tempGridField.IsAllowSearch = field.IsAllowSearch;
                        tempGridField.IsAllowSort = field.IsAllowSort;
                        tempGridField.IsAllowHide = field.IsAllowHide;
                        tempGridField.Align = field.Align;
                        tempGridField.IsAllowExport = field.IsAllowExport;
                        tempGridField.FieldFormatter = field.FieldFormatter;
                        tempGridField.EditorFormatter = field.EditorFormatter;
                        tempGridField.CreateDate = field.CreateDate;
                        tempGridField.CreateUserId = field.CreateUserId;
                        tempGridField.CreateUserName = field.CreateUserName;
                        tempGridField.ModifyDate = field.ModifyDate;
                        tempGridField.ModifyUserId = field.ModifyUserId;
                        tempGridField.ModifyUserName = field.ModifyUserName;
                        #endregion
                        list.Add(tempGridField);
                        continue;
                    }
                    //是否要添加name字段，对于外键字段需要添加name字段
                    if (addForeignNameField && field.IsVisible)
                    {
                        Sys_Field sysField = SystemOperate.GetFieldById(field.Sys_FieldId.Value);
                        bool isForeign = !string.IsNullOrEmpty(sysField.ForeignModuleName); //IsForeignField(sysField);
                        if (isForeign) //需要添加
                        {
                            #region 复制字段
                            Sys_GridField tempGridField = new Sys_GridField();
                            tempGridField.Id = field.Id;
                            tempGridField.Sys_GridId = field.Sys_GridId;
                            tempGridField.Sys_GridName = field.Sys_GridName;
                            tempGridField.Sys_FieldId = field.Sys_FieldId;
                            tempGridField.Sys_FieldName = field.Sys_FieldName;
                            tempGridField.Display = field.Display;
                            tempGridField.MinWidth = field.MinWidth;
                            tempGridField.IsFrozen = field.IsFrozen;
                            tempGridField.Sort = field.Sort;
                            tempGridField.IsAllowSort = field.IsAllowSort;
                            tempGridField.IsAllowHide = field.IsAllowHide;
                            tempGridField.Align = field.Align;
                            tempGridField.IsAllowExport = field.IsAllowExport;
                            tempGridField.FieldFormatter = field.FieldFormatter;
                            tempGridField.EditorFormatter = field.EditorFormatter;
                            tempGridField.CreateDate = field.CreateDate;
                            tempGridField.CreateUserId = field.CreateUserId;
                            tempGridField.CreateUserName = field.CreateUserName;
                            tempGridField.ModifyDate = field.ModifyDate;
                            tempGridField.ModifyUserId = field.ModifyUserId;
                            tempGridField.ModifyUserName = field.ModifyUserName;
                            tempGridField.IsVisible = false; //原字段不可见
                            tempGridField.IsAllowSearch = false; //不可搜索
                            tempGridField.IsGroupField = false; //不可分组
                            tempGridField.Width = 0;
                            #endregion
                            list.Add(tempGridField);
                            //增加一个name字段
                            #region 增加外键Name字段
                            Sys_GridField tempNameGridField = new Sys_GridField();
                            tempNameGridField.Id = field.Id;
                            tempNameGridField.Sys_GridId = field.Sys_GridId;
                            tempNameGridField.Sys_GridName = field.Sys_GridName;
                            tempNameGridField.Sys_FieldId = field.Sys_FieldId;
                            tempNameGridField.Display = field.Display;
                            tempNameGridField.MinWidth = field.MinWidth;
                            tempNameGridField.Width = field.Width;
                            tempNameGridField.IsFrozen = field.IsFrozen;
                            tempNameGridField.IsGroupField = field.IsGroupField;
                            tempNameGridField.IsVisible = field.IsVisible;
                            tempNameGridField.Sort = field.Sort;
                            tempNameGridField.IsAllowSearch = field.IsAllowSearch;
                            tempNameGridField.IsAllowSort = field.IsAllowSort;
                            tempNameGridField.IsAllowHide = field.IsAllowHide;
                            tempNameGridField.Align = field.Align;
                            tempNameGridField.IsAllowExport = field.IsAllowExport;
                            tempNameGridField.FieldFormatter = field.FieldFormatter;
                            tempNameGridField.EditorFormatter = field.EditorFormatter;
                            tempNameGridField.CreateDate = field.CreateDate;
                            tempNameGridField.CreateUserId = field.CreateUserId;
                            tempNameGridField.CreateUserName = field.CreateUserName;
                            tempNameGridField.ModifyDate = field.ModifyDate;
                            tempNameGridField.ModifyUserId = field.ModifyUserId;
                            tempNameGridField.ModifyUserName = field.ModifyUserName;
                            //字段名称为原字段名称去掉最后的Id加上Name
                            tempNameGridField.Sys_FieldName = isForeign ? field.Sys_FieldName.Substring(0, field.Sys_FieldName.Length - 2) + "Name" : field.Sys_FieldName + "Name";
                            #endregion
                            list.Add(tempNameGridField);
                        }
                        else
                        {
                            list.Add(field);
                        }
                    }
                    else //不需要添加外键Name字段
                    {
                        list.Add(field);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取默认视图搜索字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_GridField> GetDefaultSearchGridFields(Guid moduleId)
        {
            Sys_Grid grid = GetGrids(moduleId).Where(x => x.IsDefault).FirstOrDefault();
            if (grid == null) return new List<Sys_GridField>();
            return GetSearchGridFields(grid.Id);
        }

        /// <summary>
        /// 取搜索字段
        /// </summary>
        /// <param name="viewId">视图Id</param>
        /// <param name="containId">是否包含Id列</param>
        /// <returns></returns>
        public static List<Sys_GridField> GetSearchGridFields(Guid viewId, bool containId = false)
        {
            string errMsg = string.Empty;
            List<Sys_GridField> list = GetGridFields(viewId, false).Where(x => x.IsAllowSearch).ToList();
            if (list != null && list.Count > 0)
            {
                List<Sys_GridField> tempList = new List<Sys_GridField>();
                foreach (Sys_GridField field in list)
                {
                    if (!field.Sys_FieldId.HasValue) continue;
                    if (!containId)
                    {
                        if (field.Sys_FieldName == "Id") continue;
                    }
                    tempList.Add(field);
                }
                return tempList;
            }
            return new List<Sys_GridField>();
        }

        /// <summary>
        /// 获取默认列表分组字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static Sys_GridField GetDefaultGridGroupField(Guid moduleId)
        {
            Sys_Grid grid = GetGrids(moduleId).Where(x => x.IsDefault).FirstOrDefault();
            if (grid == null) return null;
            return GetGridGroupField(grid.Id);
        }

        /// <summary>
        /// 获取分组字段
        /// </summary>
        /// <param name="viewId">视图Id</param>
        /// <returns></returns>
        public static Sys_GridField GetGridGroupField(Guid viewId)
        {
            string errMsg = string.Empty;
            Sys_GridField field = GetGridFields(viewId).Where(x => x.IsGroupField).FirstOrDefault();
            return field;
        }

        /// <summary>
        /// 获取列表行过滤规则
        /// </summary>
        /// <param name="moduleId">模块id</param>
        /// <param name="gridFields">模块字段</param>
        /// <param name="noFilterFields">不参与过滤的字段</param>
        /// <returns></returns>
        public static StringBuilder GetGridRowFilterRules(Guid moduleId, List<Sys_GridField> gridFields, out List<string> noFilterFields)
        {
            StringBuilder ruleFilters = new StringBuilder();
            //不参与过滤的字段
            noFilterFields = new List<string>();
            if (SystemOperate.GetGridHeadButtons(moduleId).Count > 0)
            {
                noFilterFields.Add("RowOperateBtn");
            }
            foreach (Sys_GridField field in gridFields)
            {
                if (!field.Sys_FieldId.HasValue) continue;
                Sys_Field sysField = SystemOperate.GetFieldById(field.Sys_FieldId.Value);
                if (sysField == null) continue;
                if (!field.IsAllowSearch)
                {
                    noFilterFields.Add(field.Sys_FieldName);
                    continue;
                }
                Type fieldType = SystemOperate.GetFieldType(moduleId, sysField.Name);
                if (fieldType == null) continue;
                if (!CommonDefine.BaseEntityFields.Contains(field.Sys_FieldName))
                {
                    Sys_FormField formField = SystemOperate.GetDefaultFormSingleField(sysField);
                    if (formField != null)
                    {
                        switch (formField.ControlTypeOfEnum)
                        {
                            case ControlTypeEnum.TextBox:
                            case ControlTypeEnum.TextAreaBox:
                                ruleFilters.Append("{field:'" + sysField.Name + "',type:'textbox',op:['contains','isnull','isnotnull']},");
                                break;
                            case ControlTypeEnum.IntegerBox:
                            case ControlTypeEnum.NumberBox:
                                if (fieldType.IsGenericType)
                                    ruleFilters.Append("{field:'" + sysField.Name + "',type:'numberbox',op:['equal','notequal','less','greater','isnull','isnotnull']},");
                                else
                                    ruleFilters.Append("{field:'" + sysField.Name + "',type:'numberbox',op:['equal','notequal','less','greater']},");
                                break;
                            case ControlTypeEnum.SingleCheckBox:
                                if (fieldType.IsGenericType)
                                    ruleFilters.Append("{field:'" + sysField.Name + "',type:'checkbox',op:['equal','notequal','isnull','isnotnull']},");
                                else
                                    ruleFilters.Append("{field:'" + sysField.Name + "',type:'checkbox',op:['equal','notequal']},");
                                break;
                            case ControlTypeEnum.DialogGrid:
                                {
                                    string fieldName = sysField.Name;
                                    fieldName = fieldName.Substring(0, fieldName.Length - 2) + "Name";
                                    ruleFilters.Append("{field:'" + fieldName + "',type:'textbox',op:['contains','isnull','isnotnull']},");
                                }
                                break;
                            case ControlTypeEnum.ComboBox:
                            case ControlTypeEnum.ComboGrid:
                            case ControlTypeEnum.ComboTree:
                                {
                                    Sys_Module foreignModule = SystemOperate.GetModuleByName(sysField.ForeignModuleName);
                                    string valueField = formField.ValueField;
                                    string textField = formField.TextField;
                                    string fieldUrl = formField.UrlOrData;
                                    string type = "combobox";
                                    if (formField.ControlTypeOfEnum == ControlTypeEnum.ComboTree)
                                    {
                                        type = "combotree";
                                        if (string.IsNullOrEmpty(valueField)) valueField = "id";
                                        if (string.IsNullOrEmpty(textField)) textField = "text";
                                        if (fieldUrl == null && foreignModule != null)
                                        {
                                            fieldUrl = string.Format("/{0}/GetTreeByNode.html?moduleId={1}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, foreignModule.Id);
                                        }
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(valueField)) valueField = "Id";
                                        if (string.IsNullOrEmpty(textField))
                                        {
                                            string foreignTitleKey = foreignModule == null ? null : SystemOperate.GetModuleTitleKey(foreignModule.Id);
                                            textField = string.IsNullOrWhiteSpace(foreignTitleKey) ? "Name" : foreignTitleKey;
                                        }
                                        if (string.IsNullOrEmpty(fieldUrl))
                                        {
                                            if (foreignModule != null) //外键字段
                                                fieldUrl = string.Format("/{0}/BindForeignFieldComboData.html?moduleId={1}&fieldName={2}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, moduleId, field.Sys_FieldName);
                                            else if (SystemOperate.IsEnumField(moduleId, field.Sys_FieldName)) //枚举字段
                                                fieldUrl = string.Format("/{0}/BindEnumFieldData.html?moduleId={1}&fieldName={2}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, moduleId, field.Sys_FieldName);
                                            else if (SystemOperate.IsDictionaryBindField(moduleId, field.Sys_FieldName)) //字典绑定字段
                                                fieldUrl = string.Format("/{0}/BindDictionaryData.html?moduleId={1}&fieldName={2}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, moduleId, field.Sys_FieldName);
                                        }
                                    }
                                    string fieldName = sysField.Name;
                                    if (foreignModule != null)
                                        fieldName = fieldName.Substring(0, fieldName.Length - 2) + "Name";
                                    string loadFilterElseStr = formField.ControlTypeOfEnum == ControlTypeEnum.ComboTree ? ",loadFilter:RuleFilterLoadComTreeFilter" : string.Empty;
                                    if (fieldType.IsGenericType)
                                        ruleFilters.Append("{field:'" + fieldName + "',type:'" + type + "',options:{panelWidth:'auto',url:'" + fieldUrl + "',valueField:'" + valueField + "',textField:'" + textField + "'" + loadFilterElseStr + "},op:['equal','notequal','isnull','isnotnull']},");
                                    else
                                        ruleFilters.Append("{field:'" + fieldName + "',type:'" + type + "',options:{panelWidth:'auto',url:'" + fieldUrl + "',valueField:'" + valueField + "',textField:'" + textField + "'" + loadFilterElseStr + "},op:['equal','notequal']},");
                                }
                                break;
                            case ControlTypeEnum.DateBox:
                            case ControlTypeEnum.DateTimeBox:
                                if (fieldType.IsGenericType)
                                    ruleFilters.Append("{field:'" + sysField.Name + "',type:'datebox',op:['less','greater','isnull','isnotnull']},");
                                else
                                    ruleFilters.Append("{field:'" + sysField.Name + "',type:'datebox',op:['less','greater']},");
                                break;
                        }
                    }
                    else
                    {
                        if (fieldType == typeof(String))
                        {
                            ruleFilters.Append("{field:'" + sysField.Name + "',type:'textbox',op:['contains','isnull','isnotnull']},");
                        }
                        else if (GetModuleByName(sysField.ForeignModuleName) != null) //外键字段
                        {
                            ruleFilters.Append("{field:'" + sysField.Name + "',type:'textbox',op:['contains','isnull','isnotnull']},");
                        }
                        else if (SystemOperate.IsEnumField(moduleId, sysField.Name)) //枚举字段
                        {
                            string fieldUrl = string.Format("/{0}/BindEnumFieldData.html?moduleId={1}&fieldName={2}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, moduleId, field.Sys_FieldName);
                            if (fieldType.IsGenericType)
                                ruleFilters.Append("{field:'" + sysField.Name + "',type:'combobox',options:{panelWidth:'auto',url:'" + fieldUrl + "',valueField:'Id',textField:'Name'},op:['equal','notequal','isnull','isnotnull']},");
                            else
                                ruleFilters.Append("{field:'" + sysField.Name + "',type:'combobox',options:{panelWidth:'auto',url:'" + fieldUrl + "',valueField:'Id',textField:'Name'},op:['equal','notequal']},");
                        }
                        else if (fieldType == typeof(Int16) || fieldType == typeof(Int32) || fieldType == typeof(Int64) ||
                            fieldType == typeof(Int16?) || fieldType == typeof(Int32?) || fieldType == typeof(Int64?) ||
                            fieldType == typeof(Double) || fieldType == typeof(float) || fieldType == typeof(Decimal) ||
                            fieldType == typeof(Double?) || fieldType == typeof(float?) || fieldType == typeof(Decimal?))
                        {
                            if (fieldType.IsGenericType)
                                ruleFilters.Append("{field:'" + sysField.Name + "',type:'numberbox',op:['equal','notequal','less','greater','isnull','isnotnull']},");
                            else
                                ruleFilters.Append("{field:'" + sysField.Name + "',type:'numberbox',op:['equal','notequal','less','greater']},");
                        }
                        else if (fieldType == typeof(Boolean) || fieldType == typeof(Boolean?))
                        {
                            if (fieldType.IsGenericType)
                                ruleFilters.Append("{field:'" + sysField.Name + "',type:'checkbox',op:['equal','notequal','isnull','isnotnull']},");
                            else
                                ruleFilters.Append("{field:'" + sysField.Name + "',type:'checkbox',op:['equal','notequal']},");
                        }
                        else if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?))
                        {
                            if (fieldType.IsGenericType)
                                ruleFilters.Append("{field:'" + sysField.Name + "',type:'datebox',op:['less','greater','isnull','isnotnull']},");
                            else
                                ruleFilters.Append("{field:'" + sysField.Name + "',type:'datebox',op:['less','greater']},");
                        }
                    }
                }
                else //基类字段
                {
                    if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?))
                    {
                        ruleFilters.Append("{field:'" + sysField.Name + "',type:'datebox',op:['less','greater']},");
                    }
                }
            }
            return ruleFilters;
        }

        #endregion

        #region 视图

        /// <summary>
        /// 获取模块所有视图
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_Grid> GetGrids(Guid moduleId)
        {
            string errMsg = string.Empty;
            List<Sys_Grid> list = CommonOperate.GetEntities<Sys_Grid>(out errMsg, x => x.Sys_ModuleId == moduleId && !x.IsDeleted);
            if (list == null) list = new List<Model.Sys.Sys_Grid>();
            return list;
        }

        /// <summary>
        /// 获取默认视图
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public static Sys_Grid GetDefaultGrid(Guid moduleId)
        {
            string errMsg = string.Empty;
            Sys_Grid grid = CommonOperate.GetEntity<Sys_Grid>(x => x.Sys_ModuleId == moduleId && !x.IsDeleted && x.IsDefault, null, out errMsg);
            return grid;
        }

        /// <summary>
        /// 取用户默认列表视图
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static Sys_Grid GetUserDefaultGrid(Guid userId, Guid moduleId)
        {
            string errMsg = string.Empty;
            Sys_Grid grid = null;
            List<Sys_Grid> grids = CommonOperate.GetEntities<Sys_Grid>(out errMsg, x => x.Sys_ModuleId == moduleId && !x.IsDeleted);
            List<Sys_UserGrid> userGrids = CommonOperate.GetEntities<Sys_UserGrid>(out errMsg, x => x.Sys_UserId == userId);
            if (userGrids != null && userGrids.Count > 0)
            {
                List<Guid> gridIds = grids != null ? grids.Select(x => x.Id).ToList() : new List<Guid>();
                Sys_UserGrid userGrid = userGrids.Where(x => x.Sys_GridId != null && gridIds.Contains(x.Sys_GridId.Value) && x.IsDefault && !x.IsDeleted).FirstOrDefault();
                if (userGrid != null)
                {
                    Guid gridId = userGrid.Sys_GridId.Value;
                    grid = grids.Where(x => x.Id == gridId).FirstOrDefault();
                }
            }
            if (grid == null)
            {
                grid = grids.Where(x => x.IsDefault).FirstOrDefault();
            }
            return grid;
        }

        /// <summary>
        /// 取用户自己创建的列表视图
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_Grid> GetUserGrids(Guid userId, Guid moduleId)
        {
            string errMsg = string.Empty;
            List<Sys_UserGrid> userGrids = CommonOperate.GetEntities<Sys_UserGrid>(out errMsg, x => x.Sys_UserId == userId && !x.IsDeleted);
            if (userGrids != null)
            {
                List<Guid> gridIds = userGrids.Select(x => x.Sys_GridId.Value).ToList();
                List<Sys_Grid> grids = GetGrids(moduleId);
                if (grids != null)
                {
                    return grids.Where(x => gridIds.Contains(x.Id)).ToList();
                }
            }
            return new List<Sys_Grid>();
        }

        /// <summary>
        /// 获取系统列表视图
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_Grid> GetSystemGrids(Guid moduleId)
        {
            string errMsg = string.Empty;
            List<Sys_Grid> grids = GetGrids(moduleId);
            List<Sys_UserGrid> userGrids = CommonOperate.GetEntities<Sys_UserGrid>(out errMsg);
            if (userGrids != null)
            {
                List<Guid> gridIds = userGrids.Select(x => x.Sys_GridId.Value).ToList();
                return grids.Where(x => !gridIds.Contains(x.Id)).ToList();
            }
            return grids;
        }

        /// <summary>
        /// 是否是用户视图
        /// </summary>
        /// <param name="viewId">视图Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static bool IsUserGridView(Guid viewId, Guid userId)
        {
            string errMsg = string.Empty;
            List<Sys_UserGrid> userGrids = CommonOperate.GetEntities<Sys_UserGrid>(out errMsg, x => x.Sys_GridId == viewId && x.Sys_UserId == userId);
            return userGrids != null && userGrids.Count > 0;
        }

        /// <summary>
        /// 判断是否为默认视图
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="viewId">视图Id</param>
        /// <returns></returns>
        public static bool IsUserDefaultGridView(Guid userId, Guid viewId)
        {
            string errMsg = string.Empty;
            Sys_UserGrid userGrid = CommonOperate.GetEntity<Sys_UserGrid>(x => x.Sys_UserId == userId && x.Sys_GridId == viewId && x.IsDefault, null, out errMsg);
            return userGrid != null;
        }

        /// <summary>
        /// 删除用户视图
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="viewId">视图Id</param>
        /// <returns>返回异常信息</returns>
        public static string DeleteUserGrid(Guid userId, Guid viewId)
        {
            string errMsg = string.Empty;
            Sys_UserGrid userGrid = CommonOperate.GetEntity<Sys_UserGrid>(x => x.Sys_UserId == userId && x.Sys_GridId == viewId && x.IsDefault, null, out errMsg);
            //删除用户视图
            if (userGrid != null)
            {
                var ids = new List<Guid>() { userGrid.Id };
                bool rs = CommonOperate.DeleteRecords<Sys_UserGrid>(ids, out errMsg);
                if (!rs) return errMsg;
            }
            //删除视图
            bool delRs = CommonOperate.DeleteRecords<Sys_Grid>(new List<Guid>() { viewId }, out errMsg);
            //删除视图明细
            if (delRs)
            {
                CommonOperate.DeleteRecordsByExpression<Sys_GridField>(x => x.Sys_GridId == viewId, out errMsg);
            }
            return errMsg;
        }

        /// <summary>
        /// 获取视图
        /// </summary>
        /// <param name="id">视图Id</param>
        /// <returns></returns>
        public static Sys_Grid GetGrid(Guid id)
        {
            string errMsg = string.Empty;
            Sys_Grid grid = CommonOperate.GetEntityById<Sys_Grid>(id, out errMsg);
            return grid;
        }

        #endregion

        #region 视图按钮

        /// <summary>
        /// 获取视图按钮
        /// </summary>
        /// <param name="btnId">按钮Id</param>
        /// <returns></returns>
        public static Sys_GridButton GetGridButton(Guid btnId)
        {
            string errMsg = string.Empty;
            Sys_GridButton btn = CommonOperate.GetEntityById<Sys_GridButton>(btnId, out errMsg);
            return btn;
        }

        /// <summary>
        /// 获取视图按钮
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="btnText">按钮显示文本</param>
        /// <returns></returns>
        public static Sys_GridButton GetGridButton(Guid moduleId, string btnText)
        {
            return GetGridButtons(moduleId).Where(x => x.ButtonText == btnText).FirstOrDefault();
        }

        /// <summary>
        /// 获取列表工具栏所有按钮
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_GridButton> GetGridButtons(Guid moduleId)
        {
            string errMsg = string.Empty;
            List<Sys_GridButton> list = CommonOperate.GetEntities<Sys_GridButton>(out errMsg, x => x.Sys_ModuleId == moduleId && x.IsValid && !x.IsDeleted, string.Empty, true, new List<string>() { "Sort" }, new List<bool>() { false });
            if (list == null) list = new List<Sys_GridButton>();
            return list;
        }

        /// <summary>
        /// 获取顶级按钮
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_GridButton> GetTopButtons(Guid moduleId)
        {
            List<Sys_GridButton> list = GetGridButtons(moduleId).Where(x => x.ParentId == null || x.ParentId == Guid.Empty).ToList();
            if (list == null) list = new List<Sys_GridButton>();
            return list;
        }

        /// <summary>
        /// 获取模块的文件菜单按钮
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_GridButton> GetFileMenuButtons(Guid moduleId)
        {
            List<Sys_GridButton> list = GetGridButtons(moduleId);
            List<Sys_GridButton> tempList = new List<Sys_GridButton>();
            foreach (Sys_GridButton btn in list)
            {
                var childBtns = list.Where(x => x.ParentId == btn.Id).ToList();
                if (childBtns != null && childBtns.Count > 0 && btn.OperateButtonType == (int)OperateButtonTypeEnum.FileMenuButton)
                {
                    tempList.Add(btn);
                }
            }
            return tempList;
        }

        /// <summary>
        /// 是否是文件菜单按钮
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="btnId">按钮Id</param>
        /// <returns></returns>
        public static bool IsFileMenuButton(Guid moduleId, Guid btnId)
        {
            Sys_GridButton btn = GetGridButton(btnId);
            return btn.OperateButtonType == (int)OperateButtonTypeEnum.FileMenuButton && HasChildButton(moduleId, btnId);
        }

        /// <summary>
        /// 获取按钮的所有子按钮
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="btnId">按钮Id</param>
        /// <returns></returns>
        public static List<Sys_GridButton> GetChildButtons(Guid moduleId, Guid btnId)
        {
            List<Sys_GridButton> list = GetGridButtons(moduleId);
            if (list != null)
            {
                return list.Where(x => x.ParentId == btnId).ToList();
            }
            return new List<Sys_GridButton>();
        }

        /// <summary>
        /// 获取网格行头按钮集合，在网格行头显示的按钮
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_GridButton> GetGridHeadButtons(Guid moduleId)
        {
            List<Sys_GridButton> list = GetGridButtons(moduleId);
            if (list != null)
            {
                return list.Where(x => x.GridButtonLocation == (int)GridButtonLocationEnum.RowHead).ToList();
            }
            return new List<Sys_GridButton>();
        }

        /// <summary>
        /// 是否有子按钮
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="btnId">按钮Id</param>
        /// <returns></returns>
        public static bool HasChildButton(Guid moduleId, Guid btnId)
        {
            List<Sys_GridButton> list = GetChildButtons(moduleId, btnId);
            return list.Count > 0;
        }

        #endregion

        #region 格式化

        /// <summary>
        /// 视图字段格式化
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="sysField">字段</param>
        /// <param name="gridId">网格domId</param>
        /// <param name="isAllowFieldEdit">允许网格字段编辑</param>
        /// <param name="otherFormatParams">其他格式化参数，主要针对主键字段</param>
        /// <param name="foreignFormatParams">外键格式化参数，针对外键字段</param>
        /// <param name="gridFieldName">网格字段名称</param>
        /// <returns></returns>
        public static string GetGridFormatFunction(Guid moduleId, Sys_Field sysField, string gridId = null, bool isAllowFieldEdit = false, string otherFormatParams = null, string foreignFormatParams = null, string gridFieldName = null)
        {
            Sys_Module module = GetModuleById(moduleId);
            return GetGridFormatFunction(module, sysField, gridId, isAllowFieldEdit, otherFormatParams, foreignFormatParams, gridFieldName);
        }

        /// <summary>
        /// 视图字段格式化
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="sysField">字段</param>
        /// <param name="gridId">网格domId</param>
        /// <param name="isAllowFieldEdit">允许网格字段编辑</param>
        /// <param name="otherFormatParams">其他格式化参数，主要针对主键字段</param>
        /// <param name="foreignFormatParams">外键格式化参数，针对外键字段</param>
        /// <param name="gridFieldName">网格字段名称</param>
        /// <returns></returns>
        public static string GetGridFormatFunction(Sys_Module module, Sys_Field sysField, string gridId = null, bool isAllowFieldEdit = false, string otherFormatParams = null, string foreignFormatParams = null, string gridFieldName = null)
        {
            if (module == null || sysField == null) return string.Empty;
            Sys_FormField field = GetDefaultFormSingleField(module.Id, sysField.Name);
            StringBuilder sb = new StringBuilder();
            if (module == null || sysField == null) return string.Empty;
            Type modelType = CommonOperate.GetModelType(module.TableName); //实体类型
            bool isForeignKey = !string.IsNullOrEmpty(sysField.ForeignModuleName); //IsForeignField(sysField); //是否外键字段
            bool isEnum = IsEnumField(modelType, sysField.Name); //是否枚举字段
            bool isDic = IsDictionaryBindField(module.Id, sysField.Name); //是否字典字段
            Type fieldType = GetFieldType(modelType, sysField.Name); //字段类型
            string paramsObj = string.Empty; //字段参数对象，列表字段编辑时用到
            if (isAllowFieldEdit && field != null && field.IsAllowEdit.HasValue && field.IsAllowEdit.Value && field.IsAllowBatchEdit.HasValue && field.IsAllowBatchEdit.Value) //允许编辑
            {
                int w = 300;
                if (field.Sys_FormId.HasValue)
                {
                    Sys_Form form = GetForm(field.Sys_FormId.Value);
                    w = field.Width.HasValue && field.Width.Value > 0 ? field.Width.Value : 180;
                    w += form.LabelWidth > 0 ? form.LabelWidth : 90;
                    w += form.SpaceWidth > 0 ? form.SpaceWidth : 40;
                }
                paramsObj = "{moduleId:'" + module.Id + "',fieldName:'" + sysField.Name + "',fieldDisplay:'" + sysField.Display + "',fieldWidth:" + w + ",gridId:'" + gridId.ObjToStr() + "'}";
                paramsObj = HttpUtility.UrlEncode(paramsObj, Encoding.UTF8).Replace("+", "%20");
            }
            string formatFunction = string.Empty;
            if (module.TitleKey == sysField.Name) //是主键
            {
                string otherParams = otherFormatParams == null ? string.Empty : otherFormatParams;
                formatFunction = string.Format("return TitleKeyFormatter(value, row, index,'{0}','{1}', '{2}','{3}');", module.Name, sysField.Name, paramsObj, otherParams);
            }
            else if (isForeignKey) //外键
            {
                Sys_Module foreignModule = GetForeignModule(sysField);
                if (foreignModule != null)
                {
                    string otherParams = foreignFormatParams == null ? string.Empty : foreignFormatParams;
                    string tempFieldName = string.IsNullOrEmpty(gridFieldName) ? sysField.Name : gridFieldName;
                    formatFunction = string.Format("return ForeignKeyFormatter(value, row, index, '{0}', '{1}','{2}','{3}','{4}');", module.Name, tempFieldName, foreignModule.Name, paramsObj, otherParams);
                }
            }
            else if (isEnum) //枚举字段
            {
                object dic = EnumFieldDataJson(module.Id, sysField.Name);
                string json = HttpUtility.UrlEncode(JsonHelper.Serialize(dic), Encoding.UTF8).Replace("+", "%20"); ;
                formatFunction = string.Format("return EnumFieldFormatter(value, row, index, '{0}', '{1}','{2}','{3}');", module.Name, sysField.Name, json, paramsObj);
            }
            else if (isDic) //字典字段
            {
                object dic = DictionaryDataJson(module.Id, sysField.Name);
                string json = HttpUtility.UrlEncode(JsonHelper.Serialize(dic), Encoding.UTF8).Replace("+", "%20"); ;
                formatFunction = string.Format("return DicFieldFormatter(value, row, index, '{0}', '{1}','{2}','{3}');", module.Name, sysField.Name, json, paramsObj);
            }
            else if (fieldType == typeof(DateTime) || fieldType == typeof(DateTime?)) //日期类型
            {
                string format = field != null && field.ControlTypeOfEnum == ControlTypeEnum.DateTimeBox ? "yyyy-MM-dd HH:mm:ss" : "yyyy-MM-dd";
                formatFunction = string.Format("return DateFormatter(value, row, index, '{0}', '{1}','{2}','{3}');", module.Name, sysField.Name, format, paramsObj);
            }
            else if (fieldType == typeof(Boolean) || fieldType == typeof(Boolean?)) //bool类型
            {
                formatFunction = string.Format("return BoolFormatter(value, row, index,'{0}','{1}','{2}');", module.Name, sysField.Name, paramsObj);
            }
            else if (field != null && field.ControlTypeOfEnum == ControlTypeEnum.MutiCheckBox) //多选CheckBox格式化
            {
                formatFunction = string.Format("return MutiCheckBoxFormatter(value, row, index,'{0}','{1}','{2}','{3}');", module.Name, sysField.Name, field.TextField.ObjToStr(), paramsObj);
            }
            else
            {
                formatFunction = string.Format("return GeneralFormatter(value, row, index,'{0}','{1}','{2}')", module.Name, sysField.Name, paramsObj);
            }
            if (string.IsNullOrEmpty(formatFunction)) return null;
            sb.Append("function(value, row, index)");
            sb.Append("{");
            sb.Append(formatFunction);
            sb.Append("}");
            return sb.ToString();
        }

        /// <summary>
        /// 视图字段格式化
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="gridId">网格domId</param>
        /// <param name="isAllowFieldEdit">允许网格字段编辑</param>
        /// <param name="otherFormatParams">其他格式化参数，主要针对主键字段</param>
        /// <param name="foreignFormatParams">外键格式化参数，针对外键字段</param>
        /// <param name="gridFieldName">网格字段名称</param>
        /// <returns></returns>
        public static string GetGridFormatFunction(Guid moduleId, string fieldName, string gridId = null, bool isAllowFieldEdit = false, string otherFormatParams = null, string foreignFormatParams = null, string gridFieldName = null)
        {
            Sys_Field sysField = GetFieldInfo(moduleId, fieldName);
            string formatString = GetGridFormatFunction(moduleId, sysField, gridId, isAllowFieldEdit, otherFormatParams, foreignFormatParams, gridFieldName);
            return formatString;
        }

        /// <summary>
        /// 获取字段编辑器
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static string GetFieldEditor(Sys_Module module, string fieldName)
        {
            Sys_Field sysField = GetFieldInfo(module.Id, fieldName);
            return GetFieldEditor(module, sysField);
        }

        /// <summary>
        /// 获取字段编辑器
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="sysField">字段</param>
        /// <returns></returns>
        public static string GetFieldEditor(Sys_Module module, Sys_Field sysField)
        {
            if (module == null || sysField == null) return string.Empty;
            Sys_FormField formField = GetDefaultFormSingleField(module.Id, sysField.Name);
            if (formField == null || !formField.IsAllowEdit.HasValue || !formField.IsAllowEdit.Value)
                return string.Empty;
            return GetFieldEditor(module, sysField, formField);
        }

        /// <summary>
        /// 获取字段编辑器
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="sysField">字段</param>
        /// <param name="formField">表单字段</param>
        /// <returns></returns>
        public static string GetFieldEditor(Sys_Module module, Sys_Field sysField, Sys_FormField formField)
        {
            if (module == null || sysField == null) return string.Empty;
            if (formField == null || !formField.IsAllowEdit.HasValue || !formField.IsAllowEdit.Value)
                return string.Empty;
            StringBuilder sb = new StringBuilder();
            string valueField = formField.ValueField;
            string textField = formField.TextField;
            string fieldUrl = formField.UrlOrData;
            #region 外键字段、枚举字段、字典字段处理
            if (string.IsNullOrEmpty(formField.UrlOrData))
            {
                //外键字段
                if (SystemOperate.IsForeignField(sysField))
                {
                    valueField = "Id";
                    textField = "Name";
                    fieldUrl = string.Format("/{0}/BindForeignFieldComboData.html?moduleId={1}&fieldName={2}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, module.Id, formField.Sys_FieldName);
                }
                else if (SystemOperate.IsEnumField(module.Id, formField.Sys_FieldName)) //枚举字段
                {
                    valueField = "Id";
                    textField = "Name";
                    fieldUrl = string.Format("/{0}/BindEnumFieldData.html?moduleId={1}&fieldName={2}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, module.Id, formField.Sys_FieldName);
                }
                else if (SystemOperate.IsDictionaryBindField(module.Id, formField.Sys_FieldName)) //字典绑定字段
                {
                    valueField = "Id";
                    textField = "Name";
                    fieldUrl = string.Format("/{0}/BindDictionaryData.html?moduleId={1}&fieldName={2}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, module.Id, formField.Sys_FieldName);
                }
            }
            #endregion
            #region 验证处理
            string options = "onChange:function(newValue,oldValue){if(typeof(OnFieldValueChanged)=='function'){OnFieldValueChanged({moduleId:'" + module.Id + "',moduleName:'" + module.Name + "'},'" + formField.Sys_FieldName + "',newValue,oldValue);}}";
            //必填性验证
            if (formField.IsRequired.HasValue && formField.IsRequired.Value)
            {
                options += ",required:true";
            }
            //字符长度验证
            string validTypeStr = string.Empty;
            if (formField.MinCharLen.HasValue && formField.MinCharLen.Value > 0 && formField.MaxCharLen.HasValue && formField.MaxCharLen.Value > 0)
            {
                validTypeStr = string.Format("'length[{0},{1}'", formField.MinCharLen.Value, formField.MaxCharLen.Value);
            }
            else if (formField.MinCharLen.HasValue && formField.MinCharLen.Value > 0)
            {
                validTypeStr = string.Format("'minLength[{0}]'", formField.MinCharLen.Value);
            }
            else if (formField.MaxCharLen.HasValue && formField.MaxCharLen.Value > 0)
            {
                validTypeStr = string.Format("'maxLength:[{0}]'", formField.MaxCharLen.Value);
            }
            //其他验证类型
            switch (formField.ValidateTypeOfEnum)
            {
                case ValidateTypeEnum.email:
                    validTypeStr += ",'email'";
                    break;
                case ValidateTypeEnum.url:
                    validTypeStr = ",'url'";
                    break;
                case ValidateTypeEnum.intNum:
                    validTypeStr = ",'int'";
                    break;
                case ValidateTypeEnum.floatNum:
                    validTypeStr = ",'float'";
                    break;
            }
            if (!string.IsNullOrEmpty(validTypeStr))
            {
                options += string.Format(",validType:[{0}]", validTypeStr);
            }
            #endregion
            #region 控件处理
            switch (formField.ControlTypeOfEnum)
            {
                case ControlTypeEnum.TextBox:
                case ControlTypeEnum.TextAreaBox:
                    {
                        sb.Append("{type:'textbox',options:{");
                        if (formField.ControlTypeOfEnum == ControlTypeEnum.TextAreaBox)
                            options += ",multiline:true";
                        options += string.Format(",value:'{0}'", formField.DefaultValue);
                        sb.Append(options);
                        sb.Append("}}");
                    }
                    break;
                case ControlTypeEnum.IntegerBox:
                case ControlTypeEnum.NumberBox:
                    {
                        sb.Append("{type:'numberbox',options:{");
                        int precision = sysField.Precision.HasValue ? sysField.Precision.Value : 2;
                        if (formField.ControlTypeOfEnum == ControlTypeEnum.IntegerBox) precision = 0;
                        options += string.Format(",precision:{0}", precision);
                        if (formField.MinValue.HasValue)
                            options += string.Format(",min:{0}", formField.MinValue.Value);
                        if (formField.MaxValue.HasValue)
                            options += string.Format(",max:{0}", formField.MaxValue.Value);
                        if (formField.DefaultValue.ObjToDouble() > 0)
                            options += string.Format(",value:'{0}'", formField.DefaultValue);
                        sb.Append(options);
                        sb.Append("}}");
                    }
                    break;
                case ControlTypeEnum.SingleCheckBox:
                    {
                        sb.Append("{type:'checkbox',options:{on:'1',off:'0'}}");
                    }
                    break;
                case ControlTypeEnum.DateBox:
                case ControlTypeEnum.DateTimeBox:
                    {
                        string boxType = "datebox";
                        if (formField.ControlTypeOfEnum == ControlTypeEnum.DateTimeBox)
                            boxType = "datetimebox";
                        sb.Append("{type:'" + boxType + "',options:{");
                        options += string.Format(",value:'{0}'", formField.DefaultValue);
                        sb.Append(options);
                        sb.Append("}}");
                    }
                    break;
                case ControlTypeEnum.ComboBox:
                    {
                        sb.Append("{type:'combobox',options:{");
                        options += string.Format(",valueField:'{0}',textField:'{1}',url:'{2}'", valueField, textField, fieldUrl);
                        options += string.Format(",value:'{0}'", formField.DefaultValue);
                        sb.Append(options);
                        sb.Append("}}");
                    }
                    break;
                case ControlTypeEnum.IconBox: //图标控件
                    {
                    }
                    break;
            }
            #endregion
            return sb.ToString();
        }

        #endregion

        #endregion

        #region 表单

        #region 表单字段

        /// <summary>
        /// 获取默认表单单个字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static Sys_FormField GetDefaultFormSingleField(Guid moduleId, string fieldName)
        {
            List<Sys_FormField> fields = GetDefaultFormField(moduleId);
            if (fields != null && fields.Count > 0)
            {
                return fields.Where(x => x.Sys_FieldName == fieldName).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 获取默认表单单个字段
        /// </summary>
        /// <param name="sysField">字段信息</param>
        /// <returns></returns>
        public static Sys_FormField GetDefaultFormSingleField(Sys_Field sysField)
        {
            if (sysField == null || !sysField.Sys_ModuleId.HasValue) return null;
            return GetDefaultFormSingleField(sysField.Sys_ModuleId.Value, sysField.Name);
        }

        /// <summary>
        /// 获取默认表单单个字段
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static Sys_FormField GetDefaultFormSingleField(string moduleName, string fieldName)
        {
            Guid moduleId = GetModuleIdByName(moduleName);
            return GetDefaultFormSingleField(moduleId, fieldName);
        }

        /// <summary>
        /// 获取各种类型表单的默认表单字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_FormField> GetDefaultFormField(Guid moduleId)
        {
            //获取表单
            Sys_Form form = GetDefaultForm(moduleId);
            if (form == null) return new List<Sys_FormField>();
            if (form.FormFields != null && form.FormFields.Count > 0)
                return form.FormFields;
            List<Sys_FormField> formFields = GetFormField(moduleId, form.Name);
            if (formFields.Count > 0)
            {
                form.FormFields = formFields;
            }
            return formFields;
        }

        /// <summary>
        /// 获取表单字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="formName">表单名称</param>
        /// <returns></returns>
        public static List<Sys_FormField> GetFormField(Guid moduleId, string formName)
        {
            //获取表单
            Sys_Form form = GetForm(moduleId, formName);
            if (form == null) return new List<Sys_FormField>();
            if (form.FormFields != null && form.FormFields.Count > 0)
                return form.FormFields;
            //获表单表字段
            List<Sys_FormField> formFields = GetFormField(form.Id);
            if (formFields.Count > 0)
            {
                form.FormFields = formFields;
            }
            return formFields;
        }

        /// <summary>
        /// 获取表单字段
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="formName">表单名称</param>
        /// <returns></returns>
        public static List<Sys_FormField> GetFormField(string moduleName, string formName)
        {
            //获取表单
            Sys_Form form = GetForm(moduleName, formName);
            if (form == null) return new List<Sys_FormField>();
            if (form.FormFields != null && form.FormFields.Count > 0)
                return form.FormFields;
            //获表单表字段
            List<Sys_FormField> formFields = GetFormField(form.Id);
            if (formFields.Count > 0)
            {
                form.FormFields = formFields;
            }
            return formFields;
        }

        /// <summary>
        /// 获取表单字段
        /// </summary>
        /// <param name="formId">表单id</param>
        /// <returns></returns>
        public static List<Sys_FormField> GetFormField(Guid formId)
        {
            string errMsg = string.Empty;
            //获表单表字段
            List<Sys_FormField> formFields = CommonOperate.GetEntities<Sys_FormField>(out errMsg, x => x.Sys_FormId == formId && x.IsVisible == true);
            if (formFields == null) formFields = new List<Sys_FormField>();
            formFields = formFields.OrderBy(x => x.ColNo).OrderBy(x => x.RowNo).ToList();
            return formFields;
        }

        /// <summary>
        /// 获取用户表单字段
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_FormField> GetUserFormFields(Guid userId, Guid moduleId)
        {
            Sys_Form form = GetUserForm(userId, moduleId);
            if (form != null)
            {
                if (form.FormFields != null && form.FormFields.Count > 0)
                    return form.FormFields;
                List<Sys_FormField> formFields = GetFormField(form.Id);
                if (formFields.Count > 0)
                {
                    form.FormFields = formFields;
                }
                return formFields;
            }
            return new List<Sys_FormField>();
        }

        /// <summary>
        /// 获取角色表单字段集合
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_FormField> GetRoleFormFields(Guid roleId, Guid moduleId)
        {
            Sys_Form form = GetRoleForm(roleId, moduleId);
            if (form != null)
            {
                if (form.FormFields != null && form.FormFields.Count > 0)
                    return form.FormFields;
                List<Sys_FormField> formFields = GetFormField(form.Id);
                if (formFields.Count > 0)
                {
                    form.FormFields = formFields;
                }
                return formFields;
            }
            return new List<Sys_FormField>();
        }

        /// <summary>
        /// 获取单个用户表单字段
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="sysField">字段信息</param>
        /// <returns></returns>
        public static Sys_FormField GetUserFormField(Guid userId, Guid moduleId, Sys_Field sysField)
        {
            if (sysField == null) return null;
            List<Sys_FormField> list = GetUserFormFields(userId, moduleId);
            return list.Where(x => x.Sys_FieldId == sysField.Id).FirstOrDefault();
        }

        /// <summary>
        /// 获取单个角色表单字段
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="sysField">字段信息</param>
        /// <returns></returns>
        public static Sys_FormField GetRoleFormField(Guid roleId, Guid moduleId, Sys_Field sysField)
        {
            if (sysField == null) return null;
            List<Sys_FormField> list = GetRoleFormFields(roleId, moduleId);
            return list.Where(x => x.Sys_FieldId == sysField.Id).FirstOrDefault();
        }

        /// <summary>
        /// 获取搜索表单字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_FormField> GetSearchFormField(Guid moduleId)
        {
            //先取搜索视图字段
            List<Sys_GridField> searchFields = GetDefaultSearchGridFields(moduleId);
            //找到对应的表单字段
            List<Guid> fieldIds = searchFields.Select(x => x.Sys_FieldId.Value).ToList();
            List<Sys_FormField> formFields = GetDefaultFormField(moduleId);
            List<Sys_FormField> list = new List<Sys_FormField>();
            //按搜索视图字段的顺序
            foreach (Guid fieldId in fieldIds)
            {
                Sys_FormField formField = formFields.Where(x => x.Sys_FieldId.Value == fieldId).FirstOrDefault();
                if (formField != null)
                {
                    list.Add(formField);
                }
            }
            return list;
        }

        /// <summary>
        /// 获取视图对应的表单字段
        /// </summary>
        /// <param name="viewId"></param>
        /// <returns></returns>
        public static List<Sys_FormField> GetSearchFormFieldOfView(Guid viewId)
        {
            Sys_Grid grid = GetGrid(viewId);
            if (grid == null || !grid.Sys_ModuleId.HasValue) return new List<Sys_FormField>();
            List<Sys_FormField> list = new List<Sys_FormField>();
            //先取搜索视图字段
            List<Sys_GridField> searchFields = GetSearchGridFields(viewId);
            foreach (Sys_GridField gridField in searchFields)
            {
                if (!gridField.Sys_FieldId.HasValue) continue;
                Sys_Field sysField = GetFieldById(gridField.Sys_FieldId.Value);
                if (sysField == null) continue;
                Sys_FormField formField = GetDefaultFormSingleField(sysField);
                if (formField == null) continue;
                list.Add(formField);
            }
            return list;
        }

        /// <summary>
        /// 格式化表单字段
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns>返回格式化后的字段</returns>
        public static Sys_FormField FormatFormField(Sys_Module module, string fieldName)
        {
            Sys_FormField field = SystemOperate.GetDefaultFormSingleField(module.Id, fieldName);
            Sys_Field sysField = SystemOperate.GetFieldById(field.Sys_FieldId.Value);
            Sys_Module foreignModule = SystemOperate.GetForeignModule(sysField);
            #region 外键字段、枚举字段、字典字段处理
            string valueField = string.IsNullOrWhiteSpace(field.ValueField) ? "Id" : field.ValueField;
            string textField = string.IsNullOrWhiteSpace(field.TextField) ? "Name" : field.TextField;
            string fieldUrl = field.UrlOrData;
            if (field.UrlOrData == null && field.ControlTypeOfEnum == ControlTypeEnum.ComboBox)
            {
                //外键字段
                if (SystemOperate.IsForeignField(sysField))
                {
                    valueField = "Id";
                    textField = "Name";
                    fieldUrl = string.Format("/{0}/BindForeignFieldComboData.html?moduleId={1}&fieldName={2}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, module.Id, fieldName);
                }
                else if (SystemOperate.IsEnumField(module.Id, field.Sys_FieldName)) //枚举字段
                {
                    valueField = "Id";
                    textField = "Name";
                    fieldUrl = string.Format("/{0}/BindEnumFieldData.html?moduleId={1}&fieldName={2}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, module.Id, fieldName);
                }
                else if (SystemOperate.IsDictionaryBindField(module.Id, field.Sys_FieldName)) //字典绑定字段
                {
                    valueField = "Id";
                    textField = "Name";
                    fieldUrl = string.Format("/{0}/BindDictionaryData.html?moduleId={1}&fieldName={2}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, module.Id, fieldName);
                }
            }
            else if (foreignModule != null)
            {
                if (field.ControlTypeOfEnum == ControlTypeEnum.ComboTree)
                {
                    if (string.IsNullOrEmpty(valueField)) valueField = "id";
                    if (string.IsNullOrEmpty(textField)) textField = "text";
                    if (fieldUrl == null && foreignModule != null)
                    {
                        fieldUrl = string.Format("/{0}/GetTreeByNode.html?moduleId={1}", GlobalConst.ASYNC_DATA_CONTROLLER_NAME, foreignModule.Id);
                    }
                }
                else
                {
                    fieldUrl = string.Format("/Page/Grid.html?page=fdGrid&moduleId={0}&initModule={1}&initField={2}", foreignModule.Id, HttpUtility.UrlEncode(module.Name), fieldName);
                    if (string.IsNullOrEmpty(textField)) textField = SystemOperate.GetModuleTitleKey(foreignModule.Id);
                }
            }
            #endregion
            Sys_FormField tempField = new Sys_FormField();
            ObjectHelper.CopyValue(field, tempField);
            tempField.ValueField = valueField;
            tempField.TextField = textField;
            tempField.UrlOrData = fieldUrl;
            return tempField;
        }

        /// <summary>
        /// 获取默认表单批量编辑字段集合
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_FormField> GetDefaultBatchEditFields(Guid moduleId)
        {
            List<Sys_FormField> list = GetDefaultFormField(moduleId);
            if (list != null && list.Count > 0)
            {
                Sys_Module module = GetModuleById(moduleId);
                list = list.Where(x => x.Sys_FieldId.HasValue && x.IsAllowBatchEdit.HasValue && x.IsAllowBatchEdit.Value && x.Sys_FieldName != "Id" && x.Sys_FieldName != module.TitleKey).ToList();
                list = list.OrderBy(x => x.ColNo).OrderBy(x => x.RowNo).ToList();
            }
            if (list == null) list = new List<Sys_FormField>();
            return list;
        }

        /// <summary>
        /// 获取表单批量编辑字段集合
        /// </summary>
        /// <param name="formId">表单Id</param>
        /// <returns></returns>
        public static List<Sys_FormField> GetBatchEditFields(Guid formId)
        {
            List<Sys_FormField> list = GetFormField(formId);
            if (list != null && list.Count > 0)
            {
                list = list.Where(x => x.IsAllowBatchEdit.HasValue && x.IsAllowBatchEdit.Value).ToList();
                list = list.OrderBy(x => x.ColNo).OrderBy(x => x.RowNo).ToList();
            }
            if (list == null) list = new List<Sys_FormField>();
            return list;
        }

        /// <summary>
        /// 获取唯一性验证字段集合
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_FormField> GetUniqueVerifyFields(Guid moduleId)
        {
            string errMsg = string.Empty;
            List<Sys_FormField> list = CommonOperate.GetEntities<Sys_FormField>(out errMsg, x => x.IsUnique == true && !x.IsDeleted);
            if (list == null) list = new List<Sys_FormField>();
            return list;
        }

        #endregion

        #region 表单

        /// <summary>
        /// 获取默认表单
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static Sys_Form GetDefaultForm(Guid moduleId)
        {
            string errMsg = string.Empty;
            //获取表单
            Sys_Form form = CommonOperate.GetEntity<Sys_Form>(x => x.Sys_ModuleId == moduleId && x.IsDefault, string.Empty, out errMsg);
            return form;
        }

        /// <summary>
        /// 获取模块表单
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Sys_Form> GetModuleForms(Guid moduleId)
        {
            string errMsg = string.Empty;
            List<Sys_Form> forms = CommonOperate.GetEntities<Sys_Form>(out errMsg, x => x.Sys_ModuleId == moduleId && !x.IsDeleted);
            if (forms == null) forms = new List<Sys_Form>();
            return forms;
        }

        /// <summary>
        /// 获取表单
        /// </summary>
        /// <param name="formId">表单id</param>
        /// <returns></returns>
        public static Sys_Form GetForm(Guid formId)
        {
            string errMsg = string.Empty;
            Sys_Form form = CommonOperate.GetEntityById<Sys_Form>(formId, out errMsg);
            return form;
        }

        /// <summary>
        /// 获取表单
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="formName">表单名称</param>
        /// <returns></returns>
        public static Sys_Form GetForm(Guid moduleId, string formName)
        {
            string errMsg = string.Empty;
            //获取表单
            Sys_Form form = CommonOperate.GetEntity<Sys_Form>(x => x.Sys_ModuleId == moduleId && x.Name == formName, string.Empty, out errMsg);
            return form;
        }

        /// <summary>
        /// 获取表单
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="formName">表单名称</param>
        /// <returns></returns>
        public static Sys_Form GetForm(string moduleName, string formName)
        {
            Guid moduleId = GetModuleIdByName(moduleName);
            return GetForm(moduleId, formName);
        }

        /// <summary>
        /// 获取角色表单
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static Sys_Form GetRoleForm(Guid roleId, Guid moduleId)
        {
            Sys_Form form = null;
            string errMsg = string.Empty;
            Sys_RoleForm roleForm = CommonOperate.GetEntity<Sys_RoleForm>(x => x.Sys_RoleId == roleId && x.Sys_ModuleId == moduleId && !x.IsDeleted, null, out errMsg);
            if (roleForm != null && roleForm.Sys_FormId.HasValue)
            {
                form = GetForm(roleForm.Sys_FormId.Value);
            }
            if (form == null)
            {
                form = GetDefaultForm(moduleId);
            }
            return form;
        }

        /// <summary>
        /// 获取用户表单
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static Sys_Form GetUserForm(Guid userId, Guid moduleId)
        {
            Sys_Form form = null;
            string errMsg = string.Empty;
            List<Sys_UserRole> userRoles = CommonOperate.GetEntities<Sys_UserRole>(out errMsg, x => x.Sys_UserId == userId);
            if (userRoles != null && userRoles.Count > 0)
            {
                List<Guid?> roleIds = userRoles.Select(x => x.Sys_RoleId).ToList();
                Sys_RoleForm roleForm = CommonOperate.GetEntity<Sys_RoleForm>(x => roleIds.Contains(x.Sys_RoleId) && x.Sys_ModuleId == moduleId && !x.IsDeleted, null, out errMsg);
                if (roleForm != null && roleForm.Sys_FormId.HasValue)
                {
                    form = GetForm(roleForm.Sys_FormId.Value);
                }
            }
            if (form == null)
            {
                form = GetDefaultForm(moduleId);
            }
            return form;
        }

        /// <summary>
        /// 获取用户最终表单
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="moduleId">模块ID</param>
        /// <param name="todoTaskId">待办任务ID</param>
        /// <returns></returns>
        public static Sys_Form GetUserFinalForm(Guid userId, Guid moduleId, Guid? todoTaskId = null)
        {
            Sys_Form form = null; //表单对象
            bool isEnabledFlow = BpmOperate.IsEnabledWorkflow(moduleId); //是否启用流程
            Sys_Form tempForm = SystemOperate.GetUserForm(UserInfo.CurrentUserInfo.UserId, moduleId);
            if (isEnabledFlow && tempForm != null && tempForm.Id == SystemOperate.GetDefaultForm(moduleId).Id)
            {
                Sys_Form flowNodeForm = null;
                if (todoTaskId.HasValue && todoTaskId.Value != Guid.Empty)
                {
                    if (BpmOperate.IsCurrentToDoTaskHandler(todoTaskId.Value, UserInfo.CurrentUserInfo))
                    {
                        flowNodeForm = BpmOperate.GetWorkNodeForm(BpmOperate.GetWorkNodeIdByTodoId(todoTaskId.Value));
                    }
                }
                else
                {
                    if (BpmOperate.IsAllowLaunchFlow(moduleId, UserInfo.CurrentUserInfo, null))
                    {
                        flowNodeForm = BpmOperate.GetLaunchNodeForm(moduleId);
                    }
                }
                if (flowNodeForm != null && flowNodeForm.Id != tempForm.Id)
                    form = flowNodeForm;
                else
                    form = tempForm;
            }
            else
            {
                form = tempForm;
            }
            return form;
        }

        #endregion

        #region 表单按钮

        /// <summary>
        /// 获取表单按钮
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="formType">表单类型</param>
        /// <param name="isAdd">是否新增表单</param>
        /// <param name="isDraft">是否是草稿</param>
        /// <param name="recordId">表单记录ID</param>
        /// <returns></returns>
        public static List<FormButton> GetFormButtons(Guid moduleId, FormTypeEnum formType, bool isAdd = false, bool isDraft = false, Guid? recordId = null, Guid? toDoTaskId = null)
        {
            Sys_Module module = GetModuleById(moduleId);
            return GetFormButtons(module, formType, isAdd, isDraft, recordId, toDoTaskId);
        }

        /// <summary>
        /// 获取表单按钮
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="formType">表单类型</param>
        /// <param name="isAdd">是否新增表单</param>
        /// <param name="isDraft">是否是草稿</param>
        /// <param name="recordId">表单记录ID</param>
        /// <param name="toDoTaskId">待办任务ID,针对流程表单</param>
        /// <returns></returns>
        public static List<FormButton> GetFormButtons(Sys_Module module, FormTypeEnum formType, bool isAdd = false, bool isDraft = false, Guid? recordId = null, Guid? toDoTaskId = null)
        {
            List<FormButton> btns = new List<FormButton>();
            bool isEnabledFlow = BpmOperate.IsEnabledWorkflow(module.Id); //是否启用流程
            switch (formType)
            {
                case FormTypeEnum.EditForm: //新增、编辑页面
                    {
                        if (!isEnabledFlow || (isEnabledFlow && (!toDoTaskId.HasValue || toDoTaskId.Value == Guid.Empty))) //未启用流程或启用流程并且是发起
                        {
                            bool canAdd = PermissionOperate.HasButtonPermission(UserInfo.CurrentUserInfo.UserId, module.Id, "新增");
                            bool canEdit = PermissionOperate.HasButtonPermission(UserInfo.CurrentUserInfo.UserId, module.Id, "编辑");
                            if (canAdd || canEdit) //有新增或编辑权限
                            {
                                btns.Add(new FormButton()
                                {
                                    TagId = "btnSave",
                                    DisplayText = "保存",
                                    IconType = ButtonIconType.Save,
                                    ClickMethod = "Save(this)",
                                    Icon = "eu-icon-save"
                                });
                            }
                            if (isDraft && (canAdd || canEdit)) //是否草稿，草稿时表单添加保存发布按钮
                            {
                                btns.Add(new FormButton()
                                {
                                    TagId = "btnSaveRelease",
                                    DisplayText = "保存并发布",
                                    IconType = ButtonIconType.DraftRelease,
                                    ClickMethod = "Save(this)",
                                    Icon = "eu-icon-ok"
                                });
                            }
                            else if (isAdd && canAdd && !SystemOperate.IsDetailModule(module.Id) && module.IsEnabledDraft) //启用草稿
                            {
                                btns.Add(new FormButton()
                                {
                                    TagId = "btnSaveDraft",
                                    DisplayText = "保存为草稿",
                                    IconType = ButtonIconType.SaveDraft,
                                    ClickMethod = "Save(this,null,false,true)",
                                    Icon = "icon-draft"
                                });
                            }
                            if (module.IsAllowAdd && !isDraft && canAdd)
                            {
                                btns.Add(new FormButton()
                                {
                                    TagId = "btnSaveAndAdd",
                                    DisplayText = "保存并新增",
                                    IconType = ButtonIconType.SaveAndNew,
                                    ClickMethod = "Save(this,null,true)",
                                    Icon = "eu-icon-save"
                                });
                            }
                            if (isEnabledFlow && BpmOperate.IsAllowLaunchFlow(module.Id, UserInfo.CurrentUserInfo, recordId)) //发起流程
                            {
                                btns.AddRange(BpmOperate.GetLaunchNodeFlowBtns());
                            }
                        }
                        else if (recordId.HasValue && recordId.Value != Guid.Empty)//审批流程
                        {
                            if (BpmOperate.IsCurrentToDoTaskHandler(toDoTaskId.Value, UserInfo.CurrentUserInfo))
                            {
                                btns.AddRange(BpmOperate.GetNodeFlowBtns(toDoTaskId.Value));
                            }
                        }
                    }
                    break;
                case FormTypeEnum.ViewForm: //查看页面
                    {
                        if (!isEnabledFlow || BpmOperate.GetRecordFlowStatus(module.Id, recordId) == WorkFlowStatusEnum.NoStatus)
                        {
                            bool canEdit = PermissionOperate.HasButtonPermission(UserInfo.CurrentUserInfo.UserId, module.Id, "编辑");
                            if (module.IsAllowEdit && canEdit)
                            {
                                btns.Add(new FormButton()
                                {
                                    TagId = "btnEdit",
                                    DisplayText = "编辑",
                                    IconType = ButtonIconType.Edit,
                                    ClickMethod = "ToEdit(this)",
                                    Icon = "eu-icon-edit"
                                });
                            }
                        }
                    }
                    break;
            }
            btns.Add(new FormButton()
            {
                TagId = "btnClose",
                DisplayText = "关闭",
                IconType = ButtonIconType.Close,
                ClickMethod = "CloseTab()",
                Icon = "eu-icon-close"
            });
            //调用自定义表单按钮方法
            List<FormButton> newBtns = CommonOperate.GetFormButtons(module.Id, formType, btns, isAdd, isDraft);
            if (newBtns != null && newBtns.Count > 0)
            {
                return newBtns;
            }
            return btns;
        }

        /// <summary>
        /// 获取表单工具标签按钮集合
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="formType">表单类型</param>
        /// <param name="isAdd">是否新增页面</param>
        /// <returns></returns>
        public static List<FormToolTag> GetFormToolTags(Sys_Module module, FormTypeEnum formType, bool isAdd = false)
        {
            List<FormToolTag> tags = new List<FormToolTag>();
            if (formType == FormTypeEnum.ViewForm || (formType == FormTypeEnum.EditForm && !isAdd))
            {
                tags.Add(new FormToolTag()
                {
                    TagId = "btnPreRecord",
                    Title = "上一记录",
                    ClickMethod = "PreRecord(this)",
                    Icon = "eu-p2-icon-resultset_previous"
                });
                tags.Add(new FormToolTag()
                {
                    TagId = "btnNextRecord",
                    Title = "下一记录",
                    ClickMethod = "NextRecord(this)",
                    Icon = "eu-p2-icon-resultset_next"
                });
                if (module.IsEnabledPrint && PermissionOperate.HasButtonPermission(UserInfo.CurrentUserInfo.UserId, module.Id, "打印"))
                {
                    tags.Add(new FormToolTag()
                    {
                        TagId = "btnPrint",
                        Text = "打印",
                        ClickMethod = "PrintForm(this)",
                        Icon = "eu-icon-print"
                    });
                }
            }
            //调用自定义方法
            List<FormToolTag> newTags = CommonOperate.GetFormToolTags(module.Id, formType, tags, isAdd);
            if (newTags != null && newTags.Count > 0)
            {
                return newTags;
            }
            return tags;
        }

        #endregion

        #region 编码规则

        /// <summary>
        /// 获取编码规则字段
        /// </summary>
        /// <param name="module">模块</param>
        /// <returns></returns>
        public static string GetBillCodeFieldName(Sys_Module module)
        {
            if (module == null || !module.IsEnableCodeRule)
                return string.Empty;
            string errMsg = string.Empty;
            Sys_BillCodeRule billCodeRule = CommonOperate.GetEntity<Sys_BillCodeRule>(x => x.Sys_ModuleId == module.Id && !x.IsDeleted && !x.IsDraft, null, out errMsg);
            if (billCodeRule == null || string.IsNullOrWhiteSpace(billCodeRule.FieldName))
                return string.Empty;
            return billCodeRule.FieldName;
        }

        /// <summary>
        /// 获取编码规则字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static string GetBillCodeFieldName(Guid moduleId)
        {
            Sys_Module module = GetModuleById(moduleId);
            return GetBillCodeFieldName(module);
        }

        /// <summary>
        /// 获取单据编码
        /// </summary>
        /// <param name="module">模块</param>
        /// <returns></returns>
        public static string GetBillCode(Sys_Module module)
        {
            if (module == null || !module.IsEnableCodeRule)
                return string.Empty;
            string errMsg = string.Empty;
            Sys_BillCodeRule billCodeRule = CommonOperate.GetEntity<Sys_BillCodeRule>(x => x.Sys_ModuleId == module.Id && !x.IsDeleted && !x.IsDraft, null, out errMsg);
            if (billCodeRule == null || string.IsNullOrWhiteSpace(billCodeRule.FieldName))
                return string.Empty;
            //输出参数
            Dictionary<string, object> outParams = new Dictionary<string, object>();
            outParams.Add("returnChar", "");
            //输出参数
            object inParams = new { ModuleId = module.Id };
            //执行存储过程
            CommonOperate.RunProcedureNoQuery(out errMsg, ref outParams, "[dbo].[GetBillCode]", inParams);
            return outParams["returnChar"].ObjToStr();
        }

        /// <summary>
        /// 获取单据编码
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static string GetBillCode(Guid moduleId)
        {
            Sys_Module module = GetModuleById(moduleId);
            return GetBillCode(module);
        }

        #endregion

        #region 其他

        /// <summary>
        /// 取表单的过滤条件，过滤条件为SQL时whereSql为SQL语句条件，否则为Json或条件名称时返回条件表达式
        /// </summary>
        /// <param name="whereSql">SQL条件语句</param>
        /// <param name="moduleName">原始模块名称</param>
        /// <param name="fieldName">原始字段名称</param>
        /// <param name="foreignModuleId">外键模块Id</param>
        /// <param name="relyFieldsValue">依赖字段值</param>
        /// <returns></returns>
        public static object GetFormFieldFilterCondition(ref string whereSql, string moduleName, string fieldName, Guid foreignModuleId, string relyFieldsValue = null)
        {
            if (string.IsNullOrEmpty(moduleName) || string.IsNullOrEmpty(fieldName))
                return null;
            Sys_FormField formField = GetDefaultFormSingleField(moduleName, fieldName);
            if (formField == null || string.IsNullOrEmpty(formField.FilterCondition))
                return null;
            string condition = formField.FilterCondition;
            if (string.IsNullOrEmpty(condition)) return null;
            #region 条件表达式转化
            if ((condition.StartsWith("[") && condition.EndsWith("]")) ||
                (condition.StartsWith("{") && condition.EndsWith("}"))) //JSON条件，常量条件
            {
                if (condition.StartsWith("{")) //单个ConditionItem或Dictionary<string, string>对象或单个匿名对象
                {
                    //先反序列化为ConditionItem，如果失败则尝试反序列化为Dictionary<string, string>
                    try
                    {
                        ConditionItem conditionItem = JsonHelper.Deserialize<ConditionItem>(condition);
                        object exp = CommonOperate.GetQueryCondition(foreignModuleId, new List<ConditionItem>() { conditionItem });
                        return exp;
                    }
                    catch
                    {
                        Dictionary<string, string> dic = new Dictionary<string, string>();
                        try
                        {
                            dic = JsonHelper.Deserialize<Dictionary<string, string>>(condition);
                        }
                        catch
                        {
                            condition = condition.Replace("{", "").Replace("}", "");
                            var arr = condition.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                            if (arr.Length > 0)
                            {
                                foreach (var item in arr)
                                {
                                    var val = item.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                    if (val.Count() != 2) continue;
                                    if (!dic.ContainsKey(val[0]))
                                    {
                                        dic.Add(val[0], val[1]);
                                    }
                                }
                            }
                        }
                        if (dic != null && dic.Count > 0)
                        {
                            object exp = CommonOperate.GetQueryCondition(foreignModuleId, dic);
                            return exp;
                        }
                    }
                }
                else //List<ConditionItem>对象
                {
                    try
                    {
                        List<ConditionItem> conditionItemList = JsonHelper.Deserialize<List<ConditionItem>>(condition);
                        object exp = CommonOperate.GetQueryCondition(foreignModuleId, conditionItemList);
                        return exp;
                    }
                    catch
                    { }
                }
            }
            else if (condition.StartsWith("(") && condition.EndsWith(")")) //SQL条件语句
            {
                if (!string.IsNullOrWhiteSpace(whereSql)) whereSql += " AND ";
                whereSql += condition;
            }
            //else //条件名称
            //{
            //    string errMsg = string.Empty;
            //    try
            //    {
            //        Sys_Condition sysCondition = CommonOperate.GetEntity<Sys_Condition>(x => x.Name == condition.Trim() && x.IsValid && !x.IsDeleted, string.Empty, out errMsg);
            //        if (sysCondition != null)
            //        {

            //            List<ConditionItem> conditionItemList = JsonHelper.Deserialize<List<ConditionItem>>(sysCondition.ConditionJson);
            //            object exp = CommonOperate.GetQueryCondition(foreignModuleId, conditionItemList);
            //            return exp;
            //        }
            //    }
            //    catch
            //    { }
            //}
            #endregion
            return null;
        }

        #endregion

        #endregion

        #region 字典

        /// <summary>
        /// 获取字段绑定的字典分类
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static string GetBindDictonaryClass(Guid moduleId, string fieldName)
        {
            string errMsg = string.Empty;
            Expression<Func<Sys_BindDictionary, bool>> expression = x => x.Sys_ModuleId == moduleId && x.FieldName == fieldName;
            Sys_BindDictionary bindDictionary = CommonOperate.GetEntity<Sys_BindDictionary>(expression, string.Empty, out errMsg);
            if (bindDictionary != null)
            {
                return bindDictionary.ClassName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取字典数据
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static List<Sys_Dictionary> GetDictionaryData(Guid moduleId, string fieldName)
        {
            string className = GetBindDictonaryClass(moduleId, fieldName);
            if (string.IsNullOrEmpty(className)) return new List<Sys_Dictionary>();
            string errMsg = string.Empty;
            Expression<Func<Sys_Dictionary, bool>> expression = x => x.ClassName == className && !x.IsDeleted;
            List<Sys_Dictionary> list = CommonOperate.GetEntities<Sys_Dictionary>(out errMsg, expression);
            return list;
        }

        #endregion

        #region 图标

        /// <summary>
        /// 根据图标类型过滤分页图标
        /// </summary>
        /// <param name="iconType">图标类型</param>
        /// <param name="total">总记录数</param>
        /// <param name="pageInfo">分页信息</param>
        /// <returns></returns>
        public static List<Sys_IconManage> GetPageIcons(out long total, IconTypeEnum? iconType, PageInfo pageInfo = null)
        {
            total = 0;
            PageInfo tempPageInfo = pageInfo == null ? PageInfo.GetDefaultPageInfo() : pageInfo;
            int type = (int)iconType;
            string errMsg = string.Empty;
            Expression<Func<Sys_IconManage, bool>> expression = null;
            if (iconType.HasValue)
            {
                expression = x => x.IconType == type && !x.IsDeleted;
                if (iconType == IconTypeEnum.Piex16)
                    expression = x => (x.IconType == type || x.IconType == null) && !x.IsDeleted;
            }
            else
            {
                expression = x => x.IsDeleted == false;
            }
            List<Sys_IconManage> list = CommonOperate.GetPageEntities<Sys_IconManage>(out errMsg, tempPageInfo, true, expression);
            total = tempPageInfo.totalCount;
            if (list == null) list = new List<Sys_IconManage>();
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="total">总记录数</param>
        /// <param name="pageSize">每页图标数</param>
        /// <param name="iconType">图标类型</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="w">图标面板宽，默认832</param>
        /// <param name="h">图标面板高，默认384</param>
        /// <param name="iconSize">图标所占大小，默认64</param>
        /// <returns></returns>
        public static string GetPageIconsHtml(out long total, out int pageSize, IconTypeEnum? iconType, int pageIndex = 1, int w = 832, int h = 384, int iconSize = 64)
        {
            total = 0;
            int size = iconSize < 32 ? 32 : iconSize; //图标大小最低点32个像素
            int width = w < size * 4 ? size * 4 : w; //最少4列
            int height = h < size * 2 ? size * 2 : h; //最少2行
            int col = (int)(width / size); //列数
            int row = (int)(height / size); //行数
            int page_Size = row * col; //每页多少个图标
            pageSize = page_Size;
            int page_index = pageIndex < 1 ? 1 : pageIndex;
            PageInfo pageInfo = new PageInfo(page_index, page_Size, null, null);
            List<Sys_IconManage> list = SystemOperate.GetPageIcons(out total, iconType, pageInfo);
            if (list.Count == 0) return string.Empty;
            StringBuilder sb = new StringBuilder();
            //图标内容
            sb.Append("<table>");
            for (int i = 0; i < list.Count; i++)
            {
                int r = i / col; //当前所在行
                int c = i % col; //当前所在列
                if (i == 0)
                {
                    sb.Append("<tr>");
                }
                else if (c == 0)
                {
                    sb.Append("</tr><tr>");
                }
                string imgUrl = string.Empty;
                if (list[i].IconClass == (int)IconClassTypeEnum.CustomerIcon) //自定义图标
                {
                    imgUrl = string.Format("/Css/{0}", list[i].IconAddr);
                }
                else if (list[i].IconClass == (int)IconClassTypeEnum.SystemIcon) //自定义图标
                {
                    imgUrl = string.Format("/Scripts/jquery-easyui/themes/{0}", list[i].IconAddr);
                }
                else
                {
                    imgUrl = list[i].IconAddr;
                }
                sb.AppendFormat("<td style=\"text-align:center;width:64px;height:64px;cursor:pointer;\"><img src=\"{0}\" styleName=\"{1}\" /></td>", imgUrl, list[i].StyleClassName);
            }
            sb.Append("</tr></table>");
            return sb.ToString();
        }

        /// <summary>
        /// 获取图标url
        /// </summary>
        /// <param name="iconClassName">图标类名</param>
        /// <returns></returns>
        public static string GetIconUrl(string iconClassName)
        {
            string errMsg = string.Empty;
            Sys_IconManage icon = CommonOperate.GetEntity<Sys_IconManage>(x => x.StyleClassName == iconClassName && !x.IsDeleted, null, out errMsg);
            if (icon == null) return string.Empty;
            if (icon.IconClass == (int)IconClassTypeEnum.CustomerIcon) //自定义图标
                return string.Format("/Css/{0}", icon.IconAddr);
            else if (icon.IconClass == (int)IconClassTypeEnum.SystemIcon) //系统图标
                return string.Format("/Scripts/jquery-easyui/themes/{0}", icon.IconAddr);
            else //用户上传
                return icon.IconAddr;
        }

        #endregion

        #region 桌面

        /// <summary>
        /// 获取桌面项
        /// </summary>
        /// <returns></returns>
        public static List<Desktop_Item> GetDesktopItems()
        {
            string errMsg = string.Empty;
            List<Desktop_Item> desktopItems = CommonOperate.GetEntities<Desktop_Item>(out errMsg, x => !x.IsDeleted && !x.IsDraft, null, false, new List<string>() { "Sort" }, new List<bool>() { false }, null, true);
            if (desktopItems == null) desktopItems = new List<Desktop_Item>();
            return desktopItems;
        }

        /// <summary>
        /// 获取桌面配置字段
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static List<Desktop_GridField> GetDesktopGridFields(Guid moduleId)
        {
            string errMsg = string.Empty;
            List<Desktop_GridField> list = CommonOperate.GetEntities<Desktop_GridField>(out errMsg, x => x.Sys_ModuleId == moduleId && !x.IsDeleted, null, false, new List<string>() { "Sort" }, new List<bool>() { false });
            if (list == null) list = new List<Desktop_GridField>();
            List<Desktop_GridField> deskGridFields = new List<Desktop_GridField>();
            //循环处理字段
            foreach (Desktop_GridField field in list)
            {
                if (!field.Sys_ModuleId.HasValue) continue;
                //对于外键字段需要添加name字段
                Sys_Field sysField = SystemOperate.GetFieldInfo(field.Sys_ModuleId.Value, field.FieidName);
                bool isForeign = !string.IsNullOrEmpty(sysField.ForeignModuleName); //IsForeignField(sysField);
                if (isForeign) //需要添加
                {
                    #region 复制字段
                    Desktop_GridField tempField = new Desktop_GridField();
                    tempField.Id = field.Id;
                    tempField.Sys_ModuleId = field.Sys_ModuleId;
                    tempField.FieidName = field.FieidName;
                    tempField.Sort = field.Sort;
                    tempField.Sys_ModuleName = field.Sys_ModuleName;
                    tempField.CreateDate = field.CreateDate;
                    tempField.CreateUserId = field.CreateUserId;
                    tempField.CreateUserName = field.CreateUserName;
                    tempField.ModifyDate = field.ModifyDate;
                    tempField.ModifyUserId = field.ModifyUserId;
                    tempField.ModifyUserName = field.ModifyUserName;
                    tempField.Width = 0;
                    #endregion
                    deskGridFields.Add(tempField);
                    //增加一个name字段
                    #region 增加外键Name字段
                    Desktop_GridField tempNameField = new Desktop_GridField();
                    tempNameField.Id = field.Id;
                    tempNameField.Sys_ModuleId = field.Sys_ModuleId;
                    tempNameField.Sort = field.Sort;
                    tempNameField.Sys_ModuleName = field.Sys_ModuleName;
                    tempNameField.Width = field.Width;
                    tempNameField.CreateDate = field.CreateDate;
                    tempNameField.CreateUserId = field.CreateUserId;
                    tempNameField.CreateUserName = field.CreateUserName;
                    tempNameField.ModifyDate = field.ModifyDate;
                    tempNameField.ModifyUserId = field.ModifyUserId;
                    tempNameField.ModifyUserName = field.ModifyUserName;
                    //字段名称为原字段名称去掉最后的Id加上Name
                    tempNameField.FieidName = isForeign ? field.FieidName.Substring(0, field.FieidName.Length - 2) + "Name" : field.FieidName + "Name";
                    #endregion
                    deskGridFields.Add(tempNameField);
                }
                else
                {
                    deskGridFields.Add(field);
                }
            }
            return deskGridFields;
        }

        #endregion

        #region UI处理

        #region 下拉框

        /// <summary>
        /// 获取枚举集合对象
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <returns></returns>
        public static object GetEnumTypeList(Type enumType)
        {
            List<object> list = new List<object>();
            Dictionary<string, string> dic = EnumHelper.GetEnumDescValue(enumType);
            foreach (string key in dic.Keys)
            {
                list.Add(new
                {
                    Id = dic[key],
                    Name = key
                });
            }
            return list;
        }

        /// <summary>
        /// 绑定枚举字段下拉框数据
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <returns></returns>
        public static object EnumFieldDataJson(Guid moduleId, string fieldName)
        {
            List<object> list = new List<object>();
            Dictionary<string, string> dic = CommonOperate.GetFieldEnumTypeList(moduleId, fieldName);
            if (dic == null) return null;
            foreach (string key in dic.Keys)
            {
                list.Add(new
                {
                    Id = dic[key],
                    Name = key
                });
            }
            list.Insert(0, new { Id = string.Empty, Name = "请选择" });
            return list;
        }

        /// <summary>
        /// 获取枚举字段显示值
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <returns></returns>
        public static string GetEnumFieldDisplayText(Guid moduleId, string fieldName, string fieldValue)
        {
            Dictionary<string, string> dic = CommonOperate.GetFieldEnumTypeList(moduleId, fieldName);
            if (dic != null && dic.Count > 0)
            {
                return dic.Where(x => x.Value == fieldValue).Select(x => x.Key).FirstOrDefault();
            }
            return string.Empty;
        }

        /// <summary>
        /// 绑定外键字段下拉框数据
        /// </summary>
        /// <param name="moduleId"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object ForeignFieldComboDataJson(Guid moduleId, string fieldName)
        {
            Type modelType = GetModelType(moduleId);
            //验证是否为外键字段
            bool isForeignKey = SystemOperate.IsForeignField(moduleId, fieldName);
            if (!isForeignKey) return string.Empty;
            //获取外键模块
            Sys_Module foreignModule = SystemOperate.GetForeignModule(moduleId, fieldName);
            if (foreignModule == null) return string.Empty;
            Type foreignModelType = GetModelType(foreignModule.Id);
            string errMsg = string.Empty; //异常信息
            string whereSql = string.Empty; //where语句
            string initModule = GetModuleNameById(moduleId); //原始模块
            //组装条件表达式
            object conditionExp = CommonOperate.GetGridFilterCondition(ref whereSql, foreignModule.Id, null, DataGridType.DialogGrid, null, initModule, fieldName);
            //联动条件表达式
            object formFieldConditionExp = SystemOperate.GetFormFieldFilterCondition(ref whereSql, initModule, fieldName, foreignModule.Id);
            //合并条件
            conditionExp = CommonOperate.ConditionMerge(foreignModule.Id, conditionExp, formFieldConditionExp);
            //取外键模块数据
            object data = CommonOperate.GetEntities(out errMsg, foreignModule.Id, conditionExp, whereSql);
            if (data != null)
            {
                List<object> list = new List<object>();
                foreach (object obj in (data as IEnumerable))
                {
                    list.Add(obj);
                }
                Sys_FormField formField = SystemOperate.GetDefaultFormSingleField(moduleId, fieldName);
                if (formField != null)
                {
                    string valueField = string.IsNullOrEmpty(formField.ValueField) ? "Id" : formField.ValueField;
                    string textField = string.IsNullOrEmpty(formField.TextField) ? "Name" : formField.TextField;
                    PropertyInfo pId = foreignModelType.GetProperty(valueField);
                    PropertyInfo pName = foreignModelType.GetProperty(textField);
                    if (pId != null && pName != null)
                    {
                        object tempObj = Activator.CreateInstance(foreignModelType);
                        pId.SetValue(tempObj, string.Empty.ObjToGuid(), null);
                        pName.SetValue(tempObj, "请选择", null);
                        list.Insert(0, tempObj);
                    }
                }
                return list;
            }
            return null;
        }

        /// <summary>
        /// 绑定下拉框字典数据
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static object DictionaryDataJson(Guid moduleId, string fieldName)
        {
            List<Sys_Dictionary> list = SystemOperate.GetDictionaryData(moduleId, fieldName);
            if (list != null && list.Count > 0)
            {
                List<object> data = new List<object>();
                foreach (Sys_Dictionary dictionay in list)
                {
                    data.Add(new
                    {
                        Id = dictionay.Value,
                        Name = dictionay.Name
                    });
                }
                data.Insert(0, new { Id = string.Empty, Name = "请选择" });
                return data;
            }
            return null;
        }

        /// <summary>
        /// 获取字段字典显示值
        /// </summary>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名称</param>
        /// <param name="fieldValue">字段值</param>
        /// <returns></returns>
        public static string GetDictionaryDisplayText(Guid moduleId, string fieldName, string fieldValue)
        {
            List<Sys_Dictionary> list = SystemOperate.GetDictionaryData(moduleId, fieldName);
            if (list != null && list.Count > 0)
            {
                return list.Where(x => x.Value == fieldValue).Select(x => x.Name).FirstOrDefault();
            }
            return string.Empty;
        }

        #endregion

        #region IM

        /// <summary>
        /// 是否启用IM功能
        /// </summary>
        /// <returns></returns>
        public static bool IsEnableIM()
        {
            string errMsg = string.Empty;
            string sql = "SELECT COUNT(1) FROM [dbo].[Users]";
            string imConn = WebConfigHelper.GetConfigFileConnectionString(AppDomain.CurrentDomain.BaseDirectory + "IM\\web.config", "IM_ConnectString");
            if (string.IsNullOrEmpty(imConn)) return false;
            object obj = CommonOperate.ExecuteScale(out errMsg, sql, null, imConn);
            return obj != null;
        }

        #endregion

        #endregion

        #region 其他

        /// <summary>
        /// 判断数据库是否存在
        /// </summary>
        /// <param name="dbLinkArgs">数据库连接参数</param>
        /// <returns></returns>
        public static bool DbIsExists(DbLinkArgs dbLinkArgs)
        {
            if (dbLinkArgs == null) return false;
            string errMsg = string.Empty;
            string sql = string.Empty;
            if (dbLinkArgs.DbType == DatabaseType.MsSqlServer)
            {
                sql = string.Format("SELECT 1 FROM master.sys.sysdatabases WHERE NAME=N'{0}'", dbLinkArgs.DbName);
                string connStr = string.Format("Data Source={0};Initial Catalog=master;User ID={1};Password={2};Pooling=true;MAX Pool Size=512;Min Pool Size=50;Connection Lifetime=30", dbLinkArgs.DataSource, dbLinkArgs.UserId, dbLinkArgs.Pwd);
                object obj = CommonOperate.ExecuteScale(out errMsg, sql, null, connStr, dbLinkArgs.DbType);
                return obj != null;
            }
            return false;
        }

        /// <summary>
        /// 注册其他链接数据服务器
        /// </summary>
        public static void RegCrossDbServer()
        {
            List<DbLinkArgs> list = ModelConfigHelper.GetCrossServerDbLinkArgs();
            string errMsg = string.Empty;
            foreach (DbLinkArgs linkArgs in list)
            {
                string sql = string.Empty;
                if (linkArgs.DbType == DatabaseType.MsSqlServer)
                    sql = string.Format("exec sp_dropserver '{0}','droplogins';exec sp_addlinkedserver  '{0}','','SQLOLEDB','{0}';exec sp_addlinkedsrvlogin '{0}','false',null,'{1}','{2}'", linkArgs.DataSource, linkArgs.UserId, linkArgs.Pwd);
                CommonOperate.ExecuteNonQuery(out errMsg, sql);
            }
        }

        /// <summary>
        /// 向数据库中注册存储过程
        /// </summary>
        public static void RegStoredProcedure()
        {
            //本地数据库连接对象
            DbLinkArgs dbLinkArgs = ModelConfigHelper.GetLocalDbLinkArgs();
            //获取跨服务的数据库连接对象
            List<DbLinkArgs> list = ModelConfigHelper.GetCrossServerDbLinkArgs(false);
            list.Add(dbLinkArgs);
            string errMsg = string.Empty;
            //开始注册存储过程
            foreach (DbLinkArgs linkArgs in list)
            {
                RegStoredProcedure(linkArgs);
            }
        }

        /// <summary>
        /// 注册存储过程
        /// </summary>
        /// <param name="dbLinkArgs">数据库链接对象</param>
        private static void RegStoredProcedure(DbLinkArgs dbLinkArgs)
        {
            if (dbLinkArgs == null) return;
            StringBuilder sb = new StringBuilder(); //要执行的存储过程
            StringBuilder delSb = new StringBuilder(); //删除存储过程语句
            string errMsg = string.Empty;
            if (dbLinkArgs.DbType == DatabaseType.MsSqlServer)
            {
                #region 创建数据库
                if (!string.IsNullOrWhiteSpace(dbLinkArgs.DbName))
                {
                    string appDataPath = string.Format("{0}App_Data", Globals.GetWebDir());
                    if (!Directory.Exists(appDataPath))
                    {
                        Directory.CreateDirectory(appDataPath);
                    }
                    StringBuilder createDbSb = new StringBuilder();
                    createDbSb.AppendLine("IF NOT EXISTS(SELECT 1 FROM master.sys.sysdatabases WHERE NAME=N'" + dbLinkArgs.DbName + "') --如果数据库不存在");
                    createDbSb.AppendLine("BEGIN");
                    createDbSb.AppendLine(" CREATE DATABASE " + dbLinkArgs.DbName + "");
                    createDbSb.AppendLine(" ON");
                    createDbSb.AppendLine(" PRIMARY  --创建主数据库文件");
                    createDbSb.AppendLine(" (");
                    createDbSb.AppendLine(" 	NAME='" + dbLinkArgs.DbName + "',");
                    createDbSb.AppendLine(" 	FILENAME='" + appDataPath + "\\" + dbLinkArgs.DbName + ".mdf',");
                    createDbSb.AppendLine(" 	FileGrowth=1MB");
                    createDbSb.AppendLine(" )");
                    createDbSb.AppendLine(" LOG ON --创建日志文件");
                    createDbSb.AppendLine(" (");
                    createDbSb.AppendLine(" 	NAME='HkTempLog',");
                    createDbSb.AppendLine("	    FileName='" + appDataPath + "\\" + dbLinkArgs.DbName + ".ldf',");
                    createDbSb.AppendLine("	    FileGrowth=1MB");
                    createDbSb.AppendLine(" )");
                    createDbSb.AppendLine(" END");
                    string connStr = string.Format("Data Source={0};Initial Catalog=master;User ID={1};Password={2};Pooling=true;MAX Pool Size=512;Min Pool Size=50;Connection Lifetime=30", dbLinkArgs.DataSource, dbLinkArgs.UserId, dbLinkArgs.Pwd);
                    CommonOperate.ExecuteNonQuery(out errMsg, createDbSb.ToString(), null, connStr);
                }
                #endregion
                #region 分页存储过程
                #region 删除
                delSb.AppendLine("IF EXISTS(select * from dbo.sysobjects where id=object_id(N'[dbo].[GetPageChangingTableByRowNumber]') and OBJECTPROPERTY(id,N'IsProcedure')=1)");
                delSb.AppendLine("BEGIN");
                delSb.AppendLine("   DROP PROCEDURE [dbo].[GetPageTableByRowNumber]");
                delSb.AppendLine("END");
                #endregion
                #region 创建
                sb.AppendLine("CREATE PROCEDURE [dbo].[GetPageTableByRowNumber]");
                sb.AppendLine(" @Field nvarchar(1000),");
                sb.AppendLine(" @TableName  nvarchar(2000),");
                sb.AppendLine(" @condition  nvarchar(4000),--格式为：and (查询条件)  如'and (key=value and key1=value1)'");
                sb.AppendLine(" @OrderField nvarchar(100),");
                sb.AppendLine(" @OrderType int,");
                sb.AppendLine(" @pageindx int,");
                sb.AppendLine(" @PageSize  int,");
                sb.AppendLine(" @RecordCount int output     --记录的总数");
                sb.AppendLine("as");
                sb.AppendLine("BEGIN");
                sb.AppendLine(" --判断是否有排序字段");
                sb.AppendLine("    if(@OrderField is null or ltrim(rtrim(@OrderField))='')");
                sb.AppendLine("     begin");
                sb.AppendLine("       RAISERROR('排序字段不能为空',16,1)");
                sb.AppendLine("       return");
                sb.AppendLine("     end");
                sb.AppendLine("    --组装order语句 ");
                sb.AppendLine(" declare @temp nvarchar(200)");
                sb.AppendLine(" set @temp=' order by '+@OrderField");
                sb.AppendLine(" if(@OrderType=1)");
                sb.AppendLine("  begin ");
                sb.AppendLine("     set @temp=@temp+' asc '");
                sb.AppendLine("        end");
                sb.AppendLine("     else");
                sb.AppendLine("  begin ");
                sb.AppendLine("     set @temp=@temp+' desc '");
                sb.AppendLine("        end");
                sb.AppendLine("    --组装查询条件，如果没有查询条件，直接跳过");
                sb.AppendLine("   if(@condition is not null and ltrim(rtrim(@condition))!='')");
                sb.AppendLine("   begin");
                sb.AppendLine("     set @condition='where 1=1'+@condition");
                sb.AppendLine("   end");
                sb.AppendLine(" else");
                sb.AppendLine("   begin");
                sb.AppendLine("     set @condition=''");
                sb.AppendLine("   end");
                sb.AppendLine(" --求记录的总数");
                sb.AppendLine(" declare @Countsql nvarchar(max)");
                sb.AppendLine(" set @Countsql='select @a= count(1) from '+@TableName +' '+@condition");
                sb.AppendLine(" exec sp_executesql  @Countsql,N'@a int output',@RecordCount output  ");
                sb.AppendLine(" print @RecordCount");
                sb.AppendLine(" declare @sql nvarchar(max)");
                sb.AppendLine(" --分页");
                sb.AppendLine(" if(@pageindx=1)");
                sb.AppendLine("  begin");
                sb.AppendLine("    set @sql=' select top '+cast(@pagesize as nvarchar )+'  '+ @Field+' from '+@TableName +' '+@condition+' '+@temp");
                sb.AppendLine("  end");
                sb.AppendLine(" else");
                sb.AppendLine("  begin");
                sb.AppendLine("    declare @startNumber   int");
                sb.AppendLine("    set @startNumber =(@pageindx-1)*@pagesize");
                sb.AppendLine("    set @sql='select ROW_NUMBER() over('+@temp+') as number, '+@Field+' from '+@TableName+'  '+@condition ");
                sb.AppendLine("    set @sql='SET ROWCOUNT '+Convert(varchar(4),@PageSize)+'; WITH SP_TABLE AS('+@sql   +')  SELECT '+@Field+' from SP_TABLE   where  number>'+CAST(@startNumber as nvarchar) ");
                sb.AppendLine("  end");
                sb.AppendLine(" print @sql");
                sb.AppendLine(" exec(@sql)");
                sb.AppendLine("END");
                #endregion
                #region 执行
                try
                {
                    //先删除存储过程
                    int rs = CommonOperate.ExecuteNonQuery(out errMsg, delSb.ToString(), null, dbLinkArgs.ConnString, dbLinkArgs.DbType);
                    if (rs > 0)
                    {
                        //创建存储过程
                        CommonOperate.ExecuteNonQuery(out errMsg, sb.ToString(), null, dbLinkArgs.ConnString, dbLinkArgs.DbType);
                    }
                }
                catch { }
                #endregion
                #endregion
                #region 获取单据编码
                #region 删除
                delSb = new StringBuilder();
                delSb.AppendLine("IF EXISTS(select * from dbo.sysobjects where id=object_id(N'[dbo].[GetBillCode]') and OBJECTPROPERTY(id,N'IsProcedure')=1)");
                delSb.AppendLine("BEGIN");
                delSb.AppendLine("   DROP PROCEDURE [dbo].[GetBillCode]");
                delSb.AppendLine("END");
                #endregion
                #region 创建
                sb = new StringBuilder();
                sb.AppendLine("CREATE PROCEDURE [dbo].[GetBillCode] ");
                sb.AppendLine("	@ModuleId NVARCHAR(36),");
                sb.AppendLine("	@returnChar  NVARCHAR(1000)  output  ");
                sb.AppendLine("AS");
                sb.AppendLine("BEGIN");
                sb.AppendLine("	DECLARE @TableName nvarchar(255)");
                sb.AppendLine("	DECLARE @BillNo nvarchar(255)");
                sb.AppendLine("	DECLARE @TempBillNo nvarchar(255)");
                sb.AppendLine("	DECLARE @IdentifyExistNo nvarchar(255)");
                sb.AppendLine("	DECLARE @IdentifyExistSN nvarchar(255)");
                sb.AppendLine("	DECLARE @IdentifyTempSN nvarchar(255)");
                sb.AppendLine("	DECLARE @Date DATETIME");
                sb.AppendLine("	DECLARE @strSql nvarchar(max)");
                sb.AppendLine("	DECLARE @Prefix nvarchar(255)");
                sb.AppendLine("	DECLARE @FieldName nvarchar(255)");
                sb.AppendLine("	DECLARE @IsEnableDate BIT");
                sb.AppendLine("	DECLARE @DateFormat INT");
                sb.AppendLine("	DECLARE @SerialNumber INT");
                sb.AppendLine("	DECLARE @PlaceHolder INT");
                sb.AppendLine("	DECLARE @SNLength INT");
                sb.AppendLine("	DECLARE @RuleFormat nvarchar(255)");
                sb.AppendLine("	SET @Date=GETDATE()");
                sb.AppendLine(string.Format(" SELECT @TableName=TableName FROM {0} WHERE Id=@ModuleId", ModelConfigHelper.GetModuleTableName(typeof(Sys_Module))));
                sb.AppendLine("	SELECT @Prefix=Prefix,@IsEnableDate=IsEnableDate,@DateFormat=DateFormat,@SerialNumber=SerialNumber,@PlaceHolder=PlaceHolder,@SNLength=SNLength,@RuleFormat=RuleFormat,@FieldName=FieldName ");
                sb.AppendLine(string.Format(" FROM {0} WHERE Sys_ModuleId= @ModuleId ", ModelConfigHelper.GetModuleTableName(typeof(Sys_BillCodeRule))));
                sb.AppendLine("	SET @BillNo=N''");
                sb.AppendLine("	SET @BillNo=@BillNo+@Prefix");
                sb.AppendLine("	IF @IsEnableDate=1		");
                sb.AppendLine("	BEGIN	");
                sb.AppendLine("		IF @DateFormat=0 SET @BillNo=@BillNo+CONVERT(CHAR(2),@Date,12)");
                sb.AppendLine("		ELSE IF @DateFormat=1 SET @BillNo=@BillNo+CONVERT(CHAR(4),@Date,112)");
                sb.AppendLine("		ELSE IF @DateFormat=2 SET @BillNo=@BillNo+CONVERT(CHAR(4),@Date,12)");
                sb.AppendLine("		ELSE IF @DateFormat=3 SET @BillNo=@BillNo+CONVERT(CHAR(6),@Date,112)");
                sb.AppendLine("		ELSE IF @DateFormat=4 SET @BillNo=@BillNo+CONVERT(CHAR(6),@Date,12)");
                sb.AppendLine("		ELSE IF @DateFormat=5 SET @BillNo=@BillNo+CONVERT(CHAR(8),@Date,112)");
                sb.AppendLine("		ELSE IF @DateFormat=6 SET @BillNo=@BillNo+CONVERT(CHAR(2),@Date,1)+CONVERT(CHAR(2),@Date,12)");
                sb.AppendLine("		ELSE IF @DateFormat=7 SET @BillNo=@BillNo+CONVERT(CHAR(2),@Date,1)+CONVERT(CHAR(4),@Date,112)");
                sb.AppendLine("		ELSE IF @DateFormat=8 SET @BillNo=@BillNo+CONVERT(CHAR(2),@Date,12)+'-'+CONVERT(CHAR(2),@Date,1)");
                sb.AppendLine("		ELSE IF @DateFormat=9 SET @BillNo=@BillNo+CONVERT(CHAR(7),@Date,120)");
                sb.AppendLine("		ELSE IF @DateFormat=10 SET @BillNo=@BillNo+REPLACE(CONVERT(CHAR(8),@Date,11),'/','-')");
                sb.AppendLine("		ELSE IF @DateFormat=11 SET @BillNo=@BillNo+CONVERT(CHAR(10),@Date,120)");
                sb.AppendLine("		ELSE IF @DateFormat=12 SET @BillNo=@BillNo+CONVERT(CHAR(2),@Date,1)+'-'+CONVERT(CHAR(2),@Date,12)");
                sb.AppendLine("		ELSE IF @DateFormat=13 SET @BillNo=@BillNo+CONVERT(CHAR(2),@Date,1)+'-'+CONVERT(CHAR(4),@Date,112)");
                sb.AppendLine("		ELSE IF @DateFormat=14 SET @BillNo=@BillNo+REPLACE(CONVERT(CHAR(8),@Date,1),'/','-')");
                sb.AppendLine("		ELSE IF @DateFormat=15 SET @BillNo=@BillNo+REPLACE(CONVERT(CHAR(10),@Date,101),'/','-')");
                sb.AppendLine("		ELSE IF @DateFormat=16 SET @BillNo=@BillNo+CONVERT(CHAR(8),@Date,1)");
                sb.AppendLine("		ELSE IF @DateFormat=17 SET @BillNo=@BillNo+CONVERT(CHAR(10),@Date,101)");
                sb.AppendLine("		ELSE IF @DateFormat=18 SET @BillNo=@BillNo+CONVERT(CHAR(8),@Date,11)");
                sb.AppendLine("		ELSE IF @DateFormat=19 SET @BillNo=@BillNo+CONVERT(CHAR(10),@Date,111)");
                sb.AppendLine("	END");
                sb.AppendLine("	SET @TempBillNo=@BillNo");
                sb.AppendLine("	SET @TempBillNo=@TempBillNo+REPLACE(right(cast(power(10,9) as varchar)+@SerialNumber,@SNLength),'0',@PlaceHolder)");
                sb.AppendLine("	SET @strSql=N''");
                sb.AppendLine("	SET @strSql =@strSql+ ' SELECT DISTINCT  @IdentifyExistNo='+@FieldName+',@IdentifyExistSN=SUBSTRING('+@FieldName+',LEN('+@FieldName+')-'+CONVERT(NVARCHAR,(@SNLength-1))+',LEN('+@FieldName+'))'");
                sb.AppendLine("	SET @strSql =@strSql+ ' FROM  '+@TableName");
                sb.AppendLine("	SET @strSql =@strSql+ ' WHERE LEN('+@FieldName+')='+CONVERT(NVARCHAR,LEN(@TempBillNo))+' AND '+@FieldName+' LIKE '''+@BillNo+'%'''  ");
                sb.AppendLine("	SET @strSql =@strSql+ ' AND SUBSTRING('+@FieldName+',LEN('+@FieldName+')-'+CONVERT(NVARCHAR,(@SNLength-1))+',LEN('+@FieldName+'))=(SELECT MAX(SUBSTRING('+@FieldName+',LEN('+@FieldName+')-'+CONVERT(NVARCHAR,(@SNLength-1))+',LEN('+@FieldName+'))) FROM '+@TableName+' WHERE LEN('+@FieldName+')='+CONVERT(NVARCHAR,LEN(@TempBillNo))");
                sb.AppendLine("	SET @strSql =@strSql+ ' AND SUBSTRING('+@FieldName+',0,LEN('+@FieldName+')-'+CONVERT(NVARCHAR,(@SNLength-1))+')='''+@BillNo+''''+')'");
                sb.AppendLine("	EXEC sys.sp_executesql @strSql,N'@IdentifyExistNo nvarchar(255) output,@IdentifyExistSN nvarchar(255) output',@IdentifyExistNo output,@IdentifyExistSN output");
                sb.AppendLine("	IF @IdentifyExistNo<>''");
                sb.AppendLine("	BEGIN");
                sb.AppendLine("	SET @IdentifyTempSN= CONVERT(NVARCHAR,CONVERT(DECIMAL,@IdentifyExistSN)+1)");
                sb.AppendLine("	IF LEN(@IdentifyTempSN)<@IdentifyTempSN");
                sb.AppendLine("	SET @IdentifyTempSN= REPLACE(right(cast(power(10,9) as varchar)+@IdentifyTempSN,@SNLength),'0',@PlaceHolder)");
                sb.AppendLine("	SET @returnChar=@BillNo + @IdentifyTempSN");
                sb.AppendLine("	END");
                sb.AppendLine("	ELSE IF @IdentifyExistNo=''");
                sb.AppendLine("	SET @returnChar=@BillNo+ REPLACE(right(cast(power(10,9) as varchar)+@SerialNumber,@SNLength),'0',@PlaceHolder)");
                sb.AppendLine("	ELSE IF @IdentifyExistNo IS NULL");
                sb.AppendLine("	SET @returnChar=@BillNo+ REPLACE(right(cast(power(10,9) as varchar)+@SerialNumber,@SNLength),'0',@PlaceHolder)");
                sb.AppendLine("END");
                #endregion
                #region 执行
                try
                {
                    //先删除存储过程
                    CommonOperate.ExecuteNonQuery(out errMsg, delSb.ToString(), null, dbLinkArgs.ConnString, dbLinkArgs.DbType);
                    //创建存储过程
                    CommonOperate.ExecuteNonQuery(out errMsg, sb.ToString(), null, dbLinkArgs.ConnString, dbLinkArgs.DbType);
                }
                catch { }
                #endregion
                #endregion
            }
        }

        /// <summary>
        /// 获取模块数据库类型，默认MsSqlServer
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="connString">连接字符串</param>
        /// <returns></returns>
        public static DatabaseType GetModuleDbType(Sys_Module module, out string connString)
        {
            connString = string.Empty;
            DatabaseType dbTypeEnum = DatabaseType.MsSqlServer;
            if (module == null || module.DataSourceTypeOfEnum != ModuleDataSourceType.DbTable ||
                string.IsNullOrEmpty(module.TableName))
                return dbTypeEnum;
            Type modelType = GetModelType(module.Id);
            if (modelType == null) return dbTypeEnum;
            string errMsg = string.Empty;
            string dbType = string.Empty;
            connString = ModelConfigHelper.GetModelConnString(modelType, out dbType);
            if (string.IsNullOrWhiteSpace(dbType)) dbType = "0";
            try
            {
                dbTypeEnum = (DatabaseType)Enum.Parse(typeof(DatabaseType), dbType);
            }
            catch { }
            return dbTypeEnum;
        }

        /// <summary>
        /// 获取模块索引信息
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        public static TbIndexInfo GetTableIndexInfo(string moduleName)
        {
            Sys_Module module = GetModuleByName(moduleName);
            if (module == null || module.DataSourceTypeOfEnum != ModuleDataSourceType.DbTable ||
               string.IsNullOrEmpty(module.TableName))
                return null;
            string errMsg = string.Empty;
            string connString = string.Empty;
            DatabaseType dbTypeEnum = GetModuleDbType(module, out connString);
            if (dbTypeEnum == DatabaseType.MsSqlServer)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT '[' + DB_NAME() + '].[' + OBJECT_SCHEMA_NAME(ddips.[object_id],");
                sb.Append(" DB_ID())+ '].['");
                sb.Append("+ OBJECT_NAME(ddips.[object_id], DB_ID()) + ']' AS [TableName] ,");
                sb.Append("i.[name] AS [IndexName] ,");
                sb.Append("ddips.[index_type_desc] AS [IndexTypeDes],");
                sb.Append("ddips.[partition_number] AS [PartitionNum],");
                sb.Append("ddips.[alloc_unit_type_desc],");
                sb.Append("ddips.[index_depth] ,");
                sb.Append("ddips.[index_level] ,");
                sb.Append("CAST(ddips.[avg_fragmentation_in_percent]AS SMALLINT) AS [FragmentationPercent] ,");
                sb.Append("CAST(ddips.[avg_fragment_size_in_pages]AS SMALLINT) AS [avg_frag_size_in_pages] ,");
                sb.Append("ddips.[fragment_count] ,");
                sb.Append("ddips.[page_count] ");
                sb.Append("FROM sys.dm_db_index_physical_stats(DB_ID(), NULL, NULL, NULL, 'limited') ddips ");
                sb.Append("INNER JOIN sys.[indexes] i ON ddips.[object_id] = i.[object_id] ");
                sb.Append("AND ddips.[index_id] = i.[index_id] ");
                sb.AppendFormat("WHERE OBJECT_NAME(ddips.[object_id])='{0}' AND ddips.[alloc_unit_type_desc]='IN_ROW_DATA'", module.TableName);
                DataTable dt = CommonOperate.ExecuteQuery(out errMsg, sb.ToString(), null, connString, dbTypeEnum);
                TbIndexInfo tbIndex = ObjectHelper.FillModel<TbIndexInfo>(dt).FirstOrDefault();
                return tbIndex;
            }
            return null;
        }

        /// <summary>
        /// 重建数据表索引
        /// </summary>
        /// <param name="moduleName"></param>
        public static void RebuildTableIndex(string moduleName)
        {
            Sys_Module module = GetModuleByName(moduleName);
            if (module == null || module.DataSourceTypeOfEnum != ModuleDataSourceType.DbTable ||
                string.IsNullOrEmpty(module.TableName))
                return;
            string errMsg = string.Empty;
            string connString = string.Empty;
            DatabaseType dbTypeEnum = GetModuleDbType(module, out connString);
            if (dbTypeEnum == DatabaseType.MsSqlServer)
            {
                CommonOperate.ExecuteNonQuery(out errMsg, string.Format("DBCC DBREINDEX('{0}')", module.TableName), null, connString, dbTypeEnum);
            }
        }

        /// <summary>
        /// 重建所有模块索引
        /// </summary>
        public static void RebuildAllTableIndex()
        {
            string errMsg = string.Empty;
            List<Sys_DbConfig> list = CommonOperate.GetEntities<Sys_DbConfig>(out errMsg, null, null, false);
            if (list.Count > 0)
            {
                foreach (Sys_DbConfig dbConfig in list)
                {
                    if (!dbConfig.AutoReCreateIndex) continue;
                    TbIndexInfo indexInfo = GetTableIndexInfo(dbConfig.ModuleName);
                    if (indexInfo != null && indexInfo.FragmentationPercent > 0 &&
                        dbConfig.CreateIndexPageDensity > 0 &&
                        indexInfo.FragmentationPercent >= dbConfig.CreateIndexPageDensity)
                    {
                        RebuildTableIndex(dbConfig.ModuleName);
                    }
                }
            }
        }

        /// <summary>
        /// 获取模块主键索引名
        /// </summary>
        /// <param name="module">模块</param>
        public static string GetModulePrimarykeyIndexName(Sys_Module module)
        {
            if (module == null || module.DataSourceTypeOfEnum != ModuleDataSourceType.DbTable ||
                string.IsNullOrEmpty(module.TableName))
                return string.Empty;
            string errMsg = string.Empty;
            string connString = string.Empty;
            DatabaseType dbTypeEnum = GetModuleDbType(module, out connString);
            if (dbTypeEnum == DatabaseType.MsSqlServer)
            {
                string sql = string.Format("SELECT B.NAME FROM  SYSOBJECTS A JOIN SYSOBJECTS B ON A.ID=B.PARENT_OBJ AND A.XTYPE='U' AND B.XTYPE='PK' AND A.NAME = '{0}'", module.TableName);
                object obj = CommonOperate.ExecuteScale(out errMsg, sql, null, connString, dbTypeEnum);
                return obj.ObjToStr();
            }
            return string.Empty;
        }

        /// <summary>
        /// 加载所有模块缓存
        /// </summary>
        public static void LoadAllModuleCache()
        {
            string errMsg = string.Empty;
            List<Sys_CacheConfig> list = CommonOperate.GetEntities<Sys_CacheConfig>(out errMsg, x => x.IsEnableCache == true, null, false).ToList();
            if (list != null && list.Count > 0)
            {
                foreach (Sys_CacheConfig cacheConfig in list)
                {
                    Guid moduleId = SystemOperate.GetModuleIdByName(cacheConfig.ModuleName);
                    if (moduleId != Guid.Empty)
                    {
                        CommonOperate.GetEntity(moduleId, null, null, out errMsg);
                    }
                }
            }
        }

        #endregion
    }
}
