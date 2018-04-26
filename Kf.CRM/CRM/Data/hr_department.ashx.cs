using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using KfCrm.Common;
using System.Web.Security;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// hr_department 的摘要说明
    /// </summary>
    public class hr_department : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.hr_department dep = new BLL.hr_department();
            Model.hr_department model = new Model.hr_department();

            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            string CoockiesID = ticket.UserData;

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(CoockiesID);
            DataSet dsemp = emp.GetList("id=" + emp_id);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "department")
            {
                string did = PageValidate.InputText(request["did"], 50);
                if (!string.IsNullOrEmpty(did))
                {
                    DataSet ds;
                    if (did == "-2")
                    {
                        ds = dep.GetList(" ISNULL(isDelete,0)=0 and parentid=0 ");
                    }
                    else
                    {
                        ds = dep.GetList(" id=" + int.Parse(did));
                    }

                    string outstring = did + ",";
                    outstring += ds.Tables[0].Rows[0]["d_name"].ToString() + ",";
                    outstring += ds.Tables[0].Rows[0]["d_fuzeren"].ToString() + ",";
                    outstring += ds.Tables[0].Rows[0]["d_tel"].ToString() + ",";
                    outstring += ds.Tables[0].Rows[0]["d_fax"].ToString() + ",";
                    outstring += ds.Tables[0].Rows[0]["d_add"].ToString() + ",";
                    outstring += ds.Tables[0].Rows[0]["d_email"].ToString() + ",";
                    outstring += ds.Tables[0].Rows[0]["d_miaoshu"].ToString() + ",";
                    outstring += ds.Tables[0].Rows[0]["d_order"].ToString();

                    context.Response.Write(outstring);
                }
            }
            if (request["Action"] == "deptree")
            {
                DataSet ds = dep.GetAllList();
                StringBuilder str = new StringBuilder();
                str.Append("[");
                str.Append("{id:0,text:'无',d_icon:''},");
                str.Append(GetTreeString(0, ds.Tables[0], null));
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
                context.Response.Write(str);
            }
            if (request["Action"] == "treegrid")
            {
                DataSet ds = dep.GetAllList();
                string dt = "{Rows:[" + GetTasksString(0, ds.Tables[0]) + "]}";
                context.Response.Write(dt);
            }
            if (request["Action"] == "tree")
            {
                string serchtxt = " 1=1 ";

                string authtxt = PageValidate.InputText(request["auth"], 50);
                if (!string.IsNullOrEmpty(authtxt))
                {
                    Data.GetDataAuth dataauth = new Data.GetDataAuth();
                    string txt = dataauth.GetDataAuthByid(authtxt, "Sys_add", emp_id.ToString());
                    string[] arr = txt.Split(':');
                    switch (arr[0])
                    {
                        case "my":
                        case "dep":
                            string did = dsemp.Tables[0].Rows[0]["d_id"].ToString();
                            if (string.IsNullOrEmpty(did))
                                did = "0";
                            authtxt = did;
                            break;
                        case "all":
                            authtxt = "0";
                            break;
                        case "depall":
                            DataSet dsdep = dep.GetAllList();
                            string deptask = GetDepTask(int.Parse(arr[1]), dsdep.Tables[0]);
                            string intext = arr[1] + "," + deptask;
                            authtxt = intext.TrimEnd(',');
                            break;
                    }
                }
                //context.Response.Write(authtxt);
                DataSet ds = dep.GetList(0, serchtxt, " d_order");
                StringBuilder str = new StringBuilder();
                str.Append("[");
                str.Append(GetTreeString(0, ds.Tables[0], authtxt));
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
                context.Response.Write(str);
            }
            //Form JSON
            if (request["Action"] == "form")
            {
                int depid = int.Parse(request["id"]);
                DataSet ds = dep.GetList("id=" + depid);

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }
            //save
            if (request["Action"] == "save")
            {
                string parentid;//= string.IsNullOrEmpty(request["T_parent"]) ? "0" : request["T_parentid"];
                if (string.IsNullOrEmpty(request["T_parent_val"]) || request["T_parent_val"] == "null")
                {
                    parentid = "0";
                }
                else
                {
                    parentid = request["T_parent_val"];
                }

                model.d_name = PageValidate.InputText(request["T_depname"], 255);
                model.parentid = int.Parse(parentid);
                model.parentname = PageValidate.InputText(request["T_parent"], 250);
                model.d_type = PageValidate.InputText(request["T_deptype"], 250);
                model.d_order = int.Parse(request["T_sort"]);
                model.d_fuzeren = PageValidate.InputText(request["T_leader"], 255);
                model.d_tel = PageValidate.InputText(request["T_tel"], 255);
                model.d_email = PageValidate.InputText(request["T_email"], 255);
                model.d_fax = PageValidate.InputText(request["T_fax"], 255);
                model.d_add = PageValidate.InputText(request["T_add"], 255);
                model.d_miaoshu = PageValidate.InputText(request["T_descript"], 255);

                if (model.d_type == "部门")
                    model.d_icon = "images/icon/88.png";
                else
                    model.d_icon = "images/icon/61.png";

                string id = request["id"];

                if (!string.IsNullOrEmpty(id) && id != "null")
                {
                    model.id = int.Parse(id);
                    DataSet ds = dep.GetList("id=" + int.Parse(id));
                    DataRow dr = ds.Tables[0].Rows[0];
                    dep.Update(model);

                    //日志
                    C_Sys_log log = new C_Sys_log();

                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.d_name;
                    string EventType = "组织架构修改";
                    int EventID = model.id;

                    if (dr["d_name"].ToString() != request["T_depname"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "机构名称", dr["d_name"].ToString(), request["T_depname"]);

                    if (dr["parentname"].ToString() != request["T_parent"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "上级机构", dr["parentname"].ToString(), request["T_parent"]);

                    if (dr["d_type"].ToString() != request["T_deptype"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "机构类型", dr["d_type"].ToString(), request["T_deptype"]);

                    if (dr["d_order"].ToString() != request["T_sort"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "部门排序", dr["d_order"].ToString(), request["T_sort"]);

                    if (dr["d_fuzeren"].ToString() != request["T_leader"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "负责人", dr["d_fuzeren"].ToString(), request["T_leader"]);

                    if (dr["d_tel"].ToString() != request["T_tel"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "电话", dr["d_tel"].ToString(), request["T_tel"]);

                    if (dr["d_email"].ToString() != request["T_email"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "邮箱", dr["d_email"].ToString(), request["T_email"]);

                    if (dr["d_fax"].ToString() != request["T_fax"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "传真", dr["d_fax"].ToString(), request["T_fax"]);

                    if (dr["d_add"].ToString() != request["T_add"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "地址", dr["d_add"].ToString(), request["T_add"]);

                    if (dr["d_miaoshu"].ToString() != request["T_descript"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "描述", dr["d_miaoshu"].ToString(), request["T_descript"]);
                }
                else
                {
                    model.isDelete = 0;
                    dep.Add(model);
                }
            }
            //del
            if (request.Params["Action"] == "del")
            {
                int d_id = int.Parse(request["id"]);

                string EventType = "组织架构删除";

                DataSet ds = emp.GetList("d_id = " + d_id);

                BLL.hr_post post = new BLL.hr_post();
                if (d_id == 1)
                {
                    //根目录不能删除
                    context.Response.Write("false:first");
                }
                else if (post.GetList("dep_id=" + d_id).Tables[0].Rows.Count > 0)
                {
                    //含有岗位信息不能删除
                    context.Response.Write("false:post");
                }
                else if (emp.GetList("d_id=" + d_id).Tables[0].Rows.Count > 0)
                {
                    //含有员工信息不能删除
                    context.Response.Write("false:emp");
                }
                else
                {
                    bool isdel = dep.Delete(d_id);
                    if (isdel)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int UserID = emp_id;
                            string UserName = empname;
                            string IPStreet = request.UserHostAddress;
                            int EventID = d_id;
                            string EventTitle = ds.Tables[0].Rows[i]["d_name"].ToString();
                            string Original_txt = null;
                            string Current_txt = null;
                            C_Sys_log log = new C_Sys_log();
                            log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null, Original_txt, Current_txt);
                        }
                        context.Response.Write("true");
                    }
                    else
                    {
                        context.Response.Write("false");
                    }
                }
            }
        }
        private static string GetDepTask(int Id, DataTable table)
        {
            DataRow[] rows = table.Select("parentid=" + Id.ToString());

            if (rows.Length == 0) return string.Empty; ;
            StringBuilder str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                str.Append(row["id"] + ",");
                if (GetDepTask((int)row["id"], table).Length > 0)
                    str.Append(GetDepTask((int)row["id"], table));
            }
            return str.ToString();
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
                    str.Append(row.Table.Columns[i].ColumnName);
                    str.Append(":'");
                    str.Append(row[i].ToString());
                    str.Append("'");
                }
                if (GetTasksString((int)row["id"], table).Length > 0)
                {
                    str.Append(",children:[");
                    str.Append(GetTasksString((int)row["id"], table));
                    str.Append("]},");
                }
                else
                {
                    str.Append("},");
                }
            }
            return str[str.Length - 1] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
        }
        private static string GetTreeString(int Id, DataTable table, string authtxt)
        {
            DataRow[] rows = table.Select(string.Format("parentid={0}", Id));

            if (rows.Length == 0) return string.Empty; ;
            StringBuilder str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                string d_icon = "../" + (string)row["d_icon"];

                if (!string.IsNullOrEmpty(authtxt) && authtxt.IndexOf(row["id"].ToString()) == -1 && authtxt!="0")
                    d_icon = "../images/icon/50.png";

                str.Append("{id:" + (int)row["id"] + ",text:'" + row["d_name"].ToString() + "',d_icon:'" + d_icon + "'");

                if (GetTreeString((int)row["id"], table, authtxt).Length > 0)
                {
                    str.Append(",children:[");
                    str.Append(GetTreeString((int)row["id"], table, authtxt));
                    str.Append("]},");
                }
                else
                {
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