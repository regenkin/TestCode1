using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
	/// <summary>
	/// ���ݷ�����:Sys_online
	/// </summary>
	public partial class Sys_online
	{
		public Sys_online()
		{}
		#region  Method



		/// <summary>
		/// ����һ������
		/// </summary>
		public void Add(KfCrm.Model.Sys_online model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Sys_online(");
			strSql.Append("UserID,UserName,LastLogTime)");
			strSql.Append(" values (");
			strSql.Append("@UserID,@UserName,@LastLogTime)");
			SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@LastLogTime", SqlDbType.DateTime)};
			parameters[0].Value = model.UserID;
			parameters[1].Value = model.UserName;
			parameters[2].Value = model.LastLogTime;

			DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
		}
        /// <summary>
        /// ����һ������
        /// </summary>
        public bool Update(KfCrm.Model.Sys_online model, string where)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Sys_online set ");
            strSql.Append("UserID=@UserID,");
            strSql.Append("UserName=@UserName,");
            strSql.Append("LastLogTime=@LastLogTime");
            strSql.Append(" where " + where);
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.VarChar,50),
					new SqlParameter("@LastLogTime", SqlDbType.DateTime)
                                        };
            parameters[0].Value = model.UserID;
            parameters[1].Value = model.UserName;
            parameters[2].Value = model.LastLogTime;

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
        /// ɾ��һ������
        /// </summary>
        public bool Delete(string wherestr)
        {
            //�ñ���������Ϣ�����Զ�������/�����ֶ�
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Sys_online ");
            strSql.Append(" where " + wherestr + "  ");
            SqlParameter[] parameters = {
};

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
		/// ��������б�
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select UserID,UserName,LastLogTime ");
			strSql.Append(" FROM Sys_online ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// ���ǰ��������
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" UserID,UserName,LastLogTime ");
			strSql.Append(" FROM Sys_online ");
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

