using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Sys_Menu
	/// </summary>
	public partial class Sys_Menu
	{
		public Sys_Menu()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Menu_id", "Sys_Menu"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Menu_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Sys_Menu");
			strSql.Append(" where Menu_id=@Menu_id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Menu_id", SqlDbType.Int,4)};
			parameters[0].Value = Menu_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.Sys_Menu model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sys_Menu(");
			strSql.Append("Menu_name,parentid,parentname,App_id,Menu_url,Menu_icon,Menu_handler,Menu_order,Menu_type)");
			strSql.Append(" values (");
			strSql.Append("@Menu_name,@parentid,@parentname,@App_id,@Menu_url,@Menu_icon,@Menu_handler,@Menu_order,@Menu_type)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Menu_name", SqlDbType.VarChar,255),
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@parentname", SqlDbType.VarChar,255),
					new SqlParameter("@App_id", SqlDbType.Int,4),
					new SqlParameter("@Menu_url", SqlDbType.VarChar,255),
					new SqlParameter("@Menu_icon", SqlDbType.VarChar,50),
					new SqlParameter("@Menu_handler", SqlDbType.VarChar,50),
					new SqlParameter("@Menu_order", SqlDbType.Int,4),
					new SqlParameter("@Menu_type", SqlDbType.VarChar,50)};
			parameters[0].Value = model.Menu_name;
			parameters[1].Value = model.parentid;
			parameters[2].Value = model.parentname;
			parameters[3].Value = model.App_id;
			parameters[4].Value = model.Menu_url;
			parameters[5].Value = model.Menu_icon;
			parameters[6].Value = model.Menu_handler;
			parameters[7].Value = model.Menu_order;
			parameters[8].Value = model.Menu_type;

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
		public bool Update(KfCrm.Model.Sys_Menu model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Sys_Menu set ");
			strSql.Append("Menu_name=@Menu_name,");
			strSql.Append("parentid=@parentid,");
			strSql.Append("parentname=@parentname,");
			strSql.Append("App_id=@App_id,");
			strSql.Append("Menu_url=@Menu_url,");
			strSql.Append("Menu_icon=@Menu_icon,");
			strSql.Append("Menu_handler=@Menu_handler,");
			strSql.Append("Menu_order=@Menu_order,");
			strSql.Append("Menu_type=@Menu_type");
			strSql.Append(" where Menu_id=@Menu_id");
			SqlParameter[] parameters = {
					new SqlParameter("@Menu_name", SqlDbType.VarChar,255),
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@parentname", SqlDbType.VarChar,255),
					new SqlParameter("@App_id", SqlDbType.Int,4),
					new SqlParameter("@Menu_url", SqlDbType.VarChar,255),
					new SqlParameter("@Menu_icon", SqlDbType.VarChar,50),
					new SqlParameter("@Menu_handler", SqlDbType.VarChar,50),
					new SqlParameter("@Menu_order", SqlDbType.Int,4),
					new SqlParameter("@Menu_type", SqlDbType.VarChar,50),
					new SqlParameter("@Menu_id", SqlDbType.Int,4)};
			parameters[0].Value = model.Menu_name;
			parameters[1].Value = model.parentid;
			parameters[2].Value = model.parentname;
			parameters[3].Value = model.App_id;
			parameters[4].Value = model.Menu_url;
			parameters[5].Value = model.Menu_icon;
			parameters[6].Value = model.Menu_handler;
			parameters[7].Value = model.Menu_order;
			parameters[8].Value = model.Menu_type;
			parameters[9].Value = model.Menu_id;

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
			strSql.Append("update Sys_Menu set ");
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
		public bool Delete(int Menu_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Sys_Menu ");
			strSql.Append(" where Menu_id=@Menu_id");
			SqlParameter[] parameters = {
					new SqlParameter("@Menu_id", SqlDbType.Int,4)
};
			parameters[0].Value = Menu_id;

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
		public bool DeleteList(string Menu_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Sys_Menu ");
			strSql.Append(" where Menu_id in ("+Menu_idlist + ")  ");
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
		public KfCrm.Model.Sys_Menu GetModel(int Menu_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Menu_id,Menu_name,parentid,parentname,App_id,Menu_url,Menu_icon,Menu_handler,Menu_order,Menu_type from Sys_Menu ");
			strSql.Append(" where Menu_id=@Menu_id");
			SqlParameter[] parameters = {
					new SqlParameter("@Menu_id", SqlDbType.Int,4)
};
			parameters[0].Value = Menu_id;

			KfCrm.Model.Sys_Menu model=new KfCrm.Model.Sys_Menu();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Menu_id"]!=null && ds.Tables[0].Rows[0]["Menu_id"].ToString()!="")
				{
					model.Menu_id=int.Parse(ds.Tables[0].Rows[0]["Menu_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Menu_name"]!=null && ds.Tables[0].Rows[0]["Menu_name"].ToString()!="")
				{
					model.Menu_name=ds.Tables[0].Rows[0]["Menu_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["parentid"]!=null && ds.Tables[0].Rows[0]["parentid"].ToString()!="")
				{
					model.parentid=int.Parse(ds.Tables[0].Rows[0]["parentid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["parentname"]!=null && ds.Tables[0].Rows[0]["parentname"].ToString()!="")
				{
					model.parentname=ds.Tables[0].Rows[0]["parentname"].ToString();
				}
				if(ds.Tables[0].Rows[0]["App_id"]!=null && ds.Tables[0].Rows[0]["App_id"].ToString()!="")
				{
					model.App_id=int.Parse(ds.Tables[0].Rows[0]["App_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Menu_url"]!=null && ds.Tables[0].Rows[0]["Menu_url"].ToString()!="")
				{
					model.Menu_url=ds.Tables[0].Rows[0]["Menu_url"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Menu_icon"]!=null && ds.Tables[0].Rows[0]["Menu_icon"].ToString()!="")
				{
					model.Menu_icon=ds.Tables[0].Rows[0]["Menu_icon"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Menu_handler"]!=null && ds.Tables[0].Rows[0]["Menu_handler"].ToString()!="")
				{
					model.Menu_handler=ds.Tables[0].Rows[0]["Menu_handler"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Menu_order"]!=null && ds.Tables[0].Rows[0]["Menu_order"].ToString()!="")
				{
					model.Menu_order=int.Parse(ds.Tables[0].Rows[0]["Menu_order"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Menu_type"]!=null && ds.Tables[0].Rows[0]["Menu_type"].ToString()!="")
				{
					model.Menu_type=ds.Tables[0].Rows[0]["Menu_type"].ToString();
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
			strSql.Append("select Menu_id,Menu_name,parentid,parentname,App_id,Menu_url,Menu_icon,Menu_handler,Menu_order,Menu_type ");
			strSql.Append(" FROM Sys_Menu ");
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
			strSql.Append(" Menu_id,Menu_name,parentid,parentname,App_id,Menu_url,Menu_icon,Menu_handler,Menu_order,Menu_type ");
			strSql.Append(" FROM Sys_Menu ");
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
			strSql.Append(" top " + PageSize + " * FROM Sys_Menu ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM Sys_Menu ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM Sys_Menu ");
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

