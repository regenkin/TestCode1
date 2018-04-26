using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// hr_department
	/// </summary>
	public partial class hr_department
	{
		private readonly KfCrm.DAL.hr_department dal=new KfCrm.DAL.hr_department();
		public hr_department()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
			return dal.GetMaxId();
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			return dal.Exists(id);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(KfCrm.Model.hr_department model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.hr_department model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 预删除
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
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			
			return dal.Delete(id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string idlist )
		{
			return dal.DeleteList(idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public KfCrm.Model.hr_department GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public KfCrm.Model.hr_department GetModelByCache(int id)
		{
			
			string CacheKey = "hr_departmentModel-" + id;
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
			return (KfCrm.Model.hr_department)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<KfCrm.Model.hr_department> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<KfCrm.Model.hr_department> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.hr_department> modelList = new List<KfCrm.Model.hr_department>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.hr_department model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.hr_department();
					if(dt.Rows[n]["id"]!=null && dt.Rows[n]["id"].ToString()!="")
					{
						model.id=int.Parse(dt.Rows[n]["id"].ToString());
					}
					if(dt.Rows[n]["d_name"]!=null && dt.Rows[n]["d_name"].ToString()!="")
					{
					model.d_name=dt.Rows[n]["d_name"].ToString();
					}
					if(dt.Rows[n]["parentid"]!=null && dt.Rows[n]["parentid"].ToString()!="")
					{
						model.parentid=int.Parse(dt.Rows[n]["parentid"].ToString());
					}
					if(dt.Rows[n]["parentname"]!=null && dt.Rows[n]["parentname"].ToString()!="")
					{
					model.parentname=dt.Rows[n]["parentname"].ToString();
					}
					if(dt.Rows[n]["d_type"]!=null && dt.Rows[n]["d_type"].ToString()!="")
					{
					model.d_type=dt.Rows[n]["d_type"].ToString();
					}
					if(dt.Rows[n]["d_icon"]!=null && dt.Rows[n]["d_icon"].ToString()!="")
					{
					model.d_icon=dt.Rows[n]["d_icon"].ToString();
					}
					if(dt.Rows[n]["d_fuzeren"]!=null && dt.Rows[n]["d_fuzeren"].ToString()!="")
					{
					model.d_fuzeren=dt.Rows[n]["d_fuzeren"].ToString();
					}
					if(dt.Rows[n]["d_tel"]!=null && dt.Rows[n]["d_tel"].ToString()!="")
					{
					model.d_tel=dt.Rows[n]["d_tel"].ToString();
					}
					if(dt.Rows[n]["d_fax"]!=null && dt.Rows[n]["d_fax"].ToString()!="")
					{
					model.d_fax=dt.Rows[n]["d_fax"].ToString();
					}
					if(dt.Rows[n]["d_add"]!=null && dt.Rows[n]["d_add"].ToString()!="")
					{
					model.d_add=dt.Rows[n]["d_add"].ToString();
					}
					if(dt.Rows[n]["d_email"]!=null && dt.Rows[n]["d_email"].ToString()!="")
					{
					model.d_email=dt.Rows[n]["d_email"].ToString();
					}
					if(dt.Rows[n]["d_miaoshu"]!=null && dt.Rows[n]["d_miaoshu"].ToString()!="")
					{
					model.d_miaoshu=dt.Rows[n]["d_miaoshu"].ToString();
					}
					if(dt.Rows[n]["d_order"]!=null && dt.Rows[n]["d_order"].ToString()!="")
					{
					model.d_order= int.Parse( dt.Rows[n]["d_order"].ToString());
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
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
		{
			return dal.GetList(PageSize, PageIndex, strWhere, filedOrder, out Total);
		}

		#endregion  Method
	}
}

