using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Sys_role
	/// </summary>
	public partial class Sys_role
	{
		public Sys_role()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("RoleID", "Sys_role"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int RoleID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Sys_role");
			strSql.Append(" where RoleID=@RoleID ");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleID", SqlDbType.Int,4)};
			parameters[0].Value = RoleID;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.Sys_role model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sys_role(");
			strSql.Append("RoleName,RoleDscript,RoleSort,CreateID,CreateDate,UpdateID,UpdateDate)");
			strSql.Append(" values (");
			strSql.Append("@RoleName,@RoleDscript,@RoleSort,@CreateID,@CreateDate,@UpdateID,@UpdateDate)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleName", SqlDbType.VarChar,255),
					new SqlParameter("@RoleDscript", SqlDbType.VarChar,255),
					new SqlParameter("@RoleSort", SqlDbType.Int,4),
					new SqlParameter("@CreateID", SqlDbType.Int,4),
					new SqlParameter("@CreateDate", SqlDbType.DateTime),
					new SqlParameter("@UpdateID", SqlDbType.Int,4),
					new SqlParameter("@UpdateDate", SqlDbType.DateTime)};
			parameters[0].Value = model.RoleName;
			parameters[1].Value = model.RoleDscript;
			parameters[2].Value = model.RoleSort;
			parameters[3].Value = model.CreateID;
			parameters[4].Value = model.CreateDate;
			parameters[5].Value = model.UpdateID;
			parameters[6].Value = model.UpdateDate;

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
		public bool Update(KfCrm.Model.Sys_role model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Sys_role set ");
			strSql.Append("RoleName=@RoleName,");
			strSql.Append("RoleDscript=@RoleDscript,");
			strSql.Append("RoleSort=@RoleSort,");			
			strSql.Append("UpdateID=@UpdateID,");
			strSql.Append("UpdateDate=@UpdateDate");
			strSql.Append(" where RoleID=@RoleID");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleName", SqlDbType.VarChar,255),
					new SqlParameter("@RoleDscript", SqlDbType.VarChar,255),
					new SqlParameter("@RoleSort", SqlDbType.Int,4),					
					new SqlParameter("@UpdateID", SqlDbType.Int,4),
					new SqlParameter("@UpdateDate", SqlDbType.DateTime),
					new SqlParameter("@RoleID", SqlDbType.Int,4)};
			parameters[0].Value = model.RoleName;
			parameters[1].Value = model.RoleDscript;
			parameters[2].Value = model.RoleSort;
			parameters[3].Value = model.UpdateID;
			parameters[4].Value = model.UpdateDate;
			parameters[5].Value = model.RoleID;

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
			strSql.Append("update Sys_role set ");
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
		public bool Delete(int RoleID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Sys_role ");
			strSql.Append(" where RoleID=@RoleID");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleID", SqlDbType.Int,4)
};
			parameters[0].Value = RoleID;

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
		public bool DeleteList(string RoleIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Sys_role ");
			strSql.Append(" where RoleID in ("+RoleIDlist + ")  ");
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
		public KfCrm.Model.Sys_role GetModel(int RoleID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 RoleID,RoleName,RoleDscript,RoleSort,CreateID,CreateDate,UpdateID,UpdateDate from Sys_role ");
			strSql.Append(" where RoleID=@RoleID");
			SqlParameter[] parameters = {
					new SqlParameter("@RoleID", SqlDbType.Int,4)
};
			parameters[0].Value = RoleID;

			KfCrm.Model.Sys_role model=new KfCrm.Model.Sys_role();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["RoleID"]!=null && ds.Tables[0].Rows[0]["RoleID"].ToString()!="")
				{
					model.RoleID=int.Parse(ds.Tables[0].Rows[0]["RoleID"].ToString());
				}
				if(ds.Tables[0].Rows[0]["RoleName"]!=null && ds.Tables[0].Rows[0]["RoleName"].ToString()!="")
				{
					model.RoleName=ds.Tables[0].Rows[0]["RoleName"].ToString();
				}
				if(ds.Tables[0].Rows[0]["RoleDscript"]!=null && ds.Tables[0].Rows[0]["RoleDscript"].ToString()!="")
				{
					model.RoleDscript=ds.Tables[0].Rows[0]["RoleDscript"].ToString();
				}
				if(ds.Tables[0].Rows[0]["RoleSort"]!=null && ds.Tables[0].Rows[0]["RoleSort"].ToString()!="")
				{
					model.RoleSort= int.Parse( ds.Tables[0].Rows[0]["RoleSort"].ToString());
				}
				if(ds.Tables[0].Rows[0]["CreateID"]!=null && ds.Tables[0].Rows[0]["CreateID"].ToString()!="")
				{
					model.CreateID=int.Parse(ds.Tables[0].Rows[0]["CreateID"].ToString());
				}
				if(ds.Tables[0].Rows[0]["CreateDate"]!=null && ds.Tables[0].Rows[0]["CreateDate"].ToString()!="")
				{
					model.CreateDate=DateTime.Parse(ds.Tables[0].Rows[0]["CreateDate"].ToString());
				}
				if(ds.Tables[0].Rows[0]["UpdateID"]!=null && ds.Tables[0].Rows[0]["UpdateID"].ToString()!="")
				{
					model.UpdateID=int.Parse(ds.Tables[0].Rows[0]["UpdateID"].ToString());
				}
				if(ds.Tables[0].Rows[0]["UpdateDate"]!=null && ds.Tables[0].Rows[0]["UpdateDate"].ToString()!="")
				{
					model.UpdateDate=DateTime.Parse(ds.Tables[0].Rows[0]["UpdateDate"].ToString());
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
			strSql.Append("select RoleID,RoleName,RoleDscript,RoleSort,CreateID,CreateDate,UpdateID,UpdateDate ");
			strSql.Append(" FROM Sys_role ");
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
			strSql.Append(" RoleID,RoleName,RoleDscript,RoleSort,CreateID,CreateDate,UpdateID,UpdateDate ");
			strSql.Append(" FROM Sys_role ");
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
			strSql.Append(" top " + PageSize + " * FROM Sys_role ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM Sys_role ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM Sys_role ");
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

