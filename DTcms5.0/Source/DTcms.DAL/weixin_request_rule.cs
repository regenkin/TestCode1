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
    /// 数据访问类:请求回复规格
    /// </summary>
    public partial class weixin_request_rule
    {
        private string databaseprefix;//数据库表名前缀
        public weixin_request_rule(string _databaseprefix)
        {
            databaseprefix = _databaseprefix;
        }

        #region 基本方法================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from  " + databaseprefix + "weixin_request_rule");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.weixin_request_rule model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();//打开数据连接
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region 主表信息==========================
                        StringBuilder strSql = new StringBuilder();
                        StringBuilder str1 = new StringBuilder();//数据字段
                        StringBuilder str2 = new StringBuilder();//数据参数
                        //利用反射获得属性的所有公共属性
                        PropertyInfo[] pros = model.GetType().GetProperties();
                        List<SqlParameter> paras = new List<SqlParameter>();
                        strSql.Append("insert into " + databaseprefix + "weixin_request_rule(");
                        foreach (PropertyInfo pi in pros)
                        {
                            //如果不是主键或List<T>则追加sql字符串
                            if (!pi.Name.Equals("id") && !pi.Name.Equals("contents"))
                            {
                                //判断属性值是否为空
                                if (pi.GetValue(model, null) != null && !pi.GetValue(model, null).ToString().Equals(""))
                                {
                                    str1.Append(pi.Name + ",");//拼接字段
                                    str2.Append("@" + pi.Name + ",");//声明参数
                                    paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(model, null)));//对参数赋值
                                }
                            }
                        }
                        strSql.Append(str1.ToString().Trim(','));
                        strSql.Append(") values (");
                        strSql.Append(str2.ToString().Trim(','));
                        strSql.Append(") ");
                        strSql.Append(";select @@IDENTITY;");
                        object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), paras.ToArray());//带事务
                        model.id = Convert.ToInt32(obj);
                        #endregion

                        #region 规则内容表信息====================
                        if (model.contents != null)
                        {
                            StringBuilder strSql2; //SQL字符串
                            StringBuilder str21; //数据库字段
                            StringBuilder str22; //声明参数
                            foreach (Model.weixin_request_content modelt in model.contents)
                            {
                                strSql2 = new StringBuilder();
                                str21 = new StringBuilder();
                                str22 = new StringBuilder();
                                PropertyInfo[] pros2 = modelt.GetType().GetProperties();
                                List<SqlParameter> paras2 = new List<SqlParameter>();
                                strSql2.Append("insert into " + databaseprefix + "weixin_request_content(");
                                foreach (PropertyInfo pi in pros2)
                                {
                                    if (!pi.Name.Equals("id"))
                                    {
                                        if (pi.GetValue(modelt, null) != null && !pi.GetValue(modelt, null).ToString().Equals(""))
                                        {
                                            str21.Append(pi.Name + ",");
                                            str22.Append("@" + pi.Name + ",");
                                            if (pi.Name.Equals("rule_id"))
                                            {
                                                paras2.Add(new SqlParameter("@" + pi.Name, model.id)); //将刚插入的父ID赋值
                                            }
                                            else
                                            {
                                                paras2.Add(new SqlParameter("@" + pi.Name, pi.GetValue(modelt, null)));
                                            }
                                        }
                                    }
                                }
                                strSql2.Append(str21.ToString().Trim(','));
                                strSql2.Append(") values (");
                                strSql2.Append(str22.ToString().Trim(','));
                                strSql2.Append(") ");
                                DbHelperSQL.ExecuteSql(conn, trans, strSql2.ToString(), paras2.ToArray());
                            }
                        }
                        #endregion

                        trans.Commit();//提交事务
                    }
                    catch
                    {
                        trans.Rollback();//回滚事务
                        return 0;
                    }
                }
            }
            return model.id;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(Model.weixin_request_rule model)
        {
            using (SqlConnection conn = new SqlConnection(DbHelperSQL.connectionString))
            {
                conn.Open();//打开数据连接
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        #region 主表信息==========================
                        StringBuilder strSql = new StringBuilder();
                        StringBuilder str1 = new StringBuilder();
                        //利用反射获得属性的所有公共属性
                        PropertyInfo[] pros = model.GetType().GetProperties();
                        List<SqlParameter> paras = new List<SqlParameter>();
                        strSql.Append("update " + databaseprefix + "weixin_request_rule set ");
                        foreach (PropertyInfo pi in pros)
                        {
                            //如果不是主键或List<T>则追加sql字符串
                            if (!pi.Name.Equals("id") && !pi.Name.Equals("contents"))
                            {
                                //判断属性值是否为空
                                if (pi.GetValue(model, null) != null && !pi.GetValue(model, null).ToString().Equals(""))
                                {
                                    str1.Append(pi.Name + "=@" + pi.Name + ",");//声明参数
                                    paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(model, null)));//对参数赋值
                                }
                            }
                        }
                        strSql.Append(str1.ToString().Trim(','));
                        strSql.Append(" where id=@id ");
                        paras.Add(new SqlParameter("@id", model.id));
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), paras.ToArray());
                        #endregion

                        #region 规则内容表信息====================
                        //先删除的规则内容
                        DbHelperSQL.ExecuteSql(conn, trans, "delete from " + databaseprefix + "weixin_request_content where rule_id=" + model.id);
                        //重新添加规则内容
                        if (model.contents != null)
                        {
                            StringBuilder strSql2; //SQL字符串
                            StringBuilder str21; //数据库字段
                            StringBuilder str22; //声明参数
                            foreach (Model.weixin_request_content modelt in model.contents)
                            {
                                strSql2 = new StringBuilder();
                                str21 = new StringBuilder();
                                str22 = new StringBuilder();
                                PropertyInfo[] pros2 = modelt.GetType().GetProperties();
                                List<SqlParameter> paras2 = new List<SqlParameter>();
                                strSql2.Append("insert into " + databaseprefix + "weixin_request_content(");
                                foreach (PropertyInfo pi in pros2)
                                {
                                    if (!pi.Name.Equals("id"))
                                    {
                                        if (pi.GetValue(modelt, null) != null && !pi.GetValue(modelt, null).ToString().Equals(""))
                                        {
                                            str21.Append(pi.Name + ",");
                                            str22.Append("@" + pi.Name + ",");
                                            if (pi.Name.Equals("rule_id"))
                                            {
                                                paras2.Add(new SqlParameter("@" + pi.Name, model.id)); //将规则ID赋值
                                            }
                                            else
                                            {
                                                paras2.Add(new SqlParameter("@" + pi.Name, pi.GetValue(modelt, null)));
                                            }
                                        }
                                    }
                                }
                                strSql2.Append(str21.ToString().Trim(','));
                                strSql2.Append(") values (");
                                strSql2.Append(str22.ToString().Trim(','));
                                strSql2.Append(") ");
                                DbHelperSQL.ExecuteSql(conn, trans, strSql2.ToString(), paras2.ToArray());
                            }
                        }
                        #endregion

                        trans.Commit();//提交事务
                    }
                    catch
                    {
                        trans.Rollback();//回滚事务
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //删除规则内容表
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from " + databaseprefix + "weixin_request_content");
            strSql2.Append(" where rule_id=@rule_id ");
            SqlParameter[] parameters2 = {
					new SqlParameter("@rule_id", SqlDbType.Int,4)};
            parameters2[0].Value = id;
            CommandInfo cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            //删除规则主表
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from " + databaseprefix + "weixin_request_rule");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;
            cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            return DbHelperSQL.ExecuteSqlTran(sqllist) > 0;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.weixin_request_rule GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();
            Model.weixin_request_rule model = new Model.weixin_request_rule();
            //利用反射获得属性的所有公共属性
            PropertyInfo[] pros = model.GetType().GetProperties();
            foreach (PropertyInfo p in pros)
            {
                //拼接字段，忽略List<T>
                if (!p.Name.Equals("values") && !p.Name.Equals("contents"))
                {
                    str1.Append(p.Name + ",");
                }
            }
            strSql.Append("select top 1 " + str1.ToString().Trim(','));
            strSql.Append(" from " + databaseprefix + "weixin_request_rule");
            strSql.Append(" where id=@id");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;
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
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * ");
            strSql.Append(" FROM  " + databaseprefix + "weixin_request_rule ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得查询分页数据
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM " + databaseprefix + "weixin_request_rule");
            if (strWhere.Trim() != "")
            {
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
        public void UpdateField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update " + databaseprefix + "weixin_request_rule set " + strValue);
            strSql.Append(" where id=" + id);
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.weixin_request_rule GetModel(int account_id, int request_type)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();
            Model.weixin_request_rule model = new Model.weixin_request_rule();
            //利用反射获得属性的所有公共属性
            PropertyInfo[] pros = model.GetType().GetProperties();
            foreach (PropertyInfo p in pros)
            {
                //拼接字段，忽略List<T>
                if (!p.Name.Equals("values") && !p.Name.Equals("contents"))
                {
                    str1.Append(p.Name + ",");
                }
            }
            strSql.Append("select top 1 " + str1.ToString().Trim(','));
            strSql.Append(" from " + databaseprefix + "weixin_request_rule");
            strSql.Append(" where account_id=@account_id and request_type=@request_type");
            SqlParameter[] parameters = {
					new SqlParameter("@account_id", SqlDbType.Int,4),
                    new SqlParameter("@request_type", SqlDbType.Int,4)};
            parameters[0].Value = account_id;
            parameters[1].Value = request_type;

            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public Model.weixin_request_rule DataRowToModel(DataRow row)
        {
            Model.weixin_request_rule model = new Model.weixin_request_rule();
            if (row != null)
            {
                #region 主表信息======================
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
                #endregion

                #region 子表信息======================
                StringBuilder strSql1 = new StringBuilder();
                strSql1.Append("select * from " + databaseprefix + "weixin_request_content");
                strSql1.Append(" where rule_id=@rule_id");
                SqlParameter[] parameters1 = {
					    new SqlParameter("@rule_id", SqlDbType.Int,4)};
                parameters1[0].Value = model.id;
                DataTable dt1 = DbHelperSQL.Query(strSql1.ToString(), parameters1).Tables[0];

                if (dt1.Rows.Count > 0)
                {
                    int rowsCount = dt1.Rows.Count;
                    List<Model.weixin_request_content> models = new List<Model.weixin_request_content>();
                    Model.weixin_request_content modelt;
                    for (int n = 0; n < rowsCount; n++)
                    {
                        modelt = new Model.weixin_request_content();
                        Type modeltType = modelt.GetType();
                        for (int i = 0; i < dt1.Rows[n].Table.Columns.Count; i++)
                        {
                            PropertyInfo proInfo = modeltType.GetProperty(dt1.Rows[n].Table.Columns[i].ColumnName);
                            if (proInfo != null && dt1.Rows[n][i] != DBNull.Value)
                            {
                                proInfo.SetValue(modelt, dt1.Rows[n][i], null);
                            }
                        }
                        models.Add(modelt);
                    }
                    model.contents = models;
                }
                #endregion
            }
            return model;
        }
        #endregion

        #region 微信通讯方法============================
        /// <summary>
        /// 得到规则ID以及回复类型
        /// </summary>
        public int GetRuleIdAndResponseType(int account_id, string strWhere, out int response_type)
        {
            int rule_id = 0;
            response_type = 0;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 id,response_type from " + databaseprefix + "weixin_request_rule");
            strSql.Append(" where account_id=" + account_id);
            if (strWhere != null && strWhere.Length > 0)
            {
                strSql.Append(" and " + strWhere);
            }
            SqlDataReader sr = DbHelperSQL.ExecuteReader(strSql.ToString());

            while (sr.Read())
            {
                rule_id = int.Parse(sr["id"].ToString());
                response_type = int.Parse(sr["response_type"].ToString());
            }
            sr.Close();

            return rule_id;
        }

        /// <summary>
        /// 得到关健字查询的规则ID及回复类型(如需提高效率可使用存储过程)
        /// </summary>
        public int GetKeywordsRuleId(int account_id, string keywords, out int response_type)
        {
            int rule_id = 0;
            //精确匹配
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("select top 1 id,response_type from " + databaseprefix + "weixin_request_rule");
            strSql3.Append(" where account_id=" + account_id + " and request_type=1");
            strSql3.Append(" and(keywords like '" + keywords + "|%' or keywords='%|" + keywords + "' or keywords like '%|" + keywords + "|%' or keywords='" + keywords + "')");
            strSql3.Append(" order by sort_id asc,add_time desc");
            DataSet ds3 = DbHelperSQL.Query(strSql3.ToString());
            if (ds3.Tables[0].Rows.Count > 0)
            {
                rule_id = int.Parse(ds3.Tables[0].Rows[0][0].ToString());
                response_type = int.Parse(ds3.Tables[0].Rows[0][1].ToString());
                return rule_id;
            }
            //模糊匹配
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("select top 1 id,response_type from " + databaseprefix + "weixin_request_rule");
            strSql2.Append(" where account_id=" + account_id + " and request_type=1 and keywords like '%" + keywords + "%'");
            strSql2.Append(" order by sort_id asc,add_time desc");
            DataSet ds2 = DbHelperSQL.Query(strSql2.ToString());
            if (ds2.Tables[0].Rows.Count > 0)
            {
                rule_id = int.Parse(ds2.Tables[0].Rows[0][0].ToString());
                response_type = int.Parse(ds2.Tables[0].Rows[0][1].ToString());
                return rule_id;
            }
            //默认回复
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("select top 1 id,response_type from " + databaseprefix + "weixin_request_rule");
            strSql1.Append(" where account_id=" + account_id + " and request_type=0");
            strSql1.Append(" order by sort_id asc,add_time desc");
            DataSet ds1 = DbHelperSQL.Query(strSql1.ToString());
            if (ds1.Tables[0].Rows.Count > 0)
            {
                rule_id = int.Parse(ds1.Tables[0].Rows[0][0].ToString());
                response_type = int.Parse(ds1.Tables[0].Rows[0][1].ToString());
                return rule_id;
            }
            response_type = 0;
            return rule_id;
        }
        #endregion
    }
}