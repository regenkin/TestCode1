using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// hr_post
	/// </summary>
	public partial class hr_post
	{
		private readonly KfCrm.DAL.hr_post dal=new KfCrm.DAL.hr_post();
		public hr_post()
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
		public bool Exists(int post_id)
		{
			return dal.Exists(post_id);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int  Add(KfCrm.Model.hr_post model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.hr_post model)
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
		public bool Delete(int post_id)
		{
			
			return dal.Delete(post_id);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string post_idlist )
		{
			return dal.DeleteList(post_idlist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public KfCrm.Model.hr_post GetModel(int post_id)
		{
			
			return dal.GetModel(post_id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public KfCrm.Model.hr_post GetModelByCache(int post_id)
		{
			
			string CacheKey = "hr_postModel-" + post_id;
			object objModel = KfCrm.Common.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(post_id);
					if (objModel != null)
					{
						int ModelCache = KfCrm.Common.ConfigHelper.GetConfigInt("ModelCache");
						KfCrm.Common.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (KfCrm.Model.hr_post)objModel;
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
		public List<KfCrm.Model.hr_post> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<KfCrm.Model.hr_post> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.hr_post> modelList = new List<KfCrm.Model.hr_post>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.hr_post model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.hr_post();
					if(dt.Rows[n]["post_id"]!=null && dt.Rows[n]["post_id"].ToString()!="")
					{
						model.post_id=int.Parse(dt.Rows[n]["post_id"].ToString());
					}
					if(dt.Rows[n]["post_name"]!=null && dt.Rows[n]["post_name"].ToString()!="")
					{
					model.post_name=dt.Rows[n]["post_name"].ToString();
					}
					if(dt.Rows[n]["position_id"]!=null && dt.Rows[n]["position_id"].ToString()!="")
					{
						model.position_id=int.Parse(dt.Rows[n]["position_id"].ToString());
					}
					if(dt.Rows[n]["position_name"]!=null && dt.Rows[n]["position_name"].ToString()!="")
					{
					model.position_name=dt.Rows[n]["position_name"].ToString();
					}
					if(dt.Rows[n]["position_order"]!=null && dt.Rows[n]["position_order"].ToString()!="")
					{
					model.position_order= int.Parse( dt.Rows[n]["position_order"].ToString());
					}
					if(dt.Rows[n]["dep_id"]!=null && dt.Rows[n]["dep_id"].ToString()!="")
					{
						model.dep_id=int.Parse(dt.Rows[n]["dep_id"].ToString());
					}
					if(dt.Rows[n]["depname"]!=null && dt.Rows[n]["depname"].ToString()!="")
					{
					model.depname=dt.Rows[n]["depname"].ToString();
					}
					if(dt.Rows[n]["emp_id"]!=null && dt.Rows[n]["emp_id"].ToString()!="")
					{
						model.emp_id=int.Parse(dt.Rows[n]["emp_id"].ToString());
					}
					if(dt.Rows[n]["emp_name"]!=null && dt.Rows[n]["emp_name"].ToString()!="")
					{
					model.emp_name=dt.Rows[n]["emp_name"].ToString();
					}
					if(dt.Rows[n]["default_post"]!=null && dt.Rows[n]["default_post"].ToString()!="")
					{
						model.default_post=int.Parse(dt.Rows[n]["default_post"].ToString());
					}
					if(dt.Rows[n]["note"]!=null && dt.Rows[n]["note"].ToString()!="")
					{
					model.note=dt.Rows[n]["note"].ToString();
					}
					if(dt.Rows[n]["post_descript"]!=null && dt.Rows[n]["post_descript"].ToString()!="")
					{
					model.post_descript=dt.Rows[n]["post_descript"].ToString();
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
        /// <summary>
        /// 更新岗位人员
        /// </summary>
        public bool UpdatePostEmp(KfCrm.Model.hr_post model)
        {
            return dal.UpdatePostEmp(model);
        }
        /// <summary>
        /// 清空更新岗位人员
        /// </summary>
        public bool UpdatePostEmpbyEid(int empid)
        {
            return dal.UpdatePostEmpbyEid(empid);
        }
		#endregion  Method
	}
}

