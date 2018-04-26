using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Xml;
using DTcms.Web.MVC.UI;
using DTcms.Common;

namespace DTcms.Web.Mvc.Plugin.SiteBackup {
   /// <summary>
   /// AJAX处理页
   /// </summary>
   public class ajax : IHttpHandler, IRequiresSessionState {
      DTcms.Model.sysconfig sysConfig;
      public void ProcessRequest(HttpContext context) {
         //取得处事类型
         string action = context.Request.Params["action"];
         sysConfig = new DTcms.BLL.sysconfig().loadConfig();//获得系统配置信息
         switch (action) {
            case "site_export": //导出站点
               site_export(context);
               break;
            case "site_import": //导入站点
               site_import(context);
               break;
         }

      }

      #region 站点导出================================
      private void site_export(HttpContext context) {
         BLL.SiteBackup bll = new BLL.SiteBackup();
         int _site_id = DTRequest.GetQueryInt("site");
         DataSet exportDataSet = new DataSet();

         XmlDocument doc = GetConfigDocument();
         XmlNode xn = doc.SelectSingleNode("tables");
         XmlNodeList xnList = xn.ChildNodes;

         for (int i = 0; i < xnList.Count; i++) {//遍历xml配置文件中table标签,逐个表处理
            XmlElement xe = (XmlElement)xnList[i];
            string table_name = xe.Attributes["name"].Value;
            string where = "";
            if (i == 0) {//kfcms_sites表信息必须放在第一行, 按照客户端传入的site_id值查询数据行
               where = " where id = " + _site_id;
            }

            #region 如果表名是{kfcms_article}变量, 需要根据通道信息确定实际的文章表并导出全部数据
            if (table_name == "{"+sysConfig.sysdatabaseprefix+"article}") {
               //处理通道内容表信息
                foreach (DataRow dr in exportDataSet.Tables[sysConfig.sysdatabaseprefix + "site_channel"].Rows)
                {
                    string article_table = sysConfig.sysdatabaseprefix + "channel_article_" + dr["name"].ToString();
                  DataSet ds = bll.Query("select * from " + article_table);
                  ds.Tables[0].TableName = article_table;
                  exportDataSet.Tables.Add(ds.Tables[0].Copy());
               }
               continue;
            }
            #endregion

            #region 存在外键的表, 设置where条件字符串,筛选与已导出主表相关联的数据行
            if (xe.ChildNodes.Count != 0) {
               StringBuilder sb = new StringBuilder();
               for (int ci = 0; ci < xe.ChildNodes.Count; ci++) {
                  XmlElement cxe = (XmlElement)xe.ChildNodes[ci];
                  string foreigh_key = cxe.Attributes["key"].Value;
                  string primary_table = cxe.Attributes["primary_table"].Value;
                  string primary_key = cxe.Attributes["primary_key"].Value;
                  if (primary_table == "{"+sysConfig.sysdatabaseprefix+"article}") {
                     //外键表指向通道内容表
                     break;
                  }
                  else {
                     DataTable dt = exportDataSet.Tables[primary_table];
                     if (dt == null) {
                        string msg = string.Format("未寻找到 {0} 表 {1} 字段所关联的主表 {2} !", table_name, foreigh_key, primary_table);
                        context.Response.Write("{\"status\": 0, \"msg\": \"" + msg + "\"}");
                        return;
                     }
                     string ids = GetIds(dt, primary_key);//在已导的外键主表中查询主键id
                     if (ci == 0) {
                        sb.Append(string.Format("{0} in ({1})", foreigh_key, ids));
                     }
                     else {
                        sb.Append(string.Format("and {0} in ({1})", foreigh_key, ids));
                     }

                  }
               }
               where = " where " + sb.ToString();
            }
            #endregion

            string sql = "select * from " + table_name + where;//设置查询语句
            if (xe.Attributes["ParentField"] == null) {
               //不是父子结构表
               DataSet ds = null;
               try {
                  ds = bll.Query(sql);
               }
               catch (Exception e) {
                  context.Response.Write("{\"status\": 0, \"msg\": \"TableName=" + table_name + "," + e.Message + "\"}");
                  return;
               }
               ds.Tables[0].TableName = table_name;
               exportDataSet.Tables.Add(ds.Tables[0].Copy());
            }
            else {
               //是父子结构表
               string parentField = xe.Attributes["ParentField"].Value;
               DataSet ds = null;
               try {
                  ds = bll.Query(sql);
               }
               catch (Exception e) {
                  context.Response.Write("{\"status\": 0, \"msg\": \"TableName=" + table_name + "" + e.Message + "\"}");
                  return;
               }
               ds.Tables[0].TableName = table_name;
               DataTable tempList = ds.Tables[0].Clone();
               foreach (DataRow dr in ds.Tables[0].Rows) {
                  int _parent_id = Convert.ToInt32(dr[parentField]);
                  FillParentRows(tempList, "select * from " + table_name, parentField, _parent_id);//填充父行数据,递归至根数据行
               }
               foreach (DataRow dr in ds.Tables[0].Rows) {
                  tempList.ImportRow(dr);
               }
               //按parent_id字段重新排序,保证上级数据在前
               tempList.DefaultView.Sort = parentField + " ASC";
               exportDataSet.Tables.Add(tempList.DefaultView.ToTable().Copy());//添加到导出数据集中
            }
         }

         DTcms.BLL.sites bll_site = new DTcms.BLL.sites();
         Model.sites site = bll_site.GetModel(_site_id);
         string path = Utils.GetMapPath("~/templates/" + site.templet_path + "/data/");
         if (!Directory.Exists(path)) {
            Directory.CreateDirectory(path);
         }
         exportDataSet.WriteXml(path + "DTcms5.0db.xml");
         exportDataSet.WriteXmlSchema(path + "DTcms5.0db_schema.xml");
         context.Response.Write("{\"status\": 0, \"msg\": \"导出站点数据成功！\"}");
         return;
      }
      #endregion

