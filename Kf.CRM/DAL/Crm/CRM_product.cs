using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:CRM_product
	/// </summary>
	public partial class CRM_product
	{
		public CRM_product()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("product_id", "CRM_product"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int product_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from CRM_product");
			strSql.Append(" where product_id=@product_id ");
			SqlParameter[] parameters = {
					new SqlParameter("@product_id", SqlDbType.Int,4)};
			parameters[0].Value = product_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.CRM_product model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into CRM_product(");
			strSql.Append("product_name,category_id,category_name,specifications,status,unit,remarks,price,isDelete,Delete_time)");
			strSql.Append(" values (");
			strSql.Append("@product_name,@category_id,@category_name,@specifications,@status,@unit,@remarks,@price,@isDelete,@Delete_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@product_name", SqlDbType.VarChar,250),
					new SqlParameter("@category_id", SqlDbType.Int,4),
					new SqlParameter("@category_name", SqlDbType.VarChar,250),
					new SqlParameter("@specifications", SqlDbType.VarChar,250),
					new SqlParameter("@status", SqlDbType.VarChar,250),
					new SqlParameter("@unit", SqlDbType.VarChar,250),
					new SqlParameter("@remarks", SqlDbType.VarChar),
					new SqlParameter("@price", SqlDbType.Float,8),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime)};
			parameters[0].Value = model.product_name;
			parameters[1].Value = model.category_id;
			parameters[2].Value = model.category_name;
			parameters[3].Value = model.specifications;
			parameters[4].Value = model.status;
			parameters[5].Value = model.unit;
			parameters[6].Value = model.remarks;
			parameters[7].Value = model.price;
			parameters[8].Value = model.isDelete;
			parameters[9].Value = model.Delete_time;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.CRM_product model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update CRM_product set ");
			strSql.Append("product_name=@product_name,");
			strSql.Append("category_id=@category_id,");
			strSql.Append("category_name=@category_name,");
			strSql.Append("specifications=@specifications,");
			strSql.Append("status=@status,");
			strSql.Append("unit=@unit,");
			strSql.Append("remarks=@remarks,");
			strSql.Append("price=@price");

			strSql.Append(" where product_id=@product_id");
			SqlParameter[] parameters = {
					new SqlParameter("@product_name", SqlDbType.VarChar,250),
					new SqlParameter("@category_id", SqlDbType.Int,4),
					new SqlParameter("@category_name", SqlDbType.VarChar,250),
					new SqlParameter("@specifications", SqlDbType.VarChar,250),
					new SqlParameter("@status", SqlDbType.VarChar,250),
					new SqlParameter("@unit", SqlDbType.VarChar,250),
					new SqlParameter("@remarks", SqlDbType.VarChar),
					new SqlParameter("@price", SqlDbType.Float,8), 
					new SqlParameter("@product_id", SqlDbType.Int,4)};
			parameters[0].Value = model.product_name;
			parameters[1].Value = model.category_id;
			parameters[2].Value = model.category_name;
			parameters[3].Value = model.specifications;
			parameters[4].Value = model.status;
			parameters[5].Value = model.unit;
			parameters[6].Value = model.remarks;
			parameters[7].Value = model.price;    
			parameters[8].Value = model.product_id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 预删除
		/// </summary>
		public bool AdvanceDelete(int id, int isDelete, string time)
		{
			StringBuilder strSql = new StringBuilder();
			strSql.Append("update CRM_product set ");
			strSql.Append("isDelete=" + isDelete);
			strSql.Append(",Delete_time='" + time + "'");
            strSql.Append(" where product_id=" + id);
			int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(int product_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from CRM_product ");
			strSql.Append(" where product_id=@product_id");
			SqlParameter[] parameters = {
					new SqlParameter("@product_id", SqlDbType.Int,4)
};
			parameters[0].Value = product_id;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string product_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from CRM_product ");
			strSql.Append(" where product_id in ("+product_idlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public KfCrm.Model.CRM_product GetModel(int product_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 product_id,product_name,category_id,category_name,specifications,status,unit,remarks,price,isDelete,Delete_time from CRM_product ");
			strSql.Append(" where product_id=@product_id");
			SqlParameter[] parameters = {
					new SqlParameter("@product_id", SqlDbType.Int,4)
};
			parameters[0].Value = product_id;

			KfCrm.Model.CRM_product model=new KfCrm.Model.CRM_product();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["product_id"]!=null && ds.Tables[0].Rows[0]["product_id"].ToString()!="")
				{
					model.product_id=int.Parse(ds.Tables[0].Rows[0]["product_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["product_name"]!=null && ds.Tables[0].Rows[0]["product_name"].ToString()!="")
				{
					model.product_name=ds.Tables[0].Rows[0]["product_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["category_id"]!=null && ds.Tables[0].Rows[0]["category_id"].ToString()!="")
				{
					model.category_id=int.Parse(ds.Tables[0].Rows[0]["category_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["category_name"]!=null && ds.Tables[0].Rows[0]["category_name"].ToString()!="")
				{
					model.category_name=ds.Tables[0].Rows[0]["category_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["specifications"]!=null && ds.Tables[0].Rows[0]["specifications"].ToString()!="")
				{
					model.specifications=ds.Tables[0].Rows[0]["specifications"].ToString();
				}
				if(ds.Tables[0].Rows[0]["status"]!=null && ds.Tables[0].Rows[0]["status"].ToString()!="")
				{
					model.status=ds.Tables[0].Rows[0]["status"].ToString();
				}
				if(ds.Tables[0].Rows[0]["unit"]!=null && ds.Tables[0].Rows[0]["unit"].ToString()!="")
				{
					model.unit=ds.Tables[0].Rows[0]["unit"].ToString();
				}
				if(ds.Tables[0].Rows[0]["remarks"]!=null && ds.Tables[0].Rows[0]["remarks"].ToString()!="")
				{
					model.remarks=ds.Tables[0].Rows[0]["remarks"].ToString();
				}
				if(ds.Tables[0].Rows[0]["price"]!=null && ds.Tables[0].Rows[0]["price"].ToString()!="")
				{
					model.price=decimal.Parse(ds.Tables[0].Rows[0]["price"].ToString());
				}
				if(ds.Tables[0].Rows[0]["isDelete"]!=null && ds.Tables[0].Rows[0]["isDelete"].ToString()!="")
				{
					model.isDelete=int.Parse(ds.Tables[0].Rows[0]["isDelete"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Delete_time"]!=null && ds.Tables[0].Rows[0]["Delete_time"].ToString()!="")
				{
					model.Delete_time=DateTime.Parse(ds.Tables[0].Rows[0]["Delete_time"].ToString());
				}
				return model;
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select product_id,product_name,category_id,category_name,specifications,status,unit,remarks,price,isDelete,Delete_time ");
			strSql.Append(" FROM CRM_product ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" product_id,product_name,category_id,category_name,specifications,status,unit,remarks,price,isDelete,Delete_time ");
			strSql.Append(" FROM CRM_product ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize, int PageIndex, string strWhere, string filedOrder, out string Total)
		{
			StringBuilder strSql = new StringBuilder();
			StringBuilder strSql1 = new StringBuilder();
			strSql.Append("select ");
			strSql.Append(" top " + PageSize + " * FROM CRM_product ");
            strSql.Append(" WHERE product_id not in ( SELECT top " + (PageIndex - 1) * PageSize + " product_id FROM CRM_product ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
            strSql1.Append(" select count(product_id) FROM CRM_product ");
			if (strWhere.Trim() != "")
			{
			    strSql.Append(" and " + strWhere);
			    strSql1.Append(" where " + strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			Total = DbHelperSQL.Query(strSql1.ToString()).Tables[0].Rows[0][0].ToString();
			return DbHelperSQL.Query(strSql.ToString());
		}

		#endregion  Method
	}
}

