using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:public_news
	/// </summary>
	public partial class public_news
	{
		public public_news()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "public_news"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from public_news");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.public_news model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into public_news(");
			strSql.Append("news_title,news_content,create_id,create_name,dep_id,dep_name,news_time,isDelete,Delete_time)");
			strSql.Append(" values (");
			strSql.Append("@news_title,@news_content,@create_id,@create_name,@dep_id,@dep_name,@news_time,@isDelete,@Delete_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@news_title", SqlDbType.VarChar,250),
					new SqlParameter("@news_content", SqlDbType.VarChar,4000),
					new SqlParameter("@create_id", SqlDbType.Int,4),
					new SqlParameter("@create_name", SqlDbType.VarChar,250),
					new SqlParameter("@dep_id", SqlDbType.Int,4),
					new SqlParameter("@dep_name", SqlDbType.VarChar,250),
					new SqlParameter("@news_time", SqlDbType.DateTime),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime)};
			parameters[0].Value = model.news_title;
			parameters[1].Value = model.news_content;
			parameters[2].Value = model.create_id;
			parameters[3].Value = model.create_name;
			parameters[4].Value = model.dep_id;
			parameters[5].Value = model.dep_name;
			parameters[6].Value = model.news_time;
			parameters[7].Value = model.isDelete;
			parameters[8].Value = model.Delete_time;

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
		public bool Update(KfCrm.Model.public_news model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update public_news set ");
			strSql.Append("news_title=@news_title,");
			strSql.Append("news_content=@news_content,");
			strSql.Append("create_id=@create_id,");
			strSql.Append("create_name=@create_name,");
			strSql.Append("dep_id=@dep_id,");
			strSql.Append("dep_name=@dep_name,");
			strSql.Append("news_time=@news_time,");
			strSql.Append("isDelete=@isDelete,");
			strSql.Append("Delete_time=@Delete_time");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@news_title", SqlDbType.VarChar,250),
					new SqlParameter("@news_content", SqlDbType.VarChar),
					new SqlParameter("@create_id", SqlDbType.Int,4),
					new SqlParameter("@create_name", SqlDbType.VarChar,250),
					new SqlParameter("@dep_id", SqlDbType.Int,4),
					new SqlParameter("@dep_name", SqlDbType.VarChar,250),
					new SqlParameter("@news_time", SqlDbType.DateTime),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.news_title;
			parameters[1].Value = model.news_content;
			parameters[2].Value = model.create_id;
			parameters[3].Value = model.create_name;
			parameters[4].Value = model.dep_id;
			parameters[5].Value = model.dep_name;
			parameters[6].Value = model.news_time;
			parameters[7].Value = model.isDelete;
			parameters[8].Value = model.Delete_time;
			parameters[9].Value = model.id;

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
			strSql.Append("update public_news set ");
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
			strSql.Append("delete from public_news ");
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
			strSql.Append("delete from public_news ");
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
		public KfCrm.Model.public_news GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,news_title,news_content,create_id,create_name,dep_id,dep_name,news_time,isDelete,Delete_time from public_news ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.public_news model=new KfCrm.Model.public_news();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["news_title"]!=null && ds.Tables[0].Rows[0]["news_title"].ToString()!="")
				{
					model.news_title=ds.Tables[0].Rows[0]["news_title"].ToString();
				}
				if(ds.Tables[0].Rows[0]["news_content"]!=null && ds.Tables[0].Rows[0]["news_content"].ToString()!="")
				{
					model.news_content=ds.Tables[0].Rows[0]["news_content"].ToString();
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
				if(ds.Tables[0].Rows[0]["news_time"]!=null && ds.Tables[0].Rows[0]["news_time"].ToString()!="")
				{
					model.news_time=DateTime.Parse(ds.Tables[0].Rows[0]["news_time"].ToString());
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
			strSql.Append("select id,news_title,news_content,create_id,create_name,dep_id,dep_name,news_time,isDelete,Delete_time ");
			strSql.Append(" FROM public_news ");
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
			strSql.Append(" id,news_title,news_content,create_id,create_name,dep_id,dep_name,news_time,isDelete,Delete_time ");
			strSql.Append(" FROM public_news ");
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
			strSql.Append(" top " + PageSize + " * FROM public_news ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM public_news ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM public_news ");
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

