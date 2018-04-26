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
    /// toolbar 的摘要说明
    /// </summary>
    public class toolbar : IHttpHandler
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

            //sys toolbar
            if (request["Action"] == "GetSys")
            {
                BLL.Sys_Button btn = new BLL.Sys_Button();

                DataSet dsemp1 = emp.GetList("ID=" + emp_id);
                bool BtnAble = false;
                if (dsemp1.Tables[0].Rows.Count > 0)
                {
                    if (dsemp1.Tables[0].Rows[0]["uid"].ToString() == "admin")
                    {
                        BtnAble = true;
                    }
                }
                DataSet ds = btn.GetList(0, "Menu_id = " + int.Parse(request["mid"]), "convert(int,[Btn_order])");
                Data.GetAuthorityByUid getauth = new Data.GetAuthorityByUid();
                string toolbarscript = "{Items:[";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    toolbarscript += "{";
                    toolbarscript += "type: 'button',";
                    toolbarscript += "text: '" + ds.Tables[0].Rows[i]["Btn_name"].ToString() + "',";
                    toolbarscript += "icon: '" + ds.Tables[0].Rows[i]["Btn_icon"].ToString() + "',";
                    if (BtnAble)
                    {
                        toolbarscript += "disable: true,";
                    }
                    else
                    {
                        toolbarscript += "disable: " + getauth.GetBtnAuthority(emp_id.ToString(), ds.Tables[0].Rows[i]["Btn_id"].ToString()) + ",";
                    }
                    toolbarscript += "click: function() {";
                    toolbarscript += ds.Tables[0].Rows[i]["Btn_handler"].ToString().Replace("()", "(" + int.Parse(request["mid"]) + ")");
                    toolbarscript += "}";
                    toolbarscript += "},";

                }
                toolbarscript = toolbarscript.Substring(0, toolbarscript.Length - 1);
                toolbarscript += "]}";
                context.Response.Write(toolbarscript);
            }
            else
            {
                context.Response.Write("none");
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