using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// sys_log_err 的摘要说明
    /// </summary>
    public class sys_log_err : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            if (request["Action"] == "grid")
            {
                BLL.Sys_log_Err log = new BLL.Sys_log_Err();

                int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                string sortname = request["sortname"];
                string sortorder = request["sortorder"];

                if (string.IsNullOrEmpty(sortname))
                    sortname = " Err_time";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = " desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total = "0";

                DataSet ds = null;

                string serchtext = "1=1";

                if (!string.IsNullOrEmpty(request["stype"]))
                    serchtext += " and Err_type = '" + request["stype"] + "'";

                if (!string.IsNullOrEmpty(request["sstart"]))
                    serchtext += " and Err_time >= '" + DateTime.Parse(request["sstart"]) + "'";

                if (!string.IsNullOrEmpty(request["sdend"]))
                    serchtext += " and Err_time <= '" + DateTime.Parse(request["sdend"]).AddHours(23).AddMinutes(59).AddSeconds(59) + "'";

                if (!string.IsNullOrEmpty(request["stext"]))
                {
                    serchtext += " and (Err_url like N'%" + request["stext"] + "%'";
                    serchtext += " or Err_message like N'%" + request["stext"] + "%'";
                    serchtext += " or Err_source like N'%" + request["stext"] + "%'";
                    serchtext += " or Err_ip like N'%" + request["stext"] + "%'";
                }

                ds = log.GetList(PageSize, PageIndex, serchtext, sorttext, out Total);

                string dt = Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
                context.Response.Write(dt);
            }
            if (request["Action"] == "logtype")
            {
                BLL.Sys_log_Err log = new BLL.Sys_log_Err();

                DataSet ds = log.GetLogtype();

                StringBuilder str = new StringBuilder();

                str.Append("[");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{value:'" + ds.Tables[0].Rows[i]["Err_type"].ToString() + "',text:'" + ds.Tables[0].Rows[i]["Err_type"] + "'},");
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