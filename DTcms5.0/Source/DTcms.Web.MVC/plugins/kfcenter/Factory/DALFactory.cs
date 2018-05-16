using DTcms.Web.Mvc.Plugin.KfCenter.IDal;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Factory
{
    /// <summary>
    /// DAL工厂，读取App.config的配置文件实例化相应的DAL对象
    /// </summary>
    public class DALFactory<T>
    {
        /// <summary>
        /// 账套
        /// </summary>
        /// <returns></returns>
        public static IkfActSetDAL<T> CreatekfActSetDAL()
        {
            return new DTcms.Web.Mvc.Plugin.KfCenter.SQLServerDAL.kfActSetDAL<T>();
        }
        /// <summary>
        /// 账套组
        /// </summary>
        /// <returns></returns>
        public static IkfActGroupDAL<T> CreatekfActGroupDAL()
        {
            return new DTcms.Web.Mvc.Plugin.KfCenter.SQLServerDAL.kfActGroupDAL<T>();
        }
    }

    public class DALFactory
    {
        /// <summary>
        /// 账套
        /// </summary>
        /// <returns></returns>
        public static IkfActSetDAL<DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActSet> CreatekfActSetDAL()
        {
            return new DTcms.Web.Mvc.Plugin.KfCenter.SQLServerDAL.kfActSetDAL<DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActSet>();
        }
        /// <summary>
        /// 账套组
        /// </summary>
        /// <returns></returns>
        public static IkfActGroupDAL<DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActGroup> CreatekfActGroupDAL()
        {
            return new DTcms.Web.Mvc.Plugin.KfCenter.SQLServerDAL.kfActGroupDAL<DTcms.Web.Mvc.Plugin.KfCenter.Models.kfActGroup>();
        }
    }
}
