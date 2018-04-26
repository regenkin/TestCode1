using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Text;

namespace XHD.CRM
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(LogOn())
                Response.Redirect("main.aspx");
        }

        public bool LogOn()
        {
            bool b = false;
            string tiecket = (Request["tiecket"] ?? "").ToString();
            string appid = System.Configuration.ConfigurationManager.AppSettings["appid"];
            string appsecret = System.Configuration.ConfigurationManager.AppSettings["appsecret"];
            string webapiurl = System.Configuration.ConfigurationManager.AppSettings["webapiurl"];
            string ssourl = System.Configuration.ConfigurationManager.AppSettings["ssourl"];
            if (string.IsNullOrWhiteSpace(tiecket))
            {
                UriBuilder returnToBuilder = new UriBuilder(Request.Url);
                Response.Redirect(ssourl + string.Format("?appid={0}&redirecturl={1}", appid, returnToBuilder.Uri.ToString()));
            }
            else
            {
                int userid;
                //根据APPKEY+APPSERITE+USERI获取toke是不是登陆获过期
                //如果过期返回token为空跳到单点登陆界面登陆后返回toke
                //用token去查询是不是登陆过了

                #region 用临时令牌获取全局令牌
                NameValueCollection postdata = new NameValueCollection();
                postdata.Add("token", tiecket);
                postdata.Add("appid", appid);
                postdata.Add("appsecret", appsecret);
                postdata.Add("data", string.Format("{{\"ClientID\":\"{0}\"}}", ""));
                WebClientHelp webClientHelp = new WebClientHelp();
                string value = webClientHelp.WebClientPOST(webapiurl + "/api/OAth/Account/GetAccessToken", postdata);
                XHD.CRM.WebClientHelp.ReturnSimple apiReturnObject = Newtonsoft.Json.JsonConvert.DeserializeObject<XHD.CRM.WebClientHelp.ReturnSimple>(value);

                UserToken MUserToken = null;
                if (apiReturnObject.Status == 1)
                {
                    MUserToken = Newtonsoft.Json.JsonConvert.DeserializeObject<UserToken>(apiReturnObject.Data.ToString());
                }
                #endregion

                if (MUserToken != null)
                {
                    KfCrm.Model.hr_employee mEmp = new KfCrm.Model.hr_employee();
                    KfCrm.BLL.hr_employee bllEmp = new KfCrm.BLL.hr_employee();
                    KfCrm.BLL.hr_employee_openid bllEmpOpenID = new KfCrm.BLL.hr_employee_openid();

                    mEmp = bllEmpOpenID.GetEmployeeModel(1, MUserToken.UserID);
                    if (mEmp == null)
                    {
                        mEmp = new KfCrm.Model.hr_employee();
                        string s = "kf" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        mEmp.uid = s;
                        mEmp.email = "";
                        mEmp.name = s;
                        mEmp.birthday = "";
                        mEmp.sex = "";
                        mEmp.idcard = "";
                        mEmp.tel = "";
                        mEmp.status = "";
                        mEmp.EntryDate = "";
                        mEmp.address = "";
                        mEmp.schools ="";
                        mEmp.education = "";
                        mEmp.professional = "";
                        mEmp.remarks = "";
                        mEmp.title = "";
                        mEmp.canlogin = 1;
                        mEmp.isDelete = 0;
                        mEmp.pwd = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile("kinfar123456", "MD5");
                        int empid = bllEmp.Add(mEmp);

                        KfCrm.Model.hr_employee_openid mEmpOpenID = new KfCrm.Model.hr_employee_openid();
                        mEmpOpenID.hr_employeeID = empid;
                        mEmpOpenID.Type = 1;
                        mEmpOpenID.OpenID = MUserToken.UserID;
                        bllEmpOpenID.Add(mEmpOpenID);
                    }

                    System.Web.Security.FormsAuthenticationTicket ticket = new System.Web.Security.FormsAuthenticationTicket(
                        1,
                        mEmp.name,
                        DateTime.Now,
                        DateTime.Now.AddMinutes(20),
                        true,
                        mEmp.ID.ToString(),
                        "/"
                        );                                
                    var cookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, System.Web.Security.FormsAuthentication.Encrypt(ticket));
                    cookie.HttpOnly = true;
                    Response.Cookies.Add(cookie);

                    //日志
                    KfCrm.BLL.Sys_log log = new KfCrm.BLL.Sys_log();
                    KfCrm.Model.Sys_log modellog = new KfCrm.Model.Sys_log();
                    modellog.EventType = "系统登录";

                    modellog.EventDate = DateTime.Now;
                    modellog.UserID = mEmp.ID;
                    modellog.UserName = mEmp.name;
                    modellog.IPStreet = Request.UserHostAddress;

                    log.Add(modellog);

                    //online
                    KfCrm.BLL.Sys_online sol = new KfCrm.BLL.Sys_online();
                    KfCrm.Model.Sys_online model = new KfCrm.Model.Sys_online();

                    model.UserName = mEmp.name;
                    model.UserID = mEmp.ID;
                    model.LastLogTime = DateTime.Now;

                    System.Data.DataSet ds1 = sol.GetList(" UserID=" + mEmp.ID.ToString());

                    //添加当前用户信息
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        sol.Update(model, " UserID=" + mEmp.ID.ToString());
                    }
                    else
                    {
                        sol.Add(model);
                    }
                    //删除超时用户
                    sol.Delete(" LastLogTime<DATEADD(MI,-1,getdate())");

                    b=true;
                }
            }
            return b;
        }
    }

    public class UserToken
    {
        private int _id;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _token;

        public string Token
        {
            get { return _token; }
            set { _token = value; }
        }

        private string _accesstoken;

        public string AccessToken
        {
            get { return _accesstoken; }
            set { _accesstoken = value; }
        }
        private System.DateTime _expire;

        public System.DateTime Expire
        {
            get { return _expire; }
            set { _expire = value; }
        }
        private string _appID;

        public string AppID
        {
            get { return _appID; }
            set { _appID = value; }
        }
        private byte[] _timestamp;

        public byte[] Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }
        private int _userID;

        public int UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public string CallBackUrl { set; get; }
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

                byte[] postData = Encoding.Default.GetBytes(data);

                byte[] responseData = wc.UploadData(url, "POST", postData);

                return Encoding.UTF8.GetString(responseData);
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
        public string MyWebClientPOSTNameValue(string validateCode, string publicKey, string url, NameValueCollection data)
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

                return Encoding.UTF8.GetString(responseData);
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
        public string WebClientPOST(string url, NameValueCollection data)
        {
            try
            {
                System.Net.WebClient wc = new System.Net.WebClient();
                byte[] responseData = wc.UploadValues(url, "POST", data);
                return Encoding.UTF8.GetString(responseData);
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