using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// CRM_contract
	/// </summary>
	public partial class CRM_contract
	{
		private readonly KfCrm.DAL.CRM_contract dal=new KfCrm.DAL.CRM_contract();
		public CRM_contract()
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
		public int  Add(KfCrm.Model.CRM_contract model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.CRM_contract model)
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
		public KfCrm.Model.CRM_contract GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public KfCrm.Model.CRM_contract GetModelByCache(int id)
		{
			
			string CacheKey = "CRM_contractModel-" + id;
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
			return (KfCrm.Model.CRM_contract)objModel;
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
		public List<KfCrm.Model.CRM_contract> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<KfCrm.Model.CRM_contract> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.CRM_contract> modelList = new List<KfCrm.Model.CRM_contract>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.CRM_contract model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.CRM_contract();
					if(dt.Rows[n]["id"]!=null && dt.Rows[n]["id"].ToString()!="")
					{
						model.id=int.Parse(dt.Rows[n]["id"].ToString());
					}
					if(dt.Rows[n]["Contract_name"]!=null && dt.Rows[n]["Contract_name"].ToString()!="")
					{
					model.Contract_name=dt.Rows[n]["Contract_name"].ToString();
					}
					if(dt.Rows[n]["Serialnumber"]!=null && dt.Rows[n]["Serialnumber"].ToString()!="")
					{
					model.Serialnumber=dt.Rows[n]["Serialnumber"].ToString();
					}
					if(dt.Rows[n]["Customer_id"]!=null && dt.Rows[n]["Customer_id"].ToString()!="")
					{
						model.Customer_id=int.Parse(dt.Rows[n]["Customer_id"].ToString());
					}
					if(dt.Rows[n]["Customer_name"]!=null && dt.Rows[n]["Customer_name"].ToString()!="")
					{
					model.Customer_name=dt.Rows[n]["Customer_name"].ToString();
					}
					if(dt.Rows[n]["C_depid"]!=null && dt.Rows[n]["C_depid"].ToString()!="")
					{
						model.C_depid=int.Parse(dt.Rows[n]["C_depid"].ToString());
					}
					if(dt.Rows[n]["C_depname"]!=null && dt.Rows[n]["C_depname"].ToString()!="")
					{
					model.C_depname=dt.Rows[n]["C_depname"].ToString();
					}
					if(dt.Rows[n]["C_empid"]!=null && dt.Rows[n]["C_empid"].ToString()!="")
					{
						model.C_empid=int.Parse(dt.Rows[n]["C_empid"].ToString());
					}
					if(dt.Rows[n]["C_empname"]!=null && dt.Rows[n]["C_empname"].ToString()!="")
					{
					model.C_empname=dt.Rows[n]["C_empname"].ToString();
					}
					if(dt.Rows[n]["Contract_amount"]!=null && dt.Rows[n]["Contract_amount"].ToString()!="")
					{
						model.Contract_amount=decimal.Parse(dt.Rows[n]["Contract_amount"].ToString());
					}
					if(dt.Rows[n]["Pay_cycle"]!=null && dt.Rows[n]["Pay_cycle"].ToString()!="")
					{
						model.Pay_cycle=int.Parse(dt.Rows[n]["Pay_cycle"].ToString());
					}
					if(dt.Rows[n]["Start_date"]!=null && dt.Rows[n]["Start_date"].ToString()!="")
					{
					model.Start_date=dt.Rows[n]["Start_date"].ToString();
					}
					if(dt.Rows[n]["End_date"]!=null && dt.Rows[n]["End_date"].ToString()!="")
					{
					model.End_date=dt.Rows[n]["End_date"].ToString();
					}
					if(dt.Rows[n]["Sign_date"]!=null && dt.Rows[n]["Sign_date"].ToString()!="")
					{
					model.Sign_date=dt.Rows[n]["Sign_date"].ToString();
					}
					if(dt.Rows[n]["Customer_Contractor"]!=null && dt.Rows[n]["Customer_Contractor"].ToString()!="")
					{
					model.Customer_Contractor=dt.Rows[n]["Customer_Contractor"].ToString();
					}
					if(dt.Rows[n]["Our_Contractor_depid"]!=null && dt.Rows[n]["Our_Contractor_depid"].ToString()!="")
					{
						model.Our_Contractor_depid=int.Parse(dt.Rows[n]["Our_Contractor_depid"].ToString());
					}
					if(dt.Rows[n]["Our_Contractor_depname"]!=null && dt.Rows[n]["Our_Contractor_depname"].ToString()!="")
					{
					model.Our_Contractor_depname=dt.Rows[n]["Our_Contractor_depname"].ToString();
					}
					if(dt.Rows[n]["Our_Contractor_id"]!=null && dt.Rows[n]["Our_Contractor_id"].ToString()!="")
					{
						model.Our_Contractor_id=int.Parse(dt.Rows[n]["Our_Contractor_id"].ToString());
					}
					if(dt.Rows[n]["Our_Contractor_name"]!=null && dt.Rows[n]["Our_Contractor_name"].ToString()!="")
					{
					model.Our_Contractor_name=dt.Rows[n]["Our_Contractor_name"].ToString();
					}
					if(dt.Rows[n]["Creater_id"]!=null && dt.Rows[n]["Creater_id"].ToString()!="")
					{
						model.Creater_id=int.Parse(dt.Rows[n]["Creater_id"].ToString());
					}
					if(dt.Rows[n]["Creater_name"]!=null && dt.Rows[n]["Creater_name"].ToString()!="")
					{
					model.Creater_name=dt.Rows[n]["Creater_name"].ToString();
					}
					if(dt.Rows[n]["Create_time"]!=null && dt.Rows[n]["Create_time"].ToString()!="")
					{
						model.Create_time=DateTime.Parse(dt.Rows[n]["Create_time"].ToString());
					}
					if(dt.Rows[n]["Main_Content"]!=null && dt.Rows[n]["Main_Content"].ToString()!="")
					{
					model.Main_Content=dt.Rows[n]["Main_Content"].ToString();
					}
					if(dt.Rows[n]["Remarks"]!=null && dt.Rows[n]["Remarks"].ToString()!="")
					{
					model.Remarks=dt.Rows[n]["Remarks"].ToString();
					}
					if(dt.Rows[n]["File_serialnumber"]!=null && dt.Rows[n]["File_serialnumber"].ToString()!="")
					{
					model.File_serialnumber=dt.Rows[n]["File_serialnumber"].ToString();
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
        /// 同比环比
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public DataSet Compared_empcuscontract(string year1, string month1, string year2, string month2, string idlist)
        {
            return dal.Compared_empcuscontract( year1,  month1,  year2,  month2,  idlist);
        }

        /// <summary>
        /// 客户成交统计
        /// </summary>
        /// <param name="year"></param>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public DataSet report_empcontract(int year, string idlist)
        {
            return dal.report_empcontract(year, idlist);
        }

		#endregion  Method
	}
}

