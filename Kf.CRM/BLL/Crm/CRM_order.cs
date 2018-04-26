using System;
using System.Data;
using System.Collections.Generic;
using KfCrm.Common;
using KfCrm.Model;
namespace KfCrm.BLL
{
	/// <summary>
	/// CRM_order
	/// </summary>
	public partial class CRM_order
	{
		private readonly KfCrm.DAL.CRM_order dal=new KfCrm.DAL.CRM_order();
		public CRM_order()
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
		public int  Add(KfCrm.Model.CRM_order model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.CRM_order model)
		{
			return dal.Update(model);
		}

        /// <summary>
        /// 批量
        /// </summary>
        public bool Update_batch(KfCrm.Model.CRM_order model)
        {
            return dal.Update_batch(model);
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
		public KfCrm.Model.CRM_order GetModel(int id)
		{
			
			return dal.GetModel(id);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public KfCrm.Model.CRM_order GetModelByCache(int id)
		{
			
			string CacheKey = "CRM_orderModel-" + id;
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
			return (KfCrm.Model.CRM_order)objModel;
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
		public List<KfCrm.Model.CRM_order> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<KfCrm.Model.CRM_order> DataTableToList(DataTable dt)
		{
			List<KfCrm.Model.CRM_order> modelList = new List<KfCrm.Model.CRM_order>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				KfCrm.Model.CRM_order model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = new KfCrm.Model.CRM_order();
					if(dt.Rows[n]["id"]!=null && dt.Rows[n]["id"].ToString()!="")
					{
						model.id=int.Parse(dt.Rows[n]["id"].ToString());
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
					if(dt.Rows[n]["Order_date"]!=null && dt.Rows[n]["Order_date"].ToString()!="")
					{
						model.Order_date=DateTime.Parse(dt.Rows[n]["Order_date"].ToString());
					}
					if(dt.Rows[n]["pay_type_id"]!=null && dt.Rows[n]["pay_type_id"].ToString()!="")
					{
						model.pay_type_id=int.Parse(dt.Rows[n]["pay_type_id"].ToString());
					}
					if(dt.Rows[n]["pay_type"]!=null && dt.Rows[n]["pay_type"].ToString()!="")
					{
					model.pay_type=dt.Rows[n]["pay_type"].ToString();
					}
					if(dt.Rows[n]["Order_details"]!=null && dt.Rows[n]["Order_details"].ToString()!="")
					{
					model.Order_details=dt.Rows[n]["Order_details"].ToString();
					}
					if(dt.Rows[n]["Order_status_id"]!=null && dt.Rows[n]["Order_status_id"].ToString()!="")
					{
						model.Order_status_id=int.Parse(dt.Rows[n]["Order_status_id"].ToString());
					}
					if(dt.Rows[n]["Order_status"]!=null && dt.Rows[n]["Order_status"].ToString()!="")
					{
					model.Order_status=dt.Rows[n]["Order_status"].ToString();
					}
					if(dt.Rows[n]["Order_amount"]!=null && dt.Rows[n]["Order_amount"].ToString()!="")
					{
						model.Order_amount=decimal.Parse(dt.Rows[n]["Order_amount"].ToString());
					}
					if(dt.Rows[n]["create_id"]!=null && dt.Rows[n]["create_id"].ToString()!="")
					{
						model.create_id=int.Parse(dt.Rows[n]["create_id"].ToString());
					}
					if(dt.Rows[n]["create_date"]!=null && dt.Rows[n]["create_date"].ToString()!="")
					{
						model.create_date=DateTime.Parse(dt.Rows[n]["create_date"].ToString());
					}
					if(dt.Rows[n]["C_dep_id"]!=null && dt.Rows[n]["C_dep_id"].ToString()!="")
					{
						model.C_dep_id=int.Parse(dt.Rows[n]["C_dep_id"].ToString());
					}
					if(dt.Rows[n]["C_dep_name"]!=null && dt.Rows[n]["C_dep_name"].ToString()!="")
					{
					model.C_dep_name=dt.Rows[n]["C_dep_name"].ToString();
					}
					if(dt.Rows[n]["C_emp_id"]!=null && dt.Rows[n]["C_emp_id"].ToString()!="")
					{
						model.C_emp_id=int.Parse(dt.Rows[n]["C_emp_id"].ToString());
					}
					if(dt.Rows[n]["C_emp_name"]!=null && dt.Rows[n]["C_emp_name"].ToString()!="")
					{
					model.C_emp_name=dt.Rows[n]["C_emp_name"].ToString();
					}
					if(dt.Rows[n]["F_dep_id"]!=null && dt.Rows[n]["F_dep_id"].ToString()!="")
					{
						model.F_dep_id=int.Parse(dt.Rows[n]["F_dep_id"].ToString());
					}
					if(dt.Rows[n]["F_dep_name"]!=null && dt.Rows[n]["F_dep_name"].ToString()!="")
					{
					model.F_dep_name=dt.Rows[n]["F_dep_name"].ToString();
					}
					if(dt.Rows[n]["F_emp_id"]!=null && dt.Rows[n]["F_emp_id"].ToString()!="")
					{
						model.F_emp_id=int.Parse(dt.Rows[n]["F_emp_id"].ToString());
					}
					if(dt.Rows[n]["F_emp_name"]!=null && dt.Rows[n]["F_emp_name"].ToString()!="")
					{
					model.F_emp_name=dt.Rows[n]["F_emp_name"].ToString();
					}
					if(dt.Rows[n]["receive_money"]!=null && dt.Rows[n]["receive_money"].ToString()!="")
					{
						model.receive_money=decimal.Parse(dt.Rows[n]["receive_money"].ToString());
					}
					if(dt.Rows[n]["arrears_money"]!=null && dt.Rows[n]["arrears_money"].ToString()!="")
					{
						model.arrears_money=decimal.Parse(dt.Rows[n]["arrears_money"].ToString());
					}
					if(dt.Rows[n]["invoice_money"]!=null && dt.Rows[n]["invoice_money"].ToString()!="")
					{
						model.invoice_money=decimal.Parse(dt.Rows[n]["invoice_money"].ToString());
					}
					if(dt.Rows[n]["arrears_invoice"]!=null && dt.Rows[n]["arrears_invoice"].ToString()!="")
					{
						model.arrears_invoice=decimal.Parse(dt.Rows[n]["arrears_invoice"].ToString());
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
        /// 更新发票金额
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public bool UpdateInvoice(string orderid)
        {
            return dal.UpdateInvoice(orderid);
        }

        /// <summary>
        /// 更新收款金额
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public bool UpdateReceive(string orderid)
        {
            return dal.UpdateReceive(orderid);
        }
		#endregion  Method
	}
}

