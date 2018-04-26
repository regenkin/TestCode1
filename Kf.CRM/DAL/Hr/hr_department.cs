using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:hr_department
	/// </summary>
	public partial class hr_department
	{
		public hr_department()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "hr_department"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from hr_department");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.hr_department model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into hr_department(");
			strSql.Append("d_name,parentid,parentname,d_type,d_icon,d_fuzeren,d_tel,d_fax,d_add,d_email,d_miaoshu,d_order,isDelete,Delete_time)");
			strSql.Append(" values (");
			strSql.Append("@d_name,@parentid,@parentname,@d_type,@d_icon,@d_fuzeren,@d_tel,@d_fax,@d_add,@d_email,@d_miaoshu,@d_order,@isDelete,@Delete_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@d_name", SqlDbType.VarChar,50),
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@parentname", SqlDbType.VarChar,50),
					new SqlParameter("@d_type", SqlDbType.VarChar,50),
					new SqlParameter("@d_icon", SqlDbType.VarChar,50),
					new SqlParameter("@d_fuzeren", SqlDbType.VarChar,50),
					new SqlParameter("@d_tel", SqlDbType.VarChar,50),
					new SqlParameter("@d_fax", SqlDbType.VarChar,50),
					new SqlParameter("@d_add", SqlDbType.VarChar,255),
					new SqlParameter("@d_email", SqlDbType.VarChar,50),
					new SqlParameter("@d_miaoshu", SqlDbType.VarChar,255),
					new SqlParameter("@d_order", SqlDbType.Int,4),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime)};
			parameters[0].Value = model.d_name;
			parameters[1].Value = model.parentid;
			parameters[2].Value = model.parentname;
			parameters[3].Value = model.d_type;
			parameters[4].Value = model.d_icon;
			parameters[5].Value = model.d_fuzeren;
			parameters[6].Value = model.d_tel;
			parameters[7].Value = model.d_fax;
			parameters[8].Value = model.d_add;
			parameters[9].Value = model.d_email;
			parameters[10].Value = model.d_miaoshu;
			parameters[11].Value = model.d_order;
			parameters[12].Value = model.isDelete;
			parameters[13].Value = model.Delete_time;

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
		public bool Update(KfCrm.Model.hr_department model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update hr_department set ");
			strSql.Append("d_name=@d_name,");
			strSql.Append("parentid=@parentid,");
			strSql.Append("parentname=@parentname,");
			strSql.Append("d_type=@d_type,");
			strSql.Append("d_icon=@d_icon,");
			strSql.Append("d_fuzeren=@d_fuzeren,");
			strSql.Append("d_tel=@d_tel,");
			strSql.Append("d_fax=@d_fax,");
			strSql.Append("d_add=@d_add,");
			strSql.Append("d_email=@d_email,");
			strSql.Append("d_miaoshu=@d_miaoshu,");
			strSql.Append("d_order=@d_order");

			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@d_name", SqlDbType.VarChar,50),
					new SqlParameter("@parentid", SqlDbType.Int,4),
					new SqlParameter("@parentname", SqlDbType.VarChar,50),
					new SqlParameter("@d_type", SqlDbType.VarChar,50),
					new SqlParameter("@d_icon", SqlDbType.VarChar,50),
					new SqlParameter("@d_fuzeren", SqlDbType.VarChar,50),
					new SqlParameter("@d_tel", SqlDbType.VarChar,50),
					new SqlParameter("@d_fax", SqlDbType.VarChar,50),
					new SqlParameter("@d_add", SqlDbType.VarChar,255),
					new SqlParameter("@d_email", SqlDbType.VarChar,50),
					new SqlParameter("@d_miaoshu", SqlDbType.VarChar,255),
					new SqlParameter("@d_order", SqlDbType.Int,4),  
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.d_name;
			parameters[1].Value = model.parentid;
			parameters[2].Value = model.parentname;
			parameters[3].Value = model.d_type;
			parameters[4].Value = model.d_icon;
			parameters[5].Value = model.d_fuzeren;
			parameters[6].Value = model.d_tel;
			parameters[7].Value = model.d_fax;
			parameters[8].Value = model.d_add;
			parameters[9].Value = model.d_email;
			parameters[10].Value = model.d_miaoshu;
			parameters[11].Value = model.d_order;   
			parameters[12].Value = model.id;

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
			strSql.Append("update hr_department set ");
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
			strSql.Append("delete from hr_department ");
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
			strSql.Append("delete from hr_department ");
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
		public KfCrm.Model.hr_department GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,d_name,parentid,parentname,d_type,d_icon,d_fuzeren,d_tel,d_fax,d_add,d_email,d_miaoshu,d_order,isDelete,Delete_time from hr_department ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.hr_department model=new KfCrm.Model.hr_department();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["d_name"]!=null && ds.Tables[0].Rows[0]["d_name"].ToString()!="")
				{
					model.d_name=ds.Tables[0].Rows[0]["d_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["parentid"]!=null && ds.Tables[0].Rows[0]["parentid"].ToString()!="")
				{
					model.parentid=int.Parse(ds.Tables[0].Rows[0]["parentid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["parentname"]!=null && ds.Tables[0].Rows[0]["parentname"].ToString()!="")
				{
					model.parentname=ds.Tables[0].Rows[0]["parentname"].ToString();
				}
				if(ds.Tables[0].Rows[0]["d_type"]!=null && ds.Tables[0].Rows[0]["d_type"].ToString()!="")
				{
					model.d_type=ds.Tables[0].Rows[0]["d_type"].ToString();
				}
				if(ds.Tables[0].Rows[0]["d_icon"]!=null && ds.Tables[0].Rows[0]["d_icon"].ToString()!="")
				{
					model.d_icon=ds.Tables[0].Rows[0]["d_icon"].ToString();
				}
				if(ds.Tables[0].Rows[0]["d_fuzeren"]!=null && ds.Tables[0].Rows[0]["d_fuzeren"].ToString()!="")
				{
					model.d_fuzeren=ds.Tables[0].Rows[0]["d_fuzeren"].ToString();
				}
				if(ds.Tables[0].Rows[0]["d_tel"]!=null && ds.Tables[0].Rows[0]["d_tel"].ToString()!="")
				{
					model.d_tel=ds.Tables[0].Rows[0]["d_tel"].ToString();
				}
				if(ds.Tables[0].Rows[0]["d_fax"]!=null && ds.Tables[0].Rows[0]["d_fax"].ToString()!="")
				{
					model.d_fax=ds.Tables[0].Rows[0]["d_fax"].ToString();
				}
				if(ds.Tables[0].Rows[0]["d_add"]!=null && ds.Tables[0].Rows[0]["d_add"].ToString()!="")
				{
					model.d_add=ds.Tables[0].Rows[0]["d_add"].ToString();
				}
				if(ds.Tables[0].Rows[0]["d_email"]!=null && ds.Tables[0].Rows[0]["d_email"].ToString()!="")
				{
					model.d_email=ds.Tables[0].Rows[0]["d_email"].ToString();
				}
				if(ds.Tables[0].Rows[0]["d_miaoshu"]!=null && ds.Tables[0].Rows[0]["d_miaoshu"].ToString()!="")
				{
					model.d_miaoshu=ds.Tables[0].Rows[0]["d_miaoshu"].ToString();
				}
				if(ds.Tables[0].Rows[0]["d_order"]!=null && ds.Tables[0].Rows[0]["d_order"].ToString()!="")
				{
					model.d_order= int.Parse(ds.Tables[0].Rows[0]["d_order"].ToString());
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
			strSql.Append("select id,d_name,parentid,parentname,d_type,d_icon,d_fuzeren,d_tel,d_fax,d_add,d_email,d_miaoshu,d_order,isDelete,Delete_time ");
			strSql.Append(" FROM hr_department ");
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
			strSql.Append(" id,d_name,parentid,parentname,d_type,d_icon,d_fuzeren,d_tel,d_fax,d_add,d_email,d_miaoshu,d_order,isDelete,Delete_time ");
			strSql.Append(" FROM hr_department ");
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
			strSql.Append(" top " + PageSize + " * FROM hr_department ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM hr_department ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM hr_department ");
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

