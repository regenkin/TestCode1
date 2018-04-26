using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kinfar.Frame.Common;
using Kinfar.Frame.Model.OrgM;
using System.Linq.Expressions;
using Kinfar.Frame.Operate.Base.TempModel;
using Kinfar.Frame.Model.EnumSpace;
using Kinfar.Frame.Base.Set;

namespace Kinfar.Frame.Operate.Base
{
    /// <summary>
    /// 组织机构操作
    /// </summary>
    public static class OrgMOperate
    {
        #region 部门

        #region 基本

        /// <summary>
        /// 获取所有部门
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<OrgM_Dept> GetAllDepts(Expression<Func<OrgM_Dept, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<OrgM_Dept, bool>> exp = x => !x.IsDeleted && !x.IsDraft && x.IsValid;
            if (expression != null) exp = ExpressionExtension.And<OrgM_Dept>(exp, expression);
            List<OrgM_Dept> depts = CommonOperate.GetEntities<OrgM_Dept>(out errMsg, exp, null, false, new List<string>() { "Sort" }, new List<bool> { false });
            if (depts == null) depts = new List<OrgM_Dept>();
            return depts;
        }

        /// <summary>
        /// 获取部门根结点
        /// </summary>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static OrgM_Dept GetDeptRoot(Guid? companyId = null)
        {
            if (companyId.HasValue && companyId.Value != Guid.Empty)
                return GetDeptById(companyId.Value);
            List<OrgM_Dept> list = GetAllDepts(x => x.ParentId == null || x.ParentId == Guid.Empty);
            if (list.Count == 1) return list.FirstOrDefault();
            return null;
        }

        /// <summary>
        /// 获取所有的分管部门，公司下的一级部门
        /// </summary>
        /// <returns></returns>
        public static List<OrgM_Dept> GetChargeDepts()
        {
            OrgM_Dept root = GetDeptRoot();
            if (root != null)
                return GetAllDepts(x => x.ParentId == root.Id);
            return new List<OrgM_Dept>();
        }

        /// <summary>
        /// 根据部门ID获取部门信息
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public static OrgM_Dept GetDeptById(Guid deptId)
        {
            string errMsg = string.Empty;
            OrgM_Dept dept = CommonOperate.GetEntityById<OrgM_Dept>(deptId, out errMsg);
            return dept;
        }

        /// <summary>
        /// 根据部门编码获取部门信息
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        public static OrgM_Dept GetDeptByCode(string deptCode)
        {
            string errMsg = string.Empty;
            OrgM_Dept dept = CommonOperate.GetEntity<OrgM_Dept>(x => x.Code == deptCode && !x.IsDeleted && !x.IsDraft && x.IsValid, null, out errMsg);
            return dept;
        }

        /// <summary>
        /// 根据部门名称获取部门信息
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <returns></returns>
        public static OrgM_Dept GetDeptByName(string deptName)
        {
            string errMsg = string.Empty;
            OrgM_Dept dept = CommonOperate.GetEntity<OrgM_Dept>(x => x.Name == deptName && !x.IsDeleted && !x.IsDraft && x.IsValid, null, out errMsg);
            return dept;
        }

        /// <summary>
        /// 获取层级部门，第一层级、第二层级、...
        /// </summary>
        /// <param name="levelDepth">层级</param>
        /// <returns></returns>
        public static List<OrgM_Dept> GetLevelDepthDepts(int levelDepth)
        {
            string errMsg = string.Empty;
            Expression<Func<OrgM_Dept, bool>> exp = x => x.LevelDepth == levelDepth && !x.IsDeleted && !x.IsDraft && x.IsValid;
            List<OrgM_Dept> depts = CommonOperate.GetEntities<OrgM_Dept>(out errMsg, exp, null, false, new List<string>() { "Sort" }, new List<bool>() { false });
            if (depts == null) depts = new List<OrgM_Dept>();
            return depts;
        }

        #endregion

        #region 父部门

        /// <summary>
        /// 获取部门的父部门
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public static OrgM_Dept GetParentDept(Guid deptId)
        {
            OrgM_Dept dept = GetDeptById(deptId);
            if (dept.ParentId.HasValue && dept.ParentId.Value != Guid.Empty)
            {
                return GetDeptById(dept.ParentId.Value);
            }
            return null;
        }

        /// <summary>
        /// 获取部门的父部门
        /// </summary>
        /// <param name="deptCode">部门编码</param>
        /// <returns></returns>
        public static OrgM_Dept GetParentDeptByCode(string deptCode)
        {
            OrgM_Dept dept = GetDeptByCode(deptCode);
            if (dept.ParentId.HasValue && dept.ParentId.Value != Guid.Empty)
            {
                return GetDeptById(dept.ParentId.Value);
            }
            return null;
        }

        /// <summary>
        /// 获取部门的父部门
        /// </summary>
        /// <param name="deptName">部门名称</param>
        /// <returns></returns>
        public static OrgM_Dept GetParentDeptByName(string deptName)
        {
            OrgM_Dept dept = GetDeptByName(deptName);
            if (dept.ParentId.HasValue && dept.ParentId.Value != Guid.Empty)
            {
                return GetDeptById(dept.ParentId.Value);
            }
            return null;
        }

