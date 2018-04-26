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
    /// CRM_Follow 的摘要说明
    /// </summary>
    public class CRM_Follow : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.CRM_Follow follow = new BLL.CRM_Follow();
            Model.CRM_Follow model = new Model.CRM_Follow();

            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            string CoockiesID = ticket.UserData;

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(CoockiesID);
            DataSet dsemp = emp.GetList("id=" + emp_id);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "save")
            {
                model.Customer_id = int.Parse(request["cid"]);
                BLL.CRM_Customer ccc = new BLL.CRM_Customer();
                string cname = ccc.GetList("id=" + model.Customer_id).Tables[0].Rows[0]["Customer"].ToString();
                model.Customer_name = PageValidate.InputText(cname, 250);
                model.Follow = PageValidate.InputText(request["T_follow"], 4000);

                model.Follow_Type_id = int.Parse(request["T_followtype_val"]);
                model.Follow_Type = PageValidate.InputText(request["T_followtype"], 255);

                string fid = PageValidate.InputText( request["fid"],50);
                if (!string.IsNullOrEmpty(fid) && fid != "null")
                {
                    DataSet ds = follow.GetList("id=" + int.Parse(fid));
                    DataRow dr = ds.Tables[0].Rows[0];                   
                    model.id = int.Parse(fid);

                    follow.Update(model);

                    //最后跟进
                    ccc.UpdateLastFollow(model.Customer_id.ToString());

                    //日志
                    C_Sys_log log = new C_Sys_log();
                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.Customer_name;
                    string EventType = "客户跟进修改";
                    int EventID = model.id;

                    if (dr["Follow"].ToString() != request["T_follow"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "跟进内容", "跟进内容被修改", "跟进内容被修改");
                    }
                    if (dr["Follow_Type"].ToString() != request["T_followtype"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "跟进类型", dr["Follow_Type"].ToString(), request["T_followtype"]);
                    }
                }
                else
                {
                    model.isDelete = 0;
                    DataRow dr1 = dsemp.Tables[0].Rows[0];
                    model.employee_id = int.Parse(dr1["ID"].ToString());
                    model.employee_name = dr1["name"].ToString();
                    string depid = dr1["d_id"].ToString();
                    if (string.IsNullOrEmpty(depid))
                        depid = "0";
                    model.department_id = int.Parse(depid);
                    model.department_name = dr1["dname"].ToString();

                    model.Follow_date = DateTime.Now;

                    int customerid = follow.Add(model);

                    //最后跟进
                    ccc.UpdateLastFollow(model.Customer_id.ToString());

                }
                if (!string.IsNullOrEmpty(request["T_content"]))
                {
                    BLL.Personal_Calendar calendar = new BLL.Personal_Calendar();
                    Model.Personal_Calendar modelcalendar = new Model.Personal_Calendar();

                    int clientzone = 8;
                    int serverzone = GetTimeZone();
                    var zonediff = serverzone - clientzone;

                    modelcalendar.StartTime = DateTime.Parse(request["T_starttime"]).AddHours(zonediff);
                    modelcalendar.EndTime = DateTime.Parse(request["T_endtime"]).AddHours(zonediff);

                    modelcalendar.Subject = PageValidate.InputText("【" + cname + "】" + request["T_content"], 4000);

                    modelcalendar.emp_id = emp_id;
                    modelcalendar.UPAccount = emp_id.ToString();
                    modelcalendar.UPTime = DateTime.Now;
                    modelcalendar.MasterId = clientzone;
                    modelcalendar.CalendarType = 1;
                    modelcalendar.Category = "4";//跟进提醒
                    modelcalendar.companyid = model.Customer_id;
                    modelcalendar.InstanceType = 0;
                    modelcalendar.IsAllDayEvent = PageValidate.InputText(request["allday"], 255) == "True" ? true : false;

                    calendar.Add(modelcalendar);
                }
            }

            if (request["Action"] == "form")
            {
                string fid = PageValidate.InputText(request["fid"], 50);
                string dt;
                if (PageValidate.IsNumber(fid))
                {
                    DataSet ds = follow.GetList("id=" + fid + DataAuth(emp_id.ToString()));
                    dt = Common.DataToJson.DataToJSON(ds);
                }
                else
                {
                    dt = "{}";
                }

                context.Response.Write(dt);
            }
            //del
            if (request["Action"] == "del")
            {
                string id = request["id"];

                DataSet ds = follow.GetList("id=" + int.Parse(id));

                bool canedel = true;
                if (uid != "admin")
                {
                    Data.GetDataAuth dataauth = new Data.GetDataAuth();
                    string txt = dataauth.GetDataAuthByid("2", "Sys_del", emp_id.ToString());

                    string[] arr = txt.Split(':');
                    switch (arr[0])
                    {
                        case "none":
                            canedel = false;
                            break;
                        case "my":
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["employee_id"].ToString() == arr[1])
                                    canedel = true;
                                else
                                    canedel = false;
                            }
                            break;
                        case "dep":
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["dep_id"].ToString() == arr[1])
                                    canedel = true;
                                else
                                    canedel = false;
                            }
                            break;
                        case "all":
                            canedel = true;
                            break;
                    }
                }
                if (canedel)
                {
                    bool isdel = follow.Delete(int.Parse(request["id"]));
                    //context.Response.Write("{success:success}");
                    if (isdel)
                    {
                        //日志
                        string EventType = "跟进删除";

                        int UserID = emp_id;
                        string UserName = empname;
                        string IPStreet = request.UserHostAddress;
                        int EventID = int.Parse(id);
                        string EventTitle = ds.Tables[0].Rows[0]["Customer_name"].ToString();
                        string Original_txt = ds.Tables[0].Rows[0]["Follow"].ToString();
                        string Current_txt = null;

                        C_Sys_log log = new C_Sys_log();

                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "跟进", Original_txt, Current_txt);

                        context.Response.Write("true");
                    }
                    else
                    {
                        context.Response.Write("false");
                    }
                }
                else
                {
                    context.Response.Write("delfalse");
                }
            }
           
            //serch
            if (request["Action"] == "grid")
            {
                int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                string sortname = request["sortname"];
                string sortorder = request["sortorder"];

                if (string.IsNullOrEmpty(sortname))
                    sortname = " id ";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = " desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total;

                 string serchtxt = "1=1";

                if (!string.IsNullOrEmpty(request["customer_id"]))
                    serchtxt += " and Customer_id=" + int.Parse(request["customer_id"]);

                if (!string.IsNullOrEmpty(request["company"]))
                    serchtxt += " and Customer_name like N'%" + PageValidate.InputText(request["company"], 255) + "%'";

                if (!string.IsNullOrEmpty(request["department"]))
                    serchtxt += " and department_id = " + int.Parse(request["department_val"]);

                if (!string.IsNullOrEmpty(request["employee"]))
                    serchtxt += " and employee_id = " + int.Parse(request["employee_val"]);

                if (!string.IsNullOrEmpty(request["followtype"]))
                    serchtxt += " and Follow_Type_id = " + int.Parse(request["followtype_val"]);

                if (!string.IsNullOrEmpty(request["startdate"]))
                    serchtxt += " and Follow_date >= '" + PageValidate.InputText(request["startdate"], 255) + "'";

                if (!string.IsNullOrEmpty(request["enddate"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and Follow_date  <= '" + enddate + "'";
                }

                if (!string.IsNullOrEmpty(request["startdate_del"]))
                    serchtxt += " and Delete_time >= '" + PageValidate.InputText(request["startdate_del"], 255) + "'";

                if (!string.IsNullOrEmpty(request["enddate_del"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate_del"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and Delete_time  <= '" + enddate + "'";
                }
                if (!string.IsNullOrEmpty(request["T_smart"]))
                {
                    if (request["T_smart"] != "输入关键词智能搜索跟进内容")
                        serchtxt += " and Follow like N'%" + PageValidate.InputText(request["T_smart"], 255) + "%'";
                }
                //权限
                serchtxt += DataAuth(emp_id.ToString());

                DataSet ds = follow.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

                string dt = Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
                context.Response.Write(dt);
            }

            if (request["Action"] == "Compared_follow")
            {
                string year1 = PageValidate.InputText(request["year1"], 50);
                string year2 = PageValidate.InputText(request["year2"], 50);
                string month1 = PageValidate.InputText(request["month1"], 50);
                string month2 = PageValidate.InputText(request["month2"], 50);

                DataSet ds = follow.Compared_follow(year1,month1,year2,month2);

                string dt = GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }

            if (request["Action"] == "Compared_empcusfollow")
            {
                var idlist = PageValidate.InputText(request["idlist"].Replace(";", ",").Replace("-", "").Replace("p",""), int.MaxValue);
                string year1 = PageValidate.InputText(request["year1"], 50);
                string year2 = PageValidate.InputText(request["year2"], 50);
                string month1 = PageValidate.InputText(request["month1"], 50);
                string month2 = PageValidate.InputText(request["month2"], 50);

                if (idlist.Length < 1)
                    idlist = "0";

                BLL.hr_post post = new BLL.hr_post();
                DataSet dspost = post.GetList("post_id in(" + idlist + ")");

                string emplist = "(";

                for (int i = 0; i < dspost.Tables[0].Rows.Count; i++)
                {
                    emplist += dspost.Tables[0].Rows[i]["emp_id"] + ",";
                }
                emplist += "0)";

                //context.Response.Write(emplist);

                DataSet ds = follow.Compared_empcusfollow(year1,month1,year2,month2, emplist);

                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }

            if (request["Action"] == "emp_cusfollow")
            {
                var idlist = PageValidate.InputText(request["idlist"].Replace(";", ",").Replace("-", "").Replace("p",""), int.MaxValue);
                var syear = request["syear"];

                BLL.hr_post post = new BLL.hr_post();
                DataSet dspost = post.GetList("post_id in(" + idlist + ")");

                string emplist = "(";

                for (int i = 0; i < dspost.Tables[0].Rows.Count - 1; i++)
                {
                    emplist += dspost.Tables[0].Rows[i]["emp_id"] + ",";
                }
                emplist += dspost.Tables[0].Rows[dspost.Tables[0].Rows.Count - 1]["emp_id"] + ")";

                //context.Response.Write(emplist);

                DataSet ds = follow.report_empfollow(int.Parse(syear), emplist);

                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }


        }

        private string DataAuth(string uid)
        {
            //权限
            BLL.hr_employee emp = new BLL.hr_employee();
            DataSet dsemp = emp.GetList("ID=" + int.Parse(uid));

            string returntxt = " and 1=1 ";
            if (dsemp.Tables[0].Rows.Count > 0)
            {
                if (dsemp.Tables[0].Rows[0]["uid"].ToString() != "admin")
                {
                    Data.GetDataAuth dataauth = new Data.GetDataAuth();
                    string txt = dataauth.GetDataAuthByid("2", "Sys_view", uid);

                    string[] arr = txt.Split(':');
                    switch (arr[0])
                    {
                        case "none":
                            returntxt = " and 1=2 ";
                            break;
                        case "my":
                            returntxt = " and  Employee_id=" + arr[1];
                            break;
                        case "dep":
                            if (string.IsNullOrEmpty(arr[1]))
                                returntxt = " and  Employee_id=" + int.Parse(uid);
                            else
                                returntxt = " and  Department_id=" + arr[1];
                            break;
                        case "depall":
                            BLL.hr_department dep = new BLL.hr_department();
                            DataSet ds = dep.GetAllList();
                            string deptask = GetDepTask(int.Parse(arr[1]), ds.Tables[0]);
                            string intext = arr[1] + "," + deptask;
                            returntxt = " and  Department_id in (" + intext.TrimEnd(',') + ")";
                            break;
                    }
                }
            }
            return returntxt;
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
        private static int GetTimeZone()
        {
            DateTime now = DateTime.Now;
            var utcnow = now.ToUniversalTime();

            var sp = now - utcnow;

            return sp.Hours;
        }
        private static long MilliTimeStamp(DateTime theDate)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = theDate.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return (long)ts.TotalMilliseconds;
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