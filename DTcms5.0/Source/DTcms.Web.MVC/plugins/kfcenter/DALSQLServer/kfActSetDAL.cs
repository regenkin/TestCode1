/*****************
Name: kfActSetDAL
Author: kinfar
Description: 
****************/
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DTcms.Web.Mvc.Plugin.KfCenter.Factory;
using DTcms.Web.Mvc.Plugin.KfCenter.IDal;
using System.Text;
using System;

namespace DTcms.Web.Mvc.Plugin.KfCenter.SQLServerDAL
{
    public class kfActSetDAL<T> : IkfActSetDAL<T>
    {
        #region kinfar Auto Create
        private IDbConnection conn;
        private IDbConnection Conn
        {
            get
            {
                if (conn == null || conn.State != ConnectionState.Open)
                {
                    conn = ConnectionFactory.CreateConnection();
                }
                return conn;
            }
        }

        public int? Insert(T kfactset)
        {
            using (Conn)
            {
                string query = "INSERT INTO kfActSet([ActsetNum],[ActsetName],[ActsetDBName],[ActsetType],[ActSetGroupKey],[LoginType],[LoginUserName],[LoginPwd],[DBServerName],[CreateDate],[NewBackUpDate],[DBGUID],[DBVersion],[Visible],[UIStyle],[LimitCount],[DBMaxSize],[EndDate])VALUES(@ActsetNum,@ActsetName,@ActsetDBName,@ActsetType,@ActSetGroupKey,@LoginType,@LoginUserName,@LoginPwd,@DBServerName,@CreateDate,@NewBackUpDate,@DBGUID,@DBVersion,@Visible,@UIStyle,@LimitCount,@DBMaxSize,@EndDate)";
                return conn.Execute(query, kfactset);
            }
        }

        public int? Update(T kfactset)
        {
            using (Conn)
            {
                string query = "UPDATE kfActSet SET [ActsetNum]=@ActsetNum,[ActsetName]=@ActsetName,[ActsetDBName]=@ActsetDBName,[ActsetType]=@ActsetType,[ActSetGroupKey]=@ActSetGroupKey,[LoginType]=@LoginType,[LoginUserName]=@LoginUserName,[LoginPwd]=@LoginPwd,[DBServerName]=@DBServerName,[CreateDate]=@CreateDate,[NewBackUpDate]=@NewBackUpDate,[DBGUID]=@DBGUID,[DBVersion]=@DBVersion,[Visible]=@Visible,[UIStyle]=@UIStyle,[LimitCount]=@LimitCount,[DBMaxSize]=@DBMaxSize,[EndDate]=@EndDate  WHERE ID =@ID";
                return conn.Execute(query, kfactset);
            }
        }

        public int? Delete(T kfactset)
        {
            using (Conn)
            {
                string query = "DELETE FROM kfActSet WHERE ID = @ID";
                return conn.Execute(query, kfactset);
            }
        }

        public int? Delete(int? id)
        {
            using (Conn)
            {
                string query = "DELETE FROM kfActSet WHERE ID = @ID";
                return conn.Execute(query, new { ID = id });
            }
        }

        public IList<T> GetList()
        {
            using (Conn)
            {
                string query = "SELECT  top 20 * FROM kfActSet";
                return conn.Query<T>(query).ToList();
            }
        }

        public T GetEntity(int? id)
        {
            T entity;
            string query = "SELECT top 1 * FROM kfActSet WHERE ID = @ID";
            using (Conn)
            {
                entity = conn.Query<T>(query, new { ID = id }).SingleOrDefault();
                return entity;
            }
        }
        #endregion

        #region Customer Define
        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM kfActSet");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperSQLByConStr.GetSingle(DTcms.Common.PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQLByConStr.Query(DTcms.Common.PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }
        #endregion
    }
}

