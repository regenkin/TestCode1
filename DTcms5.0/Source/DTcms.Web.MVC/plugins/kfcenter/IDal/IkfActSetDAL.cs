using DTcms.Web.Mvc.Plugin.KfCenter.Model;

namespace DTcms.Web.Mvc.Plugin.KfCenter.IDal
{
    public interface IkfActSetDAL<T> : IBaseDAL<T>
    {
        #region Customer Define
        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        System.Data.DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount);
        #endregion
    }
}