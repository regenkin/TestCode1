using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Data;
using System.Text;
using KfCrm.Common;
using System.Web.Security;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// Sys_role_emp 的摘要说明
    /// </summary>
    public class Sys_role_emp : IHttpHandler
    {


        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.Sys_role_emp rm = new BLL.Sys_role_emp();
            Model.Sys_role_emp model = new Model.Sys_role_emp();

            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            string CoockiesID = ticket.UserData;

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(CoockiesID);
            DataSet dsemp = emp.GetList("id=" + emp_id);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "add")
            {
                string rid = PageValidate.InputText(request["role_id"], 50);
                string empids = Common.PageValidate.InputText(request["empids"], int.MaxValue);
                //rm.Delete(string.Format("RoleID={0} and empID in ({1})", int.Parse(rid), empids));
                string[] emplist = empids.Split(',');
                model.RoleID = int.Parse(rid);
                for (int i = 0; i < emplist.Length; i++)
                {
                    model.empID = int.Parse(emplist[i].ToString());
                    rm.Add(model);
                }

                BLL.Sys_log log = new BLL.Sys_log();
                Model.Sys_log modellog = new Model.Sys_log();

                modellog.EventDate = DateTime.Now;
                modellog.UserID = emp_id;
                modellog.UserName = PageValidate.InputText(empname, 255);
                modellog.IPStreet = context.Request.UserHostAddress;

                modellog.EventType = "权限人员调整";
                modellog.EventID = rid.ToString();
                log.Add(modellog);
            }
            if (request["Action"] == "remove")
            {
                string rid = PageValidate.InputText(request["role_id"], 50);
                string empids = Common.PageValidate.InputText(request["empids"], int.MaxValue);
                rm.Delete(string.Format("RoleID={0} and empID in ({1})", int.Parse(rid), empids));

                BLL.Sys_log log = new BLL.Sys_log();
                Model.Sys_log modellog = new Model.Sys_log();

                modellog.EventDate = DateTime.Now;
                modellog.UserID = emp_id;
                modellog.UserName = PageValidate.InputText(empname, 255);
                modellog.IPStreet = context.Request.UserHostAddress;

                modellog.EventType = "权限人员调整";
                modellog.EventID = rid.ToString();
                log.Add(modellog);
            }
            if (request["Action"] == "emplist")
            {
                string rid = PageValidate.InputText(request["role_id"], 50);

                string sql = (string.Format("ID not in (select empID from Sys_role_emp where RoleID={0}) and ID !=1 ", rid));
                if (!string.IsNullOrEmpty(request["stext"]))
                {
                    sql += " and name like N'%" + PageValidate.InputText(request["stext"], 255) + "%'";
                }

                int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                string sortname = request["sortname"];
                string sortorder = request["sortorder"];

                if (string.IsNullOrEmpty(sortname))
                    sortname = " ID";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = " desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total;

                dsemp = emp.GetList(PageSize, PageIndex, sql, sorttext, out Total);

                string dt = Common.GetGridJSON.DataTableToJSON1(dsemp.Tables[0], Total);
                context.Response.Write(dt);
            }
            if (request["Action"] == "get")
            {
                string rid = PageValidate.InputText(request["role_id"], 50);
                if (!string.IsNullOrEmpty(rid))
                {
                    string sql = (string.Format("ID in (select empID from Sys_role_emp where RoleID={0})",int.Parse( rid)));
                    if (!string.IsNullOrEmpty(request["stext"]))
                    {
                        sql += " and name like N'%" + PageValidate.InputText(request["stext"], 255) + "%'";
                    }
                    int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                    int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                    string sortname = request["sortname"];
                    string sortorder = request["sortorder"];

                    if (string.IsNullOrEmpty(sortname))
                        sortname = " ID";
                    if (string.IsNullOrEmpty(sortorder))
                        sortorder = " desc";

                    string sorttext = " " + sortname + " " + sortorder;

                    string Total;

                    dsemp = emp.GetList(PageSize, PageIndex, sql, sorttext, out Total);

                    string dt = Common.GetGridJSON.DataTableToJSON1(dsemp.Tables[0], Total);
                    context.Response.Write(dt);

                }
                else
                {
                    context.Response.Write("test" + rid);
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