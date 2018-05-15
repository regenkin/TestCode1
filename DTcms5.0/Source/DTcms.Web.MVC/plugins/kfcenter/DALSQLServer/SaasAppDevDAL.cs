/*****************
Name: SaasAppDevDAL
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
    public class SaasAppDevDAL<T> : ISaasAppDevDAL<T>
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

        public int? Insert(SaasAppDev saasappdev)
        {
            using (Conn)
            {
                string query = "INSERT INTO saas_AppDev([AppID],[AppName],[AppSecret],[Token],[CallBackUrl],[EncodingAESKey],[Type])VALUES(@AppID,@AppName,@AppSecret,@Token,@CallBackUrl,@EncodingAESKey,@Type)";
                return conn.Execute(query, saasappdev);
            }
        }

        public int? Update(SaasAppDev saasappdev)
        {
            using (Conn)
            {
                string query = "UPDATE saas_AppDev SET [AppID]=@AppID,[AppName]=@AppName,[AppSecret]=@AppSecret,[Token]=@Token,[CallBackUrl]=@CallBackUrl,[EncodingAESKey]=@EncodingAESKey,[Type]=@Type  WHERE ID =@ID";
                return conn.Execute(query, saasappdev);
            }
        }

        public int? Delete(SaasAppDev saasappdev)
        {
            using (Conn)
            {
                string query = "DELETE FROM saas_AppDev WHERE ID = @ID";
                return conn.Execute(query, saasappdev);
            }
        }

        public int? Delete(int? id)
        {
            using (Conn)
            {
                string query = "DELETE FROM saas_AppDev WHERE ID = @ID";
                return conn.Execute(query, new { ID = id });
            }
        }

        public IList<SaasAppDev> GetList()
        {
            using (Conn)
            {
                string query = "SELECT  top 1 * FROM saas_AppDev";
                return conn.Query<SaasAppDev>(query).ToList();
            }
        }

        public SaasAppDev GetEntity(int? id)
        {
            SaasAppDev entity;
            string query = "SELECT top 1 * FROM saas_AppDev WHERE ID = @ID";
            using (Conn)
            {
                entity = conn.Query<SaasAppDev>(query, new { ID = id }).SingleOrDefault();
                return entity;
            }
        }
        #endregion

        #region Customer Define
         public T GetEntityByAppId(string appid)
        {
            T entity;
            string query = "SELECT top 1 * FROM saas_AppDev WHERE AppID = @AppID ";
            using (Conn)
            {
                entity = conn.Query<T>(query, new { AppID = appid }).SingleOrDefault();
                return entity;
            }
        }

        public T GetEntityByAppIdAndAppSecret(string appid, string appseret)
        {
            T entity;
            string query = "SELECT top 1 * FROM saas_AppDev WHERE AppID = @AppID and AppSecret=@AppSecret";
            using (Conn)
            {
                entity = conn.Query<T>(query, new { AppID = appid, AppSecret = appseret }).SingleOrDefault();
                return entity;
            }
        }
        #endregion
    }
}

