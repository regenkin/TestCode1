using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KfCrm.BLL
{
    public class hr_employee_openid
    {
        private readonly KfCrm.DAL.hr_employee_openid dal = new KfCrm.DAL.hr_employee_openid();
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Type, int OpenID)
        {
            return dal.Exists(Type, OpenID);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(KfCrm.Model.hr_employee_openid model)
        {
            return dal.Add(model);
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(KfCrm.Model.hr_employee_openid model)
        {
            return dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            return dal.Delete(ID);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            return DeleteList(IDlist);
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public KfCrm.Model.hr_employee_openid GetModel(int ID)
        {
            return dal.GetModel(ID);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public KfCrm.Model.hr_employee GetEmployeeModel(int Type, int OpenID)
        {
            return dal.GetEmployeeModel(Type, OpenID);
        }
        #endregion  Method
    }
}
