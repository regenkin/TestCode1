using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using KfCrm.Common;
using System.IO;
using System.Data.SqlClient;
using System.Xml;
using System.Reflection;

namespace CRM.Data
{
    /// <summary>
    /// install 的摘要说明
    /// </summary>
    public class install : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            if (request["Action"] == "initcheck")
            {
                //conn.config check
                string filename = null;
                if (context != null)
                    filename = context.Server.MapPath(@"\conn.config");
                int check_config = CheckConfig(filename);

                //floder check
                int check_floder = CheckFolder("install");
                int check_datafloder = CheckFolder("App_Data");
                int check_configed = configed();


                context.Response.Write(string.Format("{0},{1},{2},{3}", check_config, check_floder, check_datafloder, check_configed));
            }
            if (request["Action"] == "checkconnect")
            {
                string servername = PageValidate.InputText(request["t_name"], 255);
                string uid = PageValidate.InputText(request["t_uid"], 255);
                string pwd = PageValidate.InputText(request["t_pwd"], 255);
                string dbname = PageValidate.InputText(request["t_db_name"], 255);

                string connstr1 = string.Format("server={0};uid={1};pwd={2}", servername, uid, pwd);
                string connstr2 = string.Format("server={0};uid={1};pwd={2};database={3}", servername, uid, pwd, dbname);

                int check_configed = configed();
                if (check_configed == 1)
                {
                    context.Response.Write("false:configed");
                    return;
                }
                try
                {
                    SqlConnection myconn1 = new SqlConnection(connstr1);
                    myconn1.Open();
                    try
                    {
                        SqlConnection myconn2 = new SqlConnection(connstr2);
                        myconn2.Open();

                        context.Response.Write("warn:dbname");
                    }
                    catch
                    {
                        int check_datafile = CheckFile(context.Server.MapPath(@"..\App_Data\\" + dbname + ".mdf"));
                        if (check_datafile == 0)
                            context.Response.Write("success");
                        else
                            context.Response.Write("false:dbfile");
                    }
                }
                catch
                {
                    context.Response.Write("false:connect");
                }
            }
            if (request["Action"] == "startconfig")
            {
                string servername = PageValidate.InputText(request["t_name"], 255);
                string uid = PageValidate.InputText(request["t_uid"], 255);
                string pwd = PageValidate.InputText(request["t_pwd"], 255);
                string dbname = PageValidate.InputText(request["t_db_name"], 255);
                string Encrypt = PageValidate.InputText(request["t_Encrypt_val"], 255);

                string connstr1 = string.Format("server={0};uid={1};pwd={2}", servername, uid, pwd);
                string connstr2 = string.Format("server={0};uid={1};pwd={2};database={3}", servername, uid, pwd, dbname);

                string sqlencrypt = "false";
                string regconnstr = connstr2;
                if (Encrypt == "0")
                {
                    sqlencrypt = "true";
                    regconnstr = KfCrm.Common.DEncrypt.DESEncrypt.Encrypt(connstr2);
                }

                //创建数据库测试
                SqlConnection myconn1 = new SqlConnection(connstr1);
                string physicsPath = context.Server.MapPath(@"..\App_Data\\");
                string sql = "";

                sql += "if DB_ID('" + dbname + "') is null ";
                sql += " CREATE DATABASE [" + dbname + "] ON  PRIMARY ";
                sql += "( NAME = N'" + dbname + "', FILENAME = N'" + physicsPath + dbname + ".mdf' , SIZE = 5120KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )  ";
                sql += " LOG ON        ";
                sql += "( NAME = N'" + dbname + "_log', FILENAME = N'" + physicsPath + dbname + ".ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)  ";

                SqlCommand cmd = new SqlCommand(sql, myconn1);
                myconn1.Open();
                int ex = cmd.ExecuteNonQuery();
                myconn1.Close();

                //context.Response.Write(ex);

                //执行sql
                StringBuilder sb = new StringBuilder();
                using (StreamReader objReader = new StreamReader(context.Server.MapPath(@"..\install\sql.sql"), Encoding.UTF8))
                {
                    sb.Append(objReader.ReadToEnd());
                    objReader.Close();
                }
                string commandText =sb.ToString();
                string splitter = "\r\nGO\r\n";

                int startPos = 0;
                do
                {
                    int lastPos = commandText.IndexOf(splitter, startPos);
                    int len = (lastPos > startPos ? lastPos : commandText.Length) - startPos;
                    string query = commandText.Substring(startPos, len);

                    if (query.Trim().Length > 0)
                    {
                        query = "USE [" + dbname + "] " + query; 
                        SqlCommand cmdtable = new SqlCommand(query, myconn1);
                        myconn1.Open();
                        cmdtable.ExecuteNonQuery();
                        myconn1.Close();
                    }

                    if (lastPos == -1)
                        break;
                    else
                        startPos = lastPos + splitter.Length;
                } while (startPos < commandText.Length);


                //保存连接字符串
                saveconfig(sqlencrypt, regconnstr);
            }
        }

        public int CheckConfig(string webconfigfile)
        {
            try
            {
                HttpContext context = HttpContext.Current;

                StreamReader sr = new StreamReader(webconfigfile);
                string content = sr.ReadToEnd();
                sr.Close();
                content += " ";
                StreamWriter sw = new StreamWriter(webconfigfile, false);
                sw.Write(content);
                sw.Close();

                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public static int CheckFolder(string foldername)
        {
            HttpContext context = HttpContext.Current;
            string physicsPath = context.Server.MapPath(@"..\" + foldername);
            try
            {
                using (FileStream fs = new FileStream(physicsPath + "\\a.txt", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    fs.Close();
                }
                if (File.Exists(physicsPath + "\\a.txt"))
                {
                    System.IO.File.Delete(physicsPath + "\\a.txt");
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }
        public static int CheckFile(string filename)
        {
            bool ex = File.Exists(filename);
            if (ex)
                return 1;
            else
                return 0;
        }
        public static void saveconfig(string sqlencrypt, string regconnstr)
        {
            //保存连接字符串
            XmlDocument webconfigDoc = new XmlDocument();
            string filePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\conn.config";
            string xPath = "/appSettings/add[@key='?']";
            webconfigDoc.Load(filePath);
            XmlNode regkey1 = webconfigDoc.SelectSingleNode(xPath.Replace("?", "ConStringEncrypt"));
            XmlNode regkey2 = webconfigDoc.SelectSingleNode(xPath.Replace("?", "ConnectionString"));
            XmlNode regkey3 = webconfigDoc.SelectSingleNode(xPath.Replace("?", "CompleteConfig"));
            string RegKeyString1 = regkey1.Attributes["value"].InnerText;
            string RegKeyString2 = regkey2.Attributes["value"].InnerText;
            string RegKeyString3 = regkey3.Attributes["value"].InnerText;

            regkey1.Attributes["value"].InnerText = sqlencrypt;
            regkey2.Attributes["value"].InnerText = regconnstr;
            regkey3.Attributes["value"].InnerText = "true";
            webconfigDoc.Save(filePath);
        }
        public int configed()
        {
            //string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            try
            {
                XmlDocument webconfigDoc = new XmlDocument();
                string filePath = HttpContext.Current.Request.PhysicalApplicationPath + @"\conn.config";
                string xPath = "/appSettings/add[@key='?']";
                webconfigDoc.Load(filePath);
                XmlNode regkey1 = webconfigDoc.SelectSingleNode(xPath.Replace("?", "CompleteConfig"));
                string RegKeyString1 = regkey1.Attributes["value"].InnerText;

                if (RegKeyString1 == "true")
                    return 1;
                else
                    return 0;

            }
            catch
            {
                return 0;
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}