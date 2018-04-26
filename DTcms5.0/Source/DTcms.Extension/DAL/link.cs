using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DTcms.Common;
using DTcms.DBUtility;

namespace DTcms.DAL {
   class link {
      private string databaseprefix; //数据库表名前缀
      public link(string _databaseprefix) {
         databaseprefix = _databaseprefix;
      }

      #region 基本方法================================
      /// <summary>
      /// 是否存在该记录
      /// </summary>
      public bool Exists(int id) {
         StringBuilder strSql = new StringBuilder();
         strSql.Append("select count(1) from " + databaseprefix + "link");
         strSql.Append(" where id=@id ");
         SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
         parameters[0].Value = id;

         return DbHelperSQL.Exists(strSql.ToString(), parameters);
      }

      /// <summary>
      /// 增加一条数据
      /// </summary>
      public int Add(Model.link model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into " + databaseprefix + "link(");
            strSql.Append("site_path,title,user_name,user_tel,email,site_url,img_url,is_image,sort_id,is_red,is_lock,add_time)");
            strSql.Append(" values (");
            strSql.Append("@site_path,@title,@user_name,@user_tel,@email,@site_url,@img_url,@is_image,@sort_id,@is_red,@is_lock,@add_time)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
               new SqlParameter("@site_path", SqlDbType.NVarChar, 100), 
               new SqlParameter("@title", SqlDbType.NVarChar, 255), 
               new SqlParameter("@user_name", SqlDbType.NVarChar, 50), 
               new SqlParameter("@user_tel", SqlDbType.NVarChar, 20), 
               new SqlParameter("@email", SqlDbType.NVarChar, 50), 
               new SqlParameter("@site_url", SqlDbType.NVarChar, 255), 
               new SqlParameter("@img_url", SqlDbType.NVarChar, 255), 
               new SqlParameter("@is_image", SqlDbType.Int, 4), 
               new SqlParameter("@sort_id", SqlDbType.Int, 4), 
               new SqlParameter("@is_red", SqlDbType.TinyInt, 1), 
               new SqlParameter("@is_lock", SqlDbType.TinyInt, 1), 
               new SqlParameter("@add_time", SqlDbType.DateTime)};
            parameters[0].Value = model.site_path;
            parameters[1].Value = model.title;
            parameters[2].Value = model.user_name;
            parameters[3].Value = model.user_tel;
            parameters[4].Value = model.email;
            parameters[5].Value = model.site_url;
            parameters[6].Value = model.img_url;
            parameters[7].Value = model.is_image;
            parameters[8].Value = model.sort_id;
            parameters[9].Value = model.is_red;
            parameters[10].Value = model.is_lock;
            parameters[11].Value = model.add_time;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

      /// <summary>
      /// 更新一条数据
      /// </summary>
      public bool Update(Model.link model) {
         StringBuilder builder = new StringBuilder();
         builder.Append("update " + this.databaseprefix + "link set ");
         builder.Append("site_path=@site_path,");
         builder.Append("title=@title,");
         builder.Append("user_name=@user_name,");
         builder.Append("user_tel=@user_tel,");
         builder.Append("email=@email,");
         builder.Append("site_url=@site_url,");
         builder.Append("img_url=@img_url,");
         builder.Append("is_image=@is_image,");
         builder.Append("sort_id=@sort_id,");
         builder.Append("is_red=@is_red,");
         builder.Append("is_lock=@is_lock,");
         builder.Append("add_time=@add_time");
         builder.Append(" where id=@id");

         SqlParameter[] parameters = new SqlParameter[] { 
              new SqlParameter("@site_path", SqlDbType.NVarChar, 100), 
              new SqlParameter("@title", SqlDbType.NVarChar, 255), 
              new SqlParameter("@user_name", SqlDbType.NVarChar, 50),
              new SqlParameter("@user_tel", SqlDbType.NVarChar, 20), 
              new SqlParameter("@email", SqlDbType.NVarChar, 50), 
              new SqlParameter("@site_url", SqlDbType.NVarChar, 255), 
              new SqlParameter("@img_url", SqlDbType.NVarChar, 255), 
              new SqlParameter("@is_image", SqlDbType.Int, 4), 
              new SqlParameter("@sort_id", SqlDbType.Int, 4), 
              new SqlParameter("@is_red", SqlDbType.TinyInt, 1), 
              new SqlParameter("@is_lock", SqlDbType.TinyInt, 1), 
              new SqlParameter("@add_time", SqlDbType.DateTime), 
              new SqlParameter("@id", SqlDbType.Int, 4) };
         parameters[0].Value = model.site_path;
         parameters[1].Value = model.title;
         parameters[2].Value = model.user_name;
         parameters[3].Value = model.user_tel;
         parameters[4].Value = model.email;
         parameters[5].Value = model.site_url;
         parameters[6].Value = model.img_url;
         parameters[7].Value = model.is_image;
         parameters[8].Value = model.sort_id;
         parameters[9].Value = model.is_red;
         parameters[10].Value = model.is_lock;
         parameters[11].Value = model.add_time;
         parameters[12].Value = model.id;


         int rows = DbHelperSQL.ExecuteSql(builder.ToString(), parameters);
         if (rows > 0) {
            return true;
         }
         else {
            return false;
         }
      }

      /// <summary>
      /// 删除一条数据
      /// </summary>
      public bool Delete(int id) {
         //获取用户旧数据
         Model.link model = GetModel(id);
         if (model == null) {
            return false;
         }
         StringBuilder builder = new StringBuilder();
         builder.Append("delete from " + this.databaseprefix + "link ");
         builder.Append(" where id=@id");
         SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
         parameterArray[0].Value = id;
         return (DbHelperSQL.ExecuteSql(builder.ToString(), parameterArray) > 0);

      }

      /// <summary>
      /// 得到一个对象实体
      /// </summary>
      public Model.link GetModel(int id) {
         StringBuilder builder = new StringBuilder();
         builder.Append("select  top 1 id,site_path,title,user_name,user_tel,email,site_url,img_url,is_image,sort_id,is_red,is_lock,add_time from " + this.databaseprefix + "link ");
         builder.Append(" where id=@id");
         SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
         parameterArray[0].Value = id;
         Model.link link = new Model.link();
         DataSet set = DbHelperSQL.Query(builder.ToString(), parameterArray);
         if (set.Tables[0].Rows.Count > 0) {
            if ((set.Tables[0].Rows[0]["id"] != null) && (set.Tables[0].Rows[0]["id"].ToString() != "")) {
               link.id = int.Parse(set.Tables[0].Rows[0]["id"].ToString());
            }
            if ((set.Tables[0].Rows[0]["site_path"] != null) && (set.Tables[0].Rows[0]["site_path"].ToString() != "")) {
               link.site_path = set.Tables[0].Rows[0]["site_path"].ToString();
            }
            if ((set.Tables[0].Rows[0]["title"] != null) && (set.Tables[0].Rows[0]["title"].ToString() != "")) {
               link.title = set.Tables[0].Rows[0]["title"].ToString();
            }
            if ((set.Tables[0].Rows[0]["user_name"] != null) && (set.Tables[0].Rows[0]["user_name"].ToString() != "")) {
               link.user_name = set.Tables[0].Rows[0]["user_name"].ToString();
            }
            if ((set.Tables[0].Rows[0]["user_tel"] != null) && (set.Tables[0].Rows[0]["user_tel"].ToString() != "")) {
               link.user_tel = set.Tables[0].Rows[0]["user_tel"].ToString();
            }
            if ((set.Tables[0].Rows[0]["email"] != null) && (set.Tables[0].Rows[0]["email"].ToString() != "")) {
               link.email = set.Tables[0].Rows[0]["email"].ToString();
            }
            if ((set.Tables[0].Rows[0]["site_url"] != null) && (set.Tables[0].Rows[0]["site_url"].ToString() != "")) {
               link.site_url = set.Tables[0].Rows[0]["site_url"].ToString();
            }
            if ((set.Tables[0].Rows[0]["img_url"] != null) && (set.Tables[0].Rows[0]["img_url"].ToString() != "")) {
               link.img_url = set.Tables[0].Rows[0]["img_url"].ToString();
            }
            if ((set.Tables[0].Rows[0]["is_image"] != null) && (set.Tables[0].Rows[0]["is_image"].ToString() != "")) {
               link.is_image = int.Parse(set.Tables[0].Rows[0]["is_image"].ToString());
            }
            if ((set.Tables[0].Rows[0]["sort_id"] != null) && (set.Tables[0].Rows[0]["sort_id"].ToString() != "")) {
               link.sort_id = int.Parse(set.Tables[0].Rows[0]["sort_id"].ToString());
            }
            if ((set.Tables[0].Rows[0]["is_red"] != null) && (set.Tables[0].Rows[0]["is_red"].ToString() != "")) {
               link.is_red = int.Parse(set.Tables[0].Rows[0]["is_red"].ToString());
            }
            if ((set.Tables[0].Rows[0]["is_lock"] != null) && (set.Tables[0].Rows[0]["is_lock"].ToString() != "")) {
               link.is_lock = int.Parse(set.Tables[0].Rows[0]["is_lock"].ToString());
            }
            if ((set.Tables[0].Rows[0]["add_time"] != null) && (set.Tables[0].Rows[0]["add_time"].ToString() != "")) {
               link.add_time = DateTime.Parse(set.Tables[0].Rows[0]["add_time"].ToString());
            }
            return link;
         }
         return null;

      }

      public DataSet GetList(string strWhere) {
         StringBuilder builder = new StringBuilder();
         builder.Append("select id,site_path,title,user_name,user_tel,email,site_url,img_url,is_image,sort_id,is_red,is_lock,add_time ");
         builder.Append(" FROM " + this.databaseprefix + "link ");
         if (strWhere.Trim() != "") {
            builder.Append(" where " + strWhere);
         }
         return DbHelperSQL.Query(builder.ToString());
      }

      /// <summary>
      /// 获得前几行数据
      /// </summary>
      public DataSet GetList(int Top, string strWhere) {
         StringBuilder builder = new StringBuilder();
         builder.Append("select ");
         if (Top > 0) {
            builder.Append(" top " + Top.ToString());
         }
         builder.Append(" id,site_path,title,user_name,user_tel,email,site_url,img_url,is_image,sort_id,is_red,is_lock,add_time ");
         builder.Append(" FROM " + this.databaseprefix + "link ");
         if (strWhere.Trim() != "") {
            builder.Append(" where " + strWhere);
         }
         builder.Append(" order by sort_id asc,add_time desc");
         return DbHelperSQL.Query(builder.ToString());
      }

      /// <summary>
      /// 获得查询分页数据
      /// </summary>
      public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount) {
         StringBuilder strSql = new StringBuilder();
         strSql.Append("select * FROM " + databaseprefix + "link");
         if (strWhere.Trim() != "") {
            strSql.Append(" where " + strWhere);
         }
         recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
         return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
      }

      #endregion

      #region 扩展方法================================
      /// <summary>
      /// 修改一列数据
      /// </summary>
      public int UpdateField(int id, string strValue) {
         StringBuilder strSql = new StringBuilder();
         strSql.Append("update " + databaseprefix + "link set " + strValue);
         strSql.Append(" where id=" + id);
         return DbHelperSQL.ExecuteSql(strSql.ToString());
      }
      #endregion
   }
}
