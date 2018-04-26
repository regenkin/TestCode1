using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using DTcms.Common;

namespace DTcms.BLL {
   /// <summary>
   /// 关联内容
   /// </summary>
   public partial class article_link {
      private readonly Model.sysconfig sysConfig = new BLL.sysconfig().loadConfig(); //获得站点配置信息
      private readonly DAL.article_link dal;
      public article_link() {
         dal = new DAL.article_link(sysConfig.sysdatabaseprefix);
      }

      #region 基本方法
      /// <summary>
      /// 获取一个关联内容对象实体
      /// </summary>
      /// <param name="channel_id">通道Id</param>
      /// <param name="article_id">文章Id</param>
      /// <param name="link_channel_id">关联文章所属通道Id</param>
      /// <param name="link_article_id">关联文章Id</param>
      /// <returns></returns>
      public Model.article_link GetModel(int channel_id, int article_id, int link_channel_id, int link_article_id) {
         return dal.GetModel(channel_id, article_id, link_channel_id, link_article_id);
      }

      /// <summary>
      /// 获取数据实体列表
      /// </summary>
      /// <param name="article_id">文章Id</param>
      /// <returns></returns>
      public List<Model.article_link> GetList(int channel_id, int article_id) {
         return dal.GetList(channel_id, article_id);
      }

      /// <summary>
      /// 获取指定文章所关联文章列表
      /// </summary>
      /// <param name="acticle_id"></param>
      /// <returns></returns>
      public DataTable GetAricleList(int channel_id, int article_id) {
         DataTable result = CreateDataTable();
         List<Model.article_link> linkList = GetList(channel_id, article_id);//获取指定内容所关联内容列表
         foreach (Model.article_link item in linkList) {
            DataRow newrow = result.NewRow();
            newrow["link_id"] = item.id;
            newrow["channel_id"] = item.link_channel_id;
            newrow["id"] = item.link_article_id;
            result.Rows.Add(newrow);
         }
         Dictionary<int, string> dicts = new Dictionary<int, string>();
         foreach (Model.article_link item in linkList) {
            if (dicts.ContainsKey(item.link_channel_id)) {
               dicts[item.link_channel_id] = dicts[item.link_channel_id] + "," + item.link_article_id.ToString();
            }
            else {
               dicts.Add(item.link_channel_id, item.link_article_id.ToString());
            }
         }
         BLL.article bll_article = new article();
         foreach (KeyValuePair<int, string> dict in dicts) {
            string strWhere = "id in(" + dict.Value + ")";
            int link_channel_id = dict.Key;
            DataSet ds = bll_article.GetList(link_channel_id, 99, strWhere, "sort_id");
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows) {
               DataRow[] findrows = result.Select("channel_id=" + dr["channel_id"].ToString() + " and id=" + dr["id"].ToString());
               if (findrows.Length > 0) {
                  DataRow findrow = findrows[0];
                  findrow["category_id"] = dr["category_id"];
                  findrow["title"] = dr["title"];
                  findrow["sort_id"] = dr["sort_id"];
                  findrow["add_time"] = dr["add_time"];
                  findrow["is_msg"] = dr["is_msg"];
                  findrow["is_top"] = dr["is_top"];
                  findrow["is_red"] = dr["is_red"];
                  findrow["is_hot"] = dr["is_hot"];
                  findrow["is_slide"] = dr["is_slide"];
               }
            }
         }
         return result;
      }

      private DataTable CreateDataTable() {
         DataTable result = new DataTable();
         result.Columns.AddRange(new DataColumn[]{
            new DataColumn("link_id", Type.GetType("System.Int32")),
            new DataColumn("channel_id", Type.GetType("System.Int32")),
            new DataColumn("id", Type.GetType("System.Int32")),
            new DataColumn("category_id", Type.GetType("System.Int32")),
            new DataColumn("category_name", Type.GetType("System.String")),
            new DataColumn("title", Type.GetType("System.String")),
            new DataColumn("sort_id", Type.GetType("System.Int32")),
            new DataColumn("add_time", Type.GetType("System.DateTime")),
            new DataColumn("is_msg", Type.GetType("System.Int32")),
            new DataColumn("is_top", Type.GetType("System.Int32")),
            new DataColumn("is_red", Type.GetType("System.Int32")),
            new DataColumn("is_hot", Type.GetType("System.Int32")),
            new DataColumn("is_slide", Type.GetType("System.Int32"))
         });
         foreach (DataColumn dc in result.Columns) {
            dc.AllowDBNull = true;
         }
         return result;
      }

      /// <summary>
      /// 添加一个对象实体
      /// </summary>
      /// <param name="?"></param>
      /// <returns></returns>
      public bool Add(Model.article_link model){
         return dal.Add(model);
      }

      /// <summary>
      /// 删除一个对象实体
      /// </summary>
      /// <param name="model"></param>
      /// <returns></returns>
      public bool Delete(Model.article_link model) {
         return dal.Delete(model.id);
      }

      /// <summary>
      /// 删除一个对象实体
      /// </summary>
      /// <param name="article_id">文章Id</param>
      /// <param name="link_article_id">关联文章Id</param>
      /// <returns></returns>
      public bool Delete(int id) {
         return dal.Delete(id);
      }
      #endregion  Method

   }
}