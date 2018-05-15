/*****************
Name: SaasUserDAL
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
    public class SaasUserDAL<T> : ISaasUserDAL<T>
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

        public int? Insert(SaasUser saasuser)
        {
            using (Conn)
            {
                string query = "INSERT INTO saas_User([Name],[Password],[NiName],[Type],[CreateTime],[OpenID],[Mobile],[Email],[Status],[IsEmailValidated],[Sex],[Tag])VALUES(@Name,@Password,@NiName,@Type,@CreateTime,@OpenID,@Mobile,@Email,@Status,@IsEmailValidated,@Sex,@Tag)";
                return conn.Execute(query, saasuser);
            }
        }

        public int? Update(SaasUser saasuser)
        {
            using (Conn)
            {
                string query = "UPDATE saas_User SET [Name]=@Name,[Password]=@Password,[NiName]=@NiName,[Type]=@Type,[CreateTime]=@CreateTime,[OpenID]=@OpenID,[Mobile]=@Mobile,[Email]=@Email,[Status]=@Status,[IsEmailValidated]=@IsEmailValidated,[Sex]=@Sex,[Tag]=@Tag  WHERE ID =@ID";
                return conn.Execute(query, saasuser);
            }
        }

        public int? Delete(SaasUser saasuser)
        {
            using (Conn)
            {
                string query = "DELETE FROM saas_User WHERE ID = @ID";
                return conn.Execute(query, saasuser);
            }
        }

        public int? Delete(int? id)
        {
            using (Conn)
            {
                string query = "DELETE FROM saas_User WHERE ID = @ID";
                return conn.Execute(query, new { ID = id });
            }
        }

        public IList<SaasUser> GetList()
        {
            using (Conn)
            {
                string query = "SELECT  top 1 * FROM saas_User";
                return conn.Query<SaasUser>(query).ToList();
            }
        }

        public SaasUser GetEntity(int? id)
        {
            SaasUser entity;
            string query = "SELECT top 1 * FROM saas_User WHERE ID = @ID";
            using (Conn)
            {
                entity = conn.Query<SaasUser>(query, new { ID = id }).SingleOrDefault();
                return entity;
            }
        }
        #endregion

        #region Customer Define
        public T GetBaseUserInfo(string accesstoken)
        {
            T entity;
            string query = "SELECT top 1 t1.* FROM saas_User t1 left join saas_Token t2 WHERE t1.ID=t2.UserID and t2.AccessToken=@AccessToken";
            using (Conn)
            {
                entity = conn.Query<T>(query, new { AccessToken = accesstoken }).SingleOrDefault();
                return entity;
            }
        }
        #endregion
    }
}

