using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using KfCrm.Common;
using System.Web.Security;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// CRM_contract 的摘要说明
    /// </summary>
    public class CRM_contract : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.CRM_contract cc = new BLL.CRM_contract();
            Model.CRM_contract model = new Model.CRM_contract();

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
                DataRow dremp = dsemp.Tables[0].Rows[0];

                model.Serialnumber = PageValidate.InputText(request["T_contract_num"], 255);
                model.Contract_name = PageValidate.InputText(request["T_contract_name"], 255);
                model.Customer_id = int.Parse(request["T_Customer_val"]);
                model.Customer_name = PageValidate.InputText(request["T_Customer"], 255);

                model.C_depid = int.Parse(request["c_dep_val"].ToString());
                model.C_depname = PageValidate.InputText(request["c_dep"].ToString(), 255);
                model.C_empid = int.Parse(request["c_emp_val"].ToString());
                model.C_empname = PageValidate.InputText(request["c_emp"].ToString(), 255);

                model.Contract_amount = decimal.Parse(request["T_contract_amount"]);
                model.Pay_cycle = int.Parse(request["T_pay_cycle"]);

                model.Start_date = PageValidate.InputText(request["T_start_date"].ToString(), 255);
                model.End_date = PageValidate.InputText(request["T_end_date"].ToString(), 255);
                model.Sign_date = PageValidate.InputText(request["T_contract_date"].ToString(), 255);
                model.Customer_Contractor = PageValidate.InputText(request["T_contractor"].ToString(), 255);
                model.Our_Contractor_depid = int.Parse(request["f_dep_val"].ToString());
                model.Our_Contractor_depname = PageValidate.InputText(request["f_dep"], 255);
                model.Our_Contractor_id = int.Parse(request["f_emp_val"].ToString());
                model.Our_Contractor_name = PageValidate.InputText(request["f_emp"].ToString(), 255);

                model.Main_Content = PageValidate.InputText(request["T_content"].ToString(), int.MaxValue);
                model.Remarks = PageValidate.InputText(request["T_remarks"].ToString(), int.MaxValue);

                string cid = PageValidate.InputText( request["cid"],50);
                int contract_id = -1;
                if (!string.IsNullOrEmpty(cid) && cid != "null")
                {
                    contract_id = int.Parse(cid);
                    model.id = contract_id;

                    DataSet ds = cc.GetList(" id=" + model.id);
                    DataRow dr = ds.Tables[0].Rows[0]; 

                    cc.Update(model);

                    C_Sys_log log = new C_Sys_log();
                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.Contract_name;
                    string EventType = "合同修改";
                    int EventID = model.id;

                    if (dr["Customer_name"].ToString() != request["T_Customer"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "客户", dr["Customer_name"].ToString(), request["T_Customer"]);
                    
                    if (dr["Contract_name"].ToString() != request["T_contract_name"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "合同名称", dr["Contract_name"].ToString(), request["T_contract_name"]);
                    
                    if (dr["Serialnumber"].ToString() != request["T_contract_num"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "合同编号", dr["Serialnumber"].ToString(), request["T_contract_num"]);
                    
                    if (dr["Contract_amount"].ToString() != request["T_contract_amount"].Replace(",", "").Replace(".00", ""))                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "合同金额", dr["Contract_amount"].ToString(), request["T_contract_amount"].Replace(",", "").Replace(".00", ""));
                    
                    if (dr["Customer_Contractor"].ToString() != request["T_contractor"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "对方签约人", dr["Customer_Contractor"].ToString(), request["T_contractor"]);
                    
                    if (dr["Our_Contractor_depname"].ToString() != request["f_dep"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "我方签约人部门", dr["Our_Contractor_depname"].ToString(), request["f_dep"]);
                    
                    if (dr["Our_Contractor_name"].ToString() != request["f_emp"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "我方签约人名字", dr["Our_Contractor_name"].ToString(), request["f_emp"]);
                    
                    if (dr["Main_Content"].ToString() != request["T_content"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "主要条款", "原内容被修改", "原内容被修改");
                    
                    if (dr["Remarks"].ToString() != request["T_remarks"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "备注", "原内容被修改", "原内容被修改");
                    
                    if (dr["Start_date"].ToString() != request["T_start_date"].ToString())                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "开始时间", dr["Start_date"].ToString(), request["T_start_date"].ToString());
                    
                    if (dr["End_date"].ToString() != request["T_end_date"].ToString())                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "结束时间", dr["End_date"].ToString(), request["T_end_date"].ToString());
                    
                    if (dr["Sign_date"].ToString() != request["T_contract_date"].ToString())                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "签约时间", dr["Sign_date"].ToString(), request["T_contract_date"].ToString());                    
                }
                else
                {
                    model.isDelete = 0;
                    model.Creater_id = emp_id;
                    model.Creater_name = dremp["name"].ToString();
                    model.Create_time = DateTime.Now;

                    contract_id = cc.Add(model);
                }

                //attachment
                BLL.CRM_contract_attachment cca = new BLL.CRM_contract_attachment();
                string page_id = PageValidate.InputText(request["page_id"], 255);
                cca.UpdateMailid(contract_id, page_id);
            }

            if (request["Action"] == "grid")
            {
                int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                string sortname = request["sortname"];
                string sortorder = request["sortorder"];

                if (string.IsNullOrEmpty(sortname))
                    sortname = " id";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = "desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total;
                string serchtxt = "1=1";

                string customer_id = request["cid"];
                if (!string.IsNullOrEmpty(customer_id) && customer_id != "null")
                    serchtxt += " and Customer_id=" + int.Parse(customer_id);

                if (!string.IsNullOrEmpty(request["company"]))
                    serchtxt += " and Customer_name like N'%" + PageValidate.InputText(request["company"], 255) + "%'";

                if (!string.IsNullOrEmpty(request["contact"]))
                    serchtxt += " and Contract_name like N'%" + PageValidate.InputText(request["contact"], 255) + "%'";

                if (!string.IsNullOrEmpty(request["department"]))
                    serchtxt += " and C_depid =" + int.Parse(request["department_val"]);

                if (!string.IsNullOrEmpty(request["employee"]))
                    serchtxt += " and C_empid =" + int.Parse(request["employee_val"]);

                if (!string.IsNullOrEmpty(request["startdate"]))
                    serchtxt += " and Create_time >= '" + PageValidate.InputText(request["startdate"], 255) + "'";

                if (!string.IsNullOrEmpty(request["enddate"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and Create_time  <= '" + enddate + "'";
                }

                if (!string.IsNullOrEmpty(request["startdate_del"]))
                    serchtxt += " and Delete_time >= '" + PageValidate.InputText(request["startdate_del"], 255) + "'";

                if (!string.IsNullOrEmpty(request["enddate_del"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate_del"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and Delete_time  <= '" + enddate + "'";
                }
                //权限 
                serchtxt += DataAuth(emp_id.ToString());

                DataSet ds = cc.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

                context.Response.Write(Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total));
            }


            if (request["Action"] == "form")
            {
                string contract_id = request["cid"];
                DataSet ds = cc.GetList("id=" + int.Parse(contract_id) + DataAuth(emp_id.ToString()));
                string dt = Common.DataToJson.DataToJSON(ds);
                context.Response.Write(dt);
            }
            //del
            if (request["Action"] == "del")
            {
                string c_id = PageValidate.InputText( request["id"],50);
                DataSet ds = cc.GetList("id=" + int.Parse(c_id));

                bool canedel = true;
                if (uid != "admin")
                {
                    Data.GetDataAuth dataauth = new Data.GetDataAuth();
                    string txt = dataauth.GetDataAuthByid("4", "Sys_del", emp_id.ToString());

                    string[] arr = txt.Split(':');
                    switch (arr[0])
                    {
                        case "none":
                            canedel = false;
                            break;
                        case "my":
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["C_empid"].ToString() == arr[1])
                                    canedel = true;
                                else
                                    canedel = false;
                            }
                            break;
                        case "dep":
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (ds.Tables[0].Rows[i]["C_depid"].ToString() == arr[1])
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
                    bool isdel = cc.Delete(int.Parse(c_id));
                    BLL.CRM_contract_attachment atta = new BLL.CRM_contract_attachment();
                    atta.Delete("contract_id=" + int.Parse(c_id));
                    if (isdel)
                    {
                        //日志
                        string EventType = "合同删除";

                        int UserID = emp_id;
                        string UserName = empname;
                        string IPStreet = request.UserHostAddress;
                        int EventID = int.Parse(c_id);
                        string EventTitle = ds.Tables[0].Rows[0]["Contract_name"].ToString();
                        string Original_txt = null;
                        string Current_txt = null;

                        C_Sys_log log = new C_Sys_log();

                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null, Original_txt, Current_txt);

                        context.Response.Write("true");
                    }
                    else
                    {
                        context.Response.Write("false");
                    }
                }
            }

            if (request["Action"] == "Compared_empcuscontract")
            {
                var idlist = PageValidate.InputText(request["idlist"].Replace(";", ",").Replace("-", "").Replace("p",""), int.MaxValue);
                string year1 = PageValidate.InputText(request["year1"], 50);
                string year2 = PageValidate.InputText(request["year2"], 50);
                string month1 = PageValidate.InputText(request["month1"], 50);
                string month2 = PageValidate.InputText(request["month2"], 50);

                BLL.hr_post post = new BLL.hr_post();
                DataSet dspost = post.GetList("post_id in(" + idlist + ")");

                string emplist = "(";

                for (int i = 0; i < dspost.Tables[0].Rows.Count - 1; i++)
                {
                    emplist += dspost.Tables[0].Rows[i]["emp_id"] + ",";
                }
                emplist += dspost.Tables[0].Rows[dspost.Tables[0].Rows.Count - 1]["emp_id"] + ")";

                //context.Response.Write(emplist);

                DataSet ds = cc.Compared_empcuscontract(year1,month1,year2,month2, emplist);

                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }

            if (request["Action"] == "emp_cuscontract")
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

                DataSet ds = cc.report_empcontract(int.Parse(syear), emplist);

                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }


        }


        private string DataAuth(string uid)
        {
            //权限
            BLL.hr_employee emp = new BLL.hr_employee();
            DataSet dsemp = emp.GetList("ID=" + int.Parse(uid));

            string returntxt = " and 1=1";
            if (dsemp.Tables[0].Rows.Count > 0)
            {
                if (dsemp.Tables[0].Rows[0]["uid"].ToString() != "admin")
                {
                    Data.GetDataAuth dataauth = new Data.GetDataAuth();
                    string txt = dataauth.GetDataAuthByid("4", "Sys_view", uid);

                    string[] arr = txt.Split(':');
                    switch (arr[0])
                    {
                        case "none":
                            returntxt = " and 1=2 ";
                            break;
                        case "my":
                            returntxt = " and  C_empid=" + arr[1];
                            break;
                        case "dep":
                            if (string.IsNullOrEmpty(arr[1]))
                                returntxt = " and  C_empid=" + int.Parse(uid);
                            else
                                returntxt = " and  C_depid=" + arr[1];
                            break;
                        case "depall":
                            BLL.hr_department dep = new BLL.hr_department();
                            DataSet ds = dep.GetAllList();
                            string deptask = GetDepTask(int.Parse(arr[1]), ds.Tables[0]);
                            string intext = arr[1] + "," + deptask;
                            returntxt = " and  C_depid in (" + intext.TrimEnd(',') + ")";
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}