using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// Sys_role
	/// </summary>
	public partial class Sys_role
	{
		private readonly KfCrm.DAL.Sys_role dal=new KfCrm.DAL.Sys_role();
		public Sys_role()
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
		public bool Exists(int RoleID)
		{
			return dal.Exists(RoleID);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(KfCrm.Model.Sys_role model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(KfCrm.Model.Sys_role model)
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
		public bool Delete(int RoleID)
		{
			
			return dal.Delete(RoleID);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string RoleIDlist )
		{
			return dal.DeleteList(RoleIDlist );
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public KfCrm.Model.Sys_role GetModel(int RoleID)
		{
			
			return dal.GetModel(RoleID);
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public KfCrm.Model.Sys_role GetModelByCache(int RoleID)
		{
			
			string CacheKey = "Sys_roleModel-" + RoleID;
			object objModel = KfCrm.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(RoleID);
					if (objModel != null)
					{
						int ModelCache = KfCrm.Common.ConfigHelper.GetConfigInt("ModelCache");
						KfCrm.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (KfCrm.Model.Sys_role)objModel;
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
		public List<KfCrm.Model.Sys_role> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<KfCrm.Model.Sys_role> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.Sys_role> modelList = new List<KfCrm.Model.Sys_role>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.Sys_role model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.Sys_role();
					if(dt.Rows[n]["RoleID"]!=null && dt.Rows[n]["RoleID"].ToString()!="")
					{
						model.RoleID=int.Parse(dt.Rows[n]["RoleID"].ToString());
					}
					if(dt.Rows[n]["RoleName"]!=null && dt.Rows[n]["RoleName"].ToString()!="")
					{
					model.RoleName=dt.Rows[n]["RoleName"].ToString();
					}
					if(dt.Rows[n]["RoleDscript"]!=null && dt.Rows[n]["RoleDscript"].ToString()!="")
					{
					model.RoleDscript=dt.Rows[n]["RoleDscript"].ToString();
					}
					if(dt.Rows[n]["RoleSort"]!=null && dt.Rows[n]["RoleSort"].ToString()!="")
					{
					model.RoleSort= int.Parse( dt.Rows[n]["RoleSort"].ToString());
					}
					if(dt.Rows[n]["CreateID"]!=null && dt.Rows[n]["CreateID"].ToString()!="")
					{
						model.CreateID=int.Parse(dt.Rows[n]["CreateID"].ToString());
					}
					if(dt.Rows[n]["CreateDate"]!=null && dt.Rows[n]["CreateDate"].ToString()!="")
					{
						model.CreateDate=DateTime.Parse(dt.Rows[n]["CreateDate"].ToString());
					}
					if(dt.Rows[n]["UpdateID"]!=null && dt.Rows[n]["UpdateID"].ToString()!="")
					{
						model.UpdateID=int.Parse(dt.Rows[n]["UpdateID"].ToString());
					}
					if(dt.Rows[n]["UpdateDate"]!=null && dt.Rows[n]["UpdateDate"].ToString()!="")
					{
						model.UpdateDate=DateTime.Parse(dt.Rows[n]["UpdateDate"].ToString());
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

