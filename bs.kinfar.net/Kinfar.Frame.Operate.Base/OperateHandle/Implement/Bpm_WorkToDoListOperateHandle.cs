using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Kinfar.Frame.Common;
using Kinfar.Frame.Base;
using Kinfar.Frame.Base.User;
using Kinfar.Frame.Model.Bpm;
using Kinfar.Frame.Model.EnumSpace;
using Kinfar.Frame.Model.OrgM;
using Kinfar.Frame.Operate.Base.EnumDef;

namespace Kinfar.Frame.Operate.Base.OperateHandle.Implement
{
    /// <summary>
    /// 待办任务权限过滤
    /// </summary>
    class Bpm_WorkToDoListOperateHandle : IPermissionHandle<Bpm_WorkToDoList>, IGridOperateHandle<Bpm_WorkToDoList>, IGridSearchHandle<Bpm_WorkToDoList>
    {
        #region 权限接口
        /// <summary>
        /// 权限过滤
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="filterWhere"></param>
        /// <param name="queryCache"></param>
        /// <returns></returns>
        public Expression<Func<Bpm_WorkToDoList, bool>> GetPermissionExp(Guid userId, out string filterWhere, bool queryCache)
        {
            filterWhere = string.Empty;
            OrgM_Emp emp = OrgMOperate.GetEmpByUserId(userId);
            if (emp != null)
            {
                return x => x.OrgM_EmpId == emp.Id || x.CreateUserId == userId;
            }
            return x => x.CreateUserId == Guid.Empty;
        }

        /// <summary>
        /// 是否有记录操作权限
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="t"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool HasRecordOperatePermission(Guid userId, Bpm_WorkToDoList t, int type)
        {
            return true;
        }
        #endregion
        #region 网格接口
        /// <summary>
        /// 网格参数设置
        /// </summary>
        /// <param name="gridType"></param>
        /// <param name="gridParams"></param>
        public void GridParamsSet(EnumDef.DataGridType gridType, TempModel.GridParams gridParams)
        {
        }

        /// <summary>
        /// 网格数据处理
        /// </summary>
        /// <param name="data"></param>
        /// <param name="otherParams"></param>
        public void PageGridDataHandle(List<Bpm_WorkToDoList> data, object[] otherParams = null)
        {
            if (UserInfo.CurrentUserInfo.UserName != "admin" && data != null && data.Count > 0)
            {
                string errMsg = string.Empty;
                foreach (Bpm_WorkToDoList todo in data)
                {
                    if (!todo.Bpm_WorkFlowInstanceId.HasValue) continue;
                    Bpm_WorkFlowInstance flowInst = BpmOperate.GetWorkflowInstanceById(todo.Bpm_WorkFlowInstanceId.Value);
                    if (flowInst == null) continue;
                    todo.StatusOfEnum = flowInst.StatusOfEnum;
                    int status = (int)flowInst.StatusOfEnum;
                    Bpm_WorkToDoList tempTodo = CommonOperate.GetEntity<Bpm_WorkToDoList>(x => x.Bpm_WorkFlowInstanceId == todo.Bpm_WorkFlowInstanceId.Value && x.Status == status, null, out errMsg);
                    if (tempTodo != null && !string.IsNullOrEmpty(todo.NextNodeHandler))
                    {
                        string nextHandlerStr = string.Empty;
                        List<Guid> tempEmpIds = tempTodo.NextNodeHandler.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => x.ObjToGuid()).Where(x => x != Guid.Empty).ToList();
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
                        todo.OrgM_EmpName = nextHandlerStr;
                    }
                }
            }
        }

        /// <summary>
        /// 获取网格过滤条件
        /// </summary>
        /// <param name="where"></param>
        /// <param name="gridType"></param>
        /// <param name="condition"></param>
        /// <param name="initModule"></param>
        /// <param name="initField"></param>
        /// <param name="otherParams"></param>
        /// <returns></returns>
        public Expression<Func<Bpm_WorkToDoList, bool>> GetGridFilterCondition(out string where, DataGridType gridType, Dictionary<string, string> condition = null, string initModule = null, string initField = null, Dictionary<string, string> otherParams = null)
        {
            where = string.Empty;
            bool isAdmin = UserInfo.CurrentUserInfo.UserName == "admin";
            if (UserInfo.CurrentUserInfo.EmpId.HasValue || isAdmin)
            {
                Guid userId = UserInfo.CurrentUserInfo.UserId;
                Guid empId = UserInfo.CurrentUserInfo.EmpId.HasValue ? UserInfo.CurrentUserInfo.EmpId.Value : Guid.Empty;
                if (gridType == DataGridType.MainGrid && condition != null && condition.Count > 0)
                {
                    int noAction = (int)WorkActionEnum.NoAction;
                    if (condition.ContainsKey("tp"))
                    {
                        if (condition["tp"] == "0") //我的待办
                        {
                            if (isAdmin)
                                return x => x.WorkAction == noAction;
                            return x => x.OrgM_EmpId == empId && x.WorkAction == noAction;
                        }
                        else if (condition["tp"] == "1") //我的申请
                        {
                            int startAction = (int)WorkActionEnum.Starting;
                            int startSubAction = (int)WorkActionEnum.SubStarting;
                            if (isAdmin)
                                return x => x.WorkAction == startAction || x.WorkAction == startSubAction;
                            return x => x.CreateUserId == userId && (x.WorkAction == startAction || x.WorkAction == startSubAction);
                        }
                        else if (condition["tp"] == "2") //我的审批
                        {
                            if (isAdmin)
                                return x => x.WorkAction != noAction;
                            return x => x.OrgM_EmpId == empId && x.WorkAction != noAction;
                        }
                    }
                }
            }
            return x => x.CreateUserId == Guid.Empty;
        }

        /// <summary>
        /// 网络按钮验证
        /// </summary>
        /// <param name="buttonText"></param>
        /// <param name="ids"></param>
        /// <param name="otherParams"></param>
        /// <returns></returns>
        public string GridButtonOperateVerify(string buttonText, List<Guid> ids, object[] otherParams = null)
        {
            return string.Empty;
        }
        #endregion
        #region 搜索接口
        public List<TempModel.ConditionItem> GetSeachResults(Dictionary<string, string> q, out string whereSql)
        {
            whereSql = string.Empty;
            return null;
        }

        public TempModel.ConditionItem GetSearchResult(string fieldName, object value, out string whereSql)
        {
            whereSql = string.Empty;
            if (!UserInfo.IsSuperAdmin() && !string.IsNullOrWhiteSpace(value.ObjToStr()))
            {
                if (fieldName == "OrgM_EmpId") //待办人
                {
                    whereSql = string.Format("CHARINDEX((SELECT TOP 1 NextNodeHandler FROM dbo.Bpm_WorkToDoList WHERE Bpm_WorkFlowInstanceId=Bpm_WorkFlowInstanceId AND Status=Status), (SELECT Id FROM dbo.OrgM_Emp WHERE Name LIKE '%{0}%'))>0", value.ObjToStr());
                }
            }
            return null;
        }
        #endregion
    }
}