        /// <summary>
        /// 获取所有父级部门
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static List<OrgM_Dept> GetParentsDepts(Guid deptId, Guid? companyId = null)
        {
            List<OrgM_Dept> depts = new List<OrgM_Dept>();
            OrgM_Dept dept = GetDeptById(deptId);
            bool flag = dept != null && dept.ParentId.HasValue && dept.ParentId.Value != Guid.Empty;
            if (companyId.HasValue && companyId.Value != Guid.Empty)
                flag = flag && deptId != companyId.Value;
            if (flag)
            {
                depts.AddRange(GetParentsDepts(dept.ParentId.Value));
            }
            return depts;
        }

        /// <summary>
        /// 获取当前部门的分管部门
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static OrgM_Dept GetCurrChargeDept(Guid deptId, Guid? companyId = null)
        {
            List<OrgM_Dept> depts = GetParentsDepts(deptId, companyId);
            OrgM_Dept root = GetDeptRoot(companyId);
            if (root != null)
                return depts.Where(x => x.ParentId == root.Id).FirstOrDefault();
            return null;
        }

        #endregion

        #region 子部门

        /// <summary>
        /// 获取子部门
        /// </summary>
        /// <param name="parentDeptId">父级部门ID</param>
        /// <param name="isDirect">是否直接子部门，否则取所有子级部门</param>
        /// <returns></returns>
        public static List<OrgM_Dept> GetChildDepts(Guid parentDeptId, bool isDirect = false)
        {
            List<OrgM_Dept> depts = GetAllDepts(x => x.ParentId != null && x.ParentId == parentDeptId);
            if (isDirect) //取直接子部门
            {
                return depts;
            }
            List<OrgM_Dept> list = new List<OrgM_Dept>();
            foreach (OrgM_Dept dept in depts)
            {
                list.Add(dept);
                list.AddRange(GetChildDepts(dept.Id, isDirect));
            }
            return list;
        }

        #endregion

        #region 部门树

        /// <summary>
        /// 加载部门树
        /// </summary>
        /// <param name="deptId">部门根节点Id，为空是加载整棵树</param>
        /// <param name="expression">条件过滤表达式</param>
        /// <returns></returns>
        public static TreeNode LoadDeptTree(Guid? deptId, Expression<Func<OrgM_Dept, bool>> expression = null)
        {
            OrgM_Dept root = !deptId.HasValue || deptId == Guid.Empty?GetDeptRoot():GetDeptById(deptId.Value);
            if (root == null)
            {
                TreeNode node = null;
                //加载部门节点
                List<OrgM_Dept> listDelts = GetAllDepts();
                if (listDelts != null && listDelts.Count > 0)
                {
                    node = new TreeNode()
                    {
                        id = Guid.Empty.ToString(),
                        text = "根结点",
                        iconCls = "eu-icon-dept"
                    };
                    List<TreeNode> deptNodes = listDelts.Select(x => new TreeNode()
                    {
                        id = x.Id.ToString(),
                        text = x.Name,
                        iconCls = "eu-icon-dept"
                    }).ToList();
                    node.children = deptNodes;
                }
                return node;
            }
            else
            {
                List<OrgM_Dept> listChilds = GetChildDepts(root.Id, false); //获取子部门
                if (expression != null)
                {
                    listChilds = listChilds.Where(expression.Compile()).ToList();
                }
                var tree = CommonOperate.GetTree<OrgM_Dept>(listChilds, root, null, "ParentId", "Alias", "eu-icon-dept");
                return tree;
            }
        }

        #endregion

        #region 部门岗位

        /// <summary>
        /// 获取部门岗位
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public static List<OrgM_DeptDuty> GetDeptPositions(Guid deptId)
        {
            return GetAllPositions(x => x.OrgM_DeptId == deptId);
        }

        #endregion

        #region 部门职务

        /// <summary>
        /// 获取部门所有职务
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public static List<OrgM_Duty> GetDeptDutys(Guid deptId)
        {
            List<OrgM_DeptDuty> positions = GetDeptPositions(deptId);
            if (positions.Count > 0)
            {
                List<Guid> dutyIds = positions.Where(x => x.OrgM_DutyId.HasValue).Select(x => x.OrgM_DutyId.Value).ToList();
                return GetAllDutys(x => dutyIds.Contains(x.Id));
            }
            return new List<OrgM_Duty>();
        }

        #endregion

        #region 部门员工

