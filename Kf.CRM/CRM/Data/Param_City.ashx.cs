using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using KfCrm.Common;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// Param_City 的摘要说明
    /// </summary>
    public class Param_City : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.Param_City pc = new BLL.Param_City(); 
            Model.Param_City model = new Model.Param_City();

            if (request["Action"] == "treegrid")
            {
                DataSet ds = pc.GetList(0, "", "City_order");
                string dt = "{Rows:[" + GetTasksString(0, ds.Tables[0]) + "]}";
                context.Response.Write(dt);
            }
            if (request["Action"] == "tree")
            {
                DataSet ds = pc.GetAllList();  
                StringBuilder str = new StringBuilder();
                str.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",pid:" + ds.Tables[0].Rows[i]["parentid"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["City"]  + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");
                context.Response.Write(str);
            }
            //save
            if (request["Action"] == "save")
            {
                model.City = Common.PageValidate.InputText(request["T_City"], 255);
                model.City_order = int.Parse(request["T_order"]);
                string pid = request["T_Parent_val"];
                if (string.IsNullOrEmpty(pid))
                {
                    pid = "0";
                }
                model.parentid = int.Parse(pid);

                string id = PageValidate.InputText( request["id"],50);

                if (!string.IsNullOrEmpty(id) && id != "null")
                {
                    model.id = int.Parse(id);
                    pc.Update(model);
                }
                else
                {
                    pc.Add(model);
                }
            }
            //Form JSON
            if (request["Action"] == "form")
            {
                string id = PageValidate.InputText(request["id"], 50);
                string dt;

                if (PageValidate.IsNumber(id))
                {
                    DataSet ds = pc.GetList("id=" + id);

                    dt = Common.DataToJson.DataToJSON(ds);
                }
                else
                {
                    dt = "{}";
                }

                context.Response.Write(dt);
            }
            //del
            if (request["Action"] == "del")
            {
                string c_id = PageValidate.InputText( request["id"],50);
                DataSet ds = pc.GetList(" parentid=" + int.Parse(c_id));
                BLL.CRM_Customer cus = new BLL.CRM_Customer();
                DataSet ds1 = cus.GetList(string.Format("Provinces_id={0} or City_id={0}", int.Parse(c_id)));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    context.Response.Write("false:parent");
                }
                else if(ds1.Tables[0].Rows.Count>0)
                {
                    context.Response.Write("false:customer");
                }
                else
                {
                    bool isdel = pc.Delete(int.Parse(c_id));
                    if (isdel)
                    {
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
                DataSet ds = pc.GetList("parentid=0");

                StringBuilder str = new StringBuilder();

                str.Append("[");
                str.Append("{id:0,text:'无'},");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["City"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");

                context.Response.Write(str);
            }
            if (request["Action"] == "combo1")
            {
                DataSet ds = pc.GetList("parentid=0");

                StringBuilder str = new StringBuilder();

                str.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["City"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");

                context.Response.Write(str);
            }
            if (request["Action"] == "combo2")
            {
                DataSet ds = pc.GetList("parentid=" + int.Parse( request["pid"]));

                StringBuilder str = new StringBuilder();

                str.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["City"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");

                context.Response.Write(str);
            }
        }
        private static string GetTasksString(int Id, DataTable table)
        {
            DataRow[] rows = table.Select("parentid=" + Id.ToString());

            if (rows.Length == 0) return string.Empty; ;
            StringBuilder str = new StringBuilder();

            foreach (DataRow row in rows)
            {
                str.Append("{");
                for (int i = 0; i < row.Table.Columns.Count; i++)
                {
                    if (i != 0) str.Append(",");
                    str.Append(row.Table.Columns[i].ColumnName);
                    str.Append(":'");
                    str.Append(row[i].ToString());
                    str.Append("'");
                }
                if (GetTasksString((int)row["id"], table).Length > 0)
                {
                    str.Append(",children:[");
                    str.Append(GetTasksString((int)row["id"], table));
                    str.Append("]},");
                }
                else
                {
                    str.Append("},");
                }
            }
            return str[str.Length - 1] == ',' ? str.ToString(0, str.Length - 1) : str.ToString();
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