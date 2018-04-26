using Dapper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dapper.IDAL
{
    public interface IBookDAL:IBaseDAL<Book>
    {
        /// <summary>
        /// 根据主键获得Book包括BookReview
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Book GetEntityWithRefence(int? id);
    }
}
