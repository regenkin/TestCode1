using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// CRM_Follow
	/// </summary>
	public partial class CRM_Follow
	{
		private readonly KfCrm.DAL.CRM_Follow dal=new KfCrm.DAL.CRM_Follow();
		public CRM_Follow()
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
		public int  Add(KfCrm.Model.CRM_Follow model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(KfCrm.Model.CRM_Follow model)
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
		public KfCrm.Model.CRM_Follow GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public KfCrm.Model.CRM_Follow GetModelByCache(int id)
		{
			
			string CacheKey = "CRM_FollowModel-" + id;
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
			return (KfCrm.Model.CRM_Follow)objModel;
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
		public List<KfCrm.Model.CRM_Follow> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<KfCrm.Model.CRM_Follow> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.CRM_Follow> modelList = new List<KfCrm.Model.CRM_Follow>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.CRM_Follow model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.CRM_Follow();
					if(dt.Rows[n]["id"]!=null && dt.Rows[n]["id"].ToString()!="")
					{
						model.id=int.Parse(dt.Rows[n]["id"].ToString());
					}
					if(dt.Rows[n]["Customer_id"]!=null && dt.Rows[n]["Customer_id"].ToString()!="")
					{
						model.Customer_id=int.Parse(dt.Rows[n]["Customer_id"].ToString());
					}
					if(dt.Rows[n]["Customer_name"]!=null && dt.Rows[n]["Customer_name"].ToString()!="")
					{
					model.Customer_name=dt.Rows[n]["Customer_name"].ToString();
					}
					if(dt.Rows[n]["Follow"]!=null && dt.Rows[n]["Follow"].ToString()!="")
					{
					model.Follow=dt.Rows[n]["Follow"].ToString();
					}
					if(dt.Rows[n]["Follow_date"]!=null && dt.Rows[n]["Follow_date"].ToString()!="")
					{
						model.Follow_date=DateTime.Parse(dt.Rows[n]["Follow_date"].ToString());
					}
					if(dt.Rows[n]["Follow_Type_id"]!=null && dt.Rows[n]["Follow_Type_id"].ToString()!="")
					{
						model.Follow_Type_id=int.Parse(dt.Rows[n]["Follow_Type_id"].ToString());
					}
					if(dt.Rows[n]["Follow_Type"]!=null && dt.Rows[n]["Follow_Type"].ToString()!="")
					{
					model.Follow_Type=dt.Rows[n]["Follow_Type"].ToString();
					}
					if(dt.Rows[n]["department_id"]!=null && dt.Rows[n]["department_id"].ToString()!="")
					{
						model.department_id=int.Parse(dt.Rows[n]["department_id"].ToString());
					}
					if(dt.Rows[n]["department_name"]!=null && dt.Rows[n]["department_name"].ToString()!="")
					{
					model.department_name=dt.Rows[n]["department_name"].ToString();
					}
					if(dt.Rows[n]["employee_id"]!=null && dt.Rows[n]["employee_id"].ToString()!="")
					{
						model.employee_id=int.Parse(dt.Rows[n]["employee_id"].ToString());
					}
					if(dt.Rows[n]["employee_name"]!=null && dt.Rows[n]["employee_name"].ToString()!="")
					{
					model.employee_name=dt.Rows[n]["employee_name"].ToString();
					}
					if(dt.Rows[n]["isDelete"]!=null && dt.Rows[n]["isDelete"].ToString()!="")
					{
						model.isDelete=int.Parse(dt.Rows[n]["isDelete"].ToString());
					}
					if(dt.Rows[n]["Delete_time"]!=null && dt.Rows[n]["Delete_time"].ToString()!="")
					{
						model.Delete_time=DateTime.Parse(dt.Rows[n]["Delete_time"].ToString());
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
        /// ��ȱ���
        /// </summary>
        /// <param name="items"></param>
        /// <param name="year"></param>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataSet Reports_year(string items, int year, string where)
        {
            return dal.Reports_year(items, year, where);
        }

        /// <summary>
        /// ͬ�Ȼ���
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public DataSet Compared_follow(string year1, string month1, string year2, string month2)
        {
            return dal.Compared_follow(year1, month1, year2, month2);
        }
        /// <summary>
        /// Ա������
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public DataSet Compared_empcusfollow(string year1, string month1, string year2, string month2, string idlist)
        {
            return dal.Compared_empcusfollow( year1,  month1,  year2,  month2, idlist);
        }

        /// <summary>
        /// �ͻ�����ͳ��
        /// </summary>
        /// <param name="year"></param>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public DataSet report_empfollow(int year, string idlist)
        {
            return dal.report_empfollow(year, idlist);
        }

		#endregion  Method
	}
}

