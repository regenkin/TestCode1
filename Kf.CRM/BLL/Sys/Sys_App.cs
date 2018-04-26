using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// Sys_App
	/// </summary>
	public partial class Sys_App
	{
		private readonly KfCrm.DAL.Sys_App dal=new KfCrm.DAL.Sys_App();
		public Sys_App()
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
		public int  Add(KfCrm.Model.Sys_App model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.Sys_App model)
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
		public KfCrm.Model.Sys_App GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public KfCrm.Model.Sys_App GetModelByCache(int id)
		{
			
			string CacheKey = "Sys_AppModel-" + id;
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
			return (KfCrm.Model.Sys_App)objModel;
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
		public List<KfCrm.Model.Sys_App> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<KfCrm.Model.Sys_App> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.Sys_App> modelList = new List<KfCrm.Model.Sys_App>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.Sys_App model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.Sys_App();
					if(dt.Rows[n]["id"]!=null && dt.Rows[n]["id"].ToString()!="")
					{
						model.id=int.Parse(dt.Rows[n]["id"].ToString());
					}
					if(dt.Rows[n]["App_name"]!=null && dt.Rows[n]["App_name"].ToString()!="")
					{
					model.App_name=dt.Rows[n]["App_name"].ToString();
					}
					if(dt.Rows[n]["App_order"]!=null && dt.Rows[n]["App_order"].ToString()!="")
					{
						model.App_order=int.Parse(dt.Rows[n]["App_order"].ToString());
					}
					if(dt.Rows[n]["App_url"]!=null && dt.Rows[n]["App_url"].ToString()!="")
					{
					model.App_url=dt.Rows[n]["App_url"].ToString();
					}
					if(dt.Rows[n]["App_handler"]!=null && dt.Rows[n]["App_handler"].ToString()!="")
					{
					model.App_handler=dt.Rows[n]["App_handler"].ToString();
					}
					if(dt.Rows[n]["App_type"]!=null && dt.Rows[n]["App_type"].ToString()!="")
					{
					model.App_type=dt.Rows[n]["App_type"].ToString();
					}
					if(dt.Rows[n]["App_icon"]!=null && dt.Rows[n]["App_icon"].ToString()!="")
					{
					model.App_icon=dt.Rows[n]["App_icon"].ToString();
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

