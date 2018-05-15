using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DTcms.Web.Mvc.Plugin.KfCenter.Factory;
using DTcms.Web.Mvc.Plugin.KfCenter.IDal;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Bll
{
    public class BaseBLL<T>
    {
        private static dynamic dal;
        public static dynamic Dal
        {
            get{return dal;}
            set{dal=value;}
        }

        public bool Insert(T model)
        {
            return dal.Insert(model) > 0 ? true : false;
        }

        public bool Update(T model)
        {
            return dal.Update(model) > 0 ? true : false;
        }

        public bool Delete(T model)
        {
            return dal.Delete(model) > 0 ? true : false;
        }

        public bool Delete(int? ID)
        {
            return dal.Delete(ID) > 0 ? true : false;
        }

        public IList<T> GetList()
        {
            return dal.GetList();
        }

        public T GetEntity(int? ID)
        {
            return dal.GetEntity(ID);
        }
    }
}
