using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:public_notice
	/// </summary>
	public partial class public_notice
	{
		public public_notice()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "public_notice"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from public_notice");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.public_notice model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into public_notice(");
			strSql.Append("notice_title,notice_content,create_id,create_name,dep_id,dep_name,notice_time)");
			strSql.Append(" values (");
			strSql.Append("@notice_title,@notice_content,@create_id,@create_name,@dep_id,@dep_name,@notice_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@notice_title", SqlDbType.VarChar,250),
					new SqlParameter("@notice_content", SqlDbType.VarChar,4000),
					new SqlParameter("@create_id", SqlDbType.Int,4),
					new SqlParameter("@create_name", SqlDbType.VarChar,250),
					new SqlParameter("@dep_id", SqlDbType.Int,4),
					new SqlParameter("@dep_name", SqlDbType.VarChar,250),
					new SqlParameter("@notice_time", SqlDbType.DateTime)};
			parameters[0].Value = model.notice_title;
			parameters[1].Value = model.notice_content;
			parameters[2].Value = model.create_id;
			parameters[3].Value = model.create_name;
			parameters[4].Value = model.dep_id;
			parameters[5].Value = model.dep_name;
			parameters[6].Value = model.notice_time;

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
		public bool Update(KfCrm.Model.public_notice model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update public_notice set ");
			strSql.Append("notice_title=@notice_title,");
			strSql.Append("notice_content=@notice_content,");
			strSql.Append("create_id=@create_id,");
			strSql.Append("create_name=@create_name,");
			strSql.Append("dep_id=@dep_id,");
			strSql.Append("dep_name=@dep_name,");
			strSql.Append("notice_time=@notice_time");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@notice_title", SqlDbType.VarChar,250),
					new SqlParameter("@notice_content", SqlDbType.VarChar),
					new SqlParameter("@create_id", SqlDbType.Int,4),
					new SqlParameter("@create_name", SqlDbType.VarChar,250),
					new SqlParameter("@dep_id", SqlDbType.Int,4),
					new SqlParameter("@dep_name", SqlDbType.VarChar,250),
					new SqlParameter("@notice_time", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.notice_title;
			parameters[1].Value = model.notice_content;
			parameters[2].Value = model.create_id;
			parameters[3].Value = model.create_name;
			parameters[4].Value = model.dep_id;
			parameters[5].Value = model.dep_name;
			parameters[6].Value = model.notice_time;
			parameters[7].Value = model.id;

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
			strSql.Append("update public_notice set ");
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
			strSql.Append("delete from public_notice ");
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
			strSql.Append("delete from public_notice ");
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
		public KfCrm.Model.public_notice GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,notice_title,notice_content,create_id,create_name,dep_id,dep_name,notice_time from public_notice ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.public_notice model=new KfCrm.Model.public_notice();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["notice_title"]!=null && ds.Tables[0].Rows[0]["notice_title"].ToString()!="")
				{
					model.notice_title=ds.Tables[0].Rows[0]["notice_title"].ToString();
				}
				if(ds.Tables[0].Rows[0]["notice_content"]!=null && ds.Tables[0].Rows[0]["notice_content"].ToString()!="")
				{
					model.notice_content=ds.Tables[0].Rows[0]["notice_content"].ToString();
				}
				if(ds.Tables[0].Rows[0]["create_id"]!=null && ds.Tables[0].Rows[0]["create_id"].ToString()!="")
				{
					model.create_id=int.Parse(ds.Tables[0].Rows[0]["create_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["create_name"]!=null && ds.Tables[0].Rows[0]["create_name"].ToString()!="")
				{
					model.create_name=ds.Tables[0].Rows[0]["create_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["dep_id"]!=null && ds.Tables[0].Rows[0]["dep_id"].ToString()!="")
				{
					model.dep_id=int.Parse(ds.Tables[0].Rows[0]["dep_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["dep_name"]!=null && ds.Tables[0].Rows[0]["dep_name"].ToString()!="")
				{
					model.dep_name=ds.Tables[0].Rows[0]["dep_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["notice_time"]!=null && ds.Tables[0].Rows[0]["notice_time"].ToString()!="")
				{
					model.notice_time=DateTime.Parse(ds.Tables[0].Rows[0]["notice_time"].ToString());
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
			strSql.Append("select id,notice_title,notice_content,create_id,create_name,dep_id,dep_name,notice_time ");
			strSql.Append(" FROM public_notice ");
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
			strSql.Append(" id,notice_title,notice_content,create_id,create_name,dep_id,dep_name,notice_time ");
			strSql.Append(" FROM public_notice ");
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
			strSql.Append(" top " + PageSize + " * FROM public_notice ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM public_notice ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM public_notice ");
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

