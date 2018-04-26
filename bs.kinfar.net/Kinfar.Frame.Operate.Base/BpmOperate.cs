using Kinfar.Frame.Base;
using Kinfar.Frame.Base.User;
using Kinfar.Frame.Common;
using Kinfar.Frame.Common.PubDefine;
using Kinfar.Frame.Model.Bpm;
using Kinfar.Frame.Model.EnumSpace;
using Kinfar.Frame.Model.OrgM;
using Kinfar.Frame.Model.Sys;
using Kinfar.Frame.Operate.Base.EnumDef;
using Kinfar.Frame.Operate.Base.OperateHandle;
using Kinfar.Frame.Operate.Base.TempModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Kinfar.Frame.Operate.Base
{
    /// <summary>
    /// 工作流操作类
    /// </summary>
    public static class BpmOperate
    {
        #region 流程分类

        /// <summary>
        /// 获取所流程分类
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<Bpm_FlowClass> GetAllFlowClass(Expression<Func<Bpm_FlowClass, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<Bpm_FlowClass, bool>> exp = x => !x.IsDeleted && !x.IsDraft;
            if (expression != null) exp = ExpressionExtension.And<Bpm_FlowClass>(exp, expression);
            List<Bpm_FlowClass> flowClass = CommonOperate.GetEntities<Bpm_FlowClass>(out errMsg, exp, null, false, new List<string>() { "Sort" }, new List<bool>() { false });
            if (flowClass == null) flowClass = new List<Bpm_FlowClass>();
            return flowClass;
        }

        /// <summary>
        /// 根据ID获取流程分类
        /// </summary>
        /// <returns></returns>
        public static Bpm_FlowClass GetFlowClassById(Guid classId)
        {
            return GetAllFlowClass(x => x.Id == classId).FirstOrDefault();
        }

        /// <summary>
        /// 流程分类树，包含分类下的流程
        /// </summary
        /// <param name="classId">分类根结点ID，为空是加载整棵树</param>
        /// <returns></returns>
        public static TreeNode LoadFlowClassTree(Guid? classId)
        {
            TreeNode node = CommonOperate.GetTreeNode<Bpm_FlowClass>(classId, null, null, null, null, "eu-icon-folder");
            HandleFlowTreeNode(node);
            return node;
        }

        /// <summary>
        /// 处理流程分类节点，在分类叶子节点上添加流程信息节点
        /// </summary>
        /// <param name="node"></param>
        private static void HandleFlowTreeNode(TreeNode node)
        {
            if (node == null) return;
            List<Bpm_WorkFlow> workflows = GetWorkFlowOfFlowClass(node.id.ObjToGuid());
            if (workflows.Count > 0)
            {
                List<TreeNode> childs = workflows.Select(x => new TreeNode() { id = x.Id.ToString(), iconCls = "eu-icon-cog", text = string.IsNullOrEmpty(x.DisplayName) ? x.Name : x.DisplayName, attribute = new TreeAttributes() { obj = new { workflowId = x.Id } } }).ToList();
                List<TreeNode> list = new List<TreeNode>();
                if (node.children != null)
                {
                    list.AddRange(node.children.Cast<TreeNode>());
                }
                list.AddRange(childs);
                node.children = list;
            }
            else
            {
                if (node.children != null)
                {
                    foreach (TreeNode child in node.children)
                    {
                        HandleFlowTreeNode(child);
                    }
                }
            }
        }

        #endregion

        #region 流程信息

        /// <summary>
        /// 获取所有流程
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<Bpm_WorkFlow> GetAllWorkflows(Expression<Func<Bpm_WorkFlow, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<Bpm_WorkFlow, bool>> exp = x => !x.IsDeleted && !x.IsDraft;
            if (expression != null) exp = ExpressionExtension.And<Bpm_WorkFlow>(exp, expression);
            List<Bpm_WorkFlow> workflows = CommonOperate.GetEntities<Bpm_WorkFlow>(out errMsg, exp, null, false);
            if (workflows == null) workflows = new List<Bpm_WorkFlow>();
            return workflows;
        }

        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="id">流程ID</param>
        /// <returns></returns>
        public static Bpm_WorkFlow GetWorkflow(Guid id)
        {
            return GetAllWorkflows(x => x.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// 获取模块流程
        /// </summary>
        /// <param name="moduleId">模块id</param>
        /// <returns></returns>
        public static Bpm_WorkFlow GetModuleWorkFlow(Guid moduleId)
        {
            return GetAllWorkflows(x => x.Sys_ModuleId == moduleId).FirstOrDefault();
        }

        /// <summary>
        /// 获取流程分类下的流程
        /// </summary>
        /// <param name="classId">分类ID</param>
        /// <returns></returns>
        public static List<Bpm_WorkFlow> GetWorkFlowOfFlowClass(Guid classId)
        {
            return GetAllWorkflows(x => x.Bpm_FlowClassId == classId);
        }

        /// <summary>
        /// 获取流程的关联模块ID
        /// </summary>
        /// <param name="workflowId">流程ID</param>
        /// <returns></returns>
        public static Guid GetWorkflowModuleId(Guid workflowId)
        {
            Bpm_WorkFlow workflow = GetWorkflow(workflowId);
            if (workflow != null && workflow.Sys_ModuleId.HasValue && workflow.Sys_ModuleId.Value != Guid.Empty)
                return workflow.Sys_ModuleId.Value;
            return Guid.Empty;
        }

        /// <summary>
        /// 判断是不是子流程
        /// </summary>
        /// <param name="workflowId">流程ID</param>
        /// <returns></returns>
        public static bool IsSubWorkFlow(Guid workflowId)
        {
            return GetAllWorkNodes(x => x.Bpm_WorkFlowSubId == workflowId).Count > 0;
        }

        /// <summary>
        /// 获取记录的流程实例
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <param name="recordId">记录ID</param>
        /// <returns></returns>
        public static Bpm_WorkFlowInstance GetWorkflowInstance(Guid moduleId, Guid recordId)
        {
            Bpm_WorkFlow workflow = GetModuleWorkFlow(moduleId);
            if (workflow != null)
            {
                string errMsg = string.Empty;
                Bpm_WorkFlowInstance workflowInst = CommonOperate.GetEntity<Bpm_WorkFlowInstance>(x => x.Bpm_WorkFlowId == workflow.Id && x.RecordId == recordId, null, out errMsg);
                return workflowInst;
            }
            return null;
        }

        /// <summary>
        /// 获取流程实例
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public static Bpm_WorkFlowInstance GetWorkflowInstanceById(Guid id)
        {
            string errMsg = string.Empty;
            Bpm_WorkFlowInstance workflowInst = CommonOperate.GetEntityById<Bpm_WorkFlowInstance>(id, out errMsg);
            return workflowInst;
        }

        #endregion

        #region 流程结点

        /// <summary>
        /// 获取所有流程结点
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<Bpm_WorkNode> GetAllWorkNodes(Expression<Func<Bpm_WorkNode, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<Bpm_WorkNode, bool>> exp = x => !x.IsDeleted && !x.IsDraft;
            if (expression != null) exp = ExpressionExtension.And<Bpm_WorkNode>(exp, expression);
            List<Bpm_WorkNode> workNodes = CommonOperate.GetEntities<Bpm_WorkNode>(out errMsg, exp, null, false);
            if (workNodes == null) workNodes = new List<Bpm_WorkNode>();
            return workNodes;
        }

        /// <summary>
        /// 获取流程结点
        /// </summary>
        /// <param name="workNodeId">流程结点ID</param>
        /// <returns></returns>
        public static Bpm_WorkNode GetWorkNode(Guid workNodeId)
        {
            return GetAllWorkNodes(x => x.Id == workNodeId).FirstOrDefault();
        }

        /// <summary>
        /// 获取流程的所有流程结点
        /// </summary>
        /// <param name="workflowId">流程ID</param>
        /// <returns></returns>
        public static List<Bpm_WorkNode> GetWorkNodesOfFlow(Guid workflowId)
        {
            return GetAllWorkNodes(x => x.Bpm_WorkFlowId == workflowId);
        }

        /// <summary>
        /// 获取发起节点
        /// </summary>
        /// <param name="workflowId">流程id</param>
        /// <returns></returns>
        public static Bpm_WorkNode GetLaunchNode(Guid workflowId)
        {
            string errMsg = string.Empty;
            int workNodeType = (int)WorkNodeTypeEnum.Start;
            Bpm_WorkNode startNode = GetAllWorkNodes(x => x.Bpm_WorkFlowId == workflowId && x.WorkNodeType == workNodeType).FirstOrDefault();
            if (startNode == null) return null;
            Bpm_WorkLine startLine = GetAllWorkLines(x => x.Bpm_WorkNodeStartId == startNode.Id).FirstOrDefault();
            if (startLine == null || !startLine.Bpm_WorkNodeEndId.HasValue) return null;
            Bpm_WorkNode launchNode = GetWorkNode(startLine.Bpm_WorkNodeEndId.Value);
            return launchNode;
        }

        /// <summary>
        /// 获取结束结点
        /// </summary>
        /// <param name="workflowId">流程ID</param>
        /// <returns></returns>
        public static Bpm_WorkNode GetEndNode(Guid workflowId)
        {
            string errMsg = string.Empty;
            int workNodeType = (int)WorkNodeTypeEnum.End;
            Bpm_WorkNode endNode = GetAllWorkNodes(x => x.Bpm_WorkFlowId == workflowId && x.WorkNodeType == workNodeType).FirstOrDefault();
            return endNode;
        }

        /// <summary>
        /// 根据模块ID获取发起结点
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns></returns>
        public static Bpm_WorkNode GetLaunchNodeByModuleId(Guid moduleId)
        {
            Bpm_WorkFlow workflow = GetModuleWorkFlow(moduleId);
            if (workflow != null)
            {
                return GetLaunchNode(workflow.Id);
            }
            return null;
        }

        /// <summary>
        /// 获取当前结点的前一结点
        /// </summary>
        /// <param name="workflowId">流程ID</param>
        /// <param name="workNodeId">当前结点ID</param>
        /// <returns></returns>
        public static Bpm_WorkNode GetPrexNode(Guid workflowId, Guid workNodeId)
        {
            Bpm_WorkLine workLine = GetAllWorkLines(x => x.Bpm_WorkFlowId == workflowId && x.Bpm_WorkNodeEndId == workNodeId).FirstOrDefault();
            if (workLine != null && workLine.Bpm_WorkNodeStartId.HasValue && workLine.Bpm_WorkNodeStartId.Value != Guid.Empty)
                return GetWorkNode(workLine.Bpm_WorkNodeStartId.Value);
            return null;
        }

        /// <summary>
        /// 获取当前结点的所有前面结点集合
        /// </summary>
        /// <param name="workflowId">流程ID</param>
        /// <param name="workNodeId">当前结点ID</param>
        /// <returns></returns>
        public static List<Bpm_WorkNode> GetPrexNodes(Guid workflowId, Guid workNodeId)
        {
            List<Bpm_WorkNode> preNodes = new List<Bpm_WorkNode>();
            Bpm_WorkNode preNode = GetPrexNode(workflowId, workNodeId);
            if (preNode != null)
            {
                preNodes.Add(preNode);
                preNodes.AddRange(GetPrexNodes(workNodeId, preNode.Id));
            }
            return preNodes;
        }

        /// <summary>
        /// 获取当前节点的下一节点集合
        /// </summary>
        /// <param name="workflowId">流程id</param>
        /// <param name="currNodeId">起始节点id</param>
        /// <returns></returns>
        public static List<Bpm_WorkNode> GetNextWorkNodes(Guid workflowId, Guid currNodeId)
        {
            string errMsg = string.Empty;
            List<Bpm_WorkLine> workLines = GetAllWorkLines(x => x.Bpm_WorkFlowId == workflowId && x.Bpm_WorkNodeStartId == currNodeId);
            if (workLines != null && workLines.Count > 0)
            {
                List<Guid> workNodeIds = workLines.Where(x => x.Bpm_WorkNodeEndId.HasValue).Select(x => x.Bpm_WorkNodeEndId.Value).ToList();
                List<Bpm_WorkNode> workNodes = GetAllWorkNodes(x => workNodeIds.Contains(x.Id));
                if (workNodes == null) workNodes = new List<Bpm_WorkNode>();
                workNodes = workNodes.Where(x => x.WorkNodeType != (int)WorkNodeTypeEnum.End).ToList();
                return workNodes;
            }
            return new List<Bpm_WorkNode>();
        }

        /// <summary>
        /// 获取下一审批节点
        /// </summary>
        /// <param name="nextNodes">下一结点集合</param>
        /// <param name="workflow">流程</param>
        /// <param name="currNode">当前结点</param>
        /// <param name="workFlowInst">流程实例</param>
        /// <param name="currNodeInst">当前实例结点</param>
        /// <returns></returns>
        public static Bpm_WorkNode GetNextActionNode(List<Bpm_WorkNode> nextNodes, Bpm_WorkFlow workflow, Bpm_WorkNode currNode, Bpm_WorkFlowInstance workFlowInst, Bpm_WorkNodeInstance currNodeInst)
        {
            string errMsg = string.Empty;
            Guid moduleId = workflow.Sys_ModuleId.Value;
            foreach (Bpm_WorkNode nextNode in nextNodes)
            {
                Bpm_WorkLine workLine = GetAllWorkLines(x => x.Bpm_WorkFlowId == workflow.Id && x.Bpm_WorkNodeStartId == currNode.Id && x.Bpm_WorkNodeEndId == nextNode.Id && !x.IsDeleted).FirstOrDefault();
                if (workLine == null) continue;
                if (!workLine.IsCustomerCondition)
                {
                    bool formConn = true;
                    bool dutyConn = true;
                    bool deptConn = true;
                    bool sqlConn = true;
                    if (!string.IsNullOrEmpty(workLine.FormCondition))
                    {
                        try
                        {
                            List<ConditionItem> items = JsonHelper.Deserialize<List<ConditionItem>>(workLine.FormCondition);
                            object queryLamda = CommonOperate.GetQueryCondition(moduleId, items);
                            object model = CommonOperate.GetEntity(moduleId, queryLamda, string.Format("Id='{0}'", workFlowInst.RecordId), out errMsg);
                            formConn = model != null;
                        }
                        catch { }
                    }
                    if (!string.IsNullOrEmpty(workLine.DutyCondition))
                    {
                    }
                    if (!string.IsNullOrEmpty(workLine.DeptCondition))
                    {
                    }
                    if (!string.IsNullOrEmpty(workLine.SqlCondition))
                    {
                    }
                    if (formConn && dutyConn && deptConn && sqlConn)
                    {
                        return nextNode;
                    }
                }
                else //自定义条件
                {
                    object rsObj = CommonOperate.ExecuteCustomeOperateHandleMethod(moduleId, "IsFlowTurn", new object[] { workFlowInst.RecordId, currNode.Name, nextNode.Name });
                    if (rsObj.ObjToBool())
                        return nextNode;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据tagId获取流程结点
        /// </summary>
        /// <param name="workflowId">流程ID</param>
        /// <param name="tagId">tagid</param>
        /// <returns></returns>
        public static Bpm_WorkNode GetWorkNodeByTagId(Guid workflowId, string tagId)
        {
            return GetAllWorkNodes(x => x.Bpm_WorkFlowId == workflowId && x.TagId == tagId).FirstOrDefault();
        }

        /// <summary>
        /// 获取下一结点的处理者
        /// </summary>
        /// <param name="nextNode">下一结点</param>
        /// <param name="currUser">当前结点处理人</param>
        /// <param name="workFlowInst">流程实例</param>
        /// <returns>返回员工ID集合</returns>
        public static List<Guid> GetNextNodeHandler(Bpm_WorkNode nextNode, UserInfo currUser, Bpm_WorkFlowInstance workFlowInst)
        {
            if (nextNode == null || currUser == null || workFlowInst == null)
                return new List<Guid>();
            if (!nextNode.Bpm_WorkFlowId.HasValue || nextNode.WorkNodeTypeOfEnum == WorkNodeTypeEnum.End)
                return new List<Guid>();
            List<Guid> empIds = new List<Guid>();
            Guid moduleId = GetWorkflowModuleId(nextNode.Bpm_WorkFlowId.Value);
            //调用自定义查找结点处理人方法
            object rsObj = CommonOperate.ExecuteCustomeOperateHandleMethod(moduleId, "GetNodeHandler", new object[] { workFlowInst.RecordId, nextNode.Name, workFlowInst.Id, currUser });
            if (rsObj != null)
            {
                try
                {
                    empIds = rsObj as List<Guid>;
                }
                catch { }
            }
            if (empIds.Count > 0) return empIds;
            //调用通用查找结点处理人
            List<Guid> filter = new List<Guid>(); //过滤范围
            if (!string.IsNullOrEmpty(nextNode.HandleRange))
            {
                string[] token = nextNode.HandleRange.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (token.Length > 0)
                    filter = token.Select(x => x.ObjToGuid()).Where(x => x != Guid.Empty).ToList();
            }
            switch (nextNode.HandlerTypeOfEnum) //处理者类型
            {
                case NodeHandlerTypeEnum.All:
                    {
                        empIds = OrgMOperate.GetAllEmps(x => x.Id != currUser.EmpId.Value).Select(x => x.Id).ToList();
                        if (filter.Count > 0) empIds = empIds.Where(x => filter.Contains(x)).ToList();
                    }
                    break;
                case NodeHandlerTypeEnum.Dept:
                    foreach (Guid deptId in filter)
                    {
                        empIds.AddRange(OrgMOperate.GetDeptEmps(deptId, true).Select(x => x.Id));
                    }
                    break;
                case NodeHandlerTypeEnum.Duty:
                    foreach (Guid dutyId in filter)
                    {
                        empIds.AddRange(OrgMOperate.GetDutyEmps(dutyId).Select(x => x.Id));
                    }
                    break;
                case NodeHandlerTypeEnum.Employee:
                    empIds = filter;
                    break;
                case NodeHandlerTypeEnum.Position:
                    foreach (Guid positionId in filter)
                    {
                        empIds.AddRange(OrgMOperate.GetPositionEmps(positionId).Select(x => x.Id));
                    }
                    break;
                case NodeHandlerTypeEnum.Role:
                    foreach (Guid roleId in filter)
                    {
                        List<string> usernames = PermissionOperate.GetRoleUsers(roleId).Select(x => x.UserName).ToList();
                        empIds.AddRange(OrgMOperate.GetEmpsByUserNames(usernames).Select(x => x.Id));
                    }
                    break;
                case NodeHandlerTypeEnum.FormFieldValue:
                    {
                        Guid tempEmpId = nextNode.FormFieldName.ObjToGuid();
                        if (tempEmpId != Guid.Empty)
                            empIds.Add(tempEmpId);
                    }
                    break;
                case NodeHandlerTypeEnum.LastHandlerDirectLeader:
                    {
                        var tempEmp = OrgMOperate.GetEmpParentEmp(currUser.EmpId.Value);
                        if (tempEmp != null) empIds.Add(tempEmp.Id);
                    }
                    break;
                case NodeHandlerTypeEnum.LastHandlerChargeLeader:
                    {
                        var tempEmp = OrgMOperate.GetEmpChargeLeader(currUser.EmpId.Value);
                        if (tempEmp != null) empIds.Add(tempEmp.Id);
                    }
                    break;
                case NodeHandlerTypeEnum.StarterDirectLeader:
                    {
                        if (workFlowInst.OrgM_EmpId.HasValue)
                        {
                            var tempEmp = OrgMOperate.GetEmpParentEmp(workFlowInst.OrgM_EmpId.Value);
                            if (tempEmp != null) empIds.Add(tempEmp.Id);
                        }
                    }
                    break;
                case NodeHandlerTypeEnum.StarterChargeLeader:
                    {
                        if (workFlowInst.OrgM_EmpId.HasValue)
                        {
                            var tempEmp = OrgMOperate.GetEmpChargeLeader(workFlowInst.OrgM_EmpId.Value);
                            if (tempEmp != null) empIds.Add(tempEmp.Id);
                        }
                    }
                    break;
            }
            return empIds;
        }

        /// <summary>
        /// 获取发起结点处理者
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns></returns>
        public static List<Guid> GetLaunchNodeHandler(Guid moduleId)
        {
            List<Guid> empIds = new List<Guid>();
            Bpm_WorkNode launchNode = GetLaunchNodeByModuleId(moduleId);
            if (launchNode == null) return empIds;
            List<Guid> filter = new List<Guid>(); //过滤范围
            if (!string.IsNullOrEmpty(launchNode.HandleRange))
            {
                string[] token = launchNode.HandleRange.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (token.Length > 0)
                    filter = token.Select(x => x.ObjToGuid()).Where(x => x != Guid.Empty).ToList();
            }
            switch (launchNode.HandlerTypeOfEnum) //处理者类型
            {
                case NodeHandlerTypeEnum.All:
                    {
                        empIds = OrgMOperate.GetAllEmps().Select(x => x.Id).ToList();
                        if (filter.Count > 0) empIds = empIds.Where(x => filter.Contains(x)).ToList();
                    }
                    break;
                case NodeHandlerTypeEnum.Dept:
                    foreach (Guid deptId in filter)
                    {
                        empIds.AddRange(OrgMOperate.GetDeptEmps(deptId, true).Select(x => x.Id));
                    }
                    break;
                case NodeHandlerTypeEnum.Duty:
                    foreach (Guid dutyId in filter)
                    {
                        empIds.AddRange(OrgMOperate.GetDutyEmps(dutyId).Select(x => x.Id));
                    }
                    break;
                case NodeHandlerTypeEnum.Employee:
                    empIds = filter;
                    break;
                case NodeHandlerTypeEnum.Position:
                    foreach (Guid positionId in filter)
                    {
                        empIds.AddRange(OrgMOperate.GetPositionEmps(positionId).Select(x => x.Id));
                    }
                    break;
                case NodeHandlerTypeEnum.Role:
                    foreach (Guid roleId in filter)
                    {
                        List<string> usernames = PermissionOperate.GetRoleUsers(roleId).Select(x => x.UserName).ToList();
                        empIds.AddRange(OrgMOperate.GetEmpsByUserNames(usernames).Select(x => x.Id));
                    }
                    break;
            }
            return empIds;
        }

        /// <summary>
        /// 获取子流程的父流程当前结点
        /// </summary>
        /// <param name="subWorkflowId">子流程结点</param>
        /// <returns></returns>
        public static Bpm_WorkNode GetParentFlowNode(Guid subWorkflowId)
        {
            return GetAllWorkNodes(x => x.Bpm_WorkFlowSubId == subWorkflowId).FirstOrDefault();
        }

        /// <summary>
        /// 获取回退结点集合
        /// </summary>
        /// <param name="toDoTaskId">待办ID</param>
        /// <returns></returns>
        public static List<Bpm_WorkNode> GetBackNodes(Guid toDoTaskId)
        {
            Guid workNodeId = BpmOperate.GetWorkNodeIdByTodoId(toDoTaskId);
            Bpm_WorkNode workNode = BpmOperate.GetWorkNode(workNodeId);
            List<Bpm_WorkNode> bakcNodes = new List<Bpm_WorkNode>();
            if (workNode != null && workNode.Bpm_WorkFlowId.HasValue && workNode.Bpm_WorkFlowId.Value != Guid.Empty)
            {
                if (workNode.BackTypeOfEnum == NodeBackTypeEnum.BackToLast) //只能回退到上一结点
                {
                    Bpm_WorkNode preNode = BpmOperate.GetPrexNode(workNode.Bpm_WorkFlowId.Value, workNodeId);
                    if (preNode != null)
                        bakcNodes = new List<Bpm_WorkNode>() { preNode };
                }
                else if (workNode.BackTypeOfEnum == NodeBackTypeEnum.BackToFirst) //只能回退到发起结点
                {
                    Bpm_WorkNode launchNode = BpmOperate.GetLaunchNode(workNode.Bpm_WorkFlowId.Value);
                    if (launchNode != null)
                        bakcNodes = new List<Bpm_WorkNode>() { launchNode };
                }
                else if (workNode.BackTypeOfEnum == NodeBackTypeEnum.BackToAll) //允许回退到任意结点，并且要排除并行节点
                {
                    bakcNodes = BpmOperate.GetPrexNodes(workNode.Bpm_WorkFlowId.Value, workNodeId).Where(x => x.HandleStrategyOfEnum != HandleStrategyTypeEnum.AllAgree).ToList();
                }
            }
            return bakcNodes;
        }

        #endregion

        #region 流程连线

        /// <summary>
        /// 获取所有流程连线
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<Bpm_WorkLine> GetAllWorkLines(Expression<Func<Bpm_WorkLine, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<Bpm_WorkLine, bool>> exp = x => !x.IsDeleted && !x.IsDraft;
            if (expression != null) exp = ExpressionExtension.And<Bpm_WorkLine>(exp, expression);
            List<Bpm_WorkLine> workLines = CommonOperate.GetEntities<Bpm_WorkLine>(out errMsg, exp, null, false);
            if (workLines == null) workLines = new List<Bpm_WorkLine>();
            return workLines;
        }

        /// <summary>
        /// 获取流程中所有流程连线
        /// </summary>
        /// <param name="workflowId">流程ID</param>
        /// <returns></returns>
        public static List<Bpm_WorkLine> GetWorkLinesOfFlow(Guid workflowId)
        {
            return GetAllWorkLines(x => x.Bpm_WorkFlowId == workflowId);
        }

        /// <summary>
        /// 根据tagId获取流程连线
        /// </summary>
        /// <param name="workflowId">流程ID</param>
        /// <param name="tagId">tagid</param>
        /// <returns></returns>
        public static Bpm_WorkLine GetWorkLineByTagId(Guid workflowId, string tagId)
        {
            return GetAllWorkLines(x => x.Bpm_WorkFlowId == workflowId && x.TagId == tagId).FirstOrDefault();
        }

        #endregion

        #region 流程按钮

        /// <summary>
        /// 获取所有流程按钮
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<Bpm_FlowBtn> GetAllWorkButtons(Expression<Func<Bpm_FlowBtn, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<Bpm_FlowBtn, bool>> exp = x => !x.IsDeleted && !x.IsDraft;
            if (expression != null) exp = ExpressionExtension.And<Bpm_FlowBtn>(exp, expression);
            List<Bpm_FlowBtn> workBtns = CommonOperate.GetEntities<Bpm_FlowBtn>(out errMsg, exp, null, false, new List<string>() { "Sort" }, new List<bool>() { false });
            if (workBtns == null) workBtns = new List<Bpm_FlowBtn>();
            return workBtns;
        }

        /// <summary>
        /// 获取所有审批按钮配置
        /// </summary>
        /// <param name="expression">条件表达式</param>
        /// <returns></returns>
        public static List<Bpm_NodeBtnConfig> GetAllApprovalBtnConfigs(Expression<Func<Bpm_NodeBtnConfig, bool>> expression = null)
        {
            string errMsg = string.Empty;
            Expression<Func<Bpm_NodeBtnConfig, bool>> exp = x => !x.IsDeleted && !x.IsDraft;
            if (expression != null) exp = ExpressionExtension.And<Bpm_NodeBtnConfig>(exp, expression);
            List<Bpm_NodeBtnConfig> btnConfigs = CommonOperate.GetEntities<Bpm_NodeBtnConfig>(out errMsg, exp, null, false);
            if (btnConfigs == null) return new List<Bpm_NodeBtnConfig>();
            Dictionary<int, Bpm_NodeBtnConfig> dic = new Dictionary<int, Bpm_NodeBtnConfig>();
            foreach (Bpm_NodeBtnConfig btnConfig in btnConfigs)
            {
                if (btnConfig.Bpm_FlowBtnId.HasValue && btnConfig.Bpm_FlowBtnId.Value != Guid.Empty)
                {
                    Bpm_FlowBtn btn = GetAllWorkButtons(x => x.Id == btnConfig.Bpm_FlowBtnId.Value).FirstOrDefault();
                    if (btn != null)
                    {
                        int sort = btn.Sort;
                        if (dic.ContainsKey(sort))
                            sort = dic.Keys.Max() + 1;
                        dic.Add(sort, btnConfig);
                    }
                }
            }
            return dic.OrderBy(x => x.Key).Select(x => x.Value).ToList();
        }

        /// <summary>
        /// 获取流程表单按钮集合
        /// </summary>
        /// <param name="workflowId">流程ID</param>
        /// <param name="workNodeId">流程结点ID</param>
        /// <returns></returns>
        public static List<FormButton> GetWorkNodeFormBtns(Guid workflowId, Guid workNodeId)
        {
            List<FormButton> btns = new List<FormButton>();
            Bpm_WorkNode workNode = GetWorkNode(workNodeId);
            if (workNode == null) return new List<FormButton>();
            List<Bpm_NodeBtnConfig> nodeConfigBtns = GetAllApprovalBtnConfigs(x => x.Bpm_WorkFlowId == workflowId && x.Bpm_WorkNodeId == workNodeId && x.IsEnabled && !x.IsDeleted);
            foreach (Bpm_NodeBtnConfig btnConfig in nodeConfigBtns)
            {
                Bpm_FlowBtn flowBtn = GetAllWorkButtons(x => x.Id == btnConfig.Bpm_FlowBtnId).FirstOrDefault();
                if (flowBtn == null) continue;
                string clickMethod = flowBtn.ClickMethod;
                if (string.IsNullOrEmpty(clickMethod))
                {
                    if (flowBtn.ButtonTypeOfEnum == FlowButtonTypeEnum.BackBtn)
                        clickMethod = "BackFlow(this)";
                    else if (flowBtn.ButtonTypeOfEnum == FlowButtonTypeEnum.AssignBtn)
                        clickMethod = "DirectFlow(this)";
                    else
                        clickMethod = "ApprovalFlow(this)";
                }
                btns.Add(new FormButton() { TagId = string.Format("flowBtn_{0}", flowBtn.Id), IconType = ButtonIconType.Save, Icon = flowBtn.ButtonIcon, DisplayText = btnConfig.BtnDisplay, ClickMethod = clickMethod });
            }
            return btns;
        }

        /// <summary>
        /// 获取发起结点流程操作按钮集合
        /// </summary>
        /// <param name="todoTaskId">待办ID，针对回退到发起结点时的待办ID</param>
        /// <returns></returns>
        public static List<FormButton> GetLaunchNodeFlowBtns(Guid? todoTaskId = null)
        {
            List<FormButton> btns = new List<FormButton>();
            btns.Add(new FormButton() { TagId = string.Format("flowBtn_{0}", todoTaskId.HasValue ? todoTaskId.Value : Guid.Empty), DisplayText = "保存并提交", IconType = ButtonIconType.Save, ClickMethod = "SubmitFlow(this)", Icon = "eu-icon-tosubmit" });
            return btns;
        }

        /// <summary>
        /// 根据待办任务ID获取审批表单流程操作按钮集合
        /// </summary>
        /// <param name="todoTaskId">待办任务ID</param>
        /// <returns></returns>
        public static List<FormButton> GetNodeFlowBtns(Guid todoTaskId)
        {
            string errMsg = string.Empty;
            Bpm_WorkToDoList todo = CommonOperate.GetEntityById<Bpm_WorkToDoList>(todoTaskId, out errMsg);
            if (todo != null && todo.Bpm_WorkNodeInstanceId.HasValue && todo.Bpm_WorkFlowInstanceId.HasValue)
            {
                Bpm_WorkFlowInstance workflowInst = CommonOperate.GetEntityById<Bpm_WorkFlowInstance>(todo.Bpm_WorkFlowInstanceId.Value, out errMsg);
                if (workflowInst != null && workflowInst.Bpm_WorkFlowId.HasValue && workflowInst.Bpm_WorkFlowId.Value != Guid.Empty)
                {
                    Bpm_WorkNodeInstance workNodeInst = CommonOperate.GetEntityById<Bpm_WorkNodeInstance>(todo.Bpm_WorkNodeInstanceId.Value, out errMsg);
                    if (workNodeInst != null && workNodeInst.Bpm_WorkNodeId.HasValue && workNodeInst.Bpm_WorkNodeId.Value != Guid.Empty)
                    {
                        Bpm_WorkNode launchNode = BpmOperate.GetLaunchNode(workflowInst.Bpm_WorkFlowId.Value);
                        if (launchNode != null && launchNode.Id == workNodeInst.Bpm_WorkNodeId.Value)
                            return GetLaunchNodeFlowBtns(todoTaskId);
                        return GetWorkNodeFormBtns(workflowInst.Bpm_WorkFlowId.Value, workNodeInst.Bpm_WorkNodeId.Value);
                    }
                }
            }
            return new List<FormButton>();
        }

        #endregion

        #region 流程操作

        /// <summary>
        /// 发起流程
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <param name="recordId">记录ID</param>
        /// <param name="currUser">发起人</param>
        /// <param name="parentFlowInstId">父流程实例ID</param>
        /// <returns></returns>
        public static string StartProcess(Guid moduleId, Guid recordId, UserInfo currUser, Guid? parentFlowInstId = null)
        {
            string errMsg = string.Empty;
            string errPrex = GetFlowErrPrex(WorkActionEnum.Starting);
            Bpm_WorkFlow workFlow = GetModuleWorkFlow(moduleId);
            if (workFlow == null)
                return string.Format("{0}模块流程配置获取失败", errPrex);
            Bpm_WorkNode launchNode = GetLaunchNode(workFlow.Id);
            if (launchNode == null)
                return string.Format("{0}发起结点信息获取失败", errPrex);
            if (currUser == null)
                return string.Format("{0}当前用户信息获取失败", errPrex);
            object model = CommonOperate.GetEntityById(moduleId, recordId, out errMsg);
            if (model == null)
                return string.Format("{0}记录信息不存在，{1}", errPrex, errMsg);
            if (BpmOperate.IsLaunchFlowCompleted(moduleId, recordId))
                return string.Format("{0}流程已发起，请不要重复发起", errPrex);
            if (!BpmOperate.IsAllowLaunchFlow(moduleId, currUser, null))
                return string.Format("{0}您没有该流程的发起权限，如有疑问请联系管理员", errPrex);
            string userAliasName = string.IsNullOrEmpty(currUser.AliasName) ? currUser.UserName : currUser.AliasName;
            string subStr = parentFlowInstId.HasValue && parentFlowInstId.Value != Guid.Empty ? "_SUB" : string.Empty;
            Bpm_WorkFlowInstance workFlowInst = new Bpm_WorkFlowInstance()
            {
                Code = string.Format("WFI{0}{1}", subStr, DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                Bpm_WorkFlowId = workFlow.Id,
                Status = (int)WorkFlowStatusEnum.Start,
                StartDate = DateTime.Now,
                OrgM_EmpId = currUser.EmpId,
                OrgM_EmpName = currUser.EmpName,
                RecordId = recordId,
                ParentId = parentFlowInstId,
                CreateUserId = currUser.UserId,
                CreateUserName = userAliasName,
                ModifyUserId = currUser.UserId,
                ModifyUserName = userAliasName,
            };
            Bpm_WorkNodeInstance launchNodeInst = new Bpm_WorkNodeInstance()
            {
                SerialNo = string.Format("WNI{0}{1}", subStr, DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                StartDate = DateTime.Now,
                FinishDate = DateTime.Now,
                StatusOfEnum = WorkNodeStatusEnum.Do,
                CreateUserId = currUser.UserId,
                CreateUserName = userAliasName,
                ModifyUserId = currUser.UserId,
                ModifyUserName = userAliasName,
            };
            WorkActionEnum workAction = parentFlowInstId.HasValue && parentFlowInstId.Value != Guid.Empty ? WorkActionEnum.SubStarting : WorkActionEnum.Starting;
            string opinions = workAction == WorkActionEnum.SubStarting ? "发起子流程" : "发起流程";
            errMsg = HandleProcess(workFlow, workFlowInst, launchNode, launchNodeInst, currUser, workAction, opinions, null);
            return errMsg;
        }

        /// <summary>
        /// 审批流程
        /// </summary>
        /// <param name="workTodoId">待办任务ID</param>
        /// <param name="approvalOpinions">审批意见</param>
        /// <param name="currUser">当前用户</param>
        /// <param name="workAction">审批动作</param>
        /// <param name="returnNodeId">针对回退时回退节点ID</param>
        /// <param name="directHandler">针对指派时被指派人ID</param>
        /// <returns></returns>
        public static string ApproveProcess(Guid workTodoId, string approvalOpinions, UserInfo currUser, WorkActionEnum workAction, Guid? returnNodeId = null, Guid? directHandler = null)
        {
            WorkFlowStatusEnum flowStatus = WorkFlowStatusEnum.NoStatus;
            string errPrex = string.Empty;
            GetFlowStatusAndErrPrex(workAction, out flowStatus, out errPrex);
            if (workAction == WorkActionEnum.NoAction || workAction == WorkActionEnum.Starting)
                return string.Format("{0}没有可用的操作", errPrex);
            string errMsg = string.Empty;
            Bpm_WorkToDoList workTodo = CommonOperate.GetEntityById<Bpm_WorkToDoList>(workTodoId, out errMsg);
            if (workTodo == null || !workTodo.Bpm_WorkFlowInstanceId.HasValue || !workTodo.Bpm_WorkNodeInstanceId.HasValue)
                return string.Format("{0}当前待办任务数据丢失", errPrex);
            Bpm_WorkFlowInstance workFlowInst = CommonOperate.GetEntityById<Bpm_WorkFlowInstance>(workTodo.Bpm_WorkFlowInstanceId.Value, out errMsg);
            if (workFlowInst == null || !workFlowInst.Bpm_WorkFlowId.HasValue)
                return string.Format("{0}当前待办任务对应的流程实例数据丢失", errPrex);
            Bpm_WorkNodeInstance workNodeInst = CommonOperate.GetEntityById<Bpm_WorkNodeInstance>(workTodo.Bpm_WorkNodeInstanceId.Value, out errMsg);
            if (workNodeInst == null || !workNodeInst.Bpm_WorkNodeId.HasValue)
                return string.Format("{0}当前待办任务对应的流程结点实例数据丢失", errPrex);
            Bpm_WorkFlow workFlow = GetWorkflow(workFlowInst.Bpm_WorkFlowId.Value);
            if (workFlow == null)
                return string.Format("{0}当前待办任务对应的流程结点数据丢失", errPrex);
            Bpm_WorkNode workNode = GetWorkNode(workNodeInst.Bpm_WorkNodeId.Value);
            if (workNode == null)
                return string.Format("{0}当前待办任务对应的流程结点数据丢失", errPrex);
            if (currUser == null)
                return string.Format("{0}当前用户信息获取失败", errPrex);
            if (!BpmOperate.IsCurrentToDoTaskHandler(workTodoId, currUser))
                return string.Format("{0}您不是当前待办的处理者，如有疑问请联系管理员", errPrex);
            Bpm_WorkNode returnNode = null;
            if (workAction == WorkActionEnum.Returning) //回退时
            {
                if (returnNodeId.HasValue && returnNodeId.Value != Guid.Empty) //有回退结点ID参数
                    returnNode = GetWorkNode(returnNodeId.Value);
                else //默认回退到前一结点
                    returnNode = GetPrexNode(workFlow.Id, workNode.Id);
                if (returnNode == null)
                    return string.Format("{0}回退节点不存在，请联系系统运维人员", errPrex);
            }
            else if (workAction == WorkActionEnum.Directing) //指派时
            {
                if (!directHandler.HasValue || directHandler.Value == Guid.Empty)
                    return string.Format("{0}指派时必须指定处理人", errPrex);
                if (directHandler == currUser.EmpId)
                    return string.Format("{0}不能将待办指派给自己", errPrex);
            }
            workFlowInst.StatusOfEnum = flowStatus;
            errMsg = HandleProcess(workFlow, workFlowInst, workNode, workNodeInst, currUser, workAction, approvalOpinions, workTodo, returnNode, directHandler);
            return errMsg;
        }

        /// <summary>
        /// 处理流程
        /// </summary>
        /// <param name="workflow">流程</param>
        /// <param name="workFlowInst">流程实例</param>
        /// <param name="workNode">流程结点</param>
        /// <param name="workNodeInst">流程结点实例</param>
        /// <param name="currUser">当前用户</param>
        /// <param name="workAction">操作动作</param>
        /// <param name="approvalOpinions">审批意见</param>
        /// <param name="workTodo">审批时处理的当前待办任务</param>
        /// <param name="returnNode">回退时的回退结点</param>
        /// <param name="directHandler">指派时的人员ID</param>
        /// <returns></returns>
        private static string HandleProcess(Bpm_WorkFlow workflow, Bpm_WorkFlowInstance workFlowInst, Bpm_WorkNode workNode, Bpm_WorkNodeInstance workNodeInst, UserInfo currUser, WorkActionEnum workAction, string approvalOpinions, Bpm_WorkToDoList workTodo, Bpm_WorkNode returnNode = null, Guid? directHandler = null)
        {
            #region 准备参数
            string errMsg = string.Empty;
            string userAliasName = string.IsNullOrEmpty(currUser.AliasName) ? currUser.UserName : currUser.AliasName;
            WorkFlowStatusEnum workflowStatus = workFlowInst.StatusOfEnum; //流程状态
            string errPrex = GetFlowErrPrex(workAction);
            DatabaseType dbType = DatabaseType.MsSqlServer;
            string connStr = ModelConfigHelper.GetModelConnStr(typeof(Bpm_WorkFlow), out dbType, false);
            string subStr = workFlowInst.ParentId.HasValue && workFlowInst.ParentId.Value != Guid.Empty ? "_SUB" : string.Empty;
            bool isParallelApproval = false; //是否是并行审批
            //并行审批判断条件：是并行审批结点并且对应该结点的并行审批还有其他人员未处理的，即并行审批
            //最后一个人审批时按非并行审批处理，并行审批不允许回退、拒绝
            if (workTodo != null && workNode.HandleStrategyOfEnum == HandleStrategyTypeEnum.AllAgree)
            {
                if (workAction == WorkActionEnum.Returning || workAction == WorkActionEnum.Refusing)
                    return string.Format("{0}该审批结点为并行结点不允许进行该操作", errPrex);
                List<Guid?> tempHandlers = workTodo.NextNodeHandler.ObjToStr().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => x.ObjToGuidNull()).Where(x => x != Guid.Empty && x != currUser.EmpId.Value).ToList();
                if (tempHandlers.Count > 0) //除了当前人还有其他并行审批人员
                {
                    int tempWorkAction = (int)WorkActionEnum.NoAction;
                    List<Bpm_WorkToDoList> noHandleTodos = CommonOperate.GetEntities<Bpm_WorkToDoList>(out errMsg, x => x.Bpm_WorkFlowInstanceId == workFlowInst.Id && x.Bpm_WorkNodeInstanceId == workNodeInst.Id && tempHandlers.Contains(x.OrgM_EmpId) && x.WorkAction == tempWorkAction && !x.IsDeleted, null, false);
                    if (noHandleTodos != null && noHandleTodos.Count > 0) //还有其他并行审批人员未审批
                        isParallelApproval = true;
                }
            }
            bool isStartSubFlow = false; //是否发起子流程
            List<Guid> nextTodoIds = new List<Guid>(); //下一结点待办ID集合
            #endregion
            #region 事务处理
            //启用事务
            CommonOperate.TransactionHandle((conn) =>
            {
                #region 针对流程发起处理流程实例及结点实例
                //针对流程发起，初始化流程实例、流程结点实例
                if (workFlowInst.Id == Guid.Empty && workTodo == null)
                {
                    Guid workFlowInstId = CommonOperate.OperateRecord<Bpm_WorkFlowInstance>(workFlowInst, ModelRecordOperateType.Add, out errMsg, null, false, false, null, null, conn);
                    if (workFlowInstId == Guid.Empty)
                        throw new Exception(string.Format("{0}流程实例初始化失败，{1}", errPrex, errMsg));
                    workFlowInst.Id = workFlowInstId;
                    workNodeInst.Bpm_WorkNodeId = workNode.Id;
                    workNodeInst.Bpm_WorkFlowInstanceId = workFlowInstId;
                    Guid workNodeInstId = CommonOperate.OperateRecord<Bpm_WorkNodeInstance>(workNodeInst, ModelRecordOperateType.Add, out errMsg, null, false, false, null, null, conn);
                    if (workNodeInstId == Guid.Empty)
                    {
                        errMsg = string.Format("{0}流程结点实例初始化失败，{1}", errPrex, errMsg);
                        throw new Exception(errMsg);
                    }
                    workNodeInst.Id = workNodeInstId;
                }
                #endregion
                #region 针对非并行审批
                if (!isParallelApproval) //非并行审批
                {
                    if (workAction != WorkActionEnum.Directing) //非指派操作
                    {
                        #region 找下一审批结点及处理人
                        //处理当前结点实例
                        workNodeInst.FinishDate = DateTime.Now;
                        workNodeInst.StatusOfEnum = WorkNodeStatusEnum.Do;
                        Guid result = CommonOperate.OperateRecord<Bpm_WorkNodeInstance>(workNodeInst, ModelRecordOperateType.Edit, out errMsg, new List<string>() { "Status", "FinishDate" }, false, false, null, null, conn);
                        if (result == Guid.Empty)
                            throw new Exception(string.Format("{0}流程结点实例状态更新失败，{1}", errPrex, errMsg));
                        //待办处理
                        List<Guid> handleEmpIds = new List<Guid>(); //下一结点待办人
                        Bpm_WorkNode nextApprovalNode = null; //下一审批结点
                        List<string> updateWorkflowInstFields = new List<string>() { "Status" };
                        List<Bpm_WorkNode> nextNodes = new List<Bpm_WorkNode>();
                        if (workAction == WorkActionEnum.Approving || workAction == WorkActionEnum.Starting ||
                            workAction == WorkActionEnum.SubStarting || workAction == WorkActionEnum.ReStarting)
                        {
                            nextNodes = GetNextWorkNodes(workflow.Id, workNode.Id);
                        }
                        else if (workAction == WorkActionEnum.Returning) //回退
                        {
                            nextNodes = new List<Bpm_WorkNode>() { returnNode };
                            workflowStatus = WorkFlowStatusEnum.Return;
                        }
                        else if (workAction == WorkActionEnum.Refusing) //拒绝
                        {
                            nextNodes = new List<Bpm_WorkNode>();
                            workflowStatus = WorkFlowStatusEnum.Refused;
                        }
                        if (nextNodes.Count > 1) //对应的下一结点有多个
                        {
                            nextApprovalNode = GetNextActionNode(nextNodes, workflow, workNode, workFlowInst, workNodeInst);
                            if (nextApprovalNode == null)
                            {
                                errMsg = string.Format("{0}找不到当前结点【{1}】的下一处理结点，指向所有下一结点流转条件均不满足", errPrex, workNode.Name);
                                throw new Exception(errMsg);
                            }
                            handleEmpIds = GetNextNodeHandler(nextApprovalNode, currUser, workFlowInst);
                        }
                        else if (nextNodes.Count == 1) //只有一个
                        {
                            nextApprovalNode = nextNodes.FirstOrDefault();
                            handleEmpIds = GetNextNodeHandler(nextApprovalNode, currUser, workFlowInst);
                        }
                        else //没有下一结点，流程结束
                        {
                            if (workAction != WorkActionEnum.Refusing)
                                workflowStatus = WorkFlowStatusEnum.Over;
                            workFlowInst.EndDate = DateTime.Now;
                            updateWorkflowInstFields.Add("EndDate");
                        }
                        //更新流程实例状态
                        workFlowInst.StatusOfEnum = workflowStatus;
                        result = CommonOperate.OperateRecord<Bpm_WorkFlowInstance>(workFlowInst, ModelRecordOperateType.Edit, out errMsg, updateWorkflowInstFields, false, false, null, null, conn);
                        if (result == Guid.Empty)
                            throw new Exception(string.Format("{0}流程实例状态更新失败，{1}", errPrex, errMsg));
                        #endregion
                        #region 处理当前待办
                        //处理当前待办
                        if (workTodo == null) //发起节点
                        {
                            #region 发起待办初始化
                            string titleKeyDisplay = SystemOperate.GetModuleTitleKeyDisplay(workflow.Sys_ModuleId.Value);
                            string titleKeyValue = string.IsNullOrEmpty(titleKeyDisplay) ? string.Empty : CommonOperate.GetModelTitleKeyValue(workflow.Sys_ModuleId.Value, workFlowInst.RecordId);
                            string formatStr = string.IsNullOrEmpty(titleKeyDisplay) || string.IsNullOrEmpty(titleKeyValue) ? string.Empty : string.Format("－{0}：{1}", titleKeyDisplay, titleKeyValue);
                            workTodo = new Bpm_WorkToDoList()
                            {
                                Code = string.Format("WT{0}{1}", subStr, DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                                Title = string.Format("{0}{1}发起【{2}】流程{3}", OrgMOperate.GetEmpMainDeptName(workFlowInst.OrgM_EmpId.Value), workFlowInst.OrgM_EmpName.ObjToStr(), workflow.Sys_ModuleName, formatStr),
                                Bpm_WorkFlowInstanceId = workFlowInst.Id,
                                Bpm_WorkNodeInstanceId = workNodeInst.Id,
                                OrgM_EmpId = currUser.EmpId,
                                Launcher = workFlowInst.OrgM_EmpName,
                                LaunchTime = workFlowInst.StartDate,
                                ModuleId = workflow.Sys_ModuleId.Value,
                                RecordId = workFlowInst.RecordId,
                                StartDate = DateTime.Now,
                                FinishDate = DateTime.Now,
                                StatusOfEnum = workflowStatus,
                                WorkActionOfEnum = WorkActionEnum.Starting,
                                ApprovalOpinions = approvalOpinions,
                                Bpm_WorkNodeId = nextApprovalNode != null ? nextApprovalNode.Id : (Guid?)null,
                                NextNodeHandler = string.Join(",", handleEmpIds),
                                CreateUserId = currUser.UserId,
                                CreateUserName = userAliasName,
                                ModifyUserId = currUser.UserId,
                                ModifyUserName = userAliasName,
                            };
                            result = CommonOperate.OperateRecord<Bpm_WorkToDoList>(workTodo, ModelRecordOperateType.Add, out errMsg, null, false, false, null, null, conn);
                            if (result == Guid.Empty)
                                throw new Exception(string.Format("{0}当前待办任务初始化失败,{1}", errPrex, errMsg));
                            #endregion
                        }
                        else //审批节点
                        {
                            #region 当前待办初始化
                            List<string> upFn = new List<string>() { "ApprovalOpinions", "FinishDate", "Status", "WorkAction" };
                            workTodo.ApprovalOpinions = approvalOpinions;
                            workTodo.FinishDate = DateTime.Now;
                            workTodo.StatusOfEnum = workflowStatus;
                            workTodo.WorkActionOfEnum = workAction;
                            if (nextApprovalNode == null) //不存在下一审批节点
                            {
                                if (workAction != WorkActionEnum.Refusing) //审批结束
                                {
                                    Bpm_WorkNode endNode = GetEndNode(workflow.Id);
                                    if (endNode != null)
                                    {
                                        workTodo.NextNodeHandler = string.Empty;
                                        workTodo.Bpm_WorkNodeId = endNode.Id;
                                        upFn.Add("Bpm_WorkNodeId");
                                        upFn.Add("NextNodeHandler");
                                    }
                                }
                                else //审批拒绝
                                {
                                    workTodo.NextNodeHandler = string.Empty;
                                    workTodo.Bpm_WorkNodeId = null;
                                    upFn.Add("Bpm_WorkNodeId");
                                    upFn.Add("NextNodeHandler");
                                }
                            }
                            else //有下一审批节点
                            {
                                workTodo.Bpm_WorkNodeId = nextApprovalNode.Id;
                                workTodo.NextNodeHandler = string.Join(",", handleEmpIds);
                                upFn.Add("Bpm_WorkNodeId");
                                upFn.Add("NextNodeHandler");
                            }
                            result = CommonOperate.OperateRecord<Bpm_WorkToDoList>(workTodo, ModelRecordOperateType.Edit, out errMsg, upFn, false, false, null, null, conn);
                            if (result == Guid.Empty)
                                throw new Exception(string.Format("{0}当前待办任务状态更新失败,{1}", errPrex, errMsg));
                            #endregion
                        }
                        #endregion
                        //处理下一结点
                        if (nextApprovalNode != null)
                        {
                            #region 初始化下一审批结点实例和下一审批结点待办
                            //初始化下一结点实例
                            Bpm_WorkNodeInstance nextApprovalNodeInst = new Bpm_WorkNodeInstance()
                            {
                                SerialNo = string.Format("WNI{0}{1}", subStr, DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                                Bpm_WorkFlowInstanceId = workFlowInst.Id,
                                Bpm_WorkNodeId = nextApprovalNode.Id,
                                StartDate = DateTime.Now,
                                StatusOfEnum = WorkNodeStatusEnum.Undo,
                                CreateUserId = currUser.UserId,
                                CreateUserName = userAliasName,
                                ModifyUserId = currUser.UserId,
                                ModifyUserName = userAliasName,
                            };
                            Guid nextApprovalNodeInstId = CommonOperate.OperateRecord<Bpm_WorkNodeInstance>(nextApprovalNodeInst, ModelRecordOperateType.Add, out errMsg, null, false, false, null, null, conn);
                            if (nextApprovalNodeInstId == Guid.Empty)
                                throw new Exception(string.Format("{0}下一审批结点实例初始化失败,{1}", errPrex, errMsg));
                            nextApprovalNodeInst.Id = nextApprovalNodeInstId;
                            //生成待办
                            List<Bpm_WorkToDoList> nextTodoList = new List<Bpm_WorkToDoList>();
                            foreach (Guid empId in handleEmpIds)
                            {
                                Bpm_WorkToDoList toDo = new Bpm_WorkToDoList()
                                {
                                    Code = string.Format("WT{0}{1}", subStr, DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                                    Title = workTodo.Title,
                                    Bpm_WorkFlowInstanceId = workFlowInst.Id,
                                    Bpm_WorkNodeInstanceId = nextApprovalNodeInst.Id,
                                    OrgM_EmpId = empId,
                                    Launcher = workTodo.Launcher,
                                    LaunchTime = workTodo.LaunchTime,
                                    ModuleId = workTodo.ModuleId,
                                    RecordId = workTodo.RecordId,
                                    StartDate = DateTime.Now,
                                    StatusOfEnum = workflowStatus,
                                    WorkActionOfEnum = WorkActionEnum.NoAction,
                                    Bpm_WorkNodeId = nextApprovalNode != null ? nextApprovalNode.Id : (Guid?)null,
                                    NextNodeHandler = string.Join(",", handleEmpIds),
                                    CreateUserId = currUser.UserId,
                                    CreateUserName = userAliasName,
                                    ModifyUserId = currUser.UserId,
                                    ModifyUserName = userAliasName,
                                };
                                nextTodoList.Add(toDo);
                            }
                            bool rs = CommonOperate.OperateRecords<Bpm_WorkToDoList>(nextTodoList, ModelRecordOperateType.Add, out errMsg, false, false, null, null, conn);
                            if (!rs)
                                throw new Exception(string.Format("{0}下一审批节点待办任务初始化失败，{1}", errPrex, errMsg));
                            nextTodoIds = nextTodoList.Select(x => x.Id).ToList();
                            isStartSubFlow = workNode.Bpm_WorkFlowSubId.HasValue && workNode.Bpm_WorkFlowSubId.Value != Guid.Empty;
                            #endregion
                        }
                        #region 流程结束
                        else if (workflowStatus == WorkFlowStatusEnum.Over) //流程结束
                        {
                            //如果当前流程是子流程，如果所有子流程都结束，则主流程继续流到下一结点
                            Bpm_WorkNode parentNode = GetParentFlowNode(workflow.Id); //当前子流程的父流程结点
                            if (parentNode != null && parentNode.Bpm_WorkFlowId.HasValue && workFlowInst.ParentId.HasValue && workFlowInst.ParentId.Value != Guid.Empty) //当前是子流程
                            {
                                int tempStatus = (int)WorkFlowStatusEnum.Over;
                                long subFlowCount = CommonOperate.Count<Bpm_WorkFlowInstance>(out errMsg, false, x => x.Bpm_WorkFlowId == workflow.Id && x.Id != workFlowInst.Id && x.Status == tempStatus && x.ParentId == workFlowInst.ParentId);
                                if (subFlowCount == 0) //所有对应子流程均结束
                                {
                                    //父流程下一审批结点
                                    Bpm_WorkNode parentNextNode = GetNextWorkNodes(parentNode.Bpm_WorkFlowId.Value, parentNode.Id).FirstOrDefault();
                                    if (parentNextNode != null)
                                    {
                                        //初始化父流程下一结点实例
                                        Bpm_WorkNodeInstance parentNextNodeInst = CommonOperate.GetEntity<Bpm_WorkNodeInstance>(x => x.Bpm_WorkFlowInstanceId == workFlowInst.ParentId.Value && x.Bpm_WorkNodeId == parentNextNode.Id, null, out errMsg);
                                        if (parentNextNodeInst == null)
                                            throw new Exception(string.Format("{0}父流程下一审批结点实例未初始化,{1}", errPrex, errMsg));
                                        //显示父流程下一结点的所有待办
                                        bool rs = CommonOperate.UpdateRecordsByExpression<Bpm_WorkToDoList>(new { IsDeleted = false }, x => x.Bpm_WorkFlowInstanceId == workFlowInst.ParentId.Value && x.Bpm_WorkNodeInstanceId == parentNextNodeInst.Id, out errMsg);
                                        if (!rs)
                                            throw new Exception(string.Format("{0}父流程下一审批结点待办初始化失败,{1}", errPrex, errMsg));
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    else //流程指派
                    {
                        #region 指派处理
                        //指派人待办
                        Bpm_WorkToDoList directTodo = new Bpm_WorkToDoList()
                        {
                            Code = string.Format("WT{0}{1}", subStr, DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                            Title = workTodo.Title,
                            Bpm_WorkFlowInstanceId = workTodo.Bpm_WorkFlowInstanceId,
                            Bpm_WorkNodeInstanceId = workTodo.Bpm_WorkNodeInstanceId,
                            OrgM_EmpId = directHandler,
                            Launcher = workTodo.Launcher,
                            LaunchTime = workTodo.LaunchTime,
                            ModuleId = workTodo.ModuleId,
                            RecordId = workTodo.RecordId,
                            StartDate = DateTime.Now,
                            StatusOfEnum = workTodo.StatusOfEnum,
                            WorkActionOfEnum = WorkActionEnum.NoAction,
                            Bpm_WorkNodeId = workTodo.Bpm_WorkNodeId,
                            NextNodeHandler = workTodo.NextNodeHandler,
                            CreateUserId = currUser.UserId,
                            CreateUserName = userAliasName,
                            ModifyUserId = currUser.UserId,
                            ModifyUserName = userAliasName,
                        };
                        List<string> upFn = new List<string>() { "ApprovalOpinions", "FinishDate", "WorkAction", "Bpm_WorkNodeId", "NextNodeHandler" };
                        workTodo.ApprovalOpinions = approvalOpinions;
                        workTodo.FinishDate = DateTime.Now;
                        workTodo.WorkActionOfEnum = workAction;
                        workTodo.Bpm_WorkNodeId = workNode.Id; //下一结点仍然指向当前结点
                        workTodo.NextNodeHandler = directHandler.ObjToStr(); //下一结点处理人指向指派人
                        Guid result = CommonOperate.OperateRecord<Bpm_WorkToDoList>(workTodo, ModelRecordOperateType.Edit, out errMsg, upFn, false, false, null, null, conn);
                        if (result != Guid.Empty)
                        {
                            result = CommonOperate.OperateRecord<Bpm_WorkToDoList>(directTodo, ModelRecordOperateType.Add, out errMsg, null, false, false, null, null, conn);
                            if (result == Guid.Empty)
                                throw new Exception(string.Format("{0}指派人待办任务初始化失败,{1}", errPrex, errMsg));
                        }
                        else
                        {
                            throw new Exception(string.Format("{0}当前待办任务状态更新失败,{1}", errPrex, errMsg));
                        }
                        #endregion
                    }
                }
                #endregion
                #region 并行审批
                else //并行审批
                {
                    //处理当前待办
                    List<string> upFn = new List<string>() { "ApprovalOpinions", "FinishDate", "WorkAction" };
                    workTodo.ApprovalOpinions = approvalOpinions;
                    workTodo.FinishDate = DateTime.Now;
                    workTodo.WorkActionOfEnum = workAction;
                    #region 指派处理
                    Bpm_WorkToDoList directTodo = null; //指派人待办
                    if (workAction == WorkActionEnum.Directing)
                    {
                        //指派人待办
                        directTodo = new Bpm_WorkToDoList()
                        {
                            Code = string.Format("WT{0}{1}", subStr, DateTime.Now.ToString("yyyyMMddHHmmssfff")),
                            Title = workTodo.Title,
                            Bpm_WorkFlowInstanceId = workTodo.Bpm_WorkFlowInstanceId,
                            Bpm_WorkNodeInstanceId = workTodo.Bpm_WorkNodeInstanceId,
                            OrgM_EmpId = directHandler,
                            Launcher = workTodo.Launcher,
                            LaunchTime = workTodo.LaunchTime,
                            ModuleId = workTodo.ModuleId,
                            RecordId = workTodo.RecordId,
                            StartDate = DateTime.Now,
                            StatusOfEnum = workTodo.StatusOfEnum,
                            WorkActionOfEnum = WorkActionEnum.NoAction,
                            Bpm_WorkNodeId = workTodo.Bpm_WorkNodeId,
                            NextNodeHandler = workTodo.NextNodeHandler,
                            CreateUserId = currUser.UserId,
                            CreateUserName = userAliasName,
                            ModifyUserId = currUser.UserId,
                            ModifyUserName = userAliasName,
                        };
                        workTodo.Bpm_WorkNodeId = workNode.Id; //下一结点仍然指向当前结点
                        workTodo.NextNodeHandler = directHandler.ObjToStr(); //下一结点处理人指向指派人
                        upFn.Add("Bpm_WorkNodeId");
                        upFn.Add("NextNodeHandler");
                    }
                    else
                    {
                        workTodo.StatusOfEnum = workflowStatus;
                        upFn.Add("Status");
                    }
                    #endregion
                    Guid result = CommonOperate.OperateRecord<Bpm_WorkToDoList>(workTodo, ModelRecordOperateType.Edit, out errMsg, upFn, false, false, null, null, conn);
                    if (result != Guid.Empty)
                    {
                        if (directTodo != null)
                        {
                            result = CommonOperate.OperateRecord<Bpm_WorkToDoList>(directTodo, ModelRecordOperateType.Add, out errMsg, null, false, false, null, null, conn);
                            if (result == Guid.Empty)
                                throw new Exception(string.Format("{0}指派人待办任务初始化失败,{1}", errPrex, errMsg));
                        }
                    }
                    else
                    {
                        throw new Exception(string.Format("{0}当前待办任务状态更新失败,{1}", errPrex, errMsg));
                    }
                }
                #endregion
            }, out errMsg, connStr, dbType);
            #endregion
            #region 发起子流程
            if (isStartSubFlow) //需要发子流程
            {
                Guid mainModuleId = GetWorkflowModuleId(workflow.Id); //当前流程模块ID
                Type mainModelType = CommonOperate.GetModelType(mainModuleId); //当前流程模块类型
                Guid subModuleId = GetWorkflowModuleId(workNode.Bpm_WorkFlowSubId.Value); //子流程模块
                Type subModelType = CommonOperate.GetModelType(subModuleId);
                string tableName = ModelConfigHelper.GetModuleTableName(subModelType);
                //获取当前记录的明细ID集合
                DatabaseType tempDbType = DatabaseType.MsSqlServer;
                string tempConnStr = ModelConfigHelper.GetModelConnStr(subModelType, out tempDbType, true);
                string sql = string.Format("SELECT Id FROM {0} WHERE {1}Id='{2}'", tableName, mainModelType.Name, workTodo.RecordId);
                DataTable dt = CommonOperate.ExecuteQuery(out errMsg, sql, null, tempConnStr, tempDbType);
                if (dt != null && dt.Rows.Count > 0)
                {
                    List<Guid> subIds = dt.Rows.Cast<DataRow>().Select(x => x[0].ObjToGuid()).ToList();
                    foreach (Guid subId in subIds)
                    {
                        string msg = StartProcess(subModuleId, subId, currUser, workFlowInst.Id); //以当前人身份发起子流程
                        if (string.IsNullOrEmpty(msg)) //子流程发起成功
                        {
                            //隐藏主流程当前结点的下一结点待办
                            CommonOperate.UpdateRecordsByExpression<Bpm_WorkToDoList>(new { IsDeleted = true }, x => nextTodoIds.Contains(x.Id), out errMsg);
                        }
                    }
                }
            }
            #endregion
            #region 操作完成事件触发
            CommonOperate.ExecuteCustomeOperateHandleMethod(workflow.Sys_ModuleId.Value, "AfterFlowOperateCompleted", new object[] { workFlowInst.RecordId, workNode.Name, currUser, string.IsNullOrEmpty(errMsg), workAction, workflowStatus });
            #endregion
            return errMsg;
        }

        /// <summary>
        /// 根据操作动作获取流程状态
        /// </summary>
        /// <param name="workAction">操作动作</param>
        /// <param name="workflowStatus">流程状态</param>
        /// <param name="errPrex">异常前缀</param>
        private static void GetFlowStatusAndErrPrex(WorkActionEnum workAction, out WorkFlowStatusEnum workflowStatus, out string errPrex)
        {
            workflowStatus = WorkFlowStatusEnum.NoStatus;
            errPrex = string.Empty;
            switch (workAction)
            {
                case WorkActionEnum.Starting:
                case WorkActionEnum.ReStarting:
                    workflowStatus = WorkFlowStatusEnum.Start;
                    errPrex = "【流程发起】";
                    break;
                case WorkActionEnum.Approving:
                case WorkActionEnum.Directing:
                    workflowStatus = WorkFlowStatusEnum.Approving;
                    errPrex = "【流程审批】";
                    break;
                case WorkActionEnum.Returning:
                    workflowStatus = WorkFlowStatusEnum.Return;
                    errPrex = "【流程回退】";
                    break;
                case WorkActionEnum.Refusing:
                    workflowStatus = WorkFlowStatusEnum.Refused;
                    errPrex = "【流程拒绝】";
                    break;
            }
        }

        /// <summary>
        /// 获取流程异常前缀
        /// </summary>
        /// <param name="workAction">操作动作</param>
        /// <returns></returns>
        private static string GetFlowErrPrex(WorkActionEnum workAction)
        {
            switch (workAction)
            {
                case WorkActionEnum.Starting:
                case WorkActionEnum.ReStarting:
                    return "【流程发起】";
                case WorkActionEnum.Approving:
                case WorkActionEnum.Directing:
                    return "【流程审批】";
                case WorkActionEnum.Returning:
                    return "【流程回退】";
                case WorkActionEnum.Refusing:
                    return "【流程拒绝】";
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取流程操作类型描述
        /// </summary>
        /// <param name="workAction"></param>
        /// <returns></returns>
        public static string GetFlowOpTypeDes(WorkActionEnum workAction)
        {
            switch (workAction)
            {
                case WorkActionEnum.Starting:
                case WorkActionEnum.ReStarting:
                    return "发起流程";
                case WorkActionEnum.Approving:
                    return "审批流程";
                case WorkActionEnum.Directing:
                    return "指派流程";
                case WorkActionEnum.Returning:
                    return "回退流程";
                case WorkActionEnum.Refusing:
                    return "拒绝流程";
                case WorkActionEnum.SubStarting:
                    return "发起子流程";
                case WorkActionEnum.Communicating:
                    return "沟通流程";
            }
            return string.Empty;
        }

        /// <summary>
        /// 是否启用工作流
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns></returns>
        public static bool IsEnabledWorkflow(Guid moduleId)
        {
            Bpm_WorkFlow workflow = GetModuleWorkFlow(moduleId);
            return workflow != null;
        }

        /// <summary>
        /// 获取单据的流程状态
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <param name="id">记录ID</param>
        /// <returns></returns>
        public static WorkFlowStatusEnum GetRecordFlowStatus(Guid moduleId, Guid? id)
        {
            if (id.HasValue && id.Value != Guid.Empty)
            {
                Bpm_WorkFlowInstance workflowInst = GetWorkflowInstance(moduleId, id.Value);
                if (workflowInst != null)
                    return workflowInst.StatusOfEnum;
            }
            return WorkFlowStatusEnum.NoStatus;
        }

        /// <summary>
        /// 获取记录的审批信息
        /// </summary>
        /// <param name="workflowInstId">流程实例ID</param>
        /// <returns></returns>
        public static List<ApprovalInfo> GetRecordApprovalInfos(Guid workflowInstId)
        {
            List<ApprovalInfo> approvalInfos = new List<ApprovalInfo>();
            string errMsg = string.Empty;
            List<Bpm_WorkToDoList> todoList = CommonOperate.GetEntities<Bpm_WorkToDoList>(out errMsg, x => x.Bpm_WorkFlowInstanceId == workflowInstId && !x.IsDeleted, null, false, new List<string>() { "FinishDate" }, new List<bool>() { true });
            if (todoList != null && todoList.Count > 0)
            {
                foreach (Bpm_WorkToDoList todo in todoList)
                {
                    if (!todo.Bpm_WorkNodeInstanceId.HasValue)
                        continue;
                    Bpm_WorkNodeInstance nodeInst = CommonOperate.GetEntityById<Bpm_WorkNodeInstance>(todo.Bpm_WorkNodeInstanceId.Value, out errMsg);
                    if (nodeInst == null || !nodeInst.Bpm_WorkNodeId.HasValue)
                        continue;
                    Bpm_WorkNode currNode = GetWorkNode(nodeInst.Bpm_WorkNodeId.Value);
                    if (currNode == null || !currNode.Bpm_WorkFlowId.HasValue) continue;
                    if (todo.WorkActionOfEnum != WorkActionEnum.Directing) //非指派
                    {
                        if (currNode.HandleStrategyOfEnum == HandleStrategyTypeEnum.AllAgree)
                        {
                            if (todo.WorkActionOfEnum == WorkActionEnum.NoAction)
                                continue;
                        }
                        else
                        {
                            if (nodeInst.StatusOfEnum == WorkNodeStatusEnum.Undo)
                                continue;
                        }
                    }
                    string empIdStr = string.Empty;
                    if (todo.OrgM_EmpId.HasValue && todo.OrgM_EmpId.Value != Guid.Empty)
                    {
                        OrgM_Emp tempEmp = OrgMOperate.GetEmp(todo.OrgM_EmpId.Value);
                        if (tempEmp != null)
                            empIdStr = tempEmp.Name;
                    }
                    string handleTime = todo.FinishDate.HasValue ? todo.FinishDate.Value.ToString() : string.Empty;
                    string handleResult = string.Empty;
                    if (todo.WorkActionOfEnum != WorkActionEnum.Starting)
                    {
                        int btnType = (int)FlowButtonTypeEnum.AgreeBtn;
                        switch (todo.WorkActionOfEnum)
                        {
                            case WorkActionEnum.Returning:
                                btnType = (int)FlowButtonTypeEnum.BackBtn;
                                break;
                            case WorkActionEnum.Refusing:
                                btnType = (int)FlowButtonTypeEnum.RejectBtn;
                                break;
                            case WorkActionEnum.Directing:
                                btnType = (int)FlowButtonTypeEnum.AssignBtn;
                                break;
                        }
                        Bpm_FlowBtn flowBtn = GetAllWorkButtons(x => x.ButtonType == btnType).FirstOrDefault();
                        if (flowBtn != null)
                        {
                            Bpm_NodeBtnConfig btnConfig = GetAllApprovalBtnConfigs(x => x.Bpm_FlowBtnId == flowBtn.Id && x.Bpm_WorkFlowId == currNode.Bpm_WorkFlowId.Value && x.Bpm_WorkNodeId == currNode.Id).FirstOrDefault();
                            if (btnConfig != null)
                                handleResult = btnConfig.BtnDisplay;
                            else
                                handleResult = flowBtn.ButtonText;
                        }
                    }
                    else
                    {
                        handleResult = "发起";
                    }
                    string nextHandlerStr = string.Empty;
                    if (!string.IsNullOrEmpty(todo.NextNodeHandler))
                    {
                        List<Guid> tempEmpIds = todo.NextNodeHandler.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => x.ObjToGuid()).Where(x => x != Guid.Empty).ToList();
                        if (currNode.HandleStrategyOfEnum == HandleStrategyTypeEnum.AllAgree)
                        {
                            List<Guid> hasHandler = todoList.Where(x => x.Bpm_WorkNodeInstanceId == nodeInst.Id && x.OrgM_EmpId.HasValue && x.WorkActionOfEnum != WorkActionEnum.NoAction).Select(x => x.OrgM_EmpId.Value).ToList();
                            tempEmpIds = tempEmpIds.Where(x => !hasHandler.Contains(x)).ToList();
                        }
                        if (tempEmpIds.Count > 0)
                        {
                            foreach (Guid tempEmpId in tempEmpIds)
                            {
                                OrgM_Emp tempEmp = OrgMOperate.GetEmp(tempEmpId);
                                if (tempEmp == null) continue;
                                if (nextHandlerStr != string.Empty)
                                    nextHandlerStr += ",";
                                nextHandlerStr += tempEmp.Name;
                            }
                        }
                    }
                    Bpm_WorkNode nextNode = todo.Bpm_WorkNodeId.HasValue ? GetWorkNode(todo.Bpm_WorkNodeId.Value) : null;
                    ApprovalInfo item = new ApprovalInfo() { NodeName = currNode.Name, Handler = empIdStr, HandleTime = handleTime, HandleResult = handleResult, ApprovalOpinions = todo.ApprovalOpinions, NextNodeName = nextNode != null ? nextNode.Name : string.Empty, NextHandler = nextHandlerStr };
                    approvalInfos.Add(item);
                }
            }
            return approvalInfos;
        }

        /// <summary>
        /// 根据待办任务ID获取审批信息
        /// </summary>
        /// <param name="todoTaskId">待办任务ID</param>
        /// <returns></returns>
        public static List<ApprovalInfo> GetRecordApprovalInfosByTodoId(Guid todoTaskId)
        {
            string errMsg = string.Empty;
            Bpm_WorkToDoList todo = CommonOperate.GetEntityById<Bpm_WorkToDoList>(todoTaskId, out errMsg);
            if (todo != null && todo.Bpm_WorkFlowInstanceId.HasValue)
            {
                Bpm_WorkFlowInstance workFlowInst = CommonOperate.GetEntityById<Bpm_WorkFlowInstance>(todo.Bpm_WorkFlowInstanceId.Value, out errMsg);
                if (workFlowInst != null)
                    return GetRecordApprovalInfos(workFlowInst.Id);
            }
            return new List<ApprovalInfo>();
        }

        /// <summary>
        /// 根据模块记录ID获取审批信息
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <param name="id">记录ID</param>
        /// <returns></returns>
        public static List<ApprovalInfo> GetModuleRecordApprovalInfos(Guid moduleId, Guid id)
        {
            Bpm_WorkFlowInstance workflowInst = GetWorkflowInstance(moduleId, id);
            if (workflowInst != null)
                return GetRecordApprovalInfos(workflowInst.Id);
            return new List<ApprovalInfo>();
        }

        /// <summary>
        /// 获取当前审批节点
        /// </summary>
        /// <param name="todoTaskId">待办任务ID</param>
        /// <returns></returns>
        public static Bpm_WorkNode GetCurrentApprovalNode(Guid todoTaskId)
        {
            string errMsg = string.Empty;
            Bpm_WorkToDoList todo = CommonOperate.GetEntityById<Bpm_WorkToDoList>(todoTaskId, out errMsg);
            if (todo != null && todo.Bpm_WorkNodeInstanceId.HasValue)
            {
                Bpm_WorkNodeInstance workNodeInst = CommonOperate.GetEntityById<Bpm_WorkNodeInstance>(todo.Bpm_WorkNodeInstanceId.Value, out errMsg);
                if (workNodeInst != null && workNodeInst.Bpm_WorkNodeId.HasValue)
                    return GetWorkNode(workNodeInst.Bpm_WorkNodeId.Value);
            }
            return null;
        }

        /// <summary>
        /// 获取流程结点表单
        /// </summary>
        /// <param name="workNodeId">流程结点ID</param>
        /// <returns></returns>
        public static Sys_Form GetWorkNodeForm(Guid workNodeId)
        {
            Bpm_WorkNode workNode = GetWorkNode(workNodeId);
            if (workNode != null && workNode.Sys_FormId.HasValue)
                return SystemOperate.GetForm(workNode.Sys_FormId.Value);
            return null;
        }

        /// <summary>
        /// 获取自定义流程表单URL
        /// </summary>
        /// <param name="workNodeId">流程结点ID</param>
        /// <returns></returns>
        public static string GetCustomerWorkNodeFormUrl(Guid workNodeId)
        {
            Bpm_WorkNode workNode = GetWorkNode(workNodeId);
            if (workNode != null && workNode.Sys_FormId.HasValue)
                return workNode.FormUrl;
            return null;
        }

        /// <summary>
        /// 获取发起结点表单
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns></returns>
        public static Sys_Form GetLaunchNodeForm(Guid moduleId)
        {
            Bpm_WorkFlow workflow = GetModuleWorkFlow(moduleId);
            if (workflow != null)
            {
                Bpm_WorkNode launchNode = GetLaunchNode(workflow.Id);
                if (launchNode != null)
                    return GetWorkNodeForm(launchNode.Id);
            }
            return null;
        }

        /// <summary>
        /// 获取发起结点自定义表单
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <returns></returns>
        public static string GetLaunchNodeCustomerFormUrl(Guid moduleId)
        {
            Bpm_WorkFlow workflow = GetModuleWorkFlow(moduleId);
            if (workflow != null)
            {
                Bpm_WorkNode launchNode = GetLaunchNode(workflow.Id);
                if (launchNode != null)
                    return GetCustomerWorkNodeFormUrl(launchNode.Id);
            }
            return null;
        }

        /// <summary>
        /// 根据流程待办任务获取流程结点ID
        /// </summary>
        /// <param name="workTodoId">待办任务ID</param>
        /// <returns></returns>
        public static Guid GetWorkNodeIdByTodoId(Guid workTodoId)
        {
            string errMsg = string.Empty;
            Bpm_WorkToDoList todo = CommonOperate.GetEntityById<Bpm_WorkToDoList>(workTodoId, out errMsg);
            if (todo != null && todo.Bpm_WorkNodeInstanceId.HasValue)
            {
                Bpm_WorkNodeInstance workNodeInst = CommonOperate.GetEntityById<Bpm_WorkNodeInstance>(todo.Bpm_WorkNodeInstanceId.Value, out errMsg);
                if (workNodeInst != null && workNodeInst.Bpm_WorkNodeId.HasValue)
                    return workNodeInst.Bpm_WorkNodeId.Value;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// 根据待办ID获取模块ID和记录ID
        /// </summary>
        /// <param name="workTodoId">待办ID</param>
        /// <param name="moduleId">模块ID</param>
        /// <param name="recordId">记录ID</param>
        /// <returns></returns>
        public static void GetModuleIdAndRecordIdByTodoId(Guid workTodoId, out Guid moduleId, out Guid recordId)
        {
            moduleId = Guid.Empty;
            recordId = Guid.Empty;
            string errMsg = string.Empty;
            Bpm_WorkToDoList workTodo = CommonOperate.GetEntityById<Bpm_WorkToDoList>(workTodoId, out errMsg);
            if (workTodo != null)
            {
                moduleId = workTodo.ModuleId;
                recordId = workTodo.RecordId;
            }
        }

        /// <summary>
        /// 是否是当前待办任务的处理者
        /// </summary>
        /// <param name="todoTaskId">当前待办任务ID</param>
        /// <param name="currUser">用户信息</param>
        /// <returns></returns>
        public static bool IsCurrentToDoTaskHandler(Guid todoTaskId, UserInfo currUser)
        {
            if (currUser != null)
            {
                string errMsg = string.Empty;
                Bpm_WorkToDoList todo = CommonOperate.GetEntityById<Bpm_WorkToDoList>(todoTaskId, out errMsg);
                if (todo != null && todo.WorkActionOfEnum == WorkActionEnum.NoAction && (todo.StatusOfEnum == WorkFlowStatusEnum.Start || todo.StatusOfEnum == WorkFlowStatusEnum.Approving || todo.StatusOfEnum == WorkFlowStatusEnum.Return))
                {
                    if (todo.Bpm_WorkNodeInstanceId.HasValue && todo.Bpm_WorkNodeInstanceId.Value != Guid.Empty)
                    {
                        Bpm_WorkNodeInstance workNodeInst = CommonOperate.GetEntityById<Bpm_WorkNodeInstance>(todo.Bpm_WorkNodeInstanceId.Value, out errMsg);
                        if (workNodeInst != null && workNodeInst.StatusOfEnum == WorkNodeStatusEnum.Undo)
                        {
                            return currUser != null && currUser.EmpId.HasValue && todo.OrgM_EmpId.HasValue && todo.OrgM_EmpId.Value == currUser.EmpId.Value;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 是否允许发起流程
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <param name="currUser">当前用户</param>
        /// <param name="id">记录ID</param>
        /// <returns></returns>
        public static bool IsAllowLaunchFlow(Guid moduleId, UserInfo currUser, Guid? id)
        {
            if (currUser == null) return false;
            if (currUser == null || !currUser.EmpId.HasValue || currUser.EmpId.Value == Guid.Empty)
                return false;
            Guid empId = currUser.EmpId.Value;
            if (id.HasValue && id.Value != Guid.Empty)
            {
                Bpm_WorkFlowInstance workflowInst = GetWorkflowInstance(moduleId, id.Value);
                if (workflowInst != null || (workflowInst != null && workflowInst.StatusOfEnum != WorkFlowStatusEnum.NoStatus))
                    return false;
                string errMsg = string.Empty;
                List<int> statusList = new List<int>() { (int)WorkFlowStatusEnum.Start, (int)WorkFlowStatusEnum.Return, (int)WorkFlowStatusEnum.Refused, (int)WorkFlowStatusEnum.Over, (int)WorkFlowStatusEnum.Freezed, (int)WorkFlowStatusEnum.Freezed, (int)WorkFlowStatusEnum.Approving };
                Bpm_WorkToDoList todo = CommonOperate.GetEntity<Bpm_WorkToDoList>(x => x.Bpm_WorkFlowInstanceId == workflowInst.Id && x.OrgM_EmpId == empId && statusList.Contains(x.Status), null, out errMsg);
                if (todo != null) return false;
            }
            List<Guid> empIds = GetLaunchNodeHandler(moduleId);
            return empIds.Contains(currUser.EmpId.Value);
        }

        /// <summary>
        /// 流程是否已发起了
        /// </summary>
        /// <param name="moduleId">模块ID</param>
        /// <param name="id">记录ID</param>
        /// <returns></returns>
        public static bool IsLaunchFlowCompleted(Guid moduleId, Guid id)
        {
            Bpm_WorkFlowInstance workflowInst = GetWorkflowInstance(moduleId, id);
            return workflowInst != null && workflowInst.StatusOfEnum == WorkFlowStatusEnum.Start;
        }

        /// <summary>
        /// 获取当前处理人
        /// </summary>
        /// <param name="todoTaskId">待办任务ID</param>
        /// <returns></returns>
        public static OrgM_Emp GetCurrentToDoHandler(Guid todoTaskId)
        {
            string errMsg = string.Empty;
            Bpm_WorkToDoList todo = CommonOperate.GetEntityById<Bpm_WorkToDoList>(todoTaskId, out errMsg);
            if (todo != null && todo.OrgM_EmpId.HasValue)
            {
                return OrgMOperate.GetEmp(todo.OrgM_EmpId.Value);
            }
            return null;
        }

        #endregion
    }
}
