/*****************
Name: kfActSetBLL
Author: kinfar
Description: 
****************/
using System.Collections.Generic;
using DTcms.Web.Mvc.Plugin.KfCenter.Factory;
using DTcms.Web.Mvc.Plugin.KfCenter.Model;
using DTcms.Web.Mvc.Plugin.KfCenter.IDal;


namespace DTcms.Web.Mvc.Plugin.KfCenter.Bll
{
    public class kfActSetBLL<T> : BaseBLL<T>
    {
        #region Auto By Pufang Auto Creater
        private static kfActSetBLL<T> _kfActSetBLL;
        public static kfActSetBLL<T> Instance()
        {
            Dal = DALFactory.CreatekfActSetDAL();
            if (_kfActSetBLL == null)
            {
                _kfActSetBLL = new kfActSetBLL<T>();
            }
            return _kfActSetBLL;
        }

        private static IkfActSetDAL<T> dal = DALFactory<T>.CreatekfActSetDAL();
        #endregion

        #region Customer Define
        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public System.Data.DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            return dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
        }
        #endregion
    }
}
