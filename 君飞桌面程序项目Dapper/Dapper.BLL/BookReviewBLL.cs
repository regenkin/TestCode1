using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper.Model;
using Dapper.Factory;
using Dapper.IDAL;

namespace Dapper.BLL
{
    public class BookReviewBLL
    {
        private static readonly IBookReviewDAL dal = DALFactory.CreateBookReviewDAL();

        public bool Insert(BookReview bookReview)
        {
            return dal.Insert(bookReview) > 0 ? true : false;
        }

        public bool Update(BookReview bookReview)
        {
            return dal.Update(bookReview) > 0 ? true : false;
        }

        public bool Delete(BookReview bookReview)
        {
            return dal.Delete(bookReview) > 0 ? true : false;
        }

        public bool Delete(int? id)
        {
            return dal.Delete(id) > 0 ? true : false;
        }

        public IList<BookReview> GetList()
        {
            return dal.GetList();
        }

        public BookReview GetEntity(int? id)
        {
            return dal.GetEntity(id);
        }
    }
}
