using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:CRM_contract
	/// </summary>
	public partial class CRM_contract
	{
		public CRM_contract()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("id", "CRM_contract"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from CRM_contract");
			strSql.Append(" where id=@id ");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.CRM_contract model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into CRM_contract(");
			strSql.Append("Contract_name,Serialnumber,Customer_id,Customer_name,C_depid,C_depname,C_empid,C_empname,Contract_amount,Pay_cycle,Start_date,End_date,Sign_date,Customer_Contractor,Our_Contractor_depid,Our_Contractor_depname,Our_Contractor_id,Our_Contractor_name,Creater_id,Creater_name,Create_time,Main_Content,Remarks,File_serialnumber,isDelete,Delete_time)");
			strSql.Append(" values (");
			strSql.Append("@Contract_name,@Serialnumber,@Customer_id,@Customer_name,@C_depid,@C_depname,@C_empid,@C_empname,@Contract_amount,@Pay_cycle,@Start_date,@End_date,@Sign_date,@Customer_Contractor,@Our_Contractor_depid,@Our_Contractor_depname,@Our_Contractor_id,@Our_Contractor_name,@Creater_id,@Creater_name,@Create_time,@Main_Content,@Remarks,@File_serialnumber,@isDelete,@Delete_time)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Contract_name", SqlDbType.VarChar,250),
					new SqlParameter("@Serialnumber", SqlDbType.VarChar,250),
					new SqlParameter("@Customer_id", SqlDbType.Int,4),
					new SqlParameter("@Customer_name", SqlDbType.VarChar,250),
					new SqlParameter("@C_depid", SqlDbType.Int,4),
					new SqlParameter("@C_depname", SqlDbType.VarChar,250),
					new SqlParameter("@C_empid", SqlDbType.Int,4),
					new SqlParameter("@C_empname", SqlDbType.VarChar,250),
					new SqlParameter("@Contract_amount", SqlDbType.Float,8),
					new SqlParameter("@Pay_cycle", SqlDbType.Int,4),
					new SqlParameter("@Start_date", SqlDbType.VarChar,250),
					new SqlParameter("@End_date", SqlDbType.VarChar,250),
					new SqlParameter("@Sign_date", SqlDbType.VarChar,250),
					new SqlParameter("@Customer_Contractor", SqlDbType.VarChar,250),
					new SqlParameter("@Our_Contractor_depid", SqlDbType.Int,4),
					new SqlParameter("@Our_Contractor_depname", SqlDbType.VarChar,250),
					new SqlParameter("@Our_Contractor_id", SqlDbType.Int,4),
					new SqlParameter("@Our_Contractor_name", SqlDbType.VarChar,250),
					new SqlParameter("@Creater_id", SqlDbType.Int,4),
					new SqlParameter("@Creater_name", SqlDbType.VarChar,250),
					new SqlParameter("@Create_time", SqlDbType.DateTime),
					new SqlParameter("@Main_Content", SqlDbType.VarChar),
					new SqlParameter("@Remarks", SqlDbType.VarChar),
					new SqlParameter("@File_serialnumber", SqlDbType.VarChar,250),
					new SqlParameter("@isDelete", SqlDbType.Int,4),
					new SqlParameter("@Delete_time", SqlDbType.DateTime)};
			parameters[0].Value = model.Contract_name;
			parameters[1].Value = model.Serialnumber;
			parameters[2].Value = model.Customer_id;
			parameters[3].Value = model.Customer_name;
			parameters[4].Value = model.C_depid;
			parameters[5].Value = model.C_depname;
			parameters[6].Value = model.C_empid;
			parameters[7].Value = model.C_empname;
			parameters[8].Value = model.Contract_amount;
			parameters[9].Value = model.Pay_cycle;
			parameters[10].Value = model.Start_date;
			parameters[11].Value = model.End_date;
			parameters[12].Value = model.Sign_date;
			parameters[13].Value = model.Customer_Contractor;
			parameters[14].Value = model.Our_Contractor_depid;
			parameters[15].Value = model.Our_Contractor_depname;
			parameters[16].Value = model.Our_Contractor_id;
			parameters[17].Value = model.Our_Contractor_name;
			parameters[18].Value = model.Creater_id;
			parameters[19].Value = model.Creater_name;
			parameters[20].Value = model.Create_time;
			parameters[21].Value = model.Main_Content;
			parameters[22].Value = model.Remarks;
			parameters[23].Value = model.File_serialnumber;
			parameters[24].Value = model.isDelete;
			parameters[25].Value = model.Delete_time;

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
		public bool Update(KfCrm.Model.CRM_contract model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update CRM_contract set ");
			strSql.Append("Contract_name=@Contract_name,");
			strSql.Append("Serialnumber=@Serialnumber,");
			strSql.Append("Customer_id=@Customer_id,");
			strSql.Append("Customer_name=@Customer_name,");
			strSql.Append("C_depid=@C_depid,");
			strSql.Append("C_depname=@C_depname,");
			strSql.Append("C_empid=@C_empid,");
			strSql.Append("C_empname=@C_empname,");
			strSql.Append("Contract_amount=@Contract_amount,");
			strSql.Append("Pay_cycle=@Pay_cycle,");
			strSql.Append("Start_date=@Start_date,");
			strSql.Append("End_date=@End_date,");
			strSql.Append("Sign_date=@Sign_date,");
			strSql.Append("Customer_Contractor=@Customer_Contractor,");
			strSql.Append("Our_Contractor_depid=@Our_Contractor_depid,");
			strSql.Append("Our_Contractor_depname=@Our_Contractor_depname,");
			strSql.Append("Our_Contractor_id=@Our_Contractor_id,");
			strSql.Append("Our_Contractor_name=@Our_Contractor_name,"); 
			strSql.Append("Main_Content=@Main_Content,");
			strSql.Append("Remarks=@Remarks");  
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@Contract_name", SqlDbType.VarChar,250),
					new SqlParameter("@Serialnumber", SqlDbType.VarChar,250),
					new SqlParameter("@Customer_id", SqlDbType.Int,4),
					new SqlParameter("@Customer_name", SqlDbType.VarChar,250),
					new SqlParameter("@C_depid", SqlDbType.Int,4),
					new SqlParameter("@C_depname", SqlDbType.VarChar,250),
					new SqlParameter("@C_empid", SqlDbType.Int,4),
					new SqlParameter("@C_empname", SqlDbType.VarChar,250),
					new SqlParameter("@Contract_amount", SqlDbType.Float,8),
					new SqlParameter("@Pay_cycle", SqlDbType.Int,4),
					new SqlParameter("@Start_date", SqlDbType.VarChar,250),
					new SqlParameter("@End_date", SqlDbType.VarChar,250),
					new SqlParameter("@Sign_date", SqlDbType.VarChar,250),
					new SqlParameter("@Customer_Contractor", SqlDbType.VarChar,250),
					new SqlParameter("@Our_Contractor_depid", SqlDbType.Int,4),
					new SqlParameter("@Our_Contractor_depname", SqlDbType.VarChar,250),
					new SqlParameter("@Our_Contractor_id", SqlDbType.Int,4),
					new SqlParameter("@Our_Contractor_name", SqlDbType.VarChar,250),  
					new SqlParameter("@Main_Content", SqlDbType.VarChar),
					new SqlParameter("@Remarks", SqlDbType.VarChar),
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = model.Contract_name;
			parameters[1].Value = model.Serialnumber;
			parameters[2].Value = model.Customer_id;
			parameters[3].Value = model.Customer_name;
			parameters[4].Value = model.C_depid;
			parameters[5].Value = model.C_depname;
			parameters[6].Value = model.C_empid;
			parameters[7].Value = model.C_empname;
			parameters[8].Value = model.Contract_amount;
			parameters[9].Value = model.Pay_cycle;
			parameters[10].Value = model.Start_date;
			parameters[11].Value = model.End_date;
			parameters[12].Value = model.Sign_date;
			parameters[13].Value = model.Customer_Contractor;
			parameters[14].Value = model.Our_Contractor_depid;
			parameters[15].Value = model.Our_Contractor_depname;
			parameters[16].Value = model.Our_Contractor_id;
			parameters[17].Value = model.Our_Contractor_name;
			parameters[18].Value = model.Main_Content;
            parameters[19].Value = model.Remarks;
			parameters[20].Value = model.id;

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
			strSql.Append("update CRM_contract set ");
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
			strSql.Append("delete from CRM_contract ");
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
			strSql.Append("delete from CRM_contract ");
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
		public KfCrm.Model.CRM_contract GetModel(int id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 id,Contract_name,Serialnumber,Customer_id,Customer_name,C_depid,C_depname,C_empid,C_empname,Contract_amount,Pay_cycle,Start_date,End_date,Sign_date,Customer_Contractor,Our_Contractor_depid,Our_Contractor_depname,Our_Contractor_id,Our_Contractor_name,Creater_id,Creater_name,Create_time,Main_Content,Remarks,File_serialnumber,isDelete,Delete_time from CRM_contract ");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)
};
			parameters[0].Value = id;

			KfCrm.Model.CRM_contract model=new KfCrm.Model.CRM_contract();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["id"]!=null && ds.Tables[0].Rows[0]["id"].ToString()!="")
				{
					model.id=int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Contract_name"]!=null && ds.Tables[0].Rows[0]["Contract_name"].ToString()!="")
				{
					model.Contract_name=ds.Tables[0].Rows[0]["Contract_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Serialnumber"]!=null && ds.Tables[0].Rows[0]["Serialnumber"].ToString()!="")
				{
					model.Serialnumber=ds.Tables[0].Rows[0]["Serialnumber"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Customer_id"]!=null && ds.Tables[0].Rows[0]["Customer_id"].ToString()!="")
				{
					model.Customer_id=int.Parse(ds.Tables[0].Rows[0]["Customer_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Customer_name"]!=null && ds.Tables[0].Rows[0]["Customer_name"].ToString()!="")
				{
					model.Customer_name=ds.Tables[0].Rows[0]["Customer_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_depid"]!=null && ds.Tables[0].Rows[0]["C_depid"].ToString()!="")
				{
					model.C_depid=int.Parse(ds.Tables[0].Rows[0]["C_depid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["C_depname"]!=null && ds.Tables[0].Rows[0]["C_depname"].ToString()!="")
				{
					model.C_depname=ds.Tables[0].Rows[0]["C_depname"].ToString();
				}
				if(ds.Tables[0].Rows[0]["C_empid"]!=null && ds.Tables[0].Rows[0]["C_empid"].ToString()!="")
				{
					model.C_empid=int.Parse(ds.Tables[0].Rows[0]["C_empid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["C_empname"]!=null && ds.Tables[0].Rows[0]["C_empname"].ToString()!="")
				{
					model.C_empname=ds.Tables[0].Rows[0]["C_empname"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Contract_amount"]!=null && ds.Tables[0].Rows[0]["Contract_amount"].ToString()!="")
				{
					model.Contract_amount=decimal.Parse(ds.Tables[0].Rows[0]["Contract_amount"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Pay_cycle"]!=null && ds.Tables[0].Rows[0]["Pay_cycle"].ToString()!="")
				{
					model.Pay_cycle=int.Parse(ds.Tables[0].Rows[0]["Pay_cycle"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Start_date"]!=null && ds.Tables[0].Rows[0]["Start_date"].ToString()!="")
				{
					model.Start_date=ds.Tables[0].Rows[0]["Start_date"].ToString();
				}
				if(ds.Tables[0].Rows[0]["End_date"]!=null && ds.Tables[0].Rows[0]["End_date"].ToString()!="")
				{
					model.End_date=ds.Tables[0].Rows[0]["End_date"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Sign_date"]!=null && ds.Tables[0].Rows[0]["Sign_date"].ToString()!="")
				{
					model.Sign_date=ds.Tables[0].Rows[0]["Sign_date"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Customer_Contractor"]!=null && ds.Tables[0].Rows[0]["Customer_Contractor"].ToString()!="")
				{
					model.Customer_Contractor=ds.Tables[0].Rows[0]["Customer_Contractor"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Our_Contractor_depid"]!=null && ds.Tables[0].Rows[0]["Our_Contractor_depid"].ToString()!="")
				{
					model.Our_Contractor_depid=int.Parse(ds.Tables[0].Rows[0]["Our_Contractor_depid"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Our_Contractor_depname"]!=null && ds.Tables[0].Rows[0]["Our_Contractor_depname"].ToString()!="")
				{
					model.Our_Contractor_depname=ds.Tables[0].Rows[0]["Our_Contractor_depname"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Our_Contractor_id"]!=null && ds.Tables[0].Rows[0]["Our_Contractor_id"].ToString()!="")
				{
					model.Our_Contractor_id=int.Parse(ds.Tables[0].Rows[0]["Our_Contractor_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Our_Contractor_name"]!=null && ds.Tables[0].Rows[0]["Our_Contractor_name"].ToString()!="")
				{
					model.Our_Contractor_name=ds.Tables[0].Rows[0]["Our_Contractor_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Creater_id"]!=null && ds.Tables[0].Rows[0]["Creater_id"].ToString()!="")
				{
					model.Creater_id=int.Parse(ds.Tables[0].Rows[0]["Creater_id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Creater_name"]!=null && ds.Tables[0].Rows[0]["Creater_name"].ToString()!="")
				{
					model.Creater_name=ds.Tables[0].Rows[0]["Creater_name"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Create_time"]!=null && ds.Tables[0].Rows[0]["Create_time"].ToString()!="")
				{
					model.Create_time=DateTime.Parse(ds.Tables[0].Rows[0]["Create_time"].ToString());
				}
				if(ds.Tables[0].Rows[0]["Main_Content"]!=null && ds.Tables[0].Rows[0]["Main_Content"].ToString()!="")
				{
					model.Main_Content=ds.Tables[0].Rows[0]["Main_Content"].ToString();
				}
				if(ds.Tables[0].Rows[0]["Remarks"]!=null && ds.Tables[0].Rows[0]["Remarks"].ToString()!="")
				{
					model.Remarks=ds.Tables[0].Rows[0]["Remarks"].ToString();
				}
				if(ds.Tables[0].Rows[0]["File_serialnumber"]!=null && ds.Tables[0].Rows[0]["File_serialnumber"].ToString()!="")
				{
					model.File_serialnumber=ds.Tables[0].Rows[0]["File_serialnumber"].ToString();
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
			strSql.Append("select id,Contract_name,Serialnumber,Customer_id,Customer_name,C_depid,C_depname,C_empid,C_empname,Contract_amount,Pay_cycle,Start_date,End_date,Sign_date,Customer_Contractor,Our_Contractor_depid,Our_Contractor_depname,Our_Contractor_id,Our_Contractor_name,Creater_id,Creater_name,Create_time,Main_Content,Remarks,File_serialnumber,isDelete,Delete_time ");
			strSql.Append(" FROM CRM_contract ");
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
			strSql.Append(" id,Contract_name,Serialnumber,Customer_id,Customer_name,C_depid,C_depname,C_empid,C_empname,Contract_amount,Pay_cycle,Start_date,End_date,Sign_date,Customer_Contractor,Our_Contractor_depid,Our_Contractor_depname,Our_Contractor_id,Our_Contractor_name,Creater_id,Creater_name,Create_time,Main_Content,Remarks,File_serialnumber,isDelete,Delete_time ");
			strSql.Append(" FROM CRM_contract ");
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
			strSql.Append(" top " + PageSize + " * FROM CRM_contract ");
			strSql.Append(" WHERE id not in ( SELECT top " + (PageIndex - 1) * PageSize + " id FROM CRM_contract ");
			strSql.Append(" where " + strWhere + " order by " + filedOrder + " ) ");
			strSql1.Append(" select count(id) FROM CRM_contract ");
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
        /// 同比环比
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="idlist"></param>
        /// <returns></returns>
        public DataSet Compared_empcuscontract(string year1, string month1, string year2, string month2, string idlist)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select hr_employee.name as yy,");
            strSql.Append(" SUM(case when YEAR( CRM_contract.Create_time)=('" + year1 + "') and MONTH(CRM_contract.Create_time)=('" + month1 + "') then 1 else 0 end) as dt1, ");
            strSql.Append(" SUM(case when YEAR( CRM_contract.Create_time)=('" + year2 + "') and MONTH(CRM_contract.Create_time)=('" + month2 + "') then 1 else 0 end) as dt2 ");
            strSql.Append(" from hr_employee left outer join CRM_contract ");
            strSql.Append(" on hr_employee.ID=CRM_contract.Our_Contractor_id ");
            strSql.Append(" where hr_employee.ID in " + idlist);
            strSql.Append(" group by hr_employee.name,hr_employee.ID ");
            strSql.Append(" order by hr_employee.ID");

            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 客户成交统计
        /// </summary>
        public DataSet report_empcontract(int year, string idlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select name,yy,isnull([1],0) as 'm1',isnull([2],0) as 'm2',isnull([3],0) as 'm3',isnull([4],0) as 'm4',isnull([5],0) as 'm5',isnull([6],0) as 'm6',");
            strSql.Append(" isnull([7],0) as 'm7',isnull([8],0) as 'm8',isnull([9],0) as 'm9',isnull([10],0) as 'm10',isnull([11],0) as 'm11',isnull([12],0) as 'm12' ");
            strSql.Append(" from");
            strSql.Append(" (SELECT   hr_employee.ID, hr_employee.name, COUNT(derivedtbl_1.id) AS cn, YEAR(derivedtbl_1.Sign_date) AS yy, ");
            strSql.Append(" MONTH(derivedtbl_1.Sign_date) AS mm");
            strSql.Append(" FROM      hr_employee LEFT OUTER JOIN");
            strSql.Append("  (SELECT   id, Our_Contractor_id, Sign_date");
            strSql.Append("  FROM CRM_contract");
            strSql.Append("  where ISNULL(isdelete,0)=0 and (YEAR(Sign_date) = " + year + ")) AS derivedtbl_1 ON hr_employee.ID = derivedtbl_1.Our_Contractor_id");
            strSql.Append(" WHERE hr_employee.ID in " + idlist);

            strSql.Append(" GROUP BY hr_employee.ID, hr_employee.name, YEAR(derivedtbl_1.Sign_date), MONTH(derivedtbl_1.Sign_date)) as tt");
            strSql.Append(" pivot");
            strSql.Append(" (sum(cn) for mm in ([1],[2],[3],[4],[5],[6],[7],[8],[9],[10],[11],[12]))");
            strSql.Append(" as pvt");

            return DbHelperSQL.Query(strSql.ToString());
        }
		#endregion  Method
	}
}