        /// <summary>
        /// 获取部门人员
        /// </summary>
        /// <param name="deptId">部门Id</param>
        /// <param name="isDirect">是否取直接部门下人员,为否时包含子部门下人员</param>
        /// <returns></returns>
        public static List<OrgM_Emp> GetDeptEmps(Guid deptId, bool isDirect = false)
        {
            List<OrgM_Emp> listEmps = new List<OrgM_Emp>();
            if (isDirect) //取部门人员，不包括子部门人员
            {
                List<OrgM_EmpDeptDuty> empPositions = GetAllEmpPositions(x => x.OrgM_DeptId == deptId);
                if (empPositions.Count > 0)
                {
                    List<Guid> empIds = empPositions.Where(x => x.OrgM_EmpId.HasValue).Select(x => x.OrgM_EmpId.Value).ToList();
                    listEmps = GetAllEmps(x => empIds.Contains(x.Id));
                }
            }
            else //取部门人员，包含所有子部门人员
            {
                listEmps.AddRange(GetDeptEmps(deptId, true)); //添加当前部门人员
                List<OrgM_Dept> listDepts = GetChildDepts(deptId); //取所有子部门
                foreach (OrgM_Dept dept in listDepts)
                {
                    listEmps.AddRange(GetDeptEmps(dept.Id, true)); //添加子部门人员
                }
            }
            return listEmps;
        }