      /// <summary>
      /// 获取站点相关数据表xml配置文件
      /// </summary>
      /// <returns></returns>
      private XmlDocument GetConfigDocument() {
         //读入站点备份配置文件
         string xmlpath = Utils.GetMapPath("~/plugins/sitebackup/DTcms5.0db_table.xml");
         XmlDocument doc = new XmlDocument();
         XmlReaderSettings settings = new XmlReaderSettings();
         settings.IgnoreComments = true;//排除注释行
         XmlReader reader = XmlReader.Create(xmlpath, settings);
         doc.Load(reader);
         return doc;
      }

      /// <summary>
      /// 拼接指定字段值为字符串,逗号分隔
      /// </summary>
      /// <param name="table"></param>
      /// <param name="columnName">要拼接的字段名称</param>
      /// <returns></returns>
      private string GetIds(DataTable table, string columnName) {
         StringBuilder result = new StringBuilder();
         foreach(DataRow dr in table.Rows){
            result.Append(dr[columnName].ToString() + ",");
         }
         return result.ToString().TrimEnd(',');
      }

      /// <summary>
      /// 在数据库中查找父行数据,插入到指定DataTable中
      /// </summary>
      /// <param name="table"></param>
      /// <param name="sql">Sql语句,无where部分</param>
      /// <param name="fieldName">parent_id字段名称</param>
      /// <param name="parent_id">parent_id值</param>
      private void FillParentRows(DataTable table, string sql, string parentfieldName, int parent_id) {
         BLL.SiteBackup bll = new BLL.SiteBackup();
         while (true) {
            DataSet ds = bll.Query(sql + " where id = " + parent_id);//从数据库查询父行数据,
            if (ds.Tables.Count == 0) {
               break;
            }
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0) {
               break;
            }
            DataRow dr = dt.Rows[0];//父行应该唯一
            DataRow[] existRows = table.Select("id=" + dr["id"].ToString());
            if (existRows.Length == 0) {//如果在已导出数据中存在此父行数据则返回
               table.ImportRow(dr);
            }
            parent_id = Convert.ToInt32(dr[parentfieldName]);//更新parent_id值,执行下一循环,向更上级查询
         }
      }

