﻿/*----------------------------------------------------------------
        // Copyright (C) 2016 Kinfar.
        // 版权所有
        // 开发者：Kinfar.
        // Email：kinfar@foxmail.net
        // QQ：3133119519
//----------------------------------------------------------------*/

using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Kinfar.Frame.Base;
using Kinfar.Frame.Operate.Base.TempModel;
using Kinfar.Frame.Model.EnumSpace;
using Kinfar.Frame.Model.Sys;
using Kinfar.Frame.Common;
using Kinfar.Frame.Operate.Base;
using Kinfar.Frame.Operate.Base.OperateHandle;
using Kinfar.Frame.Model.OrgM;

namespace Kinfar.Frame.Operate.Base
{
    /// <summary>
    /// 用户操作类
    /// </summary>
    public static class UserOperate
    {
        #region 用户

        /// <summary>
        /// 临时用户操作
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="userpwd">用户密码</param>
        private static string TempUserOperate(Guid userId, string userpwd)
        {
            string errMsg = string.Empty;
            try
            {
                ModelRecordOperateType operateType = ModelRecordOperateType.Edit;
                Sys_TempUser tempUser = CommonOperate.GetEntity<Sys_TempUser>(x => x.FieldInfo1 == userId.ToString(), string.Empty, out errMsg);
                if (tempUser == null)
                {
                    operateType = ModelRecordOperateType.Add;
                    tempUser = new Sys_TempUser();
                    tempUser.FieldInfo1 = userId.ToString();
                }
                tempUser.FieldInfo2 = MySecurity.DES3EncryptString(string.Format("{0}_0123456789", Guid.Empty.ToString()), userpwd, "sy654321");
                CommonOperate.OperateRecord<Sys_TempUser>(tempUser, operateType, out errMsg);
            }
            catch (Exception ex)
            {
                errMsg = ex.Message;
            }
            return errMsg;
        }

        /// <summary>
        /// 根据用户Id获取用户名
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static string GetUserNameByUserId(Guid userId)
        {
            string errMsg = string.Empty;
            Sys_User tempUser = CommonOperate.GetEntityById<Sys_User>(userId, out errMsg);
            if (tempUser != null)
            {
                return tempUser.UserName;
            }
            return string.Empty;
        }

