using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// Sys_Menu
	/// </summary>
	public partial class Sys_Menu
	{
		private readonly KfCrm.DAL.Sys_Menu dal=new KfCrm.DAL.Sys_Menu();
		public Sys_Menu()
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
		public bool Exists(int Menu_id)
		{
			return dal.Exists(Menu_id);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(KfCrm.Model.Sys_Menu model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.Sys_Menu model)
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
		public bool Delete(int Menu_id)
		{
			
			return dal.Delete(Menu_id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string Menu_idlist )
		{
			return dal.DeleteList(Menu_idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public KfCrm.Model.Sys_Menu GetModel(int Menu_id)
		{
			
			return dal.GetModel(Menu_id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public KfCrm.Model.Sys_Menu GetModelByCache(int Menu_id)
		{
			
			string CacheKey = "Sys_MenuModel-" + Menu_id;
			object objModel = KfCrm.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(Menu_id);
					if (objModel != null)
					{
						int ModelCache = KfCrm.Common.ConfigHelper.GetConfigInt("ModelCache");
						KfCrm.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (KfCrm.Model.Sys_Menu)objModel;
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
		public List<KfCrm.Model.Sys_Menu> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<KfCrm.Model.Sys_Menu> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.Sys_Menu> modelList = new List<KfCrm.Model.Sys_Menu>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.Sys_Menu model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.Sys_Menu();
					if(dt.Rows[n]["Menu_id"]!=null && dt.Rows[n]["Menu_id"].ToString()!="")
					{
						model.Menu_id=int.Parse(dt.Rows[n]["Menu_id"].ToString());
					}
					if(dt.Rows[n]["Menu_name"]!=null && dt.Rows[n]["Menu_name"].ToString()!="")
					{
					model.Menu_name=dt.Rows[n]["Menu_name"].ToString();
					}
					if(dt.Rows[n]["parentid"]!=null && dt.Rows[n]["parentid"].ToString()!="")
					{
						model.parentid=int.Parse(dt.Rows[n]["parentid"].ToString());
					}
					if(dt.Rows[n]["parentname"]!=null && dt.Rows[n]["parentname"].ToString()!="")
					{
					model.parentname=dt.Rows[n]["parentname"].ToString();
					}
					if(dt.Rows[n]["App_id"]!=null && dt.Rows[n]["App_id"].ToString()!="")
					{
						model.App_id=int.Parse(dt.Rows[n]["App_id"].ToString());
					}
					if(dt.Rows[n]["Menu_url"]!=null && dt.Rows[n]["Menu_url"].ToString()!="")
					{
					model.Menu_url=dt.Rows[n]["Menu_url"].ToString();
					}
					if(dt.Rows[n]["Menu_icon"]!=null && dt.Rows[n]["Menu_icon"].ToString()!="")
					{
					model.Menu_icon=dt.Rows[n]["Menu_icon"].ToString();
					}
					if(dt.Rows[n]["Menu_handler"]!=null && dt.Rows[n]["Menu_handler"].ToString()!="")
					{
					model.Menu_handler=dt.Rows[n]["Menu_handler"].ToString();
					}
					if(dt.Rows[n]["Menu_order"]!=null && dt.Rows[n]["Menu_order"].ToString()!="")
					{
						model.Menu_order=int.Parse(dt.Rows[n]["Menu_order"].ToString());
					}
					if(dt.Rows[n]["Menu_type"]!=null && dt.Rows[n]["Menu_type"].ToString()!="")
					{
					model.Menu_type=dt.Rows[n]["Menu_type"].ToString();
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

