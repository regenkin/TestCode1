using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// Sys_authority
	/// </summary>
	public partial class Sys_authority
	{
		private readonly KfCrm.DAL.Sys_authority dal=new KfCrm.DAL.Sys_authority();
		public Sys_authority()
		{}
		#region  Method

		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(KfCrm.Model.Sys_authority model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(KfCrm.Model.Sys_authority model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete()
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			return dal.Delete();
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public KfCrm.Model.Sys_authority GetModel()
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			return dal.GetModel();
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public KfCrm.Model.Sys_authority GetModelByCache()
		{
			//�ñ���������Ϣ�����Զ�������/�����ֶ�
			string CacheKey = "Sys_authorityModel-" ;
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
			return (KfCrm.Model.Sys_authority)objModel;
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
		public List<KfCrm.Model.Sys_authority> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<KfCrm.Model.Sys_authority> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.Sys_authority> modelList = new List<KfCrm.Model.Sys_authority>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.Sys_authority model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.Sys_authority();
					if(dt.Rows[n]["Role_id"]!=null && dt.Rows[n]["Role_id"].ToString()!="")
					{
						model.Role_id=int.Parse(dt.Rows[n]["Role_id"].ToString());
					}
					if(dt.Rows[n]["App_ids"]!=null && dt.Rows[n]["App_ids"].ToString()!="")
					{
					model.App_ids=dt.Rows[n]["App_ids"].ToString();
					}
					if(dt.Rows[n]["Menu_ids"]!=null && dt.Rows[n]["Menu_ids"].ToString()!="")
					{
					model.Menu_ids=dt.Rows[n]["Menu_ids"].ToString();
					}
					if(dt.Rows[n]["Button_ids"]!=null && dt.Rows[n]["Button_ids"].ToString()!="")
					{
					model.Button_ids=dt.Rows[n]["Button_ids"].ToString();
					}
					if(dt.Rows[n]["Create_id"]!=null && dt.Rows[n]["Create_id"].ToString()!="")
					{
						model.Create_id=int.Parse(dt.Rows[n]["Create_id"].ToString());
					}
					if(dt.Rows[n]["Create_date"]!=null && dt.Rows[n]["Create_date"].ToString()!="")
					{
						model.Create_date=DateTime.Parse(dt.Rows[n]["Create_date"].ToString());
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
        /// <summary>
        /// ɾ��һ������
        /// </summary>
        public bool DeleteWhere(string wherestr)
        {
            //�ñ���������Ϣ�����Զ�������/�����ֶ�
            return dal.DeleteWhere(wherestr);
        }
		#endregion  Method
	}
}