        /// <summary>
        /// 获取部门负责人，部门leader
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <returns></returns>
        public static OrgM_Emp GetDeptLeader(Guid deptId)
        {
            List<OrgM_DeptDuty> positions = GetDeptPositions(deptId);
            if (positions.Count > 0)
            {
                OrgM_DeptDuty position = positions.Where(x => x.IsDeptCharge).FirstOrDefault();
                if (position != null)
                    return GetPositionEmps(position.Id).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// 获取没有部门的员工
        /// </summary>
        /// <returns></returns>
        public static List<OrgM_Emp> GetNoDeptEmps()
        {
            List<Guid> empIds = GetAllEmpPositions().Where(x => x.OrgM_EmpId.HasValue && x.OrgM_EmpId.Value != Guid.Empty).Select(x => x.OrgM_EmpId.Value).ToList();
            if (empIds.Count > 0)
                return GetAllEmps(x => !empIds.Contains(x.Id));
            else
                return GetAllEmps();
        }

        #endregion

        #endregion

        #region 岗位

        #region 职务

        /// <summary>
        /// 获取所有职务信息
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<OrgM_Duty> GetAllDutys(Expression<Func<OrgM_Duty, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<OrgM_Duty, bool>> exp = x => !x.IsDeleted && !x.IsDraft && x.IsValid;
            if (expression != null) exp = ExpressionExtension.And<OrgM_Duty>(exp, expression);
            List<OrgM_Duty> dutys = CommonOperate.GetEntities<OrgM_Duty>(out errMsg, exp, null, false, new List<string>() { "Sort" }, new List<bool>() { false });
            if (dutys == null) dutys = new List<OrgM_Duty>();
            return dutys;
        }

        /// <summary>
        /// 获取职务
        /// </summary>
        /// <param name="dutyId">职务ID</param>
        /// <returns></returns>
        public static OrgM_Duty GetDuty(Guid dutyId)
        {
            List<OrgM_Duty> dutys = GetAllDutys(x => x.Id == dutyId);
            return dutys.FirstOrDefault();
        }

        /// <summary>
        /// 获取职务人员
        /// </summary>
        /// <param name="dutyId">职务ID</param>
        /// <returns></returns>
        public static List<OrgM_Emp> GetDutyEmps(Guid dutyId)
        {
            List<OrgM_Emp> list = new List<OrgM_Emp>();
            List<OrgM_DeptDuty> positions = GetAllPositions(x => x.OrgM_DutyId == dutyId);
            foreach (OrgM_DeptDuty position in positions)
            {
                list.AddRange(GetPositionEmps(position.Id));
            }
            return list;
        }

        #endregion

        #region 岗位

        /// <summary>
        /// 获取所有岗位信息
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<OrgM_DeptDuty> GetAllPositions(Expression<Func<OrgM_DeptDuty, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<OrgM_DeptDuty, bool>> exp = x => !x.IsDeleted && !x.IsDraft && x.IsValid;
            if (expression != null) exp = ExpressionExtension.And<OrgM_DeptDuty>(exp, expression);
            List<OrgM_DeptDuty> positions = CommonOperate.GetEntities<OrgM_DeptDuty>(out errMsg, exp, null, false);
            if (positions == null) positions = new List<OrgM_DeptDuty>();
            return positions;
        }

        /// <summary>
        /// 获取岗位根结点
        /// </summary>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static OrgM_DeptDuty GetPositionRoot(Guid? companyId = null)
        {
            List<OrgM_DeptDuty> list = null;
            if (companyId.HasValue && companyId.Value != Guid.Empty)
            {
                list = GetAllPositions(x => x.OrgM_DeptId == companyId);
            }
            else
            {
                list = GetAllPositions(x => x.ParentId == null || x.ParentId == Guid.Empty);
            }
            if (list.Count == 1) return list.FirstOrDefault();
            return null;
        }

        /// <summary>
        /// 获取岗位信息
        /// </summary>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public static OrgM_DeptDuty GetPosition(Guid positionId)
        {
            return GetAllPositions(x => x.Id == positionId).FirstOrDefault();
        }

        /// <summary>
        /// 获取上级岗位
        /// </summary>
        /// <param name="positionId">岗位ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static OrgM_DeptDuty GetParentPosition(Guid positionId, Guid? companyId = null)
        {
            OrgM_DeptDuty position = GetPosition(positionId);
            bool flag = position != null && position.ParentId.HasValue && position.ParentId.Value != Guid.Empty;
            if (companyId.HasValue && companyId.Value != Guid.Empty)
                flag = flag && position.OrgM_DeptId != companyId.Value;
            if (flag)
                return GetPosition(position.ParentId.Value);
            return null;
        }

        /// <summary>
        /// 获取子岗位
        /// </summary>
        /// <param name="parentId">父岗位ID</param>
        /// <param name="isDirect">是否直接子岗位，否则获取所有下级岗位</param>
        /// <returns></returns>
        public static List<OrgM_DeptDuty> GetChildPositions(Guid parentId, bool isDirect = false)
        {
            List<OrgM_DeptDuty> listPositions = GetAllPositions(x => x.ParentId != null && x.ParentId == parentId);
            if (isDirect) //取直接子岗位
            {
                return listPositions;
            }
            List<OrgM_DeptDuty> list = new List<OrgM_DeptDuty>();
            foreach (OrgM_DeptDuty position in listPositions)
            {
                list.Add(position);
                list.AddRange(GetChildPositions(position.Id, isDirect));
            }
            return list;
        }

        /// <summary>
        /// 加载岗位树
        /// </summary>
        /// <param name="positionId">岗位根结点Id，为空时加载整棵树</param>
        /// <param name="expression">岗位过滤表达式</param>
        /// <returns></returns>
        public static TreeNode LoadPositionTree(Guid? positionId, Expression<Func<OrgM_Dept, bool>> expression = null)
        {
            OrgM_DeptDuty root = !positionId.HasValue || positionId == Guid.Empty ? GetPositionRoot() : GetPosition(positionId.Value);
            if (root == null)
            {
                //加载岗位节点
                TreeNode node = null;
                List<OrgM_DeptDuty> listPositions = GetAllPositions();
                if (listPositions != null && listPositions.Count > 0)
                {
                    node = new TreeNode()
                    {
                        id = Guid.Empty.ToString(),
                        text = "根结点",
                        iconCls = "eu-icon-dept"
                    };
                    List<TreeNode> positionNodes = listPositions.Select(x => new TreeNode()
                    {
                        id = x.Id.ToString(),
                        text = x.Name,
                        iconCls = "eu-icon-dept"
                    }).ToList();
                    node.children = positionNodes;
                }
                return node;
            }
            else
            {
                List<OrgM_DeptDuty> listChilds = GetChildPositions(root.Id, false); //获取子岗位列表
                TreeNode tree = CommonOperate.GetTree<OrgM_DeptDuty>(listChilds, root, null, "ParentId", "Name", "eu-icon-dept");
                return tree;
            }
        }

        /// <summary>
        /// 加载部门岗位树
        /// </summary>
        /// <param name="deptId">部门根结点ID，为空是加载整棵树</param>
        /// <returns></returns>
        public static TreeNode LoadDeptPositionTree(Guid? deptId)
        {
            TreeNode node = null;
            OrgM_Dept root = deptId.HasValue && deptId.Value != Guid.Empty ? GetDeptById(deptId.Value) : GetDeptRoot();
            List<TreeNode> list = new List<TreeNode>();
            if (root != null)
            {
                //部门根结点
                node = new TreeNode()
                {
                    id = root.Id.ToString(),
                    text = root.Alias,
                    iconCls = "eu-icon-dept"
                };
                //加载岗位节点
                //获取部门下岗位
                List<OrgM_DeptDuty> positionList = GetDeptPositions(root.Id);
                if (positionList != null && positionList.Count > 0)
                {
                    List<TreeNode> positionNodes = positionList.Select(x => new TreeNode()
                    {
                        id = x.Id.ToString(),
                        text = x.Name,
                        iconCls = string.Empty
                    }).ToList();
                    if (positionNodes != null && positionNodes.Count > 0)
                    {
                        list.AddRange(positionNodes);
                    }
                }
                //加载部门子结点
                List<OrgM_Dept> listDepts = GetChildDepts(root.Id, true);
                foreach (OrgM_Dept dept in listDepts)
                {
                    TreeNode tempNode = LoadDeptPositionTree(dept.Id);
                    if (tempNode != null)
                    {
                        list.Add(tempNode);
                    }
                }
                node.children = list;
            }
            else
            {
                //加载岗位节点
                List<OrgM_DeptDuty> listPositions = GetAllPositions();
                if (listPositions != null && listPositions.Count > 0)
                {
                    node = new TreeNode()
                    {
                        id = Guid.Empty.ToString(),
                        text = "根结点",
                        iconCls = "eu-icon-dept"
                    };
                    List<TreeNode> positionNodes = listPositions.Select(x => new TreeNode()
                    {
                        id = x.Id.ToString(),
                        text = x.Name,
                        iconCls = string.Empty
                    }).ToList();
                    list.AddRange(positionNodes);
                    node.children = list;
                }
            }
            return node;
        }

        /// <summary>
        /// 获取岗位人员
        /// </summary>
        /// <param name="deptId">部门ID</param>
        /// <param name="dutyId">职务ID</param>
        /// <returns></returns>
        public static List<OrgM_Emp> GetPositionEmps(Guid deptId, Guid dutyId)
        {
            List<OrgM_EmpDeptDuty> empPositions = GetAllEmpPositions(x => x.OrgM_DeptId == deptId && x.OrgM_DutyId == dutyId);
            if (empPositions.Count > 0)
            {
                List<Guid> empIds = empPositions.Where(x => x.OrgM_EmpId.HasValue).Select(x => x.OrgM_EmpId.Value).ToList();
                return GetAllEmps(x => empIds.Contains(x.Id));
            }
            return new List<OrgM_Emp>();
        }

        /// <summary>
        /// 获取岗位人员
        /// </summary>
        /// <returns></returns>
        public static List<OrgM_Emp> GetPositionEmps(Guid positionId)
        {
            OrgM_DeptDuty position = GetPosition(positionId);
            if (position != null && position.OrgM_DeptId.HasValue &&
                position.OrgM_DeptId.Value != Guid.Empty && position.OrgM_DutyId.HasValue &&
                position.OrgM_DutyId.Value != Guid.Empty)
            {
                return GetPositionEmps(position.OrgM_DeptId.Value, position.OrgM_DutyId.Value);
            }
            return new List<OrgM_Emp>();
        }

        #endregion

        #endregion

        #region 员工

        #region 基本

        /// <summary>
        /// 获取所有员工，只包含在职员工
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<OrgM_Emp> GetAllEmps(Expression<Func<OrgM_Emp, bool>> expression = null)
        {
            string errMsg = string.Empty;
            int empStatus = (int)EmpStatusEnum.Work;
            Expression<Func<OrgM_Emp, bool>> exp = x => x.EmpStatus == empStatus && !x.IsDeleted && !x.IsDraft;
            if (expression != null) exp = ExpressionExtension.And<OrgM_Emp>(exp, expression);
            List<OrgM_Emp> emps = CommonOperate.GetEntities<OrgM_Emp>(out errMsg, exp, null, false, new List<string>() { "Sort" }, new List<bool>() { false });
            if (emps == null) emps = new List<OrgM_Emp>();
            return emps;
        }

        /// <summary>
        /// 获取员工信息
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <returns></returns>
        public static OrgM_Emp GetEmp(Guid empId)
        {
            List<OrgM_Emp> emps = GetAllEmps(x => x.Id == empId);
            return emps.FirstOrDefault();
        }

        /// <summary>
        /// 根据员工编号获取员工信息
        /// </summary>
        /// <param name="empCode">员工编号</param>
        /// <returns></returns>
        public static OrgM_Emp GetEmpByCode(string empCode)
        {
            List<OrgM_Emp> emps = GetAllEmps(x => x.Code == empCode);
            return emps.FirstOrDefault();
        }

        #endregion

        #region 员工岗位

        /// <summary>
        /// 获取所有员工岗位
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<OrgM_EmpDeptDuty> GetAllEmpPositions(Expression<Func<OrgM_EmpDeptDuty, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<OrgM_EmpDeptDuty, bool>> exp = x => !x.IsDeleted && !x.IsDraft && x.IsValid;
            if (expression != null) exp = ExpressionExtension.And<OrgM_EmpDeptDuty>(exp, expression);
            List<OrgM_EmpDeptDuty> empPositions = CommonOperate.GetEntities<OrgM_EmpDeptDuty>(out errMsg, exp, null, false);
            if (empPositions == null) empPositions = new List<OrgM_EmpDeptDuty>();
            return empPositions;
        }

        /// <summary>
        /// 获取员工岗位，包含兼职
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static List<OrgM_EmpDeptDuty> GetEmpPositions(Guid empId, Guid? companyId = null)
        {
            if (companyId.HasValue && companyId.Value != Guid.Empty)
            {
                List<Guid?> childDeptIds = GetChildDepts(companyId.Value).Select(x => (Guid?)x.Id).ToList();
                return GetAllEmpPositions(x => x.OrgM_EmpId == empId && childDeptIds.Contains(x.OrgM_DeptId));
            }
            return GetAllEmpPositions(x => x.OrgM_EmpId == empId);
        }

        /// <summary>
        /// 获取员工主职岗位
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static OrgM_EmpDeptDuty GetEmpMainPosition(Guid empId, Guid? companyId = null)
        {
            if (companyId.HasValue && companyId.Value != Guid.Empty)
            {
                List<Guid?> childDeptIds = GetChildDepts(companyId.Value).Select(x => (Guid?)x.Id).ToList();
                return GetAllEmpPositions(x => x.OrgM_EmpId == empId && x.IsMainDuty && childDeptIds.Contains(x.OrgM_DeptId)).FirstOrDefault();
            }
            return GetAllEmpPositions(x => x.OrgM_EmpId == empId && x.IsMainDuty).FirstOrDefault();
        }

        /// <summary>
        /// 获取员工兼职岗位集合
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static List<OrgM_EmpDeptDuty> GetEmpPartTimePosition(Guid empId, Guid? companyId = null)
        {
            if (companyId.HasValue && companyId.Value != Guid.Empty)
            {
                List<Guid?> childDeptIds = GetChildDepts(companyId.Value).Select(x => (Guid?)x.Id).ToList();
                return GetAllEmpPositions(x => x.OrgM_EmpId == empId && !x.IsMainDuty && childDeptIds.Contains(x.OrgM_DeptId));
            }
            return GetAllEmpPositions(x => x.OrgM_EmpId == empId && !x.IsMainDuty);
        }

        /// <summary>
        /// 获取员工职务，包含兼职
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static List<OrgM_Duty> GetEmpDutys(Guid empId, Guid? companyId = null)
        {
            List<OrgM_EmpDeptDuty> empPositions = GetEmpPositions(empId, companyId);
            if (empPositions.Count > 0)
            {
                List<Guid> dutyIds = empPositions.Where(x => x.OrgM_DutyId.HasValue).Select(x => x.OrgM_DutyId.Value).ToList();
                return GetAllDutys(x => dutyIds.Contains(x.Id));
            }
            return new List<OrgM_Duty>();
        }

        /// <summary>
        /// 获取员工主职职务
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static OrgM_Duty GetEmpMainDuty(Guid empId, Guid? companyId = null)
        {
            OrgM_EmpDeptDuty empPosition = GetEmpMainPosition(empId, companyId);
            if (empPosition != null && empPosition.OrgM_DutyId.HasValue && empPosition.OrgM_DutyId.Value != Guid.Empty)
                return GetDuty(empPosition.OrgM_DutyId.Value);
            return null;
        }

        /// <summary>
        /// 获取员工兼职职务集合
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static List<OrgM_Duty> GetEmpPartTimeDutys(Guid empId, Guid? companyId = null)
        {
            List<OrgM_EmpDeptDuty> empPositions = GetEmpPartTimePosition(empId, companyId);
            if (empPositions != null && empPositions.Count > 0)
            {
                List<Guid> dutyIds = empPositions.Where(x => x.OrgM_DutyId.HasValue).Select(x => x.OrgM_DutyId.Value).ToList();
                return GetAllDutys(x => dutyIds.Contains(x.Id));
            }
            return new List<OrgM_Duty>();
        }

        #endregion

        #region 员工部门

        /// <summary>
        /// 获取员工所属部门，包括兼职部门
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static List<OrgM_Dept> GetEmpDepts(Guid empId, Guid? companyId = null)
        {
            List<OrgM_Dept> depts = new List<OrgM_Dept>();
            List<OrgM_EmpDeptDuty> empPositions = GetEmpPositions(empId, companyId);
            if (empPositions.Count > 0)
            {
                List<Guid> deptIds = empPositions.Where(x => x.OrgM_DeptId.HasValue).Select(x => x.OrgM_DeptId.Value).ToList();
                depts = GetAllDepts(x => deptIds.Contains(x.Id));
            }
            return depts;
        }

        /// <summary>
        /// 获取员工主职部门
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static OrgM_Dept GetEmpMainDept(Guid empId, Guid? companyId = null)
        {
            OrgM_EmpDeptDuty empPosition = GetEmpMainPosition(empId, companyId);
            if (empPosition != null && empPosition.OrgM_DeptId.HasValue && empPosition.OrgM_DeptId.Value != Guid.Empty)
            {
                return GetDeptById(empPosition.OrgM_DeptId.Value);
            }
            return null;
        }

        /// <summary>
        /// 获取员工部门名称
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static string GetEmpMainDeptName(Guid empId, Guid? companyId = null)
        {
            OrgM_Dept dept = GetEmpMainDept(empId, companyId);
            if (dept != null)
                return dept.Alias;
            return string.Empty;
        }

        /// <summary>
        /// 获取员工兼职部门
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static List<OrgM_Dept> GetEmpPartTimeDepts(Guid empId, Guid? companyId = null)
        {
            List<OrgM_Dept> depts = new List<OrgM_Dept>();
            List<OrgM_EmpDeptDuty> empPositions = GetEmpPartTimePosition(empId, companyId);
            if (empPositions.Count > 0)
            {
                List<Guid> deptIds = empPositions.Where(x => x.OrgM_DeptId.HasValue).Select(x => x.OrgM_DeptId.Value).ToList();
                depts = GetAllDepts(x => deptIds.Contains(x.Id));
            }
            return depts;
        }

        /// <summary>
        /// 获取员工所属公司
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <returns></returns>
        public static List<OrgM_Dept> GetEmpCompanys(Guid empId)
        {
            List<Guid> deptIds = GetAllEmpPositions(x => x.OrgM_EmpId == empId && x.IsMainDuty).Where(x => x.OrgM_DeptId.HasValue && x.OrgM_DeptId.Value != Guid.Empty).Select(x => x.OrgM_DeptId.Value).ToList();
            return GetAllDepts(x => deptIds.Contains(x.Id) && x.IsCompany);
        }

        #endregion

        #region 部门员工树

        /// <summary>
        /// 部门人员树
        /// </summary
        /// <param name="deptId">部门根结点ID，为空是加载整棵树</param>
        /// <returns></returns>
        public static TreeNode LoadDeptEmpTree(Guid? deptId)
        {
            TreeNode node = null;
            OrgM_Dept deptRoot = GetDeptRoot();
            OrgM_Dept root = deptId.HasValue && deptId.Value != Guid.Empty ? GetDeptById(deptId.Value) : deptRoot;
            List<TreeNode> list = new List<TreeNode>();
            if (root != null)
            {
                //部门根结点
                node = new TreeNode()
                {
                    id = root.Id.ToString(),
                    text = root.Alias,
                    iconCls = "eu-icon-dept"
                };
                //加载人员节点
                List<OrgM_Emp> listEmps = GetDeptEmps(root.Id, true); //该部门下人员
                if (listEmps != null && listEmps.Count > 0)
                {
                    List<TreeNode> empNodes = listEmps.Select(x => new TreeNode()
                    {
                        id = x.Id.ToString(),
                        text = x.Name,
                        iconCls = "eu-icon-user"
                    }).ToList();
                    list.AddRange(empNodes);
                }
                //加载部门子结点
                List<OrgM_Dept> listDepts = GetChildDepts(root.Id, true);
                foreach (OrgM_Dept dept in listDepts)
                {
                    TreeNode tempNode = LoadDeptEmpTree(dept.Id);
                    if (tempNode != null && tempNode.children != null && tempNode.children.ToList().Count > 0)
                    {
                        list.Add(tempNode);
                    }
                }
                node.children = list;
                if (deptRoot != null && deptRoot.Id == root.Id)
                {
                    //加载没有部门的员工
                    List<OrgM_Emp> noDeptEmps = OrgMOperate.GetNoDeptEmps();
                    if (noDeptEmps.Count > 0)
                    {
                        TreeNode tempNode = new TreeNode()
                        {
                            id = Guid.Empty.ToString(),
                            text = "根结点",
                            iconCls = "eu-icon-dept"
                        };
                        List<TreeNode> tempChildren = new List<TreeNode>() { node };
                        List<TreeNode> noEmpNodes = noDeptEmps.Select(x => new TreeNode()
                        {
                            id = x.Id.ToString(),
                            text = x.Name,
                            iconCls = "eu-icon-user"
                        }).ToList();
                        tempChildren.AddRange(noEmpNodes);
                        tempNode.children = tempChildren;
                        node = tempNode;
                    }
                }
            }
            else
            {
                //加载人员节点
                List<OrgM_Emp> listEmps = GetAllEmps();
                if (listEmps != null && listEmps.Count > 0)
                {
                    node = new TreeNode()
                    {
                        id = Guid.Empty.ToString(),
                        text = "根结点",
                        iconCls = "eu-icon-dept"
                    };
                    List<TreeNode> empNodes = listEmps.Select(x => new TreeNode()
                    {
                        id = x.Id.ToString(),
                        text = x.Name,
                        iconCls = "eu-icon-user"
                    }).ToList();
                    list.AddRange(empNodes);
                    node.children = list;
                }
            }
            return node;
        }

        #endregion

        #region 员工上下级

        /// <summary>
        /// 获取员工上级人员
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="deptId">针对有兼职的员工需要传部门ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static OrgM_Emp GetEmpParentEmp(Guid empId, Guid? deptId = null, Guid? companyId = null)
        {
            List<OrgM_EmpDeptDuty> empPositions = GetEmpPositions(empId, companyId);
            if (empPositions.Count > 0)
            {
                if (empPositions.Count > 1 && deptId.HasValue && deptId.Value != Guid.Empty) //有兼职岗位
                    empPositions = empPositions.Where(x => x.OrgM_DeptId == deptId.Value).ToList();
                if (empPositions.Count > 0)
                {
                    OrgM_DeptDuty parentPosition = GetParentPosition(empPositions.FirstOrDefault().Id, companyId);
                    return GetPositionEmps(parentPosition.Id).FirstOrDefault();
                }
            }
            return null;
        }

        /// <summary>
        /// 获取员工下级人员
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="isDirect">是否直接下属，否则所有下属</param>
        /// <param name="deptId">针对有兼职的员工需要传部门ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static List<OrgM_Emp> GetEmpChildsEmps(Guid empId, bool isDirect = true, Guid? deptId = null, Guid? companyId = null)
        {
            List<OrgM_Emp> emps = new List<OrgM_Emp>();
            List<OrgM_EmpDeptDuty> empPositions = GetEmpPositions(empId, companyId);
            if (empPositions.Count > 0)
            {
                if (empPositions.Count > 1 && deptId.HasValue && deptId.Value != Guid.Empty) //有兼职岗位
                    empPositions = empPositions.Where(x => x.OrgM_DeptId == deptId.Value).ToList();
                if (empPositions.Count > 0)
                {
                    List<OrgM_DeptDuty> childPositions = GetChildPositions(empPositions.FirstOrDefault().Id, isDirect);
                    foreach (OrgM_DeptDuty position in childPositions)
                    {
                        emps.AddRange(GetPositionEmps(position.Id));
                    }
                }
            }
            return emps;
        }

        /// <summary>
        /// 获取员工的分管领导
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <param name="deptId">针对有兼职的员工需要传部门ID</param>
        /// <param name="companyId">所属公司ID</param>
        /// <returns></returns>
        public static OrgM_Emp GetEmpChargeLeader(Guid empId, Guid? deptId = null, Guid? companyId = null)
        {
            if (deptId.HasValue && deptId.Value != Guid.Empty)
            {
                return GetDeptLeader(deptId.Value);
            }
            OrgM_Dept dept = GetEmpMainDept(empId, companyId);
            return GetDeptLeader(dept.Id);
        }

        #endregion

        #region 员工用户

        /// <summary>
        /// 根据用户名获取员工对象
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public static OrgM_Emp GetEmpByUserName(string username)
        {
            OrgM_Emp emp = null;
            if (!string.IsNullOrEmpty(username))
            {
                switch (GlobalSet.EmpUserNameConfigRule)
                {
                    case UserNameAndEmpConfigRule.EmpCode:
                        emp = GetEmpByCode(username);
                        break;
                    case UserNameAndEmpConfigRule.EmailPre:
                        emp = GetAllEmps(x => x.Email.Substring(0, x.Email.IndexOf("@")) == username).FirstOrDefault();
                        break;
                    case UserNameAndEmpConfigRule.Email:
                        emp = GetAllEmps(x => x.Email == username).FirstOrDefault();
                        break;
                    case UserNameAndEmpConfigRule.Mobile:
                        emp = GetAllEmps(x => x.Mobile == username).FirstOrDefault();
                        break;
                }
            }
            return emp;
        }

        /// <summary>
        /// 根据用户ID获取员工
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public static OrgM_Emp GetEmpByUserId(Guid userId)
        {
            string username = UserOperate.GetUserNameByUserId(userId);
            OrgM_Emp emp = GetEmpByUserName(username);
            return emp;
        }

        /// <summary>
        /// 根据用户名集合获取员工集合
        /// </summary>
        /// <param name="usernames">用户名集合</param>
        /// <returns></returns>
        public static List<OrgM_Emp> GetEmpsByUserNames(List<string> usernames)
        {
            List<OrgM_Emp> emps = new List<OrgM_Emp>();
            if (usernames != null && usernames.Count > 0)
            {
                switch (GlobalSet.EmpUserNameConfigRule)
                {
                    case UserNameAndEmpConfigRule.EmpCode:
                        emps = GetAllEmps(x => usernames.Contains(x.Code));
                        break;
                    case UserNameAndEmpConfigRule.EmailPre:
                        emps = GetAllEmps(x => usernames.Contains(x.Email.Substring(0, x.Email.IndexOf("@"))));
                        break;
                    case UserNameAndEmpConfigRule.Email:
                        emps = GetAllEmps(x => usernames.Contains(x.Email));
                        break;
                    case UserNameAndEmpConfigRule.Mobile:
                        emps = OrgMOperate.GetAllEmps(x => usernames.Contains(x.Mobile));
                        break;
                }
            }
            return emps;
        }

        /// <summary>
        /// 根据员工信息获取用户名
        /// </summary>
        /// <param name="emp">员工信息</param>
        /// <returns></returns>
        public static string GetUserNameByEmp(OrgM_Emp emp)
        {
            if (emp == null) return string.Empty;
            string username = string.Empty;
            switch (GlobalSet.EmpUserNameConfigRule)
            {
                case UserNameAndEmpConfigRule.EmpCode:
                    username = emp.Code;
                    break;
                case UserNameAndEmpConfigRule.EmailPre:
                    username = emp.Email.Substring(0, emp.Email.IndexOf("@"));
                    break;
                case UserNameAndEmpConfigRule.Email:
                    username = emp.Email;
                    break;
                case UserNameAndEmpConfigRule.Mobile:
                    username = emp.Mobile;
                    break;
            }
            return username;
        }

        /// <summary>
        /// 根据员工ID获取用户名
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <returns></returns>
        public static string GetUserNameByEmpId(Guid empId)
        {
            OrgM_Emp emp = GetEmp(empId);
            if (emp == null) return string.Empty;
            return GetUserNameByEmp(emp);
        }

        /// <summary>
        /// 根据员工ID获取用户ID
        /// </summary>
        /// <param name="empId">员工ID</param>
        /// <returns></returns>
        public static Guid GetUserIdByEmpId(Guid empId)
        {
            string username = GetUserNameByEmpId(empId);
            if (!string.IsNullOrEmpty(username))
            {
                return UserOperate.GetUserIdByUserName(username);
            }
            return Guid.Empty;
        }

        #endregion

        #endregion
    }
}
