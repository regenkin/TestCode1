using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:CRM_invoice
	/// </summary>
	public partial class CRM_invoice
	{
		public CRM_invoice()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "CRM_invoice"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from CRM_invoice");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.CRM_invoice model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into CRM_invoice(");
			strSql.Append("Customer_id,Customer_name,invoice_num,invoice_type_id,invoice_type,invoice_amount,invoice_content,invoice_date,C_depid,C_depname,C_empid,C_empname,create_id,create_name,create_date,order_id,isDelete,Delete_time)");
			strSql.Append(" values (");
			strSql.Append("@Customer_id,@Customer_name,@invoice_num,@invoice_type_id,@invoice_type,@invoice_amount,@invoice_content,@invoice_date,@C_depid,@C_depname,@C_empid,@C_empname,@create_id,@create_name,@create_date,@order_id,@isDelete,@Delete_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Customer_id", SqlDbType.Int,4),
					new SqlParameter("@Customer_name", SqlDbType.VarChar,250),
					new SqlParameter("@invoice_num", SqlDbType.VarChar,250),
					new SqlParameter("@invoice_type_id", SqlDbType.Int,4),
					new SqlParameter("@invoice_type", SqlDbType.VarChar,250),
					new SqlParameter("@invoice_amount", SqlDbType.Float,8),
					new SqlParameter("@invoice_content", SqlDbType.VarChar),
					new SqlParameter("@invoice_date", SqlDbType.DateTime),
					new SqlParameter("@C_depid", SqlDbType.Int,4),
					new SqlParameter("@C_depname", SqlDbType.VarChar,250),
					new SqlParameter("@C_empid", SqlDbType.Int,4),
					new SqlParameter("@C_empname", SqlDbType.VarChar,250),
					new SqlParameter("@create_id", SqlDbType.Int,4),
					new SqlParameter("@create_name", SqlDbType.VarChar,250),
					new SqlParameter("@create_date", SqlDbType.DateTime),
					new SqlParameter("@order_id", SqlDbType.Int,4),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime)};
			parameters[0].Value = model.Customer_id;
			parameters[1].Value = model.Customer_name;
			parameters[2].Value = model.invoice_num;
			parameters[3].Value = model.invoice_type_id;
			parameters[4].Value = model.invoice_type;
			parameters[5].Value = model.invoice_amount;
			parameters[6].Value = model.invoice_content;
			parameters[7].Value = model.invoice_date;
			parameters[8].Value = model.C_depid;
			parameters[9].Value = model.C_depname;
			parameters[10].Value = model.C_empid;
			parameters[11].Value = model.C_empname;
			parameters[12].Value = model.create_id;
			parameters[13].Value = model.create_name;
			parameters[14].Value = model.create_date;
			parameters[15].Value = model.order_id;
			parameters[16].Value = model.isDelete;
			parameters[17].Value = model.Delete_time;

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
		public bool Update(KfCrm.Model.CRM_invoice model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update CRM_invoice set ");
			strSql.Append("Customer_id=@Customer_id,");
			strSql.Append("Customer_name=@Customer_name,");
			strSql.Append("invoice_num=@invoice_num,");
			strSql.Append("invoice_type_id=@invoice_type_id,");
			strSql.Append("invoice_type=@invoice_type,");
			strSql.Append("invoice_amount=@invoice_amount,");
			strSql.Append("invoice_content=@invoice_content,");
			strSql.Append("invoice_date=@invoice_date,");
			strSql.Append("C_depid=@C_depid,");
			strSql.Append("C_depname=@C_depname,");
			strSql.Append("C_empid=@C_empid,");
			strSql.Append("C_empname=@C_empname,");  
			strSql.Append("order_id=@order_id"); 
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@Customer_id", SqlDbType.Int,4),
					new SqlParameter("@Customer_name", SqlDbType.VarChar,250),
					new SqlParameter("@invoice_num", SqlDbType.VarChar,250),
					new SqlParameter("@invoice_type_id", SqlDbType.Int,4),
					new SqlParameter("@invoice_type", SqlDbType.VarChar,250),
					new SqlParameter("@invoice_amount", SqlDbType.Float,8),
					new SqlParameter("@invoice_content", SqlDbType.VarChar),
					new SqlParameter("@invoice_date", SqlDbType.DateTime),
					new SqlParameter("@C_depid", SqlDbType.Int,4),
					new SqlParameter("@C_depname", SqlDbType.VarChar,250),
					new SqlParameter("@C_empid", SqlDbType.Int,4),
					new SqlParameter("@C_empname", SqlDbType.VarChar,250), 
					new SqlParameter("@order_id", SqlDbType.Int,4),   
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.Customer_id;
			parameters[1].Value = model.Customer_name;
			parameters[2].Value = model.invoice_num;
			parameters[3].Value = model.invoice_type_id;
			parameters[4].Value = model.invoice_type;
			parameters[5].Value = model.invoice_amount;
			parameters[6].Value = model.invoice_content;
			parameters[7].Value = model.invoice_date;
			parameters[8].Value = model.C_depid;
			parameters[9].Value = model.C_depname;
			parameters[10].Value = model.C_empid;
			parameters[11].Value = model.C_empname; 		
			parameters[12].Value = model.order_id;  	
			parameters[13].Value = model.id;

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
			strSql.Append("update CRM_invoice set ");
			strSql.Append("isDelete=" + isDelete);
			strSql.Append(",Delete_time='" + time + "'");
			strSql.Append(" where id=" + id);
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
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from CRM_invoice ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

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
		public bool DeleteList(string idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from CRM_invoice ");
			strSql.Append(" where id in ("+idlist + ")  ");
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
		public KfCrm.Model.CRM_invoice GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,Customer_id,Customer_name,invoice_num,invoice_type_id,invoice_type,invoice_amount,invoice_content,invoice_date,C_depid,C_depname,C_empid,C_empname,create_id,create_name,create_date,order_id,isDelete,Delete_time from CRM_invoice ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.CRM_invoice model=new KfCrm.Model.CRM_invoice();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Customer_id"]!=null && ds.Tables[0].Rows[0]["Customer_id"].ToString()!="")
				{
					model.Customer_id=int.Parse(ds.Tables[0].Rows[0]["Customer_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Customer_name"]!=null && ds.Tables[0].Rows[0]["Customer_name"].ToString()!="")
				{
					model.Customer_name=ds.Tables[0].Rows[0]["Customer_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["invoice_num"]!=null && ds.Tables[0].Rows[0]["invoice_num"].ToString()!="")
				{
					model.invoice_num=ds.Tables[0].Rows[0]["invoice_num"].ToString();
				}
				if(ds.Tables[0].Rows[0]["invoice_type_id"]!=null && ds.Tables[0].Rows[0]["invoice_type_id"].ToString()!="")
				{
					model.invoice_type_id=int.Parse(ds.Tables[0].Rows[0]["invoice_type_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["invoice_type"]!=null && ds.Tables[0].Rows[0]["invoice_type"].ToString()!="")
				{
					model.invoice_type=ds.Tables[0].Rows[0]["invoice_type"].ToString();
				}
				if(ds.Tables[0].Rows[0]["invoice_amount"]!=null && ds.Tables[0].Rows[0]["invoice_amount"].ToString()!="")
				{
					model.invoice_amount=decimal.Parse(ds.Tables[0].Rows[0]["invoice_amount"].ToString());
				}
				if(ds.Tables[0].Rows[0]["invoice_content"]!=null && ds.Tables[0].Rows[0]["invoice_content"].ToString()!="")
				{
					model.invoice_content=ds.Tables[0].Rows[0]["invoice_content"].ToString();
				}
				if(ds.Tables[0].Rows[0]["invoice_date"]!=null && ds.Tables[0].Rows[0]["invoice_date"].ToString()!="")
				{
					model.invoice_date=DateTime.Parse(ds.Tables[0].Rows[0]["invoice_date"].ToString());
				}
				if(ds.Tables[0].Rows[0]["C_depid"]!=null && ds.Tables[0].Rows[0]["C_depid"].ToString()!="")
				{
					model.C_depid=int.Parse(ds.Tables[0].Rows[0]["C_depid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["C_depname"]!=null && ds.Tables[0].Rows[0]["C_depname"].ToString()!="")
				{
					model.C_depname=ds.Tables[0].Rows[0]["C_depname"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_empid"]!=null && ds.Tables[0].Rows[0]["C_empid"].ToString()!="")
				{
					model.C_empid=int.Parse(ds.Tables[0].Rows[0]["C_empid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["C_empname"]!=null && ds.Tables[0].Rows[0]["C_empname"].ToString()!="")
				{
					model.C_empname=ds.Tables[0].Rows[0]["C_empname"].ToString();
				}
				if(ds.Tables[0].Rows[0]["create_id"]!=null && ds.Tables[0].Rows[0]["create_id"].ToString()!="")
				{
					model.create_id=int.Parse(ds.Tables[0].Rows[0]["create_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["create_name"]!=null && ds.Tables[0].Rows[0]["create_name"].ToString()!="")
				{
					model.create_name=ds.Tables[0].Rows[0]["create_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["create_date"]!=null && ds.Tables[0].Rows[0]["create_date"].ToString()!="")
				{
					model.create_date=DateTime.Parse(ds.Tables[0].Rows[0]["create_date"].ToString());
				}
				if(ds.Tables[0].Rows[0]["order_id"]!=null && ds.Tables[0].Rows[0]["order_id"].ToString()!="")
				{
					model.order_id=int.Parse(ds.Tables[0].Rows[0]["order_id"].ToString());
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
			strSql.Append("select id,Customer_id,Customer_name,invoice_num,invoice_type_id,invoice_type,invoice_amount,invoice_content,invoice_date,C_depid,C_depname,C_empid,C_empname,create_id,create_name,create_date,order_id,isDelete,Delete_time ");
			strSql.Append(" FROM CRM_invoice ");
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
			strSql.Append(" id,Customer_id,Customer_name,invoice_num,invoice_type_id,invoice_type,invoice_amount,invoice_content,invoice_date,C_depid,C_depname,C_empid,C_empname,create_id,create_name,create_date,order_id,isDelete,Delete_time ");
			strSql.Append(" FROM CRM_invoice ");
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
			strSql.Append(" top " + PageSize + " * FROM CRM_invoice ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM CRM_invoice ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM CRM_invoice ");
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

