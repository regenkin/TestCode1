using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DTcms.BLL {
   public class feedback {
      // Fields
      private readonly DAL.feedback dal;
      private readonly Model.sysconfig sysConfig = new BLL.sysconfig().loadConfig(); //获得站点配置信息

      // Methods
      public feedback() {
         dal = new DAL.feedback(sysConfig.sysdatabaseprefix);
      }

      public int Add(Model.feedback model) {
         return dal.Add(model);
      }

      public bool Delete(int id) {
         return dal.Delete(id);
      }

      public bool Exists(int id) {
         return dal.Exists(id);
      }

      public DataSet GetList(int Top, string strWhere) {
         return dal.GetList(Top, strWhere);
      }

      public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount) {
         return dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);
      }

      public Model.feedback GetModel(int id) {
         return dal.GetModel(id);
      }

      public bool Update(Model.feedback model) {
         return dal.Update(model);
      }

      public void UpdateField(int id, string strValue) {
         dal.UpdateField(id, strValue);
      }
   }
}
