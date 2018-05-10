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
                
                #region webapi清除token
                
                //string tokenkey = System.Configuration.ConfigurationManager.AppSettings["tokenkey"];
                //string appid = System.Configuration.ConfigurationManager.AppSettings["appid"];
                //string appsecret = System.Configuration.ConfigurationManager.AppSettings["appsecret"];
                //string webapiurl = System.Configuration.ConfigurationManager.AppSettings["webapiurl"];
                //cookie = context.Request.Cookies[tokenkey];
                //if (cookie != null)
                //{
                //    var ticket = FormsAuthentication.Decrypt(cookie.Value);
                //    string accesstoken = ticket.UserData;
                //    System.Collections.Specialized.NameValueCollection postdata = new System.Collections.Specialized.NameValueCollection();
                //    postdata.Add("token", accesstoken);
                //    postdata.Add("appid", appid);
                //    postdata.Add("appsecret", appsecret);
                //    WebClientHelp webClientHelp = new WebClientHelp();
                //    string value = webClientHelp.WebClientPOST(webapiurl + "/api/OAth/Account/LoginOut", postdata);
                //    XHD.CRM.WebClientHelp.ReturnSimple apiReturnObject = Newtonsoft.Json.JsonConvert.DeserializeObject<XHD.CRM.WebClientHelp.ReturnSimple>(value);
                //    if (apiReturnObject.Status == 1) { }
                //}
                #endregion

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

    public class WebClientHelp
    {
        /// <summary>
        /// 发送Post请求。http://202.104.146.35:9506/api/Account/Test
        /// </summary>
        /// <param name="validateCode"></param>
        /// <param name="publicKey"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string MyWebClientPOST(string validateCode, string publicKey, string url, string data)
        {
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();

                // 采取POST方式必须加的Header
                //wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                //byte[] value = PFRSACryp.Encrypt(publicKey, validateCode, true);

                //var authString = Convert.ToBase64String(value);

                //wc.Headers.Add("Authorization", "Basic " + authString);

                byte[] postData = System.Text.Encoding.Default.GetBytes(data);

                byte[] responseData = wc.UploadData(url, "POST", postData);

                return System.Text.Encoding.UTF8.GetString(responseData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送Post请求。
        /// </summary>
        /// <param name="validateCode"></param>
        /// <param name="publicKey"></param>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string MyWebClientPOSTNameValue(string validateCode, string publicKey, string url, System.Collections.Specialized.NameValueCollection data)
        {
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();

                // 采取POST方式必须加的Header
                //wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

                //byte[] value = PFRSACryp.Encrypt(publicKey, validateCode, true);

                //var authString = Convert.ToBase64String(value);

                //wc.Headers.Add("Authorization", "Basic " + authString);

                byte[] responseData = wc.UploadValues(url, "POST", data);

                return System.Text.Encoding.UTF8.GetString(responseData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string WebClientPOST(string url, System.Collections.Specialized.NameValueCollection data)
        {
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] responseData = wc.UploadValues(url, "POST", data);
                return System.Text.Encoding.UTF8.GetString(responseData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public class ReturnSimple
        {
            /// <summary>
            /// 状态：1-成功，非1-不成功
            /// </summary>
            public int Status { get; set; }

            /// <summary>
            /// 消息
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// 数据
            /// </summary>
            public object Data { get; set; }
        }

        public class ApiReturnObject
        {
            /// <summary>
            /// 1:成功 >=0:失败 
            /// </summary>
            public int Status;
            /// <summary>
            /// 返回信息
            /// </summary>
            public string Message;
            /// <summary>
            /// 返回内容
            /// </summary>
            public object Data;
        }

        public enum AjaxReturnStatus
        {
            Timeout = -2,
            Error = -1,
            Success = 1,
        }
    }
}