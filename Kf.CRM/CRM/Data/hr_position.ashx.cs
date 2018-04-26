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
    /// hr_position 的摘要说明
    /// </summary>
    public class hr_position : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.hr_position zw = new BLL.hr_position();
            Model.hr_position model = new Model.hr_position();

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
                string serchtxt = "1=1";
                DataSet ds = zw.GetList(0, serchtxt, "convert(int,[position_order])");
                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }             

            //save
            if (request["Action"] == "save")
            {
                model.position_name = PageValidate.InputText(request["T_position"], 255);
                model.position_order = int.Parse( request["T_order"]);
                model.position_level =  PageValidate.InputText( request["T_level"],50);

                string id = PageValidate.InputText(request["id"], 250);

                if (!string.IsNullOrEmpty(id) && id != "null")
                {
                    model.id = int.Parse(id);
                    DataSet ds = zw.GetList(" id=" + int.Parse(id));
                    DataRow dr = ds.Tables[0].Rows[0];
                    zw.Update(model);

                    //日志
                    C_Sys_log log = new C_Sys_log();

                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.position_name;
                    string EventType = "职位修改";
                    int EventID = model.id;

                    if (dr["position_name"].ToString() != request["T_position"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "职务名称", dr["position_name"].ToString(), request["T_position"]);
                    }
                    if (dr["position_level"].ToString() != request["T_level"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "职务级别", dr["position_level"].ToString(), request["T_level"]);
                    }
                    if (dr["position_order"].ToString() != request["T_order"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "行号", dr["position_order"].ToString(), request["T_order"]);
                    }
                }
                else
                {
                    model.isDelete = 0;
                    model.create_id = emp_id;
                    model.create_date = DateTime.Now;
                    zw.Add(model);
                }
            }
            //Form JSON
            if (request["Action"] == "form")
            {
                string id = PageValidate.InputText(request["id"],50);

                DataSet ds = zw.GetList("id=" + int.Parse( id));

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }
           
            if (request["Action"] == "del")
            {
                string id = PageValidate.InputText(request["id"],50) ;
                string EventType = "职务删除";
                DataSet ds = zw.GetList(" id=" + int.Parse( id));
                if (emp.GetList("zhiwuid=" + int.Parse( id)).Tables[0].Rows.Count > 0)
                {
                    //含有员工信息不能删除
                    context.Response.Write("false:emp");
                }
                else
                {
                    bool isdel = zw.Delete(int.Parse(request["id"]));
                    if (isdel)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int UserID = emp_id;
                            string UserName = empname;
                            string IPStreet = request.UserHostAddress;
                            int EventID = int.Parse( id);
                            string EventTitle = ds.Tables[0].Rows[i]["position_name"].ToString();
                            string Original_txt = null;
                            string Current_txt = null;
                            C_Sys_log log = new C_Sys_log();
                            log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, null, Original_txt, Current_txt);
                        }
                        context.Response.Write("true");
                    }
                    else
                    {
                        context.Response.Write("false");
                    }
                }
            }


            if (request["Action"] == "combo")
            {
                DataSet ds = zw.GetList(0, "", "position_level");
                StringBuilder str = new StringBuilder();
                str.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["position_name"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
                context.Response.Write(str);
            }

            if (request["Action"] == "getlevel")
            {
                int position_id = int.Parse(request["position_id"]);

                BLL.hr_position hz = new BLL.hr_position();
                DataSet ds = hz.GetList("id=" + position_id);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    context.Response.Write(ds.Tables[0].Rows[0]["position_level"]);
                }
                else
                {
                    context.Response.Write("-1");
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