using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// Sys_data_authority 的摘要说明
    /// </summary>
    public class Sys_data_authority : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.Sys_data_authority auth = new BLL.Sys_data_authority();
            Model.Sys_data_authority model = new Model.Sys_data_authority();

            if (request["Action"] == "get")
            {
                DataSet ds = auth.GetList("Role_id=" + int.Parse( request["Role_id"]));
                if (ds.Tables[0].Rows.Count == 4)
                {
                    context.Response.Write(Common.GetGridJSON.DataTableToJSON(ds.Tables[0]));
                }
                else
                {
                    string datatxt = "";

                    datatxt += "{Rows: [";
                    datatxt += "        { '__status': null, 'option_id': 1, 'Sys_option': '客户管理', 'Sys_view': 1, 'Sys_add': 1, 'Sys_edit': 1, 'Sys_del': 1 },";
                    datatxt += "        { '__status': null, 'option_id': 2, 'Sys_option': '跟进管理', 'Sys_view': 1, 'Sys_add': 1, 'Sys_edit': 1, 'Sys_del': 1 },";
                    datatxt += "        { '__status': null, 'option_id': 3, 'Sys_option': '订单管理', 'Sys_view': 1, 'Sys_add': 1, 'Sys_edit': 1, 'Sys_del': 1 },";
                    datatxt += "        { '__status': null, 'option_id': 4, 'Sys_option': '合同管理', 'Sys_view': 1, 'Sys_add': 1, 'Sys_edit': 1, 'Sys_del': 1 }";

                    datatxt += "    ],Total: 4 }";
                    context.Response.Write(datatxt);
                }
            }
            if (request["Action"] == "save")
            {
                string rid = request["rid"];
                string savestring = request["savestring"];

                model.Role_id = int.Parse(rid);

                auth.Delete("Role_id=" + int.Parse(rid));

                JavaScriptSerializer json = new JavaScriptSerializer();
                List<AuthData> _list = json.Deserialize<List<AuthData>>(savestring);

                foreach (AuthData authdata in _list)
                {
                    model.option_id = authdata.option_id;
                    model.Sys_view = authdata.Sys_view;
                    model.Sys_add = authdata.Sys_add;
                    model.Sys_edit = authdata.Sys_edit;
                    model.Sys_del = authdata.Sys_del;
                    model.Sys_option = Common.PageValidate.InputText( authdata.Sys_option,50);

                    auth.Add(model);
                }
                context.Response.Write("true");
            }
        }

        public class AuthData
        {
            public int option_id { get; set; }
            public string Sys_option { get; set; }
            public int Sys_view { get; set; }
            public int Sys_add { get; set; }
            public int Sys_edit { get; set; }
            public int Sys_del { get; set; }

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