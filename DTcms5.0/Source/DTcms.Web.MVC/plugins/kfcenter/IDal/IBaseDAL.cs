using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTcms.Web.Mvc.Plugin.KfCenter.IDal
{
    public interface IBaseDAL<T>
    {
         #region CRUD

        int? Insert(T entity);

        int? Update(T entity);

        int? Delete(T entity);

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
