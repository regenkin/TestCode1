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
    /// ���ݷ�����:��Ա��۸�
    /// </summary>
    public partial class user_group_price
    {
        private string databaseprefix;//���ݿ����ǰ׺
        public user_group_price(string _databaseprefix)
        {
            databaseprefix = _databaseprefix;
        }

        #region ��������================================
        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.user_group_price GetModel(int group_id, int goods_id)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();
            Model.user_group_price model = new Model.user_group_price();
            //���÷��������Ե����й�������
            PropertyInfo[] pros = model.GetType().GetProperties();
            foreach (PropertyInfo p in pros)
            {
                str1.Append(p.Name + ",");//ƴ���ֶ�
            }
            strSql.Append("select top 1 " + str1.ToString().Trim(','));
            strSql.Append(" from " + databaseprefix + "user_group_price");
            strSql.Append(" where group_id=@group_id and goods_id=@goods_id");
            SqlParameter[] parameters = {
					new SqlParameter("@group_id", SqlDbType.Int,4),
                    new SqlParameter("@goods_id", SqlDbType.Int,4)};
            parameters[0].Value = group_id;
            parameters[1].Value = goods_id;
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
        #endregion

        #region ��չ����================================
        /// <summary>
        /// ������ת��ʵ��
        /// </summary>
        public Model.user_group_price DataRowToModel(DataRow row)
        {
            Model.user_group_price model = new Model.user_group_price();
            if (row != null)
            {
                //���÷��������Ե����й�������
                Type modelType = model.GetType();
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    //����ʵ���Ƿ�����б���ͬ�Ĺ�������
                    PropertyInfo proInfo = modelType.GetProperty(row.Table.Columns[i].ColumnName);
                    if (proInfo != null && row[i] != DBNull.Value)
                    {
                        proInfo.SetValue(model, row[i], null);//������ֵ��������ֵ
                    }
                }
            }
            return model;
        }
        #endregion
    }
}