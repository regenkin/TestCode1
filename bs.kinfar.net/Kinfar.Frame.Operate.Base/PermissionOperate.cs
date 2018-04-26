﻿using Kinfar.Frame.Common;
using Kinfar.Frame.Model.EnumSpace;
using Kinfar.Frame.Model.Sys;
using Kinfar.Frame.Operate.Base.EnumDef;
using Kinfar.Frame.Operate.Base.OperateHandle;
using Kinfar.Frame.Operate.Base.TempModel;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Kinfar.Frame.Base;
using Kinfar.Frame.IBLL.Base;
using Kinfar.Frame.Bridge;
using Kinfar.Frame.IBLL.Sys;
using Kinfar.Frame.EntityBase;
using System.Linq.Expressions;
using Kinfar.Frame.Model.OrgM;

namespace Kinfar.Frame.Operate.Base
{
    /// <summary>
    /// 权限操作
    /// </summary>
    public static class PermissionOperate
    {
        #region 角色

        /// <summary>
        /// 添加管理员角色
        /// </summary>
        public static void AddAdminRole()
        {
            string errMsg = string.Empty;
            //判断管理员角色是否存在
            long count = CommonOperate.Count<Sys_Role>(out errMsg, false, x => x.Name == "系统管理员");
            if (count > 0) return; //管理员已存在直接退出
            //添加管理员角色
            Sys_Role adminRole = new Sys_Role()
            {
                Name = "系统管理员",
                Des = "系统管理员_admin",
                IsValid = true,
                CreateDate = DateTime.Now,
                ModifyDate = DateTime.Now
            };
            Guid adminRoleId = CommonOperate.OperateRecord<Sys_Role>(adminRole, OperateHandle.ModelRecordOperateType.Add, out errMsg);
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        public static Sys_Role GetRole(Guid roleId)
        {
            string errMsg = string.Empty;
            Sys_Role role = CommonOperate.GetEntityById<Sys_Role>(roleId, out errMsg);
            return role;
        }

        /// <summary>
        /// 获取角色
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns></returns>
        public static Sys_Role GetRole(string roleName)
        {
            string errMsg = string.Empty;
            Sys_Role role = CommonOperate.GetEntity<Sys_Role>(x => x.Name == roleName, null, out errMsg);
            return role;
        }

        /// <summary>
        /// 获取角色名称
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        public static string GetRoleName(Guid roleId)
        {
            string errMsg = string.Empty;
            Sys_Role role = CommonOperate.GetEntityById<Sys_Role>(roleId, out errMsg);
            if (role != null)
                return role.Name;
            return string.Empty;
        }

        /// <summary>
        /// 获取父角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        public static Sys_Role GetParentRole(Guid roleId)
        {
            string errMsg = string.Empty;
            Sys_Role role = CommonOperate.GetEntityById<Sys_Role>(roleId, out errMsg);
            if (role != null && role.ParentId.HasValue)
            {
                Sys_Role parentRole = CommonOperate.GetEntityById<Sys_Role>(role.ParentId.Value, out errMsg);
                return parentRole;
            }
            return null;
        }

        /// <summary>
        /// 获取所有祖宗角色，包含父角色，祖父角色，。。
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns></returns>
        public static List<Sys_Role> GetParentsRole(Guid roleId)
        {
            List<Sys_Role> list = new List<Sys_Role>();
            Sys_Role role = GetRole(roleId);
            if (role != null)
            {
                Sys_Role parentRole = GetParentRole(roleId);
                if (parentRole != null) //存在父角色
                {
                    list.Add(parentRole); //将父角色添加到列表
                    list.AddRange(GetParentsRole(parentRole.Id));
                }
                if (!string.IsNullOrEmpty(role.OtherParentRoles))
                {
                    List<Guid> otherParentRoleIds = role.OtherParentRoles.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => x.ObjToGuid()).Where(x => x!=Guid.Empty).Distinct().ToList();
                    List<Sys_Role> otherParentRoles = otherParentRoleIds.Select(x => GetRole(x)).Where(x => x != null).ToList();
                    if (otherParentRoles != null && otherParentRoles.Count > 0)
                    {
                        list.AddRange(otherParentRoles);
                        foreach (Sys_Role tempRole in otherParentRoles)
                        {
                            list.AddRange(GetParentsRole(tempRole.Id));
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 获取所有子角色
        /// </summary>
        /// <param name="parentRoleId">根结点角色Id</param>
        /// <param name="isDirect">是否直接子结点</param>
        /// <returns></returns>
        public static List<Sys_Role> GetChildRoles(Guid? parentRoleId = null, bool isDirect = true)
        {
            string errMsg = string.Empty;
            //找子级角色
            List<Sys_Role> listTemp = new List<Sys_Role>();
            if (parentRoleId.HasValue && parentRoleId.Value != Guid.Empty)
            {
                listTemp = CommonOperate.GetEntities<Sys_Role>(out errMsg, x => x.ParentId == parentRoleId, null, false);
            }
            else
            {
                listTemp = CommonOperate.GetEntities<Sys_Role>(out errMsg, x => x.ParentId == null || x.ParentId == Guid.Empty, null, false);
            }
            if (listTemp == null) listTemp = new List<Sys_Role>();
            if (isDirect) //取直接子角色
            {
                return listTemp;
            }
            List<Sys_Role> list = new List<Sys_Role>();
            foreach (Sys_Role role in listTemp)
            {
                list.Add(role);
                list.AddRange(GetChildRoles(role.Id, isDirect));
            }
            return list;
        }

        /// <summary>
        /// 获取当前人所有下级员工角色集合
        /// </summary>
        /// <param name="userId">当前用户</param>
        /// <returns></returns>
        private static List<Sys_Role> GetAllSubordinateStaffRoles(Guid userId)
        {
            List<Sys_Role> roles = new List<Sys_Role>();
            OrgM_Emp emp = OrgMOperate.GetEmpByUserId(userId);
            if (emp != null)
            {
                List<OrgM_Emp> childEmps = new List<OrgM_Emp>();
                List<OrgM_Dept> depts = OrgMOperate.GetEmpPartTimeDepts(emp.Id);
                if (depts.Count > 0)
                {
                    foreach (OrgM_Dept dept in depts)
                    {
                        childEmps.AddRange(OrgMOperate.GetEmpChildsEmps(emp.Id, false, dept.Id));
                    }
                }
                else
                {
                    childEmps = OrgMOperate.GetEmpChildsEmps(emp.Id, false);
                }
                foreach (OrgM_Emp tempEmp in childEmps)
                {
                    Guid tempUserId = OrgMOperate.GetUserIdByEmpId(tempEmp.Id);
                    if (tempUserId!=Guid.Empty)
                    {
                        List<Sys_Role> tempRoles = GetUserRoles(tempUserId);
                        tempRoles = tempRoles.Distinct(new DistinctComparer<Sys_Role>("Id")).ToList();
                        roles.AddRange(tempRoles);
                    }
                }
            }
            return roles;
        }

        /// <summary>
        /// 获取用户角色，包含父角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static List<Sys_Role> GetUserRoles(Guid userId)
        {
            string errMsg = string.Empty;
            List<Sys_Role> roles = CommonOperate.GetEntities<Sys_Role>(out errMsg, null, string.Format("Id IN(SELECT Sys_RoleId FROM Sys_UserRole WHERE Sys_UserId='{0}')", userId), false);
            if (roles == null) roles = new List<Sys_Role>();
            List<Sys_Role> list = new List<Sys_Role>();
            list.AddRange(roles);
            foreach (Sys_Role role in roles)
            {
                List<Sys_Role> parentsRoles = GetParentsRole(role.Id); //获取所有父角色
                list.AddRange(parentsRoles);
            }
            list.AddRange(GetAllSubordinateStaffRoles(userId));
            list = list.Distinct(new DistinctComparer<Sys_Role>("Id")).ToList();
            return list;
        }

        #endregion

        #region 用户

        /// <summary>
        /// 获取角色的所有用户集合
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public static List<Sys_User> GetRoleUsers(Guid roleId)
        {
            string errMsg = string.Empty;
            List<Sys_UserRole> userRoles = CommonOperate.GetEntities<Sys_UserRole>(out errMsg, x => x.Sys_RoleId == roleId, null, false);
            if (userRoles != null && userRoles.Count > 0)
            {
                List<Guid> userIds = userRoles.Where(x => x.Sys_UserId.HasValue).Select(x => x.Sys_UserId.Value).ToList();
                List<Sys_User> users = CommonOperate.GetEntities<Sys_User>(out errMsg, x => userIds.Contains(x.Id), null, false);
                if (users == null) users = new List<Sys_User>();
                return users;
            }
            return new List<Sys_User>();
        }

        #endregion

        #region 权限

        #region 功能权限

        /// <summary>
        /// 获取用户的功能（菜单或网格按钮）权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="functionType">功能类型（菜单、网格按钮）</param>
        /// <returns></returns>
        public static List<Guid> GetUserFunPermissions(Guid userId, FunctionTypeEnum functionType)
        {
            ISys_PermissionFunBLL permissionFunBll = BridgeObject.Resolve<ISys_PermissionFunBLL>();
            return permissionFunBll.GetUserFunPermissions(userId, (int)functionType).Keys.ToList();
        }

        /// <summary>
        /// 获取角色功能权限，子角色默认继承父角色的所有权限，当前返回字典，
        /// 字典第一个参数为功能Id（菜单或按钮Id）,第二个参数为是否该功能Id继承父角色，
        /// 非当前角色所有
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="functionType">功能类型，菜单或网格按钮</param>
        /// <returns>返回功能Id、是否来自父角色的权限字典</returns>
        public static Dictionary<Guid, bool> GetRoleFunPermissions(Guid roleId, FunctionTypeEnum functionType)
        {
            ISys_PermissionFunBLL permissionFunBll = BridgeObject.Resolve<ISys_PermissionFunBLL>();
            return permissionFunBll.GetRoleFunPermissions(roleId, (int)functionType);
        }

        /// <summary>
        /// 某个用户是否有某菜单权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="menuId">菜单Id</param>
        /// <returns></returns>
        public static bool HasMenuPermission(Guid userId, Guid menuId)
        {
            if (UserOperate.IsSuperAdmin(userId)) return true;
            List<Guid> menuIds = GetUserFunPermissions(userId, FunctionTypeEnum.Menu);
            return menuIds.Contains(menuId);
        }

        /// <summary>
        /// 某用户是否有某个模块的列表页面浏览权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public static bool HasModuleBrowerPermission(Guid userId, Guid moduleId)
        {
            if (UserOperate.IsSuperAdmin(userId)) return true;
            Sys_Menu menu = SystemOperate.GetMenuOfModule(moduleId);
            if (menu != null)
                return HasMenuPermission(userId, menu.Id);
            string errMsg = string.Empty;
            Guid funId = moduleId;
            int funType = (int)FunctionTypeEnum.Menu;
            Sys_PermissionFun permissionFun = CommonOperate.GetEntity<Sys_PermissionFun>(x => x.FunId == funId && x.FunType == funType, null, out errMsg);
            return permissionFun != null;
        }

        /// <summary>
        /// 某用户是否有某按钮权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="btnId">按钮Id</param>
        /// <returns></returns>
        public static bool HasButtonPermission(Guid userId, Guid btnId)
        {
            if (UserOperate.IsSuperAdmin(userId)) return true;
            List<Guid> btnIds = GetUserFunPermissions(userId, FunctionTypeEnum.Button);
            return btnIds.Contains(btnId);
        }

        /// <summary>
        /// 某用户是否有某按钮权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="buttonText">按钮显示文本</param>
        /// <returns></returns>
        public static bool HasButtonPermission(Guid userId, Guid moduleId, string buttonText)
        {
            if (UserOperate.IsSuperAdmin(userId)) return true;
            Sys_GridButton btn = SystemOperate.GetGridButton(moduleId, buttonText);
            if (btn != null)
            {
                return HasButtonPermission(userId, btn.Id);
            }
            return false;
        }

        #endregion

        #region 字段权限

        /// <summary>
        /// 获取角色（查看、新增、编辑）字段权限，返回允许（查看、新增、编辑）的字段名字典，
        /// 第一个参数为字段名，第二个参数为是否来自父角色的权限，
        /// 非当前角色权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldPermissionType">字段权限类型</param>
        /// <param name="read">是否为应用读取，应用读取快速返回</param>
        /// <returns>返回允许（查看、新增、编辑）的字段名字典</returns>
        public static Dictionary<string, bool> GetRoleFieldPermissions(Guid roleId, Guid moduleId, FieldPermissionTypeEnum fieldPermissionType, bool read = false)
        {
            ISys_PermissionFieldBLL permissionFunBll = BridgeObject.Resolve<ISys_PermissionFieldBLL>();
            return permissionFunBll.GetRoleFieldPermissions(roleId, moduleId, (int)fieldPermissionType, read);
        }

        /// <summary>
        /// 获取用户（查看、新增、编辑）字段权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldPermissionType">字段权限类型</param>
        /// <returns>返回允许（查看、新增、编辑）的字段名集合</returns>
        public static List<string> GetUserFieldsPermissions(Guid userId, Guid moduleId, FieldPermissionTypeEnum fieldPermissionType)
        {
            ISys_PermissionFieldBLL permissionFunBll = BridgeObject.Resolve<ISys_PermissionFieldBLL>();
            return permissionFunBll.GetUserFieldsPermissions(userId, moduleId, (int)fieldPermissionType);
        }

        /// <summary>
        /// 是否有权限查看某字段
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static bool CanViewField(Guid userId, Guid moduleId, string fieldName)
        {
            if (UserOperate.IsSuperAdmin(userId)) return true;
            //字段权限
            List<string> permissionFields = PermissionOperate.GetUserFieldsPermissions(userId, moduleId, FieldPermissionTypeEnum.ViewField);
            //是否需要过滤
            bool isFilter = permissionFields.Count > 0 && !permissionFields.Contains("-1");
            if (isFilter) //需要过滤
            {
                //不是基类字段并且不是主键字段
                if (!CommonDefine.BaseEntityFieldsContainId.Contains(fieldName) &&
                    !SystemOperate.GetModulePrimaryKeyFields(moduleId).Contains(fieldName))
                {
                    if (!permissionFields.Contains(fieldName))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 是否有新增某字段的权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static bool CanAddField(Guid userId, Guid moduleId, string fieldName)
        {
            if (UserOperate.IsSuperAdmin(userId)) return true;
            if (!CanViewField(userId, moduleId, fieldName)) return false;
            //字段权限
            List<string> permissionFields = PermissionOperate.GetUserFieldsPermissions(userId, moduleId, FieldPermissionTypeEnum.AddField);
            //是否需要过滤
            bool isFilter = permissionFields.Count > 0 && !permissionFields.Contains("-1");
            if (isFilter) //需要过滤
            {
                if (!permissionFields.Contains(fieldName))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取有权限操作的字段
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldPermissionType">字段权限类型</param>
        /// <returns></returns>
        public static List<string> GetCanOperateFields(Guid userId, Guid moduleId, FieldPermissionTypeEnum fieldPermissionType)
        {
            List<string> permissionFields = PermissionOperate.GetUserFieldsPermissions(userId, moduleId, FieldPermissionTypeEnum.ViewField);
            if (fieldPermissionType == FieldPermissionTypeEnum.ViewField)
                return permissionFields;
            //字段权限
            //新增权限
            List<string> permissionAddFields = PermissionOperate.GetUserFieldsPermissions(userId, moduleId, fieldPermissionType);
            //是否需要过滤
            bool isFilter = permissionFields.Count > 0 && !permissionFields.Contains("-1");
            if (isFilter) //需要过滤
            {
                if (permissionAddFields.Count == 0 || permissionAddFields.Contains("-1"))
                    return permissionFields;
                return permissionAddFields.Where(x => permissionFields.Contains(x)).ToList();
            }
            return permissionAddFields;
        }

        /// <summary>
        /// 是否有编辑某字段的权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="fieldName">字段名</param>
        /// <returns></returns>
        public static bool CanEditField(Guid userId, Guid moduleId, string fieldName)
        {
            if (UserOperate.IsSuperAdmin(userId)) return true;
            if (!CanViewField(userId, moduleId, fieldName)) return false;
            //字段权限
            List<string> permissionFields = PermissionOperate.GetUserFieldsPermissions(userId, moduleId, FieldPermissionTypeEnum.EditField);
            //是否需要过滤
            bool isFilter = permissionFields.Count > 0 && !permissionFields.Contains("-1");
            if (isFilter) //需要过滤
            {
                if (!permissionFields.Contains(fieldName))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region 数据权限

        /// <summary>
        /// 获取权限表达式
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="filterWhere">过虑条件SQL</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetPermissionExp<T>(Guid userId, out string filterWhere) where T : BaseEntity
        {
            bool queryCache = ModelConfigHelper.IsModelEnableCache(typeof(T));
            filterWhere = string.Empty;
            Expression<Func<T, bool>> exp = null;
            //先检查有没有自定义权限处理，有则先调用各模块的自定义权限过滤处理，没有调用通用权限处理
            try
            {
                exp = new OperateHandleFactory<T>().GetPermissionExp(userId, out filterWhere, queryCache);
                if (queryCache)
                {
                    filterWhere = string.Empty;
                    if (exp != null)
                        return exp;
                }
                else
                {
                    if (exp != null || !string.IsNullOrEmpty(filterWhere))
                        return exp;
                }
            }
            catch { }
            //调用通用权限处理
            if (queryCache)
                return GetUserDataPermissionExpression<T>(userId, DataPermissionTypeEnum.ViewData);
            Guid moduleId = SystemOperate.GetModuleIdByTableName(typeof(T).Name);
            filterWhere = GetUserDataPermissionWhere(userId, moduleId, DataPermissionTypeEnum.ViewData);
            return null;
        }

        /// <summary>
        /// 用户是否有操作某条记录的权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="id">记录Id</param>
        /// <param name="dataPermissionType">数据权限类型</param>
        /// <returns></returns>
        public static bool UserHasOperateRecordPermission(Guid userId, Guid moduleId, Guid id, DataPermissionTypeEnum dataPermissionType)
        {
            if (UserOperate.IsSuperAdmin(userId)) return true;
            string errMsg = string.Empty;
            BaseEntity t = CommonOperate.GetEntityById(moduleId, id, out errMsg) as BaseEntity;
            if (t == null) return false;
            UserInfo userInfo = UserOperate.GetUserInfo(userId);
            if (userInfo == null) return false;
            //先检查有没有自定义权限处理，有则先调用各模块的自定义权限过滤处理，没有调用通用权限处理
            try
            {
                object instance = CommonOperate.ExecuteCustomeOperateHandleMethod(moduleId, "GetOperateHandleInstance", new object[] { OperateInterfaceType.PermissionOperate });
                if (instance != null)
                {
                    object obj = CommonOperate.ExecuteCustomeOperateHandleMethod(moduleId, "HasRecordOperatePermission", new object[] { userId, t, (int)dataPermissionType });
                    return obj.ObjToBool();
                }
            }
            catch { }
            List<string> orgIds = GetUserDataPermissions(userId, moduleId, dataPermissionType);
            if (orgIds.Count > 0)
            {
                if (orgIds.Contains("-1")) return true;
                List<Guid?> tempOrgIds = orgIds.Select(x => x.ObjToGuidNull()).ToList();
                if (orgIds.Contains(Guid.Empty.ObjToStr()) && userInfo.OrganizationId.HasValue && userInfo.OrganizationId.Value!=Guid.Empty &&
                    !orgIds.Contains(userInfo.OrganizationId.Value.ToString()))
                    tempOrgIds.Add(userInfo.OrganizationId.Value);
                if (tempOrgIds.Count > 0)
                    return tempOrgIds.Contains(t.OrgId);
                else
                    return t.CreateUserId == userId;
            }
            return t.CreateUserId == userId;
        }

        /// <summary>
        /// 获取角色可（查看、编辑、删除）模块中哪些组织下的数据，返回可（查看、编辑、删除）
        /// 的组织Id集合字典，
        /// 第一个参数为组织Id，第二个参数为是否来自父角色的权限，非当前
        /// 角色的权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="dataPermissionType">数据权限类型</param>
        /// <param name="read">是否为应用读取</param>
        /// <returns>返回组织Id集合字典</returns>
        public static Dictionary<string, bool> GetRoleDataPermissions(Guid roleId, Guid moduleId, DataPermissionTypeEnum dataPermissionType, bool read = false)
        {
            ISys_PermissionDataBLL permissionFunBll = BridgeObject.Resolve<ISys_PermissionDataBLL>();
            return permissionFunBll.GetRoleDataPermissions(roleId, moduleId, (int)dataPermissionType, read);
        }

        /// <summary>
        /// 获取用户可（查看、编辑、删除）模块中哪些组织下的数据，
        /// 返回可（查看、编辑、删除）的组织Id集合
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="dataPermissionType">数据权限类型</param>
        /// <returns>返回组织Id集合字典</returns>
        public static List<string> GetUserDataPermissions(Guid userId, Guid moduleId, DataPermissionTypeEnum dataPermissionType)
        {
            ISys_PermissionDataBLL permissionFunBll = BridgeObject.Resolve<ISys_PermissionDataBLL>();
            return permissionFunBll.GetUserDataPermissions(userId, moduleId, (int)dataPermissionType);
        }

        /// <summary>
        /// 获取用户数据权限条件语句
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="moduleId">模块id</param>
        /// <param name="dataPermissionType">数据权限类型</param>
        /// <returns></returns>
        public static string GetUserDataPermissionWhere(Guid userId, Guid moduleId, DataPermissionTypeEnum dataPermissionType)
        {
            List<string> orgIds = PermissionOperate.GetUserDataPermissions(userId, moduleId, dataPermissionType);
            if (orgIds.Contains("-1")) //有全部权限
                return string.Empty;
            UserInfo userInfo = UserOperate.GetUserInfo(userId);
            if (orgIds.Count > 0) //有配置数据权限
            {
                List<string> tempOrgIds = orgIds.Where(x => x!="-1").ToList();
                if (orgIds.Contains(Guid.Empty.ObjToStr()) && userInfo.OrganizationId.HasValue && userInfo.OrganizationId.Value!=Guid.Empty &&
                    !orgIds.Contains(userInfo.OrganizationId.Value.ToString()))
                    tempOrgIds.Add(userInfo.OrganizationId.Value.ToString());
                string where = tempOrgIds.Count > 0 ? string.Format("OrgId IN('{0}')", string.Join("','", tempOrgIds)) :
                              string.Format("CreateUserId='{0}'", userId);
                return where;
            }
            return string.Format("CreateUserId='{0}'", userId);
        }

        /// <summary>
        /// 获取用户数据权限条件表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="userId">用户Id</param>
        /// <param name="dataPermissionType">数据权限类型</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetUserDataPermissionExpression<T>(Guid userId, DataPermissionTypeEnum dataPermissionType) where T : BaseEntity
        {
            Sys_Module module = SystemOperate.GetModuleByTableName(typeof(T).Name);
            if (module == null) return x => false;
            List<string> orgIds = PermissionOperate.GetUserDataPermissions(userId, module.Id, dataPermissionType);
            if (orgIds.Contains("-1")) //有全部权限
                return x => true;
            UserInfo userInfo = UserOperate.GetUserInfo(userId);
            if (orgIds.Count > 0) //有配置数据权限
            {
                List<Guid?> tempOrgIds = orgIds.Where(x => x!="-1").Select(x => x.ObjToGuidNull()).ToList();
                if (orgIds.Contains(Guid.Empty.ToString()) && userInfo.OrganizationId.HasValue && userInfo.OrganizationId.Value!=Guid.Empty &&
                    !orgIds.Contains(userInfo.OrganizationId.Value.ToString()))
                    tempOrgIds.Add(userInfo.OrganizationId.Value);
                if (tempOrgIds.Count > 0)
                    return x => tempOrgIds.Contains(x.OrgId);
                else
                    return x => x.CreateUserId == userId;
            }
            return x => x.CreateUserId == userId;
        }

        /// <summary>
        /// 获取当前用户的数据权限条件语句
        /// </summary>
        /// <param name="moduleId">模块id</param>
        /// <param name="dataPermissionType">数据权限类型</param>
        /// <returns></returns>
        public static string GetCurrentUserDataPermissionWhere(Guid moduleId, DataPermissionTypeEnum dataPermissionType)
        {
            UserInfo userInfo = UserInfo.CurrentUserInfo;
            Guid userId = userInfo.UserId;
            return GetUserDataPermissionWhere(userId, moduleId, dataPermissionType);
        }

        /// <summary>
        /// 获取当前用户数据权限条件表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataPermissionType">数据权限类型</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> GetCurrentUserDataPermissionExpression<T>(DataPermissionTypeEnum dataPermissionType) where T : BaseEntity
        {
            UserInfo userInfo = UserInfo.CurrentUserInfo;
            Guid userId = userInfo.UserId;
            return GetUserDataPermissionExpression<T>(userId, dataPermissionType);
        }

        #endregion

        #region 权限保存

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <param name="permissionModels">权限集合</param>
        /// <param name="topMenuId">顶部菜单Id</param>
        /// <returns></returns>
        public static string SaveRolePermission(Guid roleId, List<PermissionModel> permissionModels, Guid? topMenuId = null)
        {
            Sys_Role role = GetRole(roleId);
            if (role == null) return "角色不存在，请确认角色Id是否正确！";
            if (permissionModels == null || permissionModels.Count == 0)
                return "权限数据异常！";
            string errMsg = string.Empty;
            //先删除该角色的所有权限
            if (topMenuId.HasValue && topMenuId.Value!=Guid.Empty)
            {
                //删除topMenuId下的子菜单权限和模块相关权限
                //获取子菜单
                List<Sys_Menu> menus = SystemOperate.GetChildMenus(topMenuId, false, true);
                List<Guid> menuIds = menus.Select(x => x.Id).ToList();
                //删除菜单权限
                int menuFunType = (int)FunctionTypeEnum.Menu;
                CommonOperate.DeleteRecordsByExpression<Sys_PermissionFun>(x => x.Sys_RoleId == roleId && x.FunType == menuFunType && menuIds.Contains(x.FunId), out errMsg);
                //删除网格按钮权限
                List<Guid?> moduleIds = menus.Where(x => x.Sys_ModuleId.HasValue && x.Sys_ModuleId.Value!=Guid.Empty).Select(x => x.Sys_ModuleId).ToList();
                int btnFunType = (int)FunctionTypeEnum.Button;
                List<Sys_GridButton> btns = CommonOperate.GetEntities<Sys_GridButton>(out errMsg, x => moduleIds.Contains(x.Sys_ModuleId), null, false);
                if (btns == null) btns = new List<Sys_GridButton>();
                List<Guid> btnIds = btns.Select(x => x.Id).ToList();
                CommonOperate.DeleteRecordsByExpression<Sys_PermissionFun>(x => x.Sys_RoleId == roleId && x.FunType == btnFunType && btnIds.Contains(x.FunId), out errMsg);
                //删除字段权限
                CommonOperate.DeleteRecordsByExpression<Sys_PermissionField>(x => x.Sys_RoleId == roleId && moduleIds.Contains(x.Sys_ModuleId), out errMsg);
                //删除数据权限
                CommonOperate.DeleteRecordsByExpression<Sys_PermissionData>(x => x.Sys_RoleId == roleId && moduleIds.Contains(x.Sys_ModuleId), out errMsg);
            }
            else
            {
                //删除全部功能权限
                CommonOperate.DeleteRecordsByExpression<Sys_PermissionFun>(x => x.Sys_RoleId == roleId, out errMsg);
                //删除全部字段权限
                CommonOperate.DeleteRecordsByExpression<Sys_PermissionField>(x => x.Sys_RoleId == roleId, out errMsg);
                //删除全部数据权限
                CommonOperate.DeleteRecordsByExpression<Sys_PermissionData>(x => x.Sys_RoleId == roleId, out errMsg);
            }
            //功能权限装载器
            List<Sys_PermissionFun> permissionFuns = new List<Sys_PermissionFun>();
            //字段权限装载器
            List<Sys_PermissionField> permissionFields = new List<Sys_PermissionField>();
            //数据权限装载器
            List<Sys_PermissionData> permissionDatas = new List<Sys_PermissionData>();
            foreach (PermissionModel permissionModel in permissionModels)
            {
                //菜单权限
                if (permissionModel.CanOpMeuId.HasValue && permissionModel.CanOpMeuId.Value!=Guid.Empty)
                {
                    permissionFuns.Add(new Sys_PermissionFun() { Sys_RoleId = roleId, FunId = permissionModel.CanOpMeuId.Value, FunTypeOfEnum = FunctionTypeEnum.Menu });
                }
                Sys_Module module = null;
                if (permissionModel.ModuleId.HasValue && permissionModel.ModuleId.Value!=Guid.Empty)
                    module = SystemOperate.GetModuleById(permissionModel.ModuleId.Value);
                if (module == null)
                    module = SystemOperate.GetModuleByName(permissionModel.ModuleName);
                if (module == null) continue;
                //操作权限
                if (permissionModel.CanOpBtnIds != null && permissionModel.CanOpBtnIds.Count > 0)
                {
                    permissionFuns.AddRange(permissionModel.CanOpBtnIds.Select(x => new Sys_PermissionFun() { Sys_RoleId = roleId, FunId = x, FunTypeOfEnum = FunctionTypeEnum.Button }));
                }
                //字段权限
                Sys_PermissionField permissionField = new Sys_PermissionField() { Sys_RoleId = roleId, Sys_ModuleId = module.Id };
                if (permissionModel.CanViewFields != null && permissionModel.CanViewFields.Count > 0)
                {
                    permissionField.CanViewFields = string.Join(",", permissionModel.CanViewFields);
                }
                if (permissionModel.CanAddFields != null && permissionModel.CanAddFields.Count > 0)
                {
                    permissionField.CanAddFields = string.Join(",", permissionModel.CanAddFields);
                }
                if (permissionModel.CanEditFields != null && permissionModel.CanEditFields.Count > 0)
                {
                    permissionField.CanEditFields = string.Join(",", permissionModel.CanEditFields);
                }
                permissionFields.Add(permissionField);
                //数据权限
                Sys_PermissionData permissionData = new Sys_PermissionData() { Sys_RoleId = roleId, Sys_ModuleId = module.Id };
                if (permissionModel.CanViewDataOrgIds != null && permissionModel.CanViewDataOrgIds.Count > 0)
                {
                    permissionData.CanViewOrgIds = string.Join(",", permissionModel.CanViewDataOrgIds);
                }
                if (permissionModel.CanEditDataOrgIds != null && permissionModel.CanEditDataOrgIds.Count > 0)
                {
                    permissionData.CanEditOrgIds = string.Join(",", permissionModel.CanEditDataOrgIds);
                }
                if (permissionModel.CanDelDataOrgIds != null && permissionModel.CanDelDataOrgIds.Count > 0)
                {
                    permissionData.CanDelOrgIds = string.Join(",", permissionModel.CanDelDataOrgIds);
                }
                permissionDatas.Add(permissionData);
            }
            StringBuilder errSb = new StringBuilder();
            //保存功能权限
            CommonOperate.OperateRecords<Sys_PermissionFun>(permissionFuns, ModelRecordOperateType.Add, out errMsg);
            if (!string.IsNullOrEmpty(errMsg)) errSb.AppendLine(errMsg);
            //保存字段权限
            CommonOperate.OperateRecords<Sys_PermissionField>(permissionFields, ModelRecordOperateType.Add, out errMsg);
            if (!string.IsNullOrEmpty(errMsg)) errSb.AppendLine(errMsg);
            //保存数据权限
            CommonOperate.OperateRecords<Sys_PermissionData>(permissionDatas, ModelRecordOperateType.Add, out errMsg);
            if (!string.IsNullOrEmpty(errMsg)) errSb.AppendLine(errMsg);
            return errSb.ToString();
        }

        /// <summary>
        /// 保存用户权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permissionModels">权限集合</param>
        /// <param name="topMenuId">顶部菜单ID</param>
        /// <returns></returns>
        public static string SaveUserPermission(Guid userId, List<PermissionModel> permissionModels, Guid? topMenuId = null)
        {
            Sys_User user = UserOperate.GetUser(userId);
            if (user == null) return "用户不存在，请确认用户Id是否正确！";
            if (permissionModels == null || permissionModels.Count == 0)
                return "权限数据异常！";
            string errMsg = string.Empty;
            //先删除该角色的所有权限
            if (topMenuId.HasValue && topMenuId.Value != Guid.Empty)
            {
                //删除topMenuId下的子菜单权限和模块相关权限
                //获取子菜单
                List<Sys_Menu> menus = SystemOperate.GetChildMenus(topMenuId, false, true);
                List<Guid> menuIds = menus.Select(x => x.Id).ToList();
                //删除菜单权限
                int menuFunType = (int)FunctionTypeEnum.Menu;
                CommonOperate.DeleteRecordsByExpression<Sys_UserPermissionFun>(x => x.Sys_UserId == userId && x.FunType == menuFunType && menuIds.Contains(x.FunId), out errMsg);
                //删除网格按钮权限
                List<Guid?> moduleIds = menus.Where(x => x.Sys_ModuleId.HasValue && x.Sys_ModuleId.Value != Guid.Empty).Select(x => x.Sys_ModuleId).ToList();
                int btnFunType = (int)FunctionTypeEnum.Button;
                List<Sys_GridButton> btns = CommonOperate.GetEntities<Sys_GridButton>(out errMsg, x => moduleIds.Contains(x.Sys_ModuleId), null, false);
                if (btns == null) btns = new List<Sys_GridButton>();
                List<Guid> btnIds = btns.Select(x => x.Id).ToList();
                CommonOperate.DeleteRecordsByExpression<Sys_UserPermissionFun>(x => x.Sys_UserId == userId && x.FunType == btnFunType && btnIds.Contains(x.FunId), out errMsg);
                //删除字段权限
                CommonOperate.DeleteRecordsByExpression<Sys_UserPermissionField>(x => x.Sys_UserId == userId && moduleIds.Contains(x.Sys_ModuleId), out errMsg);
                //删除数据权限
                CommonOperate.DeleteRecordsByExpression<Sys_UserPermissionData>(x => x.Sys_UserId == userId && moduleIds.Contains(x.Sys_ModuleId), out errMsg);
            }
            else
            {
                //删除全部功能权限
                CommonOperate.DeleteRecordsByExpression<Sys_UserPermissionFun>(x => x.Sys_UserId == userId, out errMsg);
                //删除全部字段权限
                CommonOperate.DeleteRecordsByExpression<Sys_UserPermissionField>(x => x.Sys_UserId == userId, out errMsg);
                //删除全部数据权限
                CommonOperate.DeleteRecordsByExpression<Sys_UserPermissionData>(x => x.Sys_UserId == userId, out errMsg);
            }
            //功能权限装载器
            List<Sys_UserPermissionFun> permissionFuns = new List<Sys_UserPermissionFun>();
            //字段权限装载器
            List<Sys_UserPermissionField> permissionFields = new List<Sys_UserPermissionField>();
            //数据权限装载器
            List<Sys_UserPermissionData> permissionDatas = new List<Sys_UserPermissionData>();
            foreach (PermissionModel permissionModel in permissionModels)
            {
                //菜单权限
                if (permissionModel.CanOpMeuId.HasValue && permissionModel.CanOpMeuId.Value != Guid.Empty)
                {
                    permissionFuns.Add(new Sys_UserPermissionFun() { Sys_UserId = userId, FunId = permissionModel.CanOpMeuId.Value, FunTypeOfEnum = FunctionTypeEnum.Menu });
                }
                Sys_Module module = null;
                if (permissionModel.ModuleId.HasValue && permissionModel.ModuleId.Value != Guid.Empty)
                    module = SystemOperate.GetModuleById(permissionModel.ModuleId.Value);
                if (module == null)
                    module = SystemOperate.GetModuleByName(permissionModel.ModuleName);
                if (module == null) continue;
                //操作权限
                if (permissionModel.CanOpBtnIds != null && permissionModel.CanOpBtnIds.Count > 0)
                {
                    permissionFuns.AddRange(permissionModel.CanOpBtnIds.Select(x => new Sys_UserPermissionFun() { Sys_UserId = userId, FunId = x, FunTypeOfEnum = FunctionTypeEnum.Button }));
                }
                //字段权限
                Sys_UserPermissionField permissionField = new Sys_UserPermissionField() { Sys_UserId = userId, Sys_ModuleId = module.Id };
                if (permissionModel.CanViewFields != null && permissionModel.CanViewFields.Count > 0)
                {
                    permissionField.CanViewFields = string.Join(",", permissionModel.CanViewFields);
                }
                if (permissionModel.CanAddFields != null && permissionModel.CanAddFields.Count > 0)
                {
                    permissionField.CanAddFields = string.Join(",", permissionModel.CanAddFields);
                }
                if (permissionModel.CanEditFields != null && permissionModel.CanEditFields.Count > 0)
                {
                    permissionField.CanEditFields = string.Join(",", permissionModel.CanEditFields);
                }
                permissionFields.Add(permissionField);
                //数据权限
                Sys_UserPermissionData permissionData = new Sys_UserPermissionData() { Sys_UserId = userId, Sys_ModuleId = module.Id };
                if (permissionModel.CanViewDataOrgIds != null && permissionModel.CanViewDataOrgIds.Count > 0)
                {
                    permissionData.CanViewOrgIds = string.Join(",", permissionModel.CanViewDataOrgIds);
                }
                if (permissionModel.CanEditDataOrgIds != null && permissionModel.CanEditDataOrgIds.Count > 0)
                {
                    permissionData.CanEditOrgIds = string.Join(",", permissionModel.CanEditDataOrgIds);
                }
                if (permissionModel.CanDelDataOrgIds != null && permissionModel.CanDelDataOrgIds.Count > 0)
                {
                    permissionData.CanDelOrgIds = string.Join(",", permissionModel.CanDelDataOrgIds);
                }
                permissionDatas.Add(permissionData);
            }
            StringBuilder errSb = new StringBuilder();
            //保存功能权限
            CommonOperate.OperateRecords<Sys_UserPermissionFun>(permissionFuns, ModelRecordOperateType.Add, out errMsg);
            if (!string.IsNullOrEmpty(errMsg)) errSb.AppendLine(errMsg);
            //保存字段权限
            CommonOperate.OperateRecords<Sys_UserPermissionField>(permissionFields, ModelRecordOperateType.Add, out errMsg);
            if (!string.IsNullOrEmpty(errMsg)) errSb.AppendLine(errMsg);
            //保存数据权限
            CommonOperate.OperateRecords<Sys_UserPermissionData>(permissionDatas, ModelRecordOperateType.Add, out errMsg);
            if (!string.IsNullOrEmpty(errMsg)) errSb.AppendLine(errMsg);
            return errSb.ToString();
        }

        #endregion

        #endregion
    }
}
