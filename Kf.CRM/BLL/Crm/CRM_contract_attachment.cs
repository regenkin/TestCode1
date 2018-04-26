using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// CRM_contract_attachment
	/// </summary>
	public partial class CRM_contract_attachment
	{
		private readonly KfCrm.DAL.CRM_contract_attachment dal=new KfCrm.DAL.CRM_contract_attachment();
		public CRM_contract_attachment()
		{}
		#region  Method

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(KfCrm.Model.CRM_contract_attachment model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string wherestr)
		{
			//该表无主键信息，请自定义主键/条件字段
            return dal.Delete(wherestr);
		} 		

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
	
         /// <summary>
        /// 更新ID
        /// </summary>
        public bool UpdateMailid(int contract_id, string page_id)
        {
            return dal.UpdateMailid(contract_id, page_id);
        }

		#endregion  Method
	}
}

