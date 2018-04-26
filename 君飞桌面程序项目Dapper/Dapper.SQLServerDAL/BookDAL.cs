using Dapper;
using Dapper.IDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper.Factory;
using System.Data;
using Dapper.Model;

namespace Dapper.SQLServerDAL
{
    public class BookDAL : IBookDAL
    {
        private static readonly IDbConnection conn = ConnectionFactory.CreateConnection();

        public int? Insert(Model.Book book)
        {
            using (conn)
            {
                string query = "INSERT INTO Book(Name)VALUES(@name)";
                return conn.Execute(query, book);
            }
        }

        public int? Update(Model.Book book)
        {
            using (conn)
            {
                string query = "UPDATE Book SET  Name=@name WHERE id =@id";
                return conn.Execute(query, book);
            }
        }

        public int? Delete(Model.Book book)
        {
            using (conn)
            {
                string query = "DELETE FROM Book WHERE id = @id";
                return conn.Execute(query, book);
            }
        }

        public int? Delete(int? id)
        {
            using (conn)
            {
                string query = "DELETE FROM Book WHERE id = @id";
                return conn.Execute(query, new { id = id });
            }
        }

        public IList<Model.Book> GetList()
        {
            using (conn)
            {
                string query = "SELECT * FROM Book";
                return conn.Query<Book>(query).ToList();
            }
        }

        public Model.Book GetEntity(int? id)
        {
            Book book;
            string query = "SELECT * FROM Book WHERE id = @id";
            using (conn)
            {
                book = conn.Query<Book>(query, new { id = id }).SingleOrDefault();
                return book;
            }
        }

        public Book GetEntityWithRefence(int? id)
        {
            using (conn)
            {
                string query = "SELECT * FROM Book b LEFT JOIN BookReview br ON br.BookId = b.Id WHERE b.id = @id";
                Book lookup = null;
                var b = conn.Query<Book, BookReview, Book>(query,
                    (book, bookReview) =>
                    {
                        if (lookup == null || lookup.Id != book.Id)
                            lookup = book;
                        if (bookReview != null)
                            lookup.Reviews.Add(bookReview);
                        return lookup;
                    }, new { id = id }).Distinct().SingleOrDefault();
                return b;
            }
        }
    }
}
