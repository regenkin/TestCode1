using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kf.WebApi.OAth.Models;

namespace Kf.WebApi.OAth.IDal
{
    public interface ISaasUserDAL<T> : IBaseDAL<SaasUser>
    {
        #region Customer Define
        T GetBaseUserInfo(string accesstoken);
        #endregion
    }
}
