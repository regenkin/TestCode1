using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KfCrm.Common;
using System.Text;
using System.Web.Security;


namespace KfCrm.CRM.Data
{
    /// <summary>
    /// tool_batch 的摘要说明
    /// </summary>
    public class tool_batch : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.tool_batch batch = new BLL.tool_batch();
            Model.tool_batch model = new Model.tool_batch();

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
                model.batch_type = PageValidate.InputText(request["type"], 50);
                model.b_count = 0;

                model.o_dep_id = int.Parse(request["T_dep1_val"]);
                model.o_dep = PageValidate.InputText(request["T_dep1"], 250);
                model.o_emp_id = int.Parse(request["T_employee1_val"]);
                model.o_emp = PageValidate.InputText(request["T_employee11"], 250);

                model.c_dep_id = int.Parse(request["T_dep2_val"]);
                model.c_dep = PageValidate.InputText(request["T_dep2"], 250);
                model.c_emp_id = int.Parse(request["T_employee2_val"]);
                model.c_emp = PageValidate.InputText(request["T_employee22"], 250);

                model.create_id = emp_id;
                model.create_name = PageValidate.InputText(empname, 250);
                model.create_date = DateTime.Now;

                switch (model.batch_type)
                {
                    case "customer":

                        string serchtxt = " isDelete=0 ";

                        if (!string.IsNullOrEmpty(request["T_employee1_val"]))
                            serchtxt += string.Format(" and Employee_id={0}", PageValidate.InputText(request["T_employee1_val"], 50));

                        if (!string.IsNullOrEmpty(request["T_customertype"]))
                            serchtxt += " and CustomerType_id = " + int.Parse(request["T_customertype_val"]);

                        if (!string.IsNullOrEmpty(request["T_customerlevel"]))
                            serchtxt += " and CustomerLevel_id = " + int.Parse(request["T_customerlevel_val"]);

                        if (!string.IsNullOrEmpty(request["T_CustomerSource"]))
                            serchtxt += " and CustomerSource_id = " + int.Parse(request["T_CustomerSource_val"]);

                        if (!string.IsNullOrEmpty(request["startdate"]))
                            serchtxt += " and Create_date >= '" + PageValidate.InputText(request["startdate"], 255) + "'";

                        if (!string.IsNullOrEmpty(request["enddate"]))
                        {
                            DateTime enddate = DateTime.Parse(request["enddate"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                            serchtxt += " and Create_date <= '" + enddate + "'";
                        }

                        if (!string.IsNullOrEmpty(request["startfollow"]))
                            serchtxt += " and lastfollow >= '" + PageValidate.InputText(request["startfollow"], 255) + "'";

                        if (!string.IsNullOrEmpty(request["endfollow"]))
                        {
                            DateTime enddate = DateTime.Parse(request["endfollow"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                            serchtxt += " and lastfollow <= '" + enddate + "'";
                        }

                        BLL.CRM_Customer customer = new BLL.CRM_Customer();
                        Model.CRM_Customer model_cus = new Model.CRM_Customer();

                        model.b_count = customer.GetList(string.Format("Employee_id={0} and {1}", model.o_emp_id,serchtxt)).Tables[0].Rows.Count;

                        model_cus.Department_id = model.c_dep_id;
                        model_cus.Department = model.c_dep;
                        model_cus.Employee_id = model.c_emp_id;
                        model_cus.Employee = model.c_emp;
                        model_cus.Create_id = model.o_emp_id;//

                        

                        customer.Update_batch(model_cus, serchtxt);
                        break;

                    case "order":
                        BLL.CRM_order order = new BLL.CRM_order();
                        Model.CRM_order model_order = new Model.CRM_order();

                        model.b_count = order.GetList(string.Format("F_emp_id={0}", model.o_emp_id)).Tables[0].Rows.Count;

                        model_order.F_dep_id = model.c_dep_id;
                        model_order.F_dep_name = model.c_dep;
                        model_order.F_emp_id = model.c_emp_id;
                        model_order.F_emp_name = model.c_emp;
                        model_order.create_id = model.o_emp_id;

                        order.Update_batch(model_order);
                        break;
                }

                batch.Add(model);

            }
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

                //context.Response.Write(serchtxt);

                DataSet ds = batch.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

                string dt = Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
                context.Response.Write(dt);
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