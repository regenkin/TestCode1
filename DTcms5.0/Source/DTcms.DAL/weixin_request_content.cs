using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Text;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.DAL
{
    /// <summary>
    /// 数据访问类:请求回复内容
    /// </summary>
    public partial class weixin_request_content
    {
        private string databaseprefix; //数据库表名前缀
        public weixin_request_content(string _databaseprefix)
        {
            databaseprefix = _databaseprefix;
        }

        #region 基本方法================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int rule_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from  " + databaseprefix + "weixin_request_content");
            strSql.Append(" where rule_id=@rule_id");
            SqlParameter[] parameters = {
					new SqlParameter("@rule_id", SqlDbType.Int,4)};
            parameters[0].Value = rule_id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.weixin_request_content GetModel(int rule_id)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();
            Model.weixin_request_content model = new Model.weixin_request_content();
            //利用反射获得属性的所有公共属性
            PropertyInfo[] pros = model.GetType().GetProperties();
            foreach (PropertyInfo p in pros)
            {
                str1.Append(p.Name + ",");//拼接字段
            }
            strSql.Append("select top 1 " + str1.ToString().Trim(','));
            strSql.Append(" from " + databaseprefix + "weixin_request_content");
            strSql.Append(" where rule_id=@rule_id");
            SqlParameter[] parameters = {
					new SqlParameter("@rule_id", SqlDbType.Int,4)};
            parameters[0].Value = rule_id;
            DataTable dt = DbHelperSQL.Query(strSql.ToString(), parameters).Tables[0];

            if (dt.Rows.Count > 0)
            {
                return DataRowToModel(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, int ruleId, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.Append(" FROM " + databaseprefix + "weixin_request_content where rule_id=" + ruleId);
            if (strWhere.Trim() != "")
            {
                strSql.Append(" and " + strWhere);
            }
            strSql.Append(" order by sort_id asc,id desc");
            return DbHelperSQL.Query(strSql.ToString());
        }
        #endregion

        #region 扩展方法================================
        /// <summary>
        /// 返回规则的第一条标题
        /// </summary>
        public string GetTitle(int rule_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 title from " + databaseprefix + "weixin_request_content");
            strSql.Append(" where rule_id=@rule_id");
            SqlParameter[] parameters = {
					new SqlParameter("@rule_id", SqlDbType.Int,4)};
            parameters[0].Value = rule_id;
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj != null)
            {
                return obj.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 返回规则的第一条内容
        /// </summary>
        public string GetContent(int rule_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 content from " + databaseprefix + "weixin_request_content");
            strSql.Append(" where rule_id=@rule_id");
            SqlParameter[] parameters = {
					new SqlParameter("@rule_id", SqlDbType.Int,4)};
            parameters[0].Value = rule_id;
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
            if (obj != null)
            {
                return obj.ToString();
            }
            return string.Empty;
        }

        /// <summary>
        /// 返回规则下面的内容数量
        /// </summary>
        public int GetCount(int rule_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(*) as H ");
            strSql.Append(" from " + databaseprefix + "weixin_request_content");
            strSql.Append(" where rule_id=@rule_id");
            SqlParameter[] parameters = {
					new SqlParameter("@rule_id", SqlDbType.Int,4)};
            parameters[0].Value = rule_id;
            return Convert.ToInt32(DbHelperSQL.GetSingle(strSql.ToString(), parameters));
        }

        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public Model.weixin_request_content DataRowToModel(DataRow row)
        {
            Model.weixin_request_content model = new Model.weixin_request_content();
            if (row != null)
            {
                //利用反射获得属性的所有公共属性
                Type modelType = model.GetType();
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    //查找实体是否存在列表相同的公共属性
                    PropertyInfo proInfo = modelType.GetProperty(row.Table.Columns[i].ColumnName);
                    if (proInfo != null && row[i] != DBNull.Value)
                    {
                        proInfo.SetValue(model, row[i], null);//用索引值设置属性值
                    }
                }
            }
            return model;
        }
        #endregion
    }
}