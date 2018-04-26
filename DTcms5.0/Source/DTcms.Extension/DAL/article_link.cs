using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.DAL {
   /// <summary>
   /// 内容关联
   /// </summary>
   public partial class article_link {
      private string databaseprefix; //数据库表名前缀
      public article_link(string _databaseprefix) {
         databaseprefix = _databaseprefix;
      }

      /// <summary>
      /// 得到一个对象实体
      /// </summary>
      /// <param name="article_id"></param>
      /// <param name="link_article_id"></param>
      /// <returns></returns>
      public Model.article_link GetModel(int channel_id,int article_id,int link_channel_id, int link_article_id) {
         Model.article_link result = null;
         StringBuilder strSql = new StringBuilder();
         strSql.Append("select id,site_id,channel_id,article_id,link_site_id,link_channel_id,link_article_id,link_category_id,add_time");
         strSql.Append(" from " + databaseprefix + "article_link");
         strSql.Append(" where channel_id=@channel_id and article_id=@article_id and link_channel_id=@link_channel_id and link_article_id=@link_article_id");
         SqlParameter[] parameters = {
                                        new SqlParameter("@channel_id", SqlDbType.Int, 4),
                                        new SqlParameter("@article_id", SqlDbType.Int, 4),
                                        new SqlParameter("@link_channel_id", SqlDbType.Int,4),
                                        new SqlParameter("@link_article_id", SqlDbType.Int,4)
                                     };
         parameters[0].Value = channel_id;
         parameters[1].Value = article_id;
         parameters[2].Value = link_channel_id;
         parameters[3].Value = link_article_id;
         SqlDataReader reader = DbHelperSQL.ExecuteReader(strSql.ToString(), parameters);
         if (reader.Read()) {
            result = new Model.article_link();
            result.id = reader.GetInt32(0);
            result.site_id = reader.GetInt32(1);
            result.channel_id = reader.GetInt32(2);
            result.article_id = reader.GetInt32(3);
            result.link_site_id = reader.GetInt32(4);
            result.link_channel_id = reader.GetInt32(5);
            result.link_article_id = reader.GetInt32(6);
            result.link_category_id = reader.GetInt32(7);
            result.add_time = reader.IsDBNull(8) ? DateTime.Parse("1900-01-01") : reader.GetDateTime(8);
         }
         reader.Close();
         return result;
      }

      /// <summary>
      /// 获得数据列表
      /// </summary>
      public List<Model.article_link> GetList(int channel_id, int article_id) {
         List<Model.article_link> modelList = new List<Model.article_link>();

         StringBuilder strSql = new StringBuilder();
         strSql.Append("select id,site_id,channel_id,article_id,link_site_id,link_channel_id,link_article_id,link_category_id,add_time ");
         strSql.Append(" FROM " + databaseprefix + "article_link ");
         strSql.Append(" where channel_id="+ channel_id +" and article_id=" + article_id);
         DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];

         int rowsCount = dt.Rows.Count;
         if (rowsCount > 0) {
            Model.article_link model;
            for (int n = 0; n < rowsCount; n++) {
               model = new Model.article_link();
               if (dt.Rows[n]["id"] != null && dt.Rows[n]["id"].ToString() != "") {
                  model.id = int.Parse(dt.Rows[n]["id"].ToString());
               }
               if (dt.Rows[n]["site_id"] != null && dt.Rows[n]["site_id"].ToString() != "") {
                  model.site_id = int.Parse(dt.Rows[n]["site_id"].ToString());
               }
               if (dt.Rows[n]["channel_id"] != null && dt.Rows[n]["channel_id"].ToString() != "") {
                  model.channel_id = int.Parse(dt.Rows[n]["channel_id"].ToString());
               }
               if (dt.Rows[n]["article_id"] != null && dt.Rows[n]["article_id"].ToString() != "") {
                  model.article_id = int.Parse(dt.Rows[n]["article_id"].ToString());
               }
               if (dt.Rows[n]["link_site_id"] != null && dt.Rows[n]["link_site_id"].ToString() != "") {
                  model.link_site_id = int.Parse(dt.Rows[n]["link_site_id"].ToString());
               }
               if (dt.Rows[n]["link_channel_id"] != null && dt.Rows[n]["link_channel_id"].ToString() != "") {
                  model.link_channel_id = int.Parse(dt.Rows[n]["link_channel_id"].ToString());
               }
               if (dt.Rows[n]["link_article_id"] != null && dt.Rows[n]["link_article_id"].ToString() != "") {
                  model.link_article_id = int.Parse(dt.Rows[n]["link_article_id"].ToString());
               }
               if (dt.Rows[n]["link_category_id"] != null && dt.Rows[n]["link_category_id"].ToString() != "") {
                  model.link_category_id = int.Parse(dt.Rows[n]["link_category_id"].ToString());
               }
               if (dt.Rows[0]["add_time"].ToString() != "") {
                  model.add_time = DateTime.Parse(dt.Rows[0]["add_time"].ToString());
               }
               modelList.Add(model);
            }
         }
         return modelList;
      }

      /// <summary>
      /// 添加一个对象实体
      /// </summary>
      /// <param name="model"></param>
      /// <returns></returns>
      public bool Add(Model.article_link model){
         if (GetModel(model.channel_id, model.article_id, model.link_channel_id, model.link_article_id) != null) {
            return false;
         }
         StringBuilder strSql = new StringBuilder();
         strSql.Append("insert into " + databaseprefix + "article_link(");
         strSql.Append("site_id,channel_id,article_id,link_site_id,link_channel_id,link_article_id,link_category_id,add_time)");
         strSql.Append(" values(");
         strSql.Append("@site_id,@channel_id,@article_id,@link_site_id,@link_channel_id,@link_article_id,@link_category_id,@add_time);select @@IDENTITY;");
         SqlParameter[] parameters = {
                                        new SqlParameter("@site_id", SqlDbType.Int,4),
                                        new SqlParameter("@channel_id", SqlDbType.Int,4),
                                        new SqlParameter("@article_id", SqlDbType.Int,4),
                                        new SqlParameter("@link_site_id", SqlDbType.Int,4),
                                        new SqlParameter("@link_channel_id", SqlDbType.Int,4),
                                        new SqlParameter("@link_article_id", SqlDbType.Int,4),
                                        new SqlParameter("@link_category_id", SqlDbType.Int,4),
                                        new SqlParameter("@add_time", SqlDbType.DateTime)};
         parameters[0].Value = model.site_id;
         parameters[1].Value = model.channel_id;
         parameters[2].Value = model.article_id;
         parameters[3].Value = model.link_site_id;
         parameters[4].Value = model.link_channel_id;
         parameters[5].Value = model.link_article_id;
         parameters[6].Value = model.link_category_id;
         parameters[7].Value = model.add_time;
         object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
         model.id = Convert.ToInt32(obj);
         return true;
      }

      /// <summary>
      /// 删除一个对象实体
      /// </summary>
      /// <param name="model"></param>
      /// <returns></returns>
      public bool Delete(Model.article_link model) {
         return Delete(model.id);
      }

      /// <summary>
      /// 删除一个对象实体
      /// </summary>
      /// <param name="article_id">文章Id</param>
      /// <param name="link_article_id">关联文章Id</param>
      /// <returns></returns>
      public bool Delete(int id) {
         StringBuilder strSql = new StringBuilder();
         strSql.Append("delete from " + databaseprefix + "article_link");
         strSql.Append(" where id=@id");
         SqlParameter[] parameters = {
                                        new SqlParameter("@id", SqlDbType.Int, 4),
                                     };
         parameters[0].Value = id;
         int count = DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
         return count == 1;
      }
   }
}

