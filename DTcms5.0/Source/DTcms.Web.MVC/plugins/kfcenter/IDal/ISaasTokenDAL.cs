using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kf.WebApi.OAth.Models;

namespace Kf.WebApi.OAth.IDal
{
    public interface ISaasTokenDAL<T> : IBaseDAL<SaasToken>
    {
        #region Customer Define
        /// <summary>
        /// 通过UserID和APPID获取实体
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        T GetEntityByAppIDAndUserID(int userid,string appid);

        T GetEntityByToken(string token);
        #endregion
    }
}
