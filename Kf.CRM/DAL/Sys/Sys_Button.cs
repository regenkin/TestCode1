using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Sys_Button
	/// </summary>
	public partial class Sys_Button
	{
		public Sys_Button()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Btn_id", "Sys_Button"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Btn_id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Sys_Button");
			strSql.Append(" where Btn_id=@Btn_id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Btn_id", SqlDbType.Int,4)};
			parameters[0].Value = Btn_id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.Sys_Button model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sys_Button(");
			strSql.Append("Btn_name,Btn_icon,Btn_handler,Menu_id,Menu_name,Btn_order)");
			strSql.Append(" values (");
			strSql.Append("@Btn_name,@Btn_icon,@Btn_handler,@Menu_id,@Menu_name,@Btn_order)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Btn_name", SqlDbType.VarChar,255),
					new SqlParameter("@Btn_icon", SqlDbType.VarChar,50),
					new SqlParameter("@Btn_handler", SqlDbType.VarChar,255),
					new SqlParameter("@Menu_id", SqlDbType.Int,4),
					new SqlParameter("@Menu_name", SqlDbType.VarChar,255),
					new SqlParameter("@Btn_order", SqlDbType.VarChar,50)};
			parameters[0].Value = model.Btn_name;
			parameters[1].Value = model.Btn_icon;
			parameters[2].Value = model.Btn_handler;
			parameters[3].Value = model.Menu_id;
			parameters[4].Value = model.Menu_name;
			parameters[5].Value = model.Btn_order;

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
		public bool Update(KfCrm.Model.Sys_Button model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Sys_Button set ");
			strSql.Append("Btn_name=@Btn_name,");
			strSql.Append("Btn_icon=@Btn_icon,");
			strSql.Append("Btn_handler=@Btn_handler,");
			strSql.Append("Menu_id=@Menu_id,");
			strSql.Append("Menu_name=@Menu_name,");
			strSql.Append("Btn_order=@Btn_order");
			strSql.Append(" where Btn_id=@Btn_id");
			SqlParameter[] parameters = {
					new SqlParameter("@Btn_name", SqlDbType.VarChar,255),
					new SqlParameter("@Btn_icon", SqlDbType.VarChar,50),
					new SqlParameter("@Btn_handler", SqlDbType.VarChar,255),
					new SqlParameter("@Menu_id", SqlDbType.Int,4),
					new SqlParameter("@Menu_name", SqlDbType.VarChar,255),
					new SqlParameter("@Btn_order", SqlDbType.VarChar,50),
					new SqlParameter("@Btn_id", SqlDbType.Int,4)};
			parameters[0].Value = model.Btn_name;
			parameters[1].Value = model.Btn_icon;
			parameters[2].Value = model.Btn_handler;
			parameters[3].Value = model.Menu_id;
			parameters[4].Value = model.Menu_name;
			parameters[5].Value = model.Btn_order;
			parameters[6].Value = model.Btn_id;

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
			strSql.Append("update Sys_Button set ");
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
		public bool Delete(int Btn_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Sys_Button ");
			strSql.Append(" where Btn_id=@Btn_id");
			SqlParameter[] parameters = {
					new SqlParameter("@Btn_id", SqlDbType.Int,4)
};
			parameters[0].Value = Btn_id;

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
		public bool DeleteList(string Btn_idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Sys_Button ");
			strSql.Append(" where Btn_id in ("+Btn_idlist + ")  ");
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
		public KfCrm.Model.Sys_Button GetModel(int Btn_id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Btn_id,Btn_name,Btn_icon,Btn_handler,Menu_id,Menu_name,Btn_order from Sys_Button ");
			strSql.Append(" where Btn_id=@Btn_id");
			SqlParameter[] parameters = {
					new SqlParameter("@Btn_id", SqlDbType.Int,4)
};
			parameters[0].Value = Btn_id;

			KfCrm.Model.Sys_Button model=new KfCrm.Model.Sys_Button();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Btn_id"]!=null && ds.Tables[0].Rows[0]["Btn_id"].ToString()!="")
				{
					model.Btn_id=int.Parse(ds.Tables[0].Rows[0]["Btn_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Btn_name"]!=null && ds.Tables[0].Rows[0]["Btn_name"].ToString()!="")
				{
					model.Btn_name=ds.Tables[0].Rows[0]["Btn_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Btn_icon"]!=null && ds.Tables[0].Rows[0]["Btn_icon"].ToString()!="")
				{
					model.Btn_icon=ds.Tables[0].Rows[0]["Btn_icon"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Btn_handler"]!=null && ds.Tables[0].Rows[0]["Btn_handler"].ToString()!="")
				{
					model.Btn_handler=ds.Tables[0].Rows[0]["Btn_handler"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Menu_id"]!=null && ds.Tables[0].Rows[0]["Menu_id"].ToString()!="")
				{
					model.Menu_id=int.Parse(ds.Tables[0].Rows[0]["Menu_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Menu_name"]!=null && ds.Tables[0].Rows[0]["Menu_name"].ToString()!="")
				{
					model.Menu_name=ds.Tables[0].Rows[0]["Menu_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Btn_order"]!=null && ds.Tables[0].Rows[0]["Btn_order"].ToString()!="")
				{
					model.Btn_order=ds.Tables[0].Rows[0]["Btn_order"].ToString();
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
			strSql.Append("select Btn_id,Btn_name,Btn_icon,Btn_handler,Menu_id,Menu_name,Btn_order ");
			strSql.Append(" FROM Sys_Button ");
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
			strSql.Append(" Btn_id,Btn_name,Btn_icon,Btn_handler,Menu_id,Menu_name,Btn_order ");
			strSql.Append(" FROM Sys_Button ");
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
			strSql.Append(" top " + PageSize + " * FROM Sys_Button ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM Sys_Button ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM Sys_Button ");
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

