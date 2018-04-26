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
    /// CRM_receive 的摘要说明
    /// </summary>
    public class CRM_receive : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.CRM_receive cci = new BLL.CRM_receive();
            Model.CRM_receive model = new Model.CRM_receive();

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

                model.Receive_num = PageValidate.InputText(request["T_invoice_num"], 255);

                string orderid = PageValidate.InputText( request["orderid"],50);

                BLL.CRM_order order = new BLL.CRM_order();
                DataSet dsorder = order.GetList("id=" + int.Parse( orderid));

                model.order_id = int.Parse(orderid);
                if (dsorder.Tables[0].Rows.Count > 0)
                {
                    model.Customer_id = int.Parse(dsorder.Tables[0].Rows[0]["Customer_id"].ToString());
                    model.Customer_name = PageValidate.InputText(dsorder.Tables[0].Rows[0]["Customer_name"].ToString(), 255);
                }

                model.C_depid = int.Parse(request["T_dep_val"].ToString());
                model.C_depname = PageValidate.InputText(request["T_dep"].ToString(), 255);
                model.C_empid = int.Parse(request["T_employee_val"].ToString());
                model.C_empname = PageValidate.InputText(request["T_employee1"].ToString(), 255);

                model.receive_real = decimal.Parse(request["T_invoice_amount"]);
                model.Receive_date = DateTime.Parse(request["T_invoice_date"].ToString());
                model.Pay_type_id = int.Parse(request["T_invoice_type_val"].ToString());
                model.Pay_type = PageValidate.InputText(request["T_invoice_type"].ToString(), 255);
                model.remarks = PageValidate.InputText(request["T_content"].ToString(), 12000);
                model.receive_direction_id = int.Parse(request["T_receive_direction_val"].ToString());
                model.receive_direction_name = PageValidate.InputText(request["T_receive_direction"], 255);
                model.Receive_amount = model.receive_direction_id * model.receive_real;

                string cid = PageValidate.InputText( request["receiveid"],50);
                if (!string.IsNullOrEmpty(cid) && cid != "null")
                {
                    model.id = int.Parse(PageValidate.IsNumber(cid) ? cid : "-1");

                    DataSet ds = cci.GetList(" id=" + model.id);
                    DataRow dr = ds.Tables[0].Rows[0];
                    
                    cci.Update(model);

                    C_Sys_log log = new C_Sys_log();

                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.Receive_num;
                    string EventType = "收款修改";
                    int EventID = model.id;

                    if (dr["Receive_amount"].ToString() != request["T_invoice_amount"].Replace(",", "").Replace(".00", ""))                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "收款金额", dr["Receive_amount"].ToString(), request["T_invoice_amount"].Replace(",", "").Replace(".00", ""));                    

                    if (dr["Pay_type"].ToString() != request["T_invoice_type"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "付款方式", dr["Pay_type"].ToString(), request["T_invoice_type"]);
                    
                    if (dr["receive_direction_name"].ToString() != request["T_receive_direction"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "收款类别", dr["receive_direction_name"].ToString(), request["T_receive_direction"]);
                    
                    if (dr["Receive_num"].ToString() != request["T_invoice_num"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "凭证号码", dr["Receive_num"].ToString(), request["T_invoice_num"]);
                    
                    if (dr["Receive_date"].ToString() != request["T_invoice_date"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "收款时间", dr["Receive_date"].ToString(), request["T_invoice_date"]);                    

                    if (dr["remarks"].ToString() != request["T_content"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "收款内容", "原内容被修改", "原内容被修改");
                    
                    if (dr["C_depname"].ToString() != request["T_dep"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "收款人部门", dr["C_depname"].ToString(), request["T_dep"]);
                    
                    if (dr["C_empname"].ToString() != request["T_employee1"])                    
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "收款人姓名", dr["C_empname"].ToString(), request["T_employee1"]);                    
                }
                else
                {
                    model.isDelete = 0;
                    model.create_id = emp_id;
                    model.create_name = dremp["name"].ToString();
                    model.create_date = DateTime.Now;

                    cci.Add(model);
                }
                //更新订单收款金额
                order.UpdateReceive(orderid);
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
                    sortorder = " desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total;
                string serchtxt = "1=1";
                string order_id = request["orderid"];
                if (!string.IsNullOrEmpty(order_id) && order_id != "null")
                    serchtxt += " and order_id=" + int.Parse(order_id);

                string customerid = request["customerid"];
                if (!string.IsNullOrEmpty(customerid) && customerid != "null")
                    serchtxt += " and Customer_id=" + int.Parse(customerid);

                if (!string.IsNullOrEmpty(request["company"]))
                    serchtxt += " and Customer_name like N'%" + PageValidate.InputText( request["company"],250) + "%'";

                if (!string.IsNullOrEmpty(request["receive_num"]))
                    serchtxt += " and Receive_num like N'%" + PageValidate.InputText( request["receive_num"],50) + "%'";

                if (!string.IsNullOrEmpty(request["pay_type"]))
                    serchtxt += " and Pay_type_id =" + int.Parse( request["pay_type_val"]);

                if (!string.IsNullOrEmpty(request["department"]))
                    serchtxt += " and C_depid =" + int.Parse( request["department_val"]);

                if (!string.IsNullOrEmpty(request["employee"]))
                    serchtxt += " and C_empid =" + int.Parse( request["employee_val"]);

                if (!string.IsNullOrEmpty(request["startdate"]))
                    serchtxt += " and Receive_date >= '" + PageValidate.InputText( request["startdate"],50) + "'";

                if (!string.IsNullOrEmpty(request["enddate"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate"]);
                    serchtxt += " and Receive_date  <= '" + enddate + "'";
                }
                if (!string.IsNullOrEmpty(request["startdate_del"]))
                {
                    serchtxt += " and Delete_time >= '" + PageValidate.InputText( request["startdate_del"],50) + "'";
                }
                if (!string.IsNullOrEmpty(request["enddate_del"]))
                {
                    DateTime enddate = DateTime.Parse(request["enddate_del"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and Delete_time  <= '" + enddate + "'";
                }


                //权限
                DataSet ds = cci.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

                string dt = Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
                context.Response.Write(dt);
            }



            if (request["Action"] == "form")
            {
                string invoiceid = PageValidate.InputText(request["receiveid"],50);
                string dt;
                if (PageValidate.IsNumber(invoiceid))
                {
                    DataSet ds = cci.GetList("id=" + invoiceid);
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
                //参数安全过滤
                string c_id = PageValidate.InputText( request["id"],50);                
                DataSet ds = cci.GetList("id=" + int.Parse( c_id));

                bool isdel = cci.Delete(int.Parse(c_id));

                //更新订单金额
                BLL.CRM_order order = new BLL.CRM_order();
                string orderid = ds.Tables[0].Rows[0]["order_id"].ToString();
                order.UpdateReceive(orderid);

                if (isdel)
                {
                    //日志
                    string EventType = "收款删除";

                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    int EventID = int.Parse(c_id);
                    string EventTitle = ds.Tables[0].Rows[0]["Customer_name"].ToString();
                    string Original_txt = ds.Tables[0].Rows[0]["Receive_amount"].ToString();
                    string Current_txt = null;

                    C_Sys_log log = new C_Sys_log();

                    log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "金额", Original_txt, Current_txt);

                    context.Response.Write("true");
                }
                else
                {
                    context.Response.Write("false");
                }
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