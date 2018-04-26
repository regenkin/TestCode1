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
    public class BookReviewDAL : IBookReviewDAL
    {

        private static readonly IDbConnection conn = ConnectionFactory.CreateConnection();

        public int? Insert(BookReview bookReview)
        {
            using (conn)
            {
                string query = "INSERT INTO BookReview(BookId,Content)VALUES(@bookId,@content)";
                return conn.Execute(query, bookReview);
            }
        }

        public int? Update(BookReview bookReview)
        {
            using (conn)
            {
                string query = "UPDATE BookReview SET  BookId=@bookId,Content=@content  WHERE id =@id";
                return conn.Execute(query, bookReview);
            }
        }

        public int? Delete(BookReview bookReview)
        {
            using (conn)
            {
                string query = "DELETE FROM BookReview WHERE id = @id";
                return conn.Execute(query, bookReview);
            }
        }

        public int? Delete(int? id)
        {
            using (conn)
            {
                string query = "DELETE FROM BookReview WHERE id = @id";
                return conn.Execute(query, new { id = id });
            }
        }

        public IList<BookReview> GetList()
        {
            using (conn)
            {
                string query = "SELECT * FROM BookReview";
                return conn.Query<BookReview>(query).ToList();
            }
        }

        public BookReview GetEntity(int? id)
        {
            BookReview br;
            string query = "SELECT * FROM BookReview WHERE id = @id";
            using (conn)
            {
                br = conn.Query<BookReview, Book, BookReview>(query,
                  (bookReview, book) =>
                  {
                      bookReview.AssoicationWithBook = book;
                      return bookReview;
                  }, new { id = id }).SingleOrDefault();
                return br;
            }
        }
    }
}
