using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper.Model;
using Dapper.Factory;
using Dapper.IDAL;

namespace Dapper.BLL
{
    public class UsersBLL
    {
        private static readonly IUsersDAL dal = DALFactory.CreateUserDAL();

        public bool Insert(Users user)
        {
            return dal.Insert(user) > 0 ? true : false;
        }

        public bool Update(Users user)
        {
            return dal.Update(user) > 0 ? true : false;
        }

        public bool Delete(Users user)
        {
            return dal.Delete(user) > 0 ? true : false;
        }

        public bool Delete(int? id)
        {
            return dal.Delete(id) > 0 ? true : false;
        }

        public IList<Users> GetList()
        {
            return dal.GetList();
        }

        public Users GetEntity(int? id)
        {
            return dal.GetEntity(id);
        }
    }
}
