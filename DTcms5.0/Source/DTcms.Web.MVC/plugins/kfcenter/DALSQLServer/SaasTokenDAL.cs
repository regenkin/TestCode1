/*****************
Name: SaasTokenDAL
Author: kinfar
Description: 
****************/
using System.Collections.Generic;
using System.Linq;
using Kf.WebApi.OAth.IDal;
using System.Data;
using Kf.WebApi.OAth.Factory;
using Kf.WebApi.OAth.Models;
using Dapper;

namespace Kf.WebApi.OAth.SQLServerDAL
{
    public class SaasTokenDAL<T> : ISaasTokenDAL<T>
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

        public int? Insert(SaasToken saastoken)
        {
            using (Conn)
            {
                string query = "INSERT INTO saas_Token([UserID],[AppID],[Token],[AccessToken],[expire])VALUES(@UserID,@AppID,@Token,@AccessToken,@expire)";
                return conn.Execute(query, saastoken);
            }
        }

        public int? Update(SaasToken saastoken)
        {
            using (Conn)
            {
                string query = "UPDATE saas_Token SET [UserID]=@UserID,[AppID]=@AppID,[Token]=@Token,[AccessToken]=@AccessToken,[expire]=@expire  WHERE ID =@ID";
                return conn.Execute(query, saastoken);
            }
        }

        public int? Delete(SaasToken saastoken)
        {
            using (Conn)
            {
                string query = "DELETE FROM saas_Token WHERE ID = @ID";
                return conn.Execute(query, saastoken);
            }
        }

        public int? Delete(int? id)
        {
            using (Conn)
            {
                string query = "DELETE FROM saas_Token WHERE ID = @ID";
                return conn.Execute(query, new { ID = id });
            }
        }

        public IList<SaasToken> GetList()
        {
            using (Conn)
            {
                string query = "SELECT  top 1 * FROM saas_Token";
                return conn.Query<SaasToken>(query).ToList();
            }
        }

        public SaasToken GetEntity(int? id)
        {
            SaasToken entity;
            string query = "SELECT top 1 * FROM saas_Token WHERE ID = @ID";
            using (Conn)
            {
                entity = conn.Query<SaasToken>(query, new { ID = id }).SingleOrDefault();
                return entity;
            }
        }
        #endregion

        #region Customer Define
        public T GetEntityByAppIDAndUserID(int userid,string appid)
        {
            T entity;
            string query = "SELECT top 1 t1.*,t2.CallBackUrl FROM saas_Token t1 left join saas_AppDev t2 on t2.AppID=t1.AppID WHERE t1.UserID=@UserID and t1.AppID = @AppID";
            using (Conn)
            {
                entity = conn.Query<T>(query, new { UserID = userid, AppID = appid }).SingleOrDefault();
                return entity;
            }
        }

        public T GetEntityByToken(string token)
        {
            T entity;
            string query = "SELECT top 1 * FROM saas_Token WHERE Token=@Token";
            using (Conn)
            {
                entity = conn.Query<T>(query, new { Token = token}).SingleOrDefault();
                return entity;
            }
        }
        #endregion
    }
}

