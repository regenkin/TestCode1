using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// Param_SysParam
	/// </summary>
	public partial class Param_SysParam
	{
		private readonly KfCrm.DAL.Param_SysParam dal=new KfCrm.DAL.Param_SysParam();
		public Param_SysParam()
		{}
		#region  Method

		/// <summary>
		/// �õ����ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(KfCrm.Model.Param_SysParam model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(KfCrm.Model.Param_SysParam model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// Ԥɾ��
		/// </summary>
		/// <param name="id"></param>
		/// <param name="isDelete"></param>
		/// <param name="time"></param>
		/// <returns></returns>
		public bool AdvanceDelete(int id, int isDelete, string time)
		{
			return dal.AdvanceDelete(id, isDelete, time);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int id)
		{
			
			return dal.Delete(id);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string idlist )
		{
			return dal.DeleteList(idlist );
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public KfCrm.Model.Param_SysParam GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public KfCrm.Model.Param_SysParam GetModelByCache(int id)
		{
			
			string CacheKey = "Param_SysParamModel-" + id;
			object objModel = KfCrm.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(id);
					if (objModel != null)
					{
						int ModelCache = KfCrm.Common.ConfigHelper.GetConfigInt("ModelCache");
						KfCrm.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (KfCrm.Model.Param_SysParam)objModel;
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
		public List<KfCrm.Model.Param_SysParam> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<KfCrm.Model.Param_SysParam> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.Param_SysParam> modelList = new List<KfCrm.Model.Param_SysParam>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.Param_SysParam model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.Param_SysParam();
					if(dt.Rows[n]["id"]!=null && dt.Rows[n]["id"].ToString()!="")
					{
						model.id=int.Parse(dt.Rows[n]["id"].ToString());
					}
					if(dt.Rows[n]["parentid"]!=null && dt.Rows[n]["parentid"].ToString()!="")
					{
						model.parentid=int.Parse(dt.Rows[n]["parentid"].ToString());
					}
					if(dt.Rows[n]["params_name"]!=null && dt.Rows[n]["params_name"].ToString()!="")
					{
					model.params_name=dt.Rows[n]["params_name"].ToString();
					}
					if(dt.Rows[n]["params_order"]!=null && dt.Rows[n]["params_order"].ToString()!="")
					{
					model.params_order= int.Parse( dt.Rows[n]["params_order"].ToString());
					}
					if(dt.Rows[n]["Create_id"]!=null && dt.Rows[n]["Create_id"].ToString()!="")
					{
						model.Create_id=int.Parse(dt.Rows[n]["Create_id"].ToString());
					}
					if(dt.Rows[n]["Create_date"]!=null && dt.Rows[n]["Create_date"].ToString()!="")
					{
						model.Create_date=DateTime.Parse(dt.Rows[n]["Create_date"].ToString());
					}
					if(dt.Rows[n]["Update_id"]!=null && dt.Rows[n]["Update_id"].ToString()!="")
					{
						model.Update_id=int.Parse(dt.Rows[n]["Update_id"].ToString());
					}
					if(dt.Rows[n]["Update_date"]!=null && dt.Rows[n]["Update_date"].ToString()!="")
					{
						model.Update_date=DateTime.Parse(dt.Rows[n]["Update_date"].ToString());
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

