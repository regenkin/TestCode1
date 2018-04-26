using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Sys_data_authority
	/// </summary>
	public partial class Sys_data_authority
	{
		public Sys_data_authority()
		{}
		#region  Method



		/// <summary>
		/// 增加一条数据
		/// </summary>
		public void Add(KfCrm.Model.Sys_data_authority model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sys_data_authority(");
			strSql.Append("Role_id,option_id,Sys_option,Sys_view,Sys_add,Sys_edit,Sys_del,Create_id,Create_date)");
			strSql.Append(" values (");
			strSql.Append("@Role_id,@option_id,@Sys_option,@Sys_view,@Sys_add,@Sys_edit,@Sys_del,@Create_id,@Create_date)");
			SqlParameter[] parameters = {
					new SqlParameter("@Role_id", SqlDbType.Int,4),
					new SqlParameter("@option_id", SqlDbType.Int,4),
					new SqlParameter("@Sys_option", SqlDbType.VarChar,250),
					new SqlParameter("@Sys_view", SqlDbType.Int,4),
					new SqlParameter("@Sys_add", SqlDbType.Int,4),
					new SqlParameter("@Sys_edit", SqlDbType.Int,4),
					new SqlParameter("@Sys_del", SqlDbType.Int,4),
					new SqlParameter("@Create_id", SqlDbType.Int,4),
					new SqlParameter("@Create_date", SqlDbType.DateTime)};
			parameters[0].Value = model.Role_id;
			parameters[1].Value = model.option_id;
			parameters[2].Value = model.Sys_option;
			parameters[3].Value = model.Sys_view;
			parameters[4].Value = model.Sys_add;
			parameters[5].Value = model.Sys_edit;
			parameters[6].Value = model.Sys_del;
			parameters[7].Value = model.Create_id;
			parameters[8].Value = model.Create_date;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
	

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string where)
		{
			//该表无主键信息，请自定义主键/条件字段
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Sys_data_authority ");
			strSql.Append(" where "+where);
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
	

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select Role_id,option_id,Sys_option,Sys_view,Sys_add,Sys_edit,Sys_del,Create_id,Create_date ");
			strSql.Append(" FROM Sys_data_authority ");
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
			strSql.Append(" Role_id,option_id,Sys_option,Sys_view,Sys_add,Sys_edit,Sys_del,Create_id,Create_date ");
			strSql.Append(" FROM Sys_data_authority ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		#endregion  Method
	}
}

