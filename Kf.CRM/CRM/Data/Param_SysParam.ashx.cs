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
    /// Param_SysParam 的摘要说明
    /// </summary>
    public class Param_SysParam : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.Param_SysParam psp = new BLL.Param_SysParam();
            Model.Param_SysParam model = new Model.Param_SysParam();

            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            string CoockiesID = ticket.UserData;

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(CoockiesID);
            DataSet dsemp = emp.GetList("id=" + emp_id);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "GetApp")
            {
                BLL.Param_SysParam_Type cpst = new BLL.Param_SysParam_Type();
                DataSet ds = cpst.GetList(0, "", "params_order");

                StringBuilder str = new StringBuilder();
                str.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",pid:0,text:'" + ds.Tables[0].Rows[i]["params_name"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
                context.Response.Write(str);
            }
            if (request["Action"] == "GetParams")
            {
                string parentid = PageValidate.InputText(request["parentid"], 50);

                DataSet ds = psp.GetList(0, " parentid=" + int.Parse(parentid), "params_order");
                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }

            //combo
            if (request["Action"] == "combo")
            {
                string parentid = PageValidate.InputText(request["parentid"], 50);

                DataSet ds = psp.GetList(0, " parentid=" + int.Parse(parentid), "params_order");

                StringBuilder str = new StringBuilder();

                str.Append("[");
                //str.Append("{id:0,text:'无'},");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["params_name"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");

                context.Response.Write(str);
            }
            //Form JSON
            if (request["Action"] == "form")
            {
                DataSet ds = psp.GetList("id=" + int.Parse(request["paramid"]));

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }
            //save
            if (request["Action"] == "save")
            {
                model.params_name = PageValidate.InputText(request["T_param_name"], 255);
                model.params_order = int.Parse(request["T_param_order"]);

                string id = request["paramid"];

                if (!string.IsNullOrEmpty(id) && id != "null")
                {
                    DataSet ds = psp.GetList("id=" + int.Parse(id));
                    model.parentid = int.Parse(ds.Tables[0].Rows[0]["parentid"].ToString());
                    model.id = int.Parse(id);
                    psp.Update(model);
                }
                else
                {
                    model.parentid = int.Parse(request["parentid"]);
                    psp.Add(model);
                }
            }
            //del
            if (request["Action"] == "del")
            {
                string parentid = request["parentid"];
                string paramid = request["paramid"];

                BLL.CRM_Customer customer = new BLL.CRM_Customer();
                BLL.CRM_Follow follow = new BLL.CRM_Follow();
                BLL.CRM_order order = new BLL.CRM_order();
                BLL.CRM_invoice invoice = new BLL.CRM_invoice();
                BLL.CRM_receive receive = new BLL.CRM_receive();

                DataSet ds = null;

                switch (int.Parse(parentid))
                {
                    case 8: ds = customer.GetList("industry_id=" + int.Parse(paramid)); break;
                    case 1: ds = customer.GetList("CustomerType_id=" + int.Parse(paramid)); break;
                    case 2: ds = customer.GetList("CustomerLevel_id=" + int.Parse(paramid)); break;
                    case 3: ds = customer.GetList("CustomerSource_id=" + int.Parse(paramid)); break;
                    case 4: ds = follow.GetList("Follow_Type_id=" + int.Parse(paramid)); break;
                    case 5: ds = order.GetList("pay_type_id=" + int.Parse(paramid)); if (ds.Tables[0].Rows.Count == 0) ds = receive.GetList("Pay_type_id=" + int.Parse(paramid)); break;
                    case 6: ds = order.GetList("Order_status_id=" + int.Parse(paramid)); break;
                    case 7: ds = invoice.GetList("invoice_type_id=" + int.Parse(paramid)); break;
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    context.Response.Write("false:data");
                    return;
                }

                bool isdel = psp.Delete(int.Parse(paramid));
                if (isdel)
                {
                    context.Response.Write("true");
                }
                else
                {
                    context.Response.Write("false");
                }
            }
            if (request["Action"] == "validate")
            {
                string param_name = PageValidate.InputText(request["T_param_name"], 50);
                string parentid = PageValidate.InputText(request["parentid"], 50);
                string cid = PageValidate.InputText(request["T_cid"], 50) ;
                if (string.IsNullOrEmpty(cid) || cid == "null")
                    cid = "0";

                DataSet ds = psp.GetList(string.Format(" params_name='{0}' and parentid={1} and id!={2} ",param_name,parentid,cid));
                //context.Response.Write(" Count:" + ds.Tables[0].Rows.Count);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    context.Response.Write("false");
                }
                else
                {
                    context.Response.Write("true");
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