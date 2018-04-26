using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Sys_log
	/// </summary>
	public partial class Sys_log
	{
		public Sys_log()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "Sys_log"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Sys_log");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.Sys_log model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sys_log(");
			strSql.Append("EventType,EventID,EventTitle,Original_txt,Current_txt,UserID,UserName,IPStreet,EventDate)");
			strSql.Append(" values (");
			strSql.Append("@EventType,@EventID,@EventTitle,@Original_txt,@Current_txt,@UserID,@UserName,@IPStreet,@EventDate)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@EventType", SqlDbType.VarChar,250),
					new SqlParameter("@EventID", SqlDbType.VarChar,50),
					new SqlParameter("@EventTitle", SqlDbType.VarChar,250),
					new SqlParameter("@Original_txt", SqlDbType.VarChar,4000),
					new SqlParameter("@Current_txt", SqlDbType.VarChar,4000),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@IPStreet", SqlDbType.VarChar,50),
					new SqlParameter("@EventDate", SqlDbType.DateTime)};
			parameters[0].Value = model.EventType;
			parameters[1].Value = model.EventID;
			parameters[2].Value = model.EventTitle;
			parameters[3].Value = model.Original_txt;
			parameters[4].Value = model.Current_txt;
			parameters[5].Value = model.UserID;
			parameters[6].Value = model.UserName;
			parameters[7].Value = model.IPStreet;
			parameters[8].Value = model.EventDate;

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
        /// 获得数据列表
        /// </summary>
        public DataSet GetLogtype()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select distinct EventType FROM Sys_log order by EventType");

            return DbHelperSQL.Query(strSql.ToString());
        }


		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,EventType,EventID,EventTitle,Original_txt,Current_txt,UserID,UserName,IPStreet,EventDate ");
			strSql.Append(" FROM Sys_log ");
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
			strSql.Append(" id,EventType,EventID,EventTitle,Original_txt,Current_txt,UserID,UserName,IPStreet,EventDate ");
			strSql.Append(" FROM Sys_log ");
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
			strSql.Append(" top " + PageSize + " * FROM Sys_log ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM Sys_log ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM Sys_log ");
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

