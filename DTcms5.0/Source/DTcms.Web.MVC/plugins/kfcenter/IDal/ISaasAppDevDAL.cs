using Kf.WebApi.OAth.Models;

namespace Kf.WebApi.OAth.IDal
{
    public interface ISaasAppDevDAL<T> : IBaseDAL<SaasAppDev>
    {
        #region Customer Define
        /// <summary>
        /// 根据AppID和AppSecret获取SaasAppDev
        /// </summary>
        /// <param name="AppID"></param>
        /// <param name="AppSeret"></param>
        /// <returns></returns>
        T GetEntityByAppIdAndAppSecret(string AppID,string AppSeret);

        /// <summary>
        /// 根据AppID和AppSecret
        /// </summary>
        /// <param name="AppID"></param>
        /// <returns></returns>
        T GetEntityByAppId(string AppID);
        #endregion
    }
}
