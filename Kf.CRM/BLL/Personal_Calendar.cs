using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// Personal_Calendar
	/// </summary>
	public partial class Personal_Calendar
	{
		private readonly KfCrm.DAL.Personal_Calendar dal=new KfCrm.DAL.Personal_Calendar();
		public Personal_Calendar()
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
		public bool Exists(int Id)
		{
			return dal.Exists(Id);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int  Add(KfCrm.Model.Personal_Calendar model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public bool Update(KfCrm.Model.Personal_Calendar model)
		{
			return dal.Update(model);
		}

        public bool quickUpdate(KfCrm.Model.Personal_Calendar model)
        {
            return dal.quickUpdate(model);
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int Id)
		{
			
			return dal.Delete(Id);
		}
		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool DeleteList(string Idlist )
		{
			return dal.DeleteList(Idlist );
		}

		/// <summary>
		/// �õ�һ������ʵ��
		/// </summary>
		public KfCrm.Model.Personal_Calendar GetModel(int Id)
		{
			
			return dal.GetModel(Id);
		}

		/// <summary>
		/// �õ�һ������ʵ�壬�ӻ�����
		/// </summary>
		public KfCrm.Model.Personal_Calendar GetModelByCache(int Id)
		{
			
			string CacheKey = "Personal_CalendarModel-" + Id;
			object objModel = KfCrm.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Id);
					if (objModel != null)
					{
						int ModelCache = KfCrm.Common.ConfigHelper.GetConfigInt("ModelCache");
						KfCrm.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (KfCrm.Model.Personal_Calendar)objModel;
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
		public List<KfCrm.Model.Personal_Calendar> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// ��������б�
		/// </summary>
		public List<KfCrm.Model.Personal_Calendar> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.Personal_Calendar> modelList = new List<KfCrm.Model.Personal_Calendar>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.Personal_Calendar model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.Personal_Calendar();
					if(dt.Rows[n]["Id"].ToString()!="")
					{
						model.Id=int.Parse(dt.Rows[n]["Id"].ToString());
					}
					if(dt.Rows[n]["emp_id"].ToString()!="")
					{
						model.emp_id=int.Parse(dt.Rows[n]["emp_id"].ToString());
					}
					model.emp_name=dt.Rows[n]["emp_name"].ToString();
					if(dt.Rows[n]["companyid"].ToString()!="")
					{
						model.companyid=int.Parse(dt.Rows[n]["companyid"].ToString());
					}
					model.Subject=dt.Rows[n]["Subject"].ToString();
					model.Location=dt.Rows[n]["Location"].ToString();
					if(dt.Rows[n]["MasterId"].ToString()!="")
					{
						model.MasterId=int.Parse(dt.Rows[n]["MasterId"].ToString());
					}
					model.Description=dt.Rows[n]["Description"].ToString();
					if(dt.Rows[n]["CalendarType"].ToString()!="")
					{
						model.CalendarType=int.Parse(dt.Rows[n]["CalendarType"].ToString());
					}
					if(dt.Rows[n]["StartTime"].ToString()!="")
					{
						model.StartTime=DateTime.Parse(dt.Rows[n]["StartTime"].ToString());
					}
					if(dt.Rows[n]["EndTime"].ToString()!="")
					{
						model.EndTime=DateTime.Parse(dt.Rows[n]["EndTime"].ToString());
					}
					if(dt.Rows[n]["IsAllDayEvent"].ToString()!="")
					{
						if((dt.Rows[n]["IsAllDayEvent"].ToString()=="1")||(dt.Rows[n]["IsAllDayEvent"].ToString().ToLower()=="true"))
						{
						model.IsAllDayEvent=true;
						}
						else
						{
							model.IsAllDayEvent=false;
						}
					}
					if(dt.Rows[n]["HasAttachment"].ToString()!="")
					{
						if((dt.Rows[n]["HasAttachment"].ToString()=="1")||(dt.Rows[n]["HasAttachment"].ToString().ToLower()=="true"))
						{
						model.HasAttachment=true;
						}
						else
						{
							model.HasAttachment=false;
						}
					}
					model.Category=dt.Rows[n]["Category"].ToString();
					if(dt.Rows[n]["InstanceType"].ToString()!="")
					{
						model.InstanceType=int.Parse(dt.Rows[n]["InstanceType"].ToString());
					}
					model.Attendees=dt.Rows[n]["Attendees"].ToString();
					model.AttendeeNames=dt.Rows[n]["AttendeeNames"].ToString();
					model.OtherAttendee=dt.Rows[n]["OtherAttendee"].ToString();
					model.UPAccount=dt.Rows[n]["UPAccount"].ToString();
					model.UPName=dt.Rows[n]["UPName"].ToString();
					if(dt.Rows[n]["UPTime"].ToString()!="")
					{
						model.UPTime=DateTime.Parse(dt.Rows[n]["UPTime"].ToString());
					}
					model.RecurringRule=dt.Rows[n]["RecurringRule"].ToString();
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
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  Method
	}
}

