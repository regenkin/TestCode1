using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using System.Reflection;
using DTcms.DBUtility;
using DTcms.Common;

namespace DTcms.Web.Plugin.Feedback.DAL
{
	/// <summary>
	/// ���ݷ�����:��������
	/// </summary>
	public partial class feedback
	{
        private string databaseprefix; //���ݿ����ǰ׺
        public feedback(string _databaseprefix)
		{
            databaseprefix = _databaseprefix;
        }

        #region ��������================================
        /// <summary>
		/// �Ƿ���ڸü�¼
		/// </summary>
		public bool Exists(int id)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("select count(1) from " + databaseprefix + "feedback");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}

		/// <summary>
		/// ����һ������
		/// </summary>
		public int Add(Model.feedback model)
		{
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();//�����ֶ�
            StringBuilder str2 = new StringBuilder();//���ݲ���
            //���÷��������Ե����й�������
            PropertyInfo[] pros = model.GetType().GetProperties();
            List<SqlParameter> paras = new List<SqlParameter>();
            strSql.Append("insert into " + databaseprefix + "feedback(");
            foreach (PropertyInfo pi in pros)
            {
                //�������������׷��sql�ַ���
                if (!pi.Name.Equals("id"))
                {
                    //�ж�����ֵ�Ƿ�Ϊ��
                    if (pi.GetValue(model, null) != null && !pi.GetValue(model, null).ToString().Equals(""))
                    {
                        str1.Append(pi.Name + ",");//ƴ���ֶ�
                        str2.Append("@" + pi.Name + ",");//��������
                        paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(model, null)));//�Բ�����ֵ
                    }
                }
            }
            strSql.Append(str1.ToString().Trim(','));
            strSql.Append(") values (");
            strSql.Append(str2.ToString().Trim(','));
            strSql.Append(") ");
            strSql.Append(";select @@IDENTITY;");
            object obj = DbHelperSQL.GetSingle(strSql.ToString(), paras.ToArray());
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
        /// ����һ������
        /// </summary>
        public bool Update(Model.feedback model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();
            //���÷��������Ե����й�������
            PropertyInfo[] pros = model.GetType().GetProperties();
            List<SqlParameter> paras = new List<SqlParameter>();
            strSql.Append("update  " + databaseprefix + "feedback set ");
            foreach (PropertyInfo pi in pros)
            {
                //�������������׷��sql�ַ���
                if (!pi.Name.Equals("id"))
                {
                    //�ж�����ֵ�Ƿ�Ϊ��
                    if (pi.GetValue(model, null) != null && !pi.GetValue(model, null).ToString().Equals(""))
                    {
                        str1.Append(pi.Name + "=@" + pi.Name + ",");//��������
                        paras.Add(new SqlParameter("@" + pi.Name, pi.GetValue(model, null)));//�Բ�����ֵ
                    }
                }
            }
            strSql.Append(str1.ToString().Trim(','));
            strSql.Append(" where id=@id ");
            paras.Add(new SqlParameter("@id", model.id));
            return DbHelperSQL.ExecuteSql(strSql.ToString(), paras.ToArray()) > 0;
        }

		/// <summary>
		/// ɾ��һ������
		/// </summary>
		public bool Delete(int id)
		{
			StringBuilder strSql=new StringBuilder();
            strSql.Append("delete from " + databaseprefix + "feedback");
			strSql.Append(" where id=@id");
			SqlParameter[] parameters = {
					new SqlParameter("@id", SqlDbType.Int,4)};
			parameters[0].Value = id;
            return DbHelperSQL.ExecuteSql(strSql.ToString(), parameters) > 0;
		}

        /// <summary>
        /// �õ�һ������ʵ��
        /// </summary>
        public Model.feedback GetModel(int id)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder str1 = new StringBuilder();
            Model.feedback model = new Model.feedback();
            //���÷��������Ե����й�������
            PropertyInfo[] pros = model.GetType().GetProperties();
            foreach (PropertyInfo p in pros)
            {
                str1.Append(p.Name + ",");//ƴ���ֶ�
            }
            strSql.Append("select top 1 " + str1.ToString().Trim(','));
            strSql.Append(" from " + databaseprefix + "feedback");
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
        /// ���ǰ��������
        /// </summary>
        public DataSet GetList(int Top, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" * FROM " + databaseprefix + "feedback ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by add_time desc");
            return DbHelperSQL.Query(strSql.ToString());
        }

        /// <summary>
        /// ��ò�ѯ��ҳ����
        /// </summary>
        public DataSet GetList(int pageSize, int pageIndex, string strWhere, string filedOrder, out int recordCount)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * FROM " + databaseprefix + "feedback");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            recordCount = Convert.ToInt32(DbHelperSQL.GetSingle(PagingHelper.CreateCountingSql(strSql.ToString())));
            return DbHelperSQL.Query(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql.ToString(), filedOrder));
        }

		#endregion

        #region ��չ����================================
        /// <summary>
        /// �޸�һ������
        /// </summary>
        public void UpdateField(int id, string strValue)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update " + databaseprefix + "feedback set " + strValue);
            strSql.Append(" where id=" + id);
            DbHelperSQL.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// ������ת��ʵ��
        /// </summary>
        public Model.feedback DataRowToModel(DataRow row)
        {
            Model.feedback model = new Model.feedback();
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

