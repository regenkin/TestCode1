using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// CRM_product
	/// </summary>
	public partial class CRM_product
	{
		private readonly KfCrm.DAL.CRM_product dal=new KfCrm.DAL.CRM_product();
		public CRM_product()
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
		public bool Exists(int product_id)
		{
			return dal.Exists(product_id);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(KfCrm.Model.CRM_product model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.CRM_product model)
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
		public bool Delete(int product_id)
		{
			
			return dal.Delete(product_id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string product_idlist )
		{
			return dal.DeleteList(product_idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public KfCrm.Model.CRM_product GetModel(int product_id)
		{
			
			return dal.GetModel(product_id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public KfCrm.Model.CRM_product GetModelByCache(int product_id)
		{
			
			string CacheKey = "CRM_productModel-" + product_id;
			object objModel = KfCrm.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(product_id);
					if (objModel != null)
					{
						int ModelCache = KfCrm.Common.ConfigHelper.GetConfigInt("ModelCache");
						KfCrm.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (KfCrm.Model.CRM_product)objModel;
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
		public List<KfCrm.Model.CRM_product> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<KfCrm.Model.CRM_product> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.CRM_product> modelList = new List<KfCrm.Model.CRM_product>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.CRM_product model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.CRM_product();
					if(dt.Rows[n]["product_id"]!=null && dt.Rows[n]["product_id"].ToString()!="")
					{
						model.product_id=int.Parse(dt.Rows[n]["product_id"].ToString());
					}
					if(dt.Rows[n]["product_name"]!=null && dt.Rows[n]["product_name"].ToString()!="")
					{
					model.product_name=dt.Rows[n]["product_name"].ToString();
					}
					if(dt.Rows[n]["category_id"]!=null && dt.Rows[n]["category_id"].ToString()!="")
					{
						model.category_id=int.Parse(dt.Rows[n]["category_id"].ToString());
					}
					if(dt.Rows[n]["category_name"]!=null && dt.Rows[n]["category_name"].ToString()!="")
					{
					model.category_name=dt.Rows[n]["category_name"].ToString();
					}
					if(dt.Rows[n]["specifications"]!=null && dt.Rows[n]["specifications"].ToString()!="")
					{
					model.specifications=dt.Rows[n]["specifications"].ToString();
					}
					if(dt.Rows[n]["status"]!=null && dt.Rows[n]["status"].ToString()!="")
					{
					model.status=dt.Rows[n]["status"].ToString();
					}
					if(dt.Rows[n]["unit"]!=null && dt.Rows[n]["unit"].ToString()!="")
					{
					model.unit=dt.Rows[n]["unit"].ToString();
					}
					if(dt.Rows[n]["remarks"]!=null && dt.Rows[n]["remarks"].ToString()!="")
					{
					model.remarks=dt.Rows[n]["remarks"].ToString();
					}
					if(dt.Rows[n]["price"]!=null && dt.Rows[n]["price"].ToString()!="")
					{
						model.price=decimal.Parse(dt.Rows[n]["price"].ToString());
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

