using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.Security;
using KfCrm.Common;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// hr_post 的摘要说明
    /// </summary>
    public class hr_post : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.hr_post post = new BLL.hr_post();
            Model.hr_post model = new Model.hr_post();

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
                string depid = request["depid"];
                string emps = request["empstatus"];
                int empstuats = 0;
                if (!string.IsNullOrEmpty(emps))
                {
                    empstuats = Common.PageValidate.IsNumber(request["empstatus"]) ? int.Parse(request["empstatus"]) : 0;
                }

                string serchtxt = "";

                switch (empstuats)
                {
                    case 0: serchtxt += "1=1 ";
                        break;
                    case 1: serchtxt += "emp_id=-1 ";
                        break;
                    case 2: serchtxt += "emp_id!=-1 ";
                        break;
                }

                if (!string.IsNullOrEmpty(depid) && depid != "null")
                    serchtxt += " and dep_id=" + int.Parse(depid);

                DataSet ds = post.GetList(0, serchtxt, " position_order");
                string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                context.Response.Write(dt);
            }


            if (request["Action"] == "save")
            {
                //dep
                model.dep_id = int.Parse(request["T_depname_val"]);
                model.depname = PageValidate.InputText(request["T_depname"], 250);
                //name
                model.post_name = PageValidate.InputText(request["T_postname"], 250);
                //position
                model.position_id = int.Parse(request["T_position_val"]);
                model.position_name = PageValidate.InputText(request["T_position"], 250);
                model.position_order = int.Parse(request["T_position_leavel"]);
                //emp
                int empid = Common.PageValidate.IsNumber(request["T_emp_val"]) ? int.Parse(request["T_emp_val"]) : -1;
                model.emp_id = empid;
                model.emp_name = PageValidate.InputText(request["T_emp"], 250);
                //note
                model.note = PageValidate.InputText(request["T_descript"], 4000);

                Model.hr_employee modelemp = new Model.hr_employee();
                //更新员工岗位
                modelemp.d_id = model.dep_id;
                modelemp.dname = model.depname;
               
                modelemp.post = model.post_name;
                modelemp.zhiwuid = model.position_id;
                modelemp.zhiwu = model.position_name;
                modelemp.ID = empid;

                string postid = PageValidate.InputText( request["postid"],50);
                if (!string.IsNullOrEmpty(postid) && postid != "null")
                {
                    model.post_id = int.Parse(postid);
                    DataSet ds = post.GetList(" post_id=" + int.Parse(postid));
                    DataRow dr = null;
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dr = ds.Tables[0].Rows[0];

                        //判断默认岗位
                        if (model.emp_id == -1)
                        {
                            model.default_post = 0;
                        }
                        else
                        {
                            DataSet ds1 = post.GetList(string.Format("default_post=1 and emp_id={0} and post_id!={1}", model.emp_id, int.Parse(postid)));
                            if (ds1.Tables[0].Rows.Count > 0)
                                model.default_post = 0; //此员工有默认岗位  
                            else
                            {
                                model.default_post = 1; //设置此岗位为此员工默认岗位 

                                //更新员工岗位
                                modelemp.postid = model.post_id;   
                                emp.UpdatePost(modelemp);
                            }
                        }
                    }
                    post.Update(model);

                    //日志
                    C_Sys_log log = new C_Sys_log();

                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.position_name;
                    string EventType = "岗位修改";
                    int EventID = model.post_id;

                    if (dr["post_name"].ToString() != request["T_postname"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "岗位名称", dr["post_name"].ToString(), request["T_postname"]);
                    }
                    if (dr["position_name"].ToString() != request["T_position"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "岗位级别", dr["position_name"].ToString(), request["T_position"]);
                    }
                    if (dr["emp_name"].ToString() != request["T_emp"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "岗位员工", dr["emp_name"].ToString(), request["T_emp"]);
                    }
                    if (dr["note"].ToString() != request["T_descript"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "描述", dr["note"].ToString(), request["T_descript"]);
                    }
                }
                else
                { 
                    model.isDelete = 0;
                    postid= post.Add(model).ToString();    

                    //判断默认岗位
                    if (model.emp_id == -1)
                    {
                        model.default_post = 0;
                    }
                    else
                    {
                        DataSet ds1 = post.GetList(string.Format("default_post=1 and emp_id={0} and post_id!={1}", model.emp_id, int.Parse(postid)));
                        if (ds1.Tables[0].Rows.Count > 0)
                            model.default_post = 0; //此员工有默认岗位  
                        else
                        {
                            model.default_post = 1; //设置此岗位为此员工默认岗位 

                            //更新员工岗位
                            modelemp.postid = int.Parse(postid);
                            emp.UpdatePost(modelemp);
                        }
                    }
                    post.UpdatePostEmp(model);
                }
            }
            //Form JSON
            if (request["Action"] == "form")
            {
                string postid = PageValidate.InputText(request["postid"],50);
                string dt;

                if (PageValidate.IsNumber(postid))
                {
                    DataSet ds = post.GetList("post_id=" + postid);

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
                string postid = request["id"];

                string EventType = "岗位删除";
                DataSet ds = post.GetList(" post_id=" + int.Parse(postid));

                if (ds.Tables[0].Rows[0]["emp_id"].ToString()!="-1")
                {
                    //含有员工信息不能删除
                    context.Response.Write("false:emp");
                }
                else
                {
                    bool isdel = post.Delete(int.Parse(postid));
                    if (isdel)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int UserID = emp_id;
                            string UserName = empname;
                            string IPStreet = request.UserHostAddress;
                            int EventID = int.Parse(postid);
                            string EventTitle = ds.Tables[0].Rows[i]["post_name"].ToString();
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

            //get post by empid
            if (request["Action"] == "getpostbyempid")
            {
                BLL.hr_post hp = new BLL.hr_post();
                int empid = int.Parse(request["empid"]);
                DataSet ds = hp.GetList(" emp_id=" + empid);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                    context.Response.Write(dt);
                }
            }
            //serch
            if (request["Action"] == "serch")
            {
                BLL.hr_post hp = new BLL.hr_post();
                string serchtxt = PageValidate.InputText(request["Serchtext"], 255);
                DataSet ds = hp.GetList(" isDelete=0 and post_name like N'%" + serchtxt + "%'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string dt = Common.GetGridJSON.DataTableToJSON(ds.Tables[0]);
                    context.Response.Write(dt);
                }
                else
                {
                    context.Response.Write("post_name like N'%" + serchtxt + "%'");
                }
            }
            //post_emp
            if (request["Action"] == "postemp")
            {
                string json = request["PostData"].ToLower();
                JavaScriptSerializer js = new JavaScriptSerializer();

                PostData[] postdata;
                postdata = js.Deserialize<PostData[]>(json);

                BLL.hr_post hp = new BLL.hr_post();


                string empid = request["empid"];
                int Eid = -1;
                if (!string.IsNullOrEmpty(empid))
                {
                    Eid = Common.PageValidate.IsNumber(empid) ? int.Parse(empid) : -1;
                }
                Model.hr_employee modelemp = new Model.hr_employee();
                model.emp_id = Eid;
                modelemp.ID = Eid;
                model.emp_name = PageValidate.InputText(request["emp_name"], 255);

                for (int i = 0; i < postdata.Length; i++)
                {
                    model.post_id = postdata[i].Post_id;
                    model.default_post = postdata[i].Default_post;

                    if (postdata[i].Default_post == 1)
                    {
                        modelemp.d_id = postdata[i].Dep_id;
                        modelemp.dname = postdata[i].Depname;
                        modelemp.zhiwuid = postdata[i].Position_id;
                        modelemp.zhiwu = postdata[i].Position_name;
                        modelemp.postid = postdata[i].Post_id;
                        modelemp.post = postdata[i].Post_name;
                        //context.Response.Write(postdata[i].Depname + "@");
                        emp.UpdatePost(modelemp);
                    }

                    hp.UpdatePostEmp(model);
                }
            }
            //combo
            if (request["Action"] == "combo")
            {
                int postid = int.Parse(request["postid"]);

                DataSet ds = post.GetList(" dep_id=" + postid);

                StringBuilder str = new StringBuilder();

                str.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    str.Append("{id:" + ds.Tables[0].Rows[i]["post_id"].ToString() + ",text:'" + ds.Tables[0].Rows[i]["post_name"] + "'},");
                }
                str.Replace(",", "", str.Length - 1, 1);
                str.Append("]");

                context.Response.Write(str);

            }
        }


        public class PostData
        {
            private int post_id;
            private string post_name;
            private int? emp_id;
            private string emp_name;
            private int? default_post;
            private int? dep_id;
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
            public int? Dep_id
            {
                get { return dep_id; }
                set { dep_id = value; }
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