using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// Sys_role_emp
	/// </summary>
	public partial class Sys_role_emp
	{
		private readonly KfCrm.DAL.Sys_role_emp dal=new KfCrm.DAL.Sys_role_emp();
		public Sys_role_emp()
		{}
		#region  Method

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(KfCrm.Model.Sys_role_emp model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(KfCrm.Model.Sys_role_emp model)
		{
			return dal.Update(model);
		}

        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool Delete(string strWhere)
        {
            //�ñ���������Ϣ�����Զ�������/�����ֶ�
            return dal.Delete(strWhere);
        }

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public KfCrm.Model.Sys_role_emp GetModel()
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			return dal.GetModel();
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public KfCrm.Model.Sys_role_emp GetModelByCache()
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			string CacheKey = "Sys_role_empModel-" ;
			object objModel = KfCrm.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel();
					if (objModel != null)
					{
						int ModelCache = KfCrm.Common.ConfigHelper.GetConfigInt("ModelCache");
						KfCrm.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (KfCrm.Model.Sys_role_emp)objModel;
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
		public List<KfCrm.Model.Sys_role_emp> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<KfCrm.Model.Sys_role_emp> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.Sys_role_emp> modelList = new List<KfCrm.Model.Sys_role_emp>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.Sys_role_emp model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.Sys_role_emp();
					if(dt.Rows[n]["RoleID"]!=null && dt.Rows[n]["RoleID"].ToString()!="")
					{
						model.RoleID=int.Parse(dt.Rows[n]["RoleID"].ToString());
					}
					if(dt.Rows[n]["empID"]!=null && dt.Rows[n]["empID"].ToString()!="")
					{
						model.empID=int.Parse(dt.Rows[n]["empID"].ToString());
					}
					modelList.Add(model);
				}
			}
			return modelList;
		}

		/// <summary>
		/// ��������б�
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// ��ҳ��ȡ�����б�
		/// </summary>
		public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
		{
			return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
		}

		#endregion  Method
	}
}

