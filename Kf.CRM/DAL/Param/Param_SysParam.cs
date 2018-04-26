using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Param_SysParam
	/// </summary>
	public partial class Param_SysParam
	{
		public Param_SysParam()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "Param_SysParam"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Param_SysParam");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.Param_SysParam model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Param_SysParam(");
			strSql.Append("parentid,params_name,params_order,Create_id,Create_date,Update_id,Update_date)");
			strSql.Append(" values (");
			strSql.Append("@parentid,@params_name,@params_order,@Create_id,@Create_date,@Update_id,@Update_date)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@params_name", SqlDbType.VarChar,250),
					new SqlParameter("@params_order", SqlDbType.Int,4),
					new SqlParameter("@Create_id", SqlDbType.Int,4),
					new SqlParameter("@Create_date", SqlDbType.DateTime),
					new SqlParameter("@Update_id", SqlDbType.Int,4),
					new SqlParameter("@Update_date", SqlDbType.DateTime)};
			parameters[0].Value = model.parentid;
			parameters[1].Value = model.params_name;
			parameters[2].Value = model.params_order;
			parameters[3].Value = model.Create_id;
			parameters[4].Value = model.Create_date;
			parameters[5].Value = model.Update_id;
			parameters[6].Value = model.Update_date;

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
		public bool Update(KfCrm.Model.Param_SysParam model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Param_SysParam set ");
			strSql.Append("parentid=@parentid,");
			strSql.Append("params_name=@params_name,");
			strSql.Append("params_order=@params_order,");
			strSql.Append("Create_id=@Create_id,");
			strSql.Append("Create_date=@Create_date,");
			strSql.Append("Update_id=@Update_id,");
			strSql.Append("Update_date=@Update_date");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@params_name", SqlDbType.VarChar,250),
					new SqlParameter("@params_order", SqlDbType.Int,4),
					new SqlParameter("@Create_id", SqlDbType.Int,4),
					new SqlParameter("@Create_date", SqlDbType.DateTime),
					new SqlParameter("@Update_id", SqlDbType.Int,4),
					new SqlParameter("@Update_date", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.parentid;
			parameters[1].Value = model.params_name;
			parameters[2].Value = model.params_order;
			parameters[3].Value = model.Create_id;
			parameters[4].Value = model.Create_date;
			parameters[5].Value = model.Update_id;
			parameters[6].Value = model.Update_date;
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
			strSql.Append("update Param_SysParam set ");
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
			strSql.Append("delete from Param_SysParam ");
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
			strSql.Append("delete from Param_SysParam ");
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
		public KfCrm.Model.Param_SysParam GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,parentid,params_name,params_order,Create_id,Create_date,Update_id,Update_date from Param_SysParam ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.Param_SysParam model=new KfCrm.Model.Param_SysParam();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["parentid"]!=null && ds.Tables[0].Rows[0]["parentid"].ToString()!="")
				{
					model.parentid=int.Parse(ds.Tables[0].Rows[0]["parentid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["params_name"]!=null && ds.Tables[0].Rows[0]["params_name"].ToString()!="")
				{
					model.params_name=ds.Tables[0].Rows[0]["params_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["params_order"]!=null && ds.Tables[0].Rows[0]["params_order"].ToString()!="")
				{
					model.params_order= int.Parse( ds.Tables[0].Rows[0]["params_order"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Create_id"]!=null && ds.Tables[0].Rows[0]["Create_id"].ToString()!="")
				{
					model.Create_id=int.Parse(ds.Tables[0].Rows[0]["Create_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Create_date"]!=null && ds.Tables[0].Rows[0]["Create_date"].ToString()!="")
				{
					model.Create_date=DateTime.Parse(ds.Tables[0].Rows[0]["Create_date"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Update_id"]!=null && ds.Tables[0].Rows[0]["Update_id"].ToString()!="")
				{
					model.Update_id=int.Parse(ds.Tables[0].Rows[0]["Update_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Update_date"]!=null && ds.Tables[0].Rows[0]["Update_date"].ToString()!="")
				{
					model.Update_date=DateTime.Parse(ds.Tables[0].Rows[0]["Update_date"].ToString());
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
			strSql.Append("select id,parentid,params_name,params_order,Create_id,Create_date,Update_id,Update_date ");
			strSql.Append(" FROM Param_SysParam ");
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
			strSql.Append(" id,parentid,params_name,params_order,Create_id,Create_date,Update_id,Update_date ");
			strSql.Append(" FROM Param_SysParam ");
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
			strSql.Append(" top " + PageSize + " * FROM Param_SysParam ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM Param_SysParam ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM Param_SysParam ");
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

