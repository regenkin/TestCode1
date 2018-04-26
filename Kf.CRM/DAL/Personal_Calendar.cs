using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// 数据访问类:Personal_Calendar
	/// </summary>
	public partial class Personal_Calendar
	{
		public Personal_Calendar()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("Id", "Personal_Calendar"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int Id)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Personal_Calendar");
			strSql.Append(" where Id=@Id ");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)};
			parameters[0].Value = Id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(KfCrm.Model.Personal_Calendar model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Personal_Calendar(");
			strSql.Append("emp_id,emp_name,companyid,Subject,Location,MasterId,Description,CalendarType,StartTime,EndTime,IsAllDayEvent,HasAttachment,Category,InstanceType,Attendees,AttendeeNames,OtherAttendee,UPAccount,UPName,UPTime,RecurringRule)");
			strSql.Append(" values (");
			strSql.Append("@emp_id,@emp_name,@companyid,@Subject,@Location,@MasterId,@Description,@CalendarType,@StartTime,@EndTime,@IsAllDayEvent,@HasAttachment,@Category,@InstanceType,@Attendees,@AttendeeNames,@OtherAttendee,@UPAccount,@UPName,@UPTime,@RecurringRule)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@emp_id", SqlDbType.Int,4),
					new SqlParameter("@emp_name", SqlDbType.VarChar,250),
					new SqlParameter("@companyid", SqlDbType.Int,4),
					new SqlParameter("@Subject", SqlDbType.VarChar,4000),
					new SqlParameter("@Location", SqlDbType.VarChar,4000),
					new SqlParameter("@MasterId", SqlDbType.Int,4),
					new SqlParameter("@Description", SqlDbType.VarChar,4000),
					new SqlParameter("@CalendarType", SqlDbType.TinyInt,1),
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@IsAllDayEvent", SqlDbType.Bit,1),
					new SqlParameter("@HasAttachment", SqlDbType.Bit,1),
					new SqlParameter("@Category", SqlDbType.VarChar,4000),
					new SqlParameter("@InstanceType", SqlDbType.TinyInt,1),
					new SqlParameter("@Attendees", SqlDbType.VarChar,4000),
					new SqlParameter("@AttendeeNames", SqlDbType.VarChar,4000),
					new SqlParameter("@OtherAttendee", SqlDbType.VarChar,4000),
					new SqlParameter("@UPAccount", SqlDbType.VarChar,250),
					new SqlParameter("@UPName", SqlDbType.VarChar,250),
					new SqlParameter("@UPTime", SqlDbType.DateTime),
					new SqlParameter("@RecurringRule", SqlDbType.VarChar,4000)};
			parameters[0].Value = model.emp_id;
			parameters[1].Value = model.emp_name;
			parameters[2].Value = model.companyid;
			parameters[3].Value = model.Subject;
			parameters[4].Value = model.Location;
			parameters[5].Value = model.MasterId;
			parameters[6].Value = model.Description;
			parameters[7].Value = model.CalendarType;
			parameters[8].Value = model.StartTime;
			parameters[9].Value = model.EndTime;
			parameters[10].Value = model.IsAllDayEvent;
			parameters[11].Value = model.HasAttachment;
			parameters[12].Value = model.Category;
			parameters[13].Value = model.InstanceType;
			parameters[14].Value = model.Attendees;
			parameters[15].Value = model.AttendeeNames;
			parameters[16].Value = model.OtherAttendee;
			parameters[17].Value = model.UPAccount;
			parameters[18].Value = model.UPName;
			parameters[19].Value = model.UPTime;
			parameters[20].Value = model.RecurringRule;

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
		public bool Update(KfCrm.Model.Personal_Calendar model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Personal_Calendar set ");  
			strSql.Append("Subject=@Subject,");  
			strSql.Append("StartTime=@StartTime,");
			strSql.Append("EndTime=@EndTime,");
			strSql.Append("IsAllDayEvent=@IsAllDayEvent"); 
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {  					
					new SqlParameter("@Subject", SqlDbType.VarChar,4000), 					
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),
					new SqlParameter("@IsAllDayEvent", SqlDbType.Bit,1), 					
					new SqlParameter("@Id", SqlDbType.Int,4)};
		
			parameters[0].Value = model.Subject; 			
			parameters[1].Value = model.StartTime;
			parameters[2].Value = model.EndTime;
			parameters[3].Value = model.IsAllDayEvent;			
			parameters[4].Value = model.Id;

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
        /// 更新一条数据
        /// </summary>
        public bool quickUpdate(KfCrm.Model.Personal_Calendar model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Personal_Calendar set ");            
            strSql.Append("MasterId=@MasterId,");           
            strSql.Append("StartTime=@StartTime,");
            strSql.Append("EndTime=@EndTime,");            
            strSql.Append("UPAccount=@UPAccount,");
            strSql.Append("UPTime=@UPTime");           
            strSql.Append(" where Id=@Id");
            SqlParameter[] parameters = {
					new SqlParameter("@MasterId", SqlDbType.Int,4),				
					new SqlParameter("@StartTime", SqlDbType.DateTime),
					new SqlParameter("@EndTime", SqlDbType.DateTime),					
					new SqlParameter("@UPAccount", SqlDbType.VarChar,250),					
					new SqlParameter("@UPTime", SqlDbType.DateTime),
					new SqlParameter("@Id", SqlDbType.Int,4)};           
            parameters[0].Value = model.MasterId;            
            parameters[1].Value = model.StartTime;
            parameters[2].Value = model.EndTime;            
            parameters[3].Value = model.UPAccount;            
            parameters[4].Value = model.UPTime;           
            parameters[5].Value = model.Id;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
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
		public bool Delete(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Personal_Calendar ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
			parameters[0].Value = Id;

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
		public bool DeleteList(string Idlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Personal_Calendar ");
			strSql.Append(" where Id in ("+Idlist + ")  ");
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
		public KfCrm.Model.Personal_Calendar GetModel(int Id)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 Id,emp_id,emp_name,companyid,Subject,Location,MasterId,Description,CalendarType,StartTime,EndTime,IsAllDayEvent,HasAttachment,Category,InstanceType,Attendees,AttendeeNames,OtherAttendee,UPAccount,UPName,UPTime,RecurringRule from Personal_Calendar ");
			strSql.Append(" where Id=@Id");
			SqlParameter[] parameters = {
					new SqlParameter("@Id", SqlDbType.Int,4)
};
			parameters[0].Value = Id;

			KfCrm.Model.Personal_Calendar model=new KfCrm.Model.Personal_Calendar();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
			if(ds.Tables[0].Rows.Count>0)
			{
				if(ds.Tables[0].Rows[0]["Id"].ToString()!="")
				{
					model.Id=int.Parse(ds.Tables[0].Rows[0]["Id"].ToString());
				}
				if(ds.Tables[0].Rows[0]["emp_id"].ToString()!="")
				{
					model.emp_id=int.Parse(ds.Tables[0].Rows[0]["emp_id"].ToString());
				}
				model.emp_name=ds.Tables[0].Rows[0]["emp_name"].ToString();
				if(ds.Tables[0].Rows[0]["companyid"].ToString()!="")
				{
					model.companyid=int.Parse(ds.Tables[0].Rows[0]["companyid"].ToString());
				}
				model.Subject=ds.Tables[0].Rows[0]["Subject"].ToString();
				model.Location=ds.Tables[0].Rows[0]["Location"].ToString();
				if(ds.Tables[0].Rows[0]["MasterId"].ToString()!="")
				{
					model.MasterId=int.Parse(ds.Tables[0].Rows[0]["MasterId"].ToString());
				}
				model.Description=ds.Tables[0].Rows[0]["Description"].ToString();
				if(ds.Tables[0].Rows[0]["CalendarType"].ToString()!="")
				{
					model.CalendarType=int.Parse(ds.Tables[0].Rows[0]["CalendarType"].ToString());
				}
				if(ds.Tables[0].Rows[0]["StartTime"].ToString()!="")
				{
					model.StartTime=DateTime.Parse(ds.Tables[0].Rows[0]["StartTime"].ToString());
				}
				if(ds.Tables[0].Rows[0]["EndTime"].ToString()!="")
				{
					model.EndTime=DateTime.Parse(ds.Tables[0].Rows[0]["EndTime"].ToString());
				}
				if(ds.Tables[0].Rows[0]["IsAllDayEvent"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["IsAllDayEvent"].ToString()=="1")||(ds.Tables[0].Rows[0]["IsAllDayEvent"].ToString().ToLower()=="true"))
					{
						model.IsAllDayEvent=true;
					}
					else
					{
						model.IsAllDayEvent=false;
					}
				}
				if(ds.Tables[0].Rows[0]["HasAttachment"].ToString()!="")
				{
					if((ds.Tables[0].Rows[0]["HasAttachment"].ToString()=="1")||(ds.Tables[0].Rows[0]["HasAttachment"].ToString().ToLower()=="true"))
					{
						model.HasAttachment=true;
					}
					else
					{
						model.HasAttachment=false;
					}
				}
				model.Category=ds.Tables[0].Rows[0]["Category"].ToString();
				if(ds.Tables[0].Rows[0]["InstanceType"].ToString()!="")
				{
					model.InstanceType=int.Parse(ds.Tables[0].Rows[0]["InstanceType"].ToString());
				}
				model.Attendees=ds.Tables[0].Rows[0]["Attendees"].ToString();
				model.AttendeeNames=ds.Tables[0].Rows[0]["AttendeeNames"].ToString();
				model.OtherAttendee=ds.Tables[0].Rows[0]["OtherAttendee"].ToString();
				model.UPAccount=ds.Tables[0].Rows[0]["UPAccount"].ToString();
				model.UPName=ds.Tables[0].Rows[0]["UPName"].ToString();
				if(ds.Tables[0].Rows[0]["UPTime"].ToString()!="")
				{
					model.UPTime=DateTime.Parse(ds.Tables[0].Rows[0]["UPTime"].ToString());
				}
				model.RecurringRule=ds.Tables[0].Rows[0]["RecurringRule"].ToString();
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
			strSql.Append("select Id,emp_id,emp_name,companyid,Subject,Location,MasterId,Description,CalendarType,StartTime,EndTime,IsAllDayEvent,HasAttachment,Category,InstanceType,Attendees,AttendeeNames,OtherAttendee,UPAccount,UPName,UPTime,RecurringRule ");
			strSql.Append(" FROM Personal_Calendar ");
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
			strSql.Append(" Id,emp_id,emp_name,companyid,Subject,Location,MasterId,Description,CalendarType,StartTime,EndTime,IsAllDayEvent,HasAttachment,Category,InstanceType,Attendees,AttendeeNames,OtherAttendee,UPAccount,UPName,UPTime,RecurringRule ");
			strSql.Append(" FROM Personal_Calendar ");
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
			parameters[0].Value = "Personal_Calendar";
			parameters[1].Value = "Id";
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

