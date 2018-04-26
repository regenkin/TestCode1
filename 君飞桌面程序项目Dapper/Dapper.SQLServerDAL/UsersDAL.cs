using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.IDAL;
using Dapper.Factory;
using System.Data;
using Dapper.Model;

namespace Dapper.SQLServerDAL
{
    public class UsersDAL : IUsersDAL
    {

        private static readonly IDbConnection conn = ConnectionFactory.CreateConnection();

        public int? Insert(Users user)
        {
            using (conn)
            {
                string query = "INSERT INTO Users(Number,Name,Password)VALUES(@Number,@Name,@Password)";
                return conn.Execute(query, user);
            }
        }

        public int? Update(Users user)
        {
            using (conn)
            {
                string query = "UPDATE Users SET  Number=@Number,Name=@Name,Password=@Password  WHERE id =@id";
                return conn.Execute(query, user);
            }
        }

        public int? Delete(Users user)
        {
            using (conn)
            {
                string query = "DELETE FROM Users WHERE id = @id";
                return conn.Execute(query, user);
            }
        }

        public int? Delete(int? id)
        {
            using (conn)
            {
                string query = "DELETE FROM Users WHERE id = @id";
                return conn.Execute(query, new { id = id });
            }
        }

        public IList<Users> GetList()
        {
            using (conn)
            {
                string query = "SELECT * FROM Users";
                return conn.Query<Users>(query).ToList();
            }
        }

        public Users GetEntity(int? id)
        {
            Users br;
            string query = "SELECT * FROM Users WHERE id = @id";
            using (conn)
            {
                br = conn.Query<Users>(query, new { id = id }).SingleOrDefault();
                return br;
            }
        }
    }
}
