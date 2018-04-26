using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// Sys_Button
	/// </summary>
	public partial class Sys_Button
	{
		private readonly KfCrm.DAL.Sys_Button dal=new KfCrm.DAL.Sys_Button();
		public Sys_Button()
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
		public bool Exists(int Btn_id)
		{
			return dal.Exists(Btn_id);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(KfCrm.Model.Sys_Button model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(KfCrm.Model.Sys_Button model)
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
		public bool Delete(int Btn_id)
		{
			
			return dal.Delete(Btn_id);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string Btn_idlist )
		{
			return dal.DeleteList(Btn_idlist );
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public KfCrm.Model.Sys_Button GetModel(int Btn_id)
		{
			
			return dal.GetModel(Btn_id);
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public KfCrm.Model.Sys_Button GetModelByCache(int Btn_id)
		{
			
			string CacheKey = "Sys_ButtonModel-" + Btn_id;
			object objModel = KfCrm.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Btn_id);
					if (objModel != null)
					{
						int ModelCache = KfCrm.Common.ConfigHelper.GetConfigInt("ModelCache");
						KfCrm.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (KfCrm.Model.Sys_Button)objModel;
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
		public List<KfCrm.Model.Sys_Button> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<KfCrm.Model.Sys_Button> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.Sys_Button> modelList = new List<KfCrm.Model.Sys_Button>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.Sys_Button model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.Sys_Button();
					if(dt.Rows[n]["Btn_id"]!=null && dt.Rows[n]["Btn_id"].ToString()!="")
					{
						model.Btn_id=int.Parse(dt.Rows[n]["Btn_id"].ToString());
					}
					if(dt.Rows[n]["Btn_name"]!=null && dt.Rows[n]["Btn_name"].ToString()!="")
					{
					model.Btn_name=dt.Rows[n]["Btn_name"].ToString();
					}
					if(dt.Rows[n]["Btn_icon"]!=null && dt.Rows[n]["Btn_icon"].ToString()!="")
					{
					model.Btn_icon=dt.Rows[n]["Btn_icon"].ToString();
					}
					if(dt.Rows[n]["Btn_handler"]!=null && dt.Rows[n]["Btn_handler"].ToString()!="")
					{
					model.Btn_handler=dt.Rows[n]["Btn_handler"].ToString();
					}
					if(dt.Rows[n]["Menu_id"]!=null && dt.Rows[n]["Menu_id"].ToString()!="")
					{
						model.Menu_id=int.Parse(dt.Rows[n]["Menu_id"].ToString());
					}
					if(dt.Rows[n]["Menu_name"]!=null && dt.Rows[n]["Menu_name"].ToString()!="")
					{
					model.Menu_name=dt.Rows[n]["Menu_name"].ToString();
					}
					if(dt.Rows[n]["Btn_order"]!=null && dt.Rows[n]["Btn_order"].ToString()!="")
					{
					model.Btn_order=dt.Rows[n]["Btn_order"].ToString();
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

