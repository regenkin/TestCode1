/*****************
Name: kfActGroupBLL
Author: kinfar
Description:账套组
****************/
using System.Collections.Generic;
using DTcms.Web.Mvc.Plugin.KfCenter.Factory;
using DTcms.Web.Mvc.Plugin.KfCenter.Models;
using DTcms.Web.Mvc.Plugin.KfCenter.IDal;


namespace DTcms.Web.Mvc.Plugin.KfCenter.BLL
{
    public class kfActGroupBLL<T> : BaseBLL<T>
    {
        #region Auto By Pufang Auto Creater
        private static kfActGroupBLL<T> _kfActGroupBLL;
        public static kfActGroupBLL<T> Instance()
        {
            Dal = DALFactory.CreatekfActGroupDAL();
            if (_kfActGroupBLL == null)
            {
                _kfActGroupBLL = new kfActGroupBLL<T>();
            }
            return _kfActGroupBLL;
        }

        private static IkfActGroupDAL<T> dal = DALFactory<T>.CreatekfActGroupDAL();
        #endregion

        #region Customer Define

        #endregion
    }
}
