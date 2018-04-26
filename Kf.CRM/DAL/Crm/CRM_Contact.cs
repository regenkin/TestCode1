using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:CRM_Contact
	/// </summary>
	public partial class CRM_Contact
	{
		public CRM_Contact()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "CRM_Contact"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from CRM_Contact");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.CRM_Contact model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into CRM_Contact(");
			strSql.Append("C_name,C_sex,C_department,C_position,C_birthday,C_tel,C_fax,C_email,C_mob,C_QQ,C_add,C_hobby,C_remarks,C_customerid,C_customername,C_createId,C_createDate,isDelete,Delete_time)");
			strSql.Append(" values (");
			strSql.Append("@C_name,@C_sex,@C_department,@C_position,@C_birthday,@C_tel,@C_fax,@C_email,@C_mob,@C_QQ,@C_add,@C_hobby,@C_remarks,@C_customerid,@C_customername,@C_createId,@C_createDate,@isDelete,@Delete_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@C_name", SqlDbType.NVarChar,250),
					new SqlParameter("@C_sex", SqlDbType.NVarChar,250),
					new SqlParameter("@C_department", SqlDbType.NVarChar,250),
					new SqlParameter("@C_position", SqlDbType.NVarChar,250),
					new SqlParameter("@C_birthday", SqlDbType.NVarChar,250),
					new SqlParameter("@C_tel", SqlDbType.NVarChar,250),
					new SqlParameter("@C_fax", SqlDbType.NVarChar,250),
					new SqlParameter("@C_email", SqlDbType.NVarChar,250),
					new SqlParameter("@C_mob", SqlDbType.NVarChar,250),
					new SqlParameter("@C_QQ", SqlDbType.NVarChar,250),
					new SqlParameter("@C_add", SqlDbType.NVarChar,250),
					new SqlParameter("@C_hobby", SqlDbType.NVarChar,250),
					new SqlParameter("@C_remarks", SqlDbType.NVarChar),
					new SqlParameter("@C_customerid", SqlDbType.Int,4),
					new SqlParameter("@C_customername", SqlDbType.NVarChar,250),
					new SqlParameter("@C_createId", SqlDbType.Int,4),
					new SqlParameter("@C_createDate", SqlDbType.DateTime),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime)};
			parameters[0].Value = model.C_name;
			parameters[1].Value = model.C_sex;
			parameters[2].Value = model.C_department;
			parameters[3].Value = model.C_position;
			parameters[4].Value = model.C_birthday;
			parameters[5].Value = model.C_tel;
			parameters[6].Value = model.C_fax;
			parameters[7].Value = model.C_email;
			parameters[8].Value = model.C_mob;
			parameters[9].Value = model.C_QQ;
			parameters[10].Value = model.C_add;
			parameters[11].Value = model.C_hobby;
			parameters[12].Value = model.C_remarks;
			parameters[13].Value = model.C_customerid;
			parameters[14].Value = model.C_customername;
			parameters[15].Value = model.C_createId;
			parameters[16].Value = model.C_createDate;
			parameters[17].Value = model.isDelete;
			parameters[18].Value = model.Delete_time;

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
		public bool Update(KfCrm.Model.CRM_Contact model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update CRM_Contact set ");
			strSql.Append("C_name=@C_name,");
			strSql.Append("C_sex=@C_sex,");
			strSql.Append("C_department=@C_department,");
			strSql.Append("C_position=@C_position,");
			strSql.Append("C_birthday=@C_birthday,");
			strSql.Append("C_tel=@C_tel,");
			strSql.Append("C_fax=@C_fax,");
			strSql.Append("C_email=@C_email,");
			strSql.Append("C_mob=@C_mob,");
			strSql.Append("C_QQ=@C_QQ,");
			strSql.Append("C_add=@C_add,");
			strSql.Append("C_hobby=@C_hobby,");
			strSql.Append("C_remarks=@C_remarks,");
			strSql.Append("C_customerid=@C_customerid,");
			strSql.Append("C_customername=@C_customername"); 
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@C_name", SqlDbType.NVarChar,250),
					new SqlParameter("@C_sex", SqlDbType.NVarChar,250),
					new SqlParameter("@C_department", SqlDbType.NVarChar,250),
					new SqlParameter("@C_position", SqlDbType.NVarChar,250),
					new SqlParameter("@C_birthday", SqlDbType.NVarChar,250),
					new SqlParameter("@C_tel", SqlDbType.NVarChar,250),
					new SqlParameter("@C_fax", SqlDbType.NVarChar,250),
					new SqlParameter("@C_email", SqlDbType.NVarChar,250),
					new SqlParameter("@C_mob", SqlDbType.NVarChar,250),
					new SqlParameter("@C_QQ", SqlDbType.NVarChar,250),
					new SqlParameter("@C_add", SqlDbType.NVarChar,250),
					new SqlParameter("@C_hobby", SqlDbType.NVarChar,250),
					new SqlParameter("@C_remarks", SqlDbType.NVarChar),
					new SqlParameter("@C_customerid", SqlDbType.Int,4),
					new SqlParameter("@C_customername", SqlDbType.NVarChar,250),  
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.C_name;
			parameters[1].Value = model.C_sex;
			parameters[2].Value = model.C_department;
			parameters[3].Value = model.C_position;
			parameters[4].Value = model.C_birthday;
			parameters[5].Value = model.C_tel;
			parameters[6].Value = model.C_fax;
			parameters[7].Value = model.C_email;
			parameters[8].Value = model.C_mob;
			parameters[9].Value = model.C_QQ;
			parameters[10].Value = model.C_add;
			parameters[11].Value = model.C_hobby;
			parameters[12].Value = model.C_remarks;
			parameters[13].Value = model.C_customerid;
			parameters[14].Value = model.C_customername; 
			parameters[15].Value = model.id;

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
			strSql.Append("update CRM_Contact set ");
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
			strSql.Append("delete from CRM_Contact ");
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
			strSql.Append("delete from CRM_Contact ");
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
		public KfCrm.Model.CRM_Contact GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,C_name,C_sex,C_department,C_position,C_birthday,C_tel,C_fax,C_email,C_mob,C_QQ,C_add,C_hobby,C_remarks,C_customerid,C_customername,C_createId,C_createDate,isDelete,Delete_time from CRM_Contact ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.CRM_Contact model=new KfCrm.Model.CRM_Contact();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["C_name"]!=null && ds.Tables[0].Rows[0]["C_name"].ToString()!="")
				{
					model.C_name=ds.Tables[0].Rows[0]["C_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_sex"]!=null && ds.Tables[0].Rows[0]["C_sex"].ToString()!="")
				{
					model.C_sex=ds.Tables[0].Rows[0]["C_sex"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_department"]!=null && ds.Tables[0].Rows[0]["C_department"].ToString()!="")
				{
					model.C_department=ds.Tables[0].Rows[0]["C_department"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_position"]!=null && ds.Tables[0].Rows[0]["C_position"].ToString()!="")
				{
					model.C_position=ds.Tables[0].Rows[0]["C_position"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_birthday"]!=null && ds.Tables[0].Rows[0]["C_birthday"].ToString()!="")
				{
					model.C_birthday=ds.Tables[0].Rows[0]["C_birthday"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_tel"]!=null && ds.Tables[0].Rows[0]["C_tel"].ToString()!="")
				{
					model.C_tel=ds.Tables[0].Rows[0]["C_tel"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_fax"]!=null && ds.Tables[0].Rows[0]["C_fax"].ToString()!="")
				{
					model.C_fax=ds.Tables[0].Rows[0]["C_fax"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_email"]!=null && ds.Tables[0].Rows[0]["C_email"].ToString()!="")
				{
					model.C_email=ds.Tables[0].Rows[0]["C_email"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_mob"]!=null && ds.Tables[0].Rows[0]["C_mob"].ToString()!="")
				{
					model.C_mob=ds.Tables[0].Rows[0]["C_mob"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_QQ"]!=null && ds.Tables[0].Rows[0]["C_QQ"].ToString()!="")
				{
					model.C_QQ=ds.Tables[0].Rows[0]["C_QQ"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_add"]!=null && ds.Tables[0].Rows[0]["C_add"].ToString()!="")
				{
					model.C_add=ds.Tables[0].Rows[0]["C_add"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_hobby"]!=null && ds.Tables[0].Rows[0]["C_hobby"].ToString()!="")
				{
					model.C_hobby=ds.Tables[0].Rows[0]["C_hobby"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_remarks"]!=null && ds.Tables[0].Rows[0]["C_remarks"].ToString()!="")
				{
					model.C_remarks=ds.Tables[0].Rows[0]["C_remarks"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_customerid"]!=null && ds.Tables[0].Rows[0]["C_customerid"].ToString()!="")
				{
					model.C_customerid=int.Parse(ds.Tables[0].Rows[0]["C_customerid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["C_customername"]!=null && ds.Tables[0].Rows[0]["C_customername"].ToString()!="")
				{
					model.C_customername=ds.Tables[0].Rows[0]["C_customername"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_createId"]!=null && ds.Tables[0].Rows[0]["C_createId"].ToString()!="")
				{
					model.C_createId=int.Parse(ds.Tables[0].Rows[0]["C_createId"].ToString());
				}
				if(ds.Tables[0].Rows[0]["C_createDate"]!=null && ds.Tables[0].Rows[0]["C_createDate"].ToString()!="")
				{
					model.C_createDate=DateTime.Parse(ds.Tables[0].Rows[0]["C_createDate"].ToString());
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
			strSql.Append("select id,C_name,C_sex,C_department,C_position,C_birthday,C_tel,C_fax,C_email,C_mob,C_QQ,C_add,C_hobby,C_remarks,C_customerid,C_customername,C_createId,C_createDate,isDelete,Delete_time ");
			strSql.Append(" FROM CRM_Contact ");
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
			strSql.Append(" id,C_name,C_sex,C_department,C_position,C_birthday,C_tel,C_fax,C_email,C_mob,C_QQ,C_add,C_hobby,C_remarks,C_customerid,C_customername,C_createId,C_createDate,isDelete,Delete_time ");
			strSql.Append(" FROM CRM_Contact ");
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
			strSql.Append(" top " + PageSize + " * FROM CRM_Contact ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM CRM_Contact ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM CRM_Contact ");
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

