using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.IO;
using KfCrm.Common;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// _base 的摘要说明
    /// </summary>
    public class _base : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.Sys_Menu menu = new BLL.Sys_Menu();

            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                string CoockiesID = ticket.UserData;

                BLL.hr_employee emp = new BLL.hr_employee();
                int emp_id = int.Parse(CoockiesID);
                DataSet dsemp = emp.GetList("id=" + emp_id);
                string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
                string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

                if (request["Action"] == "GetSysApp")
                {
                    DataSet ds = null;

                    int appid = int.Parse(request["appid"]);

                    if (dsemp.Tables[0].Rows.Count > 0)
                    {
                        if (dsemp.Tables[0].Rows[0]["uid"].ToString() == "admin")
                        {
                            ds = menu.GetList(0, "App_id=" + appid, "Menu_order");
                        }
                        else
                        {
                            Data.GetAuthorityByUid getauth = new Data.GetAuthorityByUid();
                            string menus = getauth.GetAuthority(emp_id.ToString(), "Menus");
                            ds = menu.GetList(0, "App_id=" + appid + " and Menu_id in " + menus, "Menu_order");
                        }
                    }

                    string dt = "[" + GetTasksString(0, ds.Tables[0]) + "]";

                    context.Response.Write(dt);
                }
                if (request["Action"] == "getUserTree")
                {
                    BLL.Sys_online sol = new BLL.Sys_online();
                    Model.Sys_online model = new Model.Sys_online();

                    model.UserName = PageValidate.InputText(empname, 250);
                    model.UserID = emp_id;
                    model.LastLogTime = DateTime.Now;

                    DataSet ds1 = sol.GetList(" UserID=" + emp_id);

                    //添加当前用户信息
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        sol.Update(model, " UserID=" + emp_id);
                    }
                    else
                    {
                        sol.Add(model);
                    }

                    //删除超时用户
                    sol.Delete(" LastLogTime<DATEADD(MI,-2,getdate())");

                    BLL.hr_department dep = new BLL.hr_department();
                    BLL.hr_post hp = new BLL.hr_post();

                    DataSet ds = dep.GetList(0, "", "d_order");
                    StringBuilder str = new StringBuilder();
                    str.Append("[");
                    str.Append(GetTreeString(0, ds.Tables[0], 1));
                    str.Replace(",", "", str.Length - 1, 1);
                    str.Append("]");
                    context.Response.Write(str);

                }
                if (request["Action"] == "GetUserInfo")
                {
                    string dt = Common.DataToJson.DataToJSON(dsemp);

                    context.Response.Write(dt);

                }
                if (request["Action"] == "GetOnline")
                {
                    BLL.Sys_online sol = new BLL.Sys_online();
                    Model.Sys_online model = new Model.Sys_online();


                    model.UserName = empname;
                    model.UserID = emp_id;
                    model.LastLogTime = DateTime.Now;

                    DataSet ds1 = sol.GetList(" UserID=" + emp_id);

                    //添加当前用户信息
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        sol.Update(model, " UserID=" + emp_id);
                    }
                    else
                    {
                        sol.Add(model);
                    }
                    //}

                    //删除超时用户
                    sol.Delete(" LastLogTime<DATEADD(MI,-2,getdate())");

                    context.Response.Write(Common.GetGridJSON.DataTableToJSON(sol.GetAllList().Tables[0]));
                }
            }
            if (request["Action"] == "GetIcons")
            {
                try
                {
                    var icontype = request["icontype"];

                    var rootPath = context.Server.MapPath("~/images/icon/");
                    Common.ObjectListToJSON objtojson = new Common.ObjectListToJSON();
                    List<FileInfo> lp = GetAllFilesInDirectory(rootPath);
                    string a = objtojson.toJSON(lp);
                    context.Response.Write(a);

                }
                catch (Exception err)
                {
                    context.Response.Write("系统错误:" + err.Message);
                }
            }
        }
        public List<FileInfo> GetAllFilesInDirectory(string strDirectory)
        {
            List<FileInfo> listFiles = new List<FileInfo>();
            DirectoryInfo directory = new DirectoryInfo(strDirectory);
            DirectoryInfo[] directoryArray = directory.GetDirectories();
            FileInfo[] fileInfoArray = directory.GetFiles();
            if (fileInfoArray.Length > 0) listFiles.AddRange(fileInfoArray);
            foreach (DirectoryInfo _directoryInfo in directoryArray)
            {
                DirectoryInfo directoryA = new DirectoryInfo(_directoryInfo.FullName);
                DirectoryInfo[] directoryArrayA = directoryA.GetDirectories();
                FileInfo[] fileInfoArrayA = directoryA.GetFiles();
                if (fileInfoArrayA.Length > 0) listFiles.AddRange(fileInfoArrayA);
                GetAllFilesInDirectory(_directoryInfo.FullName);//递归遍历  
            }
            return listFiles;
        }

        private static string GetTasksString(int Id, DataTable table)
        {
            DataRow[] rows = table.Select("parentid=" + Id.ToString());

            if (rows.Length == 0) return string.Empty; ;
            StringBuilder str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                str.Append("{");
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    if (i != 0) str.Append(",");
                    str.Append("\"");
                    str.Append(row.Table.Columns[i].ColumnName);
                    str.Append("\":\"");
                    str.Append(row[i].ToString());
                    str.Append("\"");
                }
                if (GetTasksString((int)row["Menu_id"], table).Length > 0)
                {
                    str.Append(",\"children\":[");
                    str.Append(GetTasksString((int)row["Menu_id"], table));
                    str.Append("]},");
                }
                else
                {
                    str.Append("},");
                }
            }
            return str[str.Length - 1] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
        }
        private static string GetTreeString(int Id, DataTable table, int todo)
        {
            BLL.hr_post hp = new BLL.hr_post();
            BLL.Sys_online sol = new BLL.Sys_online();
            DataRow[] rows = table.Select(string.Format("parentid={0}", Id));

            if (rows.Length == 0) return string.Empty; ;
            StringBuilder str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                str.Append("{id:" + (int)row["id"] + ",text:'" + (string)row["d_name"] + "',d_icon:'../" + (string)row["d_icon"] + "'");

                if (GetTreeString((int)row["id"], table, 0).Length > 0)
                {
                    str.Append(",children:[");
                    if (todo == 1)
                    {
                        DataSet dsp = hp.GetList("dep_id=" + (int)row["id"]);
                        if (dsp.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsp.Tables[0].Rows.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(dsp.Tables[0].Rows[j]["emp_name"].ToString()))
                                {
                                    DataSet dso = sol.GetList("UserID=" + dsp.Tables[0].Rows[j]["emp_id"]);
                                    string posticon = "images/icon/93.png";
                                    if (dso.Tables[0].Rows.Count > 0)
                                        posticon = "images/icon/38.png";//95

                                    str.Append("{id:'p" + dsp.Tables[0].Rows[j]["post_id"].ToString() + "',text:'" + dsp.Tables[0].Rows[j]["emp_name"] + "',d_icon:'" + posticon + "'}");
                                    str.Append(",");
                                }
                            }
                        }
                    }
                    str.Append(GetTreeString((int)row["id"], table, 1));
                    str.Append("]},");
                }
                else
                {
                    if (todo == 1)
                    {
                        DataSet dsp = hp.GetList("dep_id=" + (int)row["id"]);
                        if (dsp.Tables[0].Rows.Count > 0)
                        {
                            str.Append(",children:[");
                            for (int j = 0; j < dsp.Tables[0].Rows.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(dsp.Tables[0].Rows[j]["emp_name"].ToString()))
                                {
                                    DataSet dso = sol.GetList("UserID=" + dsp.Tables[0].Rows[j]["emp_id"]);
                                    string posticon = "images/icon/93.png";
                                    if (dso.Tables[0].Rows.Count > 0)
                                        posticon = "images/icon/38.png";//95

                                    str.Append("{id:'p" + dsp.Tables[0].Rows[j]["post_id"].ToString() + "',text:'" + dsp.Tables[0].Rows[j]["emp_name"] + "',d_icon:'" + posticon + "'},");
                                    //if (j < dsp.Tables[0].Rows.Count - 1)
                                    //    str.Append(",");
                                }
                            }
                            if (str[str.Length - 1] == ',')
                                str.Remove(str.Length - 1, 1);
                            str.Append("]");
                        }
                    }
                    str.Append("},");
                }
            }
            return str[str.Length - 1] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
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