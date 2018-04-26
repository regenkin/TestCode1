using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Sys_App
	/// </summary>
	public partial class Sys_App
	{
		public Sys_App()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "Sys_App"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Sys_App");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.Sys_App model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sys_App(");
			strSql.Append("App_name,App_order,App_url,App_handler,App_type,App_icon)");
			strSql.Append(" values (");
			strSql.Append("@App_name,@App_order,@App_url,@App_handler,@App_type,@App_icon)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@App_name", SqlDbType.VarChar,100),
					new SqlParameter("@App_order", SqlDbType.Int,4),
					new SqlParameter("@App_url", SqlDbType.VarChar,250),
					new SqlParameter("@App_handler", SqlDbType.VarChar,250),
					new SqlParameter("@App_type", SqlDbType.VarChar,50),
					new SqlParameter("@App_icon", SqlDbType.VarChar,250)};
			parameters[0].Value = model.App_name;
			parameters[1].Value = model.App_order;
			parameters[2].Value = model.App_url;
			parameters[3].Value = model.App_handler;
			parameters[4].Value = model.App_type;
			parameters[5].Value = model.App_icon;

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
		public bool Update(KfCrm.Model.Sys_App model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Sys_App set ");
			strSql.Append("App_name=@App_name,");
			strSql.Append("App_order=@App_order,");
			strSql.Append("App_url=@App_url,");
			strSql.Append("App_handler=@App_handler,");
			strSql.Append("App_type=@App_type,");
			strSql.Append("App_icon=@App_icon");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@App_name", SqlDbType.VarChar,100),
					new SqlParameter("@App_order", SqlDbType.Int,4),
					new SqlParameter("@App_url", SqlDbType.VarChar,250),
					new SqlParameter("@App_handler", SqlDbType.VarChar,250),
					new SqlParameter("@App_type", SqlDbType.VarChar,50),
					new SqlParameter("@App_icon", SqlDbType.VarChar,250),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.App_name;
			parameters[1].Value = model.App_order;
			parameters[2].Value = model.App_url;
			parameters[3].Value = model.App_handler;
			parameters[4].Value = model.App_type;
			parameters[5].Value = model.App_icon;
			parameters[6].Value = model.id;

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
			strSql.Append("update Sys_App set ");
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
			strSql.Append("delete from Sys_App ");
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
			strSql.Append("delete from Sys_App ");
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
		public KfCrm.Model.Sys_App GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,App_name,App_order,App_url,App_handler,App_type,App_icon from Sys_App ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.Sys_App model=new KfCrm.Model.Sys_App();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["App_name"]!=null && ds.Tables[0].Rows[0]["App_name"].ToString()!="")
				{
					model.App_name=ds.Tables[0].Rows[0]["App_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["App_order"]!=null && ds.Tables[0].Rows[0]["App_order"].ToString()!="")
				{
					model.App_order=int.Parse(ds.Tables[0].Rows[0]["App_order"].ToString());
				}
				if(ds.Tables[0].Rows[0]["App_url"]!=null && ds.Tables[0].Rows[0]["App_url"].ToString()!="")
				{
					model.App_url=ds.Tables[0].Rows[0]["App_url"].ToString();
				}
				if(ds.Tables[0].Rows[0]["App_handler"]!=null && ds.Tables[0].Rows[0]["App_handler"].ToString()!="")
				{
					model.App_handler=ds.Tables[0].Rows[0]["App_handler"].ToString();
				}
				if(ds.Tables[0].Rows[0]["App_type"]!=null && ds.Tables[0].Rows[0]["App_type"].ToString()!="")
				{
					model.App_type=ds.Tables[0].Rows[0]["App_type"].ToString();
				}
				if(ds.Tables[0].Rows[0]["App_icon"]!=null && ds.Tables[0].Rows[0]["App_icon"].ToString()!="")
				{
					model.App_icon=ds.Tables[0].Rows[0]["App_icon"].ToString();
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
			strSql.Append("select id,App_name,App_order,App_url,App_handler,App_type,App_icon ");
			strSql.Append(" FROM Sys_App ");
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
			strSql.Append(" id,App_name,App_order,App_url,App_handler,App_type,App_icon ");
			strSql.Append(" FROM Sys_App ");
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
			strSql.Append(" top " + PageSize + " * FROM Sys_App ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM Sys_App ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM Sys_App ");
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

