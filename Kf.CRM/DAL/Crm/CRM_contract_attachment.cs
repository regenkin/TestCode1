using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;//Please add references
namespace KfCrm.DAL
{
    /// <summary>
    /// 数据访问类:CRM_contract_attachment
    /// </summary>
    public partial class CRM_contract_attachment
    {
        public CRM_contract_attachment()
        { }
        #region  Method

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(KfCrm.Model.CRM_contract_attachment model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into CRM_contract_attachment(");
            strSql.Append("contract_id,page_id,file_id,file_name,real_name,file_size,create_id,create_name,create_date)");
            strSql.Append(" values (");
            strSql.Append("@contract_id,@page_id,@file_id,@file_name,@real_name,@file_size,@create_id,@create_name,@create_date)");
            SqlParameter[] parameters = {
					new SqlParameter("@contract_id", SqlDbType.Int,4),
					new SqlParameter("@page_id", SqlDbType.VarChar,250),
					new SqlParameter("@file_id", SqlDbType.VarChar,250),
					new SqlParameter("@file_name", SqlDbType.VarChar,250),
					new SqlParameter("@real_name", SqlDbType.VarChar,250),
					new SqlParameter("@file_size", SqlDbType.Int,4),
					new SqlParameter("@create_id", SqlDbType.Int,4),
					new SqlParameter("@create_name", SqlDbType.VarChar,250),
					new SqlParameter("@create_date", SqlDbType.DateTime)};
            parameters[0].Value = model.contract_id;
            parameters[1].Value = model.page_id;
            parameters[2].Value = model.file_id;
            parameters[3].Value = model.file_name;
            parameters[4].Value = model.real_name;
            parameters[5].Value = model.file_size;
            parameters[6].Value = model.create_id;
            parameters[7].Value = model.create_name;
            parameters[8].Value = model.create_date;

            DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string wherestr)
        {
            //该表无主键信息，请自定义主键/条件字段
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from CRM_contract_attachment ");
            strSql.Append(" where " + wherestr);
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
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * ");
            strSql.Append(" FROM CRM_contract_attachment ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.Append(" FROM CRM_contract_attachment ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 更新ID
        /// </summary>
        public bool UpdateMailid(int contract_id, string page_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update CRM_contract_attachment set ");
            strSql.Append("contract_id=" + contract_id);
            strSql.Append(" where ");
            strSql.Append("page_id=" + page_id);

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
        #endregion  Method
    }
}

