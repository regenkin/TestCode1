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
    /// 数据访问类:商品价格
    /// </summary>
    public partial class article_goods
    {
        private string databaseprefix;//数据库表名前缀
        public article_goods(string _databaseprefix)
        {
            databaseprefix = _databaseprefix;
        }

        #region 基本方法================================
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int channel_id, int article_id, int id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from  " + databaseprefix + "article_goods");
            strSql.Append(" where channel_id=@channel_id and article_id=@article_id and id=@id");
            SqlParameter[] parameters = {
                    new SqlParameter("@channel_id", SqlDbType.Int,4),
                    new SqlParameter("@article_id", SqlDbType.Int,4),
					new SqlParameter("@id", SqlDbType.Int,4)};
            parameters[0].Value = channel_id;
            parameters[1].Value = article_id;
            parameters[2].Value = id;

            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public void Add(SqlConnection conn, SqlTransaction trans, Model.article_goods model, int channel_id, int article_id)
        {
            #region 主表信息==========================
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();//数据字段
            StringBuilder str2 = new StringBuilder();//数据参数
            //利用反射获得属性的所有公共属性
            PropertyInfo[] pros = model.GetType().GetProperties();
            List<SqlParameter> paras = new List<SqlParameter>();
            strSql.Append("insert into " + databaseprefix + "article_goods(");
            foreach (PropertyInfo pi in pros)
            {
                //如果不是主键则追加sql字符串
                if (!pi.Name.Equals("id") && !pi.Name.Equals("group_prices"))
                {
                    //判断属性值是否为空
                    if (pi.GetValue(model, null) != null && !pi.GetValue(model, null).ToString().Equals(""))
                    {
                        str1.Append(pi.Name + ",");//拼接字段
                        str2.Append("@" + pi.Name + ",");//声明参数
                        switch (pi.Name)
                        {
                            case "channel_id":
                                paras.Add(new SqlParameter("@" + pi.Name, channel_id));
                                break;
                            case "article_id":
                                paras.Add(new SqlParameter("@" + pi.Name, article_id));
                                break;
                            default:
                                paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(model, null)));//对参数赋值
                                break;
                        }
                    }
                }
            }
            strSql.Append(str1.ToString().Trim(','));
            strSql.Append(") values (");
            strSql.Append(str2.ToString().Trim(','));
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY;");
            object obj = DbHelperSQL.GetSingle(conn, trans, strSql.ToString(), paras.ToArray());
            model.id = Convert.ToInt32(obj);
            #endregion

            #region 自定义会员组价格==================
            if (model.group_prices != null)
            {
                StringBuilder strSql2; //SQL字符串
                StringBuilder str21; //数据库字段
                StringBuilder str22; //声明参数
                foreach (Model.user_group_price modelt in model.group_prices)
                {
                    strSql2 = new StringBuilder();
                    str21 = new StringBuilder();
                    str22 = new StringBuilder();
                    PropertyInfo[] pros2 = modelt.GetType().GetProperties();
                    List<SqlParameter> paras2 = new List<SqlParameter>();
                    strSql2.Append("insert into " + databaseprefix + "user_group_price(");
                    foreach (PropertyInfo pi in pros2)
                    {
                        if (!pi.Name.Equals("id"))
                        {
                            if (pi.GetValue(modelt, null) != null && !pi.GetValue(modelt, null).ToString().Equals(""))
                            {
                                str21.Append(pi.Name + ",");
                                str22.Append("@" + pi.Name + ",");
                                switch (pi.Name)
                                {
                                    case "channel_id":
                                        paras2.Add(new SqlParameter("@" + pi.Name, channel_id));
                                        break;
                                    case "article_id":
                                        paras2.Add(new SqlParameter("@" + pi.Name, article_id));
                                        break;
                                    case "goods_id":
                                        paras2.Add(new SqlParameter("@" + pi.Name, model.id)); //将刚插入的父ID赋值
                                        break;
                                    default:
                                        paras2.Add(new SqlParameter("@" + pi.Name, pi.GetValue(modelt, null)));
                                        break;
                                }
                            }
                        }
                    }
                    strSql2.Append(str21.ToString().Trim(','));
                    strSql2.Append(") values (");
                    strSql2.Append(str22.ToString().Trim(','));
                    strSql.Append(")");
                    DbHelperSQL.ExecuteSql(conn, trans, strSql2.ToString(), paras2.ToArray());
                }
            }
            #endregion
        }

        /// <summary>
        /// 删除一条数据，带事务
        /// </summary>
        public void Delete(SqlConnection conn, SqlTransaction trans, int channel_id, int article_id)
        {
            //删除用户组价格
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from " + databaseprefix + "user_group_price");
            strSql.Append(" where channel_id=@channel_id and article_id=@article_id");
            SqlParameter[] parameters = {
                    new SqlParameter("@channel_id", SqlDbType.Int,4),
					new SqlParameter("@article_id", SqlDbType.Int,4)};
            parameters[0].Value = channel_id;
            parameters[1].Value = article_id;
            DbHelperSQL.ExecuteSql(conn, trans, strSql.ToString(), parameters);

            //删除主表
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from " + databaseprefix + "article_goods");
            strSql2.Append(" where channel_id=@channel_id and article_id=@article_id");
            SqlParameter[] parameters2 = {
					new SqlParameter("@channel_id", SqlDbType.Int,4),
					new SqlParameter("@article_id", SqlDbType.Int,4)};
            parameters2[0].Value = channel_id;
            parameters2[1].Value = article_id;
            DbHelperSQL.ExecuteSql(conn, trans, strSql2.ToString(), parameters2);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public Model.article_goods GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();
            Model.article_goods model = new Model.article_goods();
            //利用反射获得属性的所有公共属性
            PropertyInfo[] pros = model.GetType().GetProperties();
            foreach (PropertyInfo p in pros)
            {
                //拼接字段，忽略List<T>
                if (!p.Name.Equals("group_prices"))
                {
                    str1.Append(p.Name + ",");
                }
            }
            strSql.Append("select top 1 " + str1.ToString().Trim(','));
            strSql.Append(" from " + databaseprefix + "article_goods");
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
        /// 根据规格列表查询商品实体
        /// </summary>
        public Model.article_goods GetModel(int channel_id, int article_id, string spec_ids)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();
            Model.article_goods model = new Model.article_goods();
            //利用反射获得属性的所有公共属性
            PropertyInfo[] pros = model.GetType().GetProperties();
            foreach (PropertyInfo p in pros)
            {
                //拼接字段，忽略List<T>
                if (!p.Name.Equals("group_prices"))
                {
                    str1.Append(p.Name + ",");
                }
            }
            strSql.Append("select top 1 " + str1.ToString().Trim(','));
            strSql.Append(" from " + databaseprefix + "article_goods");
            strSql.Append(" where channel_id=@channel_id and article_id=@article_id and spec_ids=@spec_ids");
            SqlParameter[] parameters = {
					new SqlParameter("@channel_id", SqlDbType.Int,4),
                    new SqlParameter("@article_id", SqlDbType.Int,4),
					new SqlParameter("@spec_ids", SqlDbType.NVarChar,500)};
            parameters[0].Value = channel_id;
            parameters[1].Value = article_id;
            parameters[2].Value = spec_ids;
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
        /// 获得数据列表
        /// </summary>
        public List<Model.article_goods> GetList(int channel_id, int article_id)
        {
            List<Model.article_goods> modelList = new List<Model.article_goods>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM " + databaseprefix + "article_goods");
            strSql.Append(" where channel_id=" + channel_id + " and article_id=" + article_id);
            DataTable dt = DbHelperSQL.Query(strSql.ToString()).Tables[0];

            if (dt.Rows.Count > 0)
            {
                Model.article_goods model;
                for (int n = 0; n < dt.Rows.Count; n++)
                {
                    #region 主表数据========================
                    model = new Model.article_goods();
                    //利用反射获得属性的所有公共属性
                    Type modelType = model.GetType();
                    for (int i = 0; i < dt.Rows[n].Table.Columns.Count; i++)
                    {
                        //查找实体是否存在列表相同的公共属性
                        PropertyInfo proInfo = modelType.GetProperty(dt.Rows[n].Table.Columns[i].ColumnName);
                        if (proInfo != null && dt.Rows[n][i] != DBNull.Value)
                        {
                            proInfo.SetValue(model, dt.Rows[n][i], null);//用索引值设置属性值
                        }
                    }
                    #endregion

                    #region 用户组价格数据==================
                    StringBuilder strSql2 = new StringBuilder();
                    strSql2.Append("select * FROM " + databaseprefix + "user_group_price");
                    strSql2.Append(" where goods_id=" + model.id);
                    DataTable dt2 = DbHelperSQL.Query(strSql2.ToString()).Tables[0];
                    if (dt2.Rows.Count > 0)
                    {
                        List<Model.user_group_price> ls = new List<Model.user_group_price>();
                        Model.user_group_price gpModel;
                        for (int j = 0; j < dt2.Rows.Count; j++)
                        {
                            gpModel = new Model.user_group_price();
                            //利用反射获得属性的所有公共属性
                            Type gpType = gpModel.GetType();
                            for (int i = 0; i < dt2.Rows[j].Table.Columns.Count; i++)
                            {
                                //查找实体是否存在列表相同的公共属性
                                PropertyInfo proInfo = gpType.GetProperty(dt2.Rows[j].Table.Columns[i].ColumnName);
                                if (proInfo != null && dt.Rows[j][i] != DBNull.Value)
                                {
                                    proInfo.SetValue(gpModel, dt2.Rows[j][i], null);//用索引值设置属性值
                                }
                            }
                            ls.Add(gpModel);
                        }
                        model.group_prices = ls;
                    }
                    #endregion

                    modelList.Add(model);
                }
            }
            return modelList;
        }
        #endregion

        #region 扩展方法================================
        /// <summary>
        /// 将对象转换实体
        /// </summary>
        public Model.article_goods DataRowToModel(DataRow row)
        {
            Model.article_goods model = new Model.article_goods();
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