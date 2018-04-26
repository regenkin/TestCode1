using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Sys_authority
	/// </summary>
	public partial class Sys_authority
	{
		public Sys_authority()
		{}
		#region  Method



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(KfCrm.Model.Sys_authority model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sys_authority(");
			strSql.Append("Role_id,App_ids,Menu_ids,Button_ids,Create_id,Create_date)");
			strSql.Append(" values (");
			strSql.Append("@Role_id,@App_ids,@Menu_ids,@Button_ids,@Create_id,@Create_date)");
			SqlParameter[] parameters = {
					new SqlParameter("@Role_id", SqlDbType.Int,4),
					new SqlParameter("@App_ids", SqlDbType.VarChar,250),
					new SqlParameter("@Menu_ids", SqlDbType.VarChar,4000),
					new SqlParameter("@Button_ids", SqlDbType.VarChar,4000),
					new SqlParameter("@Create_id", SqlDbType.Int,4),
					new SqlParameter("@Create_date", SqlDbType.DateTime)};
			parameters[0].Value = model.Role_id;
			parameters[1].Value = model.App_ids;
			parameters[2].Value = model.Menu_ids;
			parameters[3].Value = model.Button_ids;
			parameters[4].Value = model.Create_id;
			parameters[5].Value = model.Create_date;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.Sys_authority model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Sys_authority set ");
			strSql.Append("Role_id=@Role_id,");
			strSql.Append("App_ids=@App_ids,");
			strSql.Append("Menu_ids=@Menu_ids,");
			strSql.Append("Button_ids=@Button_ids,");
			strSql.Append("Create_id=@Create_id,");
			strSql.Append("Create_date=@Create_date");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
					new SqlParameter("@Role_id", SqlDbType.Int,4),
					new SqlParameter("@App_ids", SqlDbType.VarChar,250),
					new SqlParameter("@Menu_ids", SqlDbType.VarChar,4000),
					new SqlParameter("@Button_ids", SqlDbType.VarChar,4000),
					new SqlParameter("@Create_id", SqlDbType.Int,4),
					new SqlParameter("@Create_date", SqlDbType.DateTime)};
			parameters[0].Value = model.Role_id;
			parameters[1].Value = model.App_ids;
			parameters[2].Value = model.Menu_ids;
			parameters[3].Value = model.Button_ids;
			parameters[4].Value = model.Create_id;
			parameters[5].Value = model.Create_date;

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
			strSql.Append("delete from Sys_authority ");
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
		public KfCrm.Model.Sys_authority GetModel()
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Role_id,App_ids,Menu_ids,Button_ids,Create_id,Create_date from Sys_authority ");
			strSql.Append(" where ");
			SqlParameter[] parameters = {
};

			KfCrm.Model.Sys_authority model=new KfCrm.Model.Sys_authority();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Role_id"]!=null && ds.Tables[0].Rows[0]["Role_id"].ToString()!="")
				{
					model.Role_id=int.Parse(ds.Tables[0].Rows[0]["Role_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["App_ids"]!=null && ds.Tables[0].Rows[0]["App_ids"].ToString()!="")
				{
					model.App_ids=ds.Tables[0].Rows[0]["App_ids"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Menu_ids"]!=null && ds.Tables[0].Rows[0]["Menu_ids"].ToString()!="")
				{
					model.Menu_ids=ds.Tables[0].Rows[0]["Menu_ids"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Button_ids"]!=null && ds.Tables[0].Rows[0]["Button_ids"].ToString()!="")
				{
					model.Button_ids=ds.Tables[0].Rows[0]["Button_ids"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Create_id"]!=null && ds.Tables[0].Rows[0]["Create_id"].ToString()!="")
				{
					model.Create_id=int.Parse(ds.Tables[0].Rows[0]["Create_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Create_date"]!=null && ds.Tables[0].Rows[0]["Create_date"].ToString()!="")
				{
					model.Create_date=DateTime.Parse(ds.Tables[0].Rows[0]["Create_date"].ToString());
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
			strSql.Append("select Role_id,App_ids,Menu_ids,Button_ids,Create_id,Create_date ");
			strSql.Append(" FROM Sys_authority ");
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
			strSql.Append(" Role_id,App_ids,Menu_ids,Button_ids,Create_id,Create_date ");
			strSql.Append(" FROM Sys_authority ");
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
			strSql.Append(" top " + PageSize + " * FROM Sys_authority ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM Sys_authority ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM Sys_authority ");
			if (strWhere.Trim() != "")
			{
			    strSql.Append(" and " + strWhere);
			    strSql1.Append(" where " + strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			Total = DbHelperSQL.Query(strSql1.ToString()).Tables[0].Rows[0][0].ToString();
			return DbHelperSQL.Query(strSql.ToString());
		}
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteWhere(string wherestr)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Sys_authority ");
            strSql.Append(" where " + wherestr + "  ");
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
		#endregion  Method
	}
}

