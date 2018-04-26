using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:sys_info
	/// </summary>
	public partial class sys_info
	{
		public sys_info()
		{}
		#region  Method
        		
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(KfCrm.Model.sys_info model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update sys_info set ");
			strSql.Append("sys_value=@sys_value");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@sys_key", SqlDbType.VarChar,50),
					new SqlParameter("@sys_value", SqlDbType.VarChar),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.sys_key;
			parameters[1].Value = model.sys_value;
			parameters[2].Value = model.id;

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
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select id,sys_key,sys_value ");
			strSql.Append(" FROM sys_info ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		
		#endregion  Method
	}
}

