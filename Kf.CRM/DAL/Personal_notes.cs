using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Personal_notes
	/// </summary>
	public partial class Personal_notes
	{
		public Personal_notes()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "Personal_notes"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Personal_notes");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.Personal_notes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Personal_notes(");
			strSql.Append("emp_id,emp_name,note_content,note_color,xyz,note_time)");
			strSql.Append(" values (");
			strSql.Append("@emp_id,@emp_name,@note_content,@note_color,@xyz,@note_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@emp_id", SqlDbType.Int,4),
					new SqlParameter("@emp_name", SqlDbType.NVarChar,250),
					new SqlParameter("@note_content", SqlDbType.NVarChar,4000),
					new SqlParameter("@note_color", SqlDbType.NVarChar,250),
					new SqlParameter("@xyz", SqlDbType.NVarChar,250),
					new SqlParameter("@note_time", SqlDbType.DateTime)};
			parameters[0].Value = model.emp_id;
			parameters[1].Value = model.emp_name;
			parameters[2].Value = model.note_content;
			parameters[3].Value = model.note_color;
			parameters[4].Value = model.xyz;
			parameters[5].Value = model.note_time;

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
		public bool Update(KfCrm.Model.Personal_notes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Personal_notes set ");			
			strSql.Append("xyz=@xyz ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {					
					new SqlParameter("@xyz", SqlDbType.NVarChar,250),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.xyz;
			parameters[1].Value = model.id;

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
			strSql.Append("delete from Personal_notes ");
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
			strSql.Append("delete from Personal_notes ");
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
		public KfCrm.Model.Personal_notes GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,emp_id,emp_name,note_content,note_color,xyz,note_time from Personal_notes ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.Personal_notes model=new KfCrm.Model.Personal_notes();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["emp_id"].ToString()!="")
				{
					model.emp_id=int.Parse(ds.Tables[0].Rows[0]["emp_id"].ToString());
				}
				model.emp_name=ds.Tables[0].Rows[0]["emp_name"].ToString();
				model.note_content=ds.Tables[0].Rows[0]["note_content"].ToString();
				model.note_color=ds.Tables[0].Rows[0]["note_color"].ToString();
				model.xyz=ds.Tables[0].Rows[0]["xyz"].ToString();
				if(ds.Tables[0].Rows[0]["note_time"].ToString()!="")
				{
					model.note_time=DateTime.Parse(ds.Tables[0].Rows[0]["note_time"].ToString());
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
			strSql.Append("select id,emp_id,emp_name,note_content,note_color,xyz,note_time ");
			strSql.Append(" FROM Personal_notes ");
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
			strSql.Append(" id,emp_id,emp_name,note_content,note_color,xyz,note_time ");
			strSql.Append(" FROM Personal_notes ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "Personal_notes";
			parameters[1].Value = "id";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  Method
	}
}

