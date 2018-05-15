/*****************
Name: SaasUserBLL
Author: kinfar
Description: 
****************/
using System.Collections.Generic;
using Kf.WebApi.OAth.Factory;
using Kf.WebApi.OAth.Models;
using Kf.WebApi.OAth.IDal;

namespace Kf.WebApi.OAth.Bll
{
    public class SaasUserBLL<T> : BaseBLL<SaasUser>
    {
        #region Auto By Kinfar Auto Creater
        private static SaasUserBLL<T> _SaasUserBLL;
        public static SaasUserBLL<T> Instance()
        {
            Dal = DALFactory.CreateSaasUserDAL();
            if (_SaasUserBLL == null)
            {
                _SaasUserBLL = new SaasUserBLL<T>();
            }
            return _SaasUserBLL;
        }

        private static ISaasUserDAL<T> dal = DALFactory<T>.CreateSaasUserDAL();
        #endregion

        #region Customer Define
        public T GetBaseUserInfo(string accesstoken)
        {
            return dal.GetBaseUserInfo(accesstoken);
        }
        #endregion


    }
}
