using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using KfCrm.Common;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// Reports_CRM 的摘要说明
    /// </summary>
    public class Reports_CRM : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            if (request["Action"] == "CRM_Reports_year")
            {
                BLL.CRM_Customer ccc = new BLL.CRM_Customer();

                string items = PageValidate.InputText(context.Request["stype_val"], 255);
                int year = int.Parse(context.Request["syear_val"]);

                DataSet ds = ccc.Reports_year(items, year, "");

                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }
            if (request["Action"] == "Follow_Reports_year")
            {
                BLL.CRM_Follow follow = new BLL.CRM_Follow();

                string items = "Follow_Type";
                int year = int.Parse(context.Request["syear_val"]);

                DataSet ds = follow.Reports_year(items, year, "");

                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }

            if (request["Action"] == "Funnel")
            {
                string whereStr = "";
                int year = int.Parse(context.Request["syear_val"]);      

                string items = PageValidate.InputText(context.Request["stype_val"], 255);
                if (!string.IsNullOrEmpty(items) && items != "null")
                    whereStr = string.Format("  a.id in({0})", items.Replace(';', ','));

                //context.Response.Write(whereStr);

                BLL.CRM_Customer ccc = new BLL.CRM_Customer();
                DataSet ds = ccc.Funnel(whereStr,year.ToString());

                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
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