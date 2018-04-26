using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// Sys_log_Err
	/// </summary>
	public partial class Sys_log_Err
	{
		private readonly KfCrm.DAL.Sys_log_Err dal=new KfCrm.DAL.Sys_log_Err();
		public Sys_log_Err()
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
		public int  Add(KfCrm.Model.Sys_log_Err model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.Sys_log_Err model)
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
		public KfCrm.Model.Sys_log_Err GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public KfCrm.Model.Sys_log_Err GetModelByCache(int id)
		{
			
			string CacheKey = "Sys_log_ErrModel-" + id;
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
			return (KfCrm.Model.Sys_log_Err)objModel;
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
		public List<KfCrm.Model.Sys_log_Err> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<KfCrm.Model.Sys_log_Err> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.Sys_log_Err> modelList = new List<KfCrm.Model.Sys_log_Err>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.Sys_log_Err model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.Sys_log_Err();
					if(dt.Rows[n]["id"]!=null && dt.Rows[n]["id"].ToString()!="")
					{
						model.id=int.Parse(dt.Rows[n]["id"].ToString());
					}
					if(dt.Rows[n]["Err_typeid"]!=null && dt.Rows[n]["Err_typeid"].ToString()!="")
					{
						model.Err_typeid=int.Parse(dt.Rows[n]["Err_typeid"].ToString());
					}
					if(dt.Rows[n]["Err_type"]!=null && dt.Rows[n]["Err_type"].ToString()!="")
					{
					model.Err_type=dt.Rows[n]["Err_type"].ToString();
					}
					if(dt.Rows[n]["Err_time"]!=null && dt.Rows[n]["Err_time"].ToString()!="")
					{
						model.Err_time=DateTime.Parse(dt.Rows[n]["Err_time"].ToString());
					}
					if(dt.Rows[n]["Err_url"]!=null && dt.Rows[n]["Err_url"].ToString()!="")
					{
					model.Err_url=dt.Rows[n]["Err_url"].ToString();
					}
					if(dt.Rows[n]["Err_message"]!=null && dt.Rows[n]["Err_message"].ToString()!="")
					{
					model.Err_message=dt.Rows[n]["Err_message"].ToString();
					}
					if(dt.Rows[n]["Err_source"]!=null && dt.Rows[n]["Err_source"].ToString()!="")
					{
					model.Err_source=dt.Rows[n]["Err_source"].ToString();
					}
					if(dt.Rows[n]["Err_trace"]!=null && dt.Rows[n]["Err_trace"].ToString()!="")
					{
					model.Err_trace=dt.Rows[n]["Err_trace"].ToString();
					}
					if(dt.Rows[n]["Err_emp_id"]!=null && dt.Rows[n]["Err_emp_id"].ToString()!="")
					{
						model.Err_emp_id=int.Parse(dt.Rows[n]["Err_emp_id"].ToString());
					}
					if(dt.Rows[n]["Err_emp_name"]!=null && dt.Rows[n]["Err_emp_name"].ToString()!="")
					{
					model.Err_emp_name=dt.Rows[n]["Err_emp_name"].ToString();
					}
					if(dt.Rows[n]["Err_ip"]!=null && dt.Rows[n]["Err_ip"].ToString()!="")
					{
					model.Err_ip=dt.Rows[n]["Err_ip"].ToString();
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

        /// <summary>
        /// 获取日志类型
        /// </summary>
        /// <returns></returns>
        public DataSet GetLogtype()
        {
            return dal.GetLogtype();
        }
		#endregion  Method
	}
}

