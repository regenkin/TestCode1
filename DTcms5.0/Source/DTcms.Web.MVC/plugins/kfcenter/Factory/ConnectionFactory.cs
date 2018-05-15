using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;

namespace DTcms.Web.Mvc.Plugin.KfCenter.Factory
{
    /// <summary>
    /// Connection工厂用于实例化对应的IDbConnection对象，传递给dDapper。
    /// </summary>
    public class ConnectionFactory
    {
        //private static readonly string connectionName = ConfigurationManager.AppSettings["SqlCEConnectionName"];
        //private static readonly string connString = ConfigurationManager.ConnectionStrings["SqlCEConnectionName"].ConnectionString;

        public static IDbConnection CreateConnection()
        {
            IDbConnection conn = null;
            //switch (connectionName)
            //{
            //    case "SQLServer":
            //        conn = new System.Data.SqlClient.SqlConnection(connString);
            //        break;
            //    case "MySQL":
            //        conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            //        break;
            //    case "Access":
            //        conn = new System.Data.OleDb.OleDbConnection(connString);
            //        break;
            //    case "SqlCE":
            //        string sss = "Data Source=kinfar.sdf";
            //        var connSqlCE = new System.Data.SqlServerCe.SqlCeConnection(sss);
            //        connSqlCE.Open();
            //        return connSqlCE;
            //    default:
            //          conn = new System.Data.SqlClient.SqlConnection(connString);
            //        break;
            //}
            conn = new System.Data.SqlClient.SqlConnection("server=tcp:119.29.91.240,50362;uid=sa;pwd=sa;database=kfcenter;Connection Lifetime=30");
            conn.Open();
            return conn;
        }

    }
}
