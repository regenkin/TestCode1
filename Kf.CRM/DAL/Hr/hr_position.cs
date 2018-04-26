using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:hr_position
	/// </summary>
	public partial class hr_position
	{
		public hr_position()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "hr_position"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hr_position");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.hr_position model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hr_position(");
			strSql.Append("position_name,position_order,position_level,create_id,create_date,isDelete,Delete_time)");
			strSql.Append(" values (");
			strSql.Append("@position_name,@position_order,@position_level,@create_id,@create_date,@isDelete,@Delete_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@position_name", SqlDbType.VarChar,250),
					new SqlParameter("@position_order", SqlDbType.Int,4),
					new SqlParameter("@position_level", SqlDbType.VarChar,50),
					new SqlParameter("@create_id", SqlDbType.Int,4),
					new SqlParameter("@create_date", SqlDbType.DateTime),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime)};
			parameters[0].Value = model.position_name;
			parameters[1].Value = model.position_order;
			parameters[2].Value = model.position_level;
			parameters[3].Value = model.create_id;
			parameters[4].Value = model.create_date;
			parameters[5].Value = model.isDelete;
			parameters[6].Value = model.Delete_time;

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
		public bool Update(KfCrm.Model.hr_position model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hr_position set ");
			strSql.Append("position_name=@position_name,");
			strSql.Append("position_order=@position_order,");
			strSql.Append("position_level=@position_level");    
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@position_name", SqlDbType.VarChar,250),
					new SqlParameter("@position_order", SqlDbType.Int,4),
					new SqlParameter("@position_level", SqlDbType.VarChar,50), 
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.position_name;
			parameters[1].Value = model.position_order;
			parameters[2].Value = model.position_level; 
			parameters[3].Value = model.id;

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
			strSql.Append("update hr_position set ");
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
			strSql.Append("delete from hr_position ");
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
			strSql.Append("delete from hr_position ");
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
		public KfCrm.Model.hr_position GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,position_name,position_order,position_level,create_id,create_date,isDelete,Delete_time from hr_position ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.hr_position model=new KfCrm.Model.hr_position();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["position_name"]!=null && ds.Tables[0].Rows[0]["position_name"].ToString()!="")
				{
					model.position_name=ds.Tables[0].Rows[0]["position_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["position_order"]!=null && ds.Tables[0].Rows[0]["position_order"].ToString()!="")
				{
					model.position_order= int.Parse( ds.Tables[0].Rows[0]["position_order"].ToString());
				}
				if(ds.Tables[0].Rows[0]["position_level"]!=null && ds.Tables[0].Rows[0]["position_level"].ToString()!="")
				{
					model.position_level=ds.Tables[0].Rows[0]["position_level"].ToString();
				}
				if(ds.Tables[0].Rows[0]["create_id"]!=null && ds.Tables[0].Rows[0]["create_id"].ToString()!="")
				{
					model.create_id=int.Parse(ds.Tables[0].Rows[0]["create_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["create_date"]!=null && ds.Tables[0].Rows[0]["create_date"].ToString()!="")
				{
					model.create_date=DateTime.Parse(ds.Tables[0].Rows[0]["create_date"].ToString());
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
			strSql.Append("select id,position_name,position_order,position_level,create_id,create_date,isDelete,Delete_time ");
			strSql.Append(" FROM hr_position ");
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
			strSql.Append(" id,position_name,position_order,position_level,create_id,create_date,isDelete,Delete_time ");
			strSql.Append(" FROM hr_position ");
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
			strSql.Append(" top " + PageSize + " * FROM hr_position ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM hr_position ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM hr_position ");
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