      #region 站点导入
      private void site_import(HttpContext context) {
         BLL.SiteBackup bll = new BLL.SiteBackup();
         int _site_id = DTRequest.GetQueryInt("site");
         DTcms.BLL.sites bll_site = new DTcms.BLL.sites();
         DTcms.Model.sites site = bll_site.GetModel(_site_id);


         string path = Utils.GetMapPath("~/templates/" + site.templet_path + "/data/");
         DataSet dsData = new DataSet();
         //读入数据文件
         dsData.ReadXmlSchema(path + "DTcms5.0db_schema.xml");
         dsData.ReadXml(path + "DTcms5.0db.xml");
         DataSet dsSchema = new DataSet();
         //读入数据表结构文件
         dsSchema.ReadXmlSchema(path + "DTcms5.0db_schema.xml");
         XmlDocument doc = GetConfigDocument();
         XmlNode xn = doc.SelectSingleNode("tables");
         XmlNodeList xnList = xn.ChildNodes;
         int count = 0;
         string channel_table_name = string.Empty;

         #region 处理td_sites表
         XmlElement xe = (XmlElement)xnList[0];
         string table_name = xe.Attributes["name"].Value;
         DataTable dt = dsData.Tables[table_name];
         DataColumn newdc = new DataColumn("old_id", Type.GetType("System.Int32"));
         dt.Columns.Add(newdc);
         DataRow dr = dsData.Tables[table_name].Rows[0];
         dr["old_id"] = dr["id"];
         dr["id"] = _site_id;
         //当前站点信息将被备份数据表站点信息覆盖
         count += bll.UpdateByAutoId(table_name, dr);
         #endregion

         for (int i = 1; i < xnList.Count; i++)
         {//遍历xml配置文件中table标签,跳过kfcmssites表,逐个表处理
            xe = (XmlElement)xnList[i];
            table_name = xe.Attributes["name"].Value;
            if (!dsData.Tables.Contains(table_name) && table_name != "{"+sysConfig.sysdatabaseprefix+"article}") {//如果备份数据中没有当前表，则跳过
               continue;
            }

            if (table_name.IndexOf("site_channel") >= 0) {
               if (table_name.Substring(table_name.Length - 12, 12) == "site_channel") {
                  channel_table_name = table_name;
                  dsData.Tables[table_name].PrimaryKey = new DataColumn[] { dsData.Tables[table_name].Columns["id"] };
               }
            }

            string uniqueField = string.Empty;//不允许出现重复值的字段名称（多个字段联合用逗号分开）
            if (xe.Attributes["UniqueField"] != null) {
               uniqueField = xe.Attributes["UniqueField"].Value;
            }

            string parentField = string.Empty;//父子结构表的parent_id字段名称(单一字段)
            if (xe.Attributes["ParentField"] != null) {
               parentField = xe.Attributes["ParentField"].Value;
            }

            #region 如果表名是{kf_cmsarticle}变量, 需要根据通道信息确定实际的文章表并导入数据
            if (table_name == "{"+sysConfig.sysdatabaseprefix+"article}") {//处理通道文章表
               foreach (DataRow row in dsData.Tables[channel_table_name].Rows) {//遍历通道表，每个通道都一个对应的文章表
                  int channel_id = Convert.ToInt32(row["id"]);
                  string channel_name = row["name"].ToString();
                  string article_table_name = sysConfig.sysdatabaseprefix + DTKeys.TABLE_CHANNEL_ARTICLE + row["name"];//当前通道对应文章表名称
                  DataTable artList = dsData.Tables[article_table_name];
                  if (artList == null) {
                     continue;
                  }
                  foreach (XmlElement cxe in xe.ChildNodes) {
                     //更新外键值
                     UpdateArticleForeighKey(dsData, article_table_name, channel_table_name, cxe);
                  }
                  bll.InsertByArticle(channel_id, channel_name, article_table_name, artList, uniqueField);
               }
               continue;
            }
            #endregion

            if (xe.ChildNodes.Count != 0) {//如果包含外键信息，需要更新外键为与数据库中主键表相符的值
               foreach (XmlElement cxe in xe.ChildNodes) {
                  UpdateForeighKey(dsData, table_name, channel_table_name, cxe);
               }
            }
            count += bll.Insert(table_name, dsData.Tables[table_name], uniqueField, parentField);//插入当前行到数据库,会按uniqueField值跳过重复行
         }
         string msg = string.Format("导入 {0} 行数据", count);
         context.Response.Write("{\"status\": 0, \"msg\": \"" + msg + "！\"}");
         return;
      }
      #endregion

