using DTcms.Web.Mvc.Plugin.KfCenter.IDal;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Factory
{
    /// <summary>
    /// DAL工厂，读取App.config的配置文件实例化相应的DAL对象
    /// </summary>
    public class DALFactory<T>
    {
        public static IkfActSetDAL<T> CreatekfActSetDAL()
        {
            return new DTcms.Web.Mvc.Plugin.KfCenter.SQLServerDAL.kfActSetDAL<T>();
        }
    }

    public class DALFactory
    {
        public static IkfActSetDAL<DTcms.Web.Mvc.Plugin.KfCenter.Model.kfActSet> CreatekfActSetDAL()
        {
            return new DTcms.Web.Mvc.Plugin.KfCenter.SQLServerDAL.kfActSetDAL<DTcms.Web.Mvc.Plugin.KfCenter.Model.kfActSet>();
        }
    }
}
