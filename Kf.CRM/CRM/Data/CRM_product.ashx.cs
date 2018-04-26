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
    /// CRM_product 的摘要说明
    /// </summary>
    public class CRM_product : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.CRM_product ccp = new BLL.CRM_product();
            Model.CRM_product model = new Model.CRM_product();

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
                model.category_id = int.Parse(request["T_product_category_val"]);
                model.category_name = PageValidate.InputText(request["T_product_category"], 255);
                model.product_name = PageValidate.InputText(request["T_product_name"], 255);
                model.specifications = PageValidate.InputText(request["T_specifications"], 255);
                model.unit = PageValidate.InputText(request["T_product_unit"], 255);
                model.remarks = PageValidate.InputText(request["T_remarks"], 255);
                model.price = decimal.Parse(request["T_price"].ToString());

                string pid = PageValidate.InputText( request["pid"],50) ;
                if (!string.IsNullOrEmpty(pid) && pid != "null")
                {
                    model.product_id = int.Parse(PageValidate.IsNumber(pid) ? pid : "-1");
                    DataSet ds = ccp.GetList(" product_id=" + int.Parse(pid));
                    DataRow dr = ds.Tables[0].Rows[0];
                    ccp.Update(model);

                    //日志
                    C_Sys_log log = new C_Sys_log();

                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.product_name;
                    string EventType = "产品修改";
                    int EventID = model.product_id;
                    if (dr["category_name"].ToString() != request["T_product_category"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "产品类别", dr["category_name"].ToString(), request["T_product_category"]);
                    }
                    if (dr["product_name"].ToString() != request["T_product_name"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "产品名字", dr["product_name"].ToString(), request["T_product_name"]);
                    }
                    if (dr["specifications"].ToString() != request["T_specifications"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "产品规格", dr["specifications"].ToString(), request["T_specifications"]);
                    }
                    if (dr["unit"].ToString() != request["T_product_unit"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "单位", dr["unit"].ToString(), request["T_product_unit"]);
                    }
                    if (dr["remarks"].ToString() != request["T_remarks"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "备注", dr["remarks"].ToString(), request["T_remarks"]);
                    }
                    if (dr["price"].ToString() != request["T_price"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "价格", dr["price"].ToString(), request["T_price"]);
                    }
                }
                else
                {
                    model.isDelete = 0;
                    ccp.Add(model);
                }
            }

            if (request["Action"] == "grid")
            {
                int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                string sortname = request["sortname"];
                string sortorder = request["sortorder"];

                if (string.IsNullOrEmpty(sortname))
                    sortname = " category_id";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = "desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total;
                string serchtxt = "1=1";  
                if (!string.IsNullOrEmpty(request["categoryid"]))
                    serchtxt += string.Format(" and category_id={0}", int.Parse( request["categoryid"]));

                if (!string.IsNullOrEmpty(request["stext"]))
                    serchtxt += " and product_name like N'%" + PageValidate.InputText(request["stext"], 255) + "%'";

                //权限
                DataSet ds = ccp.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

                string dt = Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
                context.Response.Write(dt);
            }
            if (request["Action"] == "form")
            {
                int pid = int.Parse(request["pid"]);
                DataSet ds = ccp.GetList(" product_id=" + pid);

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }
            //del
            if (request["Action"] == "del")
            {
                //参数安全过滤
                string c_id = PageValidate.InputText(request["id"], 50);
                DataSet ds = ccp.GetList(" product_id=" + int.Parse(c_id));

                BLL.CRM_order_details ccod = new BLL.CRM_order_details();
                if (ccod.GetList("product_id=" + int.Parse(c_id)).Tables[0].Rows.Count > 0)
                {
                    //order
                    context.Response.Write("false:order");
                }
                else
                {
                    bool isdel = ccp.Delete(int.Parse(c_id));
                    if (isdel)
                    {
                        //日志
                        string EventType = "产品删除";

                        int UserID = emp_id;
                        string UserName = empname;
                        string IPStreet = request.UserHostAddress;
                        int EventID = int.Parse(c_id);
                        string EventTitle = ds.Tables[0].Rows[0]["product_name"].ToString();
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