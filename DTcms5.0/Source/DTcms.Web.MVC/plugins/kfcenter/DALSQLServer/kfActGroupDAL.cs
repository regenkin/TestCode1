/*****************
Name: kfActGroupDAL
Author: Kinfar
Description:数据中心账套组
****************/
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using DTcms.Web.Mvc.Plugin.KfCenter.Factory;
using DTcms.Web.Mvc.Plugin.KfCenter.IDal;

namespace DTcms.Web.Mvc.Plugin.KfCenter.SQLServerDAL
{
    public class kfActGroupDAL<T> : IkfActGroupDAL<T>
    {
        #region Pufang Auto Create
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

        public int? Insert(T kfactgroup)
        {
            using (Conn)
            {
                string query = "INSERT INTO kfActGroup([ParentGroupKey],[ActGroupNum],[ActGroupName])VALUES(@ParentGroupKey,@ActGroupNum,@ActGroupName)";
                return conn.Execute(query, kfactgroup);
            }
        }

        public int? Update(T kfactgroup)
        {
            using (Conn)
            {
                string query = "UPDATE kfActGroup SET [ParentGroupKey]=@ParentGroupKey,[ActGroupNum]=@ActGroupNum,[ActGroupName]=@ActGroupName  WHERE ID =@ID";
                return conn.Execute(query, kfactgroup);
            }
        }

        public int? Delete(T kfactgroup)
        {
            using (Conn)
            {
                string query = "DELETE FROM kfActGroup WHERE ID = @ID";
                return conn.Execute(query, kfactgroup);
            }
        }

        public int? Delete(int? id)
        {
            using (Conn)
            {
                string query = "DELETE FROM kfActGroup WHERE ID = @ID";
                return conn.Execute(query, new { ID = id });
            }
        }

        public IList<T> GetList()
        {
            using (Conn)
            {
                string query = "SELECT  top 20 * FROM kfActGroup";
                return conn.Query<T>(query).ToList();
            }
        }

        public T GetEntity(int? id)
        {
            T entity;
            string query = "SELECT top 1 * FROM kfActGroup WHERE ID = @ID";
            using (Conn)
            {
                entity = conn.Query<T>(query, new { ID = id }).SingleOrDefault();
                return entity;
            }
        }
        #endregion

        #region Customer Define

        #endregion
    }
}

