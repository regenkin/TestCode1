using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// CRM_Contact
	/// </summary>
	public partial class CRM_Contact
	{
		private readonly KfCrm.DAL.CRM_Contact dal=new KfCrm.DAL.CRM_Contact();
		public CRM_Contact()
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
		public int  Add(KfCrm.Model.CRM_Contact model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(KfCrm.Model.CRM_Contact model)
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
		public KfCrm.Model.CRM_Contact GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public KfCrm.Model.CRM_Contact GetModelByCache(int id)
		{
			
			string CacheKey = "CRM_ContactModel-" + id;
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
			return (KfCrm.Model.CRM_Contact)objModel;
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
		public List<KfCrm.Model.CRM_Contact> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<KfCrm.Model.CRM_Contact> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.CRM_Contact> modelList = new List<KfCrm.Model.CRM_Contact>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.CRM_Contact model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.CRM_Contact();
					if(dt.Rows[n]["id"]!=null && dt.Rows[n]["id"].ToString()!="")
					{
						model.id=int.Parse(dt.Rows[n]["id"].ToString());
					}
					if(dt.Rows[n]["C_name"]!=null && dt.Rows[n]["C_name"].ToString()!="")
					{
					model.C_name=dt.Rows[n]["C_name"].ToString();
					}
					if(dt.Rows[n]["C_sex"]!=null && dt.Rows[n]["C_sex"].ToString()!="")
					{
					model.C_sex=dt.Rows[n]["C_sex"].ToString();
					}
					if(dt.Rows[n]["C_department"]!=null && dt.Rows[n]["C_department"].ToString()!="")
					{
					model.C_department=dt.Rows[n]["C_department"].ToString();
					}
					if(dt.Rows[n]["C_position"]!=null && dt.Rows[n]["C_position"].ToString()!="")
					{
					model.C_position=dt.Rows[n]["C_position"].ToString();
					}
					if(dt.Rows[n]["C_birthday"]!=null && dt.Rows[n]["C_birthday"].ToString()!="")
					{
					model.C_birthday=dt.Rows[n]["C_birthday"].ToString();
					}
					if(dt.Rows[n]["C_tel"]!=null && dt.Rows[n]["C_tel"].ToString()!="")
					{
					model.C_tel=dt.Rows[n]["C_tel"].ToString();
					}
					if(dt.Rows[n]["C_fax"]!=null && dt.Rows[n]["C_fax"].ToString()!="")
					{
					model.C_fax=dt.Rows[n]["C_fax"].ToString();
					}
					if(dt.Rows[n]["C_email"]!=null && dt.Rows[n]["C_email"].ToString()!="")
					{
					model.C_email=dt.Rows[n]["C_email"].ToString();
					}
					if(dt.Rows[n]["C_mob"]!=null && dt.Rows[n]["C_mob"].ToString()!="")
					{
					model.C_mob=dt.Rows[n]["C_mob"].ToString();
					}
					if(dt.Rows[n]["C_QQ"]!=null && dt.Rows[n]["C_QQ"].ToString()!="")
					{
					model.C_QQ=dt.Rows[n]["C_QQ"].ToString();
					}
					if(dt.Rows[n]["C_add"]!=null && dt.Rows[n]["C_add"].ToString()!="")
					{
					model.C_add=dt.Rows[n]["C_add"].ToString();
					}
					if(dt.Rows[n]["C_hobby"]!=null && dt.Rows[n]["C_hobby"].ToString()!="")
					{
					model.C_hobby=dt.Rows[n]["C_hobby"].ToString();
					}
					if(dt.Rows[n]["C_remarks"]!=null && dt.Rows[n]["C_remarks"].ToString()!="")
					{
					model.C_remarks=dt.Rows[n]["C_remarks"].ToString();
					}
					if(dt.Rows[n]["C_customerid"]!=null && dt.Rows[n]["C_customerid"].ToString()!="")
					{
						model.C_customerid=int.Parse(dt.Rows[n]["C_customerid"].ToString());
					}
					if(dt.Rows[n]["C_customername"]!=null && dt.Rows[n]["C_customername"].ToString()!="")
					{
					model.C_customername=dt.Rows[n]["C_customername"].ToString();
					}
					if(dt.Rows[n]["C_createId"]!=null && dt.Rows[n]["C_createId"].ToString()!="")
					{
						model.C_createId=int.Parse(dt.Rows[n]["C_createId"].ToString());
					}
					if(dt.Rows[n]["C_createDate"]!=null && dt.Rows[n]["C_createDate"].ToString()!="")
					{
						model.C_createDate=DateTime.Parse(dt.Rows[n]["C_createDate"].ToString());
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

		#endregion  Method
	}
}

