using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Param_SysParam_Type
	/// </summary>
	public partial class Param_SysParam_Type
	{
		public Param_SysParam_Type()
		{}
		#region  Method



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(KfCrm.Model.Param_SysParam_Type model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Param_SysParam_Type(");
			strSql.Append("id,params_name,params_order,isDelete,Delete_time)");
			strSql.Append(" values (");
			strSql.Append("@id,@params_name,@params_order,@isDelete,@Delete_time)");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@params_name", SqlDbType.VarChar,250),
					new SqlParameter("@params_order", SqlDbType.Int,4),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.params_name;
			parameters[2].Value = model.params_order;
			parameters[3].Value = model.isDelete;
			parameters[4].Value = model.Delete_time;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.Param_SysParam_Type model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Param_SysParam_Type set ");
			strSql.Append("id=@id,");
			strSql.Append("params_name=@params_name,");
			strSql.Append("params_order=@params_order,");
			strSql.Append("isDelete=@isDelete,");
			strSql.Append("Delete_time=@Delete_time");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4),
					new SqlParameter("@params_name", SqlDbType.VarChar,250),
					new SqlParameter("@params_order", SqlDbType.Int,4),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime)};
			parameters[0].Value = model.id;
			parameters[1].Value = model.params_name;
			parameters[2].Value = model.params_order;
			parameters[3].Value = model.isDelete;
			parameters[4].Value = model.Delete_time;

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
		public bool Delete()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Param_SysParam_Type ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
};

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
		/// 得到一个对象实体
		/// </summary>
		public KfCrm.Model.Param_SysParam_Type GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,params_name,params_order,isDelete,Delete_time from Param_SysParam_Type ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
};

			KfCrm.Model.Param_SysParam_Type model=new KfCrm.Model.Param_SysParam_Type();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["params_name"]!=null && ds.Tables[0].Rows[0]["params_name"].ToString()!="")
				{
					model.params_name=ds.Tables[0].Rows[0]["params_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["params_order"]!=null && ds.Tables[0].Rows[0]["params_order"].ToString()!="")
				{
					model.params_order= int.Parse( ds.Tables[0].Rows[0]["params_order"].ToString());
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
			strSql.Append("select id,params_name,params_order,isDelete,Delete_time ");
			strSql.Append(" FROM Param_SysParam_Type ");
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
			strSql.Append(" id,params_name,params_order,isDelete,Delete_time ");
			strSql.Append(" FROM Param_SysParam_Type ");
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
			strSql.Append(" top " + PageSize + " * FROM Param_SysParam_Type ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM Param_SysParam_Type ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM Param_SysParam_Type ");
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

