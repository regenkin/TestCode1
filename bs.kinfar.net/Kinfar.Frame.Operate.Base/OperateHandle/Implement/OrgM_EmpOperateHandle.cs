using Kinfar.Frame.Base;
using Kinfar.Frame.Base.Set;
using Kinfar.Frame.Model.OrgM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kinfar.Frame.Operate.Base.OperateHandle.Implement
{
    /// <summary>
    /// 员工操作处理类
    /// </summary>
    class OrgM_EmpOperateHandle : IModelOperateHandle<OrgM_Emp>
    {
        /// <summary>
        /// 员工操作完成
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="t">员工对象</param>
        /// <param name="result">操作结果</param>
        /// <param name="currUser">当前用户</param>
        /// <param name="otherParams"></param>
        public void OperateCompeletedHandle(ModelRecordOperateType operateType, OrgM_Emp t, bool result, UserInfo currUser, object[] otherParams = null)
        {
            if (result)
            {
                string errMsg = string.Empty;
                string username = OrgMOperate.GetUserNameByEmp(t);
                if (operateType == ModelRecordOperateType.Add)
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        UserOperate.AddUser(out errMsg, username, string.Format("{0}_123456", username), null, t.Name);
                    }
                }
                else if (operateType == ModelRecordOperateType.Edit)
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        UserOperate.UpdateUserAliasName(username, t.Name);
                    }
                }
                else if (operateType == ModelRecordOperateType.Del)
                {
                    if (!string.IsNullOrEmpty(username))
                    {
                        UserOperate.DelUser(username); //删除账号
                    }
                }
            }
        }

        /// <summary>
        /// 员工操作前验证
        /// </summary>
        /// <param name="operateType">操作类型</param>
        /// <param name="t">操作对象</param>
        /// <param name="errMsg">异常信息</param>
        /// <param name="otherParams"></param>
        /// <returns></returns>
        public bool BeforeOperateVerifyOrHandle(ModelRecordOperateType operateType, OrgM_Emp t, out string errMsg, object[] otherParams = null)
        {
            errMsg = string.Empty;
            return true;
        }

        /// <summary>
        /// 批量操作完成后事件
        /// </summary>
        /// <param name="operateType"></param>
        /// <param name="ts"></param>
        /// <param name="result"></param>
        /// <param name="currUser">当前用户</param>
        /// <param name="otherParams"></param>
        public void OperateCompeletedHandles(ModelRecordOperateType operateType, List<OrgM_Emp> ts, bool result, UserInfo currUser, object[] otherParams = null)
        {
            if (ts != null && ts.Count > 0)
            {
                foreach (OrgM_Emp t in ts)
                {
                    OperateCompeletedHandle(operateType, t, result, currUser, otherParams);
                }
            }
        }

        /// <summary>
        /// 批量操作前验证事件
        /// </summary>
        /// <param name="operateType"></param>
        /// <param name="ts"></param>
        /// <param name="errMsg"></param>
        /// <param name="otherParams"></param>
        /// <returns></returns>
        public bool BeforeOperateVerifyOrHandles(ModelRecordOperateType operateType, List<OrgM_Emp> ts, out string errMsg, object[] otherParams = null)
        {
            errMsg = string.Empty;
            return true;
        }
    }
}