      /// <summary>
      /// 更新导入数据表外键值
      /// </summary>
      /// <param name="ds">待导入的数据集</param>
      /// <param name="tableName">要更新的数据表名称</param>
      /// <param name="channelTableName">通道表名称</param>
      /// <param name="xmlElement">外键信息（<foreigh key="外键名称" primary_table="主键表" primary_key="主键表主键名称" />）</param>
      private void UpdateForeighKey(DataSet ds, string tableName, string channelTableName, XmlElement xmlElement) {
         string foreigh_key = xmlElement.Attributes["key"].Value;
         string primary_table_name = xmlElement.Attributes["primary_table"].Value;
         string primary_key = xmlElement.Attributes["primary_key"].Value;
         DataTable primaryTable = ds.Tables[primary_table_name];
         DataTable dt = ds.Tables[tableName];
         foreach (DataRow dr in dt.Rows) {
            if (primary_table_name == "{"+sysConfig.sysdatabaseprefix+"article}") {//如果主键表是文章表需要判断是指向那个通道的文章表，并修正主键表名称
               //外键有article_id时必然有channel_id，因为DTcms5.0的文章表是每个通道对应一个表，需要有channel_id才可以确定指向主键表是那个文章表
               int channel_id = Convert.ToInt32(dr["channel_id"]);
               DataTable channelList = ds.Tables[channelTableName].Copy();
               DataRow[] channelRows = channelList.Select("[id]=" + channel_id);//channel_id外键的更新是在article_id之前，所以这时channel_id外键字段保存的是更新后的channel_id，与通道表id字段对应
               if (channelRows.Length == 0) {
                  StringBuilder sb = new StringBuilder();
                  foreach (DataRow channelRow in channelList.Rows) {
                     sb.Append(channelRow["id"].ToString() + ",");
                  }
                  throw new Exception("未寻找到通道信息:channel_id=" + channel_id + " , 通道表包含的id有（" + sb.ToString().TrimEnd(',') + "）");
               }
               string article_table_name = sysConfig.sysdatabaseprefix + DTKeys.TABLE_CHANNEL_ARTICLE + channelRows[0]["name"];
               primaryTable = ds.Tables[article_table_name];//设置主键表为对应通道文章表
            }
            string id = dr[foreigh_key].ToString();
            if (int.Parse(id) <= 0) {//如果外键是parent_id类型的字段，会用0值来表示当前数据没有父行，此外键值也不需要更新
               continue;
            }
            string expression = string.Format("old_{0}={1}", primary_key, id);//此时主键表已经导入完毕，id字段保存新的自增字段值，old_id字段保存原始id值
            DataRow[] findRows = primaryTable.Select(expression);
            if (findRows.Length == 0) {
               throw new Exception(string.Format("更新外键字段失败:当前表:{0} 外键:名称={1} 值={2},主表:名称={3}", tableName, foreigh_key, id, primary_table_name));
            }
            DataRow findRow = findRows[0];
            dr[foreigh_key] = findRow[primary_key];
         }
      }

      /// <summary>
      /// 更新导入数据表外键值（此方法只用于通道文章表更新）
      /// </summary>
      /// <param name="ds">待导入的数据集</param>
      /// <param name="tableName">当前要更新的文章表名称</param>
      /// <param name="channelTableName">通道表名称</param>
      /// <param name="xmlElement">外键信息（<foreigh key="外键名称" primary_table="主表" primary_key="主表主键名称" />）</param>
      private void UpdateArticleForeighKey(DataSet ds, string tableName, string channelTableName, XmlElement xmlElement) {
         string foreigh_key = xmlElement.Attributes["key"].Value;
         string primary_table_name = xmlElement.Attributes["primary_table"].Value;
         string primary_key = xmlElement.Attributes["primary_key"].Value;
         
         DataTable dt = ds.Tables[tableName];
         foreach (DataRow dr in dt.Rows) {
            int channel_id = Convert.ToInt32(dr["channel_id"]);
            DataTable channelList = ds.Tables[channelTableName];
            DataRow[] channelRows = channelList.Select("old_id=" + channel_id);
            if (channelRows.Length == 0) {
               StringBuilder sb = new StringBuilder();
               foreach (DataRow channelRow in channelList.Rows) {
                  sb.Append(channelRow["id"].ToString() + ",");
               }
               throw new Exception("未寻找到通道信息:channel_id=" + channel_id + " , 通道表包含的id有（"+ sb.ToString().TrimEnd(',') +"）");
            }
            //string article_table_name = sysConfig.sysdatabaseprefix + DTKeys.TABLE_CHANNEL_ARTICLE + channelRows[0]["name"];
            DataTable primaryTable = ds.Tables[primary_table_name];
            string id = dr[foreigh_key].ToString();
            if (int.Parse(id) <= 0) {
               continue;
            }
            string expression = string.Format("old_{0}={1}", primary_key, id);
            DataRow[] findRows = primaryTable.Select(expression);
            if (findRows.Length == 0) {
               throw new Exception(string.Format("更新外键字段失败:当前表:{0} 外键:名称={1} 值={2},主表:名称={3}", tableName, foreigh_key, id, primary_table_name));
            }
            DataRow findRow = findRows[0];
            dr[foreigh_key] = findRow[primary_key];
         }
      }

      public bool IsReusable {
         get {
            return false;
         }
      }
   }
}