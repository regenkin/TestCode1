using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using KfCrm.DBUtility;
using System.Data;

namespace KfCrm.DAL
{
    public class hr_employee_openid
    {
        public hr_employee_openid()
        { }
        #region  Method
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int Type,int OpenID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from hr_employee_openid");
            strSql.Append(" where Type=@Type and OpenID=@OpenID");
            SqlParameter[] parameters = {
					new SqlParameter("@Type", SqlDbType.Int,4),
                    new SqlParameter("@OpenID", SqlDbType.Int,4),
                                        };
            parameters[0].Value = Type;
            parameters[2].Value = OpenID;
            return DbHelperSQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(KfCrm.Model.hr_employee_openid model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into hr_employee_openid(");
            strSql.Append("hr_employeeID,Type,OpenID)");
            strSql.Append(" values (");
            strSql.Append("@hr_employeeID,@Type,@OpenID)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@hr_employeeID",SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@OpenID", SqlDbType.Int,4)
					};
            parameters[0].Value = model.hr_employeeID;
            parameters[1].Value = model.Type;
            parameters[2].Value = model.OpenID;

            object obj = DbHelperSQL.GetSingle(strSql.ToString(), parameters);
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
        /// 更新一条数据
        /// </summary>
        public bool Update(KfCrm.Model.hr_employee_openid model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update hr_employee_openid set ");
            strSql.Append("hr_employeeID=@uid,");
            strSql.Append("Type=@name,");
            strSql.Append("OpenID=@idcard,");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
                    new SqlParameter("@ID",SqlDbType.Int,4),
					new SqlParameter("@hr_employeeID",SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@OpenID", SqlDbType.Int,4)};
            parameters[0].Value = model.ID;
            parameters[1].Value = model.hr_employeeID;
            parameters[2].Value = model.Type;
            parameters[3].Value = model.OpenID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from hr_employee_openid ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
};
            parameters[0].Value = ID;

            int rows = DbHelperSQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from hr_employee_openid ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
            int rows = DbHelperSQL.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public KfCrm.Model.hr_employee_openid GetModel(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ID,hr_employeeID,Type,OpenID from hr_employee_openid ");
            strSql.Append(" where ID=@ID");
            SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
            };
            parameters[0].Value = ID;

            KfCrm.Model.hr_employee_openid model = new KfCrm.Model.hr_employee_openid();
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["ID"] != null && ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["hr_employeeID"] != null && ds.Tables[0].Rows[0]["hr_employeeID"].ToString() != "")
                {
                    model.hr_employeeID = int.Parse(ds.Tables[0].Rows[0]["hr_employeeID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Type"] != null && ds.Tables[0].Rows[0]["Type"].ToString() != "")
                {
                    model.Type = int.Parse(ds.Tables[0].Rows[0]["Type"].ToString());
                }
                if (ds.Tables[0].Rows[0]["OpenID"] != null && ds.Tables[0].Rows[0]["OpenID"].ToString() != "")
                {
                    model.OpenID = int.Parse(ds.Tables[0].Rows[0]["OpenID"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public KfCrm.Model.hr_employee GetEmployeeModel(int Type,int OpenID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 t1.* from hr_employee t1 inner join hr_employee_openid t2 on t1.ID=t2.hr_employeeID");
            strSql.Append(" where t2.Type=@Type and t2.OpenID=@OpenID");
            SqlParameter[] parameters = {
					new SqlParameter("@Type", SqlDbType.Int,4),
                    new SqlParameter("@OpenID", SqlDbType.Int,4),
};
            parameters[0].Value = Type;
            parameters[1].Value = OpenID;

            KfCrm.Model.hr_employee model;
            DataSet ds = DbHelperSQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                model = new KfCrm.Model.hr_employee();
                if (ds.Tables[0].Rows[0]["ID"] != null && ds.Tables[0].Rows[0]["ID"].ToString() != "")
                {
                    model.ID = int.Parse(ds.Tables[0].Rows[0]["ID"].ToString());
                }
                if (ds.Tables[0].Rows[0]["uid"] != null && ds.Tables[0].Rows[0]["uid"].ToString() != "")
                {
                    model.uid = ds.Tables[0].Rows[0]["uid"].ToString();
                }
                if (ds.Tables[0].Rows[0]["pwd"] != null && ds.Tables[0].Rows[0]["pwd"].ToString() != "")
                {
                    model.pwd = ds.Tables[0].Rows[0]["pwd"].ToString();
                }
                if (ds.Tables[0].Rows[0]["name"] != null && ds.Tables[0].Rows[0]["name"].ToString() != "")
                {
                    model.name = ds.Tables[0].Rows[0]["name"].ToString();
                }
                if (ds.Tables[0].Rows[0]["idcard"] != null && ds.Tables[0].Rows[0]["idcard"].ToString() != "")
                {
                    model.idcard = ds.Tables[0].Rows[0]["idcard"].ToString();
                }
                if (ds.Tables[0].Rows[0]["birthday"] != null && ds.Tables[0].Rows[0]["birthday"].ToString() != "")
                {
                    model.birthday = ds.Tables[0].Rows[0]["birthday"].ToString();
                }
                if (ds.Tables[0].Rows[0]["d_id"] != null && ds.Tables[0].Rows[0]["d_id"].ToString() != "")
                {
                    model.d_id = int.Parse(ds.Tables[0].Rows[0]["d_id"].ToString());
                }
                if (ds.Tables[0].Rows[0]["dname"] != null && ds.Tables[0].Rows[0]["dname"].ToString() != "")
                {
                    model.dname = ds.Tables[0].Rows[0]["dname"].ToString();
                }
                if (ds.Tables[0].Rows[0]["postid"] != null && ds.Tables[0].Rows[0]["postid"].ToString() != "")
                {
                    model.postid = int.Parse(ds.Tables[0].Rows[0]["postid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["post"] != null && ds.Tables[0].Rows[0]["post"].ToString() != "")
                {
                    model.post = ds.Tables[0].Rows[0]["post"].ToString();
                }
                if (ds.Tables[0].Rows[0]["email"] != null && ds.Tables[0].Rows[0]["email"].ToString() != "")
                {
                    model.email = ds.Tables[0].Rows[0]["email"].ToString();
                }
                if (ds.Tables[0].Rows[0]["sex"] != null && ds.Tables[0].Rows[0]["sex"].ToString() != "")
                {
                    model.sex = ds.Tables[0].Rows[0]["sex"].ToString();
                }
                if (ds.Tables[0].Rows[0]["tel"] != null && ds.Tables[0].Rows[0]["tel"].ToString() != "")
                {
                    model.tel = ds.Tables[0].Rows[0]["tel"].ToString();
                }
                if (ds.Tables[0].Rows[0]["status"] != null && ds.Tables[0].Rows[0]["status"].ToString() != "")
                {
                    model.status = ds.Tables[0].Rows[0]["status"].ToString();
                }
                if (ds.Tables[0].Rows[0]["zhiwuid"] != null && ds.Tables[0].Rows[0]["zhiwuid"].ToString() != "")
                {
                    model.zhiwuid = int.Parse(ds.Tables[0].Rows[0]["zhiwuid"].ToString());
                }
                if (ds.Tables[0].Rows[0]["zhiwu"] != null && ds.Tables[0].Rows[0]["zhiwu"].ToString() != "")
                {
                    model.zhiwu = ds.Tables[0].Rows[0]["zhiwu"].ToString();
                }
                if (ds.Tables[0].Rows[0]["sort"] != null && ds.Tables[0].Rows[0]["sort"].ToString() != "")
                {
                    model.sort = int.Parse(ds.Tables[0].Rows[0]["sort"].ToString());
                }
                if (ds.Tables[0].Rows[0]["EntryDate"] != null && ds.Tables[0].Rows[0]["EntryDate"].ToString() != "")
                {
                    model.EntryDate = ds.Tables[0].Rows[0]["EntryDate"].ToString();
                }
                if (ds.Tables[0].Rows[0]["address"] != null && ds.Tables[0].Rows[0]["address"].ToString() != "")
                {
                    model.address = ds.Tables[0].Rows[0]["address"].ToString();
                }
                if (ds.Tables[0].Rows[0]["remarks"] != null && ds.Tables[0].Rows[0]["remarks"].ToString() != "")
                {
                    model.remarks = ds.Tables[0].Rows[0]["remarks"].ToString();
                }
                if (ds.Tables[0].Rows[0]["education"] != null && ds.Tables[0].Rows[0]["education"].ToString() != "")
                {
                    model.education = ds.Tables[0].Rows[0]["education"].ToString();
                }
                if (ds.Tables[0].Rows[0]["level"] != null && ds.Tables[0].Rows[0]["level"].ToString() != "")
                {
                    model.level = ds.Tables[0].Rows[0]["level"].ToString();
                }
                if (ds.Tables[0].Rows[0]["professional"] != null && ds.Tables[0].Rows[0]["professional"].ToString() != "")
                {
                    model.professional = ds.Tables[0].Rows[0]["professional"].ToString();
                }
                if (ds.Tables[0].Rows[0]["schools"] != null && ds.Tables[0].Rows[0]["schools"].ToString() != "")
                {
                    model.schools = ds.Tables[0].Rows[0]["schools"].ToString();
                }
                if (ds.Tables[0].Rows[0]["title"] != null && ds.Tables[0].Rows[0]["title"].ToString() != "")
                {
                    model.title = ds.Tables[0].Rows[0]["title"].ToString();
                }
                if (ds.Tables[0].Rows[0]["isDelete"] != null && ds.Tables[0].Rows[0]["isDelete"].ToString() != "")
                {
                    model.isDelete = int.Parse(ds.Tables[0].Rows[0]["isDelete"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Delete_time"] != null && ds.Tables[0].Rows[0]["Delete_time"].ToString() != "")
                {
                    model.Delete_time = DateTime.Parse(ds.Tables[0].Rows[0]["Delete_time"].ToString());
                }
                if (ds.Tables[0].Rows[0]["portal"] != null && ds.Tables[0].Rows[0]["portal"].ToString() != "")
                {
                    model.portal = ds.Tables[0].Rows[0]["portal"].ToString();
                }
                if (ds.Tables[0].Rows[0]["theme"] != null && ds.Tables[0].Rows[0]["theme"].ToString() != "")
                {
                    model.theme = ds.Tables[0].Rows[0]["theme"].ToString();
                }
                if (ds.Tables[0].Rows[0]["canlogin"] != null && ds.Tables[0].Rows[0]["canlogin"].ToString() != "")
                {
                    model.canlogin = int.Parse(ds.Tables[0].Rows[0]["canlogin"].ToString());
                }
                return model;
            }
            else
            {
                return null;
            }
        }
        #endregion  Method
    }
}
