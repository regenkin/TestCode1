using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Security;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// Sys_log 的摘要说明
    /// </summary>
    public class Sys_log : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            string CoockiesID = ticket.UserData;

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(CoockiesID);
            DataSet dsemp = emp.GetList("id=" + emp_id);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "grid")
            {
                BLL.Sys_log log = new BLL.Sys_log();

                int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                string sortname = request["sortname"];
                string sortorder = request["sortorder"];

                if (string.IsNullOrEmpty(sortname))
                    sortname = " EventDate";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = " desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total = "0";

                DataSet ds = null;

                string serchtext = " 1=1 ";

                if (!string.IsNullOrEmpty(request["stype"]))
                    serchtext += " and EventType = '" + Common.PageValidate.InputText( request["stype"],255) + "'";

                if (!string.IsNullOrEmpty(request["sstart"]))
                    serchtext += " and EventDate >= '" + Common.PageValidate.InputText( request["sstart"],255) + "'";

                if (!string.IsNullOrEmpty(request["sdend"]))
                {
                    DateTime enddate = DateTime.Parse(request["sdend"]);
                    serchtext += " and EventDate <= '" + DateTime.Parse(request["sdend"]).AddHours(23).AddMinutes(59).AddSeconds(59) + "'";
                }

                if (!string.IsNullOrEmpty(request["stext"]))
                {
                    string stext = Common.PageValidate.InputText(request["stext"], 10000);
                    serchtext += " and (EventID like N'%" + stext + "%'";
                    serchtext += " or EventTitle like N'%" + stext + "%'";
                    serchtext += " or Original_txt like N'%" + stext + "%'";
                    serchtext += " or Current_txt like N'%" + stext + "%'";
                    serchtext += " or IPStreet like N'%" + stext + "%'";
                    serchtext += " or UserName like N'%" + stext + "%')";
                }

                ds = log.GetList(PageSize, PageIndex, serchtext, sorttext, out Total);

                string dt = Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
                context.Response.Write(dt);
            }
            if (request["Action"] == "logtype")
            {
                BLL.Sys_log log = new BLL.Sys_log();

                DataSet ds = log.GetLogtype();

                StringBuilder str = new StringBuilder();

                str.Append("[");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{value:'" + ds.Tables[0].Rows[i]["EventType"].ToString() + "',text:'" + ds.Tables[0].Rows[i]["EventType"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");

                context.Response.Write(str);
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