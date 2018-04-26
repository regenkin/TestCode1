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
    /// 数据访问类:商品规格
    /// </summary>
    public partial class article_spec
    {
        private string databaseprefix;//数据库表名前缀
        public article_spec(string _databaseprefix)
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
            strSql.Append("select count(1) from  " + databaseprefix + "article_spec");
            strSql.Append(" where ");
            strSql.Append(" id = @id  ");
            SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(Model.article_spec model)
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
                        //数据字段
                        StringBuilder str1 = new StringBuilder();
                        //数据参数
                        StringBuilder str2 = new StringBuilder();
                        //利用反射获得属性的所有公共属性
                        PropertyInfo[] pros = model.GetType().GetProperties();
                        List<SqlParameter> paras = new List<SqlParameter>();
                        strSql.Append("insert into " + databaseprefix + "article_spec(");
                        foreach (PropertyInfo pi in pros)
                        {
                            //如果不是主键或List<T>则追加sql字符串
                            if (!pi.Name.Equals("id") && !typeof(System.Collections.IList).IsAssignableFrom(pi.PropertyType))
                            {
                                //判断属性值是否为空
                                if (pi.GetValue(model, null) != null && !pi.GetValue(model, null).ToString().Equals(""))
                                {
                                    //拼接字段
                                    str1.Append(pi.Name + ",");
                                    //声明参数
                                    str2.Append("@" + pi.Name + ",");
                                    //对参数赋值
                                    paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(model, null)));
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

                        #region 规格值信息========================
                        if (model.values != null)
                        {
                            StringBuilder strSql2;//SQL字符串
                            StringBuilder str21;//数据库字段
                            StringBuilder str22;//声明参数
                            foreach (Model.article_spec_value modelt in model.values)
                            {
                                strSql2 = new StringBuilder();
                                str21 = new StringBuilder();
                                str22 = new StringBuilder();
                                PropertyInfo[] pros2 = modelt.GetType().GetProperties();
                                List<SqlParameter> paras2 = new List<SqlParameter>();
                                strSql2.Append("insert into " + databaseprefix + "article_spec(");
                                foreach (PropertyInfo pi in pros2)
                                {
                                    if (!pi.Name.Equals("id"))
                                    {
                                        if (pi.GetValue(modelt, null) != null && !pi.GetValue(modelt, null).ToString().Equals(""))
                                        {
                                            str21.Append(pi.Name + ",");
                                            str22.Append("@" + pi.Name + ",");
                                            if (pi.Name.Equals("parent_id"))
                                            {
                                                paras2.Add(new SqlParameter("@" + pi.Name, model.id));//将刚插入的父ID赋值
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
        public bool Update(Model.article_spec model)
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
                        strSql.Append("update  " + databaseprefix + "article_spec set ");
                        foreach (PropertyInfo pi in pros)
                        {
                            //如果不是主键或List<T>则追加sql字符串
                            if (!pi.Name.Equals("id") && !typeof(System.Collections.IList).IsAssignableFrom(pi.PropertyType))
                            {
                                //判断属性值是否为空
                                if (pi.GetValue(model, null) != null && !pi.GetValue(model, null).ToString().Equals(""))
                                {
                                    //声明参数
                                    str1.Append(pi.Name + "=@" + pi.Name + ",");
                                    //对参数赋值
                                    paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(model, null)));
                                }
                            }
                        }
                        strSql.Append(str1.ToString().Trim(','));
                        strSql.Append(" where id=@id ");
                        paras.Add(new SqlParameter("@id", model.id));
                        DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), paras.ToArray());
                        #endregion

                        #region 规格值信息========================
                        //删除已删除的规格值
                        DeleteValues(conn, trans, model.values, model.id);
                        //添加/修改规格值
                        if (model.values != null)
                        {
                            StringBuilder strSql2; //SQL字符串
                            StringBuilder str21; //数据库字段
                            StringBuilder str22; //声明参数
                            foreach (Model.article_spec_value modelt in model.values)
                            {
                                strSql2 = new StringBuilder();
                                str21 = new StringBuilder();
                                str22 = new StringBuilder();
                                PropertyInfo[] pros2 = modelt.GetType().GetProperties();
                                List<SqlParameter> paras2 = new List<SqlParameter>();
                                if (modelt.id > 0)
                                {
                                    strSql2.Append("update " + databaseprefix + "article_spec set ");
                                    foreach (PropertyInfo pi in pros2)
                                    {
                                        //如果不是主键则追加sql字符串
                                        if (!pi.Name.Equals("id"))
                                        {
                                            //判断属性值是否为空
                                            if (pi.GetValue(modelt, null) != null && !pi.GetValue(modelt, null).ToString().Equals(""))
                                            {
                                                str21.Append(pi.Name + "=@" + pi.Name + ",");
                                                paras2.Add(new SqlParameter("@" + pi.Name, pi.GetValue(modelt, null)));
                                            }
                                        }
                                    }
                                    strSql2.Append(str21.ToString().Trim(','));
                                    strSql2.Append(" where id=@id ");
                                    paras2.Add(new SqlParameter("@id", modelt.id));
                                    DbHelperSQL.ExecuteSql(conn, trans, strSql2.ToString(), paras2.ToArray());
                                }
                                else
                                {
                                    strSql2.Append("insert into " + databaseprefix + "article_spec(");
                                    foreach (PropertyInfo pi in pros2)
                                    {
                                        if (!pi.Name.Equals("id"))
                                        {
                                            if (pi.GetValue(modelt, null) != null && !pi.GetValue(modelt, null).ToString().Equals(""))
                                            {
                                                str21.Append(pi.Name + ",");
                                                str22.Append("@" + pi.Name + ",");
                                                paras2.Add(new SqlParameter("@" + pi.Name, pi.GetValue(modelt, null)));
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
            //删除规格值
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("delete from " + databaseprefix + "article_spec");
            strSql1.Append(" where parent_id=@parent_id");
            SqlParameter[] parameters1 = {
					new SqlParameter("@parent_id", SqlDbType.Int,4)};
            parameters1[0].Value = id;
            List<CommandInfo> sqllist = new List<CommandInfo>();
            CommandInfo cmd = new CommandInfo(strSql1.ToString(), parameters1);
            sqllist.Add(cmd);

            //删除规格
			StringBuilder strSql=new StringBuilder();
            strSql.Append("delete from " + databaseprefix + "article_spec ");
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
        public Model.article_spec GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();
            Model.article_spec model = new Model.article_spec();
            //利用反射获得属性的所有公共属性
            PropertyInfo[] pros = model.GetType().GetProperties();
            foreach (PropertyInfo p in pros)
            {
                //拼接字段，忽略List<T>
                if (!typeof(System.Collections.IList).IsAssignableFrom(p.PropertyType))
                {
                    str1.Append(p.Name + ",");
                }
            }
            strSql.Append("select top 1 " + str1.ToString().Trim(','));
            strSql.Append(" from " + databaseprefix + "article_spec");
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
            strSql.Append(" FROM  " + databaseprefix + "article_spec ");
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
            strSql.Append("select * FROM " + databaseprefix + "article_spec");
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
        public bool UpdateField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update " + databaseprefix + "article_spec set " + strValue);
            strSql.Append(" where id=" + id);
            return DbHelperSQL.ExecuteSql(strSql.ToString()) > 0;
        }

        /// <summary>
        /// 返回栏目规格列表
        /// </summary>
        public DataSet GetCategorySpecList(int category_id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select S.* FROM " + databaseprefix + "article_spec as S INNER JOIN " + databaseprefix + "article_category_spec as C");
            strSql.Append(" ON S.id=C.spec_id and C.category_id=" + category_id);
            strSql.Append(" order by sort_id asc,S.id desc");
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public Model.article_spec DataRowToModel(DataRow row)
        {
            Model.article_spec model = new Model.article_spec();
            if (row != null)
            {
                #region 主表信息======================
                //利用反射获得属性的所有公共属性
                Type modelType = model.GetType();
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    PropertyInfo proInfo = modelType.GetProperty(row.Table.Columns[i].ColumnName);
                    if (proInfo != null && row[i] != DBNull.Value)
                    {
                        //用索引值设置属性值
                        proInfo.SetValue(model, row[i], null);
                    }
                }
                #endregion

                #region 子表信息======================
                StringBuilder strSql1 = new StringBuilder();
                strSql1.Append("select * from " + databaseprefix + "article_spec");
                strSql1.Append(" where parent_id=@parent_id");
                SqlParameter[] parameters1 = {
					    new SqlParameter("@parent_id", SqlDbType.Int,4)};
                parameters1[0].Value = model.id;
                DataTable dt1 = DbHelperSQL.Query(strSql1.ToString(), parameters1).Tables[0];

                if (dt1.Rows.Count > 0)
                {
                    int rowsCount = dt1.Rows.Count;
                    List<Model.article_spec_value> models = new List<Model.article_spec_value>();
                    Model.article_spec_value modelt;
                    for (int n = 0; n < rowsCount; n++)
                    {
                        modelt = new Model.article_spec_value();
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
                    model.values = models;
                }
                #endregion
            }
            return model;
        }
        #endregion

        #region 私有方法================================
        /// <summary>
        /// 查找不存在的规格值并删除
        /// </summary>
        private void DeleteValues(SqlConnection conn, SqlTransaction trans, List<Model.article_spec_value> models, int parentId)
        {
            StringBuilder idList = new StringBuilder();
            if (models != null)
            {
                foreach (Model.article_spec_value modelt in models)
                {
                    if (modelt.id > 0)
                    {
                        idList.Append(modelt.id + ",");
                    }
                }
                string id_list = Utils.DelLastChar(idList.ToString(), ",");
                if (!string.IsNullOrEmpty(id_list))
                {
                    DbHelperSQL.ExecuteSql(conn, trans, "delete from " + databaseprefix + "article_spec where parent_id=" + parentId + " and id not in(" + id_list + ")");
                }
            }
        }
        #endregion
    }
}