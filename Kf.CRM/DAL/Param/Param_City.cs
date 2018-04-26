using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Param_City
	/// </summary>
	public partial class Param_City
	{
		public Param_City()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "Param_City"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Param_City");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.Param_City model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Param_City(");
            strSql.Append("parentid,City,City_order,Create_id,Create_date,Update_id,Update_date)");
			strSql.Append(" values (");
            strSql.Append("@parentid,@City,@City_order,@Create_id,@Create_date,@Update_id,@Update_date)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@City", SqlDbType.VarChar,250),
                    new SqlParameter("@City_order",SqlDbType.Int,4),
					new SqlParameter("@Create_id", SqlDbType.Int,4),
					new SqlParameter("@Create_date", SqlDbType.DateTime),
					new SqlParameter("@Update_id", SqlDbType.Int,4),
					new SqlParameter("@Update_date", SqlDbType.DateTime)};
			parameters[0].Value = model.parentid;
			parameters[1].Value = model.City;
            parameters[2].Value = model.City_order;
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
		public bool Update(KfCrm.Model.Param_City model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Param_City set ");
			strSql.Append("parentid=@parentid,");
			strSql.Append("City=@City,");
            strSql.Append("City_order=@City_order,");
			strSql.Append("Create_id=@Create_id,");
			strSql.Append("Create_date=@Create_date,");
			strSql.Append("Update_id=@Update_id,");
			strSql.Append("Update_date=@Update_date");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@City", SqlDbType.VarChar,250),
                    new SqlParameter("@City_order",SqlDbType.Int,4),
					new SqlParameter("@Create_id", SqlDbType.Int,4),
					new SqlParameter("@Create_date", SqlDbType.DateTime),
					new SqlParameter("@Update_id", SqlDbType.Int,4),
					new SqlParameter("@Update_date", SqlDbType.DateTime),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.parentid;
			parameters[1].Value = model.City;
            parameters[2].Value = model.City_order;
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
		/// 删除一条数据
		/// </summary>
		public bool Delete(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Param_City ");
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
			strSql.Append("delete from Param_City ");
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
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select id,parentid,City,City_order ");
			strSql.Append(" FROM Param_City ");
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
			strSql.Append(" id,parentid,City,City_order ");
			strSql.Append(" FROM Param_City ");
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
            strSql.Append(" top " + PageSize + " id,parentid,City,City_order FROM Param_City ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM Param_City ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM Param_City ");
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