        /// <summary>
        /// 根据用户名获取用户Id
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public static Guid GetUserIdByUserName(string username)
        {
            string errMsg = string.Empty;
            Sys_User tempUser = CommonOperate.GetEntity<Sys_User>(x => x.UserName == username && !x.IsDeleted && x.IsActivated && x.IsValid, null, out errMsg);
            if (tempUser != null)
            {
                return tempUser.Id;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="userpwd">密码</param>
        /// <param name="errMsg">异常信息</param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(string username, string userpwd, out string errMsg)
        {
            errMsg = string.Empty;
            Sys_User tempUser = CommonOperate.GetEntity<Sys_User>(x => !x.IsDeleted && x.UserName.ToLower() == username.Trim().ToLower(), string.Empty, out errMsg);
            string passwordSalt = tempUser == null ? string.Empty : tempUser.PasswordSalt;
            if (tempUser == null || tempUser.IsDeleted)
            {
                errMsg = "账户不存在！";
                return null;
            }
            if (string.IsNullOrEmpty(passwordSalt))
            {
                errMsg = "账户存在异常！";
                return null;
            }
            string passwordHash = SecurityHelper.EncodePassword(MySecurity.MD5(userpwd), passwordSalt);
            Sys_User user = CommonOperate.GetEntity<Sys_User>(x => !x.IsDeleted && x.UserName.ToLower() == username.Trim().ToLower() &&
                           x.PasswordHash == passwordHash, string.Empty, out errMsg);
            if (user == null)
            {
                errMsg = "密码输入有误";
                return null;
            }
            if (!user.IsValid)
            {
                errMsg = "账户被冻结";
                return null;
            }
            if (!user.IsActivated)
            {
                errMsg = "账户未激活，请先激活账户";
                return null;
            }
            UserInfo userInfo = new UserInfo()
            {
                UserId = user.Id,
                UserName = user.UserName,
                AliasName = string.IsNullOrEmpty(user.AliasName) ? user.UserName : user.AliasName,
                OrganizationId = user.Sys_OrganizationId
            };
            //员工信息
            OrgM_Emp emp = OrgMOperate.GetEmpByUserName(username);
            if (emp != null)
            {
                userInfo.EmpId = emp.Id;
                userInfo.EmpCode = emp.Code;
                userInfo.EmpName = emp.Name;
            }
            userInfo.ExtendUserObject = UserExtendEventHandler.GetUserExtendInfo(userInfo);
            return userInfo;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static UserInfo GetUserInfo(Guid userId)
        {
            string errMsg = string.Empty;
            Sys_User user = CommonOperate.GetEntityById<Sys_User>(userId, out errMsg);
            if (user == null) return null;
            UserInfo userInfo = new UserInfo()
            {
                UserId = user.Id,
                UserName = user.UserName,
                AliasName = string.IsNullOrEmpty(user.AliasName) ? user.UserName : user.AliasName,
                OrganizationId = user.Sys_OrganizationId
            };
            //员工信息
            OrgM_Emp emp = OrgMOperate.GetEmpByUserName(user.UserName);
            if (emp != null)
            {
                userInfo.EmpId = emp.Id;
                userInfo.EmpCode = emp.Code;
                userInfo.EmpName = emp.Name;
            }
            userInfo.ExtendUserObject = UserExtendEventHandler.GetUserExtendInfo(userInfo);
            return userInfo;
        }

        /// <summary>
        /// 账户是否有效
        /// </summary>
        /// <param name="username"></param>
        /// <param name="errMsg">异常信息</param>
        /// <returns></returns>
        public static bool UserIsValid(string username, out string errMsg)
        {
            errMsg = string.Empty;
            Sys_User tempUser = CommonOperate.GetEntity<Sys_User>(x => !x.IsDeleted && x.IsValid && x.IsActivated && x.UserName.ToLower() == username.Trim().ToLower(), string.Empty, out errMsg);
            return tempUser != null;
        }

        /// <summary>
        /// 获取用户密码，找回密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static string GetPassword(Guid userId)
        {
            string errMsg = string.Empty;
            //从用户临时表中找密码
            Sys_TempUser tempUser = CommonOperate.GetEntity<Sys_TempUser>(x => x.FieldInfo1 == userId.ToString(), string.Empty, out errMsg);
            if (tempUser == null) return string.Empty;
            string pwd = MySecurity.DES3EncryptString(string.Format("{0}_0123456789", Guid.Empty.ToString()), tempUser.FieldInfo2, "sy654321");
            return pwd;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="newPwd">新密码</param>
        /// <param name="errMsg">错误信息</param>
        /// <returns></returns>
        public static bool ModifyPassword(Guid userId, string newPwd, out string errMsg)
        {
            errMsg = string.Empty;
            Sys_User user = CommonOperate.GetEntity<Sys_User>(x => x.Id == userId && !x.IsDeleted && x.IsValid && x.IsActivated, string.Empty, out errMsg);
            if (user == null)
            {
                errMsg = "非法用户！";
                return false;
            }
            //获取混淆码
            string passwordSalt = SecurityHelper.GenerateSalt();
            //获取混淆码加密过的密码
            string passwordHash = SecurityHelper.EncodePassword(MySecurity.MD5(newPwd), passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            Guid rs = CommonOperate.OperateRecord<Sys_User>(user, ModelRecordOperateType.Edit, out errMsg, new List<string>() { "PasswordHash", "PasswordSalt" });
            if (rs!=Guid.Empty) //修改密码成功
            {
                errMsg = TempUserOperate(userId, newPwd);
            }
            return string.IsNullOrEmpty(errMsg);
        }

        /// <summary>
        /// 更新用户的别名
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="aliasName">用户别名</param>
        /// <returns>成功返回空字符串，失败返回异常信息</returns>
        public static string UpdateUserAliasName(Guid userId, string aliasName)
        {
            if (string.IsNullOrWhiteSpace(aliasName))
                return "用户别名不能为空！";
            string errMsg = string.Empty;
            Sys_User user = CommonOperate.GetEntityById<Sys_User>(userId, out errMsg);
            if (user == null) return "用户信息不存在！";
            if (user.AliasName != aliasName)
            {
                user.AliasName = aliasName;
                Guid rs = CommonOperate.OperateRecord<Sys_User>(user, ModelRecordOperateType.Edit, out errMsg, new List<string>() { "AliasName" });
            }
            return errMsg;
        }

        /// <summary>
        /// 修改用户别名
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="aliasName">用户别名</param>
        /// <returns></returns>
        public static string UpdateUserAliasName(string username, string aliasName)
        {
            if (string.IsNullOrWhiteSpace(aliasName))
                return "用户别名不能为空！";
            string errMsg = string.Empty;
            Sys_User user = CommonOperate.GetEntity<Sys_User>(x => x.UserName == username, null, out errMsg);
            if (user == null) return "用户信息不存在！";
            if (user.AliasName != aliasName)
            {
                user.AliasName = aliasName;
                Guid rs = CommonOperate.OperateRecord<Sys_User>(user, ModelRecordOperateType.Edit, out errMsg, new List<string>() { "AliasName" });
            }
            return errMsg;
        }

        /// <summary>
        /// 获取超级管理员用户信息
        /// </summary>
        /// <param name="isLoadExpand">是否加载扩展用户信息</param>
        /// <returns></returns>
        public static UserInfo GetSuperAdmin()
        {
            string errMsg = string.Empty;
            Sys_User user = CommonOperate.GetEntity<Sys_User>(x => x.UserName == "admin", null, out errMsg);
            if (user == null) user = new Sys_User() { Id = Guid.Empty, UserName = null, AliasName = null };
            UserInfo userInfo = new UserInfo()
            {
                UserId = user.Id,
                UserName = user.UserName,
                AliasName = string.IsNullOrEmpty(user.AliasName) ? user.UserName : user.AliasName
            };
            return userInfo;
        }

        /// <summary>
        /// 是否为超级管理员
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public static bool IsSuperAdmin(Guid userId)
        {
            UserInfo userInfo = GetSuperAdmin();
            return userInfo.UserId == userId;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="errMsg">异常信息</param>
        /// <param name="username">用户名</param>
        /// <param name="userpwd">用户密码</param>
        /// <param name="orgId">组织Id</param>
        /// <param name="aliasName">用户别名</param>
        /// <returns></returns>
        public static Guid AddUser(out string errMsg, string username, string userpwd, Guid? orgId = null, string aliasName = null)
        {
            errMsg = string.Empty;
            if (string.IsNullOrWhiteSpace(username))
            {
                errMsg = "用户名不能为空！";
                return Guid.Empty;
            }
            if (string.IsNullOrWhiteSpace(userpwd))
            {
                errMsg = "用户密码不能为空！";
                return Guid.Empty;
            }
            Sys_User tempUser = CommonOperate.GetEntity<Sys_User>(x => x.UserName.ToLower() == username.Trim().ToLower(), null, out errMsg);
            if (tempUser != null)
            {
                errMsg = string.Format("用户【{0}】已存在！", username);
                return Guid.Empty;
            }
            UserInfo adminUser = GetSuperAdmin(); //超级管理员用户
            //获取混淆码
            string passwordSalt = SecurityHelper.GenerateSalt();
            //获取混淆码加密过的密码
            string passwordHash = SecurityHelper.EncodePassword(MySecurity.MD5(userpwd), passwordSalt);
            Sys_User user = new Sys_User()
            {
                UserName = username,
                AliasName = aliasName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsValid = true,
                IsActivated = true,
                ActivatedDate = DateTime.Now
            };
            if (orgId.HasValue && orgId.Value!=Guid.Empty)
                user.Sys_OrganizationId = orgId.Value;
            if (adminUser != null)
            {
                user.CreateUserName = adminUser.AliasName;
                user.CreateDate = DateTime.Now;
                user.CreateUserId = adminUser.UserId;
                user.ModifyUserName = adminUser.AliasName;
                user.ModifyDate = DateTime.Now;
                user.ModifyUserId = adminUser.UserId;
            }
            Guid userId = CommonOperate.OperateRecord<Sys_User>(user, ModelRecordOperateType.Add, out errMsg);
            //临时用户操作
            if (userId!=Guid.Empty)
            {
                TempUserOperate(userId, userpwd);
            }
            return userId;
        }

        /// <summary>
        /// 冻结账号
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public static string FrozenUser(string username)
        {
            string errMsg = string.Empty;
            Sys_User user = CommonOperate.GetEntity<Sys_User>(x => x.UserName == username && !x.IsDeleted && x.IsValid && x.IsActivated, string.Empty, out errMsg);
            if (user == null) return "非法用户！";
            user.IsValid = false;
            Guid rs = CommonOperate.OperateRecord<Sys_User>(user, ModelRecordOperateType.Edit, out errMsg, new List<string>() { "IsValid" });
            return errMsg;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public static string DelUser(string username)
        {
            string errMsg = string.Empty;
            Sys_User user = CommonOperate.GetEntity<Sys_User>(x => x.UserName == username && !x.IsDeleted && x.IsValid && x.IsActivated, string.Empty, out errMsg);
            if (user == null) return "非法用户！";
            bool rs = CommonOperate.DeleteRecordsByExpression<Sys_User>(x => x.UserName == username, out errMsg);
            if (rs) //用户删除成功后删除临时用户
            {
                CommonOperate.DeleteRecordsByExpression<Sys_TempUser>(x => x.FieldInfo1 == user.Id.ToString(), out errMsg);
            }
            return errMsg;
        }

        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<Sys_User> GetAllUsers(Expression<Func<Sys_User, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<Sys_User, bool>> exp = x => !x.IsDeleted && !x.IsDraft;
            if (expression != null) exp = ExpressionExtension.And<Sys_User>(exp, expression);
            List<Sys_User> users = CommonOperate.GetEntities<Sys_User>(out errMsg, exp, null, false);
            if (users == null) users = new List<Sys_User>();
            return users;
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public static Sys_User GetUser(string username)
        {
            return GetAllUsers(x => x.UserName == username).FirstOrDefault();
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static Sys_User GetUser(Guid userId)
        {
            return GetAllUsers(x => x.Id == userId).FirstOrDefault();
        }

        /// <summary>
        /// 组织用户树
        /// </summary
        /// <param name="orgId">组织根结点ID，为空是加载整棵树</param>
        /// <returns></returns>
        public static TreeNode LoadOrgUserTree(Guid? orgId)
        {
            TreeNode node = null;
            Sys_Organization orgRoot = GetOrgRoot();
            Sys_Organization root = orgId.HasValue && orgId.Value!=Guid.Empty ? GetOrg(orgId.Value) : orgRoot;
            List<TreeNode> list = new List<TreeNode>();
            if (root != null)
            {
                //组织根结点
                node = new TreeNode()
                {
                    id = root.Id.ToString(),
                    text = root.Name,
                    iconCls = "eu-icon-dept"
                };
                //加载用户节点
                List<Sys_User> listUsers = GetOrgUsers(root.Id, true); //该组织下用户
                if (listUsers != null && listUsers.Count > 0)
                {
                    List<TreeNode> empNodes = listUsers.Select(x => new TreeNode()
                    {
                        id = x.Id.ToString(),
                        text = string.IsNullOrEmpty(x.AliasName) ? x.UserName : string.Format("{0}({1})", x.UserName, x.AliasName),
                        iconCls = "eu-icon-user"
                    }).ToList();
                    list.AddRange(empNodes);
                }
                //加载组织子结点
                List<Sys_Organization> listOrgs = GetChildOrgs(root.Id, true);
                foreach (Sys_Organization org in listOrgs)
                {
                    TreeNode tempNode = LoadOrgUserTree(org.Id);
                    if (tempNode != null && tempNode.children != null && tempNode.children.ToList().Count > 0)
                    {
                        list.Add(tempNode);
                    }
                }
                node.children = list;
                if (orgRoot != null && orgRoot.Id == root.Id)
                {
                    //加载没有组织的用户
                    List<Sys_User> noOrgUsers = GetAllUsers(x => x.Sys_OrganizationId == null || x.Sys_OrganizationId == Guid.Empty);
                    if (noOrgUsers.Count > 0)
                    {
                        TreeNode tempNode = new TreeNode()
                        {
                            id = Guid.Empty.ToString(),
                            text = "根结点",
                            iconCls = "eu-icon-dept"
                        };
                        List<TreeNode> tempChildren = new List<TreeNode>() { node };
                        List<TreeNode> noUserNodes = noOrgUsers.Select(x => new TreeNode()
                        {
                            id = x.Id.ToString(),
                            text = string.IsNullOrEmpty(x.AliasName) ? x.UserName : string.Format("{0}({1})", x.UserName, x.AliasName),
                            iconCls = "eu-icon-user"
                        }).ToList();
                        tempChildren.AddRange(noUserNodes);
                        tempNode.children = tempChildren;
                        node = tempNode;
                    }
                }
            }
            else
            {
                //加载用户节点
                List<Sys_User> listUsers = GetAllUsers();
                if (listUsers != null && listUsers.Count > 0)
                {
                    node = new TreeNode()
                    {
                        id = Guid.Empty.ToString(),
                        text = "根结点",
                        iconCls = "eu-icon-dept"
                    };
                    List<TreeNode> empNodes = listUsers.Select(x => new TreeNode()
                    {
                        id = x.Id.ToString(),
                        text = string.IsNullOrEmpty(x.AliasName) ? x.UserName : string.Format("{0}({1})", x.UserName, x.AliasName),
                        iconCls = "eu-icon-user"
                    }).ToList();
                    list.AddRange(empNodes);
                    node.children = list;
                }
            }
            return node;
        }

        #endregion

        #region 组织

        /// <summary>
        /// 获取所有组织
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<Sys_Organization> GetAllOrgs(Expression<Func<Sys_Organization, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<Sys_Organization, bool>> exp = x => !x.IsDeleted && !x.IsDraft;
            if (expression != null) exp = ExpressionExtension.And<Sys_Organization>(exp, expression);
            List<Sys_Organization> orgs = CommonOperate.GetEntities<Sys_Organization>(out errMsg, exp, null, false);
            if (orgs == null) orgs = new List<Sys_Organization>();
            return orgs;
        }

        /// <summary>
        /// 获取组织根结点
        /// </summary>
        /// <returns></returns>
        public static Sys_Organization GetOrgRoot()
        {
            List<Sys_Organization> list = GetAllOrgs(x => x.ParentId == null || x.ParentId == Guid.Empty);
            if (list.Count == 1) return list.FirstOrDefault();
            return null;
        }

        /// <summary>
        /// 获取组织
        /// </summary>
        /// <param name="orgId">组织Id</param>
        /// <returns></returns>
        public static Sys_Organization GetOrg(Guid orgId)
        {
            return GetAllOrgs(x => x.Id == orgId).FirstOrDefault();
        }

        /// <summary>
        /// 获取组织
        /// </summary>
        /// <param name="orgName">组织名称</param>
        /// <returns></returns>
        public static Sys_Organization GetOrg(string orgName)
        {
            return GetAllOrgs(x => x.Name == orgName).FirstOrDefault();
        }

        /// <summary>
        /// 获取组织用户
        /// </summary>
        /// <param name="orgId">组织Id</param>
        /// <param name="isDirect">是否取直接组织下用户,为否时包含子组织下用户</param>
        /// <returns></returns>
        public static List<Sys_User> GetOrgUsers(Guid orgId, bool isDirect = false)
        {
            List<Sys_User> listUsers = new List<Sys_User>();
            if (isDirect) //取组织用户，不包括子组织用户
            {
                listUsers = GetAllUsers(x => x.Sys_OrganizationId == orgId);
            }
            else //取组织用户，包含所有子组织用户
            {
                listUsers.AddRange(GetOrgUsers(orgId, true)); //添加当前组织用户
                List<Sys_Organization> listOrgs = GetChildOrgs(orgId); //取所有子组织
                foreach (Sys_Organization org in listOrgs)
                {
                    listUsers.AddRange(GetOrgUsers(org.Id, true)); //添加子组织用户
                }
            }
            return listUsers;
        }

        /// <summary>
        /// 获取子组织
        /// </summary>
        /// <param name="parentOrgId">父级组织ID</param>
        /// <param name="isDirect">是否直接子组织，否则取所有子级组织</param>
        /// <returns></returns>
        public static List<Sys_Organization> GetChildOrgs(Guid parentOrgId, bool isDirect = false)
        {
            List<Sys_Organization> orgs = GetAllOrgs(x => x.ParentId != null && x.ParentId == parentOrgId);
            if (isDirect) //取直接子组织
            {
                return orgs;
            }
            List<Sys_Organization> list = new List<Sys_Organization>();
            foreach (Sys_Organization org in orgs)
            {
                list.Add(org);
                list.AddRange(GetChildOrgs(org.Id, isDirect));
            }
            return list;
        }

        /// <summary>
        /// 根据组织Id集合获取组织集合，包括为0的组织，为0的组织表示"本部门"，
        /// 为-1的表示全部
        /// </summary>
        /// <param name="orgIds">组织Id集合</param>
        /// <returns></returns>
        public static List<TempOrganization> GetFormatOrgs(List<string> orgIds)
        {
            if (orgIds == null || orgIds.Count == 0)
                return new List<TempOrganization>();
            List<TempOrganization> orgs = new List<TempOrganization>();
            foreach (string orgId in orgIds)
            {
                if (orgId == Guid.Empty.ToString())
                    orgs.Add(new TempOrganization() { Id = "0", Name = "本部门", Des = "本部门" });
                else if (orgId == "-1")
                    orgs.Add(new TempOrganization() { Id = "-1", Name = "全部", Des = "全部" });
                Sys_Organization org = GetOrg(orgId.ObjToGuid());
                if (org != null)
                    orgs.Add(new TempOrganization() { Id = org.Id.ToString(), Name = org.Name, Des = org.Des });
            }
            return orgs;
        }

        #endregion
    }

    /// <summary>
    /// 临时组织类
    /// </summary>
    public class TempOrganization
    {
        /// <summary>
        /// 组织ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 组织描述
        /// </summary>
        public string Des { get; set; }
    }
}
