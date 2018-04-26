using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// sys_info
	/// </summary>
	public partial class sys_info
	{
		private readonly KfCrm.DAL.sys_info dal=new KfCrm.DAL.sys_info();
		public sys_info()
		{}
		#region  Method
        
		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(KfCrm.Model.sys_info model)
		{
			return dal.Update(model);
		}		

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
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

