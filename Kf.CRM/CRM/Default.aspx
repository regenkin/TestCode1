<%@ Page Language="C#" AutoEventWireup="true" %>

<!--Èë¿ÚÒ³Ãæ->
<%
    //Ë¢ÐÂ¾²Ì¬·½·¨»º´æ  
    CRM.Data.install inss = new CRM.Data.install();
    string filename = Server.MapPath("/conn.config");
    inss.CheckConfig(filename);
    string filename1 = Server.MapPath("/Web.config");
    inss.CheckConfig(filename1);

    //ÅÐ¶ÏÊÇ·ñÒÑÅäÖÃ
    CRM.Data.install ins = new CRM.Data.install();
    int configed = ins.configed();

    if (configed == 1)
    {
        //ÅÐ¶ÏÊÇ·ñµÇÂ½
        HttpCookie cookie = Request.Cookies[FormsAuthentication.FormsCookieName]; 
        if (Request.IsAuthenticated && null!=cookie)
            Response.Redirect("main.aspx");

        else
            Response.Redirect("login.aspx");
        //Response.Redirect("login.aspx");
    }
    else
        Response.Redirect("install/index.aspx");
 %>
