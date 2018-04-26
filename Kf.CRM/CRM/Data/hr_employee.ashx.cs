using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Security;
using System.Web.Script.Serialization;
using KfCrm.Common;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// hr_employee 的摘要说明
    /// </summary>
    public class hr_employee : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            Model.hr_employee model = new Model.hr_employee();

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
                int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                string sortname = request["sortname"];
                string sortorder = request["sortorder"];

                if (string.IsNullOrEmpty(sortname))
                    sortname = " ID";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = " desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total;
                string serchtxt = "uid!='admin'";

                string did = request["did"];
                if (!string.IsNullOrEmpty(did) && did != null && did != "null")
                    serchtxt += " and d_id=" + int.Parse(did);

                string authtxt = request["auth"];
                if (authtxt == "1")
                {
                    Data.GetDataAuth dataauth = new Data.GetDataAuth();
                    string txt = dataauth.GetDataAuthByid("1", "Sys_add", emp_id.ToString());
                    string[] arr = txt.Split(':');
                    switch (arr[0])
                    {
                        case "my": serchtxt += " and ID=" + emp_id;
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(request["stext"]))
                {
                    if (request["stext"] != "输入姓名搜索")
                        serchtxt += " and name like N'%" + PageValidate.InputText(request["stext"], 255) + "%'";
                }
                //权限
                DataSet ds = emp.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

                string dt = Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total);
                context.Response.Write(dt);
            }
            //表格json
            if (request["Action"] == "getRole")
            {
                int r_empid = int.Parse(request["empid"]);
                DataSet ds = emp.GetRole(r_empid);

                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);

                context.Response.Write(dt);
            }
            //validate
            if (request["Action"] == "Exist")
            {
                string user_id = request["T_uid"];
                string T_emp_id = request["emp_id"];
                if (string.IsNullOrEmpty(T_emp_id) || T_emp_id == "null")
                    T_emp_id = "0";

                DataSet ds1 = emp.GetList(" uid='" + PageValidate.InputText(user_id, 250) + "' and  ID!=" + int.Parse(T_emp_id));

                context.Response.Write(ds1.Tables[0].Rows.Count > 0 ? "false" : "true");

            }

            //Form JSON
            if (request["Action"] == "form")
            {
                string eid = PageValidate.InputText(request["id"], 50);

                if (eid == "epu")
                    eid = emp_id.ToString();

                DataSet ds = emp.GetList("id=" + int.Parse(eid));

                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);

            }
            //save
            if (request["Action"] == "save")
            {
                model.uid = PageValidate.InputText(request["T_uid"], 255);
                model.email = PageValidate.InputText(request["T_email"], 255);
                model.name = PageValidate.InputText(request["T_name"], 255);
                model.birthday = PageValidate.InputText(request["T_birthday"], 255);
                model.sex = PageValidate.InputText(request["T_sex"], 255);
                model.idcard = PageValidate.InputText(request["T_idcard"], 255);
                model.tel = PageValidate.InputText(request["T_tel"], 255);
                model.status = PageValidate.InputText(request["T_status"], 255);
                model.EntryDate = PageValidate.InputText(request["T_entryDate"], 255);
                model.address = PageValidate.InputText(request["T_Adress"], 255);
                model.schools = PageValidate.InputText(request["T_school"], 255);
                model.education = PageValidate.InputText(request["T_edu"], 255);
                model.professional = PageValidate.InputText(request["T_professional"], 255);
                model.remarks = PageValidate.InputText(request["T_remarks"], 255);
                model.title = PageValidate.InputText(request["headurl"], 255);
                model.canlogin = int.Parse(request["canlogin"]);

                int empid;
                string id = PageValidate.InputText(request["id"], 50);
                if (!string.IsNullOrEmpty(id) && id != "null")
                {
                    DataSet ds = emp.GetList(" ID=" + int.Parse(id));
                    DataRow dr = ds.Tables[0].Rows[0];
                    model.ID = int.Parse(id);
                    empid = model.ID;

                    emp.Update(model);

                    C_Sys_log log = new C_Sys_log();

                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.name;
                    string EventType = "员工修改";
                    int EventID = model.ID;

                    if (dr["email"].ToString() != request["T_email"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "邮箱", dr["email"].ToString(), request["T_email"]);

                    if (dr["name"].ToString() != request["T_name"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "员工姓名", dr["name"].ToString(), request["T_name"]);

                    if (dr["birthday"].ToString() != request["T_birthday"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "员工生日", dr["birthday"].ToString(), request["T_birthday"]);

                    if (dr["sex"].ToString() != request["T_sex"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "员工性别", dr["sex"].ToString(), request["T_sex"]);

                    if (dr["status"].ToString() != request["T_status"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "状态", dr["status"].ToString(), request["T_status"]);

                    if (dr["idcard"].ToString() != request["T_idcard"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "身份证", dr["idcard"].ToString(), request["T_idcard"]);

                    if (dr["tel"].ToString() != request["T_tel"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "手机", dr["tel"].ToString(), request["T_tel"]);

                    if (dr["EntryDate"].ToString() != request["T_entryDate"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "入职日期", dr["EntryDate"].ToString(), request["T_entryDate"]);

                    if (dr["address"].ToString() != request["T_Adress"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "地址", dr["address"].ToString(), request["T_Adress"]);

                    if (dr["schools"].ToString() != request["T_school"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "毕业学校", dr["schools"].ToString(), request["T_school"]);

                    if (dr["education"].ToString() != request["T_edu"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "学历", dr["education"].ToString(), request["T_edu"]);

                    if (dr["professional"].ToString() != request["T_professional"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "专业", dr["professional"].ToString(), request["T_professional"]);

                    if (dr["remarks"].ToString() != request["T_remarks"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "备注", dr["remarks"].ToString(), request["T_remarks"]);

                    if (dr["canlogin"].ToString() != request["canlogin"])
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "能否登录", dr["canlogin"].ToString(), request["canlogin"]);


                    context.Response.Write(id + "|" + model.name);
                }
                else
                {
                    model.isDelete = 0;
                    model.pwd = FormsAuthentication.HashPasswordForStoringInConfigFile("123456", "MD5");
                    empid = emp.Add(model);
                    context.Response.Write("{success:success}");
                }

                //post
                string json = request["PostData"].ToLower();
                JavaScriptSerializer js = new JavaScriptSerializer();

                PostData[] postdata;
                postdata = js.Deserialize<PostData[]>(json);

                BLL.hr_post hp = new BLL.hr_post();
                Model.hr_post modelpost = new Model.hr_post();

                modelpost.emp_id = empid;
                model.ID = empid;
                modelpost.emp_name = PageValidate.InputText(request["T_name"], 255);

                for (int i = 0; i < postdata.Length; i++)
                {
                    modelpost.post_id = postdata[i].Post_id;
                    modelpost.default_post = postdata[i].Default_post;

                    if (postdata[i].Default_post == 1)
                    {
                        model.d_id = postdata[i].dep_id;
                        model.dname = postdata[i].Depname;
                        model.zhiwuid = postdata[i].Position_id;
                        model.zhiwu = postdata[i].Position_name;
                        model.postid = postdata[i].Post_id;
                        model.post = postdata[i].Post_name;
                        //context.Response.Write(postdata[i].Depname + "@");
                        //更新默认岗位
                        emp.UpdatePost(model);

                        // 更新客户，订单，合同，收款，开票 人员
                        emp.UpdateCOCRI(model);

                        //清除员工
                        hp.UpdatePostEmpbyEid(empid);
                    }

                    //设置员工
                    hp.UpdatePostEmp(modelpost);
                    //context.Response.Write("{success:success}");
                }
            }
            if (request["Action"] == "PersonalUpdate")
            {
                model.email = PageValidate.InputText(request["T_email"], 255);
                model.name = PageValidate.InputText(request["T_name"], 255);
                model.birthday = PageValidate.InputText(request["T_birthday"], 255);
                model.sex = PageValidate.InputText(request["T_sex"], 255);
                model.idcard = PageValidate.InputText(request["T_idcard"], 255);
                model.tel = PageValidate.InputText(request["T_tel"], 255);


                model.address = PageValidate.InputText(request["T_Adress"], 255);
                model.schools = PageValidate.InputText(request["T_school"], 255);
                model.education = PageValidate.InputText(request["T_edu"], 255);
                model.professional = PageValidate.InputText(request["T_professional"], 255);
                model.remarks = PageValidate.InputText(request["T_remarks"], 255);
                model.title = PageValidate.InputText(request["headurl"], 255);

                DataRow dr = dsemp.Tables[0].Rows[0];
                model.ID = emp_id;

                bool isup = emp.PersonalUpdate(model);

                if (isup)
                    context.Response.Write("true");
                else
                    context.Response.Write("false");

                C_Sys_log log = new C_Sys_log();

                int UserID = emp_id;
                string UserName = empname;
                string IPStreet = request.UserHostAddress;
                string EventTitle = model.name;
                string EventType = "个人信息修改";
                int EventID = emp_id;

                if (dr["email"].ToString() != request["T_email"])
                    log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "邮箱", dr["email"].ToString(), request["T_email"]);

                if (dr["name"].ToString() != request["T_name"])
                    log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "员工姓名", dr["name"].ToString(), request["T_name"]);

                if (dr["birthday"].ToString() != request["T_birthday"])
                    log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "员工生日", dr["birthday"].ToString(), request["T_birthday"]);

                if (dr["sex"].ToString() != request["T_sex"])
                    log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "员工性别", dr["sex"].ToString(), request["T_sex"]);

                if (dr["idcard"].ToString() != request["T_idcard"])
                    log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "身份证", dr["idcard"].ToString(), request["T_idcard"]);

                if (dr["tel"].ToString() != request["T_tel"])
                    log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "手机", dr["tel"].ToString(), request["T_tel"]);

                if (dr["address"].ToString() != request["T_Adress"])
                    log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "地址", dr["address"].ToString(), request["T_Adress"]);

                if (dr["schools"].ToString() != request["T_school"])
                    log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "毕业学校", dr["schools"].ToString(), request["T_school"]);

                if (dr["education"].ToString() != request["T_edu"])
                    log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "学历", dr["education"].ToString(), request["T_edu"]);

                if (dr["professional"].ToString() != request["T_professional"])
                    log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "专业", dr["professional"].ToString(), request["T_professional"]);


            }
            //combo
            if (request["Action"] == "combo")
            {
                string serchtxt = " 1=1 ";

                string did = request["did"];
                if (!string.IsNullOrEmpty(did) && did != null && did != "null")
                    serchtxt += " and d_id=" + int.Parse(did);

                string authtxt = request["auth"];
                if (authtxt == "1")
                {
                    Data.GetDataAuth dataauth = new Data.GetDataAuth();
                    string txt = dataauth.GetDataAuthByid("1", "Sys_add", emp_id.ToString());
                    string[] arr = txt.Split(':');
                    switch (arr[0])
                    {
                        case "my": serchtxt += " and ID=" + emp_id;
                            break;
                    }
                }

                DataSet ds = emp.GetList(serchtxt);

                StringBuilder str = new StringBuilder();

                str.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["id"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["name"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");

                context.Response.Write(str);

            }
            //init
            if (request["Action"] == "init")
            {

                DataSet ds = emp.GetList("and ID=" + emp_id);

                StringBuilder str = new StringBuilder();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    str.Append(ds.Tables[0].Rows[0]["ID"].ToString() + "|" + ds.Tables[0].Rows[0]["d_id"]);
                }


                context.Response.Write(str);
            }
            //changepwd
            if (request["Action"] == "changepwd")
            {

                DataSet ds = emp.GetPWD(emp_id);

                string oldpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(request["T_oldpwd"], "MD5");
                string newpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(request["T_newpwd"], "MD5");

                if (ds.Tables[0].Rows[0]["pwd"].ToString() == oldpwd)
                {
                    model.pwd = newpwd;
                    model.ID = (emp_id);
                    emp.changepwd(model);
                    context.Response.Write("true");
                }
                else
                {
                    context.Response.Write("false");
                }
            }

            //allchangepwd
            if (request["Action"] == "allchangepwd")
            {
                string empid = request["empid"];

                string newpwd = FormsAuthentication.HashPasswordForStoringInConfigFile(request["T_newpwd"], "MD5");

                model.pwd = newpwd;
                model.ID = int.Parse(empid);
                emp.changepwd(model);
            }
            //del
            if (request["Action"] == "del")
            {
                BLL.hr_post hp = new BLL.hr_post();
                string empid = PageValidate.InputText(request["id"], 50);

                string EventType = "员工删除";

                DataSet ds = emp.GetList(" id=" + int.Parse(empid));
                BLL.CRM_Customer customer = new BLL.CRM_Customer();
                int cc = customer.GetList("Employee_id=" + int.Parse(empid)).Tables[0].Rows.Count;

                if (cc > 0)
                {
                    context.Response.Write("false:customer");
                }
                else
                {
                    bool isdel = false;
                    isdel = emp.Delete(int.Parse(request["id"]));
                    //update post
                    hp.UpdatePostEmpbyEid(int.Parse(empid));


                    if (isdel)
                    {
                        int UserID = emp_id;
                        string UserName = empname;
                        string IPStreet = request.UserHostAddress;
                        int EventID = int.Parse(empid);
                        string EventTitle = ds.Tables[0].Rows[0]["name"].ToString();
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
        public class PostData
        {
            private int post_id;
            private string post_name;
            private int? emp_id;
            private string emp_name;
            private int? default_post;
            private int? _dep_id;

            public int? dep_id
            {
                get { return _dep_id; }
                set { _dep_id = value; }
            }
            private string depname;
            private int? position_id;
            private string position_name;

            public int Post_id
            {
                get { return post_id; }
                set { post_id = value; }
            }
            public string Post_name
            {
                get { return post_name; }
                set { post_name = value; }
            }
            public int? Emp_id
            {
                get { return emp_id; }
                set { emp_id = value; }
            }
            public string Emp_name
            {
                get { return emp_name; }
                set { emp_name = value; }
            }
            public int? Default_post
            {
                get { return default_post; }
                set { default_post = value; }
            }

            public string Depname
            {
                get { return depname; }
                set { depname = value; }
            }
            public int? Position_id
            {
                get { return position_id; }
                set { position_id = value; }
            }
            public string Position_name
            {
                get { return position_name; }
                set { position_name = value; }
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