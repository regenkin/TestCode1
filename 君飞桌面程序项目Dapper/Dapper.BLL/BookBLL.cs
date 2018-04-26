using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper.Model;
using Dapper.IDAL;
using Dapper.Factory;

namespace Dapper.BLL
{
    public class BookBLL
    {
        private static readonly IBookDAL dal = DALFactory.CreateBookDAL();

        public bool Insert(Book book)
        {
            return dal.Insert(book) > 0 ? true : false;
        }

        public bool Update(Book book)
        {
            return dal.Update(book) > 0 ? true : false;
        }

        public bool Delete(Book book)
        {
            return dal.Delete(book) > 0 ? true : false;
        }

        public bool Delete(int? id)
        {
            return dal.Delete(id) > 0 ? true : false;
        }

        public IList<Book> GetBookList()
        {
            return dal.GetList();
        }

        public Book GetEntity(int? id)
        {
            return dal.GetEntity(id);
        }

        public Book GetEntityWithRefence(int? id)
        {
            return dal.GetEntityWithRefence(id);
        }
    }
}
