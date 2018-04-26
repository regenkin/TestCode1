using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Data;

namespace KfCrm.CRM
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // ��Ӧ�ó�������ʱ���еĴ���
            
        }

        void Application_End(object sender, EventArgs e)
        {
            //  ��Ӧ�ó���ر�ʱ���еĴ���

        }

        void Application_Error(object sender, EventArgs e)
        {
            // �ڳ���δ����Ĵ���ʱ���еĴ���
            Exception objErr = Server.GetLastError().GetBaseException();

            if (objErr.GetType() == typeof(HttpException))
            {
                int i = ((HttpException)objErr).GetHttpCode();
                if (i == 404)
                {
                    //Response.Redirect("~/ErrorPage/FileNotFind.html");
                }
                else if (i == 403)
                {
                    //Response.Redirect("~/ErrorPage/NoAccess.html");
                }
            }
            else
            {
                BLL.Sys_log_Err ssle = new BLL.Sys_log_Err();
                Model.Sys_log_Err model = new Model.Sys_log_Err();

                model.Err_typeid = 2;
                model.Err_type = "CRMϵͳ";
                model.Err_time = DateTime.Now;
                model.Err_url = Common.PageValidate.InputText(Request.Url.ToString(), 500);
                model.Err_message = Common.PageValidate.InputText(objErr.Message, int.MaxValue);
                model.Err_source = Common.PageValidate.InputText(objErr.Source, 500);
                model.Err_trace = Common.PageValidate.InputText(objErr.StackTrace, int.MaxValue);
                model.Err_ip = Request.UserHostAddress;

                var cookie = Request.Cookies[FormsAuthentication.FormsCookieName];
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                string CoockiesID = ticket.UserData;
                
                //����Cookie�Ƿ��Ѿ����� 
                if (null == cookie)
                {
                    model.Err_emp_id = -1;
                    model.Err_emp_name = "δ��¼";
                }
                else
                {
                    if (Common.PageValidate.IsNumber(CoockiesID))
                    {
                        BLL.hr_employee emp = new BLL.hr_employee();
                        int emp_id = int.Parse(CoockiesID);
                        DataSet dsemp = emp.GetList("id=" + emp_id);
                        string empname = dsemp.Tables[0].Rows[0]["name"].ToString();
                      
                        model.Err_emp_id = emp_id;
                        model.Err_emp_name = empname;
                    }
                    else
                    {
                        model.Err_emp_id = -1;
                        model.Err_emp_name = "�쳣��¼";
                    }
                }

                ssle.Add(model);

                //Server.ClearError();
            }

        }

        void Session_Start(object sender, EventArgs e)
        {
            // ���»Ự����ʱ���еĴ���

        }

        void Session_End(object sender, EventArgs e)
        {
            // �ڻỰ����ʱ���еĴ��롣 
            // ע��: ֻ���� Web.config �ļ��е� sessionstate ģʽ����Ϊ
            // InProc ʱ���Ż����� Session_End �¼�������Ựģʽ����Ϊ StateServer 
            // �� SQLServer���򲻻��������¼���

        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //string url = Request.Url.ToString();
            //string[] urlstr = url.Split('/');
            //string validate=urlstr[urlstr.Length-1];
            //if (validate != "login.aspx")
            //{
            //    string[] v = validate.Split('?');
            //    if (v[0] == "ValidateCode.aspx" || v[0]=="login.ashx" )
            //    { }
            //    else
            //    {
            //        //��ô�cookie���� 
            //        HttpCookie cookie = Request.Cookies["UserID"];
            //        //����Cookie�Ƿ��Ѿ����� 
            //        if (null == cookie)
            //        {
            //            Response.Redirect("login.aspx");
            //        }
            //    }
            //}
        }

        ////����ҳ��ִ��ʱ��
        //protected void Application_BeginRequest(Object sender, EventArgs e)
        //{
        //    Application["StartTime"] = System.DateTime.Now;
        //}
        //protected void Application_EndRequest(Object sender, EventArgs e)
        //{
        //    System.DateTime startTime = (System.DateTime)Application["StartTime"];
        //    System.DateTime endTime = System.DateTime.Now;
        //    System.TimeSpan ts = endTime - startTime;
        //    Response.Write("ҳ��ִ��ʱ��:" + ts.Milliseconds + " ����");
        //}

    }
}
