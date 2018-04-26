using System;
using System.Data;
using System.Web;
using System.Web.Security;
using KfCrm.Common;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// Personal_notes 便签 的摘要说明
    /// </summary>
    public class Personal_notes : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.Personal_notes notes = new BLL.Personal_notes();
            Model.Personal_notes model = new Model.Personal_notes();

            var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
            var ticket = FormsAuthentication.Decrypt(cookie.Value);
            string CoockiesID = ticket.UserData;

            BLL.hr_employee emp = new BLL.hr_employee();
            int emp_id = int.Parse(CoockiesID);
            DataSet dsemp = emp.GetList("id=" + emp_id);
            string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
            string uid = dsemp.Tables[0].Rows[0]["uid"].ToString();

            if (request["Action"] == "Get")
            {
                DataSet ds = notes.GetList("emp_id=" + emp_id);

                context.Response.Write(GetGridJSON.DataTableToJSON2(ds.Tables[0]));
            }
            if (request["Action"] == "save")
            {
                model.emp_id = emp_id;
                model.note_content = PageValidate.InputText( request["body"],int.MaxValue);
                model.note_time = DateTime.Now;
                model.note_color = PageValidate.InputText( request["color"],50);
                model.xyz = decimal.Parse(request["left"]) + "," + decimal.Parse(request["top"]) + "," + decimal.Parse(request["zindex"]);

                int id = notes.Add(model);

                context.Response.Write(id);
            }
            if (request["Action"] == "update")
            {
                model.xyz = decimal.Parse(request["x"].ToString()) + "," + decimal.Parse(request["y"].ToString()) + "," + decimal.Parse(request["z"].ToString());
                model.id = int.Parse(request["id"]);

                notes.Update(model);
            }
            if (request["Action"] == "delete")
            {       
                bool a = notes.Delete(int.Parse(request["id"]));
                context.Response.Write(a);
            }
            if (request["Action"] == "grid")
            {    
                DataSet ds = notes.GetList(0, "emp_id=" + emp_id, "note_time desc");
                DataTable dt = ds.Tables[0];

                context.Response.Write(GetGridJSON.DataTableToJSON(dt));
            }

            if (request["Action"] == "notesremind")
            {
                DataSet ds = notes.GetList(7, "emp_id=" + emp_id, " note_time desc");
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