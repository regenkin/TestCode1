using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dapper.IDAL
{
    public interface IBaseDAL<T>
    {
         #region CRUD

        int? Insert(T book);

        int? Update(T book);

        int? Delete(T book);

        int? Delete(int? id);

        IList<T> GetList();

        /// <summary>
        /// 根据主键获得Book
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetEntity(int? id);

        #endregion
    }
}
