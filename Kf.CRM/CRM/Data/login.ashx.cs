using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Data;
using KfCrm.Common;

namespace KfCrm.CRM.Data
{
    /// <summary>
    /// login 的摘要说明
    /// </summary>
    public class login : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Charset = "utf-8";
            HttpRequest request = context.Request;

            if (request["Action"] == "login")
            {
                BLL.hr_employee emp = new BLL.hr_employee();

                string username = PageValidate.InputText(request["username"], 255);
                //string password = FormsAuthentication.HashPasswordForStoringInConfigFile(request["password"], "MD5");
                string password = PageValidate.InputText(request["password"], 255);
                string validate = PageValidate.InputText(request["validate"], 255);

                if (!string.IsNullOrEmpty(validate) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    if (validate == context.Session["CheckCode"].ToString() || validate.ToLower() == context.Session["CheckCode"].ToString().ToLower())
                    {
                        DataSet ds = emp.GetList(" uid='" + username + "' and pwd='" + password + "'");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["canlogin"].ToString() == "1")
                            {
                                string userid = ds.Tables[0].Rows[0]["ID"].ToString();
                                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                                    1,
                                    username,
                                    DateTime.Now,
                                    DateTime.Now.AddMinutes(20),
                                    true,
                                    userid,
                                    "/"
                                    );                                
                                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                                cookie.HttpOnly = true;
                                context.Response.Cookies.Add(cookie);
   
                                //FormsAuthentication.SetAuthCookie(userid, true);

                                //日志
                                BLL.Sys_log log = new BLL.Sys_log();
                                Model.Sys_log modellog = new Model.Sys_log();
                                modellog.EventType = "系统登录";

                                modellog.EventDate = DateTime.Now;
                                modellog.UserID = int.Parse(userid);
                                modellog.UserName = ds.Tables[0].Rows[0]["name"].ToString();
                                modellog.IPStreet = request.UserHostAddress;

                                log.Add(modellog);

                                //online
                                BLL.Sys_online sol = new BLL.Sys_online();
                                Model.Sys_online model = new Model.Sys_online();

                                model.UserName = ds.Tables[0].Rows[0]["name"].ToString();
                                model.UserID = int.Parse(ds.Tables[0].Rows[0]["id"].ToString());
                                model.LastLogTime = DateTime.Now;

                                DataSet ds1 = sol.GetList(" UserID=" + ds.Tables[0].Rows[0]["id"].ToString());

                                //添加当前用户信息
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    sol.Update(model, " UserID=" + ds.Tables[0].Rows[0]["id"].ToString());
                                }
                                else
                                {
                                    sol.Add(model);
                                }
                                //删除超时用户
                                sol.Delete(" LastLogTime<DATEADD(MI,-1,getdate())");

                                //验证完毕，允许登录
                                context.Response.Write("2");
                            }
                            else
                            {
                                context.Response.Write("4");//不允许登录
                            }
                        }
                        else
                        {
                            context.Response.Write("1");//用户名或密码错误
                        }
                    }
                    else
                    {
                        context.Response.Write("0");//验证码错误
                    }
                }
                else
                {
                    context.Response.Write("999");//系统数据错误
                }
            }
           
            if (request["Action"] == "logout")
            {
                var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];

                if (null != cookie)
                {
                    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                    string CoockiesID = ticket.UserData;

                    FormsAuthentication.SignOut();
                    context.Response.Write("true");

                    //online
                    BLL.Sys_online sol = new BLL.Sys_online();
                    try
                    {
                        if (!string.IsNullOrEmpty(CoockiesID))
                        {
                            sol.Delete(" UserID=" + int.Parse(CoockiesID));
                        }
                    }
                    catch
                    {
                    }
                }


            }
            if (request["Action"] == "checkpwd")
            {
                var cookie = context.Request.Cookies[FormsAuthentication.FormsCookieName];
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                string CoockiesID = ticket.UserData;

                BLL.hr_employee emp = new BLL.hr_employee();

                int emp_id = int.Parse(CoockiesID);
                string password = FormsAuthentication.HashPasswordForStoringInConfigFile(request["password"], "MD5");


                DataSet ds = emp.GetList(string.Format("ID={0} and pwd='{1}'", emp_id, password));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    context.Response.Write("{sucess:sucess}");
                }
                else
                {
                    context.Response.Write("{sucess:false}");
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