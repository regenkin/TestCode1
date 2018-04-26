using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.DAL {
   public class feedback {
      // Fields
      private string databaseprefix;

      // Methods
      public feedback(string _databaseprefix) {
         databaseprefix = _databaseprefix;
      }

      public int Add(Model.feedback model) {
         StringBuilder builder = new StringBuilder();
         builder.Append("insert into " + this.databaseprefix + "feedback(");
         builder.Append("site_path,title,content,user_name,user_tel,user_qq,user_email,add_time,is_lock)");
         builder.Append(" values (");
         builder.Append("@site_path,@title,@content,@user_name,@user_tel,@user_qq,@user_email,@add_time,@is_lock)");
         builder.Append(";select @@IDENTITY");
         SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@site_path", SqlDbType.NVarChar, 100), new SqlParameter("@title", SqlDbType.NVarChar, 100), new SqlParameter("@content", SqlDbType.NText), new SqlParameter("@user_name", SqlDbType.NVarChar, 50), new SqlParameter("@user_tel", SqlDbType.NVarChar, 30), new SqlParameter("@user_qq", SqlDbType.NVarChar, 30), new SqlParameter("@user_email", SqlDbType.NVarChar, 100), new SqlParameter("@add_time", SqlDbType.DateTime), new SqlParameter("@is_lock", SqlDbType.TinyInt, 1) };
         parameterArray[0].Value = model.site_path;
         parameterArray[1].Value = model.title;
         parameterArray[2].Value = model.content;
         parameterArray[3].Value = model.user_name;
         parameterArray[4].Value = model.user_tel;
         parameterArray[5].Value = model.user_qq;
         parameterArray[6].Value = model.user_email;
         parameterArray[7].Value = model.add_time;
         parameterArray[8].Value = model.is_lock;
         object single = DbHelperSQL.GetSingle(builder.ToString(), parameterArray);
         if (single == null) {
            return 0;
         }
         return Convert.ToInt32(single);
      }

      public bool Delete(int id) {
         StringBuilder builder = new StringBuilder();
         builder.Append("delete from " + this.databaseprefix + "feedback ");
         builder.Append(" where id=@id");
         SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
         parameterArray[0].Value = id;
         return (DbHelperSQL.ExecuteSql(builder.ToString(), parameterArray) > 0);

      }

      public bool Exists(int id) {
         StringBuilder builder = new StringBuilder();
         builder.Append("select count(1) from " + this.databaseprefix + "feedback");
         builder.Append(" where id=@id");
         SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
         parameterArray[0].Value = id;
         return DbHelperSQL.Exists(builder.ToString(), parameterArray);

      }

      public DataSet GetList(int Top, string strWhere) {
         StringBuilder builder = new StringBuilder();
         builder.Append("select ");
         if (Top > 0) {
            builder.Append(" top " + Top.ToString());
         }
         builder.Append(" id,site_path,title,content,user_name,user_tel,user_qq,user_email,add_time,reply_content,reply_time,is_lock ");
         builder.Append(" FROM " + this.databaseprefix + "feedback ");
         if (strWhere.Trim() != "") {
            builder.Append(" where " + strWhere);
         }
         builder.Append(" order by add_time desc");
         return DbHelperSQL.Query(builder.ToString());
      }

      public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount) {
         StringBuilder builder = new StringBuilder();
         builder.Append("select * FROM " + this.databaseprefix + "feedback");
         if (strWhere.Trim() != "") {
            builder.Append(" where " + strWhere);
         }
         recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(builder.ToString())));
         return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, builder.ToString(), filedOrder));
      }

      public Model.feedback GetModel(int id) {
         StringBuilder builder = new StringBuilder();
         builder.Append("select top 1 id,site_path,title,content,user_name,user_tel,user_qq,user_email,add_time,reply_content,reply_time,is_lock");
         builder.Append(" from " + this.databaseprefix + "feedback ");
         builder.Append(" where id=@id");
         SqlParameter[] parameterArray = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
         parameterArray[0].Value = id;
         Model.feedback feedback = new Model.feedback();
         DataSet set = DbHelperSQL.Query(builder.ToString(), parameterArray);
         if (set.Tables[0].Rows.Count > 0) {
            if ((set.Tables[0].Rows[0]["id"] != null) && (set.Tables[0].Rows[0]["id"].ToString() != "")) {
               feedback.id = int.Parse(set.Tables[0].Rows[0]["id"].ToString());
            }
            if ((set.Tables[0].Rows[0]["site_path"] != null) && (set.Tables[0].Rows[0]["site_path"].ToString() != "")) {
               feedback.site_path = set.Tables[0].Rows[0]["site_path"].ToString();
            }
            if ((set.Tables[0].Rows[0]["title"] != null) && (set.Tables[0].Rows[0]["title"].ToString() != "")) {
               feedback.title = set.Tables[0].Rows[0]["title"].ToString();
            }
            if ((set.Tables[0].Rows[0]["content"] != null) && (set.Tables[0].Rows[0]["content"].ToString() != "")) {
               feedback.content = set.Tables[0].Rows[0]["content"].ToString();
            }
            if ((set.Tables[0].Rows[0]["user_name"] != null) && (set.Tables[0].Rows[0]["user_name"].ToString() != "")) {
               feedback.user_name = set.Tables[0].Rows[0]["user_name"].ToString();
            }
            if ((set.Tables[0].Rows[0]["user_tel"] != null) && (set.Tables[0].Rows[0]["user_tel"].ToString() != "")) {
               feedback.user_tel = set.Tables[0].Rows[0]["user_tel"].ToString();
            }
            if ((set.Tables[0].Rows[0]["user_qq"] != null) && (set.Tables[0].Rows[0]["user_qq"].ToString() != "")) {
               feedback.user_qq = set.Tables[0].Rows[0]["user_qq"].ToString();
            }
            if ((set.Tables[0].Rows[0]["user_email"] != null) && (set.Tables[0].Rows[0]["user_email"].ToString() != "")) {
               feedback.user_email = set.Tables[0].Rows[0]["user_email"].ToString();
            }
            if ((set.Tables[0].Rows[0]["add_time"] != null) && (set.Tables[0].Rows[0]["add_time"].ToString() != "")) {
               feedback.add_time = DateTime.Parse(set.Tables[0].Rows[0]["add_time"].ToString());
            }
            if ((set.Tables[0].Rows[0]["reply_content"] != null) && (set.Tables[0].Rows[0]["reply_content"].ToString() != "")) {
               feedback.reply_content = set.Tables[0].Rows[0]["reply_content"].ToString();
            }
            if ((set.Tables[0].Rows[0]["reply_time"] != null) && (set.Tables[0].Rows[0]["reply_time"].ToString() != "")) {
               feedback.reply_time = new DateTime?(DateTime.Parse(set.Tables[0].Rows[0]["reply_time"].ToString()));
            }
            if ((set.Tables[0].Rows[0]["is_lock"] != null) && (set.Tables[0].Rows[0]["is_lock"].ToString() != "")) {
               feedback.is_lock = int.Parse(set.Tables[0].Rows[0]["is_lock"].ToString());
            }
            return feedback;
         }
         return null;
      }

      public bool Update(Model.feedback model) {
         StringBuilder builder = new StringBuilder();
         builder.Append("update " + this.databaseprefix + "feedback set ");
         builder.Append("site_path=@site_path,");
         builder.Append("title=@title,");
         builder.Append("content=@content,");
         builder.Append("user_name=@user_name,");
         builder.Append("user_tel=@user_tel,");
         builder.Append("user_qq=@user_qq,");
         builder.Append("user_email=@user_email,");
         builder.Append("add_time=@add_time,");
         builder.Append("reply_content=@reply_content,");
         builder.Append("reply_time=@reply_time,");
         builder.Append("is_lock=@is_lock");
         builder.Append(" where id=@id");
         SqlParameter[] parameterArray = new SqlParameter[] { 
            new SqlParameter("@site_path", SqlDbType.NVarChar, 100), 
            new SqlParameter("@title", SqlDbType.NVarChar, 100), 
            new SqlParameter("@content", SqlDbType.NText), 
            new SqlParameter("@user_name", SqlDbType.NVarChar, 50), 
            new SqlParameter("@user_tel", SqlDbType.NVarChar, 30), 
            new SqlParameter("@user_qq", SqlDbType.NVarChar, 30), 
            new SqlParameter("@user_email", SqlDbType.NVarChar, 100), 
            new SqlParameter("@add_time", SqlDbType.DateTime), 
            new SqlParameter("@reply_content", SqlDbType.NText), 
            new SqlParameter("@reply_time", SqlDbType.DateTime), 
            new SqlParameter("@is_lock", SqlDbType.TinyInt, 1), 
            new SqlParameter("@id", SqlDbType.Int, 4) };
         parameterArray[0].Value = model.site_path;
         parameterArray[1].Value = model.title;
         parameterArray[2].Value = model.content;
         parameterArray[3].Value = model.user_name;
         parameterArray[4].Value = model.user_tel;
         parameterArray[5].Value = model.user_qq;
         parameterArray[6].Value = model.user_email;
         parameterArray[7].Value = model.add_time;
         parameterArray[8].Value = model.reply_content;
         parameterArray[9].Value = model.reply_time;
         parameterArray[10].Value = model.is_lock;
         parameterArray[11].Value = model.id;
         return (DbHelperSQL.ExecuteSql(builder.ToString(), parameterArray) > 0);
      }

      public void UpdateField(int id, string strValue) {
         StringBuilder builder = new StringBuilder();
         builder.Append("update " + this.databaseprefix + "feedback set " + strValue);
         builder.Append(" where id=" + id);
         DbHelperSQL.ExecuteSql(builder.ToString());
      }
   }
}
