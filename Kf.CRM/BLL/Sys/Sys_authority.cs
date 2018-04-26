using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// Sys_authority
	/// </summary>
	public partial class Sys_authority
	{
		private readonly KfCrm.DAL.Sys_authority dal=new KfCrm.DAL.Sys_authority();
		public Sys_authority()
		{}
		#region  Method

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(KfCrm.Model.Sys_authority model)
		{
			dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.Sys_authority model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.Delete();
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public KfCrm.Model.Sys_authority GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			return dal.GetModel();
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public KfCrm.Model.Sys_authority GetModelByCache()
		{
			//该表无主键信息，请自定义主键/条件字段
			string CacheKey = "Sys_authorityModel-" ;
			object objModel = KfCrm.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel();
					if (objModel != null)
					{
						int ModelCache = KfCrm.Common.ConfigHelper.GetConfigInt("ModelCache");
						KfCrm.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (KfCrm.Model.Sys_authority)objModel;
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
		public List<KfCrm.Model.Sys_authority> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<KfCrm.Model.Sys_authority> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.Sys_authority> modelList = new List<KfCrm.Model.Sys_authority>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.Sys_authority model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.Sys_authority();
					if(dt.Rows[n]["Role_id"]!=null && dt.Rows[n]["Role_id"].ToString()!="")
					{
						model.Role_id=int.Parse(dt.Rows[n]["Role_id"].ToString());
					}
					if(dt.Rows[n]["App_ids"]!=null && dt.Rows[n]["App_ids"].ToString()!="")
					{
					model.App_ids=dt.Rows[n]["App_ids"].ToString();
					}
					if(dt.Rows[n]["Menu_ids"]!=null && dt.Rows[n]["Menu_ids"].ToString()!="")
					{
					model.Menu_ids=dt.Rows[n]["Menu_ids"].ToString();
					}
					if(dt.Rows[n]["Button_ids"]!=null && dt.Rows[n]["Button_ids"].ToString()!="")
					{
					model.Button_ids=dt.Rows[n]["Button_ids"].ToString();
					}
					if(dt.Rows[n]["Create_id"]!=null && dt.Rows[n]["Create_id"].ToString()!="")
					{
						model.Create_id=int.Parse(dt.Rows[n]["Create_id"].ToString());
					}
					if(dt.Rows[n]["Create_date"]!=null && dt.Rows[n]["Create_date"].ToString()!="")
					{
						model.Create_date=DateTime.Parse(dt.Rows[n]["Create_date"].ToString());
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
        /// 删除一条数据
        /// </summary>
        public bool DeleteWhere(string wherestr)
        {
            //该表无主键信息，请自定义主键/条件字段
            return dal.DeleteWhere(wherestr);
        }
		#endregion  Method
	}
}

