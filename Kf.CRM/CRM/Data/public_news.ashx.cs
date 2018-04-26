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
    /// public_news 的摘要说明
    /// </summary>
    public class public_news : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            HttpRequest request = context.Request;

            BLL.public_news news = new BLL.public_news();
            Model.public_news model = new Model.public_news();

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
                DataRow dremp = dsemp.Tables[0].Rows[0];

                model.news_time = DateTime.Now;

                model.news_title = PageValidate.InputText(request["T_title"], 255);
                model.news_content = PageValidate.InputText(request["T_content"], int.MaxValue);

                string nid = PageValidate.InputText( request["nid"],50);
                if (!string.IsNullOrEmpty(nid) && nid != "null")
                {
                    if (!Common.PageValidate.IsNumber(nid))
                    {
                        nid = "-1";
                    }
                    DataSet ds = news.GetList("id=" + int.Parse( nid));
                    DataRow dr = ds.Tables[0].Rows[0];


                    model.dep_id = int.Parse(dr["dep_id"].ToString());
                    model.dep_name = dr["dep_name"].ToString();
                    model.create_id = int.Parse(dr["create_id"].ToString());
                    model.create_name = dr["create_name"].ToString();

                    model.id = int.Parse(nid);

                    news.Update(model);

                    C_Sys_log log = new C_Sys_log();

                    int UserID = emp_id;
                    string UserName = empname;
                    string IPStreet = request.UserHostAddress;
                    string EventTitle = model.news_title;
                    string EventType = "新闻修改";
                    int EventID = model.id;

                    if (dr["news_title"].ToString() != request["T_title"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "新闻标题", dr["news_title"].ToString(), request["T_title"]);
                    }
                    if (dr["news_content"].ToString() != request["T_content"])
                    {
                        log.Add_log(UserID, UserName, IPStreet, EventTitle, EventType, EventID, "新闻内容", "原内容被修改", "原内容被修改");
                    }

                }
                else
                {

                    int depid = int.Parse(dremp["d_id"].ToString());
                    string depname = dremp["dname"].ToString();

                    model.dep_id = depid;
                    model.dep_name = depname;
                    model.create_id = emp_id;
                    model.create_name = empname;

                    news.Add(model);


                }
            }
            if (request["Action"] == "grid")
            {
                int PageIndex = int.Parse(request["page"] == null ? "1" : request["page"]);
                int PageSize = int.Parse(request["pagesize"] == null ? "30" : request["pagesize"]);
                string sortname = request["sortname"];
                string sortorder = request["sortorder"];

                if (string.IsNullOrEmpty(sortname))
                    sortname = " news_time";
                if (string.IsNullOrEmpty(sortorder))
                    sortorder = "desc";

                string sorttext = " " + sortname + " " + sortorder;

                string Total,serchtxt="1=1 ";   
                
                if (!string.IsNullOrEmpty(request["sstart"]))
                    serchtxt += " and news_time >= '" + PageValidate.InputText( request["sstart"],50) + "'";

                if (!string.IsNullOrEmpty(request["sdend"]))
                {
                    DateTime enddate = DateTime.Parse(request["sdend"]).AddHours(23).AddMinutes(59).AddSeconds(59);
                    serchtxt += " and news_time  <= '" + enddate + "'";
                }

                if (!string.IsNullOrEmpty(request["stext"]))
                {
                    if (request["stext"] != "输入关键词搜索")
                        serchtxt += " and news_title like N'%" + PageValidate.InputText( request["stext"],500) + "%'";
                }

                DataSet ds = news.GetList(PageSize, PageIndex, serchtxt, sorttext, out Total);

                context.Response.Write(Common.GetGridJSON.DataTableToJSON1(ds.Tables[0], Total));
            }
            if (request["Action"] == "form")
            {
                string nid = PageValidate.InputText( request["nid"],50);
                
                DataSet ds = news.GetList("id=" + int.Parse( nid));
                string dt = Common.DataToJson.DataToJSON(ds);

                context.Response.Write(dt);
            }
            //del
            if (request["Action"] == "del")
            {

                bool canDel = false;
                if (dsemp.Tables[0].Rows.Count > 0)
                {
                    if (dsemp.Tables[0].Rows[0]["uid"].ToString() == "admin")
                    {
                        canDel = true;
                    }
                    else
                    {
                        Data.GetAuthorityByUid getauth = new Data.GetAuthorityByUid();
                        string delauth = getauth.GetBtnAuthority(emp_id.ToString(), "10");
                        if (delauth == "F")
                            canDel = false;
                        else
                            canDel = true;
                    }
                }
                if (canDel)
                {
                    int id = int.Parse(request["id"]);

                    DataSet ds = news.GetList("id=" + id);

                    string EventType = "彻底删除新闻";

                    bool isdel = news.Delete(id);
                    if (isdel)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            int UserID = emp_id;
                            string UserName = empname;
                            string IPStreet = request.UserHostAddress;
                            int EventID = id;
                            string EventTitle = ds.Tables[0].Rows[i]["news_title"].ToString();
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
                else
                {
                    context.Response.Write("auth");
                }
            }

            if (request["Action"] == "newsremind")
            {
                DataSet ds = news.GetList(7, " ", " news_time desc");
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