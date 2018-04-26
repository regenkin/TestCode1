using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// Sys_data_authority
	/// </summary>
	public partial class Sys_data_authority
	{
		private readonly KfCrm.DAL.Sys_data_authority dal=new KfCrm.DAL.Sys_data_authority();
		public Sys_data_authority()
		{}
		#region  Method

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(KfCrm.Model.Sys_data_authority model)
		{
			dal.Add(model);
		}

	

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(string where)
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			return dal.Delete(where);
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

		

		#endregion  Method
	}
}

