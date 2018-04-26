using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:CRM_order
	/// </summary>
	public partial class CRM_order
	{
		public CRM_order()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "CRM_order"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from CRM_order");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.CRM_order model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into CRM_order(");
			strSql.Append("Serialnumber,Customer_id,Customer_name,Order_date,pay_type_id,pay_type,Order_details,Order_status_id,Order_status,Order_amount,create_id,create_date,C_dep_id,C_dep_name,C_emp_id,C_emp_name,F_dep_id,F_dep_name,F_emp_id,F_emp_name,receive_money,arrears_money,invoice_money,arrears_invoice,isDelete,Delete_time)");
			strSql.Append(" values (");
			strSql.Append("@Serialnumber,@Customer_id,@Customer_name,@Order_date,@pay_type_id,@pay_type,@Order_details,@Order_status_id,@Order_status,@Order_amount,@create_id,@create_date,@C_dep_id,@C_dep_name,@C_emp_id,@C_emp_name,@F_dep_id,@F_dep_name,@F_emp_id,@F_emp_name,@receive_money,@arrears_money,@invoice_money,@arrears_invoice,@isDelete,@Delete_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Serialnumber", SqlDbType.VarChar,250),
					new SqlParameter("@Customer_id", SqlDbType.Int,4),
					new SqlParameter("@Customer_name", SqlDbType.VarChar,250),
					new SqlParameter("@Order_date", SqlDbType.DateTime),
					new SqlParameter("@pay_type_id", SqlDbType.Int,4),
					new SqlParameter("@pay_type", SqlDbType.VarChar,250),
					new SqlParameter("@Order_details", SqlDbType.VarChar),
					new SqlParameter("@Order_status_id", SqlDbType.Int,4),
					new SqlParameter("@Order_status", SqlDbType.VarChar,250),
					new SqlParameter("@Order_amount", SqlDbType.Float,8),
					new SqlParameter("@create_id", SqlDbType.Int,4),
					new SqlParameter("@create_date", SqlDbType.DateTime),
					new SqlParameter("@C_dep_id", SqlDbType.Int,4),
					new SqlParameter("@C_dep_name", SqlDbType.VarChar,250),
					new SqlParameter("@C_emp_id", SqlDbType.Int,4),
					new SqlParameter("@C_emp_name", SqlDbType.VarChar,250),
					new SqlParameter("@F_dep_id", SqlDbType.Int,4),
					new SqlParameter("@F_dep_name", SqlDbType.VarChar,250),
					new SqlParameter("@F_emp_id", SqlDbType.Int,4),
					new SqlParameter("@F_emp_name", SqlDbType.VarChar,250),
					new SqlParameter("@receive_money", SqlDbType.Float,8),
					new SqlParameter("@arrears_money", SqlDbType.Float,8),
					new SqlParameter("@invoice_money", SqlDbType.Float,8),
					new SqlParameter("@arrears_invoice", SqlDbType.Float,8),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime)};
			parameters[0].Value = model.Serialnumber;
			parameters[1].Value = model.Customer_id;
			parameters[2].Value = model.Customer_name;
			parameters[3].Value = model.Order_date;
			parameters[4].Value = model.pay_type_id;
			parameters[5].Value = model.pay_type;
			parameters[6].Value = model.Order_details;
			parameters[7].Value = model.Order_status_id;
			parameters[8].Value = model.Order_status;
			parameters[9].Value = model.Order_amount;
			parameters[10].Value = model.create_id;
			parameters[11].Value = model.create_date;
			parameters[12].Value = model.C_dep_id;
			parameters[13].Value = model.C_dep_name;
			parameters[14].Value = model.C_emp_id;
			parameters[15].Value = model.C_emp_name;
			parameters[16].Value = model.F_dep_id;
			parameters[17].Value = model.F_dep_name;
			parameters[18].Value = model.F_emp_id;
			parameters[19].Value = model.F_emp_name;
			parameters[20].Value = model.receive_money;
			parameters[21].Value = model.arrears_money;
			parameters[22].Value = model.invoice_money;
			parameters[23].Value = model.arrears_invoice;
			parameters[24].Value = model.isDelete;
			parameters[25].Value = model.Delete_time;

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
		public bool Update(KfCrm.Model.CRM_order model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update CRM_order set ");
			strSql.Append("Customer_id=@Customer_id,");
			strSql.Append("Customer_name=@Customer_name,");
			strSql.Append("Order_date=@Order_date,");
			strSql.Append("pay_type_id=@pay_type_id,");
			strSql.Append("pay_type=@pay_type,");
			strSql.Append("Order_details=@Order_details,");
			strSql.Append("Order_status_id=@Order_status_id,");
			strSql.Append("Order_status=@Order_status,");
			strSql.Append("Order_amount=@Order_amount,");
			strSql.Append("C_dep_id=@C_dep_id,");
			strSql.Append("C_dep_name=@C_dep_name,");
			strSql.Append("C_emp_id=@C_emp_id,");
			strSql.Append("C_emp_name=@C_emp_name,");
			strSql.Append("F_dep_id=@F_dep_id,");
			strSql.Append("F_dep_name=@F_dep_name,");
			strSql.Append("F_emp_id=@F_emp_id,");
			strSql.Append("F_emp_name=@F_emp_name,");
			strSql.Append("receive_money=@receive_money,");
			strSql.Append("arrears_money=@arrears_money,");
			strSql.Append("invoice_money=@invoice_money,");
			strSql.Append("arrears_invoice=@arrears_invoice");

			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {	
					new SqlParameter("@Customer_id", SqlDbType.Int,4),
					new SqlParameter("@Customer_name", SqlDbType.VarChar,250),
					new SqlParameter("@Order_date", SqlDbType.DateTime),
					new SqlParameter("@pay_type_id", SqlDbType.Int,4),
					new SqlParameter("@pay_type", SqlDbType.VarChar,250),
					new SqlParameter("@Order_details", SqlDbType.VarChar),
					new SqlParameter("@Order_status_id", SqlDbType.Int,4),
					new SqlParameter("@Order_status", SqlDbType.VarChar,250),
					new SqlParameter("@Order_amount", SqlDbType.Float,8),					
					new SqlParameter("@C_dep_id", SqlDbType.Int,4),
					new SqlParameter("@C_dep_name", SqlDbType.VarChar,250),
					new SqlParameter("@C_emp_id", SqlDbType.Int,4),
					new SqlParameter("@C_emp_name", SqlDbType.VarChar,250),
					new SqlParameter("@F_dep_id", SqlDbType.Int,4),
					new SqlParameter("@F_dep_name", SqlDbType.VarChar,250),
					new SqlParameter("@F_emp_id", SqlDbType.Int,4),
					new SqlParameter("@F_emp_name", SqlDbType.VarChar,250),
					new SqlParameter("@receive_money", SqlDbType.Float,8),
					new SqlParameter("@arrears_money", SqlDbType.Float,8),
					new SqlParameter("@invoice_money", SqlDbType.Float,8),
					new SqlParameter("@arrears_invoice", SqlDbType.Float,8),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.Customer_id;
			parameters[1].Value = model.Customer_name;
			parameters[2].Value = model.Order_date;
			parameters[3].Value = model.pay_type_id;
			parameters[4].Value = model.pay_type;
			parameters[5].Value = model.Order_details;
			parameters[6].Value = model.Order_status_id;
			parameters[7].Value = model.Order_status;
			parameters[8].Value = model.Order_amount;			
			parameters[9].Value = model.C_dep_id;
			parameters[10].Value = model.C_dep_name;
			parameters[11].Value = model.C_emp_id;
			parameters[12].Value = model.C_emp_name;
			parameters[13].Value = model.F_dep_id;
			parameters[14].Value = model.F_dep_name;
			parameters[15].Value = model.F_emp_id;
			parameters[16].Value = model.F_emp_name;
			parameters[17].Value = model.receive_money;
			parameters[18].Value = model.arrears_money;
			parameters[19].Value = model.invoice_money;
			parameters[20].Value = model.arrears_invoice;
			parameters[21].Value = model.id;

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
        /// 批量
        /// </summary>
        public bool Update_batch(KfCrm.Model.CRM_order model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CRM_order set ");            
            strSql.Append("F_dep_id=@F_dep_id,");
            strSql.Append("F_dep_name=@F_dep_name,");
            strSql.Append("F_emp_id=@F_emp_id,");
            strSql.Append("F_emp_name=@F_emp_name");
            strSql.Append(" where F_emp_id=@create_id");
            SqlParameter[] parameters = {					
					new SqlParameter("@F_dep_id", SqlDbType.Int,4),
					new SqlParameter("@F_dep_name", SqlDbType.VarChar,250),
					new SqlParameter("@F_emp_id", SqlDbType.Int,4),
					new SqlParameter("@F_emp_name", SqlDbType.VarChar,250),		
					new SqlParameter("@create_id", SqlDbType.Int,4)};
            
            parameters[0].Value = model.F_dep_id;
            parameters[1].Value = model.F_dep_name;
            parameters[2].Value = model.F_emp_id;
            parameters[3].Value = model.F_emp_name;
            parameters[4].Value = model.create_id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
			strSql.Append("update CRM_order set ");
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
			strSql.Append("delete from CRM_order ");
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
			strSql.Append("delete from CRM_order ");
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
		public KfCrm.Model.CRM_order GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,Serialnumber,Customer_id,Customer_name,Order_date,pay_type_id,pay_type,Order_details,Order_status_id,Order_status,Order_amount,create_id,create_date,C_dep_id,C_dep_name,C_emp_id,C_emp_name,F_dep_id,F_dep_name,F_emp_id,F_emp_name,receive_money,arrears_money,invoice_money,arrears_invoice,isDelete,Delete_time from CRM_order ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.CRM_order model=new KfCrm.Model.CRM_order();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Serialnumber"]!=null && ds.Tables[0].Rows[0]["Serialnumber"].ToString()!="")
				{
					model.Serialnumber=ds.Tables[0].Rows[0]["Serialnumber"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Customer_id"]!=null && ds.Tables[0].Rows[0]["Customer_id"].ToString()!="")
				{
					model.Customer_id=int.Parse(ds.Tables[0].Rows[0]["Customer_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Customer_name"]!=null && ds.Tables[0].Rows[0]["Customer_name"].ToString()!="")
				{
					model.Customer_name=ds.Tables[0].Rows[0]["Customer_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Order_date"]!=null && ds.Tables[0].Rows[0]["Order_date"].ToString()!="")
				{
					model.Order_date=DateTime.Parse(ds.Tables[0].Rows[0]["Order_date"].ToString());
				}
				if(ds.Tables[0].Rows[0]["pay_type_id"]!=null && ds.Tables[0].Rows[0]["pay_type_id"].ToString()!="")
				{
					model.pay_type_id=int.Parse(ds.Tables[0].Rows[0]["pay_type_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["pay_type"]!=null && ds.Tables[0].Rows[0]["pay_type"].ToString()!="")
				{
					model.pay_type=ds.Tables[0].Rows[0]["pay_type"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Order_details"]!=null && ds.Tables[0].Rows[0]["Order_details"].ToString()!="")
				{
					model.Order_details=ds.Tables[0].Rows[0]["Order_details"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Order_status_id"]!=null && ds.Tables[0].Rows[0]["Order_status_id"].ToString()!="")
				{
					model.Order_status_id=int.Parse(ds.Tables[0].Rows[0]["Order_status_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Order_status"]!=null && ds.Tables[0].Rows[0]["Order_status"].ToString()!="")
				{
					model.Order_status=ds.Tables[0].Rows[0]["Order_status"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Order_amount"]!=null && ds.Tables[0].Rows[0]["Order_amount"].ToString()!="")
				{
					model.Order_amount=decimal.Parse(ds.Tables[0].Rows[0]["Order_amount"].ToString());
				}
				if(ds.Tables[0].Rows[0]["create_id"]!=null && ds.Tables[0].Rows[0]["create_id"].ToString()!="")
				{
					model.create_id=int.Parse(ds.Tables[0].Rows[0]["create_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["create_date"]!=null && ds.Tables[0].Rows[0]["create_date"].ToString()!="")
				{
					model.create_date=DateTime.Parse(ds.Tables[0].Rows[0]["create_date"].ToString());
				}
				if(ds.Tables[0].Rows[0]["C_dep_id"]!=null && ds.Tables[0].Rows[0]["C_dep_id"].ToString()!="")
				{
					model.C_dep_id=int.Parse(ds.Tables[0].Rows[0]["C_dep_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["C_dep_name"]!=null && ds.Tables[0].Rows[0]["C_dep_name"].ToString()!="")
				{
					model.C_dep_name=ds.Tables[0].Rows[0]["C_dep_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_emp_id"]!=null && ds.Tables[0].Rows[0]["C_emp_id"].ToString()!="")
				{
					model.C_emp_id=int.Parse(ds.Tables[0].Rows[0]["C_emp_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["C_emp_name"]!=null && ds.Tables[0].Rows[0]["C_emp_name"].ToString()!="")
				{
					model.C_emp_name=ds.Tables[0].Rows[0]["C_emp_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["F_dep_id"]!=null && ds.Tables[0].Rows[0]["F_dep_id"].ToString()!="")
				{
					model.F_dep_id=int.Parse(ds.Tables[0].Rows[0]["F_dep_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["F_dep_name"]!=null && ds.Tables[0].Rows[0]["F_dep_name"].ToString()!="")
				{
					model.F_dep_name=ds.Tables[0].Rows[0]["F_dep_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["F_emp_id"]!=null && ds.Tables[0].Rows[0]["F_emp_id"].ToString()!="")
				{
					model.F_emp_id=int.Parse(ds.Tables[0].Rows[0]["F_emp_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["F_emp_name"]!=null && ds.Tables[0].Rows[0]["F_emp_name"].ToString()!="")
				{
					model.F_emp_name=ds.Tables[0].Rows[0]["F_emp_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["receive_money"]!=null && ds.Tables[0].Rows[0]["receive_money"].ToString()!="")
				{
					model.receive_money=decimal.Parse(ds.Tables[0].Rows[0]["receive_money"].ToString());
				}
				if(ds.Tables[0].Rows[0]["arrears_money"]!=null && ds.Tables[0].Rows[0]["arrears_money"].ToString()!="")
				{
					model.arrears_money=decimal.Parse(ds.Tables[0].Rows[0]["arrears_money"].ToString());
				}
				if(ds.Tables[0].Rows[0]["invoice_money"]!=null && ds.Tables[0].Rows[0]["invoice_money"].ToString()!="")
				{
					model.invoice_money=decimal.Parse(ds.Tables[0].Rows[0]["invoice_money"].ToString());
				}
				if(ds.Tables[0].Rows[0]["arrears_invoice"]!=null && ds.Tables[0].Rows[0]["arrears_invoice"].ToString()!="")
				{
					model.arrears_invoice=decimal.Parse(ds.Tables[0].Rows[0]["arrears_invoice"].ToString());
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
			strSql.Append("select id,Serialnumber,Customer_id,Customer_name,Order_date,pay_type_id,pay_type,Order_details,Order_status_id,Order_status,Order_amount,create_id,create_date,C_dep_id,C_dep_name,C_emp_id,C_emp_name,F_dep_id,F_dep_name,F_emp_id,F_emp_name,receive_money,arrears_money,invoice_money,arrears_invoice,isDelete,Delete_time ");
			strSql.Append(" FROM CRM_order ");
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
			strSql.Append(" id,Serialnumber,Customer_id,Customer_name,Order_date,pay_type_id,pay_type,Order_details,Order_status_id,Order_status,Order_amount,create_id,create_date,C_dep_id,C_dep_name,C_emp_id,C_emp_name,F_dep_id,F_dep_name,F_emp_id,F_emp_name,receive_money,arrears_money,invoice_money,arrears_invoice,isDelete,Delete_time ");
			strSql.Append(" FROM CRM_order ");
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
			strSql.Append(" top " + PageSize + " * FROM CRM_order ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM CRM_order ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM CRM_order ");
			if (strWhere.Trim() != "")
			{
			    strSql.Append(" and " + strWhere);
			    strSql1.Append(" where " + strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			Total = DbHelperSQL.Query(strSql1.ToString()).Tables[0].Rows[0][0].ToString();
			return DbHelperSQL.Query(strSql.ToString());
		}
        /// <summary>
        /// 更新发票
        /// </summary>
        public bool UpdateInvoice(string orderid)
        {
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append(" /*更新发票*/ ");
            strSql1.Append(" UPDATE CRM_order SET ");
            strSql1.Append("     invoice_money=(SELECT SUM(ISNULL(invoice_amount,0)) AS Expr1 FROM CRM_invoice WHERE ( ISNULL(isDelete,0)=0 and  order_id='" + orderid + "'))  ");
            strSql1.Append(" WHERE (id='" + orderid + "') ");

            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append(" /*更新发票*/ ");
            strSql2.Append(" UPDATE CRM_order SET ");
            strSql2.Append("     arrears_invoice= ISNULL(Order_amount,0) - ISNULL(invoice_money,0)  ");
            strSql2.Append(" WHERE (id='" + orderid + "') ");

            int rows1 = DbHelperSQL.ExecuteSql(strSql1.ToString());
            int rows2 = DbHelperSQL.ExecuteSql(strSql2.ToString());

            if (rows1 > 0 && rows2 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 更新发票
        /// </summary>
        public bool UpdateReceive(string orderid)
        {
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append(" /*更新收款*/ ");
            strSql1.Append(" UPDATE CRM_order SET ");
            strSql1.Append("     receive_money=(SELECT SUM(ISNULL(Receive_amount,0)) AS Expr1 FROM CRM_receive WHERE ( ISNULL(isDelete,0)=0 and order_id='" + orderid + "'))  ");
            strSql1.Append(" WHERE (id='" + orderid + "') ");

            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append(" /*更新收款*/ ");
            strSql2.Append(" UPDATE CRM_order SET ");
            strSql2.Append("     arrears_money= ISNULL(Order_amount,0) - ISNULL(receive_money,0)  ");
            strSql2.Append(" WHERE (id='" + orderid + "') ");

            int rows1 = DbHelperSQL.ExecuteSql(strSql1.ToString());
            int rows2 = DbHelperSQL.ExecuteSql(strSql2.ToString());

            if (rows1 > 0 && rows2 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

		#endregion  Method
	}
}

