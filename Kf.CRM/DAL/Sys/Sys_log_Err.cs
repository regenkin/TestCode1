using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Sys_log_Err
	/// </summary>
	public partial class Sys_log_Err
	{
		public Sys_log_Err()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "Sys_log_Err"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Sys_log_Err");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.Sys_log_Err model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sys_log_Err(");
			strSql.Append("Err_typeid,Err_type,Err_time,Err_url,Err_message,Err_source,Err_trace,Err_emp_id,Err_emp_name,Err_ip)");
			strSql.Append(" values (");
			strSql.Append("@Err_typeid,@Err_type,@Err_time,@Err_url,@Err_message,@Err_source,@Err_trace,@Err_emp_id,@Err_emp_name,@Err_ip)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Err_typeid", SqlDbType.Int,4),
					new SqlParameter("@Err_type", SqlDbType.VarChar,250),
					new SqlParameter("@Err_time", SqlDbType.DateTime),
					new SqlParameter("@Err_url", SqlDbType.VarChar,500),
					new SqlParameter("@Err_message", SqlDbType.VarChar),
					new SqlParameter("@Err_source", SqlDbType.VarChar,500),
					new SqlParameter("@Err_trace", SqlDbType.VarChar),
					new SqlParameter("@Err_emp_id", SqlDbType.Int,4),
					new SqlParameter("@Err_emp_name", SqlDbType.VarChar,250),
					new SqlParameter("@Err_ip", SqlDbType.VarChar,250)};
			parameters[0].Value = model.Err_typeid;
			parameters[1].Value = model.Err_type;
			parameters[2].Value = model.Err_time;
			parameters[3].Value = model.Err_url;
			parameters[4].Value = model.Err_message;
			parameters[5].Value = model.Err_source;
			parameters[6].Value = model.Err_trace;
			parameters[7].Value = model.Err_emp_id;
			parameters[8].Value = model.Err_emp_name;
			parameters[9].Value = model.Err_ip;

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
		public bool Update(KfCrm.Model.Sys_log_Err model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Sys_log_Err set ");
			strSql.Append("Err_typeid=@Err_typeid,");
			strSql.Append("Err_type=@Err_type,");
			strSql.Append("Err_time=@Err_time,");
			strSql.Append("Err_url=@Err_url,");
			strSql.Append("Err_message=@Err_message,");
			strSql.Append("Err_source=@Err_source,");
			strSql.Append("Err_trace=@Err_trace,");
			strSql.Append("Err_emp_id=@Err_emp_id,");
			strSql.Append("Err_emp_name=@Err_emp_name,");
			strSql.Append("Err_ip=@Err_ip");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@Err_typeid", SqlDbType.Int,4),
					new SqlParameter("@Err_type", SqlDbType.VarChar,250),
					new SqlParameter("@Err_time", SqlDbType.DateTime),
					new SqlParameter("@Err_url", SqlDbType.VarChar,500),
					new SqlParameter("@Err_message", SqlDbType.VarChar),
					new SqlParameter("@Err_source", SqlDbType.VarChar,500),
					new SqlParameter("@Err_trace", SqlDbType.VarChar),
					new SqlParameter("@Err_emp_id", SqlDbType.Int,4),
					new SqlParameter("@Err_emp_name", SqlDbType.VarChar,250),
					new SqlParameter("@Err_ip", SqlDbType.VarChar,250),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.Err_typeid;
			parameters[1].Value = model.Err_type;
			parameters[2].Value = model.Err_time;
			parameters[3].Value = model.Err_url;
			parameters[4].Value = model.Err_message;
			parameters[5].Value = model.Err_source;
			parameters[6].Value = model.Err_trace;
			parameters[7].Value = model.Err_emp_id;
			parameters[8].Value = model.Err_emp_name;
			parameters[9].Value = model.Err_ip;
			parameters[10].Value = model.id;

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
			strSql.Append("update Sys_log_Err set ");
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
			strSql.Append("delete from Sys_log_Err ");
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
			strSql.Append("delete from Sys_log_Err ");
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
		public KfCrm.Model.Sys_log_Err GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,Err_typeid,Err_type,Err_time,Err_url,Err_message,Err_source,Err_trace,Err_emp_id,Err_emp_name,Err_ip from Sys_log_Err ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.Sys_log_Err model=new KfCrm.Model.Sys_log_Err();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Err_typeid"]!=null && ds.Tables[0].Rows[0]["Err_typeid"].ToString()!="")
				{
					model.Err_typeid=int.Parse(ds.Tables[0].Rows[0]["Err_typeid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Err_type"]!=null && ds.Tables[0].Rows[0]["Err_type"].ToString()!="")
				{
					model.Err_type=ds.Tables[0].Rows[0]["Err_type"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Err_time"]!=null && ds.Tables[0].Rows[0]["Err_time"].ToString()!="")
				{
					model.Err_time=DateTime.Parse(ds.Tables[0].Rows[0]["Err_time"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Err_url"]!=null && ds.Tables[0].Rows[0]["Err_url"].ToString()!="")
				{
					model.Err_url=ds.Tables[0].Rows[0]["Err_url"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Err_message"]!=null && ds.Tables[0].Rows[0]["Err_message"].ToString()!="")
				{
					model.Err_message=ds.Tables[0].Rows[0]["Err_message"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Err_source"]!=null && ds.Tables[0].Rows[0]["Err_source"].ToString()!="")
				{
					model.Err_source=ds.Tables[0].Rows[0]["Err_source"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Err_trace"]!=null && ds.Tables[0].Rows[0]["Err_trace"].ToString()!="")
				{
					model.Err_trace=ds.Tables[0].Rows[0]["Err_trace"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Err_emp_id"]!=null && ds.Tables[0].Rows[0]["Err_emp_id"].ToString()!="")
				{
					model.Err_emp_id=int.Parse(ds.Tables[0].Rows[0]["Err_emp_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Err_emp_name"]!=null && ds.Tables[0].Rows[0]["Err_emp_name"].ToString()!="")
				{
					model.Err_emp_name=ds.Tables[0].Rows[0]["Err_emp_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Err_ip"]!=null && ds.Tables[0].Rows[0]["Err_ip"].ToString()!="")
				{
					model.Err_ip=ds.Tables[0].Rows[0]["Err_ip"].ToString();
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
			strSql.Append("select id,Err_typeid,Err_type,Err_time,Err_url,Err_message,Err_source,Err_trace,Err_emp_id,Err_emp_name,Err_ip ");
			strSql.Append(" FROM Sys_log_Err ");
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
			strSql.Append(" id,Err_typeid,Err_type,Err_time,Err_url,Err_message,Err_source,Err_trace,Err_emp_id,Err_emp_name,Err_ip ");
			strSql.Append(" FROM Sys_log_Err ");
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
            strSql1.Append(" select count(id) FROM Sys_log_Err ");
            strSql.Append("SELECT * FROM Sys_log_Err w1 ");
            strSql.Append("WHERE id in( ");
            strSql.Append(" SELECT TOP " + PageSize + " id FROM( ");
            strSql.Append("     SELECT TOP " + PageSize * PageIndex + " id FROM Sys_log_Err ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" WHERE " + strWhere);
                strSql1.Append(" WHERE " + strWhere);
            }
            strSql.Append("     ORDER BY id desc ");
            strSql.Append("	)w ORDER BY id asc) ");
            strSql.Append(" ORDER BY w1." + filedOrder);
            Total = DbHelperSQL.Query(strSql1.ToString()).Tables[0].Rows[0][0].ToString();
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetLogtype()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct Err_type FROM Sys_log_Err order by Err_type");

            return DbHelperSQL.Query(strSql.ToString());
        }
		#endregion  Method
	}
}

