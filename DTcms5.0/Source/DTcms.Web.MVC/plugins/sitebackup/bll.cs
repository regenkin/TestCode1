using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Text;

namespace DTcms.Web.Mvc.Plugin.SiteBackup.BLL {
   public class SiteBackup {
      DAL.SiteBackup dal;
      public SiteBackup() {
         dal = new DAL.SiteBackup();
      }

      public DataSet Query(string SQLString) {
         return dal.Quest(SQLString);
      }

      /// <summary>
      /// 主键为自增字段表，插入整表数据
      /// </summary>
      /// <param name="tableName">插入表名称</param>
      /// <param name="dt">来自备份的数据表</param>
      /// <param name="uniqueField">不允许内容重复的字段名称（多字段组合用逗号分开）</param>
      /// <param name="parentField">父子结构表parentId字段名称，非父子结构表设置为空字符串</param>
      /// <returns></returns>
      public int Insert(string tableName, DataTable dt, string uniqueField, string parentField) {
         int count = 0;
         if (!DBUtility.DbHelperSQL.TabExists(tableName)) {
            return count;
         }
         DataTable schemaTable = dal.GetTableSchema(tableName);
         DataRow[] findRows = schemaTable.Select("IsIdentity=true");
         if (findRows.Length == 1) {
            string id_name = findRows[0]["ColumnName"].ToString();//设置自增字段名称
            //添加新字段，用来保存原始id值,id字段值会更新为新插入行的自增字段值
            DataColumn dc = new DataColumn("old_" + id_name, Type.GetType(findRows[0]["DataType"].ToString()));
            dc.AllowDBNull = true;
            dt.Columns.Add(dc);
            count = dal.InsertByAutoId(tableName, dt, schemaTable, uniqueField, parentField);
         }
         dt.AcceptChanges();
         return count;
      }

      public int InsertByArticle(int channel_id, string channel_name, string tableName, DataTable dt, string uniqueField) {
         int count = 0;
         if (!DBUtility.DbHelperSQL.TabExists(tableName)) {
            //如果数据库中不存在文章表, 则创建表
            dal.CreateArticleTable(channel_id, tableName);
         }
         DataTable schemaTable = dal.GetTableSchema(tableName);
         DataRow[] findRows = schemaTable.Select("IsIdentity=true");
         if (findRows.Length == 1) {
            string id_name = findRows[0]["ColumnName"].ToString();//设置自增字段名称
            //添加新字段，用来保存原始id值,id字段值会更新为新插入行的自增字段值
            DataColumn dc = new DataColumn("old_" + id_name, Type.GetType(findRows[0]["DataType"].ToString()));
            dc.AllowDBNull = true;
            dt.Columns.Add(dc);
            count = dal.InsertByArticle(tableName, channel_name, dt, uniqueField);
         }
         return count;
      }

      public int UpdateByAutoId(string tableName, DataRow dr) {
         DataTable schemaTable = dal.GetTableSchema(tableName);
         return dal.UpdateByAutoId(tableName, dr, schemaTable);
      }
   }
}
