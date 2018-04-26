using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.Web.Mvc.Plugin.SiteBackup.DAL
{
    /// <summary>
    /// 备份插件
    /// </summary>
    public class SiteBackup
    {
        DTcms.Model.sysconfig sysConfig;
        public SiteBackup()
        {
            sysConfig = new DTcms.BLL.sysconfig().loadConfig();//获得系统配置信息
        }

        /// <summary>
        /// 执行一条sql语句，将查询结果填充到dataset并返回
        /// </summary>
        /// <param name="SQLString"></param>
        /// <returns></returns>
        public DataSet Quest(string SQLString)
        {
            return DBUtility.DbHelperSQL.Query(SQLString);
        }

        /// <summary>
        /// 获取指定表的结构信息
        /// </summary>
        /// <param name="tableName">数据库表名称</param>
        /// <returns></returns>
        public DataTable GetTableSchema(string tableName)
        {
            DataTable result = null;
            using (SqlConnection conn = new SqlConnection(DBUtility.DbHelperSQL.connectionString))
            {
                string sql = "select * from " + tableName;
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    try
                    {
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                        result = reader.GetSchemaTable();
                    }
                    catch (System.Data.SqlClient.SqlException e)
                    {
                        conn.Close();
                        throw e;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 主键为自增字段表，插入整表数据
        /// </summary>
        /// <param name="tableName">插入表名称</param>
        /// <param name="dt">来自备份的数据表</param>
        /// <param name="schemaTable">插入表结构信息</param>
        /// <param name="uniqueField">不允许内容重复的字段名称（多字段组合用逗号分开）</param>
        /// <param name="parentField">父子结构表parentId字段名称,非父子结构表设置为空字符串</param>
        /// <returns></returns>
        public int InsertByAutoId(string tableName, DataTable dt, DataTable schemaTable, string uniqueField, string parentField)
        {
            int count = 0;
            bool checkUnique = !string.IsNullOrEmpty(uniqueField);
            DataRow[] findRows = schemaTable.Select("IsIdentity=true");
            string id_name = findRows[0]["ColumnName"].ToString();//设置自增字段名称,反正都叫id，没有必要

            //拼接Insert语句
            string sql = GetInsertSql(schemaTable, dt);

            DataTable existList = null;
            //查询重复数据
            string[] uniqueFields = uniqueField.Split(',');
            bool isString = false;
            if (checkUnique && uniqueFields.Length == 1)
            {//如果不可重复字段为单一字段,一次性从数据库查询保存到existList表, 避免多次查询数据库
                isString = TypeIsString(tableName, uniqueField, schemaTable);//判断字段类型是否是字符串
                StringBuilder names = new StringBuilder();
                foreach (DataRow dr in dt.Rows)
                {
                    if (isString)
                    {
                        names.Append("'" + dr[uniqueField].ToString() + "',");
                    }
                    else
                    {
                        names.Append(dr[uniqueField].ToString() + ",");
                    }
                }

                string sql1 = string.Format("select id,{1} from {0} where {1} in ({2})", tableName, uniqueField, names.ToString().TrimEnd(','));
                DataSet ds = DbHelperSQL.Query(sql1);
                if (ds.Tables.Count == 1)
                {
                    existList = ds.Tables[0];
                }
            }

            //遍历数据行Insert数据
            foreach (DataRow dr in dt.Rows)
            {
                dr["old_id"] = dr["id"];//将原始id值保存在old_id字段，如果数据库插入数据库，id值会更新为新行自增字段值
                if (uniqueFields.Length == 1)
                {
                    if (existList != null)
                    {
                        // 有一个不可重复字段
                        DataRow[] findExistRows;
                        if (isString)
                        {
                            findExistRows = existList.Select(uniqueField + "='" + dr[uniqueField].ToString() + "'");
                        }
                        else
                        {
                            findExistRows = existList.Select(uniqueField + "=" + dr[uniqueField].ToString());
                        }
                        if (findExistRows.Length == 0)
                        {
                            dr["id"] = InsertByAutoId(tableName, dr, schemaTable, sql);
                            count++;
                        }
                        else
                        {
                            dr["id"] = findExistRows[0]["id"];
                        }
                        if (!string.IsNullOrEmpty(parentField))
                        {//如果是父子结构表,需要处理parent_id字段
                            UpdateChildParentId(dr, parentField);
                        }
                    }
                }
                else
                {
                    // 多个字段组合后不可重复
                    StringBuilder sb = new StringBuilder("select * from " + tableName + " where ");
                    for (int i = 0; i < uniqueFields.Length; i++)
                    {
                        isString = TypeIsString(tableName, uniqueFields[i], schemaTable);//判断字段类型是否是字符串
                        if (isString)
                            sb.Append(uniqueFields[i] + "='" + dr[uniqueFields[i]] + "' and ");
                        else
                            sb.Append(uniqueFields[i] + "=" + dr[uniqueFields[i]] + " and ");
                    }
                    string sql2 = sb.ToString().Substring(0, sb.Length - 5);
                    DataSet db_quest_list = DbHelperSQL.Query(sql2);
                    if (db_quest_list.Tables.Count != 0 && db_quest_list.Tables[0].Rows.Count != 0)
                    {//判断数据库是否存在重复数据
                        DataRow db_quest_row = db_quest_list.Tables[0].Rows[0];
                        dr["id"] = db_quest_row["id"];
                    }
                    else
                    {
                        dr["id"] = InsertByAutoId(tableName, dr, schemaTable, sql);//取得新插入行的自增字段并保存到new_id字段
                        count++;
                    }
                    if (!string.IsNullOrEmpty(parentField))
                    {//如果是父子结构表,需要处理parent_id字段
                        UpdateChildParentId(dr, parentField);
                    }
                }
            }
            return count;
        }

        /// <summary>
        /// 更新下一级数据的parent_id字段值
        /// </summary>
        /// <param name="dr">当前处理的数据行</param>
        /// <param name="parentField">parent_id字段名称</param>
        /// <returns></returns>
        private int UpdateChildParentId(DataRow dr, string parentField)
        {
            DataRow[] childRows = dr.Table.Select(string.Format("{0}={1}", parentField, dr["old_id"].ToString()));
            int count = childRows.Length;
            foreach (DataRow cdr in childRows)
            {
                cdr[parentField] = dr["id"];

            }
            Array.Clear(childRows, 0, count);
            return count;
        }

        /// <summary>
        /// 主键为自增字段表，插入一行数据
        /// </summary>
        /// <param name="tableName">插入表名称</param>
        /// <param name="dr">来源数据行</param>
        /// <param name="schemaTable">插入表结构信息</param>
        /// <returns>新插入行id值</returns>
        public int InsertByAutoId(string tableName, DataRow dr, DataTable schemaTable)
        {
            string sql = GetInsertSql(schemaTable, dr.Table);
            return InsertByAutoId(tableName, dr, schemaTable, sql);
        }

        /// <summary>
        /// 主键为自增字段表，插入一行数据
        /// </summary>
        /// <param name="tableName">插入表名称</param>
        /// <param name="dr">来源数据行</param>
        /// <param name="schemaTable">插入表结构信息</param>
        /// <param name="sql">insert语句</param>
        /// <returns>新插入行id值</returns>
        private int InsertByAutoId(string tableName, DataRow dr, DataTable schemaTable, string sql)
        {
            List<SqlParameter> parameters = GetInsertParameter(schemaTable, dr);
            object obj = DbHelperSQL.GetSingle(sql, parameters.ToArray());
            int id = Convert.ToInt32(obj);
            parameters.Clear();
            return id;
        }

        /// <summary>
        /// 带有自增字段表更新一行数据
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="dr">数据行</param>
        /// <param name="schemaTable">表结构信息</param>
        /// <returns>影响的行数</returns>
        public int UpdateByAutoId(string tableName, DataRow dr, DataTable schemaTable)
        {
            string sql = GetUpdateSql(schemaTable, dr.Table);
            List<SqlParameter> parameters = GetUpdateParameter(schemaTable, dr);
            int count = DbHelperSQL.ExecuteSql(sql, parameters.ToArray());
            parameters.Clear();
            return count;
        }

        /// <summary>
        /// 文章表插入整表数据
        /// </summary>
        /// <param name="tableName">表名称</param>
        /// <param name="channelName">所属通道名称</param>
        /// <param name="dt">来自备份的数据表</param>
        /// <param name="uniqueField">不允许内容重复的字段名称</param>
        /// <returns></returns>
        public int InsertByArticle(string tableName, string channelName, DataTable dt, string uniqueField)
        {
            int count = 0;
            DTcms.BLL.site_channel bll_channel = new DTcms.BLL.site_channel();
            DTcms.Model.site_channel channelModel = bll_channel.GetModel(channelName);
            if (channelModel == null)
            {
                return 0;
            }
            if (!DbHelperSQL.TabExists(tableName))
            {//如果数据库中没有对应表,则创建通道对应文章表
                CreateArticleTable(channelModel.id, tableName);
            }
            //获取通道文章表架构
            DataTable schemaTable = GetTableSchema(tableName);
            if (schemaTable == null)
            {
                return 0;
            }
            count = InsertByAutoId(tableName, dt, schemaTable, uniqueField, "");
            return count;
        }

        /// <summary>
        /// 为通道创建对应的文章表
        /// </summary>
        public bool CreateArticleTable(int channel_id, string tableName)
        {
            //创建频道数据表
            StringBuilder strSql2 = new StringBuilder();//存储创建频道表SQL语句
            strSql2.Append("CREATE TABLE " + tableName + "(\r\n");
            strSql2.Append("[id] int IDENTITY(1,1) PRIMARY KEY,\r\n");
            strSql2.Append("[site_id] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[channel_id] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[category_id] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[brand_id] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[call_index] nvarchar(50),\r\n");
            strSql2.Append("[title] nvarchar(100),\r\n");
            strSql2.Append("[link_url] nvarchar(255),\r\n");
            strSql2.Append("[img_url] nvarchar(255),\r\n");
            strSql2.Append("[seo_title] nvarchar(255),\r\n");
            strSql2.Append("[seo_keywords] nvarchar(255),\r\n");
            strSql2.Append("[seo_description] nvarchar(255),\r\n");
            strSql2.Append("[tags] nvarchar(500),\r\n");
            strSql2.Append("[zhaiyao] nvarchar(255),\r\n");
            strSql2.Append("[content] ntext,\r\n");
            strSql2.Append("[sort_id] int NOT NULL DEFAULT ((99)),\r\n");
            strSql2.Append("[click] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[status] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[is_msg] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[is_top] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[is_red] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[is_hot] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[is_slide] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[is_sys] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[user_name] nvarchar(100),\r\n");
            strSql2.Append("[like_count] int NOT NULL DEFAULT ((0)),\r\n");
            strSql2.Append("[add_time] datetime NOT NULL DEFAULT (getdate()),\r\n");
            strSql2.Append("[update_time] datetime,\r\n");

            DTcms.BLL.article_attribute_field bll_attribute = new DTcms.BLL.article_attribute_field();
            string ids = GetChannelFieldIds(channel_id);//从kfcms_site_channel_field表获取包含的扩展字段
            DataSet ds = bll_attribute.GetList(99, "id in(" + ids + ")", "id");
            if (ds.Tables.Count == 1)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    strSql2.Append("[" + dr["name"].ToString() + "] " + dr["data_type"].ToString() + ",\r\n");
                }
            }

            //执行SQL创表语句
            DbHelperSQL.ExecuteSql(strSql2.ToString().TrimEnd(',') + ")");
            return true;
        }

        /// <summary>
        /// 获取指定通道所关联的扩展字段Id
        /// </summary>
        /// <param name="channel_id"></param>
        /// <returns></returns>
        private string GetChannelFieldIds(int channel_id)
        {
            StringBuilder ids = new StringBuilder();
            string sql = "select field_id from " + sysConfig.sysdatabaseprefix + "site_channel_field where channel_id=@channel_id";
            SqlParameter par = new SqlParameter("channel_id", SqlDbType.Int);
            par.Value = channel_id;
            SqlDataReader reader = DbHelperSQL.ExecuteReader(sql, par);
            while (reader.Read())
            {
                ids.Append(reader["field_id"].ToString() + ",");
            }
            reader.Close();
            return ids.ToString().TrimEnd(',');
        }

        private string GetInsertSql(DataTable schemaTable, DataTable dt)
        {
            string tableName = dt.TableName;
            DataRow[] findRows = schemaTable.Select("IsIdentity=true");
            string id_name = findRows[0]["ColumnName"].ToString();//设置自增字段名称
            StringBuilder sb1 = new System.Text.StringBuilder();
            StringBuilder sb2 = new System.Text.StringBuilder();
            foreach (DataRow dr in schemaTable.Rows)
            {
                string columnName = dr["ColumnName"].ToString();
                if (columnName == id_name)
                {//跳过自增字段
                    continue;
                }
                if (!dt.Columns.Contains(columnName))
                {//如果备份数据表中没有当前字段，则跳过
                    continue;
                }
                sb1.Append(columnName + ",");
                sb2.Append("@" + columnName + ",");
            }
            string sql = string.Format("insert into {0}({1})values({2});select @@IDENTITY;", tableName, sb1.ToString().TrimEnd(','), sb2.ToString().TrimEnd(','));
            return sql;
        }

        private string GetUpdateSql(DataTable schemaTable, DataTable dt)
        {
            string tableName = dt.TableName;
            DataRow[] findRows = schemaTable.Select("IsIdentity=true");
            string id_name = findRows[0]["ColumnName"].ToString();//设置自增字段名称
            StringBuilder sb1 = new System.Text.StringBuilder();
            foreach (DataRow dr in schemaTable.Rows)
            {
                string columnName = dr["ColumnName"].ToString();
                if (columnName == id_name)
                {//跳过自增字段
                    continue;
                }
                if (!dt.Columns.Contains(columnName))
                {//如果备份数据表中没有当前字段，则跳过
                    continue;
                }
                sb1.Append(string.Format("{0}=@{0},", columnName));
            }
            string sql = string.Format("update {0} set {1} where {2}=@{2}", tableName, sb1.ToString().TrimEnd(','), id_name);
            return sql;
        }

        private List<SqlParameter> GetInsertParameter(DataTable schemaTable, DataRow dr)
        {
            DataTable dt = dr.Table;
            List<SqlParameter> parameters = new List<SqlParameter>();
            //遍历数据列，为字段赋值
            foreach (DataRow column in schemaTable.Rows)
            {
                if (Convert.ToBoolean(column["IsIdentity"]))
                {//跳过自增字段
                    continue;
                }
                string columnName = column["ColumnName"].ToString();
                if (!dt.Columns.Contains(columnName))
                {//如果备份数据表中没有当前字段，则跳过
                    continue;
                }
                SqlParameter par = new SqlParameter(columnName, (SqlDbType)Convert.ToInt32(column["NonVersionedProviderType"]), Convert.ToInt32(column["ColumnSize"]));
                par.Value = dr[columnName];
                parameters.Add(par);
            }
            return parameters;
        }

        private List<SqlParameter> GetUpdateParameter(DataTable schemaTable, DataRow dr)
        {
            DataTable dt = dr.Table;
            List<SqlParameter> parameters = new List<SqlParameter>();
            //遍历数据列，为字段赋值
            foreach (DataRow column in schemaTable.Rows)
            {
                string columnName = column["ColumnName"].ToString();
                if (!dt.Columns.Contains(columnName))
                {//如果备份数据表中没有当前字段，则跳过
                    continue;
                }
                SqlParameter par = new SqlParameter(columnName, (SqlDbType)Convert.ToInt32(column["NonVersionedProviderType"]), Convert.ToInt32(column["ColumnSize"]));
                par.Value = dr[columnName];
                parameters.Add(par);
            }
            return parameters;
        }

        private bool TypeIsString(string tableName, string fieldName, DataTable schemaTable)
        {
            bool isString = false;
            //判断字段类型
            DataRow[] uniqueFieldRows = schemaTable.Select("ColumnName='" + fieldName + "'");
            if (uniqueFieldRows.Length == 0)
            {
                throw new Exception("在数据库 " + tableName + " 表未查找到" + fieldName + " Unique字段!");
            }

            switch (uniqueFieldRows[0]["DataTypeName"].ToString())
            {
                case "int":
                    isString = false;
                    break;
                default:
                    isString = true;
                    break;
            }
            return isString;
        }
    }
}
