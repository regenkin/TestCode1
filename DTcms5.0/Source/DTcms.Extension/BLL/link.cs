using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DTcms.Model;

namespace DTcms.BLL {
   public class link {
      // Fields
      private readonly DAL.link dal;
      private readonly Model.sysconfig sysConfig = new BLL.sysconfig().loadConfig(); //获得站点配置信息

      // Methods
      public link() {
         dal = new DAL.link(sysConfig.sysdatabaseprefix);
      }
      public int Add(Model.link model) {
         return this.dal.Add(model);
      }
      public bool Delete(int id) {
         return this.dal.Delete(id);
      }
      public bool Exists(int id) {
         return this.dal.Exists(id);
      }
      public DataSet GetList(string strWhere) {
         return this.dal.GetList(strWhere);
      }
      public DataSet GetList(int Top, string strWhere) {
         return this.dal.GetList(Top, strWhere);
      }
      public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount) {
         return this.dal.GetList(pageSize, pageIndex, strWhere, filedOrder, out recordCount);

      }
      public Model.link GetModel(int id) {
         return this.dal.GetModel(id);
      }
      public bool Update(Model.link model) {
         return this.dal.Update(model);
      }
      public void UpdateField(int id, string strValue) {
         this.dal.UpdateField(id, strValue);
      }
   }
}
