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
		/// ����һ������
		/// </summary>
		public void Add(KfCrm.Model.CRM_contract_attachment model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(string wherestr)
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
            return dal.Delete(wherestr);
		} 		

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// ���ǰ��������
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}
	
         /// <summary>
        /// ����ID
        /// </summary>
        public bool UpdateMailid(int contract_id, string page_id)
        {
            return dal.UpdateMailid(contract_id, page_id);
        }

		#endregion  Method
	}
}

